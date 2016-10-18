using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace WayControls.Windows.Controls
{
    public partial class InputBox : Form
    {
        public string Value
        {
            get
            {
                return  this.textBox1.Text.Trim();
            }
            set
            {
                this.textBox1.Text = value;
            }
        }
        public InputBox(string title , string label)
        {
            
            InitializeComponent();
            this.Text = title;
            this.label1.Text = label;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
        }
    }
}