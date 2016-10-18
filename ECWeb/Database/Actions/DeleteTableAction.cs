using ECWeb.Database.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ECWeb.Database.Actions
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
  
             ITableService service = DBHelper.CreateInstance<ITableService>(invokingDB.GetType().Name);
             service.DeleteTable( invokingDB , this.TableName);
        }

        public override string ToString()
        {
            return string.Format("Delete Table {0}" , this.TableName);
        }
    }
}