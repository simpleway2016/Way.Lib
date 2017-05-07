using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EJ;
using Way.EntityDB.Design.Services;

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

        public void CreateEasyJobTable(EntityDB.IDatabaseService db)
        {
            bool exists = Convert.ToInt32(db.ExecSqlString("select count(*) from pg_tables where tablename='__WayEasyJob'")) > 0;
            
            if (!exists)
            {
                db.ExecSqlString("create table  __WayEasyJob (contentConfig VARCHAR(1000) NOT NULL)");
                db.ExecSqlString("insert into __WayEasyJob (contentConfig) values (@p0)", new DataBaseConfig().ToJsonString());
            }
        }

        public string GetObjectFormat()
        {
            return "{0}";
        }
    }
}
