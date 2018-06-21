using Microsoft.EntityFrameworkCore.Migrations.Operations;
using System;
using System.Collections.Generic;
using System.Text;

namespace Way.EntityDB.Design.Actions
{
    public class EF_DeleteTable_Action : EFAction
    {
        public EJ.DBTable Table;
        public EJ.IDXIndex[] Indexes;
        public EF_DeleteTable_Action()
        {

        }

        protected override List<MigrationOperation> GetOperations()
        {
            List<MigrationOperation> operations = new List<MigrationOperation>();
            #region operations
            if(Indexes != null)
            {
                foreach( var index in Indexes )
                {
                    var _DropIndexOperation = new DropIndexOperation();
                    _DropIndexOperation.Table = this.Table.Name.ToLower();
                    _DropIndexOperation.Name = EF_CreateTable_Action.GetIndexName(index.Keys.Split(','), this.Table.id.Value);
                    operations.Add(_DropIndexOperation);
                }
            }

            var _DropTableOperation = new DropTableOperation() {
                Name = this.Table.Name.ToLower()
            };
            operations.Add(_DropTableOperation);
            #endregion
            return operations;
        }

        public EF_DeleteTable_Action(EJ.DBTable table, EJ.IDXIndex[] indexInfos)
        {
            this.Table = table.ToJsonString().ToJsonObject<EJ.DBTable>();
            this.Indexes = indexInfos.ToJsonString().ToJsonObject<EJ.IDXIndex[]>();
        }

        public override void Invoke(IDatabaseService invokingDB)
        {
            base.Invoke(invokingDB);
        }

        internal override void BeforeSave()
        {
            
        }
    }
}
