
using Way.EntityDB.Design.Services;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Way.EntityDB.Design.Database.Sqlite
{
    [EntityDB.Attributes.DatabaseTypeAttribute(DatabaseType.Sqlite)]
    class SqliteDatabaseService : IDatabaseDesignService
    {
        public void Drop(EJ.Databases database)
        {
            string constr = database.conStr;
            var m = Regex.Match(database.conStr, @"data source=(?<f>(\w|:|\\|\/|\.)+)", RegexOptions.IgnoreCase);
            if (m != null && m.Length > 0)
            {
                string filename = m.Groups["f"].Value;
                if (filename.StartsWith("\"") || filename.StartsWith("\'"))
                    filename = filename.Substring(1, filename.Length - 2);
                if (System.IO.File.Exists(filename))
                {
                    System.IO.File.Delete(filename);
                }
               
            }
            else
                throw new Exception("无法从连接字符串获取数据库文件路径");
        }
        public void Create(EJ.Databases database)
        {

            var db = EntityDB.DBContext.CreateDatabaseService(database.conStr, EntityDB.DatabaseType.Sqlite);
            CreateEasyJobTable(db);
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
                db.ExecSqlString("CREATE TABLE [__wayeasyjob](contentConfig TEXT  NOT NULL)");
                var dbconfig = new DataBaseConfig();
                try
                {
                    dbconfig.LastUpdatedID = Convert.ToInt32( db.ExecSqlString("select lastID from __EasyJob"));
                }
                catch
                {
                }
                db.ExecSqlString("insert into __wayeasyjob (contentConfig) values (@p0)", dbconfig.ToJsonString());
               
            }

            //try
            //{
            //    db.ExecSqlString("CREATE TABLE  [] ([lastID] integer  NOT NULL)");
            //    db.ExecSqlString("insert into  (lastID) values (0)");
            //}
            //catch
            //{
            //}
        }
        public List<IndexInfo> GetCurrentIndexes(IDatabaseService db, string tablename)
        {
            var result = new List<IndexInfo>();
            using (var dtable = db.SelectTable("select * from sqlite_master where type='index' and tbl_name='" + tablename + "' "))
            {
                foreach (var drow in dtable.Rows)
                {
                    var sql = drow["sql"].ToSafeString();
                    sql = sql.Substring(sql.LastIndexOf("(")).Trim();
                    var columnNames = sql.Substring(1, sql.Length - 2).Trim().Split(',');
                    columnNames = (from m in columnNames
                                   select m.Trim()).OrderBy(m => m).ToArray();
                    result.Add(new IndexInfo
                    {
                        Name = drow["name"].ToSafeString(),
                        IsUnique = drow["sql"].ToSafeString().Contains(" UNIQUE "),
                        IsClustered = false,
                        ColumnNames = columnNames
                    });
                }
            }
            return result;
        }
        public List<EJ.DBColumn> GetCurrentColumns(IDatabaseService db, string tablename)
        {
            List<EJ.DBColumn> result = new List<EJ.DBColumn>();
            var createTableSql = db.ExecSqlString($"select sql from sqlite_master where type='table' and name='{tablename}'");
            if(createTableSql != null)
            {
              
                var sql = createTableSql.ToString();
                var match = Regex.Match(sql, @"create( )+table( )+(\w|\[|\])+( )+",  RegexOptions.IgnoreCase);
                sql = sql.Substring(match.Index + match.Length).Trim();
                if (sql.StartsWith("("))
                    sql = sql.Substring(1, sql.Length - 2);
                List<string> fieldStrings = new List<string>();
                StringBuilder itemString = new StringBuilder();
                bool stringBegined = false;
                for(int i = 0; i < sql.Length; i ++)
                {
                    char c = sql[i];
                    if( c == ',' && stringBegined == false)
                    {
                        fieldStrings.Add(itemString.ToString().Trim());
                        itemString = new StringBuilder();
                        continue;
                    }
                    else if( c == '\'' )
                    {
                        if (stringBegined == false)
                        {
                            stringBegined = true;
                        }
                        else
                        {
                            if (i + 1 < sql.Length && sql[i + 1] == '\'')
                            {
                                itemString.Append('\'');
                                i++;
                            }
                            else
                            {
                                stringBegined = false;
                            }
                        }
                    }
                    itemString.Append(c);
                }
                if(itemString.ToString().Trim().Length > 0)
                {
                    fieldStrings.Add(itemString.ToString().Trim());
                }

                foreach( var itemstring in fieldStrings )
                {
                    if(itemstring.Length > 0)
                    {
                        sql = itemstring;
                        match = Regex.Match(sql, @"(\w|\[|\])+( )+", RegexOptions.IgnoreCase);
                        EJ.DBColumn column = new EJ.DBColumn();
                        column.Name = match.Value.Trim();
                        if (column.Name.StartsWith("["))
                            column.Name = column.Name.Substring(1, column.Name.Length - 2);

                        sql = sql.Substring(match.Index + match.Length);
                        match = Regex.Match(sql, @"(\w)+", RegexOptions.IgnoreCase);
                        column.dbType = match.Value;
                        int typeindex = -1;
                        for (int i = 0; i < Database.Sqlite.SqliteTableService.ColumnType.Count; i++)
                        {
                            if (string.Equals(Database.Sqlite.SqliteTableService.ColumnType[i], column.dbType, StringComparison.CurrentCultureIgnoreCase))
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

                        sql = sql.Substring(match.Index + match.Length);
                        if(sql.StartsWith("("))
                        {
                            column.length = sql.Substring(0 , sql.IndexOf(")"));
                            column.length = column.length.Substring(1);

                            sql = sql.Substring(sql.IndexOf(")") + 1);
                        }
                        match = Regex.Match(sql, @"DEFAULT( )+", RegexOptions.IgnoreCase);
                        if(match != null && match.Length > 0)
                        {
                            column.defaultValue = sql.Substring(match.Index + match.Length).Trim();
                            if(column.defaultValue.StartsWith("'"))
                            {
                                column.defaultValue = column.defaultValue.Substring(1, column.defaultValue.LastIndexOf("'") - 1);
                                sql = sql.Replace("'" + column.defaultValue + "'", "");
                                column.defaultValue = column.defaultValue.Replace("''", "'");
                            }
                            else
                            {
                                column.defaultValue = Regex.Match(column.defaultValue, @"(\w)+").Value;
                            }
                        }
                        column.CanNull = !sql.ToUpper().Contains("NOT NULL");
                        column.IsPKID = sql.ToUpper().Contains("PRIMARY KEY");
                        column.IsAutoIncrement = sql.ToUpper().Contains("AUTOINCREMENT");

                        column.ChangedProperties.Clear();
                        result.Add(column);
                    }
                }
               
            }
            return result;
        }
        public List<string> GetCurrentTableNames(IDatabaseService db)
        {
            List<string> result = new List<string>();
            db.ExecuteReader((reader) => {
                result.Add(reader[0].ToSafeString());
                return true;
            }, "select name from sqlite_master where type='table' and name<>'__wayeasyjob'  COLLATE NOCASE and name<>'sqlite_sequence' order by name");

            return result;
        }
        public void ChangeName(EJ.Databases database, string newName, string newConnectString)
        {
            var dbnameMatch_old = System.Text.RegularExpressions.Regex.Match(database.conStr, @"Data Source=(?<dname>(\w|\:|\\)+)", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            var dbnameMatch = System.Text.RegularExpressions.Regex.Match(newConnectString, @"Data Source=(?<dname>(\w|\:|\\)+)", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            string newfilepath = dbnameMatch.Groups["dname"].Value;
            string oldfilepath = dbnameMatch_old.Groups["dname"].Value;
            System.IO.File.Move(oldfilepath, newfilepath);
        }

        public void GetViews(EntityDB.IDatabaseService db, out List<EJ.DBTable> tables, out List<EJ.DBColumn> columns)
        {
            tables = new List<EJ.DBTable>();
            columns = new List<EJ.DBColumn>();
        }


        public string GetObjectFormat()
        {
            return "[{0}]";
        }
    }
}
 