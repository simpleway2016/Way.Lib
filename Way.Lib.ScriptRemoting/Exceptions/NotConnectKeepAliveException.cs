using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Way.Lib.ScriptRemoting
{
    /// <summary>
    /// 客户端目前还没有成功与服务器建立长连接
    /// </summary>
    public class NotConnectKeepAliveException:Exception
    {
        public NotConnectKeepAliveException(string msg) : base(msg)
        {
        }
    }
}
