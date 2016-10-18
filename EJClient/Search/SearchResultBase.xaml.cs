using EJClient.UI;
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

namespace EJClient.Search
{
    /// <summary>
    /// SearchResultBase.xaml 的交互逻辑
    /// </summary>
    public partial class SearchResultBase : UserControl, ISearchResult
    {
        ISearchResult m_Result;
        public SearchResultBase()
        {
            InitializeComponent();
        }

        public void Show()
        {
            m_Result.Show();
        }



        public string Title
        {
            get {
                return m_Result.Title;
            }
        }

        protected override void OnMouseDoubleClick(MouseButtonEventArgs e)
        {
            e.Handled = true;
            this.Cursor = Cursors.Wait;
            try
            {
                this.Show();
            }
            catch (Exception ex)
            {
                Helper.ShowError(ex);
            }
            finally
            {
                this.Cursor = null;
            }
        }

        private void UserControl_Loaded_1(object sender, RoutedEventArgs e)
        {
            
            SearchContent data = (SearchContent)this.DataContext;
            m_Result = (ISearchResult)Activator.CreateInstance(typeof(DBTableResult).Assembly.GetType("EJClient.Search." + data.Type),new object[]{data});
            txtTitle.Text = this.Title;
        }
    }
}
