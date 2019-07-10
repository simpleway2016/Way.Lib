
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
普通员工 = 1<<1,

/// <summary>
/// </summary>
大客户 = 1<<2
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
result.Append("H4sIAAAAAAAAC+1d3U/bVhT/V5Cfw4g/EqAqDyx0U7aOIWDVpqYPTnwDXh2b2c5YxSIVbe2oOhU2tSp0qB1aN/qwAWun0jWq+s/USfgvem3nwx8JOFAn1+l5Ad/rG/uc6+vz8TvnXK9Q83xWQhp17vKKfTjNFxB1jpr7RhJ1RMWoWWXZPpvWUcE+qg8RBXz6Ei8VcYMu");
result.Append("xZr9+rUl1DpDpVTE68i69mROFxWZcozNKbKOZN0xfCVj05GhzuFDUcD/6ViGyvFL5m9xK0NV7z6prB2+eb1TWd3PUPikeTHrzBcaUtNyXrF6p3idz/IaSk/VryFeVHJX8XG8hBspRSoWZA03Lzvv07yU2YPbaW2yqCtpOaeiAiYUn9LVIjJ/z8vTRUnCHXle0sweITuP");
result.Append("Obd/bI6M1Tlp3j+tzXxqNeqXUFQBqdadMUUNIhgPs5VnT4ybPxt399+8uF7be1zbX63+vurmehYtfFzsSG+DvBbB9ds76P2WV3OLvGpdQULygr5odSfix3HRuHCLDbrFBut9LHaj3xQyLQo5J4UzvKYt42HvnkI63iWJbIvEhHct3Lpd2X5prB+6n//MoiKj6WIhi9T+");
result.Append("zzDXIj/pJPJCgRel/pOXaJE36pld4/9nxvqBZ2olXs8raqH/hCdLV8wBU1+mFDkvLliCy+6x5IdTqAp1wecW0FcsWT6nY2FsC5zjJTpzjERf5OWFriT655LQVC5+QT2Nlo85q+iLSAVpDdIapDVI62hJaxkttxFcY16tvrlvbPxlHD6tltcr2M4rb1bLW5/wpqnqZA7f");
result.Append("jb92Bs6you7lymoLKM8XJd2S1lZvl6yOWqzmLJ0gONg1ewUkId3X2zslxobplngt9eqzcrX8yLbXfTb63DUNE4ot9ZQioNO5J+MhuidMUPeEjjupyDWYCfdlYwK7HbTXV3zz6jV2F6sPfrSdxvq0uS2Ns1j9jan0vlWBiWd6bNdxodp1/mXuM+78QzpZeGQs+IiuKLfu");
result.Append("aS+i27/UvYMTTnqvcetDrAeLS/bKFGZUZQmpuohM6leaJJsLURUXRJmXWlrMFk2lUokEPWR6neBMAfQF0BdAXwB9de9MxQbad2rvJtI+/2KrfLR109i4U737qLK2gS0QOyrg8zUuKlgXNq2rvvrBYwQ7h+ayAqUMShmUMihlUMqglFtKOTaAOriDkcG2TTeo3H9+dP8/");
result.Append("X5bBvHimGD7W2khvXKIzvHFiCJ9ck2I0TLzZ+6hs/Akvwcq9tcrDbeOfzcrOH34w6kI+j3L6KTNiuBAhODYwBJfoMcDGBsebXTkOH4kNnDP0d54NnOdCe/Mcqv+WjYe33QvFvv+Zp/SCXCxMobwo1+NxqvI1XnsTeKll5HkVu2UTTDA/iu6OXbbHIPoYuA6QytZntc6S");
result.Append("n8rGRTuVLUF2KlsyqqlsowOL540NmOsw7uAnqp5CkEAoCb7DeOhwpMMTaJty6TjfMekSHAJwCCLjEHTAPEyl01z4pPhr5Aom2hoCkgmgilCWPvt+QRUOn4hcQdRBcLoS8i4hVbOe05kfRddW+Ak8JEgWpqFWSrqss0mhIMqmXXc67JeJh4j9coHLIek+FRJygTFghukN");
result.Append("/NItiQ4MmHGVY84qUggStPb8RvXlbnVvp7px0/hlc2hiiD5/Hq+hjFzZ2ju6/gD3GYd/2t007j3Bt+PIQnzpcOvhBjBvOu7FpMgoYR78WoL2ypvhPNzUDlabSJSbkc/4q0jtPzcsyWr8uMoiKI+FzQwgAgQRIIgAQQQIIkCkRYCauskXkt6+jqNz2Eat/L1TO/gBG3mp");
result.Append("RZS7ahpC7aJcF3lNtwYQEevCIC3B1hIUnYLzRL7zFIu6r9RB0CV9iaMmK+1kmslNb8TZCfxwJEuzcKt53bCtDwZyn+4EAQF4C+At6eBtB2k1SmKmIBdhcQV1jrDvZugvCCQrQ7IyJCtDsjIkK58aqooNNDLVwdpzpUBNiZqd0EJcYj9O2yHXwmOOKzuF1HGoJR3UBE3z");
result.Append("8JKmT7DvZ6ZmLJqJmVEpyKHp8OUq7IEOX6wILH2hzDcUbALKfKHMF8p8o1zmO7iecyyijnJ7b59lvc/HTgao7f5au/XUs7pCKVIYGbHjWfZ9sfswragFXpqwQl0jI8aN1drei9rr7eqT25U7u8bac9w9KUnK8iwqKDqaaETFRkZSXzFM8ypmwzxHD30/5Bhujgxnlomu");
result.Append("F6XD3R4HaiWgVuK9T/eJRT+7p5OO8DMEOqKdjmCiXAU7HuqXedovoepW2Xh1r7Z3gP/6sw+tbXdmUa5Ru9l1wSzLjoZZeJcMWnnHsi5bsRfJicng3wVlXVilNek9kklJsj7EQzPh76sB8CLAiwAvwi6CsIsg7CIIuwgCvDgI8KLDlAQ08V2iiZGJyzM0OQU4HUAMLt4m");
result.Append("EPF4t/lx2Mq9g6Of1r3wzHcppVh3tnqcuMEFDpGynHdbFOPwqbH2m1F+2Y4p25kXSGXM/k4zqeucIXWd92/ScLN+bcqcNvvYOXkmbGMuG9y2AwUfpGU9yVk3co2uT6h//JyuivKC/wetWQ3+GxdPHUm7YrJlnp1DeuNmCSGL4kl+mBuLc8NcMssMj4/zaJjJJvJ5ls/T");
result.Append("Qj5Lld4ChZcAD7+HAAA=");
return result.ToString();}
}}
