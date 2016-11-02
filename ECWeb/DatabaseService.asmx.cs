
using EntityDB.Design;
using EntityDB.Design.Actions;
using EntityDB.Design.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Services;

namespace ECWeb
{
    /// <summary>
    /// DatabaseService 的摘要说明
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // 若要允许使用 ASP.NET AJAX 从脚本中调用此 Web 服务，请取消注释以下行。 
    // [System.Web.Script.Services.ScriptService]
    public class DatabaseService : WebServiceBase
    {
        [WebMethod(EnableSession = true)]
        public string test( )
        {
            //string constr = @"data source=D:\代码\EasyJob\ECWeb\test.sqlite";
            //using (EntityDB.IDataBaseService db = EntityDB.IDataBaseService.CreateDatabase(constr, DataSpace.DatabaseType.Sqlite))
            //{
            //    var dt = db.Select("select * from (select * from t1) as t2");
            //}
            return HttpContext.Current.Request.UserHostAddress;
        }
        [WebMethod(EnableSession = true)]
        public int[] Login(string name , string pwd)
        {
            string clientip = System.Configuration.ConfigurationManager.AppSettings["ClientIP"];
            if (!string.IsNullOrEmpty(clientip))
            {
                if (clientip.Contains(HttpContext.Current.Request.UserHostAddress) == false)
                    throw new Exception("此IP不允许登录");
            }
            using (EJDB db = new EJDB())
            {
                if (db.User.Any() == false)
                {
                    //如果没有任何用户，需要添加一个sa用户
                    var saUser = new EJ.User();
                    saUser.Name = "sa";
                    saUser.Password = "1";
                    saUser.Role = EJ.User_RoleEnum.管理员;
                    db.Insert(saUser);
                }
                var user = db.User.FirstOrDefault(m=>m.Name == name);
                if(user.Password != pwd)
                    throw new Exception("用户名密码错误");
                Session["user"] = user;
                return new int[]{(int)user.Role,user.id.Value};
            }
        }
        [WebMethod(EnableSession = true)]
        public string CreateProject(string name)
        {
            if(this.User == null)
                throw new Exception("请重新登陆");
            if (this.User.Role != EJ.User_RoleEnum.管理员)
                throw new Exception("无权进行此项操作");
            using (EJDB db = new EJDB())
            {
                EJ.Project project = new EJ.Project()
                {
                    Name = name
                };
                db.Update(project);
                return project.ToJsonString();
            }
        }
        [WebMethod(EnableSession = true)]
        public void DeleteProject(int id )
        {
            if (this.User == null)
                throw new Exception("请重新登陆");
            if (this.User.Role != EJ.User_RoleEnum.管理员)
                throw new Exception("无权进行此项操作");
            using (EJDB db = new EJDB())
            {
                db.Delete(db.Project.Where(m=>m.id == id));
            }
        }
        [WebMethod(EnableSession = true)]
        public void UpdateProject(int id , string name)
        {
            if (this.User == null)
                throw new Exception("请重新登陆");
            if(this.User.Role != EJ.User_RoleEnum.管理员)
                throw new Exception("无权进行此项操作");
            using (EJDB db = new EJDB())
            {
                var project = db.Project.FirstOrDefault(m=>m.id == id);
                project.Name = name;
                db.Update(project); 
            }
        }
        [WebMethod(EnableSession = true)]
        public string GetUserNameByID(int id)
        {
            if (this.User == null)
                throw new Exception("请重新登陆"); 
            using (EJDB db = new EJDB())
            {
                return db.User.FirstOrDefault(m => m.id == id).Name;
            }
        }
        [WebMethod(EnableSession = true)]
        public void ChangeModuleParent(int moduleid, int parentid)
        {
            using (EJDB_Check db = new EJDB_Check())
            {
                var module = db.DBModule.FirstOrDefault(m => m.id == moduleid);
                if (module == null)
                {
                    throw new Exception("你无法访问此数据模块");
                }
                module.parentID = parentid;
                db.Update(module);
            }
        }

        [WebMethod(EnableSession = true)]
        public void ChangePassword(string oldpwd,string newpwd)
        {
            if (this.User == null)
                throw new Exception("请重新登陆");
            if(this.User.Password != oldpwd)
                throw new Exception("旧密码错误");

            using (EJDB db = new EJDB())
            {
                this.User.Password = newpwd;
                db.Update(this.User);
            }
        }
        /// <summary>
        /// 获取当前用户有权查看的项目
        /// </summary>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        public string GetCurrentUserProjectList()
        {
            if (this.User == null)
                throw new Exception("请重新登陆"); 
            using (EJDB_Check db = new EJDB_Check())
            {
                return db.Project.ToList().ToJsonString();
            }
        }
        [WebMethod(EnableSession = true)]
        public string GetTablesInModule(int moduleid)
        {
            if (this.User == null)
                throw new Exception("请重新登陆"); 
            using (EJDB_Check db = new EJDB_Check())
            {
                var result = (from m in db.TableInModule
                             where m.ModuleID == moduleid
                             select m).ToList();
                foreach (var tinm in result)
                {
                    tinm.flag = db.DBTable.FirstOrDefault(m => m.id == tinm.TableID).ToJsonString();
                    tinm.flag2 = GetColumns(tinm.TableID.Value);
                }

                return result.ToJsonString();
            }
        }
        [WebMethod(EnableSession = true)]
        public string GetPkColumnName(int tableid)
        {
            if (this.User == null)
                throw new Exception("请重新登陆"); 
            using (EJDB db = new EJDB())
            {
                var table = db.DBTable.FirstOrDefault(m => m.id == tableid);
                return db.DBColumn.FirstOrDefault(m => m.TableID == table.id && m.IsPKID == true).Name;
            }
        }
        [WebMethod(EnableSession = true)]
        public void SaveDataTable(System.Data.DataTable dt,int tableid,List<string> delIds)
        {
            if (this.User == null)
                throw new Exception("请重新登陆"); 
            try
            {
                using (EJDB db = new EJDB())
                {
                    var table = db.DBTable.FirstOrDefault(m => m.id == tableid);
                    var database = db.Databases.FirstOrDefault(m => m.id == table.DatabaseID);
                    var invokingDB = DBHelper.CreateInvokeDatabase(database);
                    
                        IDatabaseDesignService service = DBHelper.CreateDatabaseDesignService(invokingDB.DBContext.DatabaseType);
                        var columns = db.DBColumn.Where(m=>m.TableID == table.id).ToList();
                        var pkcolumn = columns.FirstOrDefault(m => m.IsPKID == true && m.TableID == table.id);
                        if (pkcolumn == null)
                            throw new Exception(string.Format("表{0}缺少主键",table.Name));
                        foreach (string id in delIds)
                        {
                            invokingDB.ExecSqlString("delete from " + string.Format(service.GetObjectFormat(), table.Name) + " where " + string.Format(service.GetObjectFormat(), pkcolumn.Name) + "='" + id + "'");
                        }
                        foreach (System.Data.DataRow drow in dt.Rows)
                        {
                            if (drow.RowState == System.Data.DataRowState.Added)
                            {
                                var dataitem = new EntityDB.CustomDataItem(table.Name , null,null);
                                foreach (var column in columns)
                                {
                                    if(column.IsAutoIncrement == false)
                                        dataitem.SetValue(column.Name, drow[column.Name]);
                                }
                                invokingDB.Insert(dataitem);
                            }
                            else if (drow.RowState == System.Data.DataRowState.Modified)
                            {
                                var dataitem = new EntityDB.CustomDataItem(table.Name, pkcolumn.Name, drow[pkcolumn.Name]);
                                foreach (var column in columns)
                                {
                                    if (column.IsAutoIncrement == false && column.IsPKID == false)
                                        dataitem.SetValue(column.Name, drow[column.Name]);
                                }
                                invokingDB.Update(dataitem);
                            }
                        }

                    
                    
                }

              
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                dt.Dispose();
            }
        }
        [WebMethod(EnableSession = true)]
        public void DeleteDatabase(int databaseid)
        {
            if (this.User == null)
                throw new Exception("请重新登陆"); 
            using (EJDB db = new EJDB())
            {
                db.Delete(db.Databases.Where(m=>m.id == databaseid));
            }
        }
        [WebMethod(EnableSession = true)]
        public System.Data.DataTable GetActions(int databaseid)
        {
            if (this.User == null)
                throw new Exception("请重新登陆"); 
            using (EJDB db = new EJDB())
            {
                var database = db.Databases.FirstOrDefault(m=>m.id == databaseid);
                var dt = db.Database.SelectTable("select * from __action where databaseid=" + databaseid + " order by [id]");
                dt.TableName = database.dbType.ToString();
                return dt;
            }
        }
        [WebMethod(EnableSession = true)]
        public void RemoveTableFromModule(int tableInModuleId,int tableid)
        {
            if (this.User == null)
                throw new Exception("请重新登陆"); 
            using (EJDB db = new EJDB())
            {
                var item = db.TableInModule.FirstOrDefault(m=>m.id == tableInModuleId);
                if (item != null)
                    db.Delete(item);
            }
        }

        static System.Reflection.MethodInfo SqlQueryMethod;
        [WebMethod(EnableSession = true)]
        public System.Data.DataTable GetDataTable(byte[] bs ,int tableid, int pageindex ,int pagesize)
        {
            if (this.User == null)
                throw new Exception("请重新登陆");
            using (EJDB db = new EJDB())
            {
                string sql = System.Text.Encoding.UTF8.GetString(bs);
                var dbtable = db.DBTable.FirstOrDefault(m => m.id == tableid);
                var database = db.Databases.FirstOrDefault(m => m.id == dbtable.DatabaseID);
                EntityDB.IDatabaseService invokingDB = DBHelper.CreateInvokeDatabase(database);

                if (sql.StartsWith("select "))
                {
                   
                    var dt = invokingDB.SelectTable(sql, pageindex * pagesize, pagesize);
                    dt.TableName = invokingDB.ExecSqlString(string.Format("select count(*) from ({0}) as t1", sql)).ToString();
                    return dt;
                }
                else
                {
                    var dt = invokingDB.SelectTable(sql );
                    dt.TableName = dt.Rows.Count.ToString();
                   
                    return dt;
                }
            }
        }
        [WebMethod(EnableSession = true)]
        public void ImportData(System.Data.DataSet dset , int databaseid,bool clearDataFirst)
        {
            if (this.User == null)
                throw new Exception("请重新登陆");
            try
            {
                using (EJDB db = new EJDB())
                {
                    var database = db.Databases.FirstOrDefault(m => m.id == databaseid);
                    var invokingDB = DBHelper.CreateInvokeDatabase(database);
                    {
                        //不开事务，太慢
                        var service = DBHelper.CreateDatabaseDesignService(database.dbType);
                        service.ImportData(invokingDB, db, dset, clearDataFirst);
                        dset.Dispose();
                    }
                }
            }
            catch (Exception ex)
            {
                using (CLog log = new CLog("ImportData error "))
                {
                    log.Log(ex.ToString());
                }
                throw ex;
            }
        }
        [WebMethod(EnableSession = true)]
        public System.Data.DataSet GetDataSet(int[] tableids)
        {
            if (this.User == null)
                throw new Exception("请重新登陆"); 
            using (EJDB db = new EJDB())
            {
                int tid = tableids[0];
                var dbtable = db.DBTable.FirstOrDefault(m => m.id == tid);
                var database = db.Databases.FirstOrDefault(m => m.id == dbtable.DatabaseID);
                var invokingDB = DBHelper.CreateInvokeDatabase(database);
                {
                    StringBuilder sql = new StringBuilder();
                    string[] tablenames = new string[tableids.Length];
                    for (int i = 0; i < tableids.Length; i ++ )
                    {
                        int tableid = tableids[i];
                        dbtable = db.DBTable.FirstOrDefault(m => m.id == tableid);
                        tablenames[i] = dbtable.Name;
                        sql.Append(string.Format("select * from {0}\r\n",invokingDB.FormatObjectName( dbtable.Name)));
                    }
                    var dataset = invokingDB.SelectDataSet(sql.ToString());
                    for (int i = 0; i < tablenames.Length; i++)
                    {
                        dataset.Tables[i].TableName = tablenames[i];
                    }
                    return dataset;
                }

            }
        }
        [WebMethod(EnableSession = true)]
        public string GetDatabase(int databaseid)
        {
            if (this.User == null)
                throw new Exception("请重新登陆"); 
            using (EJDB db = new EJDB())
            {
                return db.Databases.FirstOrDefault(m => m.id == databaseid).ToJsonString();
            }
            return null;
        }
        [WebMethod(EnableSession = true)]
        public string GetDatabaseList(int projectid)
        {
            if (this.User == null)
                throw new Exception("请重新登陆"); 
            using (EJDB_Check db = new EJDB_Check())
            {
                return db.Databases.Where(m => m.ProjectID == projectid).OrderBy(m => m.Name).ToList().ToJsonString();
            }
        }
        [WebMethod(EnableSession = true)]
        public int GetDBModuleID(int tableid)
        {
            if (this.User == null)
                throw new Exception("请重新登陆"); 
            using (EJDB_Check db = new EJDB_Check())
            {
                var table = db.DBTable.FirstOrDefault(m => m.id == tableid);
                var module = (from m in db.DBModule
                             join n in db.TableInModule on m.id equals n.ModuleID
                             where n.TableID == tableid
                             select m).FirstOrDefault();
                if (module != null)
                    return module.id.Value;
                return 0;
            }
        }
        [WebMethod(EnableSession = true)]
        public int UpdateTableInMoudle(string tableInModule)
        {
            if (this.User == null)
                throw new Exception("请重新登陆"); 
            using (EJDB db = new EJDB())
            {
                try
                {
                    db.BeginTransaction();

                    EJ.TableInModule tInM = tableInModule.ToJsonObject<EJ.TableInModule>();
                    if (tInM.id == null && this.User.Role.HasFlag(EJ.User_RoleEnum.数据库设计师) == false)
                        throw new Exception("你没有权限进行此操作");

                    if (tInM.id != null && this.User.Role.HasFlag(EJ.User_RoleEnum.数据库设计师) == false)
                    {
                        //更新位置的话直接忽略
                        return 0;
                    }

                    if (db.DBTable.Count(m => m.id == tInM.TableID) == 0)
                        throw new Exception("该数据表已被删除");
                    if (db.DBModule.Count(m => m.id == tInM.ModuleID) == 0)
                        throw new Exception("该数据模块已被删除");

                    if (tInM.id == null)
                    {
                        if( db.TableInModule.Count(m=>m.TableID == tInM.TableID && m.ModuleID == tInM.ModuleID) > 0 )
                            throw new Exception("该数据表在此模块已经存在");
                    }

                    db.Update(tInM);

                    db.CommitTransaction();
                    return tInM.id.GetValueOrDefault();
                }
                catch (Exception ex)
                {
                    db.RollbackTransaction();
                    throw ex;
                }
            }
        }

        [WebMethod(EnableSession = true)]
        public string GetDeleteConfigInModule(int moduleid)
        {
            if (this.User == null)
                throw new Exception("请重新登陆"); 
            using (EJDB db = new EJDB())
            {
                var tableids = from m in db.TableInModule
                            where m.ModuleID == moduleid
                            select m.TableID;
                var result = from m in db.DBDeleteConfig
                             where tableids.Contains(m.TableID) && tableids.Contains(m.RelaTableID)
                             select m;
                return result.ToArray().ToJsonString();
            }
        }

        [WebMethod(EnableSession = true)]
        public string[] GetTableNames(int databaseid)
        {
            if (this.User == null)
                throw new Exception("请重新登陆"); 
            using (EJDB db = new EJDB())
            {
                var names = from m in db.DBTable
                            where m.DatabaseID == databaseid
                            select m.Name;
                return names.ToArray();
            }
        }
        [WebMethod(EnableSession = true)]
        public string[] GetColumnNames(int databaseid,string tableName)
        {
            if (this.User == null)
                throw new Exception("请重新登陆"); 
            using (EJDB db = new EJDB())
            {
                var tableid = (from m in db.DBTable
                            where m.DatabaseID == databaseid && m.Name == tableName
                            select m.id).FirstOrDefault();
                var names = from m in db.DBColumn
                            where m.TableID == tableid
                            orderby m.orderid
                            select m.Name;
                return names.ToArray();
            }
        }
        [WebMethod(EnableSession = true)]
        public string GetColumns(  int tableid)
        {
            if (this.User == null)
                throw new Exception("请重新登陆"); 
            using (EJDB db = new EJDB())
            {
                var result = from m in db.DBColumn
                            where m.TableID == tableid
                            orderby m.orderid
                            select m;
                return result.ToList().ToJsonString();
            }
        }
        [WebMethod(EnableSession = true)]
        public string GetDBModuleList(int databaseid,int parentid)
        {
            if (this.User == null)
                throw new Exception("请重新登陆"); 
            using (EJDB db = new EJDB())
            {
                return db.DBModule.Where(m => m.DatabaseID == databaseid && m.parentID == parentid).OrderBy(m => m.Name).ToList().ToJsonString();
            }
        }
        [WebMethod(EnableSession = true)]
        public string GetTableDeleteConfigList(int tableid)
        {
            if (this.User == null)
                throw new Exception("请重新登陆"); 
            using (EJDB db = new EJDB())
            {
                var results = db.DBDeleteConfig.Where(m => m.TableID == tableid).ToList();
                foreach (var item in results)
                {
                    item.RelaTable_Desc = db.DBTable.FirstOrDefault(m => m.id == item.RelaTableID).Name;
                    item.RelaColumn_Desc = db.DBColumn.FirstOrDefault(m => m.id == item.RelaColumID).Name;
                }
                return results.ToJsonString();
            }
        }
        [WebMethod(EnableSession = true)]
        public string GetTableIDXIndexList(int tableid)
        {
            if (this.User == null)
                throw new Exception("请重新登陆"); 
            using (EJDB db = new EJDB())
            {
                var results = db.IDXIndex.Where(m => m.TableID == tableid).ToList();
                return results.ToJsonString();
            }
        }
        [WebMethod(EnableSession = true)]
        public string GetTableList(int databaseid)
        {
            if (this.User == null)
                throw new Exception("请重新登陆"); 
            using (EJDB_Check db = new EJDB_Check())
            {
                return db.DBTable.Where(m => m.DatabaseID == databaseid).OrderBy(m => m.Name).ToList().ToJsonString();
            }
        }
         [WebMethod(EnableSession = true)]
        public string GetObjectFormat(int tableid)
        {
            using (EJDB db = new EJDB())
            {
                var dbid = db.DBTable.Where(m => m.id == tableid).FirstOrDefault().DatabaseID;
                var database = db.Databases.FirstOrDefault(m => m.id == dbid);
                var invokingDB = DBHelper.CreateInvokeDatabase(database);
                {
                    IDatabaseDesignService service = DBHelper.CreateDatabaseDesignService(invokingDB.DBContext.DatabaseType);
                    return service.GetObjectFormat();
                }
            }
           
        }
        [WebMethod(EnableSession = true)]
        public string GetColumnList(int tableid)
        {
            if (this.User == null)
                throw new Exception("请重新登陆"); 
            using (EJDB db = new EJDB())
            {
                return db.DBColumn.Where(m => m.TableID == tableid).OrderBy(m=>m.orderid).ToList().ToJsonString();
            }
            return null;
        }
        [WebMethod(EnableSession = true)]
        public void ModifyTable(string newtablejson, string nowcolumnsjson, string deleteConfigJson,string idxJson)
        {
            if (this.User == null)
                throw new Exception("请重新登陆"); 
            using (EJDB db = new EJDB())
            {
                EntityDB.IDatabaseService invokingDB = null;
                IndexInfo[] idxConfigs = idxJson.ToJsonObject<IndexInfo[]>();

                EJ.DBTable newtable = newtablejson.ToJsonObject<EJ.DBTable>();
                EJ.DBTable oldtable = db.DBTable.FirstOrDefault(m=>m.id == newtable.id);

                EJ.DBColumn[] nowcolumns = nowcolumnsjson.ToJsonObject<EJ.DBColumn[]>();
                var nowids = (from m in nowcolumns
                              where m.id != null
                              select m.id).ToArray();

                try
                {
                    db.BeginTransaction(System.Data.IsolationLevel.ReadCommitted);
                    //锁住，防止同时添加table
                    EJ.Databases database = db.Databases.Where(m => m.id == oldtable.DatabaseID).FirstOrDefault();
                    database.iLock++;
                    db.Update(database);

                    if (db.DBTable.Where(m => m.DatabaseID == oldtable.DatabaseID && m.id != newtable.id && m.Name == newtable.Name).Count() > 0)
                        throw new Exception("数据表名称重复");

                    db.Database.ExecSqlString("delete from IDXIndex where TableID=" + oldtable.id);
                    foreach (var config in idxConfigs)
                    {
                        EJ.IDXIndex idxIndex = new EJ.IDXIndex();
                        idxIndex.TableID = oldtable.id;
                        idxIndex.IsUnique = config.IsUnique;
                        idxIndex.IsClustered = config.IsClustered;
                        idxIndex.Keys = config.ColumnNames.ToSplitString();
                        db.Update(idxIndex);
                    }

                    var oldcolumns = db.DBColumn.Where(m => m.TableID == oldtable.id).ToArray().ToJsonString().ToJsonObject<EJ.DBColumn[]>();
                    //找出下面这些对象
                    EJ.DBColumn[] addcolumns = nowcolumns.Where(m => m.id == null).ToArray();
                    EJ.DBColumn[] delColumns = oldcolumns.Where(m => nowids.Contains(m.id) == false).ToArray();

                    List<EJ.DBColumn> otherColumns = new List<EJ.DBColumn>();
                    otherColumns.AddRange(oldcolumns.Where(m => nowids.Contains(m.id)).ToArray());

                    var maybeChanges = oldcolumns.Where(m => nowids.Contains(m.id)).ToArray();
                    List<EJ.DBColumn> changedColumns = new List<EJ.DBColumn>();
                    for (int i = 0; i < maybeChanges.Length; i++)
                    {
                        var c = maybeChanges[i];
                        var nowcolumn = nowcolumns.FirstOrDefault(m => m.id == c.id);
                        c.ChangedProperties.Clear();
                        c.Name = nowcolumn.Name;
                        c.length = nowcolumn.length;
                        c.dbType = nowcolumn.dbType;
                        c.defaultValue = nowcolumn.defaultValue;
                        c.IsPKID = nowcolumn.IsPKID;
                        c.IsAutoIncrement = nowcolumn.IsAutoIncrement;
                        c.CanNull = nowcolumn.CanNull;
                        if (c.ChangedProperties.Count > 0)
                        {
                            changedColumns.Add(c);

                            otherColumns.Remove(c);
                        }
                    }

                    string conStr = string.Format(database.conStr, HttpContext.Current.Request.MapPath("/"));
                    if (conStr.ToLower() == db.Database.ConnectionString.ToLower())
                    {
                        invokingDB = db.Database;
                    }
                    else
                    {
                        invokingDB = DBHelper.CreateInvokeDatabase(database);
                        invokingDB.DBContext.BeginTransaction();
                    }



                    ChangeTableAction action = new ChangeTableAction(newtable.DatabaseID.Value, oldtable.Name, newtable.Name,
                        addcolumns, changedColumns.ToArray(), delColumns,otherColumns.ToArray(),
                        idxConfigs);
                    action.Invoke(invokingDB);


                    oldtable.Name = newtable.Name;
                    oldtable.caption = newtable.caption;
                    db.Update(oldtable);

                    foreach (var c in delColumns)
                    {
                        db.Delete(c);
                    }
                    foreach (var c in changedColumns)
                    {
                        db.Update(c);
                    }
                    foreach (var c in addcolumns)
                    {
                        c.TableID = oldtable.id;
                        db.Update(c);
                    }
                    for (int i = 0; i < nowcolumns.Length; i++)
                    {
                        nowcolumns[i].orderid = i;
                        db.Update(nowcolumns[i]);
                    }

                    //添加级联删除
                    db.Database.ExecSqlString("delete from DBDeleteConfig where TableID=" + oldtable.id);
                    EJ.DBDeleteConfig[] delConfigs = deleteConfigJson.ToJsonObject<EJ.DBDeleteConfig[]>();
                    foreach (var delconfig in delConfigs)
                    {
                        delconfig.ChangedProperties.Clear();

                        EJ.DBTable relatable = db.DBTable.FirstOrDefault(m => m.DatabaseID == newtable.DatabaseID && m.Name == delconfig.RelaTable_Desc);
                        if (relatable == null)
                            throw new Exception("表" + delconfig.RelaTable_Desc + "不存在，级联删除设置失败");
                        delconfig.RelaTableID = relatable.id;

                        EJ.DBColumn relacolumn = db.DBColumn.FirstOrDefault(m => m.TableID == relatable.id && m.Name == delconfig.RelaColumn_Desc);
                        if (relacolumn == null)
                            throw new Exception("字段" + delconfig.RelaColumn_Desc + "不存在，级联删除设置失败");

                        delconfig.RelaColumID = relacolumn.id;
                        delconfig.TableID = newtable.id;
                        db.Update(delconfig);
                    }

                    object actionid = action.Save(db, database.id.GetValueOrDefault());
                    SetLastUpdateID(actionid, database.Guid, invokingDB);

                    if (invokingDB.DBContext != db)
                    {
                        invokingDB.DBContext.CommitTransaction();
                    }
                    db.CommitTransaction();

                }
                catch (Exception ex)
                {
                    if (invokingDB.DBContext != db)
                    {
                        invokingDB.DBContext.RollbackTransaction();
                    }
                    db.RollbackTransaction();
                    throw ex;
                }
                finally
                {
                    if (invokingDB != null)
                    {
                        invokingDB.DBContext.Dispose();
                    }
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

            var data = new EntityDB.CustomDataItem("__WayEasyJob",null,null);
            data.SetValue("contentConfig", dbconfig.ToJsonString());
            db.Update(data);
        }

        [WebMethod(EnableSession = true)]
        public void DeleteModule(string modulestring)
        {
            if (this.User == null)
                throw new Exception("请重新登陆"); 
            using (EJDB db = new EJDB())
            {
                EJ.DBModule module = modulestring.ToJsonObject<EJ.DBModule>();
                db.Delete(module);
            }
        }
        [WebMethod(EnableSession = true)]
        public string GetDBTablePath(int tableid)
        {
            if (this.User == null)
                throw new Exception("请重新登陆"); 

            using (EJDB_Check db = new EJDB_Check())
            {
                StringBuilder result = new StringBuilder();
                var table = db.DBTable.FirstOrDefault(m => m.id == tableid);
                result.Append(table.Name + " " + table.caption);

                var database = db.Databases.FirstOrDefault(m => m.id == table.DatabaseID);
                result.Insert(0, database.Name + "/");

                var project = db.Project.FirstOrDefault(m => m.id == database.ProjectID);
                result.Insert(0, project.Name + "/Database/");
                return result.ToString();
            }
        }
        [WebMethod(EnableSession = true)]
        public string GetDBModulePath(int moduleid)
        {
            if (this.User == null)
                throw new Exception("请重新登陆"); 

            using (EJDB_Check db = new EJDB_Check())
            {
                StringBuilder result = new StringBuilder();
                var module = db.DBModule.FirstOrDefault(m => m.id == moduleid);
                result.Insert(0, module.Name);
                while (module.parentID != 0)
                {
                    module = db.DBModule.FirstOrDefault(m => m.id == module.parentID);
                    result.Insert(0, module.Name + "/");
                }

                var database = db.Databases.FirstOrDefault(m => m.id == module.DatabaseID);
                result.Insert(0, database.Name + "/");

                var project = db.Project.FirstOrDefault(m=>m.id == database.ProjectID);
                result.Insert(0, project.Name + "/Database/");
                return result.ToString();
            }
        }
        [WebMethod(EnableSession = true)]
        public int UpdateDBModule(string modulestring)
        {
            if (this.User == null)
                throw new Exception("请重新登陆"); 

            using (EJDB db = new EJDB())
            {
                try
                {
                    db.BeginTransaction();

                    EJ.DBModule module = modulestring.ToJsonObject<EJ.DBModule>();
                    if (db.Databases.Count(m => m.id == module.DatabaseID) == 0)
                        throw new Exception("DBModule归属的数据库已被删除");

                    if (module.id == null)
                    {
                        if (db.DBModule.Count(m => m.Name == module.Name && m.DatabaseID == module.DatabaseID && m.parentID == module.parentID) > 0)
                            throw new Exception("名称重复");
                    }
                    else
                    {
                        if (db.DBModule.Count(m => m.id != module.id && m.Name == module.Name && m.DatabaseID == module.DatabaseID && m.parentID == module.parentID) > 0)
                            throw new Exception("名称重复");
                    }

                    db.Update(module);
                   
                    db.CommitTransaction();
                    return module.id.GetValueOrDefault();
                }
                catch(Exception ex)
                {
                    db.RollbackTransaction();
                    throw ex;
                }
            }
        }

        [WebMethod(EnableSession = true)]
        public void DeleteTable(int databaseID,string tableName)
        {
            if (this.User == null)
                throw new Exception("请重新登陆"); 

            EntityDB.IDatabaseService invokingDB = null;
            using (EJDB db = new EJDB())
            {
                try
                {
                    db.BeginTransaction(System.Data.IsolationLevel.ReadCommitted);
                    //锁住，防止同时添加table
                    EJ.Databases database = db.Databases.Where(m => m.id == databaseID).FirstOrDefault();
                    database.iLock++;
                    db.Update(database);

                    string conStr = string.Format(database.conStr, HttpContext.Current.Request.MapPath("/"));
                    if (conStr.ToLower() == db.Database.ConnectionString.ToLower())
                    {
                        invokingDB = db.Database;
                    }
                    else
                    {
                        invokingDB = DBHelper.CreateInvokeDatabase(database);
                        invokingDB.DBContext.BeginTransaction();
                    }


                    var action = new DeleteTableAction(tableName);
                    action.Invoke(invokingDB);

                    EJ.DBTable table = db.DBTable.FirstOrDefault(m => m.DatabaseID == databaseID && m.Name == tableName);
                    db.Delete(table);


                    object actionid = action.Save(db, databaseID);
                    SetLastUpdateID(actionid, database.Guid, invokingDB);

                    if (invokingDB.DBContext != db)
                    {
                        invokingDB.DBContext.CommitTransaction();
                    }
                    db.CommitTransaction();
                }
                catch (Exception ex)
                {
                    if (invokingDB.DBContext != db)
                    {
                        invokingDB.DBContext.RollbackTransaction();
                    }
                    db.RollbackTransaction();
                    throw ex;
                }
                finally
                {
                    if (invokingDB != null)
                        invokingDB.DBContext.Dispose();
                }
            }
        }

       
     
        [WebMethod(EnableSession = true)]
        public string CreateTable(string tablejson,string columnsJson,string deleteConfigJson,string idxJson)
        {
            if (this.User == null)
                throw new Exception("请重新登陆"); 

            using (EJDB db = new EJDB())
            {
                EntityDB.IDatabaseService invokingDB = null;
                EJ.DBTable table = tablejson.ToJsonObject<EJ.DBTable>();
                EJ.DBColumn[] columns = columnsJson.ToJsonObject<EJ.DBColumn[]>();
                IndexInfo[] idxConfigs = idxJson.ToJsonObject<IndexInfo[]>();
                try
                {
                    db.BeginTransaction(System.Data.IsolationLevel.ReadCommitted);
                    //锁住，防止同时添加table
                    EJ.Databases database = db.Databases.Where(m => m.id == table.DatabaseID).FirstOrDefault();
                    database.iLock++;
                    db.Update(database);


                    string conStr = string.Format(database.conStr, HttpContext.Current.Request.MapPath("/"));
                    if (conStr.ToLower() == db.Database.ConnectionString.ToLower())
                    {
                        invokingDB = db.Database;
                    }
                    else
                    {
                        invokingDB = DBHelper.CreateInvokeDatabase(database);
                        invokingDB.DBContext.BeginTransaction();
                    }


                    if (db.DBTable.Where(m => m.DatabaseID == table.DatabaseID && m.Name == table.Name).Any())
                        throw new Exception("数据表名称重复");

                    var action = new CreateTableAction(table, columns, idxConfigs);
                    action.Invoke(invokingDB);

                    db.Update(table);
                    foreach (EJ.DBColumn column in columns)
                    {
                        column.TableID = table.id;
                        db.Update(column);
                    }

                    foreach (var config in idxConfigs)
                    {
                        EJ.IDXIndex idxIndex = new EJ.IDXIndex();
                        idxIndex.TableID = table.id;
                        idxIndex.IsUnique = config.IsUnique;
                        idxIndex.IsClustered = config.IsClustered;
                        idxIndex.Keys = config.ColumnNames.ToSplitString();
                        db.Update(idxIndex);
                    }

                    //添加级联删除
                    EJ.DBDeleteConfig[] delConfigs = deleteConfigJson.ToJsonObject<EJ.DBDeleteConfig[]>();
                    foreach (var delconfig in delConfigs)
                    {
                        delconfig.ChangedProperties.Clear();

                        EJ.DBTable relatable = db.DBTable.FirstOrDefault(m => m.DatabaseID == table.DatabaseID && m.Name == delconfig.RelaTable_Desc);
                        if (relatable == null)
                            throw new Exception("表" + delconfig.RelaTable_Desc + "不存在，级联删除设置失败");
                        delconfig.RelaTableID = relatable.id;

                        EJ.DBColumn relacolumn = db.DBColumn.FirstOrDefault(m => m.TableID == relatable.id && m.Name == delconfig.RelaColumn_Desc);
                        if (relacolumn == null)
                            throw new Exception("字段" + delconfig.RelaColumn_Desc + "不存在，级联删除设置失败");

                        delconfig.RelaColumID = relacolumn.id;
                        delconfig.TableID = table.id;
                        db.Update(delconfig);
                    }

                    object actionid = action.Save(db, database.id.GetValueOrDefault());
                    SetLastUpdateID(actionid, database.Guid, invokingDB);

                    if (invokingDB.DBContext != db)
                    {
                        invokingDB.DBContext.CommitTransaction();
                    }
                    db.CommitTransaction();

                    return table.ToJsonString();
                }
                catch (Exception ex)
                {
                    if (invokingDB.DBContext != db)
                    {
                        invokingDB.DBContext.RollbackTransaction();
                    }
                    db.RollbackTransaction();
                    throw ex;
                }
                finally
                {
                    if (invokingDB != null)
                        invokingDB.DBContext.Dispose();
                }
            }
            return null;
        }
        #region 从config导入
        void importMenus(EJDB db, System.Data.DataSet dset,int databaseid , int parentid , int parentModuleId)
        {
            if (this.User == null)
                throw new Exception("请重新登陆"); 

            System.Data.DataView dv_tablepos = dset.Tables["TablePos"].DefaultView;
            System.Data.DataView dv_tables = dset.Tables["tables"].DefaultView;

            System.Data.DataTable dt_Menus = dset.Tables["Menus"];
            dt_Menus.DefaultView.RowFilter = "parentid=" + parentid;
            System.Data.DataTable dtable = dt_Menus.DefaultView.ToTable();
            foreach (System.Data.DataRow datarow in dtable.Rows)
            {
                string Caption = datarow["Caption"].ToString();
                int MenusId = (int)datarow["MenusId"];

                EJ.DBModule module = new EJ.DBModule();
                module.parentID = parentModuleId;
                module.Name = Caption;
                module.DatabaseID = databaseid;
                dt_Menus.DefaultView.RowFilter = "parentid=" + MenusId;
                module.IsFolder = dt_Menus.DefaultView.Count > 0;
                db.Update(module);

                dv_tablepos.RowFilter = "menusid=" + MenusId;
                string ids = ",";
                foreach (System.Data.DataRowView drow in dv_tablepos)
                {
                    ids += drow["tablesid"] + ",";
                }
                dv_tables.RowFilter = "tablesid in (-1" + ids + "-1)";
                foreach (System.Data.DataRowView drow in dv_tables)
                {
                    string tablename = drow["Name"].ToString();
                    int tablesid = (int)drow["tablesid"];
                    dv_tablepos.RowFilter = "tablesid=" + tablesid +   " and MenusId=" + MenusId;
                    int x = (int)dv_tablepos[0]["x"];
                    int y = (int)dv_tablepos[0]["y"];

                    EJ.TableInModule tInM = new EJ.TableInModule();
                    tInM.ModuleID = module.id;
                    tInM.x = x;
                    tInM.y = y;
                    tInM.TableID = db.DBTable.FirstOrDefault(m => m.DatabaseID == databaseid && m.Name == tablename).id;
                    db.Update(tInM);
                }

                importMenus(db, dset, databaseid, MenusId, module.id.Value);
            }
        }
        [WebMethod(EnableSession = true)]
        public int ImportDatabaseConfig(int projectid, string databaseJson,System.Data.DataSet dset)
        {
            if (this.User == null)
                throw new Exception("请重新登陆"); 

            using (EJDB db = new EJDB())
            {
                db.BeginTransaction(System.Data.IsolationLevel.ReadCommitted);
                try
                {
                  
                    EJ.Databases database = databaseJson.ToJsonObject<EJ.Databases>();
                    if (db.Databases.Any(m => m.Name == database.Name))
                        throw new Exception("数据库名称重复");
                    db.Update(database);

                    //导入table
                    #region 导入table
                    System.Data.DataTable dt_tables = dset.Tables["tables"];
                    foreach (System.Data.DataRow datarow in dt_tables.Rows)
                    {
                        string tablesId = datarow["TablesId"].ToString();
                        string name = datarow["Name"].ToString();
                        string caption = datarow["Caption"].ToString();
                        EJ.DBTable dbtable = new EJ.DBTable();
                        dbtable.Name = name;
                        dbtable.caption = caption;
                        dbtable.DatabaseID = database.id;
                        db.Update(dbtable);

                        System.Data.DataView columnsDV = dset.Tables["Columns"].DefaultView;
                        columnsDV.RowFilter = "TablesId=" + tablesId;
                        int orderid = 0;
                        foreach (System.Data.DataRowView drow in columnsDV)
                        {
                            EJ.DBColumn dbcolumn = new EJ.DBColumn();
                            dbcolumn.TableID = dbtable.id;
                            dbcolumn.Name = drow["Name"].ToString();
                            dbcolumn.caption = drow["caption"].ToString();
                            string comment = drow["Comment"].ToString();
                            if (!string.IsNullOrEmpty(comment))
                            {
                                dbcolumn.caption += "," + comment;
                            }
                            dbcolumn.dbType = drow["DataType"].ToString();
                            string _LengthForDecimal = drow["LengthForDecimal"].ToString().Replace("(", "").Replace(")", "");
                            string _Length = drow["Length"].ToString();
                            if (!string.IsNullOrEmpty(_LengthForDecimal))
                            {
                                _Length = _LengthForDecimal;
                            }
                            if (_Length != "0")
                                dbcolumn.length = _Length;
                            else
                            {
                                if (dbcolumn.dbType == "varchar")
                                    dbcolumn.length = "50";
                            }
                            dbcolumn.defaultValue = drow["DefaultValue"].ToString();
                            dbcolumn.EnumDefine = drow["enum"].ToString();
                            dbcolumn.CanNull = (bool)drow["CanNull"];
                            dbcolumn.IsPKID = (bool)drow["ISPKID"];
                            dbcolumn.IsAutoIncrement = (bool)drow["AutoIncrement"];
                            dbcolumn.orderid = orderid++;
                            db.Update(dbcolumn);
                        }
                    } 
                    #endregion

                    importMenus(db, dset, database.id.GetValueOrDefault() ,-1 , 0);

                    dset.Dispose();

                    IDatabaseDesignService dbservice = DBHelper.CreateDatabaseDesignService(database.dbType);
                    dbservice.Create(database);

                    db.CommitTransaction();
                    return database.id.Value;
                }
                catch (Exception ex)
                {
                    db.RollbackTransaction();
                    throw ex;
                }
            }
        }
        /// <summary>
        /// 创建数据库
        /// </summary>
        /// <param name="databaseType">数据库类型，如SqlServer</param>
        /// <param name="connectionString">连接字符串</param>
        /// <param name="dllpath"></param>
        /// <param name="dbname"></param>
        [WebMethod(EnableSession = true)]
        public void CreateDatabase(int projectid, EntityDB.DatabaseType databaseType, string connectionString, string dllpath, string dbname)
        {
            if (this.User == null)
                throw new Exception("请重新登陆"); 

            using (EJDB db = new EJDB())
            {
                db.BeginTransaction(System.Data.IsolationLevel.ReadCommitted);
                try
                {
                    EJ.Databases dataitem = new EJ.Databases();
                    dataitem.conStr = connectionString;
                    dataitem.dbType = databaseType;
                    dataitem.dllPath = dllpath;
                    dataitem.Name = dbname;
                    dataitem.ProjectID = projectid;
                    dataitem.Guid = Guid.NewGuid().ToString();
                    db.Update(dataitem);

                    IDatabaseDesignService dbservice = DBHelper.CreateDatabaseDesignService(dataitem.dbType);
                    dbservice.Create(dataitem);

                    db.CommitTransaction();
                }
                catch (Exception ex)
                {
                    db.RollbackTransaction();
                    throw ex;
                }
            }

        }
        #endregion


        [WebMethod(EnableSession = true)]
        public string[] GetProjectDllFiles(int projectid)
        {
            if (this.User == null)
                throw new Exception("请重新登陆"); 

            using (EJDB_Check db = new EJDB_Check())
            {
                var datas = db.DLLImport.Where(m => m.ProjectID == projectid).ToList();
                string[] files = new string[datas.Count];
                for (int i = 0; i < files.Length; i++)
                {
                    files[i] = datas[i].path;
                }
                return files;
            }
        }
        [WebMethod(EnableSession = true)]
        public string GetInterfaceModuleList(int projectid, int parentid)
        {
            if (this.User == null)
                throw new Exception("请重新登陆"); 

            using (EJDB_Check db = new EJDB_Check())
            {
                return db.InterfaceModule.Where(m => m.ProjectID == projectid && m.ParentID == parentid).OrderBy(m => m.Name).ToList().ToJsonString();
            }
        }
        [WebMethod(EnableSession = true)]
        public void DeleteInterfaceModule(string modulestring)
        {
            if (this.User == null)
                throw new Exception("请重新登陆"); 

            using (EJDB db = new EJDB())
            {
                EJ.InterfaceModule module = modulestring.ToJsonObject<EJ.InterfaceModule>();
                db.Delete(module);
            }
        }
        [WebMethod(EnableSession = true)]
        public void UnLockInterfaceModule(int moduleid)
        {
            if (this.User == null)
                throw new Exception("请重新登陆"); 

            using (EJDB_Check db = new EJDB_Check())
            {
                var module = db.InterfaceModule.FirstOrDefault(m => m.id == moduleid);
                if (module.LockUserId != this.User.id)
                    throw new Exception("不能解锁他人锁定的模块");
                module.LockUserId = null;
                db.Update(module);
            }
        }
        [WebMethod(EnableSession = true)]
        public int LockInterfaceModule(int moduleid)
        {
            if (this.User == null)
                throw new Exception("请重新登陆"); 

            int userid = this.User.id.Value;
            using (EJDB_Check db = new EJDB_Check())
            {
                db.BeginTransaction();
                try
                {
                    var module = db.InterfaceModule.FirstOrDefault(m=>m.id == moduleid);
                    if (module.LockUserId == null)
                    {
                        module.LockUserId = userid;
                    }
                    else if (module.LockUserId != null && module.LockUserId != userid)
                        throw new Exception("已被其他用户锁定");

                    db.Update(module);
                    db.CommitTransaction();
                    return userid;
                }
                catch (Exception ex)
                {
                    db.RollbackTransaction();
                    throw ex;
                }
            }
        }
        [WebMethod(EnableSession = true)]
        public int UpdateInterfaceModule(string modulestring)
        {
            if (this.User == null)
                throw new Exception("请重新登陆"); 

            using (EJDB db = new EJDB())
            {
                try
                {
                    db.BeginTransaction();

                    EJ.InterfaceModule module = modulestring.ToJsonObject<EJ.InterfaceModule>();
                    if (db.Project.Count(m => m.id == module.ProjectID) == 0)
                        throw new Exception("归属的工程已被删除");

                    if (module.id == null)
                    {
                        if (db.InterfaceModule.Count(m => m.Name == module.Name && m.ProjectID == module.ProjectID && m.ParentID == module.ParentID) > 0)
                            throw new Exception("名称重复");
                    }
                    else
                    {
                        if (db.InterfaceModule.Count(m => m.id != module.id && m.Name == module.Name && m.ProjectID == module.ProjectID && m.ParentID == module.ParentID) > 0)
                            throw new Exception("名称重复");
                    }

                    db.Update(module);

                    db.CommitTransaction();
                    return module.id.GetValueOrDefault();
                }
                catch (Exception ex)
                {
                    db.RollbackTransaction();
                    throw ex;
                }
            }
        }

        [WebMethod(EnableSession = true)]
        public string GetInterfaceModulePath(int moduleid)
        {
            if (this.User == null)
                throw new Exception("请重新登陆"); 

            using (EJDB_Check db = new EJDB_Check())
            {
                StringBuilder result = new StringBuilder();
                var module = db.InterfaceModule.FirstOrDefault(m => m.id == moduleid);
                result.Insert(0, module.Name);
                while (module.ParentID != 0)
                {
                    module = db.InterfaceModule.FirstOrDefault(m => m.id == module.ParentID);
                    result.Insert(0, module.Name + "/");
                }

                var project = db.Project.FirstOrDefault(m => m.id == module.ProjectID);
                result.Insert(0, project.Name + "/Interface/");
                return result.ToString();
            }
        }
        [WebMethod(EnableSession = true)]
        public int UpdateInterfaceInModule(string mJonsString)
        {
            if (this.User == null)
                throw new Exception("请重新登陆"); 

            using (EJDB db = new EJDB())
            {
                
                EJ.InterfaceInModule module = mJonsString.ToJsonObject<EJ.InterfaceInModule>();
                var docModule = db.InterfaceModule.FirstOrDefault(m=>m.id == module.ModuleID);
                if (docModule.LockUserId != null && docModule.LockUserId != this.User.id)
                {
                    throw new Exception("无法修改锁定的模块");
                }
                db.Update(module);
                return module.id.Value;
            }
        }
        [WebMethod(EnableSession = true)]
        public void DeleteInterfaceInModule(string mJonsString)
        {
            if (this.User == null)
                throw new Exception("请重新登陆"); 

            using (EJDB db = new EJDB())
            {
                EJ.InterfaceInModule module = mJonsString.ToJsonObject<EJ.InterfaceInModule>();
                var docModule = db.InterfaceModule.FirstOrDefault(m => m.id == module.ModuleID);
                if (docModule.LockUserId != null && docModule.LockUserId != this.User.id)
                {
                    throw new Exception("无法修改锁定的模块");
                }

                db.Delete(module);
            }
        }
        [WebMethod(EnableSession = true)]
        public string GetInterfaceInModule(int moduleid)
        {
            if (this.User == null)
                throw new Exception("请重新登陆"); 

            using (EJDB_Check db = new EJDB_Check())
            {
                var modules = db.InterfaceInModule.Where(m => m.ModuleID == moduleid).ToList();
                return modules.ToJsonString();
            }
        }
        [WebMethod(EnableSession = true)]
        public int GetInterfaceModuleID(int interfaceInModuleId)
        {
            if (this.User == null)
                throw new Exception("请重新登陆"); 

            using (EJDB_Check db = new EJDB_Check())
            {
                var item = db.InterfaceInModule.FirstOrDefault(m => m.id == interfaceInModuleId);
                if (item != null)
                    return item.ModuleID.GetValueOrDefault();
                return 0;
            }
        }
        [WebMethod(EnableSession = true)]
        public string GetInterfaceInModulePath(int itemid)
        {
            if (this.User == null)
                throw new Exception("请重新登陆"); 

            using (EJDB_Check db = new EJDB_Check())
            {
                StringBuilder result = new StringBuilder();
                var item = db.InterfaceInModule.FirstOrDefault(m => m.id == itemid);
                if (item == null)
                    return null;
                var module = db.InterfaceModule.FirstOrDefault(m => m.id == item.ModuleID);
                result.Insert(0, module.Name);
                while (module.ParentID != 0)
                {
                    module = db.InterfaceModule.FirstOrDefault(m => m.id == module.ParentID);
                    result.Insert(0, module.Name + "/");
                }

                var project = db.Project.FirstOrDefault(m => m.id == module.ProjectID);
                result.Insert(0, project.Name + "/Interface/");

                if (item.Type.Contains("ClassView"))
                {
                    var json = item.JsonData.ToJsonObject<JsonObject_ClassView>();
                    if (json != null)
                    {
                        result.Append('/');
                        result.Append(json.FullName);
                    }
                }
                else if (item.Type.Contains("DescriptionView"))
                {
                    var json = item.JsonData.ToJsonObject<JsonObject_DescriptionView>();
                    if (json != null)
                    {
                        result.Append('/');
                        result.Append(json.Content);
                    }
                }
                return result.ToString();
            }
        }

        [WebMethod(EnableSession = true)]
        public string Search(string key,int pagesize,int pageindex)
        {
            if (this.User == null)
                throw new Exception("请重新登陆"); 

            using (EJDB_Check db = new EJDB_Check())
            {
                return db.SearchContents.Where(m => m.Content.Contains(key)).OrderBy(m=>m.Type).Skip(pagesize*pageindex).Take(pagesize).ToList().ToJsonString();
            }
        }

        [WebMethod(EnableSession = true)]
        public void SubmitBug(string title, byte[] textContent,byte[] picContent)
        {
            if (this.User == null)
                throw new Exception("请重新登陆");

            using (EJDB db = new EJDB())
            {
                db.BeginTransaction();
                try
                {
                    EJ.Bug bug = new EJ.Bug()
                        {
                            SubmitTime = DateTime.Now,
                            Title = title,
                            SubmitUserID = this.User.id,
                            Status = EJ.Bug_StatusEnum.提交给开发人员,
                        };
                    db.Insert(bug);

                    EJ.BugImages bugimg = new EJ.BugImages()
                        {
                            BugID = bug.id,
                            content = picContent,
                            orderID = 0,
                        };
                    db.Insert(bugimg);

                    EJ.BugHandleHistory history = new EJ.BugHandleHistory()
                    {
                        BugID = bug.id,
                        content = textContent,
                        SendTime = DateTime.Now,
                        UserID = this.User.id,
                    };
                    db.Insert(history);

                    db.CommitTransaction();
                }
                catch (Exception ex)
                {
                    db.RollbackTransaction();
                    throw ex;
                }
            }
        }

        [WebMethod(EnableSession = true)]
        public int GetMyBugListCount( )
        {
            if (this.User == null)
                throw new Exception("请重新登陆");
            using (EJDB_Check db = new EJDB_Check())
            {
                if(this.User.Role == EJ.User_RoleEnum.客户端测试人员)
                    return db.MyBugList.Where(m => m.SubmitUserID == this.User.id && m.Status == EJ.Bug_StatusEnum.反馈给提交者).Count();
                return db.MyBugList.Where(m=>m.Status == EJ.Bug_StatusEnum.提交给开发人员).Count();
            }
        }

        [WebMethod(EnableSession = true)]
        public string GetMyBugs( )
        {
            if (this.User == null)
                throw new Exception("请重新登陆");
            using (EJDB_Check db = new EJDB_Check())
            {
                var query = from m in db.MyBugList
                            join u in db.User on m.SubmitUserID equals u.id
                            orderby m.Status,m.SubmitTime
                            where m.Status != EJ.Bug_StatusEnum.处理完毕
                            select new BugItem
                            {
                                FinishTime = m.FinishTime,
                                HandlerID = m.HandlerID,
                                id = m.id,
                                LastDate = m.LastDate,
                                SubmitUserName = u.Name,
                                Status = m.Status,
                                SubmitTime = m.SubmitTime,
                                SubmitUserID = m.SubmitUserID,
                                HandlerUserName = db.User.Where(p=>p.id == m.HandlerID).Select(p=>p.Name).FirstOrDefault(),
                            };
                return query.Take(50).ToArray().ToJsonString();
            }

        }

        [WebMethod(EnableSession = true)]
        public string GetBugHistories(int bugid)
        {
            if (this.User == null)
                throw new Exception("请重新登陆");
            using (EJDB_Check db = new EJDB_Check())
            {
                
                var query = from m in db.BugHandleHistory
                            join u in db.User on m.UserID equals u.id
                            where m.BugID == bugid
                            orderby m.SendTime
                            select new BugHistoryItem
                            {
                                Content = m.content,
                                SubmitTime = m.SendTime,
                                UserName = u.Name,
                            };
                return query.ToArray().ToJsonString();
            }
        }

        [WebMethod(EnableSession = true)]
        public byte[] GetBugPicture(int bugid)
        {
            if (this.User == null)
                throw new Exception("请重新登陆");
            using (EJDB_Check db = new EJDB_Check())
            {
                var pic = db.BugImages.FirstOrDefault(m=>m.BugID == bugid);
                if (pic != null)
                    return pic.content;
            }
            return null;
        }
        [WebMethod(EnableSession = true)]
        public void BugFinish(int bugid)
        {
            if (this.User == null)
                throw new Exception("请重新登陆");
            using (EJDB_Check db = new EJDB_Check())
            {
                var bug = db.Bug.FirstOrDefault(m => m.id == bugid);
                bug.Status = EJ.Bug_StatusEnum.处理完毕;
                db.Update(bug);
            }
        }
        [WebMethod(EnableSession = true)]
        public void SubmitHistory(int bugid, byte[] txtContent)
        {
            if (this.User == null)
                throw new Exception("请重新登陆");
            using (EJDB_Check db = new EJDB_Check())
            {
                if (txtContent != null)
                {
                    if (txtContent != null && txtContent.Length > 0)
                    {
                        EJ.BugHandleHistory his = new EJ.BugHandleHistory()
                            {
                                BugID = bugid,
                                content = txtContent,
                                SendTime = DateTime.Now,
                                UserID = this.User.id,
                            };
                        db.Insert(his);
                    }
                }
                var bug = db.Bug.FirstOrDefault(m => m.id == bugid);
                bug.LastDate = DateTime.Now;
                if (bug.SubmitUserID == this.User.id )
                {
                    bug.Status = EJ.Bug_StatusEnum.提交给开发人员;
                }
                else
                {
                    bug.Status = EJ.Bug_StatusEnum.反馈给提交者;
                }
                db.Update(bug);
            }
        }

        [WebMethod(EnableSession = true)]
        public string GetUpdateFileList( )
        {
            string folder = HttpContext.Current.Request.MapPath("/updates");
            if (System.IO.Directory.Exists(folder) == false)
                return "[]";
            List<FileInfo> fileinfos = new List<FileInfo>();
            var files = System.IO.Directory.GetFiles(folder);
            foreach (string filepath in files)
            {
                string ext = System.IO.Path.GetExtension(filepath).ToLower();
                if (filepath.Contains(".vshost."))
                    continue;
                if (ext == ".dll" || ext == ".exe")
                {
                    fileinfos.Add(new FileInfo()
                    {
                        SavePath = System.IO.Path.GetFileName(filepath),
                            FileName = System.IO.Path.GetFileName(filepath),
                            LastWriteTime = new System.IO.FileInfo(filepath).LastWriteTime.ToFileTime(),
                        });
                }
            }
            return fileinfos.ToArray().ToJsonString();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="savepath">可能是: x86\lib\test.dll形式</param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        public byte[] DownLoadFile(string savepath)
        {
            string folder = HttpContext.Current.Request.MapPath("/updates");
            return System.IO.File.ReadAllBytes(folder + "\\" + savepath);
        }
    }
    class FileInfo
    {
        public string SavePath;
        public string FileName;
        public long LastWriteTime;
    }
    class JsonObject_ClassView
    {
        public string FullName;
    }
    class JsonObject_DescriptionView
    {
        public string Content;
    }
}
