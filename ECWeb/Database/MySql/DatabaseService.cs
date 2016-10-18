using ECWeb.Database.Services;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;

namespace ECWeb.Database.MySql
{
    public class DatabaseService : IDatabaseService
    {


        public void Create(EJ.Databases database)
        {
            var dbnameMatch = System.Text.RegularExpressions.Regex.Match(database.conStr, @"database=(?<dname>(\w)+)", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            if (dbnameMatch == null)
            {
                throw new Exception("连接字符串必须采用以下形式server=localhost;User Id=root;password=123456;Database=testDB");
            }

            var db = EntityDB.DBContext.CreateDatabaseService(database.conStr.Replace(dbnameMatch.Value, ""), EntityDB.DatabaseType.MySql);
            db.ExecSqlString("create database `" + database.Name.ToLower() + "` CHARACTER SET 'utf8' COLLATE 'utf8_general_ci'");

            //创建必须表
            db = EntityDB.DBContext.CreateDatabaseService(database.conStr, EntityDB.DatabaseType.MySql);
            CreateEasyJobTable(db);
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
                db.ExecSqlString("create table  `__WayEasyJob` (contentConfig varchar(1000)  not null)");
                db.ExecSqlString("insert into __WayEasyJob (contentConfig) values (@p0)", new DataBaseConfig().ToJsonString());
            }
            

            //try
            //{
            //    db.ExecSqlString("create table  `` (lastID int(11)  not null)");
            //    db.ExecSqlString("insert into `` (lastID) values (0)");
            //}
            //catch
            //{
            //}
        }

        public void ChangeName(EJ.Databases database, string newName, string newConnectString)
        {
            throw new Exception("MySql 不支持修改数据库名称");
            var dbnameMatch = System.Text.RegularExpressions.Regex.Match(database.conStr, @"database=(?<dname>(\w)+)", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            if (dbnameMatch == null)
            {
                throw new Exception("连接字符串必须采用以下形式server=localhost;User Id=root;password=123456;Database=testDB");
            }
            var db = EntityDB.DBContext.CreateDatabaseService(database.conStr.Replace(dbnameMatch.Value, ""), EntityDB.DatabaseType.MySql);
            db.ExecSqlString(string.Format("RENAME database `{0}` TO `{1}`", database.Name.ToLower(), newName.ToLower()));

            try
            {
                var db2 = EntityDB.DBContext.CreateDatabaseService(newConnectString, EntityDB.DatabaseType.MySql);
                db2.ExecSqlString("select 1");
            }
            catch
            {
                //
                db.ExecSqlString(string.Format("RENAME database `{0}` TO `{1}`", newName.ToLower(), database.Name.ToLower()));
                throw new Exception("连接字符串错误");
            }
            
        }

        public void GetViews(EntityDB.IDatabaseService db, out List<EJ.DBTable> tables, out List<EJ.DBColumn> columns)
        {
            tables = new List<EJ.DBTable>();
            columns = new List<EJ.DBColumn>();
        }

        public void ImportData(EntityDB.IDatabaseService db, EJDB ejdb, System.Data.DataSet dset, bool clearDataFirst)
        {
            #region 导入数据
            foreach (DataTable dtable in dset.Tables)
            {
                var db_table = ejdb.DBTable.FirstOrDefault(m => m.Name == dtable.TableName);
                if (db_table == null)
                    throw new Exception(string.Format("找不到{0}数据表定义", dtable.TableName));
                var pkcolumn = ejdb.DBColumn.FirstOrDefault(m => m.TableID == db_table.id && m.IsPKID == true);
                bool hasAutoColumn = ejdb.DBColumn.Count(m => m.TableID == db_table.id && m.IsAutoIncrement == true) > 0;

                if (pkcolumn == null)
                    throw (new Exception(string.Format("{0}-{1}没有设置主键", db_table.caption, db_table.Name)));


                if (clearDataFirst)
                {
                    db.ExecSqlString("delete from `" + dtable.TableName + "`");
                }
                try
                {
                    DataTable 现在数据库DataTable = null;
                    try
                    {
                        现在数据库DataTable = db.SelectTable("select * from `" + dtable.TableName + "` limit 0,0");
                    }
                    catch (Exception ex)
                    {
                        continue;
                    }
                    foreach (DataRow row in dtable.Rows)
                    {

                        if (db.ExecSqlString("select 1 from `" + dtable.TableName + "` where `" + pkcolumn.Name + "`=" + row[pkcolumn.Name]) != null)
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
                                str_fields.Append("`" + dtable.Columns[columnindex].ColumnName + "`=@p"+pIndex);
                                values.Add(row[columnindex]);
                                pIndex++;

                            }
                            values.Add(row[pkcolumn.Name]);
                            db.ExecSqlString("update `" + dtable.TableName + "` set " + str_fields + " where `" + pkcolumn.Name + "`=@p" + pIndex,  values.ToArray());
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
                                str_fields.Append("`" + dtable.Columns[columnindex].ColumnName + "`");

                                if (str_values.Length > 0)
                                    str_values.Append(',');
                                str_values.Append("@p" + pIndex);
                                values.Add(row[columnindex]);
                                pIndex++;
                            }
                            db.ExecSqlString("insert into `" + dtable.TableName + "` (" + str_fields + ") values ("+str_values+")", values.ToArray());
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw (ex);
                }
            }
            #endregion
        }


        public string GetObjectFormat()
        {
            return "`{0}`";
        }
    }
}