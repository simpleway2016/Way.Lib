using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace Way.Lib
{

    public class CodeLog : IDisposable
    {
        static bool ENABLED = true;
        FileStream m_file;
        StreamWriter m_writer;
        static string _SavePath;
        /// <summary>
        /// 保存路径
        /// </summary>
        public virtual string SavePath
        {
            get
            {
                return _SavePath ?? (_SavePath = PlatformHelper.GetAppDirectory() + "CodeLog");
            }
        }
        public string Name
        {
            get;
            set;
        }
        /// <summary>
        /// 文件最大大小
        /// </summary>
        const int MAXFILESIZE = 1024 * 1024 * 2;
        bool _autoclose;
        DateTime _createFileTime;
        static List<CodeLog> RunningLogs = new List<CodeLog>();
        public CodeLog(string name)
            : this(name, true)
        {
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="name">参数如果给空字符串，则代表不记录</param>
        /// <param name="autoClose">是否自动关闭文件，多个不同方法间调用时，要禁止这个</param>
        public CodeLog(string name, bool autoClose)
        {
            if (!string.IsNullOrEmpty(name) && ENABLED)
            {
                name = name.Replace("/", "_").Replace("\\", "_");
                _autoclose = autoClose;
                if (System.IO.Directory.Exists(SavePath) == false)
                    Directory.CreateDirectory(SavePath);
                if (!ENABLED)
                    return;
                this.Name = name;
                if (autoClose == false)
                {
                    CodeLog existLog = RunningLogs.FirstOrDefault(m => m.Name == name);
                    if (existLog != null)
                    {
                        m_writer = existLog.m_writer;
                        m_file = existLog.m_file;
                        lockObj = existLog.lockObj;
                        return;
                    }
                    else
                    {
                        RunningLogs.Add(this);
                    }
                }

                lockObj = new object();
                createFileStream();
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
            string filepath = string.Format("{0}/{1}_{2} {3}.txt", SavePath, this.Name, index, DateTime.Now.ToString("yyyyMMdd"));
            while (File.Exists(filepath))
            {
                index++;
                filepath = string.Format("{0}/{1}_{2} {3}.txt", SavePath, this.Name, index, DateTime.Now.ToString("yyyyMMdd"));
            }
            return filepath;
        }

        /// <summary>
        /// 使CodeLog生效或者永久失效
        /// </summary>
        /// <param name="enabled"></param>
        public static void SetEnable(bool enabled)
        {
            ENABLED = enabled;
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
                m_writer.WriteLine(string.Format("{0} {1}", DateTime.Now, content));
                m_writer.Flush();
                if (m_file.Length >= MAXFILESIZE || (DateTime.Now - _createFileTime).Days > 0 || DateTime.Now.Day != _createFileTime.Day)
                {
                    m_writer.Dispose();
                    m_file.Dispose();
                    createFileStream();
                }
            }
        }
      
        public void LogJson(object obj)
        {
            if (m_writer == null || obj == null)
                return;
            this.Log(Newtonsoft.Json.JsonConvert.SerializeObject(obj));
        }
        public void Log(string content, params object[] objs)
        {
            if (m_writer == null || content == null)
                return;
            if (objs != null && objs.Length > 0)
                content = string.Format(content, objs);
            Log(content);
        }

        public void Close()
        {
            if (m_writer != null)
            {
                try
                {
                    m_writer.Dispose();
                    m_file.Dispose();
                }
                catch
                {
                }
                m_writer = null;
            }
            if (_autoclose == false)
            {
                CodeLog existLog = RunningLogs.FirstOrDefault(m => m.Name == this.Name);
                if (existLog != null)
                    RunningLogs.Remove(existLog);
            }
        }
        void IDisposable.Dispose()
        {
            if (_autoclose)
            {
                this.Close();
            }
        }
    }

    public class CLog : CodeLog
    {
        public static string SaveFolder = null;
        static bool setedEnabled = false;
        static string _SavePath;
        public override string SavePath
        {
            get
            {
                if (!setedEnabled)
                {
                    try
                    {
                        SetEnable(true);
                        setedEnabled = true;
                    }
                    catch
                    {
                    }
                }
                if (_SavePath == null)
                {
                    if (SaveFolder != null)
                    {
                        _SavePath = SaveFolder;
                        if (_SavePath.Contains("\\"))
                        {
                            _SavePath = _SavePath.Replace("\\", "/");
                        }

                        while (_SavePath.EndsWith("/"))
                            _SavePath = _SavePath.Substring(0, _SavePath.Length - 1);

                    }
                    else
                    {
                        _SavePath = PlatformHelper.GetAppDirectory() + "CLog";
                    }
                    
                    if (_mSavePath == null)
                    {
                        _mSavePath = _SavePath;
                        new System.Threading.Thread(deleteFiles) { IsBackground = true }.Start();
                    }
                }
                return _SavePath;
            }
        }

        static string _mSavePath;
        void deleteFiles()
        {
            while (true)
            {

                try
                {
                    string[] files = System.IO.Directory.GetFiles(_SavePath);
                    foreach (string file in files)
                    {
                        int minutes = 60 * 24;
                        if (file.Contains("error"))
                        {
                            minutes = 60 * 24 * 2;
                        }
                        DateTime writetime = new System.IO.FileInfo(file).LastWriteTime;
                        if ((DateTime.Now - writetime).TotalMinutes > minutes)
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
                System.Threading.Thread.Sleep(60000 * 60);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name">日志名称，空字符串表示不记录日志</param>
        public CLog(string name)
            : base(name)
        {
        }
        public CLog(string name, bool autoDispose)
            : base(name, autoDispose)
        {
        }
    }
}
