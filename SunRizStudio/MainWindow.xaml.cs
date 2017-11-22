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
using SunRizStudio.Models.Nodes;
using SunRizStudio.Models;

namespace SunRizStudio
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public static MainWindow Instance;
        Models.SolutionNodeCollection Nodes = new Models.SolutionNodeCollection(null);
        public MainWindow()
        {
            Instance = this;
            InitializeComponent();

            // 加载树形节点
            initNodes();
            //new Form1().Show();
        }

        void initNodes()
        {
            
            var systemNode = new SystemNode() {
                Text = "系统",
                IsExpanded = true
            };
            Nodes.Add(systemNode);

            // 绑定树形节点
            tree.ItemsSource = Nodes;
        }

       
        /// <summary>
        /// 触发树形节点的右键菜单，被点击后的相应事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void NodeContextMenuItem_Click(object sender, RoutedEventArgs e)
        {
            e.Handled = true;
            MenuItem menuitem = sender as MenuItem;
            Models.ContextMenuItem data = menuitem.DataContext as Models.ContextMenuItem;
            if (data.ClickHandler != null)
            {
                data.ClickHandler(sender, e);
            }
        }

        /// <summary>
        /// 树形节点双击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TreeViewItem_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            e.Handled = true;
            TreeViewItem treeviewitem = sender as TreeViewItem;
            if (((FrameworkElement)e.OriginalSource).GetParentByName<TreeViewItem>(null) != treeviewitem)
                return;
            Models.SolutionNode data = treeviewitem.DataContext as Models.SolutionNode;
            if (data.DoublicClickHandler != null)
            {
                data.DoublicClickHandler(sender, e);
            }
        }

        public void SetActiveDocument(Documents.BaseDocument document)
        {
            TabItem newItem = new TabItem();
            newItem.Loaded += (s, e) => {
                newItem.GetChildByName<Button>("closeIcon").Click += (s2, e2) => {
                    bool cancel = false;
                    document.OnClose(ref cancel);
                    if(!cancel)
                    {
                        documentContainer.Items.Remove(newItem);
                    }
                };
            };
            newItem.Header = document.Header;
            newItem.Style = (Style)App.Current.Resources["TabItemDocStyle"];
            newItem.Content = document;
            documentContainer.Items.Add(newItem);
            documentContainer.SelectedItem = newItem;
        }
    }
}
