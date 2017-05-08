using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;


namespace Way.EntityDB.Design.Database.MySql
{
    //在mysql 5.7.18 版本测试
    [EntityDB.Attributes.DatabaseTypeAttribute(DatabaseType.MySql)]
    class MySqlTableService : Services.ITableDesignService
    {
        static List<string> ColumnType = new List<string>(new string[] {
                                            "varchar",
                                            "int",
                                            "BLOB",//image
                                            "TEXT",//text
                                            "smallint",
                                            "date",//smalldatetime
                                            "real",
                                            "DATETIME",//datetime
                                            "float",
                                            "double",
                                            "BIT",
                                            "decimal",
                                            "numeric",
                                            "bigint",//
                                            "VARBINARY",//varbinary
                                            "char",
                                            "timestamp", });
        string getSqlType(string dbtype)
        {
            dbtype = dbtype.ToLower();
            int index = Design.ColumnType.SupportTypes.IndexOf(dbtype);
            if (index < 0 || ColumnType[index] == null)
                throw new Exception($"不支持字段类型{dbtype}");
            return ColumnType[index];
        }
        
        public void CreateTable(EntityDB.IDatabaseService db, EJ.DBTable table, EJ.DBColumn[] columns, IndexInfo[] indexInfos)
        {
            //db.ExecSqlString("drop table if exists `" + table.Name + "`");

                string sqlstr;
                sqlstr = @"
CREATE TABLE `" + table.Name + @"` (
";

                for (int i = 0; i < columns.Length; i++)
                {
                    var column = columns[i];
                    if (i > 0)
                        sqlstr += ",\r\n";
                    string sqltype = getSqlType(column.dbType);
                    if ( string.IsNullOrEmpty( column.length) == false)
                    {
                        if (sqltype.Contains("("))
                            sqltype = sqltype.Substring(0,sqltype.IndexOf("("));
                        sqltype += "(" + column.length + ")";
                    }
                    sqlstr += "`" + column.Name + "` " + sqltype;
                   
                    if (column.CanNull == false || column.IsPKID == true || column.IsAutoIncrement == true)
                        sqlstr += " NOT";
                    sqlstr += " NULL ";
                    if (column.IsAutoIncrement == true)
                    {
                        sqlstr += "  AUTO_INCREMENT ";
                    }

                    if (!string.IsNullOrEmpty(column.defaultValue))
                    {
                        string defaultValue = column.defaultValue.Trim();
                        sqlstr += " DEFAULT '" + defaultValue.Replace("'","''") + "'";
                        
                    }


                }

                for (int i = 0; i < columns.Length; i++)
                {
                    var column = columns[i];
                    if (column.IsPKID == true)
                    {
                        sqlstr += ",\r\nPRIMARY KEY (`"+ column.Name +"`)";
                    }
                }


                if (indexInfos != null && indexInfos.Length > 0)
                {
                    foreach (var config in indexInfos)
                    {
                        string type = "";
                        if (config.IsUnique || config.IsClustered)
                        {
                            type += "UNIQUE ";
                        }
                        //if (config.IsClustered)
                        //    throw new Exception("MySql不支持定义聚集索引");
                        string keyname = table.Name.ToLower() + "_ej_" + config.ColumnNames.OrderBy(m => m).ToArray().ToSplitString("_");
                        sqlstr += (",\r\n"+type+" KEY `" + keyname + "`(" + config.ColumnNames.OrderBy(m => m).ToArray().ToSplitString("_", "`{0}`") + ")");
                    }
                }

                sqlstr += ") ENGINE=InnoDB DEFAULT CHARSET=utf8";

                db.ExecSqlString(sqlstr);

               
            
        }


        public void DeleteTable(EntityDB.IDatabaseService database, string tableName)
        {
            database.ExecSqlString(string.Format("DROP TABLE IF EXISTS `{0}`", tableName));
        }
               


        List<string> checkIfIdxChanged(EntityDB.IDatabaseService database, string tablename, List<IndexInfo> indexInfos)
        {

            List<string> needToDels = new List<string>();
            string dbname = null;
            var dbnameMatch = System.Text.RegularExpressions.Regex.Match(database.ConnectionString, @"database=(?<dname>(\w)+)", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            if (dbnameMatch == null)
            {
                throw new Exception("连接字符串必须采用以下形式server=localhost;User Id=root;password=123456;Database=testdb");
            }
            dbname = dbnameMatch.Groups["dname"].Value;
            var db = EntityDB.DBContext.CreateDatabaseService(database.ConnectionString.Replace(dbnameMatch.Value, "database=INFORMATION_SCHEMA"), EntityDB.DatabaseType.MySql);
            {
                var tableid = db.ExecSqlString("select TABLE_ID from INNODB_SYS_TABLES where Name='" + dbname + "/"+tablename+"'");
                using (var INNODB_SYS_INDEXES_table = db.SelectTable("select * from INNODB_SYS_INDEXES where TABLE_ID=" + tableid + " and type<>3"))
                {
                    foreach (var drow in INNODB_SYS_INDEXES_table.Rows)
                    {
                        string indexName = drow["NAME"].ToString();
                        bool isUnique = Convert.ToInt32(drow["TYPE"]) == 2;

                        var findExistItem = indexInfos.FirstOrDefault(m => tablename.ToLower() + "_ej_" + m.ColumnNames.OrderBy(p => p).ToArray().ToSplitString("_") == indexName
                            && m.IsUnique == isUnique);

                        if (findExistItem == null)
                        {
                            if(indexName != "GEN_CLUST_INDEX")//GEN_CLUST_INDEX好像是表示没有主键的意思
                                needToDels.Add(indexName);
                        }
                        else
                        {
                            indexInfos.Remove(findExistItem);
                        }
                    }
 
                }
            }


            return needToDels;
        }
        void deletecolumn(EntityDB.IDatabaseService database, string table, string column)
        {
            table = table.ToLower();
            column = column.ToLower();
            database.ExecSqlString(string.Format("alter table `{0}` drop column `{1}`" , table , column));
        }
        void dropTableIndex(EntityDB.IDatabaseService database, string table, string indexName)
        {
            table = table.ToLower();
            database.ExecSqlString("ALTER TABLE `" + table + "` DROP INDEX `" + indexName + "`");
        }
        public void ChangeTable(EntityDB.IDatabaseService database, string oldTableName, string newTableName, EJ.DBColumn[] addColumns, EJ.DBColumn[] changedColumns, EJ.DBColumn[] deletedColumns, EJ.DBColumn[] otherColumns, IndexInfo[] _indexInfos)
        {
            List<IndexInfo> indexInfos = new List<IndexInfo>(_indexInfos);

            //先判断表明是否更改
            if (oldTableName.ToLower() != newTableName.ToLower())
            {
                //更改表名
                database.ExecSqlString(string.Format("alter table `{0}` rename `{1}`", oldTableName, newTableName));
            }

            var needToDels = checkIfIdxChanged(database, newTableName,indexInfos);

            foreach (var column in deletedColumns)
            {
                deletecolumn(database, newTableName, column.Name);
            }

            foreach (string delIndexName in needToDels)
                dropTableIndex(database, newTableName, delIndexName);

            foreach (var column in changedColumns)
            {
                string sqltype = getSqlType(column.dbType);
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
                   
                    database.ExecSqlString(string.Format("alter table `{0}` change `{1}` `{2}` {3}",newTableName , changeitem.OriginalValue.ToString().ToLower() , column.Name.ToLower() , sqltype));
                    #endregion
                }

                changeitem = column.BackupChangedProperties["IsAutoIncrement"];
                if (changeitem != null)
                {
                    changeColumnCount++;

                    #region 变更自增长
                    if (column.IsAutoIncrement == false)
                    {
                        //去掉自增长
                        database.ExecSqlString(string.Format("Alter table `{0}` change `{1}` `{1}` {2}", newTableName, column.Name.ToLower(), sqltype));
                    }
                    else
                    {
                        //设为自增长
                        database.ExecSqlString(string.Format("Alter table `{0}` change `{1}` `{1}` {2} not null auto_increment", newTableName, column.Name.ToLower(), sqltype));
                    }

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
                        database.ExecSqlString(string.Format("Alter table `{0}` drop primary key", newTableName));
                    }
                    else
                    {
                        //设为主键;
                        database.ExecSqlString(string.Format("Alter table `{0}` add primary key(`{1}`)", newTableName,column.Name.ToLower()));
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
                    database.ExecSqlString($"alter table `{newTableName}` MODIFY `{column.Name}` {sqltype} default null");

                    #endregion
                }

                if (column.BackupChangedProperties.Count > changeColumnCount)
                {
                    #region 如果其他地方还有更改
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

                        database.ExecSqlString("update `" + newTableName + "` set `" + column.Name + "`=" + defaultValue + " where `" + column.Name + "` is null");
                    }

                    string sql = "alter table `" + newTableName + "` MODIFY `" + column.Name + "` " + sqltype;
                    if (column.CanNull == false || column.IsPKID == true || column.IsAutoIncrement == true)
                        sql += " NOT";
                    sql += " NULL ";
                    database.ExecSqlString(sql);
                    #endregion
                }

                #region 设置默认值
                if (defaultvalueChanged && !string.IsNullOrEmpty(column.defaultValue))
                {
                    string sql = "";
                    string defaultValue = column.defaultValue.Trim();
                    sql += $"alter table `{newTableName}` MODIFY `{column.Name}` {sqltype} default '{defaultValue.Replace("'","''")}'";

                    
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

                    database.ExecSqlString("update `" + newTableName + "` set `" + column.Name + "`=" + defaultValue + " where `" + column.Name + "` is null");
                }
                #endregion
            }

            foreach (var column in addColumns)
            {
                string sqltype = getSqlType(column.dbType);
                if (column.length.IsNullOrEmpty() == false)
                {
                    if (sqltype.Contains("("))
                        sqltype = sqltype.Substring(0, sqltype.IndexOf("("));
                    sqltype += "(" + column.length + ")";
                }
                #region 新增字段
                string sql = "alter table `" + newTableName + "` add `" + column.Name + "` " + sqltype;

                if (column.IsAutoIncrement == true)
                    sql += " AUTOINCREMENT";

                if (column.CanNull == false || column.IsPKID == true || column.IsAutoIncrement == true)
                {
                    sql += " NOT";
                }
                sql += " NULL  ";
                if (!string.IsNullOrEmpty(column.defaultValue))
                {
                    string defaultValue = column.defaultValue.Trim();
                    sql += " default '" + defaultValue.Replace("'","''") + "'";
                    
                }
                database.ExecSqlString(sql);

                if (column.IsPKID == true)
                {
                    database.ExecSqlString(string.Format("Alter table `{0}` add primary key(`{1}`)", newTableName, column.Name.ToLower()));
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
            //alter table table_name add unique key `new_uk_name` (`col1`,`col2`);
            var columns = indexinfo.ColumnNames.OrderBy(m => m).ToArray();
            string columnsStr = "";
            string name = table + "_ej_" + columns.ToSplitString("_");
            for (int i = 0; i < columns.Length; i++)
            {
                columnsStr += "`" + columns[i] + "`";
                if (i < columns.Length - 1)
                    columnsStr += ",";
            }
            string type = "";
            //if (indexinfo.IsClustered)
            //    throw new Exception("MySql不支持定义聚集索引");
            if (indexinfo.IsUnique || indexinfo.IsClustered)
            {
                type += "unique ";
            }
            database.ExecSqlString(string.Format("alter table `{0}` add {3} key `{1}` ({2})", table, name, columnsStr , type));
        }
    }
}