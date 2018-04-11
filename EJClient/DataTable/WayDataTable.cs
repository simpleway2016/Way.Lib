using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EJClient
{

    public class WayDataTable:IDisposable
    {
        public string TableName
        {
            get;
            set;
        }

        public List<WayDataRow> Rows
        {
            get;
            private set;
        }
        public List<WayDataColumn> Columns
        {
            get;
            private set;
        }
        public WayDataTable()
        {
            Columns = new List<WayDataColumn>();
            this.Rows = new List<WayDataRow>();
        }


        public WayDataTable(System.Data.DataTable source):this()
        {
            this.Rows.Clear();
            this.TableName = source.TableName;
            foreach (System.Data.DataColumn column in source.Columns)
            {
                this.Columns.Add(new WayDataColumn(column.ColumnName , column.DataType.FullName));
            }
            foreach (System.Data.DataRow row in source.Rows)
            {
                var newrow = new WayDataRow();
                this.Rows.Add(newrow);
                newrow.RowState = (DataRowState)row.RowState;
                foreach (System.Data.DataColumn column in source.Columns)
                {
                    newrow[column.ColumnName] = row[column.ColumnName];
                }
            }
        }

        public System.Data.DataTable ToDataTable()
        {
            var dtable = new System.Data.DataTable();
            dtable.TableName = this.TableName;
            foreach (var column in this.Columns)
            {
                dtable.Columns.Add(new System.Data.DataColumn(column.ColumnName, typeof(int).Assembly.GetType(column.DataType)));
            }
            foreach (var row in this.Rows)
            {
                var newrow = dtable.NewRow();
                foreach (var column in this.Columns)
                {
                    if(row[column.ColumnName] != null)
                    {
                        var value = row[column.ColumnName];
                        if(column.DataType == "System.Byte[]" && value is string)
                        {
                            value = Convert.FromBase64String((string)value);
                        }
                        newrow[column.ColumnName] = value;
                     }
                }
                dtable.Rows.Add(newrow);
            }
            dtable.AcceptChanges();
            return dtable;
        }

        public void Dispose()
        {
            for (int i = 0; i < this.Rows.Count; i++)
            {
                this.Rows[i].Clear();

            }
            this.Rows.Clear();
            this.Columns.Clear();
        }
    }


    public class WayDataColumn
    {
        public string ColumnName
        {
            get;
            set;
        }
        public string DataType
        {
            get;
            set;
        }
        public WayDataColumn()
        {
        }
        public WayDataColumn(string name , string dtype)
        {
            this.DataType = dtype;
            this.ColumnName = name;
        }
    }
    public class ItemPair
    {
        public string Name;
        public object Value;
    }
    public enum DataRowState
    {
        //
        // 摘要:
        //     已创建该行，但它不是任何 System.Data.DataRowCollection 的一部分。System.Data.DataRow 在以下情况下立即处于此状态：创建之后添加到集合中之前；或从集合中移除之后。
        Detached = 1,
        //
        // 摘要:
        //     自上一次调用 System.Data.DataRow.AcceptChanges 之后，该行未更改。
        Unchanged = 2,
        //
        // 摘要:
        //     该行已添加到 System.Data.DataRowCollection 中，System.Data.DataRow.AcceptChanges 尚未调用。
        Added = 4,
        //
        // 摘要:
        //     该行已通过 System.Data.DataRow 的 System.Data.DataRow.Delete 方法被删除。
        Deleted = 8,
        //
        // 摘要:
        //     该行已被修改，System.Data.DataRow.AcceptChanges 尚未调用。
        Modified = 16
    }
    public class WayDataRow  
    {
        List<ItemPair> _items = new List<ItemPair>();
        public List<ItemPair> Items
        {
            get
            {
                return _items;
            }
        }
        public DataRowState RowState
        {
            get;
            set;
        }
        public object this[string name]
        {
            get
            {
                var item = _items.FirstOrDefault(m => string.Equals(m.Name , name , StringComparison.CurrentCultureIgnoreCase));
                if (item == null)
                {
                    return null;
                }
                else
                {
                    return item.Value;
                }
            }
            set
            {
                if (value == DBNull.Value)
                    value = null;
                var item = _items.FirstOrDefault(m => string.Equals(m.Name, name, StringComparison.CurrentCultureIgnoreCase));
                if (item==null)
                {
                    _items.Add(new ItemPair() {Name = name,Value=value });
                }
                else
                {
                    item.Value = value;
                }
            }
        }

        public object this[int index]
        {
            get
            {

                if (index >= _items.Count)
                {
                    return null;
                }
                else
                {
                    return _items[index].Value;
                }
            }
            set
            {
                if (index >= _items.Count)
                {
                    throw new Exception("索引超出范围");
                }
                else
                {
                    _items[index].Value = value;
                }

            }
        }

        public void Clear()
        {
            _items.Clear();
        }
    }
}
