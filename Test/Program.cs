using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Test
{
    class Mydb : db.DB.FileSync
    {
        public Mydb() : base("data source=\"" + AppDomain.CurrentDomain.BaseDirectory + "filesync.db\"", EntityDB.DatabaseType.Sqlite)
        {
        }
    }
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            using (Mydb mydb = new Mydb())
            {
                var newData = new db.FileChanges();
                newData.changeTime = DateTime.Now;
                newData.oldPath = "a1";
                newData.path = "a2";
                newData.MissionID = 1;
                newData.type = db.FileChanges_typeEnum.New;
                mydb.Insert(newData);
            }
                Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }
    }
}
