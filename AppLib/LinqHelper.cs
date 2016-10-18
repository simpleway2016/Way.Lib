using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace AppLib
{
    class LinqHelper
    {
        internal static void GetExpression(Type tableType , string propertyName,object value,
            out Expression left,out Expression right)
        {
            ParameterExpression param = Expression.Parameter(tableType, "n");
            System.Reflection.PropertyInfo pinfo = tableType.GetProperty(propertyName, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
            Type ptype = pinfo.PropertyType;
            left = Expression.Property(param, pinfo);
            if (ptype.IsGenericType)
            {
                ptype = ptype.GetGenericArguments()[0];
                left = Expression.Convert(left, ptype);
            }
            //等式右边的值
            right = Expression.Constant(Convert.ChangeType(value, ptype));
        }
    }
}
