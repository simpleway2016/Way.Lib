
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
public enum DBPower_PowerEnum:int
{
    

/// <summary>
/// 
	/// </summary>
只读 = 0,

/// <summary>
/// 
	/// </summary>

修改 = 1,
}

    /// <summary>
	/// 数据库权限
	/// </summary>
    [Serializable]
    [EntityDB.Attributes.Table("id")]
    [System.Data.Linq.Mapping.TableAttribute(Name = @"DBPower")]
    public class DBPower :EntityDB.DataItem
    {

/// <summary>
	/// 
	/// </summary>
public  DBPower()
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

System.Nullable<Int32> _UserID;
/// <summary>
/// 用户
	/// </summary>
[EntityDB.WayLinqColumnAttribute(Comment="",Caption="用户",Storage = "_UserID",DbType="int")]
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

DBPower_PowerEnum _Power=(DBPower_PowerEnum)int.MinValue;
/// <summary>
/// 权限
	/// </summary>
[EntityDB.WayLinqColumnAttribute(Comment="",Caption="权限",Storage = "_Power",DbType="int")]
        public DBPower_PowerEnum Power
        {
            get
            {
                return this._Power;
            }
            set
            {
                if ((this._Power != value))
                {
                    this.SendPropertyChanging("Power",this._Power,value);
                    this._Power = value;
                    this.SendPropertyChanged("Power");

                }
            }
        }

System.Nullable<Int32> _DatabaseID;
/// <summary>
/// 数据库ID
	/// </summary>
[EntityDB.WayLinqColumnAttribute(Comment="",Caption="数据库ID",Storage = "_DatabaseID",DbType="int")]
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
}}