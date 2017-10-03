using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SunRizDriver;

namespace SunRizModbusTcpDriver
{
    public class ModbusDriverServer : SunRizDriver.SunRizDriverServer
    {
        public override string Name => "Modbus Tcp Driver";
        public ModbusDriverServer(int port):base(port)
        {

        }

        protected override CommandHandler GetHandler(string type)
        {
            if (type == "AddPointToWatch")
                return new AddPointToWatchHandler(this);
            else if (type == "WriteValue")
                return new WriteValueHandler(this);
            return base.GetHandler(type);
        }
    }
}
