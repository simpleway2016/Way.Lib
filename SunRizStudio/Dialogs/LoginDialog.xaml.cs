using SunRizServer;
using SunRizStudio.Documents;
using SunRizStudio.Listeners;
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
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace SunRizStudio.Dialogs
{
    /// <summary>
    /// LoginDialog.xaml 的交互逻辑
    /// </summary>
    public partial class LoginDialog : Window
    {
        public LoginDialog()
        {
            InitializeComponent();
            this.DataContext = Helper.Config;
        }

        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {

            this.Cursor = Cursors.Wait;

            Helper.Remote.Invoke<SunRizServer.UserInfo>("Login", (ret, err) =>
            {
                this.Cursor = null;
                if (err != null)
                {
                    MessageBox.Show(this, err);
                }
                else
                {
                    Helper.Config.Save();
                    Helper.CurrentUser = ret;
                    if (ret.Role.Value.HasFlag(SunRizServer.UserInfo_RoleEnum.Designer))//拥有设计师资格
                    {
                        App.Current.MainWindow = new MainWindow();
                        App.Current.MainWindow.Show();
                        App.Current.MainWindow.Closed += MainWindow_Closed;
                        this.Close();
                    }
                    else if (ret.Role.Value.HasFlag(SunRizServer.UserInfo_RoleEnum.User))//拥有用户资格
                    {
                        //App.Current.MainWindow = new Dialogs.HistoryWindow();
                        //App.Current.MainWindow.Show();
                        //this.Close();

                        this.Cursor = Cursors.Wait;
                        Helper.Remote.Invoke<ControlWindow>("GetStartupWindow", (startupDataModel, error) =>
                        {
                            this.Cursor = null;
                            if (error != null)
                            {
                                MessageBox.Show(this, err);
                                return;
                            }
                            if(startupDataModel == null)
                            {
                                MessageBox.Show(this, "请先将某一个监控画面设置为启动画面！");
                                return;
                            }
                            var doc = new ControlWindowDocument(null, startupDataModel, true);
                            var startupWindow = new Window();
                            startupWindow.Content = doc;
                            startupWindow.Owner = this.GetParentByName<Window>(null);
                            startupWindow.GoFullscreen();
                            startupWindow.Closed += (s, e2) => {
                                bool c = false;
                                doc.OnClose(ref c);
                            };
                            startupWindow.Show();
                            App.Current.MainWindow = startupWindow;
                            App.Current.MainWindow.Closed += MainWindow_Closed;
                            this.Close();

                            AlarmListener.Alarmed += AlarmListener_Alarmed;
                            AlarmListener.Start(App.Current.MainWindow.Dispatcher);
                        });

                    }

                }
            }, txtName.Text, txtPwd.Password);
        }

        private static void MainWindow_Closed(object sender, EventArgs e)
        {
            System.Diagnostics.Process.GetCurrentProcess().Kill();
        }

        static Dialogs.AlarmWindow alarmWindow;
        /// <summary>
        /// 发现有新的报警
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void AlarmListener_Alarmed(object sender, EventArgs e)
        {
            if (alarmWindow == null)
            {
                alarmWindow = new Dialogs.AlarmWindow();

                //获取窗口句柄 
                var handle = new WindowInteropHelper(alarmWindow).Handle;
                //获取当前显示器屏幕
                var screen = System.Windows.Forms.Screen.FromHandle(handle);
                alarmWindow.Width = screen.WorkingArea.Width;
                alarmWindow.Height = 300;
                alarmWindow.Left = 0;
                alarmWindow.Top = screen.WorkingArea.Height - alarmWindow.Height;

            }
            MainWindow.showWindow(alarmWindow);
           
        }
    }
}
