using EntityDB.Design.Database.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace EntityDB.Design
{
    public class DBUpgrade
    {
        public static void Upgrade(EntityDB.DBContext dbContext)
        {
            var stream = dbContext.GetType().Assembly.GetManifestResourceStream("database.actions");
            if(stream == null)
            {
                throw new Exception(dbContext.GetType().Assembly.FullName + " 没有包含数据库结构！");
            }
            using (var dset = new System.Data.DataSet())
            {
                dset.ReadXml(stream);

                System.Data.DataTable dtable = dset.Tables[0];
                System.Web.Script.Serialization.JavaScriptSerializer jsonObj = new System.Web.Script.Serialization.JavaScriptSerializer();

                EntityDB.IDatabaseService db = dbContext.Database;
                EntityDB.DatabaseType dbType = dbContext.DatabaseType;

                IDatabaseDesignService dbservice = EntityDB.Design.DBHelper.CreateInstance<IDatabaseDesignService>(dbType.ToString());
                dbservice.CreateEasyJobTable(db);

                var dbconfig = db.ExecSqlString("select contentConfig from __WayEasyJob").ToString().ToJsonObject<DataBaseConfig>();
                if (dbconfig.DatabaseGuid.IsNullOrEmpty() == false && dbconfig.DatabaseGuid != dset.DataSetName)
                    throw new Exception("此结构脚本并不是对应此数据库");

                db.DBContext.BeginTransaction();
                try
                {
                    dtable.DefaultView.RowFilter = "id>" + dbconfig.LastUpdatedID;
                    dtable.DefaultView.Sort = "id";
                    int count = dtable.DefaultView.Count;
                    int done = 0;
                    int? lastid = null;
                    foreach (System.Data.DataRowView datarow in dtable.DefaultView)
                    {
                        string actionType = datarow["type"].ToString();
                        int id = Convert.ToInt32(datarow["id"]);

                        string json = datarow["content"].ToString();


                        Type type = typeof(EntityDB.Design.Database.Actions.Action).Assembly.GetType(actionType);
                        var actionItem = (EntityDB.Design.Database.Actions.Action)jsonObj.Deserialize(json, type);

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
            if (databaseGuid.IsNullOrEmpty())
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