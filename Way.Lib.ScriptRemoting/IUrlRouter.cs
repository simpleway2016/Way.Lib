﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Way.Lib.ScriptRemoting
{
    /// <summary>
    /// 自定义路由接口
    /// </summary>
    public interface IUrlRouter
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="originalUrl">原始url</param>
        /// <param name="fromUrl">从哪个url访问的</param>
        /// <param name="session"></param>
        /// <param name="headers">http头部数据</param>
        /// <returns>如果返回null，表示不改变路由</returns>
        string GetUrl(string originalUrl,string fromUrl,SessionState session,System.Collections.Hashtable headers);
    }
}
