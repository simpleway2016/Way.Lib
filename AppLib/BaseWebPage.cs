using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using System.Web.UI;

namespace AppLib
{
    public class BaseWebPage : System.Web.UI.Page
    {
        List<string> m_Javascript_EndOfForm = new List<string>();
        List<string> m_Javascript_BeginOfForm = new List<string>();

        /// <summary>
        /// 可重载属性，表示是否缓存当前页，默认值为false
        /// </summary>
        protected virtual bool Cacheable
        {
            get
            {
                return false;
            }
        }
        /// <summary>
        /// 是否根据URL的不同，缓存不同版本，默认值true，如果false，则全部url缓存一个版本
        /// </summary>
        protected virtual bool CacheByUrl
        {
            get
            {
                return true;
            }
        }
        /// <summary>
        /// 缓存有效时间,如果为0，则以文件的修改时间为依据
        /// </summary>
        protected virtual int CacheSeconds
        {
            get
            {
                return 0;
            }
        }

        /// <summary>
        /// 输出防止重复点击按钮，产生提交行为的相关脚本
        /// </summary>
        protected virtual bool OutputDisableRepeatClickScript
        {
            get
            {
                return true;
            }

        }

        System.Web.UI.PageStatePersister _PageStatePersister;
        protected override System.Web.UI.PageStatePersister PageStatePersister
        {
            get
            {
                if (_PageStatePersister == null)
                {
                    //_PageStatePersister = new AppLib.CacheStatePersister(this);
                    _PageStatePersister = new FileStatePersister(this);
                }
                return _PageStatePersister;
            }
        }


        AppLib.PageOutputCache m_pageCache = null;
        public BaseWebPage()
        {
            if (this.Cacheable )
            {
                m_pageCache = new AppLib.PageOutputCache(CacheSeconds, CacheByUrl);
                string filepath = m_pageCache.GetCacheFilePath();
                if (filepath != null)
                {
                    m_pageCache = null;

                    System.IO.FileInfo fileInfo = new System.IO.FileInfo(filepath);
                    HttpContext.Current.Response.AddHeader("Content-Length", fileInfo.Length.ToString());
                    HttpContext.Current.Response.ContentType = "text/html";
                    HttpContext.Current.Response.WriteFile(filepath);
                    HttpContext.Current.Response.End();
                    return;
                }

            }
        }

        protected override void Render(HtmlTextWriter writer)
        {
            if (m_pageCache != null)
            {
                HtmlTextWriter mywriter = m_pageCache.ReadToSave();
                base.Render(mywriter);
                Response.WriteFile(m_pageCache.Save(Request.PhysicalPath));
                Response.End();
                return;
            }
            base.Render(writer);
        }

        //protected override void Render(System.Web.UI.HtmlTextWriter writer)
        //{
        //    StringWriter sw = new StringWriter(); 
        //    HtmlTextWriter htmlWriter = new HtmlTextWriter(sw); 
        //    base.Render(htmlWriter); 
        //    string html = sw.ToString();
        //    sw.Dispose();

        //    writer.Write(AppHelper.MakeStringSmall(html));      
        //}

        protected override void OnPreRender(EventArgs e)
        {
            if (m_Javascript_EndOfForm.Count > 0)
            {
                if (!this.ClientScript.IsStartupScriptRegistered(this.GetType(), "__clientScript"))
                    this.ClientScript.RegisterStartupScript(this.GetType(), "__clientScript", "<script lang=\"ja\">" + m_Javascript_EndOfForm.ToArray().ToEachString() + "</script>");
            }
            if (m_Javascript_BeginOfForm.Count > 0)
            {
                if (!this.ClientScript.IsClientScriptBlockRegistered("__clientScript_begin"))
                {
                    this.ClientScript.RegisterClientScriptBlock(this.GetType(), "__clientScript_begin", "<script lang=\"ja\">" + m_Javascript_BeginOfForm.ToArray().ToEachString() + "</script>");
                }
            }
            base.OnPreRender(e);
        }


        protected override void OnLoad(EventArgs e)
        {
            if (IsPostBack)
            {
                if (Request.Form["_______pageRefresh"] != Convert.ToString(ViewState["_______pageRefresh"]))
                {
                    using (CLog log = new CLog(""))
                    {
                        log.Log("刷新了 ， {0}", Request.Url);
                        //刷新页面的
                        Response.Redirect(Request.Url.ToString());

                    }
                }
            }

            ViewState["_______pageRefresh"] = Convert.ToString(ViewState["_______pageRefresh"]).ToInt() + 1;
            this.ClientScript.RegisterHiddenField("_______pageRefresh", ViewState["_______pageRefresh"].ToString());
            base.OnLoad(e);

            if (true)
            {
                string folder = this.Page.Server.MapPath("/inc/imgs");
                if (System.IO.Directory.Exists(folder) == false)
                {
                    System.IO.Directory.CreateDirectory(folder);
                }
                if (System.IO.File.Exists(folder + "\\loading.gif") == false)
                {
                    System.IO.File.WriteAllBytes(folder + "\\loading.gif", AppLib.Controls.ResourceJS.loading_gif);
                }

            }

            if (OutputDisableRepeatClickScript)
            {
                AppHelper.RegisterJquery(this);
                this.WriteJsToTheEndOfForm(@"
var basewebpage_lastclick_x;
var basewebpage_lastclick_y;
$(document.body).click(function(event)
{
    var target = $(event.target);
    var offset = target.offset();
    basewebpage_lastclick_x = offset.left;
    basewebpage_lastclick_y = offset.top;
});
 $(document.forms[0]).submit(function () {
           
if(document.forms[0].target == '_blank')
return;
            if(window.Page_ClientValidate && !window.Page_ClientValidate())
            return;

             var loading = $(document.createElement('IMG'));
            loading.attr('src' ,  '/inc/imgs/loading.gif');
            loading.css({'position':'absolute','left':basewebpage_lastclick_x,'top':basewebpage_lastclick_y,'z-index':999999});
            document.body.appendChild(loading[0]);

            var inputs = $(""INPUT"");
            for (var i = 0 ; i < inputs.length ; i++) {
                if(inputs[i].type=='button' || inputs[i].type=='submit' )
                {
                inputs[i].style.display = ""none"";
                    }
            }

            inputs = $(""A"");
            for (var i = 0 ; i < inputs.length ; i++) {
                inputs[i].style.display = ""none"";
            }
        });
");
            }
        }


        protected override void OnInit(EventArgs e)
        {
            if (!Cacheable)
            {
                //不让IE缓存此页
                Response.Cache.SetCacheability(System.Web.HttpCacheability.NoCache);
                Response.Cache.SetNoStore();
            }

            base.OnInit(e);
        }

        /// <summary>
        /// 调用js输出alert
        /// </summary>
        /// <param name="msg"></param>
        public void Alert(string msg)
        {
            this.WriteJsToTheEndOfForm("alert(" + JSON.getString(msg) + ");");
        }

        /// <summary>
        /// 输出javascript到页面的底部、表单的最后 (/form前面)
        /// </summary>
        /// <param name="javascript"></param>
        public void WriteJsToTheEndOfForm(string javascript)
        {
            m_Javascript_EndOfForm.Add(javascript);
        }

        /// <summary>
        /// 输出javascript到页面的表单form下面
        /// </summary>
        /// <param name="javascript"></param>
        public void WriteJsToTheBeginOfForm(string javascript)
        {
            m_Javascript_BeginOfForm.Add(javascript);
        }
    }
}
