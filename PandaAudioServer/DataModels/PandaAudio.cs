
using System;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;

namespace SysDB{

/// <summary>
/// 
/// </summary>
public enum UserInfo_RoleEnum:int
{
    

/// <summary>
/// 普通用户
/// </summary>
Normal=0,

/// <summary>
/// 允许远程控制
/// </summary>
AllowRemote=1<<0,

/// <summary>
/// CY22用户
/// </summary>
CY22=1<<1 | AllowRemote,
}


    /// <summary>
	/// 用户信息
	/// </summary>
    [System.ComponentModel.DataAnnotations.Schema.Table("userinfo")]
    [Way.EntityDB.Attributes.Table("id")]
    public class UserInfo :Way.EntityDB.DataItem
    {

        /// <summary>
	    /// 
	    /// </summary>
        public  UserInfo()
        {
        }


        System.Nullable<Int32> _id;
        /// <summary>
        /// 
        /// </summary>
[System.ComponentModel.DataAnnotations.Key]
        [System.ComponentModel.DataAnnotations.Schema.Column("id")]
        [Way.EntityDB.WayDBColumnAttribute(Name="id",Comment="",Caption="",Storage = "_id",DbType="int" ,IsPrimaryKey=true,IsDbGenerated=true,CanBeNull=false)]
        public virtual System.Nullable<Int32> id
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

        String _RegGuid;
        /// <summary>
        /// 注册唯一认证码
        /// </summary>
        [System.ComponentModel.DataAnnotations.Schema.Column("regguid")]
        [Way.EntityDB.WayDBColumnAttribute(Name="regguid",Comment="",Caption="注册唯一认证码",Storage = "_RegGuid",DbType="varchar(50)")]
        public virtual String RegGuid
        {
            get
            {
                return this._RegGuid;
            }
            set
            {
                if ((this._RegGuid != value))
                {
                    this.SendPropertyChanging("RegGuid",this._RegGuid,value);
                    this._RegGuid = value;
                    this.SendPropertyChanged("RegGuid");

                }
            }
        }

        String _UserName;
        /// <summary>
        /// 
        /// </summary>
        [System.ComponentModel.DataAnnotations.Schema.Column("username")]
        [Way.EntityDB.WayDBColumnAttribute(Name="username",Comment="",Caption="",Storage = "_UserName",DbType="varchar(50)")]
        public virtual String UserName
        {
            get
            {
                return this._UserName;
            }
            set
            {
                if ((this._UserName != value))
                {
                    this.SendPropertyChanging("UserName",this._UserName,value);
                    this._UserName = value;
                    this.SendPropertyChanged("UserName");

                }
            }
        }

        String _Password;
        /// <summary>
        /// 
        /// </summary>
        [System.ComponentModel.DataAnnotations.Schema.Column("password")]
        [Way.EntityDB.WayDBColumnAttribute(Name="password",Comment="",Caption="",Storage = "_Password",DbType="varchar(100)")]
        public virtual String Password
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

        String _PhoneNumber;
        /// <summary>
        /// 手机号
        /// </summary>
        [System.ComponentModel.DataAnnotations.Schema.Column("phonenumber")]
        [Way.EntityDB.WayDBColumnAttribute(Name="phonenumber",Comment="",Caption="手机号",Storage = "_PhoneNumber",DbType="varchar(50)")]
        public virtual String PhoneNumber
        {
            get
            {
                return this._PhoneNumber;
            }
            set
            {
                if ((this._PhoneNumber != value))
                {
                    this.SendPropertyChanging("PhoneNumber",this._PhoneNumber,value);
                    this._PhoneNumber = value;
                    this.SendPropertyChanged("PhoneNumber");

                }
            }
        }

        String _Email;
        /// <summary>
        /// 
        /// </summary>
        [System.ComponentModel.DataAnnotations.Schema.Column("email")]
        [Way.EntityDB.WayDBColumnAttribute(Name="email",Comment="",Caption="",Storage = "_Email",DbType="varchar(50)")]
        public virtual String Email
        {
            get
            {
                return this._Email;
            }
            set
            {
                if ((this._Email != value))
                {
                    this.SendPropertyChanging("Email",this._Email,value);
                    this._Email = value;
                    this.SendPropertyChanged("Email");

                }
            }
        }

        String _Platform;
        /// <summary>
        /// 平台
        /// </summary>
        [System.ComponentModel.DataAnnotations.Schema.Column("platform")]
        [Way.EntityDB.WayDBColumnAttribute(Name="platform",Comment="",Caption="平台",Storage = "_Platform",DbType="varchar(50)")]
        public virtual String Platform
        {
            get
            {
                return this._Platform;
            }
            set
            {
                if ((this._Platform != value))
                {
                    this.SendPropertyChanging("Platform",this._Platform,value);
                    this._Platform = value;
                    this.SendPropertyChanged("Platform");

                }
            }
        }

        System.Nullable<Boolean> _IsPay=false;
        /// <summary>
        /// 是否已经支付给Jack
        /// </summary>
        [System.ComponentModel.DataAnnotations.Schema.Column("ispay")]
        [Way.EntityDB.WayDBColumnAttribute(Name="ispay",Comment="",Caption="是否已经支付给Jack",Storage = "_IsPay",DbType="bit")]
        public virtual System.Nullable<Boolean> IsPay
        {
            get
            {
                return this._IsPay;
            }
            set
            {
                if ((this._IsPay != value))
                {
                    this.SendPropertyChanging("IsPay",this._IsPay,value);
                    this._IsPay = value;
                    this.SendPropertyChanged("IsPay");

                }
            }
        }

        String _LoginCode;
        /// <summary>
        /// 登陆后生成的唯一码
        /// </summary>
        [System.ComponentModel.DataAnnotations.Schema.Column("logincode")]
        [Way.EntityDB.WayDBColumnAttribute(Name="logincode",Comment="",Caption="登陆后生成的唯一码",Storage = "_LoginCode",DbType="varchar(50)")]
        public virtual String LoginCode
        {
            get
            {
                return this._LoginCode;
            }
            set
            {
                if ((this._LoginCode != value))
                {
                    this.SendPropertyChanging("LoginCode",this._LoginCode,value);
                    this._LoginCode = value;
                    this.SendPropertyChanged("LoginCode");

                }
            }
        }

        System.Nullable<DateTime> _RegTime;
        /// <summary>
        /// 注册时间
        /// </summary>
        [System.ComponentModel.DataAnnotations.Schema.Column("regtime")]
        [Way.EntityDB.WayDBColumnAttribute(Name="regtime",Comment="",Caption="注册时间",Storage = "_RegTime",DbType="datetime")]
        public virtual System.Nullable<DateTime> RegTime
        {
            get
            {
                return this._RegTime;
            }
            set
            {
                if ((this._RegTime != value))
                {
                    this.SendPropertyChanging("RegTime",this._RegTime,value);
                    this._RegTime = value;
                    this.SendPropertyChanged("RegTime");

                }
            }
        }

        System.Nullable<DateTime> _LastCheckTime;
        /// <summary>
        /// 最后一次调用CheckUser时间
        /// </summary>
        [System.ComponentModel.DataAnnotations.Schema.Column("lastchecktime")]
        [Way.EntityDB.WayDBColumnAttribute(Name="lastchecktime",Comment="",Caption="最后一次调用CheckUser时间",Storage = "_LastCheckTime",DbType="datetime")]
        public virtual System.Nullable<DateTime> LastCheckTime
        {
            get
            {
                return this._LastCheckTime;
            }
            set
            {
                if ((this._LastCheckTime != value))
                {
                    this.SendPropertyChanging("LastCheckTime",this._LastCheckTime,value);
                    this._LastCheckTime = value;
                    this.SendPropertyChanged("LastCheckTime");

                }
            }
        }

        System.Nullable<Boolean> _Disable=false;
        /// <summary>
        /// 
        /// </summary>
        [System.ComponentModel.DataAnnotations.Schema.Column("disable")]
        [Way.EntityDB.WayDBColumnAttribute(Name="disable",Comment="",Caption="",Storage = "_Disable",DbType="bit")]
        public virtual System.Nullable<Boolean> Disable
        {
            get
            {
                return this._Disable;
            }
            set
            {
                if ((this._Disable != value))
                {
                    this.SendPropertyChanging("Disable",this._Disable,value);
                    this._Disable = value;
                    this.SendPropertyChanged("Disable");

                }
            }
        }

        System.Nullable<UserInfo_RoleEnum> _Role=(System.Nullable<UserInfo_RoleEnum>)(0);
        /// <summary>
        /// 用户角色
        /// </summary>
        [System.ComponentModel.DataAnnotations.Schema.Column("role")]
        [Way.EntityDB.WayDBColumnAttribute(Name="role",Comment="",Caption="用户角色",Storage = "_Role",DbType="int")]
        public virtual System.Nullable<UserInfo_RoleEnum> Role
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
}}
namespace SysDB{

/// <summary>
/// 
/// </summary>
public enum SystemRegCode_RoleEnum:int
{
    

/// <summary>
/// 普通用户
/// </summary>
Normal=0,

/// <summary>
/// 允许远程控制
/// </summary>
AllowRemote=1<<0,

/// <summary>
/// CY22用户
/// </summary>
CY22=1<<1 | AllowRemote,
}


    /// <summary>
	/// 系统注册码
	/// </summary>
    [System.ComponentModel.DataAnnotations.Schema.Table("systemregcode")]
    [Way.EntityDB.Attributes.Table("id")]
    public class SystemRegCode :Way.EntityDB.DataItem
    {

        /// <summary>
	    /// 
	    /// </summary>
        public  SystemRegCode()
        {
        }


        System.Nullable<Int32> _id;
        /// <summary>
        /// 
        /// </summary>
[System.ComponentModel.DataAnnotations.Key]
        [System.ComponentModel.DataAnnotations.Schema.Column("id")]
        [Way.EntityDB.WayDBColumnAttribute(Name="id",Comment="",Caption="",Storage = "_id",DbType="int" ,IsPrimaryKey=true,IsDbGenerated=true,CanBeNull=false)]
        public virtual System.Nullable<Int32> id
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

        String _RegGuid;
        /// <summary>
        /// 唯一认证码
        /// </summary>
        [System.ComponentModel.DataAnnotations.Schema.Column("regguid")]
        [Way.EntityDB.WayDBColumnAttribute(Name="regguid",Comment="",Caption="唯一认证码",Storage = "_RegGuid",DbType="varchar(50)")]
        public virtual String RegGuid
        {
            get
            {
                return this._RegGuid;
            }
            set
            {
                if ((this._RegGuid != value))
                {
                    this.SendPropertyChanging("RegGuid",this._RegGuid,value);
                    this._RegGuid = value;
                    this.SendPropertyChanged("RegGuid");

                }
            }
        }

        System.Nullable<Int32> _UserId;
        /// <summary>
        /// 使用的用户id
        /// </summary>
        [System.ComponentModel.DataAnnotations.Schema.Column("userid")]
        [Way.EntityDB.WayDBColumnAttribute(Name="userid",Comment="",Caption="使用的用户id",Storage = "_UserId",DbType="int")]
        public virtual System.Nullable<Int32> UserId
        {
            get
            {
                return this._UserId;
            }
            set
            {
                if ((this._UserId != value))
                {
                    this.SendPropertyChanging("UserId",this._UserId,value);
                    this._UserId = value;
                    this.SendPropertyChanged("UserId");

                }
            }
        }

        System.Nullable<Int32> _MakerUserId;
        /// <summary>
        /// 谁生成的
        /// </summary>
        [System.ComponentModel.DataAnnotations.Schema.Column("makeruserid")]
        [Way.EntityDB.WayDBColumnAttribute(Name="makeruserid",Comment="",Caption="谁生成的",Storage = "_MakerUserId",DbType="int")]
        public virtual System.Nullable<Int32> MakerUserId
        {
            get
            {
                return this._MakerUserId;
            }
            set
            {
                if ((this._MakerUserId != value))
                {
                    this.SendPropertyChanging("MakerUserId",this._MakerUserId,value);
                    this._MakerUserId = value;
                    this.SendPropertyChanged("MakerUserId");

                }
            }
        }

        System.Nullable<DateTime> _MakeTime;
        /// <summary>
        /// 生成时间
        /// </summary>
        [System.ComponentModel.DataAnnotations.Schema.Column("maketime")]
        [Way.EntityDB.WayDBColumnAttribute(Name="maketime",Comment="",Caption="生成时间",Storage = "_MakeTime",DbType="datetime")]
        public virtual System.Nullable<DateTime> MakeTime
        {
            get
            {
                return this._MakeTime;
            }
            set
            {
                if ((this._MakeTime != value))
                {
                    this.SendPropertyChanging("MakeTime",this._MakeTime,value);
                    this._MakeTime = value;
                    this.SendPropertyChanged("MakeTime");

                }
            }
        }

        System.Nullable<SystemRegCode_RoleEnum> _Role=(System.Nullable<SystemRegCode_RoleEnum>)(0);
        /// <summary>
        /// 用户角色
        /// </summary>
        [System.ComponentModel.DataAnnotations.Schema.Column("role")]
        [Way.EntityDB.WayDBColumnAttribute(Name="role",Comment="",Caption="用户角色",Storage = "_Role",DbType="int")]
        public virtual System.Nullable<SystemRegCode_RoleEnum> Role
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
}}
namespace SysDB{

/// <summary>
/// 
/// </summary>
public enum UserEffect_TypeEnum:int
{
    

/// <summary>
/// </summary>
Project=1,

/// <summary>
/// </summary>
Track=2,

/// <summary>
/// </summary>
Vst=3,

/// <summary>
/// </summary>
AdminSetting=4
}


    /// <summary>
	/// 用户的效果存档
	/// </summary>
    [System.ComponentModel.DataAnnotations.Schema.Table("usereffect")]
    [Way.EntityDB.Attributes.Table("id")]
    public class UserEffect :Way.EntityDB.DataItem
    {

        /// <summary>
	    /// 
	    /// </summary>
        public  UserEffect()
        {
        }


        System.Nullable<Int32> _id;
        /// <summary>
        /// 
        /// </summary>
[System.ComponentModel.DataAnnotations.Key]
        [System.ComponentModel.DataAnnotations.Schema.Column("id")]
        [Way.EntityDB.WayDBColumnAttribute(Name="id",Comment="",Caption="",Storage = "_id",DbType="int" ,IsPrimaryKey=true,IsDbGenerated=true,CanBeNull=false)]
        public virtual System.Nullable<Int32> id
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

        System.Nullable<Int32> _UserId;
        /// <summary>
        /// 
        /// </summary>
        [System.ComponentModel.DataAnnotations.Schema.Column("userid")]
        [Way.EntityDB.WayDBColumnAttribute(Name="userid",Comment="",Caption="",Storage = "_UserId",DbType="int")]
        public virtual System.Nullable<Int32> UserId
        {
            get
            {
                return this._UserId;
            }
            set
            {
                if ((this._UserId != value))
                {
                    this.SendPropertyChanging("UserId",this._UserId,value);
                    this._UserId = value;
                    this.SendPropertyChanged("UserId");

                }
            }
        }

        String _FileName;
        /// <summary>
        /// 
        /// </summary>
        [System.ComponentModel.DataAnnotations.Schema.Column("filename")]
        [Way.EntityDB.WayDBColumnAttribute(Name="filename",Comment="",Caption="",Storage = "_FileName",DbType="varchar(50)")]
        public virtual String FileName
        {
            get
            {
                return this._FileName;
            }
            set
            {
                if ((this._FileName != value))
                {
                    this.SendPropertyChanging("FileName",this._FileName,value);
                    this._FileName = value;
                    this.SendPropertyChanged("FileName");

                }
            }
        }

        System.Nullable<UserEffect_TypeEnum> _Type=(System.Nullable<UserEffect_TypeEnum>)(1);
        /// <summary>
        /// 类型
        /// </summary>
        [System.ComponentModel.DataAnnotations.Schema.Column("type")]
        [Way.EntityDB.WayDBColumnAttribute(Name="type",Comment="",Caption="类型",Storage = "_Type",DbType="int")]
        public virtual System.Nullable<UserEffect_TypeEnum> Type
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

        String _Name;
        /// <summary>
        /// 
        /// </summary>
        [System.ComponentModel.DataAnnotations.Schema.Column("name")]
        [Way.EntityDB.WayDBColumnAttribute(Name="name",Comment="",Caption="",Storage = "_Name",DbType="varchar(50)")]
        public virtual String Name
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

        System.Nullable<Int32> _Version=0;
        /// <summary>
        /// 
        /// </summary>
        [System.ComponentModel.DataAnnotations.Schema.Column("version")]
        [Way.EntityDB.WayDBColumnAttribute(Name="version",Comment="",Caption="",Storage = "_Version",DbType="int")]
        public virtual System.Nullable<Int32> Version
        {
            get
            {
                return this._Version;
            }
            set
            {
                if ((this._Version != value))
                {
                    this.SendPropertyChanging("Version",this._Version,value);
                    this._Version = value;
                    this.SendPropertyChanged("Version");

                }
            }
        }
}}
namespace SysDB{

/// <summary>
/// 
/// </summary>
public enum AdminUser_RoleEnum:int
{
    

/// <summary>
/// </summary>
超级管理员 = 1<<0,

/// <summary>
/// </summary>
普通员工 = 1<<1
}


    /// <summary>
	/// 
	/// </summary>
    [System.ComponentModel.DataAnnotations.Schema.Table("adminuser")]
    [Way.EntityDB.Attributes.Table("id")]
    public class AdminUser :Way.EntityDB.DataItem
    {

        /// <summary>
	    /// 
	    /// </summary>
        public  AdminUser()
        {
        }


        System.Nullable<Int32> _id;
        /// <summary>
        /// 
        /// </summary>
[System.ComponentModel.DataAnnotations.Key]
        [System.ComponentModel.DataAnnotations.Schema.Column("id")]
        [Way.EntityDB.WayDBColumnAttribute(Name="id",Comment="",Caption="",Storage = "_id",DbType="int" ,IsPrimaryKey=true,IsDbGenerated=true,CanBeNull=false)]
        public virtual System.Nullable<Int32> id
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

        String _UserName;
        /// <summary>
        /// 
        /// </summary>
        [System.ComponentModel.DataAnnotations.Schema.Column("username")]
        [Way.EntityDB.WayDBColumnAttribute(Name="username",Comment="",Caption="",Storage = "_UserName",DbType="varchar(50)")]
        public virtual String UserName
        {
            get
            {
                return this._UserName;
            }
            set
            {
                if ((this._UserName != value))
                {
                    this.SendPropertyChanging("UserName",this._UserName,value);
                    this._UserName = value;
                    this.SendPropertyChanged("UserName");

                }
            }
        }

        String _Password;
        /// <summary>
        /// 
        /// </summary>
        [System.ComponentModel.DataAnnotations.Schema.Column("password")]
        [Way.EntityDB.WayDBColumnAttribute(Name="password",Comment="",Caption="",Storage = "_Password",DbType="varchar(50)")]
        public virtual String Password
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

        System.Nullable<AdminUser_RoleEnum> _Role;
        /// <summary>
        /// 
        /// </summary>
        [System.ComponentModel.DataAnnotations.Schema.Column("role")]
        [Way.EntityDB.WayDBColumnAttribute(Name="role",Comment="",Caption="",Storage = "_Role",DbType="int")]
        public virtual System.Nullable<AdminUser_RoleEnum> Role
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

        String _PhoneNumber;
        /// <summary>
        /// 手机号
        /// </summary>
        [System.ComponentModel.DataAnnotations.Schema.Column("phonenumber")]
        [Way.EntityDB.WayDBColumnAttribute(Name="phonenumber",Comment="",Caption="手机号",Storage = "_PhoneNumber",DbType="varchar(50)")]
        public virtual String PhoneNumber
        {
            get
            {
                return this._PhoneNumber;
            }
            set
            {
                if ((this._PhoneNumber != value))
                {
                    this.SendPropertyChanging("PhoneNumber",this._PhoneNumber,value);
                    this._PhoneNumber = value;
                    this.SendPropertyChanged("PhoneNumber");

                }
            }
        }

        System.Nullable<Int32> _MaxCount=0;
        /// <summary>
        /// 最大注册码数量
        /// </summary>
        [System.ComponentModel.DataAnnotations.Schema.Column("maxcount")]
        [Way.EntityDB.WayDBColumnAttribute(Name="maxcount",Comment="",Caption="最大注册码数量",Storage = "_MaxCount",DbType="int")]
        public virtual System.Nullable<Int32> MaxCount
        {
            get
            {
                return this._MaxCount;
            }
            set
            {
                if ((this._MaxCount != value))
                {
                    this.SendPropertyChanging("MaxCount",this._MaxCount,value);
                    this._MaxCount = value;
                    this.SendPropertyChanged("MaxCount");

                }
            }
        }

        System.Nullable<Int32> _CreatedCount=0;
        /// <summary>
        /// 已创建数量
        /// </summary>
        [System.ComponentModel.DataAnnotations.Schema.Column("createdcount")]
        [Way.EntityDB.WayDBColumnAttribute(Name="createdcount",Comment="",Caption="已创建数量",Storage = "_CreatedCount",DbType="int")]
        public virtual System.Nullable<Int32> CreatedCount
        {
            get
            {
                return this._CreatedCount;
            }
            set
            {
                if ((this._CreatedCount != value))
                {
                    this.SendPropertyChanging("CreatedCount",this._CreatedCount,value);
                    this._CreatedCount = value;
                    this.SendPropertyChanged("CreatedCount");

                }
            }
        }
}}
namespace SysDB{


    /// <summary>
	/// 用户登录记录
	/// </summary>
    [System.ComponentModel.DataAnnotations.Schema.Table("userloginrecord")]
    [Way.EntityDB.Attributes.Table("id")]
    public class UserLoginRecord :Way.EntityDB.DataItem
    {

        /// <summary>
	    /// 
	    /// </summary>
        public  UserLoginRecord()
        {
        }


        System.Nullable<Int32> _id;
        /// <summary>
        /// 
        /// </summary>
[System.ComponentModel.DataAnnotations.Key]
        [System.ComponentModel.DataAnnotations.Schema.Column("id")]
        [Way.EntityDB.WayDBColumnAttribute(Name="id",Comment="",Caption="",Storage = "_id",DbType="int" ,IsPrimaryKey=true,IsDbGenerated=true,CanBeNull=false)]
        public virtual System.Nullable<Int32> id
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

        System.Nullable<Int32> _UserId;
        /// <summary>
        /// 
        /// </summary>
        [System.ComponentModel.DataAnnotations.Schema.Column("userid")]
        [Way.EntityDB.WayDBColumnAttribute(Name="userid",Comment="",Caption="",Storage = "_UserId",DbType="int")]
        public virtual System.Nullable<Int32> UserId
        {
            get
            {
                return this._UserId;
            }
            set
            {
                if ((this._UserId != value))
                {
                    this.SendPropertyChanging("UserId",this._UserId,value);
                    this._UserId = value;
                    this.SendPropertyChanged("UserId");

                }
            }
        }

        System.Nullable<DateTime> _LoginTime;
        /// <summary>
        /// 
        /// </summary>
        [System.ComponentModel.DataAnnotations.Schema.Column("logintime")]
        [Way.EntityDB.WayDBColumnAttribute(Name="logintime",Comment="",Caption="",Storage = "_LoginTime",DbType="datetime")]
        public virtual System.Nullable<DateTime> LoginTime
        {
            get
            {
                return this._LoginTime;
            }
            set
            {
                if ((this._LoginTime != value))
                {
                    this.SendPropertyChanging("LoginTime",this._LoginTime,value);
                    this._LoginTime = value;
                    this.SendPropertyChanged("LoginTime");

                }
            }
        }
}}

namespace SysDB.DB{
    /// <summary>
	/// 
	/// </summary>
    public class PandaAudio : Way.EntityDB.DBContext
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="dbType"></param>
        public PandaAudio(string connection, Way.EntityDB.DatabaseType dbType): base(connection, dbType)
        {
            if (!setEvented)
            {
                lock (lockObj)
                {
                    if (!setEvented)
                    {
                        setEvented = true;
                        Way.EntityDB.DBContext.BeforeDelete += Database_BeforeDelete;
                    }
                }
            }
        }

        static object lockObj = new object();
        static bool setEvented = false;
 

        static void Database_BeforeDelete(object sender, Way.EntityDB.DatabaseModifyEventArg e)
        {
            var db =  sender as SysDB.DB.PandaAudio;
            if (db == null)
                return;


                    if (e.DataItem is SysDB.UserInfo)
                    {
                        var deletingItem = (SysDB.UserInfo)e.DataItem;
                        
                    var items0 = (from m in db.UserEffect
                    where m.UserId == deletingItem.id
                    select new SysDB.UserEffect
                    {
                        id = m.id
                    });
                    while(true)
                    {
                        var data2del = items0.Take(100).ToList();
                        if(data2del.Count() ==0)
                            break;
                        foreach (var t in data2del)
                        {
                            db.Delete(t);
                        }
                    }

                    var items1 = (from m in db.UserLoginRecord
                    where m.UserId == deletingItem.id
                    select new SysDB.UserLoginRecord
                    {
                        id = m.id
                    });
                    while(true)
                    {
                        var data2del = items1.Take(100).ToList();
                        if(data2del.Count() ==0)
                            break;
                        foreach (var t in data2del)
                        {
                            db.Delete(t);
                        }
                    }

                    }

        }

        /// <summary>
	    /// 
	    /// </summary>
        /// <param name="modelBuilder"></param>
         protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
   modelBuilder.Entity<SysDB.UserInfo>().HasKey(m => m.id);
modelBuilder.Entity<SysDB.SystemRegCode>().HasKey(m => m.id);
modelBuilder.Entity<SysDB.UserEffect>().HasKey(m => m.id);
modelBuilder.Entity<SysDB.AdminUser>().HasKey(m => m.id);
modelBuilder.Entity<SysDB.UserLoginRecord>().HasKey(m => m.id);
}

        System.Linq.IQueryable<SysDB.UserInfo> _UserInfo;
        /// <summary>
        /// 用户信息
        /// </summary>
        public virtual System.Linq.IQueryable<SysDB.UserInfo> UserInfo
        {
             get
            {
                if (_UserInfo == null)
                {
                    _UserInfo = this.Set<SysDB.UserInfo>();
                }
                return _UserInfo;
            }
        }

        System.Linq.IQueryable<SysDB.SystemRegCode> _SystemRegCode;
        /// <summary>
        /// 系统注册码
        /// </summary>
        public virtual System.Linq.IQueryable<SysDB.SystemRegCode> SystemRegCode
        {
             get
            {
                if (_SystemRegCode == null)
                {
                    _SystemRegCode = this.Set<SysDB.SystemRegCode>();
                }
                return _SystemRegCode;
            }
        }

        System.Linq.IQueryable<SysDB.UserEffect> _UserEffect;
        /// <summary>
        /// 用户的效果存档
        /// </summary>
        public virtual System.Linq.IQueryable<SysDB.UserEffect> UserEffect
        {
             get
            {
                if (_UserEffect == null)
                {
                    _UserEffect = this.Set<SysDB.UserEffect>();
                }
                return _UserEffect;
            }
        }

        System.Linq.IQueryable<SysDB.AdminUser> _AdminUser;
        /// <summary>
        /// 
        /// </summary>
        public virtual System.Linq.IQueryable<SysDB.AdminUser> AdminUser
        {
             get
            {
                if (_AdminUser == null)
                {
                    _AdminUser = this.Set<SysDB.AdminUser>();
                }
                return _AdminUser;
            }
        }

        System.Linq.IQueryable<SysDB.UserLoginRecord> _UserLoginRecord;
        /// <summary>
        /// 用户登录记录
        /// </summary>
        public virtual System.Linq.IQueryable<SysDB.UserLoginRecord> UserLoginRecord
        {
             get
            {
                if (_UserLoginRecord == null)
                {
                    _UserLoginRecord = this.Set<SysDB.UserLoginRecord>();
                }
                return _UserLoginRecord;
            }
        }

protected override string GetDesignString(){System.Text.StringBuilder result = new System.Text.StringBuilder(); 
result.Append("\r\n");
result.Append("H4sIAAAAAAAAC+1d60/bVhT/V5A/hxE/EqAqH1jopmwdQ8CqTU0/OPENeHVsZjtjFYtUtLWj6lTY1KrQoXZo3eiHDVg7la5R1X+mjsN/0Ws7D78CDtSJnZ4v4PuIfY59fR6/c871CjHP5gWkEOcur1iH02wJEeeIuW8EXkVEgpiVlq3RrIpK1lFjCs/h4UusUMYNspJo");
result.Append("9avXllB7hMjIiFWRee7JgspLImGbW5BEFYmqbfpKzqIjR5zDhzyH/5OJHFFgl4zf4laO0O8+qa0dvnm9U1vdzxF40DiZOfKFguSsWJTM3ilWZfOsgrJTjXPwF6XCVXycrOBGRhLKJVHBzcv267ROZfTgdlaZLKtSVizIqIQJxUOqXEbG71lxuiwIuKPICorRw+XnMefW");
result.Append("j42ZiQYnretnlZlPzUbjFJLMIdm8MqaoSQTlYrb27Il282ft7v6bF9fre4/r+6v676tOrmfRwsfljvQ2yWsT3Li8jd5vWbmwyMrmGQQkLqiLZncqeRwXzRO32SDbbNDux2I1+k0h1aaQsVM4wyrKMp727ikkk12SSLdJTLnXwq3bte2X2vqh8/nPLEoimi6X8kju/x1m");
result.Append("2uSn7UReKLG80H/yUm3yRl13V/v/mbZ+4Lq1AqsWJbnUf8LTlSvGhKkvM5JY5BdMwWX1mPLDLlS5huBzCugrpiyfU7EwtgTO8RKdOkaiL7LiQlcS/XOBaykXr6CeRsvHjErqIpJBWoO0BmkN0jpe0lpEyz6Ca8yt1Tf3tY2/tMOnenW9hu286qZe3fqENUxVO3P4auy1");
result.Append("M3CW51U3V2abQ0W2LKimtDZ7u2R11GS1YOoEzsau0cshAame3t4pMTpMt8RtqevPqnr1kWWve2z0uWsKJhRb6hmJQ6dzT8ZDdE+ooO4JmbRTUWgyE+7LRgV2O0i3r/jm1WvsLuoPfrScxsZtc1oaZ7H6m7fS/VYFJp7qsV3HhGrXeZe5x7jzTulk4UVjwcd0RTl1j7+I");
result.Append("9n+pewcnnPRe49aHWA+Wl6yVyc3I0hKSVR4Z1K+0SDYWoswv8CIrtLWYJZoqlUoU9JDhdYIzBdAXQF8AfQH01b0zlRho38nfTSQ9/sVW9WjrprZxR7/7qLa2gS0QKyrg8TUuSlgXtqyrvvrBYxF2Do1lBUoZlDIoZVDKoJRBKbeVcmIAdXAHI4P2TTeo3X9+dP8/T5bB");
result.Append("PH+mGD7W2khtnqIzvHFiCD+6JsVomHiz+1FZ+BNegrV7a7WH29o/m7WdP7xg1IViERXUU2bEMCFCcHRgCC7VY4CNDo43O3IcPuKbOGfo7zwdOM+FdOc56P9WtYe3nQvFuv6Zb+kFsVyaQkVebMTjZOlrvPYm8FLLifMydssmqGB+FNkdu3SPQfQxcB0gla3Pap2Ofiob");
result.Append("E+9UtlS0U9nScU1lGx1YPG9swFyHcRs/cfUUggRCo+A7jIcOR9o8Ad+US9t4x6RLcAjAIYiNQ9AB8zCUTmvhR8Vfi65gIs0pIJkAqghl6dPvF1Rh84miK4g6CE5HQt4lJCvmczrzo+jaCj+Bh1SUhWmolZIO62ySK/GiYdedDvulkiFiv0zgckiyT4WETGAMmKJ6A790");
result.Append("S6INA6Yc5ZizkhCCBK0/v6G/3NX3dvSNm9ovm0MTQ+T583gN5cTa1t7R9Qe4Tzv80+omce8Jvh0TLcSXDLcebgDzppNuTCoaJcyDX0vgr7wpxsVN/WC1hUQ5GfmMvYrk/nNDR1mNH1dZBOWxsJkBRIAgAgQRIIgAQQQoahGglm7yhKS3r+PoHLZRa3/v1A9+wEZeZhEV");
result.Append("rhqGkF+U6yKrqOaESMS6MEgbYWsJik7BeYq+85SIu6/UQdClPYmjBit+Ms3gpjfi7AR+mChLs3CreZ2wrQcGcg53goAAvAXwNurgbQdpNRrFTEEmxuIK6hxh383QXxBIVoZkZUhWhmRlSFY+NVSVGGhkqoO150iBmuIVK6Elcon9OG0nuhYedVzZKaSOQy3poCZoGoeX");
result.Append("FHWCfj8zNRPxTMyMS0EOSYYvV2EPdPhiRWDpC2W+oWATUOYLZb5Q5hvnMt/B9ZwTMXWU/b19mnY/HysZoL77a/3WU9fqCqVIYWTEimdZ18Xuw7Qkl1hhwgx1jYxoN1brey/qr7f1J7drd3a1tee4e1IQpOVZVJJUNNGMio2MZL6iqNZZjIYxRg59P2SbbswM5y5Hul6U");
result.Append("DHd7HKiVgFqJ9z7dJxH/7J5OOsLLEOgIPx1BxbkKdjzUL/P4LyF9q6q9ulffO8B/vdmH5rY7s6jQrN3sumCWpkfDLLxLB628o2mHrdiL5MR08O+C0g6s0rzpPZJJ6Wh9iIekwt9XA+BFgBcBXoRdBGEXQdhFEHYRBHhxEOBFmykJaOK7RBNjE5enyOgU4HQAMZikTyDi");
result.Append("8W7r47C1ewdHP6274ZnvMlK54Wz1OHGDCRwipRn3tija4VNt7Tet+tKPKcuZ56LKmPWd5v6sc9xsnJswVrp1bF/vBgJh3AHctjDvD7KimmbMCzlmN94B7/w5VebFBe8P2i9C8N84eOpI2hWDLWN0DqnNi6W4PEqm2WFmLMkMM+k8NTw+zqJhKp8qFmm2SHLFPFF5C8pg");
result.Append("YTKKhgAA");
return result.ToString();}
}}
