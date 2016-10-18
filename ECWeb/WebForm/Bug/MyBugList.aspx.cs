using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ECWeb.WebForm.Bug
{
    public partial class MyBugList : VerifyPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                using (EJDB db = new EJDB())
                {
                    var users = db.User.Where(m=>(m.Role & EJ.User_RoleEnum.开发人员) == EJ.User_RoleEnum.开发人员).ToList();
                    foreach (var user in users)
                    {
                        userListSource.DataSourceItems.Add(new AppLib.Controls.DataSourceItem
                            {
                                Text = user.Name,
                                Value = user.id.ToString(),
                            });
                    }
                }
            }

        }

        void setHandler(int id)
        {
            try
            {
                if (string.IsNullOrEmpty(hidName.Value))
                {
                    throw new Exception("请选择负责人");
                }
                using (EJDB db = new EJDB())
                {
                    var bug = db.Bug.FirstOrDefault(m => m.id == id);
                    bug.HandlerID = db.User.Where(m => m.Name == hidName.Value).Select(m => m.id).FirstOrDefault();
                    db.Update(bug);

                }
                EntityGridView1.DataBind();
            }
            catch (Exception ex)
            {
                this.Alert(ex.Message);
         
            }
        }

        protected void EntityGridView1_CellDataBound(object sender, object database, TableCell cell, DataControlField column, object dataItem, AppLib.ControlEventArg e)
        {
            EJDB_Check db = (EJDB_Check)database;
            var data = (EJ.Bug)dataItem;

            if (column.HeaderText == "操作")
            {
                if ( data.HandlerID == null && this.User.Role.HasFlag(EJ.User_RoleEnum.数据库设计师))
                {
                    var selHandler = cell.FindControl("selHandler") as Control;
                    selHandler.Visible = true;
                    var button = cell.FindControl("btnSetHandler") as Button;
                    button.Visible = true;
                    button.CommandArgument = data.id.ToString();
                    button.OnClientClick = string.Format("return setHandler({0},'{1}');", data.id, selHandler.ClientID);


                    var btnView = cell.FindControl("btnView") as System.Web.UI.HtmlControls.HtmlInputButton;
                    btnView.Style["display"] = "";
                    btnView.Attributes["_id"] = data.id.ToString();
                    btnView.Attributes["_MyRole"] = "1";
                }
                else if (data.HandlerID == this.User.id)
                {
                    var btnView = cell.FindControl("btnView") as System.Web.UI.HtmlControls.HtmlInputButton;
                    btnView.Style["display"] = "";
                    btnView.Attributes["_id"] = data.id.ToString();
                    btnView.Attributes["_MyRole"] = "2";
                }
                else if ( data.SubmitUserID == this.User.id)
                {
                    var btnView = cell.FindControl("btnView") as System.Web.UI.HtmlControls.HtmlInputButton;
                    btnView.Style["display"] = "";
                    btnView.Attributes["_id"] = data.id.ToString();
                    btnView.Attributes["_MyRole"] = "0";

                    var btnDelete = cell.FindControl("btnDelete") as Button;
                    btnDelete.Visible = true;
                    btnDelete.CommandArgument = data.id.ToString();
                }
                else if (data.Status == EJ.Bug_StatusEnum.反馈给提交者 && (this.User.Role.HasFlag(EJ.User_RoleEnum.数据库设计师)))
                {
                    int myrole = 0;
                    if (data.SubmitUserID == this.User.id)
                        myrole = 0;
                    else if (this.User.Role.HasFlag(EJ.User_RoleEnum.数据库设计师))
                        myrole = 1;
                    else
                        myrole = 2;

                    var btnView = cell.FindControl("btnView") as System.Web.UI.HtmlControls.HtmlInputButton;
                    btnView.Style["display"] = "";
                    btnView.Attributes["_id"] = data.id.ToString();
                    btnView.Attributes["_MyRole"] = myrole.ToString();
                 
                }
            }
        }

        protected void EntityGridView1_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "setHandler")
            {
                setHandler( e.CommandArgument.ToString().ToInt() );
            }
            else if (e.CommandName == "del")
            {
                int id = e.CommandArgument.ToString().ToInt();
                using (EJDB db = new EJDB())
                {
                    db.Delete(db.Bug.Where(m=>m.id == id));
                }
                btnRebind_Click(null, null);
            }
        }

        protected void EntityGridView1_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                var data = (EJ.Bug)e.Row.DataItem;
                if (data.Status == EJ.Bug_StatusEnum.提交给开发人员)
                    e.Row.Style["background-color"] = "#fba5a5";
                else if (data.Status == EJ.Bug_StatusEnum.反馈给提交者)
                    e.Row.Style["background-color"] = "#9ccfe5";
            }

        }

        protected void btnRebind_Click(object sender, EventArgs e)
        {
            EntityGridView1.DataBind();
        }
    }
}