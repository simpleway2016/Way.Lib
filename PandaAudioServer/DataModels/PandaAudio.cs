
using System;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;

namespace SysDB
{

    /// <summary>
    /// 
    /// </summary>
    public enum UserInfo_RoleEnum : int
    {


        /// <summary>
        /// 普通用户
        /// </summary>
        Normal = 0,

        /// <summary>
        /// 允许远程控制
        /// </summary>
        AllowRemote = 1 << 0,

        /// <summary>
        /// CY22用户
        /// </summary>
        CY22 = 1 << 1 | AllowRemote,
    }


    /// <summary>
	/// 用户信息
	/// </summary>
    [System.ComponentModel.DataAnnotations.Schema.Table("userinfo")]
    [Way.EntityDB.Attributes.Table("id")]
    public class UserInfo : Way.EntityDB.DataItem
    {

        /// <summary>
	    /// 
	    /// </summary>
        public UserInfo()
        {
        }


        System.Nullable<Int32> _id;
        /// <summary>
        /// 
        /// </summary>
        [System.ComponentModel.DataAnnotations.Key]
        [System.ComponentModel.DataAnnotations.Schema.Column("id")]
        [Way.EntityDB.WayDBColumnAttribute(Name = "id", Comment = "", Caption = "", Storage = "_id", DbType = "int", IsPrimaryKey = true, IsDbGenerated = true, CanBeNull = false)]
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
                    this.SendPropertyChanging("id", this._id, value);
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
        [Way.EntityDB.WayDBColumnAttribute(Name = "regguid", Comment = "", Caption = "注册唯一认证码", Storage = "_RegGuid", DbType = "varchar(50)")]
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
                    this.SendPropertyChanging("RegGuid", this._RegGuid, value);
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
        [Way.EntityDB.WayDBColumnAttribute(Name = "username", Comment = "", Caption = "", Storage = "_UserName", DbType = "varchar(50)")]
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
                    this.SendPropertyChanging("UserName", this._UserName, value);
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
        [Way.EntityDB.WayDBColumnAttribute(Name = "password", Comment = "", Caption = "", Storage = "_Password", DbType = "varchar(100)")]
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
                    this.SendPropertyChanging("Password", this._Password, value);
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
        [Way.EntityDB.WayDBColumnAttribute(Name = "phonenumber", Comment = "", Caption = "手机号", Storage = "_PhoneNumber", DbType = "varchar(50)")]
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
                    this.SendPropertyChanging("PhoneNumber", this._PhoneNumber, value);
                    this._PhoneNumber = value;
                    this.SendPropertyChanged("PhoneNumber");

                }
            }
        }

        public virtual List<SysDB.UserEffect> Effects { get; set; }

        String _Email;
        /// <summary>
        /// 
        /// </summary>
        [System.ComponentModel.DataAnnotations.Schema.Column("email")]
        [Way.EntityDB.WayDBColumnAttribute(Name = "email", Comment = "", Caption = "", Storage = "_Email", DbType = "varchar(50)")]
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
                    this.SendPropertyChanging("Email", this._Email, value);
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
        [Way.EntityDB.WayDBColumnAttribute(Name = "platform", Comment = "", Caption = "平台", Storage = "_Platform", DbType = "varchar(50)")]
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
                    this.SendPropertyChanging("Platform", this._Platform, value);
                    this._Platform = value;
                    this.SendPropertyChanged("Platform");

                }
            }
        }

        System.Nullable<Boolean> _IsPay = false;
        /// <summary>
        /// 是否已经支付给Jack
        /// </summary>
        [System.ComponentModel.DataAnnotations.Schema.Column("ispay")]
        [Way.EntityDB.WayDBColumnAttribute(Name = "ispay", Comment = "", Caption = "是否已经支付给Jack", Storage = "_IsPay", DbType = "bit")]
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
                    this.SendPropertyChanging("IsPay", this._IsPay, value);
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
        [Way.EntityDB.WayDBColumnAttribute(Name = "logincode", Comment = "", Caption = "登陆后生成的唯一码", Storage = "_LoginCode", DbType = "varchar(50)")]
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
                    this.SendPropertyChanging("LoginCode", this._LoginCode, value);
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
        [Way.EntityDB.WayDBColumnAttribute(Name = "regtime", Comment = "", Caption = "注册时间", Storage = "_RegTime", DbType = "datetime")]
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
                    this.SendPropertyChanging("RegTime", this._RegTime, value);
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
        [Way.EntityDB.WayDBColumnAttribute(Name = "lastchecktime", Comment = "", Caption = "最后一次调用CheckUser时间", Storage = "_LastCheckTime", DbType = "datetime")]
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
                    this.SendPropertyChanging("LastCheckTime", this._LastCheckTime, value);
                    this._LastCheckTime = value;
                    this.SendPropertyChanged("LastCheckTime");

                }
            }
        }

        System.Nullable<Boolean> _Disable = false;
        /// <summary>
        /// 
        /// </summary>
        [System.ComponentModel.DataAnnotations.Schema.Column("disable")]
        [Way.EntityDB.WayDBColumnAttribute(Name = "disable", Comment = "", Caption = "", Storage = "_Disable", DbType = "bit")]
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
                    this.SendPropertyChanging("Disable", this._Disable, value);
                    this._Disable = value;
                    this.SendPropertyChanged("Disable");

                }
            }
        }

        System.Nullable<UserInfo_RoleEnum> _Role = (System.Nullable<UserInfo_RoleEnum>)(0);
        /// <summary>
        /// 用户角色
        /// </summary>
        [System.ComponentModel.DataAnnotations.Schema.Column("role")]
        [Way.EntityDB.WayDBColumnAttribute(Name = "role", Comment = "", Caption = "用户角色", Storage = "_Role", DbType = "int")]
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
                    this.SendPropertyChanging("Role", this._Role, value);
                    this._Role = value;
                    this.SendPropertyChanged("Role");

                }
            }
        }
    }
}
namespace SysDB
{


    /// <summary>
	/// 系统注册码
	/// </summary>
    [System.ComponentModel.DataAnnotations.Schema.Table("systemregcode")]
    [Way.EntityDB.Attributes.Table("id")]
    public class SystemRegCode : Way.EntityDB.DataItem
    {

        /// <summary>
	    /// 
	    /// </summary>
        public SystemRegCode()
        {
        }


        System.Nullable<Int32> _id;
        /// <summary>
        /// 
        /// </summary>
        [System.ComponentModel.DataAnnotations.Key]
        [System.ComponentModel.DataAnnotations.Schema.Column("id")]
        [Way.EntityDB.WayDBColumnAttribute(Name = "id", Comment = "", Caption = "", Storage = "_id", DbType = "int", IsPrimaryKey = true, IsDbGenerated = true, CanBeNull = false)]
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
                    this.SendPropertyChanging("id", this._id, value);
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
        [Way.EntityDB.WayDBColumnAttribute(Name = "regguid", Comment = "", Caption = "唯一认证码", Storage = "_RegGuid", DbType = "varchar(50)")]
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
                    this.SendPropertyChanging("RegGuid", this._RegGuid, value);
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
        [Way.EntityDB.WayDBColumnAttribute(Name = "userid", Comment = "", Caption = "使用的用户id", Storage = "_UserId", DbType = "int")]
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
                    this.SendPropertyChanging("UserId", this._UserId, value);
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
        [Way.EntityDB.WayDBColumnAttribute(Name = "makeruserid", Comment = "", Caption = "谁生成的", Storage = "_MakerUserId", DbType = "int")]
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
                    this.SendPropertyChanging("MakerUserId", this._MakerUserId, value);
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
        [Way.EntityDB.WayDBColumnAttribute(Name = "maketime", Comment = "", Caption = "生成时间", Storage = "_MakeTime", DbType = "datetime")]
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
                    this.SendPropertyChanging("MakeTime", this._MakeTime, value);
                    this._MakeTime = value;
                    this.SendPropertyChanged("MakeTime");

                }
            }
        }

        System.Nullable<UserInfo_RoleEnum> _Role = (System.Nullable<UserInfo_RoleEnum>)(0);
        /// <summary>
        /// 用户角色
        /// </summary>
        [System.ComponentModel.DataAnnotations.Schema.Column("role")]
        [Way.EntityDB.WayDBColumnAttribute(Name = "role", Comment = "", Caption = "用户角色", Storage = "_Role", DbType = "int")]
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
                    this.SendPropertyChanging("Role", this._Role, value);
                    this._Role = value;
                    this.SendPropertyChanged("Role");

                }
            }
        }
    }
}
namespace SysDB
{

    /// <summary>
    /// 
    /// </summary>
    public enum UserEffect_TypeEnum : int
    {


        /// <summary>
        /// </summary>
        Project = 1,

        /// <summary>
        /// </summary>
        Track = 2,

        /// <summary>
        /// </summary>
        Vst = 3,

        /// <summary>
        /// </summary>
        AdminSetting = 4
    }


    /// <summary>
	/// 用户的效果存档
	/// </summary>
    [System.ComponentModel.DataAnnotations.Schema.Table("usereffect")]
    [Way.EntityDB.Attributes.Table("id")]
    public class UserEffect : Way.EntityDB.DataItem
    {

        /// <summary>
	    /// 
	    /// </summary>
        public UserEffect()
        {
        }
        public virtual UserInfo UserInfo { get; set; }

        System.Nullable<Int32> _id;
        /// <summary>
        /// 
        /// </summary>
        [System.ComponentModel.DataAnnotations.Key]
        [System.ComponentModel.DataAnnotations.Schema.Column("id")]
        [Way.EntityDB.WayDBColumnAttribute(Name = "id", Comment = "", Caption = "", Storage = "_id", DbType = "int", IsPrimaryKey = true, IsDbGenerated = true, CanBeNull = false)]
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
                    this.SendPropertyChanging("id", this._id, value);
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
        [Way.EntityDB.WayDBColumnAttribute(Name = "userid", Comment = "", Caption = "", Storage = "_UserId", DbType = "int")]
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
                    this.SendPropertyChanging("UserId", this._UserId, value);
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
        [Way.EntityDB.WayDBColumnAttribute(Name = "filename", Comment = "", Caption = "", Storage = "_FileName", DbType = "varchar(50)")]
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
                    this.SendPropertyChanging("FileName", this._FileName, value);
                    this._FileName = value;
                    this.SendPropertyChanged("FileName");

                }
            }
        }

        System.Nullable<UserEffect_TypeEnum> _Type = (System.Nullable<UserEffect_TypeEnum>)(1);
        /// <summary>
        /// 类型
        /// </summary>
        [System.ComponentModel.DataAnnotations.Schema.Column("type")]
        [Way.EntityDB.WayDBColumnAttribute(Name = "type", Comment = "", Caption = "类型", Storage = "_Type", DbType = "int")]
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
                    this.SendPropertyChanging("Type", this._Type, value);
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
        [Way.EntityDB.WayDBColumnAttribute(Name = "name", Comment = "", Caption = "", Storage = "_Name", DbType = "varchar(50)")]
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
                    this.SendPropertyChanging("Name", this._Name, value);
                    this._Name = value;
                    this.SendPropertyChanged("Name");

                }
            }
        }

        System.Nullable<Int32> _Version = 0;
        /// <summary>
        /// 
        /// </summary>
        [System.ComponentModel.DataAnnotations.Schema.Column("version")]
        [Way.EntityDB.WayDBColumnAttribute(Name = "version", Comment = "", Caption = "", Storage = "_Version", DbType = "int")]
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
                    this.SendPropertyChanging("Version", this._Version, value);
                    this._Version = value;
                    this.SendPropertyChanged("Version");

                }
            }
        }
    }
}
namespace SysDB
{

    /// <summary>
    /// 
    /// </summary>
    public enum AdminUser_RoleEnum : int
    {


        /// <summary>
        /// </summary>
        超级管理员 = 1 << 0,

        /// <summary>
        /// </summary>
        普通员工 = 1 << 1
    }


    /// <summary>
	/// 
	/// </summary>
    [System.ComponentModel.DataAnnotations.Schema.Table("adminuser")]
    [Way.EntityDB.Attributes.Table("id")]
    public class AdminUser : Way.EntityDB.DataItem
    {

        /// <summary>
	    /// 
	    /// </summary>
        public AdminUser()
        {
        }


        System.Nullable<Int32> _id;
        /// <summary>
        /// 
        /// </summary>
        [System.ComponentModel.DataAnnotations.Key]
        [System.ComponentModel.DataAnnotations.Schema.Column("id")]
        [Way.EntityDB.WayDBColumnAttribute(Name = "id", Comment = "", Caption = "", Storage = "_id", DbType = "int", IsPrimaryKey = true, IsDbGenerated = true, CanBeNull = false)]
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
                    this.SendPropertyChanging("id", this._id, value);
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
        [Way.EntityDB.WayDBColumnAttribute(Name = "username", Comment = "", Caption = "", Storage = "_UserName", DbType = "varchar(50)")]
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
                    this.SendPropertyChanging("UserName", this._UserName, value);
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
        [Way.EntityDB.WayDBColumnAttribute(Name = "password", Comment = "", Caption = "", Storage = "_Password", DbType = "varchar(50)")]
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
                    this.SendPropertyChanging("Password", this._Password, value);
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
        [Way.EntityDB.WayDBColumnAttribute(Name = "role", Comment = "", Caption = "", Storage = "_Role", DbType = "int")]
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
                    this.SendPropertyChanging("Role", this._Role, value);
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
        [Way.EntityDB.WayDBColumnAttribute(Name = "phonenumber", Comment = "", Caption = "手机号", Storage = "_PhoneNumber", DbType = "varchar(50)")]
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
                    this.SendPropertyChanging("PhoneNumber", this._PhoneNumber, value);
                    this._PhoneNumber = value;
                    this.SendPropertyChanged("PhoneNumber");

                }
            }
        }
    }
}

namespace SysDB.DB
{
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
        public PandaAudio(string connection, Way.EntityDB.DatabaseType dbType) : base(connection, dbType)
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
            var db = sender as SysDB.DB.PandaAudio;
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
                while (true)
                {
                    var data2del = items0.Take(100).ToList();
                    if (data2del.Count() == 0)
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
            modelBuilder.Entity<SysDB.UserInfo>().HasMany<UserEffect>(m => m.Effects).WithOne(m => m.UserInfo).HasForeignKey(m => m.UserId);
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

        protected override string GetDesignString()
        {
            System.Text.StringBuilder result = new System.Text.StringBuilder();
            result.Append("eyJUYWJsZXMiOlt7IlRhYmxlTmFtZSI6IlNxbGl0ZSIsIlJvd3MiOlt7Ikl0ZW1zIjpbeyJOYW1lIjoiaWQiLCJWYWx1ZSI6MX0seyJOYW1lIjoidHlwZSIsIlZhbHVlIjoiQ3JlYXRlVGFibGVBY3Rpb24ifSx7Ik5hbWUiOiJjb250ZW50IiwiVmFsdWUiOiJ7XCJU");
            result.Append("YWJsZVwiOntcImlkXCI6MSxcImNhcHRpb25cIjpcIueUqOaIt+S/oeaBr1wiLFwiTmFtZVwiOlwiVXNlckluZm9cIixcIkRhdGFiYXNlSURcIjoxLFwiaUxvY2tcIjowfSxcIkNvbHVtbnNcIjpbe1wiaWRcIjoxLFwiTmFtZVwiOlwiaWRcIixcIklzQXV0b0luY3Jl");
            result.Append("bWVudFwiOnRydWUsXCJDYW5OdWxsXCI6ZmFsc2UsXCJkYlR5cGVcIjpcImludFwiLFwiVGFibGVJRFwiOjEsXCJJc1BLSURcIjp0cnVlLFwib3JkZXJpZFwiOjB9LHtcImlkXCI6MixcImNhcHRpb25cIjpcIuazqOWGjOWUr+S4gOiupOivgeeggVwiLFwiTmFtZVwi");
            result.Append("OlwiUmVnR3VpZFwiLFwiSXNBdXRvSW5jcmVtZW50XCI6ZmFsc2UsXCJDYW5OdWxsXCI6dHJ1ZSxcImRiVHlwZVwiOlwidmFyY2hhclwiLFwibGVuZ3RoXCI6XCI1MFwiLFwiVGFibGVJRFwiOjEsXCJJc1BLSURcIjpmYWxzZSxcIm9yZGVyaWRcIjoxfSx7XCJpZFwi");
            result.Append("OjMsXCJOYW1lXCI6XCJVc2VyTmFtZVwiLFwiSXNBdXRvSW5jcmVtZW50XCI6ZmFsc2UsXCJDYW5OdWxsXCI6dHJ1ZSxcImRiVHlwZVwiOlwidmFyY2hhclwiLFwibGVuZ3RoXCI6XCI1MFwiLFwiVGFibGVJRFwiOjEsXCJJc1BLSURcIjpmYWxzZSxcIm9yZGVyaWRc");
            result.Append("IjoyfSx7XCJpZFwiOjQsXCJOYW1lXCI6XCJQYXNzd29yZFwiLFwiSXNBdXRvSW5jcmVtZW50XCI6ZmFsc2UsXCJDYW5OdWxsXCI6dHJ1ZSxcImRiVHlwZVwiOlwidmFyY2hhclwiLFwibGVuZ3RoXCI6XCIxMDBcIixcIlRhYmxlSURcIjoxLFwiSXNQS0lEXCI6ZmFs");
            result.Append("c2UsXCJvcmRlcmlkXCI6M30se1wiaWRcIjo1LFwiY2FwdGlvblwiOlwi5omL5py65Y+3XCIsXCJOYW1lXCI6XCJQaG9uZU51bWJlclwiLFwiSXNBdXRvSW5jcmVtZW50XCI6ZmFsc2UsXCJDYW5OdWxsXCI6dHJ1ZSxcImRiVHlwZVwiOlwidmFyY2hhclwiLFwibGVu");
            result.Append("Z3RoXCI6XCI1MFwiLFwiVGFibGVJRFwiOjEsXCJJc1BLSURcIjpmYWxzZSxcIm9yZGVyaWRcIjo0fSx7XCJpZFwiOjYsXCJOYW1lXCI6XCJFbWFpbFwiLFwiSXNBdXRvSW5jcmVtZW50XCI6ZmFsc2UsXCJDYW5OdWxsXCI6dHJ1ZSxcImRiVHlwZVwiOlwidmFyY2hh");
            result.Append("clwiLFwibGVuZ3RoXCI6XCI1MFwiLFwiVGFibGVJRFwiOjEsXCJJc1BLSURcIjpmYWxzZSxcIm9yZGVyaWRcIjo1fSx7XCJpZFwiOjcsXCJjYXB0aW9uXCI6XCLlubPlj7BcIixcIk5hbWVcIjpcIlBsYXRmb3JtXCIsXCJJc0F1dG9JbmNyZW1lbnRcIjpmYWxzZSxc");
            result.Append("IkNhbk51bGxcIjp0cnVlLFwiZGJUeXBlXCI6XCJ2YXJjaGFyXCIsXCJsZW5ndGhcIjpcIjUwXCIsXCJUYWJsZUlEXCI6MSxcIklzUEtJRFwiOmZhbHNlLFwib3JkZXJpZFwiOjZ9XSxcIklEWENvbmZpZ3NcIjpbXSxcIklEXCI6MH0ifSx7Ik5hbWUiOiJkYXRhYmFz");
            result.Append("ZWlkIiwiVmFsdWUiOjF9XSwiUm93U3RhdGUiOjB9LHsiSXRlbXMiOlt7Ik5hbWUiOiJpZCIsIlZhbHVlIjoyfSx7Ik5hbWUiOiJ0eXBlIiwiVmFsdWUiOiJDaGFuZ2VUYWJsZUFjdGlvbiJ9LHsiTmFtZSI6ImNvbnRlbnQiLCJWYWx1ZSI6IntcIk9sZFRhYmxlTmFt");
            result.Append("ZVwiOlwiVXNlckluZm9cIixcIk5ld1RhYmxlTmFtZVwiOlwiVXNlckluZm9cIixcIm90aGVyQ29sdW1uc1wiOlt7XCJpZFwiOjEsXCJOYW1lXCI6XCJpZFwiLFwiSXNBdXRvSW5jcmVtZW50XCI6dHJ1ZSxcIkNhbk51bGxcIjpmYWxzZSxcImRiVHlwZVwiOlwiaW50");
            result.Append("XCIsXCJUYWJsZUlEXCI6MSxcIklzUEtJRFwiOnRydWUsXCJvcmRlcmlkXCI6MH0se1wiaWRcIjoyLFwiY2FwdGlvblwiOlwi5rOo5YaM5ZSv5LiA6K6k6K+B56CBXCIsXCJOYW1lXCI6XCJSZWdHdWlkXCIsXCJJc0F1dG9JbmNyZW1lbnRcIjpmYWxzZSxcIkNhbk51");
            result.Append("bGxcIjp0cnVlLFwiZGJUeXBlXCI6XCJ2YXJjaGFyXCIsXCJsZW5ndGhcIjpcIjUwXCIsXCJUYWJsZUlEXCI6MSxcIklzUEtJRFwiOmZhbHNlLFwib3JkZXJpZFwiOjF9LHtcImlkXCI6MyxcIk5hbWVcIjpcIlVzZXJOYW1lXCIsXCJJc0F1dG9JbmNyZW1lbnRcIjpm");
            result.Append("YWxzZSxcIkNhbk51bGxcIjp0cnVlLFwiZGJUeXBlXCI6XCJ2YXJjaGFyXCIsXCJsZW5ndGhcIjpcIjUwXCIsXCJUYWJsZUlEXCI6MSxcIklzUEtJRFwiOmZhbHNlLFwib3JkZXJpZFwiOjJ9LHtcImlkXCI6NCxcIk5hbWVcIjpcIlBhc3N3b3JkXCIsXCJJc0F1dG9J");
            result.Append("bmNyZW1lbnRcIjpmYWxzZSxcIkNhbk51bGxcIjp0cnVlLFwiZGJUeXBlXCI6XCJ2YXJjaGFyXCIsXCJsZW5ndGhcIjpcIjEwMFwiLFwiVGFibGVJRFwiOjEsXCJJc1BLSURcIjpmYWxzZSxcIm9yZGVyaWRcIjozfSx7XCJpZFwiOjUsXCJjYXB0aW9uXCI6XCLmiYvm");
            result.Append("nLrlj7dcIixcIk5hbWVcIjpcIlBob25lTnVtYmVyXCIsXCJJc0F1dG9JbmNyZW1lbnRcIjpmYWxzZSxcIkNhbk51bGxcIjp0cnVlLFwiZGJUeXBlXCI6XCJ2YXJjaGFyXCIsXCJsZW5ndGhcIjpcIjUwXCIsXCJUYWJsZUlEXCI6MSxcIklzUEtJRFwiOmZhbHNlLFwi");
            result.Append("b3JkZXJpZFwiOjR9LHtcImlkXCI6NixcIk5hbWVcIjpcIkVtYWlsXCIsXCJJc0F1dG9JbmNyZW1lbnRcIjpmYWxzZSxcIkNhbk51bGxcIjp0cnVlLFwiZGJUeXBlXCI6XCJ2YXJjaGFyXCIsXCJsZW5ndGhcIjpcIjUwXCIsXCJUYWJsZUlEXCI6MSxcIklzUEtJRFwi");
            result.Append("OmZhbHNlLFwib3JkZXJpZFwiOjV9LHtcImlkXCI6NyxcImNhcHRpb25cIjpcIuW5s+WPsFwiLFwiTmFtZVwiOlwiUGxhdGZvcm1cIixcIklzQXV0b0luY3JlbWVudFwiOmZhbHNlLFwiQ2FuTnVsbFwiOnRydWUsXCJkYlR5cGVcIjpcInZhcmNoYXJcIixcImxlbmd0");
            result.Append("aFwiOlwiNTBcIixcIlRhYmxlSURcIjoxLFwiSXNQS0lEXCI6ZmFsc2UsXCJvcmRlcmlkXCI6Nn1dLFwibmV3Q29sdW1uc1wiOlt7XCJpZFwiOjgsXCJjYXB0aW9uXCI6XCLmmK/lkKblt7Lnu4/mlK/ku5jnu5lKYWNrXCIsXCJOYW1lXCI6XCJJc1BheVwiLFwiSXNB");
            result.Append("dXRvSW5jcmVtZW50XCI6ZmFsc2UsXCJDYW5OdWxsXCI6dHJ1ZSxcImRiVHlwZVwiOlwiYml0XCIsXCJsZW5ndGhcIjpcIlwiLFwiZGVmYXVsdFZhbHVlXCI6XCIwXCIsXCJUYWJsZUlEXCI6MSxcIklzUEtJRFwiOmZhbHNlLFwib3JkZXJpZFwiOjd9XSxcImNoYW5n");
            result.Append("ZWRDb2x1bW5zXCI6W10sXCJkZWxldGVkQ29sdW1uc1wiOltdLFwiSURYQ29uZmlnc1wiOltdLFwiSURcIjowfSJ9LHsiTmFtZSI6ImRhdGFiYXNlaWQiLCJWYWx1ZSI6MX1dLCJSb3dTdGF0ZSI6MH0seyJJdGVtcyI6W3siTmFtZSI6ImlkIiwiVmFsdWUiOjN9LHsi");
            result.Append("TmFtZSI6InR5cGUiLCJWYWx1ZSI6IkNyZWF0ZVRhYmxlQWN0aW9uIn0seyJOYW1lIjoiY29udGVudCIsIlZhbHVlIjoie1wiVGFibGVcIjp7XCJpZFwiOjIsXCJjYXB0aW9uXCI6XCLns7vnu5/ms6jlhoznoIFcIixcIk5hbWVcIjpcIlN5c3RlbVJlZ0NvZGVcIixc");
            result.Append("IkRhdGFiYXNlSURcIjoxLFwiaUxvY2tcIjowfSxcIkNvbHVtbnNcIjpbe1wiaWRcIjo5LFwiTmFtZVwiOlwiaWRcIixcIklzQXV0b0luY3JlbWVudFwiOnRydWUsXCJDYW5OdWxsXCI6ZmFsc2UsXCJkYlR5cGVcIjpcImludFwiLFwiVGFibGVJRFwiOjIsXCJJc1BL");
            result.Append("SURcIjp0cnVlLFwib3JkZXJpZFwiOjB9LHtcImlkXCI6MTAsXCJOYW1lXCI6XCJjb2RlXCIsXCJJc0F1dG9JbmNyZW1lbnRcIjpmYWxzZSxcIkNhbk51bGxcIjp0cnVlLFwiZGJUeXBlXCI6XCJ2YXJjaGFyXCIsXCJsZW5ndGhcIjpcIjUwXCIsXCJUYWJsZUlEXCI6");
            result.Append("MixcIklzUEtJRFwiOmZhbHNlLFwib3JkZXJpZFwiOjF9LHtcImlkXCI6MTEsXCJjYXB0aW9uXCI6XCLkvb/nlKjnmoTnlKjmiLdpZFwiLFwiTmFtZVwiOlwiVXNlcklkXCIsXCJJc0F1dG9JbmNyZW1lbnRcIjpmYWxzZSxcIkNhbk51bGxcIjp0cnVlLFwiZGJUeXBl");
            result.Append("XCI6XCJpbnRcIixcImxlbmd0aFwiOlwiXCIsXCJUYWJsZUlEXCI6MixcIklzUEtJRFwiOmZhbHNlLFwib3JkZXJpZFwiOjJ9XSxcIklEWENvbmZpZ3NcIjpbXSxcIklEXCI6MH0ifSx7Ik5hbWUiOiJkYXRhYmFzZWlkIiwiVmFsdWUiOjF9XSwiUm93U3RhdGUiOjB9");
            result.Append("LHsiSXRlbXMiOlt7Ik5hbWUiOiJpZCIsIlZhbHVlIjo0fSx7Ik5hbWUiOiJ0eXBlIiwiVmFsdWUiOiJDaGFuZ2VUYWJsZUFjdGlvbiJ9LHsiTmFtZSI6ImNvbnRlbnQiLCJWYWx1ZSI6IntcIk9sZFRhYmxlTmFtZVwiOlwiU3lzdGVtUmVnQ29kZVwiLFwiTmV3VGFi");
            result.Append("bGVOYW1lXCI6XCJTeXN0ZW1SZWdDb2RlXCIsXCJvdGhlckNvbHVtbnNcIjpbe1wiaWRcIjo5LFwiTmFtZVwiOlwiaWRcIixcIklzQXV0b0luY3JlbWVudFwiOnRydWUsXCJDYW5OdWxsXCI6ZmFsc2UsXCJkYlR5cGVcIjpcImludFwiLFwiVGFibGVJRFwiOjIsXCJJ");
            result.Append("c1BLSURcIjp0cnVlLFwib3JkZXJpZFwiOjB9LHtcImlkXCI6MTEsXCJjYXB0aW9uXCI6XCLkvb/nlKjnmoTnlKjmiLdpZFwiLFwiTmFtZVwiOlwiVXNlcklkXCIsXCJJc0F1dG9JbmNyZW1lbnRcIjpmYWxzZSxcIkNhbk51bGxcIjp0cnVlLFwiZGJUeXBlXCI6XCJp");
            result.Append("bnRcIixcImxlbmd0aFwiOlwiXCIsXCJUYWJsZUlEXCI6MixcIklzUEtJRFwiOmZhbHNlLFwib3JkZXJpZFwiOjJ9XSxcIm5ld0NvbHVtbnNcIjpbXSxcImNoYW5nZWRDb2x1bW5zXCI6W3tcImlkXCI6MTAsXCJOYW1lXCI6XCJSZWdHdWlkXCIsXCJJc0F1dG9JbmNy");
            result.Append("ZW1lbnRcIjpmYWxzZSxcIkNhbk51bGxcIjp0cnVlLFwiZGJUeXBlXCI6XCJ2YXJjaGFyXCIsXCJsZW5ndGhcIjpcIjUwXCIsXCJUYWJsZUlEXCI6MixcIklzUEtJRFwiOmZhbHNlLFwib3JkZXJpZFwiOjEsXCJCYWNrdXBDaGFuZ2VkUHJvcGVydGllc1wiOntcIk5h");
            result.Append("bWVcIjp7XCJPcmlnaW5hbFZhbHVlXCI6XCJjb2RlXCJ9fX1dLFwiZGVsZXRlZENvbHVtbnNcIjpbXSxcIklEWENvbmZpZ3NcIjpbXSxcIklEXCI6MH0ifSx7Ik5hbWUiOiJkYXRhYmFzZWlkIiwiVmFsdWUiOjF9XSwiUm93U3RhdGUiOjB9LHsiSXRlbXMiOlt7Ik5h");
            result.Append("bWUiOiJpZCIsIlZhbHVlIjo1fSx7Ik5hbWUiOiJ0eXBlIiwiVmFsdWUiOiJDaGFuZ2VUYWJsZUFjdGlvbiJ9LHsiTmFtZSI6ImNvbnRlbnQiLCJWYWx1ZSI6IntcIk9sZFRhYmxlTmFtZVwiOlwiVXNlckluZm9cIixcIk5ld1RhYmxlTmFtZVwiOlwiVXNlckluZm9c");
            result.Append("IixcIm90aGVyQ29sdW1uc1wiOlt7XCJpZFwiOjEsXCJOYW1lXCI6XCJpZFwiLFwiSXNBdXRvSW5jcmVtZW50XCI6dHJ1ZSxcIkNhbk51bGxcIjpmYWxzZSxcImRiVHlwZVwiOlwiaW50XCIsXCJUYWJsZUlEXCI6MSxcIklzUEtJRFwiOnRydWUsXCJvcmRlcmlkXCI6");
            result.Append("MH0se1wiaWRcIjoyLFwiY2FwdGlvblwiOlwi5rOo5YaM5ZSv5LiA6K6k6K+B56CBXCIsXCJOYW1lXCI6XCJSZWdHdWlkXCIsXCJJc0F1dG9JbmNyZW1lbnRcIjpmYWxzZSxcIkNhbk51bGxcIjp0cnVlLFwiZGJUeXBlXCI6XCJ2YXJjaGFyXCIsXCJsZW5ndGhcIjpc");
            result.Append("IjUwXCIsXCJUYWJsZUlEXCI6MSxcIklzUEtJRFwiOmZhbHNlLFwib3JkZXJpZFwiOjF9LHtcImlkXCI6MyxcIk5hbWVcIjpcIlVzZXJOYW1lXCIsXCJJc0F1dG9JbmNyZW1lbnRcIjpmYWxzZSxcIkNhbk51bGxcIjp0cnVlLFwiZGJUeXBlXCI6XCJ2YXJjaGFyXCIs");
            result.Append("XCJsZW5ndGhcIjpcIjUwXCIsXCJUYWJsZUlEXCI6MSxcIklzUEtJRFwiOmZhbHNlLFwib3JkZXJpZFwiOjJ9LHtcImlkXCI6NCxcIk5hbWVcIjpcIlBhc3N3b3JkXCIsXCJJc0F1dG9JbmNyZW1lbnRcIjpmYWxzZSxcIkNhbk51bGxcIjp0cnVlLFwiZGJUeXBlXCI6");
            result.Append("XCJ2YXJjaGFyXCIsXCJsZW5ndGhcIjpcIjEwMFwiLFwiVGFibGVJRFwiOjEsXCJJc1BLSURcIjpmYWxzZSxcIm9yZGVyaWRcIjozfSx7XCJpZFwiOjUsXCJjYXB0aW9uXCI6XCLmiYvmnLrlj7dcIixcIk5hbWVcIjpcIlBob25lTnVtYmVyXCIsXCJJc0F1dG9JbmNy");
            result.Append("ZW1lbnRcIjpmYWxzZSxcIkNhbk51bGxcIjp0cnVlLFwiZGJUeXBlXCI6XCJ2YXJjaGFyXCIsXCJsZW5ndGhcIjpcIjUwXCIsXCJUYWJsZUlEXCI6MSxcIklzUEtJRFwiOmZhbHNlLFwib3JkZXJpZFwiOjR9LHtcImlkXCI6NixcIk5hbWVcIjpcIkVtYWlsXCIsXCJJ");
            result.Append("c0F1dG9JbmNyZW1lbnRcIjpmYWxzZSxcIkNhbk51bGxcIjp0cnVlLFwiZGJUeXBlXCI6XCJ2YXJjaGFyXCIsXCJsZW5ndGhcIjpcIjUwXCIsXCJUYWJsZUlEXCI6MSxcIklzUEtJRFwiOmZhbHNlLFwib3JkZXJpZFwiOjV9LHtcImlkXCI6NyxcImNhcHRpb25cIjpc");
            result.Append("IuW5s+WPsFwiLFwiTmFtZVwiOlwiUGxhdGZvcm1cIixcIklzQXV0b0luY3JlbWVudFwiOmZhbHNlLFwiQ2FuTnVsbFwiOnRydWUsXCJkYlR5cGVcIjpcInZhcmNoYXJcIixcImxlbmd0aFwiOlwiNTBcIixcIlRhYmxlSURcIjoxLFwiSXNQS0lEXCI6ZmFsc2UsXCJv");
            result.Append("cmRlcmlkXCI6Nn0se1wiaWRcIjo4LFwiY2FwdGlvblwiOlwi5piv5ZCm5bey57uP5pSv5LuY57uZSmFja1wiLFwiTmFtZVwiOlwiSXNQYXlcIixcIklzQXV0b0luY3JlbWVudFwiOmZhbHNlLFwiQ2FuTnVsbFwiOnRydWUsXCJkYlR5cGVcIjpcImJpdFwiLFwibGVu");
            result.Append("Z3RoXCI6XCJcIixcImRlZmF1bHRWYWx1ZVwiOlwiMFwiLFwiVGFibGVJRFwiOjEsXCJJc1BLSURcIjpmYWxzZSxcIm9yZGVyaWRcIjo3fV0sXCJuZXdDb2x1bW5zXCI6W3tcImlkXCI6MTIsXCJjYXB0aW9uXCI6XCLnmbvpmYblkI7nlJ/miJDnmoTllK/kuIDnoIFc");
            result.Append("IixcIk5hbWVcIjpcIkxvZ2luQ29kZVwiLFwiSXNBdXRvSW5jcmVtZW50XCI6ZmFsc2UsXCJDYW5OdWxsXCI6dHJ1ZSxcImRiVHlwZVwiOlwidmFyY2hhclwiLFwibGVuZ3RoXCI6XCI1MFwiLFwiVGFibGVJRFwiOjEsXCJJc1BLSURcIjpmYWxzZSxcIm9yZGVyaWRc");
            result.Append("Ijo4fV0sXCJjaGFuZ2VkQ29sdW1uc1wiOltdLFwiZGVsZXRlZENvbHVtbnNcIjpbXSxcIklEWENvbmZpZ3NcIjpbXSxcIklEXCI6MH0ifSx7Ik5hbWUiOiJkYXRhYmFzZWlkIiwiVmFsdWUiOjF9XSwiUm93U3RhdGUiOjB9LHsiSXRlbXMiOlt7Ik5hbWUiOiJpZCIs");
            result.Append("IlZhbHVlIjo2fSx7Ik5hbWUiOiJ0eXBlIiwiVmFsdWUiOiJDaGFuZ2VUYWJsZUFjdGlvbiJ9LHsiTmFtZSI6ImNvbnRlbnQiLCJWYWx1ZSI6IntcIk9sZFRhYmxlTmFtZVwiOlwiVXNlckluZm9cIixcIk5ld1RhYmxlTmFtZVwiOlwiVXNlckluZm9cIixcIm90aGVy");
            result.Append("Q29sdW1uc1wiOlt7XCJpZFwiOjEsXCJOYW1lXCI6XCJpZFwiLFwiSXNBdXRvSW5jcmVtZW50XCI6dHJ1ZSxcIkNhbk51bGxcIjpmYWxzZSxcImRiVHlwZVwiOlwiaW50XCIsXCJUYWJsZUlEXCI6MSxcIklzUEtJRFwiOnRydWUsXCJvcmRlcmlkXCI6MH0se1wiaWRc");
            result.Append("IjoyLFwiY2FwdGlvblwiOlwi5rOo5YaM5ZSv5LiA6K6k6K+B56CBXCIsXCJOYW1lXCI6XCJSZWdHdWlkXCIsXCJJc0F1dG9JbmNyZW1lbnRcIjpmYWxzZSxcIkNhbk51bGxcIjp0cnVlLFwiZGJUeXBlXCI6XCJ2YXJjaGFyXCIsXCJsZW5ndGhcIjpcIjUwXCIsXCJU");
            result.Append("YWJsZUlEXCI6MSxcIklzUEtJRFwiOmZhbHNlLFwib3JkZXJpZFwiOjF9LHtcImlkXCI6MyxcIk5hbWVcIjpcIlVzZXJOYW1lXCIsXCJJc0F1dG9JbmNyZW1lbnRcIjpmYWxzZSxcIkNhbk51bGxcIjp0cnVlLFwiZGJUeXBlXCI6XCJ2YXJjaGFyXCIsXCJsZW5ndGhc");
            result.Append("IjpcIjUwXCIsXCJUYWJsZUlEXCI6MSxcIklzUEtJRFwiOmZhbHNlLFwib3JkZXJpZFwiOjJ9LHtcImlkXCI6NCxcIk5hbWVcIjpcIlBhc3N3b3JkXCIsXCJJc0F1dG9JbmNyZW1lbnRcIjpmYWxzZSxcIkNhbk51bGxcIjp0cnVlLFwiZGJUeXBlXCI6XCJ2YXJjaGFy");
            result.Append("XCIsXCJsZW5ndGhcIjpcIjEwMFwiLFwiVGFibGVJRFwiOjEsXCJJc1BLSURcIjpmYWxzZSxcIm9yZGVyaWRcIjozfSx7XCJpZFwiOjUsXCJjYXB0aW9uXCI6XCLmiYvmnLrlj7dcIixcIk5hbWVcIjpcIlBob25lTnVtYmVyXCIsXCJJc0F1dG9JbmNyZW1lbnRcIjpm");
            result.Append("YWxzZSxcIkNhbk51bGxcIjp0cnVlLFwiZGJUeXBlXCI6XCJ2YXJjaGFyXCIsXCJsZW5ndGhcIjpcIjUwXCIsXCJUYWJsZUlEXCI6MSxcIklzUEtJRFwiOmZhbHNlLFwib3JkZXJpZFwiOjR9LHtcImlkXCI6NixcIk5hbWVcIjpcIkVtYWlsXCIsXCJJc0F1dG9JbmNy");
            result.Append("ZW1lbnRcIjpmYWxzZSxcIkNhbk51bGxcIjp0cnVlLFwiZGJUeXBlXCI6XCJ2YXJjaGFyXCIsXCJsZW5ndGhcIjpcIjUwXCIsXCJUYWJsZUlEXCI6MSxcIklzUEtJRFwiOmZhbHNlLFwib3JkZXJpZFwiOjV9LHtcImlkXCI6NyxcImNhcHRpb25cIjpcIuW5s+WPsFwi");
            result.Append("LFwiTmFtZVwiOlwiUGxhdGZvcm1cIixcIklzQXV0b0luY3JlbWVudFwiOmZhbHNlLFwiQ2FuTnVsbFwiOnRydWUsXCJkYlR5cGVcIjpcInZhcmNoYXJcIixcImxlbmd0aFwiOlwiNTBcIixcIlRhYmxlSURcIjoxLFwiSXNQS0lEXCI6ZmFsc2UsXCJvcmRlcmlkXCI6");
            result.Append("Nn0se1wiaWRcIjo4LFwiY2FwdGlvblwiOlwi5piv5ZCm5bey57uP5pSv5LuY57uZSmFja1wiLFwiTmFtZVwiOlwiSXNQYXlcIixcIklzQXV0b0luY3JlbWVudFwiOmZhbHNlLFwiQ2FuTnVsbFwiOnRydWUsXCJkYlR5cGVcIjpcImJpdFwiLFwibGVuZ3RoXCI6XCJc");
            result.Append("IixcImRlZmF1bHRWYWx1ZVwiOlwiMFwiLFwiVGFibGVJRFwiOjEsXCJJc1BLSURcIjpmYWxzZSxcIm9yZGVyaWRcIjo3fSx7XCJpZFwiOjEyLFwiY2FwdGlvblwiOlwi55m76ZmG5ZCO55Sf5oiQ55qE5ZSv5LiA56CBXCIsXCJOYW1lXCI6XCJMb2dpbkNvZGVcIixc");
            result.Append("IklzQXV0b0luY3JlbWVudFwiOmZhbHNlLFwiQ2FuTnVsbFwiOnRydWUsXCJkYlR5cGVcIjpcInZhcmNoYXJcIixcImxlbmd0aFwiOlwiNTBcIixcIlRhYmxlSURcIjoxLFwiSXNQS0lEXCI6ZmFsc2UsXCJvcmRlcmlkXCI6OH1dLFwibmV3Q29sdW1uc1wiOlt7XCJp");
            result.Append("ZFwiOjEzLFwiY2FwdGlvblwiOlwi5rOo5YaM5pe26Ze0XCIsXCJOYW1lXCI6XCJSZWdUaW1lXCIsXCJJc0F1dG9JbmNyZW1lbnRcIjpmYWxzZSxcIkNhbk51bGxcIjp0cnVlLFwiZGJUeXBlXCI6XCJkYXRldGltZVwiLFwibGVuZ3RoXCI6XCJcIixcIlRhYmxlSURc");
            result.Append("IjoxLFwiSXNQS0lEXCI6ZmFsc2UsXCJvcmRlcmlkXCI6Mn1dLFwiY2hhbmdlZENvbHVtbnNcIjpbXSxcImRlbGV0ZWRDb2x1bW5zXCI6W10sXCJJRFhDb25maWdzXCI6W10sXCJJRFwiOjB9In0seyJOYW1lIjoiZGF0YWJhc2VpZCIsIlZhbHVlIjoxfV0sIlJvd1N0");
            result.Append("YXRlIjowfSx7Ikl0ZW1zIjpbeyJOYW1lIjoiaWQiLCJWYWx1ZSI6N30seyJOYW1lIjoidHlwZSIsIlZhbHVlIjoiQ3JlYXRlVGFibGVBY3Rpb24ifSx7Ik5hbWUiOiJjb250ZW50IiwiVmFsdWUiOiJ7XCJUYWJsZVwiOntcImlkXCI6MyxcImNhcHRpb25cIjpcIueU");
            result.Append("qOaIt+eahOaViOaenOWtmOaho1wiLFwiTmFtZVwiOlwiVXNlckVmZmVjdFwiLFwiRGF0YWJhc2VJRFwiOjEsXCJpTG9ja1wiOjB9LFwiQ29sdW1uc1wiOlt7XCJpZFwiOjE0LFwiTmFtZVwiOlwiaWRcIixcIklzQXV0b0luY3JlbWVudFwiOnRydWUsXCJDYW5OdWxs");
            result.Append("XCI6ZmFsc2UsXCJkYlR5cGVcIjpcImludFwiLFwiVGFibGVJRFwiOjMsXCJJc1BLSURcIjp0cnVlLFwib3JkZXJpZFwiOjB9LHtcImlkXCI6MTUsXCJOYW1lXCI6XCJVc2VySWRcIixcIklzQXV0b0luY3JlbWVudFwiOmZhbHNlLFwiQ2FuTnVsbFwiOnRydWUsXCJk");
            result.Append("YlR5cGVcIjpcImludFwiLFwibGVuZ3RoXCI6XCJcIixcIlRhYmxlSURcIjozLFwiSXNQS0lEXCI6ZmFsc2UsXCJvcmRlcmlkXCI6MX0se1wiaWRcIjoxNixcIk5hbWVcIjpcIkZpbGVOYW1lXCIsXCJJc0F1dG9JbmNyZW1lbnRcIjpmYWxzZSxcIkNhbk51bGxcIjp0");
            result.Append("cnVlLFwiZGJUeXBlXCI6XCJ2YXJjaGFyXCIsXCJsZW5ndGhcIjpcIjUwXCIsXCJUYWJsZUlEXCI6MyxcIklzUEtJRFwiOmZhbHNlLFwib3JkZXJpZFwiOjJ9LHtcImlkXCI6MTcsXCJjYXB0aW9uXCI6XCLnsbvlnotcIixcIk5hbWVcIjpcIlR5cGVcIixcIklzQXV0");
            result.Append("b0luY3JlbWVudFwiOmZhbHNlLFwiQ2FuTnVsbFwiOnRydWUsXCJkYlR5cGVcIjpcImludFwiLFwiRW51bURlZmluZVwiOlwiUHJvamVjdD0xLFxcblRyYWNrPTJcIixcImxlbmd0aFwiOlwiXCIsXCJkZWZhdWx0VmFsdWVcIjpcIjFcIixcIlRhYmxlSURcIjozLFwi");
            result.Append("SXNQS0lEXCI6ZmFsc2UsXCJvcmRlcmlkXCI6M31dLFwiSURYQ29uZmlnc1wiOltdLFwiSURcIjowfSJ9LHsiTmFtZSI6ImRhdGFiYXNlaWQiLCJWYWx1ZSI6MX1dLCJSb3dTdGF0ZSI6MH0seyJJdGVtcyI6W3siTmFtZSI6ImlkIiwiVmFsdWUiOjh9LHsiTmFtZSI6");
            result.Append("InR5cGUiLCJWYWx1ZSI6IkNoYW5nZVRhYmxlQWN0aW9uIn0seyJOYW1lIjoiY29udGVudCIsIlZhbHVlIjoie1wiT2xkVGFibGVOYW1lXCI6XCJVc2VySW5mb1wiLFwiTmV3VGFibGVOYW1lXCI6XCJVc2VySW5mb1wiLFwib3RoZXJDb2x1bW5zXCI6W3tcImlkXCI6");
            result.Append("MSxcIk5hbWVcIjpcImlkXCIsXCJJc0F1dG9JbmNyZW1lbnRcIjp0cnVlLFwiQ2FuTnVsbFwiOmZhbHNlLFwiZGJUeXBlXCI6XCJpbnRcIixcIlRhYmxlSURcIjoxLFwiSXNQS0lEXCI6dHJ1ZSxcIm9yZGVyaWRcIjowfSx7XCJpZFwiOjIsXCJjYXB0aW9uXCI6XCLm");
            result.Append("s6jlhozllK/kuIDorqTor4HnoIFcIixcIk5hbWVcIjpcIlJlZ0d1aWRcIixcIklzQXV0b0luY3JlbWVudFwiOmZhbHNlLFwiQ2FuTnVsbFwiOnRydWUsXCJkYlR5cGVcIjpcInZhcmNoYXJcIixcImxlbmd0aFwiOlwiNTBcIixcIlRhYmxlSURcIjoxLFwiSXNQS0lE");
            result.Append("XCI6ZmFsc2UsXCJvcmRlcmlkXCI6MX0se1wiaWRcIjozLFwiTmFtZVwiOlwiVXNlck5hbWVcIixcIklzQXV0b0luY3JlbWVudFwiOmZhbHNlLFwiQ2FuTnVsbFwiOnRydWUsXCJkYlR5cGVcIjpcInZhcmNoYXJcIixcImxlbmd0aFwiOlwiNTBcIixcIlRhYmxlSURc");
            result.Append("IjoxLFwiSXNQS0lEXCI6ZmFsc2UsXCJvcmRlcmlkXCI6M30se1wiaWRcIjo0LFwiTmFtZVwiOlwiUGFzc3dvcmRcIixcIklzQXV0b0luY3JlbWVudFwiOmZhbHNlLFwiQ2FuTnVsbFwiOnRydWUsXCJkYlR5cGVcIjpcInZhcmNoYXJcIixcImxlbmd0aFwiOlwiMTAw");
            result.Append("XCIsXCJUYWJsZUlEXCI6MSxcIklzUEtJRFwiOmZhbHNlLFwib3JkZXJpZFwiOjR9LHtcImlkXCI6NSxcImNhcHRpb25cIjpcIuaJi+acuuWPt1wiLFwiTmFtZVwiOlwiUGhvbmVOdW1iZXJcIixcIklzQXV0b0luY3JlbWVudFwiOmZhbHNlLFwiQ2FuTnVsbFwiOnRy");
            result.Append("dWUsXCJkYlR5cGVcIjpcInZhcmNoYXJcIixcImxlbmd0aFwiOlwiNTBcIixcIlRhYmxlSURcIjoxLFwiSXNQS0lEXCI6ZmFsc2UsXCJvcmRlcmlkXCI6NX0se1wiaWRcIjo2LFwiTmFtZVwiOlwiRW1haWxcIixcIklzQXV0b0luY3JlbWVudFwiOmZhbHNlLFwiQ2Fu");
            result.Append("TnVsbFwiOnRydWUsXCJkYlR5cGVcIjpcInZhcmNoYXJcIixcImxlbmd0aFwiOlwiNTBcIixcIlRhYmxlSURcIjoxLFwiSXNQS0lEXCI6ZmFsc2UsXCJvcmRlcmlkXCI6Nn0se1wiaWRcIjo3LFwiY2FwdGlvblwiOlwi5bmz5Y+wXCIsXCJOYW1lXCI6XCJQbGF0Zm9y");
            result.Append("bVwiLFwiSXNBdXRvSW5jcmVtZW50XCI6ZmFsc2UsXCJDYW5OdWxsXCI6dHJ1ZSxcImRiVHlwZVwiOlwidmFyY2hhclwiLFwibGVuZ3RoXCI6XCI1MFwiLFwiVGFibGVJRFwiOjEsXCJJc1BLSURcIjpmYWxzZSxcIm9yZGVyaWRcIjo3fSx7XCJpZFwiOjgsXCJjYXB0");
            result.Append("aW9uXCI6XCLmmK/lkKblt7Lnu4/mlK/ku5jnu5lKYWNrXCIsXCJOYW1lXCI6XCJJc1BheVwiLFwiSXNBdXRvSW5jcmVtZW50XCI6ZmFsc2UsXCJDYW5OdWxsXCI6dHJ1ZSxcImRiVHlwZVwiOlwiYml0XCIsXCJsZW5ndGhcIjpcIlwiLFwiZGVmYXVsdFZhbHVlXCI6");
            result.Append("XCIwXCIsXCJUYWJsZUlEXCI6MSxcIklzUEtJRFwiOmZhbHNlLFwib3JkZXJpZFwiOjh9LHtcImlkXCI6MTIsXCJjYXB0aW9uXCI6XCLnmbvpmYblkI7nlJ/miJDnmoTllK/kuIDnoIFcIixcIk5hbWVcIjpcIkxvZ2luQ29kZVwiLFwiSXNBdXRvSW5jcmVtZW50XCI6");
            result.Append("ZmFsc2UsXCJDYW5OdWxsXCI6dHJ1ZSxcImRiVHlwZVwiOlwidmFyY2hhclwiLFwibGVuZ3RoXCI6XCI1MFwiLFwiVGFibGVJRFwiOjEsXCJJc1BLSURcIjpmYWxzZSxcIm9yZGVyaWRcIjo5fSx7XCJpZFwiOjEzLFwiY2FwdGlvblwiOlwi5rOo5YaM5pe26Ze0XCIs");
            result.Append("XCJOYW1lXCI6XCJSZWdUaW1lXCIsXCJJc0F1dG9JbmNyZW1lbnRcIjpmYWxzZSxcIkNhbk51bGxcIjp0cnVlLFwiZGJUeXBlXCI6XCJkYXRldGltZVwiLFwibGVuZ3RoXCI6XCJcIixcIlRhYmxlSURcIjoxLFwiSXNQS0lEXCI6ZmFsc2UsXCJvcmRlcmlkXCI6Mn1d");
            result.Append("LFwibmV3Q29sdW1uc1wiOltdLFwiY2hhbmdlZENvbHVtbnNcIjpbXSxcImRlbGV0ZWRDb2x1bW5zXCI6W10sXCJJRFhDb25maWdzXCI6W10sXCJJRFwiOjB9In0seyJOYW1lIjoiZGF0YWJhc2VpZCIsIlZhbHVlIjoxfV0sIlJvd1N0YXRlIjowfSx7Ikl0ZW1zIjpb");
            result.Append("eyJOYW1lIjoiaWQiLCJWYWx1ZSI6OX0seyJOYW1lIjoidHlwZSIsIlZhbHVlIjoiQ2hhbmdlVGFibGVBY3Rpb24ifSx7Ik5hbWUiOiJjb250ZW50IiwiVmFsdWUiOiJ7XCJPbGRUYWJsZU5hbWVcIjpcIlVzZXJFZmZlY3RcIixcIk5ld1RhYmxlTmFtZVwiOlwiVXNl");
            result.Append("ckVmZmVjdFwiLFwib3RoZXJDb2x1bW5zXCI6W3tcImlkXCI6MTQsXCJOYW1lXCI6XCJpZFwiLFwiSXNBdXRvSW5jcmVtZW50XCI6dHJ1ZSxcIkNhbk51bGxcIjpmYWxzZSxcImRiVHlwZVwiOlwiaW50XCIsXCJUYWJsZUlEXCI6MyxcIklzUEtJRFwiOnRydWUsXCJv");
            result.Append("cmRlcmlkXCI6MH0se1wiaWRcIjoxNSxcIk5hbWVcIjpcIlVzZXJJZFwiLFwiSXNBdXRvSW5jcmVtZW50XCI6ZmFsc2UsXCJDYW5OdWxsXCI6dHJ1ZSxcImRiVHlwZVwiOlwiaW50XCIsXCJsZW5ndGhcIjpcIlwiLFwiVGFibGVJRFwiOjMsXCJJc1BLSURcIjpmYWxz");
            result.Append("ZSxcIm9yZGVyaWRcIjoxfSx7XCJpZFwiOjE2LFwiTmFtZVwiOlwiRmlsZU5hbWVcIixcIklzQXV0b0luY3JlbWVudFwiOmZhbHNlLFwiQ2FuTnVsbFwiOnRydWUsXCJkYlR5cGVcIjpcInZhcmNoYXJcIixcImxlbmd0aFwiOlwiNTBcIixcIlRhYmxlSURcIjozLFwi");
            result.Append("SXNQS0lEXCI6ZmFsc2UsXCJvcmRlcmlkXCI6Mn0se1wiaWRcIjoxNyxcImNhcHRpb25cIjpcIuexu+Wei1wiLFwiTmFtZVwiOlwiVHlwZVwiLFwiSXNBdXRvSW5jcmVtZW50XCI6ZmFsc2UsXCJDYW5OdWxsXCI6dHJ1ZSxcImRiVHlwZVwiOlwiaW50XCIsXCJFbnVt");
            result.Append("RGVmaW5lXCI6XCJQcm9qZWN0PTEsXFxuVHJhY2s9MlwiLFwibGVuZ3RoXCI6XCJcIixcImRlZmF1bHRWYWx1ZVwiOlwiMVwiLFwiVGFibGVJRFwiOjMsXCJJc1BLSURcIjpmYWxzZSxcIm9yZGVyaWRcIjozfV0sXCJuZXdDb2x1bW5zXCI6W3tcImlkXCI6MTgsXCJO");
            result.Append("YW1lXCI6XCJOYW1lXCIsXCJJc0F1dG9JbmNyZW1lbnRcIjpmYWxzZSxcIkNhbk51bGxcIjp0cnVlLFwiZGJUeXBlXCI6XCJ2YXJjaGFyXCIsXCJsZW5ndGhcIjpcIjUwXCIsXCJUYWJsZUlEXCI6MyxcIklzUEtJRFwiOmZhbHNlLFwib3JkZXJpZFwiOjJ9XSxcImNo");
            result.Append("YW5nZWRDb2x1bW5zXCI6W10sXCJkZWxldGVkQ29sdW1uc1wiOltdLFwiSURYQ29uZmlnc1wiOltdLFwiSURcIjowfSJ9LHsiTmFtZSI6ImRhdGFiYXNlaWQiLCJWYWx1ZSI6MX1dLCJSb3dTdGF0ZSI6MH0seyJJdGVtcyI6W3siTmFtZSI6ImlkIiwiVmFsdWUiOjEw");
            result.Append("fSx7Ik5hbWUiOiJ0eXBlIiwiVmFsdWUiOiJDaGFuZ2VUYWJsZUFjdGlvbiJ9LHsiTmFtZSI6ImNvbnRlbnQiLCJWYWx1ZSI6IntcIk9sZFRhYmxlTmFtZVwiOlwiVXNlckVmZmVjdFwiLFwiTmV3VGFibGVOYW1lXCI6XCJVc2VyRWZmZWN0XCIsXCJvdGhlckNvbHVt");
            result.Append("bnNcIjpbe1wiaWRcIjoxNCxcIk5hbWVcIjpcImlkXCIsXCJJc0F1dG9JbmNyZW1lbnRcIjp0cnVlLFwiQ2FuTnVsbFwiOmZhbHNlLFwiZGJUeXBlXCI6XCJpbnRcIixcIlRhYmxlSURcIjozLFwiSXNQS0lEXCI6dHJ1ZSxcIm9yZGVyaWRcIjowfSx7XCJpZFwiOjE1");
            result.Append("LFwiTmFtZVwiOlwiVXNlcklkXCIsXCJJc0F1dG9JbmNyZW1lbnRcIjpmYWxzZSxcIkNhbk51bGxcIjp0cnVlLFwiZGJUeXBlXCI6XCJpbnRcIixcImxlbmd0aFwiOlwiXCIsXCJUYWJsZUlEXCI6MyxcIklzUEtJRFwiOmZhbHNlLFwib3JkZXJpZFwiOjF9LHtcImlk");
            result.Append("XCI6MTYsXCJOYW1lXCI6XCJGaWxlTmFtZVwiLFwiSXNBdXRvSW5jcmVtZW50XCI6ZmFsc2UsXCJDYW5OdWxsXCI6dHJ1ZSxcImRiVHlwZVwiOlwidmFyY2hhclwiLFwibGVuZ3RoXCI6XCI1MFwiLFwiVGFibGVJRFwiOjMsXCJJc1BLSURcIjpmYWxzZSxcIm9yZGVy");
            result.Append("aWRcIjozfSx7XCJpZFwiOjE3LFwiY2FwdGlvblwiOlwi57G75Z6LXCIsXCJOYW1lXCI6XCJUeXBlXCIsXCJJc0F1dG9JbmNyZW1lbnRcIjpmYWxzZSxcIkNhbk51bGxcIjp0cnVlLFwiZGJUeXBlXCI6XCJpbnRcIixcIkVudW1EZWZpbmVcIjpcIlByb2plY3Q9MSxc");
            result.Append("XG5UcmFjaz0yXCIsXCJsZW5ndGhcIjpcIlwiLFwiZGVmYXVsdFZhbHVlXCI6XCIxXCIsXCJUYWJsZUlEXCI6MyxcIklzUEtJRFwiOmZhbHNlLFwib3JkZXJpZFwiOjR9LHtcImlkXCI6MTgsXCJOYW1lXCI6XCJOYW1lXCIsXCJJc0F1dG9JbmNyZW1lbnRcIjpmYWxz");
            result.Append("ZSxcIkNhbk51bGxcIjp0cnVlLFwiZGJUeXBlXCI6XCJ2YXJjaGFyXCIsXCJsZW5ndGhcIjpcIjUwXCIsXCJUYWJsZUlEXCI6MyxcIklzUEtJRFwiOmZhbHNlLFwib3JkZXJpZFwiOjJ9XSxcIm5ld0NvbHVtbnNcIjpbe1wiaWRcIjoxOSxcIk5hbWVcIjpcIlZlcnNp");
            result.Append("b25cIixcIklzQXV0b0luY3JlbWVudFwiOmZhbHNlLFwiQ2FuTnVsbFwiOnRydWUsXCJkYlR5cGVcIjpcImludFwiLFwibGVuZ3RoXCI6XCJcIixcImRlZmF1bHRWYWx1ZVwiOlwiMFwiLFwiVGFibGVJRFwiOjMsXCJJc1BLSURcIjpmYWxzZSxcIm9yZGVyaWRcIjo1");
            result.Append("fV0sXCJjaGFuZ2VkQ29sdW1uc1wiOltdLFwiZGVsZXRlZENvbHVtbnNcIjpbXSxcIklEWENvbmZpZ3NcIjpbXSxcIklEXCI6MH0ifSx7Ik5hbWUiOiJkYXRhYmFzZWlkIiwiVmFsdWUiOjF9XSwiUm93U3RhdGUiOjB9LHsiSXRlbXMiOlt7Ik5hbWUiOiJpZCIsIlZh");
            result.Append("bHVlIjoxMX0seyJOYW1lIjoidHlwZSIsIlZhbHVlIjoiQ3JlYXRlVGFibGVBY3Rpb24ifSx7Ik5hbWUiOiJjb250ZW50IiwiVmFsdWUiOiJ7XCJUYWJsZVwiOntcImlkXCI6NCxcIk5hbWVcIjpcIkFkbWluVXNlclwiLFwiRGF0YWJhc2VJRFwiOjEsXCJpTG9ja1wi");
            result.Append("OjB9LFwiQ29sdW1uc1wiOlt7XCJpZFwiOjIwLFwiTmFtZVwiOlwiaWRcIixcIklzQXV0b0luY3JlbWVudFwiOnRydWUsXCJDYW5OdWxsXCI6ZmFsc2UsXCJkYlR5cGVcIjpcImludFwiLFwiVGFibGVJRFwiOjQsXCJJc1BLSURcIjp0cnVlLFwib3JkZXJpZFwiOjB9");
            result.Append("LHtcImlkXCI6MjEsXCJOYW1lXCI6XCJVc2VyTmFtZVwiLFwiSXNBdXRvSW5jcmVtZW50XCI6ZmFsc2UsXCJDYW5OdWxsXCI6dHJ1ZSxcImRiVHlwZVwiOlwidmFyY2hhclwiLFwibGVuZ3RoXCI6XCI1MFwiLFwiVGFibGVJRFwiOjQsXCJJc1BLSURcIjpmYWxzZSxc");
            result.Append("Im9yZGVyaWRcIjoxfSx7XCJpZFwiOjIyLFwiTmFtZVwiOlwiUGFzc3dvcmRcIixcIklzQXV0b0luY3JlbWVudFwiOmZhbHNlLFwiQ2FuTnVsbFwiOnRydWUsXCJkYlR5cGVcIjpcInZhcmNoYXJcIixcImxlbmd0aFwiOlwiNTBcIixcIlRhYmxlSURcIjo0LFwiSXNQ");
            result.Append("S0lEXCI6ZmFsc2UsXCJvcmRlcmlkXCI6Mn0se1wiaWRcIjoyMyxcIk5hbWVcIjpcIlJvbGVcIixcIklzQXV0b0luY3JlbWVudFwiOmZhbHNlLFwiQ2FuTnVsbFwiOnRydWUsXCJkYlR5cGVcIjpcImludFwiLFwiRW51bURlZmluZVwiOlwi6LaF57qn566h55CG5ZGY");
            result.Append("ID0gMTw8MCxcXG7mma7pgJrlkZjlt6UgPSAxPDwxXFxuXCIsXCJsZW5ndGhcIjpcIlwiLFwiVGFibGVJRFwiOjQsXCJJc1BLSURcIjpmYWxzZSxcIm9yZGVyaWRcIjozfV0sXCJJRFhDb25maWdzXCI6W10sXCJJRFwiOjB9In0seyJOYW1lIjoiZGF0YWJhc2VpZCIs");
            result.Append("IlZhbHVlIjoxfV0sIlJvd1N0YXRlIjowfSx7Ikl0ZW1zIjpbeyJOYW1lIjoiaWQiLCJWYWx1ZSI6MTJ9LHsiTmFtZSI6InR5cGUiLCJWYWx1ZSI6IkNoYW5nZVRhYmxlQWN0aW9uIn0seyJOYW1lIjoiY29udGVudCIsIlZhbHVlIjoie1wiT2xkVGFibGVOYW1lXCI6");
            result.Append("XCJTeXN0ZW1SZWdDb2RlXCIsXCJOZXdUYWJsZU5hbWVcIjpcIlN5c3RlbVJlZ0NvZGVcIixcIm90aGVyQ29sdW1uc1wiOlt7XCJpZFwiOjksXCJOYW1lXCI6XCJpZFwiLFwiSXNBdXRvSW5jcmVtZW50XCI6dHJ1ZSxcIkNhbk51bGxcIjpmYWxzZSxcImRiVHlwZVwi");
            result.Append("OlwiaW50XCIsXCJUYWJsZUlEXCI6MixcIklzUEtJRFwiOnRydWUsXCJvcmRlcmlkXCI6MH0se1wiaWRcIjoxMCxcImNhcHRpb25cIjpcIuWUr+S4gOiupOivgeeggVwiLFwiTmFtZVwiOlwiUmVnR3VpZFwiLFwiSXNBdXRvSW5jcmVtZW50XCI6ZmFsc2UsXCJDYW5O");
            result.Append("dWxsXCI6dHJ1ZSxcImRiVHlwZVwiOlwidmFyY2hhclwiLFwibGVuZ3RoXCI6XCI1MFwiLFwiVGFibGVJRFwiOjIsXCJJc1BLSURcIjpmYWxzZSxcIm9yZGVyaWRcIjoxfSx7XCJpZFwiOjExLFwiY2FwdGlvblwiOlwi5L2/55So55qE55So5oi3aWRcIixcIk5hbWVc");
            result.Append("IjpcIlVzZXJJZFwiLFwiSXNBdXRvSW5jcmVtZW50XCI6ZmFsc2UsXCJDYW5OdWxsXCI6dHJ1ZSxcImRiVHlwZVwiOlwiaW50XCIsXCJsZW5ndGhcIjpcIlwiLFwiVGFibGVJRFwiOjIsXCJJc1BLSURcIjpmYWxzZSxcIm9yZGVyaWRcIjoyfV0sXCJuZXdDb2x1bW5z");
            result.Append("XCI6W3tcImlkXCI6MjQsXCJjYXB0aW9uXCI6XCLosIHnlJ/miJDnmoRcIixcIk5hbWVcIjpcIk1ha2VyVXNlcklkXCIsXCJJc0F1dG9JbmNyZW1lbnRcIjpmYWxzZSxcIkNhbk51bGxcIjp0cnVlLFwiZGJUeXBlXCI6XCJpbnRcIixcImxlbmd0aFwiOlwiXCIsXCJU");
            result.Append("YWJsZUlEXCI6MixcIklzUEtJRFwiOmZhbHNlLFwib3JkZXJpZFwiOjN9XSxcImNoYW5nZWRDb2x1bW5zXCI6W10sXCJkZWxldGVkQ29sdW1uc1wiOltdLFwiSURYQ29uZmlnc1wiOltdLFwiSURcIjowfSJ9LHsiTmFtZSI6ImRhdGFiYXNlaWQiLCJWYWx1ZSI6MX1d");
            result.Append("LCJSb3dTdGF0ZSI6MH0seyJJdGVtcyI6W3siTmFtZSI6ImlkIiwiVmFsdWUiOjEzfSx7Ik5hbWUiOiJ0eXBlIiwiVmFsdWUiOiJDaGFuZ2VUYWJsZUFjdGlvbiJ9LHsiTmFtZSI6ImNvbnRlbnQiLCJWYWx1ZSI6IntcIk9sZFRhYmxlTmFtZVwiOlwiVXNlckluZm9c");
            result.Append("IixcIk5ld1RhYmxlTmFtZVwiOlwiVXNlckluZm9cIixcIm90aGVyQ29sdW1uc1wiOlt7XCJpZFwiOjEsXCJOYW1lXCI6XCJpZFwiLFwiSXNBdXRvSW5jcmVtZW50XCI6dHJ1ZSxcIkNhbk51bGxcIjpmYWxzZSxcImRiVHlwZVwiOlwiaW50XCIsXCJUYWJsZUlEXCI6");
            result.Append("MSxcIklzUEtJRFwiOnRydWUsXCJvcmRlcmlkXCI6MH0se1wiaWRcIjoyLFwiY2FwdGlvblwiOlwi5rOo5YaM5ZSv5LiA6K6k6K+B56CBXCIsXCJOYW1lXCI6XCJSZWdHdWlkXCIsXCJJc0F1dG9JbmNyZW1lbnRcIjpmYWxzZSxcIkNhbk51bGxcIjp0cnVlLFwiZGJU");
            result.Append("eXBlXCI6XCJ2YXJjaGFyXCIsXCJsZW5ndGhcIjpcIjUwXCIsXCJUYWJsZUlEXCI6MSxcIklzUEtJRFwiOmZhbHNlLFwib3JkZXJpZFwiOjF9LHtcImlkXCI6MyxcIk5hbWVcIjpcIlVzZXJOYW1lXCIsXCJJc0F1dG9JbmNyZW1lbnRcIjpmYWxzZSxcIkNhbk51bGxc");
            result.Append("Ijp0cnVlLFwiZGJUeXBlXCI6XCJ2YXJjaGFyXCIsXCJsZW5ndGhcIjpcIjUwXCIsXCJUYWJsZUlEXCI6MSxcIklzUEtJRFwiOmZhbHNlLFwib3JkZXJpZFwiOjN9LHtcImlkXCI6NCxcIk5hbWVcIjpcIlBhc3N3b3JkXCIsXCJJc0F1dG9JbmNyZW1lbnRcIjpmYWxz");
            result.Append("ZSxcIkNhbk51bGxcIjp0cnVlLFwiZGJUeXBlXCI6XCJ2YXJjaGFyXCIsXCJsZW5ndGhcIjpcIjEwMFwiLFwiVGFibGVJRFwiOjEsXCJJc1BLSURcIjpmYWxzZSxcIm9yZGVyaWRcIjo0fSx7XCJpZFwiOjUsXCJjYXB0aW9uXCI6XCLmiYvmnLrlj7dcIixcIk5hbWVc");
            result.Append("IjpcIlBob25lTnVtYmVyXCIsXCJJc0F1dG9JbmNyZW1lbnRcIjpmYWxzZSxcIkNhbk51bGxcIjp0cnVlLFwiZGJUeXBlXCI6XCJ2YXJjaGFyXCIsXCJsZW5ndGhcIjpcIjUwXCIsXCJUYWJsZUlEXCI6MSxcIklzUEtJRFwiOmZhbHNlLFwib3JkZXJpZFwiOjV9LHtc");
            result.Append("ImlkXCI6NixcIk5hbWVcIjpcIkVtYWlsXCIsXCJJc0F1dG9JbmNyZW1lbnRcIjpmYWxzZSxcIkNhbk51bGxcIjp0cnVlLFwiZGJUeXBlXCI6XCJ2YXJjaGFyXCIsXCJsZW5ndGhcIjpcIjUwXCIsXCJUYWJsZUlEXCI6MSxcIklzUEtJRFwiOmZhbHNlLFwib3JkZXJp");
            result.Append("ZFwiOjZ9LHtcImlkXCI6NyxcImNhcHRpb25cIjpcIuW5s+WPsFwiLFwiTmFtZVwiOlwiUGxhdGZvcm1cIixcIklzQXV0b0luY3JlbWVudFwiOmZhbHNlLFwiQ2FuTnVsbFwiOnRydWUsXCJkYlR5cGVcIjpcInZhcmNoYXJcIixcImxlbmd0aFwiOlwiNTBcIixcIlRh");
            result.Append("YmxlSURcIjoxLFwiSXNQS0lEXCI6ZmFsc2UsXCJvcmRlcmlkXCI6N30se1wiaWRcIjo4LFwiY2FwdGlvblwiOlwi5piv5ZCm5bey57uP5pSv5LuY57uZSmFja1wiLFwiTmFtZVwiOlwiSXNQYXlcIixcIklzQXV0b0luY3JlbWVudFwiOmZhbHNlLFwiQ2FuTnVsbFwi");
            result.Append("OnRydWUsXCJkYlR5cGVcIjpcImJpdFwiLFwibGVuZ3RoXCI6XCJcIixcImRlZmF1bHRWYWx1ZVwiOlwiMFwiLFwiVGFibGVJRFwiOjEsXCJJc1BLSURcIjpmYWxzZSxcIm9yZGVyaWRcIjo4fSx7XCJpZFwiOjEyLFwiY2FwdGlvblwiOlwi55m76ZmG5ZCO55Sf5oiQ");
            result.Append("55qE5ZSv5LiA56CBXCIsXCJOYW1lXCI6XCJMb2dpbkNvZGVcIixcIklzQXV0b0luY3JlbWVudFwiOmZhbHNlLFwiQ2FuTnVsbFwiOnRydWUsXCJkYlR5cGVcIjpcInZhcmNoYXJcIixcImxlbmd0aFwiOlwiNTBcIixcIlRhYmxlSURcIjoxLFwiSXNQS0lEXCI6ZmFs");
            result.Append("c2UsXCJvcmRlcmlkXCI6OX0se1wiaWRcIjoxMyxcImNhcHRpb25cIjpcIuazqOWGjOaXtumXtFwiLFwiTmFtZVwiOlwiUmVnVGltZVwiLFwiSXNBdXRvSW5jcmVtZW50XCI6ZmFsc2UsXCJDYW5OdWxsXCI6dHJ1ZSxcImRiVHlwZVwiOlwiZGF0ZXRpbWVcIixcImxl");
            result.Append("bmd0aFwiOlwiXCIsXCJUYWJsZUlEXCI6MSxcIklzUEtJRFwiOmZhbHNlLFwib3JkZXJpZFwiOjJ9XSxcIm5ld0NvbHVtbnNcIjpbe1wiaWRcIjoyNSxcImNhcHRpb25cIjpcIuacgOWQjuS4gOasoeiwg+eUqENoZWNrVXNlcuaXtumXtFwiLFwiTmFtZVwiOlwiTGFz");
            result.Append("dENoZWNrVGltZVwiLFwiSXNBdXRvSW5jcmVtZW50XCI6ZmFsc2UsXCJDYW5OdWxsXCI6dHJ1ZSxcImRiVHlwZVwiOlwiZGF0ZXRpbWVcIixcImxlbmd0aFwiOlwiXCIsXCJUYWJsZUlEXCI6MSxcIklzUEtJRFwiOmZhbHNlLFwib3JkZXJpZFwiOjEwfV0sXCJjaGFu");
            result.Append("Z2VkQ29sdW1uc1wiOltdLFwiZGVsZXRlZENvbHVtbnNcIjpbXSxcIklEWENvbmZpZ3NcIjpbXSxcIklEXCI6MH0ifSx7Ik5hbWUiOiJkYXRhYmFzZWlkIiwiVmFsdWUiOjF9XSwiUm93U3RhdGUiOjB9LHsiSXRlbXMiOlt7Ik5hbWUiOiJpZCIsIlZhbHVlIjoxNH0s");
            result.Append("eyJOYW1lIjoidHlwZSIsIlZhbHVlIjoiQ2hhbmdlVGFibGVBY3Rpb24ifSx7Ik5hbWUiOiJjb250ZW50IiwiVmFsdWUiOiJ7XCJPbGRUYWJsZU5hbWVcIjpcIlN5c3RlbVJlZ0NvZGVcIixcIk5ld1RhYmxlTmFtZVwiOlwiU3lzdGVtUmVnQ29kZVwiLFwib3RoZXJD");
            result.Append("b2x1bW5zXCI6W3tcImlkXCI6OSxcIk5hbWVcIjpcImlkXCIsXCJJc0F1dG9JbmNyZW1lbnRcIjp0cnVlLFwiQ2FuTnVsbFwiOmZhbHNlLFwiZGJUeXBlXCI6XCJpbnRcIixcIlRhYmxlSURcIjoyLFwiSXNQS0lEXCI6dHJ1ZSxcIm9yZGVyaWRcIjowfSx7XCJpZFwi");
            result.Append("OjEwLFwiY2FwdGlvblwiOlwi5ZSv5LiA6K6k6K+B56CBXCIsXCJOYW1lXCI6XCJSZWdHdWlkXCIsXCJJc0F1dG9JbmNyZW1lbnRcIjpmYWxzZSxcIkNhbk51bGxcIjp0cnVlLFwiZGJUeXBlXCI6XCJ2YXJjaGFyXCIsXCJsZW5ndGhcIjpcIjUwXCIsXCJUYWJsZUlE");
            result.Append("XCI6MixcIklzUEtJRFwiOmZhbHNlLFwib3JkZXJpZFwiOjF9LHtcImlkXCI6MTEsXCJjYXB0aW9uXCI6XCLkvb/nlKjnmoTnlKjmiLdpZFwiLFwiTmFtZVwiOlwiVXNlcklkXCIsXCJJc0F1dG9JbmNyZW1lbnRcIjpmYWxzZSxcIkNhbk51bGxcIjp0cnVlLFwiZGJU");
            result.Append("eXBlXCI6XCJpbnRcIixcImxlbmd0aFwiOlwiXCIsXCJUYWJsZUlEXCI6MixcIklzUEtJRFwiOmZhbHNlLFwib3JkZXJpZFwiOjJ9LHtcImlkXCI6MjQsXCJjYXB0aW9uXCI6XCLosIHnlJ/miJDnmoRcIixcIk5hbWVcIjpcIk1ha2VyVXNlcklkXCIsXCJJc0F1dG9J");
            result.Append("bmNyZW1lbnRcIjpmYWxzZSxcIkNhbk51bGxcIjp0cnVlLFwiZGJUeXBlXCI6XCJpbnRcIixcImxlbmd0aFwiOlwiXCIsXCJUYWJsZUlEXCI6MixcIklzUEtJRFwiOmZhbHNlLFwib3JkZXJpZFwiOjN9XSxcIm5ld0NvbHVtbnNcIjpbe1wiaWRcIjoyNixcImNhcHRp");
            result.Append("b25cIjpcIueUn+aIkOaXtumXtFwiLFwiTmFtZVwiOlwiTWFrZVRpbWVcIixcIklzQXV0b0luY3JlbWVudFwiOmZhbHNlLFwiQ2FuTnVsbFwiOnRydWUsXCJkYlR5cGVcIjpcImRhdGV0aW1lXCIsXCJsZW5ndGhcIjpcIlwiLFwiVGFibGVJRFwiOjIsXCJJc1BLSURc");
            result.Append("IjpmYWxzZSxcIm9yZGVyaWRcIjo0fV0sXCJjaGFuZ2VkQ29sdW1uc1wiOltdLFwiZGVsZXRlZENvbHVtbnNcIjpbXSxcIklEWENvbmZpZ3NcIjpbXSxcIklEXCI6MH0ifSx7Ik5hbWUiOiJkYXRhYmFzZWlkIiwiVmFsdWUiOjF9XSwiUm93U3RhdGUiOjB9LHsiSXRl");
            result.Append("bXMiOlt7Ik5hbWUiOiJpZCIsIlZhbHVlIjoxNX0seyJOYW1lIjoidHlwZSIsIlZhbHVlIjoiQ2hhbmdlVGFibGVBY3Rpb24ifSx7Ik5hbWUiOiJjb250ZW50IiwiVmFsdWUiOiJ7XCJPbGRUYWJsZU5hbWVcIjpcIkFkbWluVXNlclwiLFwiTmV3VGFibGVOYW1lXCI6");
            result.Append("XCJBZG1pblVzZXJcIixcIm90aGVyQ29sdW1uc1wiOlt7XCJpZFwiOjIwLFwiTmFtZVwiOlwiaWRcIixcIklzQXV0b0luY3JlbWVudFwiOnRydWUsXCJDYW5OdWxsXCI6ZmFsc2UsXCJkYlR5cGVcIjpcImludFwiLFwiVGFibGVJRFwiOjQsXCJJc1BLSURcIjp0cnVl");
            result.Append("LFwib3JkZXJpZFwiOjB9LHtcImlkXCI6MjEsXCJOYW1lXCI6XCJVc2VyTmFtZVwiLFwiSXNBdXRvSW5jcmVtZW50XCI6ZmFsc2UsXCJDYW5OdWxsXCI6dHJ1ZSxcImRiVHlwZVwiOlwidmFyY2hhclwiLFwibGVuZ3RoXCI6XCI1MFwiLFwiVGFibGVJRFwiOjQsXCJJ");
            result.Append("c1BLSURcIjpmYWxzZSxcIm9yZGVyaWRcIjoxfSx7XCJpZFwiOjIyLFwiTmFtZVwiOlwiUGFzc3dvcmRcIixcIklzQXV0b0luY3JlbWVudFwiOmZhbHNlLFwiQ2FuTnVsbFwiOnRydWUsXCJkYlR5cGVcIjpcInZhcmNoYXJcIixcImxlbmd0aFwiOlwiNTBcIixcIlRh");
            result.Append("YmxlSURcIjo0LFwiSXNQS0lEXCI6ZmFsc2UsXCJvcmRlcmlkXCI6Mn0se1wiaWRcIjoyMyxcIk5hbWVcIjpcIlJvbGVcIixcIklzQXV0b0luY3JlbWVudFwiOmZhbHNlLFwiQ2FuTnVsbFwiOnRydWUsXCJkYlR5cGVcIjpcImludFwiLFwiRW51bURlZmluZVwiOlwi");
            result.Append("6LaF57qn566h55CG5ZGYID0gMTw8MCxcXG7mma7pgJrlkZjlt6UgPSAxPDwxXFxuXCIsXCJsZW5ndGhcIjpcIlwiLFwiVGFibGVJRFwiOjQsXCJJc1BLSURcIjpmYWxzZSxcIm9yZGVyaWRcIjozfV0sXCJuZXdDb2x1bW5zXCI6W3tcImlkXCI6MjcsXCJjYXB0aW9u");
            result.Append("XCI6XCLmiYvmnLrlj7dcIixcIk5hbWVcIjpcIlBob25lTnVtYmVyXCIsXCJJc0F1dG9JbmNyZW1lbnRcIjpmYWxzZSxcIkNhbk51bGxcIjp0cnVlLFwiZGJUeXBlXCI6XCJ2YXJjaGFyXCIsXCJsZW5ndGhcIjpcIjUwXCIsXCJUYWJsZUlEXCI6NCxcIklzUEtJRFwi");
            result.Append("OmZhbHNlLFwib3JkZXJpZFwiOjR9XSxcImNoYW5nZWRDb2x1bW5zXCI6W10sXCJkZWxldGVkQ29sdW1uc1wiOltdLFwiSURYQ29uZmlnc1wiOltdLFwiSURcIjowfSJ9LHsiTmFtZSI6ImRhdGFiYXNlaWQiLCJWYWx1ZSI6MX1dLCJSb3dTdGF0ZSI6MH0seyJJdGVt");
            result.Append("cyI6W3siTmFtZSI6ImlkIiwiVmFsdWUiOjE2fSx7Ik5hbWUiOiJ0eXBlIiwiVmFsdWUiOiJDaGFuZ2VUYWJsZUFjdGlvbiJ9LHsiTmFtZSI6ImNvbnRlbnQiLCJWYWx1ZSI6IntcIk9sZFRhYmxlTmFtZVwiOlwiVXNlckluZm9cIixcIk5ld1RhYmxlTmFtZVwiOlwi");
            result.Append("VXNlckluZm9cIixcIm90aGVyQ29sdW1uc1wiOlt7XCJpZFwiOjEsXCJOYW1lXCI6XCJpZFwiLFwiSXNBdXRvSW5jcmVtZW50XCI6dHJ1ZSxcIkNhbk51bGxcIjpmYWxzZSxcImRiVHlwZVwiOlwiaW50XCIsXCJUYWJsZUlEXCI6MSxcIklzUEtJRFwiOnRydWUsXCJv");
            result.Append("cmRlcmlkXCI6MH0se1wiaWRcIjoyLFwiY2FwdGlvblwiOlwi5rOo5YaM5ZSv5LiA6K6k6K+B56CBXCIsXCJOYW1lXCI6XCJSZWdHdWlkXCIsXCJJc0F1dG9JbmNyZW1lbnRcIjpmYWxzZSxcIkNhbk51bGxcIjp0cnVlLFwiZGJUeXBlXCI6XCJ2YXJjaGFyXCIsXCJs");
            result.Append("ZW5ndGhcIjpcIjUwXCIsXCJUYWJsZUlEXCI6MSxcIklzUEtJRFwiOmZhbHNlLFwib3JkZXJpZFwiOjF9LHtcImlkXCI6MyxcIk5hbWVcIjpcIlVzZXJOYW1lXCIsXCJJc0F1dG9JbmNyZW1lbnRcIjpmYWxzZSxcIkNhbk51bGxcIjp0cnVlLFwiZGJUeXBlXCI6XCJ2");
            result.Append("YXJjaGFyXCIsXCJsZW5ndGhcIjpcIjUwXCIsXCJUYWJsZUlEXCI6MSxcIklzUEtJRFwiOmZhbHNlLFwib3JkZXJpZFwiOjN9LHtcImlkXCI6NCxcIk5hbWVcIjpcIlBhc3N3b3JkXCIsXCJJc0F1dG9JbmNyZW1lbnRcIjpmYWxzZSxcIkNhbk51bGxcIjp0cnVlLFwi");
            result.Append("ZGJUeXBlXCI6XCJ2YXJjaGFyXCIsXCJsZW5ndGhcIjpcIjEwMFwiLFwiVGFibGVJRFwiOjEsXCJJc1BLSURcIjpmYWxzZSxcIm9yZGVyaWRcIjo0fSx7XCJpZFwiOjUsXCJjYXB0aW9uXCI6XCLmiYvmnLrlj7dcIixcIk5hbWVcIjpcIlBob25lTnVtYmVyXCIsXCJJ");
            result.Append("c0F1dG9JbmNyZW1lbnRcIjpmYWxzZSxcIkNhbk51bGxcIjp0cnVlLFwiZGJUeXBlXCI6XCJ2YXJjaGFyXCIsXCJsZW5ndGhcIjpcIjUwXCIsXCJUYWJsZUlEXCI6MSxcIklzUEtJRFwiOmZhbHNlLFwib3JkZXJpZFwiOjV9LHtcImlkXCI6NixcIk5hbWVcIjpcIkVt");
            result.Append("YWlsXCIsXCJJc0F1dG9JbmNyZW1lbnRcIjpmYWxzZSxcIkNhbk51bGxcIjp0cnVlLFwiZGJUeXBlXCI6XCJ2YXJjaGFyXCIsXCJsZW5ndGhcIjpcIjUwXCIsXCJUYWJsZUlEXCI6MSxcIklzUEtJRFwiOmZhbHNlLFwib3JkZXJpZFwiOjZ9LHtcImlkXCI6NyxcImNh");
            result.Append("cHRpb25cIjpcIuW5s+WPsFwiLFwiTmFtZVwiOlwiUGxhdGZvcm1cIixcIklzQXV0b0luY3JlbWVudFwiOmZhbHNlLFwiQ2FuTnVsbFwiOnRydWUsXCJkYlR5cGVcIjpcInZhcmNoYXJcIixcImxlbmd0aFwiOlwiNTBcIixcIlRhYmxlSURcIjoxLFwiSXNQS0lEXCI6");
            result.Append("ZmFsc2UsXCJvcmRlcmlkXCI6N30se1wiaWRcIjo4LFwiY2FwdGlvblwiOlwi5piv5ZCm5bey57uP5pSv5LuY57uZSmFja1wiLFwiTmFtZVwiOlwiSXNQYXlcIixcIklzQXV0b0luY3JlbWVudFwiOmZhbHNlLFwiQ2FuTnVsbFwiOnRydWUsXCJkYlR5cGVcIjpcImJp");
            result.Append("dFwiLFwibGVuZ3RoXCI6XCJcIixcImRlZmF1bHRWYWx1ZVwiOlwiMFwiLFwiVGFibGVJRFwiOjEsXCJJc1BLSURcIjpmYWxzZSxcIm9yZGVyaWRcIjo4fSx7XCJpZFwiOjEyLFwiY2FwdGlvblwiOlwi55m76ZmG5ZCO55Sf5oiQ55qE5ZSv5LiA56CBXCIsXCJOYW1l");
            result.Append("XCI6XCJMb2dpbkNvZGVcIixcIklzQXV0b0luY3JlbWVudFwiOmZhbHNlLFwiQ2FuTnVsbFwiOnRydWUsXCJkYlR5cGVcIjpcInZhcmNoYXJcIixcImxlbmd0aFwiOlwiNTBcIixcIlRhYmxlSURcIjoxLFwiSXNQS0lEXCI6ZmFsc2UsXCJvcmRlcmlkXCI6OX0se1wi");
            result.Append("aWRcIjoxMyxcImNhcHRpb25cIjpcIuazqOWGjOaXtumXtFwiLFwiTmFtZVwiOlwiUmVnVGltZVwiLFwiSXNBdXRvSW5jcmVtZW50XCI6ZmFsc2UsXCJDYW5OdWxsXCI6dHJ1ZSxcImRiVHlwZVwiOlwiZGF0ZXRpbWVcIixcImxlbmd0aFwiOlwiXCIsXCJUYWJsZUlE");
            result.Append("XCI6MSxcIklzUEtJRFwiOmZhbHNlLFwib3JkZXJpZFwiOjJ9LHtcImlkXCI6MjUsXCJjYXB0aW9uXCI6XCLmnIDlkI7kuIDmrKHosIPnlKhDaGVja1VzZXLml7bpl7RcIixcIk5hbWVcIjpcIkxhc3RDaGVja1RpbWVcIixcIklzQXV0b0luY3JlbWVudFwiOmZhbHNl");
            result.Append("LFwiQ2FuTnVsbFwiOnRydWUsXCJkYlR5cGVcIjpcImRhdGV0aW1lXCIsXCJsZW5ndGhcIjpcIlwiLFwiVGFibGVJRFwiOjEsXCJJc1BLSURcIjpmYWxzZSxcIm9yZGVyaWRcIjoxMH1dLFwibmV3Q29sdW1uc1wiOlt7XCJpZFwiOjI4LFwiTmFtZVwiOlwiRGlzYWJs");
            result.Append("ZVwiLFwiSXNBdXRvSW5jcmVtZW50XCI6ZmFsc2UsXCJDYW5OdWxsXCI6dHJ1ZSxcImRiVHlwZVwiOlwiYml0XCIsXCJsZW5ndGhcIjpcIlwiLFwiZGVmYXVsdFZhbHVlXCI6XCIwXCIsXCJUYWJsZUlEXCI6MSxcIklzUEtJRFwiOmZhbHNlLFwib3JkZXJpZFwiOjEx");
            result.Append("fV0sXCJjaGFuZ2VkQ29sdW1uc1wiOltdLFwiZGVsZXRlZENvbHVtbnNcIjpbXSxcIklEWENvbmZpZ3NcIjpbXSxcIklEXCI6MH0ifSx7Ik5hbWUiOiJkYXRhYmFzZWlkIiwiVmFsdWUiOjF9XSwiUm93U3RhdGUiOjB9LHsiSXRlbXMiOlt7Ik5hbWUiOiJpZCIsIlZh");
            result.Append("bHVlIjoyN30seyJOYW1lIjoidHlwZSIsIlZhbHVlIjoiQ2hhbmdlVGFibGVBY3Rpb24ifSx7Ik5hbWUiOiJjb250ZW50IiwiVmFsdWUiOiJ7XCJPbGRUYWJsZU5hbWVcIjpcIlVzZXJFZmZlY3RcIixcIk5ld1RhYmxlTmFtZVwiOlwiVXNlckVmZmVjdFwiLFwib3Ro");
            result.Append("ZXJDb2x1bW5zXCI6W3tcImlkXCI6MTQsXCJOYW1lXCI6XCJpZFwiLFwiSXNBdXRvSW5jcmVtZW50XCI6dHJ1ZSxcIkNhbk51bGxcIjpmYWxzZSxcImRiVHlwZVwiOlwiaW50XCIsXCJUYWJsZUlEXCI6MyxcIklzUEtJRFwiOnRydWUsXCJvcmRlcmlkXCI6MH0se1wi");
            result.Append("aWRcIjoxNSxcIk5hbWVcIjpcIlVzZXJJZFwiLFwiSXNBdXRvSW5jcmVtZW50XCI6ZmFsc2UsXCJDYW5OdWxsXCI6dHJ1ZSxcImRiVHlwZVwiOlwiaW50XCIsXCJsZW5ndGhcIjpcIlwiLFwiVGFibGVJRFwiOjMsXCJJc1BLSURcIjpmYWxzZSxcIm9yZGVyaWRcIjox");
            result.Append("fSx7XCJpZFwiOjE2LFwiTmFtZVwiOlwiRmlsZU5hbWVcIixcIklzQXV0b0luY3JlbWVudFwiOmZhbHNlLFwiQ2FuTnVsbFwiOnRydWUsXCJkYlR5cGVcIjpcInZhcmNoYXJcIixcImxlbmd0aFwiOlwiNTBcIixcIlRhYmxlSURcIjozLFwiSXNQS0lEXCI6ZmFsc2Us");
            result.Append("XCJvcmRlcmlkXCI6M30se1wiaWRcIjoxNyxcImNhcHRpb25cIjpcIuexu+Wei1wiLFwiTmFtZVwiOlwiVHlwZVwiLFwiSXNBdXRvSW5jcmVtZW50XCI6ZmFsc2UsXCJDYW5OdWxsXCI6dHJ1ZSxcImRiVHlwZVwiOlwiaW50XCIsXCJFbnVtRGVmaW5lXCI6XCJQcm9q");
            result.Append("ZWN0PTEsXFxuVHJhY2s9MixcXG5Wc3Q9M1wiLFwibGVuZ3RoXCI6XCJcIixcImRlZmF1bHRWYWx1ZVwiOlwiMVwiLFwiVGFibGVJRFwiOjMsXCJJc1BLSURcIjpmYWxzZSxcIm9yZGVyaWRcIjo0fSx7XCJpZFwiOjE4LFwiTmFtZVwiOlwiTmFtZVwiLFwiSXNBdXRv");
            result.Append("SW5jcmVtZW50XCI6ZmFsc2UsXCJDYW5OdWxsXCI6dHJ1ZSxcImRiVHlwZVwiOlwidmFyY2hhclwiLFwibGVuZ3RoXCI6XCI1MFwiLFwiVGFibGVJRFwiOjMsXCJJc1BLSURcIjpmYWxzZSxcIm9yZGVyaWRcIjoyfSx7XCJpZFwiOjE5LFwiTmFtZVwiOlwiVmVyc2lv");
            result.Append("blwiLFwiSXNBdXRvSW5jcmVtZW50XCI6ZmFsc2UsXCJDYW5OdWxsXCI6dHJ1ZSxcImRiVHlwZVwiOlwiaW50XCIsXCJsZW5ndGhcIjpcIlwiLFwiZGVmYXVsdFZhbHVlXCI6XCIwXCIsXCJUYWJsZUlEXCI6MyxcIklzUEtJRFwiOmZhbHNlLFwib3JkZXJpZFwiOjV9");
            result.Append("XSxcIm5ld0NvbHVtbnNcIjpbXSxcImNoYW5nZWRDb2x1bW5zXCI6W10sXCJkZWxldGVkQ29sdW1uc1wiOltdLFwiSURYQ29uZmlnc1wiOltdLFwiSURcIjowfSJ9LHsiTmFtZSI6ImRhdGFiYXNlaWQiLCJWYWx1ZSI6MX1dLCJSb3dTdGF0ZSI6MH0seyJJdGVtcyI6");
            result.Append("W3siTmFtZSI6ImlkIiwiVmFsdWUiOjExN30seyJOYW1lIjoidHlwZSIsIlZhbHVlIjoiQ2hhbmdlVGFibGVBY3Rpb24ifSx7Ik5hbWUiOiJjb250ZW50IiwiVmFsdWUiOiJ7XCJPbGRUYWJsZU5hbWVcIjpcIlVzZXJJbmZvXCIsXCJOZXdUYWJsZU5hbWVcIjpcIlVz");
            result.Append("ZXJJbmZvXCIsXCJvdGhlckNvbHVtbnNcIjpbe1wiaWRcIjoxLFwiTmFtZVwiOlwiaWRcIixcIklzQXV0b0luY3JlbWVudFwiOnRydWUsXCJDYW5OdWxsXCI6ZmFsc2UsXCJkYlR5cGVcIjpcImludFwiLFwiVGFibGVJRFwiOjEsXCJJc1BLSURcIjp0cnVlLFwib3Jk");
            result.Append("ZXJpZFwiOjB9LHtcImlkXCI6MixcImNhcHRpb25cIjpcIuazqOWGjOWUr+S4gOiupOivgeeggVwiLFwiTmFtZVwiOlwiUmVnR3VpZFwiLFwiSXNBdXRvSW5jcmVtZW50XCI6ZmFsc2UsXCJDYW5OdWxsXCI6dHJ1ZSxcImRiVHlwZVwiOlwidmFyY2hhclwiLFwibGVu");
            result.Append("Z3RoXCI6XCI1MFwiLFwiVGFibGVJRFwiOjEsXCJJc1BLSURcIjpmYWxzZSxcIm9yZGVyaWRcIjoxfSx7XCJpZFwiOjMsXCJOYW1lXCI6XCJVc2VyTmFtZVwiLFwiSXNBdXRvSW5jcmVtZW50XCI6ZmFsc2UsXCJDYW5OdWxsXCI6dHJ1ZSxcImRiVHlwZVwiOlwidmFy");
            result.Append("Y2hhclwiLFwibGVuZ3RoXCI6XCI1MFwiLFwiVGFibGVJRFwiOjEsXCJJc1BLSURcIjpmYWxzZSxcIm9yZGVyaWRcIjozfSx7XCJpZFwiOjQsXCJOYW1lXCI6XCJQYXNzd29yZFwiLFwiSXNBdXRvSW5jcmVtZW50XCI6ZmFsc2UsXCJDYW5OdWxsXCI6dHJ1ZSxcImRi");
            result.Append("VHlwZVwiOlwidmFyY2hhclwiLFwibGVuZ3RoXCI6XCIxMDBcIixcIlRhYmxlSURcIjoxLFwiSXNQS0lEXCI6ZmFsc2UsXCJvcmRlcmlkXCI6NH0se1wiaWRcIjo1LFwiY2FwdGlvblwiOlwi5omL5py65Y+3XCIsXCJOYW1lXCI6XCJQaG9uZU51bWJlclwiLFwiSXNB");
            result.Append("dXRvSW5jcmVtZW50XCI6ZmFsc2UsXCJDYW5OdWxsXCI6dHJ1ZSxcImRiVHlwZVwiOlwidmFyY2hhclwiLFwibGVuZ3RoXCI6XCI1MFwiLFwiVGFibGVJRFwiOjEsXCJJc1BLSURcIjpmYWxzZSxcIm9yZGVyaWRcIjo1fSx7XCJpZFwiOjYsXCJOYW1lXCI6XCJFbWFp");
            result.Append("bFwiLFwiSXNBdXRvSW5jcmVtZW50XCI6ZmFsc2UsXCJDYW5OdWxsXCI6dHJ1ZSxcImRiVHlwZVwiOlwidmFyY2hhclwiLFwibGVuZ3RoXCI6XCI1MFwiLFwiVGFibGVJRFwiOjEsXCJJc1BLSURcIjpmYWxzZSxcIm9yZGVyaWRcIjo2fSx7XCJpZFwiOjcsXCJjYXB0");
            result.Append("aW9uXCI6XCLlubPlj7BcIixcIk5hbWVcIjpcIlBsYXRmb3JtXCIsXCJJc0F1dG9JbmNyZW1lbnRcIjpmYWxzZSxcIkNhbk51bGxcIjp0cnVlLFwiZGJUeXBlXCI6XCJ2YXJjaGFyXCIsXCJsZW5ndGhcIjpcIjUwXCIsXCJUYWJsZUlEXCI6MSxcIklzUEtJRFwiOmZh");
            result.Append("bHNlLFwib3JkZXJpZFwiOjd9LHtcImlkXCI6OCxcImNhcHRpb25cIjpcIuaYr+WQpuW3sue7j+aUr+S7mOe7mUphY2tcIixcIk5hbWVcIjpcIklzUGF5XCIsXCJJc0F1dG9JbmNyZW1lbnRcIjpmYWxzZSxcIkNhbk51bGxcIjp0cnVlLFwiZGJUeXBlXCI6XCJiaXRc");
            result.Append("IixcImxlbmd0aFwiOlwiXCIsXCJkZWZhdWx0VmFsdWVcIjpcIjBcIixcIlRhYmxlSURcIjoxLFwiSXNQS0lEXCI6ZmFsc2UsXCJvcmRlcmlkXCI6OH0se1wiaWRcIjoxMixcImNhcHRpb25cIjpcIueZu+mZhuWQjueUn+aIkOeahOWUr+S4gOeggVwiLFwiTmFtZVwi");
            result.Append("OlwiTG9naW5Db2RlXCIsXCJJc0F1dG9JbmNyZW1lbnRcIjpmYWxzZSxcIkNhbk51bGxcIjp0cnVlLFwiZGJUeXBlXCI6XCJ2YXJjaGFyXCIsXCJsZW5ndGhcIjpcIjUwXCIsXCJUYWJsZUlEXCI6MSxcIklzUEtJRFwiOmZhbHNlLFwib3JkZXJpZFwiOjl9LHtcImlk");
            result.Append("XCI6MTMsXCJjYXB0aW9uXCI6XCLms6jlhozml7bpl7RcIixcIk5hbWVcIjpcIlJlZ1RpbWVcIixcIklzQXV0b0luY3JlbWVudFwiOmZhbHNlLFwiQ2FuTnVsbFwiOnRydWUsXCJkYlR5cGVcIjpcImRhdGV0aW1lXCIsXCJsZW5ndGhcIjpcIlwiLFwiVGFibGVJRFwi");
            result.Append("OjEsXCJJc1BLSURcIjpmYWxzZSxcIm9yZGVyaWRcIjoyfSx7XCJpZFwiOjI1LFwiY2FwdGlvblwiOlwi5pyA5ZCO5LiA5qyh6LCD55SoQ2hlY2tVc2Vy5pe26Ze0XCIsXCJOYW1lXCI6XCJMYXN0Q2hlY2tUaW1lXCIsXCJJc0F1dG9JbmNyZW1lbnRcIjpmYWxzZSxc");
            result.Append("IkNhbk51bGxcIjp0cnVlLFwiZGJUeXBlXCI6XCJkYXRldGltZVwiLFwibGVuZ3RoXCI6XCJcIixcIlRhYmxlSURcIjoxLFwiSXNQS0lEXCI6ZmFsc2UsXCJvcmRlcmlkXCI6MTB9LHtcImlkXCI6MjgsXCJOYW1lXCI6XCJEaXNhYmxlXCIsXCJJc0F1dG9JbmNyZW1l");
            result.Append("bnRcIjpmYWxzZSxcIkNhbk51bGxcIjp0cnVlLFwiZGJUeXBlXCI6XCJiaXRcIixcImxlbmd0aFwiOlwiXCIsXCJkZWZhdWx0VmFsdWVcIjpcIjBcIixcIlRhYmxlSURcIjoxLFwiSXNQS0lEXCI6ZmFsc2UsXCJvcmRlcmlkXCI6MTF9XSxcIm5ld0NvbHVtbnNcIjpb");
            result.Append("e1wiaWRcIjozMzUsXCJjYXB0aW9uXCI6XCLnlKjmiLfop5LoibJcIixcIk5hbWVcIjpcIlJvbGVcIixcIklzQXV0b0luY3JlbWVudFwiOmZhbHNlLFwiQ2FuTnVsbFwiOnRydWUsXCJkYlR5cGVcIjpcImludFwiLFwiRW51bURlZmluZVwiOlwiLy/mma7pgJrnlKjm");
            result.Append("iLdcXG5Ob3JtYWw9MCxcXG4vL+WFgeiuuOi/nOeoi+aOp+WItlxcbkFsbG93UmVtb3RlPTE8PDAsXFxuLy9DWTIy55So5oi3XFxuQ1kyMj0xPDwxIHwgQWxsb3dSZW1vdGUsXFxuXCIsXCJsZW5ndGhcIjpcIlwiLFwiZGVmYXVsdFZhbHVlXCI6XCIwXCIsXCJUYWJs");
            result.Append("ZUlEXCI6MSxcIklzUEtJRFwiOmZhbHNlLFwib3JkZXJpZFwiOjEyfV0sXCJjaGFuZ2VkQ29sdW1uc1wiOltdLFwiZGVsZXRlZENvbHVtbnNcIjpbXSxcIklEWENvbmZpZ3NcIjpbXSxcIklEXCI6MH0ifSx7Ik5hbWUiOiJkYXRhYmFzZWlkIiwiVmFsdWUiOjF9XSwi");
            result.Append("Um93U3RhdGUiOjB9LHsiSXRlbXMiOlt7Ik5hbWUiOiJpZCIsIlZhbHVlIjoxMTh9LHsiTmFtZSI6InR5cGUiLCJWYWx1ZSI6IkNoYW5nZVRhYmxlQWN0aW9uIn0seyJOYW1lIjoiY29udGVudCIsIlZhbHVlIjoie1wiT2xkVGFibGVOYW1lXCI6XCJTeXN0ZW1SZWdD");
            result.Append("b2RlXCIsXCJOZXdUYWJsZU5hbWVcIjpcIlN5c3RlbVJlZ0NvZGVcIixcIm90aGVyQ29sdW1uc1wiOlt7XCJpZFwiOjksXCJOYW1lXCI6XCJpZFwiLFwiSXNBdXRvSW5jcmVtZW50XCI6dHJ1ZSxcIkNhbk51bGxcIjpmYWxzZSxcImRiVHlwZVwiOlwiaW50XCIsXCJU");
            result.Append("YWJsZUlEXCI6MixcIklzUEtJRFwiOnRydWUsXCJvcmRlcmlkXCI6MH0se1wiaWRcIjoxMCxcImNhcHRpb25cIjpcIuWUr+S4gOiupOivgeeggVwiLFwiTmFtZVwiOlwiUmVnR3VpZFwiLFwiSXNBdXRvSW5jcmVtZW50XCI6ZmFsc2UsXCJDYW5OdWxsXCI6dHJ1ZSxc");
            result.Append("ImRiVHlwZVwiOlwidmFyY2hhclwiLFwibGVuZ3RoXCI6XCI1MFwiLFwiVGFibGVJRFwiOjIsXCJJc1BLSURcIjpmYWxzZSxcIm9yZGVyaWRcIjoxfSx7XCJpZFwiOjExLFwiY2FwdGlvblwiOlwi5L2/55So55qE55So5oi3aWRcIixcIk5hbWVcIjpcIlVzZXJJZFwi");
            result.Append("LFwiSXNBdXRvSW5jcmVtZW50XCI6ZmFsc2UsXCJDYW5OdWxsXCI6dHJ1ZSxcImRiVHlwZVwiOlwiaW50XCIsXCJsZW5ndGhcIjpcIlwiLFwiVGFibGVJRFwiOjIsXCJJc1BLSURcIjpmYWxzZSxcIm9yZGVyaWRcIjoyfSx7XCJpZFwiOjI0LFwiY2FwdGlvblwiOlwi");
            result.Append("6LCB55Sf5oiQ55qEXCIsXCJOYW1lXCI6XCJNYWtlclVzZXJJZFwiLFwiSXNBdXRvSW5jcmVtZW50XCI6ZmFsc2UsXCJDYW5OdWxsXCI6dHJ1ZSxcImRiVHlwZVwiOlwiaW50XCIsXCJsZW5ndGhcIjpcIlwiLFwiVGFibGVJRFwiOjIsXCJJc1BLSURcIjpmYWxzZSxc");
            result.Append("Im9yZGVyaWRcIjozfSx7XCJpZFwiOjI2LFwiY2FwdGlvblwiOlwi55Sf5oiQ5pe26Ze0XCIsXCJOYW1lXCI6XCJNYWtlVGltZVwiLFwiSXNBdXRvSW5jcmVtZW50XCI6ZmFsc2UsXCJDYW5OdWxsXCI6dHJ1ZSxcImRiVHlwZVwiOlwiZGF0ZXRpbWVcIixcImxlbmd0");
            result.Append("aFwiOlwiXCIsXCJUYWJsZUlEXCI6MixcIklzUEtJRFwiOmZhbHNlLFwib3JkZXJpZFwiOjR9XSxcIm5ld0NvbHVtbnNcIjpbe1wiaWRcIjozMzYsXCJjYXB0aW9uXCI6XCLnlKjmiLfop5LoibJcIixcIk5hbWVcIjpcIlJvbGVcIixcIklzQXV0b0luY3JlbWVudFwi");
            result.Append("OmZhbHNlLFwiQ2FuTnVsbFwiOnRydWUsXCJkYlR5cGVcIjpcImludFwiLFwiRW51bURlZmluZVwiOlwiLy/mma7pgJrnlKjmiLdcXG5Ob3JtYWw9MCxcXG4vL+WFgeiuuOi/nOeoi+aOp+WItlxcbkFsbG93UmVtb3RlPTE8PDAsXFxuLy9DWTIy55So5oi3XFxuQ1ky");
            result.Append("Mj0xPDwxIHwgQWxsb3dSZW1vdGUsXFxuXCIsXCJsZW5ndGhcIjpcIlwiLFwiZGVmYXVsdFZhbHVlXCI6XCIwXCIsXCJUYWJsZUlEXCI6MixcIklzUEtJRFwiOmZhbHNlLFwib3JkZXJpZFwiOjV9XSxcImNoYW5nZWRDb2x1bW5zXCI6W10sXCJkZWxldGVkQ29sdW1u");
            result.Append("c1wiOltdLFwiSURYQ29uZmlnc1wiOltdLFwiSURcIjowfSJ9LHsiTmFtZSI6ImRhdGFiYXNlaWQiLCJWYWx1ZSI6MX1dLCJSb3dTdGF0ZSI6MH0seyJJdGVtcyI6W3siTmFtZSI6ImlkIiwiVmFsdWUiOjExOX0seyJOYW1lIjoidHlwZSIsIlZhbHVlIjoiQ2hhbmdl");
            result.Append("VGFibGVBY3Rpb24ifSx7Ik5hbWUiOiJjb250ZW50IiwiVmFsdWUiOiJ7XCJPbGRUYWJsZU5hbWVcIjpcIlN5c3RlbVJlZ0NvZGVcIixcIk5ld1RhYmxlTmFtZVwiOlwiU3lzdGVtUmVnQ29kZVwiLFwib3RoZXJDb2x1bW5zXCI6W3tcImlkXCI6OSxcIk5hbWVcIjpc");
            result.Append("ImlkXCIsXCJJc0F1dG9JbmNyZW1lbnRcIjp0cnVlLFwiQ2FuTnVsbFwiOmZhbHNlLFwiZGJUeXBlXCI6XCJpbnRcIixcIlRhYmxlSURcIjoyLFwiSXNQS0lEXCI6dHJ1ZSxcIm9yZGVyaWRcIjowfSx7XCJpZFwiOjEwLFwiY2FwdGlvblwiOlwi5ZSv5LiA6K6k6K+B");
            result.Append("56CBXCIsXCJOYW1lXCI6XCJSZWdHdWlkXCIsXCJJc0F1dG9JbmNyZW1lbnRcIjpmYWxzZSxcIkNhbk51bGxcIjp0cnVlLFwiZGJUeXBlXCI6XCJ2YXJjaGFyXCIsXCJsZW5ndGhcIjpcIjUwXCIsXCJUYWJsZUlEXCI6MixcIklzUEtJRFwiOmZhbHNlLFwib3JkZXJp");
            result.Append("ZFwiOjF9LHtcImlkXCI6MTEsXCJjYXB0aW9uXCI6XCLkvb/nlKjnmoTnlKjmiLdpZFwiLFwiTmFtZVwiOlwiVXNlcklkXCIsXCJJc0F1dG9JbmNyZW1lbnRcIjpmYWxzZSxcIkNhbk51bGxcIjp0cnVlLFwiZGJUeXBlXCI6XCJpbnRcIixcImxlbmd0aFwiOlwiXCIs");
            result.Append("XCJUYWJsZUlEXCI6MixcIklzUEtJRFwiOmZhbHNlLFwib3JkZXJpZFwiOjJ9LHtcImlkXCI6MjQsXCJjYXB0aW9uXCI6XCLosIHnlJ/miJDnmoRcIixcIk5hbWVcIjpcIk1ha2VyVXNlcklkXCIsXCJJc0F1dG9JbmNyZW1lbnRcIjpmYWxzZSxcIkNhbk51bGxcIjp0");
            result.Append("cnVlLFwiZGJUeXBlXCI6XCJpbnRcIixcImxlbmd0aFwiOlwiXCIsXCJUYWJsZUlEXCI6MixcIklzUEtJRFwiOmZhbHNlLFwib3JkZXJpZFwiOjN9LHtcImlkXCI6MjYsXCJjYXB0aW9uXCI6XCLnlJ/miJDml7bpl7RcIixcIk5hbWVcIjpcIk1ha2VUaW1lXCIsXCJJ");
            result.Append("c0F1dG9JbmNyZW1lbnRcIjpmYWxzZSxcIkNhbk51bGxcIjp0cnVlLFwiZGJUeXBlXCI6XCJkYXRldGltZVwiLFwibGVuZ3RoXCI6XCJcIixcIlRhYmxlSURcIjoyLFwiSXNQS0lEXCI6ZmFsc2UsXCJvcmRlcmlkXCI6NH0se1wiaWRcIjozMzYsXCJjYXB0aW9uXCI6");
            result.Append("XCLnlKjmiLfop5LoibJcIixcIk5hbWVcIjpcIlJvbGVcIixcIklzQXV0b0luY3JlbWVudFwiOmZhbHNlLFwiQ2FuTnVsbFwiOnRydWUsXCJkYlR5cGVcIjpcImludFwiLFwiRW51bURlZmluZVwiOlwiLy/mma7pgJrnlKjmiLdcXG5Ob3JtYWw9MCxcXG4vL+WFgeiu");
            result.Append("uOi/nOeoi+aOp+WItlxcbkFsbG93UmVtb3RlPTE8PDAsXFxuLy9DWTIy55So5oi3XFxuQ1kyMj0xPDwxIHwgQWxsb3dSZW1vdGUsXFxuXCIsXCJsZW5ndGhcIjpcIlwiLFwiZGVmYXVsdFZhbHVlXCI6XCIwXCIsXCJUYWJsZUlEXCI6MixcIklzUEtJRFwiOmZhbHNl");
            result.Append("LFwib3JkZXJpZFwiOjV9XSxcIm5ld0NvbHVtbnNcIjpbXSxcImNoYW5nZWRDb2x1bW5zXCI6W10sXCJkZWxldGVkQ29sdW1uc1wiOltdLFwiSURYQ29uZmlnc1wiOltdLFwiSURcIjowfSJ9LHsiTmFtZSI6ImRhdGFiYXNlaWQiLCJWYWx1ZSI6MX1dLCJSb3dTdGF0");
            result.Append("ZSI6MH1dLCJDb2x1bW5zIjpbeyJDb2x1bW5OYW1lIjoiaWQiLCJEYXRhVHlwZSI6IlN5c3RlbS5JbnQ2NCJ9LHsiQ29sdW1uTmFtZSI6InR5cGUiLCJEYXRhVHlwZSI6IlN5c3RlbS5TdHJpbmcifSx7IkNvbHVtbk5hbWUiOiJjb250ZW50IiwiRGF0YVR5cGUiOiJT");
            result.Append("eXN0ZW0uU3RyaW5nIn0seyJDb2x1bW5OYW1lIjoiZGF0YWJhc2VpZCIsIkRhdGFUeXBlIjoiU3lzdGVtLkludDY0In1dfV0sIkRhdGFTZXROYW1lIjoiYzVkYmUwNmEtNDgwNC00NmIyLTk5YWUtMmI1ZmYzYWYxZGZiIn0=");
            return result.ToString();
        }
    }
}
