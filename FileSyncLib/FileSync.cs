using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileSyncLib
{
    public class FileSync
    {
        public delegate void OnChangeHandler( string fullpath,string type,string oldpath);
        public static event OnChangeHandler Changed;

        public static void OnChanged(string fullpath, string type, string oldpath)
        {
            if (Changed != null)
            {
                Changed(fullpath, type, oldpath);
            }
        }

        List<Mission> Missions = new List<Mission>();
        public void Start(string dbfilepath)
        {
            FileSyncDB.DbFilePath = dbfilepath;
            using (FileSyncDB db = new FileSyncDB())
            {
                var missonDatas = db.Mission.ToArray();
                foreach (var item in missonDatas)
                {
                    Missions.Add(new Mission(item));
                }
            }

            foreach (var mission in Missions)
                mission.Start();
        }
    }
}
