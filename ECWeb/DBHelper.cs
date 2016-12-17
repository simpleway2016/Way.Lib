using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ECWeb
{
   

    public static class MyExtensions
    {

        static System.Web.Script.Serialization.JavaScriptSerializer json = new System.Web.Script.Serialization.JavaScriptSerializer();
        public static string ToJsonString(this object obj)
        {
            if (obj == null)
                return null;
            return json.Serialize(obj);
        }
        public static T ToJsonObject<T>(this string str)
        {
            if (str == null)
                return default(T);
            return json.Deserialize<T>(str);
        }

        public static byte[] ToJsonBytes(this object obj)
        {
            if (obj == null)
                return null;
            return System.Text.Encoding.UTF8.GetBytes( json.Serialize(obj));
        }
    }
}