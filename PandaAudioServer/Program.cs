using PandaAudioServer.Services;
using System;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading;
using Way.Lib.ScriptRemoting;

namespace PandaAudioServer
{
    class Program
    {
        static void RegisterServices()
        {
            var meisheng = new Sms_MeiSheng();
            Factory.RegisterService<ISms>(meisheng);

            //register captures;
            Way.EntityDB.DBContext.RegisterActionCapture(new ActionCaptures.UserEffect_Capture());
        }


        static void Main(string[] args)
        {
            HttpServer httpServer = null;
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
            try
            {
                Console.OutputEncoding = System.Text.Encoding.UTF8;

                using (var db = new PandaDB())
                {
                    db.UserInfo.FirstOrDefault();
                }
                Console.WriteLine($"Panda database ready");

#if DEBUG
                int[] ports = new int[] { 8988 };
#else
                 int[] ports = new int[] { 8988,80 };
#endif
                if (args != null && args.Length > 0)
                {
                    var arr = args[0].Split(',');
                    ports = new int[arr.Length];
                    for (int i = 0; i < ports.Length; i++)
                    {
                        ports[i] = Convert.ToInt32(arr[0]);
                    }
                }

             
                var webroot = $"{Way.Lib.PlatformHelper.GetAppDirectory()}web";

#if DEBUG
                if (System.IO.Directory.Exists(@"D:\projects\c#\EasyJob\Way.Lib.ScriptRemoting.Test.Web"))
                {
                    webroot = @"D:\projects\c#\EasyJob\Way.Lib.ScriptRemoting.Test.Web";
                }
#endif

                if (!System.IO.Directory.Exists(webroot))
                {
                    System.IO.Directory.CreateDirectory(webroot);
                }

                if (System.IO.File.Exists($"{webroot}/main.html") == false)
                {
                    System.IO.File.WriteAllText($"{webroot}/main.html", "<html><body></body></html>");
                }
              

                httpServer = new HttpServer(ports, webroot);
                if (File.Exists(Way.Lib.PlatformHelper.GetAppDirectory() + "ServerCert.pfx"))
                {
                    var cer = new X509Certificate2(Way.Lib.PlatformHelper.GetAppDirectory() + "ServerCert.pfx", "123456");

                    httpServer.UseHttps(cer, true);
                    Console.WriteLine($"use X509Certificate2 ssl ServerCert.pfx");
                }
                Console.WriteLine($"server starting at port:{Newtonsoft.Json.JsonConvert.SerializeObject(ports)}...");

                RegisterServices();
                httpServer.Start();
                Console.WriteLine($"path:{httpServer.Root}");

                string effectPath = $"{httpServer.Root}effects";
                if (System.IO.Directory.Exists(effectPath) == false)
                {
                    Directory.CreateDirectory(effectPath);
                }

                Console.WriteLine($"web server started");
               
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

            
            while (true)
            {
                Thread.Sleep(10000000);
                //Console.Write("Web>");
                //var line = Console.ReadLine();
                //if (line == null)
                //{
                //    //是在后台运行的
                //    while (true)
                //    {
                //        System.Threading.Thread.Sleep(10000000);
                //    }
                //}
                //else if (line == "exit")
                //    break;
            }
            if(httpServer != null)
            {
                httpServer.Stop();
            }            
            System.Diagnostics.Process.GetCurrentProcess().Kill();
        }

        private static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            using (Way.Lib.CLog log = new Way.Lib.CLog("domain error"))
            {
                log.Log(e.ExceptionObject.ToString());
            }
        }
    }
}