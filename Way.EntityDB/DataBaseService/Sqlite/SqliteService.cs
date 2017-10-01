using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Reflection;
using Microsoft.EntityFrameworkCore.Storage;

namespace Way.EntityDB
{
    [Attributes.DatabaseTypeAttribute( DatabaseType.Sqlite)]
    class SqliteService:IDatabaseService
    {
        protected DatabaseFacade _database;
        DBContext _dbcontext;

        public virtual void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite(this.ConnectionString);
        }

        public System.Data.Common.DbConnection Connection
        {
            get
            {
                return ((Microsoft.EntityFrameworkCore.DbContext)_dbcontext).Database.GetDbConnection();
            }
        }
         public SqliteService()
        {
        }
         public SqliteService(DBContext dbcontext)
         {
             _dbcontext = dbcontext;
             _database = ((Microsoft.EntityFrameworkCore.DbContext)dbcontext).Database;
        }
        //public virtual System.Data.Common.DbConnection CreateConnection(string connectString)
        //{
        //    return new SqliteConnection(connectString);
        //}
        public virtual void AllowIdentityInsert(string tablename, bool allow)
        {

        }
        public virtual string FormatObjectName(string name)
        {
            if (name.StartsWith("[") || name.StartsWith("("))
                return name;
            return string.Format("[{0}]", name);
        }
        /// <summary>
        /// GetInsertIDValueSqlString是否放在一个sql语句
        /// </summary>
        /// <returns></returns>
        protected virtual bool GetInsertIDValueSqlStringInOneSql()
        {
            return false;
        }
        protected virtual string GetInsertIDValueSqlString(string pkColumnName)
        {
            return "select last_insert_rowid()";
        }
        protected virtual System.Data.Common.DbCommand CreateCommand(string sql , params object[] parames)
        {
            var cmd = this.Connection.CreateCommand();
            if (_database.CurrentTransaction != null && cmd.Transaction == null)
            {
                cmd.Transaction = (System.Data.Common.DbTransaction)_database.CurrentTransaction.GetDbTransaction();
            }
            if (sql != null)
            {
                cmd.CommandText = sql;
            }
            if (parames != null && parames.Length > 0)
            {
                for (int i = 0; i < parames.Length; i++)
                {
                    var sqlParameter = cmd.CreateParameter();
                    sqlParameter.ParameterName = "@p" + i;
                    sqlParameter.Value = parames[i];
                    cmd.Parameters.Add(sqlParameter);
                }
            }
            return cmd;
        }
#if NET46
        protected virtual System.Data.Common.DbDataAdapter CreateDataAdapter(string sql)
        {
            return new mySQLiteDataAdapter((SqliteCommand)this.CreateCommand(sql));
        }
#endif
     
        protected virtual void ThrowSqlException(Type tableType, Exception ex)
        {
            if (!(ex is SqliteException))
                throw ex;
            if (((SqliteException)ex).SqliteErrorCode != 19)
                throw ex;

            List<string> keys = new List<string>();
            string[] captions = null;
            StringBuilder output = new StringBuilder();
            try
            {
                string msg = ex.Message.Substring(ex.Message.IndexOf("UNIQUE constraint failed:") + "UNIQUE constraint failed:".Length);

                try
                {
                    var matches = Regex.Matches(msg, @"(\w|\.)+");
                    string tableName = null;
                    foreach (Match columninfo in matches)
                    {
                        string[] arr = columninfo.Value.Split('.');
                        if (tableName == null)
                            tableName = arr[0];
                        string columnName = arr[1];
                        keys.Add(columnName);
                    }
                    captions = new string[keys.Count];

                    for (int i = 0; i < keys.Count; i++)
                    {
                        var pinfo = tableType.GetProperty(keys[i]);
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

            }
            catch
            {
                throw ex;
            }
            throw new RepeatValueException(keys.ToArray(), captions, "此" + output + "已存在");
        }

        public virtual void Insert(DataItem dataitem)
        {
            bool needToClose = false;
            if (this.Connection.State != System.Data.ConnectionState.Open)
            {
                needToClose = true;
                this.Connection.Open();
            } 
          
            string pkid = dataitem.KeyName;
            var fieldValues = dataitem.GetFieldValues(true);
            if (fieldValues.Length == 0)
                return;

            StringBuilder str_fields = new StringBuilder();
            StringBuilder str_values = new StringBuilder();
           
            try
            {
              
                using (var command = CreateCommand(null))
                {
                    int parameterIndex = 1;
                    foreach (var field in fieldValues)
                    {

                        if (str_fields.Length > 0)
                            str_fields.Append(',');
                        str_fields.Append(FormatObjectName(field.FieldName));


                        var parameter = command.CreateParameter();
                        parameter.ParameterName = "@p" + (parameterIndex++);
                        parameter.Value = field.Value;
                        command.Parameters.Add(parameter);

                        if (str_values.Length > 0)
                            str_values.Append(',');
                        str_values.Append(parameter.ParameterName);
                    }

                    string sql;
                    if( GetInsertIDValueSqlStringInOneSql() )
                    {
                        sql = string.Format("insert into {0} ({1}) values ({2}) {3}", FormatObjectName(dataitem.TableName), str_fields, str_values , this.GetInsertIDValueSqlString(pkid));
                        command.CommandText = sql;
                        object id = command.ExecuteScalar();

                        command.Parameters.Clear();
                        if (id != null && !string.IsNullOrEmpty(pkid))
                        {
                            dataitem.SetValue(pkid, id);
                        }
                    }
                    else
                    {
                        sql = string.Format("insert into {0} ({1}) values ({2})", FormatObjectName(dataitem.TableName), str_fields, str_values);
                        command.CommandText = sql;
                        command.ExecuteNonQuery();

                        command.Parameters.Clear();
                        sql = this.GetInsertIDValueSqlString(pkid);
                        if (sql != null)
                        {
                            command.CommandText = sql;
                            object id = command.ExecuteScalar();

                            if (id != null && !string.IsNullOrEmpty(pkid))
                            {
                                dataitem.SetValue(pkid, id);
                            }
                        }
                    }
                   
                }
                dataitem.ChangedProperties.Clear();
            }
            catch (Exception ex)
            {
                this.ThrowSqlException(dataitem.TableType, ex);
            }
            finally
            {
                if (needToClose)
                {
                    this.Connection.Close();
                }
            }

        }

        public virtual void Update(DataItem dataitem)
        {


            string pkid = dataitem.KeyName;
            object pkvalue = dataitem.PKValue;
            if (pkvalue == null && pkid != null)
            {
                Insert(dataitem);
                return;
            }


            bool needToClose = false;
            if (this.Connection.State != System.Data.ConnectionState.Open)
            {
                needToClose = true;
                this.Connection.Open();
            } 

            try
            {


                StringBuilder str_fields = new StringBuilder();
                int parameterIndex = 1;
               
                
                using (var command = CreateCommand(null))
                {
                    var fieldValues = dataitem.GetFieldValues(false);
                    if (fieldValues.Length == 0)
                        return;
                    foreach (var fieldValue in fieldValues)
                    {

                        if (str_fields.Length > 0)
                            str_fields.Append(',');
                        str_fields.Append(FormatObjectName(fieldValue.FieldName));
                        str_fields.Append('=');


                        object value = fieldValue.Value;
                        if (value == DBNull.Value || value == null)
                        {
                            str_fields.Append("null");

                        }
                        else
                        {
                            string parameterName = "@p" + (parameterIndex++);
                            var parameter = command.CreateParameter();
                            parameter.ParameterName = parameterName;
                            parameter.Value = value;
                            command.Parameters.Add(parameter);

                            str_fields.Append(parameterName);

                        }
                    }

                    if (pkvalue != null)
                    {
                        var parameter = command.CreateParameter();
                        parameter.ParameterName = "@pid";
                        parameter.Value = pkvalue;
                        command.Parameters.Add(parameter);

                        command.CommandText = string.Format("update {0} set {1} where {2}=@pid", FormatObjectName(dataitem.TableName), str_fields, FormatObjectName(pkid.ToLower()));
                    }
                    else
                    {
                        command.CommandText = string.Format("update {0} set {1}", FormatObjectName(dataitem.TableName), str_fields);
                    }
                    command.ExecuteNonQuery();

                    command.Parameters.Clear();
                }
            }
            catch (Exception ex)
            {
                ThrowSqlException(dataitem.TableType, ex);
            }
            finally
            {
                if (needToClose)
                {
                    this.Connection.Close();
                }
            }

        }
        public virtual void Delete(DataItem dataitem)
        {
            bool needToClose = false;
            if (this.Connection.State != System.Data.ConnectionState.Open)
            {
                needToClose = true;
                this.Connection.Open();
            }

            try
            {
                using (var command = CreateCommand(null))
                {
                    command.CommandText = string.Format("delete from {0} where {1}=@p0", FormatObjectName(dataitem.TableName), FormatObjectName(dataitem.KeyName.ToLower()));
                    var parameter = command.CreateParameter();
                    parameter.ParameterName = "@p0";
                    parameter.Value = dataitem.PKValue;
                    command.Parameters.Add(parameter);
                    command.ExecuteNonQuery();
                }
            }
            catch
            {
                throw;
            }
            finally
            {
                if (needToClose)
                {
                    this.Connection.Close();
                }
            }

        }


        public object ExecSqlString(string sql,params object[] parames)
        {
            bool needToClose = false;
            if (this.Connection.State != System.Data.ConnectionState.Open)
            {
                needToClose = true;
                this.Connection.Open();
            } 
            try
            {
                var command = CreateCommand(sql, parames);
                {
                    return command.ExecuteScalar();
                }
            }
            catch(Exception ex)
            {
                throw new Exceptons.SqlExecException(ex.Message ,sql, ex);
            }
            finally
            {
                if (needToClose)
                {
                    this.Connection.Close();
                }
            }
        }


        public WayDataTable SelectTable(string sql, params object[] parames)
        {
            bool needToClose = false;
            if (this.Connection.State != System.Data.ConnectionState.Open)
            {
                needToClose = true;
                this.Connection.Open();
            }
            try
            {
                using (var command = this.CreateCommand(sql,parames))
                {
                    var dtable = new WayDataTable();
                    using (var reader = command.ExecuteReader())
                    {
                        for (int i = 0; i < reader.FieldCount; i++)
                        {
                            string name = reader.GetName(i);
                            dtable.Columns.Add(new WayDataColumn(name, reader.GetFieldType(i).FullName));
                        }

                        while (reader.Read())
                        {
                            var row = new WayDataRow();
                            dtable.Rows.Add(row);
                            for (int i = 0; i < reader.FieldCount; i++)
                            {
                                string name = reader.GetName(i);
                                row[name] = reader[i];
                                if(reader[i] != null)
                                {
                                    string typename = reader.GetFieldType(i).FullName;
                                    if (typename != dtable.Columns[i].DataType)
                                        dtable.Columns[i].DataType = typename;
                                }
                            }
                        }
                    }
                        return dtable;
                }
            }
            catch
            {
                throw;
            }
            finally
            {
                if (needToClose)
                {
                    this.Connection.Close();
                }
            }
        }
        public WayDataSet SelectDataSet(string sql, params object[] parames)
        {
            bool needToClose = false;
            if (this.Connection.State != System.Data.ConnectionState.Open)
            {
                needToClose = true;
                this.Connection.Open();
            }
            try
            {
                using (var command = this.CreateCommand(sql, parames))
                {
                    var dataset = new WayDataSet();
                    using (var datareader = command.ExecuteReader())
                    {
                        do
                        {
                            if (datareader.HasRows)
                            {
                                var datatable = new WayDataTable();
                                for (int i = 0; i < datareader.FieldCount; i++)
                                {
                                    string name = datareader.GetName(i);
                                    datatable.Columns.Add(new WayDataColumn(name,datareader.GetFieldType(i).FullName));
                                }
                                dataset.Tables.Add(datatable);
                                while (datareader.Read())
                                {
                                    var row = new WayDataRow();
                                    datatable.Rows.Add(row);

                                    for (int i = 0; i < datareader.FieldCount; i++)
                                    {
                                        string name = datareader.GetName(i);
                                        row[name] = datareader[i];
                                    }
                                }
                            }
                        }
                        while (datareader.NextResult());
                    }
                    return dataset;
                }
              
            }
            catch
            {
                throw;
            }
            finally
            {
                if (needToClose)
                {
                    this.Connection.Close();
                }
            }
        }

        public virtual WayDataTable SelectTable(string sql, int skip, int take, params object[] sqlparameters)
        {
            sql = string.Format("select * from ({0}) as t1 limit {1},{2}", sql, skip, take);
            return SelectTable(sql, sqlparameters);
        }

        public void ExecuteReader(Func<System.Data.IDataReader,bool> func , string sql, params object[] parames)
        {
            bool needToClose = false;
            if (this.Connection.State != System.Data.ConnectionState.Open)
            {
                needToClose = true;
                this.Connection.Open();
            }
            try
            {
                using (var command = this.CreateCommand(sql, parames))
                {
                    var dataset = new WayDataSet();
                    using (var datareader = command.ExecuteReader())
                    {

                        while (datareader.Read())
                        {
                            if (func(datareader) == false)
                                break;
                        }
                        
                    }
                }

            }
            catch
            {
                throw;
            }
            finally
            {
                if (needToClose)
                {
                    this.Connection.Close();
                }
            }
        }

       

        public DBContext DBContext
        {
            get { return _dbcontext; }
        }

        public string ConnectionString
        {
            get
            {
                return _dbcontext.ConnectionString;
            }
        }

      
    }

#if NET46
    class mySQLiteDataAdapter : System.Data.Common.DbDataAdapter
    {
        public mySQLiteDataAdapter(SqliteCommand command)
        {
            this.SelectCommand = command;
        }

    }
#endif
}
