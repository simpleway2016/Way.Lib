using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppLib.Controls.Editor
{
    public class TitleFieldAttribute : Attribute
    {
        public TitleFieldAttribute(string titlefield)
        {
            TitleField = titlefield;
        }
        public string TitleField
        {
            get;
            set;
        }
    }
}
