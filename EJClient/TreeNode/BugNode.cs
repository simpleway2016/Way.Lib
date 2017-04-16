using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace EJClient.TreeNode
{
    class BugNode : TreeNodeBase
    {
        Thread m_thread_readBugCount;
        public BugNode(ProjectNode parent):base(parent)
        {
           
        }

        public override void ReBindItems()
        {
            if (m_thread_readBugCount == null)
            {
                //m_thread_readBugCount = new Thread(readBugCount);
                //m_thread_readBugCount.IsBackground = true;
                //m_thread_readBugCount.Start();
            }
            
        }

        public override void OnDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (e != null)
            {
                e.Handled = true;
            }
            foreach (UI.Document doc in MainWindow.instance.documentContainer.Items)
            {
                if (doc is UI.BugWebDocument)
                {
                    var mdoc = doc as UI.BugWebDocument;
                    MainWindow.instance.documentContainer.SelectedItem = mdoc;
                    return;
                }
            }
            if (true)
            {
                UI.BugWebDocument doc = new UI.BugWebDocument();
                MainWindow.instance.documentContainer.Items.Add(doc);
                MainWindow.instance.documentContainer.SelectedItem = doc;
            }
        }

       
    }
}
