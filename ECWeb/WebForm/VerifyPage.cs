using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI.HtmlControls;

namespace ECWeb.WebForm
{
    public class VerifyPage : AppLib.BaseWebPage
    {
        public EJ.User User
        {
            get
            {
                return Session["user"] as EJ.User;
            }
            set
            {
                Session["user"] = value;
            }
        }

        protected override void OnInit(EventArgs e)
        {
            if (this.User == null)
            {
                Response.Write("请先登录");
                Response.End();
            }
            base.OnInit(e);
        }

        protected override void OnLoad(EventArgs e)
        {
            AppHelper.RegisterJquery(this);
            RegCss("/css/dataEditor.css");
            RegCss("/css/grid.css");
            base.OnLoad(e);
        }

        protected void RegCss(string filepath)
        {
            HtmlGenericControl _CssFile = new HtmlGenericControl("link");
            _CssFile.Attributes["rel"] = "stylesheet";
            _CssFile.Attributes["type"] = "text/css";
            _CssFile.Attributes["href"] = filepath;
            this.Header.Controls.Add(_CssFile);
        }
    }
}