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
    /// AlarmWindow.xaml 的交互逻辑
    /// </summary>
    public partial class AlarmWindow : Window
    {
        public AlarmWindow()
        {
            InitializeComponent();

            List<object> items = new List<object>();
            items.Add(new {
                content = "test1",
                pointName = "name1",
                bgColor = "#cccccc",
            });
            items.Add(new
            {
                content = "test2",
                pointName = "name2",
                bgColor = "#ccff00",
            });
            list.ItemsSource = items;
        }

        private void list_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            list.SelectedIndex = -1;
        }
    }
}
