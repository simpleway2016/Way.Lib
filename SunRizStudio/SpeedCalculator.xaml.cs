using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.InteropServices;
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
            double _speed;
            public double speed {
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

        [DllImport("winmm")]
        static extern uint timeGetTime();
        [DllImport("winmm")]
        static extern void timeBeginPeriod(int t);
        [DllImport("winmm")]
        static extern void timeEndPeriod(int t);

        Model _model;
        uint _lastElapsedMilliseconds;
        uint _startElapsedMilliseconds;
        int _count = 0;
        public SpeedCalculator()
        {
            InitializeComponent();
            _model = new Model();
            timeBeginPeriod(1);


            _lastElapsedMilliseconds = timeGetTime();
            _startElapsedMilliseconds = _lastElapsedMilliseconds;
            this.DataContext = _model;
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            timeEndPeriod(1);
            base.OnClosing(e);
        }

        protected override void OnPreviewKeyDown(KeyEventArgs e)
        {
            uint time = timeGetTime();
            if ( _count >= 3 || time - _lastElapsedMilliseconds >= 1500)
            {
                //重新计算
                _startElapsedMilliseconds = time;
                _lastElapsedMilliseconds = _startElapsedMilliseconds;
                _count = 0;
            }
            else
            {
                _lastElapsedMilliseconds = time;
                _count++;
                if(_count >= 3)
                {
                    _model.speed = (_count * 1000 * 60.0) / (_lastElapsedMilliseconds - _startElapsedMilliseconds);
                }
            }
            base.OnPreviewKeyDown(e);
        }
    }
}
