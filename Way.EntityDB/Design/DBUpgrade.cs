using Way.EntityDB.Design.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Way.EntityDB.Design
{
    public class DBUpgrade
    {
        public static void Upgrade(EntityDB.DBContext dbContext,string designData)
        {
            var dllType = dbContext.GetType().GetTypeInfo();
            while(dllType.BaseType != typeof(EntityDB.DBContext))
            {
                dllType = dllType.BaseType.GetTypeInfo();
            }
            var stream = dllType.Assembly.GetManifestResourceStream("database.actions");
            if(stream == null)
            {
                throw new Exception(dllType.Assembly.FullName + " 没有包含数据库结构！");
            }
            byte[] bs = new byte[stream.Length];
            stream.Read(bs, 0, bs.Length);

            using (var dset = Newtonsoft.Json.JsonConvert.DeserializeObject<WayDataSet>(System.Text.Encoding.UTF8.GetString(bs)))
            {
                bs = null;
                EntityDB.IDatabaseService db = dbContext.Database;
                EntityDB.DatabaseType dbType = dbContext.DatabaseType;

                IDatabaseDesignService dbservice = EntityDB.Design.DBHelper.CreateDatabaseDesignService(dbType);
                dbservice.CreateEasyJobTable(db);

                var dbconfig = db.ExecSqlString("select contentConfig from __WayEasyJob").ToString().ToJsonObject<DataBaseConfig>();
                if (string.IsNullOrEmpty(dbconfig.DatabaseGuid) == false && dbconfig.DatabaseGuid != dset.DataSetName)
                    throw new Exception("此结构脚本并不是对应此数据库");

                db.DBContext.BeginTransaction();
                var dtable = dset.Tables[0];
                try
                {
                    var query = dtable.Rows.Where(m=>(int)m["id"] > dbconfig.LastUpdatedID).OrderBy(m=>(int)m["id"]);

                    int count = query.Count();
                    int done = 0;
                    int? lastid = null;
                    foreach (var datarow in query)
                    {
                        string actionType = datarow["type"].ToString();
                        int id = Convert.ToInt32(datarow["id"]);

                        string json = datarow["content"].ToString();

                        if (actionType.StartsWith("ECWeb.Database.Actions"))
                        {
                            actionType = "EntityDB.Design." + actionType.Substring("ECWeb.Database.".Length);
                        }
                        Type type = typeof(EntityDB.Design.Actions.Action).GetTypeInfo().Assembly.GetType(actionType);
                        var actionItem = (EntityDB.Design.Actions.Action)Newtonsoft.Json.JsonConvert.DeserializeObject(json, type);

                        actionItem.Invoke(db);

                        done++;
                        lastid = id;

                    }
                    if (lastid != null)
                    {
                        SetLastUpdateID(lastid.Value, dset.DataSetName, db);
                    }
                    db.DBContext.CommitTransaction();
                }
                catch (Exception ex)
                {
                    db.DBContext.RollbackTransaction();
                    throw ex;
                }
            }
        }

        public static void SetLastUpdateID(object actionid, string databaseGuid, EntityDB.IDatabaseService db)
        {
            if (string.IsNullOrEmpty(databaseGuid))
                throw new Exception("Database Guid can not be empty");
            var dbconfig = db.ExecSqlString("select contentConfig from __WayEasyJob").ToString().ToJsonObject<DataBaseConfig>();
            dbconfig.LastUpdatedID = Convert.ToInt32(actionid);
            dbconfig.DatabaseGuid = databaseGuid;

            var data = new EntityDB.CustomDataItem("__WayEasyJob", null, null);
            data.SetValue("contentConfig", dbconfig.ToJsonString());
            db.Update(data);
        }
    }
}