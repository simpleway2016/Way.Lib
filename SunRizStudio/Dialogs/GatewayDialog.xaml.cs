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
using SunRizServer;
namespace SunRizStudio.Dialogs
{
    /// <summary>
    /// GatewayDialog.xaml 的交互逻辑
    /// </summary>
    public partial class GatewayDialog : Window
    {
      
        public CommunicationDriver Data;
        public GatewayDialog()
        {
            InitializeComponent();
            Data = new CommunicationDriver();
            this.DataContext = Data;
        }
        public GatewayDialog(CommunicationDriver data)
        {
            InitializeComponent();
            Data = data;
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
                    this.Data.Status = CommunicationDriver_StatusEnum.Online;
                    this.Data.id = Helper.Remote.InvokeSync<int>("UpdateGateway", this.Data);

                    //清空对象属性的变化记录
                    this.Data.ChangedProperties.Clear();
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
