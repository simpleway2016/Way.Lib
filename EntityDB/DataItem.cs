using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;

namespace EntityDB
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
    [Serializable]
    public class DataValueChangedItem
    {
        public string PropertyName
        {
            get;
            set;
        }
        public object OriginalValue
        {
            get;
            set;
        }
    }
    public interface IDataValueChanged
    {
        List<DataValueChangedItem> ChangedProperties
        {
            get;
        }
    }
    [Serializable]
    public abstract class DataItem : IDataItem, INotifyPropertyChanging, INotifyPropertyChanged, IDataValueChanged, ICloneable
    {
        //internal bool m_notSendPropertyChanged = false;
        public static DateTime getdate()
        {
            return DateTime.Now;
        }

        public virtual void SetValue(string columnName, object value)
        {
            System.Reflection.PropertyInfo pinfo = this.GetType().GetProperty(columnName, System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.IgnoreCase);
            if (pinfo == null)
                throw new Exception(this + "不存在属性" + columnName);
            if (value == null)
            {
                pinfo.SetValue(this, null, null);
                return;
            }
            Type itemType = pinfo.PropertyType;
            if (pinfo.PropertyType.IsGenericType)
            {
                itemType = itemType.GetGenericArguments()[0];
            }
            if (itemType.IsEnum)
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

        List<DataValueChangedItem> _ChangedProperties = new List<DataValueChangedItem>();
        /// <summary>
        /// 被修改的属性的记录
        /// </summary>
        [NotMapped()]
        public List<DataValueChangedItem> ChangedProperties
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

        string _PKIDField;
        [NotMapped()]
        internal virtual string PKIDField
        {
            get
            {
                if (_PKIDField == null)
                {
                    Attributes.Table myTableAttr = this.TableType.GetCustomAttributes(typeof(Attributes.Table), true)[0] as Attributes.Table;
                    if (myTableAttr == null)
                        throw new Exception(this.TableType.FullName + "没有定义 Attributes.Table");
                    _PKIDField = myTableAttr.IDField;
                }
                return _PKIDField;
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
                    System.Data.Linq.Mapping.TableAttribute tableAttr = this.TableType.GetCustomAttributes(typeof(System.Data.Linq.Mapping.TableAttribute), true)[0] as System.Data.Linq.Mapping.TableAttribute;
                    if (tableAttr == null)
                        throw new Exception(this.TableType.FullName + "没有定义System.Data.Linq.Mapping.TableAttribute");
                    _tableName = tableAttr.Name;
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
                    _pkvalue = this.GetValue(this.PKIDField);
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
                    var attrs = pinfo.GetCustomAttributes(typeof(WayLinqColumnAttribute), true);
                    if (attrs.Length == 0)
                        continue;

                    var columnDefine = attrs[0] as WayLinqColumnAttribute;
                    if (columnDefine != null && columnDefine.IsDbGenerated)
                        continue;
                    object value = pinfo.GetValue(this);
                    if (value == null)
                        continue;

                    fields.Add(new FieldValue()
                        {
                            FieldName = pinfo.Name,
                            Value = value,
                        });
                }
            }
            else
            {
                Type tableType = this.GetType();
                foreach (var changeItem in this.ChangedProperties)
                {
                    var pinfo = tableType.GetProperty(changeItem.PropertyName);

                    var attrs = pinfo.GetCustomAttributes(typeof(WayLinqColumnAttribute), true);
                    if (attrs.Length == 0)
                        continue;

                  
                    object value = pinfo.GetValue(this);
                    fields.Add(new FieldValue()
                    {
                        FieldName = pinfo.Name,
                        Value = value,
                    });
                }
            }
            return fields.ToArray();
        }
        
        List<DataValueChangedItem> _BackupChangedProperties = new List<DataValueChangedItem>();
        /// <summary>
        /// 可以用来手动备份ChangedProperties
        /// </summary>
        [NotMapped()]
        public List<DataValueChangedItem> BackupChangedProperties
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
            //if (m_notSendPropertyChanged)
            //{
            //    return;
            //}
            //else
            //{
            //    var threadid = System.Threading.Thread.CurrentThread.ManagedThreadId;

            //    MyReadingDataHandlerState item = null;
            //    while (true)
            //    {
            //        try
            //        {
            //            int count = MyReadingDataHandler.ReadingThreads.Count;
            //            for (int i = 0; i < count; i++)
            //            {
            //                MyReadingDataHandlerState areading = MyReadingDataHandler.ReadingThreads[i] as MyReadingDataHandlerState;
            //                if (areading != null && areading.ThreadID == threadid)
            //                {
            //                    item = areading;
            //                    break;
            //                }
            //            }
            //            break;
            //        }
            //        catch (System.ArgumentOutOfRangeException)
            //        {
            //            //索引超出范围,从头检查
            //            continue;
            //        }
            //    }
            //    if (item != null)
            //    {
            //        item.DataItems.Add(this);
            //        this.m_notSendPropertyChanged = true;
            //        return;
            //    }
            //}
                DataValueChangedItem changeditem =
                    _ChangedProperties.FirstOrDefault(m => m.PropertyName == propertyName);
                if (changeditem == null)
                {
                    changeditem = new DataValueChangedItem()
                    {
                        PropertyName = propertyName,
                        OriginalValue = originalValue,
                    };
                    _ChangedProperties.Add(changeditem);
                }
                else
                {
                    if (changeditem.OriginalValue == null && nowvalue == null)
                    {
                        _ChangedProperties.Remove(changeditem);
                    }
                    else if (changeditem.OriginalValue != null && changeditem.OriginalValue.Equals(nowvalue))
                    {
                        _ChangedProperties.Remove(changeditem);
                    }
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
        public object Deserialize(byte[] Bytes)
        {
            IFormatter formatter = new BinaryFormatter();
            MemoryStream memoryStream = new MemoryStream();
            memoryStream.Write(Bytes, 0, Bytes.Length);
            memoryStream.Position = 0L;
            object result = formatter.Deserialize(memoryStream);
            memoryStream.Close();
            memoryStream.Dispose();
            return result;
        }

        public byte[] Serialize(object obj)
        {
            IFormatter formatter = new BinaryFormatter();
            MemoryStream memoryStream = new MemoryStream();
            formatter.Serialize(memoryStream, obj);
            byte[] array = new byte[memoryStream.Length];
            memoryStream.Position = 0L;
            memoryStream.Read(array, 0, array.Length);
            memoryStream.Close();
            memoryStream.Dispose();
            return array;
        }
        public object Clone()
        {
            return this.Deserialize(this.Serialize(this));
        }

    }

    class FieldValue
    {
        public string FieldName;
        public object Value;
    }
}
