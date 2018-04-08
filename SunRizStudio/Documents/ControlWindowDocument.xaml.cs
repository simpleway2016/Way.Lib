using Gecko;
using SunRizServer;
using SunRizStudio.Models.Nodes;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
        Window fullScreenWindow = null;
        ControlWindowContainerNode _parentNode;
        internal SunRizServer.ControlWindow _dataModel;
        List<Newtonsoft.Json.Linq.JToken> _AllPoints = new List<Newtonsoft.Json.Linq.JToken>();
        List<MyDriverClient> _clients = new List<MyDriverClient>();

        bool closeAfterSave = false;
        /// <summary>
        /// 页面加载完毕，选中pointName
        /// </summary>
        string _SelectWebControlByPointNameTask = null;
        Dictionary<string, PointAddrInfo> _PointAddress = new Dictionary<string, PointAddrInfo>();

        List<ControlWindow> _visitHistories = new List<ControlWindow>();
        /// <summary>
        /// 是否允许模式
        /// </summary>
        internal bool IsRunMode = false;
        public ControlWindowDocument()
        {
            InitializeComponent();

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="parent">如果不是新建的窗口，此参数可以为空</param>
        /// <param name="dataModel"></param>
        /// <param name="isRunMode"></param>
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
            

            var uri = new Uri(Helper.Config.ServerUrl);
            //often cookies are stored on domain level, so ".google.com", not "www.google.com" (leading dot is important)
            string host = uri.Host.Replace("www", "");
            CookieManager.Add(host, "/", "WayScriptRemoting", RemotingClient.SessionID, false, false, false, Helper.ConvertDateTimeInt(DateTime.Now.AddYears(3)));

            //WayScriptRemoting
            //_gecko.NoDefaultContextMenu = true; //禁用右键菜单
            //_gecko.AddMessageEventListener("copyToClipboard", copyToClipboard);
            //_gecko.AddMessageEventListener("save", save);
            _gecko.AddMessageEventListener("loadFinish", loadFinish);
            //_gecko.AddMessageEventListener("watchPointValues", watchPointValues);
            //_gecko.AddMessageEventListener("openRunMode", openRunMode);
            //_gecko.AddMessageEventListener("writePointValue", writePointValue);
            //_gecko.AddMessageEventListener("go", go);
            //_gecko.AddMessageEventListener("goBack", goBack);
            //_gecko.AddMessageEventListener("open", open);
            //_gecko.AddMessageEventListener("openCode", openCode);
            //_gecko.AddMessageEventListener("fullScreen", fullScreen);
            //_gecko.AddMessageEventListener("exitFullScreen", exitFullScreen);
            //_gecko.AddMessageEventListener("showHistoryWindow", showHistoryWindow);

            winHost.Child = _gecko;
            _gecko.ProgressChanged += Gecko_ProgressChanged;
            _gecko.CreateWindow += Gecko_CreateWindow;
            _gecko.DocumentCompleted += Gecko_DocumentCompleted;
            if (_dataModel.id != null)
            {
                _gecko.Navigate($"{Helper.Config.ServerUrl}/Home/GetWindowContent?windowid={_dataModel.id}");
            }
            else
            {
                _gecko.Navigate($"{Helper.Config.ServerUrl}/editor");
            }
        }

        /// <summary>
        /// 全屏显示
        /// </summary>
        /// <param name="param"></param>
        void fullScreen(string param)
        {
            this.TabItem.Content = null;
            fullScreenWindow = new Window();
            fullScreenWindow.Content = this;
            fullScreenWindow.Owner = MainWindow.Instance;
            fullScreenWindow.GoFullscreen();
            if (this.TabItem != null)
            {
                //是设计模式，不能直接关闭窗口
                fullScreenWindow.Closing += (s, e) =>
                {
                    if (this.TabItem != null)
                    {
                        this.TabItem.Content = this;
                        //设置网页editor为已经退出全屏状态
                        using (var jsContext = new AutoJSContext(_gecko.Window))
                        {
                            jsContext.EvaluateScript("setExitedFullScreen()");
                        }
                    }
                    else
                    {
                        bool canceled = false;
                        this.OnClose(ref canceled);
                        if (canceled)
                        {
                            e.Cancel = true;
                        }
                    }
                };
            }
            fullScreenWindow.ShowDialog();
        }
        void exitFullScreen(string pa)
        {
            var window = this.GetParentByName<Window>(null);
            if (window != null && window.IsFullscreen())
            {               
                window.ExitFullscreen();
                window.Content = null;
                window.Close();
                window = null;
                if (this.TabItem != null)
                {
                    this.TabItem.Content = this;
                }
                else
                {
                    //没有tab，那就是要关闭窗口了
                    bool canceled = false;
                    this.OnClose(ref canceled);
                }
            }
        }

        void alert(string msg)
        {
            var window = this.GetParentByName<Window>(null);
            MessageBox.Show(window, msg, "", MessageBoxButton.OK, MessageBoxImage.Exclamation);
        }

        /// <summary>
        /// 返回上一个页面
        /// </summary>
        void goBack(string p)
        {
            if(_visitHistories.Count > 0)
            {
                var win = _visitHistories.Last();
                _visitHistories.RemoveAt(_visitHistories.Count - 1);

                foreach (var client in _clients)
                {
                    client.Released = true;
                    client.NetClient.Close();
                }
                _clients.Clear();

                _dataModel = win;
                _gecko.Enabled = false;
                _gecko.Navigate($"{Helper.Config.ServerUrl}/Home/GetWindowContent?windowid={win.id}");
            }
        }

        /// <summary>
        /// 当前页面跳转
        /// </summary>
        /// <param name="windowCode">窗口编号</param>
        void go(string windowCode)
        {
            Helper.Remote.Invoke<ControlWindow>("GetWindowInfo", (win, err) => {
                if (err != null)
                {
                    MessageBox.Show(this.GetParentByName<Window>(null), err);
                }
                else
                {
                    this._visitHistories.Add(_dataModel);

                    foreach (var client in _clients)
                    {
                        client.Released = true;
                        client.NetClient.Close();
                    }
                    _clients.Clear();

                    _dataModel = win;
                    _gecko.Enabled = false;
                    
                    _gecko.Navigate($"{Helper.Config.ServerUrl}/Home/GetWindowContent?windowCode={windowCode}");
                }
            }, 0, windowCode);
            
        }
        //脚本直接编辑
        void openCode(string p)
        {
            Helper.Remote.Invoke<string>("GetWindowScript", (script, err) => {
                if (err != null)
                {
                    MessageBox.Show(this.GetParentByName<Window>(null), err);
                }
                else
                {
                    Dialogs.TextEditor frm = new Dialogs.TextEditor(script, _dataModel.id.Value);
                    frm.Title = this.Title;
                    frm.ShowDialog();
                }
            }, _dataModel.id);
        }
        public void showHistoryWindow(string pointNames)
        {
            new Dialogs.HistoryWindow(pointNames).Show();
        }
            public void SelectWebControlByPointName(string pointName)
        {
            if(_gecko.Enabled == false )
            {
                _SelectWebControlByPointNameTask = pointName;
                return;
            }
            try
            {
                string info;
                using (var jsContext = new AutoJSContext(_gecko.Window))
                {
                    jsContext.EvaluateScript($"editor.selectWebControlByPointName({pointName.ToJson()})", out info);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(this.GetParentByName<Window>(null), ex.Message);
            }
        }

        /// <summary>
        /// 打开窗口
        /// </summary>
        /// <param name="windowCode">窗口编号</param>
        void open(string windowCode)
        {
            Helper.Remote.Invoke<ControlWindow>("GetWindowInfo", (win, err) => {
                if(err != null)
                {
                    MessageBox.Show(this.GetParentByName<Window>(null), err);
                }
                else
                {
                    ControlWindowDocument doc = new ControlWindowDocument(null, win, true);
                    doc.Title = win.Name;
                    Window window = new Window();
                    window.Content = doc;
                    if (win.windowWidth != null)
                    {
                        window.Width = win.windowWidth.Value;
                    }
                    if (win.windowHeight != null)
                    {
                        window.Height = win.windowHeight.Value;
                    }
                    if(win.windowWidth == null && win.windowHeight == null)
                    {
                        window.WindowState = WindowState.Maximized;
                    }
                    else if (win.windowWidth != null && win.windowHeight != null)
                    {
                        window.ResizeMode = ResizeMode.NoResize;
                    }
                    window.Closed += (s, e) => {
                        bool c = false;
                        doc.OnClose(ref c);
                    };
                    window.Show();
                }
            }, 0 , windowCode);
        }
        void writePointValue(string arg)
        {
            try
            {
                string[] pointValue = arg.ToJsonObject<string[]>();
                string pointName = pointValue[0];// /p/a/01
                string addr = pointValue[1];// 点真实路径
                if(string.IsNullOrEmpty(addr))
                {
                    //查询点真实地址
                    try
                    {
                        if (_PointAddress.ContainsKey(pointName) == false)
                        {
                            _PointAddress[pointName] = Helper.Remote.InvokeSync<PointAddrInfo>("GetPointAddr", pointName);
                        }

                        addr = _PointAddress[pointName].addr;
                    }
                    catch
                    {
                        throw new Exception($"无法获取点“{pointName}”真实地址");
                    }
                }
                string value = pointValue[2];
                var client = _clients.FirstOrDefault(m => m.WatchingPointNames.Contains(pointName));
                if (client == null)
                {
                    //构造MyDriverClient
                    var device = Helper.Remote.InvokeSync<SunRizServer.Device>("GetDeviceAndDriver", _PointAddress[pointName].deviceId);
                    client = new MyDriverClient(device.Driver.Address, device.Driver.Port.Value);
                    client.Device = device;
                    client.WatchingPoints.Add(addr);
                    client.WatchingPointNames.Add(pointName);
                    _clients.Add(client);
                }

                if (client != null)
                {
                    var pointObj = _AllPoints.FirstOrDefault(m => m.Value<string>("addr") == addr);
                    if (pointObj != null)
                    {
                        //转换数值
                        value = SunRizDriver.Helper.GetRealValue(pointObj, value).ToString();
                    }

                    if (client.WriteValue(client.Device.Address, addr, value) == false)
                    {
                        System.Windows.Forms.MessageBox.Show(_gecko, "写入值失败！");
                    }
                    else
                    {
                        using (var jsContext = new AutoJSContext(_gecko.Window))
                        {
                            jsContext.EvaluateScript($"onReceiveValueFromServer({ (new { addr = addr, value = value }).ToJsonString()})");
                        }
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
            fullScreenWindow = new Window();
            fullScreenWindow.Content = doc;
            fullScreenWindow.Owner = this.GetParentByName<Window>(null);            
            fullScreenWindow.GoFullscreen();
            fullScreenWindow.Closed += (s, e) => {
                bool cancel = false;
                doc.OnClose(ref cancel);
            };
            fullScreenWindow.ShowDialog();

        }
        void watchPointValues(string jsonStr)
        {
            foreach (var client in _clients)
            {
                client.Released = true;
                client.NetClient.Close();
            }
            _clients.Clear();

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
                        _AllPoints.AddRange(group.points);

                        var device = Helper.Remote.InvokeSync<SunRizServer.Device>("GetDeviceAndDriver", group.deviceId);

                        var client = new MyDriverClient(device.Driver.Address, device.Driver.Port.Value);
                        client.WatchingPoints = group.points.Select(m => m.Value<string>("addr")).ToList();
                        client.WatchingPointNames = group.points.Select(m => m.Value<string>("name")).ToList();
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

        public override bool HasChanged()
        {
            string changed;
            try
            {
                using (var jsContext = new AutoJSContext(_gecko.Window))
                {
                    jsContext.EvaluateScript("editor.changed", out changed);
                }
                if (changed == "true" && IsRunMode == false)
                {
                    return true;
                }
            }
            catch (Exception ex)
            {

            }
            return false;
        }
        public override void Undo()
        {
            try
            {
                string info;
                using (var jsContext = new AutoJSContext(_gecko.Window))
                {
                    jsContext.EvaluateScript("editor.undo()", out info);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(this.GetParentByName<Window>(null), ex.Message);
            }
        }
        public override void Redo()
        {
            try
            {
                string info;
                using (var jsContext = new AutoJSContext(_gecko.Window))
                {
                    jsContext.EvaluateScript("editor.redo()", out info);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(this.GetParentByName<Window>(null), ex.Message);
            }
        }
        public override void Cut()
        {
            try
            {
                string info;
                //直接调用editor.cut()会出现the operation is insecure错误
                using (var jsContext = new AutoJSContext(_gecko.Window))
                {
                    jsContext.EvaluateScript("btnCut.click()", out info);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(this.GetParentByName<Window>(null), ex.Message);
            }
        }
        public override void Copy()
        {
            try
            {
                string info;
                //直接调用editor.copy()会出现the operation is insecure错误
                using (var jsContext = new AutoJSContext(_gecko.Window))
                {
                    jsContext.EvaluateScript("btnCopy.click()", out info);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(this.GetParentByName<Window>(null), ex.Message);
            }
        }
        public override void Paste()
        {
            try
            {
                string info;
                //直接调用editor.paste()会出现the operation is insecure错误
                using (var jsContext = new AutoJSContext(_gecko.Window))
                {
                    jsContext.EvaluateScript("btnPaste.click()", out info);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(this.GetParentByName<Window>(null), ex.Message);
            }
        }
        public override void SelectAll()
        {
            try
            {
                string info;
                using (var jsContext = new AutoJSContext(_gecko.Window))
                {
                    jsContext.EvaluateScript("editor.selectAll()", out info);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(this.GetParentByName<Window>(null), ex.Message);
            }
        }
        public override bool Save()
        {
            try
            {
                string info;
                using (var jsContext = new AutoJSContext(_gecko.Window))
                {
                    jsContext.EvaluateScript("editor.getSaveInfo()", out info);
                }
                var json = info.ToJsonObject<Newtonsoft.Json.Linq.JToken>();
                if (json.Value<string>("name").IsBlank() || json.Value<string>("code").IsBlank())
                {
                    MessageBox.Show(this.GetParentByName<Window>(null), "请点击左上角设置图标，设置监视画面的名称、编号！");
                    return false;
                }

                Helper.Remote.InvokeSync<SunRizServer.ControlWindow>("SaveWindowContent", _dataModel, json);

                return true;
            }
            catch(Exception ex)
            {
                MessageBox.Show(this.GetParentByName<Window>(null), ex.Message);
                return false;
            }
        }

        public override void OnClose(ref bool canceled)
        {
            string changed;
            try
            {
                using (var jsContext = new AutoJSContext(_gecko.Window))
                {
                    jsContext.EvaluateScript("editor.changed", out changed);
                    if (changed == "true" && IsRunMode == false)
                    {
                        var dialogResult = MessageBox.Show(MainWindow.Instance, $"“{this.Title}”已修改，是否保存？", "", MessageBoxButton.YesNoCancel);
                        if (dialogResult == MessageBoxResult.Yes)
                        {
                            string info;
                            jsContext.EvaluateScript("editor.getSaveInfo()", out info);
                            var json = info.ToJsonObject<Newtonsoft.Json.Linq.JToken>();
                            if (json.Value<string>("name").IsBlank() || json.Value<string>("code").IsBlank())
                            {
                                MessageBox.Show(this.GetParentByName<Window>(null), "请点击左上角设置图标，设置监视画面的名称、编号！");
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

            client.NetClient = client.AddPointToWatch(client.Device.Address, client.WatchingPoints.ToArray(), (point, value) =>
            {
                try
                {
                    var pointObj = _AllPoints.FirstOrDefault(m => m.Value<string>("addr") == point);
                    if(pointObj != null)
                    {
                        //转换数值
                        value = SunRizDriver.Helper.Transform(pointObj, value);
                    }
                    Debug.WriteLine($"addr:{point} value:{value}");
                    _gecko.Invoke(new ThreadStart(() =>
                    {
                        using (var jsContext = new AutoJSContext(_gecko.Window))
                        {
                            jsContext.EvaluateScript($"onReceiveValueFromServer({ (new { addr = point, value = value }).ToJsonString()})");
                        }
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
            _gecko.DomClick -= _gecko_DomClick;
            _gecko.DomClick += _gecko_DomClick;
            this.Title = _dataModel.Name;
            try
            {
                if (IsRunMode)
                {
                    //运行状态，设置窗口的标题文字
                    this.GetParentByName<Window>(null).Title = _dataModel.Name;
                }
            }
            catch { }
            _gecko.Enabled = true;

            if(_SelectWebControlByPointNameTask != null)
            {
                this.SelectWebControlByPointName(_SelectWebControlByPointNameTask);
                _SelectWebControlByPointNameTask = null;
            }
            if (IsRunMode)
            {
                try
                {
                    using (var jsContext = new AutoJSContext(_gecko.Window))
                    {
                        jsContext.EvaluateScript("run()");
                    }
                }
                catch(Exception ex)
                {
                    MessageBox.Show( this.GetParentByName<Window>(null), ex.Message);
                }
            }
        }

        private void _gecko_DomClick(object sender, DomMouseEventArgs e)
        {
            var ele = e.Target.CastToGeckoElement();
            if (ele.GetAttribute("id") == "btnMethod")
            {
                string methodName = ele.GetAttribute("value");
                string param = _gecko.Document.GetElementById("hidMethodParam").GetAttribute("value");

                try
                {
                    var method = this.GetType().GetMethod(methodName, System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
                    method.Invoke(this, new object[] { param });
                }
                catch (Exception ex)
                {
                    MessageBox.Show(this.GetParentByName<Window>(null), ex.Message);
                }
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
                    MessageBox.Show(this.GetParentByName<Window>(null), err);
                }
                else
                {
                    using (var jsContext = new AutoJSContext(_gecko.Window))
                    {
                        jsContext.EvaluateScript("editor.changed=false");
                    }

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
           
        }


        private void Gecko_CreateWindow(object sender, GeckoCreateWindowEventArgs e)
        {
            return;

            e.InitialHeight = 500;
            e.InitialWidth = 500;
            e.Cancel = true;
            try
            {
                var url = System.Web.HttpUtility.UrlDecode(e.Uri.Substring("http://invoker/".Length));
                var json = url.ToJsonObject<Newtonsoft.Json.Linq.JObject>();
                var method = this.GetType().GetMethod(json.Value<string>("name"), System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
                method.Invoke(this, new object[] { json.Value<string>("param") });
            }
            catch(Exception ex)
            {
                MessageBox.Show(this.GetParentByName<Window>(null), ex.Message);
            }
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
        public List<string> WatchingPoints = new List<string>();
        public List<string> WatchingPointNames = new List<string>();
        public SunRizServer.Device Device;
        public MyDriverClient(string addr, int port) : base(addr, port)
        {

        }
    }

    class PointAddrInfo
    {
        public string addr;
        public int deviceId;
    }
}
