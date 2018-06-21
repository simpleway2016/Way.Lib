using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Migrations.Operations;
using System;
using System.Collections.Generic;
using System.Text;
using Way.EntityDB.Design.Services;
using System.Linq;

namespace Way.EntityDB.Design.Actions
{
    public class EF_CreateTable_Action : EFAction
    {
        public EJ.DBTable Table;
        public EJ.DBColumn[] Columns;
        public EJ.IDXIndex[] Indexes;
        public EF_CreateTable_Action()
        {

        }
        public EF_CreateTable_Action(EJ.DBTable table, EJ.DBColumn[] columns, EJ.IDXIndex[] idxConfigs)
        {
            this.Table = table.ToJsonString().ToJsonObject<EJ.DBTable>();
            this.Columns = columns.ToJsonString().ToJsonObject<EJ.DBColumn[]>();
            this.Indexes = idxConfigs.ToJsonString().ToJsonObject<EJ.IDXIndex[]>();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="type"></param>
        internal static Type GetCSharpType(string type)
        {
            switch (type)
            {

                case "bigint":
                    return typeof(Int64);
                case "binary":
                    return typeof(byte[]);
                case "bit":
                    return typeof(bool);
                case "char":
                    return typeof(string);
                case "datetime":
                    return typeof(DateTime);
                case "decimal":
                    return typeof(decimal);
                case "float":
                    return typeof(float);
                case "double":
                    return typeof(double);
                case "image":
                    return typeof(byte[]);
                case "int":
                    return typeof(Int32);
                case "money":
                    return typeof(decimal);
                case "nchar":
                    return typeof(string);
                case "ntext":
                    return typeof(string);
                case "numeric":
                    return typeof(decimal);
                case "nvarchar":
                    return typeof(string);
                case "real":
                    return typeof(double);
                case "smalldatetime":
                    return typeof(DateTime);
                case "smallint":
                    return typeof(int);
                case "smallmoney":
                    return typeof(decimal);
                case "text":
                    return typeof(string);
                case "timestamp":
                    return typeof(int);
                case "varbinary":
                    return typeof(byte[]);
                case "varchar":
                    return typeof(string);
                default:
                    return null;

            }
        }
        internal static string GetIndexName(string[] columns, int tableid)
        {
            StringBuilder str = new StringBuilder();
            str.Append("ej_");
            str.Append(tableid.ToString());
            str.Append("_");
            for (int i = 0; i < columns.Length; i++)
            {
                str.Append(columns[i].ToLower());
                if (i < columns.Length - 1)
                    str.Append('_');
            }
            return str.ToString();
        }

        protected override List<MigrationOperation> GetOperations()
        {
            List<MigrationOperation> operations = new List<MigrationOperation>();
            #region operations
            if (true)
            {
                var _CreateTableOperation = new CreateTableOperation();
                operations.Add(_CreateTableOperation);
                _CreateTableOperation.Name = this.Table.Name.ToLower();
                var pkColumns = this.Columns.Where(m => m.IsPKID == true).Select(m => m.Name.ToLower()).ToArray();
                if (pkColumns.Length > 0)
                {
                    _CreateTableOperation.PrimaryKey = new AddPrimaryKeyOperation();
                    _CreateTableOperation.PrimaryKey.Columns = pkColumns;
                }

                foreach (var column in this.Columns)
                {
                    var _AddColumnOperation = new AddColumnOperation()
                    {
                        Table = _CreateTableOperation.Name,
                        ClrType = GetCSharpType(column.dbType),
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

                var idColumns = this.Columns.Where(m => m.IsAutoIncrement == true).Select(m => m.Name.ToLower()).ToArray();
                if (idColumns.Length > 0)
                {
                    foreach (var idc in idColumns)
                    {
                        var _CreateSequenceOperation = new CreateSequenceOperation()
                        {
                            StartValue = 1,
                            Name = idc,
                        };
                        operations.Add(_CreateSequenceOperation);
                    }
                }

                foreach (var indexCfg in this.Indexes)
                {
                    var keynames = indexCfg.Keys.Split(',');
                    var _CreateIndexOperation = new CreateIndexOperation();
                    _CreateIndexOperation.Table = _CreateTableOperation.Name;
                    _CreateIndexOperation.Name = GetIndexName(keynames, this.Table.id.Value);
                    _CreateIndexOperation.Columns = keynames.Select(m => m.ToLower()).ToArray();
                    _CreateIndexOperation.IsUnique = indexCfg.IsUnique.GetValueOrDefault();

                    operations.Add(_CreateIndexOperation);
                }
            }
            #endregion
            return operations;
        }

        public override void Invoke(IDatabaseService invokingDB)
        {

            base.Invoke(invokingDB);
        }

        internal override void BeforeSave()
        {
            
        }
    }
}
