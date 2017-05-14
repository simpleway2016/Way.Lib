using EJClient.Forms.BugCenter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace EJClient.UI
{
    /// <summary>
    /// BugWebDocument.xaml 的交互逻辑
    /// </summary>
    public partial class BugWebDocument : Document
    {
       
        public BugWebDocument()
        {
            InitializeComponent();
            this.Header = "Bugs";
            host.Child = new BugListControl();
        }

       
    }

    public class BugListControl : System.Windows.Forms.Control
    {
        WindowsControl.ExtendedWebBrowser web;
        public BugListControl()
        {
            web = new WindowsControl.ExtendedWebBrowser();
            web.Dock = DockStyle.Fill;
            this.Controls.Add(web);
            //string url = Helper.WebSite + "/WebForm/bug/mybuglist.aspx";
            //var cookies = Helper.CookieContainer.GetCookies(new Uri(url));
            //for (int i = 0; i < cookies.Count; i++)
            //{
            //    var cookie = cookies[i];
            //    AppLib.WindowsControl.ExtendedWebBrowser.InternetSetCookie(url, cookie.Name, cookie.Value);
            //}

            //web.DocumentCompleted += web_DocumentCompleted;
            //web.Navigate(url);
        }

        void web_DocumentCompleted(object sender, System.Windows.Forms.WebBrowserDocumentCompletedEventArgs e)
        {
            var inputs = web.Document.GetElementsByTagName("INPUT");
            foreach (HtmlElement input in inputs)
            {
                if (input.GetAttribute("_type") == "发起者")
                {
                    input.Click += 发起者_查看_Click;
                }
            }
        }

        void 发起者_查看_Click(object sender, HtmlElementEventArgs e)
        {
            HtmlElement btn = sender as HtmlElement;
            int id = Convert.ToInt32(btn.GetAttribute("_id"));
            MyRole role = (MyRole)Convert.ToInt32(btn.GetAttribute("_MyRole"));
            BugView frm = new BugView(id, role);
            frm.WindowState = WindowState.Maximized;
            if (frm.ShowDialog() == true)
            {
                web.Document.InvokeScript("bind");
            }
        }
    }
}
