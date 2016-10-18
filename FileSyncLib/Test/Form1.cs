using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Configuration;
namespace Test
{
    public partial class Form1 : Form
    {
        FileSyncLib.FileSync sync;
        RichTextBox txtInfo;
        public Form1()
        {
            InitializeComponent();
            this.Text = "硬盘同步备份";
            sync = new FileSyncLib.FileSync();
            FileSyncLib.FileSync.Changed += FileSync_Changed;
            sync.Start(ConfigurationManager.AppSettings["db"]);

            txtInfo = new RichTextBox();
            txtInfo.Dock = DockStyle.Fill;
            this.Controls.Add(txtInfo);
        }
        int lineCount = 0;
        void FileSync_Changed(string fullpath, string type, string oldpath)
        {
            this.Invoke(new MethodInvoker(delegate
            {
                if (lineCount > 100)
                {
                    string[] lines = txtInfo.Lines;
                    txtInfo.Text = "";
                    lineCount = 0;
                    for (int i = lines.Length - 20; i < lines.Length; i++)
                    {
                        txtInfo.AppendText(lines[i] + "\r\n");
                        lineCount++;
                    }
                }
                txtInfo.AppendText(DateTime.Now + " " + type + " " + fullpath + " " + oldpath + "\r\n");
                txtInfo.ScrollToCaret();
                lineCount++;
            }));
        }
    }
}
