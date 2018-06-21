using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Way.EJServer
{
   
    static class Helper
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="type"></param>
        public static Type GetCSharpType(string type)
        {
            switch (type)
            {

                case "bigint":
                    return typeof(Int64);
                case "binary":
                    return typeof(byte[]);
                case "bit":
                    return typeof(bool);
                case "char":
                    return typeof(string);
                case "datetime":
                    return typeof(DateTime);
                case "decimal":
                    return typeof(decimal);
                case "float":
                    return typeof(float);
                case "double":
                    return typeof(double);
                case "image":
                    return typeof(byte[]);
                case "int":
                    return typeof(Int32);
                case "money":
                    return typeof(decimal);
                case "nchar":
                    return typeof(string);
                case "ntext":
                    return typeof(string);
                case "numeric":
                    return typeof(decimal);
                case "nvarchar":
                    return typeof(string);
                case "real":
                    return typeof(double);
                case "smalldatetime":
                    return typeof(DateTime);
                case "smallint":
                    return typeof(int);
                case "smallmoney":
                    return typeof(decimal);
                case "text":
                    return typeof(string);
                case "timestamp":
                    return typeof(int);
                case "varbinary":
                    return typeof(byte[]);
                case "varchar":
                    return typeof(string);
                default:
                    return null;

            }
        }
    }
    public static class MyExtensions
    {

        public static string GetIndexName(this string[] columns,string tablename)
        {
            StringBuilder str = new StringBuilder();
            str.Append(tablename);
            str.Append("_ej_");
            for(int i = 0; i < columns.Length; i ++)
            {
                str.Append(columns[i].ToLower());
                if (i < columns.Length - 1)
                    str.Append('_');
            }
            return str.ToString();
        }
        /// <summary>
        /// 每个对象逗号隔开
        /// </summary>
        /// <param name="arrs"></param>
        /// <returns></returns>
        public static string ToSplitString(this Array arrs)
        {
            return arrs.ToSplitString(",");
        }
        /// <summary>
        /// 用制定字符串联数组
        /// </summary>
        /// <param name="arrs"></param>
        /// <param name="splitchar">间隔字符</param>
        /// <returns></returns>
        public static string ToSplitString(this Array arrs, string splitchar)
        {
            StringBuilder result = new StringBuilder();
            foreach (object str in arrs)
            {
                if (result.Length > 0)
                    result.Append(splitchar);
                result.Append(str.ToString().Trim());
            }
            return result.ToString();
        }
        public static string ToSplitString(this Array arrs, string splitchar, string itemFormat)
        {
            StringBuilder result = new StringBuilder();
            foreach (object str in arrs)
            {
                if (result.Length > 0)
                    result.Append(splitchar);
                result.Append(string.Format(itemFormat, str));
            }
            return result.ToString();
        }

        public static string ToJsonString(this object obj)
        {
            if (obj == null)
                return null;
            return Newtonsoft.Json.JsonConvert.SerializeObject(obj);
        }
        public static T ToJsonObject<T>(this string str)
        {
            if (str == null)
                return default(T);
            return Newtonsoft.Json.JsonConvert.DeserializeObject<T>(str);
        }

        public static byte[] ToJsonBytes(this object obj)
        {
            if (obj == null)
                return null;
            return System.Text.Encoding.UTF8.GetBytes(Newtonsoft.Json.JsonConvert.SerializeObject(obj));
        }
    }
}