using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Way.Lib.ScriptRemoting;

namespace RemoteWindow
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            Way.Lib.ScriptRemoting.ScriptRemotingServer.RegisterHandler(new PngHandler());

            Way.Lib.ScriptRemoting.ScriptRemotingServer.Start(6888, @"D:\projects\c#\EasyJob\Way.Lib.ScriptRemoting.Test.Web\RemoteWindow", 0);
            Application.Run(new Form1());
        }
    }

    class PngHandler : Way.Lib.ScriptRemoting.ICustomHttpHandler
    {
        public void Handle(string originalUrl, HttpConnectInformation connectInfo, ref bool handled)
        {
            if(originalUrl.EndsWith(".png"))
            {
                string accept_encoding = connectInfo.Request.Headers["Accept-Encoding"];
                if (!string.IsNullOrEmpty(accept_encoding) && accept_encoding.Contains("gzip"))
                {
                    handled = true;

                    connectInfo.Response.Headers["Content-Encoding"] = "gzip";
                    connectInfo.Response.Headers["Content-Type"] = "image/png";
                    var bs = System.IO.File.ReadAllBytes("f:\\a.png");
                    bs = Helper.GZip(bs);
                    connectInfo.Response.Headers["Content-Length"] = bs.Length.ToString();
                    connectInfo.Response.Write(bs);
                }
            }
        }
    }
}
