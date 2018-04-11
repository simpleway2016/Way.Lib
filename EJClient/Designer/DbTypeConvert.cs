using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EJClient.Designer
{
    internal class DbTypeConvert : System.ComponentModel.TypeConverter
	{
		public override bool GetStandardValuesSupported(System.ComponentModel.ITypeDescriptorContext context)
		{
			return true;
		}

		public override System.ComponentModel.TypeConverter.StandardValuesCollection GetStandardValues(System.ComponentModel.ITypeDescriptorContext context)
		{
			var val = ColumnType.SupportTypes;

			StandardValuesCollection svc = new StandardValuesCollection((from m in val orderby m select m).ToArray());
			return svc;
		}

		public override bool CanConvertFrom(System.ComponentModel.ITypeDescriptorContext context, System.Type sourceType)
		{
			return false;
		}

	}
}
