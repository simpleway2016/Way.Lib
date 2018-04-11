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
    public partial class ImportSchema : Window
    {
        int _targetDatabaseID;
        EJ.Databases _source;
        internal ImportSchema(List<string> tablenames , int targetDatabaseID,EJ.Databases source)
        {
            InitializeComponent();
            _source = source;
            _targetDatabaseID = targetDatabaseID;

            list.ItemsSource = tablenames;
            list.SelectAll();
            list.Focus();
        }

        int m_total = 0;
        private void btnOK_Click_1(object sender, RoutedEventArgs e)
        {
            btnOK.Focus();
            this.IsEnabled = false;

            import();
        }

        async void import()
        {
            this.Cursor = Cursors.Wait;
            string[] importTables = new string[list.SelectedItems.Count];
            for (int i = 0; i < list.SelectedItems.Count; i++)
            {
                importTables[i] = list.SelectedItems[i].ToString();
            }
            try
            {
                if (importTables.Length > 0)
                {
                    List<TableInfo> tables = new List<TableInfo>();
                    await Task.Run(() =>
                    {
                        foreach (var table in importTables)
                        {
                            var columns = Helper.Client.InvokeSync<EJ.DBColumn[]>("GetDatabaseCurrentColumns", _source, table);
                           
                            var indexes = Helper.Client.InvokeSync<IndexInfo[]>("GetDatabaseCurrentIndexes", _source, table);

                            tables.Add(new TableInfo()
                            {
                                TableName = table,
                                Columns = columns.ToArray(),
                                Indexes = indexes.ToArray(),
                            });
                        }

                       

                    });

                    foreach( var tableinfo in tables )
                    {
                        var notSureColumn = tableinfo.Columns.Where(n => n.dbType.StartsWith("[未识别]")).ToArray();
                        if (notSureColumn.Count() > 0)
                        {
                            //含有不能确定的字段
                          if(  new ConfirmColumnsType(tableinfo).ShowDialog() == false)
                            {
                                this.Cursor = null;
                                this.IsEnabled = true;
                                return;
                            }
                        }
                    }

                    await Task.Run(()=> {
                        Helper.Client.InvokeSync<int>("CreateTables", _targetDatabaseID, tables);
                    });
                }
                this.Cursor = null;
                this.DialogResult = true;
            }
            catch(Exception ex)
            {
                Helper.ShowError(this, ex);
                this.DialogResult = false;
            }
        }
    }
}
