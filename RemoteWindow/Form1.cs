using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static RemoteWindow.Helper;

namespace RemoteWindow
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            textBox1.Text = DateTime.Now.ToString();
            RECT rect;
            using (var b = Helper.GetWindowBitmap(this.Handle,out rect))
            {
                b.Save("f:\\a.png", System.Drawing.Imaging.ImageFormat.Png);
            }
        }
    }
}
