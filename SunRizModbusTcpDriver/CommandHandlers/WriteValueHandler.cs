using SunRizDriver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Way.Lib;

namespace SunRizModbusTcpDriver
{
    class WriteValueHandler : CommandHandler
    {
        ModbusDriverServer serverObj;
        string _deviceIP;
        int _port;
        public WriteValueHandler(ModbusDriverServer _serverObj)
        {
            this.serverObj = _serverObj;
        }

        public override void Handle(NetStream stream, Command command)
        {
            string[] deviceAddressInfo = command.DeviceAddress.Split('/');
            _deviceIP = deviceAddressInfo[0];
            _port = int.Parse( deviceAddressInfo[1]);
            PointDescription[] points = new PointDescription[command.Points.Length];
            for(int i = 0; i < points.Length; i ++)
            {
                points[i] = new PointDescription(command.Points[i] , i);
                points[i].Value = Convert.ToInt16(command.Values[i]);
                if (points[i].Function == FunctionCode.ReadCoilStatus)
                    points[i].Function = FunctionCode.WriteCoilStatus;
                if (points[i].Function == FunctionCode.ReadHoldingRegister)
                    points[i].Function = FunctionCode.WriteHoldingRegister;
            }

            Way.Lib.NetStream client = new Way.Lib.NetStream(_deviceIP, _port);
            foreach( var point in points)
            {
                WriteValuePackage package = new WriteValuePackage(point.Function);
                package.Address = point.Address;
                package.Value = point.Value;
                var cmd = package.BuildCommand();
                client.Write(cmd);

                var result = package.ParseAnswer((len) => {
                    return client.ReceiveDatas(len);
                });
                stream.Write(result);
            }
            client.Dispose();
        }

        void readValues( NetStream client , List<PointGroup> groups)
        { 
            foreach( var group in groups )
            {
                ReadValuePackage package = new ReadValuePackage(group.Function);
                package.StartAddress = group.StartAddress;
                package.Quantity = group.Points.Count;
                var cmd = package.BuildCommand();
                client.Write(cmd);

                var result = package.ParseAnswer((len) => {
                    return client.ReceiveDatas(len);
                });
                for(int i = 0; i < result.Length; i ++)
                {
                    group.Points[i].Value = result[i];
                    if(group.Points[i].OriginalValue == null)
                        group.Points[i].OriginalValue = result[i];
                }
            }
           
        }
    }
}
