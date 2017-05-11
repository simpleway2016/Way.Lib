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
                    await Task.Run(() =>
                    {
                        var dbservice = Way.EntityDB.Design.DBHelper.CreateDatabaseDesignService((Way.EntityDB.DatabaseType)(int)_source.dbType);
                        var db = Way.EntityDB.DBContext.CreateDatabaseService(_source.conStr, (Way.EntityDB.DatabaseType)(int)_source.dbType);
                        foreach (var table in importTables)
                        {
                            var columns = dbservice.GetCurrentColumns(db, table);
                            var indexes = dbservice.GetCurrentIndexes(db, table);

                            //Helper.Client.InvokeSync<int>
                        }

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
