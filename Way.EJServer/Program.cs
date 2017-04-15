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
            ScriptRemotingServer.RegisterHandler(new DownLoadCodeHandler());
            Console.WriteLine("server starting...");
            if (System.IO.Directory.Exists($"{Way.Lib.PlatformHelper.GetAppDirectory()}web"))
            {

                Console.WriteLine($"path:{Way.Lib.PlatformHelper.GetAppDirectory()}web");
                ScriptRemotingServer.Start(6060, $"{Way.Lib.PlatformHelper.GetAppDirectory()}web", 1);
            }
            else
            {
                Console.WriteLine("can not find web folder");
            }

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
