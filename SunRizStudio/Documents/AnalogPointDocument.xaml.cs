using SunRizServer;
using SunRizStudio.Models;
using SunRizStudio.Models.Nodes;
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
    /// AnalogPointDocument.xaml 的交互逻辑
    /// </summary>
    public partial class AnalogPointDocument : BaseDocument
    {
        internal PointDocumentController Controller;
        DevicePoint _dataModel;
        public AnalogPointDocument()
        {
            InitializeComponent();
        }
        internal AnalogPointDocument(Device device, SolutionNode parent, DevicePoint dataModel , int folderId)
        {
            _dataModel = dataModel;
            InitializeComponent();
            List<object> items = new List<object>();
            for(double i = 1; i <= 100; i ++)
            {
                items.Add(new {
                    text = i + "%",
                    value = i
                });
            }
            cmd_ValueRelativeChangeOptions.ItemsSource = items;

            items = new List<object>();
            for (double i = 1; i <= 100; i++)
            {
                items.Add(new
                {
                    text = i + "%",
                    value = i
                });
            }
            cmd_Percent.ItemsSource = items;

            items = new List<object>();
            for (double i = 1; i <= 120; i++)
            {
                items.Add(new
                {
                    text = i + "秒",
                    value = i
                });
            }
            cmd_ValueOnTimeChangeOptions.ItemsSource = items;

            items = new List<object>();
            for (double i = 1; i <= 120; i++)
            {
                items.Add(new
                {
                    text = i + "秒",
                    value = i
                });
            }
            cmd_ChangeCycle.ItemsSource = items;

            //报警组
            items = new List<object>();
            for (int i = 0; i < 16; i++)
            {
                items.Add(new
                {
                    text = (i + 1).ToString(),
                    value = (1 << i)
                });
            }
            cmd_AlarmGroup.ItemsSource = items;

            //安全区
            items = new List<object>();
            for (int i = 0; i < 26; i++)
            {
                items.Add(new
                {
                    text = ((char)(((int)('A')) + i)).ToString(),
                    value = (1 << i)
                });
            }
            cmbSafeArea.ItemsSource = items;

            Controller = new PointDocumentController(this,gridProperty, device , DevicePoint_TypeEnum.Analog , parent , dataModel , folderId);
            this.Title = Controller.OriginalModel.Desc;

            
        }

     
        private void btnOK_Click(object sender, RoutedEventArgs e)
        {
            Controller.saveToServer(true);
        }

        private void btnApply_Click(object sender, RoutedEventArgs e)
        {
           Controller.saveToServer(false);
        }

       

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            //关闭当前document
            MainWindow.Instance.CloseDocument(this);
        }

        private void btnRefresh_Click(object sender, RoutedEventArgs e)
        {
            Controller.refresh();
        }
    }
}
