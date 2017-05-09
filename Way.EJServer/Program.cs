using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Way.EntityDB;
using Way.EntityDB.Design.Actions;
using Way.EntityDB.Design.Services;
using Way.Lib.ScriptRemoting;

namespace Way.EJServer
{
    public class Program
    {
      
        public static void Main(string[] args)
        {
            
            try
            {
                IDatabaseDesignService dbservice;
                IDatabaseService db;
                string ip = "192.168.136.137";

                //IDatabaseDesignService dbservice = EntityDB.Design.DBHelper.CreateDatabaseDesignService(DatabaseType.Sqlite);
                //var db = EntityDB.DBContext.CreateDatabaseService("data source=d:\\testingdb.db", EntityDB.DatabaseType.Sqlite);
                //dbservice.GetCurrentColumns(db, "test3");
                //dbservice.GetCurrentIndexes(db, "test3");

                //dbservice = EntityDB.Design.DBHelper.CreateDatabaseDesignService(DatabaseType.PostgreSql);
                //db = EntityDB.DBContext.CreateDatabaseService("Server=" + ip + ";Port=5432;UserId=postgres;Password=123456;Database=testingdb;", EntityDB.DatabaseType.PostgreSql);
                //dbservice.GetCurrentColumns(db, "test3");
                //dbservice.GetCurrentIndexes(db, "test3");

                //dbservice = EntityDB.Design.DBHelper.CreateDatabaseDesignService(DatabaseType.SqlServer);
                //db = EntityDB.DBContext.CreateDatabaseService("Server=" + ip + ";uid=sa;pwd=Sql12345678;database=testingdb", EntityDB.DatabaseType.SqlServer);
                //dbservice.GetCurrentColumns(db, "test3");
                //dbservice.GetCurrentIndexes(db, "test3");

                //Test(new EJ.Databases()
                //{
                //    conStr = "data source=d:\\testingdb.db",
                //    Name = "testingdb",
                //    dbType = EJ.Databases_dbTypeEnum.Sqlite,
                //});

                //Test(new EJ.Databases()
                //{
                //    conStr = "Server=" + ip + ";uid=sa;pwd=Sql12345678;database=testingdb",
                //    Name = "testingdb",
                //    dbType = EJ.Databases_dbTypeEnum.SqlServer,
                //});

                //Test(new EJ.Databases()
                //{
                //    conStr = "Server=" + ip + ";Port=5432;UserId=postgres;Password=123456;Database=testingdb;",
                //    Name = "testingdb",
                //    dbType = EJ.Databases_dbTypeEnum.PostgreSql,
                //});
            }
            catch (Exception ex)
            {
                return;
            }

            Console.OutputEncoding = System.Text.Encoding.UTF8;

            int port = 6060;
            if(args != null && args.Length > 0)
            {
                port = Convert.ToInt32(args[0]);
            }

            ScriptRemotingServer.RegisterHandler(new DownLoadCodeHandler());
            ScriptRemotingServer.RegisterHandler(new DownloadTableDataHandler());
            ScriptRemotingServer.RegisterHandler(new ImportDataHandler());

            Console.WriteLine($"server starting at port:{port}...");
            var webroot = $"{Way.Lib.PlatformHelper.GetAppDirectory()}Port{port}";

            if (!System.IO.Directory.Exists(webroot))
            {
                System.IO.Directory.CreateDirectory(webroot);
            }

            if (System.IO.File.Exists($"{webroot}/main.html") == false)
            {
                System.IO.File.WriteAllText($"{webroot}/main.html", "<html><body controller=\"Way.EJServer.MainController\"></body></html>");
            }
            Console.WriteLine($"path:{webroot}");
            ScriptRemotingServer.Start(port, webroot, 1);
 
            while (true)
            {
                Console.Write("Web>");
                var line = Console.ReadLine();
                if(line == null)
                {
                    //是在后台运行的
                    while(true)
                    {
                        System.Threading.Thread.Sleep(10000000);
                    }
                }
                else if (line == "exit")
                    break;
            }
            ScriptRemotingServer.Stop();
            System.Diagnostics.Process.GetCurrentProcess().Kill();
        }

        static void Test(EJ.Databases dbconfig)
        {
            IDatabaseDesignService dbservice = EntityDB.Design.DBHelper.CreateDatabaseDesignService((EntityDB.DatabaseType)(int)dbconfig.dbType);
            EntityDB.IDatabaseService db = null;
            dbservice.Drop(dbconfig);
            try
            {
                dbservice.Create(dbconfig);
                db = EntityDB.DBContext.CreateDatabaseService(dbconfig.conStr, (EntityDB.DatabaseType)(int)dbconfig.dbType);

                List<EJ.DBColumn> allColumns = new List<EJ.DBColumn>();
                List<EntityDB.Design.IndexInfo> allindexes = new List<EntityDB.Design.IndexInfo>();

                allColumns.Add(new EJ.DBColumn()
                {
                    IsPKID = true,
                    CanNull = false,
                    Name = "id",
                    dbType = "int",
                    IsAutoIncrement = true,
                });
                allColumns.Add(new EJ.DBColumn()
                {
                    Name = "c1",
                    dbType = "varchar",
                    length = "50",
                    defaultValue = "a'b,c"
                });
                allColumns.Add(new EJ.DBColumn()
                {
                    Name = "c2",
                    dbType = "varchar",
                    length = "50",
                    defaultValue = "abc"
                });
                allColumns.Add(new EJ.DBColumn()
                {
                    Name = "c3",
                    dbType = "int",
                    defaultValue = "8"
                });
                allColumns.Add(new EJ.DBColumn()
                {
                    Name = "text1",
                    dbType = "text",
                    defaultValue = "abc"
                });
                //索引
                allindexes.Add(new EntityDB.Design.IndexInfo()
                {
                    ColumnNames = new string[] { "c1" },
                    IsUnique = true,
                });
                allindexes.Add(new EntityDB.Design.IndexInfo()
                {
                    ColumnNames = new string[] { "c2", "c3" },
                });


                EJ.DBTable table = new EJ.DBTable();
                table.Name = "test";

                #region CreateTable
                if (true)
                {                  
                    CreateTableAction _CreateTableAction = new CreateTableAction(table , allColumns.ToArray() , allindexes.ToArray());
                    _CreateTableAction.Invoke(db);

                    foreach (var c in allColumns)
                    {
                        c.ChangedProperties.Clear();
                        c.BackupChangedProperties.Clear();
                    }
                }
                #endregion

                #region 测试自增长id
                if(true)
                {
                    Way.EntityDB.CustomDataItem dataitem = new EntityDB.CustomDataItem("test", "id", null);
                    dataitem.SetValue("c1","c1");
                    dataitem.SetValue("c2", "c2");
                    dataitem.SetValue("c3", 3);
                    db.Insert(dataitem);
                    if (dataitem.GetValue("id") == null)
                        throw new Exception("测试自增长id失败");
                    db.ExecSqlString("delete from test");
                }
                #endregion

                #region ChangeTable1
                if (true)
                {
                    EJ.DBColumn[] newcolumns = new EJ.DBColumn[2];
                    newcolumns[0] = ( new EJ.DBColumn()
                    {
                        Name = "n0",
                        dbType = "varchar",
                        length = "30",
                        defaultValue = "t'b"
                    });
                    newcolumns[1] = (new EJ.DBColumn()
                    {
                        Name = "n1",
                        dbType = "int",
                        defaultValue = "18"
                    });


                    EJ.DBColumn[] changedColumns = new EJ.DBColumn[2];
                    changedColumns[0] = allColumns.FirstOrDefault(m=>m.Name == "c3");
                    changedColumns[0].Name = "c3_changed";
                    changedColumns[0].dbType = "varchar";
                    changedColumns[0].defaultValue = "1'a";
                    changedColumns[0].CanNull = false;
                    changedColumns[0].length = "88";

                   
                    changedColumns[1] = allColumns.FirstOrDefault(m => m.Name == "id");
                    changedColumns[1].IsAutoIncrement = false;
                    changedColumns[1].IsPKID = false;
                  
                  
                    EJ.DBColumn[] deletecolumns = new EJ.DBColumn[1];
                    deletecolumns[0] = allColumns.FirstOrDefault(m => m.Name == "c2");

                    allColumns.Remove(deletecolumns[0]);

                    allindexes.Clear();
                    allindexes.Add( new EntityDB.Design.IndexInfo()
                    {
                        ColumnNames = new string[] { "n0", "c3_changed" },
                        IsUnique = true,
                        IsClustered = true
                    });

                    var otherColumns = (from m in allColumns
                                        where changedColumns.Contains(m) == false
                                        select m).ToArray();

                    new ChangeTableAction("test", "test2", newcolumns, changedColumns, deletecolumns, otherColumns, allindexes.ToArray())
                    .Invoke(db);

                    allColumns.AddRange(newcolumns);

                    foreach (var c in allColumns)
                    {
                        c.ChangedProperties.Clear();
                        c.BackupChangedProperties.Clear();
                    }

                }
                #endregion

                #region ChangeTable2
                if (true)
                {
                    EJ.DBColumn[] newcolumns = new EJ.DBColumn[0];
                    EJ.DBColumn[] changedColumns = new EJ.DBColumn[1];
                    changedColumns[0] = allColumns.FirstOrDefault(m => m.Name == "id");
                    changedColumns[0].IsAutoIncrement = true;
                    changedColumns[0].IsPKID = true;

                  
                    EJ.DBColumn[] deletecolumns = new EJ.DBColumn[0];

                    var otherColumns = (from m in allColumns
                                        where changedColumns.Contains(m) == false
                                        select m).ToArray();

                    new ChangeTableAction("test2", "test3", newcolumns, changedColumns, deletecolumns, otherColumns, allindexes.ToArray())
                    .Invoke(db);

                    allColumns.AddRange(newcolumns);

                    foreach (var c in allColumns)
                    {
                        c.ChangedProperties.Clear();
                        c.BackupChangedProperties.Clear();
                    }
                }
                #endregion
            }
            catch (Exception ex)
            {
                
                throw ex;
            }
            finally
            {
                if(db != null)
                {
                    db.DBContext.Dispose();
                }
                
            }
        }
    }
}
