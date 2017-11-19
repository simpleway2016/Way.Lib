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
    public partial class AnalogPointDocument : UserControl
    {
        PointDocumentController _controller;
       
        public AnalogPointDocument()
        {
            InitializeComponent();
        }
        internal AnalogPointDocument(Device device, SolutionNode parent, DevicePoint dataModel , int folderId)
        {
           
            InitializeComponent();

            _controller = new PointDocumentController(this,gridProperty, device , DevicePoint_TypeEnum.Analog , parent , dataModel , folderId);
            
        }

        private void btnOK_Click(object sender, RoutedEventArgs e)
        {
            _controller.saveToServer(true);
        }

        private void btnApply_Click(object sender, RoutedEventArgs e)
        {
           _controller.saveToServer(false);
        }

       

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            //关闭当前document
            MainWindow.Instance.SetActiveDocument(null);
        }

        private void btnRefresh_Click(object sender, RoutedEventArgs e)
        {
            _controller.refresh();
        }
    }
}
