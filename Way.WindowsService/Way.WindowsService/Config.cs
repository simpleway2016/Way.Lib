using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Way.WindowsService
{
    class Config
    {
        public string ServiceName;
        public string Application;
        public string Args;
        public static Config Instance;
        static Config()
        {
            string appPath = System.IO.Path.GetDirectoryName( typeof(Config).Assembly.CodeBase.Replace("file:///", "")) + "\\";
            try
            {
                var jsonStr = System.IO.File.ReadAllText(appPath + "Way.WindowsService.Config.json");
                Instance = new System.Web.Script.Serialization.JavaScriptSerializer().Deserialize<Config>(jsonStr);

                Instance.ServiceName = Instance.ServiceName.Replace(" ", "");
                Instance.Application = Instance.Application.Replace("{0}", AppDomain.CurrentDomain.BaseDirectory);
                Instance.Args = Instance.Args.Replace("{0}", AppDomain.CurrentDomain.BaseDirectory);
            }
            catch(Exception ex)
            {
               // System.IO.File.WriteAllText(@"J:\testservice\err.txt", appPath + "\r\n" + ex.ToString());
            }
        }
    }
}
