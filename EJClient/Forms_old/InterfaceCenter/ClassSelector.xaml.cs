using EJClient.TreeNode;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace EJClient.Forms.InterfaceCenter
{
    /// <summary>
    /// ClassSelector.xaml 的交互逻辑
    /// </summary>
    public partial class ClassSelector : Window
    {
        int m_ProjectID;
        public ClassSelector(int projectid)
        {
            m_ProjectID = projectid;
            InitializeComponent();
            try
            {
                string[] files = Helper.Client.InvokeSync<string[]>("GetProjectDllFiles", projectid);
                System.Collections.ObjectModel.ObservableCollection<TreeNodeBase> list = new System.Collections.ObjectModel.ObservableCollection<TreeNodeBase>();
                foreach (string f in files)
                {
                    list.Add(new Nodes.DLLNode(f));
                }
                tree1.ItemsSource = list;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        Nodes.ClassNode _SelectedValue;
        internal Nodes.ClassNode SelectedValue
        {
            get
            {
                return _SelectedValue;
            }
        }


        private void item_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ClickCount > 1)
            {

                _SelectedValue = ((FrameworkElement)e.OriginalSource).DataContext as Nodes.ClassNode;
                if (_SelectedValue != null)
                {
                    this.DialogResult = true;
                    this.Close();
                }
            }
        }
    }
}
