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
        /// <summary>
        /// 该Session当前有几个长连接
        /// </summary>
        internal int KeepAliveCount;

        internal ConcurrentDictionary<string, Action> OnKeepAliveConnectEvents = new ConcurrentDictionary<string, Action>();
        internal ConcurrentDictionary<string, Action> OnKeepAliveCloseEvents = new ConcurrentDictionary<string, Action>();
        internal static System.Collections.Hashtable ThreadSessions = Hashtable.Synchronized(new Hashtable());

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
                        if(kv.Value.KeepAliveCount <=0 && (DateTime.Now - kv.Value.LastUseTime).TotalMinutes > timeout)
                        {
                            lock (AllSessionsLock)
                            {
                                //在lock中重新判断
                                if (kv.Value.KeepAliveCount <= 0 && (DateTime.Now - kv.Value.LastUseTime).TotalMinutes > timeout)
                                {
                                    AllSessions.Remove(kv.Key);
                                    kv.Value.Clear();
                                    kv.Value.OnKeepAliveCloseEvents.Clear();
                                    kv.Value.OnKeepAliveConnectEvents.Clear();
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

        /// <summary>
        /// 标识增加一个长连接
        /// </summary>
        internal void KeepAliveEntry()
        {
            this.LastUseTime = DateTime.Now;
            System.Threading.Interlocked.Increment(ref KeepAliveCount);
        }
        /// <summary>
        /// 标识减少一个长连接
        /// </summary>
        internal void KeepAliveExit()
        {
            this.LastUseTime = DateTime.Now;
            System.Threading.Interlocked.Decrement(ref KeepAliveCount);
        }
    }
}
