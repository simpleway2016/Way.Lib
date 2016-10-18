using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace FileSyncLib
{
    class Mission
    {
        FileSystemWatcher m_watcher;
        db.Mission m_data;
        string[] exts = new string[0];
        string[] excludeFolders = new string[0];
        public Mission(db.Mission data)
        {
            m_data = data;
            if (data.excludeExts != null)
            {
                exts = data.excludeExts.Split(';');
                exts = (from m in exts
                            where m.Length > 0
                        select m.ToLower().Substring(1)).ToArray();
                excludeFolders = data.excludeFolder.Split(';');
                excludeFolders = (from m in excludeFolders
                        where m.Length > 0
                        select m.ToLower()).ToArray();
            }
        }

        public void Start()
        {
            new Thread(init).Start();
        }

        void init()
        {
            using (FileSyncDB db = new FileSyncDB())
            {
                if (m_data.isInit == false)
                {
                    try
                    {
                        initFolder(m_data.sourceFolder, m_data.targetFolder, db);
                    }
                    catch (Exception ex)
                    {
                        Helper.AddLog(ex.ToString());
                    }
                    m_data.isInit = true;
                    db.Update(m_data);
                }
            }

            m_watcher = new FileSystemWatcher(m_data.sourceFolder);
            m_watcher.Changed += m_watcher_Changed;
            m_watcher.Deleted += m_watcher_Deleted;
            m_watcher.Renamed += m_watcher_Renamed;
            m_watcher.IncludeSubdirectories = true;
            m_watcher.EnableRaisingEvents = true;

            new Thread(checking)
            {
                IsBackground = true,
            }.Start();
        }

        void checking()
        {
            while (true)
            {
                try
                {
                    using (FileSyncDB db = new FileSyncDB())
                    {
                        while (true)
                        {
                            var data = (from m in db.FileChanges
                                        orderby m.changeTime
                                        select m).FirstOrDefault();
                            if (data != null)
                            {
                                db.Delete(data);
                                if (data.type == global::db.FileChanges_typeEnum.Update)
                                {
                                    while (true)
                                    {
                                        var nextData = (from m in db.FileChanges
                                                        orderby m.changeTime
                                                        select m).FirstOrDefault();
                                        if (nextData != null && nextData.path == data.path && nextData.type == global::db.FileChanges_typeEnum.Update)
                                        {
                                            db.Delete(nextData);
                                        }
                                        else
                                        {
                                            break;
                                        }
                                    }
                                }
                                if (data.type == global::db.FileChanges_typeEnum.Rename)
                                {
                                    handleRename(data.oldPath, data.path);
                                }
                                else if (data.type == global::db.FileChanges_typeEnum.Delete)
                                {
                                    handleDelete(data.path);
                                }
                                else
                                {
                                    handleUpdate(data.path);
                                }
                            }
                            else
                            {
                                break;
                            }
                        }
                        
                    }
                }
                catch (Exception ex)
                {
                    Helper.AddLog("checking:" + ex.ToString());
                }
                Thread.Sleep(10000);
            }
        }

        void m_watcher_Renamed(object sender, RenamedEventArgs e)
        {
            string old_ext = Path.GetExtension(e.OldFullPath).ToLower();
            if (exts.Contains(old_ext))
                return;
            string fullpath = e.FullPath.ToLower();
            if (excludeFolders.FirstOrDefault(m => fullpath.StartsWith(m)) != null)
                return;

            FileInfo fileinfo = new FileInfo(e.FullPath);
            if (fileinfo.Attributes.HasFlag(FileAttributes.Hidden) ||
                fileinfo.Attributes.HasFlag(FileAttributes.System) ||
                 fileinfo.Attributes.HasFlag(FileAttributes.Temporary))
                return;

            using (FileSyncDB db = new FileSyncDB())
            {
                var item = new db.FileChanges()
                {
                    path = e.FullPath,
                    oldPath = e.OldFullPath,
                    MissionID = m_data.id,
                    type = global::db.FileChanges_typeEnum.Rename,
                    changeTime = DateTime.Now,
                    isSynced = false,
                };
                db.Insert(item);
                FileSync.OnChanged(item.path, item.type.ToString(), item.oldPath);
            }
        }

        void m_watcher_Deleted(object sender, FileSystemEventArgs e)
        {
            string old_ext = Path.GetExtension(e.FullPath).ToLower();
            if (exts.Contains(old_ext))
                return;

            string fullpath = e.FullPath.ToLower();
            if (excludeFolders.FirstOrDefault(m => fullpath.StartsWith(m)) != null)
                return;

            using (FileSyncDB db = new FileSyncDB())
            {
                var item = new db.FileChanges()
                {
                    path = e.FullPath,
                    MissionID = m_data.id,
                    type = global::db.FileChanges_typeEnum.Delete,
                    changeTime = DateTime.Now,
                    isSynced = false,
                };
                db.Insert(item);
                FileSync.OnChanged(item.path, item.type.ToString(), item.oldPath);
            }
        } 
        string lastChangePath;
        DateTime lastChangeTime = DateTime.Now.AddDays(-1);
        void m_watcher_Changed(object sender, FileSystemEventArgs e)
        {
            string old_ext = Path.GetExtension(e.FullPath).ToLower();
            if (exts.Contains(old_ext))
                return;
            if (Directory.Exists(e.FullPath))//是文件夹
                return;

            string fullpath = e.FullPath.ToLower();
            if (excludeFolders.FirstOrDefault(m => fullpath.StartsWith(m)) != null)
                return;

            FileInfo fileinfo = new FileInfo(e.FullPath);
            if (fileinfo.Attributes.HasFlag(FileAttributes.Hidden))
                return;

            if (lastChangePath == e.FullPath && fileinfo.LastWriteTime <= lastChangeTime )
                return;
            lastChangePath = e.FullPath;
            lastChangeTime = fileinfo.LastWriteTime;
            using (FileSyncDB db = new FileSyncDB())
            {
                var item = new db.FileChanges()
                {
                    path = e.FullPath,
                    MissionID = m_data.id,
                    type = global::db.FileChanges_typeEnum.Update,
                    changeTime = DateTime.Now,
                    isSynced = false,
                };
                db.Insert(item);
                FileSync.OnChanged(item.path, item.type.ToString(), item.oldPath);
            }
        }

        void handleRename(string oldPath, string fullPath)
        {
            try
            {
                bool isDir = false;
                if (Directory.Exists(fullPath))
                {
                    isDir = true;
                }

                string backupPath = m_data.targetFolder + "\\" + oldPath.Replace(m_data.sourceFolder + "\\", "");
                string newPath = m_data.targetFolder + "\\" + fullPath.Replace(m_data.sourceFolder + "\\", "");
                string newFolder = Path.GetDirectoryName(newPath);
                if (isDir)
                {
                    if (Directory.Exists(backupPath) == false)
                        Directory.CreateDirectory(newPath);
                    else
                    {
                        Directory.Move(backupPath, newPath);
                    }
                }
                else
                {
                    if (Directory.Exists(newFolder) == false)
                        Directory.CreateDirectory(newFolder);
                    if (File.Exists(backupPath) == false)
                    {
                        File.Copy(fullPath, newPath,true);
                    }
                    else
                    {
                        File.Move(backupPath, newPath);
                    }
                }
            }
            catch (Exception ex)
            {
                Helper.AddLog("handleRename:" + ex.ToString());
            }
        }
        void handleDelete(  string fullPath)
        {
            try
            {
                string newPath = m_data.targetFolder + "\\" + fullPath.Replace(m_data.sourceFolder + "\\", "");
                if (Directory.Exists(newPath))
                    Directory.Delete(newPath, true);
                else
                    File.Delete(newPath);
            }
            catch (Exception ex)
            {
                Helper.AddLog("handleDelete:" + ex.ToString());
            }
        }
        void handleUpdate(string fullPath)
        {
            try
            {
                if (File.Exists(fullPath) == false)
                    return;
                string newPath = m_data.targetFolder + "\\" + fullPath.Replace(m_data.sourceFolder + "\\", "");
                string folderPath = Path.GetDirectoryName(newPath);
                if (Directory.Exists(folderPath) == false)
                    Directory.CreateDirectory(folderPath);

                File.Copy(fullPath , newPath , true);
            }
            catch (Exception ex)
            {
                Helper.AddLog("handleDelete:" + ex.ToString());
            }
        }

        void initFolder(string dir,string dstDir, FileSyncDB db)
        {
            if (Directory.Exists(dstDir) == false)
                Directory.CreateDirectory(dstDir);

            string[] childDirs = Directory.GetDirectories(dir);
            foreach (var child in childDirs)
            {
                initFolder(child, dstDir + "\\" + Path.GetFileName(child), db);
            }

            string[] filePaths = Directory.GetFiles(dir);
            foreach (string filePath in filePaths)
            {
                string ext = Path.GetExtension(filePath).ToLower();
                if (exts.Contains(ext))
                    continue;
                string targetName = dstDir + "\\" + Path.GetFileName(filePath);
                bool needupdate = true;
                 FileInfo sourceFileInfo = new FileInfo(filePath);
                if (File.Exists(targetName))
                {
                   
                    FileInfo dstFileInfo = new FileInfo(targetName);
                    if (sourceFileInfo.LastWriteTime == dstFileInfo.LastWriteTime && sourceFileInfo.Length == dstFileInfo.Length)
                        needupdate = false;
                }
                if (needupdate)
                {
                    var dataitem = new db.FileChanges()
                    {
                        changeTime = sourceFileInfo.LastWriteTime,
                        isSynced = false,
                        MissionID = m_data.id,
                        path = filePath,
                        type = global::db.FileChanges_typeEnum.Update,
                    };
                    db.Insert(dataitem);
                }
            
            }
        }
    }
}
