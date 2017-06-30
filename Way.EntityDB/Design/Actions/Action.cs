using System;
using System.Collections.Generic;
using System.Linq;


namespace Way.EntityDB.Design.Actions
{
    public abstract class Action
    {
        public int ID { get; set; }
        public abstract void Invoke( EntityDB.IDatabaseService invokingDB);
        internal abstract void BeforeSave();
        public object Save( EJ.DB.easyjob db , int databaseid)
        {
            BeforeSave();

            try
            {
                db.Database.ExecSqlString("select id from __action limit 0,1");
            }
            catch
            {
                //没有__action
                db.Database.ExecSqlString(@"
create table __action (
    [id]            integer PRIMARY KEY autoincrement,
    [type]          varchar (100),
    [content]         text,
    [databaseid]      int 
)
");
            }

            var action = new EntityDB.CustomDataItem("__action" , "id" , null);
            action.SetValue("type", this.GetType().Name);
            action.SetValue("databaseid",databaseid);
            action.SetValue("content", this.ToJsonString());

            db.Insert(action);
            return action.GetValue("id");
        }
    }

}