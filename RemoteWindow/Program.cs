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

            RemotingController.OnMessageReceiverConnect += MainController.RemotingController_OnMessageReceiverConnect;
            RemotingController.OnMessageReceive += RemotingController_OnMessageReceive;

            Way.Lib.ScriptRemoting.ScriptRemotingServer.Start(6888, @"D:\projects\c#\EasyJob\Way.Lib.ScriptRemoting.Test.Web\RemoteWindow", 0);
            Application.Run(new Form1());
        }

        private static void RemotingController_OnMessageReceive(SessionState session, string message)
        {
             
        }
    }

    class PngHandler : Way.Lib.ScriptRemoting.ICustomHttpHandler
    {
        public void Handle(string originalUrl, HttpConnectInformation connectInfo, ref bool handled)
        {
            if(originalUrl.Contains(".aspx") && connectInfo.Request.Query["id"] != null)
            {
                int id = Convert.ToInt32(connectInfo.Request.Query["id"]);
                string accept_encoding = connectInfo.Request.Headers["Accept-Encoding"];
                if (!string.IsNullOrEmpty(accept_encoding) && accept_encoding.Contains("gzip"))
                {
                    handled = true;

                    connectInfo.Response.Headers["Content-Encoding"] = "gzip";
                    connectInfo.Response.Headers["Content-Type"] = "image/png";
                    byte[] bs = null;
                    using (var bitmap = MainController.GetBitmapById(id))
                    {
                        if (bitmap != null)
                        {
                            using (var ms = new System.IO.MemoryStream())
                            {
                                bitmap.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                                bs = ms.ToArray();
                            }
                        }
                    }

                    bs = Helper.GZip(bs);
                    connectInfo.Response.Headers["Content-Length"] = bs.Length.ToString();
                    connectInfo.Response.Write(bs);
                }
            }
        }
    }
}
