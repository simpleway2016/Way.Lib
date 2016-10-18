using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EntityDB.Attributes
{
    public class Table:Attribute
    {
        public string IDField
        {
            get;
            set;
        }
        public Table(string idfield)
        {
            this.IDField = idfield;
        }
    }
}
