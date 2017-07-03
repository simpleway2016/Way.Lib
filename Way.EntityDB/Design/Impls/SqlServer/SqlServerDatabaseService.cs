
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

            db.ExecSqlString("if exists(select [dbid] from sysdatabases where [name]='" + database.Name.ToLower() + "') drop database " + database.Name.ToLower() );

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
            db.ExecSqlString("if not exists(select [dbid] from sysdatabases where [name]='" + database.Name.ToLower() + "') create database " + database.Name.ToLower() + " COLLATE Chinese_PRC_CI_AS");

            db = EntityDB.DBContext.CreateDatabaseService(database.conStr, EntityDB.DatabaseType.SqlServer);
            CreateEasyJobTable(db);
            //ado.ExecCommandTextUseSameCon("if not exists(select [dbid] from sysdatabases where [name]='" + txt_databasename.Text + "') create database " + txt_databasename.Text);
        }

        public List<EJ.DBColumn> GetCurrentColumns(IDatabaseService db, string tablename)
        {
            List<string> pkfields = new List<string>();
            using (var sp_helpResult = db.SelectDataSet("sp_help [" + tablename + "]"))
            {
                foreach (var dtable in sp_helpResult.Tables)
                {
                    if (dtable.Columns.Any(m => m.ColumnName == "index_keys"))
                    {
                        foreach (WayDataRow drow in dtable.Rows)
                        {
                            string existColumnString = drow["index_keys"].ToString();
                            string indexName = drow["index_name"].ToString();
                            string index_description = drow["index_description"].ToString();
                            if (index_description.Contains("primary key") == true)
                            {
                                //去除空格
                                string flag = existColumnString.Split(',').ToSplitString();
                                pkfields.AddRange(flag.Split(',').OrderBy(m => m).ToArray());
                            }
                        }
                    }
                }
            }

            List<EJ.DBColumn> result = new List<EJ.DBColumn>();

            var table = db.SelectTable($"select name,length,xtype,cdefault,isnullable,columnproperty(id,name,'IsIdentity') as IsAutoIncrement from syscolumns where ID=OBJECT_ID('{tablename}') ");
           
            foreach (var row in table.Rows)
            {
                EJ.DBColumn column = new EJ.DBColumn();
                column.Name = row["name"].ToSafeString();
                column.dbType = db.ExecSqlString($"select name  from SYSTYPES where xtype={row["xtype"]}").ToSafeString();
                int typeindex = -1;
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
                column.defaultValue = row["cdefault"].ToSafeString();
                if(!column.defaultValue.IsNullOrEmpty())
                {
                    column.defaultValue = db.ExecSqlString($@"SELECT TEXT FROM syscomments where id='{column.defaultValue}'").ToSafeString().Trim();
                    if (column.defaultValue.StartsWith("("))
                    {
                        column.defaultValue = column.defaultValue.Substring(1, column.defaultValue.Length - 2).Replace("''","'");
                        if (column.defaultValue.StartsWith("'"))
                            column.defaultValue = column.defaultValue.Substring(1, column.defaultValue.Length - 2);
                    }
                }

                column.CanNull = row["isnullable"].ToSafeString() == "1";
                column.IsAutoIncrement = row["IsAutoIncrement"].ToSafeString() == "1";
                //

                column.IsPKID = pkfields.Any(m => string.Equals(m , column.Name , StringComparison.CurrentCultureIgnoreCase));
                column.length = row["length"].ToString();
                column.ChangedProperties.Clear();
                result.Add(column);
            }
            return result;
        }
        public List<IndexInfo> GetCurrentIndexes(IDatabaseService db, string tablename)
        {
            List<IndexInfo> existKeys = new List<IndexInfo>();
            using (var sp_helpResult = db.SelectDataSet("sp_help [" + tablename + "]"))
            {
                foreach (var dtable in sp_helpResult.Tables)
                {
                    if (dtable.Columns.Any(m => m.ColumnName == "index_keys"))
                    {
                        foreach (WayDataRow drow in dtable.Rows)
                        {
                            string existColumnString = drow["index_keys"].ToString();
                            string indexName = drow["index_name"].ToString();
                            string index_description = drow["index_description"].ToString();
                            if (index_description.Contains("primary key") == false)
                            {
                                //去除空格
                                string flag = existColumnString.Split(',').ToSplitString();
                                string dbname = flag.Split(',').OrderBy(m => m).ToArray().ToSplitString();
                                //再排序，不要在去除空格之前排序
                                existKeys.Add(new IndexInfo
                                {
                                    Name = indexName,
                                    IsUnique = index_description.Contains("unique"),
                                    IsClustered = index_description.Contains("clustered") && !index_description.Contains("nonclustered"),
                                    ColumnNames = new string[] { dbname },
                                });
                            }
                        }
                    }
                }
            }
            return existKeys;
        }
        public List<string> GetCurrentTableNames(IDatabaseService db)
        {
            List<string> result = new List<string>();
            db.ExecuteReader( (reader)=> {
                result.Add(reader[0].ToSafeString());
                return true;
            }, "SELECT Name FROM SysObjects Where XType='U' and Name<>'__wayeasyjob' ORDER BY Name");

            return result;
        }
        public void ChangeName(EJ.Databases database, string newName, string newConnectString)
        {
             string constr = database.conStr;
            constr = Regex.Replace(database.conStr, @"database=(\w)+", "database=master" , RegexOptions.IgnoreCase);
            //throw new Exception(constr);
            var db = EntityDB.DBContext.CreateDatabaseService(constr, EntityDB.DatabaseType.SqlServer);
            {
                db.ExecSqlString("exec sp_renamedb '"+database.Name.ToLower()+"','"+newName.ToLower()+"'");

                try
                {
                    var db2 = EntityDB.DBContext.CreateDatabaseService(newConnectString, EntityDB.DatabaseType.SqlServer);
                    db2.ExecSqlString("select 1");
                }
                catch
                {
                    //
                    db.ExecSqlString("exec sp_renamedb '" + newName.ToLower() + "','" + database.Name.ToLower() + "'");
                    throw new Exception("连接字符串错误");
                }
            }
         
        }


        //public static string GetDBTypeString(Type type)
        //{
        //    if (type == typeof(long))
        //        return "bigint";
        //    if (type == typeof(Byte[]))
        //        return "binary";
        //    if (type == typeof(bool))
        //        return "bit";
        //    if (type == typeof(string))
        //        return "varchar";
        //    if (type == typeof(DateTime))
        //        return "datetime";
        //    if (type == typeof(decimal))
        //        return "decimal";
        //    if (type == typeof(double))
        //        return "float";
        //    if (type == typeof(float))
        //        return "float";
        //    if (type == typeof(int))
        //        return "int";
        //    return "";
        //}

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
                db.ExecSqlString("CREATE TABLE [__wayeasyjob](contentConfig varchar(1000) NOT NULL)");
                var dbconfig = new DataBaseConfig();
                try
                {
                    dbconfig.LastUpdatedID = Convert.ToInt32(db.ExecSqlString("select lastID from __EasyJob"));
                }
                catch
                {
                }
                db.ExecSqlString("insert into __wayeasyjob (contentConfig) values (@p0)", dbconfig.ToJsonString());
            }
        }


        public string GetObjectFormat()
        {
            return "[{0}]";
        }

       
    }
}