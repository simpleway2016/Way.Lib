using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Collections.Concurrent;
using System.Threading.Tasks;
using System.Threading;

namespace SunRizServer.Controllers
{
    class DeviceListener
    {
        static ConcurrentDictionary<string, WatchGroup[]> RunningWatches = new ConcurrentDictionary<string, WatchGroup[]>();
        /// <summary>
        /// 添加数据监控客户端
        /// </summary>
        /// <param name="groupName"></param>
        /// <param name="db"></param>
        /// <param name="points"></param>
        public static void AddClient(string groupName, SysDB db, DevicePoint[] points)
        {
            List<WatchGroup> watches = new List<WatchGroup>();
            foreach (var point in points)
            {
                var watch = watches.FirstOrDefault(m => m.DeviceId == point.DeviceId);
                if (watch == null)
                {
                    var device = db.Device.FirstOrDefault(m => m.id == point.DeviceId);
                    var gateway = db.CommunicationDriver.FirstOrDefault(m => m.id == device.DriverID);

                    watch = new WatchGroup();
                    watch.DeviceAddress = device.Address;
                    watch.Client = new SunRizDriver.SunRizDriverClient(gateway.Address, gateway.Port.Value);
                    watch.DeviceId = point.DeviceId.Value;
                    watches.Add(watch);
                }
                watch.PointAddress.Add(point.Address);
            }

            if (watches.Count > 0)
            {
                RunningWatches[groupName] = watches.ToArray();
                Task.Run(() =>
                {
                    foreach (var watch in watches)
                    {
                        StartWatches(watch);
                    }
                });
            }
        }

        static void StartWatches(WatchGroup watch)
        {
            watch.Client.AddPointToWatch(watch.DeviceAddress, watch.PointAddress.ToArray(), (point, value) =>
            {
                Way.Lib.ScriptRemoting.RemotingController.SendGroupMessage(watch.MessageGroupName, Newtonsoft.Json.JsonConvert.SerializeObject(new { point = point , value = value }));
            }, (err) =>
            {
                Task.Run(() =>
                {
                    Thread.Sleep(2000);
                    //重新连接
                    StartWatches(watch);
                });

            });
        }
    }

    class WatchGroup
    {
        public int DeviceId;
        public string DeviceAddress;
        public List<string> PointAddress = new List<string>();
        public SunRizDriver.SunRizDriverClient Client;
        public string MessageGroupName;
    }
}
