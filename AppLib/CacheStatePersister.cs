using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Caching;
using System.Web.UI;

namespace AppLib
{
    public class CacheStatePersister : PageStatePersister
    {
        private Cache mCache = HttpRuntime.Cache;
        public CacheStatePersister(Page page) : base(page) { }
        public override void Load()
        { 
            string _vskey = Page.Request["_VIEWSTATE_$_KEY"];
            if (_vskey != null)
            {
                Pair pair = HttpRuntime.Cache[_vskey] as Pair;
                if (pair != null)
                {
                    ViewState = pair.First;
                    ControlState = pair.Second;
                }
            }
        }

        public static string CreateKey(Page page)
        {
            return Guid.NewGuid().ToString().Replace("-", "");
            string url = page.Request.RawUrl;
            if (url.Contains("?"))
                url = url.Substring(0,url.IndexOf("?"));
            url = url.Replace("/", "_").Replace(".", "_");
            return HttpContext.Current.Session.SessionID + url + Guid.NewGuid().ToString().Replace("-","");
        }
 
        public override void Save()
        {
            string _vskey = Page.Request["_VIEWSTATE_$_KEY"];
            if (_vskey == null)
            {
                _vskey = "VIEWSTATE_" + HttpContext.Current.Session.SessionID + "_" + Page.Request.RawUrl +
                       "_" + System.DateTime.Now.Ticks.ToString();
            }

            Pair pair = new Pair(this.ViewState , this.ControlState);
            HttpRuntime.Cache.Insert(_vskey, pair, null,
                System.DateTime.Now.AddMinutes(HttpContext.Current.Session.Timeout*2), Cache.NoSlidingExpiration,
                            CacheItemPriority.Default, null);
 
            this.Page.ClientScript.RegisterHiddenField("_VIEWSTATE_$_KEY", _vskey);
        }
    }

    public class FileStatePersister : PageStatePersister
    {
        internal static string TempFolder = null;
        static int SessionTimeout;
        static ObjectStateFormatter formatter = new ObjectStateFormatter();
        static void CheckViewStateFiles()
        {
            while (true)
            {
                try
                {
                    string[] files = Directory.GetFiles(TempFolder);
                    foreach (string file in files)
                    {
                        DateTime lastwriteTime = new FileInfo(file).LastWriteTime;
                        if ((DateTime.Now - lastwriteTime).TotalHours >= 2)
                        {
                            File.Delete(file);
                        }
                    }
                }
                catch { }
                Thread.Sleep(1000 * 60);
            }
        }
        public FileStatePersister(Page page) : base(page) {
            if (TempFolder == null)
            {
                TempFolder = HttpContext.Current.Server.MapPath("/_viewstate_temp");
                SessionTimeout = HttpContext.Current.Session.Timeout;
                new Thread(CheckViewStateFiles).Start();
            }
            if (System.IO.Directory.Exists(TempFolder) == false)
            {
                Directory.CreateDirectory(TempFolder);
            }
        }
        public override void Load()
        {
            string _vskey = Page.Request["_VIEWSTATE_$_KEY"];
            if (_vskey != null)
            {
                try
                {
                    using (FileStream fs = File.OpenRead(TempFolder + "\\" + _vskey + ".config"))
                    {
                        Pair pair = formatter.Deserialize(fs) as Pair;
                        if (pair != null)
                        {
                            ViewState = pair.First;
                            ControlState = pair.Second;
                        }
                    }
                }
                catch(Exception ex)
                {
                    if (!Page.ClientScript.IsClientScriptBlockRegistered("__FileStatePersister"))
                    {
                        Page.ClientScript.RegisterClientScriptBlock(Page.GetType(), "__FileStatePersister", "<script lang=\"ja\">alert('您长时间没有操作此页面，请重新操作！');</script>");
                    }
                }
            }
        }

        /// <summary>
        /// 生成一个状态记录
        /// </summary>
        /// <param name="content"></param>
        public void WriteState( string filename , object content)
        {
            File.WriteAllText(TempFolder + "\\" + filename + ".config", new System.Web.Script.Serialization.JavaScriptSerializer().Serialize(content));
               
        }

       /// <summary>
        /// 读取状态
       /// </summary>
       /// <param name="filename"></param>
       /// <returns></returns>
        public T ReadState<T>(string filename  )
        {
            return (T)new System.Web.Script.Serialization.JavaScriptSerializer().Deserialize(File.ReadAllText(TempFolder + "\\" + filename + ".config") , typeof(T)); 
        }

        public override void Save()
        {
            string _vskey = Page.Request["_VIEWSTATE_$_KEY"];
            if (_vskey == null)
            {
                _vskey = "V_" + CacheStatePersister.CreateKey(Page);
            }

            Pair pair = new Pair(this.ViewState, this.ControlState);
            try
            {
                using (FileStream fs = File.Create(TempFolder + "\\" + _vskey + ".config"))
                {
                    formatter.Serialize(fs, pair);
                }
            }
            catch (Exception ex)
            {
                throw(ex);
            }

            this.Page.ClientScript.RegisterHiddenField("_VIEWSTATE_$_KEY", _vskey);
        }
    }
}

