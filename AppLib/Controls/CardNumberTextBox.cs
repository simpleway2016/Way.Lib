using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI.WebControls;

namespace AppLib.Controls
{
    public class CardNumberTextBox : TextBox
    {
        public override string Text
        {
            get
            {
                string text = base.Text;
                return text.Replace(" ", "").Trim();
            }
            set
            {
                base.Text = value;
            }
        }
        protected override void OnLoad(EventArgs e)
        {
            if (!this.Page.ClientScript.IsClientScriptIncludeRegistered("__CardNumberTextBox_JSFile"))
            {
                Page.ClientScript.RegisterClientScriptInclude("__CardNumberTextBox_JSFile",
             Page.ClientScript.GetWebResourceUrl(this.GetType(),
                                         "AppLib.js.CardNumberTextBox.js"));
            }

            AppHelper.RegisterJquery(this.Page);

            base.OnLoad(e);
        }

        protected override void Render(System.Web.UI.HtmlTextWriter writer)
        {
            base.Render(writer);
            writer.Write(string.Format("<script lang='ja'>var js_{0} = new JS_CardNumberTextBox('{0}');</script>",this.ClientID));
        }
    }
}
