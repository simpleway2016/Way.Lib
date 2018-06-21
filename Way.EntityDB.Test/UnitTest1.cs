using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Migrations.Operations;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Way.EntityDB.Design;
using Way.EntityDB.Design.Actions;
using System;

namespace Way.EntityDB.Test
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            using (var db = new MySqliteEmptyDBContext())
            {
                var generator = db.GetService<IMigrationsSqlGenerator>();

                List<MigrationOperation> operations = new List<MigrationOperation>();
                var _CreateTableOperation = new CreateTableOperation();
                _CreateTableOperation.Name = "table1";

                _CreateTableOperation.PrimaryKey = new AddPrimaryKeyOperation();
                _CreateTableOperation.PrimaryKey.Columns = new string[] { "id" };

                var _AddColumnOperation = new AddColumnOperation()
                {

                    Table = _CreateTableOperation.Name,
                    ClrType = typeof(int).Assembly.GetType("System.Int32"),
                    IsNullable = false,
                    Name = "id"
                };
                _CreateTableOperation.Columns.Add(_AddColumnOperation);

                _AddColumnOperation = new AddColumnOperation()
                {

                    Table = _CreateTableOperation.Name,
                    ClrType = typeof(decimal),
                    ColumnType = "DECIMAL(10,5)",
                    IsNullable = false,
                    Name = "de"
                };
                _CreateTableOperation.Columns.Add(_AddColumnOperation);

                _AddColumnOperation = new AddColumnOperation()
                {

                    Table = _CreateTableOperation.Name,
                    ClrType = typeof(System.Byte[]),
                    ColumnType = "Image",
                    IsNullable = false,
                    Name = "bs"
                };
                _CreateTableOperation.Columns.Add(_AddColumnOperation);

                _AddColumnOperation = new AddColumnOperation()
                {

                    Table = _CreateTableOperation.Name,
                    ClrType = typeof(System.String),
                    ColumnType = "varchar",
                    IsNullable = false,
                    MaxLength = 200,
                    Name = "str"
                };
                _CreateTableOperation.Columns.Add(_AddColumnOperation);

                operations.Add(_CreateTableOperation);

                var commands = generator.Generate(operations);
            }
        }

        [TestMethod]
        public void CreatTable()
        {
            EJ.DBTable table = new EJ.DBTable() {
                Name = "Table1",
                id = 1,
            };

            EJ.DBColumn[] columns = new EJ.DBColumn[3];
            columns[0] = new EJ.DBColumn() {
                Name = "Id",
                IsPKID = true,
                IsAutoIncrement = true,
                dbType = "int",
            };

            columns[1] = new EJ.DBColumn()
            {
                Name = "Name",
                dbType = "varchar",
                length = "50"
            };

            columns[2] = new EJ.DBColumn()
            {
                Name = "Age",
                dbType = "int",
                defaultValue = "10"
            };

            EJ.IDXIndex[] idxConfigs = new EJ.IDXIndex[1];
            idxConfigs[0] = new EJ.IDXIndex() {
                Keys = "Name,Age",
                IsUnique = true,
            };

            var invokeDB = EntityDB.DBContext.CreateDatabaseService($"data source=\"{AppDomain.CurrentDomain.BaseDirectory}\\test.sqlite\"", EntityDB.DatabaseType.Sqlite);

            var action = new EF_CreateTable_Action(table, columns, idxConfigs);
            invokeDB.DBContext.BeginTransaction();
            try
            {
                action.Invoke(invokeDB);
            }
            catch
            {
                throw;
            }
            finally
            {
                invokeDB.DBContext.RollbackTransaction();
            }
        }
    }
    class MySqlServerEmptyDBContext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("test");
            base.OnConfiguring(optionsBuilder);
        }
    }
    class MySqliteEmptyDBContext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("test");
            base.OnConfiguring(optionsBuilder);
        }
    }
}
