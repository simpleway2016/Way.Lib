using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ECWeb.Database
{
    interface ICodeBuilder
    {
        string BuilderDB(EJDB db, string database, string nameSpace, List<EJ.DBTable> tables);
        string[] BuildTable(EJDB db, string nameSpace, EJ.DBTable table);
        string BuildOldClassCode(EJDB db, string nameSpace, EJ.DBTable table, List<EJ.DBColumn> columns);
    }
}