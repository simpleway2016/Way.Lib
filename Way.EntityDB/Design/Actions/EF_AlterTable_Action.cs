using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore.Migrations.Operations;
using System.Linq;
namespace Way.EntityDB.Design.Actions
{
    public class EF_AlterTable_Action : EFAction
    {
        public string OldTableName;
        public string NewTableName;
        public int TableId;
        public EJ.DBColumn[] NewColumns;
        public EJ.DBColumn[] ChangedColumns;
        public EJ.DBColumn[] DeletedColumns;
        public EJ.IDXIndex[] DeletedIndexes;
        public EJ.IDXIndex[] NewIndexes;
        public EF_AlterTable_Action()
        {

        }
        public EF_AlterTable_Action(int tableid,string oldTableName, string newTableName,
             EJ.DBColumn[] newColumns, EJ.DBColumn[] changedColumns,
             EJ.DBColumn[] deletedColumns,EJ.IDXIndex[] deletedIndexes,EJ.IDXIndex[] newIndexes)
        {
            this.TableId = tableid;
            this.OldTableName = oldTableName;
            this.NewTableName = newTableName;
            this.NewColumns = newColumns.ToJsonString().ToJsonObject<EJ.DBColumn[]>();
            this.ChangedColumns = changedColumns.ToJsonString().ToJsonObject<EJ.DBColumn[]>();
            this.DeletedColumns = deletedColumns.ToJsonString().ToJsonObject<EJ.DBColumn[]>();

            this.NewIndexes = newIndexes.ToJsonString().ToJsonObject<EJ.IDXIndex[]>();
            this.DeletedIndexes = deletedIndexes.ToJsonString().ToJsonObject<EJ.IDXIndex[]>();
        }

        protected override List<MigrationOperation> GetOperations()
        {
            List<MigrationOperation> operations = new List<MigrationOperation>();
            if(this.DeletedIndexes != null  )
            {
                foreach(var index in DeletedIndexes)
                {
                    var _DropIndexOperation = new DropIndexOperation();
                    _DropIndexOperation.Table = this.OldTableName.ToLower();
                    _DropIndexOperation.Name = EF_CreateTable_Action.GetIndexName(index.Keys.Split(','), this.TableId);
                    operations.Add(_DropIndexOperation);
                }
            }

            if(this.DeletedColumns != null)
            {
                foreach( var column in this.DeletedColumns )
                {
                    var _DropColumnOperation = new DropColumnOperation() {
                        Name = column.Name,
                        Table = this.OldTableName.ToLower(),
                    };
                    operations.Add(_DropColumnOperation);
                }
            }

            if(this.ChangedColumns != null)
            {
                foreach (var column in this.ChangedColumns)
                {
                    if( column.ChangedProperties["Name"] != null )
                    {
                        var _RenameColumnOperation = new RenameColumnOperation() {
                            Name = column.ChangedProperties["Name"].OriginalValue.ToString().ToLower(),
                            NewName = column.Name.ToLower(),
                            Table = this.OldTableName.ToLower(),
                        };
                        operations.Add(_RenameColumnOperation);
                    }
                    if (column.ChangedProperties["IsAutoIncrement"] != null)
                    {
                        var original = (bool)column.ChangedProperties["IsAutoIncrement"].OriginalValue;
                       if(original)
                        {
                            var _DropSequenceOperation = new DropSequenceOperation()
                            {
                                Name = column.Name,
                            };
                            operations.Add(_DropSequenceOperation);
                        }
                        else
                        {
                            var _CreateSequenceOperation = new CreateSequenceOperation()
                            {
                                StartValue = 1,
                                Name = column.Name,
                            };
                            operations.Add(_CreateSequenceOperation);
                        }
                    }
                    if (column.ChangedProperties["IsPKID"] != null)
                    {
                        var original = (bool)column.ChangedProperties["IsPKID"].OriginalValue;
                        if (original)
                        {
                            var _DropPrimaryKeyOperation = new DropPrimaryKeyOperation()
                            {
                                Name = column.Name,
                                Table = this.OldTableName.ToLower(),
                            };
                            operations.Add(_DropPrimaryKeyOperation);
                        }
                        else
                        {
                            var _AddPrimaryKeyOperation = new AddPrimaryKeyOperation()
                            {
                                Columns = new string[] { column.Name },
                                Table = this.OldTableName.ToLower(),
                            };
                            operations.Add(_AddPrimaryKeyOperation);
                        }
                    }
                    if (column.ChangedProperties.Any(m=>m.Key != "Name" && m.Key != "IsAutoIncrement" && m.Key != "IsPKID") == false)
                        continue;


                    var olddbtype = column.dbType;
                    if(column.ChangedProperties["dbType"] != null)
                    {
                        olddbtype = column.ChangedProperties["dbType"].OriginalValue.ToString();
                    }
                    var oldDefaultValue = column.defaultValue;
                    if (column.ChangedProperties["defaultValue"] != null)
                    {
                        oldDefaultValue = column.ChangedProperties["defaultValue"].OriginalValue.ToString();
                    }
                    string oldComputedColumnSql = null;
                    var oldlength = column.length;
                    if (column.ChangedProperties["length"] != null)
                    {
                        oldlength = column.ChangedProperties["length"].OriginalValue.ToString();
                    }
                    if (!string.IsNullOrEmpty(oldlength))
                    {
                        //借用ComputedColumnSql字段存放length
                        oldComputedColumnSql = oldlength;
                    }

                    var _AlterColumnOperation = new AlterColumnOperation()
                    {
                        Table = this.OldTableName.ToLower(),
                        ClrType = EF_CreateTable_Action.GetCSharpType(column.dbType),
                        ColumnType = column.dbType,
                        DefaultValue = column.defaultValue,
                        IsNullable = column.CanNull.GetValueOrDefault(),
                        Name = column.Name.ToLower(),
                        OldColumn = new ColumnOperation() {
                            ClrType = EF_CreateTable_Action.GetCSharpType(olddbtype),
                            ColumnType = olddbtype,
                            DefaultValue = oldDefaultValue,
                            ComputedColumnSql = oldComputedColumnSql,
                        },
                    };
                    if (!string.IsNullOrEmpty(column.length))
                    {
                        //借用ComputedColumnSql字段存放length
                        _AlterColumnOperation.ComputedColumnSql = column.length;
                    }
                    operations.Add(_AlterColumnOperation);
                }
            }

            if(this.NewTableName.ToLower() != this.OldTableName.ToLower())
            {
                var _RenameTableOperation = new RenameTableOperation() {
                    Name = OldTableName.ToLower(),
                    NewName = NewTableName.ToLower(),
                };
                operations.Add(_RenameTableOperation);
            }

            if (this.NewColumns != null)
            {
                foreach (var column in this.NewColumns)
                {
                    var _AddColumnOperation = new AddColumnOperation()
                    {
                        Table = this.NewTableName.ToLower(),
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
                    operations.Add(_AddColumnOperation);
                }
            }

            if (this.NewIndexes != null)
            {
                foreach (var indexCfg in this.NewIndexes)
                {
                    var keynames = indexCfg.Keys.Split(',');
                    var _CreateIndexOperation = new CreateIndexOperation();
                    _CreateIndexOperation.Table = this.NewTableName.ToLower();
                    _CreateIndexOperation.Name = EF_CreateTable_Action.GetIndexName(keynames, this.TableId);
                    _CreateIndexOperation.Columns = keynames.Select(m => m.ToLower()).ToArray();
                    _CreateIndexOperation.IsUnique = indexCfg.IsUnique.GetValueOrDefault();

                    operations.Add(_CreateIndexOperation);
                }
            }

            return operations;
        }
    }
}
