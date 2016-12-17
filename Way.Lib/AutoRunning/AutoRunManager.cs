using System;
using System.Collections.Generic;
using System.IO;
using System.IO.MemoryMappedFiles;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Way.Lib.AutoRunning
{
    /// <summary>
    /// 管理自动运行
    /// </summary>
    public class AutoRunManager
    {
        class RunningItem
        {
            /// <summary>
            /// 上一次运行的时间
            /// </summary>
            public DateTime? LastInvokeTime;
            public IRun Target;
            internal Type TargetType;
        }
        static List<RunningItem> Runnings = new List<RunningItem>();
        static AutoRunMgrStatus AutoRunMgrStatus = new AutoRunMgrStatus();
        /// <summary>
        /// 注册自动运行类，建议此函数在Global.Application_Start中执行
        /// </summary>
        /// <param name="target"></param>
        public static void RegisterAutoRun(IRun target)
        {
            Type targetType = target.GetType();
            if (Runnings.Any(m => m.TargetType == targetType) == false)
            {
                Runnings.Add(new RunningItem
                {
                    Target = target,
                    TargetType = targetType
                });
            }
        }

        static void itemRun(RunningItem item)
        {
            string myguid = AutoRunMgrStatus.CurrentGuid;

            int sleepTime = 10;
            if (item.Target.Timers != null && item.Target.Timers.Length > 0)
            {
                //如果是定点执行，可以让Thread.Sleep睡眠长一点
                sleepTime = 60000;
            }
            else
            {
                sleepTime = Math.Min(60000, item.Target.Interval / 2); 
            }
            try
            {
                //如果AutoRunMgrStatus.CurrentGuid != myguid，表示新的进程启动了，这里该退出了
                while (AutoRunMgrStatus.Status == MgrStatus.Running && AutoRunMgrStatus.CurrentGuid == myguid)
                {
                    try
                    {
                        bool toRun = false;
                        if (item.Target.Timers != null && item.Target.Timers.Length > 0)
                        {
                            //每天特定时间执行
                            foreach (double hour in item.Target.Timers)
                            {
                                int h = (int)hour;//过滤出哪个小时
                                int m = (int)((hour % 1) * 100);//过滤出分钟

                                //转换成当天的执行时间点
                                DateTime time = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd " + h + ":" + m + ":00"));
                                if (DateTime.Now >= time && item.LastInvokeTime.GetValueOrDefault() < time)
                                {
                                    //log一下，记录哪个自动运行类运行了
                                    using (CLog log = new CLog(item.Target + " 运行 "))
                                    {

                                    }
                                    toRun = true;
                                    break;
                                }
                            }
                        }
                        else
                        {
                            //间隔多少豪秒执行一次
                            var milliseconds = int.MaxValue;
                            if (item.LastInvokeTime != null)
                            {
                                milliseconds = (int)(DateTime.Now - item.LastInvokeTime.Value).TotalMilliseconds;
                            }
                            if (milliseconds >= item.Target.Interval)
                            {
                                toRun = true;
                            }
                            else
                            {
                                //例如，如果6000执行一次，现在已经过了5000毫秒了，那么再sleep(1000)就可以了
                                milliseconds = item.Target.Interval - milliseconds;
                                if (milliseconds > 0 && milliseconds < sleepTime)
                                {
                                    Thread.Sleep((int)milliseconds);
                                    continue;
                                }
                            }
                        }

                        if (toRun)
                        {
                            item.LastInvokeTime = DateTime.Now;
                            item.Target.Run();
                        }
                    }
#if NET46
                    catch (ThreadAbortException)
                    {
                        //线程被终止，应该是进程被关闭了
                        return;
                    }
#endif
                    catch (Exception ex)
                    {
                        using (CLog log = new CLog(item.Target + " error "))
                        {
                            log.Log(ex.ToString());
                        }

                    }
                    Thread.Sleep(sleepTime);
                }
            }
            catch
            {

            }
        }

        /// <summary>
        /// 开始运行，建议此方法在Global.Application_Start中调用
        /// </summary>
        public static void Start()
        {
            new Thread(() =>
            {
                //如果其他进程正在运行，先等它运行完毕，或者等待60秒继续
                if (AutoRunMgrStatus.Status == MgrStatus.Running)
                {
                    //告诉正在运行的进程，准备终止任务
                    AutoRunMgrStatus.Status = MgrStatus.ReadyToStop;
                    int flag = 0;
                    while (AutoRunMgrStatus.Status == MgrStatus.ReadyToStop && flag < 60)
                    {
                        Thread.Sleep(1000);
                        flag++;
                    }
                }

                AutoRunMgrStatus.Status = MgrStatus.Running;
                using (CLog log = new CLog("AutoRunManager Start "))
                {
                    try
                    {
                        Parallel.ForEach(Runnings, (item) =>
                        {
                            itemRun(item);
                        });
                    }
                    catch (Exception ex)
                    {
                        log.Log(ex.Message);
                    }
                    finally
                    {
                        if (AutoRunMgrStatus.Status == MgrStatus.ReadyToStop)
                        {
                            AutoRunMgrStatus.Status = MgrStatus.Stopped;
                        }
                        log.Log("Exited");
                    }
                }

            }).Start();

        }
    }
}
