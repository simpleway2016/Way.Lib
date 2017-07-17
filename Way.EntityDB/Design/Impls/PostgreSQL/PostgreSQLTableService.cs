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
       internal static List<string> ColumnType = new List<string>(new string[] {
                                            "character varying",
                                            "integer",
                                            "oid",//image
                                            "text",
                                            "smallint",
                                            "date",//smalldatetime
                                            "real",
                                            "timestamp without time zone",//datetime
                                            "float",
                                            "double",
                                            "boolean",
                                            "numeric",//decimal
                                            "numeric",
                                            "bigint",
                                            "bytea",//varbinary
                                            "character",
                                            "timestamp without time zone", });
        string getSqlType(EJ.DBColumn column)
        {
            //serial不是一种自增长类型，只是一个宏，所以不要用serial
            string dbtype = column.dbType.ToLower();
            int index = Design.ColumnType.SupportTypes.IndexOf(dbtype);
            if (index < 0 || ColumnType[index] == null)
                throw new Exception($"不支持字段类型{dbtype}");
            return ColumnType[index];
        }

       

        public void CreateTable(EntityDB.IDatabaseService db, EJ.DBTable table, EJ.DBColumn[] columns, IndexInfo[] indexInfos)
        {
          
            //db.ExecSqlString("drop table if exists " + table.Name.ToLower() + "");

                string sqlstr;
                sqlstr = @"
CREATE TABLE """ + table.Name.ToLower() + @""" (
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
                    sqlstr += "\"" + column.Name.ToLower() + "\" " + sqltype;
                   
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
                        sqlstr += " DEFAULT '" + defaultValue.Replace("'", "''") + "'";
                    }
                    

                }



                sqlstr += ")";

                db.ExecSqlString(sqlstr);

           
            foreach ( var column in columns )
            {
                if(column.IsAutoIncrement == true)
                {
                    setColumn_IsAutoIncrement(db, column, table.Name.ToLower(), true);
                }
            }

            if (indexInfos != null && indexInfos.Length > 0)
            {
                foreach (var config in indexInfos)
                {
                    createIndex(db, table.Name.ToLower(), config);
                }
            }

        }

        void setColumn_IsAutoIncrement( IDatabaseService db , EJ.DBColumn column ,string table,  bool isAutoIncrement)
        {
            table = table.ToLower();
            /*
             先创建一张表，再创建一个序列，然后将表主键ID的默认值设置成这个序列的NEXT值
             SELECT c.relname FROM pg_class c WHERE c.relkind = 'S';可以查看所有SEQUENCE
             */
            if (isAutoIncrement)
            {
                db.ExecSqlString($"DROP SEQUENCE IF EXISTS {table}_{column.Name.ToLower()}_seq");

                db.ExecSqlString($@"
CREATE SEQUENCE {table}_{column.Name.ToLower()}_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;
alter table ""{table}"" alter column ""{column.Name.ToLower()}"" set default nextval('{table}_{column.Name.ToLower()}_seq');
");
            }
            else
            {
                //查找是哪个sequence
                //select column_name,data_type,column_default,is_nullable,character_maximum_length,character_octet_length from information_schema.columns where table_name = 'tbl_role';
                var seqName = db.ExecSqlString($"select column_default from information_schema.columns where table_name = '{table.ToLower()}' and column_name='{column.Name.ToLower()}'").ToSafeString();
                Match m = Regex.Match(seqName, @"nextval\(\'(?<n>(\w)+)\'");
                if (m != null && m.Length > 0)
                {
                    seqName = m.Groups["n"].Value;
                    db.ExecSqlString($"alter table \"{table}\" ALTER COLUMN \"{column.Name.ToLower()}\" DROP DEFAULT");
                    db.ExecSqlString($"DROP SEQUENCE IF EXISTS  {seqName}");
                }
            }
        }

        public void DeleteTable(EntityDB.IDatabaseService database, string tableName)
        {
            database.ExecSqlString(string.Format("DROP TABLE IF EXISTS \"{0}\"", tableName.ToLower()));
        }
               


        List<string> checkIfIdxChanged(EntityDB.IDatabaseService database, string tablename,List<IndexInfo> indexInfos)
        {
            tablename = tablename.ToLower();
               var tableindexes =  new Impls.PostgreSQL.PostgreSQLDatabaseService().GetCurrentIndexes(database, tablename.ToLower());
            List<string> needToDels = new List<string>();
            foreach( var dbindex in tableindexes)
            {
                string longname = dbindex.ColumnNames.ToArray().ToSplitString(",");
                if (indexInfos.Any(m => m.ColumnNames.OrderBy(n => n).ToArray().ToSplitString(",").ToLower() == longname.ToLower()) == false)
                {
                    needToDels.Add(dbindex.Name.ToLower());
                }
                else {
                    var existIndexes = indexInfos.Where(m => m.IsUnique == dbindex.IsUnique && m.IsClustered == dbindex.IsClustered && m.ColumnNames.OrderBy(n => n).ToArray().ToSplitString(",").ToLower() == longname.ToLower()).ToArray();
                    if (existIndexes.Length == 0)
                    {
                        needToDels.Add(dbindex.Name.ToLower());
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
            database.ExecSqlString(string.Format("alter table \"{0}\" drop column \"{1}\"" , table , column));
        }
        void dropTableIndex(EntityDB.IDatabaseService database, string table, string indexName)
        {
            table = table.ToLower();
            database.ExecSqlString("ALTER TABLE \"" + table.ToLower() + "\" DROP CONSTRAINT IF EXISTS " + indexName.ToLower() + "");
            database.ExecSqlString("DROP INDEX IF EXISTS " + indexName.ToLower() + "");
        }
        public void ChangeTable(EntityDB.IDatabaseService database, string oldTableName, string newTableName, EJ.DBColumn[] addColumns, EJ.DBColumn[] changedColumns, EJ.DBColumn[] deletedColumns, EJ.DBColumn[] otherColumns, IndexInfo[] _indexInfos)
        {
            oldTableName = oldTableName.ToLower();
            newTableName = newTableName.ToLower();
            List<IndexInfo> indexInfos = new List<IndexInfo>(_indexInfos);

            //先判断表明是否更改
            if (oldTableName  != newTableName )
            {
                //更改表名
                database.ExecSqlString($"alter table \"{oldTableName}\" RENAME TO \"{newTableName}\"");
            }
          
            var needToDels = checkIfIdxChanged(database, newTableName, indexInfos);

            foreach (var column in deletedColumns)
            {
                deletecolumn(database, newTableName, column.Name.ToLower());
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
                    database.ExecSqlString($"alter table \"{newTableName}\" rename \"{changeitem.OriginalValue.ToString().ToLower()}\" to \"{column.Name.ToLower()}\"");
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

                        database.ExecSqlString($"ALTER TABLE \"{newTableName}\" DROP CONSTRAINT IF EXISTS {oldTableName}_pkey");
                        database.ExecSqlString($"DROP INDEX IF EXISTS {oldTableName}_pkey");
                    }
                    else
                    {
                        //设为主键;
                        database.ExecSqlString($"ALTER TABLE \"{newTableName}\" ADD PRIMARY KEY (\"{column.Name.ToLower()}\")");
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
                    database.ExecSqlString($"alter table \"{newTableName}\" ALTER COLUMN \"{column.Name.ToLower()}\" DROP DEFAULT");

                    #endregion
                }

                if (column.BackupChangedProperties["dbType"] != null || column.BackupChangedProperties["length"] != null)
                {
                    changeColumnCount++;

                    #region 变更类型
                    sqltype = getSqlType(column);
                    string sql = $"alter table \"{newTableName}\" ALTER COLUMN \"{column.Name.ToLower()}\" TYPE {(sqltype + (column.length.IsNullOrEmpty()?"":$"({column.length})"))} using \"{column.Name.ToLower()}\"::{sqltype}";
                    database.ExecSqlString(sql);

                    #endregion
                }
                   if (column.BackupChangedProperties["CanNull"] != null)
                  {
                    #region CanNull更改了
                    if (column.CanNull == false && !string.IsNullOrEmpty(column.defaultValue))
                    {
                        string defaultValue = column.defaultValue.Trim();

                       

                        database.ExecSqlString($"update \"{newTableName}\" set \"{column.Name.ToLower()}\"='{defaultValue.Replace("'","''")}' where \"{column.Name.ToLower()}\" is null");
                    }

                    
                    string sql = $"alter table \"{newTableName}\" ALTER COLUMN \"{column.Name.ToLower()}\" {(column.CanNull.Value ? "drop not null" : "set not null")}";
                    database.ExecSqlString(sql);
                    #endregion
                }


                #region 设置默认值
                if (defaultvalueChanged && !string.IsNullOrEmpty(column.defaultValue))
                {
                    string sql = "";
                    string defaultValue = column.defaultValue.Trim();
                    sql += $"alter table \"{newTableName}\" ALTER COLUMN \"{column.Name.ToLower()}\" SET DEFAULT '{defaultValue.Replace("'","''")}'";


                    database.ExecSqlString(sql);

                    database.ExecSqlString($"update \"{newTableName}\" set \"{column.Name.ToLower()}\"='{defaultValue.Replace("'","''")}' where \"{column.Name.ToLower()}\" is null");
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
                string sql = "alter table \"" + newTableName + "\" add COLUMN \"" + column.Name.ToLower() + "\" " + sqltype;


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

                if (column.IsPKID == true)
                {
                    database.ExecSqlString($"ALTER TABLE \"{newTableName}\" ADD CONSTRAINT {newTableName}_pkey PRIMARY KEY(\"{column.Name.ToLower()}\")");
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
            table = table.ToLower();
               //alter table table_name add unique key new_uk_name (col1,col2);
               var columns = indexinfo.ColumnNames.OrderBy(m => m).Select(m=>m.ToLower()).ToArray();
            string name = table + "_ej_" + columns.ToSplitString("_");

            string sql = "CREATE ";
            if (indexinfo.IsUnique)
            {
                sql += "UNIQUE ";
            }
            if (indexinfo.IsClustered)
            {
                sql += $"INDEX ON \"{table}\" ({columns.ToSplitString(" ASC NULLS FIRST,")} ASC NULLS FIRST)";
            }
            else
            {
                sql += $"INDEX ON \"{table}\" ({columns.ToSplitString(",")})";
            }
            database.ExecSqlString(sql);
        }
    }
}