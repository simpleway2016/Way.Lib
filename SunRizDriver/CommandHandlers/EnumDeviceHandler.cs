using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Way.Lib;

namespace SunRizDriver.CommandHandlers
{
    class EnumDeviceHandler : CommandHandler
    {
        SunRizDriver.SunRizDriverServer serverObj;
        public EnumDeviceHandler(SunRizDriver.SunRizDriverServer _serverObj)
        {
            this.serverObj = _serverObj;
        }
        public override void Handle(NetStream stream, Command command)
        {
            var devices = this.serverObj.EnumDevice(command);
            var json = Newtonsoft.Json.JsonConvert.SerializeObject(devices);
            byte[] bs = System.Text.Encoding.UTF8.GetBytes(json);
            stream.Write(bs.Length);
            stream.Write(bs);
        }
    }
}
