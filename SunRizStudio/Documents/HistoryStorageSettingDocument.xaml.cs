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
    /// DigitalPointDocument.xaml 的交互逻辑
    /// </summary>
    public partial class HistoryStorageSettingDocument : BaseDocument
    {
        public HistoryStorageSettingDocument()
        {
            InitializeComponent();
            this.Title = "历史收集设置";
        }
     
        private void btnOK_Click(object sender, RoutedEventArgs e)
        {
            
        }

        private void btnApply_Click(object sender, RoutedEventArgs e)
        {
           
        }



        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            //关闭当前document
            MainWindow.Instance.CloseDocument(this);
        }

        private void btnRefresh_Click(object sender, RoutedEventArgs e)
        {
            
        }
    }
}
