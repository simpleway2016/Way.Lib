using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EJ;
using Way.EntityDB.Design.Services;

namespace Way.EntityDB.Design.Impls.PostgreSQL
{
    [EntityDB.Attributes.DatabaseTypeAttribute(DatabaseType.PostgreSQL)]
    class DatabaseService : Services.IDatabaseDesignService
    {
        public void ChangeName(Databases database, string newName, string newConnectString)
        {
            throw new NotImplementedException();
        }

        public void Create(Databases database)
        {

            var dbnameMatch = System.Text.RegularExpressions.Regex.Match(database.conStr, @"database=(?<dname>(\w)+)", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            if (dbnameMatch == null)
            {
                throw new Exception("连接字符串必须采用以下形式Server=;Port=5432;UserId=;Password=;Database=;");
            }

            var db = EntityDB.DBContext.CreateDatabaseService(database.conStr.Replace(dbnameMatch.Value, ""), EntityDB.DatabaseType.PostgreSQL);
            object flag = db.ExecSqlString("select count(*) from pg_catalog.pg_database where datname=@p0", database.Name);
            if (Convert.ToInt32(flag)== 0)
            {
                db.ExecSqlString("CREATE DATABASE " + database.Name + " ENCODING='UTF-8'");
            }

            //创建必须表
            db = EntityDB.DBContext.CreateDatabaseService(database.conStr, EntityDB.DatabaseType.PostgreSQL);
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
