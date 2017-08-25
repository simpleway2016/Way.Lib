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
        internal SessionState(string sessionid,string ip)
        {
            this.ClientIP = ip;
            this.SessionID = sessionid;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sessionid"></param>
        /// <returns></returns>
        internal static SessionState GetSession(string sessionid)
        {
            string clientIP = RemotingContext.Current.GetIPInformation();

            SessionState obj = null;
            lock (AllSessionsLock)
            {
                if(AllSessions == null)
                {
                    AllSessions = new Dictionary<string, SessionState>();
                    new Task(CheckSessionTimeout).Start();
                }
                if (AllSessions.ContainsKey(sessionid))
                {
                    obj = AllSessions[sessionid];
                    if (clientIP != obj.ClientIP)
                        return null;
                }
                else
                {
                    obj = new ScriptRemoting.SessionState(sessionid , clientIP);
                    AllSessions[sessionid] = obj;
                }
                obj.LastUseTime = DateTime.Now;
            }

            return obj;
        }

        internal static void CheckSessionTimeout()
        {
          
            while(true)
            {
                if (Timeout <= 0)
                    Timeout = 30;
                try
                {
                    if (Timeout != int.MaxValue)
                    {
                        foreach (var kv in AllSessions)
                        {
                            if ((DateTime.Now - kv.Value.LastUseTime).TotalMinutes > Timeout)
                            {
                                lock (AllSessionsLock)
                                {
                                    //在lock中重新判断
                                    if ((DateTime.Now - kv.Value.LastUseTime).TotalMinutes > Timeout)
                                    {
                                        AllSessions.Remove(kv.Key);
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
}
