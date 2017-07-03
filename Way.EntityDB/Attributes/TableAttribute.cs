using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Way.EntityDB.Attributes
{
    public class Table:Attribute
    {
        public string KeyName
        {
            get;
            set;
        }
        public Table( string keyname)
        {

            this.KeyName = keyname;
        }
    }
}
