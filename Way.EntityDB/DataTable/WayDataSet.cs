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

#if NET46
        public WayDataSet(System.Data.DataSet dset)
        {
            this.DataSetName = dset.DataSetName;
            foreach (System.Data.DataTable t in dset.Tables)
            {
                this.Tables.Add(new EntityDB.WayDataTable(t));
            }
        }

        public System.Data.DataSet ToDataSet()
        {
            var dset = new System.Data.DataSet();
            if (this.DataSetName.IsNullOrEmpty() == false)
            {
                dset.DataSetName = this.DataSetName;
            }
            foreach (var mytable in this.Tables)
            {
                dset.Tables.Add(mytable.ToDataTable());
            }
            return dset;
        }
#endif

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
