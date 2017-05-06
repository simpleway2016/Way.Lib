
using Way.EntityDB.Design.Services;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace Way.EntityDB.Design.Database.MySql
{
    [EntityDB.Attributes.DatabaseTypeAttribute(DatabaseType.MySql)]
    class DatabaseService : IDatabaseDesignService
    {


        public void Create(EJ.Databases database)
        {
            if (database.Name.ToLower() != database.Name)
                throw new Exception("MySql数据库名称必须是小写");
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
                db.ExecSqlString("create table  `__WayEasyJob` (contentConfig varchar(1000)  not null)");
                db.ExecSqlString("insert into __WayEasyJob (contentConfig) values (@p0)", new DataBaseConfig().ToJsonString());
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