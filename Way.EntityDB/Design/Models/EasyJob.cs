
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
            var db =  sender as EJ.DB.EasyJob;
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

static string _designData = "eyJUYWJsZXMiOlt7IlRhYmxlTmFtZSI6IlNxbGl0ZSIsIlJvd3MiOlt7Ikl0ZW1zIjpbeyJOYW1lIjoiaWQiLCJWYWx1ZSI6MTM5fSx7Ik5hbWUiOiJ0eXBlIiwiVmFsdWUiOiJFbnRpdHlEQi5EZXNpZ24uQWN0aW9ucy5DcmVhdGVUYWJsZUFjdGlvbiJ9LHsiTmFtZSI6ImNvbnRlbnQiLCJWYWx1ZSI6IntcIlRhYmxlXCI6e1wiaWRcIjozLFwiY2FwdGlvblwiOlwi6aG555uuXCIsXCJOYW1lXCI6XCJQcm9qZWN0XCIsXCJEYXRhYmFzZUlEXCI6MixcImlMb2NrXCI6MCxcIkNoYW5nZWRQcm9wZXJ0aWVzXCI6W10sXCJCYWNrdXBDaGFuZ2VkUHJvcGVydGllc1wiOltdfSxcIkNvbHVtbnNcIjpbe1wiaWRcIjoxMCxcImNhcHRpb25cIjpcImlkXCIsXCJOYW1lXCI6XCJpZFwiLFwiSXNBdXRvSW5jcmVtZW50XCI6dHJ1ZSxcIkNhbk51bGxcIjp0cnVlLFwiZGJUeXBlXCI6XCJpbnRcIixcIlR5cGVcIjpudWxsLFwiRW51bURlZmluZVwiOlwiXCIsXCJsZW5ndGhcIjpudWxsLFwiZGVmYXVsdFZhbHVlXCI6XCJcIixcIlRhYmxlSURcIjozLFwiSXNQS0lEXCI6dHJ1ZSxcIm9yZGVyaWRcIjowLFwiQ2hhbmdlZFByb3BlcnRpZXNcIjpbXSxcIkJhY2t1cENoYW5nZWRQcm9wZXJ0aWVzXCI6W119LHtcImlkXCI6MTEsXCJjYXB0aW9uXCI6XCJOYW1lXCIsXCJOYW1lXCI6XCJOYW1lXCIsXCJJc0F1dG9JbmNyZW1lbnRcIjpmYWxzZSxcIkNhbk51bGxcIjp0cnVlLFwiZGJUeXBlXCI6XCJ2YXJjaGFyXCIsXCJUeXBlXCI6bnVsbCxcIkVudW1EZWZpbmVcIjpcIlwiLFwibGVuZ3RoXCI6bnVsbCxcImRlZmF1bHRWYWx1ZVwiOlwiXCIsXCJUYWJsZUlEXCI6MyxcIklzUEtJRFwiOmZhbHNlLFwib3JkZXJpZFwiOjEsXCJDaGFuZ2VkUHJvcGVydGllc1wiOltdLFwiQmFja3VwQ2hhbmdlZFByb3BlcnRpZXNcIjpbXX1dLFwiSURYQ29uZmlnc1wiOltdLFwiSURcIjowfSJ9LHsiTmFtZSI6ImRhdGFiYXNlaWQiLCJWYWx1ZSI6Mn1dfSx7Ikl0ZW1zIjpbeyJOYW1lIjoiaWQiLCJWYWx1ZSI6MTQwfSx7Ik5hbWUiOiJ0eXBlIiwiVmFsdWUiOiJFbnRpdHlEQi5EZXNpZ24uQWN0aW9ucy5DcmVhdGVUYWJsZUFjdGlvbiJ9LHsiTmFtZSI6ImNvbnRlbnQiLCJWYWx1ZSI6IntcIlRhYmxlXCI6e1wiaWRcIjo0LFwiY2FwdGlvblwiOlwi5pWw5o2u5bqTXCIsXCJOYW1lXCI6XCJEYXRhYmFzZXNcIixcIkRhdGFiYXNlSURcIjoyLFwiaUxvY2tcIjowLFwiQ2hhbmdlZFByb3BlcnRpZXNcIjpbXSxcIkJhY2t1cENoYW5nZWRQcm9wZXJ0aWVzXCI6W119LFwiQ29sdW1uc1wiOlt7XCJpZFwiOjEyLFwiY2FwdGlvblwiOlwiaWRcIixcIk5hbWVcIjpcImlkXCIsXCJJc0F1dG9JbmNyZW1lbnRcIjp0cnVlLFwiQ2FuTnVsbFwiOnRydWUsXCJkYlR5cGVcIjpcImludFwiLFwiVHlwZVwiOm51bGwsXCJFbnVtRGVmaW5lXCI6XCJcIixcImxlbmd0aFwiOm51bGwsXCJkZWZhdWx0VmFsdWVcIjpcIlwiLFwiVGFibGVJRFwiOjQsXCJJc1BLSURcIjp0cnVlLFwib3JkZXJpZFwiOjAsXCJDaGFuZ2VkUHJvcGVydGllc1wiOltdLFwiQmFja3VwQ2hhbmdlZFByb3BlcnRpZXNcIjpbXX0se1wiaWRcIjoxMyxcImNhcHRpb25cIjpcIumhueebrklEXCIsXCJOYW1lXCI6XCJQcm9qZWN0SURcIixcIklzQXV0b0luY3JlbWVudFwiOmZhbHNlLFwiQ2FuTnVsbFwiOnRydWUsXCJkYlR5cGVcIjpcImludFwiLFwiVHlwZVwiOm51bGwsXCJFbnVtRGVmaW5lXCI6XCJcIixcImxlbmd0aFwiOm51bGwsXCJkZWZhdWx0VmFsdWVcIjpcIlwiLFwiVGFibGVJRFwiOjQsXCJJc1BLSURcIjpmYWxzZSxcIm9yZGVyaWRcIjoyLFwiQ2hhbmdlZFByb3BlcnRpZXNcIjpbXSxcIkJhY2t1cENoYW5nZWRQcm9wZXJ0aWVzXCI6W119LHtcImlkXCI6MTQsXCJjYXB0aW9uXCI6XCJOYW1lXCIsXCJOYW1lXCI6XCJOYW1lXCIsXCJJc0F1dG9JbmNyZW1lbnRcIjpmYWxzZSxcIkNhbk51bGxcIjp0cnVlLFwiZGJUeXBlXCI6XCJ2YXJjaGFyXCIsXCJUeXBlXCI6bnVsbCxcIkVudW1EZWZpbmVcIjpcIlwiLFwibGVuZ3RoXCI6bnVsbCxcImRlZmF1bHRWYWx1ZVwiOlwiXCIsXCJUYWJsZUlEXCI6NCxcIklzUEtJRFwiOmZhbHNlLFwib3JkZXJpZFwiOjMsXCJDaGFuZ2VkUHJvcGVydGllc1wiOltdLFwiQmFja3VwQ2hhbmdlZFByb3BlcnRpZXNcIjpbXX0se1wiaWRcIjoxNSxcImNhcHRpb25cIjpcIuaVsOaNruW6k+exu+Wei1wiLFwiTmFtZVwiOlwiZGJUeXBlXCIsXCJJc0F1dG9JbmNyZW1lbnRcIjpmYWxzZSxcIkNhbk51bGxcIjp0cnVlLFwiZGJUeXBlXCI6XCJpbnRcIixcIlR5cGVcIjpudWxsLFwiRW51bURlZmluZVwiOlwiU3FsU2VydmVyID0gMSxcXG5TcWxpdGUgPSAyLFxcbk15U3FsPTNcIixcImxlbmd0aFwiOm51bGwsXCJkZWZhdWx0VmFsdWVcIjpcIjFcIixcIlRhYmxlSURcIjo0LFwiSXNQS0lEXCI6ZmFsc2UsXCJvcmRlcmlkXCI6NCxcIkNoYW5nZWRQcm9wZXJ0aWVzXCI6W10sXCJCYWNrdXBDaGFuZ2VkUHJvcGVydGllc1wiOltdfSx7XCJpZFwiOjE2LFwiY2FwdGlvblwiOlwi6L+e5o6l5a2X56ym5LiyXCIsXCJOYW1lXCI6XCJjb25TdHJcIixcIklzQXV0b0luY3JlbWVudFwiOmZhbHNlLFwiQ2FuTnVsbFwiOnRydWUsXCJkYlR5cGVcIjpcInZhcmNoYXJcIixcIlR5cGVcIjpudWxsLFwiRW51bURlZmluZVwiOlwiXCIsXCJsZW5ndGhcIjpcIjIwMFwiLFwiZGVmYXVsdFZhbHVlXCI6XCJcIixcIlRhYmxlSURcIjo0LFwiSXNQS0lEXCI6ZmFsc2UsXCJvcmRlcmlkXCI6NSxcIkNoYW5nZWRQcm9wZXJ0aWVzXCI6W10sXCJCYWNrdXBDaGFuZ2VkUHJvcGVydGllc1wiOltdfSx7XCJpZFwiOjE3LFwiY2FwdGlvblwiOlwiZGxs55Sf5oiQ5paH5Lu25aS5XCIsXCJOYW1lXCI6XCJkbGxQYXRoXCIsXCJJc0F1dG9JbmNyZW1lbnRcIjpmYWxzZSxcIkNhbk51bGxcIjp0cnVlLFwiZGJUeXBlXCI6XCJ2YXJjaGFyXCIsXCJUeXBlXCI6bnVsbCxcIkVudW1EZWZpbmVcIjpcIlwiLFwibGVuZ3RoXCI6XCIxMDBcIixcImRlZmF1bHRWYWx1ZVwiOlwiXCIsXCJUYWJsZUlEXCI6NCxcIklzUEtJRFwiOmZhbHNlLFwib3JkZXJpZFwiOjYsXCJDaGFuZ2VkUHJvcGVydGllc1wiOltdLFwiQmFja3VwQ2hhbmdlZFByb3BlcnRpZXNcIjpbXX0se1wiaWRcIjoxOCxcImNhcHRpb25cIjpcImlMb2NrXCIsXCJOYW1lXCI6XCJpTG9ja1wiLFwiSXNBdXRvSW5jcmVtZW50XCI6ZmFsc2UsXCJDYW5OdWxsXCI6dHJ1ZSxcImRiVHlwZVwiOlwiaW50XCIsXCJUeXBlXCI6bnVsbCxcIkVudW1EZWZpbmVcIjpcIlwiLFwibGVuZ3RoXCI6bnVsbCxcImRlZmF1bHRWYWx1ZVwiOlwiMFwiLFwiVGFibGVJRFwiOjQsXCJJc1BLSURcIjpmYWxzZSxcIm9yZGVyaWRcIjo3LFwiQ2hhbmdlZFByb3BlcnRpZXNcIjpbXSxcIkJhY2t1cENoYW5nZWRQcm9wZXJ0aWVzXCI6W119LHtcImlkXCI6MTksXCJjYXB0aW9uXCI6XCJOYW1lU3BhY2VcIixcIk5hbWVcIjpcIk5hbWVTcGFjZVwiLFwiSXNBdXRvSW5jcmVtZW50XCI6ZmFsc2UsXCJDYW5OdWxsXCI6dHJ1ZSxcImRiVHlwZVwiOlwidmFyY2hhclwiLFwiVHlwZVwiOm51bGwsXCJFbnVtRGVmaW5lXCI6XCJcIixcImxlbmd0aFwiOm51bGwsXCJkZWZhdWx0VmFsdWVcIjpcIlwiLFwiVGFibGVJRFwiOjQsXCJJc1BLSURcIjpmYWxzZSxcIm9yZGVyaWRcIjo4LFwiQ2hhbmdlZFByb3BlcnRpZXNcIjpbXSxcIkJhY2t1cENoYW5nZWRQcm9wZXJ0aWVzXCI6W119LHtcImlkXCI6MTk5LFwiY2FwdGlvblwiOlwi5ZSv5LiA5qCH56S6SURcIixcIk5hbWVcIjpcIkd1aWRcIixcIklzQXV0b0luY3JlbWVudFwiOmZhbHNlLFwiQ2FuTnVsbFwiOnRydWUsXCJkYlR5cGVcIjpcInZhcmNoYXJcIixcIlR5cGVcIjpudWxsLFwiRW51bURlZmluZVwiOm51bGwsXCJsZW5ndGhcIjpcIjUwXCIsXCJkZWZhdWx0VmFsdWVcIjpudWxsLFwiVGFibGVJRFwiOjQsXCJJc1BLSURcIjpmYWxzZSxcIm9yZGVyaWRcIjoxLFwiQ2hhbmdlZFByb3BlcnRpZXNcIjpbXSxcIkJhY2t1cENoYW5nZWRQcm9wZXJ0aWVzXCI6W119XSxcIklEWENvbmZpZ3NcIjpbXSxcIklEXCI6MH0ifSx7Ik5hbWUiOiJkYXRhYmFzZWlkIiwiVmFsdWUiOjJ9XX0seyJJdGVtcyI6W3siTmFtZSI6ImlkIiwiVmFsdWUiOjE0MX0seyJOYW1lIjoidHlwZSIsIlZhbHVlIjoiRW50aXR5REIuRGVzaWduLkFjdGlvbnMuQ3JlYXRlVGFibGVBY3Rpb24ifSx7Ik5hbWUiOiJjb250ZW50IiwiVmFsdWUiOiJ7XCJUYWJsZVwiOntcImlkXCI6NSxcImNhcHRpb25cIjpcIuezu+e7n+eUqOaIt1wiLFwiTmFtZVwiOlwiVXNlclwiLFwiRGF0YWJhc2VJRFwiOjIsXCJpTG9ja1wiOjAsXCJDaGFuZ2VkUHJvcGVydGllc1wiOltdLFwiQmFja3VwQ2hhbmdlZFByb3BlcnRpZXNcIjpbXX0sXCJDb2x1bW5zXCI6W3tcImlkXCI6MjAsXCJjYXB0aW9uXCI6XCJpZFwiLFwiTmFtZVwiOlwiaWRcIixcIklzQXV0b0luY3JlbWVudFwiOnRydWUsXCJDYW5OdWxsXCI6dHJ1ZSxcImRiVHlwZVwiOlwiaW50XCIsXCJUeXBlXCI6bnVsbCxcIkVudW1EZWZpbmVcIjpcIlwiLFwibGVuZ3RoXCI6bnVsbCxcImRlZmF1bHRWYWx1ZVwiOlwiXCIsXCJUYWJsZUlEXCI6NSxcIklzUEtJRFwiOnRydWUsXCJvcmRlcmlkXCI6MCxcIkNoYW5nZWRQcm9wZXJ0aWVzXCI6W10sXCJCYWNrdXBDaGFuZ2VkUHJvcGVydGllc1wiOltdfSx7XCJpZFwiOjIxLFwiY2FwdGlvblwiOlwi6KeS6ImyXCIsXCJOYW1lXCI6XCJSb2xlXCIsXCJJc0F1dG9JbmNyZW1lbnRcIjpmYWxzZSxcIkNhbk51bGxcIjp0cnVlLFwiZGJUeXBlXCI6XCJpbnRcIixcIlR5cGVcIjpudWxsLFwiRW51bURlZmluZVwiOlwi5byA5Y+R5Lq65ZGYID0gMSxcXG7lrqLmiLfnq6/mtYvor5XkurrlkZggPSAxXFx1MDAzY1xcdTAwM2MxLFxcbuaVsOaNruW6k+iuvuiuoeW4iCA9IDFcXHUwMDNjXFx1MDAzYzIgfCDlvIDlj5HkurrlkZgsXFxu566h55CG5ZGYID0g5pWw5o2u5bqT6K6+6K6h5biIIHwgMVxcdTAwM2NcXHUwMDNjMyxcXG7pobnnm67nu4/nkIYgPSAxXFx1MDAzY1xcdTAwM2M0IHwg5byA5Y+R5Lq65ZGYLFwiLFwibGVuZ3RoXCI6bnVsbCxcImRlZmF1bHRWYWx1ZVwiOlwiXCIsXCJUYWJsZUlEXCI6NSxcIklzUEtJRFwiOmZhbHNlLFwib3JkZXJpZFwiOjEsXCJDaGFuZ2VkUHJvcGVydGllc1wiOltdLFwiQmFja3VwQ2hhbmdlZFByb3BlcnRpZXNcIjpbXX0se1wiaWRcIjoyMixcImNhcHRpb25cIjpcIk5hbWVcIixcIk5hbWVcIjpcIk5hbWVcIixcIklzQXV0b0luY3JlbWVudFwiOmZhbHNlLFwiQ2FuTnVsbFwiOnRydWUsXCJkYlR5cGVcIjpcInZhcmNoYXJcIixcIlR5cGVcIjpudWxsLFwiRW51bURlZmluZVwiOlwiXCIsXCJsZW5ndGhcIjpudWxsLFwiZGVmYXVsdFZhbHVlXCI6XCJcIixcIlRhYmxlSURcIjo1LFwiSXNQS0lEXCI6ZmFsc2UsXCJvcmRlcmlkXCI6MixcIkNoYW5nZWRQcm9wZXJ0aWVzXCI6W10sXCJCYWNrdXBDaGFuZ2VkUHJvcGVydGllc1wiOltdfSx7XCJpZFwiOjIzLFwiY2FwdGlvblwiOlwiUGFzc3dvcmRcIixcIk5hbWVcIjpcIlBhc3N3b3JkXCIsXCJJc0F1dG9JbmNyZW1lbnRcIjpmYWxzZSxcIkNhbk51bGxcIjp0cnVlLFwiZGJUeXBlXCI6XCJ2YXJjaGFyXCIsXCJUeXBlXCI6bnVsbCxcIkVudW1EZWZpbmVcIjpcIlwiLFwibGVuZ3RoXCI6bnVsbCxcImRlZmF1bHRWYWx1ZVwiOlwiXCIsXCJUYWJsZUlEXCI6NSxcIklzUEtJRFwiOmZhbHNlLFwib3JkZXJpZFwiOjMsXCJDaGFuZ2VkUHJvcGVydGllc1wiOltdLFwiQmFja3VwQ2hhbmdlZFByb3BlcnRpZXNcIjpbXX1dLFwiSURYQ29uZmlnc1wiOltdLFwiSURcIjowfSJ9LHsiTmFtZSI6ImRhdGFiYXNlaWQiLCJWYWx1ZSI6Mn1dfSx7Ikl0ZW1zIjpbeyJOYW1lIjoiaWQiLCJWYWx1ZSI6MTQyfSx7Ik5hbWUiOiJ0eXBlIiwiVmFsdWUiOiJFbnRpdHlEQi5EZXNpZ24uQWN0aW9ucy5DcmVhdGVUYWJsZUFjdGlvbiJ9LHsiTmFtZSI6ImNvbnRlbnQiLCJWYWx1ZSI6IntcIlRhYmxlXCI6e1wiaWRcIjo2LFwiY2FwdGlvblwiOlwi5pWw5o2u5bqT5p2D6ZmQXCIsXCJOYW1lXCI6XCJEQlBvd2VyXCIsXCJEYXRhYmFzZUlEXCI6MixcImlMb2NrXCI6MCxcIkNoYW5nZWRQcm9wZXJ0aWVzXCI6W10sXCJCYWNrdXBDaGFuZ2VkUHJvcGVydGllc1wiOltdfSxcIkNvbHVtbnNcIjpbe1wiaWRcIjoyNCxcImNhcHRpb25cIjpcImlkXCIsXCJOYW1lXCI6XCJpZFwiLFwiSXNBdXRvSW5jcmVtZW50XCI6dHJ1ZSxcIkNhbk51bGxcIjp0cnVlLFwiZGJUeXBlXCI6XCJpbnRcIixcIlR5cGVcIjpudWxsLFwiRW51bURlZmluZVwiOlwiXCIsXCJsZW5ndGhcIjpudWxsLFwiZGVmYXVsdFZhbHVlXCI6XCJcIixcIlRhYmxlSURcIjo2LFwiSXNQS0lEXCI6dHJ1ZSxcIm9yZGVyaWRcIjowLFwiQ2hhbmdlZFByb3BlcnRpZXNcIjpbXSxcIkJhY2t1cENoYW5nZWRQcm9wZXJ0aWVzXCI6W119LHtcImlkXCI6MjUsXCJjYXB0aW9uXCI6XCLnlKjmiLdcIixcIk5hbWVcIjpcIlVzZXJJRFwiLFwiSXNBdXRvSW5jcmVtZW50XCI6ZmFsc2UsXCJDYW5OdWxsXCI6dHJ1ZSxcImRiVHlwZVwiOlwiaW50XCIsXCJUeXBlXCI6bnVsbCxcIkVudW1EZWZpbmVcIjpcIlwiLFwibGVuZ3RoXCI6bnVsbCxcImRlZmF1bHRWYWx1ZVwiOlwiXCIsXCJUYWJsZUlEXCI6NixcIklzUEtJRFwiOmZhbHNlLFwib3JkZXJpZFwiOjEsXCJDaGFuZ2VkUHJvcGVydGllc1wiOltdLFwiQmFja3VwQ2hhbmdlZFByb3BlcnRpZXNcIjpbXX0se1wiaWRcIjoyNixcImNhcHRpb25cIjpcIuadg+mZkFwiLFwiTmFtZVwiOlwiUG93ZXJcIixcIklzQXV0b0luY3JlbWVudFwiOmZhbHNlLFwiQ2FuTnVsbFwiOnRydWUsXCJkYlR5cGVcIjpcImludFwiLFwiVHlwZVwiOm51bGwsXCJFbnVtRGVmaW5lXCI6XCLlj6ror7sgPSAwLFxcbuS/ruaUuSA9IDFcIixcImxlbmd0aFwiOm51bGwsXCJkZWZhdWx0VmFsdWVcIjpcIlwiLFwiVGFibGVJRFwiOjYsXCJJc1BLSURcIjpmYWxzZSxcIm9yZGVyaWRcIjoyLFwiQ2hhbmdlZFByb3BlcnRpZXNcIjpbXSxcIkJhY2t1cENoYW5nZWRQcm9wZXJ0aWVzXCI6W119LHtcImlkXCI6MjcsXCJjYXB0aW9uXCI6XCLmlbDmja7lupNJRFwiLFwiTmFtZVwiOlwiRGF0YWJhc2VJRFwiLFwiSXNBdXRvSW5jcmVtZW50XCI6ZmFsc2UsXCJDYW5OdWxsXCI6dHJ1ZSxcImRiVHlwZVwiOlwiaW50XCIsXCJUeXBlXCI6bnVsbCxcIkVudW1EZWZpbmVcIjpcIlwiLFwibGVuZ3RoXCI6bnVsbCxcImRlZmF1bHRWYWx1ZVwiOlwiXCIsXCJUYWJsZUlEXCI6NixcIklzUEtJRFwiOmZhbHNlLFwib3JkZXJpZFwiOjMsXCJDaGFuZ2VkUHJvcGVydGllc1wiOltdLFwiQmFja3VwQ2hhbmdlZFByb3BlcnRpZXNcIjpbXX1dLFwiSURYQ29uZmlnc1wiOltdLFwiSURcIjowfSJ9LHsiTmFtZSI6ImRhdGFiYXNlaWQiLCJWYWx1ZSI6Mn1dfSx7Ikl0ZW1zIjpbeyJOYW1lIjoiaWQiLCJWYWx1ZSI6MTQzfSx7Ik5hbWUiOiJ0eXBlIiwiVmFsdWUiOiJFbnRpdHlEQi5EZXNpZ24uQWN0aW9ucy5DcmVhdGVUYWJsZUFjdGlvbiJ9LHsiTmFtZSI6ImNvbnRlbnQiLCJWYWx1ZSI6IntcIlRhYmxlXCI6e1wiaWRcIjo3LFwiY2FwdGlvblwiOlwi6ZSZ6K+v5oql5ZGKXCIsXCJOYW1lXCI6XCJCdWdcIixcIkRhdGFiYXNlSURcIjoyLFwiaUxvY2tcIjowLFwiQ2hhbmdlZFByb3BlcnRpZXNcIjpbXSxcIkJhY2t1cENoYW5nZWRQcm9wZXJ0aWVzXCI6W119LFwiQ29sdW1uc1wiOlt7XCJpZFwiOjI4LFwiY2FwdGlvblwiOlwiaWRcIixcIk5hbWVcIjpcImlkXCIsXCJJc0F1dG9JbmNyZW1lbnRcIjp0cnVlLFwiQ2FuTnVsbFwiOnRydWUsXCJkYlR5cGVcIjpcImludFwiLFwiVHlwZVwiOm51bGwsXCJFbnVtRGVmaW5lXCI6XCJcIixcImxlbmd0aFwiOm51bGwsXCJkZWZhdWx0VmFsdWVcIjpcIlwiLFwiVGFibGVJRFwiOjcsXCJJc1BLSURcIjp0cnVlLFwib3JkZXJpZFwiOjAsXCJDaGFuZ2VkUHJvcGVydGllc1wiOltdLFwiQmFja3VwQ2hhbmdlZFByb3BlcnRpZXNcIjpbXX0se1wiaWRcIjoyOSxcImNhcHRpb25cIjpcIuagh+mimFwiLFwiTmFtZVwiOlwiVGl0bGVcIixcIklzQXV0b0luY3JlbWVudFwiOmZhbHNlLFwiQ2FuTnVsbFwiOnRydWUsXCJkYlR5cGVcIjpcInZhcmNoYXJcIixcIlR5cGVcIjpudWxsLFwiRW51bURlZmluZVwiOlwiXCIsXCJsZW5ndGhcIjpudWxsLFwiZGVmYXVsdFZhbHVlXCI6XCJcIixcIlRhYmxlSURcIjo3LFwiSXNQS0lEXCI6ZmFsc2UsXCJvcmRlcmlkXCI6MSxcIkNoYW5nZWRQcm9wZXJ0aWVzXCI6W10sXCJCYWNrdXBDaGFuZ2VkUHJvcGVydGllc1wiOltdfSx7XCJpZFwiOjMwLFwiY2FwdGlvblwiOlwi5o+Q5Lqk6ICFSURcIixcIk5hbWVcIjpcIlN1Ym1pdFVzZXJJRFwiLFwiSXNBdXRvSW5jcmVtZW50XCI6ZmFsc2UsXCJDYW5OdWxsXCI6dHJ1ZSxcImRiVHlwZVwiOlwiaW50XCIsXCJUeXBlXCI6bnVsbCxcIkVudW1EZWZpbmVcIjpcIlwiLFwibGVuZ3RoXCI6bnVsbCxcImRlZmF1bHRWYWx1ZVwiOlwiXCIsXCJUYWJsZUlEXCI6NyxcIklzUEtJRFwiOmZhbHNlLFwib3JkZXJpZFwiOjIsXCJDaGFuZ2VkUHJvcGVydGllc1wiOltdLFwiQmFja3VwQ2hhbmdlZFByb3BlcnRpZXNcIjpbXX0se1wiaWRcIjozMSxcImNhcHRpb25cIjpcIuaPkOS6pOaXtumXtFwiLFwiTmFtZVwiOlwiU3VibWl0VGltZVwiLFwiSXNBdXRvSW5jcmVtZW50XCI6ZmFsc2UsXCJDYW5OdWxsXCI6dHJ1ZSxcImRiVHlwZVwiOlwiZGF0ZXRpbWVcIixcIlR5cGVcIjpudWxsLFwiRW51bURlZmluZVwiOlwiXCIsXCJsZW5ndGhcIjpudWxsLFwiZGVmYXVsdFZhbHVlXCI6XCJcIixcIlRhYmxlSURcIjo3LFwiSXNQS0lEXCI6ZmFsc2UsXCJvcmRlcmlkXCI6MyxcIkNoYW5nZWRQcm9wZXJ0aWVzXCI6W10sXCJCYWNrdXBDaGFuZ2VkUHJvcGVydGllc1wiOltdfSx7XCJpZFwiOjMyLFwiY2FwdGlvblwiOlwi5aSE55CG6ICFSURcIixcIk5hbWVcIjpcIkhhbmRsZXJJRFwiLFwiSXNBdXRvSW5jcmVtZW50XCI6ZmFsc2UsXCJDYW5OdWxsXCI6dHJ1ZSxcImRiVHlwZVwiOlwiaW50XCIsXCJUeXBlXCI6bnVsbCxcIkVudW1EZWZpbmVcIjpcIlwiLFwibGVuZ3RoXCI6bnVsbCxcImRlZmF1bHRWYWx1ZVwiOlwiXCIsXCJUYWJsZUlEXCI6NyxcIklzUEtJRFwiOmZhbHNlLFwib3JkZXJpZFwiOjQsXCJDaGFuZ2VkUHJvcGVydGllc1wiOltdLFwiQmFja3VwQ2hhbmdlZFByb3BlcnRpZXNcIjpbXX0se1wiaWRcIjozMyxcImNhcHRpb25cIjpcIuacgOWQjuWPjemmiOaXtumXtFwiLFwiTmFtZVwiOlwiTGFzdERhdGVcIixcIklzQXV0b0luY3JlbWVudFwiOmZhbHNlLFwiQ2FuTnVsbFwiOnRydWUsXCJkYlR5cGVcIjpcImRhdGV0aW1lXCIsXCJUeXBlXCI6bnVsbCxcIkVudW1EZWZpbmVcIjpcIlwiLFwibGVuZ3RoXCI6bnVsbCxcImRlZmF1bHRWYWx1ZVwiOlwiXCIsXCJUYWJsZUlEXCI6NyxcIklzUEtJRFwiOmZhbHNlLFwib3JkZXJpZFwiOjUsXCJDaGFuZ2VkUHJvcGVydGllc1wiOltdLFwiQmFja3VwQ2hhbmdlZFByb3BlcnRpZXNcIjpbXX0se1wiaWRcIjoxMTgsXCJjYXB0aW9uXCI6XCLlpITnkIblrozmr5Xml7bpl7RcIixcIk5hbWVcIjpcIkZpbmlzaFRpbWVcIixcIklzQXV0b0luY3JlbWVudFwiOmZhbHNlLFwiQ2FuTnVsbFwiOnRydWUsXCJkYlR5cGVcIjpcImRhdGV0aW1lXCIsXCJUeXBlXCI6bnVsbCxcIkVudW1EZWZpbmVcIjpudWxsLFwibGVuZ3RoXCI6XCJcIixcImRlZmF1bHRWYWx1ZVwiOm51bGwsXCJUYWJsZUlEXCI6NyxcIklzUEtJRFwiOmZhbHNlLFwib3JkZXJpZFwiOjYsXCJDaGFuZ2VkUHJvcGVydGllc1wiOltdLFwiQmFja3VwQ2hhbmdlZFByb3BlcnRpZXNcIjpbXX0se1wiaWRcIjoxNDMsXCJjYXB0aW9uXCI6XCLlvZPliY3nirbmgIFcIixcIk5hbWVcIjpcIlN0YXR1c1wiLFwiSXNBdXRvSW5jcmVtZW50XCI6ZmFsc2UsXCJDYW5OdWxsXCI6dHJ1ZSxcImRiVHlwZVwiOlwiaW50XCIsXCJUeXBlXCI6bnVsbCxcIkVudW1EZWZpbmVcIjpcIuaPkOS6pOe7meW8gOWPkeS6uuWRmCA9IDAsXFxu5Y+N6aaI57uZ5o+Q5Lqk6ICFID0gMSxcXG7lpITnkIblrozmr5UgPSAyXCIsXCJsZW5ndGhcIjpcIlwiLFwiZGVmYXVsdFZhbHVlXCI6bnVsbCxcIlRhYmxlSURcIjo3LFwiSXNQS0lEXCI6ZmFsc2UsXCJvcmRlcmlkXCI6NyxcIkNoYW5nZWRQcm9wZXJ0aWVzXCI6W10sXCJCYWNrdXBDaGFuZ2VkUHJvcGVydGllc1wiOltdfV0sXCJJRFhDb25maWdzXCI6W10sXCJJRFwiOjB9In0seyJOYW1lIjoiZGF0YWJhc2VpZCIsIlZhbHVlIjoyfV19LHsiSXRlbXMiOlt7Ik5hbWUiOiJpZCIsIlZhbHVlIjoxNDR9LHsiTmFtZSI6InR5cGUiLCJWYWx1ZSI6IkVudGl0eURCLkRlc2lnbi5BY3Rpb25zLkNyZWF0ZVRhYmxlQWN0aW9uIn0seyJOYW1lIjoiY29udGVudCIsIlZhbHVlIjoie1wiVGFibGVcIjp7XCJpZFwiOjgsXCJjYXB0aW9uXCI6XCLmlbDmja7ooahcIixcIk5hbWVcIjpcIkRCVGFibGVcIixcIkRhdGFiYXNlSURcIjoyLFwiaUxvY2tcIjowLFwiQ2hhbmdlZFByb3BlcnRpZXNcIjpbXSxcIkJhY2t1cENoYW5nZWRQcm9wZXJ0aWVzXCI6W119LFwiQ29sdW1uc1wiOlt7XCJpZFwiOjM0LFwiY2FwdGlvblwiOlwiaWRcIixcIk5hbWVcIjpcImlkXCIsXCJJc0F1dG9JbmNyZW1lbnRcIjp0cnVlLFwiQ2FuTnVsbFwiOnRydWUsXCJkYlR5cGVcIjpcImludFwiLFwiVHlwZVwiOm51bGwsXCJFbnVtRGVmaW5lXCI6XCJcIixcImxlbmd0aFwiOm51bGwsXCJkZWZhdWx0VmFsdWVcIjpcIlwiLFwiVGFibGVJRFwiOjgsXCJJc1BLSURcIjp0cnVlLFwib3JkZXJpZFwiOjAsXCJDaGFuZ2VkUHJvcGVydGllc1wiOltdLFwiQmFja3VwQ2hhbmdlZFByb3BlcnRpZXNcIjpbXX0se1wiaWRcIjozNSxcImNhcHRpb25cIjpcImNhcHRpb25cIixcIk5hbWVcIjpcImNhcHRpb25cIixcIklzQXV0b0luY3JlbWVudFwiOmZhbHNlLFwiQ2FuTnVsbFwiOnRydWUsXCJkYlR5cGVcIjpcInZhcmNoYXJcIixcIlR5cGVcIjpudWxsLFwiRW51bURlZmluZVwiOlwiXCIsXCJsZW5ndGhcIjpudWxsLFwiZGVmYXVsdFZhbHVlXCI6XCJcIixcIlRhYmxlSURcIjo4LFwiSXNQS0lEXCI6ZmFsc2UsXCJvcmRlcmlkXCI6MSxcIkNoYW5nZWRQcm9wZXJ0aWVzXCI6W10sXCJCYWNrdXBDaGFuZ2VkUHJvcGVydGllc1wiOltdfSx7XCJpZFwiOjM2LFwiY2FwdGlvblwiOlwiTmFtZVwiLFwiTmFtZVwiOlwiTmFtZVwiLFwiSXNBdXRvSW5jcmVtZW50XCI6ZmFsc2UsXCJDYW5OdWxsXCI6dHJ1ZSxcImRiVHlwZVwiOlwidmFyY2hhclwiLFwiVHlwZVwiOm51bGwsXCJFbnVtRGVmaW5lXCI6XCJcIixcImxlbmd0aFwiOm51bGwsXCJkZWZhdWx0VmFsdWVcIjpcIlwiLFwiVGFibGVJRFwiOjgsXCJJc1BLSURcIjpmYWxzZSxcIm9yZGVyaWRcIjoyLFwiQ2hhbmdlZFByb3BlcnRpZXNcIjpbXSxcIkJhY2t1cENoYW5nZWRQcm9wZXJ0aWVzXCI6W119LHtcImlkXCI6MzcsXCJjYXB0aW9uXCI6XCJEYXRhYmFzZUlEXCIsXCJOYW1lXCI6XCJEYXRhYmFzZUlEXCIsXCJJc0F1dG9JbmNyZW1lbnRcIjpmYWxzZSxcIkNhbk51bGxcIjp0cnVlLFwiZGJUeXBlXCI6XCJpbnRcIixcIlR5cGVcIjpudWxsLFwiRW51bURlZmluZVwiOlwiXCIsXCJsZW5ndGhcIjpudWxsLFwiZGVmYXVsdFZhbHVlXCI6XCJcIixcIlRhYmxlSURcIjo4LFwiSXNQS0lEXCI6ZmFsc2UsXCJvcmRlcmlkXCI6MyxcIkNoYW5nZWRQcm9wZXJ0aWVzXCI6W10sXCJCYWNrdXBDaGFuZ2VkUHJvcGVydGllc1wiOltdfSx7XCJpZFwiOjM4LFwiY2FwdGlvblwiOlwiaUxvY2tcIixcIk5hbWVcIjpcImlMb2NrXCIsXCJJc0F1dG9JbmNyZW1lbnRcIjpmYWxzZSxcIkNhbk51bGxcIjp0cnVlLFwiZGJUeXBlXCI6XCJpbnRcIixcIlR5cGVcIjpudWxsLFwiRW51bURlZmluZVwiOlwiXCIsXCJsZW5ndGhcIjpudWxsLFwiZGVmYXVsdFZhbHVlXCI6XCIwXCIsXCJUYWJsZUlEXCI6OCxcIklzUEtJRFwiOmZhbHNlLFwib3JkZXJpZFwiOjQsXCJDaGFuZ2VkUHJvcGVydGllc1wiOltdLFwiQmFja3VwQ2hhbmdlZFByb3BlcnRpZXNcIjpbXX1dLFwiSURYQ29uZmlnc1wiOltdLFwiSURcIjowfSJ9LHsiTmFtZSI6ImRhdGFiYXNlaWQiLCJWYWx1ZSI6Mn1dfSx7Ikl0ZW1zIjpbeyJOYW1lIjoiaWQiLCJWYWx1ZSI6MTQ1fSx7Ik5hbWUiOiJ0eXBlIiwiVmFsdWUiOiJFbnRpdHlEQi5EZXNpZ24uQWN0aW9ucy5DcmVhdGVUYWJsZUFjdGlvbiJ9LHsiTmFtZSI6ImNvbnRlbnQiLCJWYWx1ZSI6IntcIlRhYmxlXCI6e1wiaWRcIjo5LFwiY2FwdGlvblwiOlwi5a2X5q61XCIsXCJOYW1lXCI6XCJEQkNvbHVtblwiLFwiRGF0YWJhc2VJRFwiOjIsXCJpTG9ja1wiOjAsXCJDaGFuZ2VkUHJvcGVydGllc1wiOltdLFwiQmFja3VwQ2hhbmdlZFByb3BlcnRpZXNcIjpbXX0sXCJDb2x1bW5zXCI6W3tcImlkXCI6MzksXCJjYXB0aW9uXCI6XCJpZFwiLFwiTmFtZVwiOlwiaWRcIixcIklzQXV0b0luY3JlbWVudFwiOnRydWUsXCJDYW5OdWxsXCI6dHJ1ZSxcImRiVHlwZVwiOlwiaW50XCIsXCJUeXBlXCI6bnVsbCxcIkVudW1EZWZpbmVcIjpcIlwiLFwibGVuZ3RoXCI6bnVsbCxcImRlZmF1bHRWYWx1ZVwiOlwiXCIsXCJUYWJsZUlEXCI6OSxcIklzUEtJRFwiOnRydWUsXCJvcmRlcmlkXCI6MCxcIkNoYW5nZWRQcm9wZXJ0aWVzXCI6W10sXCJCYWNrdXBDaGFuZ2VkUHJvcGVydGllc1wiOltdfSx7XCJpZFwiOjQwLFwiY2FwdGlvblwiOlwiY2FwdGlvblwiLFwiTmFtZVwiOlwiY2FwdGlvblwiLFwiSXNBdXRvSW5jcmVtZW50XCI6ZmFsc2UsXCJDYW5OdWxsXCI6dHJ1ZSxcImRiVHlwZVwiOlwidmFyY2hhclwiLFwiVHlwZVwiOm51bGwsXCJFbnVtRGVmaW5lXCI6XCJcIixcImxlbmd0aFwiOlwiMjAwXCIsXCJkZWZhdWx0VmFsdWVcIjpcIlwiLFwiVGFibGVJRFwiOjksXCJJc1BLSURcIjpmYWxzZSxcIm9yZGVyaWRcIjoxLFwiQ2hhbmdlZFByb3BlcnRpZXNcIjpbXSxcIkJhY2t1cENoYW5nZWRQcm9wZXJ0aWVzXCI6W119LHtcImlkXCI6NDEsXCJjYXB0aW9uXCI6XCJOYW1lXCIsXCJOYW1lXCI6XCJOYW1lXCIsXCJJc0F1dG9JbmNyZW1lbnRcIjpmYWxzZSxcIkNhbk51bGxcIjp0cnVlLFwiZGJUeXBlXCI6XCJ2YXJjaGFyXCIsXCJUeXBlXCI6bnVsbCxcIkVudW1EZWZpbmVcIjpcIlwiLFwibGVuZ3RoXCI6bnVsbCxcImRlZmF1bHRWYWx1ZVwiOlwiXCIsXCJUYWJsZUlEXCI6OSxcIklzUEtJRFwiOmZhbHNlLFwib3JkZXJpZFwiOjIsXCJDaGFuZ2VkUHJvcGVydGllc1wiOltdLFwiQmFja3VwQ2hhbmdlZFByb3BlcnRpZXNcIjpbXX0se1wiaWRcIjo0MixcImNhcHRpb25cIjpcIuiHquWinumVv1wiLFwiTmFtZVwiOlwiSXNBdXRvSW5jcmVtZW50XCIsXCJJc0F1dG9JbmNyZW1lbnRcIjpmYWxzZSxcIkNhbk51bGxcIjp0cnVlLFwiZGJUeXBlXCI6XCJiaXRcIixcIlR5cGVcIjpudWxsLFwiRW51bURlZmluZVwiOlwiXCIsXCJsZW5ndGhcIjpudWxsLFwiZGVmYXVsdFZhbHVlXCI6XCIwXCIsXCJUYWJsZUlEXCI6OSxcIklzUEtJRFwiOmZhbHNlLFwib3JkZXJpZFwiOjMsXCJDaGFuZ2VkUHJvcGVydGllc1wiOltdLFwiQmFja3VwQ2hhbmdlZFByb3BlcnRpZXNcIjpbXX0se1wiaWRcIjo0MyxcImNhcHRpb25cIjpcIuWPr+S7peS4uuepulwiLFwiTmFtZVwiOlwiQ2FuTnVsbFwiLFwiSXNBdXRvSW5jcmVtZW50XCI6ZmFsc2UsXCJDYW5OdWxsXCI6dHJ1ZSxcImRiVHlwZVwiOlwiYml0XCIsXCJUeXBlXCI6bnVsbCxcIkVudW1EZWZpbmVcIjpcIlwiLFwibGVuZ3RoXCI6bnVsbCxcImRlZmF1bHRWYWx1ZVwiOlwiMVwiLFwiVGFibGVJRFwiOjksXCJJc1BLSURcIjpmYWxzZSxcIm9yZGVyaWRcIjo0LFwiQ2hhbmdlZFByb3BlcnRpZXNcIjpbXSxcIkJhY2t1cENoYW5nZWRQcm9wZXJ0aWVzXCI6W119LHtcImlkXCI6NDQsXCJjYXB0aW9uXCI6XCLmlbDmja7lupPlrZfmrrXnsbvlnotcIixcIk5hbWVcIjpcImRiVHlwZVwiLFwiSXNBdXRvSW5jcmVtZW50XCI6ZmFsc2UsXCJDYW5OdWxsXCI6dHJ1ZSxcImRiVHlwZVwiOlwidmFyY2hhclwiLFwiVHlwZVwiOm51bGwsXCJFbnVtRGVmaW5lXCI6XCJcIixcImxlbmd0aFwiOm51bGwsXCJkZWZhdWx0VmFsdWVcIjpcIlwiLFwiVGFibGVJRFwiOjksXCJJc1BLSURcIjpmYWxzZSxcIm9yZGVyaWRcIjo1LFwiQ2hhbmdlZFByb3BlcnRpZXNcIjpbXSxcIkJhY2t1cENoYW5nZWRQcm9wZXJ0aWVzXCI6W119LHtcImlkXCI6NDUsXCJjYXB0aW9uXCI6XCJjI+exu+Wei1wiLFwiTmFtZVwiOlwiVHlwZVwiLFwiSXNBdXRvSW5jcmVtZW50XCI6ZmFsc2UsXCJDYW5OdWxsXCI6dHJ1ZSxcImRiVHlwZVwiOlwidmFyY2hhclwiLFwiVHlwZVwiOm51bGwsXCJFbnVtRGVmaW5lXCI6XCJcIixcImxlbmd0aFwiOm51bGwsXCJkZWZhdWx0VmFsdWVcIjpcIlwiLFwiVGFibGVJRFwiOjksXCJJc1BLSURcIjpmYWxzZSxcIm9yZGVyaWRcIjo2LFwiQ2hhbmdlZFByb3BlcnRpZXNcIjpbXSxcIkJhY2t1cENoYW5nZWRQcm9wZXJ0aWVzXCI6W119LHtcImlkXCI6NDYsXCJjYXB0aW9uXCI6XCJFbnVt5a6a5LmJXCIsXCJOYW1lXCI6XCJFbnVtRGVmaW5lXCIsXCJJc0F1dG9JbmNyZW1lbnRcIjpmYWxzZSxcIkNhbk51bGxcIjp0cnVlLFwiZGJUeXBlXCI6XCJ2YXJjaGFyXCIsXCJUeXBlXCI6bnVsbCxcIkVudW1EZWZpbmVcIjpcIlwiLFwibGVuZ3RoXCI6XCIzMDBcIixcImRlZmF1bHRWYWx1ZVwiOlwiXCIsXCJUYWJsZUlEXCI6OSxcIklzUEtJRFwiOmZhbHNlLFwib3JkZXJpZFwiOjcsXCJDaGFuZ2VkUHJvcGVydGllc1wiOltdLFwiQmFja3VwQ2hhbmdlZFByb3BlcnRpZXNcIjpbXX0se1wiaWRcIjo0NyxcImNhcHRpb25cIjpcIumVv+W6plwiLFwiTmFtZVwiOlwibGVuZ3RoXCIsXCJJc0F1dG9JbmNyZW1lbnRcIjpmYWxzZSxcIkNhbk51bGxcIjp0cnVlLFwiZGJUeXBlXCI6XCJ2YXJjaGFyXCIsXCJUeXBlXCI6bnVsbCxcIkVudW1EZWZpbmVcIjpcIlwiLFwibGVuZ3RoXCI6bnVsbCxcImRlZmF1bHRWYWx1ZVwiOlwiXCIsXCJUYWJsZUlEXCI6OSxcIklzUEtJRFwiOmZhbHNlLFwib3JkZXJpZFwiOjgsXCJDaGFuZ2VkUHJvcGVydGllc1wiOltdLFwiQmFja3VwQ2hhbmdlZFByb3BlcnRpZXNcIjpbXX0se1wiaWRcIjo0OCxcImNhcHRpb25cIjpcIum7mOiupOWAvFwiLFwiTmFtZVwiOlwiZGVmYXVsdFZhbHVlXCIsXCJJc0F1dG9JbmNyZW1lbnRcIjpmYWxzZSxcIkNhbk51bGxcIjp0cnVlLFwiZGJUeXBlXCI6XCJ2YXJjaGFyXCIsXCJUeXBlXCI6bnVsbCxcIkVudW1EZWZpbmVcIjpcIlwiLFwibGVuZ3RoXCI6XCIyMDBcIixcImRlZmF1bHRWYWx1ZVwiOlwiXCIsXCJUYWJsZUlEXCI6OSxcIklzUEtJRFwiOmZhbHNlLFwib3JkZXJpZFwiOjksXCJDaGFuZ2VkUHJvcGVydGllc1wiOltdLFwiQmFja3VwQ2hhbmdlZFByb3BlcnRpZXNcIjpbXX0se1wiaWRcIjo0OSxcImNhcHRpb25cIjpcIlRhYmxlSURcIixcIk5hbWVcIjpcIlRhYmxlSURcIixcIklzQXV0b0luY3JlbWVudFwiOmZhbHNlLFwiQ2FuTnVsbFwiOnRydWUsXCJkYlR5cGVcIjpcImludFwiLFwiVHlwZVwiOm51bGwsXCJFbnVtRGVmaW5lXCI6XCJcIixcImxlbmd0aFwiOm51bGwsXCJkZWZhdWx0VmFsdWVcIjpcIlwiLFwiVGFibGVJRFwiOjksXCJJc1BLSURcIjpmYWxzZSxcIm9yZGVyaWRcIjoxMCxcIkNoYW5nZWRQcm9wZXJ0aWVzXCI6W10sXCJCYWNrdXBDaGFuZ2VkUHJvcGVydGllc1wiOltdfSx7XCJpZFwiOjUwLFwiY2FwdGlvblwiOlwi5piv5ZCm5piv5Li76ZSuXCIsXCJOYW1lXCI6XCJJc1BLSURcIixcIklzQXV0b0luY3JlbWVudFwiOmZhbHNlLFwiQ2FuTnVsbFwiOnRydWUsXCJkYlR5cGVcIjpcImJpdFwiLFwiVHlwZVwiOm51bGwsXCJFbnVtRGVmaW5lXCI6XCJcIixcImxlbmd0aFwiOm51bGwsXCJkZWZhdWx0VmFsdWVcIjpcIjBcIixcIlRhYmxlSURcIjo5LFwiSXNQS0lEXCI6ZmFsc2UsXCJvcmRlcmlkXCI6MTEsXCJDaGFuZ2VkUHJvcGVydGllc1wiOltdLFwiQmFja3VwQ2hhbmdlZFByb3BlcnRpZXNcIjpbXX0se1wiaWRcIjo1MSxcImNhcHRpb25cIjpcIm9yZGVyaWRcIixcIk5hbWVcIjpcIm9yZGVyaWRcIixcIklzQXV0b0luY3JlbWVudFwiOmZhbHNlLFwiQ2FuTnVsbFwiOnRydWUsXCJkYlR5cGVcIjpcImludFwiLFwiVHlwZVwiOm51bGwsXCJFbnVtRGVmaW5lXCI6XCJcIixcImxlbmd0aFwiOm51bGwsXCJkZWZhdWx0VmFsdWVcIjpcIjBcIixcIlRhYmxlSURcIjo5LFwiSXNQS0lEXCI6ZmFsc2UsXCJvcmRlcmlkXCI6MTIsXCJDaGFuZ2VkUHJvcGVydGllc1wiOltdLFwiQmFja3VwQ2hhbmdlZFByb3BlcnRpZXNcIjpbXX1dLFwiSURYQ29uZmlnc1wiOltdLFwiSURcIjowfSJ9LHsiTmFtZSI6ImRhdGFiYXNlaWQiLCJWYWx1ZSI6Mn1dfSx7Ikl0ZW1zIjpbeyJOYW1lIjoiaWQiLCJWYWx1ZSI6MTQ2fSx7Ik5hbWUiOiJ0eXBlIiwiVmFsdWUiOiJFbnRpdHlEQi5EZXNpZ24uQWN0aW9ucy5DcmVhdGVUYWJsZUFjdGlvbiJ9LHsiTmFtZSI6ImNvbnRlbnQiLCJWYWx1ZSI6IntcIlRhYmxlXCI6e1wiaWRcIjoxMCxcImNhcHRpb25cIjpcIuaVsOaNruihqOadg+mZkFwiLFwiTmFtZVwiOlwiVGFibGVQb3dlclwiLFwiRGF0YWJhc2VJRFwiOjIsXCJpTG9ja1wiOjAsXCJDaGFuZ2VkUHJvcGVydGllc1wiOltdLFwiQmFja3VwQ2hhbmdlZFByb3BlcnRpZXNcIjpbXX0sXCJDb2x1bW5zXCI6W3tcImlkXCI6NTIsXCJjYXB0aW9uXCI6XCJpZFwiLFwiTmFtZVwiOlwiaWRcIixcIklzQXV0b0luY3JlbWVudFwiOnRydWUsXCJDYW5OdWxsXCI6dHJ1ZSxcImRiVHlwZVwiOlwiaW50XCIsXCJUeXBlXCI6bnVsbCxcIkVudW1EZWZpbmVcIjpcIlwiLFwibGVuZ3RoXCI6bnVsbCxcImRlZmF1bHRWYWx1ZVwiOlwiXCIsXCJUYWJsZUlEXCI6MTAsXCJJc1BLSURcIjp0cnVlLFwib3JkZXJpZFwiOjAsXCJDaGFuZ2VkUHJvcGVydGllc1wiOltdLFwiQmFja3VwQ2hhbmdlZFByb3BlcnRpZXNcIjpbXX0se1wiaWRcIjo1MyxcImNhcHRpb25cIjpcIlVzZXJJRFwiLFwiTmFtZVwiOlwiVXNlcklEXCIsXCJJc0F1dG9JbmNyZW1lbnRcIjpmYWxzZSxcIkNhbk51bGxcIjp0cnVlLFwiZGJUeXBlXCI6XCJpbnRcIixcIlR5cGVcIjpudWxsLFwiRW51bURlZmluZVwiOlwiXCIsXCJsZW5ndGhcIjpudWxsLFwiZGVmYXVsdFZhbHVlXCI6XCJcIixcIlRhYmxlSURcIjoxMCxcIklzUEtJRFwiOmZhbHNlLFwib3JkZXJpZFwiOjEsXCJDaGFuZ2VkUHJvcGVydGllc1wiOltdLFwiQmFja3VwQ2hhbmdlZFByb3BlcnRpZXNcIjpbXX0se1wiaWRcIjo1NCxcImNhcHRpb25cIjpcIlRhYmxlSURcIixcIk5hbWVcIjpcIlRhYmxlSURcIixcIklzQXV0b0luY3JlbWVudFwiOmZhbHNlLFwiQ2FuTnVsbFwiOnRydWUsXCJkYlR5cGVcIjpcImludFwiLFwiVHlwZVwiOm51bGwsXCJFbnVtRGVmaW5lXCI6XCJcIixcImxlbmd0aFwiOm51bGwsXCJkZWZhdWx0VmFsdWVcIjpcIlwiLFwiVGFibGVJRFwiOjEwLFwiSXNQS0lEXCI6ZmFsc2UsXCJvcmRlcmlkXCI6MixcIkNoYW5nZWRQcm9wZXJ0aWVzXCI6W10sXCJCYWNrdXBDaGFuZ2VkUHJvcGVydGllc1wiOltdfV0sXCJJRFhDb25maWdzXCI6W10sXCJJRFwiOjB9In0seyJOYW1lIjoiZGF0YWJhc2VpZCIsIlZhbHVlIjoyfV19LHsiSXRlbXMiOlt7Ik5hbWUiOiJpZCIsIlZhbHVlIjoxNDd9LHsiTmFtZSI6InR5cGUiLCJWYWx1ZSI6IkVudGl0eURCLkRlc2lnbi5BY3Rpb25zLkNyZWF0ZVRhYmxlQWN0aW9uIn0seyJOYW1lIjoiY29udGVudCIsIlZhbHVlIjoie1wiVGFibGVcIjp7XCJpZFwiOjExLFwiY2FwdGlvblwiOlwi6aG555uu5p2D6ZmQXCIsXCJOYW1lXCI6XCJQcm9qZWN0UG93ZXJcIixcIkRhdGFiYXNlSURcIjoyLFwiaUxvY2tcIjowLFwiQ2hhbmdlZFByb3BlcnRpZXNcIjpbXSxcIkJhY2t1cENoYW5nZWRQcm9wZXJ0aWVzXCI6W119LFwiQ29sdW1uc1wiOlt7XCJpZFwiOjU1LFwiY2FwdGlvblwiOlwiaWRcIixcIk5hbWVcIjpcImlkXCIsXCJJc0F1dG9JbmNyZW1lbnRcIjp0cnVlLFwiQ2FuTnVsbFwiOnRydWUsXCJkYlR5cGVcIjpcImludFwiLFwiVHlwZVwiOm51bGwsXCJFbnVtRGVmaW5lXCI6XCJcIixcImxlbmd0aFwiOm51bGwsXCJkZWZhdWx0VmFsdWVcIjpcIlwiLFwiVGFibGVJRFwiOjExLFwiSXNQS0lEXCI6dHJ1ZSxcIm9yZGVyaWRcIjowLFwiQ2hhbmdlZFByb3BlcnRpZXNcIjpbXSxcIkJhY2t1cENoYW5nZWRQcm9wZXJ0aWVzXCI6W119LHtcImlkXCI6NTYsXCJjYXB0aW9uXCI6XCJQcm9qZWN0SURcIixcIk5hbWVcIjpcIlByb2plY3RJRFwiLFwiSXNBdXRvSW5jcmVtZW50XCI6ZmFsc2UsXCJDYW5OdWxsXCI6dHJ1ZSxcImRiVHlwZVwiOlwiaW50XCIsXCJUeXBlXCI6bnVsbCxcIkVudW1EZWZpbmVcIjpcIlwiLFwibGVuZ3RoXCI6bnVsbCxcImRlZmF1bHRWYWx1ZVwiOlwiXCIsXCJUYWJsZUlEXCI6MTEsXCJJc1BLSURcIjpmYWxzZSxcIm9yZGVyaWRcIjoxLFwiQ2hhbmdlZFByb3BlcnRpZXNcIjpbXSxcIkJhY2t1cENoYW5nZWRQcm9wZXJ0aWVzXCI6W119LHtcImlkXCI6NTcsXCJjYXB0aW9uXCI6XCJVc2VySURcIixcIk5hbWVcIjpcIlVzZXJJRFwiLFwiSXNBdXRvSW5jcmVtZW50XCI6ZmFsc2UsXCJDYW5OdWxsXCI6dHJ1ZSxcImRiVHlwZVwiOlwiaW50XCIsXCJUeXBlXCI6bnVsbCxcIkVudW1EZWZpbmVcIjpcIlwiLFwibGVuZ3RoXCI6bnVsbCxcImRlZmF1bHRWYWx1ZVwiOlwiXCIsXCJUYWJsZUlEXCI6MTEsXCJJc1BLSURcIjpmYWxzZSxcIm9yZGVyaWRcIjoyLFwiQ2hhbmdlZFByb3BlcnRpZXNcIjpbXSxcIkJhY2t1cENoYW5nZWRQcm9wZXJ0aWVzXCI6W119XSxcIklEWENvbmZpZ3NcIjpbXSxcIklEXCI6MH0ifSx7Ik5hbWUiOiJkYXRhYmFzZWlkIiwiVmFsdWUiOjJ9XX0seyJJdGVtcyI6W3siTmFtZSI6ImlkIiwiVmFsdWUiOjE0OH0seyJOYW1lIjoidHlwZSIsIlZhbHVlIjoiRW50aXR5REIuRGVzaWduLkFjdGlvbnMuQ3JlYXRlVGFibGVBY3Rpb24ifSx7Ik5hbWUiOiJjb250ZW50IiwiVmFsdWUiOiJ7XCJUYWJsZVwiOntcImlkXCI6MTMsXCJjYXB0aW9uXCI6XCLmlbDmja7mqKHlnZdcIixcIk5hbWVcIjpcIkRCTW9kdWxlXCIsXCJEYXRhYmFzZUlEXCI6MixcImlMb2NrXCI6MCxcIkNoYW5nZWRQcm9wZXJ0aWVzXCI6W10sXCJCYWNrdXBDaGFuZ2VkUHJvcGVydGllc1wiOltdfSxcIkNvbHVtbnNcIjpbe1wiaWRcIjo2MSxcImNhcHRpb25cIjpcImlkXCIsXCJOYW1lXCI6XCJpZFwiLFwiSXNBdXRvSW5jcmVtZW50XCI6dHJ1ZSxcIkNhbk51bGxcIjp0cnVlLFwiZGJUeXBlXCI6XCJpbnRcIixcIlR5cGVcIjpudWxsLFwiRW51bURlZmluZVwiOlwiXCIsXCJsZW5ndGhcIjpudWxsLFwiZGVmYXVsdFZhbHVlXCI6XCJcIixcIlRhYmxlSURcIjoxMyxcIklzUEtJRFwiOnRydWUsXCJvcmRlcmlkXCI6MCxcIkNoYW5nZWRQcm9wZXJ0aWVzXCI6W10sXCJCYWNrdXBDaGFuZ2VkUHJvcGVydGllc1wiOltdfSx7XCJpZFwiOjYyLFwiY2FwdGlvblwiOlwiTmFtZVwiLFwiTmFtZVwiOlwiTmFtZVwiLFwiSXNBdXRvSW5jcmVtZW50XCI6ZmFsc2UsXCJDYW5OdWxsXCI6dHJ1ZSxcImRiVHlwZVwiOlwidmFyY2hhclwiLFwiVHlwZVwiOm51bGwsXCJFbnVtRGVmaW5lXCI6XCJcIixcImxlbmd0aFwiOm51bGwsXCJkZWZhdWx0VmFsdWVcIjpcIlwiLFwiVGFibGVJRFwiOjEzLFwiSXNQS0lEXCI6ZmFsc2UsXCJvcmRlcmlkXCI6MSxcIkNoYW5nZWRQcm9wZXJ0aWVzXCI6W10sXCJCYWNrdXBDaGFuZ2VkUHJvcGVydGllc1wiOltdfSx7XCJpZFwiOjYzLFwiY2FwdGlvblwiOlwiRGF0YWJhc2VJRFwiLFwiTmFtZVwiOlwiRGF0YWJhc2VJRFwiLFwiSXNBdXRvSW5jcmVtZW50XCI6ZmFsc2UsXCJDYW5OdWxsXCI6dHJ1ZSxcImRiVHlwZVwiOlwiaW50XCIsXCJUeXBlXCI6bnVsbCxcIkVudW1EZWZpbmVcIjpcIlwiLFwibGVuZ3RoXCI6bnVsbCxcImRlZmF1bHRWYWx1ZVwiOlwiXCIsXCJUYWJsZUlEXCI6MTMsXCJJc1BLSURcIjpmYWxzZSxcIm9yZGVyaWRcIjoyLFwiQ2hhbmdlZFByb3BlcnRpZXNcIjpbXSxcIkJhY2t1cENoYW5nZWRQcm9wZXJ0aWVzXCI6W119LHtcImlkXCI6NjQsXCJjYXB0aW9uXCI6XCJJc0ZvbGRlclwiLFwiTmFtZVwiOlwiSXNGb2xkZXJcIixcIklzQXV0b0luY3JlbWVudFwiOmZhbHNlLFwiQ2FuTnVsbFwiOnRydWUsXCJkYlR5cGVcIjpcImJpdFwiLFwiVHlwZVwiOm51bGwsXCJFbnVtRGVmaW5lXCI6XCJcIixcImxlbmd0aFwiOm51bGwsXCJkZWZhdWx0VmFsdWVcIjpcIjBcIixcIlRhYmxlSURcIjoxMyxcIklzUEtJRFwiOmZhbHNlLFwib3JkZXJpZFwiOjMsXCJDaGFuZ2VkUHJvcGVydGllc1wiOltdLFwiQmFja3VwQ2hhbmdlZFByb3BlcnRpZXNcIjpbXX0se1wiaWRcIjo2NSxcImNhcHRpb25cIjpcInBhcmVudElEXCIsXCJOYW1lXCI6XCJwYXJlbnRJRFwiLFwiSXNBdXRvSW5jcmVtZW50XCI6ZmFsc2UsXCJDYW5OdWxsXCI6dHJ1ZSxcImRiVHlwZVwiOlwiaW50XCIsXCJUeXBlXCI6bnVsbCxcIkVudW1EZWZpbmVcIjpcIlwiLFwibGVuZ3RoXCI6bnVsbCxcImRlZmF1bHRWYWx1ZVwiOlwiXCIsXCJUYWJsZUlEXCI6MTMsXCJJc1BLSURcIjpmYWxzZSxcIm9yZGVyaWRcIjo0LFwiQ2hhbmdlZFByb3BlcnRpZXNcIjpbXSxcIkJhY2t1cENoYW5nZWRQcm9wZXJ0aWVzXCI6W119XSxcIklEWENvbmZpZ3NcIjpbXSxcIklEXCI6MH0ifSx7Ik5hbWUiOiJkYXRhYmFzZWlkIiwiVmFsdWUiOjJ9XX0seyJJdGVtcyI6W3siTmFtZSI6ImlkIiwiVmFsdWUiOjE0OX0seyJOYW1lIjoidHlwZSIsIlZhbHVlIjoiRW50aXR5REIuRGVzaWduLkFjdGlvbnMuQ3JlYXRlVGFibGVBY3Rpb24ifSx7Ik5hbWUiOiJjb250ZW50IiwiVmFsdWUiOiJ7XCJUYWJsZVwiOntcImlkXCI6MTQsXCJjYXB0aW9uXCI6XCLnuqfogZTliKDpmaRcIixcIk5hbWVcIjpcIkRCRGVsZXRlQ29uZmlnXCIsXCJEYXRhYmFzZUlEXCI6MixcImlMb2NrXCI6MCxcIkNoYW5nZWRQcm9wZXJ0aWVzXCI6W10sXCJCYWNrdXBDaGFuZ2VkUHJvcGVydGllc1wiOltdfSxcIkNvbHVtbnNcIjpbe1wiaWRcIjo2NixcImNhcHRpb25cIjpcImlkXCIsXCJOYW1lXCI6XCJpZFwiLFwiSXNBdXRvSW5jcmVtZW50XCI6dHJ1ZSxcIkNhbk51bGxcIjp0cnVlLFwiZGJUeXBlXCI6XCJpbnRcIixcIlR5cGVcIjpudWxsLFwiRW51bURlZmluZVwiOlwiXCIsXCJsZW5ndGhcIjpudWxsLFwiZGVmYXVsdFZhbHVlXCI6XCJcIixcIlRhYmxlSURcIjoxNCxcIklzUEtJRFwiOnRydWUsXCJvcmRlcmlkXCI6MCxcIkNoYW5nZWRQcm9wZXJ0aWVzXCI6W10sXCJCYWNrdXBDaGFuZ2VkUHJvcGVydGllc1wiOltdfSx7XCJpZFwiOjY3LFwiY2FwdGlvblwiOlwiVGFibGVJRFwiLFwiTmFtZVwiOlwiVGFibGVJRFwiLFwiSXNBdXRvSW5jcmVtZW50XCI6ZmFsc2UsXCJDYW5OdWxsXCI6dHJ1ZSxcImRiVHlwZVwiOlwiaW50XCIsXCJUeXBlXCI6bnVsbCxcIkVudW1EZWZpbmVcIjpcIlwiLFwibGVuZ3RoXCI6bnVsbCxcImRlZmF1bHRWYWx1ZVwiOlwiXCIsXCJUYWJsZUlEXCI6MTQsXCJJc1BLSURcIjpmYWxzZSxcIm9yZGVyaWRcIjoxLFwiQ2hhbmdlZFByb3BlcnRpZXNcIjpbXSxcIkJhY2t1cENoYW5nZWRQcm9wZXJ0aWVzXCI6W119LHtcImlkXCI6NjgsXCJjYXB0aW9uXCI6XCLlhbPogZTooahJRFwiLFwiTmFtZVwiOlwiUmVsYVRhYmxlSURcIixcIklzQXV0b0luY3JlbWVudFwiOmZhbHNlLFwiQ2FuTnVsbFwiOnRydWUsXCJkYlR5cGVcIjpcImludFwiLFwiVHlwZVwiOm51bGwsXCJFbnVtRGVmaW5lXCI6XCJcIixcImxlbmd0aFwiOm51bGwsXCJkZWZhdWx0VmFsdWVcIjpcIlwiLFwiVGFibGVJRFwiOjE0LFwiSXNQS0lEXCI6ZmFsc2UsXCJvcmRlcmlkXCI6MixcIkNoYW5nZWRQcm9wZXJ0aWVzXCI6W10sXCJCYWNrdXBDaGFuZ2VkUHJvcGVydGllc1wiOltdfSx7XCJpZFwiOjY5LFwiY2FwdGlvblwiOlwiUmVsYVRhYmxlX0Rlc2NcIixcIk5hbWVcIjpcIlJlbGFUYWJsZV9EZXNjXCIsXCJJc0F1dG9JbmNyZW1lbnRcIjpmYWxzZSxcIkNhbk51bGxcIjp0cnVlLFwiZGJUeXBlXCI6XCJ2YXJjaGFyXCIsXCJUeXBlXCI6bnVsbCxcIkVudW1EZWZpbmVcIjpcIlwiLFwibGVuZ3RoXCI6bnVsbCxcImRlZmF1bHRWYWx1ZVwiOlwiXCIsXCJUYWJsZUlEXCI6MTQsXCJJc1BLSURcIjpmYWxzZSxcIm9yZGVyaWRcIjozLFwiQ2hhbmdlZFByb3BlcnRpZXNcIjpbXSxcIkJhY2t1cENoYW5nZWRQcm9wZXJ0aWVzXCI6W119LHtcImlkXCI6NzAsXCJjYXB0aW9uXCI6XCLlhbPogZTlrZfmrrXnmoRJRFwiLFwiTmFtZVwiOlwiUmVsYUNvbHVtSURcIixcIklzQXV0b0luY3JlbWVudFwiOmZhbHNlLFwiQ2FuTnVsbFwiOnRydWUsXCJkYlR5cGVcIjpcImludFwiLFwiVHlwZVwiOm51bGwsXCJFbnVtRGVmaW5lXCI6XCJcIixcImxlbmd0aFwiOm51bGwsXCJkZWZhdWx0VmFsdWVcIjpcIlwiLFwiVGFibGVJRFwiOjE0LFwiSXNQS0lEXCI6ZmFsc2UsXCJvcmRlcmlkXCI6NCxcIkNoYW5nZWRQcm9wZXJ0aWVzXCI6W10sXCJCYWNrdXBDaGFuZ2VkUHJvcGVydGllc1wiOltdfSx7XCJpZFwiOjcxLFwiY2FwdGlvblwiOlwiUmVsYUNvbHVtbl9EZXNjXCIsXCJOYW1lXCI6XCJSZWxhQ29sdW1uX0Rlc2NcIixcIklzQXV0b0luY3JlbWVudFwiOmZhbHNlLFwiQ2FuTnVsbFwiOnRydWUsXCJkYlR5cGVcIjpcInZhcmNoYXJcIixcIlR5cGVcIjpudWxsLFwiRW51bURlZmluZVwiOlwiXCIsXCJsZW5ndGhcIjpudWxsLFwiZGVmYXVsdFZhbHVlXCI6XCJcIixcIlRhYmxlSURcIjoxNCxcIklzUEtJRFwiOmZhbHNlLFwib3JkZXJpZFwiOjUsXCJDaGFuZ2VkUHJvcGVydGllc1wiOltdLFwiQmFja3VwQ2hhbmdlZFByb3BlcnRpZXNcIjpbXX1dLFwiSURYQ29uZmlnc1wiOltdLFwiSURcIjowfSJ9LHsiTmFtZSI6ImRhdGFiYXNlaWQiLCJWYWx1ZSI6Mn1dfSx7Ikl0ZW1zIjpbeyJOYW1lIjoiaWQiLCJWYWx1ZSI6MTUwfSx7Ik5hbWUiOiJ0eXBlIiwiVmFsdWUiOiJFbnRpdHlEQi5EZXNpZ24uQWN0aW9ucy5DcmVhdGVUYWJsZUFjdGlvbiJ9LHsiTmFtZSI6ImNvbnRlbnQiLCJWYWx1ZSI6IntcIlRhYmxlXCI6e1wiaWRcIjoxNSxcImNhcHRpb25cIjpcIlRhYmxlSW5Nb2R1bGVcIixcIk5hbWVcIjpcIlRhYmxlSW5Nb2R1bGVcIixcIkRhdGFiYXNlSURcIjoyLFwiaUxvY2tcIjowLFwiQ2hhbmdlZFByb3BlcnRpZXNcIjpbXSxcIkJhY2t1cENoYW5nZWRQcm9wZXJ0aWVzXCI6W119LFwiQ29sdW1uc1wiOlt7XCJpZFwiOjcyLFwiY2FwdGlvblwiOlwiaWRcIixcIk5hbWVcIjpcImlkXCIsXCJJc0F1dG9JbmNyZW1lbnRcIjp0cnVlLFwiQ2FuTnVsbFwiOnRydWUsXCJkYlR5cGVcIjpcImludFwiLFwiVHlwZVwiOm51bGwsXCJFbnVtRGVmaW5lXCI6XCJcIixcImxlbmd0aFwiOm51bGwsXCJkZWZhdWx0VmFsdWVcIjpcIlwiLFwiVGFibGVJRFwiOjE1LFwiSXNQS0lEXCI6dHJ1ZSxcIm9yZGVyaWRcIjowLFwiQ2hhbmdlZFByb3BlcnRpZXNcIjpbXSxcIkJhY2t1cENoYW5nZWRQcm9wZXJ0aWVzXCI6W119LHtcImlkXCI6NzMsXCJjYXB0aW9uXCI6XCJUYWJsZUlEXCIsXCJOYW1lXCI6XCJUYWJsZUlEXCIsXCJJc0F1dG9JbmNyZW1lbnRcIjpmYWxzZSxcIkNhbk51bGxcIjp0cnVlLFwiZGJUeXBlXCI6XCJpbnRcIixcIlR5cGVcIjpudWxsLFwiRW51bURlZmluZVwiOlwiXCIsXCJsZW5ndGhcIjpudWxsLFwiZGVmYXVsdFZhbHVlXCI6XCJcIixcIlRhYmxlSURcIjoxNSxcIklzUEtJRFwiOmZhbHNlLFwib3JkZXJpZFwiOjEsXCJDaGFuZ2VkUHJvcGVydGllc1wiOltdLFwiQmFja3VwQ2hhbmdlZFByb3BlcnRpZXNcIjpbXX0se1wiaWRcIjo3NCxcImNhcHRpb25cIjpcIk1vZHVsZUlEXCIsXCJOYW1lXCI6XCJNb2R1bGVJRFwiLFwiSXNBdXRvSW5jcmVtZW50XCI6ZmFsc2UsXCJDYW5OdWxsXCI6dHJ1ZSxcImRiVHlwZVwiOlwiaW50XCIsXCJUeXBlXCI6bnVsbCxcIkVudW1EZWZpbmVcIjpcIlwiLFwibGVuZ3RoXCI6bnVsbCxcImRlZmF1bHRWYWx1ZVwiOlwiXCIsXCJUYWJsZUlEXCI6MTUsXCJJc1BLSURcIjpmYWxzZSxcIm9yZGVyaWRcIjoyLFwiQ2hhbmdlZFByb3BlcnRpZXNcIjpbXSxcIkJhY2t1cENoYW5nZWRQcm9wZXJ0aWVzXCI6W119LHtcImlkXCI6NzUsXCJjYXB0aW9uXCI6XCJ4XCIsXCJOYW1lXCI6XCJ4XCIsXCJJc0F1dG9JbmNyZW1lbnRcIjpmYWxzZSxcIkNhbk51bGxcIjp0cnVlLFwiZGJUeXBlXCI6XCJpbnRcIixcIlR5cGVcIjpudWxsLFwiRW51bURlZmluZVwiOlwiXCIsXCJsZW5ndGhcIjpudWxsLFwiZGVmYXVsdFZhbHVlXCI6XCJcIixcIlRhYmxlSURcIjoxNSxcIklzUEtJRFwiOmZhbHNlLFwib3JkZXJpZFwiOjMsXCJDaGFuZ2VkUHJvcGVydGllc1wiOltdLFwiQmFja3VwQ2hhbmdlZFByb3BlcnRpZXNcIjpbXX0se1wiaWRcIjo3NixcImNhcHRpb25cIjpcInlcIixcIk5hbWVcIjpcInlcIixcIklzQXV0b0luY3JlbWVudFwiOmZhbHNlLFwiQ2FuTnVsbFwiOnRydWUsXCJkYlR5cGVcIjpcImludFwiLFwiVHlwZVwiOm51bGwsXCJFbnVtRGVmaW5lXCI6XCJcIixcImxlbmd0aFwiOm51bGwsXCJkZWZhdWx0VmFsdWVcIjpcIlwiLFwiVGFibGVJRFwiOjE1LFwiSXNQS0lEXCI6ZmFsc2UsXCJvcmRlcmlkXCI6NCxcIkNoYW5nZWRQcm9wZXJ0aWVzXCI6W10sXCJCYWNrdXBDaGFuZ2VkUHJvcGVydGllc1wiOltdfSx7XCJpZFwiOjc3LFwiY2FwdGlvblwiOlwi5Li05pe25Y+Y6YePXCIsXCJOYW1lXCI6XCJmbGFnXCIsXCJJc0F1dG9JbmNyZW1lbnRcIjpmYWxzZSxcIkNhbk51bGxcIjp0cnVlLFwiZGJUeXBlXCI6XCJ2YXJjaGFyXCIsXCJUeXBlXCI6bnVsbCxcIkVudW1EZWZpbmVcIjpcIlwiLFwibGVuZ3RoXCI6bnVsbCxcImRlZmF1bHRWYWx1ZVwiOlwiXCIsXCJUYWJsZUlEXCI6MTUsXCJJc1BLSURcIjpmYWxzZSxcIm9yZGVyaWRcIjo1LFwiQ2hhbmdlZFByb3BlcnRpZXNcIjpbXSxcIkJhY2t1cENoYW5nZWRQcm9wZXJ0aWVzXCI6W119LHtcImlkXCI6NzgsXCJjYXB0aW9uXCI6XCJmbGFnMlwiLFwiTmFtZVwiOlwiZmxhZzJcIixcIklzQXV0b0luY3JlbWVudFwiOmZhbHNlLFwiQ2FuTnVsbFwiOnRydWUsXCJkYlR5cGVcIjpcInZhcmNoYXJcIixcIlR5cGVcIjpudWxsLFwiRW51bURlZmluZVwiOlwiXCIsXCJsZW5ndGhcIjpudWxsLFwiZGVmYXVsdFZhbHVlXCI6XCJcIixcIlRhYmxlSURcIjoxNSxcIklzUEtJRFwiOmZhbHNlLFwib3JkZXJpZFwiOjYsXCJDaGFuZ2VkUHJvcGVydGllc1wiOltdLFwiQmFja3VwQ2hhbmdlZFByb3BlcnRpZXNcIjpbXX1dLFwiSURYQ29uZmlnc1wiOltdLFwiSURcIjowfSJ9LHsiTmFtZSI6ImRhdGFiYXNlaWQiLCJWYWx1ZSI6Mn1dfSx7Ikl0ZW1zIjpbeyJOYW1lIjoiaWQiLCJWYWx1ZSI6MTUxfSx7Ik5hbWUiOiJ0eXBlIiwiVmFsdWUiOiJFbnRpdHlEQi5EZXNpZ24uQWN0aW9ucy5DcmVhdGVUYWJsZUFjdGlvbiJ9LHsiTmFtZSI6ImNvbnRlbnQiLCJWYWx1ZSI6IntcIlRhYmxlXCI6e1wiaWRcIjoxNixcImNhcHRpb25cIjpcIuWUr+S4gOWAvOe0ouW8lVwiLFwiTmFtZVwiOlwiSURYSW5kZXhcIixcIkRhdGFiYXNlSURcIjoyLFwiaUxvY2tcIjowLFwiQ2hhbmdlZFByb3BlcnRpZXNcIjpbXSxcIkJhY2t1cENoYW5nZWRQcm9wZXJ0aWVzXCI6W119LFwiQ29sdW1uc1wiOlt7XCJpZFwiOjc5LFwiY2FwdGlvblwiOlwiaWRcIixcIk5hbWVcIjpcImlkXCIsXCJJc0F1dG9JbmNyZW1lbnRcIjp0cnVlLFwiQ2FuTnVsbFwiOnRydWUsXCJkYlR5cGVcIjpcImludFwiLFwiVHlwZVwiOm51bGwsXCJFbnVtRGVmaW5lXCI6XCJcIixcImxlbmd0aFwiOm51bGwsXCJkZWZhdWx0VmFsdWVcIjpcIlwiLFwiVGFibGVJRFwiOjE2LFwiSXNQS0lEXCI6dHJ1ZSxcIm9yZGVyaWRcIjowLFwiQ2hhbmdlZFByb3BlcnRpZXNcIjpbXSxcIkJhY2t1cENoYW5nZWRQcm9wZXJ0aWVzXCI6W119LHtcImlkXCI6ODAsXCJjYXB0aW9uXCI6XCJUYWJsZUlEXCIsXCJOYW1lXCI6XCJUYWJsZUlEXCIsXCJJc0F1dG9JbmNyZW1lbnRcIjpmYWxzZSxcIkNhbk51bGxcIjp0cnVlLFwiZGJUeXBlXCI6XCJpbnRcIixcIlR5cGVcIjpudWxsLFwiRW51bURlZmluZVwiOlwiXCIsXCJsZW5ndGhcIjpudWxsLFwiZGVmYXVsdFZhbHVlXCI6XCJcIixcIlRhYmxlSURcIjoxNixcIklzUEtJRFwiOmZhbHNlLFwib3JkZXJpZFwiOjEsXCJDaGFuZ2VkUHJvcGVydGllc1wiOltdLFwiQmFja3VwQ2hhbmdlZFByb3BlcnRpZXNcIjpbXX0se1wiaWRcIjo4MSxcImNhcHRpb25cIjpcIktleXNcIixcIk5hbWVcIjpcIktleXNcIixcIklzQXV0b0luY3JlbWVudFwiOmZhbHNlLFwiQ2FuTnVsbFwiOnRydWUsXCJkYlR5cGVcIjpcInZhcmNoYXJcIixcIlR5cGVcIjpudWxsLFwiRW51bURlZmluZVwiOlwiXCIsXCJsZW5ndGhcIjpcIjEwMFwiLFwiZGVmYXVsdFZhbHVlXCI6XCJcIixcIlRhYmxlSURcIjoxNixcIklzUEtJRFwiOmZhbHNlLFwib3JkZXJpZFwiOjIsXCJDaGFuZ2VkUHJvcGVydGllc1wiOltdLFwiQmFja3VwQ2hhbmdlZFByb3BlcnRpZXNcIjpbXX0se1wiaWRcIjoxNjQsXCJjYXB0aW9uXCI6XCLmmK/lkKbllK/kuIDntKLlvJVcIixcIk5hbWVcIjpcIklzVW5pcXVlXCIsXCJJc0F1dG9JbmNyZW1lbnRcIjpmYWxzZSxcIkNhbk51bGxcIjp0cnVlLFwiZGJUeXBlXCI6XCJiaXRcIixcIlR5cGVcIjpudWxsLFwiRW51bURlZmluZVwiOm51bGwsXCJsZW5ndGhcIjpcIlwiLFwiZGVmYXVsdFZhbHVlXCI6XCIxXCIsXCJUYWJsZUlEXCI6MTYsXCJJc1BLSURcIjpmYWxzZSxcIm9yZGVyaWRcIjozLFwiQ2hhbmdlZFByb3BlcnRpZXNcIjpbXSxcIkJhY2t1cENoYW5nZWRQcm9wZXJ0aWVzXCI6W119LHtcImlkXCI6MTY1LFwiY2FwdGlvblwiOlwi5piv5ZCm6IGa54SmXCIsXCJOYW1lXCI6XCJJc0NsdXN0ZXJlZFwiLFwiSXNBdXRvSW5jcmVtZW50XCI6ZmFsc2UsXCJDYW5OdWxsXCI6dHJ1ZSxcImRiVHlwZVwiOlwiYml0XCIsXCJUeXBlXCI6bnVsbCxcIkVudW1EZWZpbmVcIjpudWxsLFwibGVuZ3RoXCI6XCJcIixcImRlZmF1bHRWYWx1ZVwiOlwiMFwiLFwiVGFibGVJRFwiOjE2LFwiSXNQS0lEXCI6ZmFsc2UsXCJvcmRlcmlkXCI6NCxcIkNoYW5nZWRQcm9wZXJ0aWVzXCI6W10sXCJCYWNrdXBDaGFuZ2VkUHJvcGVydGllc1wiOltdfV0sXCJJRFhDb25maWdzXCI6W10sXCJJRFwiOjB9In0seyJOYW1lIjoiZGF0YWJhc2VpZCIsIlZhbHVlIjoyfV19LHsiSXRlbXMiOlt7Ik5hbWUiOiJpZCIsIlZhbHVlIjoxNTJ9LHsiTmFtZSI6InR5cGUiLCJWYWx1ZSI6IkVudGl0eURCLkRlc2lnbi5BY3Rpb25zLkNyZWF0ZVRhYmxlQWN0aW9uIn0seyJOYW1lIjoiY29udGVudCIsIlZhbHVlIjoie1wiVGFibGVcIjp7XCJpZFwiOjI1LFwiY2FwdGlvblwiOlwiQnVn5aSE55CG5Y6G5Y+y6K6w5b2VXCIsXCJOYW1lXCI6XCJCdWdIYW5kbGVIaXN0b3J5XCIsXCJEYXRhYmFzZUlEXCI6MixcImlMb2NrXCI6MCxcIkNoYW5nZWRQcm9wZXJ0aWVzXCI6W10sXCJCYWNrdXBDaGFuZ2VkUHJvcGVydGllc1wiOltdfSxcIkNvbHVtbnNcIjpbe1wiaWRcIjoxMDcsXCJjYXB0aW9uXCI6bnVsbCxcIk5hbWVcIjpcImlkXCIsXCJJc0F1dG9JbmNyZW1lbnRcIjp0cnVlLFwiQ2FuTnVsbFwiOmZhbHNlLFwiZGJUeXBlXCI6XCJpbnRcIixcIlR5cGVcIjpudWxsLFwiRW51bURlZmluZVwiOm51bGwsXCJsZW5ndGhcIjpudWxsLFwiZGVmYXVsdFZhbHVlXCI6bnVsbCxcIlRhYmxlSURcIjoyNSxcIklzUEtJRFwiOnRydWUsXCJvcmRlcmlkXCI6MCxcIkNoYW5nZWRQcm9wZXJ0aWVzXCI6W10sXCJCYWNrdXBDaGFuZ2VkUHJvcGVydGllc1wiOltdfSx7XCJpZFwiOjExMyxcImNhcHRpb25cIjpudWxsLFwiTmFtZVwiOlwiQnVnSURcIixcIklzQXV0b0luY3JlbWVudFwiOmZhbHNlLFwiQ2FuTnVsbFwiOnRydWUsXCJkYlR5cGVcIjpcImludFwiLFwiVHlwZVwiOm51bGwsXCJFbnVtRGVmaW5lXCI6bnVsbCxcImxlbmd0aFwiOlwiXCIsXCJkZWZhdWx0VmFsdWVcIjpudWxsLFwiVGFibGVJRFwiOjI1LFwiSXNQS0lEXCI6ZmFsc2UsXCJvcmRlcmlkXCI6MSxcIkNoYW5nZWRQcm9wZXJ0aWVzXCI6W10sXCJCYWNrdXBDaGFuZ2VkUHJvcGVydGllc1wiOltdfSx7XCJpZFwiOjE0NCxcImNhcHRpb25cIjpcIuWPkeagh+iAhUlEXCIsXCJOYW1lXCI6XCJVc2VySURcIixcIklzQXV0b0luY3JlbWVudFwiOmZhbHNlLFwiQ2FuTnVsbFwiOnRydWUsXCJkYlR5cGVcIjpcImludFwiLFwiVHlwZVwiOm51bGwsXCJFbnVtRGVmaW5lXCI6bnVsbCxcImxlbmd0aFwiOlwiXCIsXCJkZWZhdWx0VmFsdWVcIjpudWxsLFwiVGFibGVJRFwiOjI1LFwiSXNQS0lEXCI6ZmFsc2UsXCJvcmRlcmlkXCI6MixcIkNoYW5nZWRQcm9wZXJ0aWVzXCI6W10sXCJCYWNrdXBDaGFuZ2VkUHJvcGVydGllc1wiOltdfSx7XCJpZFwiOjE0NSxcImNhcHRpb25cIjpcIuWGheWuuVwiLFwiTmFtZVwiOlwiY29udGVudFwiLFwiSXNBdXRvSW5jcmVtZW50XCI6ZmFsc2UsXCJDYW5OdWxsXCI6dHJ1ZSxcImRiVHlwZVwiOlwiaW1hZ2VcIixcIlR5cGVcIjpudWxsLFwiRW51bURlZmluZVwiOm51bGwsXCJsZW5ndGhcIjpcIlwiLFwiZGVmYXVsdFZhbHVlXCI6bnVsbCxcIlRhYmxlSURcIjoyNSxcIklzUEtJRFwiOmZhbHNlLFwib3JkZXJpZFwiOjMsXCJDaGFuZ2VkUHJvcGVydGllc1wiOltdLFwiQmFja3VwQ2hhbmdlZFByb3BlcnRpZXNcIjpbXX0se1wiaWRcIjoxNDYsXCJjYXB0aW9uXCI6XCLlj5Hooajml7bpl7RcIixcIk5hbWVcIjpcIlNlbmRUaW1lXCIsXCJJc0F1dG9JbmNyZW1lbnRcIjpmYWxzZSxcIkNhbk51bGxcIjp0cnVlLFwiZGJUeXBlXCI6XCJkYXRldGltZVwiLFwiVHlwZVwiOm51bGwsXCJFbnVtRGVmaW5lXCI6bnVsbCxcImxlbmd0aFwiOlwiXCIsXCJkZWZhdWx0VmFsdWVcIjpudWxsLFwiVGFibGVJRFwiOjI1LFwiSXNQS0lEXCI6ZmFsc2UsXCJvcmRlcmlkXCI6NCxcIkNoYW5nZWRQcm9wZXJ0aWVzXCI6W10sXCJCYWNrdXBDaGFuZ2VkUHJvcGVydGllc1wiOltdfV0sXCJJRFhDb25maWdzXCI6W10sXCJJRFwiOjB9In0seyJOYW1lIjoiZGF0YWJhc2VpZCIsIlZhbHVlIjoyfV19LHsiSXRlbXMiOlt7Ik5hbWUiOiJpZCIsIlZhbHVlIjoxNTN9LHsiTmFtZSI6InR5cGUiLCJWYWx1ZSI6IkVudGl0eURCLkRlc2lnbi5BY3Rpb25zLkNyZWF0ZVRhYmxlQWN0aW9uIn0seyJOYW1lIjoiY29udGVudCIsIlZhbHVlIjoie1wiVGFibGVcIjp7XCJpZFwiOjI2LFwiY2FwdGlvblwiOlwiQnVn6ZmE5bim5oiq5Zu+XCIsXCJOYW1lXCI6XCJCdWdJbWFnZXNcIixcIkRhdGFiYXNlSURcIjoyLFwiaUxvY2tcIjowLFwiQ2hhbmdlZFByb3BlcnRpZXNcIjpbXSxcIkJhY2t1cENoYW5nZWRQcm9wZXJ0aWVzXCI6W119LFwiQ29sdW1uc1wiOlt7XCJpZFwiOjExNCxcImNhcHRpb25cIjpudWxsLFwiTmFtZVwiOlwiaWRcIixcIklzQXV0b0luY3JlbWVudFwiOnRydWUsXCJDYW5OdWxsXCI6ZmFsc2UsXCJkYlR5cGVcIjpcImludFwiLFwiVHlwZVwiOm51bGwsXCJFbnVtRGVmaW5lXCI6bnVsbCxcImxlbmd0aFwiOm51bGwsXCJkZWZhdWx0VmFsdWVcIjpudWxsLFwiVGFibGVJRFwiOjI2LFwiSXNQS0lEXCI6dHJ1ZSxcIm9yZGVyaWRcIjowLFwiQ2hhbmdlZFByb3BlcnRpZXNcIjpbXSxcIkJhY2t1cENoYW5nZWRQcm9wZXJ0aWVzXCI6W119LHtcImlkXCI6MTE1LFwiY2FwdGlvblwiOm51bGwsXCJOYW1lXCI6XCJCdWdJRFwiLFwiSXNBdXRvSW5jcmVtZW50XCI6ZmFsc2UsXCJDYW5OdWxsXCI6dHJ1ZSxcImRiVHlwZVwiOlwiaW50XCIsXCJUeXBlXCI6bnVsbCxcIkVudW1EZWZpbmVcIjpudWxsLFwibGVuZ3RoXCI6XCJcIixcImRlZmF1bHRWYWx1ZVwiOm51bGwsXCJUYWJsZUlEXCI6MjYsXCJJc1BLSURcIjpmYWxzZSxcIm9yZGVyaWRcIjoxLFwiQ2hhbmdlZFByb3BlcnRpZXNcIjpbXSxcIkJhY2t1cENoYW5nZWRQcm9wZXJ0aWVzXCI6W119LHtcImlkXCI6MTE2LFwiY2FwdGlvblwiOm51bGwsXCJOYW1lXCI6XCJjb250ZW50XCIsXCJJc0F1dG9JbmNyZW1lbnRcIjpmYWxzZSxcIkNhbk51bGxcIjp0cnVlLFwiZGJUeXBlXCI6XCJpbWFnZVwiLFwiVHlwZVwiOm51bGwsXCJFbnVtRGVmaW5lXCI6bnVsbCxcImxlbmd0aFwiOlwiXCIsXCJkZWZhdWx0VmFsdWVcIjpudWxsLFwiVGFibGVJRFwiOjI2LFwiSXNQS0lEXCI6ZmFsc2UsXCJvcmRlcmlkXCI6MixcIkNoYW5nZWRQcm9wZXJ0aWVzXCI6W10sXCJCYWNrdXBDaGFuZ2VkUHJvcGVydGllc1wiOltdfSx7XCJpZFwiOjExNyxcImNhcHRpb25cIjpcIuaOkuW6j1wiLFwiTmFtZVwiOlwib3JkZXJJRFwiLFwiSXNBdXRvSW5jcmVtZW50XCI6ZmFsc2UsXCJDYW5OdWxsXCI6dHJ1ZSxcImRiVHlwZVwiOlwiaW50XCIsXCJUeXBlXCI6bnVsbCxcIkVudW1EZWZpbmVcIjpudWxsLFwibGVuZ3RoXCI6XCJcIixcImRlZmF1bHRWYWx1ZVwiOm51bGwsXCJUYWJsZUlEXCI6MjYsXCJJc1BLSURcIjpmYWxzZSxcIm9yZGVyaWRcIjozLFwiQ2hhbmdlZFByb3BlcnRpZXNcIjpbXSxcIkJhY2t1cENoYW5nZWRQcm9wZXJ0aWVzXCI6W119XSxcIklEWENvbmZpZ3NcIjpbXSxcIklEXCI6MH0ifSx7Ik5hbWUiOiJkYXRhYmFzZWlkIiwiVmFsdWUiOjJ9XX0seyJJdGVtcyI6W3siTmFtZSI6ImlkIiwiVmFsdWUiOjE1NH0seyJOYW1lIjoidHlwZSIsIlZhbHVlIjoiRW50aXR5REIuRGVzaWduLkFjdGlvbnMuQ3JlYXRlVGFibGVBY3Rpb24ifSx7Ik5hbWUiOiJjb250ZW50IiwiVmFsdWUiOiJ7XCJUYWJsZVwiOntcImlkXCI6MjcsXCJjYXB0aW9uXCI6XCLlvJXlhaXnmoRkbGxcIixcIk5hbWVcIjpcIkRMTEltcG9ydFwiLFwiRGF0YWJhc2VJRFwiOjIsXCJpTG9ja1wiOjAsXCJDaGFuZ2VkUHJvcGVydGllc1wiOltdLFwiQmFja3VwQ2hhbmdlZFByb3BlcnRpZXNcIjpbXX0sXCJDb2x1bW5zXCI6W3tcImlkXCI6MTE5LFwiY2FwdGlvblwiOm51bGwsXCJOYW1lXCI6XCJpZFwiLFwiSXNBdXRvSW5jcmVtZW50XCI6dHJ1ZSxcIkNhbk51bGxcIjpmYWxzZSxcImRiVHlwZVwiOlwiaW50XCIsXCJUeXBlXCI6bnVsbCxcIkVudW1EZWZpbmVcIjpudWxsLFwibGVuZ3RoXCI6bnVsbCxcImRlZmF1bHRWYWx1ZVwiOm51bGwsXCJUYWJsZUlEXCI6MjcsXCJJc1BLSURcIjp0cnVlLFwib3JkZXJpZFwiOjAsXCJDaGFuZ2VkUHJvcGVydGllc1wiOltdLFwiQmFja3VwQ2hhbmdlZFByb3BlcnRpZXNcIjpbXX0se1wiaWRcIjoxMjAsXCJjYXB0aW9uXCI6XCJkbGzmlofku7bot6/lvoRcIixcIk5hbWVcIjpcInBhdGhcIixcIklzQXV0b0luY3JlbWVudFwiOmZhbHNlLFwiQ2FuTnVsbFwiOnRydWUsXCJkYlR5cGVcIjpcInZhcmNoYXJcIixcIlR5cGVcIjpudWxsLFwiRW51bURlZmluZVwiOm51bGwsXCJsZW5ndGhcIjpcIjIwMFwiLFwiZGVmYXVsdFZhbHVlXCI6bnVsbCxcIlRhYmxlSURcIjoyNyxcIklzUEtJRFwiOmZhbHNlLFwib3JkZXJpZFwiOjEsXCJDaGFuZ2VkUHJvcGVydGllc1wiOltdLFwiQmFja3VwQ2hhbmdlZFByb3BlcnRpZXNcIjpbXX0se1wiaWRcIjoxMjEsXCJjYXB0aW9uXCI6bnVsbCxcIk5hbWVcIjpcIlByb2plY3RJRFwiLFwiSXNBdXRvSW5jcmVtZW50XCI6ZmFsc2UsXCJDYW5OdWxsXCI6dHJ1ZSxcImRiVHlwZVwiOlwiaW50XCIsXCJUeXBlXCI6bnVsbCxcIkVudW1EZWZpbmVcIjpudWxsLFwibGVuZ3RoXCI6XCJcIixcImRlZmF1bHRWYWx1ZVwiOm51bGwsXCJUYWJsZUlEXCI6MjcsXCJJc1BLSURcIjpmYWxzZSxcIm9yZGVyaWRcIjoyLFwiQ2hhbmdlZFByb3BlcnRpZXNcIjpbXSxcIkJhY2t1cENoYW5nZWRQcm9wZXJ0aWVzXCI6W119XSxcIklEWENvbmZpZ3NcIjpbXSxcIklEXCI6MH0ifSx7Ik5hbWUiOiJkYXRhYmFzZWlkIiwiVmFsdWUiOjJ9XX0seyJJdGVtcyI6W3siTmFtZSI6ImlkIiwiVmFsdWUiOjE1NX0seyJOYW1lIjoidHlwZSIsIlZhbHVlIjoiRW50aXR5REIuRGVzaWduLkFjdGlvbnMuQ3JlYXRlVGFibGVBY3Rpb24ifSx7Ik5hbWUiOiJjb250ZW50IiwiVmFsdWUiOiJ7XCJUYWJsZVwiOntcImlkXCI6MjgsXCJjYXB0aW9uXCI6XCLmjqXlj6Porr7orqHnmoTnm67lvZXnu5PmnoRcIixcIk5hbWVcIjpcIkludGVyZmFjZU1vZHVsZVwiLFwiRGF0YWJhc2VJRFwiOjIsXCJpTG9ja1wiOjAsXCJDaGFuZ2VkUHJvcGVydGllc1wiOltdLFwiQmFja3VwQ2hhbmdlZFByb3BlcnRpZXNcIjpbXX0sXCJDb2x1bW5zXCI6W3tcImlkXCI6MTIyLFwiY2FwdGlvblwiOm51bGwsXCJOYW1lXCI6XCJpZFwiLFwiSXNBdXRvSW5jcmVtZW50XCI6dHJ1ZSxcIkNhbk51bGxcIjpmYWxzZSxcImRiVHlwZVwiOlwiaW50XCIsXCJUeXBlXCI6bnVsbCxcIkVudW1EZWZpbmVcIjpudWxsLFwibGVuZ3RoXCI6bnVsbCxcImRlZmF1bHRWYWx1ZVwiOm51bGwsXCJUYWJsZUlEXCI6MjgsXCJJc1BLSURcIjp0cnVlLFwib3JkZXJpZFwiOjAsXCJDaGFuZ2VkUHJvcGVydGllc1wiOltdLFwiQmFja3VwQ2hhbmdlZFByb3BlcnRpZXNcIjpbXX0se1wiaWRcIjoxMjMsXCJjYXB0aW9uXCI6bnVsbCxcIk5hbWVcIjpcIlByb2plY3RJRFwiLFwiSXNBdXRvSW5jcmVtZW50XCI6ZmFsc2UsXCJDYW5OdWxsXCI6dHJ1ZSxcImRiVHlwZVwiOlwiaW50XCIsXCJUeXBlXCI6bnVsbCxcIkVudW1EZWZpbmVcIjpudWxsLFwibGVuZ3RoXCI6XCJcIixcImRlZmF1bHRWYWx1ZVwiOm51bGwsXCJUYWJsZUlEXCI6MjgsXCJJc1BLSURcIjpmYWxzZSxcIm9yZGVyaWRcIjoxLFwiQ2hhbmdlZFByb3BlcnRpZXNcIjpbXSxcIkJhY2t1cENoYW5nZWRQcm9wZXJ0aWVzXCI6W119LHtcImlkXCI6MTI0LFwiY2FwdGlvblwiOm51bGwsXCJOYW1lXCI6XCJOYW1lXCIsXCJJc0F1dG9JbmNyZW1lbnRcIjpmYWxzZSxcIkNhbk51bGxcIjp0cnVlLFwiZGJUeXBlXCI6XCJ2YXJjaGFyXCIsXCJUeXBlXCI6bnVsbCxcIkVudW1EZWZpbmVcIjpudWxsLFwibGVuZ3RoXCI6XCI1MFwiLFwiZGVmYXVsdFZhbHVlXCI6bnVsbCxcIlRhYmxlSURcIjoyOCxcIklzUEtJRFwiOmZhbHNlLFwib3JkZXJpZFwiOjIsXCJDaGFuZ2VkUHJvcGVydGllc1wiOltdLFwiQmFja3VwQ2hhbmdlZFByb3BlcnRpZXNcIjpbXX0se1wiaWRcIjoxMjUsXCJjYXB0aW9uXCI6bnVsbCxcIk5hbWVcIjpcIlBhcmVudElEXCIsXCJJc0F1dG9JbmNyZW1lbnRcIjpmYWxzZSxcIkNhbk51bGxcIjp0cnVlLFwiZGJUeXBlXCI6XCJpbnRcIixcIlR5cGVcIjpudWxsLFwiRW51bURlZmluZVwiOm51bGwsXCJsZW5ndGhcIjpcIlwiLFwiZGVmYXVsdFZhbHVlXCI6XCIwXCIsXCJUYWJsZUlEXCI6MjgsXCJJc1BLSURcIjpmYWxzZSxcIm9yZGVyaWRcIjozLFwiQ2hhbmdlZFByb3BlcnRpZXNcIjpbXSxcIkJhY2t1cENoYW5nZWRQcm9wZXJ0aWVzXCI6W119LHtcImlkXCI6MTMyLFwiY2FwdGlvblwiOm51bGwsXCJOYW1lXCI6XCJJc0ZvbGRlclwiLFwiSXNBdXRvSW5jcmVtZW50XCI6ZmFsc2UsXCJDYW5OdWxsXCI6dHJ1ZSxcImRiVHlwZVwiOlwiYml0XCIsXCJUeXBlXCI6bnVsbCxcIkVudW1EZWZpbmVcIjpudWxsLFwibGVuZ3RoXCI6XCJcIixcImRlZmF1bHRWYWx1ZVwiOlwiMFwiLFwiVGFibGVJRFwiOjI4LFwiSXNQS0lEXCI6ZmFsc2UsXCJvcmRlcmlkXCI6NCxcIkNoYW5nZWRQcm9wZXJ0aWVzXCI6W10sXCJCYWNrdXBDaGFuZ2VkUHJvcGVydGllc1wiOltdfSx7XCJpZFwiOjEzOCxcImNhcHRpb25cIjpcIuW3sue7j+iiq+afkOS6uumUgeWumlwiLFwiTmFtZVwiOlwiTG9ja1VzZXJJZFwiLFwiSXNBdXRvSW5jcmVtZW50XCI6ZmFsc2UsXCJDYW5OdWxsXCI6dHJ1ZSxcImRiVHlwZVwiOlwiaW50XCIsXCJUeXBlXCI6bnVsbCxcIkVudW1EZWZpbmVcIjpudWxsLFwibGVuZ3RoXCI6XCJcIixcImRlZmF1bHRWYWx1ZVwiOm51bGwsXCJUYWJsZUlEXCI6MjgsXCJJc1BLSURcIjpmYWxzZSxcIm9yZGVyaWRcIjo1LFwiQ2hhbmdlZFByb3BlcnRpZXNcIjpbXSxcIkJhY2t1cENoYW5nZWRQcm9wZXJ0aWVzXCI6W119XSxcIklEWENvbmZpZ3NcIjpbXSxcIklEXCI6MH0ifSx7Ik5hbWUiOiJkYXRhYmFzZWlkIiwiVmFsdWUiOjJ9XX0seyJJdGVtcyI6W3siTmFtZSI6ImlkIiwiVmFsdWUiOjE1Nn0seyJOYW1lIjoidHlwZSIsIlZhbHVlIjoiRW50aXR5REIuRGVzaWduLkFjdGlvbnMuQ3JlYXRlVGFibGVBY3Rpb24ifSx7Ik5hbWUiOiJjb250ZW50IiwiVmFsdWUiOiJ7XCJUYWJsZVwiOntcImlkXCI6MjksXCJjYXB0aW9uXCI6bnVsbCxcIk5hbWVcIjpcIkludGVyZmFjZUluTW9kdWxlXCIsXCJEYXRhYmFzZUlEXCI6MixcImlMb2NrXCI6MCxcIkNoYW5nZWRQcm9wZXJ0aWVzXCI6W10sXCJCYWNrdXBDaGFuZ2VkUHJvcGVydGllc1wiOltdfSxcIkNvbHVtbnNcIjpbe1wiaWRcIjoxMjYsXCJjYXB0aW9uXCI6bnVsbCxcIk5hbWVcIjpcImlkXCIsXCJJc0F1dG9JbmNyZW1lbnRcIjp0cnVlLFwiQ2FuTnVsbFwiOmZhbHNlLFwiZGJUeXBlXCI6XCJpbnRcIixcIlR5cGVcIjpudWxsLFwiRW51bURlZmluZVwiOm51bGwsXCJsZW5ndGhcIjpudWxsLFwiZGVmYXVsdFZhbHVlXCI6bnVsbCxcIlRhYmxlSURcIjoyOSxcIklzUEtJRFwiOnRydWUsXCJvcmRlcmlkXCI6MCxcIkNoYW5nZWRQcm9wZXJ0aWVzXCI6W10sXCJCYWNrdXBDaGFuZ2VkUHJvcGVydGllc1wiOltdfSx7XCJpZFwiOjEyNyxcImNhcHRpb25cIjpudWxsLFwiTmFtZVwiOlwiTW9kdWxlSURcIixcIklzQXV0b0luY3JlbWVudFwiOmZhbHNlLFwiQ2FuTnVsbFwiOnRydWUsXCJkYlR5cGVcIjpcImludFwiLFwiVHlwZVwiOm51bGwsXCJFbnVtRGVmaW5lXCI6bnVsbCxcImxlbmd0aFwiOlwiXCIsXCJkZWZhdWx0VmFsdWVcIjpudWxsLFwiVGFibGVJRFwiOjI5LFwiSXNQS0lEXCI6ZmFsc2UsXCJvcmRlcmlkXCI6MSxcIkNoYW5nZWRQcm9wZXJ0aWVzXCI6W10sXCJCYWNrdXBDaGFuZ2VkUHJvcGVydGllc1wiOltdfSx7XCJpZFwiOjEyOCxcImNhcHRpb25cIjpudWxsLFwiTmFtZVwiOlwieFwiLFwiSXNBdXRvSW5jcmVtZW50XCI6ZmFsc2UsXCJDYW5OdWxsXCI6dHJ1ZSxcImRiVHlwZVwiOlwiaW50XCIsXCJUeXBlXCI6bnVsbCxcIkVudW1EZWZpbmVcIjpudWxsLFwibGVuZ3RoXCI6XCJcIixcImRlZmF1bHRWYWx1ZVwiOm51bGwsXCJUYWJsZUlEXCI6MjksXCJJc1BLSURcIjpmYWxzZSxcIm9yZGVyaWRcIjoyLFwiQ2hhbmdlZFByb3BlcnRpZXNcIjpbXSxcIkJhY2t1cENoYW5nZWRQcm9wZXJ0aWVzXCI6W119LHtcImlkXCI6MTI5LFwiY2FwdGlvblwiOm51bGwsXCJOYW1lXCI6XCJ5XCIsXCJJc0F1dG9JbmNyZW1lbnRcIjpmYWxzZSxcIkNhbk51bGxcIjp0cnVlLFwiZGJUeXBlXCI6XCJpbnRcIixcIlR5cGVcIjpudWxsLFwiRW51bURlZmluZVwiOm51bGwsXCJsZW5ndGhcIjpcIlwiLFwiZGVmYXVsdFZhbHVlXCI6bnVsbCxcIlRhYmxlSURcIjoyOSxcIklzUEtJRFwiOmZhbHNlLFwib3JkZXJpZFwiOjMsXCJDaGFuZ2VkUHJvcGVydGllc1wiOltdLFwiQmFja3VwQ2hhbmdlZFByb3BlcnRpZXNcIjpbXX0se1wiaWRcIjoxMzAsXCJjYXB0aW9uXCI6bnVsbCxcIk5hbWVcIjpcIlR5cGVcIixcIklzQXV0b0luY3JlbWVudFwiOmZhbHNlLFwiQ2FuTnVsbFwiOnRydWUsXCJkYlR5cGVcIjpcInZhcmNoYXJcIixcIlR5cGVcIjpudWxsLFwiRW51bURlZmluZVwiOm51bGwsXCJsZW5ndGhcIjpcIjEwMFwiLFwiZGVmYXVsdFZhbHVlXCI6bnVsbCxcIlRhYmxlSURcIjoyOSxcIklzUEtJRFwiOmZhbHNlLFwib3JkZXJpZFwiOjQsXCJDaGFuZ2VkUHJvcGVydGllc1wiOltdLFwiQmFja3VwQ2hhbmdlZFByb3BlcnRpZXNcIjpbXX0se1wiaWRcIjoxMzEsXCJjYXB0aW9uXCI6bnVsbCxcIk5hbWVcIjpcIkpzb25EYXRhXCIsXCJJc0F1dG9JbmNyZW1lbnRcIjpmYWxzZSxcIkNhbk51bGxcIjp0cnVlLFwiZGJUeXBlXCI6XCJ0ZXh0XCIsXCJUeXBlXCI6bnVsbCxcIkVudW1EZWZpbmVcIjpudWxsLFwibGVuZ3RoXCI6XCJcIixcImRlZmF1bHRWYWx1ZVwiOm51bGwsXCJUYWJsZUlEXCI6MjksXCJJc1BLSURcIjpmYWxzZSxcIm9yZGVyaWRcIjo1LFwiQ2hhbmdlZFByb3BlcnRpZXNcIjpbXSxcIkJhY2t1cENoYW5nZWRQcm9wZXJ0aWVzXCI6W119LHtcImlkXCI6MTMzLFwiY2FwdGlvblwiOm51bGwsXCJOYW1lXCI6XCJ3aWR0aFwiLFwiSXNBdXRvSW5jcmVtZW50XCI6ZmFsc2UsXCJDYW5OdWxsXCI6dHJ1ZSxcImRiVHlwZVwiOlwiaW50XCIsXCJUeXBlXCI6bnVsbCxcIkVudW1EZWZpbmVcIjpudWxsLFwibGVuZ3RoXCI6XCJcIixcImRlZmF1bHRWYWx1ZVwiOm51bGwsXCJUYWJsZUlEXCI6MjksXCJJc1BLSURcIjpmYWxzZSxcIm9yZGVyaWRcIjo2LFwiQ2hhbmdlZFByb3BlcnRpZXNcIjpbXSxcIkJhY2t1cENoYW5nZWRQcm9wZXJ0aWVzXCI6W119LHtcImlkXCI6MTM0LFwiY2FwdGlvblwiOm51bGwsXCJOYW1lXCI6XCJoZWlnaHRcIixcIklzQXV0b0luY3JlbWVudFwiOmZhbHNlLFwiQ2FuTnVsbFwiOnRydWUsXCJkYlR5cGVcIjpcImludFwiLFwiVHlwZVwiOm51bGwsXCJFbnVtRGVmaW5lXCI6bnVsbCxcImxlbmd0aFwiOlwiXCIsXCJkZWZhdWx0VmFsdWVcIjpudWxsLFwiVGFibGVJRFwiOjI5LFwiSXNQS0lEXCI6ZmFsc2UsXCJvcmRlcmlkXCI6NyxcIkNoYW5nZWRQcm9wZXJ0aWVzXCI6W10sXCJCYWNrdXBDaGFuZ2VkUHJvcGVydGllc1wiOltdfV0sXCJJRFhDb25maWdzXCI6W10sXCJJRFwiOjB9In0seyJOYW1lIjoiZGF0YWJhc2VpZCIsIlZhbHVlIjoyfV19LHsiSXRlbXMiOlt7Ik5hbWUiOiJpZCIsIlZhbHVlIjoxNTd9LHsiTmFtZSI6InR5cGUiLCJWYWx1ZSI6IkVudGl0eURCLkRlc2lnbi5BY3Rpb25zLkNyZWF0ZVRhYmxlQWN0aW9uIn0seyJOYW1lIjoiY29udGVudCIsIlZhbHVlIjoie1wiVGFibGVcIjp7XCJpZFwiOjMwLFwiY2FwdGlvblwiOlwiSW50ZXJmYWNlTW9kdWxl5p2D6ZmQ6K6+5a6a6KGoXCIsXCJOYW1lXCI6XCJJbnRlcmZhY2VNb2R1bGVQb3dlclwiLFwiRGF0YWJhc2VJRFwiOjIsXCJpTG9ja1wiOjAsXCJDaGFuZ2VkUHJvcGVydGllc1wiOltdLFwiQmFja3VwQ2hhbmdlZFByb3BlcnRpZXNcIjpbXX0sXCJDb2x1bW5zXCI6W3tcImlkXCI6MTM1LFwiY2FwdGlvblwiOm51bGwsXCJOYW1lXCI6XCJpZFwiLFwiSXNBdXRvSW5jcmVtZW50XCI6dHJ1ZSxcIkNhbk51bGxcIjpmYWxzZSxcImRiVHlwZVwiOlwiaW50XCIsXCJUeXBlXCI6bnVsbCxcIkVudW1EZWZpbmVcIjpudWxsLFwibGVuZ3RoXCI6bnVsbCxcImRlZmF1bHRWYWx1ZVwiOm51bGwsXCJUYWJsZUlEXCI6MzAsXCJJc1BLSURcIjp0cnVlLFwib3JkZXJpZFwiOjAsXCJDaGFuZ2VkUHJvcGVydGllc1wiOltdLFwiQmFja3VwQ2hhbmdlZFByb3BlcnRpZXNcIjpbXX0se1wiaWRcIjoxMzYsXCJjYXB0aW9uXCI6bnVsbCxcIk5hbWVcIjpcIlVzZXJJRFwiLFwiSXNBdXRvSW5jcmVtZW50XCI6ZmFsc2UsXCJDYW5OdWxsXCI6dHJ1ZSxcImRiVHlwZVwiOlwiaW50XCIsXCJUeXBlXCI6bnVsbCxcIkVudW1EZWZpbmVcIjpudWxsLFwibGVuZ3RoXCI6XCJcIixcImRlZmF1bHRWYWx1ZVwiOm51bGwsXCJUYWJsZUlEXCI6MzAsXCJJc1BLSURcIjpmYWxzZSxcIm9yZGVyaWRcIjoxLFwiQ2hhbmdlZFByb3BlcnRpZXNcIjpbXSxcIkJhY2t1cENoYW5nZWRQcm9wZXJ0aWVzXCI6W119LHtcImlkXCI6MTM3LFwiY2FwdGlvblwiOm51bGwsXCJOYW1lXCI6XCJNb2R1bGVJRFwiLFwiSXNBdXRvSW5jcmVtZW50XCI6ZmFsc2UsXCJDYW5OdWxsXCI6dHJ1ZSxcImRiVHlwZVwiOlwiaW50XCIsXCJUeXBlXCI6bnVsbCxcIkVudW1EZWZpbmVcIjpudWxsLFwibGVuZ3RoXCI6XCJcIixcImRlZmF1bHRWYWx1ZVwiOm51bGwsXCJUYWJsZUlEXCI6MzAsXCJJc1BLSURcIjpmYWxzZSxcIm9yZGVyaWRcIjoyLFwiQ2hhbmdlZFByb3BlcnRpZXNcIjpbXSxcIkJhY2t1cENoYW5nZWRQcm9wZXJ0aWVzXCI6W119XSxcIklEWENvbmZpZ3NcIjpbXSxcIklEXCI6MH0ifSx7Ik5hbWUiOiJkYXRhYmFzZWlkIiwiVmFsdWUiOjJ9XX1dLCJDb2x1bW5zIjpbeyJDb2x1bW5OYW1lIjoiaWQiLCJEYXRhVHlwZSI6IlN5c3RlbS5JbnQ2NCwgbXNjb3JsaWIsIFZlcnNpb249NC4wLjAuMCwgQ3VsdHVyZT1uZXV0cmFsLCBQdWJsaWNLZXlUb2tlbj1iNzdhNWM1NjE5MzRlMDg5In0seyJDb2x1bW5OYW1lIjoidHlwZSIsIkRhdGFUeXBlIjoiU3lzdGVtLlN0cmluZywgbXNjb3JsaWIsIFZlcnNpb249NC4wLjAuMCwgQ3VsdHVyZT1uZXV0cmFsLCBQdWJsaWNLZXlUb2tlbj1iNzdhNWM1NjE5MzRlMDg5In0seyJDb2x1bW5OYW1lIjoiY29udGVudCIsIkRhdGFUeXBlIjoiU3lzdGVtLlN0cmluZywgbXNjb3JsaWIsIFZlcnNpb249NC4wLjAuMCwgQ3VsdHVyZT1uZXV0cmFsLCBQdWJsaWNLZXlUb2tlbj1iNzdhNWM1NjE5MzRlMDg5In0seyJDb2x1bW5OYW1lIjoiZGF0YWJhc2VpZCIsIkRhdGFUeXBlIjoiU3lzdGVtLkludDY0LCBtc2NvcmxpYiwgVmVyc2lvbj00LjAuMC4wLCBDdWx0dXJlPW5ldXRyYWwsIFB1YmxpY0tleVRva2VuPWI3N2E1YzU2MTkzNGUwODkifV19XSwiRGF0YVNldE5hbWUiOiI3RjRDNEI2MC01NkJCLTRCODItQTkzMS05MzAwQjQwNzVBNzYifQ==";}}
