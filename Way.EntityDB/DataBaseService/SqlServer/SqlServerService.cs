using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Reflection;
using Microsoft.EntityFrameworkCore;

namespace Way.EntityDB
{
    [Attributes.DatabaseTypeAttribute(DatabaseType.SqlServer)]
    class SqlServerService : SqliteService
    {

         public SqlServerService()
        {
        }
         public SqlServerService(DBContext dbcontext):base(dbcontext)
         {
            
        }
        public override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(this.ConnectionString);
        }
      

        public override void AllowIdentityInsert(string tablename, bool allow)
        {
            this.ExecSqlString($"SET IDENTITY_INSERT [{tablename}] {(allow ? "on":"off")}");
        }
        public override WayDataTable SelectTable(string sql, int skip, int take, params object[] sqlparameters)
        {
            sql = string.Format("SELECT * FROM  ({0}) as t1 ORDER BY 1   OFFSET {1} ROWS FETCH NEXT {2} ROWS ONLY", sql, skip, take);
            return SelectTable(sql, sqlparameters);
        }

      
        public override string FormatObjectName(string name)
        {
            if (name.StartsWith("[") || name.StartsWith("("))
                return name;
            return string.Format("[{0}]", name);
        }
        protected override string GetInsertIDValueSqlString(string pkColumnName)
        {
            return "select Scope_Identity()";
        }
       
        protected override void ThrowSqlException(Type tableType, Exception ex)
        {
            if (!(ex is System.Data.SqlClient.SqlException))
                throw ex;
            if (((System.Data.SqlClient.SqlException)ex).Number != 2601)
                throw ex;

            string[] keys = null;
            string[] captions = null;
            StringBuilder output = new StringBuilder();
            try
            {
                string msg = ex.Message;
                var matches = Regex.Matches(msg, @"\'(\w|\.)+\'");
                string idxName = matches[0].Value.Substring(1, matches[0].Value.Length - 2);
                string tableName = matches[1].Value.Replace("dbo.", "");
                tableName = tableName.Substring(1, tableName.Length - 2);

                using (var sp_helpResult = this.SelectDataSet("sp_help [" + tableName + "]"))
                {
                    foreach (var dtable in sp_helpResult.Tables)
                    {
                        if (dtable.Columns.Any(m=>m.ColumnName == "index_keys"))
                        {
                            var row = dtable.Rows.SingleOrDefault( m=>m["index_name"].Equals(idxName) );
                            keys = row["index_keys"].ToString().Split(',');
                            break;
                        }
                    }
                }
                for (int i = 0; i < keys.Length; i++)
                    keys[i] = keys[i].Trim();
                captions = new string[keys.Length];

                for (int i = 0; i < keys.Length; i++)
                {
                    var pinfo = tableType.GetTypeInfo().GetProperty(keys[i]);
                    WayDBColumnAttribute columnAtt = pinfo.GetCustomAttribute(typeof(WayDBColumnAttribute)) as WayDBColumnAttribute;
                    captions[i] = columnAtt.Caption;
                    if (output.Length > 0)
                        output.Append(',');

                    output.Append(columnAtt.Caption.IsNullOrEmpty() ? keys[i] : columnAtt.Caption);
                }

            }
            catch
            {
                throw ex;
            }
            throw new RepeatValueException(keys, captions, "此" + output + "已存在");
        }



    }
}
