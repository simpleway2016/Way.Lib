using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace SunRizServer.HistoryRecord
{
    class HistoryAutoRec
    {
        static List<MyDriverClient> AllClients = new List<MyDriverClient>();
        /// <summary>
        /// 开始记录历史
        /// </summary>
        public static void ReStart()
        {
            if(AllClients.Count > 0)
            {
                foreach( var c in AllClients )
                {
                    c.Released = true;
                    c.NetClient.Close();
                }
                AllClients.Clear();
            }
            new Thread(start).Start();
        }

        static void start()
        {
            try
            {
                using (SysDB db = new SysDB())
                {
                    var pointGroups = from m in db.DevicePoint
                                     where m.ValueRelativeChange == true || m.ValueAbsoluteChange == true || m.ValueOnTimeChange == true
                                     group m by m.DeviceId into g
                                     select g;
                    foreach( var group in pointGroups )
                    {
                        var deviceId = group.Key.GetValueOrDefault();
                        var device = db.Device.AsTracking().FirstOrDefault(m => m.id == deviceId);
                        var driver = db.CommunicationDriver.AsTracking().FirstOrDefault(m => m.id == device.DriverID);

                        var points = group.ToArray();
                        MyDriverClient client = new MyDriverClient(driver.Address , driver.Port.Value);
                        client.Points = points;
                        AllClients.Add(client);
                        string[] pointAddrs = new string[points.Length];
                        for(int i = 0; i < points.Length; i ++)
                        {
                            pointAddrs[i] = points[i].Address;
                        }
                        watchClient(client, device, pointAddrs);
                    }
                }
            }
            catch(Exception ex)
            {
                using (Way.Lib.CLog log = new Way.Lib.CLog("HistoryAutoRec error "))
                {
                    log.Log(ex.ToString());
                }
            }
        }

        static Newtonsoft.Json.Linq.JObject GetJsonObject(DevicePoint point)
        {
            var detail = new Newtonsoft.Json.Linq.JObject();
            detail["IsSquare"] = point.IsSquare.GetValueOrDefault();
            detail["IsTransform"] = point.IsTransform.GetValueOrDefault();
            detail["IsLinear"] = point.IsLinear.GetValueOrDefault();
            detail["DPCount"] = point.DPCount;
            if (point.IsTransform == true)
            {
                detail["SensorMax"] = point.SensorMax;
                detail["SensorMin"] = point.SensorMin;
            }
            if (point.IsLinear == true)
            {
                detail["LinearX1"] = point.LinearX1;
                detail["LinearX2"] = point.LinearX2;
                detail["LinearX3"] = point.LinearX3;
                detail["LinearX4"] = point.LinearX4;
                detail["LinearX5"] = point.LinearX5;
                detail["LinearX6"] = point.LinearX6;
                detail["LinearY1"] = point.LinearY1;
                detail["LinearY2"] = point.LinearY2;
                detail["LinearY3"] = point.LinearY3;
                detail["LinearY4"] = point.LinearY4;
                detail["LinearY5"] = point.LinearY5;
                detail["LinearY6"] = point.LinearY6;
            }
            var data = new Newtonsoft.Json.Linq.JObject();
            data["detail"] = detail;
            data["max"] = point.TransMax;
            data["min"] = point.TransMin;

            return data;
        }

        static void watchClient(MyDriverClient client , Device device , string[] pointAddrs)
        {
            client.NetClient = client.AddPointToWatch(device.Address, pointAddrs, (addr, value) => 
            {
                var point = client.Points.FirstOrDefault(m => m.Address == addr);
                if(point != null)
                {
                    var jobj = GetJsonObject(point);
                    value = SunRizDriver.Helper.Transform(jobj, value);
                    System.Diagnostics.Debug.WriteLine($"name:{addr} value:{value}");
                }
               
            }, (err) => {
                if (client.Released)
                    return;

                Task.Run(()=> {
                    Thread.Sleep(2000);
                    watchClient(client, device, pointAddrs);
                });
            });
        }
    }

    class MyDriverClient : SunRizDriver.SunRizDriverClient
    {
        public bool Released = false;
        public Way.Lib.NetStream NetClient;
        public DevicePoint[] Points;
        public MyDriverClient(string addr, int port) : base(addr, port)
        {

        }
    }
}
