
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
                db.ExecSqlString("select * from __WayEasyJob");
            }
            catch
            {
                exists = false;
            }
            if (!exists)
            {
                db.ExecSqlString("CREATE TABLE [__WayEasyJob](contentConfig TEXT  NOT NULL)");
                var dbconfig = new DataBaseConfig();
                try
                {
                    dbconfig.LastUpdatedID = Convert.ToInt32( db.ExecSqlString("select lastID from __EasyJob"));
                }
                catch
                {
                }
                db.ExecSqlString("insert into __WayEasyJob (contentConfig) values (@p0)", dbconfig.ToJsonString());
               
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
            return null;
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