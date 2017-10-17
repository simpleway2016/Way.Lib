using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Way.Lib;

namespace SunRizDriver.CommandHandlers
{
    class EnumDevicePointHandler : CommandHandler
    {
        SunRizDriver.SunRizDriverServer serverObj;
        public EnumDevicePointHandler(SunRizDriver.SunRizDriverServer _serverObj)
        {
            this.serverObj = _serverObj;
        }
        public override void Handle(NetStream stream, Command command)
        {
            this.serverObj.EnumPoints(command , (path)=> {
                byte[] bs = System.Text.Encoding.UTF8.GetBytes(path);
                stream.Write(bs.Length);
                stream.Write(bs);
            });
            stream.Write(-1);//表示结束
        }
    }
}
