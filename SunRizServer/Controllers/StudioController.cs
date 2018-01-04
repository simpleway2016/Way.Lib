using System;
using System.Collections.Generic;
using System.Text;
using Way.Lib.ScriptRemoting;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace SunRizServer.Controllers
{
    [RemotingUrl("Home")]
    public class StudioController : BaseController
    { 

        [RemotingMethod]
        public int UpdateGateway(CommunicationDriver driver)
        {
            if (driver.id == null && this.db.CommunicationDriver.Any(m => m.Address == driver.Address && m.Port == driver.Port))
                throw new Exception("此网关已存在");
            else if (this.db.CommunicationDriver.Any(m => m.id != driver.id && m.Address == driver.Address && m.Port == driver.Port))
                throw new Exception("此网关已存在");

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
            var device = this.db.Device.Include(m=>m.Driver).FirstOrDefault(m => m.id == deviceId);
            return device;
        }

        [RemotingMethod]
        public int UpdateControlUnit(ControlUnit unit)
        {
            if (unit.id == null && this.db.ControlUnit.Any(m => m.Name == unit.Name))
                throw new Exception("此控制单元已存在");
            else if (this.db.ControlUnit.Any(m => m.id != unit.id && m.Name == unit.Name))
                throw new Exception("此控制单元已存在");

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
        public string GetWindowContent(int windowid,string windowCode)
        {
            ControlWindow window = null;
            if(windowCode != null)
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
            this.db.Delete(this.db.ControlWindowFolder.Where(m=>m.id == folderid));
            return 0;
        }
        [RemotingMethod]
        public int DeleteControlWindow(int id)
        {
            this.db.Delete(this.db.ControlWindow.Where(m => m.id == id));
            return 0;
        }
        [RemotingMethod]
        public string GetWindowCode(int windowid, string windowCode)
        {
            ControlWindow window = null;
            if (!string.IsNullOrEmpty( windowCode))
                window = this.db.ControlWindow.FirstOrDefault(m => m.Code == windowCode);
            else
                window = this.db.ControlWindow.FirstOrDefault(m => m.id == windowid);
            var json = System.IO.File.ReadAllText(Way.Lib.PlatformHelper.GetAppDirectory() + "windows/" + window.FilePath, System.Text.Encoding.UTF8);
            return json;
        }
        [RemotingMethod]
        public int WriteWindowCode(int windowid, string windowCode,string filecontent)
        {
            ControlWindow window = null;
            if (!string.IsNullOrEmpty(windowCode))
                window = this.db.ControlWindow.FirstOrDefault(m => m.Code == windowCode);
            else
                window = this.db.ControlWindow.FirstOrDefault(m => m.id == windowid);
            System.IO.File.WriteAllText(Way.Lib.PlatformHelper.GetAppDirectory() + "windows/" + window.FilePath, filecontent, System.Text.Encoding.UTF8);
            return 0;
        }
        [RemotingMethod]
        public ControlWindow GetWindowInfo(int windowid, string windowCode)
        {
            ControlWindow window = null;
            if (windowCode != null)
                window = this.db.ControlWindow.Select(m=>new ControlWindow { windowWidth=m.windowWidth, windowHeight=m.windowHeight, Name=m.Name,id=m.id }).FirstOrDefault(m => m.Code == windowCode);
            else
                window = this.db.ControlWindow.Select(m => new ControlWindow { windowWidth = m.windowWidth, windowHeight = m.windowHeight, Name = m.Name, id = m.id }).FirstOrDefault(m => m.id == windowid);
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

        void checkChild(int windowId,int childid)
        {
            if (this.db.ChildWindow.Any(m => m.WindowId == childid && m.ChildWindowId == windowId))
            {
                string path = GetWindowPath(childid);
                throw new Exception("不能嵌套子画面" + path);
            }
            else
            {
                var myChildrens = this.db.ChildWindow.Where(m => m.WindowId == childid).Select(m => m.ChildWindowId).ToArray();
                foreach( var c in myChildrens )
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
            for(int i = 0; i < windowids.Length; i ++)
            {
                windowids[i] = db.ControlWindow.Where(m => m.Code == windowCodes[i].ToString()).Select(m => m.id).FirstOrDefault().Value;
            }
            if (window.id != null)
            {
                //检查循环嵌套
                foreach (var item in windowids) {
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

            this.db.Update(point);
            return point.id.Value;
        }

        [RemotingMethod]
        public string CreateGuid()
        {
            return Guid.NewGuid().ToString("N");
        }
        [RemotingMethod]
        public Dictionary<string,object> GetPointAddrDetail(string pointname)
        {
            Dictionary<string, object> result = new Dictionary<string, object>();
            var point = this.db.DevicePoint.Where(m => m.Name == pointname).Select(m=>new DevicePoint {
                Address = m.Address ,
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
                        max = devPoint.TransMax??unit.Max??systemSetting.Max,
                        min = devPoint.TransMin??unit.Min??systemSetting.Min,
                        addr = devPoint.Address,
                        deviceId = devPoint.DeviceId,
                        colorLine1 = unit.LineColor1?? systemSetting.LineColor1,
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
            if(data == null)
            {
                data = new SystemSetting();
                this.db.Insert(data);
            }
            return data;
        }
        [RemotingMethod]
        public int UpdateSystemSetting(SystemSetting data)
        {
            this.db.Update(data);
            return data.id.Value;
        }
    }
}
