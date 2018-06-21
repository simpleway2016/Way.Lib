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
    public class EFAction : Action
    {
        protected virtual List<MigrationOperation> GetOperations()
        {
            return new List<MigrationOperation>();
        }
        public override void Invoke(IDatabaseService invokingDB)
        {
            var operations = this.GetOperations();

            using (var dbContext = (Microsoft.EntityFrameworkCore.DbContext)Activator.CreateInstance(typeof(EF_CreateTable_Action).Assembly.GetType($"Way.EntityDB.Design.Impls.{invokingDB.DBContext.DatabaseType}.TempDBContext")))
            {
                var dbService = (ITempDBContext)dbContext;
                foreach (var op in operations)
                {
                    if (op is CreateTableOperation)
                    {
                        var _CreateTableOperation = op as CreateTableOperation;
                        foreach (var column in _CreateTableOperation.Columns)
                        {
                            string length = null;
                            if (!string.IsNullOrEmpty(column.ComputedColumnSql))
                            {
                                length = column.ComputedColumnSql;
                                column.ComputedColumnSql = null;
                            }
                            column.ColumnType = dbService.GetColumnType(column.ColumnType, length);
                        }
                    }
                    else if (op is AddColumnOperation)
                    {
                        var column = op as AddColumnOperation;
                        string length = null;
                        if (!string.IsNullOrEmpty(column.ComputedColumnSql))
                        {
                            length = column.ComputedColumnSql;
                            column.ComputedColumnSql = null;
                        }
                        column.ColumnType = dbService.GetColumnType(column.ColumnType, length);
                    }
                    else if (op is AlterColumnOperation)
                    {
                        var column = op as AlterColumnOperation;
                        string length = null;
                        if (!string.IsNullOrEmpty(column.ComputedColumnSql))
                        {
                            length = column.ComputedColumnSql;
                            column.ComputedColumnSql = null;
                        }
                        column.ColumnType = dbService.GetColumnType(column.ColumnType, length);

                        length = null;
                        if (!string.IsNullOrEmpty(column.OldColumn.ComputedColumnSql))
                        {
                            length = column.OldColumn.ComputedColumnSql;
                            column.OldColumn.ComputedColumnSql = null;
                        }
                        column.OldColumn.ColumnType = dbService.GetColumnType(column.OldColumn.ColumnType, length);
                    }
                }

                var generator = dbContext.GetService<IMigrationsSqlGenerator>();
                var commands = generator.Generate(dbService.CheckOperations(operations,invokingDB));
                dbService.BeforeAction(invokingDB);
                foreach (var cmd in commands)
                {
                    invokingDB.ExecSqlString(cmd.CommandText);
                }
                dbService.AfterAction(invokingDB);
            }
        }

        internal override void BeforeSave()
        {

        }
    }
}
