using Gecko;
using SunRizStudio.Models.Nodes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SunRizStudio.Documents
{
    /// <summary>
    /// ControlWindowDocument.xaml 的交互逻辑
    /// </summary>
    public partial class ControlWindowDocument : BaseDocument
    {
        static bool InitFireFoxed = false;
        GeckoWebBrowser _gecko;
        ControlWindowContainerNode _parentNode;
        internal SunRizServer.ControlWindow _dataModel;
        List<MyDriverClient> _clients = new List<MyDriverClient>();
        AutoJSContext jsContext;
        /// <summary>
        /// 是否允许模式
        /// </summary>
        bool _isRunMode = false;
        public ControlWindowDocument()
        {
            InitializeComponent();

        }

        internal ControlWindowDocument(ControlWindowContainerNode parent, SunRizServer.ControlWindow dataModel,bool isRunMode)
        {
            _isRunMode = isRunMode;
            _dataModel = dataModel ?? new SunRizServer.ControlWindow()
            {
                ControlUnitId = parent._controlUnitId,
                FolderId = parent._folderId,
            };
            _parentNode = parent;
            InitializeComponent();
            this.Title = "初始化...";
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
            this.Title = "Loading...";
            _gecko = new GeckoWebBrowser();
            _gecko.CreateControl();
            _gecko.Enabled = false;
            //_gecko.NoDefaultContextMenu = true; //禁用右键菜单
            _gecko.AddMessageEventListener("copyToClipboard", copyToClipboard);
            _gecko.AddMessageEventListener("save", save);
            _gecko.AddMessageEventListener("loadFinish", loadFinish);
            _gecko.AddMessageEventListener("watchPointValues", watchPointValues);
            _gecko.AddMessageEventListener("openRunMode", openRunMode);

            winHost.Child = _gecko;
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
        void openRunMode(string arg)
        {
            var doc = new ControlWindowDocument(_parentNode, _dataModel,true);
            MainWindow.Instance.SetActiveDocument(doc);
        }
        void watchPointValues(string jsonStr)
        {
            Task.Run(()=> {
                try
                {
                    var pointArr = (Newtonsoft.Json.Linq.JArray)Newtonsoft.Json.JsonConvert.DeserializeObject(jsonStr);
                    var groups = (from m in pointArr
                                  group m by m.Value<int>("deviceId") into g
                                  select new
                                  {
                                      deviceId = g.Key,
                                      points = g.ToArray(),
                                  }).ToArray();
                    foreach (var group in groups)
                    {
                        var device = Helper.Remote.InvokeSync<SunRizServer.Device>("GetDeviceAndDriver", group.deviceId);
                        var points = group.points.Select(m => m.Value<string>("addr")).ToArray();
                        var client = new MyDriverClient(device.Driver.Address, device.Driver.Port.Value);
                        _clients.Add(client);
                        watchDevice(client , device, points);
                    }
                }
                catch(Exception ex)
                {
                    MainWindow.Instance.Dispatcher.Invoke(() => {
                        MessageBox.Show(MainWindow.Instance, ex.Message);
                    });
                }
            });            
        }

        public override void OnClose(ref bool canceled)
        {
            foreach( var client in _clients )
            {
                client.Released = true;
                client.NetClient.Close();
            }
            base.OnClose(ref canceled);
        }

        void watchDevice(MyDriverClient client , SunRizServer.Device device , string[] points)
        {

            client.NetClient = client.AddPointToWatch(device.Address, points, (point, value) =>
            {
                try
                {
                    _gecko.Invoke(new ThreadStart(()=> {
                        jsContext.EvaluateScript($"onReceiveValueFromServer({ (new { addr = point, value = value }).ToJsonString()})");
                    }));                    
                }
                catch(Exception ex)
                {

                }
            }, (err) =>
            {
                if (client.Released)
                    return;

                Task.Run(() => {
                    Thread.Sleep(2000);
                    watchDevice(client,device, points);
                });
            });
        }
        
        void loadFinish(string msg)
        {
            this.Title = _dataModel.Name;
            _gecko.Enabled = true;
            jsContext = new AutoJSContext(_gecko.Window);
            if(_isRunMode)
            {
                jsContext.EvaluateScript("run()");
            }
        }
        void copyToClipboard(string message)
        {
            Clipboard.SetText(message);
        }

        void save(string json)
        {
            this.Title = "正在保存...";
            Helper.Remote.Invoke<SunRizServer.ControlWindow>("SaveWindowContent", (ret, err) => {
                
                if (err != null)
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
                    this.Title = _dataModel.Name;
                }
                this.Title = _dataModel.Name;
            }, _dataModel, json);
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

    class MyDriverClient:SunRizDriver.SunRizDriverClient
    {
        public bool Released = false;
        public Way.Lib.NetStream NetClient;
        public MyDriverClient(string addr,int port) : base(addr , port)
        {

        }
    }
}
