using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Way.Lib.VSIX.Extend.AppCodeBuilder
{
    public interface IAppCodeBuilder
    {
        UserControl ViewControl { get; }
    }
}
