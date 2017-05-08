using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;


namespace Way.EntityDB.Design.Database.PostgreSQL
{
    //在mysql 5.7.18 版本测试
    [EntityDB.Attributes.DatabaseTypeAttribute(DatabaseType.PostgreSql)]
    class PostgreSQLTableService : Services.ITableDesignService
    {
        static List<string> ColumnType = new List<string>(new string[] {
                                            "varchar",
                                            "integer",
                                            "oid",//image
                                            "text",
                                            "smallint",
                                            "date",//smalldatetime
                                            "real",
                                            "timestamp",//datetime
                                            "float",
                                            "double",
                                            "boolean",
                                            "decimal",
                                            "numeric",
                                            "bigint",
                                            "bytea",//varbinary
                                            "char",
                                            "timestamp", });
        string getSqlType(EJ.DBColumn column)
        {
            string dbtype = column.dbType.ToLower();
            int index = Design.ColumnType.SupportTypes.IndexOf(dbtype);
            if (index < 0 || ColumnType[index] == null)
                throw new Exception($"不支持字段类型{dbtype}");
            return ColumnType[index];
        }

        public void CreateTable(EntityDB.IDatabaseService db, EJ.DBTable table, EJ.DBColumn[] columns, IndexInfo[] indexInfos)
        {
          
            //db.ExecSqlString("drop table if exists " + table.Name + "");

                string sqlstr;
                sqlstr = @"
CREATE TABLE " + table.Name + @" (
";

                for (int i = 0; i < columns.Length; i++)
                {
                    var column = columns[i];
                    if (i > 0)
                        sqlstr += ",\r\n";
                    string sqltype = getSqlType(column);
                    if ( string.IsNullOrEmpty( column.length) == false)
                    {
                        if (sqltype.Contains("("))
                            sqltype = sqltype.Substring(0,sqltype.IndexOf("("));
                        sqltype += "(" + column.length + ")";
                    }
                    sqlstr += "" + column.Name + " " + sqltype;
                   
                    if (column.CanNull == false || column.IsPKID == true || column.IsAutoIncrement == true)
                        sqlstr += " NOT";
                    sqlstr += " NULL ";

                    if (column.IsPKID == true)
                    {
                        sqlstr += " PRIMARY KEY ";
                    }

                    if (!string.IsNullOrEmpty(column.defaultValue))
                    {
                        string defaultValue = column.defaultValue.Trim();
                        if ((defaultValue.Length > 1 && defaultValue.StartsWith("'") && defaultValue.EndsWith("'")) || defaultValue.Contains("()"))
                        {
                            sqlstr += " DEFAULT " + defaultValue ;
                        }
                        else
                        {
                            sqlstr += " DEFAULT '" + defaultValue + "'";
                        }
                    }
                    

                }



                sqlstr += ")";

                db.ExecSqlString(sqlstr);

           
            foreach ( var column in columns )
            {
                if(column.IsAutoIncrement == true)
                {
                    setColumn_IsAutoIncrement(db, column, table.Name, true);
                }
            }

            if (indexInfos != null && indexInfos.Length > 0)
            {
                foreach (var config in indexInfos)
                {
                    createIndex(db, table.Name, config);
                }
            }

        }

        void setColumn_IsAutoIncrement( IDatabaseService db , EJ.DBColumn column ,string table,  bool isAutoIncrement)
        {
            /*
             先创建一张表，再创建一个序列，然后将表主键ID的默认值设置成这个序列的NEXT值
             SELECT c.relname FROM pg_class c WHERE c.relkind = 'S';可以查看所有SEQUENCE
             */
            if (isAutoIncrement)
            {
                db.ExecSqlString($@"
CREATE SEQUENCE {table}_{column.Name}_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;
alter table {table} alter column {column.Name} set default nextval('{table}_{column.Name}_seq');
");
            }
            else
            {
                //查找是哪个sequence
                //select column_name,data_type,column_default,is_nullable from information_schema.columns where table_name = 'tbl_role';
                var seqName = db.ExecSqlString($"select column_default from information_schema.columns where table_name = '{table}' and column_name='{column.Name}'").ToSafeString();
                Match m = Regex.Match(seqName, @"nextval\(\'(?<n>(\w)+)\'");
                if (m != null && m.Length > 0)
                {
                    seqName = m.Groups["n"].Value;
                    db.ExecSqlString($"alter table {table} ALTER COLUMN {column.Name} DROP DEFAULT");
                    db.ExecSqlString($"DROP SEQUENCE IF EXISTS  {seqName}");
                }
            }
        }

        public void DeleteTable(EntityDB.IDatabaseService database, string tableName)
        {
            database.ExecSqlString(string.Format("DROP TABLE IF EXISTS {0}", tableName));
        }
               


        List<string> checkIfIdxChanged(EntityDB.IDatabaseService database, string tablename,string pkid, List<IndexInfo> indexInfos)
        {

            List<string> needToDels = new List<string>();
            var indexTable = database.SelectTable($"select * from pg_indexes where tablename='{tablename}' and schemaname='public'");
            foreach( var row in indexTable.Rows )
            {
                var indexname = row["indexname"].ToSafeString();
                var indexdef = row["indexdef"].ToSafeString();
                Match ms = Regex.Match(indexdef, @"btree( )?\((?<columns>(\w| |,)+)\)");
                var t_columns = ms.Groups["columns"].Value.Split(',');
                var columns = (from m in t_columns
                               where m.Trim().Length > 0
                           select m.Trim().Split(' ')[0]).OrderBy(m=>m).ToArray();
                var isClustered = indexdef.Contains(" NULLS FIRST");
                var isUnique = indexdef.StartsWith("CREATE UNIQUE ");
                string name = columns.ToSplitString(",");
                if (name == pkid)
                    continue;
                if (indexInfos.Any(m => m.ColumnNames.OrderBy(n => n).ToArray().ToSplitString(",") == name) == false)
                {
                    needToDels.Add(indexname);
                }
                else {
                    var existIndexes = indexInfos.Where(m => m.IsUnique == isUnique && m.IsClustered == isClustered && m.ColumnNames.OrderBy(n => n).ToArray().ToSplitString(",") == name).ToArray();
                    if (existIndexes.Length == 0)
                    {
                        needToDels.Add(indexname);
                    }
                    else
                    {
                        //现有的索引是一样的，所以不用创建了
                        foreach( var indexinfo in existIndexes )
                            indexInfos.Remove(indexinfo);
                    }
                }
            }

            return needToDels;
        }
        void deletecolumn(EntityDB.IDatabaseService database, string table, string column)
        {
            table = table.ToLower();
            column = column.ToLower();
            database.ExecSqlString(string.Format("alter table {0} drop column {1}" , table , column));
        }
        void dropTableIndex(EntityDB.IDatabaseService database, string table, string indexName)
        {
            table = table.ToLower();
            database.ExecSqlString("ALTER TABLE " + table + " DROP CONSTRAINT IF EXISTS " + indexName + "");
            database.ExecSqlString("DROP INDEX IF EXISTS " + indexName + "");
        }
        public void ChangeTable(EntityDB.IDatabaseService database, string oldTableName, string newTableName, EJ.DBColumn[] addColumns, EJ.DBColumn[] changedColumns, EJ.DBColumn[] deletedColumns, EJ.DBColumn[] otherColumns, IndexInfo[] _indexInfos)
        {
            List<IndexInfo> indexInfos = new List<IndexInfo>(_indexInfos);

            //先判断表明是否更改
            if (oldTableName.ToLower() != newTableName.ToLower())
            {
                //更改表名
                database.ExecSqlString($"alter table {oldTableName} RENAME TO {newTableName}");
            }
            string pkField = null;
            var pkColumn = otherColumns.FirstOrDefault(m=>m.IsPKID == true);
            if(pkColumn == null)
            {
                pkColumn = changedColumns.FirstOrDefault(m => m.IsPKID == true);
                if(pkColumn != null && pkColumn.BackupChangedProperties["Name"] != null)
                {
                    pkField = pkColumn.BackupChangedProperties["Name"].OriginalValue as string;
                }
            }
            if (pkField == null && pkColumn != null)
                pkField = pkColumn.Name;
            var needToDels = checkIfIdxChanged(database, newTableName, pkField, indexInfos);

            foreach (var column in deletedColumns)
            {
                deletecolumn(database, newTableName, column.Name);
            }

            foreach (string delIndexName in needToDels)
                dropTableIndex(database, newTableName, delIndexName);

            foreach (var column in changedColumns)
            {
                string sqltype = getSqlType(column);
                if (column.length.IsNullOrEmpty() == false)
                {
                    if (sqltype.Contains("("))
                        sqltype = sqltype.Substring(0, sqltype.IndexOf("("));
                    sqltype += "(" + column.length + ")";
                }

                int changeColumnCount = 0;
                var changeitem = column.BackupChangedProperties["Name"];
                if (changeitem != null)
                {
                    changeColumnCount++;

                    #region 改名
                    database.ExecSqlString($"alter table {newTableName} rename {changeitem.OriginalValue} to {column.Name}");
                    #endregion
                }

                changeitem = column.BackupChangedProperties["IsAutoIncrement"];
                if (changeitem != null)
                {
                    changeColumnCount++;

                    #region 变更自增长
                    setColumn_IsAutoIncrement(database, column, newTableName, column.IsAutoIncrement.Value);

                    #endregion
                }

                changeitem = column.BackupChangedProperties["IsPKID"];
                if (changeitem != null)
                {
                    changeColumnCount++;

                    #region 变更主键
                    if (column.IsPKID == false)
                    {
                        //去除主键;//删除主建
                        //var pkeyIndexName = database.ExecSqlString($"select indexname from pg_indexes where tablename='{newTableName}' and indexname='{oldTableName}_pkey'").ToSafeString();
                        //if (pkeyIndexName.Length > 0)
                        //{
                        //    database.ExecSqlString($"ALTER TABLE {newTableName} DROP CONSTRAINT IF EXISTS {oldTableName}_pkey");
                        //    database.ExecSqlString($"DROP INDEX IF EXISTS {oldTableName}_pkey");
                        //}

                        database.ExecSqlString($"ALTER TABLE {newTableName} DROP CONSTRAINT IF EXISTS {oldTableName}_pkey");
                        database.ExecSqlString($"DROP INDEX IF EXISTS {oldTableName}_pkey");
                    }
                    else
                    {
                        //设为主键;
                        database.ExecSqlString($"ALTER TABLE {newTableName} ADD PRIMARY KEY ({column.Name})");
                    }
                    #endregion
                }


                bool defaultvalueChanged = false;
                changeitem = column.BackupChangedProperties["defaultValue"];
                if (changeitem != null)
                {
                    defaultvalueChanged = true;
                    changeColumnCount++;

                    #region 默认值
                    //删除默认值
                    database.ExecSqlString($"alter table {newTableName} ALTER COLUMN {column.Name} DROP DEFAULT");

                    #endregion
                }

                if (column.BackupChangedProperties["dbType"] != null || column.BackupChangedProperties["length"] != null)
                {
                    changeColumnCount++;

                    #region 变更类型
                    sqltype = getSqlType(column);
                    string sql = $"alter table {newTableName} ALTER COLUMN {column.Name} TYPE {(sqltype + (column.length.IsNullOrEmpty()?"":$"({column.length})"))} using {column.Name}::{sqltype}";
                    database.ExecSqlString(sql);

                    #endregion
                }
                   if (column.BackupChangedProperties["CanNull"] != null)
                  {
                    #region CanNull更改了
                    if (column.CanNull == false && !string.IsNullOrEmpty(column.defaultValue))
                    {
                        string defaultValue = column.defaultValue.Trim();

                        if (defaultValue.Length > 1 && defaultValue.StartsWith("'") && defaultValue.EndsWith("'"))
                        {
                        }
                        else
                        {
                            if (defaultValue.Contains("()"))
                            {
                            }
                            else
                            {
                                defaultValue = "'" + defaultValue + "'";
                            }
                        }

                        database.ExecSqlString($"update {newTableName} set {column.Name}={defaultValue} where {column.Name} is null");
                    }

                    
                    string sql = $"alter table {newTableName} ALTER COLUMN {column.Name} {(column.CanNull.Value ? "drop not null" : "set not null")}";
                    database.ExecSqlString(sql);
                    #endregion
                }


                #region 设置默认值
                if (defaultvalueChanged && !string.IsNullOrEmpty(column.defaultValue))
                {
                    string sql = "";
                    string defaultValue = column.defaultValue.Trim();
                    if (defaultValue.Length > 1 && defaultValue.StartsWith("'") && defaultValue.EndsWith("'"))
                    {
                        sql += $"alter table {newTableName} ALTER COLUMN {column.Name} SET DEFAULT {defaultValue}";
                    }
                    else
                    {
                        sql += $"alter table {newTableName} ALTER COLUMN {column.Name} SET DEFAULT '{defaultValue}'";

                    }
                    if (sql.Length > 0)
                        database.ExecSqlString(sql);


                    if (defaultValue.Length > 1 && defaultValue.StartsWith("'") && defaultValue.EndsWith("'"))
                    {
                    }
                    else
                    {
                        if (defaultValue.Contains("()"))
                        {
                        }
                        else
                        {
                            defaultValue = "'" + defaultValue + "'";
                        }
                    }

                    database.ExecSqlString("update " + newTableName + " set " + column.Name + "=" + defaultValue + " where " + column.Name + " is null");
                }
                #endregion
            }

            foreach (var column in addColumns)
            {
                string sqltype = getSqlType(column);
                if (column.length.IsNullOrEmpty() == false)
                {
                    if (sqltype.Contains("("))
                        sqltype = sqltype.Substring(0, sqltype.IndexOf("("));
                    sqltype += "(" + column.length + ")";
                }
                #region 新增字段
                string sql = "alter table " + newTableName + " add COLUMN " + column.Name + " " + sqltype;


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
                        sql += " default " + defaultValue ;
                    }
                    else
                    {
                        sql += " default '" + defaultValue + "'";
                    }


                }
                database.ExecSqlString(sql);

                if (column.IsPKID == true)
                {
                    database.ExecSqlString($"ALTER TABLE {newTableName} ADD CONSTRAINT {newTableName}_pkey PRIMARY KEY({column.Name})");
                }
                #endregion
            }

            foreach (var config in indexInfos)
            {
                createIndex(database, newTableName, config);
            }
        }

        void createIndex(EntityDB.IDatabaseService database, string table, IndexInfo indexinfo)
        {
            //alter table table_name add unique key new_uk_name (col1,col2);
            var columns = indexinfo.ColumnNames.OrderBy(m => m).ToArray();
            string name = table + "_ej_" + columns.ToSplitString("_");

            string sql = "CREATE ";
            if (indexinfo.IsUnique)
            {
                sql += "UNIQUE ";
            }
            if (indexinfo.IsClustered)
            {
                sql += $"INDEX ON {table} ({columns.ToSplitString(" ASC NULLS FIRST,")} ASC NULLS FIRST)";
            }
            else
            {
                sql += $"INDEX ON {table} ({columns.ToSplitString(",")})";
            }
            database.ExecSqlString(sql);
        }
    }
}