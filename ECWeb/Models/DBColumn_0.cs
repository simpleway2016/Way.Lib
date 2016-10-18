
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Linq.Mapping;
using System.Drawing;
using System.Linq;
using System.Text;
namespace EJ{

    /// <summary>
	/// 字段
	/// </summary>
    [Serializable]
    [EntityDB.Attributes.Table("id")]
    [System.Data.Linq.Mapping.TableAttribute(Name = @"DBColumn")]
    public class DBColumn :EntityDB.DataItem
    {

/// <summary>
	/// 
	/// </summary>
public  DBColumn()
        {
        }


System.Nullable<Int32> _id;
/// <summary>
/// id
	/// </summary>
[EntityDB.WayLinqColumnAttribute(Comment="",Caption="id",Storage = "_id",DbType="int" ,IsPrimaryKey=true,AutoSync=AutoSync.OnInsert,IsDbGenerated=true)]
        public System.Nullable<Int32> id
        {
            get
            {
                return this._id;
            }
            set
            {
                if ((this._id != value))
                {
                    this.SendPropertyChanging("id",this._id,value);
                    this._id = value;
                    this.SendPropertyChanged("id");

                }
            }
        }

String _caption;
/// <summary>
/// caption
	/// </summary>
[EntityDB.WayLinqColumnAttribute(Comment="",Caption="caption",Storage = "_caption",DbType="varchar(200)")]
        public String caption
        {
            get
            {
                return this._caption;
            }
            set
            {
                if ((this._caption != value))
                {
                    this.SendPropertyChanging("caption",this._caption,value);
                    this._caption = value;
                    this.SendPropertyChanged("caption");

                }
            }
        }

String _Name;
/// <summary>
/// Name
	/// </summary>
[EntityDB.WayLinqColumnAttribute(Comment="",Caption="Name",Storage = "_Name",DbType="varchar(50)")]
        public String Name
        {
            get
            {
                return this._Name;
            }
            set
            {
                if ((this._Name != value))
                {
                    this.SendPropertyChanging("Name",this._Name,value);
                    this._Name = value;
                    this.SendPropertyChanged("Name");

                }
            }
        }

System.Nullable<Boolean> _IsAutoIncrement=false;
/// <summary>
/// 自增长
	/// </summary>
[EntityDB.WayLinqColumnAttribute(Comment="",Caption="自增长",Storage = "_IsAutoIncrement",DbType="bit")]
        public System.Nullable<Boolean> IsAutoIncrement
        {
            get
            {
                return this._IsAutoIncrement;
            }
            set
            {
                if ((this._IsAutoIncrement != value))
                {
                    this.SendPropertyChanging("IsAutoIncrement",this._IsAutoIncrement,value);
                    this._IsAutoIncrement = value;
                    this.SendPropertyChanged("IsAutoIncrement");

                }
            }
        }

System.Nullable<Boolean> _CanNull=true;
/// <summary>
/// 可以为空
	/// </summary>
[EntityDB.WayLinqColumnAttribute(Comment="",Caption="可以为空",Storage = "_CanNull",DbType="bit")]
        public System.Nullable<Boolean> CanNull
        {
            get
            {
                return this._CanNull;
            }
            set
            {
                if ((this._CanNull != value))
                {
                    this.SendPropertyChanging("CanNull",this._CanNull,value);
                    this._CanNull = value;
                    this.SendPropertyChanged("CanNull");

                }
            }
        }

String _dbType;
/// <summary>
/// 数据库字段类型
	/// </summary>
[EntityDB.WayLinqColumnAttribute(Comment="",Caption="数据库字段类型",Storage = "_dbType",DbType="varchar(50)")]
        public String dbType
        {
            get
            {
                return this._dbType;
            }
            set
            {
                if ((this._dbType != value))
                {
                    this.SendPropertyChanging("dbType",this._dbType,value);
                    this._dbType = value;
                    this.SendPropertyChanged("dbType");

                }
            }
        }

String _Type;
/// <summary>
/// c#类型
	/// </summary>
[EntityDB.WayLinqColumnAttribute(Comment="",Caption="c#类型",Storage = "_Type",DbType="varchar(50)")]
        public String Type
        {
            get
            {
                return this._Type;
            }
            set
            {
                if ((this._Type != value))
                {
                    this.SendPropertyChanging("Type",this._Type,value);
                    this._Type = value;
                    this.SendPropertyChanged("Type");

                }
            }
        }

String _EnumDefine;
/// <summary>
/// Enum定义
	/// </summary>
[EntityDB.WayLinqColumnAttribute(Comment="",Caption="Enum定义",Storage = "_EnumDefine",DbType="varchar(300)")]
        public String EnumDefine
        {
            get
            {
                return this._EnumDefine;
            }
            set
            {
                if ((this._EnumDefine != value))
                {
                    this.SendPropertyChanging("EnumDefine",this._EnumDefine,value);
                    this._EnumDefine = value;
                    this.SendPropertyChanged("EnumDefine");

                }
            }
        }

String _length;
/// <summary>
/// 长度
	/// </summary>
[EntityDB.WayLinqColumnAttribute(Comment="",Caption="长度",Storage = "_length",DbType="varchar(50)")]
        public String length
        {
            get
            {
                return this._length;
            }
            set
            {
                if ((this._length != value))
                {
                    this.SendPropertyChanging("length",this._length,value);
                    this._length = value;
                    this.SendPropertyChanged("length");

                }
            }
        }

String _defaultValue;
/// <summary>
/// 默认值
	/// </summary>
[EntityDB.WayLinqColumnAttribute(Comment="",Caption="默认值",Storage = "_defaultValue",DbType="varchar(200)")]
        public String defaultValue
        {
            get
            {
                return this._defaultValue;
            }
            set
            {
                if ((this._defaultValue != value))
                {
                    this.SendPropertyChanging("defaultValue",this._defaultValue,value);
                    this._defaultValue = value;
                    this.SendPropertyChanged("defaultValue");

                }
            }
        }

System.Nullable<Int32> _TableID;
/// <summary>
/// TableID
	/// </summary>
[EntityDB.WayLinqColumnAttribute(Comment="",Caption="TableID",Storage = "_TableID",DbType="int")]
        public System.Nullable<Int32> TableID
        {
            get
            {
                return this._TableID;
            }
            set
            {
                if ((this._TableID != value))
                {
                    this.SendPropertyChanging("TableID",this._TableID,value);
                    this._TableID = value;
                    this.SendPropertyChanged("TableID");

                }
            }
        }

System.Nullable<Boolean> _IsPKID=false;
/// <summary>
/// 是否是主键
	/// </summary>
[EntityDB.WayLinqColumnAttribute(Comment="",Caption="是否是主键",Storage = "_IsPKID",DbType="bit")]
        public System.Nullable<Boolean> IsPKID
        {
            get
            {
                return this._IsPKID;
            }
            set
            {
                if ((this._IsPKID != value))
                {
                    this.SendPropertyChanging("IsPKID",this._IsPKID,value);
                    this._IsPKID = value;
                    this.SendPropertyChanged("IsPKID");

                }
            }
        }

System.Nullable<Int32> _orderid=0;
/// <summary>
/// orderid
	/// </summary>
[EntityDB.WayLinqColumnAttribute(Comment="",Caption="orderid",Storage = "_orderid",DbType="int")]
        public System.Nullable<Int32> orderid
        {
            get
            {
                return this._orderid;
            }
            set
            {
                if ((this._orderid != value))
                {
                    this.SendPropertyChanging("orderid",this._orderid,value);
                    this._orderid = value;
                    this.SendPropertyChanged("orderid");

                }
            }
        }
}}