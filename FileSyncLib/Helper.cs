using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileSyncLib
{
    class Helper
    {
        public static void AddLog(  string content)
        {
            using (FileSyncDB db = new FileSyncDB())
            {
                var log = new db.Log()
                {
                    content = content,
                    dTime = DateTime.Now,
                };
                db.Insert(log);
            }
        }
        public static void AddLog(FileSyncDB db , string content)
        {
            var log = new db.Log()
            {
                content = content,
                dTime = DateTime.Now,
            };
            db.Insert(log);
        }
    }
}
