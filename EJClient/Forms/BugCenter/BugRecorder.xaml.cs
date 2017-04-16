using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace EJClient.Forms.BugCenter
{
    /// <summary>
    /// BugRecorder.xaml 的交互逻辑
    /// </summary>
    public partial class BugRecorder : Window
    {
        public static BugRecorder instance;
        WayControls.Windows.Hook.MouseHook m_mouseHooker;
        public BugRecorder()
        {
            instance = this;
            InitializeComponent();
            m_mouseHooker = new WayControls.Windows.Hook.MouseHook();
            m_mouseHooker.忽略MouseMove事件 = true;
            m_mouseHooker.OnMouseActivity += m_mouseHooker_OnMouseActivity;
            m_mouseHooker.Start();

            if (Directory.Exists(AppDomain.CurrentDomain.BaseDirectory + "BugPics") == false)
                Directory.CreateDirectory(AppDomain.CurrentDomain.BaseDirectory + "BugPics");
            else
            {
                string[] files = Directory.GetFiles(AppDomain.CurrentDomain.BaseDirectory + "BugPics" , "*.png");
                foreach (string file in files)
                {
                    try
                    {
                        File.Delete(file);
                    }
                    catch
                    {
                    }
                }
            }
            this.Left = SystemParameters.WorkArea.Width - this.Width - 260;
            this.Top = 0;

            //new System.Threading.Thread(readBugCount)
            //{
            //    IsBackground = true,
            //}.Start();
        }

        void readBugCount()
        {

            int bugCount = 0;
            while (true)
            {

                try
                {
                    int count = Helper.Client.InvokeSync<int>("GetMyBugListCount");
                    if (count != bugCount)
                    {
                        bugCount = count;
                        this.Dispatcher.Invoke(delegate
                        {
                            if (count == 0)
                                lblParent.Visibility = System.Windows.Visibility.Collapsed;
                            else
                            {
                                lblParent.Visibility = System.Windows.Visibility.Visible;
                                lblNowCount.Content = count.ToString();
                            }
                        });
                    }
                }
                catch
                {
                }
                Thread.Sleep(2000);
            }
        }

        protected override void OnClosing(System.ComponentModel.CancelEventArgs e)
        {
            try
            {
                m_mouseHooker.Stop();
            }
            catch
            {
            }
            base.OnClosing(e);
        }
        DateTime m_lastPictureTime = DateTime.Now;
        List<string> m_histories = new List<string>();
        System.Drawing.Brush BRUSH = new System.Drawing.SolidBrush(System.Drawing.Color.FromArgb(128, System.Drawing.Color.Yellow.R, System.Drawing.Color.Yellow.G, System.Drawing.Color.Yellow.B));
        bool m_mouseHooker_OnMouseActivity(System.Windows.Forms.MouseButtons button, int clicks, int x, int y, int delta, WayControls.Windows.Hook.MouseHook.MouseActiveType ActiveType)
        {
            if (this.Visibility != System.Windows.Visibility.Visible)
                return true;

            if (ActiveType == WayControls.Windows.Hook.MouseHook.MouseActiveType.MouseDown)
            {
                if ((DateTime.Now - m_lastPictureTime).TotalMilliseconds > 500)
                {
                    if (true)
                    {
                        //Point p = this.PointToScreen(new Point(0, 0));
                        //if (new System.Drawing.Rectangle((int)p.X, (int)p.Y, (int)this.ActualWidth, (int)this.ActualHeight).Contains(x, y))
                        //    return true;
                    }
                    if (menu.IsOpen)
                    {
                        Point p = menu.PointToScreen(new Point(0, 0));
                        if (new System.Drawing.Rectangle((int)p.X, (int)p.Y, (int)menu.ActualWidth, (int)menu.ActualHeight).Contains(x, y))
                            return true;
                    }
                   

                    m_lastPictureTime = DateTime.Now;
                    var bitmap = new System.Drawing.Bitmap((int)SystemParameters.PrimaryScreenWidth, (int)SystemParameters.PrimaryScreenHeight, System.Drawing.Imaging.PixelFormat.Format24bppRgb);

                    using (System.Drawing.Graphics g = System.Drawing.Graphics.FromImage(bitmap))
                    {
                        g.CopyFromScreen(0, 0, 0, 0, bitmap.Size, System.Drawing.CopyPixelOperation.SourceCopy);
                        
                        g.FillEllipse(BRUSH ,new System.Drawing.Rectangle( x - 30 , y - 30 , 60,60 ) );

                        System.Drawing.Point p = System.Windows.Forms.Cursors.Default.HotSpot;
                        System.Drawing.Rectangle rect = new System.Drawing.Rectangle(new System.Drawing.Point(x - p.X, y - p.Y), System.Windows.Forms.Cursors.Default.Size);
                        System.Windows.Forms.Cursors.Default.Draw(g, rect);
                    }
                    string filename = AppDomain.CurrentDomain.BaseDirectory + "BugPics\\" + Guid.NewGuid().ToString() + ".png";
                    bitmap.Save(filename, System.Drawing.Imaging.ImageFormat.Png);
                    bitmap.Dispose();
                    m_histories.Add(filename);
                    if (m_histories.Count > 50)
                    {
                        try
                        {
                            File.Delete(m_histories[0]);
                        }
                        catch
                        {
                        }
                        m_histories.RemoveAt(0);
                    }
                }
            }
            return true;
        }
        protected override void OnMouseDoubleClick(MouseButtonEventArgs e)
        {
            base.OnMouseDoubleClick(e);
        }
        protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonDown(e);
            this.DragMove();
        }

        private void Close_Click_1(object sender, RoutedEventArgs e)
        {
            m_mouseHooker.Stop();
            this.Close();
        }
        void menu修改密码_Click_1(object sender, RoutedEventArgs e)
        {
            ChangePassword frm = new ChangePassword();
            frm.ShowDialog();
        }
        private void menu报告错误_Click_1(object sender, RoutedEventArgs e)
        {
            m_mouseHooker.Stop();
            this.Hide();
            BugSumbit frm = new BugSumbit();
            frm.ShowDialog();
            this.Show();
            m_mouseHooker.Start();
        }
        void menu查看反馈_Click_1(object sender, RoutedEventArgs e)
        {
            m_mouseHooker.Stop();
            using (System.Windows.Forms.Form win = new System.Windows.Forms.Form())
            {
                win.WindowState = System.Windows.Forms.FormWindowState.Maximized;
                var ctrl = new UI.BugListControl();
                ctrl.Dock = System.Windows.Forms.DockStyle.Fill;
                win.Controls.Add(ctrl);
                win.ShowDialog();
            }
            m_mouseHooker.Start();
        }


    }
}
