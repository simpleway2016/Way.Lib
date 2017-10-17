using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

namespace SunRizStudio
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        Models.SolutionNodeCollection Nodes = new Models.SolutionNodeCollection(null);
        Models.SolutionNodeCollection GatewayNodes;
        public MainWindow()
        {
            InitializeComponent();
            initNodes();
            new Form1().Show();
        }

        void initNodes()
        {
            var deviceNode = new Models.SolutionNode()
            {
                Text = "设备(loading...)",
                Icon = "/Images/solution/device.png",
            };
            GatewayNodes = deviceNode.Nodes;           

            deviceNode.ContextMenuItems.Add(new Models.ContextMenuItem() {
                Text = "添加通讯网关...",
                Icon = "/Images/solution/gateway.png",
                ClickHandler = (s, e) => AddGateway(),
            });
            Nodes.Add(deviceNode);


            Nodes.Add(new Models.SolutionNode()
            {
                Text = "组态界面",
                Icon = "/Images/solution/graphic.png",
            });
            tree.ItemsSource = Nodes;


            Helper.Remote.Invoke<Dialogs.GatewayDialog.Gateway[]>("GetGatewayList", (datas, err) => {
                deviceNode.Text = "设备";
                if (err != null)
                    MessageBox.Show(this, err);
                else
                {
                    foreach (var gateway in datas)
                    {
                        GatewayNodes.Add(new Models.GatewayNode(gateway));
                    }
                }
            });
        }

        private void AddGateway()
        {
            Dialogs.GatewayDialog dialog = new Dialogs.GatewayDialog();
            dialog.Owner = this;
            dialog.Title = "添加通讯网关";
            if(dialog.ShowDialog() == true)
            {
                var data = dialog.Data;
                GatewayNodes.Add(new Models.GatewayNode(data));
                GatewayNodes.Last().Parent.IsExpanded = true;
            }
        }

        private void NodeContextMenuItem_Click(object sender, RoutedEventArgs e)
        {
            MenuItem menuitem = sender as MenuItem;
            Models.ContextMenuItem data = menuitem.DataContext as Models.ContextMenuItem;
            if (data.ClickHandler != null)
            {
                data.ClickHandler(sender, e);
            }
        }

        private void TreeViewItem_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            TreeViewItem treeviewitem = sender as TreeViewItem;
            Models.SolutionNode data = treeviewitem.DataContext as Models.SolutionNode;
            if (data.DoublicClickHandler != null)
            {
                data.DoublicClickHandler(this, e);
            }
        }
    }
}
