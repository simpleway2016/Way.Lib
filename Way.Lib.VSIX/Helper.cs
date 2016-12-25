using EnvDTE;
using Microsoft.VisualStudio.Shell.Design;
using Microsoft.VisualStudio.Shell.Interop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Way.Lib.VSIX
{
    class Helper
    {
        public static List<Type> GetProjectTypes(Type baseType, IServiceProvider serviceProvider, Projects projects)
        {
            IVsSolution solution = (IVsSolution)serviceProvider.GetService(typeof(IVsSolution));
            DynamicTypeService typeResolver = (DynamicTypeService)serviceProvider.GetService(typeof(DynamicTypeService));

            List<Type> result = new List<Type>();
            for (int i = 1; i <= projects.Count; i++)
            {
                Project project = projects.Item(i);
                IVsHierarchy hierarchy = null;
                solution.GetProjectOfUniqueName(project.UniqueName, out hierarchy);

                //_typeResolutionService = typeResolver.GetTypeResolutionService(hierarchy);
                var typeDiscoveryService = typeResolver.GetTypeDiscoveryService(hierarchy);
                System.Collections.ICollection fined = typeDiscoveryService.GetTypes(baseType, true);
                foreach (Type t in fined)
                {
                    result.Add(t);
                }
            }
            return result;
        }
    }
}
