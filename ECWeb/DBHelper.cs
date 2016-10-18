using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ECWeb
{
    public class DBHelper
    {
        public static T CreateInstance<T>(string dbServerType)
        {
            dbServerType = dbServerType.Replace("Service", "");
            dbServerType = "." + dbServerType + ".";
            Type t = AppHelper.ViewInterfaceTypes<T>().Where(m => m.FullName.Contains(dbServerType)).FirstOrDefault();

            return (T)Activator.CreateInstance(t);
        }

        public static EntityDB.IDatabaseService CreateInvokeDatabase(EJ.Databases databaseConfig)
        {
            string conStr = string.Format(databaseConfig.conStr, HttpContext.Current.Request.MapPath("/"));
            return EntityDB.DBContext.CreateDatabaseService(conStr, (EntityDB.DatabaseType)Enum.Parse(typeof(EntityDB.DatabaseType), databaseConfig.dbType.ToString()));
        }
    }
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