using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using static RemoteWindow.Helper;

namespace RemoteWindow
{
    public partial class Form1 : Form
    {
        Bitmap b1;

        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (b1 != null)
                b1.Dispose();
            textBox1.Text = DateTime.Now.ToString();

            var hwnd = this.Handle;
            Task.Run(() =>
            {
                Thread.Sleep(1000);
                RECT rect;
                b1 = Helper.GetWindowBitmap(hwnd, out rect);
            });
        }

        private void button2_Click(object sender, EventArgs e)
        {
            button1.ForeColor = Color.GreenYellow;
            textBox1.Text = DateTime.Now.ToString();

            var hwnd = this.Handle;
            Task.Run(() =>
            {
                Thread.Sleep(1000);
                RECT rect;
                Bitmap b2 = Helper.GetWindowBitmap(hwnd, out rect);

                var b3 = Helper.CompareBitmap(b1, b2);
                b2.Dispose();
                if (b3 != null)
                {
                    b3.Save("f:\\a.png", System.Drawing.Imaging.ImageFormat.Png);
                    b3.Dispose();
                }
            });

        }
    }
}
