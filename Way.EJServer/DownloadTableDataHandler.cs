using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Way.EntityDB.Design;
using Way.Lib.ScriptRemoting;

namespace Way.EJServer
{
    public class DownloadTableDataHandler : Way.Lib.ScriptRemoting.ICustomHttpHandler
    {
        public void Handle(string originalUrl, HttpConnectInformation connectInfo, ref bool handled)
        {
            if (originalUrl.Contains("/DownloadTableData.aspx") == false)
                return;
            handled = true;
           
            if (connectInfo.Session["user"] == null || ((EJ.User)connectInfo.Session["user"]).Role.GetValueOrDefault().HasFlag( EJ.User_RoleEnum.数据库设计师 ) == false)
                return;

            var tableids = connectInfo.Request.Query["tableids"].ToJsonObject<int[]>();
            System.IO.BinaryWriter bw = new System.IO.BinaryWriter(connectInfo.Response);
            using (EJDB db = new EJDB())
            {
                int tid = tableids[0];
                var dbtable = db.DBTable.FirstOrDefault(m => m.id == tid);
                var database = db.Databases.FirstOrDefault(m => m.id == dbtable.DatabaseID);
                var invokingDB = DBHelper.CreateInvokeDatabase(database);
                {

                    string[] tableNames = new string[tableids.Length];
                    int[] rowCounts = new int[tableids.Length];
                    for (int i = 0; i < tableids.Length; i++)
                    {
                        int tableid = tableids[i];
                        dbtable = db.DBTable.FirstOrDefault(m => m.id == tableid);
                        tableNames[i] = dbtable.Name;
                        string sql = $"select count(*) from {invokingDB.FormatObjectName(dbtable.Name)}";
                        rowCounts[i] = Convert.ToInt32( invokingDB.ExecSqlString(sql));
                    }
                    bw.Write(Newtonsoft.Json.JsonConvert.SerializeObject(tableNames));
                    bw.Write(Newtonsoft.Json.JsonConvert.SerializeObject(rowCounts));

                    for (int i = 0; i < tableids.Length; i++)
                    {
                        int tableid = tableids[i];
                        dbtable = db.DBTable.FirstOrDefault(m => m.id == tableid);
                        string sql = $"select * from {invokingDB.FormatObjectName(dbtable.Name)}";
                        invokingDB.ExecuteReader((reader)=> {
                            
                            Dictionary<string, object> data = new Dictionary<string, object>();
                            for(int j = 0; j < reader.FieldCount; j ++)
                            {
                                string key = reader.GetName(j);
                                object value = reader[j];
                                if (value != null && value != DBNull.Value)
                                {
                                    data[key] = value;
                                }
                            }
                            if (data.Count > 0)
                            {
                                bw.Write(dbtable.Name);
                                bw.Write(Newtonsoft.Json.JsonConvert.SerializeObject(data));
                            }
                            return true;
                        } ,sql);
                    }
                    bw.Write(":end");
                    
                }

            }
        }

    }
}
