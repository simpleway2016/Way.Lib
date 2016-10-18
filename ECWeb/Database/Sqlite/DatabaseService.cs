using ECWeb.Database.Services;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;

namespace ECWeb.Database.Sqlite
{
    public class DatabaseService : IDatabaseService
    {

        public void Create(EJ.Databases database)
        {

            var db = EntityDB.DBContext.CreateDatabaseService(database.conStr, EntityDB.DatabaseType.Sqlite);
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
                db.ExecSqlString("CREATE TABLE [__WayEasyJob](contentConfig TEXT  NOT NULL)");
                var dbconfig = new DataBaseConfig();
                try
                {
                    dbconfig.LastUpdatedID = Convert.ToInt32( db.ExecSqlString("select lastID from __EasyJob"));
                }
                catch
                {
                }
                db.ExecSqlString("insert into __WayEasyJob (contentConfig) values (@p0)", dbconfig.ToJsonString());
               
            }

            //try
            //{
            //    db.ExecSqlString("CREATE TABLE  [] ([lastID] integer  NOT NULL)");
            //    db.ExecSqlString("insert into  (lastID) values (0)");
            //}
            //catch
            //{
            //}
        }

        public void ChangeName(EJ.Databases database, string newName, string newConnectString)
        {
            var dbnameMatch_old = System.Text.RegularExpressions.Regex.Match(database.conStr, @"Data Source=(?<dname>(\w|\:|\\)+)", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            var dbnameMatch = System.Text.RegularExpressions.Regex.Match(newConnectString, @"Data Source=(?<dname>(\w|\:|\\)+)", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            string newfilepath = dbnameMatch.Groups["dname"].Value;
            string oldfilepath = dbnameMatch_old.Groups["dname"].Value;
            System.IO.File.Move(oldfilepath, newfilepath);
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
                    db.ExecSqlString("delete from [" + dtable.TableName + "]");
                }
                try
                {
                    DataTable 现在数据库DataTable = null;
                    try
                    {
                        现在数据库DataTable = db.SelectTable("select * from [" + dtable.TableName + "] limit 0,0");
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
            }
            #endregion
        }



        public string GetObjectFormat()
        {
            return "[{0}]";
        }
    }
}