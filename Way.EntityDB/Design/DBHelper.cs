using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Way.EntityDB.Design
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
                    var types = typeof(EntityDB.Design.Services.IDatabaseDesignService).GetTypeInfo().Assembly.GetTypes();
                    foreach (var type in types)
                    {
                        if (type.GetInterfaces().Any( m=>m == compareType))
                        {
                            var att = type.GetTypeInfo().GetCustomAttribute(typeof(Attributes.DatabaseTypeAttribute)) as Attributes.DatabaseTypeAttribute;
                            if (att != null)
                            {
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
                    var types = typeof(EntityDB.Design.Services.ITableDesignService).GetTypeInfo().Assembly.GetTypes();
                    foreach (var type in types)
                    {
                        if (type.GetInterfaces().Any(m => m == compareType))
                        {
                            var att = type.GetTypeInfo().GetCustomAttribute(typeof(EntityDB.Attributes.DatabaseTypeAttribute)) as EntityDB.Attributes.DatabaseTypeAttribute;
                            if (att != null)
                            {
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
            string conStr = databaseConfig.conStr;
            return EntityDB.DBContext.CreateDatabaseService(conStr, (EntityDB.DatabaseType)Enum.Parse(typeof(EntityDB.DatabaseType), databaseConfig.dbType.ToString()));
        }
    }
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