using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace SunRizStudio.Dialogs
{
    /// <summary>
    /// DeviceDialog.xaml 的交互逻辑
    /// </summary>
    public partial class DeviceDialog : Window
    {
        class PropertyItem
        {
            public string Name;
            public Func<string> GetValueFunc;
        }

        List<PropertyItem> _propertyItems = new List<PropertyItem>();
        public SunRizServer.Device Device;
        SunRizDriver.SunRizDriverClient _curClient;
        public DeviceDialog()
        {
            InitializeComponent();

            //绑定网关列表
            bindGatewayList();

            this.Loaded += DeviceDialog_Loaded;
        }

        private void DeviceDialog_Loaded(object sender, RoutedEventArgs e)
        {
            this.Title += $"配置“{this.Device.Name}”";
            if(this.Device.DriverID != null)
            {
                _cmbGateway.SelectedValue = this.Device.DriverID;
            }
        }

        void bindGatewayList()
        {
            Helper.Remote.Invoke<SunRizServer.CommunicationDriver[]>("GetGatewayList", (datas, err) =>
            {
                if (err != null)
                    MessageBox.Show(MainWindow.Instance, err);
                else
                {
                    _cmbGateway.ItemsSource = datas;
                }
            });
        }

        private void btnOk_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                //获取控制器属性值
                Dictionary<string, string> dict = new Dictionary<string, string>();
                foreach (var proItem in _propertyItems)
                {
                    if (string.IsNullOrEmpty(proItem.GetValueFunc()))
                        throw new Exception($"请输入{proItem.Name}");
                    dict[proItem.Name] = proItem.GetValueFunc();
                }

                //翻译控制器地址
                this.Device.DriverID = ((SunRizServer.CommunicationDriver)_cmbGateway.SelectedItem).id;
                this.Device.Address = _curClient.GetDeviceAddress(dict);
                this.Device.AddrSetting = Newtonsoft.Json.JsonConvert.SerializeObject(dict);

                this.Cursor = Cursors.Hand;
                Helper.Remote.Invoke<SunRizServer.CommunicationDriver[]>("UpdateDevice", (datas, err) =>
                {
                    this.Cursor = null;
                    if (err != null)
                        MessageBox.Show(MainWindow.Instance, err);
                    else
                    {
                        this.DialogResult = true;
                    }
                } , this.Device);
            }
            catch(Exception ex)
            {
                MessageBox.Show(this, ex.Message);
            }
        }

        private void _cmbGateway_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var gateway = _cmbGateway.SelectedItem as SunRizServer.CommunicationDriver;

            //获取设备属性
            try
            {
                _curClient = new SunRizDriver.SunRizDriverClient(gateway.Address, gateway.Port.GetValueOrDefault());

                //获取属性列表
                var propertyNames = _curClient.GetDeviceProperties();
                _panelProperties.Children.Clear();
                _propertyItems = new List<PropertyItem>();
                Newtonsoft.Json.Linq.JToken curJsonObj = null;
                if(this.Device.AddrSetting != null)
                {
                    //转换Device对象的json属性JToken
                    curJsonObj = (Newtonsoft.Json.Linq.JToken)Newtonsoft.Json.JsonConvert.DeserializeObject(this.Device.AddrSetting);
                }

                foreach ( var proName in propertyNames )
                {
                    string curProName = proName;
                    Newtonsoft.Json.Linq.JToken jsonObj = null;
                    bool isEnumDevice = false;
                    string enum_addressProperty = null;
                    if(curProName.Contains("{"))
                    {
                        //判断是否包含json属性
                        int index = proName.IndexOf("{");
                        string json = curProName.Substring(index);
                        curProName = curProName.Substring(0, index);
                        jsonObj = (Newtonsoft.Json.Linq.JToken)Newtonsoft.Json.JsonConvert.DeserializeObject(json);
                        isEnumDevice = jsonObj.Value<bool>("isEnumDevice");
                        enum_addressProperty = jsonObj.Value<string>("addressProperty");
                    }

                    //生成属性输入框
                    Func<string> func;
                    var txt = new TextBlock();
                    txt.Text = $"{curProName}：";                  
                    txt.Margin = new Thickness(5, 5, 5, 0);
                    _panelProperties.Children.Add(txt);

                    if (isEnumDevice && enum_addressProperty != null)
                    {
                        //此属性可以下拉选择
                        var valueBox = new ComboBox();
                        valueBox.IsEditable = true;
                        valueBox.Height = 22;
                        valueBox.Margin = new Thickness(5, 5, 5, 0);
                        valueBox.GotFocus += (s, e2) => {
                            if(valueBox.Items.Count == 0)
                            {
                                try
                                {
                                    var addressItem = _propertyItems.FirstOrDefault(m => m.Name == enum_addressProperty);
                                    var value = addressItem.GetValueFunc();
                                    if (!string.IsNullOrEmpty(value))
                                    {
                                        valueBox.ItemsSource = _curClient.EnumDevice(value);
                                    }
                                }
                                catch
                                {

                                }
                            }
                        };
                        func = () =>
                        {
                            return valueBox.Text.Trim();
                        };
                        if (curJsonObj != null)
                        {
                            valueBox.Text = curJsonObj.Value<string>(curProName);
                        }
                        _panelProperties.Children.Add(valueBox);
                    }
                    else
                    {
                        var valueBox = new TextBox();
                        valueBox.Height = 22;
                        valueBox.Margin = new Thickness(5, 5, 5, 0);
                        func = () =>
                        {
                            return valueBox.Text.Trim();
                        };
                        if (jsonObj != null)
                        {
                            //赋默认值
                            try
                            {
                                valueBox.Text = jsonObj.Value<string>("defaultValue");
                            }
                            catch
                            {

                            }
                        }
                        if (curJsonObj != null)
                        {
                            valueBox.Text = curJsonObj.Value<string>(curProName);
                        }
                        _panelProperties.Children.Add(valueBox);
                    }
                   
                    _propertyItems.Add(new PropertyItem() {
                        Name = curProName,
                        GetValueFunc = func
                    });
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(MainWindow.Instance, ex.Message);
            }
        }
    }
}
