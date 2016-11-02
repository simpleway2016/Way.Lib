
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
	/// InterfaceModule权限设定表
	/// </summary>
    [Serializable]
    [EntityDB.Attributes.Table("id")]
    [System.Data.Linq.Mapping.TableAttribute(Name = @"InterfaceModulePower")]
    public class InterfaceModulePower :EntityDB.DataItem
    {

/// <summary>
	/// 
	/// </summary>
public  InterfaceModulePower()
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

System.Nullable<Int32> _UserID;
/// <summary>
/// 
	/// </summary>
[EntityDB.WayLinqColumnAttribute(Comment="",Caption="",Storage = "_UserID",DbType="int")]
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
}}