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
                    watch.MessageGroupName = groupName;
                    watch.DeviceAddress = device.Address;
                    watch.Client = new SunRizDriver.SunRizDriverClient(gateway.Address, gateway.Port.Value);
                    watch.DeviceId = point.DeviceId.Value;
                    watches.Add(watch);
                }
                watch.PointAddress.Add(point.Address);
            }

            RunningWatches[groupName] = watches.ToArray();
        }

        public static void StartGroup(string groupName)
        {
            var watches = RunningWatches[groupName];
            Task.Run(() =>
            {
                foreach (var watch in watches)
                {
                    StartWatches(watch);
                }
            });
        }

        public static void StopGroup(string groupName)
        {
            //未实现
            WatchGroup[] watches;
            if (RunningWatches.TryRemove(groupName, out watches))
            {
                if (watches != null)
                {
                    foreach (var watch in watches)
                    {
                        watch.Release = true;
                        watch.NetClient.Close();
                    }
                }
            }
        }

        static void StartWatches(WatchGroup watch)
        {
            watch.NetClient = watch.Client.AddPointToWatch(watch.DeviceAddress, watch.PointAddress.ToArray(), (point, value) =>
            {
                if (watch.Release)
                    return;
                Way.Lib.ScriptRemoting.RemotingController.SendGroupMessage(watch.MessageGroupName, Newtonsoft.Json.JsonConvert.SerializeObject(new { addr = point , value = value }));
            }, (err) =>
            {
                if (watch.Release)
                    return;

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
        public bool Release = false;
        public string DeviceAddress;
        public Way.Lib.NetStream NetClient;
        public List<string> PointAddress = new List<string>();
        public SunRizDriver.SunRizDriverClient Client;
        public string MessageGroupName;
    }
}
