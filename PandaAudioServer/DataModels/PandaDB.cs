using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;
using System.Xml.XPath;
namespace PandaAudioServer
{
    public class PandaDB : SysDB.DB.PandaAudio
    {
        static string ConSTR;
        static Way.EntityDB.DatabaseType DBType;
        static PandaDB()
        {
            XDocument doc = XDocument.Load($"{Way.Lib.PlatformHelper.GetAppDirectory()}config.xml");
            var ele = doc.XPathSelectElement("//db");
            ConSTR = ele.Attribute("connectionString").Value;
            DBType = (Way.EntityDB.DatabaseType)Enum.Parse(typeof(Way.EntityDB.DatabaseType), ele.Attribute("type").Value);
        }
        public PandaDB():base(ConSTR, DBType)
        {

        }
    }
}
