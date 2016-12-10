
using EntityDB.Design.Services;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ECWeb
{
    public partial class DownloadDatabaseCode : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            int databaseid = Request.QueryString["databaseid"].ToInt();
            using (EJDB db = new EJDB())
            {
               
                var database = db.Databases.FirstOrDefault(m=>m.id == databaseid);
                var tables = db.DBTable.Where(m => m.DatabaseID == databaseid).ToList();
                System.IO.BinaryWriter bw = new System.IO.BinaryWriter( Response.OutputStream);

                var invokingDB = EntityDB.Design.DBHelper.CreateInvokeDatabase(database);
                IDatabaseDesignService dbservice = EntityDB.Design.DBHelper.CreateDatabaseDesignService((EntityDB.DatabaseType)(int)database.dbType);
                List<EJ.DBTable> viewtables = new List<EJ.DBTable>();
                List<EJ.DBColumn> viewcolumns = new List<EJ.DBColumn>();
                dbservice.GetViews(invokingDB, out viewtables, out viewcolumns);

               

               
                bw.Write(tables.Count * 1 + 1 + viewtables.Count);
                Database.ICodeBuilder codeBuilder = new Database.CodeBuilder();

                foreach (var table in viewtables)
                {
                    string code = codeBuilder.BuildOldClassCode(db, database.NameSpace + ".View", table, viewcolumns.Where(m => m.TableID == table.id).ToList());
                    if (code != null)
                    {
                        bw.Write(table.Name + "_view_" + ".cs");
                        byte[] bs = System.Text.Encoding.UTF8.GetBytes(code);
                        bw.Write(bs.Length);
                        bw.Write(bs);
                    }
                }

                foreach (var table in tables)
                {
                    string[] codes = codeBuilder.BuildTable(db, database.NameSpace, table);
                    for (int i = 0; i < codes.Length; i++)
                    {
                        bw.Write(table.Name + "_" + i +".cs");
                        byte[] bs = System.Text.Encoding.UTF8.GetBytes(codes[i]);
                        bw.Write( bs.Length );
                        bw.Write(bs);
                    }
                }
                if (true)
                {
                    bw.Write(database.Name + "_db_linq.cs" );
                    string code = codeBuilder.BuilderDB(db, database.Name, database.NameSpace, tables);
                    byte[] bs = System.Text.Encoding.UTF8.GetBytes(code);
                    bw.Write(bs.Length);
                    bw.Write(bs);
                }

                bw.Write(":end");
            }
        }
    }
}