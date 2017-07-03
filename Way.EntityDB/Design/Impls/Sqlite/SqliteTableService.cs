using Way.EntityDB.Design.Services;
using Way.EntityDB.Design.Services;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;


namespace Way.EntityDB.Design.Database.Sqlite
{
    [EntityDB.Attributes.DatabaseTypeAttribute(DatabaseType.Sqlite)]
    class SqliteTableService : ITableDesignService
    {
        internal static List<string> ColumnType = new List<string>(new string[] {
                                            "VARCHAR",
                                            "INTEGER",
                                            "BLOB",//image
                                            "TEXT",
                                            "SMALLINT",//smallint
                                            "DATETIME",//smalldatetime
                                            "REAL",
                                            "DATETIME",//datetime
                                            "FLOAT",
                                            "DOUBLE",
                                            "BOOLEAN",
                                            "DECIMAL",//decimal
                                            "NUMERIC",
                                            "BIGINT",//bigint
                                            "BLOB",//varbinary
                                            "CHARACTER",
                                            "DATETIME", });
        string getSqlType(EJ.DBColumn column)
        {
            //serial不是一种自增长类型，只是一个宏，所以不要用serial
            string dbtype = column.dbType.ToLower();
            int index = Design.ColumnType.SupportTypes.IndexOf(dbtype);
            if (index < 0 || ColumnType[index] == null)
                throw new Exception($"不支持字段类型{dbtype}");
            return ColumnType[index];
        }

        string getSqliteType(EJ.DBColumn column)
        {
            var dbtype = getSqlType(column);
            if (!column.length.IsNullOrEmpty())
                dbtype += "(" + column.length + ")";
            if( dbtype.Contains("TEXT") || dbtype.Contains("CHAR") )
            {      // COLLATE NOCASE查询时，不区分大小写
                dbtype += " COLLATE NOCASE";
            }

            return dbtype;
        }

        public void CreateTable(EntityDB.IDatabaseService db, EJ.DBTable table, EJ.DBColumn[] columns, IndexInfo[] indexInfos)
        {


                string sqlstr;
                sqlstr = @"
CREATE TABLE [" + table.Name.ToLower() + @"] (
";

                for (int i = 0; i < columns.Length; i ++ )
                {
                    var column = columns[i];
                    if (i > 0)
                        sqlstr += ",\r\n";

                    sqlstr += "[" + column.Name.ToLower() + "] " + getSqliteType(column) + "";

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
                        sqlstr += " DEFAULT '" + defaultValue.Replace("'","''") + "'";
                    }


                }


                sqlstr += ")";


                db.ExecSqlString(sqlstr);

                if (indexInfos != null && indexInfos.Length > 0)
                {
                    foreach (var config in indexInfos)
                    {
                        string keyname = table.Name.ToLower() + "_ej_" + config.ColumnNames.OrderBy(m => m).ToArray().ToSplitString("_").ToLower();
                        string type = "";
                        if (config.IsUnique || config.IsClustered)
                        {
                            type += "UNIQUE ";
                        }
                        //if (config.IsClustered)
                        //{
                        //    throw new Exception("sqlite暂不支持定义聚集索引");
                        //}
                        db.ExecSqlString("CREATE " + type + " INDEX " + keyname + " ON [" + table.Name.ToLower() + "](" + config.ColumnNames.OrderBy(m => m).ToArray().ToSplitString().ToLower() + ")");
                        //CREATE UNIQUE  INDEX index_t1 ON t1(a, b, c};

                       // 第二种：

                        // CREATE UNIQUE INDEX index_a_t1 ON t1(a);
                        //DROP INDEX IF EXISTS testtable_idx;
                    }
                }
            
        }

        List<string> checkIfIdxChanged(EntityDB.IDatabaseService database, string tablename,List<IndexInfo> indexInfos)
        {
            tablename = tablename.ToLower();
            List<string> need2Dels = new List<string>();
            using (var dt = database.SelectTable("select * from sqlite_master WHERE type='index' and tbl_name='" + tablename + "'"))
                {
                    foreach (var drow in dt.Rows)
                    {
                        bool isUnique = drow["sql"].ToString().Contains(" UNIQUE ");
                        string name = drow["name"].ToString().ToLower();
                        var exitsItem = indexInfos.FirstOrDefault(m => tablename + "_ej_" + m.ColumnNames.OrderBy(p => p).ToArray().ToSplitString("_").ToLower() == name && m.IsUnique == isUnique);
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
            tableName = tableName.ToLower();
               name = name.ToLower();
            database.ExecSqlString("DROP INDEX IF EXISTS [" + name + "]");
        }
        void deleteAllIndex(EntityDB.IDatabaseService database, string tableName )
        {
            tableName = tableName.ToLower();
            using (var dtable = database.SelectTable("select * from sqlite_master where type='index' and tbl_name='" + tableName + "' "))
            {
                foreach (var drow in dtable.Rows)
                {
                    database.ExecSqlString("DROP INDEX IF EXISTS [" + drow["name"] + "]");
                }
            }
        }
        public void ChangeTable(EntityDB.IDatabaseService database, string oldTableName, string newTableName, EJ.DBColumn[] addColumns, EJ.DBColumn[] changedColumns, EJ.DBColumn[] deletedColumns, EJ.DBColumn[] otherColumns, IndexInfo[] _indexInfos)
        {
            oldTableName = oldTableName.ToLower();
            newTableName = newTableName.ToLower();

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
                    foreach (var c in dtable.Columns)
                    {
                        if (deletedColumns.Count(m => m.Name.ToLower() == c.ColumnName.ToLower()) == 0)
                        {
                            var newName = changedColumns.FirstOrDefault(m => m.BackupChangedProperties.Count(p => p.Key == "Name" && ((DataValueChangedItem)p.Value).OriginalValue.ToSafeString().ToLower() == c.ColumnName.ToLower()) > 0);
                            oldColumnNames.Add("[" + c.ColumnName.ToLower() + "]");
                            if (newName != null)
                                newColumnNames.Add("[" + newName.Name.ToLower() + "]");
                            else
                                newColumnNames.Add("[" + c.ColumnName.ToLower() + "]");
                        }
                    }
                }
                string oldfields = oldColumnNames.ToArray().ToSplitString();
                string newfields = newColumnNames.ToArray().ToSplitString();
                if (oldColumnNames.Count > 0)
                {
                    //把旧表数据拷贝到新表
                    database.ExecSqlString("insert into [" + newTableName.ToLower() + "] (" + newfields + ") select " + oldfields + " from [" + oldTableName.ToLower() + "]");
                }

                database.ExecSqlString("DROP TABLE [" + oldTableName.ToLower() + "]");
            }
            else
            {
                
               var need2dels =  checkIfIdxChanged(database, oldTableName.ToLower(), indexInfos);
               foreach( string delName in need2dels )
                   deleteIndex(database, oldTableName.ToLower(), delName.ToLower());

                if(oldTableName.ToLower() != newTableName.ToLower())
                database.ExecSqlString("ALTER TABLE [" + oldTableName.ToLower() + "] RENAME TO [" + newTableName.ToLower() + "]");

                foreach (var column in addColumns)
                {
                    #region 新增字段
                    string sql = "alter table [" + newTableName.ToLower() + "] add [" + column.Name.ToLower() + "] " + getSqliteType( column) ;

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
                        sql += " DEFAULT '" + defaultValue.Replace("'","''") + "'";
                    }
                    database.ExecSqlString(sql);

                    #endregion
                }

                foreach (var config in indexInfos)
                {
                    string keyname = newTableName.ToLower() + "_ej_" + config.ColumnNames.OrderBy(m => m).ToArray().ToSplitString("_").ToLower();
                    string type = "";
                    if (config.IsUnique || config.IsClustered)
                    {
                        type += "UNIQUE ";
                    }
                    //if (config.IsClustered)
                    //{
                    //    throw new Exception("sqlite暂不支持定义聚集索引");
                    //}

                    database.ExecSqlString("CREATE " + type + " INDEX " + keyname + " ON [" + newTableName.ToLower() + "](" + config.ColumnNames.OrderBy(m => m).ToArray().ToSplitString().ToLower() + ")");
                }
            }
        }

        public void DeleteTable(EntityDB.IDatabaseService database, string tableName)
        {
            database.ExecSqlString("DROP TABLE [" + tableName.ToLower() + "]");
        }
    }
}