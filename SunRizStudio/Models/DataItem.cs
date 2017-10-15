using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SunRizStudio.Models
{
    public class DataValueChangedItem
    {
        public object OriginalValue
        {
            get;
            set;
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
                    base.Add(key, value);
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
            foreach (var item in source)
            {
                base[item.Key] = item.Value;
            }
        }
    }
    public abstract class DataItem : INotifyPropertyChanging, INotifyPropertyChanged
    {

        public event PropertyChangingEventHandler PropertyChanging;

        public event PropertyChangedEventHandler PropertyChanged;

        DataValueChangedItemCollection _ChangedProperties = new DataValueChangedItemCollection();
        /// <summary>
        /// 被修改的属性的记录
        /// </summary>
        public DataValueChangedItemCollection ChangedProperties
        {
            get { return _ChangedProperties; }
        }

        /// <summary>
        /// 
        /// </summary>
        protected virtual void SendPropertyChanging(String propertyName, object originalValue, object nowvalue)
        {

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

        public T Clone<T>()
        {
            var dataitem = this.ToJsonString().ToJsonObject<T>();
            ((dynamic)dataitem).ChangedProperties.Clear();
            return dataitem;
        }

    }
}
