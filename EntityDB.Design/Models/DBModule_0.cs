
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
	/// 数据模块
	/// </summary>
    [Serializable]
    [EntityDB.Attributes.Table("id")]
    [System.Data.Linq.Mapping.TableAttribute(Name = @"DBModule")]
    public class DBModule :EntityDB.DataItem
    {

/// <summary>
	/// 
	/// </summary>
public  DBModule()
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

System.Nullable<Int32> _DatabaseID;
/// <summary>
/// DatabaseID
	/// </summary>
[EntityDB.WayLinqColumnAttribute(Comment="",Caption="DatabaseID",Storage = "_DatabaseID",DbType="int")]
        public System.Nullable<Int32> DatabaseID
        {
            get
            {
                return this._DatabaseID;
            }
            set
            {
                if ((this._DatabaseID != value))
                {
                    this.SendPropertyChanging("DatabaseID",this._DatabaseID,value);
                    this._DatabaseID = value;
                    this.SendPropertyChanged("DatabaseID");

                }
            }
        }

System.Nullable<Boolean> _IsFolder=false;
/// <summary>
/// IsFolder
	/// </summary>
[EntityDB.WayLinqColumnAttribute(Comment="",Caption="IsFolder",Storage = "_IsFolder",DbType="bit")]
        public System.Nullable<Boolean> IsFolder
        {
            get
            {
                return this._IsFolder;
            }
            set
            {
                if ((this._IsFolder != value))
                {
                    this.SendPropertyChanging("IsFolder",this._IsFolder,value);
                    this._IsFolder = value;
                    this.SendPropertyChanged("IsFolder");

                }
            }
        }

System.Nullable<Int32> _parentID;
/// <summary>
/// parentID
	/// </summary>
[EntityDB.WayLinqColumnAttribute(Comment="",Caption="parentID",Storage = "_parentID",DbType="int")]
        public System.Nullable<Int32> parentID
        {
            get
            {
                return this._parentID;
            }
            set
            {
                if ((this._parentID != value))
                {
                    this.SendPropertyChanging("parentID",this._parentID,value);
                    this._parentID = value;
                    this.SendPropertyChanged("parentID");

                }
            }
        }
}}