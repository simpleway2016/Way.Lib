using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ECWeb.WebForm.SetPower
{
    public partial class projectPower : VerifyPage
    {
        public int UserID
        {
            get
            {
                return Request.QueryString["userid"].ToInt();
            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (this.User.Role != EJ.User_RoleEnum.管理员)
            {
                Response.Write("not arrow");
                Response.End();
            }
            if (!IsPostBack)
            {
                using (EJDB db = new EJDB())
                {
                    var source = db.Project.OrderBy(m=>m.Name).ToList();
                    foreach (var project in source)
                    {
                        chkList.Items.Add(new ListItem(project.Name, project.id.ToString())
                            {
                                Selected = db.ProjectPower.Count(m=>m.UserID == UserID && m.ProjectID == project.id) > 0,
                            });
                    }
                }
            }
        }

        protected void btnOK_Click(object sender, EventArgs e)
        {
            using (EJDB db = new EJDB())
            {
                db.BeginTransaction();
                try
                {
                    db.Delete(db.ProjectPower.Where(m => m.UserID == UserID));
                    for (int i = 0; i < chkList.Items.Count; i++)
                    {
                        if (chkList.Items[i].Selected)
                        {
                            EJ.ProjectPower data = new EJ.ProjectPower()
                            {
                                ProjectID = chkList.Items[i].Value.ToInt(),
                                UserID = UserID,
                            };
                            db.Update(data);
                        }
                    }
                    db.CommitTransaction();
                    this.WriteJsToTheBeginOfForm("alert('成功设置');parent.dialog.close();");
                }
                catch (Exception ex)
                {
                    db.RollbackTransaction();
                    this.Alert(ex.Message);
                }
            }
        }
    }
}