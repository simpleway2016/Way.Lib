using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace EJClient
{
    class AutoUpdate
    {
        class FileInfo
        {
            public string SavePath;
            public string FileName;
            public long LastWriteTime;
        }
        public AutoUpdate()
        { 
            new Thread(check) { IsBackground = true}.Start();
        }

        void check()
        {
            try
            {
                bool hasUpdated = false;

                var fileinfos = Helper.Client.InvokeSync<FileInfo[]>("GetUpdateFileList");

                foreach (var item in fileinfos)
                {
                    if (System.IO.File.Exists(AppDomain.CurrentDomain.BaseDirectory + item.SavePath) == false)
                    {
                        downloadFile(item.SavePath);
                        hasUpdated = true;
                    }
                    else
                    {
                        var serverTime = DateTime.FromFileTime(item.LastWriteTime);
                        var myFileInfo = new System.IO.FileInfo(AppDomain.CurrentDomain.BaseDirectory + item.SavePath);
                        if (myFileInfo.LastWriteTime < serverTime)
                        {
                            downloadFile(item.SavePath);
                            hasUpdated = true;
                        }
                    }
                }

                if (hasUpdated)
                {
                    Application.Current.Dispatcher.Invoke(delegate
                    {
                        if (Application.Current.MainWindow != null)
                        {
                            if (MessageBox.Show(Application.Current.MainWindow, "发现新版本，是否现在更新？", "", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                            {
                                var json = (new Copy.DstSource
                                {
                                    dst = AppDomain.CurrentDomain.BaseDirectory ,
                                    src = AppDomain.CurrentDomain.BaseDirectory + "updates",
                                }).ToJsonString();
                                System.Diagnostics.Process.Start("copy.exe",Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes( json)));
                            }
                        }
                    });
                    
                }
            }
            catch
            {
                try
                {
                    var files = System.IO.Directory.GetFiles(AppDomain.CurrentDomain.BaseDirectory + "updates");
                    foreach( var f in files )
                    System.IO.File.Delete( f );
                }
                catch(Exception ex)
                {
                }
            }
        }

        void downloadFile(string savepath)
        {
            var filecontent = Helper.Client.InvokeSync<string>("DownLoadFile" , savepath);

            if (System.IO.Directory.Exists(AppDomain.CurrentDomain.BaseDirectory + "updates") == false)
                System.IO.Directory.CreateDirectory(AppDomain.CurrentDomain.BaseDirectory + "updates");
            if (savepath.ToLower() == "copy.exe")
            {
                System.IO.File.WriteAllBytes(AppDomain.CurrentDomain.BaseDirectory + savepath,Convert.FromBase64String(filecontent));
            }
            else
            System.IO.File.WriteAllBytes(AppDomain.CurrentDomain.BaseDirectory + "updates" + "\\" + savepath, Convert.FromBase64String(filecontent));
        }

        internal static void Update()
        {

           
        }
        /// <summary>
        /// 拷贝文件夹
        /// </summary>
        /// <param name="srcdir"></param>
        /// <param name="desdir"></param>
        internal static void CopyDirectory(string srcdir, string desdir)
        {
            string folderName = srcdir.Substring(srcdir.LastIndexOf("\\") + 1);

            string desfolderdir = desdir + "\\" + folderName;

            if (desdir.LastIndexOf("\\") == (desdir.Length - 1))
            {
                desfolderdir = desdir + folderName;
            }
            string[] filenames = Directory.GetFileSystemEntries(srcdir);

            foreach (string file in filenames)// 遍历所有的文件和目录
            {
                if (Directory.Exists(file))// 先当作目录处理如果存在这个目录就递归Copy该目录下面的文件
                {

                    string currentdir = desfolderdir + "\\" + file.Substring(file.LastIndexOf("\\") + 1);
                    if (!Directory.Exists(currentdir))
                    {
                        Directory.CreateDirectory(currentdir);
                    }

                    CopyDirectory(file, desfolderdir);
                }

                else // 否则直接copy文件
                {
                    string srcfileName = file.Substring(file.LastIndexOf("\\") + 1);

                    srcfileName = desfolderdir + "\\" + srcfileName;


                    if (!Directory.Exists(desfolderdir))
                    {
                        Directory.CreateDirectory(desfolderdir);
                    }


                    File.Copy(file, srcfileName);
                }
            }//foreach 
        }//function end
    }
}
