using System;
using System.Collections.Generic;
using System.Text;
using Way.EntityDB;
using System.Linq;
namespace SunRizServer.DBTriggers
{
    class ImageFilesTrigger : Way.EntityDB.ActionCapture<ImageFiles>
    {
        public override void BeforeDelete(object database, DatabaseModifyEventArg e)
        {
            var db = (SunRizServer.DB.SunRiz)database;
            var id = ((ImageFiles)e.DataItem).id;
            var data = db.ImageFiles.FirstOrDefault(m=>m.id == id) ;
            if(data.IsFolder == false)
            {
                try
                {
                    System.IO.File.Delete( Way.Lib.ScriptRemoting.RemotingController.WebRoot + "ImageFiles/" + data.FileName );
                }
                catch
                {

                }
            }
            else
            {
                db.Delete( db.ImageFiles.Where(m=>m.ParentId == id) );
            }
            base.BeforeDelete(database, e);
        }
    }
}
