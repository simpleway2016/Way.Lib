using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
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
    /// UserManager_Editor.xaml 的交互逻辑
    /// </summary>
    public partial class UserManager_Editor : Window
    {
        EJ.User _data;
        EJ.User _old;
        Func<EJ.User, int> _insertSuccessFunc;
        public UserManager_Editor()
        {
            InitializeComponent();
        }
        public UserManager_Editor(EJ.User user , Func<EJ.User,int> insertSuccessFunc) : this(user)
        {
            _insertSuccessFunc = insertSuccessFunc;
        }
        public UserManager_Editor(EJ.User user):this()
        {
            
            _data = user;
            if(_data == null)
            {
                _data = new EJ.User();
                _data.Password = "123456";
                _data.Role = EJ.User_RoleEnum.数据库设计师;
                lbl1.Content = "密码（不填默认为123456）";
            }
            else
            {
                _old = _data.Clone<EJ.User>();
            }
            var fields = new List<FieldInfo>(typeof(EJ.User_RoleEnum).GetFields());
            fields.RemoveAt(0);
            cmbRoles.ItemsSource = from m in fields
                                    select new
                                    {
                                        Text = m.Name,
                                        Value = m.GetValue(null),
                                    };
            main.DataContext = _data;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if(txtPwd.Password.Length > 0)
            {
                _data.Password = txtPwd.Password;
            }
            this.Cursor = Cursors.Wait;
            Helper.Client.Invoke<int>("UpdateUser" , (id,err)=> {
                this.Cursor = null;
                if (err != null)
                {
                    Helper.ShowError(this, err);
                }
                else
                {
                    if (_insertSuccessFunc != null)
                    {
                        _data.id = id;
                        _insertSuccessFunc(_data);
                    }
                    _old = null;
                    this.DialogResult = true;
                }
            } , _data);
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            if(_old != null)
            {
                _data.Name = _old.Name;
                _data.Role = _old.Role;
            }
            _data.ChangedProperties.Clear();
            base.OnClosing(e);
        }
    }
}
