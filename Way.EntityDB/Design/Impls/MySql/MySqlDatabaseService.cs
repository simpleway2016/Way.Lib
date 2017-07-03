
using Way.EntityDB.Design.Services;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace Way.EntityDB.Design.Database.MySql
{
    [EntityDB.Attributes.DatabaseTypeAttribute(DatabaseType.MySql)]
    class MySqlDatabaseService : IDatabaseDesignService
    {

        public void Drop(EJ.Databases database)
        {
            var dbnameMatch = System.Text.RegularExpressions.Regex.Match(database.conStr, @"database=(?<dname>(\w)+)", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            if (dbnameMatch == null)
            {
                throw new Exception("连接字符串必须采用以下形式server=localhost;User Id=root;password=123456;Database=testDB");
            }

            var db = EntityDB.DBContext.CreateDatabaseService(database.conStr.Replace(dbnameMatch.Value, ""), EntityDB.DatabaseType.MySql);
            db.ExecSqlString("drop database if exists `" + database.Name.ToLower() + "`");
            db.DBContext.Dispose();
        }
        public void Create(EJ.Databases database)
        {
            var dbnameMatch = System.Text.RegularExpressions.Regex.Match(database.conStr, @"database=(?<dname>(\w)+)", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            if (dbnameMatch == null)
            {
                throw new Exception("连接字符串必须采用以下形式server=localhost;User Id=root;password=123456;Database=testDB");
            }

            var db = EntityDB.DBContext.CreateDatabaseService(database.conStr.Replace(dbnameMatch.Value, ""), EntityDB.DatabaseType.MySql);
            db.ExecSqlString("create database if not exists `" + database.Name.ToLower() + "` CHARACTER SET 'utf8' COLLATE 'utf8_general_ci'");

            //创建必须表
            db = EntityDB.DBContext.CreateDatabaseService(database.conStr, EntityDB.DatabaseType.MySql);
            db.DBContext.BeginTransaction();
            try
            {
                CreateEasyJobTable(db);
                db.DBContext.CommitTransaction();
            }
            catch(Exception ex)
            {
                db.DBContext.RollbackTransaction();
                throw ex;
            }
        }

        public List<EJ.DBColumn> GetCurrentColumns(IDatabaseService db, string tablename)
        {
            List<EJ.DBColumn> result = new List<EJ.DBColumn>();
            var dbnameMatch = System.Text.RegularExpressions.Regex.Match(db.ConnectionString, @"database=(?<dname>(\w)+)", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            var dbname = dbnameMatch.Groups["dname"].Value;
            var table = db.SelectTable($"select * from information_schema.COLUMNS where TABLE_SCHEMA='{dbname}' and TABLE_NAME='{tablename}'");
            foreach (var row in table.Rows)
            {
                EJ.DBColumn column = new EJ.DBColumn();
                column.Name = row["COLUMN_NAME"].ToSafeString();
                column.CanNull = row["IS_NULLABLE"].ToSafeString() == "YES";
                column.dbType = row["DATA_TYPE"].ToSafeString().ToLower();
                int typeindex = -1;
                for (int i = 0; i < Database.MySql.MySqlTableService.ColumnType.Count; i++)
                {
                    if (string.Equals(Database.MySql.MySqlTableService.ColumnType[i], column.dbType, StringComparison.CurrentCultureIgnoreCase))
                    {
                        typeindex = i;
                        break;
                    }
                }
                if (typeindex >= 0)
                {
                    column.dbType = EntityDB.Design.ColumnType.SupportTypes[typeindex];
                }
                else
                {
                    for (int i = 0; i < Way.EntityDB.Design.ColumnType.SupportTypes.Count; i++)
                    {
                        if (string.Equals(Way.EntityDB.Design.ColumnType.SupportTypes[i], column.dbType, StringComparison.CurrentCultureIgnoreCase))
                        {
                            typeindex = i;
                            break;
                        }
                    }

                    if (typeindex >= 0)
                    {
                        column.dbType = EntityDB.Design.ColumnType.SupportTypes[typeindex];
                    }
                    else
                    {
                        column.dbType = "[未识别]" + column.dbType;
                    }
                }
                column.defaultValue = row["COLUMN_DEFAULT"].ToSafeString();
                column.IsAutoIncrement = row["EXTRA"].ToSafeString().Contains("auto_increment");
                column.IsPKID = row["COLUMN_KEY"].ToSafeString().Contains("PRI");
                column.length = row["CHARACTER_MAXIMUM_LENGTH"].ToSafeString();
                column.ChangedProperties.Clear();
                result.Add(column);
            }
            return result;
        }
        public List<IndexInfo> GetCurrentIndexes(IDatabaseService db, string tablename)
        {
            List<IndexInfo> result = new List<IndexInfo>();
               var dbnameMatch = System.Text.RegularExpressions.Regex.Match(db.ConnectionString, @"database=(?<dname>(\w)+)", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            var dbname = dbnameMatch.Groups["dname"].Value;
            var table = db.SelectTable($"SELECT *  FROM  information_schema.statistics WHERE TABLE_SCHEMA='{dbname}' and table_name='{tablename}' and INDEX_NAME<>'PRIMARY'");
            foreach( var row in table.Rows )
            {
                string indexName = row["INDEX_NAME"].ToSafeString();
                bool unique = !Convert.ToBoolean( row["NON_UNIQUE"]);
                IndexInfo indexInfo = result.FirstOrDefault(m => m.Name == indexName);
                if(indexInfo == null)
                {
                    indexInfo = new IndexInfo()
                    {
                        Name = indexName,
                        IsUnique = unique,
                        ColumnNames = new string[0],
                    };
                    result.Add(indexInfo);
                }
                string columnName = row["COLUMN_NAME"].ToSafeString();
                var columnNames = new List<string>(indexInfo.ColumnNames);
                columnNames.Add(columnName);
                indexInfo.ColumnNames = columnNames.OrderBy(m=>m).ToArray();
            }

            return result;
        }
        public List<string> GetCurrentTableNames(IDatabaseService db)
        {
            var dbnameMatch = System.Text.RegularExpressions.Regex.Match(db.ConnectionString, @"database=(?<dname>(\w)+)", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            var dbname = dbnameMatch.Groups["dname"].Value;
            List<string> result = new List<string>();
            db.ExecuteReader((reader) => {
                result.Add(reader[0].ToSafeString());
                return true;
            }, $"SELECT TABLE_NAME FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA='{dbname}' and TABLE_NAME<>'__wayeasyjob'  ORDER  BY  TABLE_NAME");

            return result;
        }
        public void CreateEasyJobTable(EntityDB.IDatabaseService db)
        {
            bool exists = true;
            try
            {
                db.ExecSqlString("select * from __wayeasyjob");
            }
            catch
            {
                exists = false;
            }
            if (!exists)
            {
                db.ExecSqlString("create table  `__wayeasyjob` (contentConfig varchar(1000)  not null)");
                db.ExecSqlString("insert into __wayeasyjob (contentConfig) values (@p0)", new DataBaseConfig().ToJsonString());
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



        public string GetObjectFormat()
        {
            return "`{0}`";
        }
    }
}