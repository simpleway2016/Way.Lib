using Microsoft.EntityFrameworkCore.Migrations.Operations;
using System;
using System.Collections.Generic;
using System.Text;

namespace Way.EntityDB.Design.Services
{
    interface ITempDBContext
    {
        void BeforeAction(IDatabaseService invokingDB);
        void AfterAction(IDatabaseService invokingDB);
        MigrationOperation[] CheckOperations(List<MigrationOperation> operations, IDatabaseService invokingDB);
        string GetColumnType(string dbtype, string length);
    }
}
