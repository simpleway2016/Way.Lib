using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace SunRizStudio.Documents
{
    class DocTabItem : TabItem
    {
        public BaseDocument Document;
        public DocTabItem(BaseDocument doc)
        {
            this.Document = doc;
            this.Style = (Style)App.Current.Resources["TabItemDocStyle"];
            this.Content = this.Document;

            Document.TitleChanged += Document_TitleChanged;
            Document.TabItem = this;
            this.Header = Document.Title;
        }
        internal void Close()
        {
            bool cancel = false;

            Document.OnClose(ref cancel);
            if (!cancel)
            {
                Document.TitleChanged -= Document_TitleChanged;
                MainWindow.Instance.documentContainer.Items.Remove(this);
            }
        }
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();           

            this.GetChildByName<Button>("closeIcon").Click += (s2, e2) => {
                bool cancel = false;

                Document.OnClose(ref cancel);
                if (!cancel)
                {
                    Document.TitleChanged -= Document_TitleChanged;
                    MainWindow.Instance.documentContainer.Items.Remove(this);
                }
            };
        }

        private void Document_TitleChanged(object sender, EventArgs e)
        {
            Documents.BaseDocument doc = (Documents.BaseDocument)sender;
            this.Header = doc.Title;
        }
    }
}
