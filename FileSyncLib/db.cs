using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileSyncLib
{
    class FileSyncDB : db.DB.FileSync
    {
        public static string DbFilePath;
        public FileSyncDB()
            : base("data source=\""+DbFilePath+"\"" , EntityDB.DatabaseType.Sqlite)
        {
        }
    }
}
