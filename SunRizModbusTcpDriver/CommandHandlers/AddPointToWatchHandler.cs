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
    class AddPointToWatchHandler : CommandHandler
    {
        ModbusDriverServer serverObj;
        string _deviceIP;
        int _port;
        public AddPointToWatchHandler(ModbusDriverServer _serverObj)
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
            }

            //找出相邻的点
            List<PointGroup> groups = new List<PointGroup>();
            points = (from m in points
                      orderby m.Function, m.Address
                      select m).ToArray();
            PointGroup currentGroup = new PointGroup();
            groups.Add(currentGroup);
            foreach (var point in points)
            {
                if (currentGroup.Points.Count == 0)
                {
                    currentGroup.Function = point.Function;
                    currentGroup.StartAddress = point.Address;
                    currentGroup.Points.Add(point);
                }
                else
                {
                    if (point.Address - currentGroup.Points.Last().Address == 1 && point.Function == currentGroup.Function)
                    {
                        currentGroup.Points.Add(point);
                    }
                    else
                    {
                        currentGroup = new PointGroup();
                        groups.Add(currentGroup);
                        currentGroup.Function = point.Function;
                        currentGroup.StartAddress = point.Address;
                        currentGroup.Points.Add(point);
                    }
                }
            }


            Way.Lib.NetStream client = new Way.Lib.NetStream(_deviceIP, _port);
            readValues(client, groups);
            //把值通过stream发送出去
            for(int i = 0; i < points.Length; i ++)
            {
                stream.Write(points[i].Index);//写index
                stream.Write((int)PointValueType.Short);
                stream.Write(points[i].Value);
            }
            while (serverObj.Started == SunRizDriverServer.ServerStatus.Running)
            {
                Thread.Sleep(command.Interval);
                readValues(client, groups);

                for (int i = 0; i < points.Length; i++)
                {
                    if (points[i].Value != points[i].OriginalValue)
                    {
                        points[i].OriginalValue = points[i].Value;
                        stream.Write(points[i].Index);//写index
                        stream.Write((int)PointValueType.Short);
                        stream.Write(points[i].Value);
                    }
                }
            }
            client.Dispose();
            stream.Close();
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
