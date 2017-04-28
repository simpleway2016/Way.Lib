using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Way.EntityDB.Exceptons
{
    public class SqlExecException : Exception
    {
        public string SqlString
        {
            get;
            private set;
        }
        public SqlExecException(string msg , string sql , Exception innerex):base(msg , innerex)
        {
            this.SqlString = sql;
        }
    }
}
