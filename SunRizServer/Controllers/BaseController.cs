using System;
using System.Collections.Generic;
using System.Text;
using Way.Lib.ScriptRemoting;

namespace SunRizServer.Controllers
{
    public class BaseController : RemotingController
    {
        SunRizDB _db;
        public SunRizDB db
        {
            get
            {
                return _db ?? (_db = new SunRizDB());
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
