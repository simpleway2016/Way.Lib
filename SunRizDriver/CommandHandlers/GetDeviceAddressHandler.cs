using SunRizDriver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Way.Lib;

namespace SunRizDriver.CommandHandlers
{
    /// <summary>
    /// 
    /// </summary>
    class GetDeviceAddressHandler : CommandHandler
    {
        SunRizDriverServer serverObj;
        public GetDeviceAddressHandler(SunRizDriverServer _serverObj)
        {
            this.serverObj = _serverObj;
        }

        public override void Handle(NetStream stream, Command command)
        {
            var str = serverObj.GetDeviceAddress(command);
            var bs = System.Text.Encoding.UTF8.GetBytes(str);
            stream.Write(bs.Length);
            stream.Write(bs , bs.Length);
        }

    }
}
