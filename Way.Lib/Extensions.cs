using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Way.Lib
{
    /// <summary>
    /// 
    /// </summary>
    public static class Extensions
    {
        /// <summary>
        /// 安全的ToString方法，对象如果是null，返回""
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string ToSafeString(this object obj)
        {
            if (obj == null)
                return "";
            return obj.ToString();
        }

        /// <summary>
        /// 判断字符串是否是null或者“”
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool IsNullOrEmpty(this string str)
        {
            return string.IsNullOrEmpty(str);
        }

        /// <summary>
        /// 把对象转换为json字符串
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string ToJsonString(this object obj)
        {
            if (obj == null)
                return null;
            return Newtonsoft.Json.JsonConvert.SerializeObject(obj);
        }

#if NET46
        /// <summary>
        /// 把Form转换为json字符串
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string ToJson(this System.Collections.Specialized.NameValueCollection form)
        {
            Dictionary<string, string> dic = new Dictionary<string, string>();
            for (int i = 0; i < form.Keys.Count; i++)
            {
                dic[form.Keys[i]] = form[i];
            }
            return dic.ToJsonString();
        }
#endif

        /// <summary>
        /// 把json字符串转换为实例对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="json"></param>
        /// <returns></returns>
        public static T FromJson<T>(this string json)
        {
            return Newtonsoft.Json.JsonConvert.DeserializeObject<T>(json);
        }

        public static DateTime ToDateTime(this string str)
        {
            try
            {
                return Convert.ToDateTime(str);
            }
            catch
            {
                return DateTime.MinValue;
            }
        }
    }
}
