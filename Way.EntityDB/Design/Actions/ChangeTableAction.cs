
using System;
using System.Collections.Generic;
using System.Linq;
using Way.EntityDB.Design.Services;

namespace Way.EntityDB.Design.Actions
{
    public class ChangeTableAction : Action
    {
        public string OldTableName
        {
            get;
            set;
        }
         public string NewTableName
        {
            get;
            set;
        }
         public EJ.DBColumn[] otherColumns
         {
             get;
             set;
         }
         public EJ.DBColumn[] newColumns
        {
            get;
            set;
        }
         public EJ.DBColumn[] changedColumns
         {
             get;
             set;
         }
         public EJ.DBColumn[] deletedColumns
         {
             get;
             set;
         }
         public IndexInfo[] IDXConfigs
         {
             get;
             set;
         }

         public ChangeTableAction()
        {
        }
         public ChangeTableAction( string oldTableName, string newTableName, 
             EJ.DBColumn[] newColumns, EJ.DBColumn[] changedColumns,
             EJ.DBColumn[] deletedColumns, EJ.DBColumn[] othercolumns, IndexInfo[] idxConfigs)
        {
            this.OldTableName = oldTableName;
            this.NewTableName = newTableName;
            this.newColumns = newColumns;

            this.otherColumns = othercolumns;
            this.changedColumns = changedColumns;
          
            this.deletedColumns = deletedColumns;
          
            this.IDXConfigs = idxConfigs;


            if (changedColumns != null)
                foreach (var c in changedColumns)
                {
                    c.BackupChangedProperties.ImportData(c.ChangedProperties);
                    c.ChangedProperties.Clear();
                }

        }
         internal override void BeforeSave()
         {
             if (newColumns != null)
                 foreach (var c in newColumns)
                 {
                     c.ChangedProperties.Clear();
                 }

             if (changedColumns != null)
                 foreach (var c in changedColumns)
                 {
                     c.ChangedProperties.Clear();
                 }

             if (deletedColumns != null)
                 foreach (var c in deletedColumns)
                 {
                     c.ChangedProperties.Clear();
                 }
         }
         public override void Invoke(EntityDB.IDatabaseService invokingDB)
        {
            ITableDesignService service = DBHelper.CreateTableDesignService(invokingDB.DBContext.DatabaseType);
             service.ChangeTable(invokingDB, OldTableName, NewTableName, newColumns, changedColumns, deletedColumns,otherColumns , IDXConfigs);
        }

        public override string ToString()
        {
            return string.Format("Modify Table {0}" , this.OldTableName);
        }
    }
}