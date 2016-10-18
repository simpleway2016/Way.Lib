using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppLib
{
    public class ExcelMakerAttribute : Attribute
    {
        public bool Enable
        {
            get;
            private set;
        }
        public ExcelMakerAttribute(bool enable)
        {
            this.Enable = enable;
        }
    }
    public class ExcelMaker
    {
        public static string OutputHtml(DataTable dtable)
        {
            StringBuilder str = new StringBuilder();
            str.Append(@"
<!DOCTYPE html>
<html xmlns=""http://www.w3.org/1999/xhtml"">
<head>
<meta http-equiv=""Content-Type"" content=""text/html; charset=utf-8""/>
    <title></title>
     <style type=""text/css"">

        table{ border-collapse:collapse; border:solid 1px Black; }

        table td{ width:50px; height:20px;  border:solid 1px Black; padding:5px;font-size:15px;}

    </style>
</head>
<body>
");
            str.AppendLine("<table><Tr>");
            bool createdHeader = false;
            foreach (DataColumn column in dtable.Columns)
            {
                str.AppendLine("<td>");
                str.Append(column.ColumnName);
                str.AppendLine("</td>");
                createdHeader = true;
            }
            if (!createdHeader)
                return "";

            str.AppendLine("</tr>");
            foreach (DataRow row in dtable.Rows)
            {
                str.AppendLine("<tr>");
                for (int i = 0; i < dtable.Columns.Count; i++)
                {
                    str.AppendLine("<td>");
                    str.Append(row[i].ToString());
                    str.AppendLine("</td>");
                }
                str.AppendLine("</tr>");
            }
            str.AppendLine("</table></body></html>");

            return str.ToString();
        }
        public static string OutputHtml(System.Collections.IEnumerable data)
        {
            StringBuilder str = new StringBuilder();
            str.Append(@"
<!DOCTYPE html>
<html xmlns=""http://www.w3.org/1999/xhtml"">
<head>
<meta http-equiv=""Content-Type"" content=""text/html; charset=utf-8""/>
    <title></title>
     <style type=""text/css"">

        table{ border-collapse:collapse; border:solid 1px Black; }

        table td{ width:50px; height:20px;  border:solid 1px Black; padding:5px;font-size:15px;}

    </style>
</head>
<body>
");
            str.AppendLine("<table><Tr>");
            bool createdHeader = false;
            List<System.Reflection.MemberInfo> properties = null;

            foreach (object item in data)
            {
                if (properties == null)
                {
                    properties = item.GetType().GetMembers().ToList();
                    properties = (from p in properties where p.MemberType == System.Reflection.MemberTypes.Property || p.MemberType == System.Reflection.MemberTypes.Field select p).ToList();
                    for (int i = 0; i < properties.Count; i++)
                    {
                        object[] atts = properties[i].GetCustomAttributes(typeof(ExcelMakerAttribute), false);
                        if (atts.Length > 0 && ((ExcelMakerAttribute)atts[0]).Enable == false)
                        {
                            properties.RemoveAt(i);
                            i--;
                        }

                    }
                }
                foreach (System.Reflection.MemberInfo m in properties)
                {
                    str.AppendLine("<td>");
                    str.Append(m.Name);
                    str.AppendLine("</td>");

                }
                createdHeader = true;
                break;
            }
            if (!createdHeader)
                return "";

            str.AppendLine("</tr>");
            foreach (object item in data)
            {
                str.AppendLine("<tr>");
                foreach (System.Reflection.MemberInfo p in properties)
                {
                    str.AppendLine("<td>");
                    if (p.MemberType == System.Reflection.MemberTypes.Field)
                    {
                        str.Append(((System.Reflection.FieldInfo)p).GetValue(item));
                    }
                    else
                    {
                        str.Append(((System.Reflection.PropertyInfo)p).GetValue(item));
                    }
                    str.AppendLine("</td>");
                }
                str.AppendLine("</tr>");
            }
            str.AppendLine("</table></body></html>");

            return str.ToString();
        }
    }
}
