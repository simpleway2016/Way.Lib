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
    /// UserManager.xaml 的交互逻辑
    /// </summary>
    public partial class UserManager : Window
    {
        System.Collections.ObjectModel.ObservableCollection<EJ.User> _users;
        public UserManager()
        {
            InitializeComponent();
            bindList();
        }
        
        void bindList()
        {
            this.Cursor = Cursors.Wait;
            Helper.Client.Invoke<EJ.User[]>("GetUsers" , (users,error)=> {
                this.Cursor = null;
                if (error != null)
                    Helper.ShowError(error);
                else
                {
                    _users = new System.Collections.ObjectModel.ObservableCollection<EJ.User>(users);
                    list.ItemsSource = _users;
                }
            });
        }

        private void edit_Click(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;
            if( new UserManager_Editor(btn.Tag as EJ.User) { Owner = this}.ShowDialog() == true )
            {
                
            }
        }

        private void powersetting_click(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;
            new UserManager_PowerSetting((btn.Tag as EJ.User).id.Value) { Owner = this }.ShowDialog();
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            new UserManager_Editor(null,(newuser)=>
            {
                _users.Add(newuser);
                return 0;
            })
            { Owner = this }.ShowDialog();
        }

        private void btnDel_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("确定删除吗？", "", MessageBoxButton.OKCancel) == MessageBoxResult.OK)
            {
                if (list.SelectedItems.Count == 0)
                {
                    Helper.ShowError("请选择需要删除的用户");
                    return;
                }
                var delUsers = new EJ.User[list.SelectedItems.Count];
                for (var i = 0; i < list.SelectedItems.Count; i++)
                {
                    var user = list.SelectedItems[i];
                    delUsers[i] = (EJ.User)user;
                }
                Helper.Client.Invoke<int>("DeleteUsers", (r, err) =>
                {
                    if (err != null)
                        Helper.ShowError(err);
                    else
                    {
                        foreach (var u in delUsers)
                            _users.Remove(u);
                    }
                }, delUsers.Select(m => m.id.Value).ToArray());
            }
        }
    }
}
