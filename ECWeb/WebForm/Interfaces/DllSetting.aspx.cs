using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ECWeb.WebForm.Interfaces
{
    public partial class DllSetting :AutoHideGridPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                grid.SqlTermString = "ProjectID=" + Request.QueryString["projectid"];
            }
        }

        protected void editor_BeforeSave(object database, object editor, AppLib.Controls.ActionType actionType, object dataitem)
        {
            var data = (EJ.DLLImport)dataitem;
            data.ProjectID = Request.QueryString["projectid"].ToInt();
        }
    }
}