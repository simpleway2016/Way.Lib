using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Way.EntityDB.Attributes
{
    public class DatabaseTypeAttribute : Attribute
    {
        public EntityDB.DatabaseType DBType
        {
            get;
            set;
        }
        public DatabaseTypeAttribute(EntityDB.DatabaseType type)
        {
            DBType = type;
        }
    }
}
