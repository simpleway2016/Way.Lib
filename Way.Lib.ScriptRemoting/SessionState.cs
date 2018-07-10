using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Way.Lib.ScriptRemoting
{
    /// <summary>
    /// 
    /// </summary>
    public class SessionState : Dictionary<string,object>
    {
        internal string ClientIP;
        static object AllSessionsLock = new object();
        static Dictionary<string, SessionState> AllSessions = null;
        /// <summary>
        /// 最后一次使用session的时间
        /// </summary>
        internal DateTime LastUseTime;

        
        public delegate void OnSessionRemovedHandler(SessionState session);
        /// <summary>
        /// session超时，被移除时触发的事件
        /// </summary>
        public static event OnSessionRemovedHandler OnSessionRemoved;
        /// <summary>
        /// session超时时间，单位（分），默认30
        /// </summary>
        public static int Timeout
        {
            get;
            set;
        }

        public string SessionID
        {
            get;
            private set;
        }

        public new object this[string key]
        {
            get
            {
                if (base.ContainsKey(key))
                    return base[key];
                return null;
            }
            set
            {
                base[key] = value;
            }
        }
        static SessionState()
        {
            AllSessions = new Dictionary<string, SessionState>();
            new Task(CheckSessionTimeout).Start();
        }
        internal SessionState(string sessionid,string ip)
        {
            this.ClientIP = ip;
            this.SessionID = sessionid;
        }

        /// <summary>
        /// 把当前服务器所有session保存到本地，下次启动server，您可以调用LoadSessionFromLocal从本地恢复session
        /// </summary>
        public static void SaveSessionsToLocal()
        {
            if (AllSessions == null)
                return;

            string data = null;
            lock (AllSessionsLock)
            {
                data = Way.Lib.Serialization.Serializer.SerializeObject(AllSessions);
            }
            var filepath = Way.Lib.PlatformHelper.GetAppDirectory() + "_$Way.Lib.ScriptRemoting.SessionBackup.dat";
            if (System.IO.File.Exists(filepath))
                System.IO.File.Delete(filepath);

            System.IO.File.WriteAllText(filepath, data, System.Text.Encoding.UTF8);
            System.IO.File.SetAttributes(filepath, System.IO.FileAttributes.Hidden);
        }
        /// <summary>
        /// 把SaveSessionsToLocal方法保存的本地session，加载到服务器上
        /// </summary>
        /// <param name="deleteAfterLoad">是否删除本地session</param>
        public static void LoadSessionFromLocal(bool deleteAfterLoad)
        {
            var filepath = Way.Lib.PlatformHelper.GetAppDirectory() + "_$Way.Lib.ScriptRemoting.SessionBackup.dat";
            if (System.IO.File.Exists(filepath) == false)
                return;

            string data = System.IO.File.ReadAllText(filepath, System.Text.Encoding.UTF8);
            lock (AllSessionsLock)
            {
                AllSessions = Way.Lib.Serialization.Serializer.DeserializeObject<Dictionary<string, SessionState>>(data);
            }

            if (deleteAfterLoad)
            {
                System.IO.File.Delete(filepath);
            }
        }
        /// <summary>
        /// 根据SessionId获取session对象
        /// </summary>
        /// <param name="sessionid"></param>
        /// <returns></returns>
        public static SessionState GetSessionById(string sessionid)
        {
            return AllSessions[sessionid];
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sessionid"></param>
        /// <returns></returns>
        internal static SessionState GetSession(string sessionid)
        {
            if (string.IsNullOrEmpty(sessionid))
                return null;

            string clientIP = RemotingContext.Current.GetIPInformation();

            SessionState obj = null;
            if (AllSessions.ContainsKey(sessionid))
            {
                try
                {
                    obj = AllSessions[sessionid];
                    if (clientIP != obj.ClientIP)
                        return null;
                }
                catch
                {
                    return null;
                }
            }
            else
            {
                obj = new ScriptRemoting.SessionState(sessionid, clientIP);
                lock (AllSessionsLock)
                {
                    AllSessions[sessionid] = obj;
                }               
            }
            obj.LastUseTime = DateTime.Now;

            return obj;
        }

        internal static void CheckSessionTimeout()
        {
          
            while(true)
            {
                if (Timeout <= 0)
                    Timeout = 1;
                try
                {
                    if (Timeout != int.MaxValue)
                    {
                        foreach (var kv in AllSessions)
                        {
                            if ((DateTime.Now - kv.Value.LastUseTime).TotalMinutes > Timeout)
                            {
                                //在lock中重新判断
                                if ((DateTime.Now - kv.Value.LastUseTime).TotalMinutes > Timeout)
                                {
                                    lock (AllSessionsLock)
                                    {
                                        AllSessions.Remove(kv.Key);
                                    }
                                    try
                                    {
                                        foreach (var keypair in kv.Value)
                                        {
                                            try
                                            {
                                                if (keypair.Value is IDisposable)
                                                {
                                                    ((IDisposable)keypair.Value).Dispose();
                                                }
                                            }
                                            catch { }
                                        }
                                    }
                                    catch
                                    {

                                    }

                                    if (OnSessionRemoved != null)
                                    {

                                        try
                                        {
                                            OnSessionRemoved(kv.Value);
                                        }
                                        catch
                                        {

                                        }
                                    }
                                    kv.Value.Clear();
                                }
                               
                                break;
                            }
                        }
                    }
                }
                catch
                {

                }
                Thread.Sleep(60000);
            }
        }

       
    }
    class SessionSaveInfoRoot
    {
        public string ClientIP;
        public string SessionID;
        public SessionSaveInfo[] keyValues;
    }
    class SessionSaveInfo
    {
        public string Type;
        public string Name;
        public string Value;
        public string Assembly;
    }
}
