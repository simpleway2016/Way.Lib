
using Way.EntityDB.Design.Services;
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

                var invokingDB = Way.EntityDB.Design.DBHelper.CreateInvokeDatabase(database);
                IDatabaseDesignService dbservice = Way.EntityDB.Design.DBHelper.CreateDatabaseDesignService((Way.EntityDB.DatabaseType)(int)database.dbType);
              
               
                bw.Write(tables.Count * 1 + 1 );
                Database.ICodeBuilder codeBuilder = new Database.CodeBuilder();


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
                    string code = codeBuilder.BuilderDB(db, database, database.NameSpace, tables);
                    byte[] bs = System.Text.Encoding.UTF8.GetBytes(code);
                    bw.Write(bs.Length);
                    bw.Write(bs);
                }

                bw.Write(":end");
            }
        }
    }
}