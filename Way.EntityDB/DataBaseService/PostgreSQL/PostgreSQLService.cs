using Microsoft.EntityFrameworkCore;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Way.EntityDB
{
    [Attributes.DatabaseTypeAttribute(DatabaseType.PostgreSQL)]
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
            sql = string.Format("SELECT * FROM  ({0}) as t1 ORDER BY 1   OFFSET {1} ROWS FETCH NEXT {2} ROWS ONLY", sql, skip, take);
            return SelectTable(sql, sqlparameters);
        }


        public override string FormatObjectName(string name)
        {
            if (name.StartsWith("[") || name.StartsWith("("))
                return name;
            return string.Format("[{0}]", name);
        }
        protected override string GetInsertIDValueSqlString()
        {
            return null;
        }

        protected override void ThrowSqlException(Type tableType, Exception ex)
        {
            if (!(ex is NpgsqlException))
                throw ex;
            throw ex;
        }



    }
}
