using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppLib.Controls
{
    public class Pager : Wuqi.Webdiyer.AspNetPager
    {
        public Pager()
        {
            this.AlwaysShow = true;

        }

        protected override void OnPageChanged(EventArgs e)
        {
            if (this.Page.EnableViewState == false)
            {
                try
                {
                    for (int i = 0; i < this.Page.Request.Form.Count; i++)
                    {
                        string name = this.Page.Request.Form.Keys[i];
                        string value = this.Page.Request.Form[i];
                        if (name == "__EVENTTARGET" && value == this.ID)
                        {
                            if (!string.IsNullOrEmpty(this.Page.Request.Form["__EVENTARGUMENT"]))
                            {
                                CurrentPageIndex = Convert.ToInt32(this.Page.Request.Form["__EVENTARGUMENT"]);
                            }
                            else if (!string.IsNullOrEmpty(this.Page.Request.Form[this.ID + "_Input"]))
                            {
                                CurrentPageIndex = Convert.ToInt32(this.Page.Request.Form[this.ID + "_Input"]);
                            }
                            break;
                        }
                    }
                }
                catch { }
            }
            base.OnPageChanged(e);

        }
    }
}
