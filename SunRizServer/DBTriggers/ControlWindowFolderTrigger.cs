using System;
using System.Collections.Generic;
using System.Text;
using Way.EntityDB;
using System.Linq;

namespace SunRizServer.DBTriggers
{
    class ControlWindowFolderTrigger : Way.EntityDB.ActionCapture<ControlWindowFolder>
    {
        public override void BeforeDelete(object database, DatabaseModifyEventArg e)
        {
            ControlWindowFolder folder = (ControlWindowFolder)e.DataItem;
            var db = (SunRizServer.DB.SunRiz)database;
            //删除子文件夹
            db.Delete(db.ControlWindowFolder.Where(m=>m.ParentId == folder.id));
            db.Delete(db.ControlWindow.Where(m => m.FolderId == folder.id));

            base.BeforeDelete(database, e);
        }
    }
}
