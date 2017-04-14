using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace EJClient.TreeNode
{
    class 数据表Node : TreeNodeBase
    {
        public override string Icon
        {
            get
            {
                return "imgs/table.png";
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
                    _ContextMenu = (ContextMenu)MainWindow.instance.Resources["treeMenu_Tables"];
                }
                return _ContextMenu;
            }
            set
            {
                _ContextMenu = value;
            }
        }
        DatabaseItemNode DatabaseItemNode;
        public 数据表Node(DatabaseItemNode parent)
            : base(parent)
        {
            DatabaseItemNode = parent;
        }

        public override void ReBindItems()
        {
            EJ.DBTable[] tables = Helper.Client.InvokeSync<EJ.DBTable[]>("GetTableList", this.DatabaseItemNode.Database.id.Value);
            //检查已经删除的
            for (int i = 0; i < this.Children.Count; i++)
            {
                DBTableNode node = this.Children[i] as DBTableNode;
                if (node != null)
                {
                    if (tables.Count(m => m.id == node.Table.id) == 0)
                    {
                        //已经删除
                        this.Children.RemoveAt(i);
                        i--;
                    }
                }
            }
            foreach (EJ.DBTable t in tables)
            {
                if (this.Children.Count(m => ((DBTableNode)m).Table.id == t.id) == 0)
                {
                    //有新增的表
                    DBTableNode node = new DBTableNode(t, this);
                    this.Children.Add(node);
                }
                else
                {
                    DBTableNode node = (DBTableNode)this.Children.FirstOrDefault(m => ((DBTableNode)m).Table.id == t.id);
                    node.Table.Name = t.Name;
                    node.Table.caption = t.caption;
                    node.Name = string.Format("{0} {1}", t.Name, t.caption);
                }
            }
        }
    }
}
