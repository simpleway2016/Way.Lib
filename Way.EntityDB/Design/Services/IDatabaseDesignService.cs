using System;
using System.Collections.Generic;
using System.Linq;


namespace Way.EntityDB.Design.Services
{
    public interface IDatabaseDesignService
    {
        void Drop(EJ.Databases database);
        void Create(EJ.Databases database);
        void CreateEasyJobTable(EntityDB.IDatabaseService db);
        void ChangeName(EJ.Databases database, string newName,string newConnectString);
        List<EJ.DBColumn> GetCurrentColumns(IDatabaseService db, string tablename);
        string GetObjectFormat();
    }
}