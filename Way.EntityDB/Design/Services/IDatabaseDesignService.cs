using System;
using System.Collections.Generic;
using System.Linq;


namespace Way.EntityDB.Design.Services
{
    public interface IDatabaseDesignService
    {
        void Create(EJ.Databases database);
        void CreateEasyJobTable(EntityDB.IDatabaseService db);
        void ChangeName(EJ.Databases database, string newName,string newConnectString);
        void ImportData(EntityDB.IDatabaseService db, EJ.DB.EasyJob ejdb, WayDataSet dset, bool clearDataFirst);
        string GetObjectFormat();
    }
}