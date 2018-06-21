using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations.Operations;
using System;
using System.Collections.Generic;
using System.Text;
using Way.EntityDB.Design.Services;

namespace Way.EntityDB.Design.Impls.SqlServer
{
    class TempDBContext : DbContext, ITempDBContext
    {
        public MigrationOperation[] CheckOperations(List<MigrationOperation> operations, IDatabaseService invokingDB)
        {
            return operations.ToArray();
        }
        public void AfterAction(IDatabaseService invokingDB)
        {
        }

        public void BeforeAction(IDatabaseService invokingDB)
        {
        }
        public string GetColumnType(string dbtype,string length)
        {
            if (length != null)
            {
                if (length.StartsWith("(") == false)
                    length = $"({length})";
            }
            return $"{dbtype}{length}";
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("test");
            base.OnConfiguring(optionsBuilder);
        }
    }
}
