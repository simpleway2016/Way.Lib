using EJClient.Forms.InterfaceCenter;
using EJClient.TreeNode;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace EJClient.UI
{
    class InterfaceModuleDocument : Document
    {
        public InterfaceItemNode InterfaceItemNode;
        Canvas m_canvas;
        Grid m_Grid;
        public InterfaceModuleDocument(TreeNode.InterfaceItemNode interfaceNode)
        {
            InterfaceItemNode = interfaceNode;
            this.DataContext = interfaceNode;
            this.SetBinding(TabItem.HeaderProperty, "Name");
            if (interfaceNode.Module.LockUserId != null)
            {
                this.HeaderStringFormat = "{0}(" + Helper.Client.InvokeSync<string>("GetUserNameByID", interfaceNode.Module.LockUserId.Value) + "锁定)";
            }
           

            ScrollViewer scrollview = new ScrollViewer();
            scrollview.HorizontalScrollBarVisibility = ScrollBarVisibility.Auto;
            scrollview.VerticalScrollBarVisibility = ScrollBarVisibility.Auto;
            this.Content = scrollview;

            Grid mainGrid = new Grid();
            scrollview.Content = mainGrid;
            mainGrid.AllowDrop = true;
            mainGrid.Background = Brushes.White;
            mainGrid.MouseDown += mainGrid_MouseDown;

            m_canvas = new Canvas();
            m_canvas.HorizontalAlignment = System.Windows.HorizontalAlignment.Stretch;
            m_canvas.VerticalAlignment = System.Windows.VerticalAlignment.Stretch;
            mainGrid.Children.Add(m_canvas);

            m_Grid = new Grid();
            m_Grid.HorizontalAlignment = System.Windows.HorizontalAlignment.Stretch;
            m_Grid.VerticalAlignment = System.Windows.VerticalAlignment.Stretch;
            mainGrid.Children.Add(m_Grid);

            CreateContextMenu();

            loadItems();
        }

        void mainGrid_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            ClearSelected();
        }

        void loadItems()
        {
            try
            {
                var items = Helper.Client.InvokeSync<EJ.InterfaceInModule[]>("GetInterfaceInModule", this.InterfaceItemNode.Module.id.Value);
                foreach (var item in items)
                {
                    Type type = typeof(InterfaceItemNode).Assembly.GetType(item.Type);
                    if (type == typeof(UI.DescriptionView))
                    {
                        DescriptionView view = new DescriptionView("", item);
                        view.LoadJsonData(item.JsonData);
                        view.Margin = new Thickness((double)item.x, (double)item.y, 0, 0);
                        if (item.width != null)
                        {
                            view.Width = item.width.Value;
                            view.Height = item.height.Value;
                        }
                        m_Grid.Children.Add(view);
                    }
                    else if (type == typeof(UI.ClassView))
                    {
                        ClassView view = new ClassView(item);
                        view.Margin = new Thickness((double)item.x, (double)item.y, 0, 0);
                        m_Grid.Children.Add(view);
                    }
                }
                this.Dispatcher.BeginInvoke(new Action(() =>
                {
                    foreach (FrameworkElement view in m_Grid.Children)
                    {
                        if (view is DescriptionView)
                        {
                            ((DescriptionView)view).LoadConnects();
                        }
                    }
                }), System.Windows.Threading.DispatcherPriority.Loaded, null);
            }
            catch (Exception ex)
            {
                Helper.ShowError(ex);
            }
        }

        void CreateContextMenu()
        {
            ContextMenu menu = new ContextMenu();
            MenuItem menuitem = new MenuItem();
            menuitem.Header = "添加描述...";
            menuitem.Click += menuitem_添加描述_Click;
            menu.Items.Add(menuitem);

            menuitem = new MenuItem();
            menuitem.Header = "添加Class/Interface...";
            menuitem.Click += menuitem_添加class_Click;
            menu.Items.Add(menuitem);

            menu.Items.Add(new Separator());

            if (this.InterfaceItemNode.Module.LockUserId == null)
            {
                menuitem = new MenuItem();
                menuitem.Header = "锁定";
                menuitem.Click += menuitem_锁定_Click;
                menu.Items.Add(menuitem);
            }
            if (this.InterfaceItemNode.Module.LockUserId == Helper.CurrentUserID)
            {
                menuitem = new MenuItem();
                menuitem.Header = "解锁";
                menuitem.Click += menuitem_解锁_Click;
                menu.Items.Add(menuitem);
            }
            this.ContextMenu = menu;
        }
        void menuitem_解锁_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            var menuitem = (MenuItem)sender; 
            try
            {
                Helper.Client.InvokeSync<string>("UnLockInterfaceModule", InterfaceItemNode.Module.id.Value);
                InterfaceItemNode.Module.LockUserId = null;
                this.HeaderStringFormat = "{0}";
                this.SetBinding(TabItem.HeaderProperty, "");
                this.SetBinding(TabItem.HeaderProperty, "Name");
                menuitem.Click -= menuitem_解锁_Click;
                menuitem.Header = "锁定";
                menuitem.Click += menuitem_锁定_Click;
            }
            catch (Exception ex)
            {
                Helper.ShowError(ex);
            }
        }
        void menuitem_锁定_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            var menuitem = (MenuItem)sender; 
            try
            {
                Helper.Client.InvokeSync<string>("LockInterfaceModule", InterfaceItemNode.Module.id.Value);
                InterfaceItemNode.Module.LockUserId = Helper.CurrentUserID;
                this.HeaderStringFormat = "{0}(" + Helper.Client.InvokeSync<string>("GetUserNameByID", InterfaceItemNode.Module.LockUserId.Value) + "锁定)";
                this.SetBinding(TabItem.HeaderProperty, "");
                this.SetBinding(TabItem.HeaderProperty, "Name");

                menuitem.Click -= menuitem_锁定_Click;
                menuitem.Header = "解锁";
                menuitem.Click += menuitem_解锁_Click;
            }
            catch (Exception ex)
            {
                Helper.ShowError(ex);
            }
        }
        void menuitem_添加class_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            ClassSelector frm = new ClassSelector(this.InterfaceItemNode.Module.ProjectID.Value);
            if (frm.ShowDialog() == true)
            {
                var value = frm.SelectedValue;
                TreeNodeBase parent = value.Parent;
                while (!(parent is EJClient.Forms.InterfaceCenter.Nodes.DLLNode))
                    parent = parent.Parent;
                string filepath = ((EJClient.Forms.InterfaceCenter.Nodes.DLLNode)parent).FilePath;
            

                EJ.InterfaceInModule data = new EJ.InterfaceInModule()
                {
                    ModuleID = InterfaceItemNode.Module.id,
                    x = (int)m_RightButtonDownPoint.X,
                    y = (int)m_RightButtonDownPoint.Y,
                    Type = typeof(ClassView).FullName,

                };
                UI.ClassView view = new ClassView(value, filepath, data);
                try
                {
                    data.JsonData = view.GetJsonData();
                    data.id = Helper.Client.InvokeSync<int>("UpdateInterfaceInModule", data);
                    data.ChangedProperties.Clear();

                    view.Margin = new Thickness(m_RightButtonDownPoint.X, m_RightButtonDownPoint.Y, 0, 0);
                    m_Grid.Children.Add(view);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(MainWindow.instance, ex.Message);
                }
            }
        }
        void menuitem_添加描述_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            InputBox frm = new InputBox("请输入内容" , "添加描述" , 200);
            frm.Owner = MainWindow.instance;
            if (frm.ShowDialog() == true)
            {
                EJ.InterfaceInModule data = new EJ.InterfaceInModule()
                    {
                        ModuleID = InterfaceItemNode.Module.id,
                        x = (int)m_RightButtonDownPoint.X,
                        y = (int)m_RightButtonDownPoint.Y,
                        Type = typeof(DescriptionView).FullName,
                        
                    };
                try
                {
                    DescriptionView desc = new DescriptionView(frm.Value, data);

                    data.JsonData = desc.GetJsonData();
                    data.id = Helper.Client.InvokeSync<int>("UpdateInterfaceInModule", data);
                    data.ChangedProperties.Clear();

                    desc.Margin = new Thickness(m_RightButtonDownPoint.X, m_RightButtonDownPoint.Y, 0, 0);
                    m_Grid.Children.Add(desc);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(MainWindow.instance, ex.Message);
                }
               
            }
        }

        public void ShowItem(int intefaceInModuleId)
        {
            ClearSelected();
            foreach (var item in m_Grid.Children)
            {
                if (item is UI.DescriptionView)
                {
                    var view = (UI.DescriptionView)item;
                    if (view.InterfaceInModule.id == intefaceInModuleId)
                    {
                        view.BringIntoView();
                        view.IsSelected = true;
                        return;
                    }
                }
                else if (item is UI.ClassView)
                {
                    var view = (UI.ClassView)item;
                    if (view.InterfaceInModule.id == intefaceInModuleId)
                    {
                        view.BringIntoView();
                        return;
                    }
                }
            }
        }

        public void ClearSelected()
        {
            foreach (var item in m_Grid.Children)
            {
                if (item is DescriptionView)
                {
                    DescriptionView view = (DescriptionView)item;
                    view.IsSelected = false;
                }
            }
        }


        Point m_RightButtonDownPoint;
        protected override void OnMouseRightButtonDown(System.Windows.Input.MouseButtonEventArgs e)
        {
            m_RightButtonDownPoint = e.GetPosition(this);
            base.OnMouseRightButtonDown(e);
        }
    }
}
