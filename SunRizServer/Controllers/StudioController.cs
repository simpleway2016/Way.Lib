using System;
using System.Collections.Generic;
using System.Text;
using Way.Lib.ScriptRemoting;
using System.Linq;
namespace SunRizServer.Controllers
{
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
        public DevicePoint[] GetDevicePoints(int deviceid, SunRizServer.DevicePoint_TypeEnum type, int folderid)
        {
            return this.db.DevicePoint.Where(m => m.DeviceId == deviceid && m.Type == type && m.FolderId == folderid).OrderBy(m => m.Name).ToArray();
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
        public int DeleteDevicePointFolder(int folderid)
        {
            this.db.Delete(this.db.DevicePointFolder.Where(m=>m.id == folderid));
            return 0;
        }
    }
}
