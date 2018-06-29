using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations.Operations;
using System;
using System.Collections.Generic;
using System.Text;
using Way.EntityDB.Design.Services;
using System.Linq;
using Way.EntityDB.Design.Actions;

namespace Way.EntityDB.Design.Impls.Sqlite
{
    class TempDBContext : DbContext, ITempDBContext
    {
        internal static List<string> ColumnType = new List<string>(new string[] {
                                            "VARCHAR",
                                            "INTEGER",
                                            "BLOB",//image
                                            "TEXT",
                                            "SMALLINT",//smallint
                                            "DATETIME",//smalldatetime
                                            "REAL",
                                            "DATETIME",//datetime
                                            "FLOAT",
                                            "DOUBLE",
                                            "BOOLEAN",
                                            "DECIMAL",//decimal
                                            "NUMERIC",
                                            "BIGINT",//bigint
                                            "BLOB",//varbinary
                                            "CHARACTER",
                                            "DATETIME", });

        public void AfterAction(IDatabaseService invokingDB)
        {
            if (_reCreateTable)
            {
                //把旧表数据拷贝到新表
                invokingDB.ExecSqlString("insert into [" + _newTableName + "] (" + _newfields + ") select " + _oldfields + " from [" + _oldTableName + "]");

                invokingDB.ExecSqlString("DROP TABLE [" + _oldTableName + "]");
            }
        }
        void deleteAllIndex(EntityDB.IDatabaseService database, string tableName)
        {
            tableName = tableName.ToLower();
            using (var dtable = database.SelectTable("select * from sqlite_master where type='index' and tbl_name='" + tableName + "' "))
            {
                foreach (var drow in dtable.Rows)
                {
                    database.ExecSqlString("DROP INDEX IF EXISTS [" + drow["name"] + "]");
                }
            }
        }
        public void BeforeAction(IDatabaseService invokingDB)
        {
            if (_reCreateTable)
            {
                string changetoname = _oldTableName + "_2";
                while (Convert.ToInt32(invokingDB.ExecSqlString("select count(*) from sqlite_master where type='table' and name='" + changetoname + "'")) > 0)
                    changetoname = changetoname + "_2";

                //删除索引
                deleteAllIndex(invokingDB, _oldTableName);

                invokingDB.ExecSqlString("ALTER TABLE [" + _oldTableName + "] RENAME TO [" + changetoname + "]");
                _oldTableName = changetoname;

            }
        }

        bool _reCreateTable = false;
        string _oldTableName;
        string _newTableName;
        StringBuilder _newfields;
        StringBuilder _oldfields;
        public MigrationOperation[] CheckOperations(List<MigrationOperation> operations, IDatabaseService invokingDB)
        {
            //sqlite 不支持column的修改      
            bool isCreateTable = operations.Any(m=>m is CreateTableOperation);
            string tableName = null;
            
            foreach (var o in operations)
            {
                if (o is DropColumnOperation)
                {
                    tableName = ((DropColumnOperation)o).Table;
                    break;
                }
                else if (o is RenameColumnOperation)
                {
                    tableName = ((RenameColumnOperation)o).Table;
                    break;
                }
                else if (o is AlterColumnOperation)
                {
                    tableName = ((AlterColumnOperation)o).Table;
                    break;
                }
            }
            _newTableName = tableName;
            List<EJ.DBColumn> nowColumns = null;
            List<EJ.DBColumn> originalColumns = null;
            List<IndexInfo> nowIndexes = null;
            List<CreateIndexOperation> newIndexOperations = new List<CreateIndexOperation>();
            List<AddColumnOperation> newColumnsOperations = new List<AddColumnOperation>();
            if (tableName != null)
            {
                _reCreateTable = true;
                var dbDesignService = DBHelper.CreateDatabaseDesignService(invokingDB.DBContext.DatabaseType);
                //取出目前数据库所有字段描述
                nowColumns = dbDesignService.GetCurrentColumns(invokingDB, tableName);
                originalColumns = new List<EJ.DBColumn>();
                originalColumns.AddRange(nowColumns);
                nowIndexes = dbDesignService.GetCurrentIndexes(invokingDB, tableName);
            }

            foreach (var o in operations)
            {
                if (o is DropColumnOperation)
                {
                    var op = o as DropColumnOperation;
                   
                    var column = nowColumns.FirstOrDefault(m=>m.Name == op.Name);
                    if (column != null)
                    {
                        nowColumns.Remove(column);
                        originalColumns.Remove(column);
                    }
                }
                else if (o is RenameColumnOperation)
                {
                    var op = o as RenameColumnOperation;

                    var column = nowColumns.FirstOrDefault(m => m.Name == op.Name);
                    if (column != null)
                    {
                        column.BackupChangedProperties["copyfrom"] = new DataValueChangedItem() { OriginalValue = column.Name };
                        column.Name = op.NewName;                        
                    }
                }
                else if (o is AlterColumnOperation)
                {
                    var op = o as AlterColumnOperation;

                    var column = nowColumns.FirstOrDefault(m => m.Name == op.Name);
                    if (column != null)
                    {
                        column.dbType = op.ColumnType;
                        column.defaultValue = (string)op.DefaultValue;
                        column.CanNull = op.IsNullable;
                    }
                }
                else if(o is DropIndexOperation)
                {
                    var op = o as DropIndexOperation;
                    if (nowIndexes != null)
                    {
                        var item = nowIndexes.FirstOrDefault(m => m.Name == op.Name);
                        if(item != null)
                        {
                            nowIndexes.Remove(item);
                        }
                    }
                }
                else if(o is CreateIndexOperation)
                {
                    newIndexOperations.Add((CreateIndexOperation)o);
                }
                else if (o is AddColumnOperation)
                {
                    newColumnsOperations.Add((AddColumnOperation)o);
                }
                else if (o is RenameTableOperation)
                {
                    _newTableName = ((RenameTableOperation)o).NewName;
                }
            }

            if(tableName != null)
            {
                //获取原来所有字段
                _newfields = new StringBuilder();
                _oldfields = new StringBuilder();
                foreach( var o in originalColumns )
                {
                    if(_oldfields.Length > 0)
                    {
                        _oldfields.Append(',');
                        _newfields.Append(',');
                    }
                    if(o.BackupChangedProperties["copyfrom"] != null)
                    {
                        _oldfields.Append($"[{o.BackupChangedProperties["copyfrom"]}]");
                        _newfields.Append($"[{o.Name}]");
                    }
                    else
                    {
                        _oldfields.Append($"[{o.Name}]");
                        _newfields.Append($"[{o.Name}]");
                    }
                }
                 

                //需要重新建表
                operations.Clear();

                var _CreateTableOperation = new CreateTableOperation();
                operations.Add(_CreateTableOperation);
                _CreateTableOperation.Name = _newTableName;
                var pkColumns = nowColumns.Where(m => m.IsPKID == true).Select(m => m.Name.ToLower()).ToArray();
                if (pkColumns.Length > 0)
                {
                    _CreateTableOperation.PrimaryKey = new AddPrimaryKeyOperation();
                    _CreateTableOperation.PrimaryKey.Columns = pkColumns;
                }
                

                foreach (var column in nowColumns)
                {
                    var _AddColumnOperation = new AddColumnOperation()
                    {
                        Table = _CreateTableOperation.Name,
                        ClrType = EF_CreateTable_Action.GetCSharpType(column.dbType),
                        ColumnType = column.dbType,
                        DefaultValue = column.defaultValue,
                        IsUnicode = true,
                        IsNullable = column.CanNull.GetValueOrDefault(),
                        Name = column.Name.ToLower(),
                    };
                    if (!string.IsNullOrEmpty(column.length))
                    {
                        //借用ComputedColumnSql字段存放length
                        _AddColumnOperation.ComputedColumnSql = column.length;
                    }
                    _CreateTableOperation.Columns.Add(_AddColumnOperation);
                }
                _CreateTableOperation.Columns.AddRange(newColumnsOperations);
                operations.AddRange(newIndexOperations);

                var idColumns = nowColumns.Where(m => m.IsAutoIncrement == true).Select(m => m.Name.ToLower()).ToArray();
                if (idColumns.Length > 0)
                {
                   foreach( var idc in idColumns )
                    {
                        var _CreateSequenceOperation = new CreateSequenceOperation() {
                            StartValue = 1,
                            Name = idc,
                        };
                        operations.Add(_CreateSequenceOperation);
                    }
                }

                foreach (var indexCfg in nowIndexes)
                {
                    var keynames = indexCfg.ColumnNames;
                    var _CreateIndexOperation = new CreateIndexOperation();
                    _CreateIndexOperation.Table = _CreateTableOperation.Name;
                    _CreateIndexOperation.Name = indexCfg.Name;
                    _CreateIndexOperation.Columns = keynames.Select(m => m.ToLower()).ToArray();
                    _CreateIndexOperation.IsUnique = indexCfg.IsUnique;

                    operations.Add(_CreateIndexOperation);
                }

            }
            return operations.ToArray();
        }

        public string GetColumnType(string dbtype, string length)
        {
            int index = Design.ColumnType.SupportTypes.IndexOf(dbtype);
            if (index < 0 || ColumnType[index] == null)
                throw new Exception($"不支持字段类型{dbtype}");
            dbtype = ColumnType[index];

            if (length != null)
            {
                if (length.StartsWith("(") == false)
                    length = $"({length})";
            }
            return $"{dbtype}{length}";
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("test");
            base.OnConfiguring(optionsBuilder);
        }
    }
}
