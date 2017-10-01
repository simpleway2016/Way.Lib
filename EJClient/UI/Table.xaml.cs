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

namespace EJClient.UI
{
    /// <summary>
    /// Table.xaml 的交互逻辑
    /// </summary>
    public partial class Table : UserControl
    {
        class MyTextBox : TextBox
        {
            public MyTextBox()
            {
                this.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
            }
            protected override void OnMouseUp(MouseButtonEventArgs e)
            {
                this.SelectAll();
                base.OnMouseUp(e);
            }
        }

        public event EventHandler MoveCompleted;
        public class _DataSource
        {
            public EJ.DBTable Table;
            public EJ.DBColumn[] Columns;
        }
        public _DataSource DataSource
        {
            get;
            set;
        }
        internal ModuleDocument OwnerDocument
        {
            get;
            private set;
        }
        public EJ.TableInModule TableInModule
        {
            get;
            set;
        }
        internal Table(ModuleDocument ownerDocument)
        {
            this.OwnerDocument = ownerDocument;
            InitializeComponent();
            this.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
            this.VerticalAlignment = System.Windows.VerticalAlignment.Top;

        }
       
       
        /// <summary>
        /// 从服务器更新当前UI
        /// </summary>
        public void ReBindFromServer()
        {
        }

        public System.Drawing.Point GetColumnLocation(int columnid)
        {
            System.Windows.Point wpfPoint = new Point(0, 10);
            if (columnid == 0)
            {
                var pkcolumn = DataSource.Columns.FirstOrDefault(m => m.IsPKID == true);
                if (pkcolumn != null)
                {
                    columnid = pkcolumn.id.GetValueOrDefault();
                }
            }

            foreach (FrameworkElement ctrl in gridColumns.Children)
            {
                if (ctrl.Tag != null && ctrl.Tag.Equals(columnid))
                {
                    System.Windows.Point p2 = ctrl.TranslatePoint(new Point(0, ctrl.ActualHeight / 2), this);
                    wpfPoint = new Point( 0 , p2.Y );
                    break;
                }
            }
            return new System.Drawing.Point(Convert.ToInt32(wpfPoint.X), Convert.ToInt32(wpfPoint.Y));
        }

        bool _IsSelected = false;
        public bool IsSelected
        {
            get
            {
                return _IsSelected;
            }
            set
            {
                if (_IsSelected != value)
                {
                    _IsSelected = value;
                    if (value)
                    {
                        titleArea.Background = new SolidColorBrush(Color.FromArgb(255, 249, 202, 111));
                        txtTitle.Foreground = Brushes.Black;
                    }
                    else
                    {
                        txtTitle.Foreground = Brushes.White;
                        titleArea.Background = new SolidColorBrush(Color.FromArgb(255, 83, 139, 217));
                    }
                }
            }
        }

    
        void setTextBoxStyle(TextBox t,int column)
        {
            t.IsReadOnly = true;
            t.BorderThickness = new Thickness(0);
            Grid.SetColumn(t, column);
            t.Background = Brushes.Transparent;
            t.Height = 21;
            if (column == 3)
            {
                t.Margin = new Thickness(5, 0, 5, 0);
            }
            else
            {
                t.Margin = new Thickness(5, 0, 0, 0);
            }
            t.Padding = new Thickness(0, 1, 0, 0);
            //IsReadOnly="True" BorderThickness="0" Grid.Column="0" Background="Transparent" Height="21" Margin="5,0,0,0" Padding="0,1,0,0" Text="Test caption"></TextBox>
        }

        protected override void OnMouseDoubleClick(MouseButtonEventArgs e)
        {
            e.Handled = true;
            TreeNode.DBTableNode.AllTableNodes.FirstOrDefault(m=>m.Table.id == this.DataSource.Table.id).OnDoubleClick(this,e);
        }

        /// <summary>
        /// 绑定显示数据
        /// </summary>
        public void DataBind()
        {
            if (this.DataSource != null)
            {
                txtTitle.Text = this.DataSource.Table.Name + " " + DataSource.Table.caption;
                gridColumns.RowDefinitions.Clear();
                gridColumns.Children.Clear();
                for (int i = 0; i < DataSource.Columns.Length; i++)
                {
                    RowDefinition rowdef = new RowDefinition();
                    rowdef.Height = GridLength.Auto;
                    gridColumns.RowDefinitions.Add(rowdef);

                    EJ.DBColumn Column = DataSource.Columns[i];

                    TextBox t1 = new MyTextBox();
                    t1.Tag = Column.id.GetValueOrDefault();

                    if (Column.caption != null && Column.caption.Contains(","))
                        t1.Text = Column.caption.Substring(0, Column.caption.IndexOf(","));
                    else if (Column.caption != null && Column.caption.Contains("，"))
                        t1.Text = Column.caption.Substring(0, Column.caption.IndexOf("，"));
                    else
                        t1.Text = Column.caption;


                   
                    setTextBoxStyle(t1 , 0);
                    Grid.SetRow(t1, i);
                    gridColumns.Children.Add(t1);

                    TextBox t2 = new MyTextBox();
                    t2.Text = Column.Name;
                    setTextBoxStyle(t2, 1);
                    Grid.SetRow(t2, i);
                    gridColumns.Children.Add(t2);

                    TextBox t3 = new MyTextBox();
                    t3.Text = Column.dbType;
                    setTextBoxStyle(t3, 2);
                    Grid.SetRow(t3, i);
                    gridColumns.Children.Add(t3);

                    TextBox t4 = new MyTextBox();
                    t4.Text = Column.length;
                    setTextBoxStyle(t4, 3);
                    Grid.SetRow(t4, i);
                    gridColumns.Children.Add(t4);
                }
            }
        }

        #region 移动
        Point m_oldMousePoint;
        bool m_titleMoving = false;
        Thickness m_oldLocation;
        private void titleArea_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                e.MouseDevice.Capture(this.titleArea);
                m_oldMousePoint = e.GetPosition(this.Parent as IInputElement);
                m_titleMoving = true;
                m_oldLocation = this.Margin;
                e.Handled = true;
            }
        }

        private void titleArea_MouseMove(object sender, MouseEventArgs e)
        {
            if (m_titleMoving)
            {
                e.Handled = true;
                Point p = e.GetPosition(this.Parent as IInputElement);
                this.Margin = new Thickness( m_oldLocation.Left + p.X - m_oldMousePoint.X , m_oldLocation.Top + p.Y - m_oldMousePoint.Y , 0,0 );
            }
        }

        private void titleArea_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (m_titleMoving)
            {
                e.Handled = true;
                e.MouseDevice.Capture(null);
                m_titleMoving = false;
                if (MoveCompleted != null)
                {
                    MoveCompleted(this, null);
                }
            }
        }
        #endregion

        private void MenuItem_ViewData_1(object sender, RoutedEventArgs e)
        {
            Forms.DataViewer frm = new Forms.DataViewer(this.DataSource.Table);
            frm.Show();
        }
        private void MenuItem_移除_1(object sender, RoutedEventArgs e)
        {
            try
            {
                if (MessageBox.Show("确定从当前模块移除吗？", "", MessageBoxButton.OKCancel) == MessageBoxResult.OK)
                {
                    Helper.Client.InvokeSync<string>("RemoveTableFromModule", this.TableInModule.id.Value, this.DataSource.Table.id.Value);
                    ((Panel)this.Parent).Children.Remove(this);
                }
            }
            catch (Exception ex)
            {
                Helper.ShowError(ex);
            }
        }
    }
}
