using System;
using System.Collections.Generic;
using System.Globalization;
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
    /// BugListDocument.xaml 的交互逻辑
    /// </summary>
    public partial class BugListDocument : Document
    {
        public BugListDocument()
        {
            InitializeComponent();
            this.Header = "Bugs";
            dataBind();
        }

        void dataBind()
        {
            try
            {
                //listView.ItemsSource = Helper.Client.InvokeSync<Way.EntityDB.Design.BugItem[]>("GetMyBugs");
            }
            catch (Exception ex)
            {
                Helper.ShowError(ex);
            }
        }

        private void View_Click_1(object sender, RoutedEventArgs e)
        {
            //var data = (ECWeb.BugItem)((Button)sender).DataContext;
            //EJClient.Forms.BugCenter.BugView frm = new Forms.BugCenter.BugView(data);
            //frm.Owner = MainWindow.instance;
            //frm.WindowState = WindowState.Maximized;
            //if (frm.ShowDialog() == true)
            //{
            //    dataBind();
            //}
        }
    }

    public class ViewListTemplateSelector : DataTemplateSelector
    {
        public DataTemplate 提交给开发人员 { get; set; }
        public DataTemplate 反馈给提交者 { get; set; }
        public DataTemplate 处理完毕 { get; set; }

        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            //var bug = (Way.EntityDB.Design.BugItem)item;
            //return (DataTemplate)this.GetType().GetProperty(bug.Status.ToString()).GetValue(this);
            return null;
        }
    }

    public sealed class BackgroundConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter,
        CultureInfo culture)
        {
            ListViewItem item = (ListViewItem)value;
            ListView listView =
            ItemsControl.ItemsControlFromItemContainer(item) as ListView;
            // Get the index of a ListViewItem 
            int index =
            listView.ItemContainerGenerator.IndexFromContainer(item);

            //if (index % 2 == 0)
            //{
            //    return Brushes.LightBlue;
            //}
            //else
            //{
            //    return Brushes.Beige;
            //}
            //var bug = (Way.EntityDB.Design.BugItem)item.DataContext;
            //if (bug.Status == EJ.Bug_StatusEnum.提交给开发人员)
            //    return new SolidColorBrush(Color.FromArgb(255,251,171,177));
            //else if (bug.Status == EJ.Bug_StatusEnum.反馈给提交者)
            //    return new SolidColorBrush(Color.FromArgb(255, 171, 209, 251));

            return new SolidColorBrush(Color.FromArgb(255, 158, 200, 170));
        }


        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}
