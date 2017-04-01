using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Way.Lib.ScriptRemoting
{
    public interface ICustomHttpHandler
    {
        /// <summary>
        /// 自定义处理socket
        /// </summary>
        /// <param name="originalUrl"></param>
        /// <param name="connectInfo"></param>
        /// <param name="handled">是否已经处理，如果是true，系统将不再处理</param>
        void Handle(string originalUrl, HttpConnectInformation connectInfo, ref bool handled);
    }
}
