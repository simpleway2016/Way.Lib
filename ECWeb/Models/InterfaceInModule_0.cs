
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
    [Serializable]
    [EntityDB.Attributes.Table("id")]
    [System.Data.Linq.Mapping.TableAttribute(Name = @"InterfaceInModule")]
    public class InterfaceInModule :EntityDB.DataItem
    {

/// <summary>
	/// 
	/// </summary>
public  InterfaceInModule()
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

System.Nullable<Int32> _ModuleID;
/// <summary>
/// 
	/// </summary>
[EntityDB.WayLinqColumnAttribute(Comment="",Caption="",Storage = "_ModuleID",DbType="int")]
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
/// 
	/// </summary>
[EntityDB.WayLinqColumnAttribute(Comment="",Caption="",Storage = "_x",DbType="int")]
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
/// 
	/// </summary>
[EntityDB.WayLinqColumnAttribute(Comment="",Caption="",Storage = "_y",DbType="int")]
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

String _Type;
/// <summary>
/// 
	/// </summary>
[EntityDB.WayLinqColumnAttribute(Comment="",Caption="",Storage = "_Type",DbType="varchar(100)")]
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

String _JsonData;
/// <summary>
/// 
	/// </summary>
[EntityDB.WayLinqColumnAttribute(Comment="",Caption="",Storage = "_JsonData",DbType="text")]
        public String JsonData
        {
            get
            {
                return this._JsonData;
            }
            set
            {
                if ((this._JsonData != value))
                {
                    this.SendPropertyChanging("JsonData",this._JsonData,value);
                    this._JsonData = value;
                    this.SendPropertyChanged("JsonData");

                }
            }
        }

System.Nullable<Int32> _width;
/// <summary>
/// 
	/// </summary>
[EntityDB.WayLinqColumnAttribute(Comment="",Caption="",Storage = "_width",DbType="int")]
        public System.Nullable<Int32> width
        {
            get
            {
                return this._width;
            }
            set
            {
                if ((this._width != value))
                {
                    this.SendPropertyChanging("width",this._width,value);
                    this._width = value;
                    this.SendPropertyChanged("width");

                }
            }
        }

System.Nullable<Int32> _height;
/// <summary>
/// 
	/// </summary>
[EntityDB.WayLinqColumnAttribute(Comment="",Caption="",Storage = "_height",DbType="int")]
        public System.Nullable<Int32> height
        {
            get
            {
                return this._height;
            }
            set
            {
                if ((this._height != value))
                {
                    this.SendPropertyChanging("height",this._height,value);
                    this._height = value;
                    this.SendPropertyChanged("height");

                }
            }
        }
}}