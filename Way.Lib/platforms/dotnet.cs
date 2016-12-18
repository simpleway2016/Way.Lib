#if NET46
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Way.Lib
{
    public class PlatformHelper
    {
        public static Assembly[] GetAppAssemblies()
        {
            return AppDomain.CurrentDomain.GetAssemblies();
        }
        public static string GetAppDirectory()
        {
            if (System.Web.HttpRuntime.AppDomainAppPath != null)
            {
                return System.Web.HttpRuntime.AppDomainAppPath;
            }
            else
            {
                return AppDomain.CurrentDomain.BaseDirectory;
            }
        }
    }
}
#endif