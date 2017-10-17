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
    class WriteValueHandler : CommandHandler
    {
        SunRizDriverServer serverObj;
        public WriteValueHandler(SunRizDriverServer _serverObj)
        {
            this.serverObj = _serverObj;
        }

        public override void Handle(NetStream stream, Command command)
        {
            var results = serverObj.WriteValue(command);
            foreach(var r in results)
            {
                stream.Write(r);
            }
        }

       
    }
}
