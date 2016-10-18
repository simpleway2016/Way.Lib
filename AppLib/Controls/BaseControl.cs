using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI.Design;

namespace AppLib.Controls
{
    public class MyControlDesigner : ControlDesigner
    {
        public override string GetDesignTimeHtml()
        {
            System.Web.UI.Control ctrl = ((System.Web.UI.Control)this.Component);
            return "<span style='border:1px solid #cccccc;padding:10px;'>" + ctrl.ID + "</span>";
        }
    }

    [Designer(typeof(MyControlDesigner))]
    public abstract class BaseControl:System.Web.UI.WebControls.WebControl
    {
        bool _Visible = true;
        public override bool Visible
        {
            get
            {
                return _Visible;
            }
            set
            {
                _Visible = value;
            }
        }
    }
}
