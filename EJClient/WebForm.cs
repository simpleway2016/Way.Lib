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

            web.Navigate(URL);
        }
    }
}
