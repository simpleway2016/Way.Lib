using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Reflection;
using System.Linq;

namespace Way.Lib
{
    public class DataModelChangedItem
    {
        public object OriginalValue
        {
            get;
            set;
        }
    }
    public class DataModelChangedItemCollection : Dictionary<string, DataModelChangedItem>
    {
        public DataModelChangedItem this[string key]
        {
            get
            {
                if (base.ContainsKey(key))
                    return (DataModelChangedItem)base[key];
                else
                    return null;
            }
            set
            {
                if (base.ContainsKey(key) == false)
                {
                    base.Add(key, value);
                }
            }
        }
    }

    class DataModelConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return true;
        }
        public override bool CanRead
        {
            get
            {
                return true;
            }
        }
        public override bool CanWrite
        {
            get
            {
                return true;
            }
        }
        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.StartObject)
            {
                DataModel dataitem = (DataModel)Activator.CreateInstance(objectType);
                dataitem.m_notSendPropertyChanged = true;
                //让serializer去读reader
                serializer.Populate(reader, dataitem);


                //这种是自己去读reader
                //string proName = null;
                //while (reader.TokenType != JsonToken.EndObject)
                //{
                //    if(reader.TokenType == JsonToken.PropertyName)
                //    {
                //        proName = reader.Value.ToString();
                //    }
                //    else if(reader.Value != null)
                //    {

                //    }
                //    reader.Read();
                //}
                dataitem.m_notSendPropertyChanged = false;
                return dataitem;
            }
            else
                return null;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            DataModel dataitem = (DataModel)value;
            var properties = value.GetType().GetTypeInfo().GetProperties();
            writer.WriteStartObject();
            foreach (var p in properties)
            {
                object pvalue = p.GetValue(dataitem);
                if (pvalue == null)
                    continue;
                writer.WritePropertyName(p.Name);
                serializer.Serialize(writer, pvalue);

            }
            if (dataitem.ChangedProperties.Count > 0)
            {
                writer.WritePropertyName("ChangedProperties");
                serializer.Serialize(writer, dataitem.ChangedProperties);
            }
            writer.WriteEndObject();
        }
    }

    /// <summary>
    /// 实现INotifyPropertyChanged的model类
    /// </summary>
    [Newtonsoft.Json.JsonConverter(typeof(DataModelConverter))]
    public class DataModel : INotifyPropertyChanged 
    {
        internal bool m_notSendPropertyChanged = false;
        public event PropertyChangedEventHandler PropertyChanged;

        DataModelChangedItemCollection _ChangedProperties = new DataModelChangedItemCollection();
        /// <summary>
        /// 被修改的属性的记录
        /// </summary>
        public DataModelChangedItemCollection ChangedProperties
        {
            get { return _ChangedProperties; }
        }

        protected virtual void OnPropertyChanged(string propertyName, object originalValue, object nowvalue)
        {
            if (m_notSendPropertyChanged)
                return;

            var changeditem = this.ChangedProperties[propertyName];
            if (changeditem != null)
            {
                if (changeditem.OriginalValue == null && nowvalue == null)
                {
                    ChangedProperties.Remove(propertyName);
                }
                else if (changeditem.OriginalValue != null && changeditem.OriginalValue.Equals(nowvalue))
                {
                    ChangedProperties.Remove(propertyName);
                }
            }
            else
            {
                this.ChangedProperties[propertyName] = new DataModelChangedItem()
                {
                    OriginalValue = originalValue,
                };
            }
            if ((this.PropertyChanged != null))
            {
                this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        /// <summary>
        /// 从source拷贝属性值
        /// </summary>
        /// <param name="source"></param>
        public void CopyValue(DataModel source)
        {
            var properties = source.GetType().GetProperties();
            foreach( var p in properties )
            {
                if(p.CanWrite)
                {
                   var value =  p.GetValue(source);
                    p.SetValue(this, value);
                }
            }
        }

        public T Clone<T>()
        {
            return Newtonsoft.Json.JsonConvert.DeserializeObject<T>( Newtonsoft.Json.JsonConvert.SerializeObject(this));
        }
    }
}
