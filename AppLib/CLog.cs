using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
            return _SavePath ?? (_SavePath = AppDomain.CurrentDomain.BaseDirectory + "CodeLog");
        }
    }
    public string Name
    {
        get;
        set;
    }
    bool _autoclose;
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
        name = name.Replace("/", "_").Replace("\\", "_");
        if (!string.IsNullOrEmpty(name) && ENABLED)
        {
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
            m_file = new FileStream(SavePath + "\\" + name + Guid.NewGuid() + ".txt", FileMode.Create,
                FileAccess.Write, FileShare.Read);
            m_writer = new StreamWriter(m_file);

        }
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
        if (m_writer == null)
            return;
        lock (lockObj)
        {
            m_writer.WriteLine(DateTime.Now + " " + content);
            m_writer.Flush();
        }
    }
  static  System.Web.Script.Serialization.JavaScriptSerializer json = new System.Web.Script.Serialization.JavaScriptSerializer();
  public void LogJson(object obj)
  {
      if (m_writer == null || obj == null)
          return;
      this.Log(json.Serialize(obj));
  }
    public void Log(string content, params object[] objs)
    {
        if (m_writer == null)
            return;
        if (objs != null && objs.Length > 0)
            content = string.Format(content, objs);
        lock (lockObj)
        {

            m_writer.WriteLine(DateTime.Now + " " + content);
            m_writer.Flush();
        }
    }

    public void Close()
    {
        if (m_writer != null)
        {
            try
            {
                m_writer.Close();
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
        static bool setedEnabled = false;
        static string _SavePath;
        public override string SavePath
        {
            get
            {
                if (!setedEnabled)
                {
                    try{
                    string config = System.Configuration.ConfigurationManager.AppSettings["CLog"];
                    if (string.IsNullOrEmpty(config) || config == "1" || config.ToLower() == "true")
                    {
                        SetEnable(true);
                    }
                    else
                    {
                        SetEnable(false);
                    }
                    setedEnabled = true;
                    }
                    catch{
                    }
                }
                if (_SavePath == null)
                {
                    try
                    {
                        if (System.Web.HttpRuntime.AppDomainAppPath != null)
                        {
                            _SavePath = System.Web.HttpRuntime.AppDomainAppPath + "CLog";
                            if (_mSavePath == null)
                            {
                                _mSavePath = _SavePath;
                                new System.Threading.Thread(deleteFiles).Start();
                            }
                        }
                        else
                        {
                            _SavePath = AppDomain.CurrentDomain.BaseDirectory + "\\CLog";
                            if (_mSavePath == null)
                            {
                                _mSavePath = _SavePath;
                                new System.Threading.Thread(deleteFiles).Start();
                            }
                        }
                    }
                    catch
                    {
                        _SavePath = AppDomain.CurrentDomain.BaseDirectory + "\\CLog";
                        if (_mSavePath == null)
                        {
                            _mSavePath = _SavePath;
                            new System.Threading.Thread(deleteFiles) { IsBackground = true}.Start();
                        }
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
                        int minutes = 60*24;
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
        public CLog(string name,bool autoDispose)
            : base(name,autoDispose)
        {
        }
    }

