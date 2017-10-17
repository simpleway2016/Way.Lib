using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SunRizStudio
{
    static class MyExtensions
    {

        public static string ToJsonString(this object obj)
        {
            if (obj == null)
                return null;
            return Newtonsoft.Json.JsonConvert.SerializeObject(obj);
        }
        public static T ToJsonObject<T>(this string str)
        {
            return Newtonsoft.Json.JsonConvert.DeserializeObject<T>(str);
        }
    }
}
