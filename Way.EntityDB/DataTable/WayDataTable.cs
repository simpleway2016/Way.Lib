using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Way.EntityDB
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

#if NET46
        public WayDataTable(System.Data.DataTable source):this()
        {
            this.Rows.Clear();
            this.TableName = source.TableName;
            foreach (System.Data.DataColumn column in source.Columns)
            {
                this.Columns.Add(new WayDataColumn(column.ColumnName , column.DataType));
            }
            foreach (System.Data.DataRow row in source.Rows)
            {
                var newrow = new WayDataRow();
                this.Rows.Add(newrow);
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
                dtable.Columns.Add(new System.Data.DataColumn(column.ColumnName, column.DataType));
            }
            foreach (var row in this.Rows)
            {
                var newrow = dtable.NewRow();
                foreach (var column in this.Columns)
                {
                    newrow[column.ColumnName] = row[column.ColumnName];
                }
                dtable.Rows.Add(newrow);
            }
            dtable.AcceptChanges();
            return dtable;
        }
#endif
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
        public Type DataType
        {
            get;
            set;
        }
        public WayDataColumn()
        {
        }
        public WayDataColumn(string name , Type dtype)
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
        public object this[string name]
        {
            get
            {
                var item = _items.FirstOrDefault(m => m.Name == name);
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
                var item = _items.FirstOrDefault(m => m.Name == name);
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
