
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
	/// 唯一值索引
	/// </summary>
    [Serializable]
    [EntityDB.Attributes.Table("id")]
    [System.Data.Linq.Mapping.TableAttribute(Name = @"IDXIndex")]
    public class IDXIndex :EntityDB.DataItem
    {

/// <summary>
	/// 
	/// </summary>
public  IDXIndex()
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

String _Keys;
/// <summary>
/// Keys
	/// </summary>
[EntityDB.WayLinqColumnAttribute(Comment="",Caption="Keys",Storage = "_Keys",DbType="varchar(100)")]
        public String Keys
        {
            get
            {
                return this._Keys;
            }
            set
            {
                if ((this._Keys != value))
                {
                    this.SendPropertyChanging("Keys",this._Keys,value);
                    this._Keys = value;
                    this.SendPropertyChanged("Keys");

                }
            }
        }

System.Nullable<Boolean> _IsUnique=true;
/// <summary>
/// 是否唯一索引
	/// </summary>
[EntityDB.WayLinqColumnAttribute(Comment="",Caption="是否唯一索引",Storage = "_IsUnique",DbType="bit")]
        public System.Nullable<Boolean> IsUnique
        {
            get
            {
                return this._IsUnique;
            }
            set
            {
                if ((this._IsUnique != value))
                {
                    this.SendPropertyChanging("IsUnique",this._IsUnique,value);
                    this._IsUnique = value;
                    this.SendPropertyChanged("IsUnique");

                }
            }
        }

System.Nullable<Boolean> _IsClustered=false;
/// <summary>
/// 是否聚焦
	/// </summary>
[EntityDB.WayLinqColumnAttribute(Comment="",Caption="是否聚焦",Storage = "_IsClustered",DbType="bit")]
        public System.Nullable<Boolean> IsClustered
        {
            get
            {
                return this._IsClustered;
            }
            set
            {
                if ((this._IsClustered != value))
                {
                    this.SendPropertyChanging("IsClustered",this._IsClustered,value);
                    this._IsClustered = value;
                    this.SendPropertyChanged("IsClustered");

                }
            }
        }
}}