using System;
using System.Collections.Generic;
using System.Text;

namespace PandaAudioServer
{
    class IPError
    {
        static List<IPError> Errors = new List<IPError>();

        public string IP
        {
            get;
            private set;
        }
       
        public int ErrorCount
        {
            get;
            private set;
        }
        DateTime _LockTime;
        /// <summary>
        /// ip是否已经锁定
        /// </summary>
        /// <param name="ip"></param>
        /// <returns></returns>
        public static bool IsIPLocked(string ip)
        {
            for (int i = 0; i < Errors.Count; i++)
            {
                if (Errors[i].IP == ip)
                {
                    if (Errors[i].ErrorCount >= 10 )
                    {
                        return true;
                    }
                    return false;
                }
            }
            return false;
        }
        public static IPError GetInstance(string ip)
        {
            for(int i = 0; i < Errors.Count; i ++)
            {
                if(Errors[i].IP == ip)
                {
                    if(Errors[i].ErrorCount >= 10 && (DateTime.Now - Errors[i]._LockTime).TotalHours > 6)
                    {
                        Errors[i].ErrorCount = 0;
                    }
                    return Errors[i];
                }
            }
            var obj = new IPError(ip);
            lock (Errors)
            {
                Errors.Add(obj);
            }
            return obj;
        }

        public IPError(string ip)
        {
            this.IP = ip;
        }

        /// <summary>
        /// 记录一次错误
        /// </summary>
        public void MarkError()
        {
            this.ErrorCount++;
            if(this.ErrorCount >= 10)
            {
                _LockTime = DateTime.Now;
            }
        }
        public void Clear(string ip)
        {
            for (int i = 0; i < Errors.Count; i++)
            {
                if (Errors[i].IP == ip)
                {
                    lock(Errors)
                    {
                        Errors.RemoveAt(i);
                    }
                    return;
                }
            }
        }
       
    }
}
