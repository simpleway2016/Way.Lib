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

namespace EJClient.Forms
{
    /// <summary>
    /// ChangePassword.xaml 的交互逻辑
    /// </summary>
    public partial class ChangePassword : Window
    {
        public ChangePassword()
        {
            InitializeComponent();
        }

        private async void Button_Click_1(object sender, RoutedEventArgs e)
        {
            if (txtNewPwd.Password != pwdConfirm.Password)
                throw new Exception("密码确认不一致");

            this.Cursor = Cursors.Wait;
            Helper.Client.Invoke<int>("ChangePassword", (r, err) => {
                this.Cursor = null;
                if (err != null)
                    Helper.ShowError(this, err);
                else
                {
                    Helper.ShowMessage(this, "成功修改密码!");
                    this.Close();
                }
            }, txtOldPwd.Password, txtNewPwd.Password);

        }
    }
}
