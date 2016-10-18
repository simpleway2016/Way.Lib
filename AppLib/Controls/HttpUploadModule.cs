using System;
using System.Collections;
using System.Collections.Specialized;
using System.Globalization;
using System.IO;
using System.Text;
using System.Web;

using System.Reflection;
using System.Text.RegularExpressions;
using System.Web.Compilation;
using System.Web.UI;
using System.Web.SessionState;
using System.Collections.Generic;
using System.Web.Caching;
using AppLib.Controls;
using System.Threading;

namespace AppLib
{
    internal class limit
    {
        public string ext;
        public int size;
    }
    //实现IHttpModule接口
    public class HttpUploadModule : IHttpModule
    {
        public HttpUploadModule()
        {

        }

        public void Init(HttpApplication application)
        {
            //订阅事件
            application.BeginRequest += new EventHandler(Application_BeginRequest);
        }

        public void Dispose()
        {
        }
        
        internal static void Application_BeginRequest(Object sender, EventArgs e)
        {
            HttpApplication app = sender as HttpApplication;
            Encoding encoding = app.Context.Request.ContentEncoding;
            app.Context.Response.ContentType = "text/html";
          
            // 返回 HTTP 请求正文已被读取的部分。
            //byte[] tempBuff = request.GetPreloadedEntityBody(); //要上传的文件
            string url = app.Context.Request.RawUrl;
            // 如果是附件上传
            if (url.Contains("______uploadfiles=") && app.Request.ContentType.ToLower().Contains("multipart/form-data"))    //判断是不是附件上传
            {
                string tempFolder = "c:\\";
                string guid = "";
               
                Http.HttpProcessor processor = new Http.HttpProcessor(app);
                List<Http.FormEntity> fileEntities = new List<Http.FormEntity>();
                using (CLog log = new CLog(""))
                {
                    try
                    {
                        while (true)
                        {
                            Http.FormEntity entity = processor.Process();

                            if (entity == null)
                                break;
                            else
                            {
                                entity.TempFolder = tempFolder;
                                entity.Guid = guid;
                                entity.GettingFileData += entity_GettingFileData;
                                entity.ProcessHeader();
                                if (entity.Name == "______uploadfiles__submit" && string.IsNullOrEmpty(entity.FileName))
                                {
                                    tempFolder = AppHelper.DecryptString(entity.value);
                                    if (System.IO.Directory.Exists(tempFolder) == false)
                                        System.IO.Directory.CreateDirectory(tempFolder);
                                }
                                else if (entity.Name == "extLimit")
                                {
                                    if (!string.IsNullOrEmpty(entity.value))
                                    {
                                        processor.extLimits = new List<limit>();
                                        string[] infos = entity.value.Split(';');
                                        foreach (string info in infos)
                                        {
                                            if (info.Length == 0)
                                                continue;
                                            string[] extinfo = info.Split('&');
                                            if (extinfo[0].Contains(",") || extinfo[0].Contains("，"))
                                            {
                                                string[] allexts = Regex.Split(extinfo[0], @",|，");
                                                foreach (string ex in allexts)
                                                {
                                                    string ext = ex.ToLower();
                                                    if (ext.StartsWith(".") == false)
                                                        ext = "." + ext;
                                                    processor.extLimits.Add(new limit()
                                                    {
                                                        ext = ext,
                                                        size = Convert.ToInt32(extinfo[1]),
                                                    });
                                                }
                                            }
                                            else
                                            {
                                                string ext = extinfo[0].ToLower();
                                                if (ext.StartsWith(".") == false)
                                                    ext = "." + ext;
                                                processor.extLimits.Add(new limit()
                                                    {
                                                        ext = ext,
                                                        size = Convert.ToInt32(extinfo[1]),
                                                    });
                                            }
                                        }
                                    }
                                }
                                else if (entity.Name == "guid")
                                {
                                    guid = entity.value;
                                    JsonResult jr = HttpRuntime.Cache["UpLoadFiles_" + guid] as JsonResult;
                                    if (jr != null)
                                    {
                                        processor.jsonResult = jr;
                                        jr.Percent = 0;
                                    }
                                    HttpRuntime.Cache.Insert("UpLoadFiles_" + guid, jr, null,
                           System.DateTime.Now.AddMinutes(30), Cache.NoSlidingExpiration,
                                       CacheItemPriority.Default, null);

                                }
                                if (!string.IsNullOrEmpty(entity.FileName))
                                {
                                    fileEntities.Add(entity);
                                }
                            }
                        }
                        foreach (Http.FormEntity entity in fileEntities)
                        {
                            processor.jsonResult.Files.Add(new UpLoadHistory()
                                {
                                    FileName = entity.FileName,
                                    SavePath = entity.TempFolder,
                                });
                        }
                        processor.jsonResult.Percent = 1000;
                    }
                    catch (Exception ex)
                    {
                        log.Log(ex.ToString());
                        processor.jsonResult.Error = ex.Message;
                    }
                    app.Context.Response.End();
                }
            }
            
        }

       static void entity_GettingFileData(Http.FormEntity entity,int totallength ,int readedLength , byte[] data, bool finished)
        {
            if (entity.FileStream == null)
            {
                entity.m_processor.jsonResult.UploadingFileName = entity.FileName;
                string filepath = entity.TempFolder + "\\" + Guid.NewGuid() + Path.GetExtension(entity.FileName);
                entity.FileStream = File.Create(filepath);
                entity.TempFolder = filepath;
            }
            entity.FileStream.Write(data , 0 , data.Length);
            entity.FileWrited += data.Length;
           if(entity.mylimit != null && entity.mylimit.size < entity.FileWrited)
               throw(new Exception( entity.FileName + "超过限定大小"));
            if (finished)
                entity.FileStream.Close();

            entity.m_processor.jsonResult.Percent = (int)(100*(readedLength / (double)totallength));
           
        }


        HttpWorkerRequest GetWorkerRequest(HttpContext context)
        {

            IServiceProvider provider = (IServiceProvider)HttpContext.Current;
            return (HttpWorkerRequest)provider.GetService(typeof(HttpWorkerRequest));
        }

        private static bool StringStartsWithAnotherIgnoreCase(string s1, string s2)
        {
            return (string.Compare(s1, 0, s2, 0, s2.Length, true, CultureInfo.InvariantCulture) == 0);
        }

    }
}
