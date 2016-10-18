using EJClient.TreeNode;
using EJClient.UI;
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

namespace EJClient
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        /// <summary>
        /// 正显示菜单的treenode
        /// </summary>
        internal static MainWindow instance;
        public MainWindow()
        {
            instance = this;
            InitializeComponent();
            this.Title += System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString();
            if (Helper.CurrentUserRole != EJ.User_RoleEnum.管理员)
            {
                menu_users_mgr.Visibility = System.Windows.Visibility.Collapsed;
            }
        }
        ProjectNode pnode;
        internal System.Collections.ObjectModel.ObservableCollection<TreeNodeBase> ProjectList;
        void bindTree()
        {
            tree1.Items.Clear();
            ProjectList = new System.Collections.ObjectModel.ObservableCollection<TreeNodeBase>();
            using (var web = Helper.CreateWebService())
            {
              
                var projects = web.GetCurrentUserProjectList().ToJsonObject<EJ.Project[]>();
                foreach (var project in projects)
                {
                    ProjectNode node = new ProjectNode(project);
                    ProjectList.Add(node);
                    pnode = node;
                }
            }
            if (ProjectList.Count == 1)
            {
                ProjectList[0].IsExpanded = true;
            }
            tree1.ItemsSource = ProjectList;


           
        }

        internal TreeNode.InterfaceItemNode FindInterfaceModule(int moduleid)
        {
            System.Collections.ObjectModel.ObservableCollection<TreeNodeBase> list = tree1.ItemsSource as System.Collections.ObjectModel.ObservableCollection<TreeNodeBase>;
            foreach (TreeNode.ProjectNode projectNode in list)
            {
                foreach (var childnode in projectNode.Children)
                {
                    if (childnode is InterfaceNode)
                    {
                        foreach (TreeNode.InterfaceItemNode itemnode in childnode.Children)
                        {
                            if (itemnode.Module.id == moduleid)
                                return itemnode;
                            else
                            {
                                var result = itemnode.FindInterfaceModule(moduleid);
                                if (result != null)
                                    return result;
                            }
                        }
                    }
                }
            }
            return null;
        }

        internal TreeNode.DBModuleNode FindDBModule(int moduleid)
        {
            System.Collections.ObjectModel.ObservableCollection<TreeNodeBase> list = tree1.ItemsSource as System.Collections.ObjectModel.ObservableCollection<TreeNodeBase>;
            foreach (TreeNode.ProjectNode projectNode in list)
            {
                foreach (var childnode in projectNode.Children)
                {
                    if (childnode is DatabaseNode)
                    {
                        foreach (TreeNode.DatabaseItemNode dbnode in childnode.Children)
                        {
                            foreach (var c2 in dbnode.Children)
                            {
                                if (c2 is 数据模块Node)
                                {
                                    TreeNode.DBModuleNode result = ((数据模块Node)c2).FindDBModule(moduleid);
                                    if (result != null)
                                        return result;
                                }
                            }
                        }
                    }
                }
            } 
            return null;
        }
        internal TreeNode.DBTableNode FindDBTable(int tableid)
        {
            System.Collections.ObjectModel.ObservableCollection<TreeNodeBase> list = tree1.ItemsSource as System.Collections.ObjectModel.ObservableCollection<TreeNodeBase>;
            foreach (TreeNode.ProjectNode projectNode in list)
            {
                foreach (var childnode in projectNode.Children)
                {
                    if (childnode is DatabaseNode)
                    {
                        foreach (TreeNode.DatabaseItemNode dbnode in childnode.Children)
                        {
                            foreach (var c2 in dbnode.Children)
                            {
                                if (c2 is 数据表Node)
                                {
                                    foreach (TreeNode.DBTableNode node in c2.Children)
                                    {
                                        if (node.Table.id == tableid)
                                            return node;
                                    }
                                }
                            }
                        }
                    }
                }
            }
            return null;
        }
    
        private void Window_Loaded_1(object sender, RoutedEventArgs e)
        {
           
            bindTree();
        }

        private void MenuItem_创建数据库_Click_1(object sender, RoutedEventArgs e)
        {
            MenuItem item = (MenuItem)sender;
            ContextMenu menu = (ContextMenu)item.Parent;
            var obj = (StackPanel)menu.PlacementTarget;
            ProjectNode projectNode = (ProjectNode)obj.Tag;
            using (Forms.DatabaseEditor form = new Forms.DatabaseEditor(projectNode.Project.id.Value))
            {
                form.Text = "新建数据库";
                if (form.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    DatabaseNode dbnode = (DatabaseNode)projectNode.Children.Where(m => m is TreeNode.DatabaseNode).FirstOrDefault();
                    dbnode.ReBindItems();
                }
            }

        }
        private void MenuItem_新建数据表_Click_1(object sender, RoutedEventArgs e)
        {
            MenuItem item= (MenuItem)sender;
            ContextMenu menu = (ContextMenu)item.Parent;
            var obj = (StackPanel)menu.PlacementTarget;
            DatabaseItemNode dbnode = (DatabaseItemNode)obj.Tag;
            Forms.DBTableEditor frm = new Forms.DBTableEditor(dbnode , null);
            frm.Owner = this;
            frm.ShowDialog();

        }
        private void MenuItem_设计数据表_Click_1(object sender, RoutedEventArgs e)
        {
            DBTableNode tableNode = ((FrameworkElement)e.OriginalSource).DataContext as DBTableNode;
            tableNode.OnDoubleClick(null, null);
        }
        private void MenuItem_查看数据表数据_1(object sender, RoutedEventArgs e)
        {
            DBTableNode tableNode = ((FrameworkElement)e.OriginalSource).DataContext as DBTableNode;
            Forms.DataViewer frm = new Forms.DataViewer(tableNode.Table);
            frm.Show();
        }
        private void MenuItem_数据库属性_Click_1(object sender, RoutedEventArgs e)
        {
            DatabaseItemNode databaseItemNode = tree1.SelectedItem as DatabaseItemNode;
            ProjectNode projectNode = databaseItemNode.Parent.Parent as ProjectNode;
            using (Forms.DatabaseEditor form = new Forms.DatabaseEditor(projectNode.Project.id.Value , databaseItemNode.Database.id.Value))
            {
                form.Text = "数据库属性";
                if (form.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    databaseItemNode.Parent.ReBindItems();
                }
            }
        }
       
        private void MenuItem_设置引用的类库_Click_1(object sender, RoutedEventArgs e)
        {
            ProjectNode projectNode = ((FrameworkElement)e.OriginalSource).DataContext as ProjectNode;
            using (Forms.InterfaceCenter.DllImportEditor frm = new Forms.InterfaceCenter.DllImportEditor(projectNode.Project.id.Value))
            {
                frm.ShowDialog();
            }
        }
        private void MenuItem_移除数据库_Click_1(object sender, RoutedEventArgs e)
        {
            DatabaseItemNode selectedItem = ((FrameworkElement)e.OriginalSource).DataContext as DatabaseItemNode;
            if (MessageBox.Show("确定移除" + selectedItem.Name + "吗？" , "" , MessageBoxButton.OKCancel) == MessageBoxResult.OK)
            {
                try
                {
                    selectedItem.Delete();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(this, ex.Message);
                }
            }
        }
        private void MenuItem_导出更新_Click_1(object sender, RoutedEventArgs e)
        {
            DatabaseItemNode selectedItem = ((FrameworkElement)e.OriginalSource).DataContext as DatabaseItemNode;

            try
            {
                using (System.Windows.Forms.SaveFileDialog fd = new System.Windows.Forms.SaveFileDialog())
                {
                    fd.Filter = "*.action|*.action";
                    fd.FileName = selectedItem.Database.Name + "_更新脚本.action";
                    if (fd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                    {
                        selectedItem.OutputAction(fd.FileName);
                        MessageBox.Show("导出完毕！");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message);
            }
        }
        private void MenuItem_导出数据_Click_1(object sender, RoutedEventArgs e)
        {
            DatabaseItemNode selectedItem = ((FrameworkElement)e.OriginalSource).DataContext as DatabaseItemNode;
            Forms.OutputDBTableData frm = new Forms.OutputDBTableData(selectedItem);
            frm.Owner = this;
            frm.ShowDialog();
        }
        private void MenuItem_导入数据_Click_1(object sender, RoutedEventArgs e)
        {
            DatabaseItemNode selectedItem = ((FrameworkElement)e.OriginalSource).DataContext as DatabaseItemNode;
            using (System.Windows.Forms.OpenFileDialog f = new System.Windows.Forms.OpenFileDialog())
            {
                f.Filter = "*.xml|*.xml";
                if (f.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    Forms.ImportData frm = new Forms.ImportData(f.FileName, selectedItem);
                    frm.ShowDialog();
                }
            }
        }
        private void MenuItem_编译_Click_1(object sender, RoutedEventArgs e)
        {
            DatabaseItemNode selectedItem = ((FrameworkElement)e.OriginalSource).DataContext as DatabaseItemNode;

            Forms.BuildeCode code = new Forms.BuildeCode(selectedItem.Database.id.Value);
            code.Owner = this;
            code.ShowDialog();
        }
        private void MenuItem_新建目录_Click_1(object sender, RoutedEventArgs e)
        {
            数据模块Node parentnode = tree1.SelectedItem as 数据模块Node;
            parentnode.CreateChild(true);

        }
        private void MenuItem_新建模块_Click_1(object sender, RoutedEventArgs e)
        {
            数据模块Node parentnode = tree1.SelectedItem as 数据模块Node;
            parentnode.CreateChild(false);
        }
        private void MenuItem_删除目录_Click_1(object sender, RoutedEventArgs e)
        {
            DBModuleNode modulenode = (DBModuleNode)tree1.SelectedItem;
            modulenode.Delete();
        }
        private void MenuItem_重命名模块_Click_1(object sender, RoutedEventArgs e)
        {
            DBModuleNode modulenode = (DBModuleNode)tree1.SelectedItem;
            modulenode.Rename();
        }
        private void MenuItem_删除项目_Click_1(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show(  "确定删除此项目吗？","", MessageBoxButton.OKCancel) == MessageBoxResult.OK)
            {
                ProjectNode projectNode = (ProjectNode)tree1.SelectedItem;
                try
                {
                    using (Web.DatabaseService web = Helper.CreateWebService())
                    {
                        web.DeleteProject(projectNode.Project.id.Value );
                        this.ProjectList.Remove(projectNode);
                    }
                }
                catch (Exception ex)
                {
                    System.Windows.MessageBox.Show(this, ex.GetBaseException().Message);
                }
            }
        }
        private void MenuItem_重命名项目_Click_1(object sender, RoutedEventArgs e)
        {
            ProjectNode projectNode = (ProjectNode)tree1.SelectedItem;
            InputBox inputBox = new InputBox("请输入新项目名称", "");
            inputBox.Value = projectNode.Name;
            if (inputBox.ShowDialog() == true && !string.IsNullOrEmpty(inputBox.Value) && inputBox.Value.Trim() != projectNode.Project.Name)
            {
                try
                {
                    using (Web.DatabaseService web = Helper.CreateWebService())
                    {
                        web.UpdateProject(projectNode.Project.id.Value, inputBox.Value.Trim());
                        projectNode.Project.Name = inputBox.Value.Trim();
                        projectNode.Name = projectNode.Project.Name;
                    }
                }
                catch (Exception ex)
                {
                    System.Windows.MessageBox.Show(this, ex.GetBaseException().Message);
                }
            }
        }
        private void MenuItem_删除数据表_Click_1(object sender, RoutedEventArgs e)
        {
            DBTableNode modulenode = (DBTableNode)tree1.SelectedItem;
            modulenode.Delete();
        }
        private void MenuItem_刷新数据表_Click_1(object sender, RoutedEventArgs e)
        {
            数据表Node modulenode = (数据表Node)tree1.SelectedItem;
            modulenode.ReBindItems();
        }

        private void MenuItem_新建接口目录_Click_1(object sender, RoutedEventArgs e)
        {
            InterfaceNode parentnode = tree1.SelectedItem as InterfaceNode;
            parentnode.CreateChild(true);

        }
        private void MenuItem_新建接口模块_Click_1(object sender, RoutedEventArgs e)
        {
            InterfaceNode parentnode = tree1.SelectedItem as InterfaceNode;
            parentnode.CreateChild(false);
        }
        private void MenuItem_删除接口目录_Click_1(object sender, RoutedEventArgs e)
        {
            InterfaceItemNode modulenode = (InterfaceItemNode)tree1.SelectedItem;
            modulenode.Delete();
        }
        private void MenuItem_重命名接口模块_Click_1(object sender, RoutedEventArgs e)
        {
            InterfaceItemNode modulenode = (InterfaceItemNode)tree1.SelectedItem;
            modulenode.Rename();
        }

        private void MenuItem_从config文件导入_Click_1(object sender, RoutedEventArgs e)
        {
            DatabaseNode databaseNode = (DatabaseNode)tree1.SelectedItem;
            ProjectNode projectNode = (ProjectNode)databaseNode.Parent;
            using (System.Windows.Forms.OpenFileDialog fd = new System.Windows.Forms.OpenFileDialog())
            {
                fd.Filter = "*.config|*.config";
                if (fd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    try
                    {
                        OldVersionService.Import.import(fd.FileName, projectNode.Project.id.Value , databaseNode);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(this, ex.Message);
                    }
                }
            }
        }

        TreeNodeBase m_drogItem;
        private void tree1_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            TreeNodeBase selectedItem = ((FrameworkElement)e.OriginalSource).DataContext as TreeNodeBase;

            if (selectedItem is DBModuleNode )
            {
                m_drogItem = selectedItem;
                DragDropEffects finalDropEffect = DragDrop.DoDragDrop((System.Windows.DependencyObject)e.OriginalSource, selectedItem, DragDropEffects.Move);
            }
            if (e.ClickCount > 1)
            {
                if (selectedItem != null)
                {
                    selectedItem.IsSelected = true;
                    selectedItem.OnDoubleClick(selectedItem, e);
                }
            }
            else
            {
                if (selectedItem != null)
                {
                    selectedItem.IsSelected = true;
                    selectedItem.LeftMouseDown(selectedItem, e);
                }
            }
        }

        private void tree1_Drop(object sender, DragEventArgs e)
        {
            try
            {
                int parentid = -1;
                TreeNodeBase selectedItem = ((FrameworkElement)e.OriginalSource).DataContext as TreeNodeBase;
                if (!(selectedItem is DBModuleNode) && selectedItem is 数据模块Node)
                {
                    //检查数据库是否相同
                    if (((数据模块Node)selectedItem).Module == null ||
                        ((DBModuleNode)m_drogItem).Module.DatabaseID != ((数据模块Node)selectedItem).Module.DatabaseID)
                    {
                        e.Effects = DragDropEffects.None;
                        e.Handled = true;
                        return;
                    }
                    parentid = 0;
                }
                else if (selectedItem is DBModuleNode && (((DBModuleNode)selectedItem).Module.IsFolder == true))
                {
                    //检查数据库是否相同
                    if (((DBModuleNode)m_drogItem).Module.DatabaseID != ((DBModuleNode)selectedItem).Module.DatabaseID)
                    {
                        e.Effects = DragDropEffects.None;
                        e.Handled = true;
                        return;
                    }

                    //判断拖动者是否是目标者的父节点
                    bool canDrog = selectedItem != m_drogItem;
                    if (canDrog)
                    {
                        for (DBModuleNode parent = selectedItem.Parent as DBModuleNode; parent != null; parent = parent.Parent as DBModuleNode)
                        {
                            if (parent == selectedItem)
                            {
                                canDrog = false;
                                break;
                            }
                        }
                    }
                    if (canDrog)
                    {
                        parentid = ((DBModuleNode)selectedItem).Module.id.Value;
                    }


                }
                if (parentid >= 0 && (((DBModuleNode)m_drogItem).Module.parentID != parentid))
                {
                    using (var web = Helper.CreateWebService())
                    {
                        web.ChangeModuleParent(((DBModuleNode)m_drogItem).Module.id.Value, parentid);
                    }
                    ((DBModuleNode)m_drogItem).Module.parentID = parentid;
                    m_drogItem.Parent.Children.Remove(m_drogItem);
                    selectedItem.Children.Add(m_drogItem);
                    m_drogItem.Parent = selectedItem;
                }

               
            }
            catch(Exception ex)
            {
                MessageBox.Show(this ,ex.Message);
            }
            finally
            {
                e.Effects = DragDropEffects.None;
                e.Handled = true;
            }
        }

        private void tree1_DragOver(object sender, DragEventArgs e)
        {
            try
            {
                TreeNodeBase selectedItem = ((FrameworkElement)e.OriginalSource).DataContext as TreeNodeBase;
                if (!(selectedItem is DBModuleNode) && selectedItem is 数据模块Node)
                { 
                    //检查数据库是否相同
                    if (((数据模块Node)selectedItem).Module == null ||
                          ((DBModuleNode)m_drogItem).Module.DatabaseID != ((数据模块Node)selectedItem).Module.DatabaseID)
                    {
                        e.Effects = DragDropEffects.None;
                        e.Handled = true;
                        return;
                    }
                    e.Effects = DragDropEffects.Move;
                }
                else if (selectedItem is DBModuleNode && (((DBModuleNode)selectedItem).Module.IsFolder==true) )
                {
                    //检查数据库是否相同
                    if (((DBModuleNode)m_drogItem).Module.DatabaseID != ((DBModuleNode)selectedItem).Module.DatabaseID)
                    {
                        e.Effects = DragDropEffects.None;
                        e.Handled = true;
                        return;
                    }

                    //判断拖动者是否是目标者的父节点
                    bool canDrog = selectedItem != m_drogItem;
                    if (canDrog)
                    {
                        for (DBModuleNode parent = selectedItem.Parent as DBModuleNode; parent != null; parent = parent.Parent as DBModuleNode)
                        {
                            if (parent == selectedItem)
                            {
                                canDrog = false;
                                break;
                            }
                        }
                    }
                    if (canDrog)
                    {
                        e.Effects = DragDropEffects.Move;
                        selectedItem.IsExpanded = true;
                    }
                    else
                    {
                        e.Effects = DragDropEffects.None;
                    }
                }
                else if (selectedItem is DBModuleNode && (((DBModuleNode)selectedItem).Module.IsFolder == false))
                {
                    e.Effects = DragDropEffects.None;
                }
                else
                {
                    e.Effects = DragDropEffects.None;
                }
                e.Handled = true;
            }
            catch
            {
            }
        }
        private void tree1_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Right)
            {
                TreeNodeBase item = ((FrameworkElement)e.OriginalSource).DataContext as TreeNodeBase;
                item.IsSelected = true;
                e.Handled = true;
            }
        }
        void menu_changePwd_Click_1(object sender, RoutedEventArgs e)
        {
            Forms.ChangePassword frm = new Forms.ChangePassword();
            frm.Owner = this;
            frm.ShowDialog();
        }
        private void menu_users_mgr_Click_1(object sender, RoutedEventArgs e)
        {
            using (Forms.UserMgr frm = new Forms.UserMgr())
            {
                frm.ShowDialog();
            }
        }

        private void txtSearch_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                UI.SearchResultDocument doc = new UI.SearchResultDocument(txtSearch.Text.Trim());
                MainWindow.instance.documentContainer.Items.Add(doc);
                MainWindow.instance.documentContainer.SelectedItem = doc;
            }
        }

        private void NewProject_Click_1(object sender, RoutedEventArgs e)
        {
            InputBox inputBox = new InputBox("请输入新项目名称", "");
            if (inputBox.ShowDialog() == true && !string.IsNullOrEmpty(inputBox.Value))
            {
                try
                {

                    using (Web.DatabaseService web = Helper.CreateWebService())
                    {
                        var project = web.CreateProject(inputBox.Value.Trim()).ToJsonObject<EJ.Project>();
                        TreeNode.ProjectNode node = new ProjectNode(project);
                        this.ProjectList.Add(node);
                    }
                }
                catch (Exception ex)
                {

                    MessageBox.Show(this, ex.GetBaseException().Message);
                }
            }
        }

      
      
    }
}
