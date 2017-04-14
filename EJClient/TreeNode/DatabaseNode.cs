using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace EJClient.TreeNode
{
    class DatabaseNode : TreeNodeBase
    {
        public override string Icon
        {
            get
            {
                return "imgs/db.png";
            }
            set
            {

            }
        }
        
        ContextMenu _ContextMenu;
        public override ContextMenu ContextMenu
        {
            get
            {
                if (_ContextMenu == null)
                {
                    _ContextMenu = (ContextMenu)MainWindow.instance.Resources["treeMenu_Database"];
                }
                return _ContextMenu;
            }
            set
            {
                _ContextMenu = value;
            }
        }
        public DatabaseNode(ProjectNode parent)
            : base(parent)
        {

        }
        public override void ReBindItems()
        {
            ProjectNode parentNode = this.Parent as ProjectNode;
            var mydatabases = Helper.Client.InvokeSync<EJ.Databases[]>("GetDatabaseList", parentNode.Project.id);
            foreach (var dbitem in mydatabases)
            {
                DatabaseItemNode dbnode = this.Children.Where(m => ((DatabaseItemNode)m).Database.id == dbitem.id).FirstOrDefault() as DatabaseItemNode;
                if (dbnode == null)
                {
                    dbnode = new DatabaseItemNode(this, dbitem);
                    this.Children.Add(dbnode);
                }
                else
                {
                    dbnode.Database = dbitem;
                }
            }

            for (int i = 0; i < Children.Count; i++)
            {
                if (mydatabases.Where(m => m.id == ((DatabaseItemNode)Children[i]).Database.id).Count() == 0)
                {
                    //删除
                    this.Children.RemoveAt(i);
                    i--;
                }
            }
        }
    }
}
