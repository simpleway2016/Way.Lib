using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Way.Lib.VSIX.Extend.AppCodeBuilder.Editors
{
    interface IDataControl
    {
        ValueDescription DBContext
        {
            get;
            set;
        }
        ValueDescription Table { get; set; }
    }
}
