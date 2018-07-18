using System;
using System.Collections.Generic;
using System.Text;
using Way.EntityDB;
using System.Linq;
using System.IO;

namespace PandaAudioServer.ActionCaptures
{
    public class UserEffect_Capture : Way.EntityDB.ActionCapture<SysDB.UserEffect>
    {
        public override void BeforeDelete(object database, DatabaseModifyEventArg e)
        {
            base.AfterDelete(database, e);

            var data = (SysDB.UserEffect)e.DataItem;
            var db = (SysDB.DB.PandaAudio)database;
            data = db.UserEffect.FirstOrDefault(m => m.id == data.id);
            string filepath = $"{Way.Lib.ScriptRemoting.RemotingContext.Current.WebRoot}effects/{data.FileName}";
            if (System.IO.File.Exists(filepath))
            {
                File.Delete(filepath);
            }
        }
    }
}
