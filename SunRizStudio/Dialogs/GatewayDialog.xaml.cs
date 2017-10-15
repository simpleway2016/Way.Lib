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
    /// GatewayDialog.xaml 的交互逻辑
    /// </summary>
    public partial class GatewayDialog : Window
    {
        public class Gateway : Models.DataItem
        {

            /// <summary>
            /// 
            /// </summary>
            public Gateway()
            {
            }


            System.Nullable<Int32> _id;
            /// <summary>
            /// 
            /// </summary>
            public virtual System.Nullable<Int32> id
            {
                get
                {
                    return this._id;
                }
                set
                {
                    if ((this._id != value))
                    {
                        this.SendPropertyChanging("id", this._id, value);
                        this._id = value;
                        this.SendPropertyChanged("id");

                    }
                }
            }

            String _Name;
            /// <summary>
            /// 名称
            /// </summary>
            public virtual String Name
            {
                get
                {
                    return this._Name;
                }
                set
                {
                    if ((this._Name != value))
                    {
                        this.SendPropertyChanging("Name", this._Name, value);
                        this._Name = value;
                        this.SendPropertyChanged("Name");

                    }
                }
            }

            String _Address;
            /// <summary>
            /// 地址
            /// </summary>
            public virtual String Address
            {
                get
                {
                    return this._Address;
                }
                set
                {
                    if ((this._Address != value))
                    {
                        this.SendPropertyChanging("Address", this._Address, value);
                        this._Address = value;
                        this.SendPropertyChanged("Address");

                    }
                }
            }

            System.Nullable<Int32> _Port;
            /// <summary>
            /// 端口
            /// </summary>
            public virtual System.Nullable<Int32> Port
            {
                get
                {
                    return this._Port;
                }
                set
                {
                    if ((this._Port != value))
                    {
                        this.SendPropertyChanging("Port", this._Port, value);
                        this._Port = value;
                        this.SendPropertyChanged("Port");

                    }
                }
            }

            int _Status;
            /// <summary>
            /// 状态
            /// </summary>
            public virtual int Status
            {
                get
                {
                    return this._Status;
                }
                set
                {
                    if ((this._Status != value))
                    {
                        this.SendPropertyChanging("Status", this._Status, value);
                        this._Status = value;
                        this.SendPropertyChanged("Status");

                    }
                }
            }

            System.Nullable<Boolean> _SupportEnumDevice = false;
            /// <summary>
            /// 
            /// </summary>
            public virtual System.Nullable<Boolean> SupportEnumDevice
            {
                get
                {
                    return this._SupportEnumDevice;
                }
                set
                {
                    if ((this._SupportEnumDevice != value))
                    {
                        this.SendPropertyChanging("SupportEnumDevice", this._SupportEnumDevice, value);
                        this._SupportEnumDevice = value;
                        this.SendPropertyChanged("SupportEnumDevice");

                    }
                }
            }

            System.Nullable<Boolean> _SupportEnumPoints = false;
            /// <summary>
            /// 
            /// </summary>
            public virtual System.Nullable<Boolean> SupportEnumPoints
            {
                get
                {
                    return this._SupportEnumPoints;
                }
                set
                {
                    if ((this._SupportEnumPoints != value))
                    {
                        this.SendPropertyChanging("SupportEnumPoints", this._SupportEnumPoints, value);
                        this._SupportEnumPoints = value;
                        this.SendPropertyChanged("SupportEnumPoints");

                    }
                }
            }
        }
    
    public Gateway Data;
        bool isModify = false;
        public GatewayDialog()
        {
            InitializeComponent();
            Data = new Gateway();
            this.DataContext = Data;
        }
        public GatewayDialog(Gateway data)
        {
            InitializeComponent();
            Data = data;
            isModify = true;
            this.DataContext = Data;
        }
        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }

        private async void btnOk_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                this.Cursor = Cursors.Wait;
                SunRizDriver.SunRizDriverClient client = new SunRizDriver.SunRizDriverClient(Data.Address, Data.Port.Value);
                await Task.Run(() =>
                {
                    if (client.CheckDriver() == false)
                    {
                        throw new Exception("无法连接此网关");
                    }
                    var serverInfo = client.GetServerInfo();
                    this.Data.Name = serverInfo.Name;
                    this.Data.SupportEnumDevice = serverInfo.SupportEnumDevice;
                    this.Data.SupportEnumPoints = serverInfo.SupportEnumPoints;
                    this.Data.Status = 1;
                    this.Data.id = Helper.Remote.InvokeSync<int>("AddGateway", this.Data);
                });
                
                this.DialogResult = true;
            }
            catch(Exception ex)
            {
                MessageBox.Show(this, ex.Message);

            }
            finally
            {
                this.Cursor = null;
            }
        }
    }
}
