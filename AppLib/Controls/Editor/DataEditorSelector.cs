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
    class EntityEditorSelector : ControlSelector
    {
        public override Type ControlType
        {
            get { return typeof(EntityEditor);}
        }

    }
    class JSDataSourceSelector : ControlSelector
    {
        public override Type ControlType
        {
            get { return typeof(JSDataSource); }
        }

    }

    class EntityGridViewSearchSelector : ControlSelector
    {
        public override Type ControlType
        {
            get { return typeof(EntityGridViewSearch); }
        }

    }
}
