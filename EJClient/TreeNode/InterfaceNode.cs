using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace EJClient.TreeNode
{
    class InterfaceNode : TreeNodeBase
    {
        public EJ.InterfaceModule Module
        {
            get;
            set;
        }
        
        public override string Icon
        {
            get
            {
                return "/imgs/interface.png";
            }
            set
            {
                base.Icon = value;
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
                    _ContextMenu = (ContextMenu)MainWindow.instance.Resources["treeMenu_interface"];
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
            InterfaceNode treenode = MainWindow.instance.tree1.SelectedItem as InterfaceNode;
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
        public InterfaceNode(TreeNodeBase parent)
            : base(parent)
        {
            if (parent is ProjectNode)
            {
                ProjectNode projectNode = parent as ProjectNode;
                this.Module = new EJ.InterfaceModule() { id = 0,};
                this.Module.ProjectID = projectNode.Project.id;
            }
        }

        public override void ReBindItems()
        {
            var modules = Helper.Client.InvokeSync<EJ.InterfaceModule[]>("GetInterfaceModuleList", this.Module.ProjectID.Value, this.Module.id.Value);
            foreach (var module in modules)
            {
                InterfaceItemNode node = new InterfaceItemNode(module, this);
                this.Children.Add(node);
                node.ReBindItems();
            }
        }

        public void CreateChild(bool isFolder)
        {
            UI.InputBox inputbox = new UI.InputBox("请输入目录名称", "新建目录");
            inputbox.Owner = MainWindow.instance;
            if (inputbox.ShowDialog() == true && inputbox.Value.Trim().Length > 0)
            {

                if (this.Children.Count(m => m.Name.ToLower() == inputbox.Value.Trim().ToLower()) > 0)
                {
                    MessageBox.Show(MainWindow.instance, "名称重复");
                    return;
                }

                EJ.InterfaceModule module = new EJ.InterfaceModule()
                {
                    ProjectID = Module.ProjectID,
                    Name = inputbox.Value.Trim(),
                    ParentID = Module.id,
                    IsFolder = isFolder,
                };
                try
                {
                    module.id = Helper.Client.InvokeSync<int>("UpdateInterfaceModule", module);
                    module.ChangedProperties.Clear();

                    this.Children.Add(new InterfaceItemNode(module, this));
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
