using Gecko;
using SunRizStudio.Models.Nodes;
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
        GeckoWebBrowser _gecko;
        ControlWindowContainerNode _parentNode;
        SunRizServer.ControlWindow _dataModel;
        internal Editor(ControlWindowContainerNode parent, SunRizServer.ControlWindow dataModel)
        {
            _dataModel = dataModel??new SunRizServer.ControlWindow() {
                ControlUnitId = parent._controlUnitId,
                FolderId = parent._folderId,
            };
            _parentNode = parent;
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
            this.Text = "正在加载画面内容";
            _gecko = new GeckoWebBrowser();
            _gecko.CreateControl();
            _gecko.Enabled = false;
            _gecko.NoDefaultContextMenu = true; //禁用右键菜单
            _gecko.AddMessageEventListener("copyToClipboard", copyToClipboard);
            _gecko.AddMessageEventListener("save", save);
            _gecko.AddMessageEventListener("loadFinish", loadFinish);

            _gecko.Dock = DockStyle.Fill;
            this.Controls.Add(_gecko);
            _gecko.ProgressChanged += Gecko_ProgressChanged;
            _gecko.CreateWindow += Gecko_CreateWindow;
            _gecko.DocumentCompleted += Gecko_DocumentCompleted;
            if (_dataModel.id != null)
            {
                _gecko.Navigate($"{Helper.Url}/Home/GetWindowContent?windowid={_dataModel.id}");
            }
            else
            {
                _gecko.Navigate($"{Helper.Url}/editor");
            }
        }
        void loadFinish(string msg)
        {
            this.Text = "监视画面编辑器";
            _gecko.Enabled = true;
        }
        void copyToClipboard(string message)
        {
            Clipboard.SetText(message);
        }

        void save(string json)
        {
            Helper.Remote.Invoke<SunRizServer.ControlWindow>("SaveWindowContent", (ret, err) => {
                if(err != null)
                {
                    MessageBox.Show(err);
                }
                else
                {
                    _dataModel.CopyValue(ret);
                    if (_dataModel.id == null)
                    {                      
                        _parentNode.Nodes.Add(new ControlWindowNode(_dataModel));
                    }
                    _dataModel.ChangedProperties.Clear();
                }
            }, _dataModel , json);
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
