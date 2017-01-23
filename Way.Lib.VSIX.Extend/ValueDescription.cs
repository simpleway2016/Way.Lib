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
        string _textMeber;
        public ValueDescription(object value, Func<string> func)
        {
            _func = func;
            this.Value = value;
        }
        public ValueDescription(object value, string textMember)
        {
            _textMeber = textMember;
            this.Value = value;
        }
        public override string ToString()
        {
            if (_textMeber != null)
                return (string)this.Value.GetType().InvokeMember(_textMeber, System.Reflection.BindingFlags.GetProperty, null, this.Value, null);
            return _func();
        }
    }
}
