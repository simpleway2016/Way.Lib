using Way.EntityDB.Design.Services;
using System;
using System.Collections.Generic;
using System.Linq;


namespace Way.EntityDB.Design.Actions
{
    public class DeleteTableAction : Action
    {
        public string TableName
        {
            get;
            set;
        }

         public DeleteTableAction()
        {
        }
         internal override void BeforeSave()
         {
             
         }
         public DeleteTableAction(string tableName)
        {
            this.TableName = tableName;
        }
         public override void Invoke(EntityDB.IDatabaseService invokingDB)
        {
  
             ITableDesignService service = DBHelper.CreateTableDesignService(invokingDB.DBContext.DatabaseType);
            service.DeleteTable( invokingDB , this.TableName);
        }

        public override string ToString()
        {
            return string.Format("Delete Table {0}" , this.TableName);
        }
    }
}