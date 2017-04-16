using System;
using System.Collections.Generic;
using System.Data;
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
    /// ImportData.xaml 的交互逻辑
    /// </summary>
    public partial class ImportData : Window
    {
        System.Data.DataSet m_dset;
        string _filepath;
        TreeNode.DatabaseItemNode m_databaseItemNode;
        internal ImportData(string filename, TreeNode.DatabaseItemNode databaseItemNode)
        {
            m_databaseItemNode = databaseItemNode;
            InitializeComponent();
            _filepath = filename;

            using (System.IO.BinaryReader br = new System.IO.BinaryReader(System.IO.File.OpenRead(filename)))
            {
                list.ItemsSource = br.ReadString().ToJsonObject<string[]>();
            }
            list.SelectAll();
            list.Focus();
        }

        int m_total = 0;
        private void btnOK_Click_1(object sender, RoutedEventArgs e)
        {
            this.IsEnabled = false;

            import();
        }

        async void import()
        {
            using (System.IO.BinaryReader br = new System.IO.BinaryReader(System.IO.File.OpenRead(_filepath)))
            {
                string result = null;
                string tablenames = br.ReadString();
                int[] rowCounts = br.ReadString().ToJsonObject<int[]>();
                var req = HttpWebRequest.Create($"{Helper.WebSite}/ImportTableData.aspx?dbid={m_databaseItemNode.Database.id}&clearDataFirst={(chkClearDataFirst.IsChecked == true ? 1 : 0)}") as System.Net.HttpWebRequest;

                string[] importTables = new string[list.SelectedItems.Count];
                for(int i = 0; i < list.SelectedItems.Count; i ++)
                {
                    importTables[i] = list.SelectedItems[i].ToString();
                }
                await Task.Run(()=> {
                    req.Headers["Cookie"] = $"WayScriptRemoting={Net.RemotingClient.SessionID}";
                    req.AllowAutoRedirect = true;
                    req.KeepAlive = false;
                    req.Timeout = 2000000;
                    req.ServicePoint.ConnectionLeaseTimeout = 2 * 60 * 1000;
                    req.Method = "POST";
                    req.ContentType = "import";

                   
                    var request = req.GetRequestStream();
                    request.Flush();

                    System.IO.BinaryWriter bw = new System.IO.BinaryWriter(request);
                    bw.Write(importTables.ToJsonString());

                  
                    while (true)
                    {
                        string tablename = br.ReadString();
                        if (tablename == ":end")
                        {
                            break;
                        }
                        string content = br.ReadString();
                        if (importTables.Contains(tablename) == false)
                            continue;
                        bw.Write(tablename);
                        bw.Write(content);
                    }
                    bw.Write(":end");

                    var res = req.GetResponse() as System.Net.HttpWebResponse;
                    var reader = new System.IO.BinaryReader(res.GetResponseStream());
                    result = reader.ReadString();
                    
                });
                if (result != "ok")
                {
                    this.IsEnabled = true;
                    Helper.ShowError(this , result);
                }
                else
                {
                   
                    Helper.ShowMessage(this, "导入完毕！");
                    this.Close();
                }
            }
        }
    }
}
