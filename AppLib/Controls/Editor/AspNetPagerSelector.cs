using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI.Design;
using System.Windows.Forms;
using System.Windows.Forms.Design;
namespace AppLib.Controls.Editor
{
    class AspNetPagerSelector : ControlSelector
    {
        public override Type ControlType
        {
            get { return typeof(Wuqi.Webdiyer.AspNetPager);}
        }

    }
}
