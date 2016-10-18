using EJClient.TreeNode;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;

namespace EJClient.Forms.InterfaceCenter.Nodes
{
    class DLLNode : TreeNode.TreeNodeBase
    {
        public string FilePath
        {
            get
            {
                return m_filePath;
            }
        }
        string m_filePath;
        public DLLNode(string filepath):base(null)
        {
            m_filePath = filepath;
            this.Name = filepath;
            this.Children.Add(new LoadingNode(this));
            new Thread(load).Start();
        }

        void load()
        {
           
            try
            {
                System.Collections.ObjectModel.ObservableCollection<TreeNodeBase> classNodes = DllClassLoader.GetAllClass(m_filePath);
                MainWindow.instance.Dispatcher.Invoke(new Action(() =>
                    {
                        this.Children.Clear();
                        foreach (var item in classNodes)
                        {
                            item.Parent = this;
                            this.Children.Add(item);
                        }
                    }));
            }
            catch(Exception ex)
            {
            }
        }
    }
}
