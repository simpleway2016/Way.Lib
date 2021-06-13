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
        /// <summary>
        /// 最后一次使用session的时间
        /// </summary>
        internal DateTime LastUseTime;

        
        public delegate void OnSessionRemovedHandler(SessionState session);
        /// <summary>
        /// session超时，被移除时触发的事件
        /// </summary>
        public static event OnSessionRemovedHandler SessionRemoved;


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

        internal SessionState(string sessionid,string ip)
        {
            this.ClientIP = ip;
            this.SessionID = sessionid;
        }

        internal static void OnSessionRemoved(SessionState session)
        {
            if(SessionRemoved != null)
            {
                SessionRemoved(session);
            }
        }

        /// <summary>
        /// 把当前服务器所有session保存到本地，下次启动server，您可以调用LoadSessionFromLocal从本地恢复session
        /// </summary>
        public static void SaveSessionsToLocal()
        {
            var context = RemotingContext.Current;
            if (context.Server.AllSessions == null)
                return;

            string data = Way.Lib.Serialization.Serializer.SerializeObject(context.Server.AllSessions);
            var filepath = AppDomain.CurrentDomain.BaseDirectory + "_$Way.Lib.ScriptRemoting.SessionBackup.dat";
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
            var context = RemotingContext.Current;
            var filepath = AppDomain.CurrentDomain.BaseDirectory + "_$Way.Lib.ScriptRemoting.SessionBackup.dat";
            if (System.IO.File.Exists(filepath) == false)
                return;

            string data = System.IO.File.ReadAllText(filepath, System.Text.Encoding.UTF8);
            context.Server.AllSessions = Way.Lib.Serialization.Serializer.DeserializeObject<ConcurrentDictionary<string, SessionState>>(data);

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
            var context = RemotingContext.Current;
            if (context == null)
                throw new Exception("RemotingContext.Current is Null");
            return context.Server.AllSessions[sessionid];
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
            var context = RemotingContext.Current;

            string clientIP = context.GetIPInformation();

            SessionState obj = null;
            if (context.Server.AllSessions.ContainsKey(sessionid))
            {
                try
                {
                    obj = context.Server.AllSessions[sessionid];
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
                context.Server.AllSessions.TryAdd(sessionid, obj);      
            }
            obj.LastUseTime = DateTime.Now;

            return obj;
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
