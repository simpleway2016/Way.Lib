using Gecko;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SunRizStudio.ControlWindow
{
    public partial class Editor : Form
    {
        static bool InitFireFoxed = false;
        GeckoWebBrowser gecko;
        public Editor()
        {
            InitializeComponent();
            this.Text = "正在初始化...";
            this.init();
        }

        async void init()
        {
            if (!InitFireFoxed)
            {
                InitFireFoxed = true;
                await Task.Run(() =>
                {
                    Thread.Sleep(1000);
                });
                Gecko.Xpcom.Initialize("Firefox");
            }
            this.Text = "监视画面编辑器";
            gecko = new GeckoWebBrowser();
            gecko.CreateControl();
            gecko.NoDefaultContextMenu = true; //禁用右键菜单
            gecko.AddMessageEventListener("copyToClipboard", copyToClipboard);

            gecko.Dock = DockStyle.Fill;
            this.Controls.Add(gecko);
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
           
           // progressBar1.Value = 0;
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
           // progressBar1.Value = value;
        }
    }
}
