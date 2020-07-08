using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace Way.Lib
{
    
    public class FileLogger
    {
        class FileItem
        {
            internal FileStream File;
            internal DateTime CreateFileTime;
        }
        static System.Collections.Concurrent.ConcurrentDictionary<string, FileItem> Dict = new System.Collections.Concurrent.ConcurrentDictionary<string, FileItem>();
        FileItem _fileItem;
        string _SavePath;
        public string Name
        {
            get;
            private set;
        }

        /// <summary>
        /// 单个文件最大的大小，默认 20M
        /// </summary>
        public int MaxFileSize { get; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="folder">保存目录</param>
        /// <param name="name">文件名</param>
        public FileLogger(string folder , string name)
        {
            this.MaxFileSize = 1024 * 1024 * 20;

            _SavePath = folder;
            if (!string.IsNullOrEmpty(name))
            {
                name = name.Replace("/", "_").Replace("\\", "_");
                if (System.IO.Directory.Exists(_SavePath) == false)
                    Directory.CreateDirectory(_SavePath);

                this.Name = name;

                if( Dict.TryGetValue(this.Name , out _fileItem) == false)
                {
                    Dict.TryAdd(this.Name, new FileItem());
                    _fileItem = Dict[this.Name];

                    lock (_fileItem)
                    {
                        if (_fileItem.File == null)
                        {
                            createFileStream();
                        }
                    }
                }
               

            }
        }



        /// <summary>
        /// 删除文件夹里指定时间之前的文件
        /// </summary>
        /// <param name="folder">目标文件夹</param>
        /// <param name="lasttime">从什么时间往前的文件需要删除</param>
        public static void DeleteFiles(string folder,DateTime lasttime)
        {
            try
            {
                string[] files = System.IO.Directory.GetFiles(folder);
                foreach (string file in files)
                {
                    DateTime writetime = new System.IO.FileInfo(file).LastWriteTime;
                    if (writetime < lasttime)
                    {
                        try
                        {
                            System.IO.File.Delete(file);
                        }
                        catch
                        {
                        }
                    }
                }
            }
            catch
            {
            }
        }


        void createFileStream()
        {
            string filepath = getNewFileName();
           
            _fileItem.File?.Dispose();

            if (File.Exists(filepath))
            {
                _fileItem.File = new FileStream(filepath, FileMode.Open,
                   FileAccess.Write, FileShare.Read);
                _fileItem.File.Seek(_fileItem.File.Length, SeekOrigin.Begin);
            }
            else
            {
                _fileItem.File = new FileStream(filepath, FileMode.Create,
                    FileAccess.Write, FileShare.Read);
            }
            _fileItem.CreateFileTime = DateTime.Now;
        }

        string getNewFileName()
        {
            string filepath = string.Format("{0}/{1} {2}.txt", _SavePath, this.Name, DateTime.Now.ToString("yyyyMMdd"));
            return filepath;
        }

        /// <summary>
        /// 同步写入文件
        /// </summary>
        /// <param name="content"></param>
        public void Log(string content)
        {
            if (_fileItem == null || content == null)
                return;
            var data = Encoding.UTF8.GetBytes(string.Format("{0} {1}\r\n", DateTime.Now, content));
            lock (_fileItem)
            {
                _fileItem.File.Write(data,0, data.Length);
                _fileItem.File.Flush();
                if (_fileItem.File.Length >= MaxFileSize || (DateTime.Now - _fileItem.CreateFileTime).Days > 0 || DateTime.Now.Day != _fileItem.CreateFileTime.Day)
                {
                    createFileStream();
                }
            }
        }

        /// <summary>
        /// 同步写入文件
        /// </summary>
        /// <param name="content"></param>
        /// <param name="objs"></param>
        public void Log(string content, params object[] objs)
        {
            if (_fileItem == null || content == null)
                return;
            if (objs != null && objs.Length > 0)
                content = string.Format(content, objs);
            Log(content);
        }

    }

}
