using System;
using Way.Lib.ScriptRemoting;
using System.Linq;
using Microsoft.EntityFrameworkCore;
namespace SunRizServer
{
    public class Program
    {
        public static void Main(string[] args)
        {
            try
            {
                //System.Diagnostics.Debugger.Launch();
                Console.OutputEncoding = System.Text.Encoding.UTF8;

                int port = 8988;
                if (args != null && args.Length > 0)
                {
                    port = Convert.ToInt32(args[0]);
                }

                Console.WriteLine($"server starting at port:{port}...");
                var webroot = $"{Way.Lib.PlatformHelper.GetAppDirectory()}web";

#if DEBUG
                if (System.IO.Directory.Exists(@"D:\注释\2016\EasyJobCore\Way.Lib.ScriptRemoting.Test.Web\组态"))
                {
                    webroot = @"D:\注释\2016\EasyJobCore\Way.Lib.ScriptRemoting.Test.Web\组态";
                }
                else if(System.IO.Directory.Exists(@"D:\projects\c#\EasyJob\Way.Lib.ScriptRemoting.Test.Web\组态"))
                {
                    webroot = @"D:\projects\c#\EasyJob\Way.Lib.ScriptRemoting.Test.Web\组态";
                }
#endif

                if (!System.IO.Directory.Exists(webroot))
                {
                    System.IO.Directory.CreateDirectory(webroot);
                }
                if (!System.IO.Directory.Exists(Way.Lib.PlatformHelper.GetAppDirectory() + "windows/"))
                {
                    System.IO.Directory.CreateDirectory(Way.Lib.PlatformHelper.GetAppDirectory() + "windows/");
                }
               

                Console.WriteLine($"path:{webroot}");

                ScriptRemotingServer.Start(port, webroot, 1);
                Console.WriteLine($"web server started");

                //注册数据库触发器
                SysDB.RegisterActionCapture(new DBTriggers.ImageFilesTrigger());
                SysDB.RegisterActionCapture(new DBTriggers.DevicePointFolderTrigger());
                SysDB.RegisterActionCapture(new DBTriggers.ControlWindowTrigger());
                SysDB.RegisterActionCapture(new DBTriggers.ControlWindowFolderTrigger());

                using (var db = new SysDB())
                {
                    db.ImageFiles.FirstOrDefault();
                }
                Console.WriteLine($"database ready");

                SystemLog.Init();

                //记录历史
                HistoryRecord.HistoryAutoRec.ReStart();
               
            }
            catch (Exception ex)
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
    }
}
