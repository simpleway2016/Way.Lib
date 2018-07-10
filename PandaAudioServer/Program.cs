using PandaAudioServer.Services;
using System;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
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
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
            try
            {
                Console.OutputEncoding = System.Text.Encoding.UTF8;
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
                if (File.Exists(Way.Lib.PlatformHelper.GetAppDirectory() + "EJServerCert.pfx"))
                {
                    ScriptRemotingServer.UseHttps(new X509Certificate(Way.Lib.PlatformHelper.GetAppDirectory() + "ServerCert.pfx", "123456"), true);
                    Console.WriteLine($"use ssl ServerCert.pfx");
                }
                Console.WriteLine($"server starting at port:{Newtonsoft.Json.JsonConvert.SerializeObject(ports)}...");
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
                Console.WriteLine($"path:{webroot}");
                RegisterServices();
                ScriptRemotingServer.Start(ports, webroot, 0);
                Console.WriteLine($"web server started");
                using (var db = new PandaDB())
                {
                    db.UserInfo.FirstOrDefault();
                }
                Console.WriteLine($"database ready");
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

            
            while (true)
            {
                Console.Write("Web>");
                var line = Console.ReadLine();
                if (line == null)
                {
                    //是在后台运行的
                    while (true)
                    {
                        System.Threading.Thread.Sleep(10000000);
                    }
                }
                else if (line == "exit")
                    break;
            }
            ScriptRemotingServer.Stop();
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