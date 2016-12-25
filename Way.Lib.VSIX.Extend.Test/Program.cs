using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Way.Lib.VSIX.Extend.AppCodeBuilder;

namespace Way.Lib.VSIX.Extend.Test
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            BuilderWindow win = new BuilderWindow(0, 0, new Way.Lib.VSIX.Extend.AppCodeBuilder.WayGridViewBuilder());
            win.ShowDialog();
         
        }
    }
}
