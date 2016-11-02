
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
public enum User_RoleEnum:int
{
    

/// <summary>
/// 
	/// </summary>
开发人员 = 1,

/// <summary>
/// 
	/// </summary>

客户端测试人员 = 1<<1,

/// <summary>
/// 
	/// </summary>

数据库设计师 = 1<<2 | 开发人员,

/// <summary>
/// 
	/// </summary>

管理员 = 数据库设计师 | 1<<3,

/// <summary>
/// 
	/// </summary>

项目经理 = 1<<4 | 开发人员,
}

    /// <summary>
	/// 系统用户
	/// </summary>
    [Serializable]
    [EntityDB.Attributes.Table("id")]
    [System.Data.Linq.Mapping.TableAttribute(Name = @"User")]
    public class User :EntityDB.DataItem
    {

/// <summary>
	/// 
	/// </summary>
public  User()
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

User_RoleEnum _Role=(User_RoleEnum)int.MinValue;
/// <summary>
/// 角色
	/// </summary>
[EntityDB.WayLinqColumnAttribute(Comment="",Caption="角色",Storage = "_Role",DbType="int")]
        public User_RoleEnum Role
        {
            get
            {
                return this._Role;
            }
            set
            {
                if ((this._Role != value))
                {
                    this.SendPropertyChanging("Role",this._Role,value);
                    this._Role = value;
                    this.SendPropertyChanged("Role");

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

String _Password;
/// <summary>
/// Password
	/// </summary>
[EntityDB.WayLinqColumnAttribute(Comment="",Caption="Password",Storage = "_Password",DbType="varchar(50)")]
        public String Password
        {
            get
            {
                return this._Password;
            }
            set
            {
                if ((this._Password != value))
                {
                    this.SendPropertyChanging("Password",this._Password,value);
                    this._Password = value;
                    this.SendPropertyChanged("Password");

                }
            }
        }
}}