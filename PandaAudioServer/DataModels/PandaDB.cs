using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;
using System.Xml.XPath;
namespace PandaAudioServer
{
    public class PandaDB : db.DB.PandaAudio
    {
        static string ConSTR;
        static PandaDB()
        {
            XDocument doc = XDocument.Load($"{Way.Lib.PlatformHelper.GetAppDirectory()}config.xml");
            ConSTR = doc.XPathSelectElement("//db").Attribute("connectionString").Value;
        }
        public PandaDB():base(ConSTR, Way.EntityDB.DatabaseType.PostgreSql)
        {

        }
    }
}
