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
    class ReadValueHandler : CommandHandler
    {
        SunRizDriverServer serverObj;
        public ReadValueHandler(SunRizDriverServer _serverObj)
        {
            this.serverObj = _serverObj;
        }

        public override void Handle(NetStream stream, Command command)
        {
            this.serverObj.ReadValue(command, (index, valuetype, value) => {
                stream.Write(index);//写index
                stream.Write((int)valuetype);
                if (valuetype == PointValueType.Short)
                    stream.Write((short)value);
                else if (valuetype == PointValueType.Int)
                    stream.Write((int)value);
                else if (valuetype == PointValueType.Float)
                    stream.Write((float)value);
                else if (valuetype == PointValueType.String)
                {
                    byte[] bs = System.Text.Encoding.UTF8.GetBytes((string)value);
                    stream.Write(bs.Length);
                    stream.Write(bs);
                }
                else
                {
                    stream.Dispose();
                }
            });
        }

    }
}
