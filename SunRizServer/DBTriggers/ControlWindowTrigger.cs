using System;
using System.Collections.Generic;
using System.Text;
using Way.EntityDB;
using System.Linq;
using System.IO;

namespace SunRizServer.DBTriggers
{
    class ControlWindowTrigger : Way.EntityDB.ActionCapture<ControlWindow>
    {
        public override void BeforeDelete(object database, DatabaseModifyEventArg e)
        {
            var db = (SunRizServer.DB.SunRiz)database;
            var id = ((ControlWindow)e.DataItem).id;
            var data = db.ControlWindow.FirstOrDefault(m => m.id == id);
            try
            {
                File.Delete($"{Way.Lib.PlatformHelper.GetAppDirectory()}windows/{data.FilePath}");
            }
            catch
            {

            }
            base.BeforeDelete(database, e);
        }
    }
}
