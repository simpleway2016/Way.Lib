using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Way.Lib.ScriptRemoting.CodeBuilder
{
    public partial class MainForm : Form
    {
        static private string xulrunnerPath = Application.StartupPath + "\\xulrunner";
        Gecko.GeckoWebBrowser browser;
        public MainForm()
        {
            InitializeComponent();

            Skybound.Gecko.Xpcom.Initialize(xulrunnerPath);
            GeckoPreferences.User["gfx.font_rendering.graphite.enabled"] = true;

            browser = new Gecko.GeckoWebBrowser();
            browser.Dock = DockStyle.Fill;
            this.Controls.Add(browser);
            browser.DocumentCompleted += Browser_DocumentCompleted;
            browser.Navigate("http://mail.qq.com");
        }

        private void Browser_DocumentCompleted(object sender, Gecko.Events.GeckoDocumentCompletedEventArgs e)
        {

        }
    }
}
