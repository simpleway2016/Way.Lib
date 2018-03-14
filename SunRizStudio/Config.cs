using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
namespace SunRizStudio
{
    class Config
    {
        public string ServerUrl { get; set; }
        public string LastUserName { get; set; }
        public Config()
        {
            if (System.IO.File.Exists(AppDomain.CurrentDomain.BaseDirectory + "config.json"))
            {
                try
                {
                    var jsonObj = (Newtonsoft.Json.Linq.JToken)Newtonsoft.Json.JsonConvert.DeserializeObject(System.IO.File.ReadAllText(AppDomain.CurrentDomain.BaseDirectory + "config.json" , System.Text.Encoding.UTF8));
                    var property = (Newtonsoft.Json.Linq.JProperty)jsonObj.First;
                    var typeInfo = this.GetType().GetTypeInfo();
                    while (property != null)
                    {
                        var field = typeInfo.GetProperty(property.Name);
                        if (field != null)
                        {
                            field.SetValue(this, Convert.ChangeType( property.Value.ToString() , field.PropertyType));
                        }
                        property = (Newtonsoft.Json.Linq.JProperty)property.Next;
                    }
                }
                catch
                {

                }
            }
        }

        public void Save()
        {
            var dict = new Dictionary<string, object>();
            var fields = this.GetType().GetTypeInfo().GetProperties();
            foreach( var f in fields )
            {
                dict[f.Name] = f.GetValue(this);
            }
            System.IO.File.WriteAllText(AppDomain.CurrentDomain.BaseDirectory + "config.json", Newtonsoft.Json.JsonConvert.SerializeObject(dict), System.Text.Encoding.UTF8);
        }
    }
}
