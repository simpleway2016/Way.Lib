
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
/// 
	/// </summary>
public enum Databases_dbTypeEnum:int
{
    

/// <summary>
/// 
	/// </summary>
SqlServer = 1,

/// <summary>
/// 
	/// </summary>

Sqlite = 2,

/// <summary>
/// 
	/// </summary>

MySql=3,
}

    /// <summary>
	/// 数据库
	/// </summary>
    [Serializable]
    [EntityDB.Attributes.Table("id")]
    [System.Data.Linq.Mapping.TableAttribute(Name = @"Databases")]
    public class Databases :EntityDB.DataItem
    {

/// <summary>
	/// 
	/// </summary>
public  Databases()
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

System.Nullable<Int32> _ProjectID;
/// <summary>
/// 项目ID
	/// </summary>
[EntityDB.WayLinqColumnAttribute(Comment="",Caption="项目ID",Storage = "_ProjectID",DbType="int")]
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

Databases_dbTypeEnum _dbType=(Databases_dbTypeEnum)(1);
/// <summary>
/// 数据库类型
	/// </summary>
[EntityDB.WayLinqColumnAttribute(Comment="",Caption="数据库类型",Storage = "_dbType",DbType="int")]
        public Databases_dbTypeEnum dbType
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

String _conStr;
/// <summary>
/// 连接字符串
	/// </summary>
[EntityDB.WayLinqColumnAttribute(Comment="",Caption="连接字符串",Storage = "_conStr",DbType="varchar(200)")]
        public String conStr
        {
            get
            {
                return this._conStr;
            }
            set
            {
                if ((this._conStr != value))
                {
                    this.SendPropertyChanging("conStr",this._conStr,value);
                    this._conStr = value;
                    this.SendPropertyChanged("conStr");

                }
            }
        }

String _dllPath;
/// <summary>
/// dll生成文件夹
	/// </summary>
[EntityDB.WayLinqColumnAttribute(Comment="",Caption="dll生成文件夹",Storage = "_dllPath",DbType="varchar(100)")]
        public String dllPath
        {
            get
            {
                return this._dllPath;
            }
            set
            {
                if ((this._dllPath != value))
                {
                    this.SendPropertyChanging("dllPath",this._dllPath,value);
                    this._dllPath = value;
                    this.SendPropertyChanged("dllPath");

                }
            }
        }

System.Nullable<Int32> _iLock=0;
/// <summary>
/// iLock
	/// </summary>
[EntityDB.WayLinqColumnAttribute(Comment="",Caption="iLock",Storage = "_iLock",DbType="int")]
        public System.Nullable<Int32> iLock
        {
            get
            {
                return this._iLock;
            }
            set
            {
                if ((this._iLock != value))
                {
                    this.SendPropertyChanging("iLock",this._iLock,value);
                    this._iLock = value;
                    this.SendPropertyChanged("iLock");

                }
            }
        }

String _NameSpace;
/// <summary>
/// NameSpace
	/// </summary>
[EntityDB.WayLinqColumnAttribute(Comment="",Caption="NameSpace",Storage = "_NameSpace",DbType="varchar(50)")]
        public String NameSpace
        {
            get
            {
                return this._NameSpace;
            }
            set
            {
                if ((this._NameSpace != value))
                {
                    this.SendPropertyChanging("NameSpace",this._NameSpace,value);
                    this._NameSpace = value;
                    this.SendPropertyChanged("NameSpace");

                }
            }
        }

String _Guid;
/// <summary>
/// 唯一标示ID
	/// </summary>
[EntityDB.WayLinqColumnAttribute(Comment="",Caption="唯一标示ID",Storage = "_Guid",DbType="varchar(50)")]
        public String Guid
        {
            get
            {
                return this._Guid;
            }
            set
            {
                if ((this._Guid != value))
                {
                    this.SendPropertyChanging("Guid",this._Guid,value);
                    this._Guid = value;
                    this.SendPropertyChanged("Guid");

                }
            }
        }
}}