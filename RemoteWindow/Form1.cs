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
        public static IntPtr HWND;
        Bitmap b1;

        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
           
        }

        private void button2_Click(object sender, EventArgs e)
        {
            
        }

        private void button3_Click(object sender, EventArgs e)
        {
            textBox1.Text = DateTime.Now.ToString();
            var random = new Random();
            textBox1.BackColor = Color.FromArgb(random.Next(0, 255), random.Next(0, 255), random.Next(0, 255));
            textBox1.ForeColor = Color.FromArgb(random.Next(0, 255), random.Next(0, 255), random.Next(0, 255));
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            HWND = this.Handle;
        }
    }
}
