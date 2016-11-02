
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
	/// Bug附带截图
	/// </summary>
    [Serializable]
    [EntityDB.Attributes.Table("id")]
    [System.Data.Linq.Mapping.TableAttribute(Name = @"BugImages")]
    public class BugImages :EntityDB.DataItem
    {

/// <summary>
	/// 
	/// </summary>
public  BugImages()
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

Byte[] _content;
/// <summary>
/// 
	/// </summary>
[EntityDB.WayLinqColumnAttribute(Comment="",Caption="",Storage = "_content",DbType="image")]
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

System.Nullable<Int32> _orderID;
/// <summary>
/// 排序
	/// </summary>
[EntityDB.WayLinqColumnAttribute(Comment="",Caption="排序",Storage = "_orderID",DbType="int")]
        public System.Nullable<Int32> orderID
        {
            get
            {
                return this._orderID;
            }
            set
            {
                if ((this._orderID != value))
                {
                    this.SendPropertyChanging("orderID",this._orderID,value);
                    this._orderID = value;
                    this.SendPropertyChanged("orderID");

                }
            }
        }
}}