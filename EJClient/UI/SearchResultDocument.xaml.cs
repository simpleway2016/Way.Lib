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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace EJClient.UI
{
    /// <summary>
    /// SearchResultDocument.xaml 的交互逻辑
    /// </summary>
    public partial class SearchResultDocument : Document
    {
        public SearchResultDocument(string key)
        {
            InitializeComponent();
            this.Header = "Search:" + key;
            using (Web.DatabaseService web = Helper.CreateWebService())
            {
                web.SearchCompleted += web_SearchCompleted;
                web.SearchAsync(key , 50 , 0);
            }
        }

        void web_SearchCompleted(object sender, Web.SearchCompletedEventArgs e)
        {
            if (e.Error != null)
            {
                MessageBox.Show(MainWindow.instance, e.Error.Message);
                return;
            }
            txtLoading.Visibility = System.Windows.Visibility.Collapsed;
            var datas = e.Result.ToJsonObject<SearchContent[]>();
            list.ItemsSource = datas;
        }
    }

    class SearchContent
    {
        public int? ID
        {
            get;
            set;
        }
        public string Title
        {
            get;
            set;
        }
        public string Content
        {
            get;
            set;
        }
        public string Type
        {
            get;
            set;
        }
    }
}
