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
        public virtual System.Data.Common.DbConnection CreateConnection(string connectString)
        {
            return new SqliteConnection(connectString);
        }
        public virtual void AllowIdentityInsert(string tablename, bool allow)
        {

        }
        public virtual string FormatObjectName(string name)
        {
            if (name.StartsWith("[") || name.StartsWith("("))
                return name;
            return string.Format("[{0}]", name);
        }
        protected virtual string GetInsertIDValueSqlString()
        {
            return "select last_insert_rowid()";
        }
        protected virtual System.Data.Common.DbCommand CreateCommand(string sql , params object[] parames)
        {
            var cmd = this.Connection.CreateCommand();
            if (_database.CurrentTransaction != null)
            {
                cmd.Transaction = (System.Data.Common.DbTransaction)_database.CurrentTransaction.GetDbTransaction();
            }
            cmd.CommandText = sql;
            if (parames != null && parames.Length > 0)
            {
                for (int i = 0; i < parames.Length; i++)
                {
                    cmd.Parameters.Add(CreateParameter("@p" + i, parames[i]));
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
        protected virtual System.Data.Common.DbParameter CreateParameter(string name,object value)
        {
            return new SqliteParameter(name , value);
        }
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

                        output.Append(columnAtt.Caption);
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
          
            string pkid = dataitem.PKIDField;
            var fieldValues = dataitem.GetFieldValues(true);
            if (fieldValues.Length == 0)
                return;

            StringBuilder str_fields = new StringBuilder();
            StringBuilder str_values = new StringBuilder();
            List<System.Data.Common.DbParameter> parameters = new List<System.Data.Common.DbParameter>();
            int parameterIndex = 1;

            foreach (var field in fieldValues)
            {

                if (str_fields.Length > 0)
                    str_fields.Append(',');
                str_fields.Append(FormatObjectName(field.FieldName));

                string parameterName = "@p" + (parameterIndex++);
                var parameter = CreateParameter(parameterName, field.Value);
                parameters.Add(parameter);

                if (str_values.Length > 0)
                    str_values.Append(',');
                str_values.Append(parameterName);
            }
            try
            {
                string sql = string.Format("insert into {0} ({1}) values ({2})", FormatObjectName(dataitem.TableName), str_fields, str_values);
                using (var command = CreateCommand(sql))
                {
                    foreach (var p in parameters)
                    {
                        command.Parameters.Add(p);
                    }
                    parameters.Clear();
                    command.ExecuteNonQuery();

                    command.Parameters.Clear();
                    command.CommandText = this.GetInsertIDValueSqlString();
                    object id = command.ExecuteScalar();

                    if (id != null && !string.IsNullOrEmpty(pkid))
                    {
                        dataitem.SetValue(pkid, id);

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


            string pkid = dataitem.PKIDField;
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
                List<System.Data.Common.DbParameter> parameters = new List<System.Data.Common.DbParameter>();
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
                        var parameter = CreateParameter(parameterName, value);
                        parameters.Add(parameter);

                        str_fields.Append(parameterName);

                    }
                }
                
                string sql;
                if (pkvalue != null)
                {
                    parameters.Add(CreateParameter("@pid", pkvalue));
                    sql = string.Format("update {0} set {1} where {2}=@pid", FormatObjectName(dataitem.TableName), str_fields, FormatObjectName(pkid));
                }
                else
                {
                    sql = string.Format("update {0} set {1}", FormatObjectName(dataitem.TableName), str_fields);
                }
                using (var command = CreateCommand(sql))
                {
                    foreach (var p in parameters)
                    {
                        command.Parameters.Add(p);
                    }
                    parameters.Clear();
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
                string sql = string.Format("delete from {0} where {1}=@p0", FormatObjectName(dataitem.TableName), FormatObjectName(dataitem.PKIDField));
                using (var command = CreateCommand(sql))
                {
                    command.Parameters.Add(CreateParameter("@p0", dataitem.PKValue));
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
