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
            string folder;
            try
            {
                if (System.Web.HttpRuntime.BinDirectory != null)
                {
                    folder = System.Web.HttpRuntime.BinDirectory;
                    if (folder.EndsWith("\\") || folder.EndsWith("/"))
                        folder = folder.Substring(0, folder.Length - 1);

                    folder = System.IO.Path.GetDirectoryName(folder).Replace("\\", "/");
                }
                else
                {
                    folder = AppDomain.CurrentDomain.BaseDirectory.Replace("\\", "/");
                }
            }
            catch
            {
                folder = AppDomain.CurrentDomain.BaseDirectory.Replace("\\", "/");
            }
            if (folder.EndsWith("/") == false)
                folder += "/";
            return folder;
        }
    }
}
#endif