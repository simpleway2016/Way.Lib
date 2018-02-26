using System;
using System.Collections.Generic;
using System.ComponentModel;
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

namespace SunRizStudio
{
    /// <summary>
    /// SpeedCalculator.xaml 的交互逻辑
    /// </summary>
    public partial class SpeedCalculator : Window
    {
        class Model: INotifyPropertyChanged
        {
            string _speed;
            public string speed {
                get
                {
                    return _speed;
                }
                set
                {
                    if(_speed != value)
                    {
                        _speed = value;
                        this.OnPropertyChanged("speed");
                    }
                }
            }
            protected virtual void OnPropertyChanged(string proName)
            {
                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs(proName));
                }
            }
            public event PropertyChangedEventHandler PropertyChanged;
        }
        Model _model;
        System.Diagnostics.Stopwatch _stopWatch = new System.Diagnostics.Stopwatch();
        long _lastElapsedMilliseconds;
        long _startElapsedMilliseconds;
        int _count = 0;
        public SpeedCalculator()
        {
            InitializeComponent();
            _model = new Model();
            _stopWatch.Start();
            _lastElapsedMilliseconds = _stopWatch.ElapsedMilliseconds;
            _startElapsedMilliseconds = _lastElapsedMilliseconds;
            this.DataContext = _model;
        }

        protected override void OnPreviewKeyDown(KeyEventArgs e)
        {
            if( _count >= 3 || _stopWatch.ElapsedMilliseconds - _lastElapsedMilliseconds >= 1500)
            {
                //重新计算
                _startElapsedMilliseconds = _stopWatch.ElapsedMilliseconds;
                _lastElapsedMilliseconds = _startElapsedMilliseconds;
                _count = 0;
            }
            else
            {
                _lastElapsedMilliseconds = _stopWatch.ElapsedMilliseconds;
                _count++;
                if(_count >= 3)
                {
                    _model.speed = Math.Round((_count * 1000 * 60.0) / (_lastElapsedMilliseconds - _startElapsedMilliseconds),2).ToString();
                }
            }
            base.OnPreviewKeyDown(e);
        }
    }
}
