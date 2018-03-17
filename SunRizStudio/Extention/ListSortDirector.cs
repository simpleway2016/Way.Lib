using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;

namespace SunRizStudio.Extention
{
    public class ListSortDecorator : Control
    {
        // Using a DependencyProperty as the backing store for SortDirection.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SortDirectionProperty =
            DependencyProperty.Register("SortDirection", typeof(ListSortDirection), typeof(ListSortDecorator));

        /// <summary>
        /// 
        /// </summary>
        /// <param name="adorner"></param>
        static ListSortDecorator()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ListSortDecorator), new FrameworkPropertyMetadata(typeof(ListSortDecorator)));
        }

        public ListSortDirection SortDirection
        {
            get { return (ListSortDirection)GetValue(SortDirectionProperty); }
            set { SetValue(SortDirectionProperty, value); }
        }
    }
}
