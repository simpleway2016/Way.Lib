using System;
using System.Collections.Generic;
using System.Text;
using Way.Lib.ScriptRemoting;
using System.Linq;
namespace SunRizServer.Controllers
{
    [RemotingUrl("Home")]
    public class StudioController : BaseController
    {
        [RemotingMethod]
        public int UpdateGateway(CommunicationDriver driver)
        {            
            if (driver.id == null &&  this.db.CommunicationDriver.Any(m => m.Address == driver.Address && m.Port == driver.Port))
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
            return this.db.Device.Where(m=>m.UnitId == unitid).OrderBy(m => m.Name).ToArray();
        }

        [RemotingMethod]
        public DevicePointFolder[] GetDevicePointFolders(int deviceid,SunRizServer.DevicePointFolder_TypeEnum type , int parentid)
        {
            return this.db.DevicePointFolder.Where(m => m.DeviceId == deviceid && m.Type == type && m.ParentId == parentid).OrderBy(m => m.Name).ToArray();
        }
        [RemotingMethod]
        public ControlWindowFolder[] GetControlWindowFolders(int controlUnitId,  int parentid)
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
        public string GetWindowContent(int windowid)
        {
            var window = this.db.ControlWindow.FirstOrDefault(m => m.id == windowid);
            var editorHtml = System.IO.File.ReadAllText(WebRoot + "editor.html", System.Text.Encoding.UTF8);
            var json = System.IO.File.ReadAllText(Way.Lib.PlatformHelper.GetAppDirectory() + "windows/" + window.FilePath, System.Text.Encoding.UTF8);
            var obj = (Newtonsoft.Json.Linq.JToken)Newtonsoft.Json.JsonConvert.DeserializeObject(json);
            editorHtml = editorHtml.Replace("//code here", obj.Value<string>("editorScript") + "\r\n" + obj.Value<string>("controlsScript"));
            return editorHtml;
        }

        [RemotingMethod]
        public ControlWindow SaveWindowContent(ControlWindow window , string content)
        {
            var obj = (Newtonsoft.Json.Linq.JToken)Newtonsoft.Json.JsonConvert.DeserializeObject(content);
            window.Name = obj.Value<string>("name");
            window.Code = obj.Value<string>("code");

            if (window.id == null && this.db.ControlWindow.Any(m => m.Name == window.Name && window.FolderId == m.FolderId && window.ControlUnitId == m.ControlUnitId))
                throw new Exception("监视画面名称已存在");
            else if (window.id == null && this.db.ControlWindow.Any(m => m.id != window.id && m.Name == window.Name && window.FolderId == m.FolderId && window.ControlUnitId == m.ControlUnitId))
                throw new Exception("监视画面名称已存在");

            if (window.id == null && this.db.ControlWindow.Any(m => m.Code == window.Code ))
                throw new Exception("监视画面编号已存在");
            else if (window.id == null && this.db.ControlWindow.Any(m => m.id != window.id && m.Code == window.Code))
                throw new Exception("监视画面编号已存在");

            if (window.FilePath == null)
            {
                window.FilePath = Guid.NewGuid().ToString("N");
            }
            this.db.Update(window);
           
            System.IO.File.WriteAllText(Way.Lib.PlatformHelper.GetAppDirectory() + "windows/" + window.FilePath,content, System.Text.Encoding.UTF8);
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
            if (folder.id == null && this.db.ControlWindowFolder.Any(m => m.ControlUnitId == folder.ControlUnitId && m.Name == folder.Name && m.ParentId == folder.ParentId ))
                throw new Exception("此文件夹已存在");
            else if (folder.id == null && this.db.ControlWindowFolder.Any(m => m.id != folder.id && m.ControlUnitId == folder.ControlUnitId && m.Name == folder.Name && m.ParentId == folder.ParentId))
                throw new Exception("此文件夹已存在");

            this.db.Update(folder);
            return folder.id.Value;
        }
        [RemotingMethod]
        public int DeleteDevicePointFolder(int folderid)
        {
            this.db.Delete(this.db.DevicePointFolder.Where(m=>m.id == folderid));
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
            if (point.id == null && this.db.DevicePointFolder.Any(m => m.Name == point.Name ))
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
        public object RegisterPointsAndGetGroupName(string[] pointNames,string groupName)
        {
            object[] objs = new object[pointNames.Length];
            List<DevicePoint> devPoints = new List<DevicePoint>();
            for(int i = 0; i < pointNames.Length; i ++)
            {
                var devPoint = this.db.DevicePoint.FirstOrDefault(m => m.Name == pointNames[i]);
                devPoints.Add(devPoint);

                objs[i] = new {
                    name = pointNames[i],
                    max = devPoint.TransMax,
                    min = devPoint.TransMin,
                };
            }
            DeviceListener.AddClient(groupName, this.db , devPoints.ToArray());
            return objs;
        }
    }
}
