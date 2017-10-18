
using Gecko;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SunRizStudio
{
    public partial class Form1 : Form
    {
        GeckoWebBrowser gecko;
        public Form1()
        {
            InitializeComponent();

            Gecko.Xpcom.Initialize("Firefox");

            gecko = new GeckoWebBrowser();
            gecko.CreateControl();
            gecko.NoDefaultContextMenu = true; //禁用右键菜单
            gecko.AddMessageEventListener("copyToClipboard", copyToClipboard);

            gecko.Dock = DockStyle.Fill;
            panel1.Controls.Add(gecko);
            gecko.ProgressChanged += Gecko_ProgressChanged;
            gecko.CreateWindow += Gecko_CreateWindow;
            gecko.DocumentCompleted += Gecko_DocumentCompleted;
            gecko.Navigate("http://localhost:8988/editor");
        }

        void copyToClipboard(string message)
        {
            Clipboard.SetText(message);
        }

        private void Gecko_DocumentCompleted(object sender, Gecko.Events.GeckoDocumentCompletedEventArgs e)
        {
            //var executor = new Gecko.JQuery.JQueryExecutor(gecko.Window);  //先获取到jquery对象


            //executor.ExecuteJQuery("$('#a')");    //然后执行jquery的代码
            using (AutoJSContext context = new AutoJSContext(gecko.Window))
            {
                string result;
                context.EvaluateScript("3 + 2;", out result);
                context.EvaluateScript("'hello' + ' ' + 'world';", out result);
            }


            progressBar1.Value = 0;
        }

        private void Gecko_CreateWindow(object sender, GeckoCreateWindowEventArgs e)
        {
            e.InitialHeight = 500;
            e.InitialWidth = 500;
        }

        private void Gecko_ProgressChanged(object sender, GeckoProgressEventArgs e)
        {
            if (e.MaximumProgress == 0)
                return;

            var value = (int)Math.Min(100, (e.CurrentProgress * 100) / e.MaximumProgress);
            if (value == 100)
                return;
            progressBar1.Value = value;
        }
    }
}
