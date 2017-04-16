using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
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
                            this.IsEnabled = false;
                            output(sf.FileName , tableids.ToArray());

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Helper.ShowError(this, ex);
            }
        }

        async void output(string filename,int[] tableids)
        {
            this.Title = $"正在导出...0%";
            System.IO.BinaryReader br = null;
            int[] rowcounts = null;
            string tableNamesDesc = null;
            await Task.Run(()=> {
                var req = HttpWebRequest.Create(Helper.WebSite + "/DownloadTableData.aspx?tableids=" + tableids.ToJsonString()) as System.Net.HttpWebRequest;
                req.Headers["Cookie"] = $"WayScriptRemoting={Net.RemotingClient.SessionID}";
                req.AllowAutoRedirect = true;
                req.KeepAlive = false;
                req.Timeout = 20000;
                req.ServicePoint.ConnectionLeaseTimeout = 2 * 60 * 1000;

                var res = req.GetResponse() as System.Net.HttpWebResponse;
                br = new System.IO.BinaryReader(res.GetResponseStream());
                tableNamesDesc = br.ReadString();
                rowcounts = br.ReadString().ToJsonObject<int[]>();
            });
            int totalCount = rowcounts.Sum();
            using (System.IO.BinaryWriter bw = new System.IO.BinaryWriter(System.IO.File.Create(filename)))
            {
                bw.Write(tableNamesDesc);
                bw.Write(rowcounts.ToJsonString());

                int readed = 0;
                bool isEnd = false;
                while (!isEnd)
                {
                    await Task.Run(()=> {
                        string tablename = br.ReadString();
                        bw.Write(tablename);
                        if (tablename == ":end")
                        {
                            isEnd = true;
                        }
                        else
                        {
                            bw.Write(br.ReadString());
                            readed++;
                        }
                    });
                    if (totalCount > 0)
                    {
                        this.Title = $"正在导出...{ (readed * 100) / totalCount }%";
                    }
                }
                bw.Close();
            }
            Helper.ShowMessage(this, "数据导出完毕！");
            this.Close();
        }
    }
}
