using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Way.Lib;

namespace SunRizDriver.CommandHandlers
{
    class GetNameHandler : CommandHandler
    {
        SunRizDriver.SunRizDriverServer serverObj;
        public GetNameHandler(SunRizDriver.SunRizDriverServer _serverObj)
        {
            this.serverObj = _serverObj;
        }
        public override void Handle(NetStream stream, Command command)
        {
            byte[] bs = System.Text.Encoding.UTF8.GetBytes(serverObj.Name);
            stream.Write(bs.Length);
            stream.Write(bs);
        }
    }
}
