using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace SunRizServer
{
    class Config
    {
        public static string ConnectionString;
        public static Way.EntityDB.DatabaseType DbType;
        static Config()
        {
            var path = Way.Lib.PlatformHelper.GetAppDirectory() + "config.xml";
            XmlDocument xmldoc = new XmlDocument();
            var stream = System.IO.File.OpenRead(path);
            xmldoc.Load(stream);
            var node = xmldoc.DocumentElement.SelectSingleNode("db");
            ConnectionString = node.Attributes["ConnectionString"].InnerText;
            DbType = Enum.Parse<Way.EntityDB.DatabaseType>(node.Attributes["Type"].InnerText);
            if(DbType == Way.EntityDB.DatabaseType.Sqlite)
            {
                ConnectionString = ConnectionString.Replace(".\\", Way.Lib.PlatformHelper.GetAppDirectory());
            }
        }
    }
}
