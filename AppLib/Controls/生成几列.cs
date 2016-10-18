using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AppLib.Controls
{
    public partial class 生成几列 : Form
    {
        public int ColumnCount;
        public 生成几列()
        {
            InitializeComponent();
        }

        private void 生成几列_Load(object sender, EventArgs e)
        {
            foreach (Control ctrl in this.Controls)
            {
                ctrl.Click += ctrl_Click;
            }
        }

        void ctrl_Click(object sender, EventArgs e)
        {
            Control ctrl = sender as Control;
            ColumnCount = Convert.ToInt32(ctrl.Text);
            this.DialogResult = System.Windows.Forms.DialogResult.OK;
        }
    }
}
