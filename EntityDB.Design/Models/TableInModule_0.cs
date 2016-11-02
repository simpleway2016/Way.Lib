
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
	/// TableInModule
	/// </summary>
    [Serializable]
    [EntityDB.Attributes.Table("id")]
    [System.Data.Linq.Mapping.TableAttribute(Name = @"TableInModule")]
    public class TableInModule :EntityDB.DataItem
    {

/// <summary>
	/// 
	/// </summary>
public  TableInModule()
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

System.Nullable<Int32> _ModuleID;
/// <summary>
/// ModuleID
	/// </summary>
[EntityDB.WayLinqColumnAttribute(Comment="",Caption="ModuleID",Storage = "_ModuleID",DbType="int")]
        public System.Nullable<Int32> ModuleID
        {
            get
            {
                return this._ModuleID;
            }
            set
            {
                if ((this._ModuleID != value))
                {
                    this.SendPropertyChanging("ModuleID",this._ModuleID,value);
                    this._ModuleID = value;
                    this.SendPropertyChanged("ModuleID");

                }
            }
        }

System.Nullable<Int32> _x;
/// <summary>
/// x
	/// </summary>
[EntityDB.WayLinqColumnAttribute(Comment="",Caption="x",Storage = "_x",DbType="int")]
        public System.Nullable<Int32> x
        {
            get
            {
                return this._x;
            }
            set
            {
                if ((this._x != value))
                {
                    this.SendPropertyChanging("x",this._x,value);
                    this._x = value;
                    this.SendPropertyChanged("x");

                }
            }
        }

System.Nullable<Int32> _y;
/// <summary>
/// y
	/// </summary>
[EntityDB.WayLinqColumnAttribute(Comment="",Caption="y",Storage = "_y",DbType="int")]
        public System.Nullable<Int32> y
        {
            get
            {
                return this._y;
            }
            set
            {
                if ((this._y != value))
                {
                    this.SendPropertyChanging("y",this._y,value);
                    this._y = value;
                    this.SendPropertyChanged("y");

                }
            }
        }

String _flag;
/// <summary>
/// 临时变量
	/// </summary>
[EntityDB.WayLinqColumnAttribute(Comment="",Caption="临时变量",Storage = "_flag",DbType="varchar(50)")]
        public String flag
        {
            get
            {
                return this._flag;
            }
            set
            {
                if ((this._flag != value))
                {
                    this.SendPropertyChanging("flag",this._flag,value);
                    this._flag = value;
                    this.SendPropertyChanged("flag");

                }
            }
        }

String _flag2;
/// <summary>
/// flag2
	/// </summary>
[EntityDB.WayLinqColumnAttribute(Comment="",Caption="flag2",Storage = "_flag2",DbType="varchar(50)")]
        public String flag2
        {
            get
            {
                return this._flag2;
            }
            set
            {
                if ((this._flag2 != value))
                {
                    this.SendPropertyChanging("flag2",this._flag2,value);
                    this._flag2 = value;
                    this.SendPropertyChanged("flag2");

                }
            }
        }
}}