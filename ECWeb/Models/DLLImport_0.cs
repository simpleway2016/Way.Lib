
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
	/// 引入的dll
	/// </summary>
    [Serializable]
    [EntityDB.Attributes.Table("id")]
    [System.Data.Linq.Mapping.TableAttribute(Name = @"DLLImport")]
    public class DLLImport :EntityDB.DataItem
    {

/// <summary>
	/// 
	/// </summary>
public  DLLImport()
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

String _path;
/// <summary>
/// dll文件路径
	/// </summary>
[EntityDB.WayLinqColumnAttribute(Comment="",Caption="dll文件路径",Storage = "_path",DbType="varchar(200)")]
        public String path
        {
            get
            {
                return this._path;
            }
            set
            {
                if ((this._path != value))
                {
                    this.SendPropertyChanging("path",this._path,value);
                    this._path = value;
                    this.SendPropertyChanged("path");

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
}}