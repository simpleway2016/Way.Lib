using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Way.EntityDB
{
    public interface IDataItem
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="columnName"></param>
        /// <param name="value"></param>
        void SetValue(string columnName, object value);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="columnName"></param>
        /// <returns></returns>
        object GetValue(string columnName);
    }
    [System.ComponentModel.DataAnnotations.Schema.NotMapped()]
    public class DataValueChangedItem
    {
        public object OriginalValue
        {
            get;
            set;
        }
    }
    public interface IDataValueChanged
    {
        DataValueChangedItemCollection ChangedProperties
        {
            get;
        }
    }

    public class DataValueChangedItemCollection : Dictionary<string, DataValueChangedItem>
    {
        public DataValueChangedItem this[string key]
        {
            get
            {
                if (base.ContainsKey(key))
                    return (DataValueChangedItem)base[key];
                else
                    return null;
            }
            set
            {
                if (base.ContainsKey(key) == false)
                {
                    base.Add(key , value);
                }
            }
        }

        /// <summary>
        /// 先清空自己，然后从source导入数据
        /// </summary>
        /// <param name="source">数据源</param>
        public void ImportData(DataValueChangedItemCollection source)
        {
            this.Clear();
            foreach( var item in source )
            {
                base[item.Key] = item.Value;
            }
        }
    }

    class DataItemConverter : JsonConverter
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
                DataItem dataitem = (DataItem)Activator.CreateInstance(objectType);
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
            DataItem dataitem = (DataItem)value;
            var properties = value.GetType().GetTypeInfo().GetProperties();
            writer.WriteStartObject();
            foreach( var p in properties )
            {
                if (p.GetCustomAttribute(typeof(NotMappedAttribute)) != null)
                    continue;
                object pvalue = p.GetValue(dataitem);
                if (pvalue == null)
                    continue;
                writer.WritePropertyName(p.Name);
                serializer.Serialize(writer, pvalue);

            }
            if(dataitem.ChangedProperties.Count > 0)
            {
                writer.WritePropertyName("ChangedProperties");
                serializer.Serialize(writer , dataitem.ChangedProperties);
            }
            if (dataitem.BackupChangedProperties.Count > 0)
            {
                writer.WritePropertyName("BackupChangedProperties");
                serializer.Serialize(writer, dataitem.BackupChangedProperties);
            }
            writer.WriteEndObject();
        }
    }


    [Newtonsoft.Json.JsonConverter(typeof(DataItemConverter))]
    public abstract class DataItem : IDataItem, INotifyPropertyChanging, INotifyPropertyChanged, IDataValueChanged
    {
        internal bool m_notSendPropertyChanged = false;
        public static DateTime getdate()
        {
            return DateTime.Now;
        }

        public virtual void SetValue(string columnName, object value)
        {
            PropertyInfo pinfo = this.GetType().GetTypeInfo().GetProperty(columnName, System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.IgnoreCase);
            if (pinfo == null)
                throw new Exception(this + "不存在属性" + columnName);
            if (value == null)
            {
                pinfo.SetValue(this, null, null);
                return;
            }
            Type itemType = pinfo.PropertyType;
            if (pinfo.PropertyType.GetTypeInfo().IsGenericType)
            {
                itemType = itemType.GetGenericArguments()[0];
            }
            if (itemType.GetTypeInfo().IsEnum)
            {
                if (value == null)
                    throw new Exception("Enum类型不能赋予null值");
                pinfo.SetValue(this, Enum.Parse(itemType, value.ToString()), null);
            }
            else
            {
                pinfo.SetValue(this, Convert.ChangeType(value, itemType), null);
            }
        }
        public static object GetValue(object obj, string columnName)
        {
            string[] columnNameArr = columnName.Split('.');
            object currentData = obj;

            for (int i = 0; i < columnNameArr.Length; i++)
            {
                System.Reflection.PropertyInfo pinfo = currentData.GetType().GetProperty(columnNameArr[i], System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.IgnoreCase);
                if (pinfo == null)
                {
                    throw new Exception(string.Format(currentData.GetType().FullName + "找不到属性{0}" , columnNameArr[i]));
                }
                if (i == columnNameArr.Length - 1)
                {
                    return pinfo.GetValue(currentData);
                }
                else
                {
                    currentData = pinfo.GetValue(currentData);
                }
            }
            return null;
        }
        public virtual object GetValue(string columnName)
        {
            return GetValue(this, columnName);
        }

        public event PropertyChangingEventHandler PropertyChanging;

        public event PropertyChangedEventHandler PropertyChanged;

        DataValueChangedItemCollection _ChangedProperties = new DataValueChangedItemCollection();
        /// <summary>
        /// 被修改的属性的记录
        /// </summary>
        [NotMapped()]
        public DataValueChangedItemCollection ChangedProperties
        {
            get { return _ChangedProperties ; }
        }

        Type _tableType;
        [NotMapped()]
        internal Type TableType
        {
            get
            {
                if (_tableType == null)
                {
                   _tableType = this.GetType();
                }
                return _tableType;
            }
        }

        string _KeyName;
        [NotMapped()]
        internal virtual string KeyName
        {
            get
            {
                if (_KeyName == null)
                {
                    Attributes.Table myTableAttr = this.TableType.GetTypeInfo().GetCustomAttribute(typeof(Attributes.Table)) as Attributes.Table;
                    if (myTableAttr == null)
                        throw new Exception(this.TableType.FullName + "没有定义 Attributes.Table");
                    _KeyName = myTableAttr.KeyName;
                }
                return _KeyName;
            }
        }

        string _tableName;
        [NotMapped()]
        internal virtual string TableName
        {
            get
            {
                if (_tableName == null)
                {
                   var myTableAttr = this.TableType.GetTypeInfo().GetCustomAttribute(typeof(System.ComponentModel.DataAnnotations.Schema.TableAttribute)) as System.ComponentModel.DataAnnotations.Schema.TableAttribute;
                    if (myTableAttr == null)
                        throw new Exception(this.TableType.FullName + "没有定义 Attributes.Table");
                    _tableName = myTableAttr.Name;
                }
                return _tableName;
            }
        }

        object _pkvalue;
             [NotMapped()]
        internal virtual object PKValue
        {
            get
            {
                if (_pkvalue == null)
                {
                    _pkvalue = this.GetValue(this.KeyName);
                }
                return _pkvalue;
            }
        }

        internal virtual FieldValue[] GetFieldValues(bool isInsert)
        {
            List<FieldValue> fields = new List<FieldValue>();
            if (isInsert)
            {
                var pinfos = this.GetType().GetProperties();

                foreach (var pinfo in pinfos)
                {
                    var columnDefine = pinfo.GetCustomAttribute(typeof(WayDBColumnAttribute)) as WayDBColumnAttribute;
                    if (columnDefine == null)
                        continue;

                    if (columnDefine.IsDbGenerated)
                        continue;
                    object value = pinfo.GetValue(this);
                    if (value == null)
                        continue;

                    var typeinfo = pinfo.PropertyType.GetTypeInfo();
                    if (typeinfo.IsEnum)
                        value = Convert.ToInt32(value);
                    else if(typeinfo.IsGenericType && typeinfo.GetGenericArguments()[0].GetTypeInfo().IsEnum)
                        value = Convert.ToInt32(value);


                    fields.Add(new FieldValue()
                        {
                            FieldName = columnDefine.Name,
                            Value = value,
                        });
                }
            }
            else
            {
                Type tableType = this.GetType();
                foreach (var changeItem in this.ChangedProperties)
                {
                    var pinfo = tableType.GetProperty(changeItem.Key);

                    var columnDefine = pinfo.GetCustomAttribute(typeof(WayDBColumnAttribute)) as WayDBColumnAttribute;
                    if (columnDefine == null)
                        continue;


                    object value = pinfo.GetValue(this);

                    var typeinfo = pinfo.PropertyType.GetTypeInfo();
                    if (typeinfo.IsEnum)
                        value = Convert.ToInt32(value);
                    else if (typeinfo.IsGenericType && typeinfo.GetGenericArguments()[0].GetTypeInfo().IsEnum)
                        value = Convert.ToInt32(value);

                    fields.Add(new FieldValue()
                    {
                        FieldName = columnDefine.Name,
                        Value = value,
                    });
                }
            }
            return fields.ToArray();
        }

        DataValueChangedItemCollection _BackupChangedProperties = new DataValueChangedItemCollection();
        /// <summary>
        /// 可以用来手动备份ChangedProperties
        /// </summary>
        [NotMapped()]
        public DataValueChangedItemCollection BackupChangedProperties
        {
            get { return _BackupChangedProperties; }
            set
            {
                _BackupChangedProperties = value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        protected virtual void SendPropertyChanging(String propertyName  , object originalValue,object nowvalue)
        {
            if (m_notSendPropertyChanged)
                return;

            DataValueChangedItem changeditem = this.ChangedProperties[propertyName];
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
                this.ChangedProperties[propertyName] = new DataValueChangedItem()
                {
                    OriginalValue = originalValue,
                };
            }
            if ((this.PropertyChanging != null))
            {
                this.PropertyChanging(this, emptyChangingEventArgs);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        protected virtual void SendPropertyChanged(String propertyName)
        {
            if ((this.PropertyChanged != null))
            {
                this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        private static PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(String.Empty);
      
        public object Clone()
        {
            Type myType = this.GetType();
            TypeInfo myTypeInfo = myType.GetTypeInfo();
            var newObj = (IDataItem)Activator.CreateInstance(myType);
            var values = this.GetFieldValues(true);
            foreach (var field in values)
            {
                newObj.SetValue(field.FieldName, field.Value);
            }
            ((DataItem)newObj).ChangedProperties.Clear();
            return newObj;
        }

    }

    class FieldValue
    {
        public string FieldName;
        public object Value;
    }
}
