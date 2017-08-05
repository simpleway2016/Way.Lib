using System;
using System.Collections.Generic;
using System.Text;
using Way.Lib.ScriptRemoting;

namespace PandaAudioServer
{
    public class BaseController : RemotingController
    {
        PandaDB _db;
        public PandaDB db
        {
            get
            {
                return _db ?? (_db = new PandaDB());
            }
        }

        public virtual int UserId
        {
            get
            {
                if (this.Session["UserId"] == null)
                    throw new Exception("请先登录");
                return (int)this.Session["UserId"];
            }
        }

        protected override void OnUnLoad()
        {
            if (_db != null)
            {
                _db.Dispose();
                _db = null;
            }
            base.OnUnLoad();

        }
    }
}
