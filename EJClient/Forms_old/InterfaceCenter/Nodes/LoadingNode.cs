using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EJClient.Forms.InterfaceCenter.Nodes
{
    class LoadingNode : TreeNode.TreeNodeBase
    {
        public LoadingNode(TreeNode.TreeNodeBase parent)
            : base(parent)
        {
            this.Name = "Loading...";
        }

    }
}
