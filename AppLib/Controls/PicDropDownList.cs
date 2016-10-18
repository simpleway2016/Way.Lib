using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI;

namespace AppLib.Controls
{
    public class PicDropDownList : System.Web.UI.WebControls.DropDownList 
    {
        public string CssClassChild
        {
            get
            {
                return Convert.ToString(ViewState["CssClassChild"]);
            }
            set
            {
                ViewState["CssClassChild"] = value;
            }
        }
        public string CssClassBorder
        {
            get
            {
                return Convert.ToString(ViewState["CssClassBorder"]);
            }
            set
            {
                ViewState["CssClassBorder"] = value;
            }
        }
        public string CssClassSelected
        {
            get
            {
                return Convert.ToString(ViewState["CssClassSelected"]);
            }
            set
            {
                ViewState["CssClassSelected"] = value;
            }
        }
        public string CssClassMouseOver
        {
            get
            {
                return Convert.ToString(ViewState["CssClassMouseOver"]);
            }
            set
            {
                ViewState["CssClassMouseOver"] = value;
            }
        }
        protected override void OnLoad(EventArgs e)
        {
            AppHelper.RegisterJquery(this.Page);

            if (!this.Page.ClientScript.IsClientScriptIncludeRegistered("__picDropDownList_JSFile"))
            {
                Page.ClientScript.RegisterClientScriptInclude("__picDropDownList_JSFile",
             Page.ClientScript.GetWebResourceUrl(this.GetType(),
                                         "AppLib.js.PicDropDownList.js"));
            }

            base.OnLoad(e);
        }
      
        protected override void Render(System.Web.UI.HtmlTextWriter writer)
        {
            writer.WriteBeginTag("input");
            writer.WriteAttribute("type", "hidden");
            writer.WriteAttribute("value", this.SelectedValue);
            writer.WriteAttribute("name", this.UniqueID);
            writer.WriteAttribute("id", this.ClientID);
            writer.Write("/>");

           
            if(this.Width.IsEmpty == false)
                writer.AddStyleAttribute(HtmlTextWriterStyle.Width, this.Width.ToString());

            writer.AddStyleAttribute(HtmlTextWriterStyle.Display, "-moz-inline-box");
            writer.AddStyleAttribute(HtmlTextWriterStyle.Display, "inline-block");//display:-moz-inline-box;   display:inline-block;
            writer.AddAttribute("id", ClientID + "_m");
            if (!string.IsNullOrEmpty(this.CssClass))
            {
                writer.AddAttribute("class", this.CssClass);
            }

            writer.RenderBeginTag("div");
            if (this.SelectedIndex >= 0 && this.Items.Count > 0)
            {
                writer.Write(System.Web.HttpUtility.HtmlEncode(this.Items[this.SelectedIndex].Text));
            }
            writer.RenderEndTag( );

            
            object[] items = new object[this.Items.Count];
            for(int i = 0 ; i < items.Length ; i ++)
            {
                items[i] = new {
                    Text = System.Web.HttpUtility.HtmlEncode(Items[i].Text),
                    Value = Items[i].Value,
                };
            }
            System.Web.Script.Serialization.JavaScriptSerializer json = new System.Web.Script.Serialization.JavaScriptSerializer();

            writer.WriteBeginTag("script");
            writer.WriteAttribute("lang", "ja");
            writer.Write(HtmlTextWriter.TagRightChar);

            string postbackEvent = "false";
            if (this.AutoPostBack )
            {
               postbackEvent = json.Serialize( this.Page.ClientScript.GetPostBackEventReference(this, "0"));
            }

            writer.Write(string.Format("var js_{0} = new PicDropDownList('{0}','{1}','{2}','{3}','{4}' , {5} , {6} , '{7}',{8});", ClientID,
                CssClassChild, CssClassBorder, CssClassSelected,CssClassMouseOver,
                json.Serialize(items), json.Serialize(this.SelectedValue), this.UniqueID, postbackEvent));
            writer.WriteEndTag("script");
        }

    }
}
