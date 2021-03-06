﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace Way.Lib.ScriptRemoting
{
    public class RemotingContext
    {
        internal HttpServer Server;
        //如果使用AsyncLocal，可以让异步上下文也能访问，但是不是同一个上下文的其他线程是不可以访问的
        //private static AsyncLocal<RemotingController> _CurrentController = new AsyncLocal<RemotingController>();
        //不要使用ThreadLocal，因为ThreadLocal在Task.Run ThreadPool的情况下，会不准确
        private static ThreadLocal<RemotingContext> _Current = new ThreadLocal<RemotingContext>();
        /// <summary>
        /// 获取当前上下文对应的RemotingController
        /// </summary>
        public static RemotingContext Current
        {
            get
            {
                if (_Current.Value == null)
                    _Current.Value = new RemotingContext();
                return _Current.Value;
            }
            internal set
            {
                if (value != null)
                    throw new Exception("can not set RemotingContext.Current");
                if(_Current.Value != null)
                {
                    _Current.Value = null;
                }
            }
        }

        public RemotingController Controller
        {
            get;
            internal set;
        }
        /// <summary>
        /// 获取web所在文件夹
        /// </summary>
        public string WebRoot
        {
            get
            {
                return Server.Root;
            }
        }

        public string GetIPInformation()
        {
            if (Request == null)
                throw new Exception("Request is null");
            StringBuilder ip = new StringBuilder(Request.RemoteEndPoint.ToString().Split(':')[0]);
            if (Request.Headers.ContainsKey("X-Real-IP"))
            {
                ip.Append(",");
                ip.Append("X-Real-IP:");
                ip.Append(Request.Headers["X-Real-IP"]);
            }
            if (Request.Headers.ContainsKey("X-Forwarded-For"))
            {
                ip.Append(",");
                ip.Append("X-Forwarded-For:");
                ip.Append(Request.Headers["X-Forwarded-For"]);
            }
            return ip.ToString();
        }

        public Net.Request Request
        {
            get;
            internal set;
        }
        public Net.Response Response
        {
            get;
            internal set;
        }
    }
}
