using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Way.EntityDB
{
    public class CustomDataItem : DataItem
    {
        List<FieldValue> _fields = new List<FieldValue>();
        string _pkid;
        internal override string KeyName
        {
            get
            {
                return _pkid;
            }
        }
        string _tableName;
        internal override string TableName
        {
            get
            {
                return _tableName;
            }
        }
        object _pkvalue;
        internal override object PKValue
        {
            get
            {
                return _pkvalue;
            }
        }
      
        /// <param name="tableName">表名</param>
        /// <param name="pkid">主键名称</param>
        /// <param name="value">值，如果是用于insert，可以为null</param>
        public CustomDataItem(string tableName, string pkid, object value)
        {
            _tableName = tableName;
            _pkid = pkid;
            _pkvalue = value;
        }
        /// <summary>
        /// 设置字段的值
        /// </summary>
        /// <param name="fieldName">字段名称</param>
        /// <param name="value">值</param>
        public override void SetValue(string fieldName, object value)
        {
            fieldName = fieldName.ToLower();
            var field = _fields.FirstOrDefault(m => m.FieldName == fieldName);
            if (field != null)
            {
                if (field.Value != value)
                {
                    SendPropertyChanging(fieldName, field.Value , value);
                    field.Value = value;
                 
                }
            }
            else
            {
                if (value != null)
                {
                    SendPropertyChanging(fieldName, null, value);
                    _fields.Add(new FieldValue()
                    {
                        FieldName = fieldName,
                        Value = value
                    });
                }
            }
        }
        public override object GetValue(string fieldName)
        {
            fieldName = fieldName.ToLower();
            var field = _fields.FirstOrDefault(m => m.FieldName == fieldName);
            if (field != null)
                return field.Value;
            else
            {
                return null;
            }
        }

        internal override FieldValue[] GetFieldValues(bool isInsert)
        {
            return _fields.ToArray();
        }
    }
}
