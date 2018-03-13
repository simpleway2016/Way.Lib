﻿using System;
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
        ObservableCollection<object> _dataSource = new ObservableCollection<object>();
        public AlarmWindow()
        {
            InitializeComponent();
           
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
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
                        _dataSource.Add(data);

                    _canLoadMoreData = (ret.Length == _pageSize);
                }
            } , "Alarms" , pageindex*_pageSize, _pageSize , "");
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

       
    }

    class MyAlarm: SunRizServer.Alarm
    {
        public override bool? IsConfirm
        {
            get => base.IsConfirm;
            set {
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
                    return "#f2abb7";
                return "#ffffff";
            }
        }
    }
}
