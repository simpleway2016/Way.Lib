using System;
using System.Collections.Generic;
using System.Data;
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
    /// ImportData.xaml 的交互逻辑
    /// </summary>
    public partial class ImportData : Window
    {
        System.Data.DataSet m_dset;
        TreeNode.DatabaseItemNode m_databaseItemNode;
        internal ImportData(string filename, TreeNode.DatabaseItemNode databaseItemNode)
        {
            m_databaseItemNode = databaseItemNode;
            InitializeComponent();

            m_dset = new System.Data.DataSet();
            m_dset.ReadXml(filename);
            List<string> names = new List<string>();
            foreach (System.Data.DataTable t in m_dset.Tables)
            {
                names.Add(t.TableName);
            }
            list.ItemsSource = names;
            list.SelectAll();
            list.Focus();
        }

        private DataTable CopyData(DataTable source, int offset, int count)
        {
            DataTable dtable = new DataTable();
            dtable.TableName = source.TableName;
            for(int i = 0 ; i < source.Columns.Count ; i ++)
            {
                dtable.Columns.Add(new DataColumn(source.Columns[i].ColumnName , source.Columns[i].DataType));
            }
            for (int i = offset; i < offset + count && i < source.Rows.Count; i++)
            {
                DataRow drow = dtable.NewRow();
                for (int j = 0; j < dtable.Columns.Count; j++)
                {
                    drow[j] = source.Rows[i][j];
                }
                dtable.Rows.Add(drow);
            }
            return dtable;
        }

        int m_total = 0;
        private void btnOK_Click_1(object sender, RoutedEventArgs e)
        {
            btnOK.IsEnabled = false;
            btnCancel.IsEnabled = false;
            List<string> names = new List<string>();
            foreach (string name in list.SelectedItems)
            {
                names.Add(name);
            }
            for (int i = 0; i < m_dset.Tables.Count; i++)
            {
                if (names.Contains(m_dset.Tables[i].TableName))
                {
                    m_total += m_dset.Tables[i].Rows.Count;
                }
            }
            new System.Threading.Thread(import).Start();
        }

        void import()
        {
            bool firstCheckValue = false;
            List<string> names = new List<string>();
            this.Dispatcher.Invoke(new Action(() =>
                {
                    firstCheckValue = chkClearDataFirst.IsChecked.GetValueOrDefault();
                    foreach (string name in list.SelectedItems)
                    {
                        names.Add(name);
                    }
                }));
            
            int done = 0;
            try
            {
                bool postchecked = true;

                for (int i = 0; i < m_dset.Tables.Count; i++)
                {
                    if (names.Contains(m_dset.Tables[i].TableName))
                    {
                        int index = 0;
                        while (true)
                        {
                            using (DataSet newSet = new DataSet())
                            {
                                DataTable dtable = CopyData(m_dset.Tables[i], index, 100);
                                index += 100;
                                if (dtable.Rows.Count == 0)
                                    break;
                                newSet.Tables.Add(dtable);

                                Helper.Client.InvokeSync<string>("ImportData", new Way.EntityDB.WayDataSet(newSet), m_databaseItemNode.Database.id.Value, postchecked ? firstCheckValue : false);
                                postchecked = false;
                                done += dtable.Rows.Count;
                                this.Dispatcher.Invoke(new Action(() =>
                                {
                                    this.Title = "进度:" + (done * 100 / m_total) + "%";
                                }));
                            }

                        }
                    }
                }

                this.Dispatcher.Invoke(new Action(() =>
                {
                    this.Title = "Import Sucessed!";
                    MessageBox.Show(this, "Import Sucessed!");
                    this.Close();
                }));
                
            }
            catch (Exception ex)
            {
                this.Dispatcher.Invoke(new Action(() =>
                {
                    btnOK.IsEnabled = true;
                    btnCancel.IsEnabled = true;
                    Helper.ShowError(ex);
                }));
               
            }
        }
    }
}
