using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
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
                //Test(new EJ.Databases()
                //{
                //    conStr = "Server=192.168.50.128;Port=5432;UserId=postgres;Password=123456;Database=testingdb;",
                //    Name = "testingdb",
                //    dbType = EJ.Databases_dbTypeEnum.PostgreSql,
                //});
            }
            catch(Exception ex)
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
            try
            {
                dbservice.Drop(dbconfig);
            }
            catch
            {
            }
            try
            {
                dbservice.Create(dbconfig);
                db = EntityDB.DBContext.CreateDatabaseService(dbconfig.conStr, (EntityDB.DatabaseType)(int)dbconfig.dbType);
               
                #region CreateTable
                if (true)
                {
                    EJ.DBTable table = new EJ.DBTable();
                    table.Name = "test";
                    EJ.DBColumn[] columns = new EJ.DBColumn[4];
                    columns[0] = new EJ.DBColumn() {
                        IsPKID = true,
                        CanNull = false,
                        Name = "id",
                        dbType = "int",
                        IsAutoIncrement = true,
                    };

                    columns[1] = new EJ.DBColumn()
                    {
                        Name = "c1",
                        dbType = "varchar",
                        length = "50",
                        defaultValue = "abc"
                    };
                    columns[2] = new EJ.DBColumn()
                    {
                        Name = "c2",
                        dbType = "varchar",
                        length = "30"
                    };
                    columns[3] = new EJ.DBColumn()
                    {
                        Name = "c3",
                        dbType = "int",
                    };
                    EntityDB.Design.IndexInfo[] indexInfos = new EntityDB.Design.IndexInfo[2];
                    indexInfos[0] = new EntityDB.Design.IndexInfo() {
                        ColumnNames = new string[] {"c1" },
                        IsUnique = true,
                    };
                    indexInfos[1] = new EntityDB.Design.IndexInfo()
                    {
                        ColumnNames = new string[] { "c2","c3" },
                    };
                    CreateTableAction _CreateTableAction = new CreateTableAction(table , columns , indexInfos);
                    _CreateTableAction.Invoke(db);
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
                }
                #endregion

                #region ChangeTable
                if (true)
                {

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
                dbservice.Drop(dbconfig);
            }
        }
    }
}
