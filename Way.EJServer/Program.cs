using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
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
            ScriptRemotingServer.RegisterHandler(new DownloadTableDataHandler());
            ScriptRemotingServer.RegisterHandler(new ImportDataHandler());

            Console.WriteLine($"server starting at port:{port}...");
            var webroot = $"{Way.Lib.PlatformHelper.GetAppDirectory()}Port{port}";

            if (!System.IO.Directory.Exists(webroot))
            {
                System.IO.Directory.CreateDirectory(webroot);
            }

            if (System.IO.File.Exists($"{webroot}/main.html") == false)
            {
                System.IO.File.WriteAllText($"{webroot}/main.html", "<html><body controller=\"Way.EJServer.MainController\"></body></html>");
            }
            Console.WriteLine($"path:{webroot}");
            ScriptRemotingServer.Start(port, webroot, 1);

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
