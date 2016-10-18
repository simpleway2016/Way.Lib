using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace EJClient.UI
{
    public class Document : TabItem
    {
        public Document()
        {
           
        }

        protected override void OnMouseDoubleClick(System.Windows.Input.MouseButtonEventArgs e)
        {
            FrameworkElement sourceCtrl = e.OriginalSource as FrameworkElement;
            if (sourceCtrl == null)
                return;
            while (sourceCtrl != null)
            {
                sourceCtrl = sourceCtrl.Parent as FrameworkElement;
                if (sourceCtrl != null && sourceCtrl is ScrollViewer)
                    return;
            }

            FrameworkElement content = this.Content as FrameworkElement;
            Point p = ((UIElement)e.OriginalSource).TranslatePoint(new Point(0, 0), content);
            if (p.Y < 0)
            {
                e.Handled = true;
                MainWindow.instance.documentContainer.Items.Remove(this);
                return;
            }
            base.OnMouseDoubleClick(e);
        }
    }
}
