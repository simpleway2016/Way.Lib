using System;
using System.Collections.Generic;
using System.Linq;

namespace Way.EJServer
{
    interface ICodeBuilder
    {
        string BuilderDB(EJDB db, EJ.Databases database, string nameSpace, List<EJ.DBTable> tables);
        string[] BuildTable(EJDB db, string nameSpace, EJ.DBTable table);
        string[] BuildSimpleTable(EJDB db, string nameSpace, EJ.DBTable table);
    }
}