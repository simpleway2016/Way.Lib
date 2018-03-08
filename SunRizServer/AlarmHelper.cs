using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Threading.Tasks;

namespace SunRizServer
{
    class AlarmHelper
    {
        /// <summary>
        /// 添加报警
        /// </summary>
        public static void AddAlarm(Alarm alarm)
        {
            alarm.AlarmTime = DateTime.Now;
            Task.Run(()=> {
                try
                {
                    using (SysDB db = new SysDB())
                    {
                        if (db.Alarm.Any(m => m.IsConfirm == false && m.IsReset == false && m.Address == alarm.Address && m.Content == alarm.Content))
                        {
                            //有同样的报警，而且没有确认
                            return;
                        }
                        db.Insert(alarm);
                    }
                }
                catch(Exception ex)
                {
                    using (Way.Lib.CLog log = new Way.Lib.CLog("AddAlarm error"))
                    {
                        log.Log(ex.ToString());
                    }
                }
            });           
        }
    }
}
