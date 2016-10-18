using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;

namespace ECWeb.Database.MySql
{
    public class TableService : Services.ITableService
    {
        string getSqlType(string dbtype)
        {
            dbtype = dbtype.ToLower();


            if (dbtype.Contains("int"))
                return "int(11)";

            if (dbtype.Contains("real"))
                return "REAL";
            if (dbtype.Contains("double"))
                return "DOUBLE";
            if (dbtype.Contains("float"))
                return "FLOAT";

            if (dbtype.Contains("numeric"))
                return "NUMERIC";
            if (dbtype.Contains("decimal"))
                return "DECIMAL";
            if (dbtype.Contains("boolean"))
                return "BIT";
            if (dbtype.Contains("date"))
                return "DATETIME";
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
CREATE TABLE `" + table.Name + @"` (
";

                for (int i = 0; i < columns.Length; i++)
                {
                    var column = columns[i];
                    if (i > 0)
                        sqlstr += ",\r\n";
                    string sqltype = getSqlType(column.dbType);
                    if (column.length.IsNullOrEmpty() == false)
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
                        if (config.IsUnique)
                        {
                            type += "UNIQUE ";
                        }
                        if (config.IsClustered)
                            throw new Exception("MySql不支持定义聚集索引");
                        string keyname = table.Name.ToLower() + "_ej_" + config.ColumnNames.OrderBy(m => m).ToArray().ToSplitString("_");
                        sqlstr += (",\r\n"+type+" KEY `" + keyname + "`(" + config.ColumnNames.OrderBy(m => m).ToArray().ToSplitString("_", "`{0}`") + ")");
                    }
                }

                sqlstr += ") ENGINE=InnoDB DEFAULT CHARSET=utf8";

                log.Log(sqlstr);
                db.ExecSqlString(sqlstr);

               
            }
        }


        public void DeleteTable(EntityDB.IDatabaseService database, string tableName)
        {
            database.ExecSqlString(string.Format("DROP TABLE IF EXISTS `{0}`", tableName));
        }

        /*
                CREATE TABLE `people` (
          `peopleid` smallint(6) NOT NULL AUTO_INCREMENT,
          `firstname` char(50) NOT NULL,
          `lastname` char(50) NOT NULL,
          `age` smallint(6) NOT NULL,
          `townid` smallint(6) NOT NULL,
          PRIMARY KEY (`peopleid`),
          UNIQUE KEY `unique_fname_lname`(`firstname`,`lastname`),
          KEY `fname_lname_age` (`firstname`,`lastname`,`age`)
        ) ;
         * 删除索引
         * ALTER TABLE good_booked DROP INDEX good_id;
         * 
         * 设置主键或自增长
         * Alter table tb add primary key(id);
Alter table tb change id id int(10) not null auto_increment=1;
4 删除自增长的主键id
先删除自增长在删除主键
Alter table tb change id id int(10);//删除自增长
Alter table tb drop primary key;//删除主建
         * 
         * 修改字段名
         *    alter table     tablename    change   旧名    new_field_name  字段类型;
         * */


        List<string> checkIfIdxChanged(EntityDB.IDatabaseService database, string tablename, List<IndexInfo> indexInfos)
        {

            List<string> needToDels = new List<string>();
            string dbname = null;
            var dbnameMatch = System.Text.RegularExpressions.Regex.Match(database.ConnectionString, @"database=(?<dname>(\w)+)", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            if (dbnameMatch == null)
            {
                throw new Exception("连接字符串必须采用以下形式server=localhost;User Id=root;password=123456;Database=testDB");
            }
            dbname = dbnameMatch.Groups["dname"].Value;
            var db = EntityDB.DBContext.CreateDatabaseService(database.ConnectionString.Replace(dbnameMatch.Value, "database=INFORMATION_SCHEMA"), EntityDB.DatabaseType.MySql);
            {
                var tableid = db.ExecSqlString("select TABLE_ID from INNODB_SYS_TABLES where Name='" + dbname + "/"+tablename+"'");
                using (DataTable INNODB_SYS_INDEXES_table = db.SelectTable("select * from INNODB_SYS_INDEXES where TABLE_ID=" + tableid + " and type<>3"))
                {
                    foreach (DataRow drow in INNODB_SYS_INDEXES_table.Rows)
                    {
                        string indexName = drow["NAME"].ToString();
                        bool isUnique = Convert.ToInt32(drow["TYPE"]) == 2;

                        var findExistItem = indexInfos.FirstOrDefault(m => tablename.ToLower() + "_ej_" + m.ColumnNames.OrderBy(p => p).ToArray().ToSplitString("_") == indexName
                            && m.IsUnique == isUnique);

                        if (findExistItem == null)
                        {
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

            oldTableName = oldTableName.ToLower();
            newTableName = newTableName.ToLower();

            //先判断表明是否更改
            if (oldTableName != newTableName)
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
                var changeitem = column.BackupChangedProperties.FirstOrDefault(m => m.PropertyName == "Name");
                if (changeitem != null)
                {
                    changeColumnCount++;

                    #region 改名
                   
                    database.ExecSqlString(string.Format("alter table `{0}` change `{1}` `{2}` {3}",newTableName , changeitem.OriginalValue.ToString().ToLower() , column.Name.ToLower() , sqltype));
                    #endregion
                }

                changeitem = column.BackupChangedProperties.FirstOrDefault(m => m.PropertyName == "IsAutoIncrement");
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
                        database.ExecSqlString(string.Format("Alter table `{0}` change `{1}` `{1}` {2} not null auto_increment=1", newTableName, column.Name.ToLower(), sqltype));
                    }

                    #endregion
                }

                changeitem = column.BackupChangedProperties.FirstOrDefault(m => m.PropertyName == "IsPKID");
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
                changeitem = column.BackupChangedProperties.FirstOrDefault(m => m.PropertyName == "defaultValue");
                if (changeitem != null)
                {
                    defaultvalueChanged = true;
                    changeColumnCount++;

                    #region 默认值
                    //删除默认值
                    database.ExecSqlString(string.Format("alter table `{0}` alter column `{1}` set default null",newTableName,column.Name));

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

                    string sql = "alter table `" + newTableName + "` alter column `" + column.Name + "` " + sqltype;
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
                    if (defaultValue.Length > 1 && defaultValue.StartsWith("'") && defaultValue.EndsWith("'"))
                    {
                        sql += "alter table `"+ newTableName +"` alter column `"+column.Name+"` set default " + defaultValue;
                    }
                    else
                    {
                        sql += "alter table `" + newTableName + "` alter column `" + column.Name + "` set default '" + defaultValue + "'";
                        
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
                    if ((defaultValue.Length > 1 && defaultValue.StartsWith("'") && defaultValue.EndsWith("'")) || defaultValue.Contains("()"))
                    {
                        sql += " default (" + defaultValue +")" ;
                    }
                    else
                    {
                        sql += " default ('" + defaultValue + "')";
                    }


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
            if (indexinfo.IsClustered)
                throw new Exception("MySql不支持定义聚集索引");
            if (indexinfo.IsUnique)
            {
                type += "unique ";
            }
            database.ExecSqlString(string.Format("alter table `{0}` add {3} key `{1}` ({2})", table, name, columnsStr , type));
        }
    }
}