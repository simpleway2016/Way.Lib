using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
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
    /// AlarmWindow.xaml 的交互逻辑
    /// </summary>
    public partial class AlarmWindow : Window
    {
        int _pageSize = 20;
        int _currentPageIndex = 0;
        /// <summary>
        /// 是否还有更多数据可以加载
        /// </summary>
        bool _canLoadMoreData;
        /// <summary>
        /// 是否正在从服务器获取数据
        /// </summary>
        bool _isLoading = false;
        SearchModel _searchModel = new SearchModel();
        string _orderBy;
        ObservableCollection<object> _dataSource = new ObservableCollection<object>();
        SunRizServer.SystemSetting _sysSetting;
        public AlarmWindow()
        {
            InitializeComponent();
           
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            panelSearch.DataContext = _searchModel;

            //获取窗口句柄 
            var handle = new WindowInteropHelper(this).Handle;
            //获取当前显示器屏幕
            var screen = System.Windows.Forms.Screen.FromHandle(handle);

            _pageSize = 5 + screen.Bounds.Height / 22;
            list.ItemsSource = _dataSource;
          

            var scrollViewer = list.GetChildByName<ScrollViewer>(null);
            scrollViewer.ScrollChanged += ScrollViewer_ScrollChanged;

            this.Cursor = Cursors.Wait;
            Helper.Remote.Invoke<SunRizServer.SystemSetting>("GetSystemSetting", (ret, err) => {
                this.Cursor = null;
                if(err != null)
                {
                    MessageBox.Show(this, err);
                }
                else
                {
                    _sysSetting = ret;
                    reload();
                }
            });
            
        }

        private void ScrollViewer_ScrollChanged(object sender, ScrollChangedEventArgs e)
        {
            if(e.VerticalOffset > e.ViewportHeight/2)
            {
                loadMore();
            }
        }

        void reload()
        {
            if (!_isLoading)
            {
                loadData(0);
            }
        }

        void loadData(int pageindex)
        {
            _isLoading = true;
            this.Cursor = Cursors.Wait;

           
            //访问服务器controller里面Alarms属性，获取数据
            Helper.Remote.Invoke<MyAlarm[]>("LoadData" , (ret,err)=> {
                this.Cursor = null;
                _isLoading = false;
                if (err != null)
                {
                    MessageBox.Show(this, err);
                }
                else
                {
                    _currentPageIndex = pageindex;                   

                    if(_currentPageIndex == 0)
                    {
                        _dataSource.Clear();
                    }

                    foreach (var data in ret)
                    {
                        data.SysSetting = _sysSetting;
                        _dataSource.Add(data);
                    }                   

                    _canLoadMoreData = (ret.Length == _pageSize);
                }
            } , "Alarms" , pageindex*_pageSize, _pageSize , _searchModel.ToJsonString(), _orderBy);
        }

        void loadMore()
        {
            if (_canLoadMoreData && !_isLoading)
            {
                loadData(_currentPageIndex + 1);
            }
        }

        private void list_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            list.SelectedIndex = -1;
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            e.Cancel = true;
            this.Hide();
            base.OnClosing(e);
        }

        private void btnConfirm_Click(object sender, RoutedEventArgs e)
        {
            FrameworkElement button = sender as FrameworkElement;
            MyAlarm data = (MyAlarm)button.DataContext;

            this.Cursor = Cursors.Wait;
            Helper.Remote.Invoke<MyAlarm[]>("ConfirmAlarm", (ret, err) => {
                this.Cursor = null;
                if (err != null)
                {
                    MessageBox.Show(this, err);
                }
                else
                {
                    data.IsConfirm = true;
                }
            }, data.id);
        }

        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            reload();
        }

        private void DatePicker_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            if (((DatePicker)sender).SelectedDate == null)
                _searchModel.AlarmTime[0] = "";
            else
                _searchModel.AlarmTime[0] = ((DatePicker)sender).SelectedDate.Value.ToString("yyyy-MM-dd");
            _searchModel.setTimeChanged();
        }

        private void DatePicker_SelectedDateChanged_1(object sender, SelectionChangedEventArgs e)
        {
            if (((DatePicker)sender).SelectedDate == null)
                _searchModel.AlarmTime[1] = "";
            else
                _searchModel.AlarmTime[1] = ((DatePicker)sender).SelectedDate.Value.ToString("yyyy-MM-dd");
            _searchModel.setTimeChanged();
        }
        private void list_Sort(string order)
        {
            _orderBy = order;
            reload();
        }
        class SearchModel : Way.Lib.DataModel
        {
            string _Address;
            public string Address
            {
                get
                {
                    return _Address;
                }
                set
                {
                    if (_Address != value)
                    {
                        _Address = value;
                    }
                }
            }

            public void setTimeChanged()
            {
                this.OnPropertyChanged("AlarmTime", null, null);
            }

            string[] _AlarmTime = new string[] { "", "" };
            public string[] AlarmTime
            {
                get
                {
                    return _AlarmTime;
                }
            }
        }

        class MyAlarm : SunRizServer.Alarm
        {
            public SunRizServer.SystemSetting SysSetting;
            public static Type SysSettingType = typeof(SunRizServer.SystemSetting);
            public override bool? IsConfirm
            {
                get => base.IsConfirm;
                set
                {
                    base.IsConfirm = value;
                    if (value == true)
                    {
                        this.OnPropertyChanged("BgColor", null, "#ffffff");
                    }
                }
            }
            public string BgColor
            {
                get
                {
                    if (this.IsConfirm == false)
                    {
                        string color = null;
                        if (this.IsBack == true)
                        {
                            //取报警返回颜色
                            var pro = SysSettingType.GetProperty("BackAlarmColor" + this.Priority);
                            if (pro != null)
                            {
                                color = pro.GetValue(SysSetting).ToSafeString();
                            }
                            if (!string.IsNullOrEmpty(color))
                                return color;
                        }
                        else
                        {
                            //取未确认颜色
                            var pro = SysSettingType.GetProperty("UnConfigAlarmColor" + this.Priority);
                            if (pro != null)
                            {
                                color = pro.GetValue(SysSetting).ToSafeString();
                            }
                            if (!string.IsNullOrEmpty(color))
                                return color;

                            //取未返回颜色
                            pro = SysSettingType.GetProperty("UnBackAlarmColor" + this.Priority);
                            if (pro != null)
                            {
                                color = pro.GetValue(SysSetting).ToSafeString();
                            }
                            if (!string.IsNullOrEmpty(color))
                                return color;

                            color = SysSetting.AlarmStatusChangeColor;
                            if (!string.IsNullOrEmpty(color))
                                return color;
                        }

                        return "#f2abb7";
                    }
                    else 
                    {
                        //确认报警
                        string color = null;
                        var pro = SysSettingType.GetProperty("ConfigAlarmColor" + this.Priority);
                        if (pro != null)
                        {
                            color = pro.GetValue(SysSetting).ToSafeString();
                        }
                        if (!string.IsNullOrEmpty(color))
                            return color;
                        return "#ffffff";
                    }
                }
            }
        }

       
    }
   
   
}
