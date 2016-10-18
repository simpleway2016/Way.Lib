using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EJClient.Forms
{
    class DatabaseEditor : WebForm
    {
        public DatabaseEditor(int projectid)
            : base(Helper.WebSite + "/WebForm/DatabaseEditor.aspx?projectid=" + projectid)
        {
            this.Width = 500;
            this.Height = 500;
            this.web.DocumentCompleted += web_DocumentCompleted;
        }
        public DatabaseEditor(int projectid,int databaseID)
            : base(Helper.WebSite + "/WebForm/DatabaseEditor.aspx?projectid=" + projectid + "&databaseid=" + databaseID)
        {
            this.Width = 500;
            this.Height = 500;
            this.web.DocumentCompleted += web_DocumentCompleted;
        }
        void web_DocumentCompleted(object sender, System.Windows.Forms.WebBrowserDocumentCompletedEventArgs e)
        {
            try
            {
                web.Document.GetElementById("btnClose").Click += btnClose_Click;
                web.Document.GetElementById("btnSelectFolder").Click += btnSelectFolder_Click;
                
            }
            catch
            {
            }
            try
            {
                web.Document.InvokeScript("webBrowser_start");
            }
            catch
            {
            }
        }

        void btnSelectFolder_Click(object sender, System.Windows.Forms.HtmlElementEventArgs e)
        {
            using (System.Windows.Forms.FolderBrowserDialog fd = new System.Windows.Forms.FolderBrowserDialog())
            {
                if (fd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    web.Document.GetElementById("txt_dllPath").SetAttribute("value", fd.SelectedPath);
                }
            }
        }

        void btnClose_Click(object sender, System.Windows.Forms.HtmlElementEventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.OK;
        }

      

    }
}
