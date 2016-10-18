using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ECWeb.Database.Services
{
    public interface IDatabaseService
    {
        void Create(EJ.Databases database);
        void CreateEasyJobTable(EntityDB.IDatabaseService db);
        void ChangeName(EJ.Databases database, string newName,string newConnectString);
        void GetViews(EntityDB.IDatabaseService db, out List<EJ.DBTable> tables, out List<EJ.DBColumn> columns);
        void ImportData(EntityDB.IDatabaseService db, EJDB ejdb, System.Data.DataSet dset, bool clearDataFirst);
        string GetObjectFormat();
    }
}