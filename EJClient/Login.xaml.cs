using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace EJClient
{
    /// <summary>
    /// Login.xaml 的交互逻辑
    /// </summary>
    public partial class Login : Window
    {
        class logininfo
        {
            public string url;
            public string username;
        }
        public Login()
        {
            InitializeComponent();
            try
            {
                var logininfo = System.IO.File.ReadAllText(AppDomain.CurrentDomain.BaseDirectory + "url.txt").ToJsonObject<logininfo>();
                txtAddress.Text = logininfo.url;
                txtUserName.Text = logininfo.username;
            }
            catch
            {
            }
        }

        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            this.Cursor = Cursors.Wait;
            try
            {
                string url = txtAddress.Text;
                while (url.EndsWith("/"))
                    url = url.Substring(0, Helper.WebSite.Length - 1);
                Helper.Client = new Net.RemotingClient(url);
                Helper.Client.Invoke<int[]>("Login", (result, error) =>
               {
                   this.Cursor = null;
                   if (error != null)
                   {
                       MessageBox.Show(this, error);
                   }
                   else
                   {
                       Helper.CurrentUserRole = (EJ.User_RoleEnum)result[0];
                       Helper.CurrentUserID = result[1];
                       System.IO.File.WriteAllText(AppDomain.CurrentDomain.BaseDirectory + "url.txt", new logininfo()
                       {
                           url = Helper.WebSite,
                           username = txtUserName.Text.Trim(),
                       }.ToJsonString());
                       if (Helper.CurrentUserRole == EJ.User_RoleEnum.客户端测试人员)
                       {
                           Application.Current.MainWindow = new Forms.BugCenter.BugRecorder();
                       }
                       else
                       {
                           Application.Current.MainWindow = new MainWindow();
                       }
                       Application.Current.MainWindow.Show();
                       this.Close();
                   }

               }, txtUserName.Text.Trim(), txtPwd.Password);
                
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message);
            }
        }
    }
}
