
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EJ{


    /// <summary>
	/// 项目
	/// </summary>
    public class Project :Way.Lib.DataModel
    {

        /// <summary>
	    /// 
	    /// </summary>
        public  Project()
        {
        }


        System.Nullable<Int32> _id;
        /// <summary>
        /// id
        /// </summary>
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
                    var original = this._id;
                    this._id = value;
                    this.OnPropertyChanged("id",original,value);

                }
            }
        }

        String _Name;
        /// <summary>
        /// Name
        /// </summary>
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
                    var original = this._Name;
                    this._Name = value;
                    this.OnPropertyChanged("Name",original,value);

                }
            }
        }
}}
namespace EJ{

/// <summary>
/// 
/// </summary>
public enum Databases_dbTypeEnum:int
{
    

/// <summary>
/// 
/// </summary>
SqlServer = 1,

/// <summary>
/// 
/// </summary>

Sqlite = 2,

/// <summary>
/// 
/// </summary>

MySql=3,

/// <summary>
/// 
/// </summary>

PostgreSql=4,
}


    /// <summary>
	/// 数据库
	/// </summary>
    public class Databases :Way.Lib.DataModel
    {

        /// <summary>
	    /// 
	    /// </summary>
        public  Databases()
        {
        }


        System.Nullable<Int32> _id;
        /// <summary>
        /// id
        /// </summary>
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
                    var original = this._id;
                    this._id = value;
                    this.OnPropertyChanged("id",original,value);

                }
            }
        }

        System.Nullable<Int32> _ProjectID;
        /// <summary>
        /// 项目ID
        /// </summary>
        public virtual System.Nullable<Int32> ProjectID
        {
            get
            {
                return this._ProjectID;
            }
            set
            {
                if ((this._ProjectID != value))
                {
                    var original = this._ProjectID;
                    this._ProjectID = value;
                    this.OnPropertyChanged("ProjectID",original,value);

                }
            }
        }

        String _Name;
        /// <summary>
        /// Name
        /// </summary>
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
                    var original = this._Name;
                    this._Name = value;
                    this.OnPropertyChanged("Name",original,value);

                }
            }
        }

        System.Nullable<Databases_dbTypeEnum> _dbType=(System.Nullable<Databases_dbTypeEnum>)(1);
        /// <summary>
        /// 数据库类型
        /// </summary>
        public virtual System.Nullable<Databases_dbTypeEnum> dbType
        {
            get
            {
                return this._dbType;
            }
            set
            {
                if ((this._dbType != value))
                {
                    var original = this._dbType;
                    this._dbType = value;
                    this.OnPropertyChanged("dbType",original,value);

                }
            }
        }

        String _conStr;
        /// <summary>
        /// 连接字符串
        /// </summary>
        public virtual String conStr
        {
            get
            {
                return this._conStr;
            }
            set
            {
                if ((this._conStr != value))
                {
                    var original = this._conStr;
                    this._conStr = value;
                    this.OnPropertyChanged("conStr",original,value);

                }
            }
        }

        String _dllPath;
        /// <summary>
        /// dll生成文件夹
        /// </summary>
        public virtual String dllPath
        {
            get
            {
                return this._dllPath;
            }
            set
            {
                if ((this._dllPath != value))
                {
                    var original = this._dllPath;
                    this._dllPath = value;
                    this.OnPropertyChanged("dllPath",original,value);

                }
            }
        }

        System.Nullable<Int32> _iLock=0;
        /// <summary>
        /// iLock
        /// </summary>
        public virtual System.Nullable<Int32> iLock
        {
            get
            {
                return this._iLock;
            }
            set
            {
                if ((this._iLock != value))
                {
                    var original = this._iLock;
                    this._iLock = value;
                    this.OnPropertyChanged("iLock",original,value);

                }
            }
        }

        String _NameSpace;
        /// <summary>
        /// NameSpace
        /// </summary>
        public virtual String NameSpace
        {
            get
            {
                return this._NameSpace;
            }
            set
            {
                if ((this._NameSpace != value))
                {
                    var original = this._NameSpace;
                    this._NameSpace = value;
                    this.OnPropertyChanged("NameSpace",original,value);

                }
            }
        }

        String _Guid;
        /// <summary>
        /// 唯一标示ID
        /// </summary>
        public virtual String Guid
        {
            get
            {
                return this._Guid;
            }
            set
            {
                if ((this._Guid != value))
                {
                    var original = this._Guid;
                    this._Guid = value;
                    this.OnPropertyChanged("Guid",original,value);

                }
            }
        }
}}
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
    public class User :Way.Lib.DataModel
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
                    var original = this._id;
                    this._id = value;
                    this.OnPropertyChanged("id",original,value);

                }
            }
        }

        System.Nullable<User_RoleEnum> _Role;
        /// <summary>
        /// 角色
        /// </summary>
        public virtual System.Nullable<User_RoleEnum> Role
        {
            get
            {
                return this._Role;
            }
            set
            {
                if ((this._Role != value))
                {
                    var original = this._Role;
                    this._Role = value;
                    this.OnPropertyChanged("Role",original,value);

                }
            }
        }

        String _Name;
        /// <summary>
        /// Name
        /// </summary>
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
                    var original = this._Name;
                    this._Name = value;
                    this.OnPropertyChanged("Name",original,value);

                }
            }
        }

        String _Password;
        /// <summary>
        /// Password
        /// </summary>
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
                    var original = this._Password;
                    this._Password = value;
                    this.OnPropertyChanged("Password",original,value);

                }
            }
        }
}}
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
    public class DBPower :Way.Lib.DataModel
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
                    var original = this._id;
                    this._id = value;
                    this.OnPropertyChanged("id",original,value);

                }
            }
        }

        System.Nullable<Int32> _UserID;
        /// <summary>
        /// 用户
        /// </summary>
        public virtual System.Nullable<Int32> UserID
        {
            get
            {
                return this._UserID;
            }
            set
            {
                if ((this._UserID != value))
                {
                    var original = this._UserID;
                    this._UserID = value;
                    this.OnPropertyChanged("UserID",original,value);

                }
            }
        }

        System.Nullable<DBPower_PowerEnum> _Power;
        /// <summary>
        /// 权限
        /// </summary>
        public virtual System.Nullable<DBPower_PowerEnum> Power
        {
            get
            {
                return this._Power;
            }
            set
            {
                if ((this._Power != value))
                {
                    var original = this._Power;
                    this._Power = value;
                    this.OnPropertyChanged("Power",original,value);

                }
            }
        }

        System.Nullable<Int32> _DatabaseID;
        /// <summary>
        /// 数据库ID
        /// </summary>
        public virtual System.Nullable<Int32> DatabaseID
        {
            get
            {
                return this._DatabaseID;
            }
            set
            {
                if ((this._DatabaseID != value))
                {
                    var original = this._DatabaseID;
                    this._DatabaseID = value;
                    this.OnPropertyChanged("DatabaseID",original,value);

                }
            }
        }
}}
namespace EJ{

/// <summary>
/// 
/// </summary>
public enum Bug_StatusEnum:int
{
    

/// <summary>
/// 
/// </summary>
提交给开发人员 = 0,

/// <summary>
/// 
/// </summary>

反馈给提交者 = 1,

/// <summary>
/// 
/// </summary>

处理完毕 = 2,
}


    /// <summary>
	/// 错误报告
	/// </summary>
    public class Bug :Way.Lib.DataModel
    {

        /// <summary>
	    /// 
	    /// </summary>
        public  Bug()
        {
        }


        System.Nullable<Int32> _id;
        /// <summary>
        /// id
        /// </summary>
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
                    var original = this._id;
                    this._id = value;
                    this.OnPropertyChanged("id",original,value);

                }
            }
        }

        String _Title;
        /// <summary>
        /// 标题
        /// </summary>
        public virtual String Title
        {
            get
            {
                return this._Title;
            }
            set
            {
                if ((this._Title != value))
                {
                    var original = this._Title;
                    this._Title = value;
                    this.OnPropertyChanged("Title",original,value);

                }
            }
        }

        System.Nullable<Int32> _SubmitUserID;
        /// <summary>
        /// 提交者ID
        /// </summary>
        public virtual System.Nullable<Int32> SubmitUserID
        {
            get
            {
                return this._SubmitUserID;
            }
            set
            {
                if ((this._SubmitUserID != value))
                {
                    var original = this._SubmitUserID;
                    this._SubmitUserID = value;
                    this.OnPropertyChanged("SubmitUserID",original,value);

                }
            }
        }

        System.Nullable<DateTime> _SubmitTime;
        /// <summary>
        /// 提交时间
        /// </summary>
        public virtual System.Nullable<DateTime> SubmitTime
        {
            get
            {
                return this._SubmitTime;
            }
            set
            {
                if ((this._SubmitTime != value))
                {
                    var original = this._SubmitTime;
                    this._SubmitTime = value;
                    this.OnPropertyChanged("SubmitTime",original,value);

                }
            }
        }

        System.Nullable<Int32> _HandlerID;
        /// <summary>
        /// 处理者ID
        /// </summary>
        public virtual System.Nullable<Int32> HandlerID
        {
            get
            {
                return this._HandlerID;
            }
            set
            {
                if ((this._HandlerID != value))
                {
                    var original = this._HandlerID;
                    this._HandlerID = value;
                    this.OnPropertyChanged("HandlerID",original,value);

                }
            }
        }

        System.Nullable<DateTime> _LastDate;
        /// <summary>
        /// 最后反馈时间
        /// </summary>
        public virtual System.Nullable<DateTime> LastDate
        {
            get
            {
                return this._LastDate;
            }
            set
            {
                if ((this._LastDate != value))
                {
                    var original = this._LastDate;
                    this._LastDate = value;
                    this.OnPropertyChanged("LastDate",original,value);

                }
            }
        }

        System.Nullable<DateTime> _FinishTime;
        /// <summary>
        /// 处理完毕时间
        /// </summary>
        public virtual System.Nullable<DateTime> FinishTime
        {
            get
            {
                return this._FinishTime;
            }
            set
            {
                if ((this._FinishTime != value))
                {
                    var original = this._FinishTime;
                    this._FinishTime = value;
                    this.OnPropertyChanged("FinishTime",original,value);

                }
            }
        }

        System.Nullable<Bug_StatusEnum> _Status;
        /// <summary>
        /// 当前状态
        /// </summary>
        public virtual System.Nullable<Bug_StatusEnum> Status
        {
            get
            {
                return this._Status;
            }
            set
            {
                if ((this._Status != value))
                {
                    var original = this._Status;
                    this._Status = value;
                    this.OnPropertyChanged("Status",original,value);

                }
            }
        }
}}
namespace EJ{


    /// <summary>
	/// 数据表
	/// </summary>
    public class DBTable :Way.Lib.DataModel
    {

        /// <summary>
	    /// 
	    /// </summary>
        public  DBTable()
        {
        }


        System.Nullable<Int32> _id;
        /// <summary>
        /// id
        /// </summary>
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
                    var original = this._id;
                    this._id = value;
                    this.OnPropertyChanged("id",original,value);

                }
            }
        }

        String _caption;
        /// <summary>
        /// caption
        /// </summary>
        public virtual String caption
        {
            get
            {
                return this._caption;
            }
            set
            {
                if ((this._caption != value))
                {
                    var original = this._caption;
                    this._caption = value;
                    this.OnPropertyChanged("caption",original,value);

                }
            }
        }

        String _Name;
        /// <summary>
        /// Name
        /// </summary>
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
                    var original = this._Name;
                    this._Name = value;
                    this.OnPropertyChanged("Name",original,value);

                }
            }
        }

        System.Nullable<Int32> _DatabaseID;
        /// <summary>
        /// DatabaseID
        /// </summary>
        public virtual System.Nullable<Int32> DatabaseID
        {
            get
            {
                return this._DatabaseID;
            }
            set
            {
                if ((this._DatabaseID != value))
                {
                    var original = this._DatabaseID;
                    this._DatabaseID = value;
                    this.OnPropertyChanged("DatabaseID",original,value);

                }
            }
        }

        System.Nullable<Int32> _iLock=0;
        /// <summary>
        /// iLock
        /// </summary>
        public virtual System.Nullable<Int32> iLock
        {
            get
            {
                return this._iLock;
            }
            set
            {
                if ((this._iLock != value))
                {
                    var original = this._iLock;
                    this._iLock = value;
                    this.OnPropertyChanged("iLock",original,value);

                }
            }
        }
}}
namespace EJ{


    /// <summary>
	/// 字段
	/// </summary>
    public class DBColumn :Way.Lib.DataModel
    {

        /// <summary>
	    /// 
	    /// </summary>
        public  DBColumn()
        {
        }


        System.Nullable<Int32> _id;
        /// <summary>
        /// id
        /// </summary>
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
                    var original = this._id;
                    this._id = value;
                    this.OnPropertyChanged("id",original,value);

                }
            }
        }

        String _caption;
        /// <summary>
        /// caption
        /// </summary>
        public virtual String caption
        {
            get
            {
                return this._caption;
            }
            set
            {
                if ((this._caption != value))
                {
                    var original = this._caption;
                    this._caption = value;
                    this.OnPropertyChanged("caption",original,value);

                }
            }
        }

        String _Name;
        /// <summary>
        /// Name
        /// </summary>
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
                    var original = this._Name;
                    this._Name = value;
                    this.OnPropertyChanged("Name",original,value);

                }
            }
        }

        System.Nullable<Boolean> _IsAutoIncrement=false;
        /// <summary>
        /// 自增长
        /// </summary>
        public virtual System.Nullable<Boolean> IsAutoIncrement
        {
            get
            {
                return this._IsAutoIncrement;
            }
            set
            {
                if ((this._IsAutoIncrement != value))
                {
                    var original = this._IsAutoIncrement;
                    this._IsAutoIncrement = value;
                    this.OnPropertyChanged("IsAutoIncrement",original,value);

                }
            }
        }

        System.Nullable<Boolean> _CanNull=true;
        /// <summary>
        /// 可以为空
        /// </summary>
        public virtual System.Nullable<Boolean> CanNull
        {
            get
            {
                return this._CanNull;
            }
            set
            {
                if ((this._CanNull != value))
                {
                    var original = this._CanNull;
                    this._CanNull = value;
                    this.OnPropertyChanged("CanNull",original,value);

                }
            }
        }

        String _dbType;
        /// <summary>
        /// 数据库字段类型
        /// </summary>
        public virtual String dbType
        {
            get
            {
                return this._dbType;
            }
            set
            {
                if ((this._dbType != value))
                {
                    var original = this._dbType;
                    this._dbType = value;
                    this.OnPropertyChanged("dbType",original,value);

                }
            }
        }

        String _Type;
        /// <summary>
        /// c#类型
        /// </summary>
        public virtual String Type
        {
            get
            {
                return this._Type;
            }
            set
            {
                if ((this._Type != value))
                {
                    var original = this._Type;
                    this._Type = value;
                    this.OnPropertyChanged("Type",original,value);

                }
            }
        }

        String _EnumDefine;
        /// <summary>
        /// Enum定义
        /// </summary>
        public virtual String EnumDefine
        {
            get
            {
                return this._EnumDefine;
            }
            set
            {
                if ((this._EnumDefine != value))
                {
                    var original = this._EnumDefine;
                    this._EnumDefine = value;
                    this.OnPropertyChanged("EnumDefine",original,value);

                }
            }
        }

        String _length;
        /// <summary>
        /// 长度
        /// </summary>
        public virtual String length
        {
            get
            {
                return this._length;
            }
            set
            {
                if ((this._length != value))
                {
                    var original = this._length;
                    this._length = value;
                    this.OnPropertyChanged("length",original,value);

                }
            }
        }

        String _defaultValue;
        /// <summary>
        /// 默认值
        /// </summary>
        public virtual String defaultValue
        {
            get
            {
                return this._defaultValue;
            }
            set
            {
                if ((this._defaultValue != value))
                {
                    var original = this._defaultValue;
                    this._defaultValue = value;
                    this.OnPropertyChanged("defaultValue",original,value);

                }
            }
        }

        System.Nullable<Int32> _TableID;
        /// <summary>
        /// TableID
        /// </summary>
        public virtual System.Nullable<Int32> TableID
        {
            get
            {
                return this._TableID;
            }
            set
            {
                if ((this._TableID != value))
                {
                    var original = this._TableID;
                    this._TableID = value;
                    this.OnPropertyChanged("TableID",original,value);

                }
            }
        }

        System.Nullable<Boolean> _IsPKID=false;
        /// <summary>
        /// 是否是主键
        /// </summary>
        public virtual System.Nullable<Boolean> IsPKID
        {
            get
            {
                return this._IsPKID;
            }
            set
            {
                if ((this._IsPKID != value))
                {
                    var original = this._IsPKID;
                    this._IsPKID = value;
                    this.OnPropertyChanged("IsPKID",original,value);

                }
            }
        }

        System.Nullable<Int32> _orderid=0;
        /// <summary>
        /// orderid
        /// </summary>
        public virtual System.Nullable<Int32> orderid
        {
            get
            {
                return this._orderid;
            }
            set
            {
                if ((this._orderid != value))
                {
                    var original = this._orderid;
                    this._orderid = value;
                    this.OnPropertyChanged("orderid",original,value);

                }
            }
        }
}}
namespace EJ{


    /// <summary>
	/// 数据表权限
	/// </summary>
    public class TablePower :Way.Lib.DataModel
    {

        /// <summary>
	    /// 
	    /// </summary>
        public  TablePower()
        {
        }


        System.Nullable<Int32> _id;
        /// <summary>
        /// id
        /// </summary>
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
                    var original = this._id;
                    this._id = value;
                    this.OnPropertyChanged("id",original,value);

                }
            }
        }

        System.Nullable<Int32> _UserID;
        /// <summary>
        /// UserID
        /// </summary>
        public virtual System.Nullable<Int32> UserID
        {
            get
            {
                return this._UserID;
            }
            set
            {
                if ((this._UserID != value))
                {
                    var original = this._UserID;
                    this._UserID = value;
                    this.OnPropertyChanged("UserID",original,value);

                }
            }
        }

        System.Nullable<Int32> _TableID;
        /// <summary>
        /// TableID
        /// </summary>
        public virtual System.Nullable<Int32> TableID
        {
            get
            {
                return this._TableID;
            }
            set
            {
                if ((this._TableID != value))
                {
                    var original = this._TableID;
                    this._TableID = value;
                    this.OnPropertyChanged("TableID",original,value);

                }
            }
        }
}}
namespace EJ{


    /// <summary>
	/// 项目权限
	/// </summary>
    public class ProjectPower :Way.Lib.DataModel
    {

        /// <summary>
	    /// 
	    /// </summary>
        public  ProjectPower()
        {
        }


        System.Nullable<Int32> _id;
        /// <summary>
        /// id
        /// </summary>
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
                    var original = this._id;
                    this._id = value;
                    this.OnPropertyChanged("id",original,value);

                }
            }
        }

        System.Nullable<Int32> _ProjectID;
        /// <summary>
        /// ProjectID
        /// </summary>
        public virtual System.Nullable<Int32> ProjectID
        {
            get
            {
                return this._ProjectID;
            }
            set
            {
                if ((this._ProjectID != value))
                {
                    var original = this._ProjectID;
                    this._ProjectID = value;
                    this.OnPropertyChanged("ProjectID",original,value);

                }
            }
        }

        System.Nullable<Int32> _UserID;
        /// <summary>
        /// UserID
        /// </summary>
        public virtual System.Nullable<Int32> UserID
        {
            get
            {
                return this._UserID;
            }
            set
            {
                if ((this._UserID != value))
                {
                    var original = this._UserID;
                    this._UserID = value;
                    this.OnPropertyChanged("UserID",original,value);

                }
            }
        }
}}
namespace EJ{


    /// <summary>
	/// 数据模块
	/// </summary>
    public class DBModule :Way.Lib.DataModel
    {

        /// <summary>
	    /// 
	    /// </summary>
        public  DBModule()
        {
        }


        System.Nullable<Int32> _id;
        /// <summary>
        /// id
        /// </summary>
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
                    var original = this._id;
                    this._id = value;
                    this.OnPropertyChanged("id",original,value);

                }
            }
        }

        String _Name;
        /// <summary>
        /// Name
        /// </summary>
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
                    var original = this._Name;
                    this._Name = value;
                    this.OnPropertyChanged("Name",original,value);

                }
            }
        }

        System.Nullable<Int32> _DatabaseID;
        /// <summary>
        /// DatabaseID
        /// </summary>
        public virtual System.Nullable<Int32> DatabaseID
        {
            get
            {
                return this._DatabaseID;
            }
            set
            {
                if ((this._DatabaseID != value))
                {
                    var original = this._DatabaseID;
                    this._DatabaseID = value;
                    this.OnPropertyChanged("DatabaseID",original,value);

                }
            }
        }

        System.Nullable<Boolean> _IsFolder=false;
        /// <summary>
        /// IsFolder
        /// </summary>
        public virtual System.Nullable<Boolean> IsFolder
        {
            get
            {
                return this._IsFolder;
            }
            set
            {
                if ((this._IsFolder != value))
                {
                    var original = this._IsFolder;
                    this._IsFolder = value;
                    this.OnPropertyChanged("IsFolder",original,value);

                }
            }
        }

        System.Nullable<Int32> _parentID;
        /// <summary>
        /// parentID
        /// </summary>
        public virtual System.Nullable<Int32> parentID
        {
            get
            {
                return this._parentID;
            }
            set
            {
                if ((this._parentID != value))
                {
                    var original = this._parentID;
                    this._parentID = value;
                    this.OnPropertyChanged("parentID",original,value);

                }
            }
        }
}}
namespace EJ{


    /// <summary>
	/// 级联删除
	/// </summary>
    public class DBDeleteConfig :Way.Lib.DataModel
    {

        /// <summary>
	    /// 
	    /// </summary>
        public  DBDeleteConfig()
        {
        }


        System.Nullable<Int32> _id;
        /// <summary>
        /// id
        /// </summary>
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
                    var original = this._id;
                    this._id = value;
                    this.OnPropertyChanged("id",original,value);

                }
            }
        }

        System.Nullable<Int32> _TableID;
        /// <summary>
        /// TableID
        /// </summary>
        public virtual System.Nullable<Int32> TableID
        {
            get
            {
                return this._TableID;
            }
            set
            {
                if ((this._TableID != value))
                {
                    var original = this._TableID;
                    this._TableID = value;
                    this.OnPropertyChanged("TableID",original,value);

                }
            }
        }

        System.Nullable<Int32> _RelaTableID;
        /// <summary>
        /// 关联表ID
        /// </summary>
        public virtual System.Nullable<Int32> RelaTableID
        {
            get
            {
                return this._RelaTableID;
            }
            set
            {
                if ((this._RelaTableID != value))
                {
                    var original = this._RelaTableID;
                    this._RelaTableID = value;
                    this.OnPropertyChanged("RelaTableID",original,value);

                }
            }
        }

        String _RelaTable_Desc;
        /// <summary>
        /// RelaTable_Desc
        /// </summary>
        public virtual String RelaTable_Desc
        {
            get
            {
                return this._RelaTable_Desc;
            }
            set
            {
                if ((this._RelaTable_Desc != value))
                {
                    var original = this._RelaTable_Desc;
                    this._RelaTable_Desc = value;
                    this.OnPropertyChanged("RelaTable_Desc",original,value);

                }
            }
        }

        System.Nullable<Int32> _RelaColumID;
        /// <summary>
        /// 关联字段的ID
        /// </summary>
        public virtual System.Nullable<Int32> RelaColumID
        {
            get
            {
                return this._RelaColumID;
            }
            set
            {
                if ((this._RelaColumID != value))
                {
                    var original = this._RelaColumID;
                    this._RelaColumID = value;
                    this.OnPropertyChanged("RelaColumID",original,value);

                }
            }
        }

        String _RelaColumn_Desc;
        /// <summary>
        /// RelaColumn_Desc
        /// </summary>
        public virtual String RelaColumn_Desc
        {
            get
            {
                return this._RelaColumn_Desc;
            }
            set
            {
                if ((this._RelaColumn_Desc != value))
                {
                    var original = this._RelaColumn_Desc;
                    this._RelaColumn_Desc = value;
                    this.OnPropertyChanged("RelaColumn_Desc",original,value);

                }
            }
        }
}}
namespace EJ{


    /// <summary>
	/// TableInModule
	/// </summary>
    public class TableInModule :Way.Lib.DataModel
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
                    var original = this._id;
                    this._id = value;
                    this.OnPropertyChanged("id",original,value);

                }
            }
        }

        System.Nullable<Int32> _TableID;
        /// <summary>
        /// TableID
        /// </summary>
        public virtual System.Nullable<Int32> TableID
        {
            get
            {
                return this._TableID;
            }
            set
            {
                if ((this._TableID != value))
                {
                    var original = this._TableID;
                    this._TableID = value;
                    this.OnPropertyChanged("TableID",original,value);

                }
            }
        }

        System.Nullable<Int32> _ModuleID;
        /// <summary>
        /// ModuleID
        /// </summary>
        public virtual System.Nullable<Int32> ModuleID
        {
            get
            {
                return this._ModuleID;
            }
            set
            {
                if ((this._ModuleID != value))
                {
                    var original = this._ModuleID;
                    this._ModuleID = value;
                    this.OnPropertyChanged("ModuleID",original,value);

                }
            }
        }

        System.Nullable<Int32> _x;
        /// <summary>
        /// x
        /// </summary>
        public virtual System.Nullable<Int32> x
        {
            get
            {
                return this._x;
            }
            set
            {
                if ((this._x != value))
                {
                    var original = this._x;
                    this._x = value;
                    this.OnPropertyChanged("x",original,value);

                }
            }
        }

        System.Nullable<Int32> _y;
        /// <summary>
        /// y
        /// </summary>
        public virtual System.Nullable<Int32> y
        {
            get
            {
                return this._y;
            }
            set
            {
                if ((this._y != value))
                {
                    var original = this._y;
                    this._y = value;
                    this.OnPropertyChanged("y",original,value);

                }
            }
        }

        String _flag;
        /// <summary>
        /// 临时变量
        /// </summary>
        public virtual String flag
        {
            get
            {
                return this._flag;
            }
            set
            {
                if ((this._flag != value))
                {
                    var original = this._flag;
                    this._flag = value;
                    this.OnPropertyChanged("flag",original,value);

                }
            }
        }

        String _flag2;
        /// <summary>
        /// flag2
        /// </summary>
        public virtual String flag2
        {
            get
            {
                return this._flag2;
            }
            set
            {
                if ((this._flag2 != value))
                {
                    var original = this._flag2;
                    this._flag2 = value;
                    this.OnPropertyChanged("flag2",original,value);

                }
            }
        }
}}
namespace EJ{


    /// <summary>
	/// 唯一值索引
	/// </summary>
    public class IDXIndex :Way.Lib.DataModel
    {

        /// <summary>
	    /// 
	    /// </summary>
        public  IDXIndex()
        {
        }


        System.Nullable<Int32> _id;
        /// <summary>
        /// id
        /// </summary>
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
                    var original = this._id;
                    this._id = value;
                    this.OnPropertyChanged("id",original,value);

                }
            }
        }

        System.Nullable<Int32> _TableID;
        /// <summary>
        /// TableID
        /// </summary>
        public virtual System.Nullable<Int32> TableID
        {
            get
            {
                return this._TableID;
            }
            set
            {
                if ((this._TableID != value))
                {
                    var original = this._TableID;
                    this._TableID = value;
                    this.OnPropertyChanged("TableID",original,value);

                }
            }
        }

        String _Keys;
        /// <summary>
        /// Keys
        /// </summary>
        public virtual String Keys
        {
            get
            {
                return this._Keys;
            }
            set
            {
                if ((this._Keys != value))
                {
                    var original = this._Keys;
                    this._Keys = value;
                    this.OnPropertyChanged("Keys",original,value);

                }
            }
        }

        System.Nullable<Boolean> _IsUnique=true;
        /// <summary>
        /// 是否唯一索引
        /// </summary>
        public virtual System.Nullable<Boolean> IsUnique
        {
            get
            {
                return this._IsUnique;
            }
            set
            {
                if ((this._IsUnique != value))
                {
                    var original = this._IsUnique;
                    this._IsUnique = value;
                    this.OnPropertyChanged("IsUnique",original,value);

                }
            }
        }

        System.Nullable<Boolean> _IsClustered=false;
        /// <summary>
        /// 是否聚焦
        /// </summary>
        public virtual System.Nullable<Boolean> IsClustered
        {
            get
            {
                return this._IsClustered;
            }
            set
            {
                if ((this._IsClustered != value))
                {
                    var original = this._IsClustered;
                    this._IsClustered = value;
                    this.OnPropertyChanged("IsClustered",original,value);

                }
            }
        }
}}
namespace EJ{


    /// <summary>
	/// Bug处理历史记录
	/// </summary>
    public class BugHandleHistory :Way.Lib.DataModel
    {

        /// <summary>
	    /// 
	    /// </summary>
        public  BugHandleHistory()
        {
        }


        System.Nullable<Int32> _id;
        /// <summary>
        /// 
        /// </summary>
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
                    var original = this._id;
                    this._id = value;
                    this.OnPropertyChanged("id",original,value);

                }
            }
        }

        System.Nullable<Int32> _BugID;
        /// <summary>
        /// 
        /// </summary>
        public virtual System.Nullable<Int32> BugID
        {
            get
            {
                return this._BugID;
            }
            set
            {
                if ((this._BugID != value))
                {
                    var original = this._BugID;
                    this._BugID = value;
                    this.OnPropertyChanged("BugID",original,value);

                }
            }
        }

        System.Nullable<Int32> _UserID;
        /// <summary>
        /// 发标者ID
        /// </summary>
        public virtual System.Nullable<Int32> UserID
        {
            get
            {
                return this._UserID;
            }
            set
            {
                if ((this._UserID != value))
                {
                    var original = this._UserID;
                    this._UserID = value;
                    this.OnPropertyChanged("UserID",original,value);

                }
            }
        }

        Byte[] _content;
        /// <summary>
        /// 内容
        /// </summary>
        public virtual Byte[] content
        {
            get
            {
                return this._content;
            }
            set
            {
                if ((this._content != value))
                {
                    var original = this._content;
                    this._content = value;
                    this.OnPropertyChanged("content",original,value);

                }
            }
        }

        System.Nullable<DateTime> _SendTime;
        /// <summary>
        /// 发表时间
        /// </summary>
        public virtual System.Nullable<DateTime> SendTime
        {
            get
            {
                return this._SendTime;
            }
            set
            {
                if ((this._SendTime != value))
                {
                    var original = this._SendTime;
                    this._SendTime = value;
                    this.OnPropertyChanged("SendTime",original,value);

                }
            }
        }
}}
namespace EJ{


    /// <summary>
	/// Bug附带截图
	/// </summary>
    public class BugImages :Way.Lib.DataModel
    {

        /// <summary>
	    /// 
	    /// </summary>
        public  BugImages()
        {
        }


        System.Nullable<Int32> _id;
        /// <summary>
        /// 
        /// </summary>
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
                    var original = this._id;
                    this._id = value;
                    this.OnPropertyChanged("id",original,value);

                }
            }
        }

        System.Nullable<Int32> _BugID;
        /// <summary>
        /// 
        /// </summary>
        public virtual System.Nullable<Int32> BugID
        {
            get
            {
                return this._BugID;
            }
            set
            {
                if ((this._BugID != value))
                {
                    var original = this._BugID;
                    this._BugID = value;
                    this.OnPropertyChanged("BugID",original,value);

                }
            }
        }

        Byte[] _content;
        /// <summary>
        /// 
        /// </summary>
        public virtual Byte[] content
        {
            get
            {
                return this._content;
            }
            set
            {
                if ((this._content != value))
                {
                    var original = this._content;
                    this._content = value;
                    this.OnPropertyChanged("content",original,value);

                }
            }
        }

        System.Nullable<Int32> _orderID;
        /// <summary>
        /// 排序
        /// </summary>
        public virtual System.Nullable<Int32> orderID
        {
            get
            {
                return this._orderID;
            }
            set
            {
                if ((this._orderID != value))
                {
                    var original = this._orderID;
                    this._orderID = value;
                    this.OnPropertyChanged("orderID",original,value);

                }
            }
        }
}}
namespace EJ{


    /// <summary>
	/// 引入的dll
	/// </summary>
    public class DLLImport :Way.Lib.DataModel
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
                    var original = this._id;
                    this._id = value;
                    this.OnPropertyChanged("id",original,value);

                }
            }
        }

        String _path;
        /// <summary>
        /// dll文件路径
        /// </summary>
        public virtual String path
        {
            get
            {
                return this._path;
            }
            set
            {
                if ((this._path != value))
                {
                    var original = this._path;
                    this._path = value;
                    this.OnPropertyChanged("path",original,value);

                }
            }
        }

        System.Nullable<Int32> _ProjectID;
        /// <summary>
        /// 
        /// </summary>
        public virtual System.Nullable<Int32> ProjectID
        {
            get
            {
                return this._ProjectID;
            }
            set
            {
                if ((this._ProjectID != value))
                {
                    var original = this._ProjectID;
                    this._ProjectID = value;
                    this.OnPropertyChanged("ProjectID",original,value);

                }
            }
        }
}}
namespace EJ{


    /// <summary>
	/// 接口设计的目录结构
	/// </summary>
    public class InterfaceModule :Way.Lib.DataModel
    {

        /// <summary>
	    /// 
	    /// </summary>
        public  InterfaceModule()
        {
        }


        System.Nullable<Int32> _id;
        /// <summary>
        /// 
        /// </summary>
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
                    var original = this._id;
                    this._id = value;
                    this.OnPropertyChanged("id",original,value);

                }
            }
        }

        System.Nullable<Int32> _ProjectID;
        /// <summary>
        /// 
        /// </summary>
        public virtual System.Nullable<Int32> ProjectID
        {
            get
            {
                return this._ProjectID;
            }
            set
            {
                if ((this._ProjectID != value))
                {
                    var original = this._ProjectID;
                    this._ProjectID = value;
                    this.OnPropertyChanged("ProjectID",original,value);

                }
            }
        }

        String _Name;
        /// <summary>
        /// 
        /// </summary>
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
                    var original = this._Name;
                    this._Name = value;
                    this.OnPropertyChanged("Name",original,value);

                }
            }
        }

        System.Nullable<Int32> _ParentID=0;
        /// <summary>
        /// 
        /// </summary>
        public virtual System.Nullable<Int32> ParentID
        {
            get
            {
                return this._ParentID;
            }
            set
            {
                if ((this._ParentID != value))
                {
                    var original = this._ParentID;
                    this._ParentID = value;
                    this.OnPropertyChanged("ParentID",original,value);

                }
            }
        }

        System.Nullable<Boolean> _IsFolder=false;
        /// <summary>
        /// 
        /// </summary>
        public virtual System.Nullable<Boolean> IsFolder
        {
            get
            {
                return this._IsFolder;
            }
            set
            {
                if ((this._IsFolder != value))
                {
                    var original = this._IsFolder;
                    this._IsFolder = value;
                    this.OnPropertyChanged("IsFolder",original,value);

                }
            }
        }

        System.Nullable<Int32> _LockUserId;
        /// <summary>
        /// 已经被某人锁定
        /// </summary>
        public virtual System.Nullable<Int32> LockUserId
        {
            get
            {
                return this._LockUserId;
            }
            set
            {
                if ((this._LockUserId != value))
                {
                    var original = this._LockUserId;
                    this._LockUserId = value;
                    this.OnPropertyChanged("LockUserId",original,value);

                }
            }
        }
}}
namespace EJ{


    /// <summary>
	/// 
	/// </summary>
    public class InterfaceInModule :Way.Lib.DataModel
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
                    var original = this._id;
                    this._id = value;
                    this.OnPropertyChanged("id",original,value);

                }
            }
        }

        System.Nullable<Int32> _ModuleID;
        /// <summary>
        /// 
        /// </summary>
        public virtual System.Nullable<Int32> ModuleID
        {
            get
            {
                return this._ModuleID;
            }
            set
            {
                if ((this._ModuleID != value))
                {
                    var original = this._ModuleID;
                    this._ModuleID = value;
                    this.OnPropertyChanged("ModuleID",original,value);

                }
            }
        }

        System.Nullable<Int32> _x;
        /// <summary>
        /// 
        /// </summary>
        public virtual System.Nullable<Int32> x
        {
            get
            {
                return this._x;
            }
            set
            {
                if ((this._x != value))
                {
                    var original = this._x;
                    this._x = value;
                    this.OnPropertyChanged("x",original,value);

                }
            }
        }

        System.Nullable<Int32> _y;
        /// <summary>
        /// 
        /// </summary>
        public virtual System.Nullable<Int32> y
        {
            get
            {
                return this._y;
            }
            set
            {
                if ((this._y != value))
                {
                    var original = this._y;
                    this._y = value;
                    this.OnPropertyChanged("y",original,value);

                }
            }
        }

        String _Type;
        /// <summary>
        /// 
        /// </summary>
        public virtual String Type
        {
            get
            {
                return this._Type;
            }
            set
            {
                if ((this._Type != value))
                {
                    var original = this._Type;
                    this._Type = value;
                    this.OnPropertyChanged("Type",original,value);

                }
            }
        }

        String _JsonData;
        /// <summary>
        /// 
        /// </summary>
        public virtual String JsonData
        {
            get
            {
                return this._JsonData;
            }
            set
            {
                if ((this._JsonData != value))
                {
                    var original = this._JsonData;
                    this._JsonData = value;
                    this.OnPropertyChanged("JsonData",original,value);

                }
            }
        }

        System.Nullable<Int32> _width;
        /// <summary>
        /// 
        /// </summary>
        public virtual System.Nullable<Int32> width
        {
            get
            {
                return this._width;
            }
            set
            {
                if ((this._width != value))
                {
                    var original = this._width;
                    this._width = value;
                    this.OnPropertyChanged("width",original,value);

                }
            }
        }

        System.Nullable<Int32> _height;
        /// <summary>
        /// 
        /// </summary>
        public virtual System.Nullable<Int32> height
        {
            get
            {
                return this._height;
            }
            set
            {
                if ((this._height != value))
                {
                    var original = this._height;
                    this._height = value;
                    this.OnPropertyChanged("height",original,value);

                }
            }
        }
}}
namespace EJ{


    /// <summary>
	/// InterfaceModule权限设定表
	/// </summary>
    public class InterfaceModulePower :Way.Lib.DataModel
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
                    var original = this._id;
                    this._id = value;
                    this.OnPropertyChanged("id",original,value);

                }
            }
        }

        System.Nullable<Int32> _UserID;
        /// <summary>
        /// 
        /// </summary>
        public virtual System.Nullable<Int32> UserID
        {
            get
            {
                return this._UserID;
            }
            set
            {
                if ((this._UserID != value))
                {
                    var original = this._UserID;
                    this._UserID = value;
                    this.OnPropertyChanged("UserID",original,value);

                }
            }
        }

        System.Nullable<Int32> _ModuleID;
        /// <summary>
        /// 
        /// </summary>
        public virtual System.Nullable<Int32> ModuleID
        {
            get
            {
                return this._ModuleID;
            }
            set
            {
                if ((this._ModuleID != value))
                {
                    var original = this._ModuleID;
                    this._ModuleID = value;
                    this.OnPropertyChanged("ModuleID",original,value);

                }
            }
        }
}}
namespace EJ{


    /// <summary>
	/// 
	/// </summary>
    public class classproperty :Way.Lib.DataModel
    {

        /// <summary>
	    /// 
	    /// </summary>
        public  classproperty()
        {
        }


        System.Nullable<Int32> _id;
        /// <summary>
        /// 
        /// </summary>
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
                    var original = this._id;
                    this._id = value;
                    this.OnPropertyChanged("id",original,value);

                }
            }
        }

        System.Nullable<Int32> _tableid;
        /// <summary>
        /// 
        /// </summary>
        public virtual System.Nullable<Int32> tableid
        {
            get
            {
                return this._tableid;
            }
            set
            {
                if ((this._tableid != value))
                {
                    var original = this._tableid;
                    this._tableid = value;
                    this.OnPropertyChanged("tableid",original,value);

                }
            }
        }

        String _name;
        /// <summary>
        /// 
        /// </summary>
        public virtual String name
        {
            get
            {
                return this._name;
            }
            set
            {
                if ((this._name != value))
                {
                    var original = this._name;
                    this._name = value;
                    this.OnPropertyChanged("name",original,value);

                }
            }
        }

        System.Nullable<Int32> _foreignkey_tableid;
        /// <summary>
        /// 属性的类型
        /// </summary>
        public virtual System.Nullable<Int32> foreignkey_tableid
        {
            get
            {
                return this._foreignkey_tableid;
            }
            set
            {
                if ((this._foreignkey_tableid != value))
                {
                    var original = this._foreignkey_tableid;
                    this._foreignkey_tableid = value;
                    this.OnPropertyChanged("foreignkey_tableid",original,value);

                }
            }
        }

        System.Nullable<Int32> _foreignkey_columnid;
        /// <summary>
        /// 
        /// </summary>
        public virtual System.Nullable<Int32> foreignkey_columnid
        {
            get
            {
                return this._foreignkey_columnid;
            }
            set
            {
                if ((this._foreignkey_columnid != value))
                {
                    var original = this._foreignkey_columnid;
                    this._foreignkey_columnid = value;
                    this.OnPropertyChanged("foreignkey_columnid",original,value);

                }
            }
        }

        System.Nullable<Boolean> _iscollection=false;
        /// <summary>
        /// 
        /// </summary>
        public virtual System.Nullable<Boolean> iscollection
        {
            get
            {
                return this._iscollection;
            }
            set
            {
                if ((this._iscollection != value))
                {
                    var original = this._iscollection;
                    this._iscollection = value;
                    this.OnPropertyChanged("iscollection",original,value);

                }
            }
        }
}}
