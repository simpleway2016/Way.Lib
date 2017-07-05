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
        private static ThreadLocal<RemotingController> _CurrentController = new ThreadLocal<RemotingController>();
        /// <summary>
        /// 获取当前上下文对应的RemotingController
        /// </summary>
        public static RemotingController CurrentController
        {
            get
            {
                return _CurrentController.Value;
            }
            set
            {
                _CurrentController.Value = value;
            }
        }


    }
}
