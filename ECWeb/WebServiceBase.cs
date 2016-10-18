using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ECWeb
{
    public class WebServiceBase : System.Web.Services.WebService
    {

        public EJ.User User
        {
            get
            {
                //if (Session["user"] == null)
                //{
                //    using (EJDB db = new EJDB())
                //    {
                //        Session["user"] = db.User.FirstOrDefault();
                //    }
                //}
                return Session["user"] as EJ.User;
            }
            set
            {
                Session["user"] = value;
            }
        }
    }
}