
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
    class SqlServerDatabaseService : IDatabaseDesignService
    {
        public void Drop(EJ.Databases database)
        {
            string constr = database.conStr;
            constr = Regex.Replace(database.conStr, @"database=(\w)+", "database=master", RegexOptions.IgnoreCase);
            //throw new Exception(constr);
            var db = EntityDB.DBContext.CreateDatabaseService(constr, EntityDB.DatabaseType.SqlServer);

            db.ExecSqlString("if exists(select [dbid] from sysdatabases where [name]='" + database.Name + "') drop database " + database.Name );

        }
        public void Create(EJ.Databases database)
        {
            string constr = database.conStr;
            constr = Regex.Replace(database.conStr, @"database=(\w)+", "database=master" , RegexOptions.IgnoreCase);
            //throw new Exception(constr);
            var db = EntityDB.DBContext.CreateDatabaseService(constr, EntityDB.DatabaseType.SqlServer);

            /*
             COLLATE Chinese_PRC_CI_AS ，如果没有这句，linux的sql server中文会乱码
             指定SQL server的排序规则
                Chinese_PRC指的是中国大陆地区，如果是台湾地区则为Chinese_Taiwan
                CI指定不区分大小写，如果要在查询时区分输入的大小写则改为CS
                AS指定区分重音，同样如果不需要区分重音，则改为AI
                COLLATE可以针对整个数据库更改排序规则，也可以单独修改某一个表或者某一个字段的排序规则，指定排序规则很有用，比如用户管理表，需要验证输入的用户名和密码的正确性，一般是要区分大小写的。
             */
            db.ExecSqlString("if not exists(select [dbid] from sysdatabases where [name]='" + database.Name + "') create database " + database.Name + " COLLATE Chinese_PRC_CI_AS");

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