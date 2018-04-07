using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace SunRizServer
{
    class SystemLog
    {
        static SunRizServer.DB.SunRiz logDB;
        public static string LogDataPath;
     
        public static void Init()
        {
            try
            {
                using (SysDB db = new SysDB())
                {
                    var sysSetting = db.SystemSetting.FirstOrDefault();
                    if (string.IsNullOrEmpty(sysSetting.LogPath))
                        return;
                    try
                    {
                        //目录不存在，创建目录
                        if (System.IO.Directory.Exists(sysSetting.LogPath) == false)
                        {
                            System.IO.Directory.CreateDirectory(sysSetting.LogPath);
                        }
                        LogDataPath = $"data source=\"{sysSetting.LogPath.Replace("\\", "/")}/log_data.db\"";
                        using (logDB = new DB.SunRiz(LogDataPath, Way.EntityDB.DatabaseType.Sqlite))
                        {

                        }
                    }
                    catch
                    {
                        return;
                    }                   
                }
            }
            catch (Exception ex)
            {
                using (Way.Lib.CLog log = new Way.Lib.CLog("SystemLog error "))
                {
                    log.Log(ex.ToString());
                }
            }
        }
    }

}
