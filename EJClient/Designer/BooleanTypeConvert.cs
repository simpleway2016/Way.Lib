using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EJClient.Designer
{
    class BooleanTypeConvert : System.ComponentModel.TypeConverter
    {
        public override bool GetStandardValuesSupported(System.ComponentModel.ITypeDescriptorContext context)
        {
            return true;
        }

        public override System.ComponentModel.TypeConverter.StandardValuesCollection GetStandardValues(System.ComponentModel.ITypeDescriptorContext context)
        {
            var val = (new string[] {"YES" , "NO"});

            StandardValuesCollection svc = new StandardValuesCollection(val);
            return svc;
        }

        public override bool CanConvertFrom(System.ComponentModel.ITypeDescriptorContext context, System.Type sourceType)
        {
            return true;
        }

        public override bool CanConvertTo(System.ComponentModel.ITypeDescriptorContext context, Type destinationType)
        {
            return true;
        }

        public override object ConvertFrom(System.ComponentModel.ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value)
        {
            if (value == null)
                return false;
            if (value.Equals("YES"))
                return true;

            return false;
        }

        public override object ConvertTo(System.ComponentModel.ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value, Type destinationType)
        {
            if (value is bool)
            {
                return (bool)value ? "YES" : "NO";
            }
            else if (value is bool?)
            {
                return ((bool?)value).GetValueOrDefault() ? "YES" : "NO";
            }
            else if(value is string)
            {
                return value;
            }
            return Convert.ToString(value);
        }
    }
}
