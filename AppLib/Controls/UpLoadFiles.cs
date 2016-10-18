using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Caching;
using System.Web.UI.Design;


namespace AppLib.Controls
{
    public class UpLoadFilesDesigner : ControlDesigner
    {
        public override string GetDesignTimeHtml()
        {
            UpLoadFiles ctrl = ((UpLoadFiles)this.Component);
            return "<div style=\""+ctrl.Attributes["style"]+"\" class='"+ctrl.CssClass+"'>"+ctrl.ID+"</div>";
        }
    }
    public class JsonResult
    {
        public List<UpLoadHistory> Files = new List<UpLoadHistory>();
        public int Percent;
        public string UploadingFileName;
        public string Error = "";
    }
    [Serializable]
    public class UpLoadHistory
    {
        /// <summary>
        /// 原始文件名
        /// </summary>
        public string FileName;
        /// <summary>
        /// 保存路径
        /// </summary>
        public string SavePath;
    }
    [Designer(typeof(UpLoadFilesDesigner))]
    public class UpLoadFiles : BaseControl
    {
        static bool _bindedApplicationBeginRequest = false;
        public static void ApplicationBeginRequest(Object sender, EventArgs e)
        {
            if (!_bindedApplicationBeginRequest)
            {
                _bindedApplicationBeginRequest = true;
            }
            try
            {
                HttpUploadModule.Application_BeginRequest(sender, e);
            }
            catch
            {
            }
        }
        private string Guid
        {
            get
            {
                if (ViewState["guid"] == null)
                    ViewState["guid"] = System.Guid.NewGuid().ToString();
                return (string)ViewState["guid"];
            }
        }

         [System.ComponentModel.Category("系统设定"), System.ComponentModel.Description("添加文件按钮的id")]
        public string AddButtonID
        {
            get
            {
                return (string)ViewState["AddButtonID"];
            }
            set {
                ViewState["AddButtonID"] = value;
            }
        }

         public string FileCSS
         {

             get
             {
                 return Convert.ToString( ViewState["FileCSS"]);
             }
             set
             {
                 ViewState["FileCSS"] = value;
             }
         }

         public string ItemWidth
         {

             get
             {
                 if (ViewState["ItemWidth"] == null)
                     ViewState["ItemWidth"] = "300px";
                 return (string)ViewState["ItemWidth"];
             }
             set
             {
                 ViewState["ItemWidth"] = value;
             }
         }

        [System.ComponentModel.Category("系统设定"), System.ComponentModel.Description("生成的临时文件放在哪个目录")]
        public string FolderPath
        {
            get
            {
                return  Convert.ToString( ViewState["FolderPath"]);
            }
            set {
                ViewState["FolderPath"] = value;
                if (System.IO.Directory.Exists(value) == false)
                {
                    System.IO.Directory.CreateDirectory(value);
                }
            }
        }

        public List<UpLoadHistory> UpLoadHistories
        {
            get
            {
                if (ViewState["UpLoadHistories"] == null)
                    ViewState["UpLoadHistories"] = new List<UpLoadHistory>();
                return (List<UpLoadHistory>)ViewState["UpLoadHistories"];
            }
        }
        static System.Web.Script.Serialization.JavaScriptSerializer JSON = new System.Web.Script.Serialization.JavaScriptSerializer();
        protected override void LoadViewState(object savedState)
        {
           
            base.LoadViewState(savedState);
            if (!string.IsNullOrEmpty(this.Page.Request.Form["xml_" + ClientID]))
            {
                try
                {
                   JsonResult jr = JSON.Deserialize<JsonResult>(this.Page.Request.Form["xml_" + ClientID]);
                    if(jr.Files != null)
                   this.UpLoadHistories.AddRange(jr.Files);
                }
                catch
                {
                }
                HttpRuntime.Cache.Remove("UpLoadFiles_" + this.Guid);
            }
        }

        protected override object SaveViewState()
        {
            //这里先获取一下guid，否则，Render的时候再获取，已经错过了SaveViewState
            object guid = this.Guid;

            return base.SaveViewState();
        }

        protected override void OnLoad(EventArgs e)
        {
            if (this.Page.Request.Form["____UpLoadFiles_init_"] != null)
            {
                string guid = this.Page.Request.Form["____UpLoadFiles_init_"];
                JsonResult jr = new JsonResult()
                {
                    Percent = 0,
                    UploadingFileName = "",
                };

                HttpRuntime.Cache.Insert("UpLoadFiles_" + guid, jr, null,
                        System.DateTime.Now.AddMinutes(30), System.Web.Caching.Cache.NoSlidingExpiration,
                                    CacheItemPriority.Default, null);
            }
            if (this.Page.Request.Form["____UpLoadFiles_getProgress"] != null)
            {
                string guid = this.Page.Request.Form["____UpLoadFiles_getProgress"];
 
                JsonResult jr = HttpRuntime.Cache["UpLoadFiles_" + guid] as JsonResult;
                if (jr != null)
                {
                    this.Page.Response.Write(JSON.Serialize(jr));
                    this.Page.Response.End();
                    return;
                }
                else
                {
                    this.Page.Response.Write(JSON.Serialize(new JsonResult()
                        {
                            Percent = 0,
                            UploadingFileName = ""
                        }));
                    this.Page.Response.End();
                }
            }
            
            base.OnLoad(e);
            WriteJS();
            //this.Page.Form.Enctype = "multipart/form-data";
        }

        static bool writedjs;
        private void WriteJS()
        {
            this.Page.ClientScript.RegisterHiddenField("xml_" + ClientID, "");
            if (writedjs == false)
            {
                writedjs = true;
                string folder = this.Page.Server.MapPath("/inc/js");
                if (System.IO.Directory.Exists(folder) == false)
                {
                    System.IO.Directory.CreateDirectory(folder);
                }

                folder = this.Page.Server.MapPath("/inc/imgs");
                if (System.IO.Directory.Exists(folder) == false)
                {
                    System.IO.Directory.CreateDirectory(folder);
                }
                if (System.IO.File.Exists(folder + "\\delete_icon.png") == false)
                {
                    System.IO.File.WriteAllBytes(folder + "\\delete_icon.png", ResourceJS.deleteicon_png);
                }
            }

            AppHelper.RegisterInvokerJS(this.Page);
            AppHelper.RegisterJquery(this.Page);

            if (!this.Page.ClientScript.IsClientScriptIncludeRegistered("__UpLoadFiles_JSFile"))
            {
                Page.ClientScript.RegisterClientScriptInclude("__UpLoadFiles_JSFile",
                Page.ClientScript.GetWebResourceUrl(this.GetType(),
                                            "AppLib.js.UpLoadFiles.js"));
            }
           
        }

        protected override void Render(System.Web.UI.HtmlTextWriter writer)
        {
            if (string.IsNullOrEmpty(FolderPath))
            {
                writer.WriteLine("<font color=red>请设置FolderPath属性</font>");
            }
            if (string.IsNullOrEmpty(AddButtonID))
            {
                writer.WriteLine("<font color=red>请设置AddButtonID【添加文件按钮的html里的id】属性</font>");
            }
            if (_bindedApplicationBeginRequest == false)
            {
                writer.WriteLine("<font color=red>请在Global的Application_BeginRequest事件里，加入代码：<Br>AppLib.Controls.UpLoadFiles.ApplicationBeginRequest(sender, e);</font>");
            }
            writer.WriteLine(string.Format("<div id=\"div_{0}\" style=\"{1}\" class='{2}'></div>", ClientID, Attributes["style"], CssClass));
            writer.WriteLine(string.Format("<script lang=\"ja\">var js_{0} = new JS_UpLoadFiles(\"{0}\",\"{1}\",\"{2}\",\"{3}\",\"{4}\",\"{5}\");</script>", ClientID, Guid, AppHelper.EncryptString(FolderPath).Replace("\\", "\\\\"), AddButtonID, ItemWidth,FileCSS));
            //writer.WriteLine(string.Format("<input type=\"file\" name=\"{0}\"  style=\"{1}\" class='{2}'>", ClientID, Attributes["style"], CssClass));
        }
    }
}
