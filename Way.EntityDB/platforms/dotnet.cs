#if NET46
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Way.EntityDB
{
    class PlatformHelper
    {
        public static void setAssemblyResolve()
        {
           
            AppDomain.CurrentDomain.AssemblyResolve += CurrentDomain_AssemblyResolve;
        }
        static List<string> tryLoadedAssemblyNames = new List<string>();
        private static Assembly CurrentDomain_AssemblyResolve(object sender, ResolveEventArgs args)
        {
            string name = args.Name.Split(',')[0].Trim();
            //if (name.StartsWith("Microsoft.VisualStudio.Web.PageInspector.Tracing.resources"))
            //    return null;
            if (tryLoadedAssemblyNames.Contains(name))
            {
                throw new Exception($"无法加载程序集{name}");
            }
            tryLoadedAssemblyNames.Add(name);
            var assembly = Assembly.Load(name);
            return assembly;
        }

        public static string GetAppDirectory()
        {
            try
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
            catch
            {
                return AppDomain.CurrentDomain.BaseDirectory;
            }
        }
    }
}
#endif