using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
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



                //Test(new EJ.Databases()
                //{
                //    conStr = "data source=d:\\test\\test.db",
                //    Name = "testingdb",
                //    dbType = EJ.Databases_dbTypeEnum.Sqlite,
                //});
                //dbservice = EntityDB.Design.DBHelper.CreateDatabaseDesignService(DatabaseType.Sqlite);
                //db = EntityDB.DBContext.CreateDatabaseService("data source=d:\\test\\test.db", EntityDB.DatabaseType.Sqlite);
                //dbservice.GetCurrentTableNames(db);
                //dbservice.GetCurrentColumns(db, "test3");
                //dbservice.GetCurrentIndexes(db, "test3");

                //Test(new EJ.Databases()
                //{
                //    conStr = "Server=" + ip + ";uid=sa;pwd=Sql12345678;database=testingdb",
                //    Name = "testingdb",
                //    dbType = EJ.Databases_dbTypeEnum.SqlServer,
                //});
                //dbservice = EntityDB.Design.DBHelper.CreateDatabaseDesignService(DatabaseType.SqlServer);
                //db = EntityDB.DBContext.CreateDatabaseService("Server=" + ip + ";uid=sa;pwd=Sql12345678;database=testingdb", EntityDB.DatabaseType.SqlServer);
                //dbservice.GetCurrentTableNames(db);
                //dbservice.GetCurrentColumns(db, "test3");
                //dbservice.GetCurrentIndexes(db, "test3");

                //Test(new EJ.Databases()
                //{
                //    conStr = "Server=" + ip + ";Port=5432;UserId=postgres;Password=123456;Database=testingdb;",
                //    Name = "testingdb",
                //    dbType = EJ.Databases_dbTypeEnum.PostgreSql,
                //});
                //dbservice = EntityDB.Design.DBHelper.CreateDatabaseDesignService(DatabaseType.PostgreSql);
                //db = EntityDB.DBContext.CreateDatabaseService("Server=" + ip + ";Port=5432;UserId=postgres;Password=123456;Database=testingdb;", EntityDB.DatabaseType.PostgreSql);
                //object result = dbservice.GetCurrentTableNames(db);
                //result = dbservice.GetCurrentColumns(db, "test3");
                //result = dbservice.GetCurrentIndexes(db, "test3");

                //Test(new EJ.Databases()
                //{
                //    conStr = "server=" + ip + ";User Id=user1;password=User.123456;Database=testingdb",
                //    Name = "testingdb",
                //    dbType = EJ.Databases_dbTypeEnum.MySql,
                //});
                //dbservice = EntityDB.Design.DBHelper.CreateDatabaseDesignService(DatabaseType.MySql);
                //db = EntityDB.DBContext.CreateDatabaseService("server=" + ip + ";User Id=user1;password=User.123456;Database=testingdb", EntityDB.DatabaseType.MySql);
                //object result = dbservice.GetCurrentTableNames(db);
                //result = dbservice.GetCurrentColumns(db, "test3");
                //result = dbservice.GetCurrentIndexes(db, "test3");
            }
            catch (Exception ex)
            {
                return;
            }

            try
            {
                Console.OutputEncoding = System.Text.Encoding.UTF8;

                int port = 6061;
                if (args != null && args.Length > 0)
                {
                    port = Convert.ToInt32(args[0]);
                }

                ScriptRemotingServer.RegisterHandler(new DownLoadCodeHandler());
                ScriptRemotingServer.RegisterHandler(new DownLoadSimpleCodeHandler());
                ScriptRemotingServer.RegisterHandler(new DownloadTableDataHandler());
                ScriptRemotingServer.RegisterHandler(new ImportDataHandler());

                ScriptRemotingServer.UseHttps(new X509Certificate(Way.Lib.PlatformHelper.GetAppDirectory() + "EJServerCert.pfx", "123456"));
                Console.WriteLine($"use ssl EJServerCert.pfx");
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

                SessionState.Timeout = 60 * 24;

                ScriptRemotingServer.Start(port, webroot, 1);
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
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

                #region CreateTable
                if (true)
                {
                    EJ.DBTable tableUser = new EJ.DBTable();
                    tableUser.Name = "User";

                    allColumns.Add(new EJ.DBColumn()
                    {
                        IsPKID = true,
                        CanNull = false,
                        Name = "Id",
                        dbType = "int",
                        IsAutoIncrement = true,
                    });
                    allColumns.Add(new EJ.DBColumn()
                    {
                        Name = "Name",
                        dbType = "varchar",
                        length = "50",
                        defaultValue = "a'b,c"
                    });

                    CreateTableAction _CreateTableAction = new CreateTableAction(tableUser, allColumns.ToArray(), allindexes.ToArray());
                    _CreateTableAction.Invoke(db);

                    DeleteTableAction _delaction = new DeleteTableAction(tableUser.Name);
                    _delaction.Invoke(db);

                    //再次创建
                    _CreateTableAction.Invoke(db);
                }
                #endregion

                allColumns.Clear();
                allindexes.Clear();
                allColumns.Add(new EJ.DBColumn()
                {
                    IsPKID = true,
                    CanNull = false,
                    Name = "Id",
                    dbType = "int",
                    IsAutoIncrement = true,
                });
                allColumns.Add(new EJ.DBColumn()
                {
                    Name = "C1",
                    dbType = "varchar",
                    length = "50",
                    defaultValue = "a'b,c"
                });
                allColumns.Add(new EJ.DBColumn()
                {
                    Name = "C2",
                    dbType = "varchar",
                    length = "50",
                    defaultValue = "abc"
                });
                allColumns.Add(new EJ.DBColumn()
                {
                    Name = "C3",
                    dbType = "int",
                    defaultValue = "8"
                });
                allColumns.Add(new EJ.DBColumn()
                {
                    Name = "Text1",
                    dbType = "text",
                });
                //索引
                allindexes.Add(new EntityDB.Design.IndexInfo()
                {
                    ColumnNames = new string[] { "C1" },
                    IsUnique = true,
                });
                allindexes.Add(new EntityDB.Design.IndexInfo()
                {
                    ColumnNames = new string[] { "C2", "C3" },
                });


                EJ.DBTable table = new EJ.DBTable();
                table.Name = "Test";

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

                    checkColumns(dbservice, db, table.Name, allColumns, allindexes);
                }
                #endregion

                #region 测试自增长id
                if(true)
                {
                    Way.EntityDB.CustomDataItem dataitem = new EntityDB.CustomDataItem("test", "id", null);
                    dataitem.SetValue("c1","C1");
                    dataitem.SetValue("c2", "C2");
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
                        Name = "N0",
                        dbType = "varchar",
                        length = "30",
                        defaultValue = "t'b"
                    });
                    newcolumns[1] = (new EJ.DBColumn()
                    {
                        Name = "N1",
                        dbType = "int",
                        defaultValue = "18"
                    });


                    EJ.DBColumn[] changedColumns = new EJ.DBColumn[2];
                    changedColumns[0] = allColumns.FirstOrDefault(m=>m.Name == "C3");
                    changedColumns[0].Name = "C3_changed";
                    changedColumns[0].dbType = "varchar";
                    changedColumns[0].defaultValue = "1'a";
                    changedColumns[0].CanNull = false;
                    changedColumns[0].length = "88";

                   
                    changedColumns[1] = allColumns.FirstOrDefault(m => m.Name == "Id");
                    changedColumns[1].IsAutoIncrement = false;
                    changedColumns[1].IsPKID = false;
                    changedColumns[1].CanNull = true;



                    EJ.DBColumn[] deletecolumns = new EJ.DBColumn[1];
                    deletecolumns[0] = allColumns.FirstOrDefault(m => m.Name == "C2");

                    allColumns.Remove(deletecolumns[0]);

                    allindexes.Clear();
                    allindexes.Add( new EntityDB.Design.IndexInfo()
                    {
                        ColumnNames = new string[] { "N0", "C3_changed" },
                        IsUnique = true,
                        IsClustered = true
                    });

                    var otherColumns = (from m in allColumns
                                        where changedColumns.Contains(m) == false
                                        select m).ToArray();
                   
                    new ChangeTableAction(table.Name, "Test2", newcolumns, changedColumns, deletecolumns, otherColumns, allindexes.ToArray())
                    .Invoke(db);
                    table.Name = "Test2";
                    allColumns.AddRange(newcolumns);

                    foreach (var c in allColumns)
                    {
                        c.ChangedProperties.Clear();
                        c.BackupChangedProperties.Clear();
                    }
                    checkColumns(dbservice, db, table.Name, allColumns, allindexes);
                }
                #endregion

                #region ChangeTable2
                if (true)
                {
                    EJ.DBColumn[] newcolumns = new EJ.DBColumn[0];
                    EJ.DBColumn[] changedColumns = new EJ.DBColumn[1];
                    changedColumns[0] = allColumns.FirstOrDefault(m => m.Name == "Id");
                    changedColumns[0].IsAutoIncrement = true;
                    changedColumns[0].IsPKID = true;
                    changedColumns[0].CanNull = false;


                    EJ.DBColumn[] deletecolumns = new EJ.DBColumn[0];

                    var otherColumns = (from m in allColumns
                                        where changedColumns.Contains(m) == false
                                        select m).ToArray();
                   
                    new ChangeTableAction(table.Name, "Test3", newcolumns, changedColumns, deletecolumns, otherColumns, allindexes.ToArray())
                    .Invoke(db);
                    table.Name = "Test3";
                    allColumns.AddRange(newcolumns);

                    foreach (var c in allColumns)
                    {
                        c.ChangedProperties.Clear();
                        c.BackupChangedProperties.Clear();
                    }

                    checkColumns(dbservice, db, table.Name, allColumns, allindexes);
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

        static void checkColumns(IDatabaseDesignService design, IDatabaseService db ,string table, List<EJ.DBColumn> allcolumns,List<EntityDB.Design.IndexInfo> allindex)
        {
            var columns = design.GetCurrentColumns(db, table.ToLower());
            var indexes = design.GetCurrentIndexes(db, table.ToLower());

            if (allcolumns.Count != columns.Count)
            {
                throw new Exception("column 数量不一致");
            }
            foreach( var column in allcolumns )
            {
                if (column.defaultValue == null)
                    column.defaultValue = "";

                var compareColumn = columns.FirstOrDefault(m => m.Name.ToLower() == column.Name.ToLower());
                if(compareColumn == null)
                    throw new Exception("找不到字段" + column.Name);

                if (compareColumn.defaultValue == null)
                    compareColumn.defaultValue = "";

                if (column.CanNull != compareColumn.CanNull)
                    throw new Exception($"column:{column.Name} CanNull 不一致");
                if (column.dbType != compareColumn.dbType)
                    throw new Exception($"column:{column.Name} dbType 不一致");
                if (column.defaultValue != compareColumn.defaultValue)
                    throw new Exception($"column:{column.Name} defaultValue 不一致 {column.defaultValue}  vs  {compareColumn.defaultValue}");
                if (column.IsAutoIncrement != compareColumn.IsAutoIncrement)
                    throw new Exception($"column:{column.Name} IsAutoIncrement 不一致");
                if (column.IsPKID != compareColumn.IsPKID)
                    throw new Exception($"column:{column.Name} IsPKID 不一致");
                if(column.dbType.Contains("char"))
                {
                    if (column.length != compareColumn.length)
                        throw new Exception($"column:{column.Name} length 不一致 {column.length}  vs  {compareColumn.length}");
                }
            }

            if (allindex.Count != indexes.Count)
            {
                throw new Exception("index 数量不一致");
            }
            foreach (var index in allindex)
            {
                if(indexes.Any(m=>m.ColumnNames.OrderBy(n => n).ToArray().ToSplitString(",").ToLower() == index.ColumnNames.OrderBy(n => n).ToArray().ToSplitString(",").ToLower()) == false)
                    throw new Exception($"index:{index.Name} ColumnNames 不一致");
                if (indexes.Any(m => m.ColumnNames.OrderBy(n => n).ToArray().ToSplitString(",").ToLower() == index.ColumnNames.OrderBy(n => n).ToArray().ToSplitString(",").ToLower() && m.IsUnique == index.IsUnique) == false)
                    throw new Exception($"index:{index.Name} IsUnique 不一致");
            }
        }
    }
}
