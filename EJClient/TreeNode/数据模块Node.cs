using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace EJClient.TreeNode
{
    class 数据模块Node:TreeNodeBase
    {
        public override string Icon
        {
            get
            {
                return "imgs/module.png";
            }
            set
            {
            }
        }
        EJ.DBModule _Module = new EJ.DBModule() {id = 0,IsFolder =true };
        public virtual EJ.DBModule Module
        {
            get
            {
                return _Module;
            }
            set
            {
            }
        }

        ContextMenu _ContextMenu;
        static bool settedEvent = false;
        public override ContextMenu ContextMenu
        {
            get
            {
                if (_ContextMenu == null)
                {
                    _ContextMenu = (ContextMenu)MainWindow.instance.Resources["treeMenu_Module"];
                    if (settedEvent == false)
                    {
                        settedEvent = true;
                        _ContextMenu.Opened += _ContextMenu_Opened;
                    }
                }
                return _ContextMenu;
            }
            set
            {
                _ContextMenu = value;
            }
        }

        void _ContextMenu_Opened(object sender, RoutedEventArgs e)
        {
            ContextMenu menu = sender as ContextMenu;
            foreach (MenuItem menuitem in menu.Items)
            {
                menuitem.Visibility = Visibility.Visible;
            }
            数据模块Node treenode = MainWindow.instance.tree1.SelectedItem as 数据模块Node;
            if (treenode.Module.id == 0)
            {
                foreach (MenuItem menuitem in menu.Items)
                {
                    if (menuitem.Header.ToString().Contains("删除") || menuitem.Header.ToString().Contains("重命名"))
                        menuitem.Visibility = Visibility.Collapsed;
                }
            }
            else if (treenode.Module.IsFolder == false)
            {
                foreach (MenuItem menuitem in menu.Items)
                {
                    if (menuitem.Header.ToString().Contains("新建"))
                        menuitem.Visibility = Visibility.Collapsed;
                }
            }
        }
  
        public 数据模块Node(TreeNodeBase parent)
            : base(parent)
        {
            if (parent is DatabaseItemNode)
                Module.DatabaseID = ((DatabaseItemNode)parent).Database.id;
        }

        public override void ReBindItems()
        {
            var modules = Helper.Client.InvokeSync<EJ.DBModule[]>("GetDBModuleList", this.Module.DatabaseID.Value, this.Module.id.Value);
            foreach (var module in modules)
            {
                DBModuleNode node = new DBModuleNode(this, module);
                this.Children.Add(node);
                node.ReBindItems();
            }
        }

        internal TreeNode.DBModuleNode FindDBModule(int moduleid)
        {
            foreach (DBModuleNode node in Children)
            {
                if (node.Module.id == moduleid)
                    return node;
                else
                {
                    var result = node.FindDBModule(moduleid);
                    if (result != null)
                        return result;
                }
            }
            return null;
        }

        public void CreateChild(bool isFolder )
        {
            UI.InputBox inputbox = new UI.InputBox(isFolder ? "请输入目录名称" : "请输入模块名称", isFolder ? "新建目录" : "新建模块");
            inputbox.Owner = MainWindow.instance;
            if (inputbox.ShowDialog() == true && inputbox.Value.Trim().Length > 0)
            {
                
                if (this.Children.Count(m => m.Name.ToLower() == inputbox.Value.Trim().ToLower()) > 0)
                {
                    MessageBox.Show(MainWindow.instance, "名称重复");
                    return;
                }

                EJ.DBModule module = new EJ.DBModule()
                {
                    DatabaseID = Module.DatabaseID,
                    Name = inputbox.Value.Trim(),
                    parentID = Module.id,
                    IsFolder = isFolder,
                };
                try
                {
                    module.id = Helper.Client.InvokeSync<int>("UpdateDBModule", module);
                    module.ChangedProperties.Clear();

                    this.Children.Add(new DBModuleNode(this, module));
                    this.IsExpanded = true;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(MainWindow.instance, ex.Message);
                }
            }
        }
    }
}
