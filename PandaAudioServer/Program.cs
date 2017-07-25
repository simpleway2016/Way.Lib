using PandaAudioServer.Services;
using System;
using Way.Lib.ScriptRemoting;

namespace PandaAudioServer
{
    class Program
    {
        static void RegisterServices()
        {
            var meisheng = new Sms_MeiSheng();
            Factory.RegisterService<ISms>(meisheng);
        }


        static void Main(string[] args)
        {

            Console.OutputEncoding = System.Text.Encoding.UTF8;

            int port = 8988;
            if (args != null && args.Length > 0)
            {
                port = Convert.ToInt32(args[0]);
            }

            Console.WriteLine($"server starting at port:{port}...");
            var webroot = $"{Way.Lib.PlatformHelper.GetAppDirectory()}Port{port}";

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
            ScriptRemotingServer.Start(port, webroot, 1);

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