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
        FileStream m_file;
        StreamWriter m_writer;
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


        bool _autoclose;
        DateTime _createFileTime;
        Action<string> _writeContentFunc;
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
                lockObj = new object();
                createFileStream();
                _writeContentFunc = (content) => writelogAction(content);

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
            _createFileTime = DateTime.Now;
            string filepath = getNewFileName();
            m_file = new FileStream(filepath, FileMode.Create,
                FileAccess.Write, FileShare.Read);
            m_writer = new StreamWriter(m_file);
        }

        string getNewFileName()
        {
            int index = 0;
            string filepath = string.Format("{0}/{1}_{2} {3}.txt", _SavePath, this.Name, index, DateTime.Now.ToString("yyyyMMdd"));
            while (File.Exists(filepath))
            {
                index++;
                filepath = string.Format("{0}/{1}_{2} {3}.txt", _SavePath, this.Name, index, DateTime.Now.ToString("yyyyMMdd"));
            }
            return filepath;
        }

        object lockObj;
        /// <summary>
        /// 写入日志
        /// </summary>
        /// <param name="content"></param>
        public void Log(string content)
        {
            if (m_writer == null || content == null)
                return;
            lock (lockObj)
            {
                _writeContentFunc(content);
            }
        }

      
        void writelogAction(string content)
        {
            m_writer.WriteLine(string.Format("{0} {1}", DateTime.Now, content));
            m_writer.Flush();
            if (m_file.Length >= MaxFileSize || (DateTime.Now - _createFileTime).Days > 0 || DateTime.Now.Day != _createFileTime.Day)
            {
                m_writer.Dispose();
                m_file.Dispose();
                createFileStream();
            }
        }
        /// <summary>
        /// 同步写入文件
        /// </summary>
        /// <param name="content"></param>
        /// <param name="objs"></param>
        public void Log(string content, params object[] objs)
        {
            if (m_writer == null || content == null)
                return;
            if (objs != null && objs.Length > 0)
                content = string.Format(content, objs);
            Log(content);
        }

    }

}
