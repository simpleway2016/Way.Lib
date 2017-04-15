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

namespace EJClient.Forms
{
    /// <summary>
    /// OutputDBTableData.xaml 的交互逻辑
    /// </summary>
    public partial class OutputDBTableData : Window
    {
        TreeNode.DatabaseItemNode m_databaeItemNode;
        internal OutputDBTableData(TreeNode.DatabaseItemNode databaeItemNode)
        {
            m_databaeItemNode = databaeItemNode;
            InitializeComponent();

            list.ItemsSource = databaeItemNode.Children.FirstOrDefault(m => m is TreeNode.数据表Node).Children;
            list.SelectAll();
            list.Focus();
        }

        private void btnOK_Click_1(object sender, RoutedEventArgs e)
        {
            try
            {
                List<int> tableids = new List<int>();
                foreach (TreeNode.DBTableNode node in list.SelectedItems)
                {
                    tableids.Add(node.Table.id.Value);
                }
                if (tableids.Count > 0)
                {
                    using (System.Windows.Forms.SaveFileDialog sf = new System.Windows.Forms.SaveFileDialog())
                    {
                        sf.Filter = "*.xml|*.xml";
                        if (sf.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                        {
                            MessageBox.Show(this, "未实现");
                            //using (Web.DatabaseService web = Helper.CreateWebService())
                            //{
                            //    using (var dataset = web.GetDataSet(tableids.ToArray()))
                            //    {
                            //        dataset.WriteXml(sf.FileName, System.Data.XmlWriteMode.WriteSchema);
                            //    }
                            //    MessageBox.Show(this, "Output Sucessed!");
                            //    this.Close();
                            //}
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Helper.ShowError(ex);
            }
        }
    }
}
