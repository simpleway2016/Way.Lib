using EJClient.TreeNode;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace EJClient.UI
{
    class ModuleDocument : Document
    {
        public TreeNode.DBModuleNode ModuleNode
        {
            get;
            private set;
        }
        Grid m_Grid;
        /// <summary>
        /// 需要聚焦显示的tableid
        /// </summary>
        int mFocusTableId;
        Canvas m_canvas;
        public ModuleDocument(TreeNode.DBModuleNode moduleNode)
        {
            this.ModuleNode = moduleNode;
            this.DataContext = moduleNode;
            this.SetBinding(TabItem.HeaderProperty, "Name");
            ScrollViewer scrollview = new ScrollViewer();
            scrollview.HorizontalScrollBarVisibility = ScrollBarVisibility.Auto;
            scrollview.VerticalScrollBarVisibility = ScrollBarVisibility.Auto;
            this.Content = scrollview;

            Grid mainGrid = new Grid();
            scrollview.Content = mainGrid;
            mainGrid.AllowDrop = true;
            mainGrid.Background = Brushes.White;
            mainGrid.Drop += m_Grid_Drop;
            mainGrid.MouseDown += mainGrid_MouseDown;

            m_canvas = new Canvas();
            m_canvas.HorizontalAlignment = System.Windows.HorizontalAlignment.Stretch;
            m_canvas.VerticalAlignment = System.Windows.VerticalAlignment.Stretch;
            mainGrid.Children.Add(m_canvas);

            m_Grid = new Grid();
            m_Grid.HorizontalAlignment = System.Windows.HorizontalAlignment.Stretch;
            m_Grid.VerticalAlignment = System.Windows.VerticalAlignment.Stretch;
            mainGrid.Children.Add(m_Grid);

 
            bindTables();

            CreateContextMenu();

            this.LayoutUpdated += ModuleDocument_LayoutUpdated;
        }

        void mainGrid_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            ClearSelected();
        }
        public void ClearSelected()
        {
            foreach (var item in m_Grid.Children)
            {
                if (item is Table)
                {
                    ((Table)item).IsSelected = false;
                }
            }
        }
        public void FocusTable(int tableid)
        {
            //该table可能会不存在，因为document刚打开，还没有加载tables，
            //所以web_GetTablesInModuleCompleted会判断mFocusTableId如果大于0，还会调用FocusTable
            mFocusTableId = tableid;
            ClearSelected();
            foreach (var item in m_Grid.Children)
            {
                if (item is Table && ((Table)item).DataSource.Table.id == tableid)
                {
                    mFocusTableId = 0;
                    ((Table)item).BringIntoView();
                    ((Table)item).IsSelected = true;
                }
            }
        }

        void CreateContextMenu()
        {
            ContextMenu menu = new ContextMenu();
            MenuItem menuitem = new MenuItem();
            menuitem.Header = "新建数据表...";
            menuitem.Click += 新建数据表;
            menu.Items.Add(menuitem);


            this.ContextMenu = menu;
            this.ContextMenu.Opened += ContextMenu_Opened;
        }

        void ContextMenu_Opened(object sender, RoutedEventArgs e)
        {
            m_newTableLocation = m_rightMouseDownLocation;
        }

        Point m_newTableLocation;
        Point m_rightMouseDownLocation;
        protected override void OnMouseRightButtonDown(System.Windows.Input.MouseButtonEventArgs e)
        {
            m_rightMouseDownLocation = e.GetPosition(m_Grid);
            base.OnMouseRightButtonDown(e);
        }

        void 新建数据表(object sender, RoutedEventArgs e)
        {
            TreeNodeBase parent = this.ModuleNode.Parent;
            while (!(parent is DatabaseItemNode))
                parent = parent.Parent;
            DatabaseItemNode databaseitemNode = (DatabaseItemNode)parent;
            Forms.DBTableEditor frm = new Forms.DBTableEditor(databaseitemNode, null);
            frm.Owner = MainWindow.instance;
            if (frm.ShowDialog() == true)
            {
                AddTable2Me((DBTableNode)databaseitemNode.Children.FirstOrDefault(m=>m is TreeNode.数据表Node).Children[0], m_newTableLocation);
            }
        }

        public bool needToLoadRelation = true;
        void ModuleDocument_LayoutUpdated(object sender, EventArgs e)
        {
            if (needToLoadRelation)
            {
                needToLoadRelation = false;
                refreshTableRelation();
            }
        }

        void bindTables()
        {
            TextBlock text = new TextBlock();
            text.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
            text.VerticalAlignment = System.Windows.VerticalAlignment.Top;
            text.Text = "Loading...";
            text.Margin = new Thickness(10,10,0,0);
            m_Grid.Children.Add(text);

            Helper.Client.Invoke<EJ.TableInModule[]>("GetTablesInModule",(locations, error)=>
            {
                if(error != null)
                {
                    Helper.ShowMessage(error);
                }
                else
                {
                    m_Grid.Children.Remove(text);
                    
                    foreach (var tInM in locations)
                    {
                        var dbtable = tInM.flag.ToJsonObject<EJ.DBTable>();
                        var columns = tInM.flag2.ToJsonObject<EJ.DBColumn[]>();
                        tInM.flag = null;
                        tInM.flag2 = null;

                        UI.Table uiTable = new Table(this);
                        uiTable.TableInModule = tInM;
                        uiTable.Margin = new Thickness(Math.Max(0, tInM.x.GetValueOrDefault()), Math.Max(0, tInM.y.GetValueOrDefault()), 0, 0);
                        uiTable.DataSource = new Table._DataSource()
                        {
                            Table = dbtable,
                            Columns = columns,
                        };
                        uiTable.DataBind();
                        uiTable.MoveCompleted += uiTable_MoveCompleted;
                        m_Grid.Children.Add(uiTable);
                    }
                    this.UpdateLayout();
                    refreshTableRelation();
                    if (mFocusTableId > 0)
                    {
                        FocusTable(mFocusTableId);
                    }
                }
            },this.ModuleNode.Module.id.Value);

        }


        public UI.Table getTableById(int id)
        {
            foreach (var ctrl in m_Grid.Children)
            {
                UI.Table table = ctrl as UI.Table;
                if (table == null)
                    continue;
                if (table.DataSource.Table.id == id)
                    return table;
            }
            return null;
        }

        /// <summary>
        /// 通知本document,table的relation发生变化
        /// </summary>
        /// <param name="tableid"></param>
        public void TableChangeRelation(int tableid)
        {
            if (getTableById(tableid) != null)
            {
                needToLoadRelation = true;
                this.InvalidateVisual();
            }
        }

        void refreshTableRelation()
        {
            m_canvas.Children.Clear();
            try
            {
                var delconfigs = Helper.Client.InvokeSync<EJ.DBDeleteConfig[]>("GetDeleteConfigInModule", this.ModuleNode.Module.id.Value);
                foreach (var configItem in delconfigs)
                {
                    UI.Table table = getTableById(configItem.TableID.GetValueOrDefault());
                    if (table == null)
                        continue;
                    UI.Table relaTable = getTableById(configItem.RelaTableID.GetValueOrDefault());
                    if (relaTable == null)
                        continue;

                    System.Drawing.Point pointFrom = table.GetColumnLocation(0);
                    System.Drawing.Rectangle rectFrom = new System.Drawing.Rectangle((int)table.Margin.Left, (int)table.Margin.Top, (int)table.ActualWidth, (int)table.ActualHeight);
                    System.Drawing.Point pointTo = relaTable.GetColumnLocation(configItem.RelaColumID.GetValueOrDefault());
                    System.Drawing.Rectangle rectTo = new System.Drawing.Rectangle((int)relaTable.Margin.Left, (int)relaTable.Margin.Top, (int)relaTable.ActualWidth, (int)relaTable.ActualHeight);

                    if (rectFrom.Right < rectTo.Left)
                    {
                        pointFrom = new System.Drawing.Point(pointFrom.X + rectFrom.Width, pointFrom.Y);
                    }
                    if (rectTo.Right < rectFrom.Left)
                    {
                        pointTo = new System.Drawing.Point(pointTo.X + rectTo.Width, pointTo.Y);
                    }
                    System.Drawing.Point[] points = PathBuilder.GetDirectPath(rectFrom, pointFrom, rectTo, pointTo);

                    for (int i = 0; i < points.Length - 1; i++)
                    {
                        if (i == points.Length - 2)
                        {
                            var arrow = new Shapes.Arrow();
                            arrow.Stroke = Brushes.Red;
                            arrow.StrokeThickness = 2;
                            arrow.X1 = points[i].X;
                            arrow.Y1 = points[i].Y;
                            arrow.X2 = points[i + 1].X;
                            arrow.Y2 = points[i + 1].Y;
                            arrow.HeadWidth = 5;
                            arrow.HeadHeight = 5;
                            m_canvas.Children.Add(arrow);
                        }
                        else
                        {
                            Line line = new Line();
                            line.Stroke = new SolidColorBrush(Color.FromArgb(255, 136, 181, 244));
                            line.StrokeThickness = 2;
                            line.X1 = points[i].X;
                            line.Y1 = points[i].Y;
                            line.X2 = points[i + 1].X;
                            line.Y2 = points[i + 1].Y;
                            m_canvas.Children.Add(line);
                        }
                    }
                }
            }
            catch(Exception ex)
            {
                Helper.ShowError(ex);
            }
        }

        void m_Grid_Drop(object sender, System.Windows.DragEventArgs e)
        {
            e.Handled = true;
            Point point = e.GetPosition(m_Grid);
            DBTableNode tableNode = e.Data.GetData(typeof(DBTableNode)) as DBTableNode;
            if (tableNode != null)
            {
                AddTable2Me(tableNode, point); 
            }
        }

        public void AddTable2Me(DBTableNode tableNode, Point point)
        {
            EJ.TableInModule tInM = new EJ.TableInModule();
            tInM.x = Math.Max( 0 , Convert.ToInt32(point.X));
            tInM.y = Math.Max(0 ,  Convert.ToInt32(point.Y));
            tInM.TableID = tableNode.Table.id;
            tInM.ModuleID = this.ModuleNode.Module.id;
            try
            {
                var columns = Helper.Client.InvokeSync<EJ.DBColumn[]>("GetColumns", tableNode.Table.id.Value);
                tInM.id = Helper.Client.InvokeSync<int>("UpdateTableInMoudle", tInM);
                tInM.ChangedProperties.Clear();

                UI.Table uiTable = new Table(this);
                uiTable.TableInModule = tInM;
                uiTable.Margin = new Thickness(point.X, point.Y, 0, 0);
                uiTable.DataSource = new Table._DataSource()
                {
                    Table = tableNode.Table,
                    Columns = columns,
                };
                uiTable.DataBind();
                uiTable.MoveCompleted += uiTable_MoveCompleted;
                m_Grid.Children.Add(uiTable);
            }
            catch (Exception ex)
            {
                MessageBox.Show(MainWindow.instance, ex.Message);
            }
        }

        void uiTable_MoveCompleted(object sender, EventArgs e)
        {
            UI.Table uitable = sender as UI.Table;
            uitable.TableInModule.x = Convert.ToInt32(uitable.Margin.Left);
            uitable.TableInModule.y = Convert.ToInt32(uitable.Margin.Top);

            Helper.Client.InvokeSync<int>("UpdateTableInMoudle", uitable.TableInModule);

            refreshTableRelation();
        }

    }
}
