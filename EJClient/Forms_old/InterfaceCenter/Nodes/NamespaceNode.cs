using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EJClient.Forms.InterfaceCenter.Nodes
{
    class NamespaceNode : TreeNode.TreeNodeBase
    {
        public NamespaceNode(string name , TreeNode.TreeNodeBase parent)
            : base(parent)
        {
            this.Name = name;
        }
    }
}
