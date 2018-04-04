using SunRizServer;
using SunRizStudio.Models;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SunRizStudio.Documents
{
    /// <summary>
    /// LogStorageSettingDocument.xaml 的交互逻辑
    /// </summary>
    public partial class LogStorageSettingDocument : BaseDocument
    {
        public LogStorageSettingDocument()
        {
            InitializeComponent();
            this.Title = "系统日志设置";
            try
            {
                Helper.Remote.Invoke<SunRizServer.SystemSetting>("GetSystemSetting", (data, err) => {
                    if (err != null)
                    {
                        MessageBox.Show(MainWindow.Instance, err);
                    }
                    else
                    {
                        this.DataContext = data;
                    }
                });
            }
            catch
            {

            }
        }
     
        private void btnOK_Click(object sender, RoutedEventArgs e)
        {
            btnApply_Click(null, null);
            MainWindow.Instance.CloseDocument(this);
        }

        private void btnApply_Click(object sender, RoutedEventArgs e)
        {
            Helper.Remote.Invoke<int>("UpdateSystemSetting", (ret, err) => {
                this.Cursor = null;
                if (err != null)
                {
                    MessageBox.Show(MainWindow.Instance, err);
                }
                else
                {

                }
            }, this.DataContext);
        }



        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            //关闭当前document
            MainWindow.Instance.CloseDocument(this);
        }

        private void btnRefresh_Click(object sender, RoutedEventArgs e)
        {
            Helper.Remote.Invoke<SunRizServer.SystemSetting>("GetSystemSetting", (data, err) => {
                if (err != null)
                {
                    MessageBox.Show(MainWindow.Instance, err);
                }
                else
                {
                    this.DataContext = data;
                }
            });
        }
    }
}
