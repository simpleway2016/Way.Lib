using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

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
            Way.Lib.ScriptRemoting.ScriptRemotingServer.Start(6888, @"D:\projects\c#\EasyJob\Way.Lib.ScriptRemoting.Test.Web\RemoteWindow", 0);
            Application.Run(new Form1());
        }
    }
}
