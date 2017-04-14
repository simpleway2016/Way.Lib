using EJClient.Forms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace EJClient.TreeNode
{
    class DBTableNode : TreeNodeBase
    {
        public static List<DBTableNode> AllTableNodes = new List<DBTableNode>();
        EJ.DBTable _Table;
        public EJ.DBTable Table
        {
            get
            {
                return _Table;
            }
            set
            {
                _Table = value;
                this.Name = string.Format("{0} {1}", value.Name, value.caption);
            }
        }
        public override string Icon
        {
            get
            {
                return "/imgs/dbtable.png";
            }
        }
        ContextMenu _ContextMenu;
        public override ContextMenu ContextMenu
        {
            get
            {
                if (_ContextMenu == null)
                {
                    _ContextMenu = (ContextMenu)MainWindow.instance.Resources["treeMenu_Table"];
                }
                return _ContextMenu;
            }
            set
            {
                _ContextMenu = value;
            }
        }
        public DBTableNode( EJ.DBTable table, 数据表Node parent)
            : base(parent)
        {
            AllTableNodes.Add(this);
            this.Table = table;
            this.Table.PropertyChanged += Table_PropertyChanged;
        }
        public void Delete()
        {
            if (MessageBox.Show(MainWindow.instance, "确定删除吗？", "", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                try
                {
                    Helper.Client.InvokeSync<string>("DeleteTable", this.Table.DatabaseID.Value, this.Table.Name);
                    this.Parent.Children.Remove(this);
                    AllTableNodes.Remove(this);
                }
                catch (Exception ex)
                {

                    MessageBox.Show(MainWindow.instance, ex.Message);
                }
            }
        }
        void Table_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "Name" || e.PropertyName == "caption")
                this.Name = string.Format("{0} {1}", this.Table.Name, this.Table.caption);
        }
        public override void OnDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (e != null)
            {
                e.Handled = true;
            }
            DBTableEditor form = new DBTableEditor((DatabaseItemNode)this.Parent.Parent, this.Table);
            form.Owner = MainWindow.instance;
            form.ShowDialog();
        }
        public override void LeftMouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            e.Handled = true;
            System.Windows.DragDrop.DoDragDrop((System.Windows.DependencyObject)e.OriginalSource, this, DragDropEffects.Copy);
        }
    }
}
