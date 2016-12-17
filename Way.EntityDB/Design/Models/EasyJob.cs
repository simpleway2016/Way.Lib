
using System;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;

namespace EJ{


    /// <summary>
	/// 项目
	/// </summary>
    [Way.EntityDB.Attributes.Table("Project","id")]
    public class Project :Way.EntityDB.DataItem
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
[Way.EntityDB.WayLinqColumnAttribute(Comment="",Caption="id",Storage = "_id",DbType="int" ,IsPrimaryKey=true,IsDbGenerated=true)]
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

String _Name;
/// <summary>
/// Name
	/// </summary>
[Way.EntityDB.WayLinqColumnAttribute(Comment="",Caption="Name",Storage = "_Name",DbType="varchar(50)")]
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
}


    /// <summary>
	/// 数据库
	/// </summary>
    [Way.EntityDB.Attributes.Table("Databases","id")]
    public class Databases :Way.EntityDB.DataItem
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
[Way.EntityDB.WayLinqColumnAttribute(Comment="",Caption="id",Storage = "_id",DbType="int" ,IsPrimaryKey=true,IsDbGenerated=true)]
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

System.Nullable<Int32> _ProjectID;
/// <summary>
/// 项目ID
	/// </summary>
[Way.EntityDB.WayLinqColumnAttribute(Comment="",Caption="项目ID",Storage = "_ProjectID",DbType="int")]
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

String _Name;
/// <summary>
/// Name
	/// </summary>
[Way.EntityDB.WayLinqColumnAttribute(Comment="",Caption="Name",Storage = "_Name",DbType="varchar(50)")]
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

Databases_dbTypeEnum _dbType=(Databases_dbTypeEnum)(1);
/// <summary>
/// 数据库类型
	/// </summary>
[Way.EntityDB.WayLinqColumnAttribute(Comment="",Caption="数据库类型",Storage = "_dbType",DbType="int")]
        public Databases_dbTypeEnum dbType
        {
            get
            {
                return this._dbType;
            }
            set
            {
                if ((this._dbType != value))
                {
                    this.SendPropertyChanging("dbType",this._dbType,value);
                    this._dbType = value;
                    this.SendPropertyChanged("dbType");

                }
            }
        }

String _conStr;
/// <summary>
/// 连接字符串
	/// </summary>
[Way.EntityDB.WayLinqColumnAttribute(Comment="",Caption="连接字符串",Storage = "_conStr",DbType="varchar(200)")]
        public String conStr
        {
            get
            {
                return this._conStr;
            }
            set
            {
                if ((this._conStr != value))
                {
                    this.SendPropertyChanging("conStr",this._conStr,value);
                    this._conStr = value;
                    this.SendPropertyChanged("conStr");

                }
            }
        }

String _dllPath;
/// <summary>
/// dll生成文件夹
	/// </summary>
[Way.EntityDB.WayLinqColumnAttribute(Comment="",Caption="dll生成文件夹",Storage = "_dllPath",DbType="varchar(100)")]
        public String dllPath
        {
            get
            {
                return this._dllPath;
            }
            set
            {
                if ((this._dllPath != value))
                {
                    this.SendPropertyChanging("dllPath",this._dllPath,value);
                    this._dllPath = value;
                    this.SendPropertyChanged("dllPath");

                }
            }
        }

System.Nullable<Int32> _iLock=0;
/// <summary>
/// iLock
	/// </summary>
[Way.EntityDB.WayLinqColumnAttribute(Comment="",Caption="iLock",Storage = "_iLock",DbType="int")]
        public System.Nullable<Int32> iLock
        {
            get
            {
                return this._iLock;
            }
            set
            {
                if ((this._iLock != value))
                {
                    this.SendPropertyChanging("iLock",this._iLock,value);
                    this._iLock = value;
                    this.SendPropertyChanged("iLock");

                }
            }
        }

String _NameSpace;
/// <summary>
/// NameSpace
	/// </summary>
[Way.EntityDB.WayLinqColumnAttribute(Comment="",Caption="NameSpace",Storage = "_NameSpace",DbType="varchar(50)")]
        public String NameSpace
        {
            get
            {
                return this._NameSpace;
            }
            set
            {
                if ((this._NameSpace != value))
                {
                    this.SendPropertyChanging("NameSpace",this._NameSpace,value);
                    this._NameSpace = value;
                    this.SendPropertyChanged("NameSpace");

                }
            }
        }

String _Guid;
/// <summary>
/// 唯一标示ID
	/// </summary>
[Way.EntityDB.WayLinqColumnAttribute(Comment="",Caption="唯一标示ID",Storage = "_Guid",DbType="varchar(50)")]
        public String Guid
        {
            get
            {
                return this._Guid;
            }
            set
            {
                if ((this._Guid != value))
                {
                    this.SendPropertyChanging("Guid",this._Guid,value);
                    this._Guid = value;
                    this.SendPropertyChanged("Guid");

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
    [Way.EntityDB.Attributes.Table("User","id")]
    public class User :Way.EntityDB.DataItem
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
[Way.EntityDB.WayLinqColumnAttribute(Comment="",Caption="id",Storage = "_id",DbType="int" ,IsPrimaryKey=true,IsDbGenerated=true)]
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
[Way.EntityDB.WayLinqColumnAttribute(Comment="",Caption="角色",Storage = "_Role",DbType="int")]
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
[Way.EntityDB.WayLinqColumnAttribute(Comment="",Caption="Name",Storage = "_Name",DbType="varchar(50)")]
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
[Way.EntityDB.WayLinqColumnAttribute(Comment="",Caption="Password",Storage = "_Password",DbType="varchar(50)")]
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
    [Way.EntityDB.Attributes.Table("DBPower","id")]
    public class DBPower :Way.EntityDB.DataItem
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
[Way.EntityDB.WayLinqColumnAttribute(Comment="",Caption="id",Storage = "_id",DbType="int" ,IsPrimaryKey=true,IsDbGenerated=true)]
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
/// 用户
	/// </summary>
[Way.EntityDB.WayLinqColumnAttribute(Comment="",Caption="用户",Storage = "_UserID",DbType="int")]
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

DBPower_PowerEnum _Power=(DBPower_PowerEnum)int.MinValue;
/// <summary>
/// 权限
	/// </summary>
[Way.EntityDB.WayLinqColumnAttribute(Comment="",Caption="权限",Storage = "_Power",DbType="int")]
        public DBPower_PowerEnum Power
        {
            get
            {
                return this._Power;
            }
            set
            {
                if ((this._Power != value))
                {
                    this.SendPropertyChanging("Power",this._Power,value);
                    this._Power = value;
                    this.SendPropertyChanged("Power");

                }
            }
        }

System.Nullable<Int32> _DatabaseID;
/// <summary>
/// 数据库ID
	/// </summary>
[Way.EntityDB.WayLinqColumnAttribute(Comment="",Caption="数据库ID",Storage = "_DatabaseID",DbType="int")]
        public System.Nullable<Int32> DatabaseID
        {
            get
            {
                return this._DatabaseID;
            }
            set
            {
                if ((this._DatabaseID != value))
                {
                    this.SendPropertyChanging("DatabaseID",this._DatabaseID,value);
                    this._DatabaseID = value;
                    this.SendPropertyChanged("DatabaseID");

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
    [Way.EntityDB.Attributes.Table("Bug","id")]
    public class Bug :Way.EntityDB.DataItem
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
[Way.EntityDB.WayLinqColumnAttribute(Comment="",Caption="id",Storage = "_id",DbType="int" ,IsPrimaryKey=true,IsDbGenerated=true)]
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

String _Title;
/// <summary>
/// 标题
	/// </summary>
[Way.EntityDB.WayLinqColumnAttribute(Comment="",Caption="标题",Storage = "_Title",DbType="varchar(50)")]
        public String Title
        {
            get
            {
                return this._Title;
            }
            set
            {
                if ((this._Title != value))
                {
                    this.SendPropertyChanging("Title",this._Title,value);
                    this._Title = value;
                    this.SendPropertyChanged("Title");

                }
            }
        }

System.Nullable<Int32> _SubmitUserID;
/// <summary>
/// 提交者ID
	/// </summary>
[Way.EntityDB.WayLinqColumnAttribute(Comment="",Caption="提交者ID",Storage = "_SubmitUserID",DbType="int")]
        public System.Nullable<Int32> SubmitUserID
        {
            get
            {
                return this._SubmitUserID;
            }
            set
            {
                if ((this._SubmitUserID != value))
                {
                    this.SendPropertyChanging("SubmitUserID",this._SubmitUserID,value);
                    this._SubmitUserID = value;
                    this.SendPropertyChanged("SubmitUserID");

                }
            }
        }

System.Nullable<DateTime> _SubmitTime;
/// <summary>
/// 提交时间
	/// </summary>
[Way.EntityDB.WayLinqColumnAttribute(Comment="",Caption="提交时间",Storage = "_SubmitTime",DbType="datetime")]
        public System.Nullable<DateTime> SubmitTime
        {
            get
            {
                return this._SubmitTime;
            }
            set
            {
                if ((this._SubmitTime != value))
                {
                    this.SendPropertyChanging("SubmitTime",this._SubmitTime,value);
                    this._SubmitTime = value;
                    this.SendPropertyChanged("SubmitTime");

                }
            }
        }

System.Nullable<Int32> _HandlerID;
/// <summary>
/// 处理者ID
	/// </summary>
[Way.EntityDB.WayLinqColumnAttribute(Comment="",Caption="处理者ID",Storage = "_HandlerID",DbType="int")]
        public System.Nullable<Int32> HandlerID
        {
            get
            {
                return this._HandlerID;
            }
            set
            {
                if ((this._HandlerID != value))
                {
                    this.SendPropertyChanging("HandlerID",this._HandlerID,value);
                    this._HandlerID = value;
                    this.SendPropertyChanged("HandlerID");

                }
            }
        }

System.Nullable<DateTime> _LastDate;
/// <summary>
/// 最后反馈时间
	/// </summary>
[Way.EntityDB.WayLinqColumnAttribute(Comment="",Caption="最后反馈时间",Storage = "_LastDate",DbType="datetime")]
        public System.Nullable<DateTime> LastDate
        {
            get
            {
                return this._LastDate;
            }
            set
            {
                if ((this._LastDate != value))
                {
                    this.SendPropertyChanging("LastDate",this._LastDate,value);
                    this._LastDate = value;
                    this.SendPropertyChanged("LastDate");

                }
            }
        }

System.Nullable<DateTime> _FinishTime;
/// <summary>
/// 处理完毕时间
	/// </summary>
[Way.EntityDB.WayLinqColumnAttribute(Comment="",Caption="处理完毕时间",Storage = "_FinishTime",DbType="datetime")]
        public System.Nullable<DateTime> FinishTime
        {
            get
            {
                return this._FinishTime;
            }
            set
            {
                if ((this._FinishTime != value))
                {
                    this.SendPropertyChanging("FinishTime",this._FinishTime,value);
                    this._FinishTime = value;
                    this.SendPropertyChanged("FinishTime");

                }
            }
        }

Bug_StatusEnum _Status=(Bug_StatusEnum)int.MinValue;
/// <summary>
/// 当前状态
	/// </summary>
[Way.EntityDB.WayLinqColumnAttribute(Comment="",Caption="当前状态",Storage = "_Status",DbType="int")]
        public Bug_StatusEnum Status
        {
            get
            {
                return this._Status;
            }
            set
            {
                if ((this._Status != value))
                {
                    this.SendPropertyChanging("Status",this._Status,value);
                    this._Status = value;
                    this.SendPropertyChanged("Status");

                }
            }
        }
}}
namespace EJ{


    /// <summary>
	/// 数据表
	/// </summary>
    [Way.EntityDB.Attributes.Table("DBTable","id")]
    public class DBTable :Way.EntityDB.DataItem
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
[Way.EntityDB.WayLinqColumnAttribute(Comment="",Caption="id",Storage = "_id",DbType="int" ,IsPrimaryKey=true,IsDbGenerated=true)]
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

String _caption;
/// <summary>
/// caption
	/// </summary>
[Way.EntityDB.WayLinqColumnAttribute(Comment="",Caption="caption",Storage = "_caption",DbType="varchar(50)")]
        public String caption
        {
            get
            {
                return this._caption;
            }
            set
            {
                if ((this._caption != value))
                {
                    this.SendPropertyChanging("caption",this._caption,value);
                    this._caption = value;
                    this.SendPropertyChanged("caption");

                }
            }
        }

String _Name;
/// <summary>
/// Name
	/// </summary>
[Way.EntityDB.WayLinqColumnAttribute(Comment="",Caption="Name",Storage = "_Name",DbType="varchar(50)")]
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

System.Nullable<Int32> _DatabaseID;
/// <summary>
/// DatabaseID
	/// </summary>
[Way.EntityDB.WayLinqColumnAttribute(Comment="",Caption="DatabaseID",Storage = "_DatabaseID",DbType="int")]
        public System.Nullable<Int32> DatabaseID
        {
            get
            {
                return this._DatabaseID;
            }
            set
            {
                if ((this._DatabaseID != value))
                {
                    this.SendPropertyChanging("DatabaseID",this._DatabaseID,value);
                    this._DatabaseID = value;
                    this.SendPropertyChanged("DatabaseID");

                }
            }
        }

System.Nullable<Int32> _iLock=0;
/// <summary>
/// iLock
	/// </summary>
[Way.EntityDB.WayLinqColumnAttribute(Comment="",Caption="iLock",Storage = "_iLock",DbType="int")]
        public System.Nullable<Int32> iLock
        {
            get
            {
                return this._iLock;
            }
            set
            {
                if ((this._iLock != value))
                {
                    this.SendPropertyChanging("iLock",this._iLock,value);
                    this._iLock = value;
                    this.SendPropertyChanged("iLock");

                }
            }
        }
}}
namespace EJ{


    /// <summary>
	/// 字段
	/// </summary>
    [Way.EntityDB.Attributes.Table("DBColumn","id")]
    public class DBColumn :Way.EntityDB.DataItem
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
[Way.EntityDB.WayLinqColumnAttribute(Comment="",Caption="id",Storage = "_id",DbType="int" ,IsPrimaryKey=true,IsDbGenerated=true)]
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

String _caption;
/// <summary>
/// caption
	/// </summary>
[Way.EntityDB.WayLinqColumnAttribute(Comment="",Caption="caption",Storage = "_caption",DbType="varchar(200)")]
        public String caption
        {
            get
            {
                return this._caption;
            }
            set
            {
                if ((this._caption != value))
                {
                    this.SendPropertyChanging("caption",this._caption,value);
                    this._caption = value;
                    this.SendPropertyChanged("caption");

                }
            }
        }

String _Name;
/// <summary>
/// Name
	/// </summary>
[Way.EntityDB.WayLinqColumnAttribute(Comment="",Caption="Name",Storage = "_Name",DbType="varchar(50)")]
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

System.Nullable<Boolean> _IsAutoIncrement=false;
/// <summary>
/// 自增长
	/// </summary>
[Way.EntityDB.WayLinqColumnAttribute(Comment="",Caption="自增长",Storage = "_IsAutoIncrement",DbType="bit")]
        public System.Nullable<Boolean> IsAutoIncrement
        {
            get
            {
                return this._IsAutoIncrement;
            }
            set
            {
                if ((this._IsAutoIncrement != value))
                {
                    this.SendPropertyChanging("IsAutoIncrement",this._IsAutoIncrement,value);
                    this._IsAutoIncrement = value;
                    this.SendPropertyChanged("IsAutoIncrement");

                }
            }
        }

System.Nullable<Boolean> _CanNull=true;
/// <summary>
/// 可以为空
	/// </summary>
[Way.EntityDB.WayLinqColumnAttribute(Comment="",Caption="可以为空",Storage = "_CanNull",DbType="bit")]
        public System.Nullable<Boolean> CanNull
        {
            get
            {
                return this._CanNull;
            }
            set
            {
                if ((this._CanNull != value))
                {
                    this.SendPropertyChanging("CanNull",this._CanNull,value);
                    this._CanNull = value;
                    this.SendPropertyChanged("CanNull");

                }
            }
        }

String _dbType;
/// <summary>
/// 数据库字段类型
	/// </summary>
[Way.EntityDB.WayLinqColumnAttribute(Comment="",Caption="数据库字段类型",Storage = "_dbType",DbType="varchar(50)")]
        public String dbType
        {
            get
            {
                return this._dbType;
            }
            set
            {
                if ((this._dbType != value))
                {
                    this.SendPropertyChanging("dbType",this._dbType,value);
                    this._dbType = value;
                    this.SendPropertyChanged("dbType");

                }
            }
        }

String _Type;
/// <summary>
/// c#类型
	/// </summary>
[Way.EntityDB.WayLinqColumnAttribute(Comment="",Caption="c#类型",Storage = "_Type",DbType="varchar(50)")]
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

String _EnumDefine;
/// <summary>
/// Enum定义
	/// </summary>
[Way.EntityDB.WayLinqColumnAttribute(Comment="",Caption="Enum定义",Storage = "_EnumDefine",DbType="varchar(300)")]
        public String EnumDefine
        {
            get
            {
                return this._EnumDefine;
            }
            set
            {
                if ((this._EnumDefine != value))
                {
                    this.SendPropertyChanging("EnumDefine",this._EnumDefine,value);
                    this._EnumDefine = value;
                    this.SendPropertyChanged("EnumDefine");

                }
            }
        }

String _length;
/// <summary>
/// 长度
	/// </summary>
[Way.EntityDB.WayLinqColumnAttribute(Comment="",Caption="长度",Storage = "_length",DbType="varchar(50)")]
        public String length
        {
            get
            {
                return this._length;
            }
            set
            {
                if ((this._length != value))
                {
                    this.SendPropertyChanging("length",this._length,value);
                    this._length = value;
                    this.SendPropertyChanged("length");

                }
            }
        }

String _defaultValue;
/// <summary>
/// 默认值
	/// </summary>
[Way.EntityDB.WayLinqColumnAttribute(Comment="",Caption="默认值",Storage = "_defaultValue",DbType="varchar(200)")]
        public String defaultValue
        {
            get
            {
                return this._defaultValue;
            }
            set
            {
                if ((this._defaultValue != value))
                {
                    this.SendPropertyChanging("defaultValue",this._defaultValue,value);
                    this._defaultValue = value;
                    this.SendPropertyChanged("defaultValue");

                }
            }
        }

System.Nullable<Int32> _TableID;
/// <summary>
/// TableID
	/// </summary>
[Way.EntityDB.WayLinqColumnAttribute(Comment="",Caption="TableID",Storage = "_TableID",DbType="int")]
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

System.Nullable<Boolean> _IsPKID=false;
/// <summary>
/// 是否是主键
	/// </summary>
[Way.EntityDB.WayLinqColumnAttribute(Comment="",Caption="是否是主键",Storage = "_IsPKID",DbType="bit")]
        public System.Nullable<Boolean> IsPKID
        {
            get
            {
                return this._IsPKID;
            }
            set
            {
                if ((this._IsPKID != value))
                {
                    this.SendPropertyChanging("IsPKID",this._IsPKID,value);
                    this._IsPKID = value;
                    this.SendPropertyChanged("IsPKID");

                }
            }
        }

System.Nullable<Int32> _orderid=0;
/// <summary>
/// orderid
	/// </summary>
[Way.EntityDB.WayLinqColumnAttribute(Comment="",Caption="orderid",Storage = "_orderid",DbType="int")]
        public System.Nullable<Int32> orderid
        {
            get
            {
                return this._orderid;
            }
            set
            {
                if ((this._orderid != value))
                {
                    this.SendPropertyChanging("orderid",this._orderid,value);
                    this._orderid = value;
                    this.SendPropertyChanged("orderid");

                }
            }
        }
}}
namespace EJ{


    /// <summary>
	/// 数据表权限
	/// </summary>
    [Way.EntityDB.Attributes.Table("TablePower","id")]
    public class TablePower :Way.EntityDB.DataItem
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
[Way.EntityDB.WayLinqColumnAttribute(Comment="",Caption="id",Storage = "_id",DbType="int" ,IsPrimaryKey=true,IsDbGenerated=true)]
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
/// UserID
	/// </summary>
[Way.EntityDB.WayLinqColumnAttribute(Comment="",Caption="UserID",Storage = "_UserID",DbType="int")]
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

System.Nullable<Int32> _TableID;
/// <summary>
/// TableID
	/// </summary>
[Way.EntityDB.WayLinqColumnAttribute(Comment="",Caption="TableID",Storage = "_TableID",DbType="int")]
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
}}
namespace EJ{


    /// <summary>
	/// 项目权限
	/// </summary>
    [Way.EntityDB.Attributes.Table("ProjectPower","id")]
    public class ProjectPower :Way.EntityDB.DataItem
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
[Way.EntityDB.WayLinqColumnAttribute(Comment="",Caption="id",Storage = "_id",DbType="int" ,IsPrimaryKey=true,IsDbGenerated=true)]
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

System.Nullable<Int32> _ProjectID;
/// <summary>
/// ProjectID
	/// </summary>
[Way.EntityDB.WayLinqColumnAttribute(Comment="",Caption="ProjectID",Storage = "_ProjectID",DbType="int")]
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

System.Nullable<Int32> _UserID;
/// <summary>
/// UserID
	/// </summary>
[Way.EntityDB.WayLinqColumnAttribute(Comment="",Caption="UserID",Storage = "_UserID",DbType="int")]
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
}}
namespace EJ{


    /// <summary>
	/// 数据模块
	/// </summary>
    [Way.EntityDB.Attributes.Table("DBModule","id")]
    public class DBModule :Way.EntityDB.DataItem
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
[Way.EntityDB.WayLinqColumnAttribute(Comment="",Caption="id",Storage = "_id",DbType="int" ,IsPrimaryKey=true,IsDbGenerated=true)]
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

String _Name;
/// <summary>
/// Name
	/// </summary>
[Way.EntityDB.WayLinqColumnAttribute(Comment="",Caption="Name",Storage = "_Name",DbType="varchar(50)")]
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

System.Nullable<Int32> _DatabaseID;
/// <summary>
/// DatabaseID
	/// </summary>
[Way.EntityDB.WayLinqColumnAttribute(Comment="",Caption="DatabaseID",Storage = "_DatabaseID",DbType="int")]
        public System.Nullable<Int32> DatabaseID
        {
            get
            {
                return this._DatabaseID;
            }
            set
            {
                if ((this._DatabaseID != value))
                {
                    this.SendPropertyChanging("DatabaseID",this._DatabaseID,value);
                    this._DatabaseID = value;
                    this.SendPropertyChanged("DatabaseID");

                }
            }
        }

System.Nullable<Boolean> _IsFolder=false;
/// <summary>
/// IsFolder
	/// </summary>
[Way.EntityDB.WayLinqColumnAttribute(Comment="",Caption="IsFolder",Storage = "_IsFolder",DbType="bit")]
        public System.Nullable<Boolean> IsFolder
        {
            get
            {
                return this._IsFolder;
            }
            set
            {
                if ((this._IsFolder != value))
                {
                    this.SendPropertyChanging("IsFolder",this._IsFolder,value);
                    this._IsFolder = value;
                    this.SendPropertyChanged("IsFolder");

                }
            }
        }

System.Nullable<Int32> _parentID;
/// <summary>
/// parentID
	/// </summary>
[Way.EntityDB.WayLinqColumnAttribute(Comment="",Caption="parentID",Storage = "_parentID",DbType="int")]
        public System.Nullable<Int32> parentID
        {
            get
            {
                return this._parentID;
            }
            set
            {
                if ((this._parentID != value))
                {
                    this.SendPropertyChanging("parentID",this._parentID,value);
                    this._parentID = value;
                    this.SendPropertyChanged("parentID");

                }
            }
        }
}}
namespace EJ{


    /// <summary>
	/// 级联删除
	/// </summary>
    [Way.EntityDB.Attributes.Table("DBDeleteConfig","id")]
    public class DBDeleteConfig :Way.EntityDB.DataItem
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
[Way.EntityDB.WayLinqColumnAttribute(Comment="",Caption="id",Storage = "_id",DbType="int" ,IsPrimaryKey=true,IsDbGenerated=true)]
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
[Way.EntityDB.WayLinqColumnAttribute(Comment="",Caption="TableID",Storage = "_TableID",DbType="int")]
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

System.Nullable<Int32> _RelaTableID;
/// <summary>
/// 关联表ID
	/// </summary>
[Way.EntityDB.WayLinqColumnAttribute(Comment="",Caption="关联表ID",Storage = "_RelaTableID",DbType="int")]
        public System.Nullable<Int32> RelaTableID
        {
            get
            {
                return this._RelaTableID;
            }
            set
            {
                if ((this._RelaTableID != value))
                {
                    this.SendPropertyChanging("RelaTableID",this._RelaTableID,value);
                    this._RelaTableID = value;
                    this.SendPropertyChanged("RelaTableID");

                }
            }
        }

String _RelaTable_Desc;
/// <summary>
/// RelaTable_Desc
	/// </summary>
[Way.EntityDB.WayLinqColumnAttribute(Comment="",Caption="RelaTable_Desc",Storage = "_RelaTable_Desc",DbType="varchar(50)")]
        public String RelaTable_Desc
        {
            get
            {
                return this._RelaTable_Desc;
            }
            set
            {
                if ((this._RelaTable_Desc != value))
                {
                    this.SendPropertyChanging("RelaTable_Desc",this._RelaTable_Desc,value);
                    this._RelaTable_Desc = value;
                    this.SendPropertyChanged("RelaTable_Desc");

                }
            }
        }

System.Nullable<Int32> _RelaColumID;
/// <summary>
/// 关联字段的ID
	/// </summary>
[Way.EntityDB.WayLinqColumnAttribute(Comment="",Caption="关联字段的ID",Storage = "_RelaColumID",DbType="int")]
        public System.Nullable<Int32> RelaColumID
        {
            get
            {
                return this._RelaColumID;
            }
            set
            {
                if ((this._RelaColumID != value))
                {
                    this.SendPropertyChanging("RelaColumID",this._RelaColumID,value);
                    this._RelaColumID = value;
                    this.SendPropertyChanged("RelaColumID");

                }
            }
        }

String _RelaColumn_Desc;
/// <summary>
/// RelaColumn_Desc
	/// </summary>
[Way.EntityDB.WayLinqColumnAttribute(Comment="",Caption="RelaColumn_Desc",Storage = "_RelaColumn_Desc",DbType="varchar(50)")]
        public String RelaColumn_Desc
        {
            get
            {
                return this._RelaColumn_Desc;
            }
            set
            {
                if ((this._RelaColumn_Desc != value))
                {
                    this.SendPropertyChanging("RelaColumn_Desc",this._RelaColumn_Desc,value);
                    this._RelaColumn_Desc = value;
                    this.SendPropertyChanged("RelaColumn_Desc");

                }
            }
        }
}}
namespace EJ{


    /// <summary>
	/// TableInModule
	/// </summary>
    [Way.EntityDB.Attributes.Table("TableInModule","id")]
    public class TableInModule :Way.EntityDB.DataItem
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
[Way.EntityDB.WayLinqColumnAttribute(Comment="",Caption="id",Storage = "_id",DbType="int" ,IsPrimaryKey=true,IsDbGenerated=true)]
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
[Way.EntityDB.WayLinqColumnAttribute(Comment="",Caption="TableID",Storage = "_TableID",DbType="int")]
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
[Way.EntityDB.WayLinqColumnAttribute(Comment="",Caption="ModuleID",Storage = "_ModuleID",DbType="int")]
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
[Way.EntityDB.WayLinqColumnAttribute(Comment="",Caption="x",Storage = "_x",DbType="int")]
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
[Way.EntityDB.WayLinqColumnAttribute(Comment="",Caption="y",Storage = "_y",DbType="int")]
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
[Way.EntityDB.WayLinqColumnAttribute(Comment="",Caption="临时变量",Storage = "_flag",DbType="varchar(50)")]
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
[Way.EntityDB.WayLinqColumnAttribute(Comment="",Caption="flag2",Storage = "_flag2",DbType="varchar(50)")]
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
namespace EJ{


    /// <summary>
	/// 唯一值索引
	/// </summary>
    [Way.EntityDB.Attributes.Table("IDXIndex","id")]
    public class IDXIndex :Way.EntityDB.DataItem
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
[Way.EntityDB.WayLinqColumnAttribute(Comment="",Caption="id",Storage = "_id",DbType="int" ,IsPrimaryKey=true,IsDbGenerated=true)]
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
[Way.EntityDB.WayLinqColumnAttribute(Comment="",Caption="TableID",Storage = "_TableID",DbType="int")]
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

String _Keys;
/// <summary>
/// Keys
	/// </summary>
[Way.EntityDB.WayLinqColumnAttribute(Comment="",Caption="Keys",Storage = "_Keys",DbType="varchar(100)")]
        public String Keys
        {
            get
            {
                return this._Keys;
            }
            set
            {
                if ((this._Keys != value))
                {
                    this.SendPropertyChanging("Keys",this._Keys,value);
                    this._Keys = value;
                    this.SendPropertyChanged("Keys");

                }
            }
        }

System.Nullable<Boolean> _IsUnique=true;
/// <summary>
/// 是否唯一索引
	/// </summary>
[Way.EntityDB.WayLinqColumnAttribute(Comment="",Caption="是否唯一索引",Storage = "_IsUnique",DbType="bit")]
        public System.Nullable<Boolean> IsUnique
        {
            get
            {
                return this._IsUnique;
            }
            set
            {
                if ((this._IsUnique != value))
                {
                    this.SendPropertyChanging("IsUnique",this._IsUnique,value);
                    this._IsUnique = value;
                    this.SendPropertyChanged("IsUnique");

                }
            }
        }

System.Nullable<Boolean> _IsClustered=false;
/// <summary>
/// 是否聚焦
	/// </summary>
[Way.EntityDB.WayLinqColumnAttribute(Comment="",Caption="是否聚焦",Storage = "_IsClustered",DbType="bit")]
        public System.Nullable<Boolean> IsClustered
        {
            get
            {
                return this._IsClustered;
            }
            set
            {
                if ((this._IsClustered != value))
                {
                    this.SendPropertyChanging("IsClustered",this._IsClustered,value);
                    this._IsClustered = value;
                    this.SendPropertyChanged("IsClustered");

                }
            }
        }
}}
namespace EJ{


    /// <summary>
	/// Bug处理历史记录
	/// </summary>
    [Way.EntityDB.Attributes.Table("BugHandleHistory","id")]
    public class BugHandleHistory :Way.EntityDB.DataItem
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
[Way.EntityDB.WayLinqColumnAttribute(Comment="",Caption="",Storage = "_id",DbType="int" ,IsPrimaryKey=true,IsDbGenerated=true,CanBeNull=false)]
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

System.Nullable<Int32> _BugID;
/// <summary>
/// 
	/// </summary>
[Way.EntityDB.WayLinqColumnAttribute(Comment="",Caption="",Storage = "_BugID",DbType="int")]
        public System.Nullable<Int32> BugID
        {
            get
            {
                return this._BugID;
            }
            set
            {
                if ((this._BugID != value))
                {
                    this.SendPropertyChanging("BugID",this._BugID,value);
                    this._BugID = value;
                    this.SendPropertyChanged("BugID");

                }
            }
        }

System.Nullable<Int32> _UserID;
/// <summary>
/// 发标者ID
	/// </summary>
[Way.EntityDB.WayLinqColumnAttribute(Comment="",Caption="发标者ID",Storage = "_UserID",DbType="int")]
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

Byte[] _content;
/// <summary>
/// 内容
	/// </summary>
[Way.EntityDB.WayLinqColumnAttribute(Comment="",Caption="内容",Storage = "_content",DbType="image")]
        public Byte[] content
        {
            get
            {
                return this._content;
            }
            set
            {
                if ((this._content != value))
                {
                    this.SendPropertyChanging("content",this._content,value);
                    this._content = value;
                    this.SendPropertyChanged("content");

                }
            }
        }

System.Nullable<DateTime> _SendTime;
/// <summary>
/// 发表时间
	/// </summary>
[Way.EntityDB.WayLinqColumnAttribute(Comment="",Caption="发表时间",Storage = "_SendTime",DbType="datetime")]
        public System.Nullable<DateTime> SendTime
        {
            get
            {
                return this._SendTime;
            }
            set
            {
                if ((this._SendTime != value))
                {
                    this.SendPropertyChanging("SendTime",this._SendTime,value);
                    this._SendTime = value;
                    this.SendPropertyChanged("SendTime");

                }
            }
        }
}}
namespace EJ{


    /// <summary>
	/// Bug附带截图
	/// </summary>
    [Way.EntityDB.Attributes.Table("BugImages","id")]
    public class BugImages :Way.EntityDB.DataItem
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
[Way.EntityDB.WayLinqColumnAttribute(Comment="",Caption="",Storage = "_id",DbType="int" ,IsPrimaryKey=true,IsDbGenerated=true,CanBeNull=false)]
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

System.Nullable<Int32> _BugID;
/// <summary>
/// 
	/// </summary>
[Way.EntityDB.WayLinqColumnAttribute(Comment="",Caption="",Storage = "_BugID",DbType="int")]
        public System.Nullable<Int32> BugID
        {
            get
            {
                return this._BugID;
            }
            set
            {
                if ((this._BugID != value))
                {
                    this.SendPropertyChanging("BugID",this._BugID,value);
                    this._BugID = value;
                    this.SendPropertyChanged("BugID");

                }
            }
        }

Byte[] _content;
/// <summary>
/// 
	/// </summary>
[Way.EntityDB.WayLinqColumnAttribute(Comment="",Caption="",Storage = "_content",DbType="image")]
        public Byte[] content
        {
            get
            {
                return this._content;
            }
            set
            {
                if ((this._content != value))
                {
                    this.SendPropertyChanging("content",this._content,value);
                    this._content = value;
                    this.SendPropertyChanged("content");

                }
            }
        }

System.Nullable<Int32> _orderID;
/// <summary>
/// 排序
	/// </summary>
[Way.EntityDB.WayLinqColumnAttribute(Comment="",Caption="排序",Storage = "_orderID",DbType="int")]
        public System.Nullable<Int32> orderID
        {
            get
            {
                return this._orderID;
            }
            set
            {
                if ((this._orderID != value))
                {
                    this.SendPropertyChanging("orderID",this._orderID,value);
                    this._orderID = value;
                    this.SendPropertyChanged("orderID");

                }
            }
        }
}}
namespace EJ{


    /// <summary>
	/// 引入的dll
	/// </summary>
    [Way.EntityDB.Attributes.Table("DLLImport","id")]
    public class DLLImport :Way.EntityDB.DataItem
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
[Way.EntityDB.WayLinqColumnAttribute(Comment="",Caption="",Storage = "_id",DbType="int" ,IsPrimaryKey=true,IsDbGenerated=true,CanBeNull=false)]
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
[Way.EntityDB.WayLinqColumnAttribute(Comment="",Caption="dll文件路径",Storage = "_path",DbType="varchar(200)")]
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
[Way.EntityDB.WayLinqColumnAttribute(Comment="",Caption="",Storage = "_ProjectID",DbType="int")]
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
namespace EJ{


    /// <summary>
	/// 接口设计的目录结构
	/// </summary>
    [Way.EntityDB.Attributes.Table("InterfaceModule","id")]
    public class InterfaceModule :Way.EntityDB.DataItem
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
[Way.EntityDB.WayLinqColumnAttribute(Comment="",Caption="",Storage = "_id",DbType="int" ,IsPrimaryKey=true,IsDbGenerated=true,CanBeNull=false)]
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

System.Nullable<Int32> _ProjectID;
/// <summary>
/// 
	/// </summary>
[Way.EntityDB.WayLinqColumnAttribute(Comment="",Caption="",Storage = "_ProjectID",DbType="int")]
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

String _Name;
/// <summary>
/// 
	/// </summary>
[Way.EntityDB.WayLinqColumnAttribute(Comment="",Caption="",Storage = "_Name",DbType="varchar(50)")]
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

System.Nullable<Int32> _ParentID=0;
/// <summary>
/// 
	/// </summary>
[Way.EntityDB.WayLinqColumnAttribute(Comment="",Caption="",Storage = "_ParentID",DbType="int")]
        public System.Nullable<Int32> ParentID
        {
            get
            {
                return this._ParentID;
            }
            set
            {
                if ((this._ParentID != value))
                {
                    this.SendPropertyChanging("ParentID",this._ParentID,value);
                    this._ParentID = value;
                    this.SendPropertyChanged("ParentID");

                }
            }
        }

System.Nullable<Boolean> _IsFolder=false;
/// <summary>
/// 
	/// </summary>
[Way.EntityDB.WayLinqColumnAttribute(Comment="",Caption="",Storage = "_IsFolder",DbType="bit")]
        public System.Nullable<Boolean> IsFolder
        {
            get
            {
                return this._IsFolder;
            }
            set
            {
                if ((this._IsFolder != value))
                {
                    this.SendPropertyChanging("IsFolder",this._IsFolder,value);
                    this._IsFolder = value;
                    this.SendPropertyChanged("IsFolder");

                }
            }
        }

System.Nullable<Int32> _LockUserId;
/// <summary>
/// 已经被某人锁定
	/// </summary>
[Way.EntityDB.WayLinqColumnAttribute(Comment="",Caption="已经被某人锁定",Storage = "_LockUserId",DbType="int")]
        public System.Nullable<Int32> LockUserId
        {
            get
            {
                return this._LockUserId;
            }
            set
            {
                if ((this._LockUserId != value))
                {
                    this.SendPropertyChanging("LockUserId",this._LockUserId,value);
                    this._LockUserId = value;
                    this.SendPropertyChanged("LockUserId");

                }
            }
        }
}}
namespace EJ{


    /// <summary>
	/// 
	/// </summary>
    [Way.EntityDB.Attributes.Table("InterfaceInModule","id")]
    public class InterfaceInModule :Way.EntityDB.DataItem
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
[Way.EntityDB.WayLinqColumnAttribute(Comment="",Caption="",Storage = "_id",DbType="int" ,IsPrimaryKey=true,IsDbGenerated=true,CanBeNull=false)]
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
[Way.EntityDB.WayLinqColumnAttribute(Comment="",Caption="",Storage = "_ModuleID",DbType="int")]
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
[Way.EntityDB.WayLinqColumnAttribute(Comment="",Caption="",Storage = "_x",DbType="int")]
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
[Way.EntityDB.WayLinqColumnAttribute(Comment="",Caption="",Storage = "_y",DbType="int")]
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
[Way.EntityDB.WayLinqColumnAttribute(Comment="",Caption="",Storage = "_Type",DbType="varchar(100)")]
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
[Way.EntityDB.WayLinqColumnAttribute(Comment="",Caption="",Storage = "_JsonData",DbType="text")]
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
[Way.EntityDB.WayLinqColumnAttribute(Comment="",Caption="",Storage = "_width",DbType="int")]
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
[Way.EntityDB.WayLinqColumnAttribute(Comment="",Caption="",Storage = "_height",DbType="int")]
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
namespace EJ{


    /// <summary>
	/// InterfaceModule权限设定表
	/// </summary>
    [Way.EntityDB.Attributes.Table("InterfaceModulePower","id")]
    public class InterfaceModulePower :Way.EntityDB.DataItem
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
[Way.EntityDB.WayLinqColumnAttribute(Comment="",Caption="",Storage = "_id",DbType="int" ,IsPrimaryKey=true,IsDbGenerated=true,CanBeNull=false)]
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
[Way.EntityDB.WayLinqColumnAttribute(Comment="",Caption="",Storage = "_UserID",DbType="int")]
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
[Way.EntityDB.WayLinqColumnAttribute(Comment="",Caption="",Storage = "_ModuleID",DbType="int")]
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

namespace EJ.DB{
    /// <summary>
	/// 
	/// </summary>
    public class EasyJob : Way.EntityDB.DBContext
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="dbType"></param>
        public EasyJob(string connection, Way.EntityDB.DatabaseType dbType): base(connection, dbType)
        {
            if (!setEvented)
            {
                Way.EntityDB.Design.DBUpgrade.Upgrade(this,_designData);
                setEvented = true;
                Way.EntityDB.DBContext.BeforeDelete += Database_BeforeDelete;
            }
        }

        static bool setEvented = false;
 

        static void Database_BeforeDelete(object sender, Way.EntityDB.DatabaseModifyEventArg e)
        {
            var db =  sender as EasyJob;
            if (db == null)
                return;


                if (e.DataItem is EJ.Project)
                {
                    var deletingItem = (EJ.Project)e.DataItem;
                    
    var items0 = (from m in db.Databases
                    where m.ProjectID == deletingItem.id
                    select new EJ.Databases
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

    var items1 = (from m in db.DLLImport
                    where m.ProjectID == deletingItem.id
                    select new EJ.DLLImport
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

    var items2 = (from m in db.InterfaceModule
                    where m.ProjectID == deletingItem.id
                    select new EJ.InterfaceModule
                    {
                        id = m.id
                    });
while(true)
{
    var data2del = items2.Take(100).ToList();
if(data2del.Count() ==0)
break;
            foreach (var t in data2del)
            {
                db.Delete(t);
            }
}

    var items3 = (from m in db.ProjectPower
                    where m.ProjectID == deletingItem.id
                    select new EJ.ProjectPower
                    {
                        id = m.id
                    });
while(true)
{
    var data2del = items3.Take(100).ToList();
if(data2del.Count() ==0)
break;
            foreach (var t in data2del)
            {
                db.Delete(t);
            }
}

                }

                if (e.DataItem is EJ.Databases)
                {
                    var deletingItem = (EJ.Databases)e.DataItem;
                    
    var items0 = (from m in db.DBPower
                    where m.DatabaseID == deletingItem.id
                    select new EJ.DBPower
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

    var items1 = (from m in db.DBTable
                    where m.DatabaseID == deletingItem.id
                    select new EJ.DBTable
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

    var items2 = (from m in db.DBModule
                    where m.DatabaseID == deletingItem.id
                    select new EJ.DBModule
                    {
                        id = m.id
                    });
while(true)
{
    var data2del = items2.Take(100).ToList();
if(data2del.Count() ==0)
break;
            foreach (var t in data2del)
            {
                db.Delete(t);
            }
}

                }

                if (e.DataItem is EJ.User)
                {
                    var deletingItem = (EJ.User)e.DataItem;
                    
    var items0 = (from m in db.InterfaceModulePower
                    where m.UserID == deletingItem.id
                    select new EJ.InterfaceModulePower
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

    var items1 = (from m in db.DBPower
                    where m.UserID == deletingItem.id
                    select new EJ.DBPower
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

    var items2 = (from m in db.TablePower
                    where m.UserID == deletingItem.id
                    select new EJ.TablePower
                    {
                        id = m.id
                    });
while(true)
{
    var data2del = items2.Take(100).ToList();
if(data2del.Count() ==0)
break;
            foreach (var t in data2del)
            {
                db.Delete(t);
            }
}

    var items3 = (from m in db.ProjectPower
                    where m.UserID == deletingItem.id
                    select new EJ.ProjectPower
                    {
                        id = m.id
                    });
while(true)
{
    var data2del = items3.Take(100).ToList();
if(data2del.Count() ==0)
break;
            foreach (var t in data2del)
            {
                db.Delete(t);
            }
}

                }

                if (e.DataItem is EJ.Bug)
                {
                    var deletingItem = (EJ.Bug)e.DataItem;
                    
    var items0 = (from m in db.BugHandleHistory
                    where m.BugID == deletingItem.id
                    select new EJ.BugHandleHistory
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

    var items1 = (from m in db.BugImages
                    where m.BugID == deletingItem.id
                    select new EJ.BugImages
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

                if (e.DataItem is EJ.DBTable)
                {
                    var deletingItem = (EJ.DBTable)e.DataItem;
                    
    var items0 = (from m in db.IDXIndex
                    where m.TableID == deletingItem.id
                    select new EJ.IDXIndex
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

    var items1 = (from m in db.DBDeleteConfig
                    where m.TableID == deletingItem.id
                    select new EJ.DBDeleteConfig
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

    var items2 = (from m in db.DBDeleteConfig
                    where m.RelaTableID == deletingItem.id
                    select new EJ.DBDeleteConfig
                    {
                        id = m.id
                    });
while(true)
{
    var data2del = items2.Take(100).ToList();
if(data2del.Count() ==0)
break;
            foreach (var t in data2del)
            {
                db.Delete(t);
            }
}

    var items3 = (from m in db.DBColumn
                    where m.TableID == deletingItem.id
                    select new EJ.DBColumn
                    {
                        id = m.id
                    });
while(true)
{
    var data2del = items3.Take(100).ToList();
if(data2del.Count() ==0)
break;
            foreach (var t in data2del)
            {
                db.Delete(t);
            }
}

    var items4 = (from m in db.TableInModule
                    where m.TableID == deletingItem.id
                    select new EJ.TableInModule
                    {
                        id = m.id
                    });
while(true)
{
    var data2del = items4.Take(100).ToList();
if(data2del.Count() ==0)
break;
            foreach (var t in data2del)
            {
                db.Delete(t);
            }
}

    var items5 = (from m in db.TablePower
                    where m.TableID == deletingItem.id
                    select new EJ.TablePower
                    {
                        id = m.id
                    });
while(true)
{
    var data2del = items5.Take(100).ToList();
if(data2del.Count() ==0)
break;
            foreach (var t in data2del)
            {
                db.Delete(t);
            }
}

                }

                if (e.DataItem is EJ.DBColumn)
                {
                    var deletingItem = (EJ.DBColumn)e.DataItem;
                    
    var items0 = (from m in db.DBDeleteConfig
                    where m.RelaColumID == deletingItem.id
                    select new EJ.DBDeleteConfig
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

                if (e.DataItem is EJ.InterfaceModule)
                {
                    var deletingItem = (EJ.InterfaceModule)e.DataItem;
                    
    var items0 = (from m in db.InterfaceInModule
                    where m.ModuleID == deletingItem.id
                    select new EJ.InterfaceInModule
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

    var items1 = (from m in db.InterfaceModulePower
                    where m.ModuleID == deletingItem.id
                    select new EJ.InterfaceModulePower
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
   modelBuilder.Entity<EJ.Project>().HasKey(m => m.id);
modelBuilder.Entity<EJ.Databases>().HasKey(m => m.id);
modelBuilder.Entity<EJ.User>().HasKey(m => m.id);
modelBuilder.Entity<EJ.DBPower>().HasKey(m => m.id);
modelBuilder.Entity<EJ.Bug>().HasKey(m => m.id);
modelBuilder.Entity<EJ.DBTable>().HasKey(m => m.id);
modelBuilder.Entity<EJ.DBColumn>().HasKey(m => m.id);
modelBuilder.Entity<EJ.TablePower>().HasKey(m => m.id);
modelBuilder.Entity<EJ.ProjectPower>().HasKey(m => m.id);
modelBuilder.Entity<EJ.DBModule>().HasKey(m => m.id);
modelBuilder.Entity<EJ.DBDeleteConfig>().HasKey(m => m.id);
modelBuilder.Entity<EJ.TableInModule>().HasKey(m => m.id);
modelBuilder.Entity<EJ.IDXIndex>().HasKey(m => m.id);
modelBuilder.Entity<EJ.BugHandleHistory>().HasKey(m => m.id);
modelBuilder.Entity<EJ.BugImages>().HasKey(m => m.id);
modelBuilder.Entity<EJ.DLLImport>().HasKey(m => m.id);
modelBuilder.Entity<EJ.InterfaceModule>().HasKey(m => m.id);
modelBuilder.Entity<EJ.InterfaceInModule>().HasKey(m => m.id);
modelBuilder.Entity<EJ.InterfaceModulePower>().HasKey(m => m.id);
}

System.Linq.IQueryable<EJ.Project> _Project;
 /// <summary>
        /// 项目
        /// </summary>
        public virtual System.Linq.IQueryable<EJ.Project> Project
        {
             get
            {
                if (_Project == null)
                {
                    _Project = new Way.EntityDB.WayQueryable<EJ.Project>(this.Set<EJ.Project>());
                }
                return _Project;
            }
        }

System.Linq.IQueryable<EJ.Databases> _Databases;
 /// <summary>
        /// 数据库
        /// </summary>
        public virtual System.Linq.IQueryable<EJ.Databases> Databases
        {
             get
            {
                if (_Databases == null)
                {
                    _Databases = new Way.EntityDB.WayQueryable<EJ.Databases>(this.Set<EJ.Databases>());
                }
                return _Databases;
            }
        }

System.Linq.IQueryable<EJ.User> _User;
 /// <summary>
        /// 系统用户
        /// </summary>
        public virtual System.Linq.IQueryable<EJ.User> User
        {
             get
            {
                if (_User == null)
                {
                    _User = new Way.EntityDB.WayQueryable<EJ.User>(this.Set<EJ.User>());
                }
                return _User;
            }
        }

System.Linq.IQueryable<EJ.DBPower> _DBPower;
 /// <summary>
        /// 数据库权限
        /// </summary>
        public virtual System.Linq.IQueryable<EJ.DBPower> DBPower
        {
             get
            {
                if (_DBPower == null)
                {
                    _DBPower = new Way.EntityDB.WayQueryable<EJ.DBPower>(this.Set<EJ.DBPower>());
                }
                return _DBPower;
            }
        }

System.Linq.IQueryable<EJ.Bug> _Bug;
 /// <summary>
        /// 错误报告
        /// </summary>
        public virtual System.Linq.IQueryable<EJ.Bug> Bug
        {
             get
            {
                if (_Bug == null)
                {
                    _Bug = new Way.EntityDB.WayQueryable<EJ.Bug>(this.Set<EJ.Bug>());
                }
                return _Bug;
            }
        }

System.Linq.IQueryable<EJ.DBTable> _DBTable;
 /// <summary>
        /// 数据表
        /// </summary>
        public virtual System.Linq.IQueryable<EJ.DBTable> DBTable
        {
             get
            {
                if (_DBTable == null)
                {
                    _DBTable = new Way.EntityDB.WayQueryable<EJ.DBTable>(this.Set<EJ.DBTable>());
                }
                return _DBTable;
            }
        }

System.Linq.IQueryable<EJ.DBColumn> _DBColumn;
 /// <summary>
        /// 字段
        /// </summary>
        public virtual System.Linq.IQueryable<EJ.DBColumn> DBColumn
        {
             get
            {
                if (_DBColumn == null)
                {
                    _DBColumn = new Way.EntityDB.WayQueryable<EJ.DBColumn>(this.Set<EJ.DBColumn>());
                }
                return _DBColumn;
            }
        }

System.Linq.IQueryable<EJ.TablePower> _TablePower;
 /// <summary>
        /// 数据表权限
        /// </summary>
        public virtual System.Linq.IQueryable<EJ.TablePower> TablePower
        {
             get
            {
                if (_TablePower == null)
                {
                    _TablePower = new Way.EntityDB.WayQueryable<EJ.TablePower>(this.Set<EJ.TablePower>());
                }
                return _TablePower;
            }
        }

System.Linq.IQueryable<EJ.ProjectPower> _ProjectPower;
 /// <summary>
        /// 项目权限
        /// </summary>
        public virtual System.Linq.IQueryable<EJ.ProjectPower> ProjectPower
        {
             get
            {
                if (_ProjectPower == null)
                {
                    _ProjectPower = new Way.EntityDB.WayQueryable<EJ.ProjectPower>(this.Set<EJ.ProjectPower>());
                }
                return _ProjectPower;
            }
        }

System.Linq.IQueryable<EJ.DBModule> _DBModule;
 /// <summary>
        /// 数据模块
        /// </summary>
        public virtual System.Linq.IQueryable<EJ.DBModule> DBModule
        {
             get
            {
                if (_DBModule == null)
                {
                    _DBModule = new Way.EntityDB.WayQueryable<EJ.DBModule>(this.Set<EJ.DBModule>());
                }
                return _DBModule;
            }
        }

System.Linq.IQueryable<EJ.DBDeleteConfig> _DBDeleteConfig;
 /// <summary>
        /// 级联删除
        /// </summary>
        public virtual System.Linq.IQueryable<EJ.DBDeleteConfig> DBDeleteConfig
        {
             get
            {
                if (_DBDeleteConfig == null)
                {
                    _DBDeleteConfig = new Way.EntityDB.WayQueryable<EJ.DBDeleteConfig>(this.Set<EJ.DBDeleteConfig>());
                }
                return _DBDeleteConfig;
            }
        }

System.Linq.IQueryable<EJ.TableInModule> _TableInModule;
 /// <summary>
        /// TableInModule
        /// </summary>
        public virtual System.Linq.IQueryable<EJ.TableInModule> TableInModule
        {
             get
            {
                if (_TableInModule == null)
                {
                    _TableInModule = new Way.EntityDB.WayQueryable<EJ.TableInModule>(this.Set<EJ.TableInModule>());
                }
                return _TableInModule;
            }
        }

System.Linq.IQueryable<EJ.IDXIndex> _IDXIndex;
 /// <summary>
        /// 唯一值索引
        /// </summary>
        public virtual System.Linq.IQueryable<EJ.IDXIndex> IDXIndex
        {
             get
            {
                if (_IDXIndex == null)
                {
                    _IDXIndex = new Way.EntityDB.WayQueryable<EJ.IDXIndex>(this.Set<EJ.IDXIndex>());
                }
                return _IDXIndex;
            }
        }

System.Linq.IQueryable<EJ.BugHandleHistory> _BugHandleHistory;
 /// <summary>
        /// Bug处理历史记录
        /// </summary>
        public virtual System.Linq.IQueryable<EJ.BugHandleHistory> BugHandleHistory
        {
             get
            {
                if (_BugHandleHistory == null)
                {
                    _BugHandleHistory = new Way.EntityDB.WayQueryable<EJ.BugHandleHistory>(this.Set<EJ.BugHandleHistory>());
                }
                return _BugHandleHistory;
            }
        }

System.Linq.IQueryable<EJ.BugImages> _BugImages;
 /// <summary>
        /// Bug附带截图
        /// </summary>
        public virtual System.Linq.IQueryable<EJ.BugImages> BugImages
        {
             get
            {
                if (_BugImages == null)
                {
                    _BugImages = new Way.EntityDB.WayQueryable<EJ.BugImages>(this.Set<EJ.BugImages>());
                }
                return _BugImages;
            }
        }

System.Linq.IQueryable<EJ.DLLImport> _DLLImport;
 /// <summary>
        /// 引入的dll
        /// </summary>
        public virtual System.Linq.IQueryable<EJ.DLLImport> DLLImport
        {
             get
            {
                if (_DLLImport == null)
                {
                    _DLLImport = new Way.EntityDB.WayQueryable<EJ.DLLImport>(this.Set<EJ.DLLImport>());
                }
                return _DLLImport;
            }
        }

System.Linq.IQueryable<EJ.InterfaceModule> _InterfaceModule;
 /// <summary>
        /// 接口设计的目录结构
        /// </summary>
        public virtual System.Linq.IQueryable<EJ.InterfaceModule> InterfaceModule
        {
             get
            {
                if (_InterfaceModule == null)
                {
                    _InterfaceModule = new Way.EntityDB.WayQueryable<EJ.InterfaceModule>(this.Set<EJ.InterfaceModule>());
                }
                return _InterfaceModule;
            }
        }

System.Linq.IQueryable<EJ.InterfaceInModule> _InterfaceInModule;
 /// <summary>
        /// 
        /// </summary>
        public virtual System.Linq.IQueryable<EJ.InterfaceInModule> InterfaceInModule
        {
             get
            {
                if (_InterfaceInModule == null)
                {
                    _InterfaceInModule = new Way.EntityDB.WayQueryable<EJ.InterfaceInModule>(this.Set<EJ.InterfaceInModule>());
                }
                return _InterfaceInModule;
            }
        }

System.Linq.IQueryable<EJ.InterfaceModulePower> _InterfaceModulePower;
 /// <summary>
        /// InterfaceModule权限设定表
        /// </summary>
        public virtual System.Linq.IQueryable<EJ.InterfaceModulePower> InterfaceModulePower
        {
             get
            {
                if (_InterfaceModulePower == null)
                {
                    _InterfaceModulePower = new Way.EntityDB.WayQueryable<EJ.InterfaceModulePower>(this.Set<EJ.InterfaceModulePower>());
                }
                return _InterfaceModulePower;
            }
        }

static string _designData = "";}}
