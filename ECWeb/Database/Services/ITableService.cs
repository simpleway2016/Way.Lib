using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ECWeb.Database.Services
{
    public interface ITableService
    {
        void CreateTable(EntityDB.IDatabaseService database, EJ.DBTable table, EJ.DBColumn[] columns, IndexInfo[] IDXConfigs);
        void ChangeTable(EntityDB.IDatabaseService database, string oldTableName, string newTableName, EJ.DBColumn[] addColumns, EJ.DBColumn[] changedColumns, EJ.DBColumn[] deletedColumns, EJ.DBColumn[] otherColumns, IndexInfo[] IDXConfigs);
        void DeleteTable(EntityDB.IDatabaseService database, string tableName);
    }
}