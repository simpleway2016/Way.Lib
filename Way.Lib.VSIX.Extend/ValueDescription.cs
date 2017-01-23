using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Way.Lib.VSIX.Extend
{
    public class ValueDescription
    {
        public object Value
        {
            get;
            set;
        }

        Func<string> _func;
        public ValueDescription(object value, Func<string> func)
        {
            _func = func;
            this.Value = value;
        }

        public override string ToString()
        {
            return _func();
        }
    }
}
