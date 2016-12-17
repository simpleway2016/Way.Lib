#if NET46
#else
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Way.Lib
{
    class PlatformHelper
    {
       
        public static string GetAppDirectory()
        {
            return AppContext.BaseDirectory;
        }
    }
}
#endif