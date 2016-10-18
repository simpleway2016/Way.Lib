
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
	/// 接口设计的目录结构
	/// </summary>
    [Serializable]
    [EntityDB.Attributes.Table("id")]
    [System.Data.Linq.Mapping.TableAttribute(Name = @"InterfaceModule")]
    public class InterfaceModule :EntityDB.DataItem
    {

/// <summary>
	/// 
	/// </summary>
public  InterfaceModule()
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

System.Nullable<Int32> _ProjectID;
/// <summary>
/// 
	/// </summary>
[EntityDB.WayLinqColumnAttribute(Comment="",Caption="",Storage = "_ProjectID",DbType="int")]
        public System.Nullable<Int32> ProjectID
        {
            get
            {
                return this._ProjectID;
            }
            set
            {
                if ((this._ProjectID != value))
                {
                    this.SendPropertyChanging("ProjectID",this._ProjectID,value);
                    this._ProjectID = value;
                    this.SendPropertyChanged("ProjectID");

                }
            }
        }

String _Name;
/// <summary>
/// 
	/// </summary>
[EntityDB.WayLinqColumnAttribute(Comment="",Caption="",Storage = "_Name",DbType="varchar(50)")]
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

System.Nullable<Int32> _ParentID=0;
/// <summary>
/// 
	/// </summary>
[EntityDB.WayLinqColumnAttribute(Comment="",Caption="",Storage = "_ParentID",DbType="int")]
        public System.Nullable<Int32> ParentID
        {
            get
            {
                return this._ParentID;
            }
            set
            {
                if ((this._ParentID != value))
                {
                    this.SendPropertyChanging("ParentID",this._ParentID,value);
                    this._ParentID = value;
                    this.SendPropertyChanged("ParentID");

                }
            }
        }

System.Nullable<Boolean> _IsFolder=false;
/// <summary>
/// 
	/// </summary>
[EntityDB.WayLinqColumnAttribute(Comment="",Caption="",Storage = "_IsFolder",DbType="bit")]
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

System.Nullable<Int32> _LockUserId;
/// <summary>
/// 已经被某人锁定
	/// </summary>
[EntityDB.WayLinqColumnAttribute(Comment="",Caption="已经被某人锁定",Storage = "_LockUserId",DbType="int")]
        public System.Nullable<Int32> LockUserId
        {
            get
            {
                return this._LockUserId;
            }
            set
            {
                if ((this._LockUserId != value))
                {
                    this.SendPropertyChanging("LockUserId",this._LockUserId,value);
                    this._LockUserId = value;
                    this.SendPropertyChanged("LockUserId");

                }
            }
        }
}}