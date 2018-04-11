using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace EJClient.TreeNode
{
    class DatabaseItemNode : TreeNodeBase
    {
        EJ.Databases _Database;
        public EJ.Databases Database
        {
            get
            {
                return _Database;
            }
            set
            {
                _Database = value;
                this.Name = value.Name;
            }
        }
        ContextMenu _ContextMenu;
        public override ContextMenu ContextMenu
        {
            get
            {
                if (_ContextMenu == null)
                {
                    _ContextMenu = (ContextMenu)MainWindow.instance.Resources["treeMenu_databaseItem"];
                }
                return _ContextMenu;
            }
            set
            {
                _ContextMenu = value;
            }
        }
        public override string Icon
        {
            get
            {
                return "imgs/dbitem.png";
            }
            set
            {

            }
        }
        public DatabaseItemNode(DatabaseNode parent , EJ.Databases db):base(parent)
        {
            this.Database = db;
            bind();
        }

        static string[] childnodeText = new string[] { "数据表", "数据模块" };
        void bind()
        {
            foreach (string t in childnodeText)
            {
                Type nodeType = typeof(TreeNodeBase).Assembly.GetType("EJClient.TreeNode." + t + "Node");
                TreeNodeBase childnode = (TreeNodeBase)Activator.CreateInstance(nodeType, new object[] { this });
                childnode.Name = t;
                this.Children.Add(childnode);
                childnode.ReBindItems();
            }
        }
        public void Delete()
        {
            Helper.Client.InvokeSync<string>("DeleteDatabase", this.Database.id.GetValueOrDefault());
            this.Parent.Children.Remove(this);
        }

        public void OutputAction(string filename)
        {
            var dt = Helper.Client.InvokeSync<WayDataTable>("GetActions", Database.id).ToDataTable();
            var dset = new System.Data.DataSet();
            dset.Tables.Add(dt);
            dset.DataSetName = this.Database.Guid;
            string path = Path.GetDirectoryName(filename);
            if (Directory.Exists(path) == false)
                Directory.CreateDirectory(path);
            System.IO.FileStream fs = System.IO.File.Create(filename);
            dt.WriteXml(fs, System.Data.XmlWriteMode.WriteSchema);
            fs.Close();

            dt.Dispose();
            dset.Dispose();
        }
    }
}
