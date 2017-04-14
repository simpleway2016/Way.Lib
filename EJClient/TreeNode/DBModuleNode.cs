using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace EJClient.TreeNode
{
    class DBModuleNode : 数据模块Node
    {
        public override EJ.DBModule Module
        {
            get;
            set;
        }
        public override string Icon
        {
            get
            {
                if( Module.IsFolder == true )
                return "imgs/folder.png";
                else
                    return "imgs/dbmodule.png";
            }
            set
            {
            }
        }
        public DBModuleNode(数据模块Node parent,EJ.DBModule module)
            : base(parent)
        {
            this.Module = module;
            if (this.Module != null)
            {
                this.Module.PropertyChanged += Module_PropertyChanged;
                this.Name = module.Name;
            }
            
        }

        void Module_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if(e.PropertyName == "Name")
                this.Name = Module.Name;
        }

    
        public void Rename()
        {
            UI.InputBox inputbox = new UI.InputBox("请输入新的名称", "重命名");
            inputbox.Owner = MainWindow.instance;
            inputbox.Value = this.Module.Name;
            if (inputbox.ShowDialog() == true)
            {
                string newName = inputbox.Value.Trim();

                if (this.Parent.Children.Count(m => m != this && m.Name.ToLower() == newName.ToLower()) > 0)
                {
                    MessageBox.Show(MainWindow.instance, "名称重复");
                    return;
                }
                string oldname = this.Module.Name;
                this.Module.ChangedProperties.Clear();
                this.Module.Name = newName;
                try
                {

                    Helper.Client.InvokeSync<string>("UpdateDBModule", this.Module);
                }
                catch (Exception ex)
                {
                    this.Module.Name = oldname;
                    MessageBox.Show(MainWindow.instance, ex.Message);
                }
            }
        }

        public void Delete()
        {
            if (MessageBox.Show(MainWindow.instance, "确定删除吗？","", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                try
                {
                    Helper.Client.InvokeSync<string>("DeleteModule", this.Module);
                    this.Parent.Children.Remove(this);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(MainWindow.instance, ex.Message);
                }
            }
        }
        internal TreeNode.DBModuleNode FindDBModule(int moduleid)
        {
            foreach (DBModuleNode node in Children)
            {
                if (node.Module.id == moduleid)
                    return node;
                else
                {
                    var result = node.FindDBModule(moduleid);
                    if (result != null)
                        return result;
                }
            }
            return null;
        }

        public void ShowTable(int tableid)
        {
            UI.ModuleDocument mydoc = null;
            foreach (UI.Document doc in MainWindow.instance.documentContainer.Items)
            {
                if (doc is UI.ModuleDocument)
                {
                    var mdoc = doc as UI.ModuleDocument;
                    if (mdoc.ModuleNode.Module.id == this.Module.id)
                    {
                        MainWindow.instance.documentContainer.SelectedItem = mdoc;
                        mydoc = mdoc;
                    }
                }
            }
            if (mydoc == null)
            {
                mydoc = new UI.ModuleDocument(this);
                MainWindow.instance.documentContainer.Items.Add(mydoc);
                MainWindow.instance.documentContainer.SelectedItem = mydoc;
            }
            //聚焦指定的table
            mydoc.FocusTable(tableid);
        }

        public override void OnDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (this.Module.IsFolder == false)
            {
                if (e != null)
                {
                    e.Handled = true;
                }
                foreach (UI.Document doc in MainWindow.instance.documentContainer.Items)
                {
                    if (doc is UI.ModuleDocument)
                    {
                        var mdoc = doc as UI.ModuleDocument;
                        if (mdoc.ModuleNode.Module.id == this.Module.id)
                        {
                            MainWindow.instance.documentContainer.SelectedItem = mdoc;
                            return;
                        }
                    }
                }
                if (true)
                {
                    UI.ModuleDocument doc = new UI.ModuleDocument(this);
                    MainWindow.instance.documentContainer.Items.Add(doc);
                    MainWindow.instance.documentContainer.SelectedItem = doc;
                }
            }
        }
    }
}
