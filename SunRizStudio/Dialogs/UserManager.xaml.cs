using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using SunRizServer;

namespace SunRizStudio.Dialogs
{
    /// <summary>
    /// UserManager.xaml 的交互逻辑
    /// </summary>
    public partial class UserManager : Window
    {
        ObservableCollection<object> _listSource;
        List<SafeArea> _SafeAreaList = new List<SafeArea>();
        MyUserInfo _currentUser;
        public UserManager()
        {
            InitializeComponent();
            ctrlEditor.Visibility = Visibility.Hidden;

            var items = new List<object>();
            items.Add(new { text = "用户", value = SunRizServer.UserInfo_RoleEnum.User });
            items.Add(new { text = "管理员", value = SunRizServer.UserInfo_RoleEnum.Admin });
            items.Add(new { text = "组态工程师", value = SunRizServer.UserInfo_RoleEnum.Designer });
            cmbRole.ItemsSource = items;


            for(int i = 0; i < 26; i ++)
            {
                _SafeAreaList.Add(new SafeArea {
                    value = 1<<i,
                    text = ((char)(((int)('A')) + i)).ToString(),
                });
            }
            listAlarmGroup.ItemsSource = _SafeAreaList;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            //load user's information
            loadData();

            //窗口一出来就被主窗口挡着，所以这么处理一下
            this.Topmost = true;

            Task.Run(() =>
            {
                System.Threading.Thread.Sleep(500);
                this.Dispatcher.Invoke(() => { this.Topmost = false; });
            });
        }

        void loadData()
        {
            this.Cursor = Cursors.Wait;
            Helper.Remote.Invoke<MyUserInfo[]>("GetUserInfos", (ret, err) =>
            {
                this.Cursor = null;
                if (err != null)
                {
                    MessageBox.Show(this, err);
                }
                else
                {
                    _listSource = new ObservableCollection<object>();
                    foreach (var item in ret)
                        _listSource.Add(item);

                    list.ItemsSource = _listSource;
                }
            });
        }

        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            FrameworkElement button = sender as FrameworkElement;
            _currentUser = (MyUserInfo)button.DataContext;
            foreach (var area in _SafeAreaList)
            {
                area.IsChecked = (_currentUser.SafeArea & area.value) == area.value;
            }
            ctrlEditor.DataContext = _currentUser.Clone<MyUserInfo>();
            ctrlEditor.Visibility = Visibility.Visible;
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            ctrlEditor.Visibility = Visibility.Hidden;
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            this.Cursor = Cursors.Wait;
            if (((MyUserInfo)ctrlEditor.DataContext).id == null && string.IsNullOrEmpty(txtPwd.Password))
            {
                MessageBox.Show(this, "请设置密码");
                return;
            }
            if (string.IsNullOrEmpty(txtPwd.Password) == false)
            {
                if(txtPwd2.Password != txtPwd.Password)
                {
                    MessageBox.Show(this , "密码确认不一致");
                    return;
                }
                //如果已经修改密码
                ((MyUserInfo)ctrlEditor.DataContext).Password = txtPwd.Password;
            }
            int safeArea = 0;
            foreach (var area in _SafeAreaList)
            {
                if(area.IsChecked )
                {
                    safeArea |= area.value;
                }
            }
            ((MyUserInfo)ctrlEditor.DataContext).SafeArea = safeArea;
            Helper.Remote.Invoke<int>("SaveUserInfo", (id, err) =>
            {
                this.Cursor = null;
                if (err != null)
                {
                    MessageBox.Show(this, err);
                }
                else
                {
                    //更新正在编辑的model       
                    _currentUser.CopyValue(ctrlEditor.DataContext as Way.Lib.DataModel);
                    _currentUser.Password = null;
                    if (_currentUser.id == null)
                    {
                        _currentUser.id = id;
                        _listSource.Insert(0, _currentUser);
                    }
                    //回到没有修改的状态
                    _currentUser.ChangedProperties.Clear();

                    ctrlEditor.Visibility = Visibility.Hidden;
                }
            }, ctrlEditor.DataContext);
        }

        private void btnNew_Click(object sender, RoutedEventArgs e)
        {
            _currentUser = new MyUserInfo();
            ctrlEditor.DataContext = _currentUser.Clone<MyUserInfo>();
            foreach( var area in _SafeAreaList )
            {
                area.IsChecked = false;
            }
            ctrlEditor.Visibility = Visibility.Visible;
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("确定删除吗？", "", MessageBoxButton.OKCancel) == MessageBoxResult.OK)
            {
                FrameworkElement button = sender as FrameworkElement;
                _currentUser = (MyUserInfo)button.DataContext;

                this.Cursor = Cursors.Wait;
                Helper.Remote.Invoke<int>("DeleteUserInfo", (id, err) =>
                {
                    this.Cursor = null;
                    if (err != null)
                    {
                        MessageBox.Show(this, err);
                    }
                    else
                    {
                        _listSource.Remove(_currentUser);
                    }
                }, _currentUser);
            }
        }
    }
    class SafeArea : Way.Lib.DataModel
    {
        string _text;
        public string text
        {
            get {
                return _text;
            }
            set
            {
                if(_text != value)
                {
                    _text = value;
                    OnPropertyChanged("text", null, value);
                }
            }
        }
        int _value;
        public int value
        {
            get
            {
                return _value;
            }
            set
            {
                if (_value != value)
                {
                    _value = value;
                    OnPropertyChanged("value", null, value);
                }
            }
        }
        bool _IsChecked;
        public bool IsChecked
        {
            get
            {
                return _IsChecked;
            }
            set
            {
                if (_IsChecked != value)
                {
                    _IsChecked = value;
                    OnPropertyChanged("IsChecked", null, IsChecked);
                }
            }
        }
    }
    class MyUserInfo : SunRizServer.UserInfo
    {
        public override UserInfo_RoleEnum? Role
        {
            get => base.Role; set
            {
                base.Role = value;
                this.OnPropertyChanged("RoleDesc",null,null);
            }
        }
        public string RoleDesc

        {
            get
            {
                switch (Role)
                {
                    case SunRizServer.UserInfo_RoleEnum.Admin:
                        return "管理员";
                    case SunRizServer.UserInfo_RoleEnum.Designer:
                        return "组态工程师";
                    case SunRizServer.UserInfo_RoleEnum.User:
                        return "用户";

                }
                return "";
            }
        }
    }
}
