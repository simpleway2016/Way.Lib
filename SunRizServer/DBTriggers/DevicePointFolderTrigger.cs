using System;
using System.Collections.Generic;
using System.Text;
using Way.EntityDB;
using System.Linq;

namespace SunRizServer.DBTriggers
{
    class DevicePointFolderTrigger : Way.EntityDB.ActionCapture<DevicePointFolder>
    {
        public override void BeforeDelete(object database, DatabaseModifyEventArg e)
        {
            DevicePointFolder folder = (DevicePointFolder)e.DataItem;
            var db = (SunRizServer.DB.SunRiz)database;
            //删除子文件夹
            db.Delete(db.DevicePointFolder.Where(m=>m.ParentId == folder.id));

            base.BeforeDelete(database, e);
        }
    }
}
