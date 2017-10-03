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
    /// 检查设备是否可以连接
    /// </summary>
    class CheckDeviceExistHandler : CommandHandler
    {
        SunRizDriverServer serverObj;
        public CheckDeviceExistHandler(SunRizDriverServer _serverObj)
        {
            this.serverObj = _serverObj;
        }

        public override void Handle(NetStream stream, Command command)
        {
            stream.Write(serverObj.CheckDeviceExist(command));
        }

    }
}
