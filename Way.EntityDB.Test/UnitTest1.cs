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
        public void UpgradeData_Check()
        {
            System.Text.StringBuilder result = new System.Text.StringBuilder();
            result.Append("\r\n");
            result.Append("H4sIAAAAAAAAC+1d60/bSBD/X/w5HLFjElKpHzi4k6KeKDro9aSmH0y8AauOzTlOqwhFAvXaclWrXl9HVfqi6gPdVYVeUWmL7u6fwUn4L279wHGcB3nZrJ35Avbuej2T3Z39zezMeJma4+ZFlKdOXVg2L6e5HKJOUbO/iIKKqAj1o3zFrE2pKGdeWU0EHlf/xIkFfEOX");
            result.Append("Ina5WlxCtRpqUkGcioy+JzKqIEuUo21GllQkqY7my2mTjjR1Cl8KPP5PR9JUhlvSn8V3aaryYKu8tnd474N261qawpV6Z0bNeU4UkWqUTXEqN8/lUWrK6kH4Qc5cwtfREr6ZlMVCTsrj2wvOt9gd6SX4PpWfKKhySsooKIfJxFWqUkD685w0XRBFXJDlxLxews/PYb7N");
            result.Append("h/WWEYsP+/2p/MwZ48bqQlZ4pBhvxhQdEcG4WK3uvtHu7NUzOZHJyAXrFY0EHtFTo9B6n4PAy5ySWeQUowcRSQvqolE8Fm1H9lHHNbrpGt2xRrrxEGnb1w/vva6nfhZhWgkgnqkRzzrpO4OKdB/UCTluAblp646yWI2yMRdlzMlSxpYu6g2mfp6UpaywYCwfs8SYxc6F");
            result.Append("zVvLr15IXDTkyayKBYI57dtLFcZLqeJeagdfX5Uf3dfW1qubW/VTdk7hpDxnvKU30RL3ULQwnYqWRKOgTPH9zCeLlNazielYfIw3lfDVG39qex8qj381h8b64WwezuWRog9NX1x0Kk2YjqVJ0sXL4Y27hy+fukR4rk8JzssFYzY3+f15lOUKomrMfKO0S1Yc4oeOunjR");
            result.Append("Pq9W3t6u52WyoChIyhQHNAjfSYXcFMoKklnlZpGONmeyOx5ZB49udKHt/V1+sH2w/6jZwM1wRcSTPHpjDs7cEs5kC6+nysZ7/Fe7+QJv0NXdt3hUtSc72tMV18AWl1R5UhakCZ5XUD5/8qss7mDOjTgqNz+VV1ZdWANvNIX8AGSca1KOjmr/XjN/zXRaOs8J6veygqfG");
            result.Append("abxe0tLoqDV/rm5pa9dxwQynqAInGnPnNG020a5t4Xq7jwnRqmY8mRUJnzfuWJuNe5GTFrrauM+KvK2ZNN2Pp9GV9g1kdREpsC/Dvgz7MuzLsC/DvkzWviyhK47NSS/JGJsk7yrlEdbZGkr929VZL9VxN55zotPKx/3K/nNbEWymo9tgVd/7ezQAsh6q6bFO1XR6rLld");
            result.Append("wm0FNDCOF6ovHY22o72tHk/H3SrH2t2D/XemqdYeRQc8sxhohtw8Nk4cwxTjM2LW9TZAzGDJOnEdGyxZYMkCSxZYskw4bNqwbDgMiLm7XT3u6a7uOO1uMIE56lpZv+DAGw684cC7gwPvoOjnCU+lTaOG3SB0Gpu0lD1k6NohU1c7majd2hnqxpQQgwO++5bLXCosmdOa");
            result.Append("n1HkJYRteUjncNkmXp/FirAgSJxYs9DZbJRKJRIW7Tgo/uDCAi4s4MICLizgwkKOC0sgFH8byyTcZ/srjyvPX5tehJbX0/798rMnGM2ZVc38naZlVcgWzymiHxDnWCc8ctWMJBg1jtErwIsfvPhJ9+JvIUnHg+rxSwdXoNJGE3CXhDAGOPwdGF4Mr4t/PMwu/omAuPgT");
            result.Append("px/pdsSQq0O9n8OEPqBpzGd3NdrTgG/WPX+NAxlt+4v29cHB59va77fMcdMlnBFifPDfZnl1u34YzRNwW8KlpKzco19o0sOzKvYY3OPzsDI+OCw0DkkL74XGhi2PE096iJprU0zU95hbtmNfXYYOqixkO/ZgZBifQ/vZjo0CTMxnqwDbiVWAUAXZ23hC8KOCxCHdJA4Z");
            result.Append("DBYlLOsIS6y90hnSHm7zpI8StV0sF0Rom9oAZE6BeJNulPDwmhwhqwiYHNvbaofC5GjrbmxQYcixwX8EK4HehsiG0b09wC7ew+2kT8R68zZ4DZI4QXLF7jMSBj254lhoVQTwSgAVYci8Ehz6fsg1ghZqkBtfut2J8EiWN3bLf+yU1z8dru+6fo1Jq9W5JYxX0Jygl/cu");
            result.Append("2XAX6lEXPackTpKs/3gb3gt4DPAY4DHAY+AlCl6igMcIxmORsMOvwBjIIHUDfH2EiK+PgIEMDGQQtuN12I71bY/1F+WHa/gyJV3mRIHHNclkEkJ6wHg2iKxcfWI6M/KGCCjXT46uptiUkHxdtLfpL8AM55cZDo4S4SgRApwBKcExI+FmLW1tQ9v/Sjre6Rrq9fTN07MF");
            result.Append("FSMEHk3LBFgd+oF4zo+3EoLsGEjDM8T2PKJWFqBUcHiDNDxgzwNnuMAcvgYWpdocueMstH/ua7/dPnyyUn2zqm08q+5cleqcL20GU/lphHjTM7MPFucF1SOfa3LPkJl2eZTAmgiYEzBnf6EI4QqySIQ59eP4EKR+9P0MWT+tggCMQAdgBBxz0tEBJUwKOUD1EXR6m+UR");
            result.Append("jrDB0AmGzvCATojsBdDZO5KHqF8AnYQZOt1T0jwQrL78q7L5vvr+le5i+W6z/HDHBal1cKbkOP2xQWidgxbTFs4m1M7pbQZfgJwAOQFyAuQMhJ1zGD5xA7EykGhmyOyc/QYvDwc89RFyepvifBjyhdadIX75qN3Zsd0nIYkoQUlEbdExHkzREQvut1sZyEoMUiaMqYoj");
            result.Append("QyFUWsjRJFkHNLHgfrqL8TaLNKCwFtYOSOUO8rFP+RgJtDgMShYxxtu0riAgQUACgPRDQDb1xSbT4acHAXrEaizqYsUf22ssuCE+MW/DyuHjtfDxWv8+XjvYpYNvrb4pffGY184lNIV70UUFvp8t5vEq+yYlqXHWeFFda2tdNbafVRVBWmh8oLa4On+mjqeWpF3U2dJr");
            result.Append("Z5FqPRhLRDmeRvRIgmXoEZalkyMcjeIjiQTD8FkW0XGUpUr/A9lm95SMvwAA");
            var data = result.ToString();

            Way.EntityDB.Design.DBUpgrade.Upgrade(null, data);
        }

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
