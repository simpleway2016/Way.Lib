using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI.WebControls;

namespace AppLib.Controls
{
    public class KindEditor : TextBox
    {
        public KindEditor()
        {

        }

        public override TextBoxMode TextMode
        {
            get
            {
                return TextBoxMode.MultiLine;
            }
            set
            {
            }
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            if (Page.ClientScript.IsClientScriptBlockRegistered("kindeditor_js") == false)
            {
                Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "kindeditor_js", @"
<link rel=""stylesheet"" href=""/kindeditor/themes/default/default.css"" />
    <link rel=""stylesheet"" href=""/kindeditor/plugins/code/prettify.css"" />
    <script charset=""utf-8"" src=""/kindeditor/kindeditor.js""></script>
    <script charset=""utf-8"" src=""/kindeditor/lang/zh_CN.js""></script>
    <script charset=""utf-8"" src=""/kindeditor/plugins/code/prettify.js""></script>");
            }
        }

        protected override void Render(System.Web.UI.HtmlTextWriter writer)
        {
            base.Render(writer);
            writer.Write(@"
<script>
var js_"+this.ID+@";
        KindEditor.ready(function (K) {
            js_" + this.ID + @" = K.create('textarea[id=""" + ClientID + @"""]', {
                cssPath: '/kindeditor/plugins/code/prettify.css',
                uploadJson: '/kindeditor/upload_json.ashx',
                fileManagerJson: '/kindeditor/file_manager_json.ashx',
                allowFileManager: true
            });
            prettyPrint();
        });
    </script>
");
        }
    }
}
