using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Threading.Tasks;

namespace SunRizServer
{
    class SystemHelper
    {
        /// <summary>
        /// 添加系统日志
        /// </summary>
        /// <param name="userid"></param>
        /// <param name="content"></param>
        public static void AddSysLog(int userid,string content)
        {
            if (string.IsNullOrEmpty(SystemLog.LogDataPath))
                return;

            using (var db = new DB.SunRiz(SystemLog.LogDataPath, Way.EntityDB.DatabaseType.Sqlite))
            {
                db.Insert(new SysLog {
                    UserId = userid,
                    Content = content,
                    Time = DateTime.Now
                });
            }
        }

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
                        if (db.Alarm.Any(m => m.IsConfirm == false && m.IsReset == false && m.IsBack == false && m.Address == alarm.Address && m.Content == alarm.Content))
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

        /// <summary>
        /// 如果不符合报警条件，则自动返回
        /// </summary>
        /// <param name="pointId"></param>
        /// <param name="value"></param>
        public static void AutoBackAlarm( int pointId , double value)
        {
            Task.Run(() => {
                try
                {
                    using (SysDB db = new SysDB())
                    {
                        var arr = db.Alarm.Where(m => m.IsConfirm == false && m.IsBack == false && m.PointId == pointId && m.Expression != null).ToArray();
                        foreach( var alarm in arr )
                        {
                            var expression = string.Format(alarm.Expression, value);
                            var result = Convert.ToBoolean( db.Database.ExecSqlString($"select ({expression})"));
                            if(result == false)
                            {
                                //不符合报警条件，可以返回
                                alarm.IsBack = true;
                                db.Update(alarm);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    using (Way.Lib.CLog log = new Way.Lib.CLog("AutoBackAlarm error"))
                    {
                        log.Log(ex.ToString());
                    }
                }
            });
        }
    }
}
