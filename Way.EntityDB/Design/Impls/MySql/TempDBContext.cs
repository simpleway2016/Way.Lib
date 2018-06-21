using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations.Operations;
using System;
using System.Collections.Generic;
using System.Text;
using Way.EntityDB.Design.Services;

namespace Way.EntityDB.Design.Impls.MySql
{
    class TempDBContext : DbContext, ITempDBContext
    {
        internal static List<string> ColumnType = new List<string>(new string[] {
                                            "varchar",
                                            "int",
                                            "blob",//image
                                            "text",//text
                                            "smallint",
                                            "date",//smalldatetime
                                            "real",
                                            "datetime",//datetime
                                            "float",
                                            "double",
                                            "bit",
                                            "decimal",
                                            "numeric",
                                            "bigint",//
                                            "varbinary",//varbinary
                                            "char",
                                            "timestamp", });

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
            optionsBuilder.UseMySql("test");
            base.OnConfiguring(optionsBuilder);
        }
    }
}
