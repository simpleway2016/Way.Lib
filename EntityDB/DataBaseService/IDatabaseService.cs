using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityDB
{
    public interface IDatabaseService
    {
        DBContext DBContext
        {
            get;
        }
        System.Data.Common.DbConnection Connection
        {
            get;
        }
        String ConnectionString
        {
            get;
        }
        System.Data.Common.DbConnection CreateConnection(string connectString);
        void Insert(DataItem dataitem);
        void Update(DataItem dataitem);
        void Delete(DataItem dataitem);
        /// <summary>
        /// 执行sql语句
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="sqlparameters">sql参数，分别对应@p0、@p1等</param>
        /// <returns></returns>
        object ExecSqlString(string sql,params object[] sqlparameters);
        /// <summary>
        /// 通过sql语句，读取DataTable
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="sqlparameters"></param>
        /// <returns></returns>
        DataTable SelectTable(string sql, params object[] sqlparameters);
        /// <summary>
        /// 通过sql语句，读取DataSet
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="sqlparameters"></param>
        /// <returns></returns>
        DataSet SelectDataSet(string sql, params object[] sqlparameters);
        /// <summary>
        /// 格式化文字，如tableName，可能需要变为[tableName]
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        string FormatObjectName(string name);
        /// <summary>
        /// 通过sql语句，读取DataTable
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="skip">跳过几天记录</param>
        /// <param name="take">读取几条记录</param>
        /// <param name="sqlparameters">sql参数，分别对应@p0、@p1等</param>
        /// <returns></returns>
        DataTable SelectTable(string sql,int skip,int take, params object[] sqlparameters);
    }
}
