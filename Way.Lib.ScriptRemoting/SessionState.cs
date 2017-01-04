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

        internal static System.Collections.Hashtable ThreadSessions = Hashtable.Synchronized(new Hashtable());
        public delegate void OnSessionRemovedHandler(SessionState session);
        public static event OnSessionRemovedHandler OnSessionRemoved;

        public string SessionID
        {
            get;
            private set;
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
        internal static SessionState GetSession(string sessionid , string clientIP)
        {
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
            }
            obj.LastUseTime = DateTime.Now;
            if (ThreadSessions.ContainsKey(Thread.CurrentThread) == false)
            {
                ThreadSessions[Thread.CurrentThread] = obj;
            }
            return obj;
        }

        internal static void CheckSessionTimeout()
        {
            var timeout = 30;
            while(true)
            {
                try
                {
                    foreach (var kv in AllSessions)
                    {
                        if((DateTime.Now - kv.Value.LastUseTime).TotalMinutes > timeout)
                        {
                            lock (AllSessionsLock)
                            {
                                //在lock中重新判断
                                if ((DateTime.Now - kv.Value.LastUseTime).TotalMinutes > timeout)
                                {
                                    AllSessions.Remove(kv.Key);
                                    if(OnSessionRemoved != null)
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
                catch
                {

                }
                Thread.Sleep(60000);
            }
        }

       
    }
}
