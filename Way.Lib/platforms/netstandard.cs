#if NET46
#else
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Loader;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyModel;

namespace Way.Lib
{
    public class PlatformHelper
    {
        public static Assembly[] GetAppAssemblies()
        {
            List<Assembly> assemblies = new List<Assembly>();
            foreach(var lib in DependencyContext.Default.CompileLibraries)
            {
                if (lib.Serviceable) continue;
                if (lib.Type == "package") continue;
                Assembly assembly = Assembly.Load(new AssemblyName(lib.Name));
                assemblies.Add(assembly);
            }
            return assemblies.ToArray();
        }
       
        public static string GetAppDirectory()
        {
            string directory = AppContext.BaseDirectory.Replace("\\" , "/");
            if(directory.EndsWith("/") == false)
             directory += "/";
            return directory;
        }
    }
}
#endif