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

namespace SunRizStudio.Dialogs
{
    /// <summary>
    /// SearchResultDialog.xaml 的交互逻辑
    /// </summary>
    public partial class SearchResultDialog : Window
    {
        public SearchResultDialog()
        {
            InitializeComponent();
        }

        internal void SetResult(string key,SearchResult[] result)
        {
            this.Title = key + "搜索结果";
            list.ItemsSource = result;
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            e.Cancel = true;
            this.Hide();
            base.OnClosing(e);
        }

        private void list_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (list.SelectedIndex < 0)
                return;
            SearchResult item =(SearchResult) list.SelectedItem  ;
            MainWindow.Instance.OpenWindow(item.id, item.content);
        }
    }
}
