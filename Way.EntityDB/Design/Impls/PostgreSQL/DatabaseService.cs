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
            if (database.Name.ToLower() != database.Name)
                throw new Exception("PostgreSQL数据库名称必须是小写");
            var dbnameMatch = System.Text.RegularExpressions.Regex.Match(database.conStr, @"database=(?<dname>(\w)+)", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            if (dbnameMatch == null)
            {
                throw new Exception("连接字符串必须采用以下形式server=localhost;User Id=root;password=123456;Database=testDB");
            }

            var db = EntityDB.DBContext.CreateDatabaseService(database.conStr.Replace(dbnameMatch.Value, ""), EntityDB.DatabaseType.MySql);
            db.ExecSqlString("CREATE DATABASE " + database.Name.ToLower() + " ENCODING='UTF-8'");

            //创建必须表
            db = EntityDB.DBContext.CreateDatabaseService(database.conStr, EntityDB.DatabaseType.MySql);
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
                db.ExecSqlString("create table  __WayEasyJob (contentConfig varchar(1000)  not null)");
                db.ExecSqlString("insert into __WayEasyJob (contentConfig) values (@p0)", new DataBaseConfig().ToJsonString());
            }
        }

        public string GetObjectFormat()
        {
            return "{0}";
        }
    }
}
