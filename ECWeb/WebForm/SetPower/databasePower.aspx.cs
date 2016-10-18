using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ECWeb.WebForm.SetPower
{
    public partial class databasePower : VerifyPage
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
                using (EJDB_Check db = new EJDB_Check())
                {
                    var source = (from m in db.Databases
                                  join p in db.Project on m.ProjectID equals p.id
                                  orderby m.Name
                                  select new EJ.Databases
                                  {
                                      Name = p.Name + "/" + m.Name,
                                      id = m.id,
                                  }).ToList();
                    foreach (var databaseitem in source)
                    {
                        chkList.Items.Add(new ListItem(databaseitem.Name, databaseitem.id.ToString())
                        {
                            Selected = db.DBPower.Count(m => m.UserID == UserID && m.DatabaseID == databaseitem.id) > 0,
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
                    db.Delete(db.DBPower.Where(m => m.UserID == UserID));
                    for (int i = 0; i < chkList.Items.Count; i++)
                    {
                        if (chkList.Items[i].Selected)
                        {
                            EJ.DBPower data = new EJ.DBPower()
                            {
                                DatabaseID = chkList.Items[i].Value.ToInt(),
                                Power = EJ.DBPower_PowerEnum.修改,
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