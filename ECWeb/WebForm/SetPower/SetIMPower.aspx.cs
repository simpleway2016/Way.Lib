using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ECWeb.WebForm.SetPower
{
    public partial class SetIMPower : VerifyPage
    {
        public int CurrentUserID
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
                    bindTree(db , 0,TreeView1.Nodes);
                }
            }
        }

        void bindTree(EJDB db , int parentid , TreeNodeCollection nodes)
        {
            var modules = db.InterfaceModule.Where(m => m.ParentID == parentid).ToList();
            foreach (var module in modules)
            {
                TreeNode node = new TreeNode(module.Name);
                node.ShowCheckBox = (module.IsFolder == false);
                node.Value = module.IsFolder == false ? module.id.ToString() : "";
                node.Checked = (db.InterfaceModulePower.Count(m => m.ModuleID == module.id && m.UserID == CurrentUserID) > 0);
                nodes.Add(node);

                bindTree(db, module.id.Value, node.ChildNodes);
            }
        }

        protected void btnOK_Click(object sender, EventArgs e)
        {
            using (EJDB db = new EJDB())
            {
                db.BeginTransaction();
                try
                {
                    db.Delete(db.InterfaceModulePower.Where(m => m.UserID == CurrentUserID));
                    CheckNodes(db, TreeView1.Nodes);

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

        private void CheckNodes(EJDB db, TreeNodeCollection nodes)
        {
            foreach (TreeNode node in nodes)
            {
                if (!string.IsNullOrEmpty(node.Value))
                {
                    if (node.Checked)
                    {
                        EJ.InterfaceModulePower data = new EJ.InterfaceModulePower()
                        {
                            ModuleID = node.Value.ToInt(),
                            UserID = CurrentUserID,
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