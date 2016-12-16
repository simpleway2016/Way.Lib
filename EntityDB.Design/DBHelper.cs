using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EntityDB.Design
{
    public class IndexInfo
    {
        public bool IsUnique;
        public bool IsClustered;
        public string[] ColumnNames;
        public string Name;
    }
    public class BugItem : EJ.Bug
    {
        public string SubmitUserName
        {
            get;
            set;
        }
        public string HandlerUserName
        {
            get;
            set;
        }
    }
    public class BugHistoryItem
    {
        public string UserName
        {
            get;
            set;
        }
        public byte[] Content
        {
            get;
            set;
        }
        public DateTime? SubmitTime
        {
            get;
            set;
        }
    }
    public class DBHelper
    {
        static DBHelper()
        {
            var t = DatabaseDesignServiceTypes.Count;
            t = TableDesignServiceTypes.Count;
        }
        static Dictionary<EntityDB.DatabaseType,Type> _DatabaseDesignServiceTypes;
        static Dictionary<EntityDB.DatabaseType, Type> DatabaseDesignServiceTypes
        {
            get
            {
                //在DBHelper 静态构造函数中调用一下，进行初始化，防止多线程同时运行这里来，造成冲突
                if (_DatabaseDesignServiceTypes == null)
                {
                    var compareType = typeof(EntityDB.Design.Services.IDatabaseDesignService);
                    _DatabaseDesignServiceTypes = new Dictionary<DatabaseType, Type>();
                    var types = typeof(EntityDB.Design.Services.IDatabaseDesignService).Assembly.GetTypes();
                    foreach (var type in types)
                    {
                        if (type.GetInterfaces().Any( m=>m == compareType))
                        {
                            var attrs = type.GetCustomAttributes(typeof(EntityDB.Attributes.DatabaseTypeAttribute), false);
                            if (attrs.Length > 0)
                            {
                                EntityDB.Attributes.DatabaseTypeAttribute att = (EntityDB.Attributes.DatabaseTypeAttribute)attrs[0];
                                _DatabaseDesignServiceTypes[att.DBType] = type;
                            }
                        }
                    }
                }
                return _DatabaseDesignServiceTypes;
            }
        }


        static Dictionary<EntityDB.DatabaseType, Type> _TableDesignServiceTypes;
        static Dictionary<EntityDB.DatabaseType, Type> TableDesignServiceTypes
        {
            get
            {
                //在DBHelper 静态构造函数中调用一下，进行初始化，防止多线程同时运行这里来，造成冲突
                if (_TableDesignServiceTypes == null)
                {
                    var compareType = typeof(EntityDB.Design.Services.ITableDesignService);
                    _TableDesignServiceTypes = new Dictionary<DatabaseType, Type>();
                    var types = typeof(EntityDB.Design.Services.ITableDesignService).Assembly.GetTypes();
                    foreach (var type in types)
                    {
                        if (type.GetInterfaces().Any(m => m == compareType))
                        {
                            var attrs = type.GetCustomAttributes(typeof(EntityDB.Attributes.DatabaseTypeAttribute), false);
                            if (attrs.Length > 0)
                            {
                                EntityDB.Attributes.DatabaseTypeAttribute att = (EntityDB.Attributes.DatabaseTypeAttribute)attrs[0];
                                _TableDesignServiceTypes[att.DBType] = type;
                            }
                        }
                    }
                }
                return _TableDesignServiceTypes;
            }
        }

        public static EntityDB.Design.Services.IDatabaseDesignService CreateDatabaseDesignService(EntityDB.DatabaseType dbtype)
        {
            var type = DatabaseDesignServiceTypes[dbtype];
            if (type == null)
            {
                throw new Exception(dbtype + "没有对应的IDatabaseDesignService实现类");
            }
            return (EntityDB.Design.Services.IDatabaseDesignService)Activator.CreateInstance(DatabaseDesignServiceTypes[dbtype]);
        }
        public static EntityDB.Design.Services.ITableDesignService CreateTableDesignService(EntityDB.DatabaseType dbtype)
        {
            var type = TableDesignServiceTypes[dbtype];
            if (type == null)
            {
                throw new Exception(dbtype + "没有对应的ITableDesignService实现类");
            }
            return (EntityDB.Design.Services.ITableDesignService)Activator.CreateInstance(TableDesignServiceTypes[dbtype]);
        }

        public static EntityDB.IDatabaseService CreateInvokeDatabase(EJ.Databases databaseConfig)
        {
            string conStr = null;
            if (System.Web.HttpRuntime.AppDomainAppPath != null)
            {
                conStr = string.Format(databaseConfig.conStr, System.Web.HttpRuntime.AppDomainAppPath);
            }
            else
            {
                conStr = string.Format(databaseConfig.conStr,AppDomain.CurrentDomain.BaseDirectory);
            }
            return EntityDB.DBContext.CreateDatabaseService(conStr, (EntityDB.DatabaseType)Enum.Parse(typeof(EntityDB.DatabaseType), databaseConfig.dbType.ToString()));
        }
    }
    static class MyExtensions
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