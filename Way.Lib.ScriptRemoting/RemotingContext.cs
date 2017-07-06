using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace Way.Lib.ScriptRemoting
{
    public class RemotingContext
    {
        //如果使用AsyncLocal，可以让异步上下文也能访问，但是不是同一个上下文的其他线程是不可以访问的
        //private static AsyncLocal<RemotingController> _CurrentController = new AsyncLocal<RemotingController>();
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
        }

        public RemotingController Controller
        {
            get;
            internal set;
        }

        ThreadLocal<Dictionary<string,object>> _Items = new ThreadLocal<Dictionary<string, object>>();
        /// <summary>
        /// 获取当前上下文对应的Items
        /// </summary>
        public Dictionary<string, object> Items
        {
            get
            {
                if (_Items.Value == null)
                    _Items.Value = new Dictionary<string, object>();

                return _Items.Value;
            }
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
