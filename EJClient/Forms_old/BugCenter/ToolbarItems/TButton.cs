using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace EJClient.Forms.BugCenter.ToolbarItems
{
    public class TButton : TextBlock,IToolBarItem
    {
        bool _IsActived = false;
        public virtual bool IsActived
        {
            get
            {
                return _IsActived;
            }
            set
            {
                if (_IsActived != value)
                {
                    _IsActived = value;
                    if (value == false)
                    {
                        BugEditor.instance.mainGrid.Cursor = null;
                        this.Background = null;
                    }
                    else
                    {
                        BugEditor.instance.mainGrid.Cursor = Cursors.IBeam;
                        this.Background = new SolidColorBrush(Color.FromArgb(100, 53, 111, 243));
                    }
                }
            }
        }


        public void Do(Point location)
        {
            TextBox t = new myTextBox();
            t.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
            t.VerticalAlignment = System.Windows.VerticalAlignment.Top;
            t.Margin = new Thickness(location.X,location.Y , 0,0);
            t.FontSize = 15;
            t.TextWrapping = System.Windows.TextWrapping.Wrap;
            t.AcceptsReturn = true;
            t.Foreground = Brushes.Red;
            t.LostFocus += t_LostFocus;
            t.Background = null;
            t.BorderThickness = new Thickness(0);

            BugEditor.instance.IsSelectCurrentPic = true;
            new Thread(() =>
                {
                    System.Threading.Thread.Sleep(100);
                    BugEditor.instance.Dispatcher.Invoke(new Action(() =>
                    {
                        BugEditor.instance.mainGrid.Children.Add(t);
                            t.Focus();
                        }));
                }).Start();
           
        }

        void t_LostFocus(object sender, RoutedEventArgs e)
        {
            //TextBox t = sender as TextBox;
            //if (t.Text.Trim().Length == 0)
            //{
            //    if(t.Parent != null)
            //    ((Panel)t.Parent).Children.Remove(t);
            //}
        }
    }

    public class myTextBox : TextBox
    {
        #region 移动
        Point m_oldMousePoint;
        bool m_titleMoving = false;
        Thickness m_oldLocation;
        protected override void OnMouseDown(MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Right)
            {
                e.MouseDevice.Capture(this);
                m_oldMousePoint = e.GetPosition(this.Parent as IInputElement);
                m_titleMoving = true;
                m_oldLocation = this.Margin;
                e.Handled = true;
                return;
            }
            base.OnMouseDown(e);
        }
        protected override void OnMouseMove(MouseEventArgs e)
        {
           
            if (m_titleMoving)
            {
                e.Handled = true;
                Point p = e.GetPosition(this.Parent as IInputElement);
                this.Margin = new Thickness(m_oldLocation.Left + p.X - m_oldMousePoint.X, m_oldLocation.Top + p.Y - m_oldMousePoint.Y, 0, 0);
                return;
            }
            base.OnMouseMove(e);
        }
        protected override void OnMouseUp(MouseButtonEventArgs e)
        {
          
            if (m_titleMoving)
            {
                e.Handled = true;
                e.MouseDevice.Capture(null);
                m_titleMoving = false;
                return;
            }
            base.OnMouseUp(e);
        }
        #endregion
    }
}
