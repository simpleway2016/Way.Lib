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
        static SunRizServer.DB.SunRizHistory hisDB;
        static DateTime LastHisTime;
        public static string HistoryDataPath;
        static HistoryAutoRec()
        {
            //检查历史记录剩余空间
            Task.Run(() => {
                while (true)
                {
                    
                    try
                    {
                        using (SysDB db = new SysDB())
                        {
                            var info = (from m in db.SystemSetting
                                              select new { path = m.HistoryPath,g = m.HistoryStoreAlarm }).FirstOrDefault();
                            if (info == null || string.IsNullOrEmpty(info.path) || info.path.Length < 2 || info.g == null || info.g == 0 || info.path[1] != ':')
                                continue;
                            System.IO.DriveInfo[] drives = System.IO.DriveInfo.GetDrives();
                            foreach (System.IO.DriveInfo drive in drives)
                            {
                                if (drive.Name.ToLower()[0] == info.path.ToLower()[0])
                                {
                                    var gb = Math.Round( drive.TotalFreeSpace / (double)(1024 * 1024 * 1024) , 2);
                                    if (gb <= info.g.GetValueOrDefault())
                                    {
                                        AlarmHelper.AddAlarm($"历史路径{info.path}剩余空间只有{gb}GB了，请尽快扩展磁盘！");
                                    }
                                    break;
                                }
                            }
                        }
                    }
                    catch(Exception ex)
                    {
                        using (Way.Lib.CLog log = new Way.Lib.CLog("HistoryAutoRec 检查磁盘剩余空间 error "))
                        {
                            log.Log(ex.ToString());
                        }

                    }
                    Thread.Sleep(1000 * 60);
                }
            });
        }

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
                if(hisDB != null)
                {
                    lock (hisDB)
                    {
                        hisDB.CommitTransaction();
                        hisDB.Dispose();
                    }
                    hisDB = null;
                }
                using (SysDB db = new SysDB())
                {
                    var sysSetting = db.SystemSetting.FirstOrDefault();
                    if (string.IsNullOrEmpty(sysSetting.HistoryPath))
                        return;
                    try
                    {
                        //目录不存在，创建目录
                        if(System.IO.Directory.Exists(sysSetting.HistoryPath) == false)
                        {
                            System.IO.Directory.CreateDirectory(sysSetting.HistoryPath);
                        }
                        HistoryDataPath = $"data source=\"{sysSetting.HistoryPath.Replace("\\", "/")}/data.db\"";
                        hisDB = new DB.SunRizHistory(HistoryDataPath, Way.EntityDB.DatabaseType.Sqlite);
                        LastHisTime = DateTime.Now;
                        hisDB.BeginTransaction();
                    }
                    catch
                    {
                        return;
                    }
                 

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
                            //利用InitValue保存上一次变化的值
                            points[i].InitValue = null;
                            pointAddrs[i] = points[i].Address;
                            if(points[i].ValueOnTimeChange == true)
                            {
                                client.SaveOnTimeInfos.Add(new SaveOnTimeInfo() {
                                    PointObj = points[i],
                                    PointId = points[i].id.Value,
                                    Interval = points[i].ValueOnTimeChangeSetting.GetValueOrDefault(),
                                });
                            }
                        }
                        watchClient(client, device, pointAddrs);
                        //启动定时保存的线程
                        saveValueOnTime_Thread(client, device);
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

        /// <summary>
        /// 写历史记录
        /// </summary>
        /// <param name="point"></param>
        /// <param name="value"></param>
        static void WriteHistory(DevicePoint point,double value)
        {
            if (hisDB == null)
                return;
            try
            {
                lock (hisDB)
                {
                    var data = new SunRizServer.History();
                    data.PointId = point.id;
                    data.Address = point.Name;
                    data.Time = DateTime.Now;
                    data.Value = value;
                    hisDB.Insert(data);

                    if ((DateTime.Now - LastHisTime).TotalMinutes >= 2)
                    {
                        hisDB.CommitTransaction();
                        LastHisTime = DateTime.Now;
                        hisDB.BeginTransaction();
                    }
                }
            }
            catch(Exception ex)
            {
                using (Way.Lib.CLog log = new Way.Lib.CLog("HistoryAutoRec WriteHistory error"))
                {
                    log.Log(ex.ToString());
                }
                hisDB.Dispose();
                hisDB = null;
                AlarmHelper.AddAlarm($"记录历史时，发生错误，错误信息：{ex.Message}");
            }
        }

        /// <summary>
        /// 定时保存值到历史记录
        /// </summary>
        /// <param name="client"></param>
        /// <param name="device"></param>
        static void saveValueOnTime_Thread(MyDriverClient client, Device device)
        {
            if (client.SaveOnTimeInfos.Count == 0)
                return;
            Task.Run(()=> {
                while(client.Released == false)
                {                    
                    foreach ( var itemInfo in client.SaveOnTimeInfos )
                    {
                        if(itemInfo.CurrentValue != itemInfo.SaveValue && (DateTime.Now - itemInfo.SaveTime).TotalSeconds >= itemInfo.PointObj.ValueOnTimeChangeSetting)
                        {
                            WriteHistory(itemInfo.PointObj, itemInfo.CurrentValue);
                            itemInfo.SaveValue = itemInfo.CurrentValue;
                            itemInfo.SaveTime = DateTime.Now;
                        }
                    }
                    Thread.Sleep(1000);
                }
            });
        }

        /// <summary>
        /// 实时监测设备值变化
        /// </summary>
        /// <param name="client"></param>
        /// <param name="device"></param>
        /// <param name="pointAddrs"></param>
        static void watchClient(MyDriverClient client , Device device , string[] pointAddrs)
        {
            client.NetClient = client.AddPointToWatch(device.Address, pointAddrs, (addr, value) => 
            {
                var point = client.Points.FirstOrDefault(m => m.Address == addr);
                if(point != null)
                {
                    var jobj = GetJsonObject(point);
                    double dblValue;
                    value = SunRizDriver.Helper.Transform(jobj, value);
                    try
                    {
                        dblValue = Convert.ToDouble(value);
                    }
                    catch
                    {
                        return;
                    }

                    if (point.Type == DevicePoint_TypeEnum.Digital)
                    {
                        //开关量，直接保存
                        WriteHistory(point, dblValue);
                    }
                    else if (point.ValueOnTimeChange == true)
                    {
                        //定时保存
                        var timeInfo = client.SaveOnTimeInfos.FirstOrDefault(m => m.PointId == point.id);
                        if (timeInfo != null)
                        {
                            timeInfo.CurrentValue = dblValue;
                        }
                    }
                    else if( point.ValueAbsoluteChange == true )
                    {
                        //绝对变化是指这个变量当前值与前一个历史值比较，变化超过设定数值后进行历史保存
                        //利用InitValue保存上一次变化的值
                        if (point.InitValue == null || Math.Abs(dblValue - point.InitValue.GetValueOrDefault()) >= point.ValueAbsoluteChangeSetting)
                        {
                            WriteHistory(point, dblValue);
                            point.InitValue = dblValue;
                        }
                    }
                    else if (point.ValueRelativeChange == true)
                    {
                        //相对变化是指这个变量当前值与前一个历史值比较，变化超过设定值（这个值是该变量量程的百分比）后进行历史保存
                        //利用InitValue保存上一次变化的值
                        if (point.InitValue == null || point.InitValue == 0 || Math.Abs(dblValue - point.InitValue.GetValueOrDefault())*100 /point.InitValue >= point.ValueRelativeChangeSetting)
                        {
                            WriteHistory(point, dblValue);
                            point.InitValue = dblValue;
                        }
                    }
                    //System.Diagnostics.Debug.WriteLine($"name:{addr} value:{value}");
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

    class SaveOnTimeInfo
    {
        public int PointId;
        /// <summary>
        /// 上次保存的值
        /// </summary>
        public double? SaveValue;
        /// <summary>
        /// 上次保存的时间
        /// </summary>
        public DateTime SaveTime = new DateTime(2000,1,1);
        public double CurrentValue;
        public DevicePoint PointObj;
        //保存间隔
        public double Interval;
    }

    class MyDriverClient : SunRizDriver.SunRizDriverClient
    {
        public bool Released = false;
        public Way.Lib.NetStream NetClient;
        public DevicePoint[] Points;
        public List<SaveOnTimeInfo> SaveOnTimeInfos = new List<SaveOnTimeInfo>();
        public MyDriverClient(string addr, int port) : base(addr, port)
        {

        }
    }
}
