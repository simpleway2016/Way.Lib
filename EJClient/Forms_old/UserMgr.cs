using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EJClient.Forms
{
    class UserMgr : WebForm
    {
        public UserMgr( )
            : base(Helper.WebSite + "/WebForm/usermgr.aspx")
        {
            this.Text = "用户管理";
            this.Width = 800;
            this.Height = 600;
            this.web.DocumentCompleted += web_DocumentCompleted;
        }

        void web_DocumentCompleted(object sender, System.Windows.Forms.WebBrowserDocumentCompletedEventArgs e)
        {
            
        }
    }
}
