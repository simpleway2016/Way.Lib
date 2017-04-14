using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using WPFArrows.Arrows;

namespace EJClient.UI
{
    class DescriptionView : Border
    {
        internal class Connect
        {
            public ArrowFromWhere ArrowFromWhere = ArrowFromWhere.LeftRight;
            public int ToWhere;
            public string Text;
            public double x1;
            public double y1;
            public double x2;
            public double y2;
        }
        public enum ArrowFromWhere
        {
            LeftRight = 0,
            TopBottom = 1,
        }
        class JsonObject
        {
            public string Content;
            public List<Connect> Connects = new List<Connect>();
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
                    if (!value)
                    {
                        this.Background = new SolidColorBrush(Color.FromRgb(211, 225, 251));
                    }
                    else
                    {
                        this.Background = new SolidColorBrush(Color.FromRgb(255, 212, 195));
                    }
                }
            }
        }


        JsonObject m_jsonObj = new JsonObject();
        TextBlock mText;
        Grid m_grid;
        Ellipse ellipse;
        internal EJ.InterfaceInModule InterfaceInModule;
        ContextMenu m_ArrowMenu;


        public DescriptionView(string content, EJ.InterfaceInModule interfaceInModule)
        {
            m_jsonObj.Content = content;
            this.InterfaceInModule = interfaceInModule;

            this.BorderBrush = Brushes.Black;
            this.Background = new SolidColorBrush(Color.FromRgb(211,225,251));
            this.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
            this.VerticalAlignment = System.Windows.VerticalAlignment.Top;
            this.BorderThickness = new System.Windows.Thickness(1);
            this.CornerRadius = new System.Windows.CornerRadius(5);


            mText = new TextBlock();
            mText.Text = content;
            mText.IsHitTestVisible = false;
            mText.Margin = new Thickness(10);
            mText.HorizontalAlignment = System.Windows.HorizontalAlignment.Center;
            mText.VerticalAlignment = System.Windows.VerticalAlignment.Center;

            Grid grid = new Grid();
            grid.Children.Add(mText);

            ellipse = new Ellipse();
            ellipse.Stroke = Brushes.Black;
            ellipse.StrokeThickness = 1;
            ellipse.Fill = Brushes.White;
            ellipse.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
            ellipse.VerticalAlignment = System.Windows.VerticalAlignment.Top;
            ellipse.Width = 8;
            ellipse.Height = 8;
            ellipse.Visibility = System.Windows.Visibility.Hidden;
            ellipse.MouseDown += ellipse_MouseDown;
            grid.Children.Add(ellipse);


            m_grid = grid;
            this.Child = grid;

            CreateContextMenu();

            m_ArrowMenu = new ContextMenu();
            MenuItem menuitem = new MenuItem();
            menuitem.Header = "删除";
            menuitem.Click += menuitem_deleteArrow_Click;
            m_ArrowMenu.Items.Add(menuitem);
        }

        void menuitem_deleteArrow_Click(object sender, RoutedEventArgs e)
        {
            ArrowLineWithText arrow = (ArrowLineWithText)m_ArrowMenu.PlacementTarget;
            Connect connect = (Connect)arrow.Tag;
            this.m_jsonObj.Connects.Remove(connect);
            try
            {
                InterfaceInModule.JsonData = m_jsonObj.ToJsonString();
                Helper.Client.InvokeSync<string>("UpdateInterfaceInModule", InterfaceInModule);

                m_grid.Children.Remove(arrow);
            }
            catch (Exception ex)
            {
                MessageBox.Show(MainWindow.instance, ex.Message);
            }
        }
        internal bool ResetConnect(Connect connect)
        {
            ArrowLineWithText arrow = null;
            foreach (var item in m_grid.Children)
            {
                if (item is ArrowLineWithText)
                {
                    arrow = (ArrowLineWithText)item;
                    if (arrow.Tag == connect)
                    {
                        break;
                    }
                    arrow = null;
                }
            }
            if (arrow == null)
            {
                return false;
            }
            Panel parent = this.Parent as Panel;
            DescriptionView target = null;
            foreach (FrameworkElement item in parent.Children)
            {
                if (item.Equals(this) == false && item is DescriptionView)
                {
                    target = item as DescriptionView;
                    if (target.InterfaceInModule.id == connect.ToWhere)
                    {
                        break;
                    }
                    target = null;
                }
            }
            if (target == null)
            {
                //目标没有了，留个箭头干嘛
                m_grid.Children.Remove(arrow);
                return false;
            }
            if (connect.ArrowFromWhere == ArrowFromWhere.LeftRight)
            {
                if (target.Margin.Left > this.Margin.Left)
                {
                    arrow.StartPoint = new Point(this.ActualWidth, connect.y1);
                    Point p = target.TranslatePoint(new Point(0, connect.y2), this);
                    arrow.EndPoint = new Point(p.X, p.Y);
                }
                else
                {
                    arrow.StartPoint = new Point(0, connect.y1);
                    Point p = target.TranslatePoint(new Point(target.ActualWidth, connect.y2), this);
                    arrow.EndPoint = new Point(p.X, p.Y);
                }
            }
            else
            {
                if (target.Margin.Top > this.Margin.Top)
                {
                    arrow.StartPoint = new Point(connect.x1, this.ActualHeight);
                    Point p = target.TranslatePoint(new Point(connect.x2, 0), this);
                    arrow.EndPoint = new Point(p.X, p.Y);
                }
                else
                {
                    arrow.StartPoint = new Point(connect.x1, 0);
                    Point p = target.TranslatePoint(new Point(connect.x2, target.ActualHeight), this);
                    arrow.EndPoint = new Point(p.X, p.Y);
                }
            }
            if (!string.IsNullOrEmpty(arrow.Text))
                arrow.InvalidateVisual();
            return true;

        }
        internal void AddConnect(Connect connect)
        {
            ArrowLineWithText arrow = new ArrowLineWithText();
            arrow.MouseDown += arrow_MouseDown;
            arrow.MouseMove += arrow_MouseMove;
            arrow.MouseUp += arrow_MouseUp;
            arrow.Tag = connect;
            Panel parent = this.Parent as Panel;
            DescriptionView target = null;
            foreach (FrameworkElement item in parent.Children)
            {
                if ( item.Equals(this) == false && item is DescriptionView)
                {
                    target = item as DescriptionView;
                    if (target.InterfaceInModule.id == connect.ToWhere)
                    {
                        break;
                    }
                    target = null;
                }
            }
            if (target == null)
                return;
            if (connect.ArrowFromWhere == ArrowFromWhere.LeftRight)
            {
                if (target.Margin.Left > this.Margin.Left)
                {
                    arrow.StartPoint = new Point(this.ActualWidth, connect.y1);
                    Point p = target.TranslatePoint(new Point(0, connect.y2), this);
                    arrow.EndPoint = new Point(p.X, p.Y);
                }
                else
                {
                    arrow.StartPoint = new Point(0, connect.y1);
                    Point p = target.TranslatePoint(new Point(target.ActualWidth, connect.y2), this);
                    arrow.EndPoint = new Point(p.X, p.Y);
                }
            }
            else
            {
                if (target.Margin.Top > this.Margin.Top)
                {
                    arrow.StartPoint = new Point(connect.x1 , this.ActualHeight);
                    Point p = target.TranslatePoint(new Point(connect.x2 , 0), this);
                    arrow.EndPoint = new Point(p.X, p.Y);
                }
                else
                {
                    arrow.StartPoint = new Point(connect.x1, 0);
                    Point p = target.TranslatePoint(new Point(connect.x2, target.ActualHeight), this);
                    arrow.EndPoint = new Point(p.X, p.Y);
                }
            }
            arrow.Stroke = Brushes.Blue;
            arrow.StrokeThickness = 1;
            arrow.Cursor = Cursors.Hand;
            arrow.TextAlignment = TextAlignment.Center;
            arrow.Margin = new Thickness(0, 0, -100000, -10000000);
            arrow.IsTextUp = true;
            arrow.Text = connect.Text;
            m_grid.Children.Add(arrow);

                 
        }
        void CreateContextMenu()
        {
            ContextMenu menu = new ContextMenu();
            MenuItem menuitem = new MenuItem();
            menuitem.Header = "删除";
            menuitem.Click += menuitem_删除_Click;
            menu.Items.Add(menuitem);

            this.ContextMenu = menu;
        }
        private void menuitem_删除_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (MessageBox.Show("确定删除吗？", "", MessageBoxButton.OKCancel) == MessageBoxResult.OK)
                {
                    Helper.Client.InvokeSync<string>("DeleteInterfaceInModule", this.InterfaceInModule);
                    Panel parent = this.Parent as Panel;
                    parent.Children.Remove(this);

                    foreach (FrameworkElement control in parent.Children)
                    {
                        if (control is DescriptionView)
                        {
                            ((DescriptionView)control).ResetConnects();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        void arrow_MouseUp(object sender, MouseButtonEventArgs e)
        {
            e.Handled = true;
            if (e.ChangedButton == MouseButton.Right)
            {
                m_ArrowMenu.PlacementTarget = sender as ArrowLineWithText;
                m_ArrowMenu.IsOpen = true;
            }
        }

        void arrow_MouseMove(object sender, MouseEventArgs e)
        {
            e.Handled = true;
        }

        void arrow_MouseDown(object sender, MouseButtonEventArgs e)
        {
           
            e.Handled = true;
            if (e.ChangedButton != MouseButton.Left)
                return;
            ArrowLineWithText arrow = (ArrowLineWithText)sender;
            Connect connect = (Connect)arrow.Tag;

            InputBox frm = new InputBox("请输入内容", "", 200);
            frm.Owner = MainWindow.instance;
            frm.Value = connect.Text;
            if (frm.ShowDialog() == true)
            {

                connect.Text = frm.Value;
                arrow.Text = connect.Text;

                InterfaceInModule.JsonData = m_jsonObj.ToJsonString();
                Helper.Client.InvokeSync<string>("UpdateInterfaceInModule", InterfaceInModule);
            }
        }

        public string GetJsonData()
        {
            return m_jsonObj.ToJsonString();
        }
        public void LoadJsonData(string json)
        {
            m_jsonObj = json.ToJsonObject<JsonObject>();
            mText.Text = m_jsonObj.Content;
           
        }
        void ResetConnects()
        {
            for (int i = 0; i < m_jsonObj.Connects.Count; i ++ )
            {
                if (this.ResetConnect(m_jsonObj.Connects[i]) == false)
                {
                    m_jsonObj.Connects.RemoveAt(i);
                    i--;
                }
            }

        }
        public void LoadConnects()
        {
                foreach (var connect in m_jsonObj.Connects)
                {
                    this.AddConnect(connect);
                }
        }
        void afterMoving()
        {
            //重新调整所有箭头
            Panel parent = this.Parent as Panel;
            DescriptionView target = null;
            foreach (FrameworkElement item in parent.Children)
            {
                if (item.Equals(this) == false && item is DescriptionView)
                {
                    target = item as DescriptionView;
                    if (target.m_jsonObj.Connects.Count(m => m.ToWhere == this.InterfaceInModule.id) > 0)
                    {
                        target.ResetConnects();
                    }
                }
            }
            this.ResetConnects();
        }
        public void OnMoveCompleted()
        {
            
            if (MoveCompleted != null)
            {
                MoveCompleted(this, null);
            }
        }
        #region 连线
        bool m_connecting = false;
        Point m_arrowStartPoint;
        ArrowLineWithText m_connectingArrow;
        ArrowFromWhere m_connectingFromWhere;
        void ellipse_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Panel parent = this.Parent as Panel;
            if (this != parent.Children[parent.Children.Count - 1])
            {
                parent.Children.Remove(this);
                parent.Children.Add(this);
            }
            e.Handled = true;
            e.MouseDevice.Capture(this);
            m_arrowStartPoint = e.GetPosition(m_grid);
            m_connecting = true;
        }

        #endregion

        private void DoubleClick()
        {
            InputBox frm = new InputBox("请输入内容", "修改描述", 200);
            frm.Owner = MainWindow.instance;
            frm.Value = mText.Text;
            if (frm.ShowDialog() == true)
            {
                mText.Text = frm.Value;
                m_jsonObj.Content = frm.Value;
                this.UpdateLayout();
                try
                {
                    InterfaceInModule.JsonData = m_jsonObj.ToJsonString();
                    Helper.Client.InvokeSync<string>("UpdateInterfaceInModule", InterfaceInModule);
                    afterMoving();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(MainWindow.instance, ex.Message);
                }
            }
        }

        #region 移动 | ChangeSize
        public event EventHandler MoveCompleted;

        Point m_oldMousePoint;
        bool m_titleMoving = false;
        bool m_changingSize = false;
        double m_w, m_h;
        Thickness m_oldLocation;
        
        protected override void OnMouseDown(System.Windows.Input.MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Right)
                return;
            if (e.ClickCount > 1)
            {
                e.Handled = true;
                DoubleClick();
                return;
            }

            e.MouseDevice.Capture(this);
            m_oldMousePoint = e.GetPosition(this.Parent as IInputElement);
           
            m_oldLocation = this.Margin;
            e.Handled = true;

            if (this.Cursor == Cursors.SizeNWSE)
            {
                m_w = this.ActualWidth;
                m_h = this.ActualHeight;
                m_changingSize = true;
            }
            else
            {
                m_titleMoving = true;
            }
        }
        protected override void OnMouseLeave(MouseEventArgs e)
        {
            ellipse.Visibility = System.Windows.Visibility.Hidden;
            base.OnMouseLeave(e);
        }
        protected override void OnMouseMove(System.Windows.Input.MouseEventArgs e)
        {
            if (m_titleMoving)
            {
                this.Cursor = null;
                ellipse.Visibility = System.Windows.Visibility.Hidden;
                e.Handled = true;
                Point p = e.GetPosition(this.Parent as IInputElement);
                this.Margin = new Thickness(m_oldLocation.Left + p.X - m_oldMousePoint.X, m_oldLocation.Top + p.Y - m_oldMousePoint.Y, 0, 0);
                afterMoving();
            }
            else if (m_changingSize)
            {
                ellipse.Visibility = System.Windows.Visibility.Hidden;
                e.Handled = true;
                Point p = e.GetPosition(this.Parent as IInputElement);
                this.Width = Math.Max(20 ,  m_w + p.X - m_oldMousePoint.X);
                this.Height = Math.Max(20, m_h + p.Y - m_oldMousePoint.Y);
            }
            else if (m_connecting)
            {
                e.Handled = true;
                if (m_connectingArrow == null)
                {
                    m_connectingArrow = new ArrowLineWithText();
                    m_connectingArrow.StartPoint = m_arrowStartPoint;
                    m_connectingArrow.Stroke = Brushes.Blue;
                    m_connectingArrow.StrokeThickness = 1;
                    m_connectingArrow.TextAlignment = TextAlignment.Center;
                    m_connectingArrow.Margin = new Thickness(0,0,-100000,-10000000);
                    m_connectingArrow.IsTextUp = true;
                    m_grid.Children.Add(m_connectingArrow);
                }

                Point targetPoint = e.GetPosition(m_grid);
                if (Math.Abs(targetPoint.X - m_connectingArrow.StartPoint.X) > 20 && Math.Abs(targetPoint.Y - m_connectingArrow.StartPoint.Y) < 10)
                    targetPoint = new Point(targetPoint.X, m_connectingArrow.StartPoint.Y);
                else if (Math.Abs(targetPoint.Y - m_connectingArrow.StartPoint.Y) > 20 && Math.Abs(targetPoint.X - m_connectingArrow.StartPoint.X) < 10)
                    targetPoint = new Point( m_connectingArrow.StartPoint.X , targetPoint.Y);
                m_connectingArrow.EndPoint = targetPoint;
            }
            else
            {
                Point p = e.GetPosition(this);
                if (p.X > this.ActualWidth - 10 && p.Y > this.ActualHeight - 10)
                {
                    ellipse.Visibility = System.Windows.Visibility.Hidden;
                    this.Cursor = Cursors.SizeNWSE;
                }
                else if (p.X > this.ActualWidth - 10)
                {
                    this.Cursor = null;
                    m_connectingFromWhere = ArrowFromWhere.LeftRight;
                    ellipse.Margin = new Thickness(this.ActualWidth - ellipse.Width / 2, p.Y - ellipse.Height / 2, -100000, -100000);
                    ellipse.Visibility = System.Windows.Visibility.Visible;
                }
                else if (p.X < 10)
                {
                    this.Cursor = null;
                    ellipse.Margin = new Thickness(-ellipse.Width / 2, p.Y - ellipse.Height / 2, -100000, -100000);
                    m_connectingFromWhere = ArrowFromWhere.LeftRight;
                    ellipse.Visibility = System.Windows.Visibility.Visible;
                }
                else if (p.Y > this.ActualHeight - 10)
                {
                    this.Cursor = null;
                    m_connectingFromWhere = ArrowFromWhere.TopBottom;
                    ellipse.Margin = new Thickness(p.X - ellipse.Width / 2, this.ActualHeight - ellipse.Height / 2, -100000, -100000);
                    ellipse.Visibility = System.Windows.Visibility.Visible;
                }
                else if (p.Y < 10)
                {
                    this.Cursor = null;
                    m_connectingFromWhere = ArrowFromWhere.TopBottom;
                    ellipse.Margin = new Thickness(p.X - ellipse.Width / 2, -ellipse.Height / 2, -100000, -100000);
                    ellipse.Visibility = System.Windows.Visibility.Visible;
                }
                else
                {
                    ellipse.Visibility = System.Windows.Visibility.Hidden;
                    this.Cursor = null;
                }
            }
        }
        protected override void OnMouseUp(System.Windows.Input.MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Right)
                return;

            if (m_titleMoving || m_changingSize)
            {
                e.Handled = true;
                e.MouseDevice.Capture(null);
             

                InterfaceInModule.x = (int)this.Margin.Left;
                InterfaceInModule.y = (int)this.Margin.Top;
                if (m_changingSize)
                {
                    m_changingSize = false;
                    InterfaceInModule.width = (int)this.Width;
                    InterfaceInModule.height = (int)this.Height;
                }
                Helper.Client.InvokeSync<string>("UpdateInterfaceInModule", InterfaceInModule);

                if (m_titleMoving)
                {
                    m_titleMoving = false;
                    this.OnMoveCompleted( );

                }
            }
            else if (m_connecting)
            {
                e.Handled = true;
                m_connecting = false;
                e.MouseDevice.Capture(null);

                if (m_connectingArrow != null)
                {
                    Panel parent = this.Parent as Panel;
                    DescriptionView target = null;
                    foreach (FrameworkElement item in parent.Children)
                    {
                        if ( item.Equals(this) == false && item is DescriptionView)
                        {
                            target = item as DescriptionView;
                            Point p = this.TranslatePoint(m_connectingArrow.EndPoint, target);
                            if (p.X >0 && p.Y > 0 && p.X < target.ActualWidth && p.Y < target.ActualHeight)
                            {
                                break;
                            }
                            target = null;
                        }
                    }
                    if (target != null)
                    {
                        Point p = this.TranslatePoint(m_connectingArrow.EndPoint, target);
                        Connect connect = new Connect()
                        {
                            ArrowFromWhere = m_connectingFromWhere,
                            ToWhere = target.InterfaceInModule.id.Value,
                            x1 = m_connectingArrow.StartPoint.X,
                            y1 = m_connectingArrow.StartPoint.Y,
                            x2 = p.X,
                            y2 = p.Y,
                           
                        };
                        this.AddConnect(connect);
                        this.m_jsonObj.Connects.Add(connect);
                        InterfaceInModule.JsonData = m_jsonObj.ToJsonString();
                        try
                        {
                            Helper.Client.InvokeSync<string>("UpdateInterfaceInModule", InterfaceInModule);
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(MainWindow.instance, ex.Message);
                        }
                    }
                    m_grid.Children.Remove(m_connectingArrow);
                    m_connectingArrow = null;
                }
            }
         
        }
        #endregion
    }

}
