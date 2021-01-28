using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace Way.Lib
{
    public class HardwareHelper
    {
        /// <summary>
        /// 获取cpuid
        /// </summary>
        /// <returns></returns>
        public static string GetCpuId()
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
                var info = Runner.Exec("dmidecode", "-t 4");
                var index = info.IndexOf("ID:");
                info = info.Substring(index);
                info = info.Substring(0,info.IndexOf("\n")).Replace("ID:","").Replace(" ","").Trim();
                return info;
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
               return Runner.Exec("wmic", "cpu get ProcessorId").Replace("ProcessorId","").Trim();
            }
            else
                throw new NotSupportedException();
        }
    }
}
