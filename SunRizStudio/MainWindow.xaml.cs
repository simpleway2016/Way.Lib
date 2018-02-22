﻿using System;
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
                if (e.ClickCount == 2)
                {
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
    }
}
