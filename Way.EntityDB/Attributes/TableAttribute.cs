using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Way.EntityDB.Attributes
{
    public class Table:Attribute
    {
        public string TableName
        {
            get;
            set;
        }
        public string IDField
        {
            get;
            set;
        }
        public Table(string tableName , string idfield)
        {
            this.TableName = tableName;
            this.IDField = idfield;
        }
    }
}
