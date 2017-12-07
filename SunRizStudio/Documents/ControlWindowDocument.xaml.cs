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
        bool closeAfterSave = false;
        /// <summary>
        /// 是否允许模式
        /// </summary>
        internal bool IsRunMode = false;
        public ControlWindowDocument()
        {
            InitializeComponent();

        }

        internal ControlWindowDocument(ControlWindowContainerNode parent, SunRizServer.ControlWindow dataModel, bool isRunMode)
        {
            IsRunMode = isRunMode;
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
            this.AllowDrop = true;
            _gecko = new GeckoWebBrowser();
            _gecko.CreateControl();
            _gecko.Enabled = false;
            _gecko.AllowDrop = true;
            //_gecko.NoDefaultContextMenu = true; //禁用右键菜单
            _gecko.AddMessageEventListener("copyToClipboard", copyToClipboard);
            _gecko.AddMessageEventListener("save", save);
            _gecko.AddMessageEventListener("loadFinish", loadFinish);
            _gecko.AddMessageEventListener("watchPointValues", watchPointValues);
            _gecko.AddMessageEventListener("openRunMode", openRunMode);
            _gecko.AddMessageEventListener("writePointValue", writePointValue);
            _gecko.AddMessageEventListener("go", go);
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

        /// <summary>
        /// 当前页面跳转
        /// </summary>
        /// <param name="windowCode">窗口编号</param>
        void go(string windowCode)
        {
            _gecko.Enabled = false;
            _gecko.Navigate($"{Helper.Url}/Home/GetWindowContent?windowCode={windowCode}");
        }
        void writePointValue(string arg)
        {
            try
            {
                string[] pointValue = arg.ToJsonObject<string[]>();
                string pointName = pointValue[0];// /p/a/01
                string addr = pointValue[1];// 点真实路径
                string value = pointValue[2];
                var client = _clients.FirstOrDefault(m => m.WatchingPointNames.Contains(pointName));
                if (client != null)
                {
                    if (client.WriteValue(client.Device.Address, addr, value) == false)
                    {
                        System.Windows.Forms.MessageBox.Show(_gecko, "写入值失败！");
                    }
                    else
                    {
                        jsContext.EvaluateScript($"onReceiveValueFromServer({ (new { addr = addr, value = value }).ToJsonString()})");
                    }
                }
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(_gecko, ex.Message);
            }
        }
        void openRunMode(string arg)
        {
            var doc = new ControlWindowDocument(_parentNode, _dataModel, true);
            MainWindow.Instance.SetActiveDocument(doc);
        }
        void watchPointValues(string jsonStr)
        {
            Task.Run(() =>
            {
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

                        var client = new MyDriverClient(device.Driver.Address, device.Driver.Port.Value);
                        client.WatchingPoints = group.points.Select(m => m.Value<string>("addr")).ToArray();
                        client.WatchingPointNames = group.points.Select(m => m.Value<string>("name")).ToArray();
                        client.Device = device;
                        _clients.Add(client);
                        watchDevice(client);
                    }
                }
                catch (Exception ex)
                {
                    MainWindow.Instance.Dispatcher.Invoke(() =>
                    {
                        MessageBox.Show(MainWindow.Instance, ex.Message);
                    });
                }
            });
        }

        public override void OnClose(ref bool canceled)
        {
            string changed;
            try
            {
                jsContext.EvaluateScript("editor.changed", out changed);
                if (changed == "true" && IsRunMode == false)
                {
                    var dialogResult = MessageBox.Show(MainWindow.Instance, "窗口已修改，是否保存？", "", MessageBoxButton.YesNoCancel);
                    if (dialogResult == MessageBoxResult.Yes)
                    {
                        string info;
                        jsContext.EvaluateScript("editor.getSaveInfo()", out info);
                        var json = info.ToJsonObject<Newtonsoft.Json.Linq.JToken>();
                        if (json.Value<string>("name").IsBlank() || json.Value<string>("code").IsBlank())
                        {
                            MessageBox.Show("请点击左上角设置图标，设置监视画面的名称、编号！");
                            canceled = true;
                            return;
                        }
                        closeAfterSave = true;
                        this.save(info);
                        canceled = true;
                        return;
                    }
                    else if (dialogResult == MessageBoxResult.Cancel)
                    {
                        canceled = true;
                        return;
                    }
                }
            }
            catch (Exception ex)
            {

            }

            foreach (var client in _clients)
            {
                client.Released = true;
                client.NetClient.Close();
            }
            _gecko.Dispose();
            base.OnClose(ref canceled);
        }

        void watchDevice(MyDriverClient client)
        {

            client.NetClient = client.AddPointToWatch(client.Device.Address, client.WatchingPoints, (point, value) =>
            {
                try
                {
                    _gecko.Invoke(new ThreadStart(() =>
                    {
                        jsContext.EvaluateScript($"onReceiveValueFromServer({ (new { addr = point, value = value }).ToJsonString()})");
                    }));
                }
                catch (Exception ex)
                {

                }
            }, (err) =>
            {
                if (client.Released)
                    return;

                Task.Run(() =>
                {
                    Thread.Sleep(2000);
                    watchDevice(client);
                });
            });
        }

        void loadFinish(string msg)
        {
            this.Title = _dataModel.Name;
            _gecko.Enabled = true;
            jsContext = new AutoJSContext(_gecko.Window);

            if (IsRunMode)
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
            Helper.Remote.Invoke<SunRizServer.ControlWindow>("SaveWindowContent", (ret, err) =>
            {

                if (err != null)
                {
                    MessageBox.Show(err);
                }
                else
                {
                    jsContext.EvaluateScript("editor.changed=false");

                    if (_dataModel.id == null)
                    {
                        _dataModel.CopyValue(ret);
                        _parentNode.Nodes.Add(new ControlWindowNode(_dataModel));
                    }
                    else
                    {
                        _dataModel.CopyValue(ret);
                    }
                    _dataModel.ChangedProperties.Clear();
                    this.Title = _dataModel.Name;
                    if (closeAfterSave)
                    {
                        closeAfterSave = false;
                        MainWindow.Instance.CloseDocument(this);
                    }
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

    class MyDriverClient : SunRizDriver.SunRizDriverClient
    {
        public bool Released = false;
        public Way.Lib.NetStream NetClient;
        public string[] WatchingPoints;
        public string[] WatchingPointNames;
        public SunRizServer.Device Device;
        public MyDriverClient(string addr, int port) : base(addr, port)
        {

        }
    }
}
