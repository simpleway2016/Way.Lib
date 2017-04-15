using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Way.Lib.ScriptRemoting;

namespace Way.EJServer
{
    public class Program
    {
        public static void Main(string[] args)
        {
            int port = 6060;
            if(args != null && args.Length > 0)
            {
                port = Convert.ToInt32(args[0]);
            }

            ScriptRemotingServer.RegisterHandler(new DownLoadCodeHandler());
            Console.WriteLine($"server starting at port:{port}...");
            if (!System.IO.Directory.Exists($"{Way.Lib.PlatformHelper.GetAppDirectory()}web"))
            {
                System.IO.Directory.CreateDirectory($"{Way.Lib.PlatformHelper.GetAppDirectory()}web");
            }

            if (System.IO.File.Exists($"{Way.Lib.PlatformHelper.GetAppDirectory()}web/main.html") == false)
            {
                System.IO.File.WriteAllText($"{Way.Lib.PlatformHelper.GetAppDirectory()}web/main.html", "<html><body controller=\"Way.EJServer.MainController\"></body></html>");
            }
            Console.WriteLine($"path:{Way.Lib.PlatformHelper.GetAppDirectory()}web");
            ScriptRemotingServer.Start(6060, $"{Way.Lib.PlatformHelper.GetAppDirectory()}web", 1);

            while (true)
            {
                Console.Write("Web>");
                if (Console.ReadLine() == "exit")
                    break;
            }
            ScriptRemotingServer.Stop();
            System.Diagnostics.Process.GetCurrentProcess().Kill();
        }
    }
}
