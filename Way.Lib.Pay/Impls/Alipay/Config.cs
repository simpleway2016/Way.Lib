using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Xml;

namespace Way.Lib.Pay.Alipay
{
    class Config
    {
        public Config(string xml)
        {
            XmlDocument xmldoc = new XmlDocument();
            xmldoc.LoadXml(xml);
            object configObject;
            if( xmldoc.SelectSingleNode("//appId") != null )
            {
                //AppConfig;
                this.AppConfig = new AppConfig();
                configObject = this.AppConfig;
            }
            else
            {
                this.WebConfig = new WebConfig();
                configObject = this.WebConfig;
            }
            Type configType = configObject.GetType();
            foreach( XmlElement element in xmldoc.DocumentElement.ChildNodes )
            {
                FieldInfo field = configType.GetField(element.Name, System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.IgnoreCase);
                if (field == null)
                {
                    PropertyInfo property = configType.GetProperty(element.Name, System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.IgnoreCase);
                    if (property == null)
                        continue;
                    object value = Convert.ChangeType(element.InnerText, property.PropertyType);
                    property.SetValue(configObject, value);
                }
                else
                {
                    object value = Convert.ChangeType(element.InnerText, field.FieldType);
                    field.SetValue(configObject, value);
                }
            }
        }
        public AppConfig AppConfig;
        public WebConfig WebConfig;

       
    }
}
