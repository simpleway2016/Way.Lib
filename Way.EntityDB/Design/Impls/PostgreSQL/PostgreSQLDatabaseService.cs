using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EJ;
using Way.EntityDB.Design.Services;
using System.Text.RegularExpressions;

namespace Way.EntityDB.Design.Impls.PostgreSQL
{
    [EntityDB.Attributes.DatabaseTypeAttribute(DatabaseType.PostgreSql)]
    class PostgreSQLDatabaseService : Services.IDatabaseDesignService
    {
        public void ChangeName(Databases database, string newName, string newConnectString)
        {
            var dbnameMatch = System.Text.RegularExpressions.Regex.Match(newConnectString, @"database=(?<dname>(\w)+)", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            if (dbnameMatch == null)
            {
                throw new Exception("连接字符串必须采用以下形式Server=;Port=5432;UserId=;Password=;Database=;");
            }

            var db = EntityDB.DBContext.CreateDatabaseService(newConnectString.Replace(dbnameMatch.Value, ""), EntityDB.DatabaseType.PostgreSql);
            db.ExecSqlString($"ALTER DATABASE {database.Name} RENAME TO {newName}");
        }
        public void Drop(Databases database)
        {

            var dbnameMatch = System.Text.RegularExpressions.Regex.Match(database.conStr, @"database=(?<dname>(\w)+)", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            if (dbnameMatch == null)
            {
                throw new Exception("连接字符串必须采用以下形式Server=;Port=5432;UserId=;Password=;Database=;");
            }

            var db = EntityDB.DBContext.CreateDatabaseService(database.conStr.Replace(dbnameMatch.Value, ""), EntityDB.DatabaseType.PostgreSql);
            db.ExecSqlString("DROP DATABASE if exists " + database.Name + "");
            db.DBContext.Dispose();
        }
        public void Create(Databases database)
        {

            var dbnameMatch = System.Text.RegularExpressions.Regex.Match(database.conStr, @"database=(?<dname>(\w)+)", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            if (dbnameMatch == null)
            {
                throw new Exception("连接字符串必须采用以下形式Server=;Port=5432;UserId=;Password=;Database=;");
            }

            var db = EntityDB.DBContext.CreateDatabaseService(database.conStr.Replace(dbnameMatch.Value, ""), EntityDB.DatabaseType.PostgreSql);
            object flag = db.ExecSqlString("select count(*) from pg_catalog.pg_database where datname=@p0", database.Name);
            if (Convert.ToInt32(flag)== 0)
            {
                db.ExecSqlString("CREATE DATABASE " + database.Name + " ENCODING='UTF-8'");
            }

            //创建必须表
            db = EntityDB.DBContext.CreateDatabaseService(database.conStr, EntityDB.DatabaseType.PostgreSql);
            db.DBContext.BeginTransaction();
            try
            {
                CreateEasyJobTable(db);
                db.DBContext.CommitTransaction();
            }
            catch (Exception ex)
            {
                db.DBContext.RollbackTransaction();
                throw ex;
            }
        }
        public List<IndexInfo> GetCurrentIndexes(IDatabaseService db, string tablename)
        {
            List<IndexInfo> result = new List<Design.IndexInfo>();
            var pkeyTable = db.SelectTable($@"select pg_constraint.conname as pk_name,pg_attribute.attname as colname,pg_type.typname as typename from 
pg_constraint  inner join pg_class 
on pg_constraint.conrelid = pg_class.oid 
inner join pg_attribute on pg_attribute.attrelid = pg_class.oid 
and  pg_attribute.attnum = pg_constraint.conkey[1]
inner join pg_type on pg_type.oid = pg_attribute.atttypid
where pg_class.relname = '{tablename}' 
and pg_constraint.contype='p'");
            var indexTable = db.SelectTable($"select * from pg_indexes where tablename='{tablename}' and schemaname='public'");
            foreach (var row in indexTable.Rows)
            {
                var indexname = row["indexname"].ToSafeString();
                var indexdef = row["indexdef"].ToSafeString();
                Match ms = Regex.Match(indexdef, @"btree( )?\((?<columns>(\w| |,)+)\)");
                var t_columns = ms.Groups["columns"].Value.Split(',');
                var columns = (from m in t_columns
                               where m.Trim().Length > 0
                               select m.Trim().Split(' ')[0]).OrderBy(m => m).ToArray();
                var isClustered = indexdef.Contains(" NULLS FIRST");
                var isUnique = indexdef.StartsWith("CREATE UNIQUE ");
                string name = columns.ToSplitString(",");
                if (pkeyTable.Rows.Any(m => m["colname"].ToSafeString() == name))
                    continue;

                result.Add(new IndexInfo()
                {
                    ColumnNames = columns,
                    IsClustered = isClustered,
                    IsUnique = isUnique,
                    Name = indexname
                });
            }
            return result;
        }
        public List<EJ.DBColumn> GetCurrentColumns(IDatabaseService db, string tablename)
        {
            List<EJ.DBColumn> result = new List<EJ.DBColumn>();
            var table = db.SelectTable($"select column_name,data_type,column_default,is_nullable,character_maximum_length,character_octet_length from information_schema.columns where table_name='{tablename}'");
            var pkeyTable = db.SelectTable($@"select pg_constraint.conname as pk_name,pg_attribute.attname as colname,pg_type.typname as typename from 
pg_constraint  inner join pg_class 
on pg_constraint.conrelid = pg_class.oid 
inner join pg_attribute on pg_attribute.attrelid = pg_class.oid 
and  pg_attribute.attnum = pg_constraint.conkey[1]
inner join pg_type on pg_type.oid = pg_attribute.atttypid
where pg_class.relname = '{tablename}' 
and pg_constraint.contype='p'");
            foreach (var row in table.Rows)
            {
                EJ.DBColumn column = new EJ.DBColumn();
                column.Name = row["column_name"].ToSafeString();
                column.dbType = row["data_type"].ToSafeString();
                int typeindex = Database.PostgreSQL.PostgreSQLTableService.ColumnType.IndexOf(column.dbType);
                if(typeindex >= 0)
                {
                    column.dbType = EntityDB.Design.ColumnType.SupportTypes[typeindex];
                }
                else
                {
                    column.dbType = "[未识别]" + column.dbType;
                }
                column.defaultValue = row["column_default"].ToSafeString();
                if (column.defaultValue.StartsWith("nextval"))
                    column.defaultValue = "";
                else if(column.defaultValue.Contains("::") )
                {
                    column.defaultValue = column.defaultValue.Substring(1, column.defaultValue.LastIndexOf("::") - 1);
                }

                column.CanNull = row["is_nullable"].ToSafeString() == "YES";
                column.IsAutoIncrement = row["column_default"].ToSafeString().StartsWith("nextval");
                column.IsPKID = pkeyTable.Rows.Any(m => m["colname"].ToSafeString() == column.Name);
                if (column.dbType.Contains("char"))
                {
                    if (row["character_maximum_length"] != null)
                        column.length = row["character_maximum_length"].ToString();
                }
                else
                {
                    if (row["character_octet_length"] != null)
                        column.length = row["character_octet_length"].ToString();
                }
                column.ChangedProperties.Clear();
                result.Add(column);
            }
            return result;
        }

        public void CreateEasyJobTable(EntityDB.IDatabaseService db)
        {
            bool exists = Convert.ToInt32(db.ExecSqlString("select count(*) from pg_tables where tablename='__wayeasyjob'")) > 0;
            
            if (!exists)
            {
                db.ExecSqlString("create table  __wayeasyjob (contentConfig VARCHAR(1000) NOT NULL)");
                db.ExecSqlString("insert into __wayeasyjob (contentConfig) values (@p0)", new DataBaseConfig().ToJsonString());
            }
        }

        public string GetObjectFormat()
        {
            return "{0}";
        }
    }
}
