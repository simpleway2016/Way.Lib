using System;
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
        /// <returns>如果返回null，表示不改变路由</returns>
        string GetUrl(string originalUrl);
    }
}
