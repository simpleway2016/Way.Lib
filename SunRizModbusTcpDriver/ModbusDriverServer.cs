using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SunRizDriver;
using Way.Lib;
using System.Threading;

namespace SunRizModbusTcpDriver
{
    public class ModbusDriverServer : SunRizDriver.SunRizDriverServer
    {
        public override string Name => "Modbus Tcp Driver";
        public ModbusDriverServer(int port):base(port)
        {

        }

        public override void AddPointToWatch(Command command, Action<int, PointValueType, object> onValueReceive)
        {
            string[] deviceAddressInfo = command.DeviceAddress.Split('/');
            var deviceIP = deviceAddressInfo[0];
            var port = int.Parse(deviceAddressInfo[1]);
            PointDescription[] points = new PointDescription[command.Points.Length];
            for (int i = 0; i < points.Length; i++)
            {
                points[i] = new PointDescription(command.Points[i], i);
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

            Way.Lib.NetStream client = new Way.Lib.NetStream(deviceIP, port);
            readValues(client, groups);
            //把值通过stream发送出去
            for (int i = 0; i < points.Length; i++)
            {
                onValueReceive(points[i].Index, PointValueType.Short, points[i].Value);
            }
            while (this.Started == SunRizDriverServer.ServerStatus.Running)
            {
                Thread.Sleep(command.Interval);
                readValues(client, groups);

                for (int i = 0; i < points.Length; i++)
                {
                    if (points[i].Value != points[i].OriginalValue)
                    {
                        points[i].OriginalValue = points[i].Value;
                        onValueReceive(points[i].Index, PointValueType.Short, points[i].Value);
                    }
                }
            }
            client.Dispose();
        }

        void readValues(NetStream client, List<PointGroup> groups)
        {
            foreach (var group in groups)
            {
                ReadValuePackage package = new ReadValuePackage(group.Function);
                package.StartAddress = group.StartAddress;
                package.Quantity = group.Points.Count;
                var cmd = package.BuildCommand();
                client.Write(cmd);

                var result = package.ParseAnswer((len) => {
                    return client.ReceiveDatas(len);
                });
                for (int i = 0; i < result.Length; i++)
                {
                    group.Points[i].Value = result[i];
                    if (group.Points[i].OriginalValue == null)
                        group.Points[i].OriginalValue = result[i];
                }
            }

        }

        public override bool[] WriteValue(Command command)
        {
            string[] deviceAddressInfo = command.DeviceAddress.Split('/');
            var deviceIP = deviceAddressInfo[0];
            var port = int.Parse(deviceAddressInfo[1]);
            PointDescription[] points = new PointDescription[command.Points.Length];
            for (int i = 0; i < points.Length; i++)
            {
                points[i] = new PointDescription(command.Points[i], i);
                points[i].Value = Convert.ToInt16( Convert.ToDouble( command.Values[i]));
                if (points[i].Function == FunctionCode.ReadCoilStatus)
                    points[i].Function = FunctionCode.WriteCoilStatus;
                if (points[i].Function == FunctionCode.ReadHoldingRegister)
                    points[i].Function = FunctionCode.WriteHoldingRegister;
            }

            Way.Lib.NetStream client = new Way.Lib.NetStream(deviceIP, port);
            bool[] result = new bool[points.Length];
            foreach (var point in points)
            {
                try
                {
                    WriteValuePackage package = new WriteValuePackage(point.Function);
                    package.Address = point.Address;
                    package.Value = point.Value;
                    var cmd = package.BuildCommand();
                    client.Write(cmd);

                    result[point.Index] = package.ParseAnswer((len) =>
                    {
                        return client.ReceiveDatas(len);
                    });
                }
                catch
                {
                    client = new Way.Lib.NetStream(deviceIP, port);
                }
            }
            client.Dispose();
            return result;
        }

        public override void ReadValue(Command command, Action<int, PointValueType, object> onValueReceive)
        {
            string[] deviceAddressInfo = command.DeviceAddress.Split('/');
            var deviceIP = deviceAddressInfo[0];
            var port = int.Parse(deviceAddressInfo[1]);
            PointDescription[] points = new PointDescription[command.Points.Length];
            for (int i = 0; i < points.Length; i++)
            {
                points[i] = new PointDescription(command.Points[i], i);
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


            Way.Lib.NetStream client = new Way.Lib.NetStream(deviceIP, port);
            readValues(client, groups);
            //把值通过stream发送出去
            for (int i = 0; i < points.Length; i++)
            {
                onValueReceive(points[i].Index, PointValueType.Short, points[i].Value);
            }
            client.Dispose();
        }
        public override string[] GetPointProperties(Command command)
        {
            return new string[] { "Modbus功能码", "Modbus点地址" };
        }
        public override string GetPointAddress(Command command)
        {
            Newtonsoft.Json.Linq.JToken proObj =(Newtonsoft.Json.Linq.JToken) command.Values[0];
            return $"{ proObj.Value<string>("Modbus功能码")}/{ proObj.Value<string>("Modbus点地址")}";
        }

        public override string[] GetDeviceProperties(Command command)
        {
            return new string[] { "IP地址", "端口{defaultValue:502}" };
        }
        public override string GetDeviceAddress(Command command)
        {
            Newtonsoft.Json.Linq.JToken proObj = (Newtonsoft.Json.Linq.JToken)command.Values[0];
            return $"{ proObj.Value<string>("IP地址")}/{ proObj.Value<string>("端口")}";
        }

        public override bool CheckDeviceExist(Command command)
        {

            string[] deviceAddressInfo = command.DeviceAddress.Split('/');
            var deviceIP = deviceAddressInfo[0];
            var port = int.Parse(deviceAddressInfo[1]);
            try
            {
                Way.Lib.NetStream client = new Way.Lib.NetStream(deviceIP, port);

                client.Dispose();

            }
            catch
            {
                return false;
            }
            return true;
        }
    }
}
