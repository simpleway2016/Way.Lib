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
			var val = (new string[] {
											"varchar",
											"int",
											"image",
											"text",
											"uniqueidentifier",
											"tinyint",
											"smallint",
											"smalldatetime",
											"real",
											"money",
											"datetime",
											"float",
											"sql_variant",
											"ntext",
											"bit",
											"decimal",
											"numeric",
											"smallmoney",
											"bigint",
											"varbinary",
											"binary",
											"char",
											"timestamp",
											"nvarchar",
											"nchar",
											"sysname" });

			StandardValuesCollection svc = new StandardValuesCollection((from m in val orderby m select m).ToArray());
			return svc;
		}

		public override bool CanConvertFrom(System.ComponentModel.ITypeDescriptorContext context, System.Type sourceType)
		{
			return false;
		}

	}
}
