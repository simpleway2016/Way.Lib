using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppLib.Controls.Editor
{
    class BaseTypeConvert : System.ComponentModel.TypeConverter
    {
        public override bool GetCreateInstanceSupported(System.ComponentModel.ITypeDescriptorContext context)
        {
            return true;
        }
    }
}
