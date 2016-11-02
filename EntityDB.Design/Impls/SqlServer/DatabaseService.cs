using EntityDB.Design.Services;
using EntityDB.Design.Services;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;

namespace EntityDB.Design.Database.SqlServer
{
    public class DatabaseService : IDatabaseDesignService
    {
        public void Create(EJ.Databases database)
        {
            string constr = database.conStr;
            constr = Regex.Replace(database.conStr, @"database=(\w)+", "database=master" , RegexOptions.IgnoreCase);
            //throw new Exception(constr);
            var db = EntityDB.DBContext.CreateDatabaseService(constr, EntityDB.DatabaseType.SqlServer);

            db.ExecSqlString("if not exists(select [dbid] from sysdatabases where [name]='" + database.Name + "') create database " + database.Name);

            db = EntityDB.DBContext.CreateDatabaseService(database.conStr, EntityDB.DatabaseType.SqlServer);
            CreateEasyJobTable(db);
            //ado.ExecCommandTextUseSameCon("if not exists(select [dbid] from sysdatabases where [name]='" + txt_databasename.Text + "') create database " + txt_databasename.Text);
        }

        public void ChangeName(EJ.Databases database, string newName, string newConnectString)
        {
             string constr = database.conStr;
            constr = Regex.Replace(database.conStr, @"database=(\w)+", "database=master" , RegexOptions.IgnoreCase);
            //throw new Exception(constr);
            var db = EntityDB.DBContext.CreateDatabaseService(constr, EntityDB.DatabaseType.SqlServer);
            {
                db.ExecSqlString("exec sp_renamedb '"+database.Name+"','"+newName+"'");

                try
                {
                    var db2 = EntityDB.DBContext.CreateDatabaseService(newConnectString, EntityDB.DatabaseType.SqlServer);
                    db2.ExecSqlString("select 1");
                }
                catch
                {
                    //
                    db.ExecSqlString("exec sp_renamedb '" + newName + "','" + database.Name + "'");
                    throw new Exception("连接字符串错误");
                }
            }
         
        }
        public static string GetDBTypeString(Type type)
        {
            if (type == typeof(long))
                return "bigint";
            if (type == typeof(Byte[]))
                return "binary";
            if (type == typeof(bool))
                return "bit";
            if (type == typeof(string))
                return "varchar";
            if (type == typeof(DateTime))
                return "datetime";
            if (type == typeof(decimal))
                return "decimal";
            if (type == typeof(double))
                return "float";
            if (type == typeof(float))
                return "float";
            if (type == typeof(int))
                return "int";
            return "";
        }
        public void GetViews(EntityDB.IDatabaseService db, out List<EJ.DBTable> tables, out List<EJ.DBColumn> columns)
        {
            tables = new List<EJ.DBTable>();
            columns = new List<EJ.DBColumn>();
            return;

            int tid = 0;
            using (DataTable dtable = db.SelectTable("select name from sysobjects where xtype='V'"))
            {
                foreach (DataRow drow in dtable.Rows)
                {
                    string viewName = drow[0].ToString();
                    EJ.DBTable dbtable = new EJ.DBTable();
                    dbtable.Name = viewName;
                    dbtable.id = tid++;
                    tables.Add(dbtable);

                    using (DataTable dc = db.SelectTable("select top 0 * from " + viewName))
                    {
                        foreach (DataColumn column in dc.Columns)
                        {
                            EJ.DBColumn dbcolumn = new EJ.DBColumn();
                            dbcolumn.dbType = GetDBTypeString(column.DataType);
                            dbcolumn.Name = column.ColumnName;
                            dbcolumn.TableID = dbtable.id;
                            columns.Add(dbcolumn);
                        }
                    }
                }
            }
        }


        public void ImportData(EntityDB.IDatabaseService db, EJ.DB.EasyJob ejdb, DataSet dset, bool clearDataFirst)
        {
            #region 导入数据
            foreach (DataTable dtable in dset.Tables)
            {
                var db_table = ejdb.DBTable.FirstOrDefault(m=>m.Name == dtable.TableName);
                if (db_table == null)
                    throw new Exception(string.Format("找不到{0}数据表定义", dtable.TableName));
                var pkcolumn = ejdb.DBColumn.FirstOrDefault( m=>m.TableID == db_table.id && m.IsPKID == true );
                bool hasAutoColumn = ejdb.DBColumn.Count(m => m.TableID == db_table.id && m.IsAutoIncrement == true) > 0;

                if (pkcolumn == null)
                    throw (new Exception(string.Format("{0}-{1}没有设置主键", db_table.caption, db_table.Name)));

                try
                {
                    db.ExecSqlString("SET IDENTITY_INSERT [" + dtable.TableName + "] on");
                }
                catch (SqlException ex)
                {
                    if (ex.Number == 8106)
                    {
                        //不是自增长，不用设
                    }
                    else
                    {
                        throw (new Exception("数据库可能不存在表" + dtable.TableName));
                    }
                }
                catch (Exception ex)
                {
                    throw (new Exception("数据库可能不存在表" + dtable.TableName));
                }

                if (clearDataFirst)
                {
                    db.ExecSqlString("delete from [" + dtable.TableName + "]");
                }
                try
                {
                    DataTable 现在数据库DataTable = null;
                    try
                    {
                        现在数据库DataTable = db.SelectTable("select top 0 * from [" + dtable.TableName + "]");
                    }
                    catch (Exception ex)
                    {
                        continue;
                    }
                    foreach (DataRow row in dtable.Rows)
                    {
                        if (db.ExecSqlString("select 1 from [" + dtable.TableName + "] where [" + pkcolumn.Name + "]=" + row[pkcolumn.Name]) != null)
                        {
                            //update
                            StringBuilder str_fields = new StringBuilder();
                            int pIndex = 0;
                            List<object> values = new List<object>();
                            for (int columnindex = 0; columnindex < dtable.Columns.Count; columnindex++)
                            {
                                if (dtable.Columns[columnindex].ColumnName.ToLower() == pkcolumn.Name.ToLower())
                                    continue;
                                if (现在数据库DataTable.Columns.Contains(dtable.Columns[columnindex].ColumnName) == false)
                                    continue;

                                if (str_fields.Length > 0)
                                    str_fields.Append(',');
                                str_fields.Append("[" + dtable.Columns[columnindex].ColumnName + "]=@p" + pIndex);
                                values.Add(row[columnindex]);
                                pIndex++;

                            }
                            values.Add(row[pkcolumn.Name]);
                            db.ExecSqlString("update [" + dtable.TableName + "] set " + str_fields + " where [" + pkcolumn.Name + "]=@p" + pIndex, values.ToArray());
                        }
                        else
                        {
                            //insert
                            StringBuilder str_fields = new StringBuilder();
                            StringBuilder str_values = new StringBuilder();
                            int pIndex = 0;
                            List<object> values = new List<object>();
                            for (int columnindex = 0; columnindex < dtable.Columns.Count; columnindex++)
                            {
                                if (row[columnindex] == DBNull.Value)
                                    continue;
                                if (现在数据库DataTable.Columns.Contains(dtable.Columns[columnindex].ColumnName) == false)
                                    continue;

                                if (str_fields.Length > 0)
                                    str_fields.Append(',');
                                str_fields.Append("[" + dtable.Columns[columnindex].ColumnName + "]");

                                if (str_values.Length > 0)
                                    str_values.Append(',');
                                str_values.Append("@p" + pIndex);
                                values.Add(row[columnindex]);
                                pIndex++;
                            }
                            db.ExecSqlString("insert into [" + dtable.TableName + "] (" + str_fields + ") values (" + str_values + ")", values.ToArray());
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw (ex);
                }
                finally
                {
                    try
                    {
                        db.ExecSqlString("SET IDENTITY_INSERT [" + dtable.TableName + "] off");
                    }
                    catch (SqlException ex)
                    {
                        if (ex.Number == 8106)
                        {
                            //不是自增长，不用设
                        }
                        else
                        {
                            throw ex;
                        }
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                }
            }
            #endregion
        }


        public void CreateEasyJobTable(EntityDB.IDatabaseService db)
        {
            bool exists = true;
            try
            {
                db.ExecSqlString("select * from __WayEasyJob");
            }
            catch
            {
                exists = false;
            }
            if (!exists)
            {
                db.ExecSqlString("CREATE TABLE [__WayEasyJob](contentConfig varchar(1000) NOT NULL)");
                var dbconfig = new DataBaseConfig();
                try
                {
                    dbconfig.LastUpdatedID = Convert.ToInt32(db.ExecSqlString("select lastID from __EasyJob"));
                }
                catch
                {
                }
                db.ExecSqlString("insert into __WayEasyJob (contentConfig) values (@p0)", dbconfig.ToJsonString());
            }
        }


        public string GetObjectFormat()
        {
            return "[{0}]";
        }
    }
}