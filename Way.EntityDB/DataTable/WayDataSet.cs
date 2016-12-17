using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Way.EntityDB
{
    public class WayDataSet : IDisposable
    {
        public List<WayDataTable> Tables
        {
            get;
            private set;
        }
        public string DataSetName
        {
            get;
            set;
        }
    
        public WayDataSet()
        {
            this.Tables = new List<WayDataTable>();
        }

        public void Dispose()
        {
            for (int i = 0; i < this.Tables.Count; i++)
            {
                this.Tables[i].Dispose();
                
            }
            this.Tables.Clear();
        }
    }
}
