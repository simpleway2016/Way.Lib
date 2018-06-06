using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Way.EntityDB;
using Way.EntityDB.Design;
using Way.Lib.ScriptRemoting;

namespace Way.EJServer
{
    public class ImportDataHandler : Way.Lib.ScriptRemoting.ICustomHttpHandler
    {
        public void Handle(string originalUrl, HttpConnectInformation connectInfo, ref bool handled)
        {
            if (originalUrl.Contains("/ImportTableData.aspx") == false)
                return;
            handled = true;

            if (connectInfo.Session["user"] == null || ((EJ.User)connectInfo.Session["user"]).Role.GetValueOrDefault().HasFlag(EJ.User_RoleEnum.数据库设计师) == false)
                return;
            try
            {
                int databaseid = Convert.ToInt32(connectInfo.Request.Query["dbid"]);
                System.IO.BinaryReader reader = new System.IO.BinaryReader(connectInfo.Request.Body);
                var tableNames = reader.ReadString().ToJsonObject<string[]>();

                using (EJDB ejdb = new EJDB())
                {
                    var database = ejdb.Databases.FirstOrDefault(m => m.id == databaseid);
                    var invokingDB = DBHelper.CreateInvokeDatabase(database);
                    { //不要开事物，大数据太慢
                        try
                        {
                            if (connectInfo.Request.Query["clearDataFirst"] == "1")
                            {
                                foreach (var tableName in tableNames)
                                {
                                    invokingDB.AllowIdentityInsert(tableName , true);
                                    invokingDB.ExecSqlString("delete from [" + tableName + "]");
                                }
                            }

                            Dictionary<string, string> pkNames = new Dictionary<string, string>();
                            for(int i = 0; i < tableNames.Length; i ++)
                            {
                                var tableName = tableNames[i];
                                var db_table = ejdb.DBTable.FirstOrDefault(m => m.Name == tableName);
                                if (db_table == null)
                                    throw new Exception(string.Format("找不到{0}数据表定义", tableName));
                                var pkcolumn = ejdb.DBColumn.FirstOrDefault(m => m.TableID == db_table.id && m.IsPKID == true);
                                bool hasAutoColumn = ejdb.DBColumn.Count(m => m.TableID == db_table.id && m.IsAutoIncrement == true) > 0;
                                pkNames[tableName] = pkcolumn.Name;

                                if (pkcolumn == null)
                                    throw (new Exception(string.Format("{0}-{1}没有设置主键", db_table.caption, db_table.Name)));
                                
                            }

                            while (true)
                            {
                                string _tablename = reader.ReadString();
                                if (_tablename == ":end")
                                    break;
                                string pkname = null;
                                if (pkNames.ContainsKey(_tablename))
                                    pkname = pkNames[_tablename];

                                Dictionary<string, object> data = reader.ReadString().ToJsonObject<Dictionary<string, object>>();
                                CustomDataItem newDataItem;

                                if (pkname != null && data.ContainsKey(pkname))
                                {
                                    if (Convert.ToInt32( invokingDB.ExecSqlString($"select count(*) from {invokingDB.FormatObjectName(_tablename)} where {invokingDB.FormatObjectName(pkname)}=@p0" , data[pkname])) > 0)
                                    {
                                        newDataItem = new CustomDataItem(_tablename, pkname, data[pkname]);
                                        foreach (var item in data)
                                            newDataItem.SetValue(item.Key, item.Value);

                                        invokingDB.Update(newDataItem);
                                        continue;
                                    }
                                }

                                newDataItem = new CustomDataItem(_tablename, pkname, null);
                                foreach (var item in data)
                                    newDataItem.SetValue(item.Key, item.Value);
                                invokingDB.Insert(newDataItem);
                            }
                        }
                        catch(Exception ex)
                        {
                            
                            throw ex;
                        }
                        finally
                        {
                            foreach (var tableName in tableNames)
                            {
                                invokingDB.AllowIdentityInsert(tableName, false);
                            }
                        }
                    }
                }
                connectInfo.Response.Write("ok\r\n");
            }
            catch(Exception ex)
            {
                connectInfo.Response.Write($"{ex.Message}\r\n");
            }
            
        }
    }
}
