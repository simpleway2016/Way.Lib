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
    /// ModifyPwd.xaml 的交互逻辑
    /// </summary>
    public partial class ModifyPwd : Window
    {
        public ModifyPwd()
        {
            InitializeComponent();
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void btnOK_Click(object sender, RoutedEventArgs e)
        {
            if(txtPwd.Password != txtPwd2.Password)
            {
                MessageBox.Show(this,"密码确认不一致");
                return;
            }
            this.Cursor = Cursors.Wait;
            Helper.Remote.Invoke<int>("ModifyPassword", (ret, err) =>
            {
                this.Cursor = null;
                if (err != null)
                {
                    MessageBox.Show(this, err);
                }
                else
                {
                    MessageBox.Show(this, "修改成功！");
                    this.Close();
                }
            }, txtOld.Password, txtPwd.Password);
        }
    }
}
