using System;
using System.Collections.Generic;
using System.Text;

namespace Way.Lib.ScriptRemoting
{
    /// <summary>
    /// 定义RemotingController对应的url
    /// </summary>
    public class RemotingUrlAttribute :Attribute
    {
        public string Url
        {
            get;
            private set;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="url">如：Home/Setting 可以响应 http://www.host.com/Home/Setting/method 的请求</param>
        public RemotingUrlAttribute(string url)
        {
            this.Url = url;
            if (!this.Url.StartsWith("/"))
                this.Url = "/" + this.Url;
            if (!this.Url.EndsWith("/"))
                this.Url += "/";
        }
    }
}
