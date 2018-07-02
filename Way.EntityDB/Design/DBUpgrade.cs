using Way.EntityDB.Design.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.IO.Compression;

namespace Way.EntityDB.Design
{
    public class ColumnType
    {
        static List<string> _supportTypes = new List<string>(new string[] {
                                            "varchar",
                                            "int",
                                            "image",
                                            "text",
                                            "smallint",
                                            "smalldatetime",
                                            "real",
                                            "datetime",
                                            "float",
                                            "double",
                                            "bit",
                                            "decimal",
                                            "numeric",
                                            "bigint",
                                            "varbinary",
                                            "char",
                                            "timestamp", });
        /// <summary>
        /// 目前支持的数据库字段类型
        /// </summary>
       public static List<string> SupportTypes
        {
            get
            {
                return _supportTypes;
            }
        }
    }
    public class DBUpgrade
    {
        static byte[] UnGzip(byte[] zippedData)
        {
            MemoryStream ms = new MemoryStream(zippedData);
            GZipStream compressedzipStream = new GZipStream(ms, CompressionMode.Decompress);
            MemoryStream outBuffer = new MemoryStream();
            byte[] block = new byte[1024];
            while (true)
            {
                int bytesRead = compressedzipStream.Read(block, 0, block.Length);
                if (bytesRead <= 0)
                    break;
                else
                    outBuffer.Write(block, 0, bytesRead);
            }
            compressedzipStream.Close();
            return outBuffer.ToArray();
        }

        public static void Upgrade(EntityDB.DBContext dbContext,string designData)
        {
            if (designData.IsNullOrEmpty())
                return;

            byte[] bs;
            if (designData.StartsWith("\r\n"))
            {
                bs = System.Convert.FromBase64String(designData.Substring(2));
                bs = UnGzip(bs);
            }
            else
            {
                bs = System.Convert.FromBase64String(designData);
            }
            using (var dset = Newtonsoft.Json.JsonConvert.DeserializeObject<WayDataSet>(System.Text.Encoding.UTF8.GetString(bs)))
            {
                if (dbContext == null)
                    return;

                bs = null;
                EntityDB.IDatabaseService db = dbContext.Database;
                EntityDB.DatabaseType dbType = dbContext.DatabaseType;

                IDatabaseDesignService dbservice = EntityDB.Design.DBHelper.CreateDatabaseDesignService(dbType);
                dbservice.CreateEasyJobTable(db);

                var dbconfig = db.ExecSqlString("select contentConfig from __wayeasyjob").ToString().ToJsonObject<DataBaseConfig>();
                if (string.IsNullOrEmpty(dbconfig.DatabaseGuid) == false && dbconfig.DatabaseGuid != dset.DataSetName)
                    throw new Exception("此结构脚本并不是对应此数据库");

               
                var dtable = dset.Tables[0];
                try
                {
                    var query = dtable.Rows.Where(m=>(long)m["id"] > dbconfig.LastUpdatedID).OrderBy(m=>(long)m["id"]).ToList();
                   
                    if (query.Count > 0)
                    {
                        int? lastid = Convert.ToInt32(query.Last()["id"]);
                        var assembly = typeof(Way.EntityDB.Design.Actions.CreateTableAction).GetTypeInfo().Assembly;
                        db.DBContext.BeginTransaction();
                        foreach (var datarow in query)
                        {
                            string actionType = datarow["type"].ToString();

                            string json = datarow["content"].ToString();

                            Type type = assembly.GetType($"Way.EntityDB.Design.Actions.{actionType}");
                            var actionItem = (EntityDB.Design.Actions.Action)Newtonsoft.Json.JsonConvert.DeserializeObject(json, type);

                            actionItem.Invoke(db);

                        }

                        SetLastUpdateID(lastid.Value, dset.DataSetName, db);
                        db.DBContext.CommitTransaction();
                    }
                }
                catch
                {
                    db.DBContext.RollbackTransaction();
                    throw;
                }
            }
        }

        public static void SetLastUpdateID(object actionid, string databaseGuid, EntityDB.IDatabaseService db)
        {
            if (string.IsNullOrEmpty(databaseGuid))
                throw new Exception("Database Guid can not be empty");
            var dbconfig = db.ExecSqlString("select contentConfig from __wayeasyjob").ToString().ToJsonObject<DataBaseConfig>();
            dbconfig.LastUpdatedID = Convert.ToInt32(actionid);
            dbconfig.DatabaseGuid = databaseGuid;

            var data = new EntityDB.CustomDataItem("__wayeasyjob", null, null);
            data.SetValue("contentConfig", dbconfig.ToJsonString());
            db.Update(data);
        }
    }
}