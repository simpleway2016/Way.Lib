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
        public WayDataColumn(string name)
        {
            this.ColumnName = name;
        }
    }

    public class WayDataRow : Dictionary<string, object> 
    {
       
    }
}
