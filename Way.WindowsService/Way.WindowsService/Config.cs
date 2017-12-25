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
        public static string ServiceName;
        public static string Application;
        public static string Args;
        static Config()
        {
            string appPath = System.IO.Path.GetDirectoryName( typeof(Config).Assembly.CodeBase.Replace("file:///", "")) + "\\";
            try
            {
               // System.IO.File.WriteAllText(@"J:\testservice\noerr.txt", appPath );
                XmlDocument xmldoc = new XmlDocument();
                xmldoc.Load(appPath + "config.xml");
                ServiceName = xmldoc.DocumentElement.Attributes["ServiceName"].InnerText.Replace(" " , "");
                Application = xmldoc.DocumentElement.Attributes["Application"].InnerText.Replace("{0}", AppDomain.CurrentDomain.BaseDirectory);
                Args = xmldoc.DocumentElement.Attributes["Args"].InnerText.Replace("{0}" , AppDomain.CurrentDomain.BaseDirectory);
                
            }
            catch(Exception ex)
            {
               // System.IO.File.WriteAllText(@"J:\testservice\err.txt", appPath + "\r\n" + ex.ToString());
            }
        }
    }
}
