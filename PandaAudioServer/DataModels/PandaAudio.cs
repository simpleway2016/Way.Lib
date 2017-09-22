
using System;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;

namespace SysDB{


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
}}
namespace SysDB{


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
}}
namespace SysDB{

/// <summary>
/// 
/// </summary>
public enum UserEffect_TypeEnum:int
{
    

/// <summary>
/// 
/// </summary>
Project=1,

/// <summary>
/// 
/// </summary>

Track=2,

/// <summary>
/// 
/// </summary>

Vst=3,
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
/// 
/// </summary>
超级管理员 = 1<<0,

/// <summary>
/// 
/// </summary>

普通员工 = 1<<1
,
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
                        Way.EntityDB.Design.DBUpgrade.Upgrade(this, _designData);
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

static string _designData = "eyJUYWJsZXMiOlt7IlRhYmxlTmFtZSI6IlBvc3RncmVTcWwiLCJSb3dzIjpbeyJJdGVtcyI6W3siTmFtZSI6ImlkIiwiVmFsdWUiOjF9LHsiTmFtZSI6InR5cGUiLCJWYWx1ZSI6IkNyZWF0ZVRhYmxlQWN0aW9uIn0seyJOYW1lIjoiY29udGVudCIsIlZhbHVlIjoie1wiVGFibGVcIjp7XCJpZFwiOjEsXCJjYXB0aW9uXCI6XCLnlKjmiLfkv6Hmga9cIixcIk5hbWVcIjpcIlVzZXJJbmZvXCIsXCJEYXRhYmFzZUlEXCI6MSxcImlMb2NrXCI6MH0sXCJDb2x1bW5zXCI6W3tcImlkXCI6MSxcIk5hbWVcIjpcImlkXCIsXCJJc0F1dG9JbmNyZW1lbnRcIjp0cnVlLFwiQ2FuTnVsbFwiOmZhbHNlLFwiZGJUeXBlXCI6XCJpbnRcIixcIlRhYmxlSURcIjoxLFwiSXNQS0lEXCI6dHJ1ZSxcIm9yZGVyaWRcIjowfSx7XCJpZFwiOjIsXCJjYXB0aW9uXCI6XCLms6jlhozllK/kuIDorqTor4HnoIFcIixcIk5hbWVcIjpcIlJlZ0d1aWRcIixcIklzQXV0b0luY3JlbWVudFwiOmZhbHNlLFwiQ2FuTnVsbFwiOnRydWUsXCJkYlR5cGVcIjpcInZhcmNoYXJcIixcImxlbmd0aFwiOlwiNTBcIixcIlRhYmxlSURcIjoxLFwiSXNQS0lEXCI6ZmFsc2UsXCJvcmRlcmlkXCI6MX0se1wiaWRcIjozLFwiTmFtZVwiOlwiVXNlck5hbWVcIixcIklzQXV0b0luY3JlbWVudFwiOmZhbHNlLFwiQ2FuTnVsbFwiOnRydWUsXCJkYlR5cGVcIjpcInZhcmNoYXJcIixcImxlbmd0aFwiOlwiNTBcIixcIlRhYmxlSURcIjoxLFwiSXNQS0lEXCI6ZmFsc2UsXCJvcmRlcmlkXCI6Mn0se1wiaWRcIjo0LFwiTmFtZVwiOlwiUGFzc3dvcmRcIixcIklzQXV0b0luY3JlbWVudFwiOmZhbHNlLFwiQ2FuTnVsbFwiOnRydWUsXCJkYlR5cGVcIjpcInZhcmNoYXJcIixcImxlbmd0aFwiOlwiMTAwXCIsXCJUYWJsZUlEXCI6MSxcIklzUEtJRFwiOmZhbHNlLFwib3JkZXJpZFwiOjN9LHtcImlkXCI6NSxcImNhcHRpb25cIjpcIuaJi+acuuWPt1wiLFwiTmFtZVwiOlwiUGhvbmVOdW1iZXJcIixcIklzQXV0b0luY3JlbWVudFwiOmZhbHNlLFwiQ2FuTnVsbFwiOnRydWUsXCJkYlR5cGVcIjpcInZhcmNoYXJcIixcImxlbmd0aFwiOlwiNTBcIixcIlRhYmxlSURcIjoxLFwiSXNQS0lEXCI6ZmFsc2UsXCJvcmRlcmlkXCI6NH0se1wiaWRcIjo2LFwiTmFtZVwiOlwiRW1haWxcIixcIklzQXV0b0luY3JlbWVudFwiOmZhbHNlLFwiQ2FuTnVsbFwiOnRydWUsXCJkYlR5cGVcIjpcInZhcmNoYXJcIixcImxlbmd0aFwiOlwiNTBcIixcIlRhYmxlSURcIjoxLFwiSXNQS0lEXCI6ZmFsc2UsXCJvcmRlcmlkXCI6NX0se1wiaWRcIjo3LFwiY2FwdGlvblwiOlwi5bmz5Y+wXCIsXCJOYW1lXCI6XCJQbGF0Zm9ybVwiLFwiSXNBdXRvSW5jcmVtZW50XCI6ZmFsc2UsXCJDYW5OdWxsXCI6dHJ1ZSxcImRiVHlwZVwiOlwidmFyY2hhclwiLFwibGVuZ3RoXCI6XCI1MFwiLFwiVGFibGVJRFwiOjEsXCJJc1BLSURcIjpmYWxzZSxcIm9yZGVyaWRcIjo2fV0sXCJJRFhDb25maWdzXCI6W10sXCJJRFwiOjB9In0seyJOYW1lIjoiZGF0YWJhc2VpZCIsIlZhbHVlIjoxfV0sIlJvd1N0YXRlIjowfSx7Ikl0ZW1zIjpbeyJOYW1lIjoiaWQiLCJWYWx1ZSI6Mn0seyJOYW1lIjoidHlwZSIsIlZhbHVlIjoiQ2hhbmdlVGFibGVBY3Rpb24ifSx7Ik5hbWUiOiJjb250ZW50IiwiVmFsdWUiOiJ7XCJPbGRUYWJsZU5hbWVcIjpcIlVzZXJJbmZvXCIsXCJOZXdUYWJsZU5hbWVcIjpcIlVzZXJJbmZvXCIsXCJvdGhlckNvbHVtbnNcIjpbe1wiaWRcIjoxLFwiTmFtZVwiOlwiaWRcIixcIklzQXV0b0luY3JlbWVudFwiOnRydWUsXCJDYW5OdWxsXCI6ZmFsc2UsXCJkYlR5cGVcIjpcImludFwiLFwiVGFibGVJRFwiOjEsXCJJc1BLSURcIjp0cnVlLFwib3JkZXJpZFwiOjB9LHtcImlkXCI6MixcImNhcHRpb25cIjpcIuazqOWGjOWUr+S4gOiupOivgeeggVwiLFwiTmFtZVwiOlwiUmVnR3VpZFwiLFwiSXNBdXRvSW5jcmVtZW50XCI6ZmFsc2UsXCJDYW5OdWxsXCI6dHJ1ZSxcImRiVHlwZVwiOlwidmFyY2hhclwiLFwibGVuZ3RoXCI6XCI1MFwiLFwiVGFibGVJRFwiOjEsXCJJc1BLSURcIjpmYWxzZSxcIm9yZGVyaWRcIjoxfSx7XCJpZFwiOjMsXCJOYW1lXCI6XCJVc2VyTmFtZVwiLFwiSXNBdXRvSW5jcmVtZW50XCI6ZmFsc2UsXCJDYW5OdWxsXCI6dHJ1ZSxcImRiVHlwZVwiOlwidmFyY2hhclwiLFwibGVuZ3RoXCI6XCI1MFwiLFwiVGFibGVJRFwiOjEsXCJJc1BLSURcIjpmYWxzZSxcIm9yZGVyaWRcIjoyfSx7XCJpZFwiOjQsXCJOYW1lXCI6XCJQYXNzd29yZFwiLFwiSXNBdXRvSW5jcmVtZW50XCI6ZmFsc2UsXCJDYW5OdWxsXCI6dHJ1ZSxcImRiVHlwZVwiOlwidmFyY2hhclwiLFwibGVuZ3RoXCI6XCIxMDBcIixcIlRhYmxlSURcIjoxLFwiSXNQS0lEXCI6ZmFsc2UsXCJvcmRlcmlkXCI6M30se1wiaWRcIjo1LFwiY2FwdGlvblwiOlwi5omL5py65Y+3XCIsXCJOYW1lXCI6XCJQaG9uZU51bWJlclwiLFwiSXNBdXRvSW5jcmVtZW50XCI6ZmFsc2UsXCJDYW5OdWxsXCI6dHJ1ZSxcImRiVHlwZVwiOlwidmFyY2hhclwiLFwibGVuZ3RoXCI6XCI1MFwiLFwiVGFibGVJRFwiOjEsXCJJc1BLSURcIjpmYWxzZSxcIm9yZGVyaWRcIjo0fSx7XCJpZFwiOjYsXCJOYW1lXCI6XCJFbWFpbFwiLFwiSXNBdXRvSW5jcmVtZW50XCI6ZmFsc2UsXCJDYW5OdWxsXCI6dHJ1ZSxcImRiVHlwZVwiOlwidmFyY2hhclwiLFwibGVuZ3RoXCI6XCI1MFwiLFwiVGFibGVJRFwiOjEsXCJJc1BLSURcIjpmYWxzZSxcIm9yZGVyaWRcIjo1fSx7XCJpZFwiOjcsXCJjYXB0aW9uXCI6XCLlubPlj7BcIixcIk5hbWVcIjpcIlBsYXRmb3JtXCIsXCJJc0F1dG9JbmNyZW1lbnRcIjpmYWxzZSxcIkNhbk51bGxcIjp0cnVlLFwiZGJUeXBlXCI6XCJ2YXJjaGFyXCIsXCJsZW5ndGhcIjpcIjUwXCIsXCJUYWJsZUlEXCI6MSxcIklzUEtJRFwiOmZhbHNlLFwib3JkZXJpZFwiOjZ9XSxcIm5ld0NvbHVtbnNcIjpbe1wiaWRcIjo4LFwiY2FwdGlvblwiOlwi5piv5ZCm5bey57uP5pSv5LuY57uZSmFja1wiLFwiTmFtZVwiOlwiSXNQYXlcIixcIklzQXV0b0luY3JlbWVudFwiOmZhbHNlLFwiQ2FuTnVsbFwiOnRydWUsXCJkYlR5cGVcIjpcImJpdFwiLFwibGVuZ3RoXCI6XCJcIixcImRlZmF1bHRWYWx1ZVwiOlwiMFwiLFwiVGFibGVJRFwiOjEsXCJJc1BLSURcIjpmYWxzZSxcIm9yZGVyaWRcIjo3fV0sXCJjaGFuZ2VkQ29sdW1uc1wiOltdLFwiZGVsZXRlZENvbHVtbnNcIjpbXSxcIklEWENvbmZpZ3NcIjpbXSxcIklEXCI6MH0ifSx7Ik5hbWUiOiJkYXRhYmFzZWlkIiwiVmFsdWUiOjF9XSwiUm93U3RhdGUiOjB9LHsiSXRlbXMiOlt7Ik5hbWUiOiJpZCIsIlZhbHVlIjozfSx7Ik5hbWUiOiJ0eXBlIiwiVmFsdWUiOiJDcmVhdGVUYWJsZUFjdGlvbiJ9LHsiTmFtZSI6ImNvbnRlbnQiLCJWYWx1ZSI6IntcIlRhYmxlXCI6e1wiaWRcIjoyLFwiY2FwdGlvblwiOlwi57O757uf5rOo5YaM56CBXCIsXCJOYW1lXCI6XCJTeXN0ZW1SZWdDb2RlXCIsXCJEYXRhYmFzZUlEXCI6MSxcImlMb2NrXCI6MH0sXCJDb2x1bW5zXCI6W3tcImlkXCI6OSxcIk5hbWVcIjpcImlkXCIsXCJJc0F1dG9JbmNyZW1lbnRcIjp0cnVlLFwiQ2FuTnVsbFwiOmZhbHNlLFwiZGJUeXBlXCI6XCJpbnRcIixcIlRhYmxlSURcIjoyLFwiSXNQS0lEXCI6dHJ1ZSxcIm9yZGVyaWRcIjowfSx7XCJpZFwiOjEwLFwiTmFtZVwiOlwiY29kZVwiLFwiSXNBdXRvSW5jcmVtZW50XCI6ZmFsc2UsXCJDYW5OdWxsXCI6dHJ1ZSxcImRiVHlwZVwiOlwidmFyY2hhclwiLFwibGVuZ3RoXCI6XCI1MFwiLFwiVGFibGVJRFwiOjIsXCJJc1BLSURcIjpmYWxzZSxcIm9yZGVyaWRcIjoxfSx7XCJpZFwiOjExLFwiY2FwdGlvblwiOlwi5L2/55So55qE55So5oi3aWRcIixcIk5hbWVcIjpcIlVzZXJJZFwiLFwiSXNBdXRvSW5jcmVtZW50XCI6ZmFsc2UsXCJDYW5OdWxsXCI6dHJ1ZSxcImRiVHlwZVwiOlwiaW50XCIsXCJsZW5ndGhcIjpcIlwiLFwiVGFibGVJRFwiOjIsXCJJc1BLSURcIjpmYWxzZSxcIm9yZGVyaWRcIjoyfV0sXCJJRFhDb25maWdzXCI6W10sXCJJRFwiOjB9In0seyJOYW1lIjoiZGF0YWJhc2VpZCIsIlZhbHVlIjoxfV0sIlJvd1N0YXRlIjowfSx7Ikl0ZW1zIjpbeyJOYW1lIjoiaWQiLCJWYWx1ZSI6NH0seyJOYW1lIjoidHlwZSIsIlZhbHVlIjoiQ2hhbmdlVGFibGVBY3Rpb24ifSx7Ik5hbWUiOiJjb250ZW50IiwiVmFsdWUiOiJ7XCJPbGRUYWJsZU5hbWVcIjpcIlN5c3RlbVJlZ0NvZGVcIixcIk5ld1RhYmxlTmFtZVwiOlwiU3lzdGVtUmVnQ29kZVwiLFwib3RoZXJDb2x1bW5zXCI6W3tcImlkXCI6OSxcIk5hbWVcIjpcImlkXCIsXCJJc0F1dG9JbmNyZW1lbnRcIjp0cnVlLFwiQ2FuTnVsbFwiOmZhbHNlLFwiZGJUeXBlXCI6XCJpbnRcIixcIlRhYmxlSURcIjoyLFwiSXNQS0lEXCI6dHJ1ZSxcIm9yZGVyaWRcIjowfSx7XCJpZFwiOjExLFwiY2FwdGlvblwiOlwi5L2/55So55qE55So5oi3aWRcIixcIk5hbWVcIjpcIlVzZXJJZFwiLFwiSXNBdXRvSW5jcmVtZW50XCI6ZmFsc2UsXCJDYW5OdWxsXCI6dHJ1ZSxcImRiVHlwZVwiOlwiaW50XCIsXCJsZW5ndGhcIjpcIlwiLFwiVGFibGVJRFwiOjIsXCJJc1BLSURcIjpmYWxzZSxcIm9yZGVyaWRcIjoyfV0sXCJuZXdDb2x1bW5zXCI6W10sXCJjaGFuZ2VkQ29sdW1uc1wiOlt7XCJpZFwiOjEwLFwiTmFtZVwiOlwiUmVnR3VpZFwiLFwiSXNBdXRvSW5jcmVtZW50XCI6ZmFsc2UsXCJDYW5OdWxsXCI6dHJ1ZSxcImRiVHlwZVwiOlwidmFyY2hhclwiLFwibGVuZ3RoXCI6XCI1MFwiLFwiVGFibGVJRFwiOjIsXCJJc1BLSURcIjpmYWxzZSxcIm9yZGVyaWRcIjoxLFwiQmFja3VwQ2hhbmdlZFByb3BlcnRpZXNcIjp7XCJOYW1lXCI6e1wiT3JpZ2luYWxWYWx1ZVwiOlwiY29kZVwifX19XSxcImRlbGV0ZWRDb2x1bW5zXCI6W10sXCJJRFhDb25maWdzXCI6W10sXCJJRFwiOjB9In0seyJOYW1lIjoiZGF0YWJhc2VpZCIsIlZhbHVlIjoxfV0sIlJvd1N0YXRlIjowfSx7Ikl0ZW1zIjpbeyJOYW1lIjoiaWQiLCJWYWx1ZSI6NX0seyJOYW1lIjoidHlwZSIsIlZhbHVlIjoiQ2hhbmdlVGFibGVBY3Rpb24ifSx7Ik5hbWUiOiJjb250ZW50IiwiVmFsdWUiOiJ7XCJPbGRUYWJsZU5hbWVcIjpcIlVzZXJJbmZvXCIsXCJOZXdUYWJsZU5hbWVcIjpcIlVzZXJJbmZvXCIsXCJvdGhlckNvbHVtbnNcIjpbe1wiaWRcIjoxLFwiTmFtZVwiOlwiaWRcIixcIklzQXV0b0luY3JlbWVudFwiOnRydWUsXCJDYW5OdWxsXCI6ZmFsc2UsXCJkYlR5cGVcIjpcImludFwiLFwiVGFibGVJRFwiOjEsXCJJc1BLSURcIjp0cnVlLFwib3JkZXJpZFwiOjB9LHtcImlkXCI6MixcImNhcHRpb25cIjpcIuazqOWGjOWUr+S4gOiupOivgeeggVwiLFwiTmFtZVwiOlwiUmVnR3VpZFwiLFwiSXNBdXRvSW5jcmVtZW50XCI6ZmFsc2UsXCJDYW5OdWxsXCI6dHJ1ZSxcImRiVHlwZVwiOlwidmFyY2hhclwiLFwibGVuZ3RoXCI6XCI1MFwiLFwiVGFibGVJRFwiOjEsXCJJc1BLSURcIjpmYWxzZSxcIm9yZGVyaWRcIjoxfSx7XCJpZFwiOjMsXCJOYW1lXCI6XCJVc2VyTmFtZVwiLFwiSXNBdXRvSW5jcmVtZW50XCI6ZmFsc2UsXCJDYW5OdWxsXCI6dHJ1ZSxcImRiVHlwZVwiOlwidmFyY2hhclwiLFwibGVuZ3RoXCI6XCI1MFwiLFwiVGFibGVJRFwiOjEsXCJJc1BLSURcIjpmYWxzZSxcIm9yZGVyaWRcIjoyfSx7XCJpZFwiOjQsXCJOYW1lXCI6XCJQYXNzd29yZFwiLFwiSXNBdXRvSW5jcmVtZW50XCI6ZmFsc2UsXCJDYW5OdWxsXCI6dHJ1ZSxcImRiVHlwZVwiOlwidmFyY2hhclwiLFwibGVuZ3RoXCI6XCIxMDBcIixcIlRhYmxlSURcIjoxLFwiSXNQS0lEXCI6ZmFsc2UsXCJvcmRlcmlkXCI6M30se1wiaWRcIjo1LFwiY2FwdGlvblwiOlwi5omL5py65Y+3XCIsXCJOYW1lXCI6XCJQaG9uZU51bWJlclwiLFwiSXNBdXRvSW5jcmVtZW50XCI6ZmFsc2UsXCJDYW5OdWxsXCI6dHJ1ZSxcImRiVHlwZVwiOlwidmFyY2hhclwiLFwibGVuZ3RoXCI6XCI1MFwiLFwiVGFibGVJRFwiOjEsXCJJc1BLSURcIjpmYWxzZSxcIm9yZGVyaWRcIjo0fSx7XCJpZFwiOjYsXCJOYW1lXCI6XCJFbWFpbFwiLFwiSXNBdXRvSW5jcmVtZW50XCI6ZmFsc2UsXCJDYW5OdWxsXCI6dHJ1ZSxcImRiVHlwZVwiOlwidmFyY2hhclwiLFwibGVuZ3RoXCI6XCI1MFwiLFwiVGFibGVJRFwiOjEsXCJJc1BLSURcIjpmYWxzZSxcIm9yZGVyaWRcIjo1fSx7XCJpZFwiOjcsXCJjYXB0aW9uXCI6XCLlubPlj7BcIixcIk5hbWVcIjpcIlBsYXRmb3JtXCIsXCJJc0F1dG9JbmNyZW1lbnRcIjpmYWxzZSxcIkNhbk51bGxcIjp0cnVlLFwiZGJUeXBlXCI6XCJ2YXJjaGFyXCIsXCJsZW5ndGhcIjpcIjUwXCIsXCJUYWJsZUlEXCI6MSxcIklzUEtJRFwiOmZhbHNlLFwib3JkZXJpZFwiOjZ9LHtcImlkXCI6OCxcImNhcHRpb25cIjpcIuaYr+WQpuW3sue7j+aUr+S7mOe7mUphY2tcIixcIk5hbWVcIjpcIklzUGF5XCIsXCJJc0F1dG9JbmNyZW1lbnRcIjpmYWxzZSxcIkNhbk51bGxcIjp0cnVlLFwiZGJUeXBlXCI6XCJiaXRcIixcImxlbmd0aFwiOlwiXCIsXCJkZWZhdWx0VmFsdWVcIjpcIjBcIixcIlRhYmxlSURcIjoxLFwiSXNQS0lEXCI6ZmFsc2UsXCJvcmRlcmlkXCI6N31dLFwibmV3Q29sdW1uc1wiOlt7XCJpZFwiOjEyLFwiY2FwdGlvblwiOlwi55m76ZmG5ZCO55Sf5oiQ55qE5ZSv5LiA56CBXCIsXCJOYW1lXCI6XCJMb2dpbkNvZGVcIixcIklzQXV0b0luY3JlbWVudFwiOmZhbHNlLFwiQ2FuTnVsbFwiOnRydWUsXCJkYlR5cGVcIjpcInZhcmNoYXJcIixcImxlbmd0aFwiOlwiNTBcIixcIlRhYmxlSURcIjoxLFwiSXNQS0lEXCI6ZmFsc2UsXCJvcmRlcmlkXCI6OH1dLFwiY2hhbmdlZENvbHVtbnNcIjpbXSxcImRlbGV0ZWRDb2x1bW5zXCI6W10sXCJJRFhDb25maWdzXCI6W10sXCJJRFwiOjB9In0seyJOYW1lIjoiZGF0YWJhc2VpZCIsIlZhbHVlIjoxfV0sIlJvd1N0YXRlIjowfSx7Ikl0ZW1zIjpbeyJOYW1lIjoiaWQiLCJWYWx1ZSI6Nn0seyJOYW1lIjoidHlwZSIsIlZhbHVlIjoiQ2hhbmdlVGFibGVBY3Rpb24ifSx7Ik5hbWUiOiJjb250ZW50IiwiVmFsdWUiOiJ7XCJPbGRUYWJsZU5hbWVcIjpcIlVzZXJJbmZvXCIsXCJOZXdUYWJsZU5hbWVcIjpcIlVzZXJJbmZvXCIsXCJvdGhlckNvbHVtbnNcIjpbe1wiaWRcIjoxLFwiTmFtZVwiOlwiaWRcIixcIklzQXV0b0luY3JlbWVudFwiOnRydWUsXCJDYW5OdWxsXCI6ZmFsc2UsXCJkYlR5cGVcIjpcImludFwiLFwiVGFibGVJRFwiOjEsXCJJc1BLSURcIjp0cnVlLFwib3JkZXJpZFwiOjB9LHtcImlkXCI6MixcImNhcHRpb25cIjpcIuazqOWGjOWUr+S4gOiupOivgeeggVwiLFwiTmFtZVwiOlwiUmVnR3VpZFwiLFwiSXNBdXRvSW5jcmVtZW50XCI6ZmFsc2UsXCJDYW5OdWxsXCI6dHJ1ZSxcImRiVHlwZVwiOlwidmFyY2hhclwiLFwibGVuZ3RoXCI6XCI1MFwiLFwiVGFibGVJRFwiOjEsXCJJc1BLSURcIjpmYWxzZSxcIm9yZGVyaWRcIjoxfSx7XCJpZFwiOjMsXCJOYW1lXCI6XCJVc2VyTmFtZVwiLFwiSXNBdXRvSW5jcmVtZW50XCI6ZmFsc2UsXCJDYW5OdWxsXCI6dHJ1ZSxcImRiVHlwZVwiOlwidmFyY2hhclwiLFwibGVuZ3RoXCI6XCI1MFwiLFwiVGFibGVJRFwiOjEsXCJJc1BLSURcIjpmYWxzZSxcIm9yZGVyaWRcIjoyfSx7XCJpZFwiOjQsXCJOYW1lXCI6XCJQYXNzd29yZFwiLFwiSXNBdXRvSW5jcmVtZW50XCI6ZmFsc2UsXCJDYW5OdWxsXCI6dHJ1ZSxcImRiVHlwZVwiOlwidmFyY2hhclwiLFwibGVuZ3RoXCI6XCIxMDBcIixcIlRhYmxlSURcIjoxLFwiSXNQS0lEXCI6ZmFsc2UsXCJvcmRlcmlkXCI6M30se1wiaWRcIjo1LFwiY2FwdGlvblwiOlwi5omL5py65Y+3XCIsXCJOYW1lXCI6XCJQaG9uZU51bWJlclwiLFwiSXNBdXRvSW5jcmVtZW50XCI6ZmFsc2UsXCJDYW5OdWxsXCI6dHJ1ZSxcImRiVHlwZVwiOlwidmFyY2hhclwiLFwibGVuZ3RoXCI6XCI1MFwiLFwiVGFibGVJRFwiOjEsXCJJc1BLSURcIjpmYWxzZSxcIm9yZGVyaWRcIjo0fSx7XCJpZFwiOjYsXCJOYW1lXCI6XCJFbWFpbFwiLFwiSXNBdXRvSW5jcmVtZW50XCI6ZmFsc2UsXCJDYW5OdWxsXCI6dHJ1ZSxcImRiVHlwZVwiOlwidmFyY2hhclwiLFwibGVuZ3RoXCI6XCI1MFwiLFwiVGFibGVJRFwiOjEsXCJJc1BLSURcIjpmYWxzZSxcIm9yZGVyaWRcIjo1fSx7XCJpZFwiOjcsXCJjYXB0aW9uXCI6XCLlubPlj7BcIixcIk5hbWVcIjpcIlBsYXRmb3JtXCIsXCJJc0F1dG9JbmNyZW1lbnRcIjpmYWxzZSxcIkNhbk51bGxcIjp0cnVlLFwiZGJUeXBlXCI6XCJ2YXJjaGFyXCIsXCJsZW5ndGhcIjpcIjUwXCIsXCJUYWJsZUlEXCI6MSxcIklzUEtJRFwiOmZhbHNlLFwib3JkZXJpZFwiOjZ9LHtcImlkXCI6OCxcImNhcHRpb25cIjpcIuaYr+WQpuW3sue7j+aUr+S7mOe7mUphY2tcIixcIk5hbWVcIjpcIklzUGF5XCIsXCJJc0F1dG9JbmNyZW1lbnRcIjpmYWxzZSxcIkNhbk51bGxcIjp0cnVlLFwiZGJUeXBlXCI6XCJiaXRcIixcImxlbmd0aFwiOlwiXCIsXCJkZWZhdWx0VmFsdWVcIjpcIjBcIixcIlRhYmxlSURcIjoxLFwiSXNQS0lEXCI6ZmFsc2UsXCJvcmRlcmlkXCI6N30se1wiaWRcIjoxMixcImNhcHRpb25cIjpcIueZu+mZhuWQjueUn+aIkOeahOWUr+S4gOeggVwiLFwiTmFtZVwiOlwiTG9naW5Db2RlXCIsXCJJc0F1dG9JbmNyZW1lbnRcIjpmYWxzZSxcIkNhbk51bGxcIjp0cnVlLFwiZGJUeXBlXCI6XCJ2YXJjaGFyXCIsXCJsZW5ndGhcIjpcIjUwXCIsXCJUYWJsZUlEXCI6MSxcIklzUEtJRFwiOmZhbHNlLFwib3JkZXJpZFwiOjh9XSxcIm5ld0NvbHVtbnNcIjpbe1wiaWRcIjoxMyxcImNhcHRpb25cIjpcIuazqOWGjOaXtumXtFwiLFwiTmFtZVwiOlwiUmVnVGltZVwiLFwiSXNBdXRvSW5jcmVtZW50XCI6ZmFsc2UsXCJDYW5OdWxsXCI6dHJ1ZSxcImRiVHlwZVwiOlwiZGF0ZXRpbWVcIixcImxlbmd0aFwiOlwiXCIsXCJUYWJsZUlEXCI6MSxcIklzUEtJRFwiOmZhbHNlLFwib3JkZXJpZFwiOjJ9XSxcImNoYW5nZWRDb2x1bW5zXCI6W10sXCJkZWxldGVkQ29sdW1uc1wiOltdLFwiSURYQ29uZmlnc1wiOltdLFwiSURcIjowfSJ9LHsiTmFtZSI6ImRhdGFiYXNlaWQiLCJWYWx1ZSI6MX1dLCJSb3dTdGF0ZSI6MH0seyJJdGVtcyI6W3siTmFtZSI6ImlkIiwiVmFsdWUiOjd9LHsiTmFtZSI6InR5cGUiLCJWYWx1ZSI6IkNyZWF0ZVRhYmxlQWN0aW9uIn0seyJOYW1lIjoiY29udGVudCIsIlZhbHVlIjoie1wiVGFibGVcIjp7XCJpZFwiOjMsXCJjYXB0aW9uXCI6XCLnlKjmiLfnmoTmlYjmnpzlrZjmoaNcIixcIk5hbWVcIjpcIlVzZXJFZmZlY3RcIixcIkRhdGFiYXNlSURcIjoxLFwiaUxvY2tcIjowfSxcIkNvbHVtbnNcIjpbe1wiaWRcIjoxNCxcIk5hbWVcIjpcImlkXCIsXCJJc0F1dG9JbmNyZW1lbnRcIjp0cnVlLFwiQ2FuTnVsbFwiOmZhbHNlLFwiZGJUeXBlXCI6XCJpbnRcIixcIlRhYmxlSURcIjozLFwiSXNQS0lEXCI6dHJ1ZSxcIm9yZGVyaWRcIjowfSx7XCJpZFwiOjE1LFwiTmFtZVwiOlwiVXNlcklkXCIsXCJJc0F1dG9JbmNyZW1lbnRcIjpmYWxzZSxcIkNhbk51bGxcIjp0cnVlLFwiZGJUeXBlXCI6XCJpbnRcIixcImxlbmd0aFwiOlwiXCIsXCJUYWJsZUlEXCI6MyxcIklzUEtJRFwiOmZhbHNlLFwib3JkZXJpZFwiOjF9LHtcImlkXCI6MTYsXCJOYW1lXCI6XCJGaWxlTmFtZVwiLFwiSXNBdXRvSW5jcmVtZW50XCI6ZmFsc2UsXCJDYW5OdWxsXCI6dHJ1ZSxcImRiVHlwZVwiOlwidmFyY2hhclwiLFwibGVuZ3RoXCI6XCI1MFwiLFwiVGFibGVJRFwiOjMsXCJJc1BLSURcIjpmYWxzZSxcIm9yZGVyaWRcIjoyfSx7XCJpZFwiOjE3LFwiY2FwdGlvblwiOlwi57G75Z6LXCIsXCJOYW1lXCI6XCJUeXBlXCIsXCJJc0F1dG9JbmNyZW1lbnRcIjpmYWxzZSxcIkNhbk51bGxcIjp0cnVlLFwiZGJUeXBlXCI6XCJpbnRcIixcIkVudW1EZWZpbmVcIjpcIlByb2plY3Q9MSxcXG5UcmFjaz0yXCIsXCJsZW5ndGhcIjpcIlwiLFwiZGVmYXVsdFZhbHVlXCI6XCIxXCIsXCJUYWJsZUlEXCI6MyxcIklzUEtJRFwiOmZhbHNlLFwib3JkZXJpZFwiOjN9XSxcIklEWENvbmZpZ3NcIjpbXSxcIklEXCI6MH0ifSx7Ik5hbWUiOiJkYXRhYmFzZWlkIiwiVmFsdWUiOjF9XSwiUm93U3RhdGUiOjB9LHsiSXRlbXMiOlt7Ik5hbWUiOiJpZCIsIlZhbHVlIjo4fSx7Ik5hbWUiOiJ0eXBlIiwiVmFsdWUiOiJDaGFuZ2VUYWJsZUFjdGlvbiJ9LHsiTmFtZSI6ImNvbnRlbnQiLCJWYWx1ZSI6IntcIk9sZFRhYmxlTmFtZVwiOlwiVXNlckluZm9cIixcIk5ld1RhYmxlTmFtZVwiOlwiVXNlckluZm9cIixcIm90aGVyQ29sdW1uc1wiOlt7XCJpZFwiOjEsXCJOYW1lXCI6XCJpZFwiLFwiSXNBdXRvSW5jcmVtZW50XCI6dHJ1ZSxcIkNhbk51bGxcIjpmYWxzZSxcImRiVHlwZVwiOlwiaW50XCIsXCJUYWJsZUlEXCI6MSxcIklzUEtJRFwiOnRydWUsXCJvcmRlcmlkXCI6MH0se1wiaWRcIjoyLFwiY2FwdGlvblwiOlwi5rOo5YaM5ZSv5LiA6K6k6K+B56CBXCIsXCJOYW1lXCI6XCJSZWdHdWlkXCIsXCJJc0F1dG9JbmNyZW1lbnRcIjpmYWxzZSxcIkNhbk51bGxcIjp0cnVlLFwiZGJUeXBlXCI6XCJ2YXJjaGFyXCIsXCJsZW5ndGhcIjpcIjUwXCIsXCJUYWJsZUlEXCI6MSxcIklzUEtJRFwiOmZhbHNlLFwib3JkZXJpZFwiOjF9LHtcImlkXCI6MyxcIk5hbWVcIjpcIlVzZXJOYW1lXCIsXCJJc0F1dG9JbmNyZW1lbnRcIjpmYWxzZSxcIkNhbk51bGxcIjp0cnVlLFwiZGJUeXBlXCI6XCJ2YXJjaGFyXCIsXCJsZW5ndGhcIjpcIjUwXCIsXCJUYWJsZUlEXCI6MSxcIklzUEtJRFwiOmZhbHNlLFwib3JkZXJpZFwiOjN9LHtcImlkXCI6NCxcIk5hbWVcIjpcIlBhc3N3b3JkXCIsXCJJc0F1dG9JbmNyZW1lbnRcIjpmYWxzZSxcIkNhbk51bGxcIjp0cnVlLFwiZGJUeXBlXCI6XCJ2YXJjaGFyXCIsXCJsZW5ndGhcIjpcIjEwMFwiLFwiVGFibGVJRFwiOjEsXCJJc1BLSURcIjpmYWxzZSxcIm9yZGVyaWRcIjo0fSx7XCJpZFwiOjUsXCJjYXB0aW9uXCI6XCLmiYvmnLrlj7dcIixcIk5hbWVcIjpcIlBob25lTnVtYmVyXCIsXCJJc0F1dG9JbmNyZW1lbnRcIjpmYWxzZSxcIkNhbk51bGxcIjp0cnVlLFwiZGJUeXBlXCI6XCJ2YXJjaGFyXCIsXCJsZW5ndGhcIjpcIjUwXCIsXCJUYWJsZUlEXCI6MSxcIklzUEtJRFwiOmZhbHNlLFwib3JkZXJpZFwiOjV9LHtcImlkXCI6NixcIk5hbWVcIjpcIkVtYWlsXCIsXCJJc0F1dG9JbmNyZW1lbnRcIjpmYWxzZSxcIkNhbk51bGxcIjp0cnVlLFwiZGJUeXBlXCI6XCJ2YXJjaGFyXCIsXCJsZW5ndGhcIjpcIjUwXCIsXCJUYWJsZUlEXCI6MSxcIklzUEtJRFwiOmZhbHNlLFwib3JkZXJpZFwiOjZ9LHtcImlkXCI6NyxcImNhcHRpb25cIjpcIuW5s+WPsFwiLFwiTmFtZVwiOlwiUGxhdGZvcm1cIixcIklzQXV0b0luY3JlbWVudFwiOmZhbHNlLFwiQ2FuTnVsbFwiOnRydWUsXCJkYlR5cGVcIjpcInZhcmNoYXJcIixcImxlbmd0aFwiOlwiNTBcIixcIlRhYmxlSURcIjoxLFwiSXNQS0lEXCI6ZmFsc2UsXCJvcmRlcmlkXCI6N30se1wiaWRcIjo4LFwiY2FwdGlvblwiOlwi5piv5ZCm5bey57uP5pSv5LuY57uZSmFja1wiLFwiTmFtZVwiOlwiSXNQYXlcIixcIklzQXV0b0luY3JlbWVudFwiOmZhbHNlLFwiQ2FuTnVsbFwiOnRydWUsXCJkYlR5cGVcIjpcImJpdFwiLFwibGVuZ3RoXCI6XCJcIixcImRlZmF1bHRWYWx1ZVwiOlwiMFwiLFwiVGFibGVJRFwiOjEsXCJJc1BLSURcIjpmYWxzZSxcIm9yZGVyaWRcIjo4fSx7XCJpZFwiOjEyLFwiY2FwdGlvblwiOlwi55m76ZmG5ZCO55Sf5oiQ55qE5ZSv5LiA56CBXCIsXCJOYW1lXCI6XCJMb2dpbkNvZGVcIixcIklzQXV0b0luY3JlbWVudFwiOmZhbHNlLFwiQ2FuTnVsbFwiOnRydWUsXCJkYlR5cGVcIjpcInZhcmNoYXJcIixcImxlbmd0aFwiOlwiNTBcIixcIlRhYmxlSURcIjoxLFwiSXNQS0lEXCI6ZmFsc2UsXCJvcmRlcmlkXCI6OX0se1wiaWRcIjoxMyxcImNhcHRpb25cIjpcIuazqOWGjOaXtumXtFwiLFwiTmFtZVwiOlwiUmVnVGltZVwiLFwiSXNBdXRvSW5jcmVtZW50XCI6ZmFsc2UsXCJDYW5OdWxsXCI6dHJ1ZSxcImRiVHlwZVwiOlwiZGF0ZXRpbWVcIixcImxlbmd0aFwiOlwiXCIsXCJUYWJsZUlEXCI6MSxcIklzUEtJRFwiOmZhbHNlLFwib3JkZXJpZFwiOjJ9XSxcIm5ld0NvbHVtbnNcIjpbXSxcImNoYW5nZWRDb2x1bW5zXCI6W10sXCJkZWxldGVkQ29sdW1uc1wiOltdLFwiSURYQ29uZmlnc1wiOltdLFwiSURcIjowfSJ9LHsiTmFtZSI6ImRhdGFiYXNlaWQiLCJWYWx1ZSI6MX1dLCJSb3dTdGF0ZSI6MH0seyJJdGVtcyI6W3siTmFtZSI6ImlkIiwiVmFsdWUiOjl9LHsiTmFtZSI6InR5cGUiLCJWYWx1ZSI6IkNoYW5nZVRhYmxlQWN0aW9uIn0seyJOYW1lIjoiY29udGVudCIsIlZhbHVlIjoie1wiT2xkVGFibGVOYW1lXCI6XCJVc2VyRWZmZWN0XCIsXCJOZXdUYWJsZU5hbWVcIjpcIlVzZXJFZmZlY3RcIixcIm90aGVyQ29sdW1uc1wiOlt7XCJpZFwiOjE0LFwiTmFtZVwiOlwiaWRcIixcIklzQXV0b0luY3JlbWVudFwiOnRydWUsXCJDYW5OdWxsXCI6ZmFsc2UsXCJkYlR5cGVcIjpcImludFwiLFwiVGFibGVJRFwiOjMsXCJJc1BLSURcIjp0cnVlLFwib3JkZXJpZFwiOjB9LHtcImlkXCI6MTUsXCJOYW1lXCI6XCJVc2VySWRcIixcIklzQXV0b0luY3JlbWVudFwiOmZhbHNlLFwiQ2FuTnVsbFwiOnRydWUsXCJkYlR5cGVcIjpcImludFwiLFwibGVuZ3RoXCI6XCJcIixcIlRhYmxlSURcIjozLFwiSXNQS0lEXCI6ZmFsc2UsXCJvcmRlcmlkXCI6MX0se1wiaWRcIjoxNixcIk5hbWVcIjpcIkZpbGVOYW1lXCIsXCJJc0F1dG9JbmNyZW1lbnRcIjpmYWxzZSxcIkNhbk51bGxcIjp0cnVlLFwiZGJUeXBlXCI6XCJ2YXJjaGFyXCIsXCJsZW5ndGhcIjpcIjUwXCIsXCJUYWJsZUlEXCI6MyxcIklzUEtJRFwiOmZhbHNlLFwib3JkZXJpZFwiOjJ9LHtcImlkXCI6MTcsXCJjYXB0aW9uXCI6XCLnsbvlnotcIixcIk5hbWVcIjpcIlR5cGVcIixcIklzQXV0b0luY3JlbWVudFwiOmZhbHNlLFwiQ2FuTnVsbFwiOnRydWUsXCJkYlR5cGVcIjpcImludFwiLFwiRW51bURlZmluZVwiOlwiUHJvamVjdD0xLFxcblRyYWNrPTJcIixcImxlbmd0aFwiOlwiXCIsXCJkZWZhdWx0VmFsdWVcIjpcIjFcIixcIlRhYmxlSURcIjozLFwiSXNQS0lEXCI6ZmFsc2UsXCJvcmRlcmlkXCI6M31dLFwibmV3Q29sdW1uc1wiOlt7XCJpZFwiOjE4LFwiTmFtZVwiOlwiTmFtZVwiLFwiSXNBdXRvSW5jcmVtZW50XCI6ZmFsc2UsXCJDYW5OdWxsXCI6dHJ1ZSxcImRiVHlwZVwiOlwidmFyY2hhclwiLFwibGVuZ3RoXCI6XCI1MFwiLFwiVGFibGVJRFwiOjMsXCJJc1BLSURcIjpmYWxzZSxcIm9yZGVyaWRcIjoyfV0sXCJjaGFuZ2VkQ29sdW1uc1wiOltdLFwiZGVsZXRlZENvbHVtbnNcIjpbXSxcIklEWENvbmZpZ3NcIjpbXSxcIklEXCI6MH0ifSx7Ik5hbWUiOiJkYXRhYmFzZWlkIiwiVmFsdWUiOjF9XSwiUm93U3RhdGUiOjB9LHsiSXRlbXMiOlt7Ik5hbWUiOiJpZCIsIlZhbHVlIjoxMH0seyJOYW1lIjoidHlwZSIsIlZhbHVlIjoiQ2hhbmdlVGFibGVBY3Rpb24ifSx7Ik5hbWUiOiJjb250ZW50IiwiVmFsdWUiOiJ7XCJPbGRUYWJsZU5hbWVcIjpcIlVzZXJFZmZlY3RcIixcIk5ld1RhYmxlTmFtZVwiOlwiVXNlckVmZmVjdFwiLFwib3RoZXJDb2x1bW5zXCI6W3tcImlkXCI6MTQsXCJOYW1lXCI6XCJpZFwiLFwiSXNBdXRvSW5jcmVtZW50XCI6dHJ1ZSxcIkNhbk51bGxcIjpmYWxzZSxcImRiVHlwZVwiOlwiaW50XCIsXCJUYWJsZUlEXCI6MyxcIklzUEtJRFwiOnRydWUsXCJvcmRlcmlkXCI6MH0se1wiaWRcIjoxNSxcIk5hbWVcIjpcIlVzZXJJZFwiLFwiSXNBdXRvSW5jcmVtZW50XCI6ZmFsc2UsXCJDYW5OdWxsXCI6dHJ1ZSxcImRiVHlwZVwiOlwiaW50XCIsXCJsZW5ndGhcIjpcIlwiLFwiVGFibGVJRFwiOjMsXCJJc1BLSURcIjpmYWxzZSxcIm9yZGVyaWRcIjoxfSx7XCJpZFwiOjE2LFwiTmFtZVwiOlwiRmlsZU5hbWVcIixcIklzQXV0b0luY3JlbWVudFwiOmZhbHNlLFwiQ2FuTnVsbFwiOnRydWUsXCJkYlR5cGVcIjpcInZhcmNoYXJcIixcImxlbmd0aFwiOlwiNTBcIixcIlRhYmxlSURcIjozLFwiSXNQS0lEXCI6ZmFsc2UsXCJvcmRlcmlkXCI6M30se1wiaWRcIjoxNyxcImNhcHRpb25cIjpcIuexu+Wei1wiLFwiTmFtZVwiOlwiVHlwZVwiLFwiSXNBdXRvSW5jcmVtZW50XCI6ZmFsc2UsXCJDYW5OdWxsXCI6dHJ1ZSxcImRiVHlwZVwiOlwiaW50XCIsXCJFbnVtRGVmaW5lXCI6XCJQcm9qZWN0PTEsXFxuVHJhY2s9MlwiLFwibGVuZ3RoXCI6XCJcIixcImRlZmF1bHRWYWx1ZVwiOlwiMVwiLFwiVGFibGVJRFwiOjMsXCJJc1BLSURcIjpmYWxzZSxcIm9yZGVyaWRcIjo0fSx7XCJpZFwiOjE4LFwiTmFtZVwiOlwiTmFtZVwiLFwiSXNBdXRvSW5jcmVtZW50XCI6ZmFsc2UsXCJDYW5OdWxsXCI6dHJ1ZSxcImRiVHlwZVwiOlwidmFyY2hhclwiLFwibGVuZ3RoXCI6XCI1MFwiLFwiVGFibGVJRFwiOjMsXCJJc1BLSURcIjpmYWxzZSxcIm9yZGVyaWRcIjoyfV0sXCJuZXdDb2x1bW5zXCI6W3tcImlkXCI6MTksXCJOYW1lXCI6XCJWZXJzaW9uXCIsXCJJc0F1dG9JbmNyZW1lbnRcIjpmYWxzZSxcIkNhbk51bGxcIjp0cnVlLFwiZGJUeXBlXCI6XCJpbnRcIixcImxlbmd0aFwiOlwiXCIsXCJkZWZhdWx0VmFsdWVcIjpcIjBcIixcIlRhYmxlSURcIjozLFwiSXNQS0lEXCI6ZmFsc2UsXCJvcmRlcmlkXCI6NX1dLFwiY2hhbmdlZENvbHVtbnNcIjpbXSxcImRlbGV0ZWRDb2x1bW5zXCI6W10sXCJJRFhDb25maWdzXCI6W10sXCJJRFwiOjB9In0seyJOYW1lIjoiZGF0YWJhc2VpZCIsIlZhbHVlIjoxfV0sIlJvd1N0YXRlIjowfSx7Ikl0ZW1zIjpbeyJOYW1lIjoiaWQiLCJWYWx1ZSI6MTF9LHsiTmFtZSI6InR5cGUiLCJWYWx1ZSI6IkNyZWF0ZVRhYmxlQWN0aW9uIn0seyJOYW1lIjoiY29udGVudCIsIlZhbHVlIjoie1wiVGFibGVcIjp7XCJpZFwiOjQsXCJOYW1lXCI6XCJBZG1pblVzZXJcIixcIkRhdGFiYXNlSURcIjoxLFwiaUxvY2tcIjowfSxcIkNvbHVtbnNcIjpbe1wiaWRcIjoyMCxcIk5hbWVcIjpcImlkXCIsXCJJc0F1dG9JbmNyZW1lbnRcIjp0cnVlLFwiQ2FuTnVsbFwiOmZhbHNlLFwiZGJUeXBlXCI6XCJpbnRcIixcIlRhYmxlSURcIjo0LFwiSXNQS0lEXCI6dHJ1ZSxcIm9yZGVyaWRcIjowfSx7XCJpZFwiOjIxLFwiTmFtZVwiOlwiVXNlck5hbWVcIixcIklzQXV0b0luY3JlbWVudFwiOmZhbHNlLFwiQ2FuTnVsbFwiOnRydWUsXCJkYlR5cGVcIjpcInZhcmNoYXJcIixcImxlbmd0aFwiOlwiNTBcIixcIlRhYmxlSURcIjo0LFwiSXNQS0lEXCI6ZmFsc2UsXCJvcmRlcmlkXCI6MX0se1wiaWRcIjoyMixcIk5hbWVcIjpcIlBhc3N3b3JkXCIsXCJJc0F1dG9JbmNyZW1lbnRcIjpmYWxzZSxcIkNhbk51bGxcIjp0cnVlLFwiZGJUeXBlXCI6XCJ2YXJjaGFyXCIsXCJsZW5ndGhcIjpcIjUwXCIsXCJUYWJsZUlEXCI6NCxcIklzUEtJRFwiOmZhbHNlLFwib3JkZXJpZFwiOjJ9LHtcImlkXCI6MjMsXCJOYW1lXCI6XCJSb2xlXCIsXCJJc0F1dG9JbmNyZW1lbnRcIjpmYWxzZSxcIkNhbk51bGxcIjp0cnVlLFwiZGJUeXBlXCI6XCJpbnRcIixcIkVudW1EZWZpbmVcIjpcIui2hee6p+euoeeQhuWRmCA9IDE8PDAsXFxu5pmu6YCa5ZGY5belID0gMTw8MVxcblwiLFwibGVuZ3RoXCI6XCJcIixcIlRhYmxlSURcIjo0LFwiSXNQS0lEXCI6ZmFsc2UsXCJvcmRlcmlkXCI6M31dLFwiSURYQ29uZmlnc1wiOltdLFwiSURcIjowfSJ9LHsiTmFtZSI6ImRhdGFiYXNlaWQiLCJWYWx1ZSI6MX1dLCJSb3dTdGF0ZSI6MH0seyJJdGVtcyI6W3siTmFtZSI6ImlkIiwiVmFsdWUiOjEyfSx7Ik5hbWUiOiJ0eXBlIiwiVmFsdWUiOiJDaGFuZ2VUYWJsZUFjdGlvbiJ9LHsiTmFtZSI6ImNvbnRlbnQiLCJWYWx1ZSI6IntcIk9sZFRhYmxlTmFtZVwiOlwiU3lzdGVtUmVnQ29kZVwiLFwiTmV3VGFibGVOYW1lXCI6XCJTeXN0ZW1SZWdDb2RlXCIsXCJvdGhlckNvbHVtbnNcIjpbe1wiaWRcIjo5LFwiTmFtZVwiOlwiaWRcIixcIklzQXV0b0luY3JlbWVudFwiOnRydWUsXCJDYW5OdWxsXCI6ZmFsc2UsXCJkYlR5cGVcIjpcImludFwiLFwiVGFibGVJRFwiOjIsXCJJc1BLSURcIjp0cnVlLFwib3JkZXJpZFwiOjB9LHtcImlkXCI6MTAsXCJjYXB0aW9uXCI6XCLllK/kuIDorqTor4HnoIFcIixcIk5hbWVcIjpcIlJlZ0d1aWRcIixcIklzQXV0b0luY3JlbWVudFwiOmZhbHNlLFwiQ2FuTnVsbFwiOnRydWUsXCJkYlR5cGVcIjpcInZhcmNoYXJcIixcImxlbmd0aFwiOlwiNTBcIixcIlRhYmxlSURcIjoyLFwiSXNQS0lEXCI6ZmFsc2UsXCJvcmRlcmlkXCI6MX0se1wiaWRcIjoxMSxcImNhcHRpb25cIjpcIuS9v+eUqOeahOeUqOaIt2lkXCIsXCJOYW1lXCI6XCJVc2VySWRcIixcIklzQXV0b0luY3JlbWVudFwiOmZhbHNlLFwiQ2FuTnVsbFwiOnRydWUsXCJkYlR5cGVcIjpcImludFwiLFwibGVuZ3RoXCI6XCJcIixcIlRhYmxlSURcIjoyLFwiSXNQS0lEXCI6ZmFsc2UsXCJvcmRlcmlkXCI6Mn1dLFwibmV3Q29sdW1uc1wiOlt7XCJpZFwiOjI0LFwiY2FwdGlvblwiOlwi6LCB55Sf5oiQ55qEXCIsXCJOYW1lXCI6XCJNYWtlclVzZXJJZFwiLFwiSXNBdXRvSW5jcmVtZW50XCI6ZmFsc2UsXCJDYW5OdWxsXCI6dHJ1ZSxcImRiVHlwZVwiOlwiaW50XCIsXCJsZW5ndGhcIjpcIlwiLFwiVGFibGVJRFwiOjIsXCJJc1BLSURcIjpmYWxzZSxcIm9yZGVyaWRcIjozfV0sXCJjaGFuZ2VkQ29sdW1uc1wiOltdLFwiZGVsZXRlZENvbHVtbnNcIjpbXSxcIklEWENvbmZpZ3NcIjpbXSxcIklEXCI6MH0ifSx7Ik5hbWUiOiJkYXRhYmFzZWlkIiwiVmFsdWUiOjF9XSwiUm93U3RhdGUiOjB9LHsiSXRlbXMiOlt7Ik5hbWUiOiJpZCIsIlZhbHVlIjoxM30seyJOYW1lIjoidHlwZSIsIlZhbHVlIjoiQ2hhbmdlVGFibGVBY3Rpb24ifSx7Ik5hbWUiOiJjb250ZW50IiwiVmFsdWUiOiJ7XCJPbGRUYWJsZU5hbWVcIjpcIlVzZXJJbmZvXCIsXCJOZXdUYWJsZU5hbWVcIjpcIlVzZXJJbmZvXCIsXCJvdGhlckNvbHVtbnNcIjpbe1wiaWRcIjoxLFwiTmFtZVwiOlwiaWRcIixcIklzQXV0b0luY3JlbWVudFwiOnRydWUsXCJDYW5OdWxsXCI6ZmFsc2UsXCJkYlR5cGVcIjpcImludFwiLFwiVGFibGVJRFwiOjEsXCJJc1BLSURcIjp0cnVlLFwib3JkZXJpZFwiOjB9LHtcImlkXCI6MixcImNhcHRpb25cIjpcIuazqOWGjOWUr+S4gOiupOivgeeggVwiLFwiTmFtZVwiOlwiUmVnR3VpZFwiLFwiSXNBdXRvSW5jcmVtZW50XCI6ZmFsc2UsXCJDYW5OdWxsXCI6dHJ1ZSxcImRiVHlwZVwiOlwidmFyY2hhclwiLFwibGVuZ3RoXCI6XCI1MFwiLFwiVGFibGVJRFwiOjEsXCJJc1BLSURcIjpmYWxzZSxcIm9yZGVyaWRcIjoxfSx7XCJpZFwiOjMsXCJOYW1lXCI6XCJVc2VyTmFtZVwiLFwiSXNBdXRvSW5jcmVtZW50XCI6ZmFsc2UsXCJDYW5OdWxsXCI6dHJ1ZSxcImRiVHlwZVwiOlwidmFyY2hhclwiLFwibGVuZ3RoXCI6XCI1MFwiLFwiVGFibGVJRFwiOjEsXCJJc1BLSURcIjpmYWxzZSxcIm9yZGVyaWRcIjozfSx7XCJpZFwiOjQsXCJOYW1lXCI6XCJQYXNzd29yZFwiLFwiSXNBdXRvSW5jcmVtZW50XCI6ZmFsc2UsXCJDYW5OdWxsXCI6dHJ1ZSxcImRiVHlwZVwiOlwidmFyY2hhclwiLFwibGVuZ3RoXCI6XCIxMDBcIixcIlRhYmxlSURcIjoxLFwiSXNQS0lEXCI6ZmFsc2UsXCJvcmRlcmlkXCI6NH0se1wiaWRcIjo1LFwiY2FwdGlvblwiOlwi5omL5py65Y+3XCIsXCJOYW1lXCI6XCJQaG9uZU51bWJlclwiLFwiSXNBdXRvSW5jcmVtZW50XCI6ZmFsc2UsXCJDYW5OdWxsXCI6dHJ1ZSxcImRiVHlwZVwiOlwidmFyY2hhclwiLFwibGVuZ3RoXCI6XCI1MFwiLFwiVGFibGVJRFwiOjEsXCJJc1BLSURcIjpmYWxzZSxcIm9yZGVyaWRcIjo1fSx7XCJpZFwiOjYsXCJOYW1lXCI6XCJFbWFpbFwiLFwiSXNBdXRvSW5jcmVtZW50XCI6ZmFsc2UsXCJDYW5OdWxsXCI6dHJ1ZSxcImRiVHlwZVwiOlwidmFyY2hhclwiLFwibGVuZ3RoXCI6XCI1MFwiLFwiVGFibGVJRFwiOjEsXCJJc1BLSURcIjpmYWxzZSxcIm9yZGVyaWRcIjo2fSx7XCJpZFwiOjcsXCJjYXB0aW9uXCI6XCLlubPlj7BcIixcIk5hbWVcIjpcIlBsYXRmb3JtXCIsXCJJc0F1dG9JbmNyZW1lbnRcIjpmYWxzZSxcIkNhbk51bGxcIjp0cnVlLFwiZGJUeXBlXCI6XCJ2YXJjaGFyXCIsXCJsZW5ndGhcIjpcIjUwXCIsXCJUYWJsZUlEXCI6MSxcIklzUEtJRFwiOmZhbHNlLFwib3JkZXJpZFwiOjd9LHtcImlkXCI6OCxcImNhcHRpb25cIjpcIuaYr+WQpuW3sue7j+aUr+S7mOe7mUphY2tcIixcIk5hbWVcIjpcIklzUGF5XCIsXCJJc0F1dG9JbmNyZW1lbnRcIjpmYWxzZSxcIkNhbk51bGxcIjp0cnVlLFwiZGJUeXBlXCI6XCJiaXRcIixcImxlbmd0aFwiOlwiXCIsXCJkZWZhdWx0VmFsdWVcIjpcIjBcIixcIlRhYmxlSURcIjoxLFwiSXNQS0lEXCI6ZmFsc2UsXCJvcmRlcmlkXCI6OH0se1wiaWRcIjoxMixcImNhcHRpb25cIjpcIueZu+mZhuWQjueUn+aIkOeahOWUr+S4gOeggVwiLFwiTmFtZVwiOlwiTG9naW5Db2RlXCIsXCJJc0F1dG9JbmNyZW1lbnRcIjpmYWxzZSxcIkNhbk51bGxcIjp0cnVlLFwiZGJUeXBlXCI6XCJ2YXJjaGFyXCIsXCJsZW5ndGhcIjpcIjUwXCIsXCJUYWJsZUlEXCI6MSxcIklzUEtJRFwiOmZhbHNlLFwib3JkZXJpZFwiOjl9LHtcImlkXCI6MTMsXCJjYXB0aW9uXCI6XCLms6jlhozml7bpl7RcIixcIk5hbWVcIjpcIlJlZ1RpbWVcIixcIklzQXV0b0luY3JlbWVudFwiOmZhbHNlLFwiQ2FuTnVsbFwiOnRydWUsXCJkYlR5cGVcIjpcImRhdGV0aW1lXCIsXCJsZW5ndGhcIjpcIlwiLFwiVGFibGVJRFwiOjEsXCJJc1BLSURcIjpmYWxzZSxcIm9yZGVyaWRcIjoyfV0sXCJuZXdDb2x1bW5zXCI6W3tcImlkXCI6MjUsXCJjYXB0aW9uXCI6XCLmnIDlkI7kuIDmrKHosIPnlKhDaGVja1VzZXLml7bpl7RcIixcIk5hbWVcIjpcIkxhc3RDaGVja1RpbWVcIixcIklzQXV0b0luY3JlbWVudFwiOmZhbHNlLFwiQ2FuTnVsbFwiOnRydWUsXCJkYlR5cGVcIjpcImRhdGV0aW1lXCIsXCJsZW5ndGhcIjpcIlwiLFwiVGFibGVJRFwiOjEsXCJJc1BLSURcIjpmYWxzZSxcIm9yZGVyaWRcIjoxMH1dLFwiY2hhbmdlZENvbHVtbnNcIjpbXSxcImRlbGV0ZWRDb2x1bW5zXCI6W10sXCJJRFhDb25maWdzXCI6W10sXCJJRFwiOjB9In0seyJOYW1lIjoiZGF0YWJhc2VpZCIsIlZhbHVlIjoxfV0sIlJvd1N0YXRlIjowfSx7Ikl0ZW1zIjpbeyJOYW1lIjoiaWQiLCJWYWx1ZSI6MTR9LHsiTmFtZSI6InR5cGUiLCJWYWx1ZSI6IkNoYW5nZVRhYmxlQWN0aW9uIn0seyJOYW1lIjoiY29udGVudCIsIlZhbHVlIjoie1wiT2xkVGFibGVOYW1lXCI6XCJTeXN0ZW1SZWdDb2RlXCIsXCJOZXdUYWJsZU5hbWVcIjpcIlN5c3RlbVJlZ0NvZGVcIixcIm90aGVyQ29sdW1uc1wiOlt7XCJpZFwiOjksXCJOYW1lXCI6XCJpZFwiLFwiSXNBdXRvSW5jcmVtZW50XCI6dHJ1ZSxcIkNhbk51bGxcIjpmYWxzZSxcImRiVHlwZVwiOlwiaW50XCIsXCJUYWJsZUlEXCI6MixcIklzUEtJRFwiOnRydWUsXCJvcmRlcmlkXCI6MH0se1wiaWRcIjoxMCxcImNhcHRpb25cIjpcIuWUr+S4gOiupOivgeeggVwiLFwiTmFtZVwiOlwiUmVnR3VpZFwiLFwiSXNBdXRvSW5jcmVtZW50XCI6ZmFsc2UsXCJDYW5OdWxsXCI6dHJ1ZSxcImRiVHlwZVwiOlwidmFyY2hhclwiLFwibGVuZ3RoXCI6XCI1MFwiLFwiVGFibGVJRFwiOjIsXCJJc1BLSURcIjpmYWxzZSxcIm9yZGVyaWRcIjoxfSx7XCJpZFwiOjExLFwiY2FwdGlvblwiOlwi5L2/55So55qE55So5oi3aWRcIixcIk5hbWVcIjpcIlVzZXJJZFwiLFwiSXNBdXRvSW5jcmVtZW50XCI6ZmFsc2UsXCJDYW5OdWxsXCI6dHJ1ZSxcImRiVHlwZVwiOlwiaW50XCIsXCJsZW5ndGhcIjpcIlwiLFwiVGFibGVJRFwiOjIsXCJJc1BLSURcIjpmYWxzZSxcIm9yZGVyaWRcIjoyfSx7XCJpZFwiOjI0LFwiY2FwdGlvblwiOlwi6LCB55Sf5oiQ55qEXCIsXCJOYW1lXCI6XCJNYWtlclVzZXJJZFwiLFwiSXNBdXRvSW5jcmVtZW50XCI6ZmFsc2UsXCJDYW5OdWxsXCI6dHJ1ZSxcImRiVHlwZVwiOlwiaW50XCIsXCJsZW5ndGhcIjpcIlwiLFwiVGFibGVJRFwiOjIsXCJJc1BLSURcIjpmYWxzZSxcIm9yZGVyaWRcIjozfV0sXCJuZXdDb2x1bW5zXCI6W3tcImlkXCI6MjYsXCJjYXB0aW9uXCI6XCLnlJ/miJDml7bpl7RcIixcIk5hbWVcIjpcIk1ha2VUaW1lXCIsXCJJc0F1dG9JbmNyZW1lbnRcIjpmYWxzZSxcIkNhbk51bGxcIjp0cnVlLFwiZGJUeXBlXCI6XCJkYXRldGltZVwiLFwibGVuZ3RoXCI6XCJcIixcIlRhYmxlSURcIjoyLFwiSXNQS0lEXCI6ZmFsc2UsXCJvcmRlcmlkXCI6NH1dLFwiY2hhbmdlZENvbHVtbnNcIjpbXSxcImRlbGV0ZWRDb2x1bW5zXCI6W10sXCJJRFhDb25maWdzXCI6W10sXCJJRFwiOjB9In0seyJOYW1lIjoiZGF0YWJhc2VpZCIsIlZhbHVlIjoxfV0sIlJvd1N0YXRlIjowfSx7Ikl0ZW1zIjpbeyJOYW1lIjoiaWQiLCJWYWx1ZSI6MTV9LHsiTmFtZSI6InR5cGUiLCJWYWx1ZSI6IkNoYW5nZVRhYmxlQWN0aW9uIn0seyJOYW1lIjoiY29udGVudCIsIlZhbHVlIjoie1wiT2xkVGFibGVOYW1lXCI6XCJBZG1pblVzZXJcIixcIk5ld1RhYmxlTmFtZVwiOlwiQWRtaW5Vc2VyXCIsXCJvdGhlckNvbHVtbnNcIjpbe1wiaWRcIjoyMCxcIk5hbWVcIjpcImlkXCIsXCJJc0F1dG9JbmNyZW1lbnRcIjp0cnVlLFwiQ2FuTnVsbFwiOmZhbHNlLFwiZGJUeXBlXCI6XCJpbnRcIixcIlRhYmxlSURcIjo0LFwiSXNQS0lEXCI6dHJ1ZSxcIm9yZGVyaWRcIjowfSx7XCJpZFwiOjIxLFwiTmFtZVwiOlwiVXNlck5hbWVcIixcIklzQXV0b0luY3JlbWVudFwiOmZhbHNlLFwiQ2FuTnVsbFwiOnRydWUsXCJkYlR5cGVcIjpcInZhcmNoYXJcIixcImxlbmd0aFwiOlwiNTBcIixcIlRhYmxlSURcIjo0LFwiSXNQS0lEXCI6ZmFsc2UsXCJvcmRlcmlkXCI6MX0se1wiaWRcIjoyMixcIk5hbWVcIjpcIlBhc3N3b3JkXCIsXCJJc0F1dG9JbmNyZW1lbnRcIjpmYWxzZSxcIkNhbk51bGxcIjp0cnVlLFwiZGJUeXBlXCI6XCJ2YXJjaGFyXCIsXCJsZW5ndGhcIjpcIjUwXCIsXCJUYWJsZUlEXCI6NCxcIklzUEtJRFwiOmZhbHNlLFwib3JkZXJpZFwiOjJ9LHtcImlkXCI6MjMsXCJOYW1lXCI6XCJSb2xlXCIsXCJJc0F1dG9JbmNyZW1lbnRcIjpmYWxzZSxcIkNhbk51bGxcIjp0cnVlLFwiZGJUeXBlXCI6XCJpbnRcIixcIkVudW1EZWZpbmVcIjpcIui2hee6p+euoeeQhuWRmCA9IDE8PDAsXFxu5pmu6YCa5ZGY5belID0gMTw8MVxcblwiLFwibGVuZ3RoXCI6XCJcIixcIlRhYmxlSURcIjo0LFwiSXNQS0lEXCI6ZmFsc2UsXCJvcmRlcmlkXCI6M31dLFwibmV3Q29sdW1uc1wiOlt7XCJpZFwiOjI3LFwiY2FwdGlvblwiOlwi5omL5py65Y+3XCIsXCJOYW1lXCI6XCJQaG9uZU51bWJlclwiLFwiSXNBdXRvSW5jcmVtZW50XCI6ZmFsc2UsXCJDYW5OdWxsXCI6dHJ1ZSxcImRiVHlwZVwiOlwidmFyY2hhclwiLFwibGVuZ3RoXCI6XCI1MFwiLFwiVGFibGVJRFwiOjQsXCJJc1BLSURcIjpmYWxzZSxcIm9yZGVyaWRcIjo0fV0sXCJjaGFuZ2VkQ29sdW1uc1wiOltdLFwiZGVsZXRlZENvbHVtbnNcIjpbXSxcIklEWENvbmZpZ3NcIjpbXSxcIklEXCI6MH0ifSx7Ik5hbWUiOiJkYXRhYmFzZWlkIiwiVmFsdWUiOjF9XSwiUm93U3RhdGUiOjB9LHsiSXRlbXMiOlt7Ik5hbWUiOiJpZCIsIlZhbHVlIjoxNn0seyJOYW1lIjoidHlwZSIsIlZhbHVlIjoiQ2hhbmdlVGFibGVBY3Rpb24ifSx7Ik5hbWUiOiJjb250ZW50IiwiVmFsdWUiOiJ7XCJPbGRUYWJsZU5hbWVcIjpcIlVzZXJJbmZvXCIsXCJOZXdUYWJsZU5hbWVcIjpcIlVzZXJJbmZvXCIsXCJvdGhlckNvbHVtbnNcIjpbe1wiaWRcIjoxLFwiTmFtZVwiOlwiaWRcIixcIklzQXV0b0luY3JlbWVudFwiOnRydWUsXCJDYW5OdWxsXCI6ZmFsc2UsXCJkYlR5cGVcIjpcImludFwiLFwiVGFibGVJRFwiOjEsXCJJc1BLSURcIjp0cnVlLFwib3JkZXJpZFwiOjB9LHtcImlkXCI6MixcImNhcHRpb25cIjpcIuazqOWGjOWUr+S4gOiupOivgeeggVwiLFwiTmFtZVwiOlwiUmVnR3VpZFwiLFwiSXNBdXRvSW5jcmVtZW50XCI6ZmFsc2UsXCJDYW5OdWxsXCI6dHJ1ZSxcImRiVHlwZVwiOlwidmFyY2hhclwiLFwibGVuZ3RoXCI6XCI1MFwiLFwiVGFibGVJRFwiOjEsXCJJc1BLSURcIjpmYWxzZSxcIm9yZGVyaWRcIjoxfSx7XCJpZFwiOjMsXCJOYW1lXCI6XCJVc2VyTmFtZVwiLFwiSXNBdXRvSW5jcmVtZW50XCI6ZmFsc2UsXCJDYW5OdWxsXCI6dHJ1ZSxcImRiVHlwZVwiOlwidmFyY2hhclwiLFwibGVuZ3RoXCI6XCI1MFwiLFwiVGFibGVJRFwiOjEsXCJJc1BLSURcIjpmYWxzZSxcIm9yZGVyaWRcIjozfSx7XCJpZFwiOjQsXCJOYW1lXCI6XCJQYXNzd29yZFwiLFwiSXNBdXRvSW5jcmVtZW50XCI6ZmFsc2UsXCJDYW5OdWxsXCI6dHJ1ZSxcImRiVHlwZVwiOlwidmFyY2hhclwiLFwibGVuZ3RoXCI6XCIxMDBcIixcIlRhYmxlSURcIjoxLFwiSXNQS0lEXCI6ZmFsc2UsXCJvcmRlcmlkXCI6NH0se1wiaWRcIjo1LFwiY2FwdGlvblwiOlwi5omL5py65Y+3XCIsXCJOYW1lXCI6XCJQaG9uZU51bWJlclwiLFwiSXNBdXRvSW5jcmVtZW50XCI6ZmFsc2UsXCJDYW5OdWxsXCI6dHJ1ZSxcImRiVHlwZVwiOlwidmFyY2hhclwiLFwibGVuZ3RoXCI6XCI1MFwiLFwiVGFibGVJRFwiOjEsXCJJc1BLSURcIjpmYWxzZSxcIm9yZGVyaWRcIjo1fSx7XCJpZFwiOjYsXCJOYW1lXCI6XCJFbWFpbFwiLFwiSXNBdXRvSW5jcmVtZW50XCI6ZmFsc2UsXCJDYW5OdWxsXCI6dHJ1ZSxcImRiVHlwZVwiOlwidmFyY2hhclwiLFwibGVuZ3RoXCI6XCI1MFwiLFwiVGFibGVJRFwiOjEsXCJJc1BLSURcIjpmYWxzZSxcIm9yZGVyaWRcIjo2fSx7XCJpZFwiOjcsXCJjYXB0aW9uXCI6XCLlubPlj7BcIixcIk5hbWVcIjpcIlBsYXRmb3JtXCIsXCJJc0F1dG9JbmNyZW1lbnRcIjpmYWxzZSxcIkNhbk51bGxcIjp0cnVlLFwiZGJUeXBlXCI6XCJ2YXJjaGFyXCIsXCJsZW5ndGhcIjpcIjUwXCIsXCJUYWJsZUlEXCI6MSxcIklzUEtJRFwiOmZhbHNlLFwib3JkZXJpZFwiOjd9LHtcImlkXCI6OCxcImNhcHRpb25cIjpcIuaYr+WQpuW3sue7j+aUr+S7mOe7mUphY2tcIixcIk5hbWVcIjpcIklzUGF5XCIsXCJJc0F1dG9JbmNyZW1lbnRcIjpmYWxzZSxcIkNhbk51bGxcIjp0cnVlLFwiZGJUeXBlXCI6XCJiaXRcIixcImxlbmd0aFwiOlwiXCIsXCJkZWZhdWx0VmFsdWVcIjpcIjBcIixcIlRhYmxlSURcIjoxLFwiSXNQS0lEXCI6ZmFsc2UsXCJvcmRlcmlkXCI6OH0se1wiaWRcIjoxMixcImNhcHRpb25cIjpcIueZu+mZhuWQjueUn+aIkOeahOWUr+S4gOeggVwiLFwiTmFtZVwiOlwiTG9naW5Db2RlXCIsXCJJc0F1dG9JbmNyZW1lbnRcIjpmYWxzZSxcIkNhbk51bGxcIjp0cnVlLFwiZGJUeXBlXCI6XCJ2YXJjaGFyXCIsXCJsZW5ndGhcIjpcIjUwXCIsXCJUYWJsZUlEXCI6MSxcIklzUEtJRFwiOmZhbHNlLFwib3JkZXJpZFwiOjl9LHtcImlkXCI6MTMsXCJjYXB0aW9uXCI6XCLms6jlhozml7bpl7RcIixcIk5hbWVcIjpcIlJlZ1RpbWVcIixcIklzQXV0b0luY3JlbWVudFwiOmZhbHNlLFwiQ2FuTnVsbFwiOnRydWUsXCJkYlR5cGVcIjpcImRhdGV0aW1lXCIsXCJsZW5ndGhcIjpcIlwiLFwiVGFibGVJRFwiOjEsXCJJc1BLSURcIjpmYWxzZSxcIm9yZGVyaWRcIjoyfSx7XCJpZFwiOjI1LFwiY2FwdGlvblwiOlwi5pyA5ZCO5LiA5qyh6LCD55SoQ2hlY2tVc2Vy5pe26Ze0XCIsXCJOYW1lXCI6XCJMYXN0Q2hlY2tUaW1lXCIsXCJJc0F1dG9JbmNyZW1lbnRcIjpmYWxzZSxcIkNhbk51bGxcIjp0cnVlLFwiZGJUeXBlXCI6XCJkYXRldGltZVwiLFwibGVuZ3RoXCI6XCJcIixcIlRhYmxlSURcIjoxLFwiSXNQS0lEXCI6ZmFsc2UsXCJvcmRlcmlkXCI6MTB9XSxcIm5ld0NvbHVtbnNcIjpbe1wiaWRcIjoyOCxcIk5hbWVcIjpcIkRpc2FibGVcIixcIklzQXV0b0luY3JlbWVudFwiOmZhbHNlLFwiQ2FuTnVsbFwiOnRydWUsXCJkYlR5cGVcIjpcImJpdFwiLFwibGVuZ3RoXCI6XCJcIixcImRlZmF1bHRWYWx1ZVwiOlwiMFwiLFwiVGFibGVJRFwiOjEsXCJJc1BLSURcIjpmYWxzZSxcIm9yZGVyaWRcIjoxMX1dLFwiY2hhbmdlZENvbHVtbnNcIjpbXSxcImRlbGV0ZWRDb2x1bW5zXCI6W10sXCJJRFhDb25maWdzXCI6W10sXCJJRFwiOjB9In0seyJOYW1lIjoiZGF0YWJhc2VpZCIsIlZhbHVlIjoxfV0sIlJvd1N0YXRlIjowfV0sIkNvbHVtbnMiOlt7IkNvbHVtbk5hbWUiOiJpZCIsIkRhdGFUeXBlIjoiU3lzdGVtLkludDY0In0seyJDb2x1bW5OYW1lIjoidHlwZSIsIkRhdGFUeXBlIjoiU3lzdGVtLlN0cmluZyJ9LHsiQ29sdW1uTmFtZSI6ImNvbnRlbnQiLCJEYXRhVHlwZSI6IlN5c3RlbS5TdHJpbmcifSx7IkNvbHVtbk5hbWUiOiJkYXRhYmFzZWlkIiwiRGF0YVR5cGUiOiJTeXN0ZW0uSW50NjQifV19XSwiRGF0YVNldE5hbWUiOiJjNWRiZTA2YS00ODA0LTQ2YjItOTlhZS0yYjVmZjNhZjFkZmIifQ==";}}
