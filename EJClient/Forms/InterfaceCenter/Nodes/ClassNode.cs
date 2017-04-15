using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EJClient.Forms.InterfaceCenter.Nodes
{
    enum ClassNodeType
    {
        method = 0,
        property = 1,
    }
    class ClassNodeMember
    {
        public ClassNodeType Type = ClassNodeType.method;
        public string Comment { get; set; }
        public string Name
        {
            get;set;
        }
    }
    class ClassNode:TreeNode.TreeNodeBase
    {
        public override string Icon
        {
            get
            {
                return "/imgs/interface.png";
            }
            set
            {
                base.Icon = value;
            }
        }
        public List<ClassNodeMember> Members
        {
            get;
            set;
        }
        public string FullName { get; set; }
        public string NameSpace;
        public string Comment { get; set; }
        public ClassNode(string fullname,string comment, TreeNode.TreeNodeBase parent)
            : base(parent)
        {
            this.FullName = fullname;
            this.Members = new List<ClassNodeMember>();
            this.Comment = comment;
            try
            {
                this.Name = fullname.Substring(fullname.LastIndexOf(".") + 1);
                NameSpace = fullname.Substring(0, fullname.LastIndexOf("."));
            }
            catch
            {
                this.Name = fullname;
            }
        }
    }
}
