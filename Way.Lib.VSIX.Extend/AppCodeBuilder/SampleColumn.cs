using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Way.Lib.VSIX.Extend.AppCodeBuilder
{
    public class SampleColumn
    {
        internal static string[] s_Tables;
        public SampleColumn()
        {
            this.RelaColumns = new System.Collections.ObjectModel.ObservableCollection<string>();
        }
        public string Name
        {
            get;
            set;
        }
        public string caption
        {
            get;
            set;
        }
    
        public bool IsSelected
        {
            get;
            set;
        }
        public bool IsExpanded
        {
            get;
            set;
        }
        public bool IsChecked
        {
            get;
            set;
        }

        public string[] Tables
        {
            get
            {
                return s_Tables;
            }
        }
        public string RelaTableName
        {
            get;
            set;
        }
        public string RelaColumnName
        {
            get;
            set;
        }
        public System.Collections.ObjectModel.ObservableCollection<string> RelaColumns
        {
            get;
            set;
        }
        public string DisplayColumnName { get; set; }
    }
}
