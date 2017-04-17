using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Way.EntityDB.Design.Services;
using Way.Lib.ScriptRemoting;

namespace Way.EJServer
{
    class DownLoadCodeHandler : Way.Lib.ScriptRemoting.ICustomHttpHandler
    {
        public void Handle(string originalUrl, HttpConnectInformation connectInfo, ref bool handled)
        {
            if (originalUrl.Contains("/DownloadDatabaseCode.aspx") == false)
                return;
            handled = true;
            try
            {
                if (connectInfo.Session["user"] == null )
                    throw new Exception("not arrow");

                int databaseid = Convert.ToInt32(connectInfo.Request.Query["databaseid"]);
                using (EJDB db = new EJDB())
                {

                    var database = db.Databases.FirstOrDefault(m => m.id == databaseid);
                    database.dllPath = connectInfo.Request.Query["filepath"];
                    db.Update(database);

                    var tables = db.DBTable.Where(m => m.DatabaseID == databaseid).ToList();
                    System.IO.BinaryWriter bw = new System.IO.BinaryWriter(connectInfo.Response);
                    bw.Write("start");

                    var invokingDB = Way.EntityDB.Design.DBHelper.CreateInvokeDatabase(database);
                    IDatabaseDesignService dbservice = Way.EntityDB.Design.DBHelper.CreateDatabaseDesignService((Way.EntityDB.DatabaseType)(int)database.dbType);


                    bw.Write(tables.Count * 1 + 1);
                    ICodeBuilder codeBuilder = new CodeBuilder();


                    foreach (var table in tables)
                    {
                        string[] codes = codeBuilder.BuildTable(db, database.NameSpace, table);
                        for (int i = 0; i < codes.Length; i++)
                        {
                            bw.Write(table.Name + "_" + i + ".cs");
                            byte[] bs = System.Text.Encoding.UTF8.GetBytes(codes[i]);
                            bw.Write(bs.Length);
                            bw.Write(bs);
                        }
                    }
                    if (true)
                    {
                        bw.Write(database.Name + "_db_linq.cs");
                        string code = codeBuilder.BuilderDB(db, database, database.NameSpace, tables);
                        byte[] bs = System.Text.Encoding.UTF8.GetBytes(code);
                        bw.Write(bs.Length);
                        bw.Write(bs);
                    }

                    bw.Write(":end");
                }
            }
            catch(Exception ex)
            {
                new System.IO.BinaryWriter(connectInfo.Response).Write(ex.Message);
            }
        }
    }
}
