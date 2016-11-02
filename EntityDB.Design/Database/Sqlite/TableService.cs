using EntityDB.Design.Database.Services;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace EntityDB.Design.Database.Sqlite
{
    public class TableService : ITableDesignService
    {
        string getSqliteType(string dbtype)
        {
            dbtype = dbtype.ToLower();

            if (dbtype.Contains("char"))
                return "TEXT";

            if (dbtype.Contains("int"))
                return "INTEGER";

            if( dbtype.Contains("real") )
                return "REAL";
            if( dbtype.Contains("double") )
                return "REAL";
            if( dbtype.Contains("float") )
                return "REAL";

             if( dbtype.Contains("numeric") )
                return "NUMERIC";
             if( dbtype.Contains("decimal") )
                return "NUMERIC";
             if( dbtype.Contains("boolean") )
                return "NUMERIC";
             if( dbtype.Contains("date") )
                 return "datetime";
             if (dbtype.Contains("byte"))
                 return "BLOB";
             if (dbtype.Contains("image"))
                 return "BLOB";
             if (dbtype.Contains("binary"))
                 return "BLOB";
            return dbtype;
        }

        public void CreateTable(EntityDB.IDatabaseService db, EJ.DBTable table, EJ.DBColumn[] columns, IndexInfo[] indexInfos)
        {

            using (CLog log = new CLog(""))
            {
                string sqlstr;
                sqlstr = @"
CREATE TABLE [" + table.Name + @"] (
";

                for (int i = 0; i < columns.Length; i ++ )
                {
                    var column = columns[i];
                    if (i > 0)
                        sqlstr += ",\r\n";

                    sqlstr += "[" + column.Name + "] " + getSqliteType(column.dbType) + "";

                    if (column.IsPKID == true)
                    {
                        sqlstr += "  PRIMARY KEY ";
                    }
                    if (column.IsAutoIncrement == true)
                    {
                        sqlstr += "  AUTOINCREMENT ";
                    }
                    if (column.CanNull == false || column.IsPKID == true || column.IsAutoIncrement == true)
                        sqlstr += " NOT";
                    sqlstr += " NULL ";


                    if (!string.IsNullOrEmpty(column.defaultValue))
                    {
                        string defaultValue = column.defaultValue.Trim();
                        if ((defaultValue.Length > 1 && defaultValue.StartsWith("'") && defaultValue.EndsWith("'")) || defaultValue.Contains("()"))
                        {
                            sqlstr += " DEFAULT " + defaultValue + "";
                        }
                        else
                        {
                            sqlstr += " DEFAULT '" + defaultValue + "'";
                        }
                    }


                }


                sqlstr += ")";

                log.Log(sqlstr);
                db.ExecSqlString(sqlstr);

                if (indexInfos != null && indexInfos.Length > 0)
                {
                    foreach (var config in indexInfos)
                    {
                        string keyname = table.Name + "_ej_" + config.ColumnNames.OrderBy(m => m).ToArray().ToSplitString("_");
                        string type = "";
                        if (config.IsUnique)
                        {
                            type += "UNIQUE ";
                        }
                        if (config.IsClustered)
                        {
                            throw new Exception("sqlite暂不支持定义聚集索引");
                        }
                        db.ExecSqlString("CREATE " + type + " INDEX " + keyname + " ON [" + table.Name + "](" + config.ColumnNames.OrderBy(m => m).ToArray().ToSplitString() + ")");
                        //CREATE UNIQUE  INDEX index_t1 ON t1(a, b, c};

                       // 第二种：

                        // CREATE UNIQUE INDEX index_a_t1 ON t1(a);
                        //DROP INDEX IF EXISTS testtable_idx;
                    }
                }
            }
        }

        List<string> checkIfIdxChanged(EntityDB.IDatabaseService database, string tablename,List<IndexInfo> indexInfos)
        {
            List<string> need2Dels = new List<string>();
            using (DataTable dt = database.SelectTable("select * from sqlite_master WHERE type='index' and tbl_name='" + tablename + "'"))
                {
                    foreach (DataRow drow in dt.Rows)
                    {
                        bool isUnique = drow["sql"].ToString().Contains(" UNIQUE ");
                        string name = drow["name"].ToString();
                        var exitsItem = indexInfos.FirstOrDefault(m => tablename + "_ej_" + m.ColumnNames.OrderBy(p => p).ToArray().ToSplitString("_") == name && m.IsUnique == isUnique);
                        if (exitsItem == null)
                        {
                            need2Dels.Add(name);
                        }
                        else
                        {
                            indexInfos.Remove(exitsItem);
                        }
                    }
                }
            return need2Dels;
        }
        /// <summary>
        /// 删除表所有索引
        /// </summary>
        /// <param name="database"></param>
        /// <param name="tableName"></param>
        void deleteIndex(EntityDB.IDatabaseService database, string tableName,string name)
        {
            database.ExecSqlString("DROP INDEX IF EXISTS [" + name + "]");
        }
        void deleteAllIndex(EntityDB.IDatabaseService database, string tableName )
        {
            using (var dtable = database.SelectTable("select * from sqlite_master where type='index' and tbl_name='" + tableName + "' "))
            {
                foreach (System.Data.DataRow drow in dtable.Rows)
                {
                    database.ExecSqlString("DROP INDEX IF EXISTS [" + drow["name"] + "]");
                }
            }
        }
        public void ChangeTable(EntityDB.IDatabaseService database, string oldTableName, string newTableName, EJ.DBColumn[] addColumns, EJ.DBColumn[] changedColumns, EJ.DBColumn[] deletedColumns, EJ.DBColumn[] otherColumns, IndexInfo[] _indexInfos)
        {
            List<IndexInfo> indexInfos = new List<IndexInfo>(_indexInfos);
            bool needToDeleteTable = (deletedColumns.Length > 0 || changedColumns.Length > 0);
            if (needToDeleteTable)
            {
                string changetoname = oldTableName + "_2";
                while (Convert.ToInt32(database.ExecSqlString("select count(*) from sqlite_master where type='table' and name='" + changetoname + "'")) > 0)
                    changetoname = changetoname + "_2";

                //删除索引
                deleteAllIndex(database, oldTableName);

                database.ExecSqlString("ALTER TABLE [" + oldTableName + "] RENAME TO [" + changetoname + "]");
                oldTableName = changetoname;


                EJ.DBTable dt = new EJ.DBTable()
                    {
                        Name = newTableName,
                    };
                //PRAGMA table_info([project]) 用name type
                List<EJ.DBColumn> allColumns = new List<EJ.DBColumn>();
                allColumns.AddRange(otherColumns);
                allColumns.AddRange(addColumns);
                allColumns.AddRange(changedColumns);

                CreateTable(database, dt, allColumns.ToArray(), _indexInfos);

                //获取原来所有字段
                List<string> oldColumnNames = new List<string>();
                List<string> newColumnNames = new List<string>();
                using (var dtable = database.SelectTable("select * from [" + oldTableName + "] limit 0,1"))
                {
                    foreach (System.Data.DataColumn c in dtable.Columns)
                    {
                        if (deletedColumns.Count(m => m.Name.ToLower() == c.ColumnName.ToLower()) == 0)
                        {
                            var newName = changedColumns.FirstOrDefault(m => m.BackupChangedProperties.Count(p => p.PropertyName == "Name" && p.OriginalValue.ToSafeString().ToLower() == c.ColumnName.ToLower()) > 0);
                            oldColumnNames.Add("[" + c.ColumnName + "]");
                            if (newName != null)
                                newColumnNames.Add("[" + newName.Name + "]");
                            else
                                newColumnNames.Add("[" + c.ColumnName + "]");
                        }
                    }
                }
                string oldfields = oldColumnNames.ToArray().ToSplitString();
                string newfields = newColumnNames.ToArray().ToSplitString();
                if (oldColumnNames.Count > 0)
                {
                    //把旧表数据拷贝到新表
                    database.ExecSqlString("insert into [" + newTableName + "] (" + newfields + ") select " + oldfields + " from [" + oldTableName + "]");
                }

                database.ExecSqlString("DROP TABLE [" + oldTableName + "]");
            }
            else
            {
                
               var need2dels =  checkIfIdxChanged(database, oldTableName, indexInfos);
               foreach( string delName in need2dels )
                   deleteIndex(database, oldTableName, delName);

                if(oldTableName != newTableName)
                database.ExecSqlString("ALTER TABLE [" + oldTableName + "] RENAME TO [" + newTableName + "]");

                foreach (var column in addColumns)
                {
                    #region 新增字段
                    string sql = "alter table [" + newTableName + "] add [" + column.Name + "] [" + getSqliteType( column.dbType) + "]";

                    if (column.IsPKID == true)
                    {
                        sql += "  PRIMARY KEY ";
                    }
                    if (column.IsAutoIncrement == true)
                    {
                        sql += "  AUTOINCREMENT ";
                    }

                    if (column.CanNull == false || column.IsPKID == true || column.IsAutoIncrement == true)
                    {
                        sql += " NOT";
                    }
                    sql += " NULL  ";
                    if (!string.IsNullOrEmpty(column.defaultValue))
                    {
                        string defaultValue = column.defaultValue.Trim();
                        if ((defaultValue.Length > 1 && defaultValue.StartsWith("'") && defaultValue.EndsWith("'")) || defaultValue.Contains("()"))
                        {
                            sql += " DEFAULT " + defaultValue + "";
                        }
                        else
                        {
                            sql += " DEFAULT '" + defaultValue + "'";
                        }
                    }
                    database.ExecSqlString(sql);

                    #endregion
                }

                foreach (var config in indexInfos)
                {
                    string keyname = newTableName + "_ej_" + config.ColumnNames.OrderBy(m => m).ToArray().ToSplitString("_");
                    string type = "";
                    if (config.IsUnique)
                    {
                        type += "UNIQUE ";
                    }
                    if (config.IsClustered)
                    {
                        throw new Exception("sqlite暂不支持定义聚集索引");
                    }

                    database.ExecSqlString("CREATE " + type + " INDEX " + keyname + " ON [" + newTableName + "](" + config.ColumnNames.OrderBy(m => m).ToArray().ToSplitString() + ")");
                }
            }
        }

        public void DeleteTable(EntityDB.IDatabaseService database, string tableName)
        {
            database.ExecSqlString("DROP TABLE [" + tableName + "]");
        }
    }
}