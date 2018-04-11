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

namespace EJClient.Forms.BugCenter
{
    /// <summary>
    /// HistoryItem.xaml 的交互逻辑
    /// </summary>
    public partial class HistoryItem : StackPanel
    {
        public object Data
        {
            get;
            private set;
        }
        public HistoryItem(object data , double width)
        {
           // this.Data = data;
           // InitializeComponent();
           // this.Margin = new Thickness(5);
           // lblName.Content = this.Data.SubmitTime.Value.ToString("yyyy/MM/dd HH:mm:ss") + " " + this.Data.UserName + ":";
           // string xaml =  System.Text.Encoding.UTF8.GetString(data.Content);
           //var doc = System.Windows.Markup.XamlReader.Parse(xaml) as FlowDocument;
           //txtContent.Document = doc;
           //txtContent.IsReadOnly = true;
           //txtContent.Width = width - 10;
        }
    }

    public class ViewRichTextBox : RichTextBox
    {
        bool adjustHeighted = false;
        private void AdjustHeight()
        {
            adjustHeighted = true;
            Rect rectStart = Document.ContentStart.GetCharacterRect(LogicalDirection.Forward);
            Rect rectEnd = Document.ContentEnd.GetCharacterRect(LogicalDirection.Forward);
            var height = rectEnd.Bottom - rectStart.Top;
            var remainH = rectEnd.Height / 2.0;
            var myHeight = Math.Min(MaxHeight, Math.Max(MinHeight, height + remainH));
            this.Height = myHeight;
        }

        protected override void OnRender(DrawingContext drawingContext)
        {
            base.OnRender(drawingContext);
            if (!adjustHeighted)
            {
                AdjustHeight();
            }

        }
    }
}
