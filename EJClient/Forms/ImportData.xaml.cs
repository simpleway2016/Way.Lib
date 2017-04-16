using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Sockets;
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
                var tablenames = br.ReadString().ToJsonObject< List<string>>();
                int[] rowCounts = br.ReadString().ToJsonObject<int[]>();

                var host = $"{Helper.WebSite}/";
                 host = host.Substring(host.IndexOf("://") + 3);
                host = host.Substring(0 , host.IndexOf("/"));
                int port = 80;
                if(host.Contains(":"))
                {
                    port = Convert.ToInt32( host.Split(':')[1]);
                    host = host.Split(':')[0];
                }

                string url = $"POST /ImportTableData.aspx?dbid={m_databaseItemNode.Database.id}&clearDataFirst={(chkClearDataFirst.IsChecked == true ? 1 : 0)}";
                string[] importTables = new string[list.SelectedItems.Count];
                for(int i = 0; i < list.SelectedItems.Count; i ++)
                {
                    importTables[i] = list.SelectedItems[i].ToString();
                }
                int totalCount = 0;
                for(int i = 0; i < importTables.Length; i ++)
                {
                    totalCount += rowCounts[tablenames.IndexOf(importTables[i])];
                }
                
                await Task.Run(()=> {
                    try
                    {
                        Socket socket = new Socket(SocketType.Stream, ProtocolType.Tcp);
                        socket.Connect(new System.Net.DnsEndPoint(host,port));
                        Way.Lib.NetStream client = new Way.Lib.NetStream(socket);
                        System.IO.StreamWriter stream = new System.IO.StreamWriter(client);
                        stream.WriteLine(url);
                        stream.WriteLine($"Cookie: WayScriptRemoting={Net.RemotingClient.SessionID}");
                        stream.WriteLine($"Content-Type: import");
                        stream.WriteLine("");
                        stream.Flush();


                        System.IO.BinaryWriter bw = new System.IO.BinaryWriter(client);
                        bw.Write(importTables.ToJsonString());

                        int uploaded = 0;
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
                            bw.Flush();
                            uploaded++;
                            if (totalCount > 0)
                            {
                                this.Dispatcher.Invoke(() =>
                                {
                                    this.Title = $"正在导入...{(uploaded * 100) / totalCount}%";
                                });
                            }
                        }
                        bw.Write(":end");
                        bw.Flush();

                        
                        var reader = new System.IO.StreamReader(client);
                        while(true)
                        {
                            if (reader.ReadLine().Length == 0)
                                break;
                        }
                        result = reader.ReadLine();
                        client.Close();
                    }
                    catch(Exception ex)
                    {
                        result = ex.Message;
                    }
                    
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
