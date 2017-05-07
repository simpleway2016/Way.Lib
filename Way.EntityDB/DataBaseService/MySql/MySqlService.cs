using Pomelo.Data.MySql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Reflection;
using Microsoft.EntityFrameworkCore;

namespace Way.EntityDB
{
    [Attributes.DatabaseTypeAttribute(DatabaseType.MySql)]
    class MySqlService:SqliteService
    { 

         public MySqlService()
        {
        }
         public MySqlService(DBContext dbcontext):base(dbcontext)
         {
            
        }
        public override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseMySql(this.ConnectionString);
        }
        public override string FormatObjectName(string name)
        {
            if (name.StartsWith("`") || name.StartsWith("("))
                return name;
            return string.Format("`{0}`", name);
        }
        protected override string GetInsertIDValueSqlString(string pkColumnName)
        {
            return "SELECT LAST_INSERT_ID()";
        }
      

        public override void AllowIdentityInsert(string tablename, bool allow)
        {
            
        }
        public override WayDataTable SelectTable(string sql, int skip, int take, params object[] sqlparameters)
        {
            sql = string.Format("select * from ({0}) as t1 limit {1},{2}", sql, skip, take);
            return SelectTable(sql, sqlparameters);
        }

      
        protected override void ThrowSqlException(Type tableType, Exception ex)
        {
            if (!(ex is MySql.Data.MySqlClient.MySqlException))
                throw ex;
            if (((MySql.Data.MySqlClient.MySqlException)ex).Number != 1062)
                throw ex;

            StringBuilder output = new StringBuilder();
            string[] captions = null;
            string[] keys = null;
            try
            {
                string msg = ex.Message;


                try
                {
                    var matches = Regex.Matches(msg, @"for key \'(?<n>(\w|\.|_)+)\'");
                    if (matches.Count > 0)
                    {
                        string indexname = matches[matches.Count - 1].Groups["n"].Value;
                        keys = indexname.Substring(indexname.IndexOf("_ej_") + 4).Split('_');

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
                }
                catch (Exception ee)
                {
                    throw ex;
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
