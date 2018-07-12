using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace Way.Lib
{

    public class CodeLog : IDisposable
    {
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


        protected virtual bool Enable
        {
            get
            {
                return true;
            }
        }

        /// <summary>
        /// 文件最大大小
        /// </summary>
        const int MAXFILESIZE = 1024 * 1024 * 2;
        bool _autoclose;
        DateTime _createFileTime;
        Action<string> _writeContentFunc;
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
            if (!string.IsNullOrEmpty(name) && this.Enable)
            {
                name = name.Replace("/", "_").Replace("\\", "_");
                _autoclose = autoClose;
                if (System.IO.Directory.Exists(SavePath) == false)
                    Directory.CreateDirectory(SavePath);
                if (!this.Enable)
                    return;
                this.Name = name;
                if (autoClose == false)
                {
                    lock (RunningLogs)
                    {
                        CodeLog existLog = RunningLogs.FirstOrDefault(m => m.Name == name);
                        if (existLog != null)
                        {
                            m_writer = existLog.m_writer;
                            m_file = existLog.m_file;
                            lockObj = existLog.lockObj;
                            _writeContentFunc = existLog._writeContentFunc;
                            return;
                        }
                        else
                        {
                            lockObj = new object();
                            createFileStream();
                            _writeContentFunc = (content) => writelogAction(content);
                            RunningLogs.Add(this);
                        }
                    }
                }
                else
                {
                    lockObj = new object();
                    createFileStream();
                    _writeContentFunc = (content) => writelogAction(content);
                }
               
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

        object lockObj;
        /// <summary>
        /// 写入日志
        /// </summary>
        /// <param name="content"></param>
        public void Log(string content)
        {
            if (this.Enable == false || m_writer == null || content == null)
                return;
            lock (lockObj)
            {
                _writeContentFunc(content);
            }
        }

        /// <summary>
        /// 写入日志,只有DEBUG模式下才会执行
        /// </summary>
        /// <param name="content"></param>
        [Conditional("DEBUG")]
        public void DebugLog(string content)
        {
            Log(content);
        }
      
        void writelogAction(string content)
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
        /// <summary>
        /// 写入日志,只有DEBUG模式下才会执行
        /// </summary>
        /// <param name="content"></param>
        [Conditional("DEBUG")]
        public void DebugLogJson(string content)
        {
            LogJson(content);
        }
        /// <summary>
        /// 写入日志
        /// </summary>
        /// <param name="obj"></param>
        public void LogJson(object obj)
        {
            if (this.Enable == false || m_writer == null || obj == null)
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
            if (_autoclose && m_writer != null)
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

        static string _SavePath;
        public override string SavePath
        {
            get
            {
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
#if NET46
                        try
                        {
                            if (!string.IsNullOrEmpty(System.Web.HttpRuntime.BinDirectory))
                            {
                                var bin = System.Web.HttpRuntime.BinDirectory;
                                while (bin.EndsWith("\\"))
                                    bin = bin.Substring(0, bin.Length - 1);
                                bin = System.IO.Path.GetDirectoryName(bin);
                                _SavePath = bin + "\\App_Data\\CLog";
                            }
                            else
                            {
                                _SavePath = PlatformHelper.GetAppDirectory() + "CLog";
                            }
                        }
                        catch
                        {
                            _SavePath = PlatformHelper.GetAppDirectory() + "CLog";
                        }
#else
                        _SavePath = PlatformHelper.GetAppDirectory() + "CLog";
#endif
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="autoDispose">如果是false，表示不会生成多个日志文件</param>
        public CLog(string name, bool autoDispose)
            : base(name, autoDispose)
        {
        }
    }

}
