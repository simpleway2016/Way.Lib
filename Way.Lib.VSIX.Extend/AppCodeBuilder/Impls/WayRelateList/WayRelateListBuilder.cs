using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Way.Lib.VSIX.Extend.AppCodeBuilder
{
    public class WayRelateListBuilder : IAppCodeBuilder
    {
        UserControl _Control;
        [System.ComponentModel.Browsable(false)]
        public UserControl ViewControl
        {
            get
            {
                return _Control;
            }
        }
        public string ControlId
        {
            get;
            set;
        }
        public WayRelateListBuilder()
        {
            this.ControlId = "relateList1";
            _Control = new Impls.WayRelateList.Control(this);
        }
    }
}
