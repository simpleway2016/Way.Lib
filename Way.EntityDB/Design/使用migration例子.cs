using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Migrations.Operations;
using System;
using System.Collections.Generic;

namespace Way.EntityDB
{
    class Program
    {
        void main()
        {


            using (var db = new MySqliteEmptyDBContext())
            {
                var generator = db.GetService<IMigrationsSqlGenerator>();
                List<MigrationOperation> operations = new List<MigrationOperation>();
                var item = new CreateIndexOperation();
                item.Columns = new string[] { "index", "age" };
                item.Table = "table";
                item.Name = "IX_name_age";
                item.IsUnique = true;
                //item.IsDestructiveChange = true;
                //item[Microsoft.EntityFrameworkCore.SqlServer.Metadata.Internal.SqlServerAnnotationNames.Clustered] = false;
                operations.Add(item);

                var commands = generator.Generate(operations);
            }

            Console.WriteLine("Hello World!");
        }
    }

    class MySqliteEmptyDBContext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("test");
            base.OnConfiguring(optionsBuilder);
        }
    }
}
