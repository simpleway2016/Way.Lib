
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
	/// Bug处理历史记录
	/// </summary>
    [Serializable]
    [EntityDB.Attributes.Table("id")]
    [System.Data.Linq.Mapping.TableAttribute(Name = @"BugHandleHistory")]
    public class BugHandleHistory :EntityDB.DataItem
    {

/// <summary>
	/// 
	/// </summary>
public  BugHandleHistory()
        {
        }


System.Nullable<Int32> _id;
/// <summary>
/// 
	/// </summary>
[EntityDB.WayLinqColumnAttribute(Comment="",Caption="",Storage = "_id",DbType="int" ,IsPrimaryKey=true,AutoSync=AutoSync.OnInsert,IsDbGenerated=true,CanBeNull=false)]
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

System.Nullable<Int32> _BugID;
/// <summary>
/// 
	/// </summary>
[EntityDB.WayLinqColumnAttribute(Comment="",Caption="",Storage = "_BugID",DbType="int")]
        public System.Nullable<Int32> BugID
        {
            get
            {
                return this._BugID;
            }
            set
            {
                if ((this._BugID != value))
                {
                    this.SendPropertyChanging("BugID",this._BugID,value);
                    this._BugID = value;
                    this.SendPropertyChanged("BugID");

                }
            }
        }

System.Nullable<Int32> _UserID;
/// <summary>
/// 发标者ID
	/// </summary>
[EntityDB.WayLinqColumnAttribute(Comment="",Caption="发标者ID",Storage = "_UserID",DbType="int")]
        public System.Nullable<Int32> UserID
        {
            get
            {
                return this._UserID;
            }
            set
            {
                if ((this._UserID != value))
                {
                    this.SendPropertyChanging("UserID",this._UserID,value);
                    this._UserID = value;
                    this.SendPropertyChanged("UserID");

                }
            }
        }

Byte[] _content;
/// <summary>
/// 内容
	/// </summary>
[EntityDB.WayLinqColumnAttribute(Comment="",Caption="内容",Storage = "_content",DbType="image")]
        public Byte[] content
        {
            get
            {
                return this._content;
            }
            set
            {
                if ((this._content != value))
                {
                    this.SendPropertyChanging("content",this._content,value);
                    this._content = value;
                    this.SendPropertyChanged("content");

                }
            }
        }

System.Nullable<DateTime> _SendTime;
/// <summary>
/// 发表时间
	/// </summary>
[EntityDB.WayLinqColumnAttribute(Comment="",Caption="发表时间",Storage = "_SendTime",DbType="datetime")]
        public System.Nullable<DateTime> SendTime
        {
            get
            {
                return this._SendTime;
            }
            set
            {
                if ((this._SendTime != value))
                {
                    this.SendPropertyChanging("SendTime",this._SendTime,value);
                    this._SendTime = value;
                    this.SendPropertyChanged("SendTime");

                }
            }
        }
}}