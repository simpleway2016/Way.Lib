#if NET46
#else
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Way.EntityDB
{
    class PlatformHelper
    {
        //public static void setAssemblyResolve()
        //{
        //    AssemblyLoadContext.Default.Resolving += Default_Resolving;
        //}
        //static List<string> tryLoadedAssemblyNames = new List<string>();
        //private static System.Reflection.Assembly Default_Resolving(AssemblyLoadContext arg1, System.Reflection.AssemblyName args)
        //{
        //    string name = args.Name.Split(',')[0].Trim();
        //    //if (name.StartsWith("Microsoft.VisualStudio.Web.PageInspector.Tracing.resources"))
        //    //    return null;
        //    if (tryLoadedAssemblyNames.Contains(name))
        //    {
        //        throw new Exception($"无法加载程序集{name}");
        //    }
        //    tryLoadedAssemblyNames.Add(name);
        //    var assembly = Assembly.Load(new AssemblyName(name));
        //    return assembly;
        //}

        public static string GetAppDirectory()
        {
            return AppContext.BaseDirectory;
        }
    }
}
#endif