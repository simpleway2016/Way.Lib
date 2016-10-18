using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ECWeb.WebForm
{
    public partial class UserMgr : AutoHideGridPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
          
            if (this.User.Role != EJ.User_RoleEnum.管理员)
            {
                Response.Write("not arrow");
                Response.End();
            }
        }
    }
}