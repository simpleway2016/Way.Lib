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
using System.ComponentModel;
using System.Text.RegularExpressions;
using SunRizStudio.Listeners;

namespace SunRizStudio
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        Dialogs.AlarmWindow _alarmWindow;
        public static MainWindow Instance;
        Models.SolutionNodeCollection Nodes = new Models.SolutionNodeCollection(null);

        Dialogs.SearchResultDialog _SearchResultDialog;
        Dialogs.SearchResultDialog SearchResultDialog
        {
            get
            {
                return _SearchResultDialog ?? (_SearchResultDialog = new Dialogs.SearchResultDialog());
            }
        }
        public MainWindow()
        {
            Instance = this;
            InitializeComponent();

            //为了服务器session不过时，启动一个listener
            AlarmListener.Start(this.Dispatcher);

            try
            {
                System.IO.Directory.Delete($"{AppDomain.CurrentDomain.BaseDirectory}temp" , true);
            }
            catch
            {

            }
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
            FrameworkElement sourceElement = (FrameworkElement)sender;
            if (sender is TreeViewItem)
            {
                TreeViewItem treeviewitem = sender as TreeViewItem;
                if (((FrameworkElement)e.OriginalSource).GetParentByName<TreeViewItem>(null) != treeviewitem)
                    return;
                sourceElement = treeviewitem;
            }
            Models.SolutionNode data = sourceElement.DataContext as Models.SolutionNode;
            if (data.DoublicClickHandler != null)
            {
                data.DoublicClickHandler(sender, e);
            }
        }

        public void CloseDocument(Documents.BaseDocument document)
        {
            document.TabItem.Close();
        }

        public void SetActiveDocument(Documents.BaseDocument document)
        {
            var newItem = new Documents.DocTabItem(document);
            documentContainer.Items.Add(newItem);
            documentContainer.SelectedItem = newItem;           
        }

        private void TreeViewItem_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
           
            FrameworkElement sourceElement = (FrameworkElement)sender;
            if (sender is TreeViewItem)
            {
                TreeViewItem treeviewitem = sender as TreeViewItem;
                if (!(e.OriginalSource is TextBlock))
                    return;
                if (((FrameworkElement)e.OriginalSource).GetParentByName<TreeViewItem>(null) != treeviewitem)
                    return;
            }
            else
            {
                //list里面，因为devicePoint在MouseDown会实现drop，所以MouseDoubleClick无法触发，只能这里触发了
                if (e.ClickCount == 2)
                {
                    e.Handled = true;
                    TreeViewItem_MouseDoubleClick(sender, e);
                    return;
                }
            }
            Models.SolutionNode data = sourceElement.DataContext as Models.SolutionNode;
            data.MouseDown(sender, e);
        }

        private void menuExitApp_Click(object sender, RoutedEventArgs e)
        {
            
            this.Close();
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            foreach (Documents.DocTabItem tab in documentContainer.Items)
            {
                if (tab.Document.HasChanged())
                {
                    var dialogResult = MessageBox.Show(MainWindow.Instance, "“" + tab.Document.Title + "”已修改，是否保存？", "", MessageBoxButton.YesNoCancel);
                    if (dialogResult == MessageBoxResult.Yes)
                    {
                        if (!tab.Document.Save())
                        {
                            e.Cancel = true;
                            return;
                        }
                    }
                    else if(dialogResult == MessageBoxResult.Cancel)
                    {
                        e.Cancel = true;
                        return;
                    }
                }
            }
            base.OnClosing(e);
        }

        private void menuUndo_Click(object sender, RoutedEventArgs e)
        {
            var doc =  (Documents.BaseDocument)documentContainer.SelectedContent;
            doc.Undo();
        }

        private void menuRedo_Click(object sender, RoutedEventArgs e)
        {
            var doc = (Documents.BaseDocument)documentContainer.SelectedContent;
            doc.Redo();
        }

        private void menuCut_Click(object sender, RoutedEventArgs e)
        {
            var doc = (Documents.BaseDocument)documentContainer.SelectedContent;
            doc.Cut();
        }

        private void menuCopy_Click(object sender, RoutedEventArgs e)
        {
            var doc = (Documents.BaseDocument)documentContainer.SelectedContent;
            doc.Copy();
        }

        private void menuPaste_Click(object sender, RoutedEventArgs e)
        {
            var doc = (Documents.BaseDocument)documentContainer.SelectedContent;
            doc.Paste();
        }

        private void menuSelectAll_Click(object sender, RoutedEventArgs e)
        {
            var doc = (Documents.BaseDocument)documentContainer.SelectedContent;
            doc.SelectAll();
        }

      
        /// <summary>
        /// 打开窗口，并且定位到引用了指定点名的组件上
        /// </summary>
        /// <param name="windowid"></param>
        /// <param name="pointName"></param>
        public void OpenWindow(int windowid , string pointName)
        {
            //查看已经打开的窗口
            foreach (Documents.DocTabItem item in documentContainer.Items)
            {
                if(item.Content is Documents.ControlWindowDocument)
                {
                    var doc = item.Content as Documents.ControlWindowDocument;
                    if(  doc._dataModel.id == windowid)
                    {
                        MainWindow.Instance.documentContainer.SelectedItem = item;
                        doc.SelectWebControlByPointName(pointName);
                        return;
                    }
                }
            }

            var node = this.Nodes.FindWindowNode(windowid);
            if(node != null)
            {
                var doc = new Documents.ControlWindowDocument(node.Parent as ControlWindowContainerNode, node.DataModel, false);
                doc.SelectWebControlByPointName(pointName);
                MainWindow.Instance.SetActiveDocument(doc);
                return;
            }
            this.Cursor = Cursors.Wait;
            Helper.Remote.Invoke<SunRizServer.ControlWindow>("GetWindowInfo", (datamodel, err) => {
                this.Cursor = null;
                if (err != null)
                {
                    MessageBox.Show(this, err);
                }
                else
                {
                    var doc = new Documents.ControlWindowDocument(null, datamodel, false);
                    doc.SelectWebControlByPointName(pointName);
                    MainWindow.Instance.SetActiveDocument(doc);
                }
            }, windowid, null);            
        }

        private void txtSearch_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.Key == Key.Enter)
            {
                search();
            }
        }

        void search()
        {
            string key = txtSearch.Text;
            this.Cursor = Cursors.Wait;
            Helper.Remote.Invoke<SearchResult[]>("FindDevicePointInWindow", (result, err) => {
                this.Cursor = null;
                if (err != null)
                {
                    MessageBox.Show(this, err);
                }
                else
                {
                    this.SearchResultDialog.SetResult(key, result);
                    this.SearchResultDialog.Owner = this;
                    this.SearchResultDialog.Show();
                }
            }, key);
        }

        internal static void showWindow(Window window)
        {
            window.Show();

            var originalTopmost = window.Topmost;
            //窗口一出来就被主窗口挡着，所以这么处理一下
            window.Topmost = true;

            Task.Run(() =>
            {
                System.Threading.Thread.Sleep(100);
                window.Dispatcher.Invoke(() => { window.Topmost = originalTopmost; });
            });
        }

        public void ShowAlarmWindow()
        {
            if (_alarmWindow == null)
                _alarmWindow = new Dialogs.AlarmWindow();
            showWindow(_alarmWindow);
        }
        public void ShowSysLogWindow()
        {
            showWindow(new Dialogs.SysLogWindow());
        }
        public void ShowHistoryWindow()
        {
            showWindow(new Dialogs.HistoryWindow());
        }
        public void ShowUserMgrWindow()
        {
            showWindow(new Dialogs.UserManager());
        }

        private void menuModifyPwd_Click(object sender, RoutedEventArgs e)
        {
            showWindow(new Dialogs.ModifyPwd());
        }

        private void TreeViewItem_PreviewMouseMove(object sender, MouseEventArgs e)
        {
            FrameworkElement sourceElement = (FrameworkElement)sender;
            if (sender is TreeViewItem)
            {
                TreeViewItem treeviewitem = sender as TreeViewItem;
                if (!(e.OriginalSource is TextBlock))
                    return;
                if (((FrameworkElement)e.OriginalSource).GetParentByName<TreeViewItem>(null) != treeviewitem)
                    return;
            }
            
            Models.SolutionNode data = sourceElement.DataContext as Models.SolutionNode;
            data.MouseMove(sender, e);
        }

        private void TreeViewItem_PreviewMouseUp(object sender, MouseButtonEventArgs e)
        {
            FrameworkElement sourceElement = (FrameworkElement)sender;
            if (sender is TreeViewItem)
            {
                TreeViewItem treeviewitem = sender as TreeViewItem;
                if (!(e.OriginalSource is TextBlock))
                    return;
                if (((FrameworkElement)e.OriginalSource).GetParentByName<TreeViewItem>(null) != treeviewitem)
                    return;
            }

            Models.SolutionNode data = sourceElement.DataContext as Models.SolutionNode;
            data.MouseUp(sender, e);
        }
    }

    /// <summary>
    /// 搜索结果
    /// </summary>
    class SearchResult
    {
        /// <summary>
        /// 画面id
        /// </summary>
        public int id { get; set; }
        /// <summary>
        /// 画面路径
        /// </summary>
        public string path { get; set; }
        /// <summary>
        /// 匹配的内容
        /// </summary>
        public string content { get; set; }
    }
}
