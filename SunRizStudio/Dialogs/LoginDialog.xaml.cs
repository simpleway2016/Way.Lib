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
                    App.Current.MainWindow = new MainWindow();
                    App.Current.MainWindow.Show();
                    this.Close();
                }
            },txtName.Text,txtPwd.Password);
        }
    }
}
