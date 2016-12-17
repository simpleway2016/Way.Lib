using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Way.Lib.AutoRunning
{
    /// <summary>
    /// 支持自动运行的接口
    /// </summary>
    public interface IRun
    {
        /// <summary>
        /// 指定每天在哪个时间点执行
        /// 如new float[] { 1.30 , 22 }，表示每天1:30的时候执行一次
        /// 和22点 的时候执行一次
        /// 如果是null，空数组，表示一直运行
        /// </summary>
        double[] Timers { get; }
        /// <summary>
        /// 间隔多少豪秒运行一次,Timers = null时生效
        /// </summary>
        int Interval { get; }

        /// <summary>
        /// 执行
        /// </summary>
        void Run();
    }
}
