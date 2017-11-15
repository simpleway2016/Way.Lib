using OPC.Data;
using SunRizDriver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SunRizOpcDriver
{
    public class OpcDriverServer:SunRizDriverServer
    {
        public override string Name => "OPC Driver";
        public override bool SupportEnumDevice => true;
        public override bool SupportEnumPoints => true;

        public OpcDriverServer(int port):base(port)
        {

        }

        public override string[] EnumDevice(Command command)
        {
            string opcip = command.DeviceAddress;
            if (string.IsNullOrEmpty(opcip))
                opcip = "127.0.0.1";
            if (opcip.Contains("|"))
                opcip = opcip.Substring(0, opcip.IndexOf("|"));

            OPC.Common.OpcServerList list = new OPC.Common.OpcServerList();
            var opcservers = list.ListAllData20(opcip);
            string[] result = new string[opcservers.Length];
            for(int i = 0; i < opcservers.Length; i ++)
            {
                result[i] = opcservers[i].ProgID;
            }

            return result;
        }

        public override void EnumPoints(Command command, Action<string> onFindPoint)
        {
            int index = command.DeviceAddress.IndexOf("|");
            string ip = command.DeviceAddress.Substring(0, index);
            string progid = command.DeviceAddress.Substring(index + 1);

            OPC.Data.OpcServer server = new OPC.Data.OpcServer();
            server.Connect(progid, ip);
            server.AsyncGetNode += new OPC.Data.OpcServer.AsyncGetNodeHandler((tag, node) => {
                var obj = new PointInfomation{
                    Path = node.ID,
                    IsFolder = !node.IsItemProperty,
                    Name = node.Name
                };
                onFindPoint( Newtonsoft.Json.JsonConvert.SerializeObject(obj) );
            });

            server.AsyncGetNodes(command.ParentPath, null , true);
        }

        public override void AddPointToWatch(Command command, Action<int, PointValueType, object> onValueReceive)
        {
            int index = command.DeviceAddress.IndexOf("|");
            string ip = command.DeviceAddress.Substring(0, index);
            string progid = command.DeviceAddress.Substring(index + 1);

            bool ready = false;

            OPC.Data.OpcServer opcServer = new OPC.Data.OpcServer();
            opcServer.Connect(progid, ip);

            OPC.Data.OpcGroup group1 = new OPC.Data.OpcGroup("g1", true, command.Interval, command.Interval, 0);
            group1.DataChanged += new OPC.Data.DataChangeEventHandler((sender, e)=> {
                if (ready)
                {
                    foreach (OPCItemState itemState in e.sts)
                    {
                        pushValue(itemState, onValueReceive);
                    }
                }
                
            });
            opcServer.OpcGroups.Add(group1);

            int tranid =0;
            foreach( var point in command.Points )
            {
                OPCItem item = new OPCItem(point, tranid ++);
                group1.Items.Add(item);
            }

            var states = group1.Items.GetItemValues();
            foreach (OPCItemState itemState in states)
            {
                pushValue(itemState, onValueReceive);
            }
            ready = true;

            while (true)
            {
                System.Threading.Thread.Sleep(5000);
                var opcState =  opcServer.GetStatus();
                if (opcState.eServerState != OPC.Data.Interface.OPCSERVERSTATE.OPC_STATUS_RUNNING)
                    throw new Exception("server error");
               
            }
        }

        void pushValue(OPCItemState itemState , Action<int, PointValueType, object> onValueReceive)
        {
            int itemTranId = itemState.HandleClient;
            if (itemState.DataValue == null)
            {
                onValueReceive(itemTranId, PointValueType.String, "");
            }
            else if (itemState.DataValue is Array)
            {
                onValueReceive(itemTranId, PointValueType.String, Newtonsoft.Json.JsonConvert.SerializeObject(itemState.DataValue));
            }
            else
            {
                onValueReceive(itemTranId, PointValueType.String, itemState.DataValue.ToString());
            }
        }

        public override void ReadValue(Command command, Action<int, PointValueType, object> onValueReceive)
        {
            int index = command.DeviceAddress.IndexOf("|");
            string ip = command.DeviceAddress.Substring(0, index);
            string progid = command.DeviceAddress.Substring(index + 1);


            OPC.Data.OpcServer opcServer = new OPC.Data.OpcServer();
            opcServer.Connect(progid, ip);

            OPC.Data.OpcGroup group1 = new OPC.Data.OpcGroup("g2", false, 1000,1000, 0);
            opcServer.OpcGroups.Add(group1);

            int tranid = 0;
            foreach (var point in command.Points)
            {
                OPCItem item = new OPCItem(point, tranid++);
                group1.Items.Add(item);
            }

            var states = group1.Items.GetItemValues();
            foreach (OPCItemState itemState in states)
            {
                pushValue(itemState, onValueReceive);
            }
            opcServer.Disconnect();

        }
        public override string[] GetPointProperties(Command command)
        {
            return new string[] { "OPC路径" };
        }
        public override string GetPointAddress(Command command)
        {
            Newtonsoft.Json.Linq.JToken proObj = (Newtonsoft.Json.Linq.JToken)command.Values[0];
            return proObj.Value<string>("OPC路径");
        }
        public override string[] GetDeviceProperties(Command command)
        {
            return new string[] { "IP地址", "OPC名称{isEnumDevice:true,addressProperty:\"IP地址\"}" };
        }
        public override string GetDeviceAddress(Command command)
        {
            Newtonsoft.Json.Linq.JToken proObj = (Newtonsoft.Json.Linq.JToken)command.Values[0];
            return $"{ proObj.Value<string>("IP地址")}|{ proObj.Value<string>("OPC名称")}";
        }
        public override bool[] WriteValue(Command command)
        {
            int index = command.DeviceAddress.IndexOf("|");
            string ip = command.DeviceAddress.Substring(0, index);
            string progid = command.DeviceAddress.Substring(index + 1);

            OPC.Data.OpcServer opcServer = new OPC.Data.OpcServer();
            opcServer.Connect(progid, ip);

            OPC.Data.OpcGroup group1 = new OPC.Data.OpcGroup("g3", false, 1000, 1000, 0);
            opcServer.OpcGroups.Add(group1);

            int tranid = 0;
            foreach (var point in command.Points)
            {
                OPCItem item = new OPCItem(point, tranid++);
                group1.Items.Add(item);
            }

            bool[] result = new bool[command.Values.Length];

            for (int i = 0; i < command.Values.Length; i ++)
            {
                if( command.Values[i] is Newtonsoft.Json.Linq.JArray)
                {
                    var jarray = (Newtonsoft.Json.Linq.JArray)command.Values[i];
                    if (jarray.Count > 0)
                    {
                        try
                        {
                            var newValue = Array.CreateInstance(((Newtonsoft.Json.Linq.JValue)jarray[0]).Value.GetType(), jarray.Count);
                            for (int j = 0; j < jarray.Count; j++)
                            {
                                newValue.SetValue(((Newtonsoft.Json.Linq.JValue)jarray[j]).Value, j);
                            }
                            command.Values[i] = newValue;
                            result[i] = true;
                        }
                        catch
                        {

                        }
                    }
                    else
                    {
                        result[i] = true;
                    }
                }
                else
                {
                    result[i] = true;
                }
            }

            group1.Items.WriteItemValues(command.Values);
            opcServer.Disconnect();
           
            return result;
        }

        public override bool CheckDeviceExist(Command command)
        {
            int index = command.DeviceAddress.IndexOf("|");
            string ip = command.DeviceAddress.Substring(0, index);
            string progid = command.DeviceAddress.Substring(index + 1);
            try
            {
                OPC.Data.OpcServer opcServer = new OPC.Data.OpcServer();
                opcServer.Connect(progid, ip);
                opcServer.Disconnect();
            }
            catch
            {
                return false;
            }
            return true;
        }
    }
}
