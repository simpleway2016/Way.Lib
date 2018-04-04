using SunRizStudio.Extention;
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
    /// HistoryWindow.xaml 的交互逻辑
    /// </summary>
    public partial class HistoryWindow : Window
    {
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
                this.OnPropertyChanged("Time", null, null);
            }

            string[] _Time = new string[] { "", "" };
            public string[] Time
            {
                get
                {
                    return _Time;
                }
            }
        }

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
        public HistoryWindow()
        {
            InitializeComponent();
        }
        public HistoryWindow(string json)
        {
            InitializeComponent();
            var obj = Newtonsoft.Json.JsonConvert.DeserializeObject<Newtonsoft.Json.Linq.JObject>(json);
            _searchModel.Address = obj.Value<string>("pointNames");

            _searchModel.Time[0] = obj.Value<string>("startDate");
            _searchModel.Time[1] = obj.Value<string>("endDate");
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
            reload();

            var scrollViewer = list.GetChildByName<ScrollViewer>(null);
            scrollViewer.ScrollChanged += ScrollViewer_ScrollChanged;


        }

        private void ScrollViewer_ScrollChanged(object sender, ScrollChangedEventArgs e)
        {
            if (e.VerticalOffset > e.ViewportHeight / 2)
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
            Helper.Remote.Invoke<MyHistory[]>("GetHistories", (ret, err) => {
                this.Cursor = null;
                _isLoading = false;
                if (err != null)
                {
                    MessageBox.Show(this, err);
                }
                else
                {
                    _currentPageIndex = pageindex;

                    if (_currentPageIndex == 0)
                    {
                        _dataSource.Clear();
                    }

                    foreach (var data in ret)
                        _dataSource.Add(data);

                    _canLoadMoreData = (ret.Length == _pageSize);
                }
            }, pageindex * _pageSize, _pageSize, _searchModel.ToJsonString(), _orderBy);
        }

        void loadMore()
        {
            if (_canLoadMoreData && !_isLoading)
            {
                loadData(_currentPageIndex + 1);
            }
        }


        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            reload();
        }

        private void DatePicker_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            if (((DatePicker)sender).SelectedDate == null)
                _searchModel.Time[0] = "";
            else
                _searchModel.Time[0] = ((DatePicker)sender).SelectedDate.Value.ToString("yyyy-MM-dd");
            _searchModel.setTimeChanged();
        }

        private void DatePicker_SelectedDateChanged_1(object sender, SelectionChangedEventArgs e)
        {
            if (((DatePicker)sender).SelectedDate == null)
                _searchModel.Time[1] = "";
            else
                _searchModel.Time[1] = ((DatePicker)sender).SelectedDate.Value.ToString("yyyy-MM-dd");
            _searchModel.setTimeChanged();
        }
        private void list_Sort(string order)
        {
            _orderBy = order;
            reload();
        }
        class MyHistory : SunRizServer.History
        {
            public string AddressDesc
            {
                get;
                set;
            }
        }

        
    }

    
}
