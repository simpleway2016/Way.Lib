
using EntityDB.Design.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ECWeb.WebForm
{
    public partial class DatabaseEditor : VerifyPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            AppHelper.RegisterJquery(this);
            if(!IsPostBack)
            {
                if (Request.QueryString["databaseID"] != null)
                {
                    int databaseid = Request.QueryString["databaseID"].ToInt();
                    using (EJDB_Check db = new EJDB_Check())
                    {
                        if (db.Databases.Count(m => m.id == databaseid) == 0)
                        {
                            Response.Write("没有权限");
                            Response.End();
                        }
                    }
                }
                
                string[] typenames = Enum.GetNames(typeof(EJ.Databases_dbTypeEnum));
                foreach (string tn in typenames)
                {
                    selDBType.Items.Add(new ListItem(tn , tn));
                }

                if(  Request.QueryString["databaseID"] != null  )
                {
                    EntityEditor1.ChangeMode(AppLib.Controls.ActionType.Update , Request.QueryString["databaseID"]);
                }
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (txtNamespace.Text.Trim() == txt_Name.Text.Trim())
            {
                this.Alert("命名空间不能和数据库名称相同");
                return;
            }
            if (Request.QueryString["databaseID"] != null)
            {
                EntityEditor1.Save();
            }
            else
            {
                try
                {
                    createDB();
                }
                catch (Exception ex)
                {
                    this.Alert(ex.Message);
                }
            }
        }

        void createDB()
        {
            using (EJDB db = new EJDB())
            {
                db.BeginTransaction(System.Data.IsolationLevel.ReadCommitted);
                try
                {
                    EJ.Databases dataitem = new EJ.Databases();
                    dataitem.conStr = txt_conStr.Text.Trim();
                    dataitem.dbType = (EJ.Databases_dbTypeEnum) Enum.Parse( typeof(EJ.Databases_dbTypeEnum) ,  selDBType.SelectedValue);
                    dataitem.dllPath = txt_dllPath.Text.Trim();
                    dataitem.Name = txt_Name.Text.Trim();
                    dataitem.NameSpace = txtNamespace.Text.Trim();
                    dataitem.Guid = Guid.NewGuid().ToString();
                    dataitem.ProjectID = Request.QueryString["projectid"].ToInt();
                    db.Update(dataitem);

                    IDatabaseDesignService dbservice = DBHelper.CreateInstance<IDatabaseDesignService>(dataitem.dbType.ToString());
                    dbservice.Create(dataitem);

                    db.CommitTransaction();

                    this.WriteJsToTheEndOfForm("function webBrowser_start(){alert('创建成功！');$('#btnClose')[0].click();}");
                }
                catch (Exception ex)
                {
                    db.RollbackTransaction();
                    throw ex;
                }
            }
        }

     
        protected void selDBType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (selDBType.SelectedValue.Contains("MySql"))
                txt_conStr.Text = "server=;User Id=;password=;Database=";
            else  if (selDBType.SelectedValue.Contains("SqlServer"))
                txt_conStr.Text = "server=;uid=;pwd=;Database=";
            else if (selDBType.SelectedValue.Contains("Sqlite"))
                txt_conStr.Text = "data source=\"\"";
        }

        protected void EntityEditor1_BeforeSave1(object database, object editor, AppLib.Controls.ActionType actionType, object dataitem)
        {
            EJDB db = (EJDB)database;
            EJ.Databases data = dataitem as EJ.Databases;
            if (data.NameSpace == "DB")
            {
                throw new Exception("NameSpace不能为DB");
            }
            if (data.NameSpace == data.Name)
            {
                throw new Exception("NameSpace不能和数据库名称相同");
            }
            if (actionType == AppLib.Controls.ActionType.Update)
            {

               
                var oldData = db.Databases.FirstOrDefault(m => m.id == data.id);
                
                if (data.dbType != oldData.dbType)
                {
                    //变更数据库类型
                    IDatabaseDesignService dbservice = DBHelper.CreateInstance<IDatabaseDesignService>(data.dbType.ToString());
                    dbservice.Create(data);
                    //更新到现在的数据结构
                    var invokeDB = DBHelper.CreateInvokeDatabase(data);
                    var dbconfig = invokeDB.ExecSqlString("select contentConfig from __WayEasyJob").ToString().ToJsonObject<DataBaseConfig>();
                    dbconfig.DatabaseGuid = data.Guid;
                    invokeDB.DBContext.BeginTransaction();
                    try
                    {
                        System.Web.Script.Serialization.JavaScriptSerializer jsonObj = new System.Web.Script.Serialization.JavaScriptSerializer();
                        int? lastid = null;
                        using( var dt = db.Database.SelectTable("select * from __action where id>"+dbconfig.LastUpdatedID+" and databaseid=" + data.id + " order by [id]"))
                        {
                            foreach (System.Data.DataRow datarow in dt.Rows)
                            {
                                string actiontype = datarow["type"].ToString();
                                int id = Convert.ToInt32(datarow["id"]);

                                string json = datarow["content"].ToString();


                                Type type = typeof(EntityDB.Design.Actions.Action).Assembly.GetType(actiontype);
                                var actionItem = (EntityDB.Design.Actions.Action)jsonObj.Deserialize(json, type);

                                actionItem.Invoke(invokeDB);

                                lastid = id;
                            }
                            if (lastid != null)
                            {
                                dbconfig.LastUpdatedID = lastid.Value;
                            }
                        }

                        var obj = new EntityDB.CustomDataItem("__WayEasyJob", null, null);
                        obj.SetValue("contentConfig", dbconfig.ToJsonString());
                        invokeDB.Update(obj);

                        invokeDB.DBContext.CommitTransaction();
                    }
                    catch
                    {
                        invokeDB.DBContext.RollbackTransaction();
                        throw;
                    }
                }
                else if (data.Name.ToLower() != oldData.Name.ToLower())
                {
                    IDatabaseDesignService dbservice = DBHelper.CreateInstance<IDatabaseDesignService>(oldData.dbType.ToString());
                    dbservice.ChangeName(oldData, data.Name, data.conStr);
                }
                this.WriteJsToTheEndOfForm("function webBrowser_start(){$('#btnClose')[0].click();}");
            }
           
        }
    }
}