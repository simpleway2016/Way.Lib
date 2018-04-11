using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EJClient.Forms
{
    public partial class DataViewer : Form
    {
        int pagecount;
        int pagesize = 20;
        int pageindex = 0;
        string m_pkName;
        EJ.DBTable m_Table;
        EJ.DBColumn[] m_columns;
        public DataViewer(EJ.DBTable table)
        {
            InitializeComponent();
            bindingNavigatorAddNewItem.Enabled = true;
            bindingNavigatorDeleteItem.Enabled = true;
            checkedListBox1.Items.Add("全选", true);

            m_Table = table;
            this.Text = table.Name;
            var history = histories.FirstOrDefault(m=>m.DBID == m_Table.DatabaseID && m.TableName == m_Table.Name);
            if (history != null)
            {
                notSelecteds = history.NotSelecteds;
            }
            
        }
        class history
        {
           public int DBID;
            public string TableName;
            public List<string> NotSelecteds;
        }
        static List<history> histories = new List<history>();
        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            if (histories.Count(m => m.DBID == m_Table.DatabaseID && m.TableName == m_Table.Name) == 0)
            {
                histories.Add(new history()
                {
                    DBID = m_Table.DatabaseID.Value,
                    TableName = m_Table.Name,
                    NotSelecteds = notSelecteds,
                });
            }

            base.OnFormClosed(e);
            this.Dispose();
        }
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            this.Cursor = Cursors.WaitCursor;
            Helper.Client.Invoke<string>("GetObjectFormat",(formatString,err) => {
                if(err != null)
                {
                    this.Cursor = Cursors.Default;
                    MessageBox.Show(this , err);
                }
                else
                {
                   
                    Helper.Client.Invoke<EJ.DBColumn[]>("GetColumnList",(ret,err2)=> {
                        this.Cursor = Cursors.Default;
                        if (err2 != null)
                        {
                            MessageBox.Show(this, err2);
                        }
                        else
                        {
                            m_columns = ret;
                            StringBuilder fieldString = new StringBuilder();
                            try
                            {
                                m_pkName = m_columns.FirstOrDefault(m => m.IsPKID == true).Name;
                            }
                            catch
                            {
                            }
                            foreach (var column in m_columns)
                            {
                                if (fieldString.Length > 0)
                                    fieldString.Append(',');
                                fieldString.Append(string.Format(formatString, column.Name.ToLower()));
                            }

                            richTextBox1.Text = string.Format("select {1} from {0}", string.Format(formatString, m_Table.Name.ToLower()), fieldString);
                            bindData();
                        }
                      
                    }, m_Table.id.Value);
                   
                }
            }, m_Table.id.Value);
            
        }

        public static SqlDbType SqlTypeString2SqlType(string sqlTypeString)
        {
            SqlDbType dbType = SqlDbType.Variant;//默认为Object
            switch (sqlTypeString)
            {
                case "int":
                    dbType = SqlDbType.Int;
                    break;
                case "varchar":
                    dbType = SqlDbType.VarChar;
                    break;
                case "bit":
                    dbType = SqlDbType.Bit;
                    break;
                case "datetime":
                    dbType = SqlDbType.DateTime;
                    break;
                case "decimal":
                    dbType = SqlDbType.Decimal;
                    break;
                case "float":
                    dbType = SqlDbType.Float;
                    break;
                case "image":
                    dbType = SqlDbType.Image;
                    break;
                case "money":
                    dbType = SqlDbType.Money;
                    break;
                case "ntext":
                    dbType = SqlDbType.NText;
                    break;
                case "nvarchar":
                    dbType = SqlDbType.NVarChar;
                    break;
                case "smalldatetime":
                    dbType = SqlDbType.SmallDateTime;
                    break;
                case "smallint":
                    dbType = SqlDbType.SmallInt;
                    break;
                case "text":
                    dbType = SqlDbType.Text;
                    break;
                case "bigint":
                    dbType = SqlDbType.BigInt;
                    break;
                case "binary":
                    dbType = SqlDbType.Binary;
                    break;
                case "char":
                    dbType = SqlDbType.Char;
                    break;
                case "nchar":
                    dbType = SqlDbType.NChar;
                    break;
                case "numeric":
                    dbType = SqlDbType.Decimal;
                    break;
                case "real":
                    dbType = SqlDbType.Real;
                    break;
                case "smallmoney":
                    dbType = SqlDbType.SmallMoney;
                    break;
                case "sql_variant":
                    dbType = SqlDbType.Variant;
                    break;
                case "timestamp":
                    dbType = SqlDbType.Timestamp;
                    break;
                case "tinyint":
                    dbType = SqlDbType.TinyInt;
                    break;
                case "uniqueidentifier":
                    dbType = SqlDbType.UniqueIdentifier;
                    break;
                case "varbinary":
                    dbType = SqlDbType.VarBinary;
                    break;
                case "xml":
                    dbType = SqlDbType.Xml;
                    break;
            }
            return dbType;
        }





        // SqlDbType转换为C#数据类型
        public static Type SqlType2CsharpType(SqlDbType sqlType)
        {
            switch (sqlType)
            {
                case SqlDbType.BigInt:
                    return typeof(Int64);
                case SqlDbType.Binary:
                    return typeof(Object);
                case SqlDbType.Bit:
                    return typeof(Boolean);
                case SqlDbType.Char:
                    return typeof(String);
                case SqlDbType.DateTime:
                    return typeof(DateTime);
                case SqlDbType.Decimal:
                    return typeof(Decimal);
                case SqlDbType.Float:
                    return typeof(Double);
                case SqlDbType.Image:
                    return typeof(Object);
                case SqlDbType.Int:
                    return typeof(Int32);
                case SqlDbType.Money:
                    return typeof(Decimal);
                case SqlDbType.NChar:
                    return typeof(String);
                case SqlDbType.NText:
                    return typeof(String);
                case SqlDbType.NVarChar:
                    return typeof(String);
                case SqlDbType.Real:
                    return typeof(Single);
                case SqlDbType.SmallDateTime:
                    return typeof(DateTime);
                case SqlDbType.SmallInt:
                    return typeof(Int16);
                case SqlDbType.SmallMoney:
                    return typeof(Decimal);
                case SqlDbType.Text:
                    return typeof(String);
                case SqlDbType.Timestamp:
                    return typeof(Object);
                case SqlDbType.TinyInt:
                    return typeof(Byte);
                case SqlDbType.Udt://自定义的数据类型
                    return typeof(Object);
                case SqlDbType.UniqueIdentifier:
                    return typeof(Object);
                case SqlDbType.VarBinary:
                    return typeof(Object);
                case SqlDbType.VarChar:
                    return typeof(String);
                case SqlDbType.Variant:
                    return typeof(Object);
                case SqlDbType.Xml:
                    return typeof(Object);
                default:
                    return null;
            }
        }

        
        List<string> notSelecteds = new List<string>();
        private void bindData()
        {
            this.Cursor = Cursors.WaitCursor;
            Helper.Client.Invoke<WayDataTable>("GetDataTable",(r,err)=> {
                this.Cursor = Cursors.Default;
                if (err != null)
                {
                    MessageBox.Show(this, err);
                }
                else
                {
                    var dt = r.ToDataTable();
                    if (dt.Rows.Count == 0)
                    {

                        //如果没有数据，可能column的数据类型都会是int，这样不对
                        for (int i = 0; i < dt.Columns.Count; i++)
                        {
                            try
                            {
                                dt.Columns[i].DataType = SqlType2CsharpType(SqlTypeString2SqlType(m_columns.FirstOrDefault(m => string.Equals( m.Name , dt.Columns[i].ColumnName, StringComparison.CurrentCultureIgnoreCase)).dbType));
                            }
                            catch {
                                dt.Columns[i].DataType = typeof(string);
                            }
                        }
                    }

                    int rowcount = Convert.ToInt32( dt.TableName);
                    dt.AcceptChanges();
                    deletedIds.Clear();
                    dataGridView1.DataSource = dt;
                    bool addChk = false;
                    if (checkedListBox1.Items.Count != dt.Columns.Count)
                    {
                        addChk = true;
                        while (checkedListBox1.Items.Count > 1)
                            checkedListBox1.Items.RemoveAt(1);

                    }

                    for (int i = 0; i < dt.Columns.Count; i++)
                    {
                        if (addChk)
                        {
                            checkedListBox1.Items.Add(dt.Columns[i].ColumnName, notSelecteds.Contains(dt.Columns[i].ColumnName) ? false : true);
                        }

                        if (dt.Columns[i].DataType == typeof(DateTime))
                        {
                            dataGridView1.Columns[i].DefaultCellStyle.Format = "yyyy-MM-dd HH:mm:ss";
                        }
                        else if (dt.Columns[i].DataType == typeof(bool))
                        {
                            dataGridView1.Columns[i].DefaultCellStyle.Format = "{0}";
                        }
                    }

                    pagecount = rowcount / pagesize;
                    if (rowcount % pagesize > 0)
                        pagecount++;

                    bindingNavigator1.Enabled = true;
                    bindingNavigatorCountItem.Text = "/ " + pagecount;
                    bindingNavigatorPositionItem.Text = Convert.ToString(pageindex + 1);
                    bindingNavigatorPositionItem.Enabled = true;

                    bindingNavigatorMovePreviousItem.Enabled = pageindex > 0;
                    bindingNavigatorMoveFirstItem.Enabled = pageindex > 0;

                    bindingNavigatorMoveNextItem.Enabled = pageindex < pagecount - 1;
                    bindingNavigatorMoveLastItem.Enabled = bindingNavigatorMoveNextItem.Enabled;

                    hideGridColumn();
                }
            }, richTextBox1.Text, m_Table.id.Value, pageindex, pagesize);

          
        }

        private void bindingNavigatorMoveNextItem_Click(object sender, EventArgs e)
        {
            pageindex++;
            bindData();
        }

        private void bindingNavigatorMoveLastItem_Click(object sender, EventArgs e)
        {
            pageindex = pagecount - 1;
            bindData();
        }

        private void bindingNavigatorMovePreviousItem_Click(object sender, EventArgs e)
        {
            pageindex--;
            bindData();
        }

        private void bindingNavigatorMoveFirstItem_Click(object sender, EventArgs e)
        {
            pageindex = 0;
            bindData();
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            pageindex = 0;
            bindData();
        }

        private void bindingNavigatorAddNewItem_Click(object sender, EventArgs e)
        {
            DataTable dt = dataGridView1.DataSource as DataTable;
            DataRow drow = dt.NewRow();
            dt.Rows.InsertAt(drow, 0);
        }
        List<string> deletedIds = new List<string>();
        private void bindingNavigatorDeleteItem_Click(object sender, EventArgs e)
        {
            while (  dataGridView1.SelectedRows.Count > 0)
            {
                DataRowView datarow = dataGridView1.SelectedRows[0].DataBoundItem as DataRowView;
                if (datarow != null)
                {
                    deletedIds.Add(datarow[m_pkName].ToString());
                    datarow.Delete();
                }
            }


        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            dataGridView1.EndEdit();
            try
            {
                var oldsource = (DataTable)dataGridView1.DataSource;
                DataTable dt = new DataTable();
                dt.TableName = oldsource.TableName;
                foreach (DataColumn column in oldsource.Columns)
                {
                    dt.Columns.Add(column.ColumnName.ToLower() , column.DataType);
                }
                foreach (DataRow drow in oldsource.Rows)
                {
                    if (drow.RowState == System.Data.DataRowState.Added || drow.RowState == System.Data.DataRowState.Modified)
                    {
                        DataRow newRow = dt.NewRow();
                        foreach (DataColumn column in oldsource.Columns)
                        {
                            newRow[column.ColumnName.ToLower()] = drow[column.ColumnName.ToLower()];
                        }
                        dt.Rows.Add(newRow);
                        newRow.AcceptChanges();

                        if (drow.RowState == System.Data.DataRowState.Added)
                            newRow.SetAdded();
                        else if (drow.RowState == System.Data.DataRowState.Deleted)
                            newRow.Delete();
                        else if (drow.RowState == System.Data.DataRowState.Modified)
                            newRow.SetModified();
                    }
                }

                if (dt != null)
                {
                    Helper.Client.InvokeSync<string>("SaveDataTable", new WayDataTable(dt), m_Table.id.Value, deletedIds.ToArray());
                    deletedIds.Clear();
                    dt.Dispose();
                    oldsource.AcceptChanges();
                }
                else
                {
                    MessageBox.Show("没有变化的数据");
                }
                dt.Dispose();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void dataGridView1_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            e.Cancel = true;
            if(e.Exception.HResult != -2147024809)
            MessageBox.Show(this, e.Exception.Message);
        }

        private void dataGridView1_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
           
            var oldsource = (DataTable)dataGridView1.DataSource;
            var row = oldsource.Rows[e.RowIndex];
            var value = row[e.ColumnIndex];
            if (row.RowState == System.Data.DataRowState.Unchanged)
            {
                row.SetModified();
                row[e.ColumnIndex] = value;
            }
        }

        void hideGridColumn()
        {
            var oldsource = (DataTable)dataGridView1.DataSource;
            for (int i = 0; i < oldsource.Columns.Count; i++)
            {
                if (notSelecteds.Contains(oldsource.Columns[i].ColumnName))
                {
                    dataGridView1.Columns[i].Visible = false;
                }
                else
                {
                    dataGridView1.Columns[i].Visible = true;
                }
            }
            this.Cursor = Cursors.Default;
        }

        private void checkedListBox1_SelectedValueChanged(object sender, EventArgs e)
        {
           
        }

        private void checkedListBox1_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            if (this.Cursor != Cursors.Default)
                return;
            if (e.Index == 0 && checkedListBox1.Items.Count > 1)
            {
                this.Cursor = Cursors.WaitCursor;
                bool _checked = (e.NewValue == CheckState.Checked);
                for (int i = 1; i < checkedListBox1.Items.Count; i++)
                {
                    checkedListBox1.SetItemChecked(i, _checked);
                    string columnName = checkedListBox1.Items[i].ToString();
                    if (!_checked)
                    {
                        if (notSelecteds.Contains(columnName) == false)
                            notSelecteds.Add(columnName);
                    }
                }
                if (_checked)
                    notSelecteds.Clear();
                hideGridColumn();
            }
            else if(e.Index > 0)
            {
                for (int i = 1; i < checkedListBox1.Items.Count; i++)
                {
                    string columnName = checkedListBox1.Items[i].ToString();
                    bool _checked = checkedListBox1.GetItemChecked(i) ;
                    if (i == e.Index && e.NewValue == CheckState.Unchecked)
                        _checked = false;
                    else if (i == e.Index && e.NewValue == CheckState.Checked)
                        _checked = true;
                    if (_checked )
                    {
                        if (notSelecteds.Contains(columnName))
                        {
                            notSelecteds.Remove(columnName);
                        }
                    }
                    else
                    {
                        if (notSelecteds.Contains(columnName) == false)
                        {
                            notSelecteds.Add(columnName);
                        }
                    }
                }
                hideGridColumn();
            }
        }
    }
}
