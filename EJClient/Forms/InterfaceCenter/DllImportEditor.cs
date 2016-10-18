using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EJClient.Forms.InterfaceCenter
{
    class DllImportEditor : WebForm
    {
        public DllImportEditor(int projectid)
            : base(Helper.WebSite + "/WebForm/interfaces/dllsetting.aspx?projectid=" + projectid)
        {
            this.Text = "工程类库设置";
            this.Width = 600;
            this.Height = 500;
            this.web.DocumentCompleted += web_DocumentCompleted;
        }

        void web_DocumentCompleted(object sender, System.Windows.Forms.WebBrowserDocumentCompletedEventArgs e)
        {
            try
            {
                web.Document.GetElementById("btnSelect").Click += btnSelectFolder_Click;

            }
            catch
            {
            }
        }

        void btnSelectFolder_Click(object sender, System.Windows.Forms.HtmlElementEventArgs e)
        {
            using (System.Windows.Forms.OpenFileDialog fd = new System.Windows.Forms.OpenFileDialog())
            {
                fd.Filter = "DLL自带XML文件.xml|*.xml";
                if (fd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    web.Document.GetElementById("txt_path").SetAttribute("value", fd.FileName);
                }
            }
        }
    }
}
