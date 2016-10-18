using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppLib.Controls
{
    public interface IAutoDataBindControl
    {
        string DatabaseConfig
        {
            get;
            set;
        }
    }
}
