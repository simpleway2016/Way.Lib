using System.Globalization;
using System;
using System.Windows.Forms;

namespace Way.Lib.VSIX
{
    [ProvideToolboxControl("Way.Lib.VSIX.ToolboxControl1", false)]
    public partial class ToolboxControl1 : UserControl
    {
        public ToolboxControl1()
        {
            InitializeComponent();
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            MessageBox.Show(string.Format(CultureInfo.CurrentUICulture, "We are inside {0}.Button1_Click()", this.ToString()));
        }
    }
}
