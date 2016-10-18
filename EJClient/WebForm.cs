using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EJClient
{
    public partial class WebForm : Form
    {
        string URL;
        public WebForm(string url)
        {
            this.URL = url;
           
            InitializeComponent();

            var cookies = Helper.CookieContainer.GetCookies(new Uri(url));
            for (int i = 0; i < cookies.Count; i++)
            {
                var cookie = cookies[i];
                AppLib.WindowsControl.ExtendedWebBrowser.InternetSetCookie(url, cookie.Name, cookie.Value);
            }

            web.Navigate(URL);
        }
    }
}
