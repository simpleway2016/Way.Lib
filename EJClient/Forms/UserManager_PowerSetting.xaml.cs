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
    /// UserManager_PowerSetting.xaml 的交互逻辑
    /// </summary>
    public partial class UserManager_PowerSetting : Window
    {
        class SettingProxy
        {
            Way.Lib.DataModel _data;
            string _textField;
            public SettingProxy(Way.Lib.DataModel p,string textField)
            {
                _data = p;
                _textField = textField;
            }
            static void click(object sender , RoutedEventArgs e)
            { }
            public RoutedEventHandler ClickEvent
            {
                get;
                set;
            }
            public bool IsChecked
            {
                get
                {
                    return _data.BackupChangedProperties["HasPower"] != null;
                }
                set
                {
                    if (value)
                        _data.BackupChangedProperties["HasPower"] = new Way.Lib.DataModelChangedItem();
                    else
                        _data.BackupChangedProperties.Clear();
                }
            }
            public SettingProxy[] Children
            {
                get;
                set;
            }
            public string Icon { get; set; }
            public int id
            {
                get
                {
                    return Convert.ToInt32(_data.GetType().GetProperty("id").GetValue(_data));
                }
            }
            Visibility _CheckBoxVisibility = Visibility.Visible;
            public Visibility CheckBoxVisibility
            {
                get
                {
                    return _CheckBoxVisibility;
                }
                set
                {
                    _CheckBoxVisibility = value;
                }
            }
            public string Text
            {
                get
                {
                    return _data.GetType().GetProperty(_textField).GetValue(_data).ToString();
                }
            }
        }
        public int _settingUserId;
        public UserManager_PowerSetting()
        {
            InitializeComponent();

        }
        public UserManager_PowerSetting(int userid):this()
        {
            _settingUserId = userid;
            bindProject();
        }
        async void bindProject()
        {
            tree_project.ItemsSource = new object[] {
                new {
                    Text = "正在加载..."
                }
            };
            Exception error = null;
            SettingProxy[] treeSource = null;
            await Task.Run(()=>
            {
                try
                {
                    var projects = Helper.Client.InvokeSync<EJ.Project[]>("GetCurrentUserProjectToSetPowerList", _settingUserId);
                    var project_source = (from m in projects
                                         select new SettingProxy(m, "Name")).ToArray();
                    foreach( var project in project_source)
                    {
                        var databases = Helper.Client.InvokeSync<EJ.Databases[]>("GetCurrentUserDatabaseToSetPowerList", _settingUserId , project.id);
                        project.Children = (from m in databases
                                            select new SettingProxy(m, "Name")).ToArray();
                        project.ClickEvent = new RoutedEventHandler(project_Click);
                        project.Icon = "/imgs/project.png";

                        foreach( var database in project.Children )
                        {
                            database.Icon = "/imgs/dbitem.png";
                            database.ClickEvent = new RoutedEventHandler(database_Click);
                            var tables = Helper.Client.InvokeSync<EJ.DBTable[]>("GetCurrentUserDBTableToSetPowerList", _settingUserId, database.id);
                            database.Children = (from m in tables
                                                 select new SettingProxy(m, "Name") {
                                                     Icon = "/imgs/dbtable.png"
                                                 }).ToArray();

                            foreach( var table in database.Children )
                            {
                                table.ClickEvent = new RoutedEventHandler(table_Click);
                            }
                           
                        }

                        bindInterface(project.id, 0, project);
                    }
                    treeSource = project_source;
                   
                }
                catch(Exception ex)
                {
                    error = ex;
                }
            });
            if(error != null)
            {
                tree_project.ItemsSource = null;
                Helper.ShowError(this, error);
            }
            else
            {
                tree_project.ItemsSource = treeSource;
            }
        }

        void bindInterface(int projectid , int parentid , SettingProxy container)
        {
            var interfaceItems = Helper.Client.InvokeSync<EJ.InterfaceModule[]>("GetCurrentUseInterfaceToSetPowerList", _settingUserId, projectid, parentid);
            var source = (from m in interfaceItems
                          select new SettingProxy(m, "Name")).ToArray();
            for(int i = 0; i < source.Length; i ++)
            {
                var iItem = source[i];
                if(interfaceItems[i].IsFolder == true)
                {
                    iItem.CheckBoxVisibility = Visibility.Hidden;
                    iItem.Icon = "/imgs/folder.png";
                }
                else
                {
                    iItem.Icon = "/imgs/dbmodule.png";
                    iItem.ClickEvent = new RoutedEventHandler(interface_Click);
                }
                bindInterface(projectid, iItem.id, iItem);
            }
            if (source.Length > 0)
            {
                if (container.Children == null)
                    container.Children = source;
                else
                {
                    List<SettingProxy> newsource = new List<SettingProxy>(container.Children);
                    newsource.AddRange(source);
                    container.Children = newsource.ToArray();
                }
            }
        }
        private void checkbox_Click(object sender, RoutedEventArgs e)
        {
            CheckBox chk = sender as CheckBox;
            var data = chk.Tag as SettingProxy;
            if (data.ClickEvent != null)
            {
                data.ClickEvent(sender, e);
            }
        }
        private void project_Click(object sender, RoutedEventArgs e)
        {
            CheckBox chk = sender as CheckBox;
            var data = chk.Tag as SettingProxy;
            chk.IsEnabled = false;
            chk.Cursor = Cursors.Wait;
            Helper.Client.Invoke<int>("SetProjectPower", (r,err) => {
                chk.Cursor = null;
                chk.IsEnabled = true;
                if (err != null)
                    Helper.ShowError(this, err);
            }, data.id, _settingUserId, chk.IsChecked.Value);
        }

        private void database_Click(object sender, RoutedEventArgs e)
        {
            CheckBox chk = sender as CheckBox;
            var data = chk.Tag as SettingProxy;
            chk.IsEnabled = false;
            chk.Cursor = Cursors.Wait;
            Helper.Client.Invoke<int>("SetDatabasePower", (r, err) => {
                chk.Cursor = null;
                chk.IsEnabled = true;
                if (err != null)
                    Helper.ShowError(this, err);
            }, data.id, _settingUserId, chk.IsChecked.Value);
        }


        private void table_Click(object sender, RoutedEventArgs e)
        {
            CheckBox chk = sender as CheckBox;
            var data = chk.Tag as SettingProxy;
            chk.IsEnabled = false;
            chk.Cursor = Cursors.Wait;
            Helper.Client.Invoke<int>("SetTablePower", (r, err) => {
                chk.Cursor = null;
                chk.IsEnabled = true;
                if (err != null)
                    Helper.ShowError(this, err);
            }, data.id, _settingUserId, chk.IsChecked.Value);
        }

        private void interface_Click(object sender, RoutedEventArgs e)
        {
            CheckBox chk = sender as CheckBox;
            var data = chk.Tag as SettingProxy;
            chk.IsEnabled = false;
            chk.Cursor = Cursors.Wait;
            Helper.Client.Invoke<int>("SetInterfaceModulePower", (r, err) => {
                chk.Cursor = null;
                chk.IsEnabled = true;
                if (err != null)
                    Helper.ShowError(this, err);
            }, data.id, _settingUserId, chk.IsChecked.Value);
        }
    }
}
