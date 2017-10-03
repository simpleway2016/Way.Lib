using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Way.Lib;

namespace SunRizDriver.CommandHandlers
{
    class GetServerInfoHandler : CommandHandler
    {
        SunRizDriver.SunRizDriverServer serverObj;
        public GetServerInfoHandler(SunRizDriver.SunRizDriverServer _serverObj)
        {
            this.serverObj = _serverObj;
        }
        public override void Handle(NetStream stream, Command command)
        {
            var serverinfo = new ServerInfo() {
                Name = serverObj.Name,
                SupportEnumDevice = serverObj.SupportEnumDevice,
                SupportEnumPoints = serverObj.SupportEnumPoints
            };
            byte[] bs = System.Text.Encoding.UTF8.GetBytes(Newtonsoft.Json.JsonConvert.SerializeObject(serverinfo));
            stream.Write(bs.Length);
            stream.Write(bs);
        }
    }
}
