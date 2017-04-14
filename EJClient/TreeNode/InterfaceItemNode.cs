using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace EJClient.TreeNode
{
    class InterfaceItemNode : InterfaceNode
    {
        public override string Icon
        {
            get
            {
                if (Module.IsFolder == true)
                    return "imgs/folder.png";
                else
                    return "imgs/dbmodule.png";
            }
            set
            {
            }
        }

        public InterfaceItemNode(EJ.InterfaceModule module, InterfaceNode parent)
            : base(parent)
        {
            this.Module = module;
            this.Name = module.Name;
            module.PropertyChanged += module_PropertyChanged;
        }

        void module_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "Name")
                this.Name = Module.Name;
        }
        internal TreeNode.InterfaceItemNode FindInterfaceModule(int moduleid)
        {
            foreach (TreeNode.InterfaceItemNode itemnode in Children)
            {
                if (itemnode.Module.id == moduleid)
                    return itemnode;
                else
                {
                    var result = itemnode.FindInterfaceModule(moduleid);
                    if (result != null)
                        return result;
                }
            }
            return null;
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
                    Helper.Client.InvokeSync<string>("UpdateInterfaceModule", this.Module);
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
            if (MessageBox.Show(MainWindow.instance, "确定删除吗？", "", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                try
                {
                    Helper.Client.InvokeSync<string>("DeleteInterfaceModule", this.Module);
                    this.Parent.Children.Remove(this);
                }
                catch (Exception ex)
                {

                    MessageBox.Show(MainWindow.instance, ex.Message);
                }
            }
        }

        public void ShowItem(int itemid)
        {
            UI.InterfaceModuleDocument mydoc = null;
            foreach (UI.Document doc in MainWindow.instance.documentContainer.Items)
            {
                if (doc is UI.InterfaceModuleDocument)
                {
                    var mdoc = doc as UI.InterfaceModuleDocument;
                    if (mdoc.InterfaceItemNode.Module.id == this.Module.id)
                    {
                        MainWindow.instance.documentContainer.SelectedItem = mdoc;
                        mydoc = mdoc;
                    }
                }
            }
            if (mydoc == null)
            {
                mydoc = new UI.InterfaceModuleDocument(this);
                MainWindow.instance.documentContainer.Items.Add(mydoc);
                MainWindow.instance.documentContainer.SelectedItem = mydoc;
            }
            if (mydoc != null)
            {
                mydoc.ShowItem(itemid);
            }
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
                    if (doc is UI.InterfaceModuleDocument)
                    {
                        var mdoc = doc as UI.InterfaceModuleDocument;
                        if (mdoc.InterfaceItemNode.Module.id == this.Module.id)
                        {
                            MainWindow.instance.documentContainer.SelectedItem = mdoc;
                            return;
                        }
                    }
                }
                if (true)
                {
                    UI.InterfaceModuleDocument doc = new UI.InterfaceModuleDocument(this);
                    MainWindow.instance.documentContainer.Items.Add(doc);
                    MainWindow.instance.documentContainer.SelectedItem = doc;
                }
            }
        }
    }
}
