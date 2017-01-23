using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows.Forms;
using Way.Lib.VSIX.Extend.AppCodeBuilder;
using Way.Lib.VSIX.Extend.Services;

namespace Way.Lib.VSIX.Extend.Test
{
    class MyDB : EJ.DB.EasyJob
    {
        public MyDB() : base(null, EntityDB.DatabaseType.Sqlite)
        {
        }
    }
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            BuilderForm.AddService(new MyTypeDiscoverer());
            BuilderForm.AddService(new MyApplication());
            BuilderForm win = new BuilderForm();
            win.ShowDialog();
         
        }

  

        class MyTypeDiscoverer : ITypeDiscoverer
        {
            public Type[] GetTypes(Type basetype)
            {
                var types = Assembly.GetExecutingAssembly().GetTypes().
                    Where(m => m.IsSubclassOf(basetype)  ).
                    OrderBy(m => m.Name).ToArray();
                return types;
            }
        }

        class MyApplication : IApplication
        {
            public string GetTemplatePath()
            {
                return AppDomain.CurrentDomain.BaseDirectory + "CodeTemplates";
            }
        }
    }
}
