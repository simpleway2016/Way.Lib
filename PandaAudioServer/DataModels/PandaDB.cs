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
            var path = $"{Way.Lib.PlatformHelper.GetAppDirectory()}config.xml";
            var stream = System.IO.File.OpenRead(path);
            XDocument doc = XDocument.Load(stream);
            stream.Dispose();
            var ele = doc.XPathSelectElement("//db");
            ConSTR = ele.Attribute("connectionString").Value.Replace("./" , AppDomain.CurrentDomain.BaseDirectory);
            DBType = (Way.EntityDB.DatabaseType)Enum.Parse(typeof(Way.EntityDB.DatabaseType), ele.Attribute("type").Value);
        }
        public PandaDB():base(ConSTR, DBType)
        {

        }
    }
}
