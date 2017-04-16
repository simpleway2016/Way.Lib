
using Way.EntityDB.Design.Services;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;


namespace Way.EntityDB.Design.Database.SqlServer
{
    [EntityDB.Attributes.DatabaseTypeAttribute( DatabaseType.SqlServer)]
    public class DatabaseService : IDatabaseDesignService
    {
        public void Create(EJ.Databases database)
        {
            string constr = database.conStr;
            constr = Regex.Replace(database.conStr, @"database=(\w)+", "database=master" , RegexOptions.IgnoreCase);
            //throw new Exception(constr);
            var db = EntityDB.DBContext.CreateDatabaseService(constr, EntityDB.DatabaseType.SqlServer);

            db.ExecSqlString("if not exists(select [dbid] from sysdatabases where [name]='" + database.Name + "') create database " + database.Name);

            db = EntityDB.DBContext.CreateDatabaseService(database.conStr, EntityDB.DatabaseType.SqlServer);
            CreateEasyJobTable(db);
            //ado.ExecCommandTextUseSameCon("if not exists(select [dbid] from sysdatabases where [name]='" + txt_databasename.Text + "') create database " + txt_databasename.Text);
        }

        public void ChangeName(EJ.Databases database, string newName, string newConnectString)
        {
             string constr = database.conStr;
            constr = Regex.Replace(database.conStr, @"database=(\w)+", "database=master" , RegexOptions.IgnoreCase);
            //throw new Exception(constr);
            var db = EntityDB.DBContext.CreateDatabaseService(constr, EntityDB.DatabaseType.SqlServer);
            {
                db.ExecSqlString("exec sp_renamedb '"+database.Name+"','"+newName+"'");

                try
                {
                    var db2 = EntityDB.DBContext.CreateDatabaseService(newConnectString, EntityDB.DatabaseType.SqlServer);
                    db2.ExecSqlString("select 1");
                }
                catch
                {
                    //
                    db.ExecSqlString("exec sp_renamedb '" + newName + "','" + database.Name + "'");
                    throw new Exception("连接字符串错误");
                }
            }
         
        }
        public static string GetDBTypeString(Type type)
        {
            if (type == typeof(long))
                return "bigint";
            if (type == typeof(Byte[]))
                return "binary";
            if (type == typeof(bool))
                return "bit";
            if (type == typeof(string))
                return "varchar";
            if (type == typeof(DateTime))
                return "datetime";
            if (type == typeof(decimal))
                return "decimal";
            if (type == typeof(double))
                return "float";
            if (type == typeof(float))
                return "float";
            if (type == typeof(int))
                return "int";
            return "";
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
                db.ExecSqlString("CREATE TABLE [__WayEasyJob](contentConfig varchar(1000) NOT NULL)");
                var dbconfig = new DataBaseConfig();
                try
                {
                    dbconfig.LastUpdatedID = Convert.ToInt32(db.ExecSqlString("select lastID from __EasyJob"));
                }
                catch
                {
                }
                db.ExecSqlString("insert into __WayEasyJob (contentConfig) values (@p0)", dbconfig.ToJsonString());
            }
        }


        public string GetObjectFormat()
        {
            return "[{0}]";
        }
    }
}