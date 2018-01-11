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
        public static void Start()
        {
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

        static void watchClient(MyDriverClient client , Device device , string[] pointAddrs)
        {
            client.NetClient = client.AddPointToWatch(device.Address, pointAddrs, (addr, value) => 
            {
                System.Diagnostics.Debug.WriteLine($"name:{addr} value:{value}");
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
        public SunRizServer.Device Device;
        public MyDriverClient(string addr, int port) : base(addr, port)
        {

        }
    }
}
