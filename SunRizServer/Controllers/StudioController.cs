using System;
using System.Collections.Generic;
using System.Text;
using Way.Lib.ScriptRemoting;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;
using System.Reflection;

namespace SunRizServer.Controllers
{
    [RemotingUrl("Home")]
    public class StudioController : BaseController
    {
        DB.SunRiz _hisDB;
        DB.SunRiz _sysLogDB;
        public IQueryable<Alarm> Alarms
        {
            get
            {
                int userGroups = 0;
                if (this.User != null)
                    userGroups = this.User.AlarmGroups.GetValueOrDefault();

                return from m in db.Alarm
                       where m.AlarmGroup == null || (userGroups & m.AlarmGroup.Value) == m.AlarmGroup.Value
                       orderby m.IsConfirm, m.AlarmTime descending
                       select m;
            }
        }

        public IQueryable<MyHistory> Histories
        {
            get
            {
                if (_hisDB == null)
                    _hisDB = new DB.SunRiz(HistoryRecord.HistoryAutoRec.HistoryDataPath, Way.EntityDB.DatabaseType.Sqlite);
                return from m in _hisDB.History
                       orderby m.Time descending
                       select new MyHistory {
                           Address = m.Address,
                           id = m.id,
                           PointId = m.PointId,
                           Time = m.Time,
                           Value = m.Value
                       };
            }
        }

        public IQueryable<SysLog> SysLogs
        {
            get
            {
                if (_sysLogDB == null)
                    _sysLogDB = new DB.SunRiz(SystemLog.LogDataPath, Way.EntityDB.DatabaseType.Sqlite);
                return from m in _sysLogDB.SysLog
                       orderby m.Time descending
                       select m;
            }
        }
        protected override void OnUnLoad()
        {
            if (_hisDB != null)
            {
                _hisDB.Dispose();
                _hisDB = null;
            }
            base.OnUnLoad();

        }
        protected override void OnBeforeInvokeMethod(MethodInfo method)
        {
            if(method.Name != "Login")
            {
                if (this.User == null)
                    throw new Exception("请重新登录");
            }
            base.OnBeforeInvokeMethod(method);
        }

        [RemotingMethod]
        public int UpdateGateway(CommunicationDriver driver)
        {
            if (driver.id == null && this.db.CommunicationDriver.Any(m => m.Address == driver.Address && m.Port == driver.Port))
                throw new Exception("此网关已存在");
            else if (this.db.CommunicationDriver.Any(m => m.id != driver.id && m.Address == driver.Address && m.Port == driver.Port))
                throw new Exception("此网关已存在");

            SystemHelper.AddSysLog(this.User.id.Value, "更新网关信息：" + Newtonsoft.Json.JsonConvert.SerializeObject(driver.ChangedProperties));

            this.db.Update(driver);           

            return driver.id.Value;
        }

        [RemotingMethod]
        public CommunicationDriver[] GetGatewayList()
        {
            return this.db.CommunicationDriver.ToArray();
        }

        [RemotingMethod]
        public Device GetDeviceAndDriver(int deviceId)
        {
            //必须using Microsoft.EntityFrameworkCore;才有include
            var device = this.db.Device.Include(m => m.Driver).FirstOrDefault(m => m.id == deviceId);
            return device;
        }

        [RemotingMethod]
        public int UpdateControlUnit(ControlUnit unit)
        {
            if (unit.id == null && this.db.ControlUnit.Any(m => m.Name == unit.Name))
                throw new Exception("此控制单元已存在");
            else if (this.db.ControlUnit.Any(m => m.id != unit.id && m.Name == unit.Name))
                throw new Exception("此控制单元已存在");

            SystemHelper.AddSysLog(this.User.id.Value, "更新控制单元信息：" + Newtonsoft.Json.JsonConvert.SerializeObject(unit.ChangedProperties));

            this.db.Update(unit);
            return unit.id.Value;
        }

        [RemotingMethod]
        public ControlUnit[] GetUnitList()
        {
            return this.db.ControlUnit.OrderBy(m => m.Name).ToArray();
        }

        [RemotingMethod]
        public int UpdateDevice(Device device)
        {
            if (device.id == null && this.db.Device.Any(m => m.Name == device.Name && device.UnitId == m.UnitId))
                throw new Exception("此控制器已存在");
            else if (this.db.Device.Any(m => m.id != device.id && m.Name == device.Name && device.UnitId == m.UnitId))
                throw new Exception("此控制器已存在");

            SystemHelper.AddSysLog(this.User.id.Value, "更新控制器信息：" + Newtonsoft.Json.JsonConvert.SerializeObject(device.ChangedProperties));

            this.db.Update(device);
            return device.id.Value;
        }

        [RemotingMethod]
        public Device[] GetDeviceList(int unitid)
        {
            return this.db.Device.Where(m => m.UnitId == unitid).OrderBy(m => m.Name).ToArray();
        }

        [RemotingMethod]
        public DevicePointFolder[] GetDevicePointFolders(int deviceid, SunRizServer.DevicePointFolder_TypeEnum type, int parentid)
        {
            return this.db.DevicePointFolder.Where(m => m.DeviceId == deviceid && m.Type == type && m.ParentId == parentid).OrderBy(m => m.Name).ToArray();
        }
        [RemotingMethod]
        public ControlWindowFolder[] GetControlWindowFolders(int controlUnitId, int parentid)
        {
            return this.db.ControlWindowFolder.Where(m => m.ControlUnitId == controlUnitId && m.ParentId == parentid).OrderBy(m => m.Name).ToArray();
        }
        [RemotingMethod]
        public DevicePoint[] GetDevicePoints(int deviceid, SunRizServer.DevicePoint_TypeEnum type, int folderid)
        {
            return this.db.DevicePoint.Where(m => m.DeviceId == deviceid && m.Type == type && m.FolderId == folderid).OrderBy(m => m.Name).ToArray();
        }
        [RemotingMethod]
        public ControlWindow[] GetWindows(int controlUnitId, int folderId)
        {
            return this.db.ControlWindow.Where(m => m.ControlUnitId == controlUnitId && m.FolderId == folderId).OrderBy(m => m.Name).ToArray();
        }
        [RemotingMethod]
        public string GetWindowContent(int windowid, string windowCode)
        {
            ControlWindow window = null;
            if (windowCode != null)
                window = this.db.ControlWindow.FirstOrDefault(m => m.Code == windowCode);
            else
                window = this.db.ControlWindow.FirstOrDefault(m => m.id == windowid);

            var editorHtml = System.IO.File.ReadAllText(WebRoot + "editor.html", System.Text.Encoding.UTF8);
            var json = System.IO.File.ReadAllText(Way.Lib.PlatformHelper.GetAppDirectory() + "windows/" + window.FilePath, System.Text.Encoding.UTF8);
            var obj = (Newtonsoft.Json.Linq.JToken)Newtonsoft.Json.JsonConvert.DeserializeObject(json);
            editorHtml = editorHtml.Replace("//code here", obj.Value<string>("editorScript") + "\r\n" + obj.Value<string>("controlsScript"));
            return editorHtml;
        }

        [RemotingMethod]
        public int DeleteControlWindowFolder(int folderid)
        {
            var folder = this.db.ControlWindowFolder.FirstOrDefault(m => m.id == folderid);
            if (folder != null)
            {
                SystemHelper.AddSysLog(this.User.id.Value, "删除文件夹：" + Newtonsoft.Json.JsonConvert.SerializeObject( folder));

                this.db.Delete(this.db.ControlWindowFolder.Where(m => m.id == folderid));
            }
            return 0;
        }
        [RemotingMethod]
        public int DeleteControlWindow(int id)
        {
            var window = this.db.ControlWindow.FirstOrDefault(m => m.id == id);
            if (window != null)
            {
                SystemHelper.AddSysLog(this.User.id.Value, "删除监控窗口：" + window.Name);

                this.db.Delete(this.db.ControlWindow.Where(m => m.id == id));
            }
            return 0;
        }
        [RemotingMethod]
        public string GetWindowCode(int windowid, string windowCode)
        {
            ControlWindow window = null;
            if (!string.IsNullOrEmpty(windowCode))
                window = this.db.ControlWindow.FirstOrDefault(m => m.Code == windowCode);
            else
                window = this.db.ControlWindow.FirstOrDefault(m => m.id == windowid);
            var json = System.IO.File.ReadAllText(Way.Lib.PlatformHelper.GetAppDirectory() + "windows/" + window.FilePath, System.Text.Encoding.UTF8);
            return json;
        }
        [RemotingMethod]
        public int WriteWindowCode(int windowid, string windowCode, string filecontent)
        {
            ControlWindow window = null;
            if (!string.IsNullOrEmpty(windowCode))
                window = this.db.ControlWindow.FirstOrDefault(m => m.Code == windowCode);
            else
                window = this.db.ControlWindow.FirstOrDefault(m => m.id == windowid);

            SystemHelper.AddSysLog(this.User.id.Value, "修改监控窗口：" + window.Name);

            System.IO.File.WriteAllText(Way.Lib.PlatformHelper.GetAppDirectory() + "windows/" + window.FilePath, filecontent, System.Text.Encoding.UTF8);
            return 0;
        }

        [RemotingMethod]
        public string GetWindowScript(int windowid)
        {
            ControlWindow window = this.db.ControlWindow.FirstOrDefault(m => m.id == windowid);
            var json = System.IO.File.ReadAllText(Way.Lib.PlatformHelper.GetAppDirectory() + "windows/" + window.FilePath, System.Text.Encoding.UTF8);
            var obj = (Newtonsoft.Json.Linq.JToken)Newtonsoft.Json.JsonConvert.DeserializeObject(json);
            return obj.Value<string>("controlsScript");
        }

        /// <summary>
        /// 在窗口中查找引用的点
        /// </summary>
        /// <param name="pointName"></param>
        /// <returns></returns>
        [RemotingMethod]
        public List<object> FindDevicePointInWindow(string pointName)
        {
            List<object> findedWindows = new List<object>();
            foreach (var window in this.db.ControlWindow)
            {
                var json = System.IO.File.ReadAllText(Way.Lib.PlatformHelper.GetAppDirectory() + "windows/" + window.FilePath, System.Text.Encoding.UTF8);
                var obj = (Newtonsoft.Json.Linq.JToken)Newtonsoft.Json.JsonConvert.DeserializeObject(json);
                string script = obj.Value<string>("controlsScript");
                //var ms = Regex.Matches(script, @"devicePoint[0-9]*[ ]*=[ ]*['""]([\w\/]+)['""]");
                //因为GroupControl会自定义变量，所以不能匹配devicePoint字眼
                var ms = Regex.Matches(script, @"=[ ]*['""]([\w\/]+)['""]");
                foreach (Match m in ms)
                {
                    var name = m.Groups[1].Value;
                    if (CompareString(name, pointName))
                    {
                        var windowPath = GetWindowPath(window.id.Value);
                        findedWindows.Add(new
                        {
                            id = window.id.Value,
                            path = windowPath,
                            content = name
                        });
                    }
                }
            }
            return findedWindows;
        }

        /// <summary>
        /// 比较字符串
        /// </summary>
        /// <param name="content"></param>
        /// <param name="str">匹配的字符串，支持通配符*</param>
        /// <returns>如果content包含str，返回true</returns>
        bool CompareString(string content, string str)
        {
            if (str.Contains("*") == false)
            {
                return string.Equals(content, str, StringComparison.CurrentCultureIgnoreCase);
            }
            else
            {
                return Regex.IsMatch(content, str.Replace("*", @"[\w\/]+"));
            }
        }

        [RemotingMethod]
        public int WriteWindowScript(int windowid, string script)
        {
            ControlWindow window = this.db.ControlWindow.FirstOrDefault(m => m.id == windowid);
            var json = System.IO.File.ReadAllText(Way.Lib.PlatformHelper.GetAppDirectory() + "windows/" + window.FilePath, System.Text.Encoding.UTF8);
            var obj = (Newtonsoft.Json.Linq.JToken)Newtonsoft.Json.JsonConvert.DeserializeObject(json);
            obj["controlsScript"] = script;
            System.IO.File.WriteAllText(Way.Lib.PlatformHelper.GetAppDirectory() + "windows/" + window.FilePath, obj.ToString(), System.Text.Encoding.UTF8);

            SystemHelper.AddSysLog(this.User.id.Value, "改写监控窗口的脚本：" + window.Name);

            return 0;
        }
        [RemotingMethod]
        public ControlWindow GetWindowInfo(int windowid, string windowCode)
        {
            ControlWindow window = null;
            if (windowCode != null)
                window = this.db.ControlWindow.Where(m => m.Code == windowCode).Select(m => new ControlWindow { windowWidth = m.windowWidth, windowHeight = m.windowHeight, Name = m.Name, id = m.id }).FirstOrDefault();
            else
                window = this.db.ControlWindow.Where(m => m.id == windowid).Select(m => new ControlWindow { windowWidth = m.windowWidth, windowHeight = m.windowHeight, Name = m.Name, id = m.id }).FirstOrDefault();
            return window;
        }
        [RemotingMethod]
        public string GetWindowPath(int windowid)
        {
            var window = this.db.ControlWindow.FirstOrDefault(m => m.id == windowid);

            StringBuilder path = new StringBuilder();
            path.Append(window.Name);
            var folder = this.db.ControlWindowFolder.FirstOrDefault(m => m.id == window.FolderId);
            while (folder != null)
            {
                path.Insert(0, folder.Name + "/");
                folder = this.db.ControlWindowFolder.FirstOrDefault(m => m.id == folder.ParentId);
            }
            var unit = this.db.ControlUnit.FirstOrDefault(m => m.id == window.ControlUnitId);
            if (unit != null)
            {
                path.Insert(0, unit.Name + "/");
            }
            return path.ToString();
        }

        void checkChild(int windowId, int childid)
        {
            if (this.db.ChildWindow.Any(m => m.WindowId == childid && m.ChildWindowId == windowId))
            {
                string path = GetWindowPath(childid);
                throw new Exception("不能嵌套子画面" + path);
            }
            else
            {
                var myChildrens = this.db.ChildWindow.Where(m => m.WindowId == childid).Select(m => m.ChildWindowId).ToArray();
                foreach (var c in myChildrens)
                {
                    checkChild(windowId, c.Value);
                }
            }
        }

        [RemotingMethod]
        public ControlWindow SaveWindowContent(ControlWindow window, string content)
        {
            var obj = (Newtonsoft.Json.Linq.JToken)Newtonsoft.Json.JsonConvert.DeserializeObject(content);
            window.Name = obj.Value<string>("name");
            window.Code = obj.Value<string>("code");
            try
            {
                window.windowWidth = obj.Value<int?>("windowWidth");
            }
            catch
            {
                throw new Exception("窗口宽度格式错误，请输入整数！");
            }
            try
            {
                window.windowHeight = obj.Value<int?>("windowHeight");
            }
            catch
            {
                throw new Exception("窗口高度格式错误，请输入整数！");
            }
            var windowCodes = obj.Value<Newtonsoft.Json.Linq.JArray>("windowCodes");
            var windowids = new int[windowCodes.Count];
            for (int i = 0; i < windowids.Length; i++)
            {
                windowids[i] = db.ControlWindow.Where(m => m.Code == windowCodes[i].ToString()).Select(m => m.id).FirstOrDefault().Value;
            }
            if (window.id != null)
            {
                //检查循环嵌套
                foreach (var item in windowids)
                {
                    checkChild(window.id.Value, item);
                }
            }
            var customProperties = obj.Value<string>("customProperties");

            if (window.id == null && this.db.ControlWindow.Any(m => m.Name == window.Name && window.FolderId == m.FolderId && window.ControlUnitId == m.ControlUnitId))
                throw new Exception("监视画面名称已存在");
            else if (window.id == null && this.db.ControlWindow.Any(m => m.id != window.id && m.Name == window.Name && window.FolderId == m.FolderId && window.ControlUnitId == m.ControlUnitId))
                throw new Exception("监视画面名称已存在");

            if (window.id == null && this.db.ControlWindow.Any(m => m.Code == window.Code))
                throw new Exception("监视画面编号已存在");
            else if (window.id == null && this.db.ControlWindow.Any(m => m.id != window.id && m.Code == window.Code))
                throw new Exception("监视画面编号已存在");

            if (window.FilePath == null)
            {
                window.FilePath = Guid.NewGuid().ToString("N");
            }

            SystemHelper.AddSysLog(this.User.id.Value, "修改监控窗口：" + window.Name);

            this.db.Update(window);

            System.IO.File.WriteAllText(Way.Lib.PlatformHelper.GetAppDirectory() + "windows/" + window.FilePath, content, System.Text.Encoding.UTF8);
            return window;
        }

        [RemotingMethod]
        public int UpdateDevicePointFolder(DevicePointFolder folder)
        {
            if (folder.id == null && this.db.DevicePointFolder.Any(m => m.DeviceId == folder.DeviceId && m.Name == folder.Name && m.ParentId == folder.ParentId && m.Type == folder.Type))
                throw new Exception("此文件夹已存在");
            else if (folder.id == null && this.db.DevicePointFolder.Any(m => m.id != folder.id && m.DeviceId == folder.DeviceId && m.Name == folder.Name && m.ParentId == folder.ParentId && m.Type == folder.Type))
                throw new Exception("此文件夹已存在");

            this.db.Update(folder);
            return folder.id.Value;
        }
        [RemotingMethod]
        public int UpdateControlWindowFolder(ControlWindowFolder folder)
        {
            if (folder.id == null && this.db.ControlWindowFolder.Any(m => m.ControlUnitId == folder.ControlUnitId && m.Name == folder.Name && m.ParentId == folder.ParentId))
                throw new Exception("此文件夹已存在");
            else if (folder.id == null && this.db.ControlWindowFolder.Any(m => m.id != folder.id && m.ControlUnitId == folder.ControlUnitId && m.Name == folder.Name && m.ParentId == folder.ParentId))
                throw new Exception("此文件夹已存在");

            this.db.Update(folder);
            return folder.id.Value;
        }
        [RemotingMethod]
        public int DeleteDevicePointFolder(int folderid)
        {
            this.db.Delete(this.db.DevicePointFolder.Where(m => m.id == folderid));
            return 0;
        }

        [RemotingMethod]
        public CommunicationDriver GetDriver(int driverid)
        {
            return this.db.CommunicationDriver.FirstOrDefault(m => m.id == driverid);
        }

        [RemotingMethod]
        public int UpdatePoint(DevicePoint point)
        {
            if (point.id == null && this.db.DevicePointFolder.Any(m => m.Name == point.Name))
                throw new Exception("点名已存在");
            else if (point.id == null && this.db.DevicePointFolder.Any(m => m.id != point.id && m.Name == point.Name))
                throw new Exception("点名已存在");

            bool needRestartHistory = false;
            if (point.ChangedProperties.Any(m => m.Key.Contains("ValueOnTimeChange") || m.Key.Contains("ValueAbsoluteChange") || m.Key.Contains("ValueRelativeChange") || m.Key.Contains("IsAlarm")))
            {
                //需要重新启动历史保存
                needRestartHistory = true;
            }

            if (point.Type == DevicePoint_TypeEnum.Analog)
            {
                if (point.ValueAbsoluteChangeSetting < point.TransMin || point.ValueAbsoluteChangeSetting > point.TransMax)
                {
                    throw new Exception($"历史数据变化定义中，绝对值变化定义超出量程范围，量程范围目前为{point.TransMin } - {point.TransMax}");
                }
            }
            SystemHelper.AddSysLog(this.User.id.Value, "更新点“"+ point.Name +"”信息：" + Newtonsoft.Json.JsonConvert.SerializeObject(point.ChangedProperties));

            this.db.Update(point);
            if (needRestartHistory)
            {
                HistoryRecord.HistoryAutoRec.ReStart();
            }
            return point.id.Value;
        }

        [RemotingMethod]
        public string CreateGuid()
        {
            return Guid.NewGuid().ToString("N");
        }
        [RemotingMethod]
        public Dictionary<string, object> GetPointAddrDetail(string pointname)
        {
            Dictionary<string, object> result = new Dictionary<string, object>();
            var point = this.db.DevicePoint.Where(m => m.Name == pointname).Select(m => new DevicePoint
            {
                Address = m.Address,
                DeviceId = m.DeviceId
            }).FirstOrDefault();

            result["addr"] = point.Address;
            result["deviceId"] = point.DeviceId;
            return result;
        }
        [RemotingMethod]
        public object GetPointDetails(string[] pointNames)
        {
            var systemSetting = this.db.SystemSetting.FirstOrDefault();
            if (systemSetting == null)
                systemSetting = new SystemSetting();

            object[] objs = new object[pointNames.Length];
            for (int i = 0; i < pointNames.Length; i++)
            {
                var devPoint = this.db.DevicePoint.FirstOrDefault(m => m.Name == pointNames[i]);
                if (devPoint == null)
                {
                    objs[i] = new
                    {
                        name = "null_can_not_find",
                        max = 100,
                        min = 0,
                        addr = "",
                        deviceId = 0,
                    };
                }
                else
                {
                    //AsTracking防止重复从数据库取值
                    var device = this.db.Device.AsTracking().FirstOrDefault(m => m.id == devPoint.DeviceId);
                    var unit = this.db.ControlUnit.AsTracking().FirstOrDefault(m => m.id == device.UnitId);
                    var detail = new Dictionary<string, object>();
                    detail["IsSquare"] = devPoint.IsSquare;
                    detail["IsTransform"] = devPoint.IsTransform;
                    detail["IsLinear"] = devPoint.IsLinear;
                    detail["DPCount"] = devPoint.DPCount;
                    if (devPoint.IsTransform == true)
                    {
                        detail["SensorMax"] = devPoint.SensorMax;
                        detail["SensorMin"] = devPoint.SensorMin;
                    }
                    if (devPoint.IsLinear == true)
                    {
                        detail["LinearX1"] = devPoint.LinearX1;
                        detail["LinearX2"] = devPoint.LinearX2;
                        detail["LinearX3"] = devPoint.LinearX3;
                        detail["LinearX4"] = devPoint.LinearX4;
                        detail["LinearX5"] = devPoint.LinearX5;
                        detail["LinearX6"] = devPoint.LinearX6;
                        detail["LinearY1"] = devPoint.LinearY1;
                        detail["LinearY2"] = devPoint.LinearY2;
                        detail["LinearY3"] = devPoint.LinearY3;
                        detail["LinearY4"] = devPoint.LinearY4;
                        detail["LinearY5"] = devPoint.LinearY5;
                        detail["LinearY6"] = devPoint.LinearY6;
                    }
                    objs[i] = new
                    {
                        name = pointNames[i],
                        isDigital = devPoint.Type == DevicePoint_TypeEnum.Digital,
                        max = devPoint.Type == DevicePoint_TypeEnum.Digital ? 1 : (devPoint.TransMax ?? unit.Max ?? systemSetting.Max),
                        min = devPoint.Type == DevicePoint_TypeEnum.Digital ? 0 : (devPoint.TransMin ?? unit.Min ?? systemSetting.Min),
                        addr = devPoint.Address,
                        deviceId = devPoint.DeviceId,
                        colorLine1 = unit.LineColor1 ?? systemSetting.LineColor1,
                        colorLine2 = unit.LineColor2 ?? systemSetting.LineColor2,
                        colorLine3 = unit.LineColor3 ?? systemSetting.LineColor3,
                        colorLine4 = unit.LineColor4 ?? systemSetting.LineColor4,
                        colorLine5 = unit.LineColor5 ?? systemSetting.LineColor5,
                        colorLine6 = unit.LineColor6 ?? systemSetting.LineColor6,
                        colorLine7 = unit.LineColor7 ?? systemSetting.LineColor7,
                        colorLine8 = unit.LineColor8 ?? systemSetting.LineColor8,
                        colorLine9 = unit.LineColor9 ?? systemSetting.LineColor9,
                        colorLine10 = unit.LineColor10 ?? systemSetting.LineColor10,
                        colorLine11 = unit.LineColor11 ?? systemSetting.LineColor11,
                        colorLine12 = unit.LineColor12 ?? systemSetting.LineColor12,
                        detail = detail
                    };
                }
            }
            return objs;
        }

        [RemotingMethod]
        public SystemSetting GetSystemSetting()
        {
            var data = this.db.SystemSetting.FirstOrDefault();
            if (data == null)
            {
                data = new SystemSetting();
                this.db.Insert(data);
            }
            return data;
        }
        [RemotingMethod]
        public int UpdateSystemSetting(SystemSetting data)
        {
            var needRestartHistory = false;
            if (data.ChangedProperties.Any(m => m.Key == "HistoryPath"))
            {
                //历史保存路径修改了
                needRestartHistory = true;

                //这里确保能创建数据库
                if (!string.IsNullOrEmpty(data.HistoryPath))
                {
                    var HistoryDataPath = $"data source=\"{data.HistoryPath.Replace("\\", "/")}/history_data.db\"";
                    using (var hisDB = new DB.SunRiz(HistoryDataPath, Way.EntityDB.DatabaseType.Sqlite))
                    {

                    }
                }
            }

            if (data.ChangedProperties.Any(m => m.Key == "LogPath"))
            {
                SystemLog.Init();
            }
            SystemHelper.AddSysLog(this.User.id.Value, "更新系统设置：" + Newtonsoft.Json.JsonConvert.SerializeObject(data.ChangedProperties));

            this.db.Update(data);
            if (needRestartHistory)
            {
                HistoryRecord.HistoryAutoRec.ReStart();
            }
            return data.id.Value;
        }

        [RemotingMethod]
        public object SearchHistory(string[] pointNames, DateTime startTime, DateTime endTime)
        {
            using (var hisDB = new DB.SunRiz(HistoryRecord.HistoryAutoRec.HistoryDataPath, Way.EntityDB.DatabaseType.Sqlite))
            {
                List<object> result = new List<object>();
                foreach (string pointName in pointNames)
                {
                    var data = (from m in hisDB.History
                                where m.Time >= startTime && m.Time <= endTime && m.Address == pointName
                                orderby m.Time
                                select m).ToArray();
                    var resultData = new object[data.Length];
                    for (int i = 0; i < resultData.Length; i++)
                    {
                        resultData[i] = new
                        {
                            seconds = Helper.ConvertDateTimeInt(data[i].Time.GetValueOrDefault()),
                            value = data[i].Value.GetValueOrDefault()
                        };
                    }
                    result.Add(resultData);
                }
                return result;
            }
        }

        /// <summary>
        /// get last alarm item time
        /// </summary>
        /// <param name="myLastAlarmId">clien last alarm raise time</param>
        /// <returns></returns>
        [RemotingMethod]
        public DateTime? GetLastAlarmTime(DateTime? myLastAlarmTime)
        {
            return (from m in db.Alarm where myLastAlarmTime == null || m.AlarmTime > myLastAlarmTime select m.AlarmTime).FirstOrDefault();
        }

        [RemotingMethod]
        public void ConfirmAlarm(int id)
        {
            var alarm = db.Alarm.FirstOrDefault(m => m.id == id);
            alarm.IsConfirm = true;
            alarm.ConfirmTime = DateTime.Now;
            if (this.User != null)
                alarm.ConfirmUserId = this.User.id;
            db.Update(alarm);
        }

        [RemotingMethod]
        public void ResetAlarm(int id)
        {
            var alarm = db.Alarm.FirstOrDefault(m => m.id == id);
            alarm.IsReset = true;
            alarm.ResetTime = DateTime.Now;
            db.Update(alarm);
        }

        [RemotingMethod]
        public UserInfo[] GetUserInfos()
        {
            if (this.User == null || this.User.Role != UserInfo_RoleEnum.Admin)
                throw new Exception("权限不足");

            var result = db.UserInfo.ToArray();
            foreach (var item in result)
            {
                item.Password = null;
                item.ChangedProperties.Clear();
            }
            return result;
        }
        [RemotingMethod]
        public UserInfo[] GetUserList()
        {
            return (from m in db.UserInfo
                    select new UserInfo
                    {
                        id = m.id,
                        Name = m.Name
                    }).ToArray();
        }
        [RemotingMethod]
        public int SaveUserInfo(UserInfo user)
        {
#if DEBUG
#else
            if (this.User == null || this.User.Role != UserInfo_RoleEnum.Admin)
                throw new Exception("权限不足");
#endif
            SystemHelper.AddSysLog(this.User.id.Value, "修改用户" + user.Name + "信息");
            
            db.Update(user);
            return user.id.Value;
        }

        [RemotingMethod]
        public void DeleteUserInfo(UserInfo user)
        {
#if DEBUG
#else
            if (this.User == null || this.User.Role != UserInfo_RoleEnum.Admin)
                throw new Exception("权限不足");
#endif
            SystemHelper.AddSysLog(this.User.id.Value, "删除用户" + user.Name);

            db.Delete(user);
        }

        [RemotingMethod]
        public UserInfo Login(string name,string pwd)
        {
            var user = this.db.UserInfo.FirstOrDefault(m => m.Name == name);
            if (user == null)
                throw new Exception("用户不存在");
            if(user.Password != pwd)
                throw new Exception("密码错误");
            this.User = user;
            user.Password = null;
            user.ChangedProperties.Clear();

            SystemHelper.AddSysLog(this.User.id.Value, "登录系统");
            return user;
        }

        [RemotingMethod]
        public int ModifyPassword(string old, string pwd)
        {
            var user = this.db.UserInfo.FirstOrDefault(m => m.id == this.User.id);
            if (user.Password != old)
                throw new Exception("旧密码不正确");
            user.Password = pwd;
            db.Update(user);
            this.User = user;
            SystemHelper.AddSysLog(this.User.id.Value, "修改密码");

            return user.id.Value;
        }

        [RemotingMethod]
        public int SetStartupWindow(int windowId)
        {
            var window = db.ControlWindow.FirstOrDefault(m => m.IsStartup == true);
            if(window != null)
            {
                window.IsStartup = false;
                db.Update(window);
            }

            window = db.ControlWindow.FirstOrDefault(m => m.id == windowId);
            window.IsStartup = true;

            SystemHelper.AddSysLog(this.User.id.Value, "设置启动窗口为：" + window.Name);

            db.Update(window);

            return 0;
        }

        [RemotingMethod]
        public ControlWindow GetStartupWindow()
        {
            return db.ControlWindow.FirstOrDefault(m => m.IsStartup == true);
        }

        [RemotingMethod]
        public object GetHistories(int skip,int take,string searchModel,string orderBy)
        {
            IEnumerable<MyHistory> histories = (IEnumerable<MyHistory>)this.LoadData("Histories", skip, take, searchModel,orderBy);
            foreach( var item in histories)
            {
                item.AddressDesc = db.DevicePoint.Where(m => m.id == item.PointId).Select(m => m.Desc).FirstOrDefault();
            }
            return histories;
        }

        [RemotingMethod]
        public object GetSysLogs(int skip, int take, string searchModel, string orderBy)
        {
            if (string.IsNullOrEmpty(SystemLog.LogDataPath))
                throw new Exception("系统日志存放路径未配置");

                var result = (IEnumerable<SysLog>) this.LoadData("SysLogs", skip, take, searchModel, orderBy);
            foreach (var item in result)
            {
                item.UserName = db.UserInfo.Where(m => m.id == item.UserId).Select(m => m.Name).FirstOrDefault();
            }
            return result;
        }
    }

    public class MyHistory:History
    {
        public string AddressDesc
        {
            get;
            set;
        }
    }
}
