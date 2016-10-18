using EJClient.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace EJClient.TreeNode
{
    class ProjectNode : TreeNodeBase
    {
        public EJ.Project Project
        {
            get;
            private set;
        }
        public override string Icon
        {
            get
            {
                return "imgs/project.png";
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
                    _ContextMenu = (ContextMenu)MainWindow.instance.Resources["treeMenu_project"];
                }
                return _ContextMenu;
            }
            set
            {
                _ContextMenu = value;
            }
        }
        public ProjectNode(EJ.Project project):base(null)
        {
            this.Project = project;
            this.Name = project.Name;
            bind();
        }

       
        static string[] childnodeText = new string[] { "Database", "Interface", "Bug", "Task" };
        void bind()
        {
            foreach (string t in childnodeText)
            {
                Type nodeType = typeof(TreeNodeBase).Assembly.GetType("EJClient.TreeNode." + t + "Node");
                TreeNodeBase childnode = (TreeNodeBase)Activator.CreateInstance(nodeType , new object[]{this});
                childnode.Name = t;
                this.Children.Add(childnode);
                childnode.ReBindItems();
            }
        }
    }
}
