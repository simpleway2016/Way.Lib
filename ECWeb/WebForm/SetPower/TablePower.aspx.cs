using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ECWeb.WebForm.SetPower
{
    public partial class TablePower : VerifyPage
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
                    var projects = db.Project.OrderBy(m => m.Name).ToList();
                    foreach (var project in projects)
                    {
                        TreeNode projectNode = new TreeNode(project.Name);
                        projectNode.ShowCheckBox = false;
                        projectNode.Value = "";
                        TreeView1.Nodes.Add(projectNode);

                        var databases = db.Databases.OrderBy(m => m.Name).ToList();
                        foreach (var database in databases)
                        {
                            TreeNode databaseNode = new TreeNode(database.Name);
                            databaseNode.ShowCheckBox = false;
                            databaseNode.Value = "";
                            projectNode.ChildNodes.Add(databaseNode);

                            var dbTables = db.DBTable.Where(m=>m.DatabaseID == database.id).OrderBy(m=>m.Name).ToList();

                            foreach (var dbtable in dbTables)
                            {
                                TreeNode tableNode = new TreeNode(dbtable.Name);
                                tableNode.Value = dbtable.id.ToString();
                                tableNode.ShowCheckBox = true;
                                tableNode.Checked = db.TablePower.Count(m => m.TableID == dbtable.id && m.UserID == UserID) > 0;
                                databaseNode.ChildNodes.Add(tableNode);
                            }
                        }
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
                    db.Delete(db.TablePower.Where(m=>m.UserID == UserID));
                    CheckNodes(db , TreeView1.Nodes);

                    db.CommitTransaction();
                    this.WriteJsToTheBeginOfForm("alert('成功设置');parent.dialog.close();");
                }
                catch (Exception ex)
                {
                    db.RollbackTransaction();
                    throw ex;
                }
            }
        }

        private void CheckNodes( EJDB db , TreeNodeCollection nodes)
        {
            foreach (TreeNode node in nodes)
            {
                if (!string.IsNullOrEmpty(node.Value) )
                {
                    if (node.Checked)
                    {
                        EJ.TablePower data = new EJ.TablePower()
                            {
                                TableID = node.Value.ToInt(),
                                UserID = UserID,
                            };
                        db.Update(data);
                    }
                }
                else
                {
                    CheckNodes(db, node.ChildNodes);
                }
            }
        }
    }
}