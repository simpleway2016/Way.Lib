using System;
using System.Collections.Generic;
using System.Text;
using Way.Lib.ScriptRemoting;

namespace SunRizServer.Controllers
{
    public class BaseController : RemotingController
    {
        SysDB _db;
        public SysDB db
        {
            get
            {
                return _db ?? (_db = new SysDB());
            }
        }

        public UserInfo User
        {
            get
            {
                return Session["user"] as UserInfo;
            }
            set
            {
                Session["user"] = value;
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
