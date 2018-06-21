using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Way.EntityDB;
using Way.EntityDB.Design;
using Way.EntityDB.Design.Actions;
using Way.EntityDB.Design.Services;
using Way.Lib;
using Way.Lib.ScriptRemoting;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations.Operations;

namespace Way.EJServer
{
    /// <summary>
    /// 目前sqlite有太多不支持的operation，所以，暂时不用migration
    /// </summary>
    public class MainController_EF : RemotingController
    {
        public EJ.User User
        {
            get
            {
                return Session["user"] as EJ.User;
            }
            set
            {
                Session["user"] = value;
            }
        }
        protected override void OnBeforeInvokeMethod(MethodInfo method)
        {
            if (method.Name != "Login")
            {
                if (this.User == null)
                    throw new Exception("请先登录");
            }
            base.OnBeforeInvokeMethod(method);
        }

        [RemotingMethod]
        public int[] Login(string name, string pwd)
        {
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
                var user = db.User.FirstOrDefault(m => m.Name == name);
                if (user == null || user.Password != pwd)
                    throw new Exception("用户名密码错误");
                Session["user"] = user;

                return new int[] { (int)user.Role, user.id.Value };
            }
        }
        [RemotingMethod]
        public EJ.User[] GetUsers()
        {
            if (this.User.Role != EJ.User_RoleEnum.管理员)
                throw new Exception("无权进行此项操作");
            using (EJDB db = new EJDB())
            {
                return (from m in db.User
                        select new EJ.User
                        {
                            Name = m.Name,
                            id = m.id,
                            Role = m.Role
                        }).ToArray();

            }
        }
        [RemotingMethod]
        public int UpdateUser(EJ.User user)
        {
            if (this.User.Role != EJ.User_RoleEnum.管理员)
                throw new Exception("无权进行此项操作");
            using (EJDB db = new EJDB())
            {
                if (user.id == null)
                {
                    if (db.User.Any(m => m.Name == user.Name))
                        throw new Exception("用户名已存在");
                }
                else
                {
                    if (db.User.Any(m => m.Name == user.Name && m.id != user.id))
                        throw new Exception("用户名已存在");
                }
                db.Update(user);
                return user.id.Value;
            }
        }
        [RemotingMethod]
        public void DeleteUsers(int[] userids)
        {
            if (this.User.Role != EJ.User_RoleEnum.管理员)
                throw new Exception("无权进行此项操作");
            using (EJDB db = new EJDB())
            {
                db.BeginTransaction();
                try
                {
                    foreach (var id in userids)
                    {
                        db.Delete(db.User.Where(m => userids.Contains(m.id.Value)));
                    }
                    db.CommitTransaction();
                }
                catch (Exception ex)
                {
                    db.RollbackTransaction();
                    throw ex;
                }
            }
        }
        [RemotingMethod]
        public EJ.Project CreateProject(string name)
        {
            if (this.User.Role != EJ.User_RoleEnum.管理员)
                throw new Exception("无权进行此项操作");
            using (EJDB db = new EJDB())
            {
                EJ.Project project = new EJ.Project()
                {
                    Name = name
                };
                db.Update(project);
                return project;
            }
        }
        [RemotingMethod]
        public void DeleteProject(int id)
        {
            if (this.User.Role != EJ.User_RoleEnum.管理员)
                throw new Exception("无权进行此项操作");
            using (EJDB db = new EJDB())
            {
                db.Delete(db.Project.Where(m => m.id == id));
            }
        }
        [RemotingMethod]
        public void UpdateProject(int id, string name)
        {
            if (this.User.Role != EJ.User_RoleEnum.管理员)
                throw new Exception("无权进行此项操作");
            using (EJDB db = new EJDB())
            {
                var project = db.Project.FirstOrDefault(m => m.id == id);
                project.Name = name;
                db.Update(project);
            }
        }
        [RemotingMethod]
        public string GetUserNameByID(int id)
        {
            using (EJDB db = new EJDB())
            {
                return db.User.FirstOrDefault(m => m.id == id).Name;
            }
        }
        [RemotingMethod]
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

        [RemotingMethod]
        public string GetProjectNameByColumnId(int columnid)
        {
            using (EJDB_Check db = new EJDB_Check())
            {
                var column = db.DBColumn.FirstOrDefault(m => m.id == columnid);
                var table = db.DBTable.FirstOrDefault(m => m.id == column.TableID);
                var database = db.Databases.FirstOrDefault(m => m.id == table.DatabaseID);
                return database.Name;
            }
        }

        [RemotingMethod]
        public string[] GetNamespacePathByColumnId(int columnId)
        {
            using (EJDB_Check db = new EJDB_Check())
            {
                var column = db.DBColumn.FirstOrDefault(m => m.id == columnId);
                var table = db.DBTable.FirstOrDefault(m => m.id == column.TableID);
                var database = db.Databases.FirstOrDefault(m => m.id == table.DatabaseID);
                return new string[] { database.NameSpace + ".DB." + database.Name, table.Name };
            }
        }

        [RemotingMethod]
        public int ChangePassword(string oldpwd, string newpwd)
        {
            if (this.User.Password != oldpwd)
                throw new Exception("旧密码错误");

            using (EJDB db = new EJDB())
            {
                this.User.Password = newpwd;
                db.Update(this.User);
            }
            return 0;
        }
        /// <summary>
        /// 获取当前用户有权查看的项目
        /// </summary>
        /// <returns></returns>
        [RemotingMethod]
        public EJ.Project[] GetCurrentUserProjectList()
        {
            using (EJDB_Check db = new EJDB_Check())
            {
                return db.Project.ToArray();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [RemotingMethod]
        public EJ.Project[] GetCurrentUserProjectToSetPowerList(int settingUserId)
        {
            using (EJDB_Check db = new EJDB_Check())
            {
                var result = db.Project.ToArray();
                foreach (var project in result)
                {
                    if (db.ProjectPower.Any(m => m.UserID == settingUserId && m.ProjectID == project.id))
                    {
                        project.BackupChangedProperties.Add("HasPower", new DataValueChangedItem());
                    }
                }
                return result;
            }
        }
        [RemotingMethod]
        public EJ.Databases[] GetCurrentUserDatabaseToSetPowerList(int settingUserId, int projectid)
        {
            using (EJDB_Check db = new EJDB_Check())
            {
                var result = db.Databases.Where(m => m.ProjectID == projectid).ToArray();
                foreach (var dbitem in result)
                {
                    if (db.DBPower.Any(m => m.UserID == settingUserId && m.DatabaseID == dbitem.id))
                    {
                        dbitem.BackupChangedProperties.Add("HasPower", new DataValueChangedItem());
                    }
                }
                return result;
            }
        }
        [RemotingMethod]
        public EJ.DBTable[] GetCurrentUserDBTableToSetPowerList(int settingUserId, int databaseid)
        {
            using (EJDB_Check db = new EJDB_Check())
            {
                var result = db.DBTable.Where(m => m.DatabaseID == databaseid).Select(m => new EJ.DBTable
                {
                    Name = m.Name,
                    id = m.id
                }).ToArray();

                foreach (var dbtable in result)
                {
                    if (db.TablePower.Any(m => m.UserID == settingUserId && m.TableID == dbtable.id))
                    {
                        dbtable.BackupChangedProperties.Add("HasPower", new DataValueChangedItem());
                    }
                }
                return result;
            }
        }
        [RemotingMethod]
        public EJ.InterfaceModule[] GetCurrentUseInterfaceToSetPowerList(int settingUserId, int projectid, int parentid)
        {
            using (EJDB_Check db = new EJDB_Check())
            {
                var result = db.InterfaceModule.Where(m => m.ProjectID == projectid && m.ParentID == parentid).ToArray();

                foreach (var interfaceItem in result)
                {
                    if (interfaceItem.IsFolder == false)
                    {
                        if (db.InterfaceModulePower.Any(m => m.UserID == settingUserId && m.ModuleID == interfaceItem.id))
                        {
                            interfaceItem.BackupChangedProperties.Add("HasPower", new DataValueChangedItem());
                        }
                    }
                }
                return result;
            }
        }
        [RemotingMethod]
        public int SetProjectPower(int projectid, int userid, bool hasPower)
        {
            using (EJDB_Check db = new EJDB_Check())
            {
                var project = db.Project.Single(m => m.id == projectid);
                if (!hasPower)
                    db.Delete(db.ProjectPower.Where(m => m.UserID == userid && m.ProjectID == projectid));
                else
                {
                    db.Insert(new EJ.ProjectPower()
                    {
                        ProjectID = projectid,
                        UserID = userid
                    });
                }
            }
            return 0;
        }

        [RemotingMethod]
        public int SetDatabasePower(int databaseid, int userid, bool hasPower)
        {
            using (EJDB_Check db = new EJDB_Check())
            {
                var database = db.Databases.Single(m => m.id == databaseid);
                if (!hasPower)
                    db.Delete(db.DBPower.Where(m => m.UserID == userid && m.DatabaseID == databaseid));
                else
                {
                    db.Insert(new EJ.DBPower()
                    {
                        DatabaseID = databaseid,
                        UserID = userid
                    });
                }
            }
            return 0;
        }

        [RemotingMethod]
        public int SetTablePower(int tableid, int userid, bool hasPower)
        {
            using (EJDB_Check db = new EJDB_Check())
            {
                var table = db.DBTable.Single(m => m.id == tableid);
                if (!hasPower)
                    db.Delete(db.TablePower.Where(m => m.UserID == userid && m.TableID == tableid));
                else
                {
                    db.Insert(new EJ.TablePower()
                    {
                        TableID = tableid,
                        UserID = userid
                    });
                }
            }
            return 0;
        }

        [RemotingMethod]
        public int SetInterfaceModulePower(int moduleid, int userid, bool hasPower)
        {
            using (EJDB_Check db = new EJDB_Check())
            {
                var interfaceModule = db.InterfaceModule.Single(m => m.id == moduleid);
                if (!hasPower)
                    db.Delete(db.InterfaceModulePower.Where(m => m.UserID == userid && m.ModuleID == moduleid));
                else
                {
                    db.Insert(new EJ.InterfaceModulePower()
                    {
                        ModuleID = moduleid,
                        UserID = userid
                    });
                }
            }
            return 0;
        }
        [RemotingMethod]
        public EJ.TableInModule[] GetTablesInModule(int moduleid)
        {
            using (EJDB_Check db = new EJDB_Check())
            {
                var result = (from m in db.TableInModule
                              where m.ModuleID == moduleid
                              select m).ToArray();
                foreach (var tinm in result)
                {
                    tinm.flag = db.DBTable.FirstOrDefault(m => m.id == tinm.TableID).ToJsonString();
                    tinm.flag2 = GetColumns(tinm.TableID.Value).ToJsonString();
                }

                return result;
            }
        }
        [RemotingMethod]
        public string GetPkColumnName(int tableid)
        {
            using (EJDB db = new EJDB())
            {
                var table = db.DBTable.FirstOrDefault(m => m.id == tableid);
                return db.DBColumn.FirstOrDefault(m => m.TableID == table.id && m.IsPKID == true).Name;
            }
        }
        [RemotingMethod]
        public void SaveDataTable(WayDataTable dt, int tableid, List<string> delIds)
        {
            try
            {
                using (EJDB db = new EJDB())
                {
                    var table = db.DBTable.FirstOrDefault(m => m.id == tableid);
                    var database = db.Databases.FirstOrDefault(m => m.id == table.DatabaseID);
                    var invokingDB = DBHelper.CreateInvokeDatabase(database);

                    IDatabaseDesignService service = DBHelper.CreateDatabaseDesignService(invokingDB.DBContext.DatabaseType);
                    var columns = db.DBColumn.Where(m => m.TableID == table.id).ToList();
                    var pkcolumn = columns.FirstOrDefault(m => m.IsPKID == true && m.TableID == table.id);
                    if (pkcolumn == null)
                        throw new Exception(string.Format("表{0}缺少主键", table.Name));
                    foreach (string id in delIds)
                    {
                        invokingDB.ExecSqlString("delete from " + string.Format(service.GetObjectFormat(), table.Name.ToLower()) + " where " + string.Format(service.GetObjectFormat(), pkcolumn.Name) + "='" + id + "'");
                    }
                    foreach (var drow in dt.Rows)
                    {
                        if (drow.RowState == DataRowState.Added)
                        {
                            var dataitem = new Way.EntityDB.CustomDataItem(table.Name.ToLower(), null, null);
                            foreach (var column in columns)
                            {
                                if (column.IsAutoIncrement == false && drow[column.Name] != null)
                                    dataitem.SetValue(column.Name.ToLower(), drow[column.Name]);
                            }
                            invokingDB.Insert(dataitem);
                        }
                        else if (drow.RowState == DataRowState.Modified)
                        {
                            var dataitem = new Way.EntityDB.CustomDataItem(table.Name.ToLower(), pkcolumn.Name.ToLower(), drow[pkcolumn.Name]);
                            foreach (var column in columns)
                            {
                                if (column.IsAutoIncrement == false && column.IsPKID == false)
                                    dataitem.SetValue(column.Name.ToLower(), drow[column.Name]);
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

        }
        [RemotingMethod]
        public void DeleteDatabase(int databaseid)
        {
            using (EJDB db = new EJDB())
            {
                db.Delete(db.Databases.Where(m => m.id == databaseid));
            }
        }
        [RemotingMethod]
        public WayDataTable GetActions(int databaseid)
        {
            using (EJDB db = new EJDB())
            {
                var database = db.Databases.FirstOrDefault(m => m.id == databaseid);
                var dt = db.Database.SelectTable("select * from __action where databaseid=" + databaseid + " order by [id]");
                dt.TableName = database.dbType.ToString();
                return dt;
            }
        }

        /// <summary>
        /// 把数据库设为新创建的状态，所有表都是createTable
        /// </summary>
        /// <param name="databaseid"></param>
        void setDataBaseIsNEW(int databaseid)
        {
            Way.EntityDB.IDatabaseService invokingDB = null;
            using (EJDB db = new EJDB())
            {
                db.BeginTransaction();
                try
                {
                    object lastid = null;
                    var database = db.Databases.FirstOrDefault(m => m.id == databaseid);
                    db.Database.ExecSqlString("delete from __action where databaseid=" + databaseid);
                    var tables = db.DBTable.Where(m => m.DatabaseID == databaseid).ToArray();
                    foreach (var dbtable in tables)
                    {
                        var columns = db.DBColumn.Where(m => m.TableID == dbtable.id).ToArray();
                        var idxInfos = db.IDXIndex.Where(m => m.TableID == dbtable.id).ToArray();

                        var action = new EF_CreateTable_Action(dbtable, columns, idxInfos);
                        lastid = action.Save(db, database.id.GetValueOrDefault());
                    }


                    string conStr = string.Format(database.conStr, $"{WebRoot}/");
                    if (conStr.ToLower() == db.Database.ConnectionString.ToLower())
                    {
                        invokingDB = db.Database;
                    }
                    else
                    {
                        invokingDB = DBHelper.CreateInvokeDatabase(database);
                    }

                    if (lastid != null)
                        SetLastUpdateID(lastid, database.Guid, invokingDB);
                    db.CommitTransaction();
                }
                catch (Exception ex)
                {
                    db.RollbackTransaction();
                    throw ex;
                }
                finally
                {
                    if (invokingDB != null && invokingDB != db.Database)
                    {
                        invokingDB.DBContext.Dispose();
                    }
                }
            }
        }

        [RemotingMethod]
        public void RemoveTableFromModule(int tableInModuleId, int tableid)
        {
            using (EJDB db = new EJDB())
            {
                var item = db.TableInModule.FirstOrDefault(m => m.id == tableInModuleId);
                if (item != null)
                    db.Delete(item);
            }
        }


        static System.Reflection.MethodInfo SqlQueryMethod;
        [RemotingMethod]
        public WayDataTable GetDataTable(string sql, int tableid, int pageindex, int pagesize)
        {
            using (EJDB db = new EJDB())
            {
                var dbtable = db.DBTable.FirstOrDefault(m => m.id == tableid);
                var database = db.Databases.FirstOrDefault(m => m.id == dbtable.DatabaseID);
                Way.EntityDB.IDatabaseService invokingDB = DBHelper.CreateInvokeDatabase(database);

                if (sql.StartsWith("select "))
                {

                    var dt = invokingDB.SelectTable(sql, pageindex * pagesize, pagesize);
                    dt.TableName = invokingDB.ExecSqlString(string.Format("select count(*) from ({0}) as t1", sql)).ToString();
                    return dt;
                }
                else
                {
                    var dt = invokingDB.SelectTable(sql);
                    dt.TableName = dt.Rows.Count.ToString();

                    return dt;
                }
            }
        }

        [RemotingMethod]
        public EJ.Databases GetDatabase(int databaseid)
        {
            using (EJDB db = new EJDB())
            {
                return db.Databases.FirstOrDefault(m => m.id == databaseid);
            }
        }
        [RemotingMethod]
        public EJ.Databases[] GetDatabaseList(int projectid)
        {
            using (EJDB_Check db = new EJDB_Check())
            {
                return db.Databases.Where(m => m.ProjectID == projectid).OrderBy(m => m.Name).ToArray();
            }
        }
        [RemotingMethod]
        public int GetDBModuleID(int tableid)
        {
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
        [RemotingMethod]
        public int UpdateTableInMoudle(EJ.TableInModule tableInModule)
        {
            using (EJDB db = new EJDB())
            {
                try
                {
                    db.BeginTransaction();

                    if (tableInModule.id == null && this.User.Role.GetValueOrDefault().HasFlag(EJ.User_RoleEnum.数据库设计师) == false)
                        throw new Exception("你没有权限进行此操作");

                    if (tableInModule.id != null && this.User.Role.GetValueOrDefault().HasFlag(EJ.User_RoleEnum.数据库设计师) == false)
                    {
                        //更新位置的话直接忽略
                        return 0;
                    }

                    if (db.DBTable.Count(m => m.id == tableInModule.TableID) == 0)
                        throw new Exception("该数据表已被删除");
                    if (db.DBModule.Count(m => m.id == tableInModule.ModuleID) == 0)
                        throw new Exception("该数据模块已被删除");

                    if (tableInModule.id == null)
                    {
                        if (db.TableInModule.Count(m => m.TableID == tableInModule.TableID && m.ModuleID == tableInModule.ModuleID) > 0)
                            throw new Exception("该数据表在此模块已经存在");
                    }

                    db.Update(tableInModule);

                    db.CommitTransaction();
                    return tableInModule.id.GetValueOrDefault();
                }
                catch (Exception ex)
                {
                    db.RollbackTransaction();
                    throw ex;
                }
            }
        }

        [RemotingMethod]
        public EJ.DBDeleteConfig[] GetDeleteConfigInModule(int moduleid)
        {
            using (EJDB db = new EJDB())
            {
                var tableids = from m in db.TableInModule
                               where m.ModuleID == moduleid
                               select m.TableID;
                var result = from m in db.DBDeleteConfig
                             where tableids.Contains(m.TableID) && tableids.Contains(m.RelaTableID)
                             select m;
                return result.ToArray();
            }
        }

        [RemotingMethod]
        public string[] GetTableNames(int databaseid)
        {
            using (EJDB db = new EJDB())
            {
                var names = from m in db.DBTable
                            where m.DatabaseID == databaseid
                            select m.Name;
                return names.ToArray();
            }
        }
        [RemotingMethod]
        public string[] GetColumnNames(int databaseid, string tableName)
        {

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
        [RemotingMethod]
        public EJ.DBColumn[] GetColumnNameAndIds(int tableid)
        {

            using (EJDB db = new EJDB())
            {
                var result = from m in db.DBColumn
                             where m.TableID == tableid
                             orderby m.orderid
                             select new EJ.DBColumn
                             {
                                 Name = m.Name,
                                 id = m.id
                             };
                return result.ToArray();
            }
        }
        [RemotingMethod]
        public EJ.DBColumn[] GetColumns(int tableid)
        {

            using (EJDB db = new EJDB())
            {
                var result = from m in db.DBColumn
                             where m.TableID == tableid
                             orderby m.orderid
                             select m;
                return result.ToArray();
            }
        }

        [RemotingMethod]
        public string[] GetColumnNamesByTableName(string tablename, int databaseid)
        {

            using (EJDB db = new EJDB())
            {
                var result = from m in db.DBColumn
                             join t in db.DBTable on m.TableID equals t.id
                             where t.Name == tablename && t.DatabaseID == databaseid
                             orderby m.orderid
                             select m.Name;
                return result.ToArray();
            }
        }
        [RemotingMethod]
        public EJ.DBModule[] GetDBModuleList(int databaseid, int parentid)
        {
            using (EJDB db = new EJDB())
            {
                return db.DBModule.Where(m => m.DatabaseID == databaseid && m.parentID == parentid).OrderBy(m => m.Name).ToArray();
            }
        }
        [RemotingMethod]
        public EJ.DBDeleteConfig[] GetTableDeleteConfigList(int tableid)
        {
            using (EJDB db = new EJDB())
            {
                var results = db.DBDeleteConfig.Where(m => m.TableID == tableid).ToArray();
                foreach (var item in results)
                {
                    item.RelaTable_Desc = db.DBTable.FirstOrDefault(m => m.id == item.RelaTableID).Name;
                    item.RelaColumn_Desc = db.DBColumn.FirstOrDefault(m => m.id == item.RelaColumID).Name;
                }
                return results;
            }
        }
        [RemotingMethod]
        public EJ.IDXIndex[] GetTableIDXIndexList(int tableid)
        {
            using (EJDB db = new EJDB())
            {
                return db.IDXIndex.Where(m => m.TableID == tableid).ToArray();
            }
        }
        [RemotingMethod]
        public EJ.DBTable[] GetTableList(int databaseid)
        {
            using (EJDB_Check db = new EJDB_Check())
            {
                return db.DBTable.Where(m => m.DatabaseID == databaseid).OrderBy(m => m.Name).ToArray();
            }
        }
        [RemotingMethod]
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
        [RemotingMethod]
        public EJ.DBColumn[] GetColumnList(int tableid)
        {
            using (EJDB db = new EJDB())
            {
                return db.DBColumn.Where(m => m.TableID == tableid).OrderBy(m => m.orderid).ToArray();
            }
            return null;
        }
        [RemotingMethod]
        public EJ.classproperty[] GetClassPropertyList(int tableid)
        {
            using (EJDB db = new EJDB())
            {
                return db.classproperty.Where(m => m.tableid == tableid).ToArray();
            }
            return null;
        }

        bool IsSame(EJ.IDXIndex index1,EJ.IDXIndex index2)
        {
            return string.Equals(index1.Keys, index2.Keys, StringComparison.CurrentCultureIgnoreCase) && 
                index1.IsUnique == index2.IsUnique && 
                index1.IsClustered == index2.IsClustered;
        }

        [RemotingMethod]
        public void ModifyTable(EJ.DBTable newtable, EJ.DBColumn[] nowcolumns, EJ.DBDeleteConfig[] delConfigs, IndexInfo[] idxConfigs, EJ.classproperty[] classProperties)
        {
            using (EJDB db = new EJDB())
            {
                Way.EntityDB.IDatabaseService invokingDB = null;

                EJ.DBTable oldtable = db.DBTable.FirstOrDefault(m => m.id == newtable.id);

                var nowids = (from m in nowcolumns
                              where m.id != null
                              select m.id).ToArray();

                try
                {
                    db.BeginTransaction();
                    //锁住，防止同时添加table
                    EJ.Databases database = db.Databases.Where(m => m.id == oldtable.DatabaseID).FirstOrDefault();
                    database.iLock++;
                    db.Update(database);

                    if (db.DBTable.Where(m => m.DatabaseID == oldtable.DatabaseID && m.id != newtable.id && m.Name == newtable.Name).Count() > 0)
                        throw new Exception("数据表名称重复");



                    var oldcolumns = db.DBColumn.Where(m => m.TableID == oldtable.id).ToArray().ToJsonString().ToJsonObject<EJ.DBColumn[]>();
                    //找出下面这些对象
                    EJ.DBColumn[] addcolumns = nowcolumns.Where(m => m.id == null).ToArray();
                    EJ.DBColumn[] delColumns = oldcolumns.Where(m => nowids.Contains(m.id) == false).ToArray();


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
                        }
                    }

                    string conStr = string.Format(database.conStr, $"{WebRoot}/");
                    if (conStr.ToLower() == db.Database.ConnectionString.ToLower())
                    {
                        invokingDB = db.Database;
                    }
                    else
                    {
                        invokingDB = DBHelper.CreateInvokeDatabase(database);
                        invokingDB.DBContext.BeginTransaction();
                    }

                    List<EJ.IDXIndex> submitedIndexes = new List<EJ.IDXIndex>();
                    foreach (var config in idxConfigs)
                    {
                        EJ.IDXIndex idxIndex = new EJ.IDXIndex();
                        idxIndex.TableID = oldtable.id;
                        idxIndex.IsUnique = config.IsUnique;
                        idxIndex.IsClustered = config.IsClustered;
                        idxIndex.Keys = config.ColumnNames.ToSplitString();
                        submitedIndexes.Add(idxIndex);
                    }

                    var nowIndexes = db.IDXIndex.Where(m => m.TableID == oldtable.id).ToList();
                    List<EJ.IDXIndex> deletedIndexes = new List<EJ.IDXIndex>();
                    EJ.IDXIndex[] newIndexes = new EJ.IDXIndex[0];

                    foreach (var changedColumn in changedColumns)
                    {
                        var changeObj = changedColumn.ChangedProperties["Name"];
                        if (changeObj != null)
                        {
                            //名字改变
                            var indexes = nowIndexes.Where(m => $",{m.Keys.ToLower()},".Contains($",{changeObj.OriginalValue.ToString().ToLower()},")).ToArray();
                            deletedIndexes.AddRange(indexes);

                            foreach (var d in deletedIndexes)
                            {
                                if (nowIndexes.Contains(d))
                                    nowIndexes.Remove(d);
                            }
                        }
                    }

                    if (nowIndexes.Count > 0)
                    {
                        var indexes = nowIndexes.Where(m => submitedIndexes.Any(o => IsSame(o,m)) == false).ToArray();
                        deletedIndexes.AddRange(indexes);
                        foreach (var d in deletedIndexes)
                        {
                            if (nowIndexes.Contains(d))
                                nowIndexes.Remove(d);
                        }

                        newIndexes = submitedIndexes.Where(m => nowIndexes.Any(o => IsSame(o, m) ) == false).ToArray();
                    }

                    var action = new EF_AlterTable_Action(oldtable.id.Value, oldtable.Name, newtable.Name,
                        addcolumns, changedColumns.ToArray(), delColumns, deletedIndexes.ToArray(), newIndexes);
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

                    db.Database.ExecSqlString("delete from IDXIndex where TableID=" + oldtable.id);
                    foreach (var index in submitedIndexes)
                    {
                        db.Insert(index);
                    }

                    //添加级联删除
                    db.Database.ExecSqlString("delete from DBDeleteConfig where TableID=" + oldtable.id);

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

                    db.Delete(db.classproperty.Where(m => m.tableid == newtable.id));
                    foreach (var p in classProperties)
                    {
                        p.tableid = newtable.id;
                        db.Insert(p);
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
                    if (invokingDB != null && invokingDB.DBContext != db)
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

        public static void SetLastUpdateID(object actionid, string databaseGuid, Way.EntityDB.IDatabaseService db)
        {
            if (databaseGuid.IsNullOrEmpty())
                throw new Exception("Database Guid can not be empty");
            var dbconfig = db.ExecSqlString("select contentConfig from __wayeasyjob").ToString().ToJsonObject<DataBaseConfig>();
            dbconfig.LastUpdatedID = Convert.ToInt32(actionid);
            dbconfig.DatabaseGuid = databaseGuid;

            var data = new Way.EntityDB.CustomDataItem("__wayeasyjob", null, null);
            data.SetValue("contentConfig", dbconfig.ToJsonString());
            db.Update(data);
        }

        [RemotingMethod]
        public void DeleteModule(EJ.DBModule module)
        {
            using (EJDB db = new EJDB())
            {
                db.Delete(module);
            }
        }
        [RemotingMethod]
        public string GetDBTablePath(int tableid)
        {

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
        [RemotingMethod]
        public string GetDBModulePath(int moduleid)
        {

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

                var project = db.Project.FirstOrDefault(m => m.id == database.ProjectID);
                result.Insert(0, project.Name + "/Database/");
                return result.ToString();
            }
        }
        [RemotingMethod]
        public int UpdateDBModule(EJ.DBModule module)
        {

            using (EJDB db = new EJDB())
            {
                try
                {
                    db.BeginTransaction();

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
                catch (Exception ex)
                {
                    db.RollbackTransaction();
                    throw ex;
                }
            }
        }

        [RemotingMethod]
        public void DeleteTable(int databaseID, string tableName)
        {

            Way.EntityDB.IDatabaseService invokingDB = null;
            using (EJDB db = new EJDB())
            {
                try
                {
                    db.BeginTransaction();
                    //锁住，防止同时添加table
                    EJ.Databases database = db.Databases.Where(m => m.id == databaseID).FirstOrDefault();
                    database.iLock++;
                    db.Update(database);

                    string conStr = string.Format(database.conStr, $"{WebRoot}/");
                    if (conStr.ToLower() == db.Database.ConnectionString.ToLower())
                    {
                        invokingDB = db.Database;
                    }
                    else
                    {
                        invokingDB = DBHelper.CreateInvokeDatabase(database);
                        invokingDB.DBContext.BeginTransaction();
                    }




                    EJ.DBTable table = db.DBTable.FirstOrDefault(m => m.DatabaseID == databaseID && m.Name == tableName);
                    EJ.IDXIndex[] indexes = db.IDXIndex.Where(m => m.TableID == table.id).ToArray();
                    var action = new EF_DeleteTable_Action(table, indexes);
                    action.Invoke(invokingDB);

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
        [RemotingMethod]
        public EJ.DBColumn[] GetDatabaseCurrentColumns(EJ.Databases config, string table)
        {
            var dbservice = Way.EntityDB.Design.DBHelper.CreateDatabaseDesignService((Way.EntityDB.DatabaseType)(int)config.dbType);
            var db = Way.EntityDB.DBContext.CreateDatabaseService(config.conStr, (Way.EntityDB.DatabaseType)(int)config.dbType);
            return dbservice.GetCurrentColumns(db, table).ToArray();
        }
        [RemotingMethod]
        public IndexInfo[] GetDatabaseCurrentIndexes(EJ.Databases config, string table)
        {
            var dbservice = Way.EntityDB.Design.DBHelper.CreateDatabaseDesignService((Way.EntityDB.DatabaseType)(int)config.dbType);
            var db = Way.EntityDB.DBContext.CreateDatabaseService(config.conStr, (Way.EntityDB.DatabaseType)(int)config.dbType);
            return dbservice.GetCurrentIndexes(db, table).ToArray();
        }

        [RemotingMethod]
        public string[] GetDatabaseCurrentTableNames(EJ.Databases config)
        {
            var dbservice = Way.EntityDB.Design.DBHelper.CreateDatabaseDesignService((Way.EntityDB.DatabaseType)(int)config.dbType);
            var db = Way.EntityDB.DBContext.CreateDatabaseService(config.conStr, (Way.EntityDB.DatabaseType)(int)config.dbType);
            return dbservice.GetCurrentTableNames(db).ToArray();
        }

        [RemotingMethod]
        public int CreateTables(int databaseid, TableInfo[] tableinfos)
        {
            using (EJDB db = new EJDB())
            {
                Way.EntityDB.IDatabaseService invokingDB = null;
                try
                {
                    db.BeginTransaction();
                    //锁住，防止同时添加table
                    EJ.Databases database = db.Databases.Where(m => m.id == databaseid).FirstOrDefault();
                    database.iLock++;
                    db.Update(database);
                    string conStr = string.Format(database.conStr, $"{WebRoot}/");
                    if (conStr.ToLower() == db.Database.ConnectionString.ToLower())
                    {
                        invokingDB = db.Database;
                    }
                    else
                    {
                        invokingDB = DBHelper.CreateInvokeDatabase(database);
                        invokingDB.DBContext.BeginTransaction();
                    }
                    object lastActionID = null;
                    foreach (var tableinfo in tableinfos)
                    {
                        var table = new EJ.DBTable()
                        {
                            Name = tableinfo.TableName,
                            DatabaseID = databaseid,
                        };
                        if (db.DBTable.Where(m => m.DatabaseID == databaseid && m.Name == table.Name).Any())
                            throw new Exception("数据表名称重复");

                        db.Update(table);



                        foreach (EJ.DBColumn column in tableinfo.Columns)
                        {
                            column.TableID = table.id;
                            db.Update(column);
                        }

                        var indexes = new List<EJ.IDXIndex>();
                        foreach (var config in tableinfo.Indexes)
                        {
                            EJ.IDXIndex idxIndex = new EJ.IDXIndex();
                            idxIndex.TableID = table.id;
                            idxIndex.IsUnique = config.IsUnique;
                            idxIndex.IsClustered = config.IsClustered;
                            idxIndex.Keys = config.ColumnNames.ToSplitString();
                            db.Update(idxIndex);
                            indexes.Add(idxIndex);
                        }

                        var action = new EF_CreateTable_Action(table, tableinfo.Columns, indexes.ToArray());
                        action.Invoke(invokingDB);


                        lastActionID = action.Save(db, database.id.GetValueOrDefault());

                    }
                    if (lastActionID != null)
                    {
                        SetLastUpdateID(lastActionID, database.Guid, invokingDB);
                    }

                    if (invokingDB.DBContext != db)
                    {
                        invokingDB.DBContext.CommitTransaction();
                    }

                    db.CommitTransaction();


                }
                catch (Exception ex)
                {
                    if (invokingDB != null && invokingDB.DBContext != db)
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
            return 0;
        }


        [RemotingMethod]
        public EJ.DBTable CreateTable(EJ.DBTable table, EJ.DBColumn[] columns, EJ.DBDeleteConfig[] delConfigs, IndexInfo[] idxConfigs, EJ.classproperty[] classProperties)
        {

            using (EJDB db = new EJDB())
            {
                Way.EntityDB.IDatabaseService invokingDB = null;
                try
                {
                    db.BeginTransaction();
                    //锁住，防止同时添加table
                    EJ.Databases database = db.Databases.Where(m => m.id == table.DatabaseID).FirstOrDefault();
                    database.iLock++;
                    db.Update(database);


                    string conStr = string.Format(database.conStr, $"{WebRoot}/");
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

                    db.Update(table);


                    foreach (EJ.DBColumn column in columns)
                    {
                        column.TableID = table.id;
                        db.Update(column);
                    }
                    var indexes = new List<EJ.IDXIndex>();
                    foreach (var config in idxConfigs)
                    {
                        EJ.IDXIndex idxIndex = new EJ.IDXIndex();
                        idxIndex.TableID = table.id;
                        idxIndex.IsUnique = config.IsUnique;
                        idxIndex.IsClustered = config.IsClustered;
                        idxIndex.Keys = config.ColumnNames.ToSplitString();
                        db.Update(idxIndex);
                        indexes.Add(idxIndex);
                    }
                    var action = new EF_CreateTable_Action(table, columns, indexes.ToArray());
                    action.Invoke(invokingDB);

                    //添加级联删除
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

                    db.Delete(db.classproperty.Where(m => m.tableid == table.id));
                    foreach (var p in classProperties)
                    {
                        p.tableid = table.id;
                        db.Insert(p);
                    }

                    object actionid = action.Save(db, database.id.GetValueOrDefault());
                    SetLastUpdateID(actionid, database.Guid, invokingDB);

                    if (invokingDB.DBContext != db)
                    {
                        invokingDB.DBContext.CommitTransaction();
                    }
                    db.CommitTransaction();

                    return table;
                }
                catch (Exception ex)
                {
                    if (invokingDB != null && invokingDB.DBContext != db)
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

        /// <summary>
        /// 重建数据库action
        /// </summary>
        void rebuildDatabaseActions(EJDB db, EJ.Databases dbobj)
        {
            db.BeginTransaction();
            try
            {
                db.Database.ExecSqlString($"delete from __action where databaseid={dbobj.id}");
                var tables = db.DBTable.Where(m => m.DatabaseID == dbobj.id).ToArray();
                foreach (var table in tables)
                {
                    var columns = db.DBColumn.Where(m => m.TableID == table.id).ToArray();
                    var indexes = db.IDXIndex.Where(m => m.TableID == table.id).ToArray();

                    var action = new EF_CreateTable_Action(table, columns, indexes);
                    action.Save(db, dbobj.id.Value);
                }
                db.CommitTransaction();
            }
            catch (Exception ex)
            {
                db.RollbackTransaction();
                throw ex;
            }
        }

        /// <summary>
        /// 创建数据库
        /// </summary>
        /// <param name="databaseType">数据库类型，如SqlServer</param>
        /// <param name="connectionString">连接字符串</param>
        /// <param name="nameSpace"></param>
        /// <param name="dbname"></param>
        [RemotingMethod]
        public int UpdateDatabase(EJ.Databases dataitem)
        {

            if (string.IsNullOrEmpty(dataitem.Name))
                throw new Exception("database name is empty");
            if (string.IsNullOrEmpty(dataitem.conStr))
                throw new Exception("connection string is empty");
            if (string.IsNullOrEmpty(dataitem.NameSpace))
                throw new Exception("namespace is empty");
            using (EJDB db = new EJDB())
            {
                db.BeginTransaction();
                try
                {
                    if (dataitem.id == null)
                    {
                        db.Insert(dataitem);
                        IDatabaseDesignService dbservice = DBHelper.CreateDatabaseDesignService((Way.EntityDB.DatabaseType)(int)dataitem.dbType);
                        dbservice.Create(dataitem);

                        db.Update(dataitem);
                    }
                    else
                    {
                        var oldData = db.Databases.FirstOrDefault(m => m.id == dataitem.id);

                        if (dataitem.dbType != oldData.dbType || dataitem.conStr != oldData.conStr)
                        {
                            //变更数据库类型
                            IDatabaseDesignService dbservice = Way.EntityDB.Design.DBHelper.CreateDatabaseDesignService((Way.EntityDB.DatabaseType)(int)dataitem.dbType);
                            dbservice.Create(dataitem);
                            db.Update(dataitem);

                            //更新到现在的数据结构
                            var invokeDB = Way.EntityDB.Design.DBHelper.CreateInvokeDatabase(dataitem);
                            var dbconfig = invokeDB.ExecSqlString("select contentConfig from __wayeasyjob").ToString().ToJsonObject<DataBaseConfig>();
                            dbconfig.DatabaseGuid = dataitem.Guid;
                            invokeDB.DBContext.BeginTransaction();
                            try
                            {
                                int? lastid = null;
                                using (var dt = db.Database.SelectTable("select * from __action where id>" + dbconfig.LastUpdatedID + " and databaseid=" + dataitem.id + " order by [id]"))
                                {
                                    foreach (var datarow in dt.Rows)
                                    {
                                        string actionType = datarow["type"].ToString();
                                        int id = Convert.ToInt32(datarow["id"]);

                                        string json = datarow["content"].ToString();


                                        Type type = typeof(Way.EntityDB.Design.Actions.Action).GetTypeInfo().Assembly.GetType($"Way.EntityDB.Design.Actions.{actionType}");
                                        var actionItem = (Way.EntityDB.Design.Actions.Action)Newtonsoft.Json.JsonConvert.DeserializeObject(json, type);

                                        actionItem.Invoke(invokeDB);

                                        lastid = id;
                                    }
                                    if (lastid != null)
                                    {
                                        dbconfig.LastUpdatedID = lastid.Value;
                                    }
                                }

                                var obj = new Way.EntityDB.CustomDataItem("__wayeasyjob", null, null);
                                obj.SetValue("contentConfig", dbconfig.ToJsonString());
                                invokeDB.Update(obj);

                                invokeDB.DBContext.CommitTransaction();
                            }
                            catch
                            {
                                invokeDB.DBContext.RollbackTransaction();
                                throw;
                            }
                        }
                        else if (dataitem.Name.ToLower() != oldData.Name.ToLower())
                        {
                            IDatabaseDesignService dbservice = Way.EntityDB.Design.DBHelper.CreateDatabaseDesignService((Way.EntityDB.DatabaseType)(int)oldData.dbType);
                            dbservice.ChangeName(oldData, dataitem.Name, dataitem.conStr);
                        }

                        db.Update(dataitem);
                    }
                    db.CommitTransaction();
                    return dataitem.id.Value;
                }
                catch (Exception ex)
                {
                    db.RollbackTransaction();
                    throw ex;
                }
            }

        }


        [RemotingMethod]
        public string[] GetProjectDllFiles(int projectid)
        {

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
        [RemotingMethod]
        public EJ.InterfaceModule[] GetInterfaceModuleList(int projectid, int parentid)
        {

            using (EJDB_Check db = new EJDB_Check())
            {
                return db.InterfaceModule.Where(m => m.ProjectID == projectid && m.ParentID == parentid).OrderBy(m => m.Name).ToArray();
            }
        }
        [RemotingMethod]
        public void DeleteInterfaceModule(EJ.InterfaceModule module)
        {

            using (EJDB db = new EJDB())
            {
                db.Delete(module);
            }
        }
        [RemotingMethod]
        public void UnLockInterfaceModule(int moduleid)
        {

            using (EJDB_Check db = new EJDB_Check())
            {
                var module = db.InterfaceModule.FirstOrDefault(m => m.id == moduleid);
                if (module.LockUserId != this.User.id)
                    throw new Exception("不能解锁他人锁定的模块");
                module.LockUserId = null;
                db.Update(module);
            }
        }
        [RemotingMethod]
        public int LockInterfaceModule(int moduleid)
        {
            int userid = this.User.id.Value;
            using (EJDB_Check db = new EJDB_Check())
            {
                db.BeginTransaction();
                try
                {
                    var module = db.InterfaceModule.FirstOrDefault(m => m.id == moduleid);
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
        [RemotingMethod]
        public int UpdateInterfaceModule(EJ.InterfaceModule module)
        {

            using (EJDB db = new EJDB())
            {
                try
                {
                    db.BeginTransaction();

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

        [RemotingMethod]
        public string GetInterfaceModulePath(int moduleid)
        {

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
        [RemotingMethod]
        public int UpdateInterfaceInModule(EJ.InterfaceInModule module)
        {

            using (EJDB db = new EJDB())
            {
                var docModule = db.InterfaceModule.FirstOrDefault(m => m.id == module.ModuleID);
                if (docModule.LockUserId != null && docModule.LockUserId != this.User.id)
                {
                    throw new Exception("无法修改锁定的模块");
                }
                db.Update(module);
                return module.id.Value;
            }
        }
        [RemotingMethod]
        public void DeleteInterfaceInModule(EJ.InterfaceInModule module)
        {

            using (EJDB db = new EJDB())
            {
                var docModule = db.InterfaceModule.FirstOrDefault(m => m.id == module.ModuleID);
                if (docModule.LockUserId != null && docModule.LockUserId != this.User.id)
                {
                    throw new Exception("无法修改锁定的模块");
                }

                db.Delete(module);
            }
        }
        [RemotingMethod]
        public EJ.InterfaceInModule[] GetInterfaceInModule(int moduleid)
        {

            using (EJDB_Check db = new EJDB_Check())
            {
                return db.InterfaceInModule.Where(m => m.ModuleID == moduleid).ToArray();
            }
        }
        [RemotingMethod]
        public int GetInterfaceModuleID(int interfaceInModuleId)
        {

            using (EJDB_Check db = new EJDB_Check())
            {
                var item = db.InterfaceInModule.FirstOrDefault(m => m.id == interfaceInModuleId);
                if (item != null)
                    return item.ModuleID.GetValueOrDefault();
                return 0;
            }
        }
        [RemotingMethod]
        public string GetInterfaceInModulePath(int itemid)
        {

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

        [RemotingMethod]
        public SearchContent[] Search(string key, int pagesize, int pageindex)
        {

            using (EJDB_Check db = new EJDB_Check())
            {
                return db.SearchContents.Where(m => m.Content.Contains(key)).OrderBy(m => m.Type).Skip(pagesize * pageindex).Take(pagesize).ToArray();
            }
        }

        [RemotingMethod]
        public void SubmitBug(string title, byte[] textContent, byte[] picContent)
        {

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

        [RemotingMethod]
        public int GetMyBugListCount()
        {
            using (EJDB_Check db = new EJDB_Check())
            {
                if (this.User.Role == EJ.User_RoleEnum.客户端测试人员)
                    return db.MyBugList.Where(m => m.SubmitUserID == this.User.id && m.Status == EJ.Bug_StatusEnum.反馈给提交者).Count();
                return db.MyBugList.Where(m => m.Status == EJ.Bug_StatusEnum.提交给开发人员).Count();
            }
        }

        [RemotingMethod]
        public BugItem[] GetMyBugs()
        {

            using (EJDB_Check db = new EJDB_Check())
            {
                var query = from m in db.MyBugList
                            join u in db.User on m.SubmitUserID equals u.id
                            orderby m.Status, m.SubmitTime
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
                                HandlerUserName = db.User.Where(p => p.id == m.HandlerID).Select(p => p.Name).FirstOrDefault(),
                            };
                return query.Take(50).ToArray();
            }

        }

        [RemotingMethod]
        public BugHistoryItem[] GetBugHistories(int bugid)
        {

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
                return query.ToArray();
            }
        }

        [RemotingMethod]
        public string GetBugPicture(int bugid)
        {

            using (EJDB_Check db = new EJDB_Check())
            {
                var pic = db.BugImages.FirstOrDefault(m => m.BugID == bugid);
                if (pic != null)
                    return Convert.ToBase64String(pic.content);
            }
            return null;
        }
        [RemotingMethod]
        public void BugFinish(int bugid)
        {

            using (EJDB_Check db = new EJDB_Check())
            {
                var bug = db.Bug.FirstOrDefault(m => m.id == bugid);
                bug.Status = EJ.Bug_StatusEnum.处理完毕;
                db.Update(bug);
            }
        }
        [RemotingMethod]
        public void SubmitHistory(int bugid, byte[] txtContent)
        {

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
                if (bug.SubmitUserID == this.User.id)
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

        [RemotingMethod]
        public FileInfo[] GetUpdateFileList()
        {
            string folder = $"{WebRoot}/updates";
            if (System.IO.Directory.Exists(folder) == false)
                return new FileInfo[0];
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
            return fileinfos.ToArray();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="savepath">可能是: x86\lib\test.dll形式</param>
        /// <returns></returns>
        [RemotingMethod]
        public string DownLoadFile(string savepath)
        {
            savepath = savepath.Replace("\\", "/");
            while (savepath.StartsWith("/"))
                savepath = savepath.Substring(1);
            return Convert.ToBase64String(System.IO.File.ReadAllBytes($"{WebRoot}/updates/{savepath}"));
        }
    }
    public class TableInfo
    {
        public string TableName;
        public EJ.DBColumn[] Columns;
        public IndexInfo[] Indexes;
    }
    public class FileInfo
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
