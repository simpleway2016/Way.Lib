using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppLib.Controls
{
    interface IKeyObject
    {
        string KeyTableName
        {
            get;
            set;
        }
        string KeyIDField
        {
            get;
            set;
        }
        string KeyTextField
        {
            get;
            set;
        }

        ISite Site
        {
            get;
        }

        IAutoDataBindControl AutoDataBindControl
        {
            get;
        }
    }
}
