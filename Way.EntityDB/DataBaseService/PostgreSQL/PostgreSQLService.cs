using Microsoft.EntityFrameworkCore;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Way.EntityDB
{
    [Attributes.DatabaseTypeAttribute(DatabaseType.PostgreSql)]
    class PostgreSQLService :SqliteService
    {

        public PostgreSQLService()
        {
        }
        public PostgreSQLService(DBContext dbcontext):base(dbcontext)
         {

        }

        public override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql(this.ConnectionString);
        }
      

        public override void AllowIdentityInsert(string tablename, bool allow)
        {
           
        }
        public override WayDataTable SelectTable(string sql, int skip, int take, params object[] sqlparameters)
        {
            sql = $"select * from ({sql}) as t1 limit {take} offset {skip}";
            return SelectTable(sql, sqlparameters);
        }


        public override string FormatObjectName(string name)
        {
            return $"\"{name}\"";
        }
        protected override bool GetInsertIDValueSqlStringInOneSql()
        {
            return true;
        }
        protected override string GetInsertIDValueSqlString(string pkColumnName)
        {
            if (pkColumnName.IsNullOrEmpty())
                return null;
            return $"RETURNING {pkColumnName}";
        }

        protected override void ThrowSqlException(Type tableType, Exception ex)
        {
            if (!(ex is NpgsqlException))
                throw ex;
            PostgresException nerror = ex as PostgresException;
            if(nerror.SqlState != "23505" || nerror.Detail.Contains(" already exists") == false)
                throw ex;

            StringBuilder output = new StringBuilder();
            string[] captions = null;
            string[] keys = null;
            try
            {
                string msg = nerror.Detail;


                try
                {
                    var match = Regex.Match(msg, @"Key \((?<c>(\w| |,)+)\)");
                    if (match != null && match.Length > 0)
                    {
                        string indexname = match.Groups["c"].Value;
                        keys = indexname.Split(',');
                        keys = (from m in keys
                                select m.Trim()).ToArray();

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
