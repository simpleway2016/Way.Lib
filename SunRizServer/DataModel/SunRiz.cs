
using System;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;

namespace SunRizServer{


    /// <summary>
	/// 图片文件
	/// </summary>
    [System.ComponentModel.DataAnnotations.Schema.Table("imagefiles")]
    [Way.EntityDB.Attributes.Table("id")]
    public class ImageFiles :Way.EntityDB.DataItem
    {

        /// <summary>
	    /// 
	    /// </summary>
        public  ImageFiles()
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

        String _Name;
        /// <summary>
        /// 显示的名称
        /// </summary>
        [System.ComponentModel.DataAnnotations.Schema.Column("name")]
        [Way.EntityDB.WayDBColumnAttribute(Name="name",Comment="",Caption="显示的名称",Storage = "_Name",DbType="varchar(50)")]
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

        System.Nullable<Int32> _ParentId=0;
        /// <summary>
        /// 
        /// </summary>
        [System.ComponentModel.DataAnnotations.Schema.Column("parentid")]
        [Way.EntityDB.WayDBColumnAttribute(Name="parentid",Comment="",Caption="",Storage = "_ParentId",DbType="int")]
        public virtual System.Nullable<Int32> ParentId
        {
            get
            {
                return this._ParentId;
            }
            set
            {
                if ((this._ParentId != value))
                {
                    this.SendPropertyChanging("ParentId",this._ParentId,value);
                    this._ParentId = value;
                    this.SendPropertyChanged("ParentId");

                }
            }
        }

        System.Nullable<Boolean> _IsFolder=false;
        /// <summary>
        /// 
        /// </summary>
        [System.ComponentModel.DataAnnotations.Schema.Column("isfolder")]
        [Way.EntityDB.WayDBColumnAttribute(Name="isfolder",Comment="",Caption="",Storage = "_IsFolder",DbType="bit")]
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
                    this.SendPropertyChanging("IsFolder",this._IsFolder,value);
                    this._IsFolder = value;
                    this.SendPropertyChanged("IsFolder");

                }
            }
        }

        String _FileName;
        /// <summary>
        /// 文件实际文件名
        /// </summary>
        [System.ComponentModel.DataAnnotations.Schema.Column("filename")]
        [Way.EntityDB.WayDBColumnAttribute(Name="filename",Comment="",Caption="文件实际文件名",Storage = "_FileName",DbType="varchar(50)")]
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

        [System.ComponentModel.DataAnnotations.Schema.ForeignKey("ParentId")]
        public virtual ImageFiles Parent { get; set; }
}}
namespace SunRizServer{

/// <summary>
/// 
/// </summary>
public enum CommunicationDriver_StatusEnum:int
{
    

/// <summary>
/// 
/// </summary>
None = 0,

/// <summary>
/// 
/// </summary>

Online = 1,

/// <summary>
/// 
/// </summary>

Offline = 2,
}


    /// <summary>
	/// 通讯驱动列表
	/// </summary>
    [System.ComponentModel.DataAnnotations.Schema.Table("communicationdriver")]
    [Way.EntityDB.Attributes.Table("id")]
    public class CommunicationDriver :Way.EntityDB.DataItem
    {

        /// <summary>
	    /// 
	    /// </summary>
        public  CommunicationDriver()
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

        String _Name;
        /// <summary>
        /// 名称
        /// </summary>
        [System.ComponentModel.DataAnnotations.Schema.Column("name")]
        [Way.EntityDB.WayDBColumnAttribute(Name="name",Comment="",Caption="名称",Storage = "_Name",DbType="varchar(50)")]
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

        String _Address;
        /// <summary>
        /// 地址
        /// </summary>
        [System.ComponentModel.DataAnnotations.Schema.Column("address")]
        [Way.EntityDB.WayDBColumnAttribute(Name="address",Comment="",Caption="地址",Storage = "_Address",DbType="varchar(50)")]
        public virtual String Address
        {
            get
            {
                return this._Address;
            }
            set
            {
                if ((this._Address != value))
                {
                    this.SendPropertyChanging("Address",this._Address,value);
                    this._Address = value;
                    this.SendPropertyChanged("Address");

                }
            }
        }

        System.Nullable<Int32> _Port;
        /// <summary>
        /// 端口
        /// </summary>
        [System.ComponentModel.DataAnnotations.Schema.Column("port")]
        [Way.EntityDB.WayDBColumnAttribute(Name="port",Comment="",Caption="端口",Storage = "_Port",DbType="int")]
        public virtual System.Nullable<Int32> Port
        {
            get
            {
                return this._Port;
            }
            set
            {
                if ((this._Port != value))
                {
                    this.SendPropertyChanging("Port",this._Port,value);
                    this._Port = value;
                    this.SendPropertyChanged("Port");

                }
            }
        }

        System.Nullable<CommunicationDriver_StatusEnum> _Status=(System.Nullable<CommunicationDriver_StatusEnum>)(0);
        /// <summary>
        /// 状态
        /// </summary>
        [System.ComponentModel.DataAnnotations.Schema.Column("status")]
        [Way.EntityDB.WayDBColumnAttribute(Name="status",Comment="",Caption="状态",Storage = "_Status",DbType="int")]
        public virtual System.Nullable<CommunicationDriver_StatusEnum> Status
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

        System.Nullable<Boolean> _SupportEnumDevice=false;
        /// <summary>
        /// 
        /// </summary>
        [System.ComponentModel.DataAnnotations.Schema.Column("supportenumdevice")]
        [Way.EntityDB.WayDBColumnAttribute(Name="supportenumdevice",Comment="",Caption="",Storage = "_SupportEnumDevice",DbType="bit")]
        public virtual System.Nullable<Boolean> SupportEnumDevice
        {
            get
            {
                return this._SupportEnumDevice;
            }
            set
            {
                if ((this._SupportEnumDevice != value))
                {
                    this.SendPropertyChanging("SupportEnumDevice",this._SupportEnumDevice,value);
                    this._SupportEnumDevice = value;
                    this.SendPropertyChanged("SupportEnumDevice");

                }
            }
        }

        System.Nullable<Boolean> _SupportEnumPoints=false;
        /// <summary>
        /// 
        /// </summary>
        [System.ComponentModel.DataAnnotations.Schema.Column("supportenumpoints")]
        [Way.EntityDB.WayDBColumnAttribute(Name="supportenumpoints",Comment="",Caption="",Storage = "_SupportEnumPoints",DbType="bit")]
        public virtual System.Nullable<Boolean> SupportEnumPoints
        {
            get
            {
                return this._SupportEnumPoints;
            }
            set
            {
                if ((this._SupportEnumPoints != value))
                {
                    this.SendPropertyChanging("SupportEnumPoints",this._SupportEnumPoints,value);
                    this._SupportEnumPoints = value;
                    this.SendPropertyChanged("SupportEnumPoints");

                }
            }
        }
}}
namespace SunRizServer{


    /// <summary>
	/// 设备信息
	/// </summary>
    [System.ComponentModel.DataAnnotations.Schema.Table("device")]
    [Way.EntityDB.Attributes.Table("id")]
    public class Device :Way.EntityDB.DataItem
    {

        /// <summary>
	    /// 
	    /// </summary>
        public  Device()
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

        System.Nullable<Int32> _DriverID;
        /// <summary>
        /// 通讯网关
        /// </summary>
        [System.ComponentModel.DataAnnotations.Schema.Column("driverid")]
        [Way.EntityDB.WayDBColumnAttribute(Name="driverid",Comment="",Caption="通讯网关",Storage = "_DriverID",DbType="int")]
        public virtual System.Nullable<Int32> DriverID
        {
            get
            {
                return this._DriverID;
            }
            set
            {
                if ((this._DriverID != value))
                {
                    this.SendPropertyChanging("DriverID",this._DriverID,value);
                    this._DriverID = value;
                    this.SendPropertyChanged("DriverID");

                }
            }
        }

        String _Address;
        /// <summary>
        /// 地址信息
        /// </summary>
        [System.ComponentModel.DataAnnotations.Schema.Column("address")]
        [Way.EntityDB.WayDBColumnAttribute(Name="address",Comment="",Caption="地址信息",Storage = "_Address",DbType="varchar(100)")]
        public virtual String Address
        {
            get
            {
                return this._Address;
            }
            set
            {
                if ((this._Address != value))
                {
                    this.SendPropertyChanging("Address",this._Address,value);
                    this._Address = value;
                    this.SendPropertyChanged("Address");

                }
            }
        }

        String _AddrSetting;
        /// <summary>
        /// 地址设置时的json对象表达
        /// </summary>
        [System.ComponentModel.DataAnnotations.Schema.Column("addrsetting")]
        [Way.EntityDB.WayDBColumnAttribute(Name="addrsetting",Comment="",Caption="地址设置时的json对象表达",Storage = "_AddrSetting",DbType="varchar(200)")]
        public virtual String AddrSetting
        {
            get
            {
                return this._AddrSetting;
            }
            set
            {
                if ((this._AddrSetting != value))
                {
                    this.SendPropertyChanging("AddrSetting",this._AddrSetting,value);
                    this._AddrSetting = value;
                    this.SendPropertyChanged("AddrSetting");

                }
            }
        }

        String _Name;
        /// <summary>
        /// 设备名称
        /// </summary>
        [System.ComponentModel.DataAnnotations.Schema.Column("name")]
        [Way.EntityDB.WayDBColumnAttribute(Name="name",Comment="",Caption="设备名称",Storage = "_Name",DbType="varchar(50)")]
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

        System.Nullable<Int32> _UnitId;
        /// <summary>
        /// 控制单元id
        /// </summary>
        [System.ComponentModel.DataAnnotations.Schema.Column("unitid")]
        [Way.EntityDB.WayDBColumnAttribute(Name="unitid",Comment="",Caption="控制单元id",Storage = "_UnitId",DbType="int")]
        public virtual System.Nullable<Int32> UnitId
        {
            get
            {
                return this._UnitId;
            }
            set
            {
                if ((this._UnitId != value))
                {
                    this.SendPropertyChanging("UnitId",this._UnitId,value);
                    this._UnitId = value;
                    this.SendPropertyChanged("UnitId");

                }
            }
        }
}}
namespace SunRizServer{

/// <summary>
/// 
/// </summary>
public enum DevicePoint_TypeEnum:int
{
    

/// <summary>
/// 
/// </summary>
Analog = 1,

/// <summary>
/// 
/// </summary>

Digital = 2,
}


    /// <summary>
	/// 
	/// </summary>
    [System.ComponentModel.DataAnnotations.Schema.Table("devicepoint")]
    [Way.EntityDB.Attributes.Table("id")]
    public class DevicePoint :Way.EntityDB.DataItem
    {

        /// <summary>
	    /// 
	    /// </summary>
        public  DevicePoint()
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

        String _Name;
        /// <summary>
        /// 点名
        /// </summary>
        [System.ComponentModel.DataAnnotations.Schema.Column("name")]
        [Way.EntityDB.WayDBColumnAttribute(Name="name",Comment="",Caption="点名",Storage = "_Name",DbType="varchar(50)")]
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

        String _Address;
        /// <summary>
        /// 
        /// </summary>
        [System.ComponentModel.DataAnnotations.Schema.Column("address")]
        [Way.EntityDB.WayDBColumnAttribute(Name="address",Comment="",Caption="",Storage = "_Address",DbType="varchar(200)")]
        public virtual String Address
        {
            get
            {
                return this._Address;
            }
            set
            {
                if ((this._Address != value))
                {
                    this.SendPropertyChanging("Address",this._Address,value);
                    this._Address = value;
                    this.SendPropertyChanged("Address");

                }
            }
        }

        System.Nullable<Boolean> _IsWatching=false;
        /// <summary>
        /// 是否监测变化
        /// </summary>
        [System.ComponentModel.DataAnnotations.Schema.Column("iswatching")]
        [Way.EntityDB.WayDBColumnAttribute(Name="iswatching",Comment="",Caption="是否监测变化",Storage = "_IsWatching",DbType="bit")]
        public virtual System.Nullable<Boolean> IsWatching
        {
            get
            {
                return this._IsWatching;
            }
            set
            {
                if ((this._IsWatching != value))
                {
                    this.SendPropertyChanging("IsWatching",this._IsWatching,value);
                    this._IsWatching = value;
                    this.SendPropertyChanged("IsWatching");

                }
            }
        }

        String _AddrSetting;
        /// <summary>
        /// 地址设置时的json对象表达
        /// </summary>
        [System.ComponentModel.DataAnnotations.Schema.Column("addrsetting")]
        [Way.EntityDB.WayDBColumnAttribute(Name="addrsetting",Comment="",Caption="地址设置时的json对象表达",Storage = "_AddrSetting",DbType="varchar(200)")]
        public virtual String AddrSetting
        {
            get
            {
                return this._AddrSetting;
            }
            set
            {
                if ((this._AddrSetting != value))
                {
                    this.SendPropertyChanging("AddrSetting",this._AddrSetting,value);
                    this._AddrSetting = value;
                    this.SendPropertyChanged("AddrSetting");

                }
            }
        }

        String _Desc;
        /// <summary>
        /// 描述
        /// </summary>
        [System.ComponentModel.DataAnnotations.Schema.Column("desc")]
        [Way.EntityDB.WayDBColumnAttribute(Name="desc",Comment="",Caption="描述",Storage = "_Desc",DbType="varchar(100)")]
        public virtual String Desc
        {
            get
            {
                return this._Desc;
            }
            set
            {
                if ((this._Desc != value))
                {
                    this.SendPropertyChanging("Desc",this._Desc,value);
                    this._Desc = value;
                    this.SendPropertyChanged("Desc");

                }
            }
        }

        double _InitValue;
        /// <summary>
        /// 初始值
        /// </summary>
        [System.ComponentModel.DataAnnotations.Schema.Column("initvalue")]
        [Way.EntityDB.WayDBColumnAttribute(Name="initvalue",Comment="",Caption="初始值",Storage = "_InitValue",DbType="double")]
        public virtual double InitValue
        {
            get
            {
                return this._InitValue;
            }
            set
            {
                if ((this._InitValue != value))
                {
                    this.SendPropertyChanging("InitValue",this._InitValue,value);
                    this._InitValue = value;
                    this.SendPropertyChanged("InitValue");

                }
            }
        }

        String _StatusOpenInfo;
        /// <summary>
        /// 开状态信息
        /// </summary>
        [System.ComponentModel.DataAnnotations.Schema.Column("statusopeninfo")]
        [Way.EntityDB.WayDBColumnAttribute(Name="statusopeninfo",Comment="",Caption="开状态信息",Storage = "_StatusOpenInfo",DbType="varchar(50)")]
        public virtual String StatusOpenInfo
        {
            get
            {
                return this._StatusOpenInfo;
            }
            set
            {
                if ((this._StatusOpenInfo != value))
                {
                    this.SendPropertyChanging("StatusOpenInfo",this._StatusOpenInfo,value);
                    this._StatusOpenInfo = value;
                    this.SendPropertyChanged("StatusOpenInfo");

                }
            }
        }

        String _StateCloseInfo;
        /// <summary>
        /// 关状态信息
        /// </summary>
        [System.ComponentModel.DataAnnotations.Schema.Column("statecloseinfo")]
        [Way.EntityDB.WayDBColumnAttribute(Name="statecloseinfo",Comment="",Caption="关状态信息",Storage = "_StateCloseInfo",DbType="varchar(50)")]
        public virtual String StateCloseInfo
        {
            get
            {
                return this._StateCloseInfo;
            }
            set
            {
                if ((this._StateCloseInfo != value))
                {
                    this.SendPropertyChanging("StateCloseInfo",this._StateCloseInfo,value);
                    this._StateCloseInfo = value;
                    this.SendPropertyChanged("StateCloseInfo");

                }
            }
        }

        System.Nullable<Boolean> _IsAlarm=false;
        /// <summary>
        /// 报警开关
        /// </summary>
        [System.ComponentModel.DataAnnotations.Schema.Column("isalarm")]
        [Way.EntityDB.WayDBColumnAttribute(Name="isalarm",Comment="",Caption="报警开关",Storage = "_IsAlarm",DbType="bit")]
        public virtual System.Nullable<Boolean> IsAlarm
        {
            get
            {
                return this._IsAlarm;
            }
            set
            {
                if ((this._IsAlarm != value))
                {
                    this.SendPropertyChanging("IsAlarm",this._IsAlarm,value);
                    this._IsAlarm = value;
                    this.SendPropertyChanged("IsAlarm");

                }
            }
        }

        double _AlarmValue;
        /// <summary>
        /// 报警值
        /// </summary>
        [System.ComponentModel.DataAnnotations.Schema.Column("alarmvalue")]
        [Way.EntityDB.WayDBColumnAttribute(Name="alarmvalue",Comment="",Caption="报警值",Storage = "_AlarmValue",DbType="double")]
        public virtual double AlarmValue
        {
            get
            {
                return this._AlarmValue;
            }
            set
            {
                if ((this._AlarmValue != value))
                {
                    this.SendPropertyChanging("AlarmValue",this._AlarmValue,value);
                    this._AlarmValue = value;
                    this.SendPropertyChanged("AlarmValue");

                }
            }
        }

        System.Nullable<Int32> _AlarmPriority;
        /// <summary>
        /// 报警优先级
        /// </summary>
        [System.ComponentModel.DataAnnotations.Schema.Column("alarmpriority")]
        [Way.EntityDB.WayDBColumnAttribute(Name="alarmpriority",Comment="",Caption="报警优先级",Storage = "_AlarmPriority",DbType="int")]
        public virtual System.Nullable<Int32> AlarmPriority
        {
            get
            {
                return this._AlarmPriority;
            }
            set
            {
                if ((this._AlarmPriority != value))
                {
                    this.SendPropertyChanging("AlarmPriority",this._AlarmPriority,value);
                    this._AlarmPriority = value;
                    this.SendPropertyChanged("AlarmPriority");

                }
            }
        }

        String _AlarmGroup;
        /// <summary>
        /// 报警组
        /// </summary>
        [System.ComponentModel.DataAnnotations.Schema.Column("alarmgroup")]
        [Way.EntityDB.WayDBColumnAttribute(Name="alarmgroup",Comment="",Caption="报警组",Storage = "_AlarmGroup",DbType="varchar(50)")]
        public virtual String AlarmGroup
        {
            get
            {
                return this._AlarmGroup;
            }
            set
            {
                if ((this._AlarmGroup != value))
                {
                    this.SendPropertyChanging("AlarmGroup",this._AlarmGroup,value);
                    this._AlarmGroup = value;
                    this.SendPropertyChanged("AlarmGroup");

                }
            }
        }

        System.Nullable<Boolean> _AlarmAutoConfirm=false;
        /// <summary>
        /// 报警自动确认
        /// </summary>
        [System.ComponentModel.DataAnnotations.Schema.Column("alarmautoconfirm")]
        [Way.EntityDB.WayDBColumnAttribute(Name="alarmautoconfirm",Comment="",Caption="报警自动确认",Storage = "_AlarmAutoConfirm",DbType="bit")]
        public virtual System.Nullable<Boolean> AlarmAutoConfirm
        {
            get
            {
                return this._AlarmAutoConfirm;
            }
            set
            {
                if ((this._AlarmAutoConfirm != value))
                {
                    this.SendPropertyChanging("AlarmAutoConfirm",this._AlarmAutoConfirm,value);
                    this._AlarmAutoConfirm = value;
                    this.SendPropertyChanged("AlarmAutoConfirm");

                }
            }
        }

        System.Nullable<Boolean> _AlarmAutoReset=false;
        /// <summary>
        /// 报警自动复位
        /// </summary>
        [System.ComponentModel.DataAnnotations.Schema.Column("alarmautoreset")]
        [Way.EntityDB.WayDBColumnAttribute(Name="alarmautoreset",Comment="",Caption="报警自动复位",Storage = "_AlarmAutoReset",DbType="bit")]
        public virtual System.Nullable<Boolean> AlarmAutoReset
        {
            get
            {
                return this._AlarmAutoReset;
            }
            set
            {
                if ((this._AlarmAutoReset != value))
                {
                    this.SendPropertyChanging("AlarmAutoReset",this._AlarmAutoReset,value);
                    this._AlarmAutoReset = value;
                    this.SendPropertyChanged("AlarmAutoReset");

                }
            }
        }

        System.Nullable<Boolean> _ValueAbsoluteChange=false;
        /// <summary>
        /// 数据绝对变化保存
        /// </summary>
        [System.ComponentModel.DataAnnotations.Schema.Column("valueabsolutechange")]
        [Way.EntityDB.WayDBColumnAttribute(Name="valueabsolutechange",Comment="",Caption="数据绝对变化保存",Storage = "_ValueAbsoluteChange",DbType="bit")]
        public virtual System.Nullable<Boolean> ValueAbsoluteChange
        {
            get
            {
                return this._ValueAbsoluteChange;
            }
            set
            {
                if ((this._ValueAbsoluteChange != value))
                {
                    this.SendPropertyChanging("ValueAbsoluteChange",this._ValueAbsoluteChange,value);
                    this._ValueAbsoluteChange = value;
                    this.SendPropertyChanged("ValueAbsoluteChange");

                }
            }
        }

        double _ValueAbsoluteChangeSetting;
        /// <summary>
        /// 数据绝对变化值
        /// </summary>
        [System.ComponentModel.DataAnnotations.Schema.Column("valueabsolutechangesetting")]
        [Way.EntityDB.WayDBColumnAttribute(Name="valueabsolutechangesetting",Comment="",Caption="数据绝对变化值",Storage = "_ValueAbsoluteChangeSetting",DbType="double")]
        public virtual double ValueAbsoluteChangeSetting
        {
            get
            {
                return this._ValueAbsoluteChangeSetting;
            }
            set
            {
                if ((this._ValueAbsoluteChangeSetting != value))
                {
                    this.SendPropertyChanging("ValueAbsoluteChangeSetting",this._ValueAbsoluteChangeSetting,value);
                    this._ValueAbsoluteChangeSetting = value;
                    this.SendPropertyChanged("ValueAbsoluteChangeSetting");

                }
            }
        }

        System.Nullable<Boolean> _ValueRelativeChange=false;
        /// <summary>
        /// 数据相对变化保存
        /// </summary>
        [System.ComponentModel.DataAnnotations.Schema.Column("valuerelativechange")]
        [Way.EntityDB.WayDBColumnAttribute(Name="valuerelativechange",Comment="",Caption="数据相对变化保存",Storage = "_ValueRelativeChange",DbType="bit")]
        public virtual System.Nullable<Boolean> ValueRelativeChange
        {
            get
            {
                return this._ValueRelativeChange;
            }
            set
            {
                if ((this._ValueRelativeChange != value))
                {
                    this.SendPropertyChanging("ValueRelativeChange",this._ValueRelativeChange,value);
                    this._ValueRelativeChange = value;
                    this.SendPropertyChanged("ValueRelativeChange");

                }
            }
        }

        double _ValueRelativeChangeSetting;
        /// <summary>
        /// 数据相对变化值
        /// </summary>
        [System.ComponentModel.DataAnnotations.Schema.Column("valuerelativechangesetting")]
        [Way.EntityDB.WayDBColumnAttribute(Name="valuerelativechangesetting",Comment="",Caption="数据相对变化值",Storage = "_ValueRelativeChangeSetting",DbType="double")]
        public virtual double ValueRelativeChangeSetting
        {
            get
            {
                return this._ValueRelativeChangeSetting;
            }
            set
            {
                if ((this._ValueRelativeChangeSetting != value))
                {
                    this.SendPropertyChanging("ValueRelativeChangeSetting",this._ValueRelativeChangeSetting,value);
                    this._ValueRelativeChangeSetting = value;
                    this.SendPropertyChanged("ValueRelativeChangeSetting");

                }
            }
        }

        System.Nullable<Boolean> _ValueOnTimeChange=false;
        /// <summary>
        /// 数据定时保存
        /// </summary>
        [System.ComponentModel.DataAnnotations.Schema.Column("valueontimechange")]
        [Way.EntityDB.WayDBColumnAttribute(Name="valueontimechange",Comment="",Caption="数据定时保存",Storage = "_ValueOnTimeChange",DbType="bit")]
        public virtual System.Nullable<Boolean> ValueOnTimeChange
        {
            get
            {
                return this._ValueOnTimeChange;
            }
            set
            {
                if ((this._ValueOnTimeChange != value))
                {
                    this.SendPropertyChanging("ValueOnTimeChange",this._ValueOnTimeChange,value);
                    this._ValueOnTimeChange = value;
                    this.SendPropertyChanged("ValueOnTimeChange");

                }
            }
        }

        double _ValueOnTimeChangeSetting;
        /// <summary>
        /// 数据定时值
        /// </summary>
        [System.ComponentModel.DataAnnotations.Schema.Column("valueontimechangesetting")]
        [Way.EntityDB.WayDBColumnAttribute(Name="valueontimechangesetting",Comment="",Caption="数据定时值",Storage = "_ValueOnTimeChangeSetting",DbType="double")]
        public virtual double ValueOnTimeChangeSetting
        {
            get
            {
                return this._ValueOnTimeChangeSetting;
            }
            set
            {
                if ((this._ValueOnTimeChangeSetting != value))
                {
                    this.SendPropertyChanging("ValueOnTimeChangeSetting",this._ValueOnTimeChangeSetting,value);
                    this._ValueOnTimeChangeSetting = value;
                    this.SendPropertyChanged("ValueOnTimeChangeSetting");

                }
            }
        }

        System.Nullable<DevicePoint_TypeEnum> _Type=(System.Nullable<DevicePoint_TypeEnum>)(2);
        /// <summary>
        /// 点类型
        /// </summary>
        [System.ComponentModel.DataAnnotations.Schema.Column("type")]
        [Way.EntityDB.WayDBColumnAttribute(Name="type",Comment="",Caption="点类型",Storage = "_Type",DbType="int")]
        public virtual System.Nullable<DevicePoint_TypeEnum> Type
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

        String _Unit;
        /// <summary>
        /// 模拟量-工程单位
        /// </summary>
        [System.ComponentModel.DataAnnotations.Schema.Column("unit")]
        [Way.EntityDB.WayDBColumnAttribute(Name="unit",Comment="",Caption="模拟量-工程单位",Storage = "_Unit",DbType="varchar(50)")]
        public virtual String Unit
        {
            get
            {
                return this._Unit;
            }
            set
            {
                if ((this._Unit != value))
                {
                    this.SendPropertyChanging("Unit",this._Unit,value);
                    this._Unit = value;
                    this.SendPropertyChanged("Unit");

                }
            }
        }

        String _DPCount;
        /// <summary>
        /// 模拟量-小数点位数
        /// </summary>
        [System.ComponentModel.DataAnnotations.Schema.Column("dpcount")]
        [Way.EntityDB.WayDBColumnAttribute(Name="dpcount",Comment="",Caption="模拟量-小数点位数",Storage = "_DPCount",DbType="varchar(50)")]
        public virtual String DPCount
        {
            get
            {
                return this._DPCount;
            }
            set
            {
                if ((this._DPCount != value))
                {
                    this.SendPropertyChanging("DPCount",this._DPCount,value);
                    this._DPCount = value;
                    this.SendPropertyChanged("DPCount");

                }
            }
        }

        System.Nullable<Boolean> _IsTransform=false;
        /// <summary>
        /// 模拟量-量程转化开关
        /// </summary>
        [System.ComponentModel.DataAnnotations.Schema.Column("istransform")]
        [Way.EntityDB.WayDBColumnAttribute(Name="istransform",Comment="",Caption="模拟量-量程转化开关",Storage = "_IsTransform",DbType="bit")]
        public virtual System.Nullable<Boolean> IsTransform
        {
            get
            {
                return this._IsTransform;
            }
            set
            {
                if ((this._IsTransform != value))
                {
                    this.SendPropertyChanging("IsTransform",this._IsTransform,value);
                    this._IsTransform = value;
                    this.SendPropertyChanged("IsTransform");

                }
            }
        }

        double _TransMax;
        /// <summary>
        /// 模拟量-量程上限
        /// </summary>
        [System.ComponentModel.DataAnnotations.Schema.Column("transmax")]
        [Way.EntityDB.WayDBColumnAttribute(Name="transmax",Comment="",Caption="模拟量-量程上限",Storage = "_TransMax",DbType="double")]
        public virtual double TransMax
        {
            get
            {
                return this._TransMax;
            }
            set
            {
                if ((this._TransMax != value))
                {
                    this.SendPropertyChanging("TransMax",this._TransMax,value);
                    this._TransMax = value;
                    this.SendPropertyChanged("TransMax");

                }
            }
        }

        double _TransMin;
        /// <summary>
        /// 模拟量-量程下限
        /// </summary>
        [System.ComponentModel.DataAnnotations.Schema.Column("transmin")]
        [Way.EntityDB.WayDBColumnAttribute(Name="transmin",Comment="",Caption="模拟量-量程下限",Storage = "_TransMin",DbType="double")]
        public virtual double TransMin
        {
            get
            {
                return this._TransMin;
            }
            set
            {
                if ((this._TransMin != value))
                {
                    this.SendPropertyChanging("TransMin",this._TransMin,value);
                    this._TransMin = value;
                    this.SendPropertyChanged("TransMin");

                }
            }
        }

        double _SensorMax;
        /// <summary>
        /// 模拟量-传感器量程上限
        /// </summary>
        [System.ComponentModel.DataAnnotations.Schema.Column("sensormax")]
        [Way.EntityDB.WayDBColumnAttribute(Name="sensormax",Comment="",Caption="模拟量-传感器量程上限",Storage = "_SensorMax",DbType="double")]
        public virtual double SensorMax
        {
            get
            {
                return this._SensorMax;
            }
            set
            {
                if ((this._SensorMax != value))
                {
                    this.SendPropertyChanging("SensorMax",this._SensorMax,value);
                    this._SensorMax = value;
                    this.SendPropertyChanged("SensorMax");

                }
            }
        }

        String _SensorMin;
        /// <summary>
        /// 模拟量-传感器量程下限
        /// </summary>
        [System.ComponentModel.DataAnnotations.Schema.Column("sensormin")]
        [Way.EntityDB.WayDBColumnAttribute(Name="sensormin",Comment="",Caption="模拟量-传感器量程下限",Storage = "_SensorMin",DbType="varchar(50)")]
        public virtual String SensorMin
        {
            get
            {
                return this._SensorMin;
            }
            set
            {
                if ((this._SensorMin != value))
                {
                    this.SendPropertyChanging("SensorMin",this._SensorMin,value);
                    this._SensorMin = value;
                    this.SendPropertyChanged("SensorMin");

                }
            }
        }

        System.Nullable<Boolean> _IsSquare=false;
        /// <summary>
        /// 模拟量-开平方
        /// </summary>
        [System.ComponentModel.DataAnnotations.Schema.Column("issquare")]
        [Way.EntityDB.WayDBColumnAttribute(Name="issquare",Comment="",Caption="模拟量-开平方",Storage = "_IsSquare",DbType="bit")]
        public virtual System.Nullable<Boolean> IsSquare
        {
            get
            {
                return this._IsSquare;
            }
            set
            {
                if ((this._IsSquare != value))
                {
                    this.SendPropertyChanging("IsSquare",this._IsSquare,value);
                    this._IsSquare = value;
                    this.SendPropertyChanged("IsSquare");

                }
            }
        }

        System.Nullable<Boolean> _IsLinear=false;
        /// <summary>
        /// 分段线性化
        /// </summary>
        [System.ComponentModel.DataAnnotations.Schema.Column("islinear")]
        [Way.EntityDB.WayDBColumnAttribute(Name="islinear",Comment="",Caption="分段线性化",Storage = "_IsLinear",DbType="bit")]
        public virtual System.Nullable<Boolean> IsLinear
        {
            get
            {
                return this._IsLinear;
            }
            set
            {
                if ((this._IsLinear != value))
                {
                    this.SendPropertyChanging("IsLinear",this._IsLinear,value);
                    this._IsLinear = value;
                    this.SendPropertyChanged("IsLinear");

                }
            }
        }

        double _LinearX1;
        /// <summary>
        /// 分段线性化X1
        /// </summary>
        [System.ComponentModel.DataAnnotations.Schema.Column("linearx1")]
        [Way.EntityDB.WayDBColumnAttribute(Name="linearx1",Comment="",Caption="分段线性化X1",Storage = "_LinearX1",DbType="double")]
        public virtual double LinearX1
        {
            get
            {
                return this._LinearX1;
            }
            set
            {
                if ((this._LinearX1 != value))
                {
                    this.SendPropertyChanging("LinearX1",this._LinearX1,value);
                    this._LinearX1 = value;
                    this.SendPropertyChanged("LinearX1");

                }
            }
        }

        double _LinearY1;
        /// <summary>
        /// 分段线性化Y1
        /// </summary>
        [System.ComponentModel.DataAnnotations.Schema.Column("lineary1")]
        [Way.EntityDB.WayDBColumnAttribute(Name="lineary1",Comment="",Caption="分段线性化Y1",Storage = "_LinearY1",DbType="double")]
        public virtual double LinearY1
        {
            get
            {
                return this._LinearY1;
            }
            set
            {
                if ((this._LinearY1 != value))
                {
                    this.SendPropertyChanging("LinearY1",this._LinearY1,value);
                    this._LinearY1 = value;
                    this.SendPropertyChanged("LinearY1");

                }
            }
        }

        double _LinearX2;
        /// <summary>
        /// 分段线性化X2
        /// </summary>
        [System.ComponentModel.DataAnnotations.Schema.Column("linearx2")]
        [Way.EntityDB.WayDBColumnAttribute(Name="linearx2",Comment="",Caption="分段线性化X2",Storage = "_LinearX2",DbType="double")]
        public virtual double LinearX2
        {
            get
            {
                return this._LinearX2;
            }
            set
            {
                if ((this._LinearX2 != value))
                {
                    this.SendPropertyChanging("LinearX2",this._LinearX2,value);
                    this._LinearX2 = value;
                    this.SendPropertyChanged("LinearX2");

                }
            }
        }

        double _LinearY2;
        /// <summary>
        /// 分段线性化Y2
        /// </summary>
        [System.ComponentModel.DataAnnotations.Schema.Column("lineary2")]
        [Way.EntityDB.WayDBColumnAttribute(Name="lineary2",Comment="",Caption="分段线性化Y2",Storage = "_LinearY2",DbType="double")]
        public virtual double LinearY2
        {
            get
            {
                return this._LinearY2;
            }
            set
            {
                if ((this._LinearY2 != value))
                {
                    this.SendPropertyChanging("LinearY2",this._LinearY2,value);
                    this._LinearY2 = value;
                    this.SendPropertyChanged("LinearY2");

                }
            }
        }

        double _LinearX3;
        /// <summary>
        /// 
        /// </summary>
        [System.ComponentModel.DataAnnotations.Schema.Column("linearx3")]
        [Way.EntityDB.WayDBColumnAttribute(Name="linearx3",Comment="",Caption="",Storage = "_LinearX3",DbType="double")]
        public virtual double LinearX3
        {
            get
            {
                return this._LinearX3;
            }
            set
            {
                if ((this._LinearX3 != value))
                {
                    this.SendPropertyChanging("LinearX3",this._LinearX3,value);
                    this._LinearX3 = value;
                    this.SendPropertyChanged("LinearX3");

                }
            }
        }

        double _LinearY3;
        /// <summary>
        /// 
        /// </summary>
        [System.ComponentModel.DataAnnotations.Schema.Column("lineary3")]
        [Way.EntityDB.WayDBColumnAttribute(Name="lineary3",Comment="",Caption="",Storage = "_LinearY3",DbType="double")]
        public virtual double LinearY3
        {
            get
            {
                return this._LinearY3;
            }
            set
            {
                if ((this._LinearY3 != value))
                {
                    this.SendPropertyChanging("LinearY3",this._LinearY3,value);
                    this._LinearY3 = value;
                    this.SendPropertyChanged("LinearY3");

                }
            }
        }

        double _LinearX4;
        /// <summary>
        /// 
        /// </summary>
        [System.ComponentModel.DataAnnotations.Schema.Column("linearx4")]
        [Way.EntityDB.WayDBColumnAttribute(Name="linearx4",Comment="",Caption="",Storage = "_LinearX4",DbType="double")]
        public virtual double LinearX4
        {
            get
            {
                return this._LinearX4;
            }
            set
            {
                if ((this._LinearX4 != value))
                {
                    this.SendPropertyChanging("LinearX4",this._LinearX4,value);
                    this._LinearX4 = value;
                    this.SendPropertyChanged("LinearX4");

                }
            }
        }

        double _AlarmValue2;
        /// <summary>
        /// 报警值2
        /// </summary>
        [System.ComponentModel.DataAnnotations.Schema.Column("alarmvalue2")]
        [Way.EntityDB.WayDBColumnAttribute(Name="alarmvalue2",Comment="",Caption="报警值2",Storage = "_AlarmValue2",DbType="double")]
        public virtual double AlarmValue2
        {
            get
            {
                return this._AlarmValue2;
            }
            set
            {
                if ((this._AlarmValue2 != value))
                {
                    this.SendPropertyChanging("AlarmValue2",this._AlarmValue2,value);
                    this._AlarmValue2 = value;
                    this.SendPropertyChanged("AlarmValue2");

                }
            }
        }

        System.Nullable<Int32> _AlarmPriority2;
        /// <summary>
        /// 报警优先级2
        /// </summary>
        [System.ComponentModel.DataAnnotations.Schema.Column("alarmpriority2")]
        [Way.EntityDB.WayDBColumnAttribute(Name="alarmpriority2",Comment="",Caption="报警优先级2",Storage = "_AlarmPriority2",DbType="int")]
        public virtual System.Nullable<Int32> AlarmPriority2
        {
            get
            {
                return this._AlarmPriority2;
            }
            set
            {
                if ((this._AlarmPriority2 != value))
                {
                    this.SendPropertyChanging("AlarmPriority2",this._AlarmPriority2,value);
                    this._AlarmPriority2 = value;
                    this.SendPropertyChanged("AlarmPriority2");

                }
            }
        }

        double _AlarmValue3;
        /// <summary>
        /// 
        /// </summary>
        [System.ComponentModel.DataAnnotations.Schema.Column("alarmvalue3")]
        [Way.EntityDB.WayDBColumnAttribute(Name="alarmvalue3",Comment="",Caption="",Storage = "_AlarmValue3",DbType="double")]
        public virtual double AlarmValue3
        {
            get
            {
                return this._AlarmValue3;
            }
            set
            {
                if ((this._AlarmValue3 != value))
                {
                    this.SendPropertyChanging("AlarmValue3",this._AlarmValue3,value);
                    this._AlarmValue3 = value;
                    this.SendPropertyChanged("AlarmValue3");

                }
            }
        }

        System.Nullable<Int32> _AlarmPriority3;
        /// <summary>
        /// 
        /// </summary>
        [System.ComponentModel.DataAnnotations.Schema.Column("alarmpriority3")]
        [Way.EntityDB.WayDBColumnAttribute(Name="alarmpriority3",Comment="",Caption="",Storage = "_AlarmPriority3",DbType="int")]
        public virtual System.Nullable<Int32> AlarmPriority3
        {
            get
            {
                return this._AlarmPriority3;
            }
            set
            {
                if ((this._AlarmPriority3 != value))
                {
                    this.SendPropertyChanging("AlarmPriority3",this._AlarmPriority3,value);
                    this._AlarmPriority3 = value;
                    this.SendPropertyChanged("AlarmPriority3");

                }
            }
        }

        double _AlarmValue4;
        /// <summary>
        /// 
        /// </summary>
        [System.ComponentModel.DataAnnotations.Schema.Column("alarmvalue4")]
        [Way.EntityDB.WayDBColumnAttribute(Name="alarmvalue4",Comment="",Caption="",Storage = "_AlarmValue4",DbType="double")]
        public virtual double AlarmValue4
        {
            get
            {
                return this._AlarmValue4;
            }
            set
            {
                if ((this._AlarmValue4 != value))
                {
                    this.SendPropertyChanging("AlarmValue4",this._AlarmValue4,value);
                    this._AlarmValue4 = value;
                    this.SendPropertyChanged("AlarmValue4");

                }
            }
        }

        System.Nullable<Int32> _AlarmPriority4;
        /// <summary>
        /// 
        /// </summary>
        [System.ComponentModel.DataAnnotations.Schema.Column("alarmpriority4")]
        [Way.EntityDB.WayDBColumnAttribute(Name="alarmpriority4",Comment="",Caption="",Storage = "_AlarmPriority4",DbType="int")]
        public virtual System.Nullable<Int32> AlarmPriority4
        {
            get
            {
                return this._AlarmPriority4;
            }
            set
            {
                if ((this._AlarmPriority4 != value))
                {
                    this.SendPropertyChanging("AlarmPriority4",this._AlarmPriority4,value);
                    this._AlarmPriority4 = value;
                    this.SendPropertyChanged("AlarmPriority4");

                }
            }
        }

        double _AlarmValue5;
        /// <summary>
        /// 
        /// </summary>
        [System.ComponentModel.DataAnnotations.Schema.Column("alarmvalue5")]
        [Way.EntityDB.WayDBColumnAttribute(Name="alarmvalue5",Comment="",Caption="",Storage = "_AlarmValue5",DbType="double")]
        public virtual double AlarmValue5
        {
            get
            {
                return this._AlarmValue5;
            }
            set
            {
                if ((this._AlarmValue5 != value))
                {
                    this.SendPropertyChanging("AlarmValue5",this._AlarmValue5,value);
                    this._AlarmValue5 = value;
                    this.SendPropertyChanged("AlarmValue5");

                }
            }
        }

        System.Nullable<Int32> _AlarmPriority5;
        /// <summary>
        /// 
        /// </summary>
        [System.ComponentModel.DataAnnotations.Schema.Column("alarmpriority5")]
        [Way.EntityDB.WayDBColumnAttribute(Name="alarmpriority5",Comment="",Caption="",Storage = "_AlarmPriority5",DbType="int")]
        public virtual System.Nullable<Int32> AlarmPriority5
        {
            get
            {
                return this._AlarmPriority5;
            }
            set
            {
                if ((this._AlarmPriority5 != value))
                {
                    this.SendPropertyChanging("AlarmPriority5",this._AlarmPriority5,value);
                    this._AlarmPriority5 = value;
                    this.SendPropertyChanged("AlarmPriority5");

                }
            }
        }

        System.Nullable<Boolean> _IsAlarmOffset=false;
        /// <summary>
        /// 偏差报警
        /// </summary>
        [System.ComponentModel.DataAnnotations.Schema.Column("isalarmoffset")]
        [Way.EntityDB.WayDBColumnAttribute(Name="isalarmoffset",Comment="",Caption="偏差报警",Storage = "_IsAlarmOffset",DbType="bit")]
        public virtual System.Nullable<Boolean> IsAlarmOffset
        {
            get
            {
                return this._IsAlarmOffset;
            }
            set
            {
                if ((this._IsAlarmOffset != value))
                {
                    this.SendPropertyChanging("IsAlarmOffset",this._IsAlarmOffset,value);
                    this._IsAlarmOffset = value;
                    this.SendPropertyChanged("IsAlarmOffset");

                }
            }
        }

        double _AlarmOffsetOriginalValue;
        /// <summary>
        /// 偏差报警设定值
        /// </summary>
        [System.ComponentModel.DataAnnotations.Schema.Column("alarmoffsetoriginalvalue")]
        [Way.EntityDB.WayDBColumnAttribute(Name="alarmoffsetoriginalvalue",Comment="",Caption="偏差报警设定值",Storage = "_AlarmOffsetOriginalValue",DbType="double")]
        public virtual double AlarmOffsetOriginalValue
        {
            get
            {
                return this._AlarmOffsetOriginalValue;
            }
            set
            {
                if ((this._AlarmOffsetOriginalValue != value))
                {
                    this.SendPropertyChanging("AlarmOffsetOriginalValue",this._AlarmOffsetOriginalValue,value);
                    this._AlarmOffsetOriginalValue = value;
                    this.SendPropertyChanged("AlarmOffsetOriginalValue");

                }
            }
        }

        double _AlarmOffsetValue;
        /// <summary>
        /// 偏差值
        /// </summary>
        [System.ComponentModel.DataAnnotations.Schema.Column("alarmoffsetvalue")]
        [Way.EntityDB.WayDBColumnAttribute(Name="alarmoffsetvalue",Comment="",Caption="偏差值",Storage = "_AlarmOffsetValue",DbType="double")]
        public virtual double AlarmOffsetValue
        {
            get
            {
                return this._AlarmOffsetValue;
            }
            set
            {
                if ((this._AlarmOffsetValue != value))
                {
                    this.SendPropertyChanging("AlarmOffsetValue",this._AlarmOffsetValue,value);
                    this._AlarmOffsetValue = value;
                    this.SendPropertyChanged("AlarmOffsetValue");

                }
            }
        }

        System.Nullable<Int32> _AlarmOffsetPriority;
        /// <summary>
        /// 偏差报警优先级
        /// </summary>
        [System.ComponentModel.DataAnnotations.Schema.Column("alarmoffsetpriority")]
        [Way.EntityDB.WayDBColumnAttribute(Name="alarmoffsetpriority",Comment="",Caption="偏差报警优先级",Storage = "_AlarmOffsetPriority",DbType="int")]
        public virtual System.Nullable<Int32> AlarmOffsetPriority
        {
            get
            {
                return this._AlarmOffsetPriority;
            }
            set
            {
                if ((this._AlarmOffsetPriority != value))
                {
                    this.SendPropertyChanging("AlarmOffsetPriority",this._AlarmOffsetPriority,value);
                    this._AlarmOffsetPriority = value;
                    this.SendPropertyChanged("AlarmOffsetPriority");

                }
            }
        }

        System.Nullable<Boolean> _IsAlarmPercent=false;
        /// <summary>
        /// 变化率报警
        /// </summary>
        [System.ComponentModel.DataAnnotations.Schema.Column("isalarmpercent")]
        [Way.EntityDB.WayDBColumnAttribute(Name="isalarmpercent",Comment="",Caption="变化率报警",Storage = "_IsAlarmPercent",DbType="bit")]
        public virtual System.Nullable<Boolean> IsAlarmPercent
        {
            get
            {
                return this._IsAlarmPercent;
            }
            set
            {
                if ((this._IsAlarmPercent != value))
                {
                    this.SendPropertyChanging("IsAlarmPercent",this._IsAlarmPercent,value);
                    this._IsAlarmPercent = value;
                    this.SendPropertyChanged("IsAlarmPercent");

                }
            }
        }

        double _Percent;
        /// <summary>
        /// 变化率值（百分比）
        /// </summary>
        [System.ComponentModel.DataAnnotations.Schema.Column("percent")]
        [Way.EntityDB.WayDBColumnAttribute(Name="percent",Comment="",Caption="变化率值（百分比）",Storage = "_Percent",DbType="double")]
        public virtual double Percent
        {
            get
            {
                return this._Percent;
            }
            set
            {
                if ((this._Percent != value))
                {
                    this.SendPropertyChanging("Percent",this._Percent,value);
                    this._Percent = value;
                    this.SendPropertyChanged("Percent");

                }
            }
        }

        System.Nullable<Int32> _ChangeCycle;
        /// <summary>
        /// 变化率周期（秒）
        /// </summary>
        [System.ComponentModel.DataAnnotations.Schema.Column("changecycle")]
        [Way.EntityDB.WayDBColumnAttribute(Name="changecycle",Comment="",Caption="变化率周期（秒）",Storage = "_ChangeCycle",DbType="int")]
        public virtual System.Nullable<Int32> ChangeCycle
        {
            get
            {
                return this._ChangeCycle;
            }
            set
            {
                if ((this._ChangeCycle != value))
                {
                    this.SendPropertyChanging("ChangeCycle",this._ChangeCycle,value);
                    this._ChangeCycle = value;
                    this.SendPropertyChanged("ChangeCycle");

                }
            }
        }

        System.Nullable<Int32> _AlarmPercentPriority;
        /// <summary>
        /// 变化率报警优先级
        /// </summary>
        [System.ComponentModel.DataAnnotations.Schema.Column("alarmpercentpriority")]
        [Way.EntityDB.WayDBColumnAttribute(Name="alarmpercentpriority",Comment="",Caption="变化率报警优先级",Storage = "_AlarmPercentPriority",DbType="int")]
        public virtual System.Nullable<Int32> AlarmPercentPriority
        {
            get
            {
                return this._AlarmPercentPriority;
            }
            set
            {
                if ((this._AlarmPercentPriority != value))
                {
                    this.SendPropertyChanging("AlarmPercentPriority",this._AlarmPercentPriority,value);
                    this._AlarmPercentPriority = value;
                    this.SendPropertyChanged("AlarmPercentPriority");

                }
            }
        }

        System.Nullable<Int32> _FolderId=0;
        /// <summary>
        /// 
        /// </summary>
        [System.ComponentModel.DataAnnotations.Schema.Column("folderid")]
        [Way.EntityDB.WayDBColumnAttribute(Name="folderid",Comment="",Caption="",Storage = "_FolderId",DbType="int")]
        public virtual System.Nullable<Int32> FolderId
        {
            get
            {
                return this._FolderId;
            }
            set
            {
                if ((this._FolderId != value))
                {
                    this.SendPropertyChanging("FolderId",this._FolderId,value);
                    this._FolderId = value;
                    this.SendPropertyChanged("FolderId");

                }
            }
        }

        System.Nullable<Int32> _DeviceId;
        /// <summary>
        /// 
        /// </summary>
        [System.ComponentModel.DataAnnotations.Schema.Column("deviceid")]
        [Way.EntityDB.WayDBColumnAttribute(Name="deviceid",Comment="",Caption="",Storage = "_DeviceId",DbType="int")]
        public virtual System.Nullable<Int32> DeviceId
        {
            get
            {
                return this._DeviceId;
            }
            set
            {
                if ((this._DeviceId != value))
                {
                    this.SendPropertyChanging("DeviceId",this._DeviceId,value);
                    this._DeviceId = value;
                    this.SendPropertyChanged("DeviceId");

                }
            }
        }
}}
namespace SunRizServer{


    /// <summary>
	/// 控制单元
	/// </summary>
    [System.ComponentModel.DataAnnotations.Schema.Table("controlunit")]
    [Way.EntityDB.Attributes.Table("id")]
    public class ControlUnit :Way.EntityDB.DataItem
    {

        /// <summary>
	    /// 
	    /// </summary>
        public  ControlUnit()
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

        String _Name;
        /// <summary>
        /// 名称
        /// </summary>
        [System.ComponentModel.DataAnnotations.Schema.Column("name")]
        [Way.EntityDB.WayDBColumnAttribute(Name="name",Comment="",Caption="名称",Storage = "_Name",DbType="varchar(50)")]
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
}}
namespace SunRizServer{

/// <summary>
/// 
/// </summary>
public enum DevicePointFolder_TypeEnum:int
{
    

/// <summary>
/// 
/// </summary>
Analog = 1,

/// <summary>
/// 
/// </summary>

Digital = 2,
}


    /// <summary>
	/// 设备点文件夹
	/// </summary>
    [System.ComponentModel.DataAnnotations.Schema.Table("devicepointfolder")]
    [Way.EntityDB.Attributes.Table("id")]
    public class DevicePointFolder :Way.EntityDB.DataItem
    {

        /// <summary>
	    /// 
	    /// </summary>
        public  DevicePointFolder()
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

        String _Name;
        /// <summary>
        /// 
        /// </summary>
        [System.ComponentModel.DataAnnotations.Schema.Column("name")]
        [Way.EntityDB.WayDBColumnAttribute(Name="name",Comment="",Caption="",Storage = "_Name",DbType="varchar(100)")]
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

        System.Nullable<Int32> _ParentId=0;
        /// <summary>
        /// 
        /// </summary>
        [System.ComponentModel.DataAnnotations.Schema.Column("parentid")]
        [Way.EntityDB.WayDBColumnAttribute(Name="parentid",Comment="",Caption="",Storage = "_ParentId",DbType="int")]
        public virtual System.Nullable<Int32> ParentId
        {
            get
            {
                return this._ParentId;
            }
            set
            {
                if ((this._ParentId != value))
                {
                    this.SendPropertyChanging("ParentId",this._ParentId,value);
                    this._ParentId = value;
                    this.SendPropertyChanged("ParentId");

                }
            }
        }

        System.Nullable<Int32> _DeviceId;
        /// <summary>
        /// 
        /// </summary>
        [System.ComponentModel.DataAnnotations.Schema.Column("deviceid")]
        [Way.EntityDB.WayDBColumnAttribute(Name="deviceid",Comment="",Caption="",Storage = "_DeviceId",DbType="int")]
        public virtual System.Nullable<Int32> DeviceId
        {
            get
            {
                return this._DeviceId;
            }
            set
            {
                if ((this._DeviceId != value))
                {
                    this.SendPropertyChanging("DeviceId",this._DeviceId,value);
                    this._DeviceId = value;
                    this.SendPropertyChanged("DeviceId");

                }
            }
        }

        System.Nullable<DevicePointFolder_TypeEnum> _Type;
        /// <summary>
        /// 
        /// </summary>
        [System.ComponentModel.DataAnnotations.Schema.Column("type")]
        [Way.EntityDB.WayDBColumnAttribute(Name="type",Comment="",Caption="",Storage = "_Type",DbType="int")]
        public virtual System.Nullable<DevicePointFolder_TypeEnum> Type
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
}}

namespace SunRizServer.DB{
    /// <summary>
	/// 
	/// </summary>
    public class SunRiz : Way.EntityDB.DBContext
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="dbType"></param>
        public SunRiz(string connection, Way.EntityDB.DatabaseType dbType): base(connection, dbType)
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
            var db =  sender as SunRizServer.DB.SunRiz;
            if (db == null)
                return;


                    if (e.DataItem is SunRizServer.Device)
                    {
                        var deletingItem = (SunRizServer.Device)e.DataItem;
                        
                    var items0 = (from m in db.DevicePoint
                    where m.DeviceId == deletingItem.id
                    select new SunRizServer.DevicePoint
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

                    var items1 = (from m in db.DevicePointFolder
                    where m.DeviceId == deletingItem.id
                    select new SunRizServer.DevicePointFolder
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

                    if (e.DataItem is SunRizServer.DevicePointFolder)
                    {
                        var deletingItem = (SunRizServer.DevicePointFolder)e.DataItem;
                        
                    var items0 = (from m in db.DevicePoint
                    where m.FolderId == deletingItem.id
                    select new SunRizServer.DevicePoint
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
   modelBuilder.Entity<SunRizServer.ImageFiles>().HasKey(m => m.id);
modelBuilder.Entity<SunRizServer.CommunicationDriver>().HasKey(m => m.id);
modelBuilder.Entity<SunRizServer.Device>().HasKey(m => m.id);
modelBuilder.Entity<SunRizServer.DevicePoint>().HasKey(m => m.id);
modelBuilder.Entity<SunRizServer.ControlUnit>().HasKey(m => m.id);
modelBuilder.Entity<SunRizServer.DevicePointFolder>().HasKey(m => m.id);
}

        System.Linq.IQueryable<SunRizServer.ImageFiles> _ImageFiles;
        /// <summary>
        /// 图片文件
        /// </summary>
        public virtual System.Linq.IQueryable<SunRizServer.ImageFiles> ImageFiles
        {
             get
            {
                if (_ImageFiles == null)
                {
                    _ImageFiles = this.Set<SunRizServer.ImageFiles>();
                }
                return _ImageFiles;
            }
        }

        System.Linq.IQueryable<SunRizServer.CommunicationDriver> _CommunicationDriver;
        /// <summary>
        /// 通讯驱动列表
        /// </summary>
        public virtual System.Linq.IQueryable<SunRizServer.CommunicationDriver> CommunicationDriver
        {
             get
            {
                if (_CommunicationDriver == null)
                {
                    _CommunicationDriver = this.Set<SunRizServer.CommunicationDriver>();
                }
                return _CommunicationDriver;
            }
        }

        System.Linq.IQueryable<SunRizServer.Device> _Device;
        /// <summary>
        /// 设备信息
        /// </summary>
        public virtual System.Linq.IQueryable<SunRizServer.Device> Device
        {
             get
            {
                if (_Device == null)
                {
                    _Device = this.Set<SunRizServer.Device>();
                }
                return _Device;
            }
        }

        System.Linq.IQueryable<SunRizServer.DevicePoint> _DevicePoint;
        /// <summary>
        /// 
        /// </summary>
        public virtual System.Linq.IQueryable<SunRizServer.DevicePoint> DevicePoint
        {
             get
            {
                if (_DevicePoint == null)
                {
                    _DevicePoint = this.Set<SunRizServer.DevicePoint>();
                }
                return _DevicePoint;
            }
        }

        System.Linq.IQueryable<SunRizServer.ControlUnit> _ControlUnit;
        /// <summary>
        /// 控制单元
        /// </summary>
        public virtual System.Linq.IQueryable<SunRizServer.ControlUnit> ControlUnit
        {
             get
            {
                if (_ControlUnit == null)
                {
                    _ControlUnit = this.Set<SunRizServer.ControlUnit>();
                }
                return _ControlUnit;
            }
        }

        System.Linq.IQueryable<SunRizServer.DevicePointFolder> _DevicePointFolder;
        /// <summary>
        /// 设备点文件夹
        /// </summary>
        public virtual System.Linq.IQueryable<SunRizServer.DevicePointFolder> DevicePointFolder
        {
             get
            {
                if (_DevicePointFolder == null)
                {
                    _DevicePointFolder = this.Set<SunRizServer.DevicePointFolder>();
                }
                return _DevicePointFolder;
            }
        }

static string _designData = "eyJUYWJsZXMiOlt7IlRhYmxlTmFtZSI6IlNxbGl0ZSIsIlJvd3MiOlt7Ikl0ZW1zIjpbeyJOYW1lIjoiaWQiLCJWYWx1ZSI6MTd9LHsiTmFtZSI6InR5cGUiLCJWYWx1ZSI6IkNyZWF0ZVRhYmxlQWN0aW9uIn0seyJOYW1lIjoiY29udGVudCIsIlZhbHVlIjoie1wiVGFibGVcIjp7XCJpZFwiOjUsXCJjYXB0aW9uXCI6XCLlm77niYfmlofku7ZcIixcIk5hbWVcIjpcIkltYWdlRmlsZXNcIixcIkRhdGFiYXNlSURcIjoyLFwiaUxvY2tcIjowfSxcIkNvbHVtbnNcIjpbe1wiaWRcIjoyOSxcIk5hbWVcIjpcImlkXCIsXCJJc0F1dG9JbmNyZW1lbnRcIjp0cnVlLFwiQ2FuTnVsbFwiOmZhbHNlLFwiZGJUeXBlXCI6XCJpbnRcIixcIlRhYmxlSURcIjo1LFwiSXNQS0lEXCI6dHJ1ZSxcIm9yZGVyaWRcIjowfSx7XCJpZFwiOjMwLFwiTmFtZVwiOlwiTmFtZVwiLFwiSXNBdXRvSW5jcmVtZW50XCI6ZmFsc2UsXCJDYW5OdWxsXCI6dHJ1ZSxcImRiVHlwZVwiOlwidmFyY2hhclwiLFwibGVuZ3RoXCI6XCI1MFwiLFwiVGFibGVJRFwiOjUsXCJJc1BLSURcIjpmYWxzZSxcIm9yZGVyaWRcIjoxfSx7XCJpZFwiOjMxLFwiTmFtZVwiOlwiUGFyZW50SWRcIixcIklzQXV0b0luY3JlbWVudFwiOmZhbHNlLFwiQ2FuTnVsbFwiOnRydWUsXCJkYlR5cGVcIjpcImludFwiLFwibGVuZ3RoXCI6XCJcIixcIlRhYmxlSURcIjo1LFwiSXNQS0lEXCI6ZmFsc2UsXCJvcmRlcmlkXCI6Mn0se1wiaWRcIjozMixcIk5hbWVcIjpcIklzRm9sZGVyXCIsXCJJc0F1dG9JbmNyZW1lbnRcIjpmYWxzZSxcIkNhbk51bGxcIjp0cnVlLFwiZGJUeXBlXCI6XCJiaXRcIixcImxlbmd0aFwiOlwiXCIsXCJkZWZhdWx0VmFsdWVcIjpcIjBcIixcIlRhYmxlSURcIjo1LFwiSXNQS0lEXCI6ZmFsc2UsXCJvcmRlcmlkXCI6M30se1wiaWRcIjozMyxcImNhcHRpb25cIjpcIuaWh+S7tuWunumZheaWh+S7tuWQjVwiLFwiTmFtZVwiOlwiRmlsZU5hbWVcIixcIklzQXV0b0luY3JlbWVudFwiOmZhbHNlLFwiQ2FuTnVsbFwiOnRydWUsXCJkYlR5cGVcIjpcInZhcmNoYXJcIixcImxlbmd0aFwiOlwiNTBcIixcIlRhYmxlSURcIjo1LFwiSXNQS0lEXCI6ZmFsc2UsXCJvcmRlcmlkXCI6NH1dLFwiSURYQ29uZmlnc1wiOltdLFwiSURcIjowfSJ9LHsiTmFtZSI6ImRhdGFiYXNlaWQiLCJWYWx1ZSI6Mn1dLCJSb3dTdGF0ZSI6MH0seyJJdGVtcyI6W3siTmFtZSI6ImlkIiwiVmFsdWUiOjE4fSx7Ik5hbWUiOiJ0eXBlIiwiVmFsdWUiOiJDaGFuZ2VUYWJsZUFjdGlvbiJ9LHsiTmFtZSI6ImNvbnRlbnQiLCJWYWx1ZSI6IntcIk9sZFRhYmxlTmFtZVwiOlwiSW1hZ2VGaWxlc1wiLFwiTmV3VGFibGVOYW1lXCI6XCJJbWFnZUZpbGVzXCIsXCJvdGhlckNvbHVtbnNcIjpbe1wiaWRcIjoyOSxcIk5hbWVcIjpcImlkXCIsXCJJc0F1dG9JbmNyZW1lbnRcIjp0cnVlLFwiQ2FuTnVsbFwiOmZhbHNlLFwiZGJUeXBlXCI6XCJpbnRcIixcIlRhYmxlSURcIjo1LFwiSXNQS0lEXCI6dHJ1ZSxcIm9yZGVyaWRcIjowfSx7XCJpZFwiOjMwLFwiTmFtZVwiOlwiTmFtZVwiLFwiSXNBdXRvSW5jcmVtZW50XCI6ZmFsc2UsXCJDYW5OdWxsXCI6dHJ1ZSxcImRiVHlwZVwiOlwidmFyY2hhclwiLFwibGVuZ3RoXCI6XCI1MFwiLFwiVGFibGVJRFwiOjUsXCJJc1BLSURcIjpmYWxzZSxcIm9yZGVyaWRcIjoxfSx7XCJpZFwiOjMxLFwiTmFtZVwiOlwiUGFyZW50SWRcIixcIklzQXV0b0luY3JlbWVudFwiOmZhbHNlLFwiQ2FuTnVsbFwiOnRydWUsXCJkYlR5cGVcIjpcImludFwiLFwibGVuZ3RoXCI6XCJcIixcIlRhYmxlSURcIjo1LFwiSXNQS0lEXCI6ZmFsc2UsXCJvcmRlcmlkXCI6Mn0se1wiaWRcIjozMixcIk5hbWVcIjpcIklzRm9sZGVyXCIsXCJJc0F1dG9JbmNyZW1lbnRcIjpmYWxzZSxcIkNhbk51bGxcIjp0cnVlLFwiZGJUeXBlXCI6XCJiaXRcIixcImxlbmd0aFwiOlwiXCIsXCJkZWZhdWx0VmFsdWVcIjpcIjBcIixcIlRhYmxlSURcIjo1LFwiSXNQS0lEXCI6ZmFsc2UsXCJvcmRlcmlkXCI6M30se1wiaWRcIjozMyxcImNhcHRpb25cIjpcIuaWh+S7tuWunumZheaWh+S7tuWQjVwiLFwiTmFtZVwiOlwiRmlsZU5hbWVcIixcIklzQXV0b0luY3JlbWVudFwiOmZhbHNlLFwiQ2FuTnVsbFwiOnRydWUsXCJkYlR5cGVcIjpcInZhcmNoYXJcIixcImxlbmd0aFwiOlwiNTBcIixcIlRhYmxlSURcIjo1LFwiSXNQS0lEXCI6ZmFsc2UsXCJvcmRlcmlkXCI6NH1dLFwibmV3Q29sdW1uc1wiOltdLFwiY2hhbmdlZENvbHVtbnNcIjpbXSxcImRlbGV0ZWRDb2x1bW5zXCI6W10sXCJJRFhDb25maWdzXCI6W10sXCJJRFwiOjB9In0seyJOYW1lIjoiZGF0YWJhc2VpZCIsIlZhbHVlIjoyfV0sIlJvd1N0YXRlIjowfSx7Ikl0ZW1zIjpbeyJOYW1lIjoiaWQiLCJWYWx1ZSI6MTl9LHsiTmFtZSI6InR5cGUiLCJWYWx1ZSI6IkNoYW5nZVRhYmxlQWN0aW9uIn0seyJOYW1lIjoiY29udGVudCIsIlZhbHVlIjoie1wiT2xkVGFibGVOYW1lXCI6XCJJbWFnZUZpbGVzXCIsXCJOZXdUYWJsZU5hbWVcIjpcIkltYWdlRmlsZXNcIixcIm90aGVyQ29sdW1uc1wiOlt7XCJpZFwiOjI5LFwiTmFtZVwiOlwiaWRcIixcIklzQXV0b0luY3JlbWVudFwiOnRydWUsXCJDYW5OdWxsXCI6ZmFsc2UsXCJkYlR5cGVcIjpcImludFwiLFwiVGFibGVJRFwiOjUsXCJJc1BLSURcIjp0cnVlLFwib3JkZXJpZFwiOjB9LHtcImlkXCI6MzAsXCJjYXB0aW9uXCI6XCLmmL7npLrnmoTlkI3np7BcIixcIk5hbWVcIjpcIk5hbWVcIixcIklzQXV0b0luY3JlbWVudFwiOmZhbHNlLFwiQ2FuTnVsbFwiOnRydWUsXCJkYlR5cGVcIjpcInZhcmNoYXJcIixcImxlbmd0aFwiOlwiNTBcIixcIlRhYmxlSURcIjo1LFwiSXNQS0lEXCI6ZmFsc2UsXCJvcmRlcmlkXCI6MX0se1wiaWRcIjozMixcIk5hbWVcIjpcIklzRm9sZGVyXCIsXCJJc0F1dG9JbmNyZW1lbnRcIjpmYWxzZSxcIkNhbk51bGxcIjp0cnVlLFwiZGJUeXBlXCI6XCJiaXRcIixcImxlbmd0aFwiOlwiXCIsXCJkZWZhdWx0VmFsdWVcIjpcIjBcIixcIlRhYmxlSURcIjo1LFwiSXNQS0lEXCI6ZmFsc2UsXCJvcmRlcmlkXCI6M30se1wiaWRcIjozMyxcImNhcHRpb25cIjpcIuaWh+S7tuWunumZheaWh+S7tuWQjVwiLFwiTmFtZVwiOlwiRmlsZU5hbWVcIixcIklzQXV0b0luY3JlbWVudFwiOmZhbHNlLFwiQ2FuTnVsbFwiOnRydWUsXCJkYlR5cGVcIjpcInZhcmNoYXJcIixcImxlbmd0aFwiOlwiNTBcIixcIlRhYmxlSURcIjo1LFwiSXNQS0lEXCI6ZmFsc2UsXCJvcmRlcmlkXCI6NH1dLFwibmV3Q29sdW1uc1wiOltdLFwiY2hhbmdlZENvbHVtbnNcIjpbe1wiaWRcIjozMSxcIk5hbWVcIjpcIlBhcmVudElkXCIsXCJJc0F1dG9JbmNyZW1lbnRcIjpmYWxzZSxcIkNhbk51bGxcIjp0cnVlLFwiZGJUeXBlXCI6XCJpbnRcIixcImxlbmd0aFwiOlwiXCIsXCJkZWZhdWx0VmFsdWVcIjpcIjBcIixcIlRhYmxlSURcIjo1LFwiSXNQS0lEXCI6ZmFsc2UsXCJvcmRlcmlkXCI6MixcIkJhY2t1cENoYW5nZWRQcm9wZXJ0aWVzXCI6e1wiZGVmYXVsdFZhbHVlXCI6e1wiT3JpZ2luYWxWYWx1ZVwiOm51bGx9fX1dLFwiZGVsZXRlZENvbHVtbnNcIjpbXSxcIklEWENvbmZpZ3NcIjpbXSxcIklEXCI6MH0ifSx7Ik5hbWUiOiJkYXRhYmFzZWlkIiwiVmFsdWUiOjJ9XSwiUm93U3RhdGUiOjB9LHsiSXRlbXMiOlt7Ik5hbWUiOiJpZCIsIlZhbHVlIjoyMH0seyJOYW1lIjoidHlwZSIsIlZhbHVlIjoiQ3JlYXRlVGFibGVBY3Rpb24ifSx7Ik5hbWUiOiJjb250ZW50IiwiVmFsdWUiOiJ7XCJUYWJsZVwiOntcImlkXCI6NixcImNhcHRpb25cIjpcIumAmuiur+mpseWKqOWIl+ihqFwiLFwiTmFtZVwiOlwiQ29tbXVuaWNhdGlvbkRyaXZlclwiLFwiRGF0YWJhc2VJRFwiOjIsXCJpTG9ja1wiOjB9LFwiQ29sdW1uc1wiOlt7XCJpZFwiOjM0LFwiTmFtZVwiOlwiaWRcIixcIklzQXV0b0luY3JlbWVudFwiOnRydWUsXCJDYW5OdWxsXCI6ZmFsc2UsXCJkYlR5cGVcIjpcImludFwiLFwiVGFibGVJRFwiOjYsXCJJc1BLSURcIjp0cnVlLFwib3JkZXJpZFwiOjB9LHtcImlkXCI6MzUsXCJjYXB0aW9uXCI6XCLlkI3np7BcIixcIk5hbWVcIjpcIk5hbWVcIixcIklzQXV0b0luY3JlbWVudFwiOmZhbHNlLFwiQ2FuTnVsbFwiOnRydWUsXCJkYlR5cGVcIjpcInZhcmNoYXJcIixcImxlbmd0aFwiOlwiNTBcIixcIlRhYmxlSURcIjo2LFwiSXNQS0lEXCI6ZmFsc2UsXCJvcmRlcmlkXCI6MX0se1wiaWRcIjozNixcImNhcHRpb25cIjpcIuWcsOWdgFwiLFwiTmFtZVwiOlwiQWRkcmVzc1wiLFwiSXNBdXRvSW5jcmVtZW50XCI6ZmFsc2UsXCJDYW5OdWxsXCI6dHJ1ZSxcImRiVHlwZVwiOlwidmFyY2hhclwiLFwibGVuZ3RoXCI6XCI1MFwiLFwiVGFibGVJRFwiOjYsXCJJc1BLSURcIjpmYWxzZSxcIm9yZGVyaWRcIjoyfSx7XCJpZFwiOjM3LFwiY2FwdGlvblwiOlwi56uv5Y+jXCIsXCJOYW1lXCI6XCJQb3J0XCIsXCJJc0F1dG9JbmNyZW1lbnRcIjpmYWxzZSxcIkNhbk51bGxcIjp0cnVlLFwiZGJUeXBlXCI6XCJpbnRcIixcImxlbmd0aFwiOlwiXCIsXCJUYWJsZUlEXCI6NixcIklzUEtJRFwiOmZhbHNlLFwib3JkZXJpZFwiOjN9LHtcImlkXCI6MzgsXCJjYXB0aW9uXCI6XCLnirbmgIFcIixcIk5hbWVcIjpcIlN0YXR1c1wiLFwiSXNBdXRvSW5jcmVtZW50XCI6ZmFsc2UsXCJDYW5OdWxsXCI6dHJ1ZSxcImRiVHlwZVwiOlwiaW50XCIsXCJFbnVtRGVmaW5lXCI6XCJOb25lID0gMCxcXG5PbmxpbmUgPSAxLFxcbk9mZmxpbmUgPSAyXCIsXCJsZW5ndGhcIjpcIlwiLFwiZGVmYXVsdFZhbHVlXCI6XCIwXCIsXCJUYWJsZUlEXCI6NixcIklzUEtJRFwiOmZhbHNlLFwib3JkZXJpZFwiOjR9XSxcIklEWENvbmZpZ3NcIjpbXSxcIklEXCI6MH0ifSx7Ik5hbWUiOiJkYXRhYmFzZWlkIiwiVmFsdWUiOjJ9XSwiUm93U3RhdGUiOjB9LHsiSXRlbXMiOlt7Ik5hbWUiOiJpZCIsIlZhbHVlIjoyMX0seyJOYW1lIjoidHlwZSIsIlZhbHVlIjoiQ3JlYXRlVGFibGVBY3Rpb24ifSx7Ik5hbWUiOiJjb250ZW50IiwiVmFsdWUiOiJ7XCJUYWJsZVwiOntcImlkXCI6NyxcImNhcHRpb25cIjpcIuiuvuWkh+S/oeaBr1wiLFwiTmFtZVwiOlwiRGV2aWNlXCIsXCJEYXRhYmFzZUlEXCI6MixcImlMb2NrXCI6MH0sXCJDb2x1bW5zXCI6W3tcImlkXCI6MzksXCJOYW1lXCI6XCJpZFwiLFwiSXNBdXRvSW5jcmVtZW50XCI6dHJ1ZSxcIkNhbk51bGxcIjpmYWxzZSxcImRiVHlwZVwiOlwiaW50XCIsXCJUYWJsZUlEXCI6NyxcIklzUEtJRFwiOnRydWUsXCJvcmRlcmlkXCI6MH0se1wiaWRcIjo0MCxcIk5hbWVcIjpcIkRyaXZlcklEXCIsXCJJc0F1dG9JbmNyZW1lbnRcIjpmYWxzZSxcIkNhbk51bGxcIjp0cnVlLFwiZGJUeXBlXCI6XCJpbnRcIixcImxlbmd0aFwiOlwiXCIsXCJUYWJsZUlEXCI6NyxcIklzUEtJRFwiOmZhbHNlLFwib3JkZXJpZFwiOjF9LHtcImlkXCI6NDEsXCJjYXB0aW9uXCI6XCLlnLDlnYDkv6Hmga9cIixcIk5hbWVcIjpcIkFkZHJlc3NcIixcIklzQXV0b0luY3JlbWVudFwiOmZhbHNlLFwiQ2FuTnVsbFwiOnRydWUsXCJkYlR5cGVcIjpcInZhcmNoYXJcIixcImxlbmd0aFwiOlwiMTAwXCIsXCJUYWJsZUlEXCI6NyxcIklzUEtJRFwiOmZhbHNlLFwib3JkZXJpZFwiOjJ9XSxcIklEWENvbmZpZ3NcIjpbXSxcIklEXCI6MH0ifSx7Ik5hbWUiOiJkYXRhYmFzZWlkIiwiVmFsdWUiOjJ9XSwiUm93U3RhdGUiOjB9LHsiSXRlbXMiOlt7Ik5hbWUiOiJpZCIsIlZhbHVlIjoyMn0seyJOYW1lIjoidHlwZSIsIlZhbHVlIjoiQ2hhbmdlVGFibGVBY3Rpb24ifSx7Ik5hbWUiOiJjb250ZW50IiwiVmFsdWUiOiJ7XCJPbGRUYWJsZU5hbWVcIjpcIkNvbW11bmljYXRpb25Ecml2ZXJcIixcIk5ld1RhYmxlTmFtZVwiOlwiQ29tbXVuaWNhdGlvbkRyaXZlclwiLFwib3RoZXJDb2x1bW5zXCI6W3tcImlkXCI6MzQsXCJOYW1lXCI6XCJpZFwiLFwiSXNBdXRvSW5jcmVtZW50XCI6dHJ1ZSxcIkNhbk51bGxcIjpmYWxzZSxcImRiVHlwZVwiOlwiaW50XCIsXCJUYWJsZUlEXCI6NixcIklzUEtJRFwiOnRydWUsXCJvcmRlcmlkXCI6MH0se1wiaWRcIjozNSxcImNhcHRpb25cIjpcIuWQjeensFwiLFwiTmFtZVwiOlwiTmFtZVwiLFwiSXNBdXRvSW5jcmVtZW50XCI6ZmFsc2UsXCJDYW5OdWxsXCI6dHJ1ZSxcImRiVHlwZVwiOlwidmFyY2hhclwiLFwibGVuZ3RoXCI6XCI1MFwiLFwiVGFibGVJRFwiOjYsXCJJc1BLSURcIjpmYWxzZSxcIm9yZGVyaWRcIjoxfSx7XCJpZFwiOjM2LFwiY2FwdGlvblwiOlwi5Zyw5Z2AXCIsXCJOYW1lXCI6XCJBZGRyZXNzXCIsXCJJc0F1dG9JbmNyZW1lbnRcIjpmYWxzZSxcIkNhbk51bGxcIjp0cnVlLFwiZGJUeXBlXCI6XCJ2YXJjaGFyXCIsXCJsZW5ndGhcIjpcIjUwXCIsXCJUYWJsZUlEXCI6NixcIklzUEtJRFwiOmZhbHNlLFwib3JkZXJpZFwiOjJ9LHtcImlkXCI6MzcsXCJjYXB0aW9uXCI6XCLnq6/lj6NcIixcIk5hbWVcIjpcIlBvcnRcIixcIklzQXV0b0luY3JlbWVudFwiOmZhbHNlLFwiQ2FuTnVsbFwiOnRydWUsXCJkYlR5cGVcIjpcImludFwiLFwibGVuZ3RoXCI6XCJcIixcIlRhYmxlSURcIjo2LFwiSXNQS0lEXCI6ZmFsc2UsXCJvcmRlcmlkXCI6M30se1wiaWRcIjozOCxcImNhcHRpb25cIjpcIueKtuaAgVwiLFwiTmFtZVwiOlwiU3RhdHVzXCIsXCJJc0F1dG9JbmNyZW1lbnRcIjpmYWxzZSxcIkNhbk51bGxcIjp0cnVlLFwiZGJUeXBlXCI6XCJpbnRcIixcIkVudW1EZWZpbmVcIjpcIk5vbmUgPSAwLFxcbk9ubGluZSA9IDEsXFxuT2ZmbGluZSA9IDJcIixcImxlbmd0aFwiOlwiXCIsXCJkZWZhdWx0VmFsdWVcIjpcIjBcIixcIlRhYmxlSURcIjo2LFwiSXNQS0lEXCI6ZmFsc2UsXCJvcmRlcmlkXCI6NH1dLFwibmV3Q29sdW1uc1wiOltdLFwiY2hhbmdlZENvbHVtbnNcIjpbXSxcImRlbGV0ZWRDb2x1bW5zXCI6W10sXCJJRFhDb25maWdzXCI6W10sXCJJRFwiOjB9In0seyJOYW1lIjoiZGF0YWJhc2VpZCIsIlZhbHVlIjoyfV0sIlJvd1N0YXRlIjowfSx7Ikl0ZW1zIjpbeyJOYW1lIjoiaWQiLCJWYWx1ZSI6MjN9LHsiTmFtZSI6InR5cGUiLCJWYWx1ZSI6IkNyZWF0ZVRhYmxlQWN0aW9uIn0seyJOYW1lIjoiY29udGVudCIsIlZhbHVlIjoie1wiVGFibGVcIjp7XCJpZFwiOjgsXCJOYW1lXCI6XCJEZXZpY2VQb2ludFwiLFwiRGF0YWJhc2VJRFwiOjIsXCJpTG9ja1wiOjB9LFwiQ29sdW1uc1wiOlt7XCJpZFwiOjQyLFwiTmFtZVwiOlwiaWRcIixcIklzQXV0b0luY3JlbWVudFwiOnRydWUsXCJDYW5OdWxsXCI6ZmFsc2UsXCJkYlR5cGVcIjpcImludFwiLFwiVGFibGVJRFwiOjgsXCJJc1BLSURcIjp0cnVlLFwib3JkZXJpZFwiOjB9LHtcImlkXCI6NDMsXCJOYW1lXCI6XCJEZXZpY2VJRFwiLFwiSXNBdXRvSW5jcmVtZW50XCI6ZmFsc2UsXCJDYW5OdWxsXCI6dHJ1ZSxcImRiVHlwZVwiOlwiaW50XCIsXCJsZW5ndGhcIjpcIlwiLFwiVGFibGVJRFwiOjgsXCJJc1BLSURcIjpmYWxzZSxcIm9yZGVyaWRcIjoxfSx7XCJpZFwiOjQ0LFwiTmFtZVwiOlwiQWRkcmVzc1wiLFwiSXNBdXRvSW5jcmVtZW50XCI6ZmFsc2UsXCJDYW5OdWxsXCI6dHJ1ZSxcImRiVHlwZVwiOlwidmFyY2hhclwiLFwibGVuZ3RoXCI6XCIyMDBcIixcIlRhYmxlSURcIjo4LFwiSXNQS0lEXCI6ZmFsc2UsXCJvcmRlcmlkXCI6Mn0se1wiaWRcIjo0NSxcImNhcHRpb25cIjpcIuaYr+WQpuebkea1i+WPmOWMllwiLFwiTmFtZVwiOlwiSXNXYXRjaGluZ1wiLFwiSXNBdXRvSW5jcmVtZW50XCI6ZmFsc2UsXCJDYW5OdWxsXCI6dHJ1ZSxcImRiVHlwZVwiOlwiYml0XCIsXCJsZW5ndGhcIjpcIlwiLFwiZGVmYXVsdFZhbHVlXCI6XCIwXCIsXCJUYWJsZUlEXCI6OCxcIklzUEtJRFwiOmZhbHNlLFwib3JkZXJpZFwiOjN9XSxcIklEWENvbmZpZ3NcIjpbXSxcIklEXCI6MH0ifSx7Ik5hbWUiOiJkYXRhYmFzZWlkIiwiVmFsdWUiOjJ9XSwiUm93U3RhdGUiOjB9LHsiSXRlbXMiOlt7Ik5hbWUiOiJpZCIsIlZhbHVlIjoyNH0seyJOYW1lIjoidHlwZSIsIlZhbHVlIjoiQ2hhbmdlVGFibGVBY3Rpb24ifSx7Ik5hbWUiOiJjb250ZW50IiwiVmFsdWUiOiJ7XCJPbGRUYWJsZU5hbWVcIjpcIkRldmljZVwiLFwiTmV3VGFibGVOYW1lXCI6XCJEZXZpY2VcIixcIm90aGVyQ29sdW1uc1wiOlt7XCJpZFwiOjM5LFwiTmFtZVwiOlwiaWRcIixcIklzQXV0b0luY3JlbWVudFwiOnRydWUsXCJDYW5OdWxsXCI6ZmFsc2UsXCJkYlR5cGVcIjpcImludFwiLFwiVGFibGVJRFwiOjcsXCJJc1BLSURcIjp0cnVlLFwib3JkZXJpZFwiOjB9LHtcImlkXCI6NDAsXCJOYW1lXCI6XCJEcml2ZXJJRFwiLFwiSXNBdXRvSW5jcmVtZW50XCI6ZmFsc2UsXCJDYW5OdWxsXCI6dHJ1ZSxcImRiVHlwZVwiOlwiaW50XCIsXCJsZW5ndGhcIjpcIlwiLFwiVGFibGVJRFwiOjcsXCJJc1BLSURcIjpmYWxzZSxcIm9yZGVyaWRcIjoxfSx7XCJpZFwiOjQxLFwiY2FwdGlvblwiOlwi5Zyw5Z2A5L+h5oGvXCIsXCJOYW1lXCI6XCJBZGRyZXNzXCIsXCJJc0F1dG9JbmNyZW1lbnRcIjpmYWxzZSxcIkNhbk51bGxcIjp0cnVlLFwiZGJUeXBlXCI6XCJ2YXJjaGFyXCIsXCJsZW5ndGhcIjpcIjEwMFwiLFwiVGFibGVJRFwiOjcsXCJJc1BLSURcIjpmYWxzZSxcIm9yZGVyaWRcIjoyfV0sXCJuZXdDb2x1bW5zXCI6W10sXCJjaGFuZ2VkQ29sdW1uc1wiOltdLFwiZGVsZXRlZENvbHVtbnNcIjpbXSxcIklEWENvbmZpZ3NcIjpbXSxcIklEXCI6MH0ifSx7Ik5hbWUiOiJkYXRhYmFzZWlkIiwiVmFsdWUiOjJ9XSwiUm93U3RhdGUiOjB9LHsiSXRlbXMiOlt7Ik5hbWUiOiJpZCIsIlZhbHVlIjoyNX0seyJOYW1lIjoidHlwZSIsIlZhbHVlIjoiQ2hhbmdlVGFibGVBY3Rpb24ifSx7Ik5hbWUiOiJjb250ZW50IiwiVmFsdWUiOiJ7XCJPbGRUYWJsZU5hbWVcIjpcIkNvbW11bmljYXRpb25Ecml2ZXJcIixcIk5ld1RhYmxlTmFtZVwiOlwiQ29tbXVuaWNhdGlvbkRyaXZlclwiLFwib3RoZXJDb2x1bW5zXCI6W3tcImlkXCI6MzQsXCJOYW1lXCI6XCJpZFwiLFwiSXNBdXRvSW5jcmVtZW50XCI6dHJ1ZSxcIkNhbk51bGxcIjpmYWxzZSxcImRiVHlwZVwiOlwiaW50XCIsXCJUYWJsZUlEXCI6NixcIklzUEtJRFwiOnRydWUsXCJvcmRlcmlkXCI6MH0se1wiaWRcIjozNSxcImNhcHRpb25cIjpcIuWQjeensFwiLFwiTmFtZVwiOlwiTmFtZVwiLFwiSXNBdXRvSW5jcmVtZW50XCI6ZmFsc2UsXCJDYW5OdWxsXCI6dHJ1ZSxcImRiVHlwZVwiOlwidmFyY2hhclwiLFwibGVuZ3RoXCI6XCI1MFwiLFwiVGFibGVJRFwiOjYsXCJJc1BLSURcIjpmYWxzZSxcIm9yZGVyaWRcIjoxfSx7XCJpZFwiOjM2LFwiY2FwdGlvblwiOlwi5Zyw5Z2AXCIsXCJOYW1lXCI6XCJBZGRyZXNzXCIsXCJJc0F1dG9JbmNyZW1lbnRcIjpmYWxzZSxcIkNhbk51bGxcIjp0cnVlLFwiZGJUeXBlXCI6XCJ2YXJjaGFyXCIsXCJsZW5ndGhcIjpcIjUwXCIsXCJUYWJsZUlEXCI6NixcIklzUEtJRFwiOmZhbHNlLFwib3JkZXJpZFwiOjJ9LHtcImlkXCI6MzcsXCJjYXB0aW9uXCI6XCLnq6/lj6NcIixcIk5hbWVcIjpcIlBvcnRcIixcIklzQXV0b0luY3JlbWVudFwiOmZhbHNlLFwiQ2FuTnVsbFwiOnRydWUsXCJkYlR5cGVcIjpcImludFwiLFwibGVuZ3RoXCI6XCJcIixcIlRhYmxlSURcIjo2LFwiSXNQS0lEXCI6ZmFsc2UsXCJvcmRlcmlkXCI6M30se1wiaWRcIjozOCxcImNhcHRpb25cIjpcIueKtuaAgVwiLFwiTmFtZVwiOlwiU3RhdHVzXCIsXCJJc0F1dG9JbmNyZW1lbnRcIjpmYWxzZSxcIkNhbk51bGxcIjp0cnVlLFwiZGJUeXBlXCI6XCJpbnRcIixcIkVudW1EZWZpbmVcIjpcIk5vbmUgPSAwLFxcbk9ubGluZSA9IDEsXFxuT2ZmbGluZSA9IDJcIixcImxlbmd0aFwiOlwiXCIsXCJkZWZhdWx0VmFsdWVcIjpcIjBcIixcIlRhYmxlSURcIjo2LFwiSXNQS0lEXCI6ZmFsc2UsXCJvcmRlcmlkXCI6NH1dLFwibmV3Q29sdW1uc1wiOlt7XCJpZFwiOjQ2LFwiTmFtZVwiOlwiU3VwcG9ydEVudW1EZXZpY2VcIixcIklzQXV0b0luY3JlbWVudFwiOmZhbHNlLFwiQ2FuTnVsbFwiOnRydWUsXCJkYlR5cGVcIjpcImJpdFwiLFwibGVuZ3RoXCI6XCJcIixcImRlZmF1bHRWYWx1ZVwiOlwiMFwiLFwiVGFibGVJRFwiOjYsXCJJc1BLSURcIjpmYWxzZSxcIm9yZGVyaWRcIjo1fV0sXCJjaGFuZ2VkQ29sdW1uc1wiOltdLFwiZGVsZXRlZENvbHVtbnNcIjpbXSxcIklEWENvbmZpZ3NcIjpbXSxcIklEXCI6MH0ifSx7Ik5hbWUiOiJkYXRhYmFzZWlkIiwiVmFsdWUiOjJ9XSwiUm93U3RhdGUiOjB9LHsiSXRlbXMiOlt7Ik5hbWUiOiJpZCIsIlZhbHVlIjoyNn0seyJOYW1lIjoidHlwZSIsIlZhbHVlIjoiQ2hhbmdlVGFibGVBY3Rpb24ifSx7Ik5hbWUiOiJjb250ZW50IiwiVmFsdWUiOiJ7XCJPbGRUYWJsZU5hbWVcIjpcIkNvbW11bmljYXRpb25Ecml2ZXJcIixcIk5ld1RhYmxlTmFtZVwiOlwiQ29tbXVuaWNhdGlvbkRyaXZlclwiLFwib3RoZXJDb2x1bW5zXCI6W3tcImlkXCI6MzQsXCJOYW1lXCI6XCJpZFwiLFwiSXNBdXRvSW5jcmVtZW50XCI6dHJ1ZSxcIkNhbk51bGxcIjpmYWxzZSxcImRiVHlwZVwiOlwiaW50XCIsXCJUYWJsZUlEXCI6NixcIklzUEtJRFwiOnRydWUsXCJvcmRlcmlkXCI6MH0se1wiaWRcIjozNSxcImNhcHRpb25cIjpcIuWQjeensFwiLFwiTmFtZVwiOlwiTmFtZVwiLFwiSXNBdXRvSW5jcmVtZW50XCI6ZmFsc2UsXCJDYW5OdWxsXCI6dHJ1ZSxcImRiVHlwZVwiOlwidmFyY2hhclwiLFwibGVuZ3RoXCI6XCI1MFwiLFwiVGFibGVJRFwiOjYsXCJJc1BLSURcIjpmYWxzZSxcIm9yZGVyaWRcIjoxfSx7XCJpZFwiOjM2LFwiY2FwdGlvblwiOlwi5Zyw5Z2AXCIsXCJOYW1lXCI6XCJBZGRyZXNzXCIsXCJJc0F1dG9JbmNyZW1lbnRcIjpmYWxzZSxcIkNhbk51bGxcIjp0cnVlLFwiZGJUeXBlXCI6XCJ2YXJjaGFyXCIsXCJsZW5ndGhcIjpcIjUwXCIsXCJUYWJsZUlEXCI6NixcIklzUEtJRFwiOmZhbHNlLFwib3JkZXJpZFwiOjJ9LHtcImlkXCI6MzcsXCJjYXB0aW9uXCI6XCLnq6/lj6NcIixcIk5hbWVcIjpcIlBvcnRcIixcIklzQXV0b0luY3JlbWVudFwiOmZhbHNlLFwiQ2FuTnVsbFwiOnRydWUsXCJkYlR5cGVcIjpcImludFwiLFwibGVuZ3RoXCI6XCJcIixcIlRhYmxlSURcIjo2LFwiSXNQS0lEXCI6ZmFsc2UsXCJvcmRlcmlkXCI6M30se1wiaWRcIjozOCxcImNhcHRpb25cIjpcIueKtuaAgVwiLFwiTmFtZVwiOlwiU3RhdHVzXCIsXCJJc0F1dG9JbmNyZW1lbnRcIjpmYWxzZSxcIkNhbk51bGxcIjp0cnVlLFwiZGJUeXBlXCI6XCJpbnRcIixcIkVudW1EZWZpbmVcIjpcIk5vbmUgPSAwLFxcbk9ubGluZSA9IDEsXFxuT2ZmbGluZSA9IDJcIixcImxlbmd0aFwiOlwiXCIsXCJkZWZhdWx0VmFsdWVcIjpcIjBcIixcIlRhYmxlSURcIjo2LFwiSXNQS0lEXCI6ZmFsc2UsXCJvcmRlcmlkXCI6NH0se1wiaWRcIjo0NixcIk5hbWVcIjpcIlN1cHBvcnRFbnVtRGV2aWNlXCIsXCJJc0F1dG9JbmNyZW1lbnRcIjpmYWxzZSxcIkNhbk51bGxcIjp0cnVlLFwiZGJUeXBlXCI6XCJiaXRcIixcImxlbmd0aFwiOlwiXCIsXCJkZWZhdWx0VmFsdWVcIjpcIjBcIixcIlRhYmxlSURcIjo2LFwiSXNQS0lEXCI6ZmFsc2UsXCJvcmRlcmlkXCI6NX1dLFwibmV3Q29sdW1uc1wiOlt7XCJpZFwiOjQ3LFwiTmFtZVwiOlwiU3VwcG9ydEVudW1Qb2ludHNcIixcIklzQXV0b0luY3JlbWVudFwiOmZhbHNlLFwiQ2FuTnVsbFwiOnRydWUsXCJkYlR5cGVcIjpcImJpdFwiLFwibGVuZ3RoXCI6XCJcIixcImRlZmF1bHRWYWx1ZVwiOlwiMFwiLFwiVGFibGVJRFwiOjYsXCJJc1BLSURcIjpmYWxzZSxcIm9yZGVyaWRcIjo2fV0sXCJjaGFuZ2VkQ29sdW1uc1wiOltdLFwiZGVsZXRlZENvbHVtbnNcIjpbXSxcIklEWENvbmZpZ3NcIjpbXSxcIklEXCI6MH0ifSx7Ik5hbWUiOiJkYXRhYmFzZWlkIiwiVmFsdWUiOjJ9XSwiUm93U3RhdGUiOjB9LHsiSXRlbXMiOlt7Ik5hbWUiOiJpZCIsIlZhbHVlIjoyOH0seyJOYW1lIjoidHlwZSIsIlZhbHVlIjoiQ2hhbmdlVGFibGVBY3Rpb24ifSx7Ik5hbWUiOiJjb250ZW50IiwiVmFsdWUiOiJ7XCJPbGRUYWJsZU5hbWVcIjpcIkRldmljZVBvaW50XCIsXCJOZXdUYWJsZU5hbWVcIjpcIkRldmljZVBvaW50XCIsXCJvdGhlckNvbHVtbnNcIjpbe1wiaWRcIjo0MixcIk5hbWVcIjpcImlkXCIsXCJJc0F1dG9JbmNyZW1lbnRcIjp0cnVlLFwiQ2FuTnVsbFwiOmZhbHNlLFwiZGJUeXBlXCI6XCJpbnRcIixcIlRhYmxlSURcIjo4LFwiSXNQS0lEXCI6dHJ1ZSxcIm9yZGVyaWRcIjowfSx7XCJpZFwiOjQzLFwiTmFtZVwiOlwiRGV2aWNlSURcIixcIklzQXV0b0luY3JlbWVudFwiOmZhbHNlLFwiQ2FuTnVsbFwiOnRydWUsXCJkYlR5cGVcIjpcImludFwiLFwibGVuZ3RoXCI6XCJcIixcIlRhYmxlSURcIjo4LFwiSXNQS0lEXCI6ZmFsc2UsXCJvcmRlcmlkXCI6MX0se1wiaWRcIjo0NCxcIk5hbWVcIjpcIkFkZHJlc3NcIixcIklzQXV0b0luY3JlbWVudFwiOmZhbHNlLFwiQ2FuTnVsbFwiOnRydWUsXCJkYlR5cGVcIjpcInZhcmNoYXJcIixcImxlbmd0aFwiOlwiMjAwXCIsXCJUYWJsZUlEXCI6OCxcIklzUEtJRFwiOmZhbHNlLFwib3JkZXJpZFwiOjJ9LHtcImlkXCI6NDUsXCJjYXB0aW9uXCI6XCLmmK/lkKbnm5HmtYvlj5jljJZcIixcIk5hbWVcIjpcIklzV2F0Y2hpbmdcIixcIklzQXV0b0luY3JlbWVudFwiOmZhbHNlLFwiQ2FuTnVsbFwiOnRydWUsXCJkYlR5cGVcIjpcImJpdFwiLFwibGVuZ3RoXCI6XCJcIixcImRlZmF1bHRWYWx1ZVwiOlwiMFwiLFwiVGFibGVJRFwiOjgsXCJJc1BLSURcIjpmYWxzZSxcIm9yZGVyaWRcIjozfV0sXCJuZXdDb2x1bW5zXCI6W3tcImlkXCI6NDgsXCJjYXB0aW9uXCI6XCLlnLDlnYDorr7nva7ml7bnmoRqc29u5a+56LGh6KGo6L6+XCIsXCJOYW1lXCI6XCJBZGRyU2V0dGluZ1wiLFwiSXNBdXRvSW5jcmVtZW50XCI6ZmFsc2UsXCJDYW5OdWxsXCI6dHJ1ZSxcImRiVHlwZVwiOlwidmFyY2hhclwiLFwibGVuZ3RoXCI6XCIyMDBcIixcIlRhYmxlSURcIjo4LFwiSXNQS0lEXCI6ZmFsc2UsXCJvcmRlcmlkXCI6M31dLFwiY2hhbmdlZENvbHVtbnNcIjpbXSxcImRlbGV0ZWRDb2x1bW5zXCI6W10sXCJJRFhDb25maWdzXCI6W10sXCJJRFwiOjB9In0seyJOYW1lIjoiZGF0YWJhc2VpZCIsIlZhbHVlIjoyfV0sIlJvd1N0YXRlIjowfSx7Ikl0ZW1zIjpbeyJOYW1lIjoiaWQiLCJWYWx1ZSI6Mjl9LHsiTmFtZSI6InR5cGUiLCJWYWx1ZSI6IkNoYW5nZVRhYmxlQWN0aW9uIn0seyJOYW1lIjoiY29udGVudCIsIlZhbHVlIjoie1wiT2xkVGFibGVOYW1lXCI6XCJEZXZpY2VQb2ludFwiLFwiTmV3VGFibGVOYW1lXCI6XCJEZXZpY2VQb2ludFwiLFwib3RoZXJDb2x1bW5zXCI6W3tcImlkXCI6NDIsXCJOYW1lXCI6XCJpZFwiLFwiSXNBdXRvSW5jcmVtZW50XCI6dHJ1ZSxcIkNhbk51bGxcIjpmYWxzZSxcImRiVHlwZVwiOlwiaW50XCIsXCJUYWJsZUlEXCI6OCxcIklzUEtJRFwiOnRydWUsXCJvcmRlcmlkXCI6MH0se1wiaWRcIjo0NCxcIk5hbWVcIjpcIkFkZHJlc3NcIixcIklzQXV0b0luY3JlbWVudFwiOmZhbHNlLFwiQ2FuTnVsbFwiOnRydWUsXCJkYlR5cGVcIjpcInZhcmNoYXJcIixcImxlbmd0aFwiOlwiMjAwXCIsXCJUYWJsZUlEXCI6OCxcIklzUEtJRFwiOmZhbHNlLFwib3JkZXJpZFwiOjJ9LHtcImlkXCI6NDUsXCJjYXB0aW9uXCI6XCLmmK/lkKbnm5HmtYvlj5jljJZcIixcIk5hbWVcIjpcIklzV2F0Y2hpbmdcIixcIklzQXV0b0luY3JlbWVudFwiOmZhbHNlLFwiQ2FuTnVsbFwiOnRydWUsXCJkYlR5cGVcIjpcImJpdFwiLFwibGVuZ3RoXCI6XCJcIixcImRlZmF1bHRWYWx1ZVwiOlwiMFwiLFwiVGFibGVJRFwiOjgsXCJJc1BLSURcIjpmYWxzZSxcIm9yZGVyaWRcIjo0fSx7XCJpZFwiOjQ4LFwiY2FwdGlvblwiOlwi5Zyw5Z2A6K6+572u5pe255qEanNvbuWvueixoeihqOi+vlwiLFwiTmFtZVwiOlwiQWRkclNldHRpbmdcIixcIklzQXV0b0luY3JlbWVudFwiOmZhbHNlLFwiQ2FuTnVsbFwiOnRydWUsXCJkYlR5cGVcIjpcInZhcmNoYXJcIixcImxlbmd0aFwiOlwiMjAwXCIsXCJUYWJsZUlEXCI6OCxcIklzUEtJRFwiOmZhbHNlLFwib3JkZXJpZFwiOjN9XSxcIm5ld0NvbHVtbnNcIjpbe1wiaWRcIjo0OSxcImNhcHRpb25cIjpcIuaPj+i/sFwiLFwiTmFtZVwiOlwiRGVzY1wiLFwiSXNBdXRvSW5jcmVtZW50XCI6ZmFsc2UsXCJDYW5OdWxsXCI6dHJ1ZSxcImRiVHlwZVwiOlwidmFyY2hhclwiLFwibGVuZ3RoXCI6XCIxMDBcIixcIlRhYmxlSURcIjo4LFwiSXNQS0lEXCI6ZmFsc2UsXCJvcmRlcmlkXCI6Mn0se1wiaWRcIjo1MCxcImNhcHRpb25cIjpcIuWIneWni+WAvFwiLFwiTmFtZVwiOlwiSW5pdFZhbHVlXCIsXCJJc0F1dG9JbmNyZW1lbnRcIjpmYWxzZSxcIkNhbk51bGxcIjp0cnVlLFwiZGJUeXBlXCI6XCJkb3VibGVcIixcImxlbmd0aFwiOlwiXCIsXCJUYWJsZUlEXCI6OCxcIklzUEtJRFwiOmZhbHNlLFwib3JkZXJpZFwiOjN9LHtcImlkXCI6NTEsXCJjYXB0aW9uXCI6XCLlvIDnirbmgIHkv6Hmga9cIixcIk5hbWVcIjpcIlN0YXR1c09wZW5JbmZvXCIsXCJJc0F1dG9JbmNyZW1lbnRcIjpmYWxzZSxcIkNhbk51bGxcIjp0cnVlLFwiZGJUeXBlXCI6XCJ2YXJjaGFyXCIsXCJsZW5ndGhcIjpcIjUwXCIsXCJUYWJsZUlEXCI6OCxcIklzUEtJRFwiOmZhbHNlLFwib3JkZXJpZFwiOjR9LHtcImlkXCI6NTIsXCJjYXB0aW9uXCI6XCLlhbPnirbmgIHkv6Hmga9cIixcIk5hbWVcIjpcIlN0YXRlQ2xvc2VJbmZvXCIsXCJJc0F1dG9JbmNyZW1lbnRcIjpmYWxzZSxcIkNhbk51bGxcIjp0cnVlLFwiZGJUeXBlXCI6XCJ2YXJjaGFyXCIsXCJsZW5ndGhcIjpcIjUwXCIsXCJUYWJsZUlEXCI6OCxcIklzUEtJRFwiOmZhbHNlLFwib3JkZXJpZFwiOjV9LHtcImlkXCI6NTMsXCJjYXB0aW9uXCI6XCLmiqXorablvIDlhbNcIixcIk5hbWVcIjpcIklzQWxhcm1cIixcIklzQXV0b0luY3JlbWVudFwiOmZhbHNlLFwiQ2FuTnVsbFwiOnRydWUsXCJkYlR5cGVcIjpcImJpdFwiLFwibGVuZ3RoXCI6XCJcIixcImRlZmF1bHRWYWx1ZVwiOlwiMFwiLFwiVGFibGVJRFwiOjgsXCJJc1BLSURcIjpmYWxzZSxcIm9yZGVyaWRcIjo4fSx7XCJpZFwiOjU0LFwiY2FwdGlvblwiOlwi5oql6K2m5YC8XCIsXCJOYW1lXCI6XCJBbGFybVZhbHVlXCIsXCJJc0F1dG9JbmNyZW1lbnRcIjpmYWxzZSxcIkNhbk51bGxcIjp0cnVlLFwiZGJUeXBlXCI6XCJkb3VibGVcIixcImxlbmd0aFwiOlwiXCIsXCJUYWJsZUlEXCI6OCxcIklzUEtJRFwiOmZhbHNlLFwib3JkZXJpZFwiOjl9LHtcImlkXCI6NTUsXCJjYXB0aW9uXCI6XCLmiqXorabkvJjlhYjnuqdcIixcIk5hbWVcIjpcIkFsYXJtUHJpb3JpdHlcIixcIklzQXV0b0luY3JlbWVudFwiOmZhbHNlLFwiQ2FuTnVsbFwiOnRydWUsXCJkYlR5cGVcIjpcImludFwiLFwibGVuZ3RoXCI6XCJcIixcIlRhYmxlSURcIjo4LFwiSXNQS0lEXCI6ZmFsc2UsXCJvcmRlcmlkXCI6MTB9LHtcImlkXCI6NTYsXCJjYXB0aW9uXCI6XCLmiqXorabnu4RcIixcIk5hbWVcIjpcIkFsYXJtR3JvdXBcIixcIklzQXV0b0luY3JlbWVudFwiOmZhbHNlLFwiQ2FuTnVsbFwiOnRydWUsXCJkYlR5cGVcIjpcInZhcmNoYXJcIixcImxlbmd0aFwiOlwiNTBcIixcIlRhYmxlSURcIjo4LFwiSXNQS0lEXCI6ZmFsc2UsXCJvcmRlcmlkXCI6MTF9LHtcImlkXCI6NTcsXCJjYXB0aW9uXCI6XCLmiqXoraboh6rliqjnoa7orqRcIixcIk5hbWVcIjpcIkFsYXJtQXV0b0NvbmZpcm1cIixcIklzQXV0b0luY3JlbWVudFwiOmZhbHNlLFwiQ2FuTnVsbFwiOnRydWUsXCJkYlR5cGVcIjpcImJpdFwiLFwibGVuZ3RoXCI6XCJcIixcImRlZmF1bHRWYWx1ZVwiOlwiMFwiLFwiVGFibGVJRFwiOjgsXCJJc1BLSURcIjpmYWxzZSxcIm9yZGVyaWRcIjoxMn0se1wiaWRcIjo1OCxcImNhcHRpb25cIjpcIuaKpeitpuiHquWKqOWkjeS9jVwiLFwiTmFtZVwiOlwiQWxhcm1BdXRvUmVzZXRcIixcIklzQXV0b0luY3JlbWVudFwiOmZhbHNlLFwiQ2FuTnVsbFwiOnRydWUsXCJkYlR5cGVcIjpcImJpdFwiLFwibGVuZ3RoXCI6XCJcIixcImRlZmF1bHRWYWx1ZVwiOlwiMFwiLFwiVGFibGVJRFwiOjgsXCJJc1BLSURcIjpmYWxzZSxcIm9yZGVyaWRcIjoxM30se1wiaWRcIjo1OSxcImNhcHRpb25cIjpcIuaVsOaNrue7neWvueWPmOWMluS/neWtmFwiLFwiTmFtZVwiOlwiVmFsdWVBYnNvbHV0ZUNoYW5nZVwiLFwiSXNBdXRvSW5jcmVtZW50XCI6ZmFsc2UsXCJDYW5OdWxsXCI6dHJ1ZSxcImRiVHlwZVwiOlwiYml0XCIsXCJsZW5ndGhcIjpcIlwiLFwiZGVmYXVsdFZhbHVlXCI6XCIwXCIsXCJUYWJsZUlEXCI6OCxcIklzUEtJRFwiOmZhbHNlLFwib3JkZXJpZFwiOjE0fSx7XCJpZFwiOjYwLFwiY2FwdGlvblwiOlwi5pWw5o2u57ud5a+55Y+Y5YyW5YC8XCIsXCJOYW1lXCI6XCJWYWx1ZUFic29sdXRlQ2hhbmdlU2V0dGluZ1wiLFwiSXNBdXRvSW5jcmVtZW50XCI6ZmFsc2UsXCJDYW5OdWxsXCI6dHJ1ZSxcImRiVHlwZVwiOlwiZG91YmxlXCIsXCJsZW5ndGhcIjpcIlwiLFwiVGFibGVJRFwiOjgsXCJJc1BLSURcIjpmYWxzZSxcIm9yZGVyaWRcIjoxNX0se1wiaWRcIjo2MSxcImNhcHRpb25cIjpcIuaVsOaNruebuOWvueWPmOWMluS/neWtmFwiLFwiTmFtZVwiOlwiVmFsdWVSZWxhdGl2ZUNoYW5nZVwiLFwiSXNBdXRvSW5jcmVtZW50XCI6ZmFsc2UsXCJDYW5OdWxsXCI6dHJ1ZSxcImRiVHlwZVwiOlwiYml0XCIsXCJsZW5ndGhcIjpcIlwiLFwiZGVmYXVsdFZhbHVlXCI6XCIwXCIsXCJUYWJsZUlEXCI6OCxcIklzUEtJRFwiOmZhbHNlLFwib3JkZXJpZFwiOjE2fSx7XCJpZFwiOjYyLFwiY2FwdGlvblwiOlwi5pWw5o2u55u45a+55Y+Y5YyW5YC8XCIsXCJOYW1lXCI6XCJWYWx1ZVJlbGF0aXZlQ2hhbmdlU2V0dGluZ1wiLFwiSXNBdXRvSW5jcmVtZW50XCI6ZmFsc2UsXCJDYW5OdWxsXCI6dHJ1ZSxcImRiVHlwZVwiOlwiZG91YmxlXCIsXCJsZW5ndGhcIjpcIlwiLFwiVGFibGVJRFwiOjgsXCJJc1BLSURcIjpmYWxzZSxcIm9yZGVyaWRcIjoxN30se1wiaWRcIjo2MyxcImNhcHRpb25cIjpcIuaVsOaNruWumuaXtuS/neWtmFwiLFwiTmFtZVwiOlwiVmFsdWVPblRpbWVDaGFuZ2VcIixcIklzQXV0b0luY3JlbWVudFwiOmZhbHNlLFwiQ2FuTnVsbFwiOnRydWUsXCJkYlR5cGVcIjpcImJpdFwiLFwibGVuZ3RoXCI6XCJcIixcImRlZmF1bHRWYWx1ZVwiOlwiMFwiLFwiVGFibGVJRFwiOjgsXCJJc1BLSURcIjpmYWxzZSxcIm9yZGVyaWRcIjoxOH0se1wiaWRcIjo2NCxcImNhcHRpb25cIjpcIuaVsOaNruWumuaXtuWAvFwiLFwiTmFtZVwiOlwiVmFsdWVPblRpbWVDaGFuZ2VTZXR0aW5nXCIsXCJJc0F1dG9JbmNyZW1lbnRcIjpmYWxzZSxcIkNhbk51bGxcIjp0cnVlLFwiZGJUeXBlXCI6XCJkb3VibGVcIixcImxlbmd0aFwiOlwiXCIsXCJUYWJsZUlEXCI6OCxcIklzUEtJRFwiOmZhbHNlLFwib3JkZXJpZFwiOjE5fV0sXCJjaGFuZ2VkQ29sdW1uc1wiOlt7XCJpZFwiOjQzLFwiTmFtZVwiOlwiTmFtZVwiLFwiSXNBdXRvSW5jcmVtZW50XCI6ZmFsc2UsXCJDYW5OdWxsXCI6dHJ1ZSxcImRiVHlwZVwiOlwiaW50XCIsXCJsZW5ndGhcIjpcIlwiLFwiVGFibGVJRFwiOjgsXCJJc1BLSURcIjpmYWxzZSxcIm9yZGVyaWRcIjoxLFwiQmFja3VwQ2hhbmdlZFByb3BlcnRpZXNcIjp7XCJOYW1lXCI6e1wiT3JpZ2luYWxWYWx1ZVwiOlwiRGV2aWNlSURcIn19fV0sXCJkZWxldGVkQ29sdW1uc1wiOltdLFwiSURYQ29uZmlnc1wiOltdLFwiSURcIjowfSJ9LHsiTmFtZSI6ImRhdGFiYXNlaWQiLCJWYWx1ZSI6Mn1dLCJSb3dTdGF0ZSI6MH0seyJJdGVtcyI6W3siTmFtZSI6ImlkIiwiVmFsdWUiOjMwfSx7Ik5hbWUiOiJ0eXBlIiwiVmFsdWUiOiJDaGFuZ2VUYWJsZUFjdGlvbiJ9LHsiTmFtZSI6ImNvbnRlbnQiLCJWYWx1ZSI6IntcIk9sZFRhYmxlTmFtZVwiOlwiRGV2aWNlUG9pbnRcIixcIk5ld1RhYmxlTmFtZVwiOlwiRGV2aWNlUG9pbnRcIixcIm90aGVyQ29sdW1uc1wiOlt7XCJpZFwiOjQyLFwiTmFtZVwiOlwiaWRcIixcIklzQXV0b0luY3JlbWVudFwiOnRydWUsXCJDYW5OdWxsXCI6ZmFsc2UsXCJkYlR5cGVcIjpcImludFwiLFwiVGFibGVJRFwiOjgsXCJJc1BLSURcIjp0cnVlLFwib3JkZXJpZFwiOjB9LHtcImlkXCI6NDMsXCJjYXB0aW9uXCI6XCLngrnlkI1cIixcIk5hbWVcIjpcIk5hbWVcIixcIklzQXV0b0luY3JlbWVudFwiOmZhbHNlLFwiQ2FuTnVsbFwiOnRydWUsXCJkYlR5cGVcIjpcImludFwiLFwibGVuZ3RoXCI6XCJcIixcIlRhYmxlSURcIjo4LFwiSXNQS0lEXCI6ZmFsc2UsXCJvcmRlcmlkXCI6MX0se1wiaWRcIjo0NCxcIk5hbWVcIjpcIkFkZHJlc3NcIixcIklzQXV0b0luY3JlbWVudFwiOmZhbHNlLFwiQ2FuTnVsbFwiOnRydWUsXCJkYlR5cGVcIjpcInZhcmNoYXJcIixcImxlbmd0aFwiOlwiMjAwXCIsXCJUYWJsZUlEXCI6OCxcIklzUEtJRFwiOmZhbHNlLFwib3JkZXJpZFwiOjZ9LHtcImlkXCI6NDUsXCJjYXB0aW9uXCI6XCLmmK/lkKbnm5HmtYvlj5jljJZcIixcIk5hbWVcIjpcIklzV2F0Y2hpbmdcIixcIklzQXV0b0luY3JlbWVudFwiOmZhbHNlLFwiQ2FuTnVsbFwiOnRydWUsXCJkYlR5cGVcIjpcImJpdFwiLFwibGVuZ3RoXCI6XCJcIixcImRlZmF1bHRWYWx1ZVwiOlwiMFwiLFwiVGFibGVJRFwiOjgsXCJJc1BLSURcIjpmYWxzZSxcIm9yZGVyaWRcIjoyMH0se1wiaWRcIjo0OCxcImNhcHRpb25cIjpcIuWcsOWdgOiuvue9ruaXtueahGpzb27lr7nosaHooajovr5cIixcIk5hbWVcIjpcIkFkZHJTZXR0aW5nXCIsXCJJc0F1dG9JbmNyZW1lbnRcIjpmYWxzZSxcIkNhbk51bGxcIjp0cnVlLFwiZGJUeXBlXCI6XCJ2YXJjaGFyXCIsXCJsZW5ndGhcIjpcIjIwMFwiLFwiVGFibGVJRFwiOjgsXCJJc1BLSURcIjpmYWxzZSxcIm9yZGVyaWRcIjo3fSx7XCJpZFwiOjQ5LFwiY2FwdGlvblwiOlwi5o+P6L+wXCIsXCJOYW1lXCI6XCJEZXNjXCIsXCJJc0F1dG9JbmNyZW1lbnRcIjpmYWxzZSxcIkNhbk51bGxcIjp0cnVlLFwiZGJUeXBlXCI6XCJ2YXJjaGFyXCIsXCJsZW5ndGhcIjpcIjEwMFwiLFwiVGFibGVJRFwiOjgsXCJJc1BLSURcIjpmYWxzZSxcIm9yZGVyaWRcIjoyfSx7XCJpZFwiOjUwLFwiY2FwdGlvblwiOlwi5Yid5aeL5YC8XCIsXCJOYW1lXCI6XCJJbml0VmFsdWVcIixcIklzQXV0b0luY3JlbWVudFwiOmZhbHNlLFwiQ2FuTnVsbFwiOnRydWUsXCJkYlR5cGVcIjpcImRvdWJsZVwiLFwibGVuZ3RoXCI6XCJcIixcIlRhYmxlSURcIjo4LFwiSXNQS0lEXCI6ZmFsc2UsXCJvcmRlcmlkXCI6M30se1wiaWRcIjo1MSxcImNhcHRpb25cIjpcIuW8gOeKtuaAgeS/oeaBr1wiLFwiTmFtZVwiOlwiU3RhdHVzT3BlbkluZm9cIixcIklzQXV0b0luY3JlbWVudFwiOmZhbHNlLFwiQ2FuTnVsbFwiOnRydWUsXCJkYlR5cGVcIjpcInZhcmNoYXJcIixcImxlbmd0aFwiOlwiNTBcIixcIlRhYmxlSURcIjo4LFwiSXNQS0lEXCI6ZmFsc2UsXCJvcmRlcmlkXCI6NH0se1wiaWRcIjo1MixcImNhcHRpb25cIjpcIuWFs+eKtuaAgeS/oeaBr1wiLFwiTmFtZVwiOlwiU3RhdGVDbG9zZUluZm9cIixcIklzQXV0b0luY3JlbWVudFwiOmZhbHNlLFwiQ2FuTnVsbFwiOnRydWUsXCJkYlR5cGVcIjpcInZhcmNoYXJcIixcImxlbmd0aFwiOlwiNTBcIixcIlRhYmxlSURcIjo4LFwiSXNQS0lEXCI6ZmFsc2UsXCJvcmRlcmlkXCI6NX0se1wiaWRcIjo1MyxcImNhcHRpb25cIjpcIuaKpeitpuW8gOWFs1wiLFwiTmFtZVwiOlwiSXNBbGFybVwiLFwiSXNBdXRvSW5jcmVtZW50XCI6ZmFsc2UsXCJDYW5OdWxsXCI6dHJ1ZSxcImRiVHlwZVwiOlwiYml0XCIsXCJsZW5ndGhcIjpcIlwiLFwiZGVmYXVsdFZhbHVlXCI6XCIwXCIsXCJUYWJsZUlEXCI6OCxcIklzUEtJRFwiOmZhbHNlLFwib3JkZXJpZFwiOjh9LHtcImlkXCI6NTQsXCJjYXB0aW9uXCI6XCLmiqXorablgLxcIixcIk5hbWVcIjpcIkFsYXJtVmFsdWVcIixcIklzQXV0b0luY3JlbWVudFwiOmZhbHNlLFwiQ2FuTnVsbFwiOnRydWUsXCJkYlR5cGVcIjpcImRvdWJsZVwiLFwibGVuZ3RoXCI6XCJcIixcIlRhYmxlSURcIjo4LFwiSXNQS0lEXCI6ZmFsc2UsXCJvcmRlcmlkXCI6OX0se1wiaWRcIjo1NSxcImNhcHRpb25cIjpcIuaKpeitpuS8mOWFiOe6p1wiLFwiTmFtZVwiOlwiQWxhcm1Qcmlvcml0eVwiLFwiSXNBdXRvSW5jcmVtZW50XCI6ZmFsc2UsXCJDYW5OdWxsXCI6dHJ1ZSxcImRiVHlwZVwiOlwiaW50XCIsXCJsZW5ndGhcIjpcIlwiLFwiVGFibGVJRFwiOjgsXCJJc1BLSURcIjpmYWxzZSxcIm9yZGVyaWRcIjoxMH0se1wiaWRcIjo1NixcImNhcHRpb25cIjpcIuaKpeitpue7hFwiLFwiTmFtZVwiOlwiQWxhcm1Hcm91cFwiLFwiSXNBdXRvSW5jcmVtZW50XCI6ZmFsc2UsXCJDYW5OdWxsXCI6dHJ1ZSxcImRiVHlwZVwiOlwidmFyY2hhclwiLFwibGVuZ3RoXCI6XCI1MFwiLFwiVGFibGVJRFwiOjgsXCJJc1BLSURcIjpmYWxzZSxcIm9yZGVyaWRcIjoxMX0se1wiaWRcIjo1NyxcImNhcHRpb25cIjpcIuaKpeitpuiHquWKqOehruiupFwiLFwiTmFtZVwiOlwiQWxhcm1BdXRvQ29uZmlybVwiLFwiSXNBdXRvSW5jcmVtZW50XCI6ZmFsc2UsXCJDYW5OdWxsXCI6dHJ1ZSxcImRiVHlwZVwiOlwiYml0XCIsXCJsZW5ndGhcIjpcIlwiLFwiZGVmYXVsdFZhbHVlXCI6XCIwXCIsXCJUYWJsZUlEXCI6OCxcIklzUEtJRFwiOmZhbHNlLFwib3JkZXJpZFwiOjEyfSx7XCJpZFwiOjU4LFwiY2FwdGlvblwiOlwi5oql6K2m6Ieq5Yqo5aSN5L2NXCIsXCJOYW1lXCI6XCJBbGFybUF1dG9SZXNldFwiLFwiSXNBdXRvSW5jcmVtZW50XCI6ZmFsc2UsXCJDYW5OdWxsXCI6dHJ1ZSxcImRiVHlwZVwiOlwiYml0XCIsXCJsZW5ndGhcIjpcIlwiLFwiZGVmYXVsdFZhbHVlXCI6XCIwXCIsXCJUYWJsZUlEXCI6OCxcIklzUEtJRFwiOmZhbHNlLFwib3JkZXJpZFwiOjEzfSx7XCJpZFwiOjU5LFwiY2FwdGlvblwiOlwi5pWw5o2u57ud5a+55Y+Y5YyW5L+d5a2YXCIsXCJOYW1lXCI6XCJWYWx1ZUFic29sdXRlQ2hhbmdlXCIsXCJJc0F1dG9JbmNyZW1lbnRcIjpmYWxzZSxcIkNhbk51bGxcIjp0cnVlLFwiZGJUeXBlXCI6XCJiaXRcIixcImxlbmd0aFwiOlwiXCIsXCJkZWZhdWx0VmFsdWVcIjpcIjBcIixcIlRhYmxlSURcIjo4LFwiSXNQS0lEXCI6ZmFsc2UsXCJvcmRlcmlkXCI6MTR9LHtcImlkXCI6NjAsXCJjYXB0aW9uXCI6XCLmlbDmja7nu53lr7nlj5jljJblgLxcIixcIk5hbWVcIjpcIlZhbHVlQWJzb2x1dGVDaGFuZ2VTZXR0aW5nXCIsXCJJc0F1dG9JbmNyZW1lbnRcIjpmYWxzZSxcIkNhbk51bGxcIjp0cnVlLFwiZGJUeXBlXCI6XCJkb3VibGVcIixcImxlbmd0aFwiOlwiXCIsXCJUYWJsZUlEXCI6OCxcIklzUEtJRFwiOmZhbHNlLFwib3JkZXJpZFwiOjE1fSx7XCJpZFwiOjYxLFwiY2FwdGlvblwiOlwi5pWw5o2u55u45a+55Y+Y5YyW5L+d5a2YXCIsXCJOYW1lXCI6XCJWYWx1ZVJlbGF0aXZlQ2hhbmdlXCIsXCJJc0F1dG9JbmNyZW1lbnRcIjpmYWxzZSxcIkNhbk51bGxcIjp0cnVlLFwiZGJUeXBlXCI6XCJiaXRcIixcImxlbmd0aFwiOlwiXCIsXCJkZWZhdWx0VmFsdWVcIjpcIjBcIixcIlRhYmxlSURcIjo4LFwiSXNQS0lEXCI6ZmFsc2UsXCJvcmRlcmlkXCI6MTZ9LHtcImlkXCI6NjIsXCJjYXB0aW9uXCI6XCLmlbDmja7nm7jlr7nlj5jljJblgLxcIixcIk5hbWVcIjpcIlZhbHVlUmVsYXRpdmVDaGFuZ2VTZXR0aW5nXCIsXCJJc0F1dG9JbmNyZW1lbnRcIjpmYWxzZSxcIkNhbk51bGxcIjp0cnVlLFwiZGJUeXBlXCI6XCJkb3VibGVcIixcImxlbmd0aFwiOlwiXCIsXCJUYWJsZUlEXCI6OCxcIklzUEtJRFwiOmZhbHNlLFwib3JkZXJpZFwiOjE3fSx7XCJpZFwiOjYzLFwiY2FwdGlvblwiOlwi5pWw5o2u5a6a5pe25L+d5a2YXCIsXCJOYW1lXCI6XCJWYWx1ZU9uVGltZUNoYW5nZVwiLFwiSXNBdXRvSW5jcmVtZW50XCI6ZmFsc2UsXCJDYW5OdWxsXCI6dHJ1ZSxcImRiVHlwZVwiOlwiYml0XCIsXCJsZW5ndGhcIjpcIlwiLFwiZGVmYXVsdFZhbHVlXCI6XCIwXCIsXCJUYWJsZUlEXCI6OCxcIklzUEtJRFwiOmZhbHNlLFwib3JkZXJpZFwiOjE4fSx7XCJpZFwiOjY0LFwiY2FwdGlvblwiOlwi5pWw5o2u5a6a5pe25YC8XCIsXCJOYW1lXCI6XCJWYWx1ZU9uVGltZUNoYW5nZVNldHRpbmdcIixcIklzQXV0b0luY3JlbWVudFwiOmZhbHNlLFwiQ2FuTnVsbFwiOnRydWUsXCJkYlR5cGVcIjpcImRvdWJsZVwiLFwibGVuZ3RoXCI6XCJcIixcIlRhYmxlSURcIjo4LFwiSXNQS0lEXCI6ZmFsc2UsXCJvcmRlcmlkXCI6MTl9XSxcIm5ld0NvbHVtbnNcIjpbe1wiaWRcIjo2NSxcImNhcHRpb25cIjpcIueCueexu+Wei1wiLFwiTmFtZVwiOlwiVHlwZVwiLFwiSXNBdXRvSW5jcmVtZW50XCI6ZmFsc2UsXCJDYW5OdWxsXCI6dHJ1ZSxcImRiVHlwZVwiOlwiaW50XCIsXCJFbnVtRGVmaW5lXCI6XCJBbmFsb2cgPSAxLFxcbkRpZ2l0YWwgPSAyXCIsXCJsZW5ndGhcIjpcIlwiLFwiZGVmYXVsdFZhbHVlXCI6XCIyXCIsXCJUYWJsZUlEXCI6OCxcIklzUEtJRFwiOmZhbHNlLFwib3JkZXJpZFwiOjN9LHtcImlkXCI6NjYsXCJjYXB0aW9uXCI6XCLmqKHmi5/ph48t5bel56iL5Y2V5L2NXCIsXCJOYW1lXCI6XCJVbml0XCIsXCJJc0F1dG9JbmNyZW1lbnRcIjpmYWxzZSxcIkNhbk51bGxcIjp0cnVlLFwiZGJUeXBlXCI6XCJ2YXJjaGFyXCIsXCJsZW5ndGhcIjpcIjUwXCIsXCJUYWJsZUlEXCI6OCxcIklzUEtJRFwiOmZhbHNlLFwib3JkZXJpZFwiOjIxfSx7XCJpZFwiOjY3LFwiY2FwdGlvblwiOlwi5qih5ouf6YePLeWwj+aVsOeCueS9jeaVsFwiLFwiTmFtZVwiOlwiRFBDb3VudFwiLFwiSXNBdXRvSW5jcmVtZW50XCI6ZmFsc2UsXCJDYW5OdWxsXCI6dHJ1ZSxcImRiVHlwZVwiOlwidmFyY2hhclwiLFwibGVuZ3RoXCI6XCI1MFwiLFwiVGFibGVJRFwiOjgsXCJJc1BLSURcIjpmYWxzZSxcIm9yZGVyaWRcIjoyMn0se1wiaWRcIjo2OCxcImNhcHRpb25cIjpcIuaooeaLn+mHjy3ph4/nqIvovazljJblvIDlhbNcIixcIk5hbWVcIjpcIklzVHJhbnNmb3JtXCIsXCJJc0F1dG9JbmNyZW1lbnRcIjpmYWxzZSxcIkNhbk51bGxcIjp0cnVlLFwiZGJUeXBlXCI6XCJiaXRcIixcImxlbmd0aFwiOlwiXCIsXCJkZWZhdWx0VmFsdWVcIjpcIjBcIixcIlRhYmxlSURcIjo4LFwiSXNQS0lEXCI6ZmFsc2UsXCJvcmRlcmlkXCI6MjN9LHtcImlkXCI6NjksXCJjYXB0aW9uXCI6XCLmqKHmi5/ph48t6YeP56iL5LiK6ZmQXCIsXCJOYW1lXCI6XCJUcmFuc01heFwiLFwiSXNBdXRvSW5jcmVtZW50XCI6ZmFsc2UsXCJDYW5OdWxsXCI6dHJ1ZSxcImRiVHlwZVwiOlwiZG91YmxlXCIsXCJsZW5ndGhcIjpcIlwiLFwiVGFibGVJRFwiOjgsXCJJc1BLSURcIjpmYWxzZSxcIm9yZGVyaWRcIjoyNH0se1wiaWRcIjo3MCxcImNhcHRpb25cIjpcIuaooeaLn+mHjy3ph4/nqIvkuIvpmZBcIixcIk5hbWVcIjpcIlRyYW5zTWluXCIsXCJJc0F1dG9JbmNyZW1lbnRcIjpmYWxzZSxcIkNhbk51bGxcIjp0cnVlLFwiZGJUeXBlXCI6XCJkb3VibGVcIixcImxlbmd0aFwiOlwiXCIsXCJUYWJsZUlEXCI6OCxcIklzUEtJRFwiOmZhbHNlLFwib3JkZXJpZFwiOjI1fSx7XCJpZFwiOjcxLFwiY2FwdGlvblwiOlwi5qih5ouf6YePLeS8oOaEn+WZqOmHj+eoi+S4iumZkFwiLFwiTmFtZVwiOlwiU2Vuc29yTWF4XCIsXCJJc0F1dG9JbmNyZW1lbnRcIjpmYWxzZSxcIkNhbk51bGxcIjp0cnVlLFwiZGJUeXBlXCI6XCJkb3VibGVcIixcImxlbmd0aFwiOlwiXCIsXCJUYWJsZUlEXCI6OCxcIklzUEtJRFwiOmZhbHNlLFwib3JkZXJpZFwiOjI2fSx7XCJpZFwiOjcyLFwiY2FwdGlvblwiOlwi5qih5ouf6YePLeS8oOaEn+WZqOmHj+eoi+S4i+mZkFwiLFwiTmFtZVwiOlwiU2Vuc29yTWluXCIsXCJJc0F1dG9JbmNyZW1lbnRcIjpmYWxzZSxcIkNhbk51bGxcIjp0cnVlLFwiZGJUeXBlXCI6XCJ2YXJjaGFyXCIsXCJsZW5ndGhcIjpcIjUwXCIsXCJUYWJsZUlEXCI6OCxcIklzUEtJRFwiOmZhbHNlLFwib3JkZXJpZFwiOjI3fSx7XCJpZFwiOjczLFwiY2FwdGlvblwiOlwi5qih5ouf6YePLeW8gOW5s+aWuVwiLFwiTmFtZVwiOlwiSXNTcXVhcmVcIixcIklzQXV0b0luY3JlbWVudFwiOmZhbHNlLFwiQ2FuTnVsbFwiOnRydWUsXCJkYlR5cGVcIjpcImJpdFwiLFwibGVuZ3RoXCI6XCJcIixcImRlZmF1bHRWYWx1ZVwiOlwiMFwiLFwiVGFibGVJRFwiOjgsXCJJc1BLSURcIjpmYWxzZSxcIm9yZGVyaWRcIjoyOH0se1wiaWRcIjo3NCxcImNhcHRpb25cIjpcIuWIhuautee6v+aAp+WMllwiLFwiTmFtZVwiOlwiSXNMaW5lYXJcIixcIklzQXV0b0luY3JlbWVudFwiOmZhbHNlLFwiQ2FuTnVsbFwiOnRydWUsXCJkYlR5cGVcIjpcImJpdFwiLFwibGVuZ3RoXCI6XCJcIixcImRlZmF1bHRWYWx1ZVwiOlwiMFwiLFwiVGFibGVJRFwiOjgsXCJJc1BLSURcIjpmYWxzZSxcIm9yZGVyaWRcIjoyOX0se1wiaWRcIjo3NSxcImNhcHRpb25cIjpcIuWIhuautee6v+aAp+WMllgxXCIsXCJOYW1lXCI6XCJMaW5lYXJYMVwiLFwiSXNBdXRvSW5jcmVtZW50XCI6ZmFsc2UsXCJDYW5OdWxsXCI6dHJ1ZSxcImRiVHlwZVwiOlwiZG91YmxlXCIsXCJsZW5ndGhcIjpcIlwiLFwiVGFibGVJRFwiOjgsXCJJc1BLSURcIjpmYWxzZSxcIm9yZGVyaWRcIjozMH0se1wiaWRcIjo3NixcImNhcHRpb25cIjpcIuWIhuautee6v+aAp+WMllkxXCIsXCJOYW1lXCI6XCJMaW5lYXJZMVwiLFwiSXNBdXRvSW5jcmVtZW50XCI6ZmFsc2UsXCJDYW5OdWxsXCI6dHJ1ZSxcImRiVHlwZVwiOlwiZG91YmxlXCIsXCJsZW5ndGhcIjpcIlwiLFwiVGFibGVJRFwiOjgsXCJJc1BLSURcIjpmYWxzZSxcIm9yZGVyaWRcIjozMX0se1wiaWRcIjo3NyxcImNhcHRpb25cIjpcIuWIhuautee6v+aAp+WMllgyXCIsXCJOYW1lXCI6XCJMaW5lYXJYMlwiLFwiSXNBdXRvSW5jcmVtZW50XCI6ZmFsc2UsXCJDYW5OdWxsXCI6dHJ1ZSxcImRiVHlwZVwiOlwiZG91YmxlXCIsXCJsZW5ndGhcIjpcIlwiLFwiVGFibGVJRFwiOjgsXCJJc1BLSURcIjpmYWxzZSxcIm9yZGVyaWRcIjozMn0se1wiaWRcIjo3OCxcImNhcHRpb25cIjpcIuWIhuautee6v+aAp+WMllkyXCIsXCJOYW1lXCI6XCJMaW5lYXJZMlwiLFwiSXNBdXRvSW5jcmVtZW50XCI6ZmFsc2UsXCJDYW5OdWxsXCI6dHJ1ZSxcImRiVHlwZVwiOlwiZG91YmxlXCIsXCJsZW5ndGhcIjpcIlwiLFwiVGFibGVJRFwiOjgsXCJJc1BLSURcIjpmYWxzZSxcIm9yZGVyaWRcIjozM30se1wiaWRcIjo3OSxcIk5hbWVcIjpcIkxpbmVhclgzXCIsXCJJc0F1dG9JbmNyZW1lbnRcIjpmYWxzZSxcIkNhbk51bGxcIjp0cnVlLFwiZGJUeXBlXCI6XCJkb3VibGVcIixcImxlbmd0aFwiOlwiXCIsXCJUYWJsZUlEXCI6OCxcIklzUEtJRFwiOmZhbHNlLFwib3JkZXJpZFwiOjM0fSx7XCJpZFwiOjgwLFwiTmFtZVwiOlwiTGluZWFyWTNcIixcIklzQXV0b0luY3JlbWVudFwiOmZhbHNlLFwiQ2FuTnVsbFwiOnRydWUsXCJkYlR5cGVcIjpcImRvdWJsZVwiLFwibGVuZ3RoXCI6XCJcIixcIlRhYmxlSURcIjo4LFwiSXNQS0lEXCI6ZmFsc2UsXCJvcmRlcmlkXCI6MzV9LHtcImlkXCI6ODEsXCJOYW1lXCI6XCJMaW5lYXJYNFwiLFwiSXNBdXRvSW5jcmVtZW50XCI6ZmFsc2UsXCJDYW5OdWxsXCI6dHJ1ZSxcImRiVHlwZVwiOlwiZG91YmxlXCIsXCJsZW5ndGhcIjpcIlwiLFwiVGFibGVJRFwiOjgsXCJJc1BLSURcIjpmYWxzZSxcIm9yZGVyaWRcIjozNn0se1wiaWRcIjo4MixcIk5hbWVcIjpcIkxpbmVhclk0XCIsXCJJc0F1dG9JbmNyZW1lbnRcIjpmYWxzZSxcIkNhbk51bGxcIjp0cnVlLFwiZGJUeXBlXCI6XCJkb3VibGVcIixcImxlbmd0aFwiOlwiXCIsXCJUYWJsZUlEXCI6OCxcIklzUEtJRFwiOmZhbHNlLFwib3JkZXJpZFwiOjM3fV0sXCJjaGFuZ2VkQ29sdW1uc1wiOltdLFwiZGVsZXRlZENvbHVtbnNcIjpbXSxcIklEWENvbmZpZ3NcIjpbXSxcIklEXCI6MH0ifSx7Ik5hbWUiOiJkYXRhYmFzZWlkIiwiVmFsdWUiOjJ9XSwiUm93U3RhdGUiOjB9LHsiSXRlbXMiOlt7Ik5hbWUiOiJpZCIsIlZhbHVlIjozMX0seyJOYW1lIjoidHlwZSIsIlZhbHVlIjoiQ2hhbmdlVGFibGVBY3Rpb24ifSx7Ik5hbWUiOiJjb250ZW50IiwiVmFsdWUiOiJ7XCJPbGRUYWJsZU5hbWVcIjpcIkRldmljZVBvaW50XCIsXCJOZXdUYWJsZU5hbWVcIjpcIkRldmljZVBvaW50XCIsXCJvdGhlckNvbHVtbnNcIjpbe1wiaWRcIjo0MixcIk5hbWVcIjpcImlkXCIsXCJJc0F1dG9JbmNyZW1lbnRcIjp0cnVlLFwiQ2FuTnVsbFwiOmZhbHNlLFwiZGJUeXBlXCI6XCJpbnRcIixcIlRhYmxlSURcIjo4LFwiSXNQS0lEXCI6dHJ1ZSxcIm9yZGVyaWRcIjowfSx7XCJpZFwiOjQzLFwiY2FwdGlvblwiOlwi54K55ZCNXCIsXCJOYW1lXCI6XCJOYW1lXCIsXCJJc0F1dG9JbmNyZW1lbnRcIjpmYWxzZSxcIkNhbk51bGxcIjp0cnVlLFwiZGJUeXBlXCI6XCJpbnRcIixcImxlbmd0aFwiOlwiXCIsXCJUYWJsZUlEXCI6OCxcIklzUEtJRFwiOmZhbHNlLFwib3JkZXJpZFwiOjF9LHtcImlkXCI6NDQsXCJOYW1lXCI6XCJBZGRyZXNzXCIsXCJJc0F1dG9JbmNyZW1lbnRcIjpmYWxzZSxcIkNhbk51bGxcIjp0cnVlLFwiZGJUeXBlXCI6XCJ2YXJjaGFyXCIsXCJsZW5ndGhcIjpcIjIwMFwiLFwiVGFibGVJRFwiOjgsXCJJc1BLSURcIjpmYWxzZSxcIm9yZGVyaWRcIjo3fSx7XCJpZFwiOjQ1LFwiY2FwdGlvblwiOlwi5piv5ZCm55uR5rWL5Y+Y5YyWXCIsXCJOYW1lXCI6XCJJc1dhdGNoaW5nXCIsXCJJc0F1dG9JbmNyZW1lbnRcIjpmYWxzZSxcIkNhbk51bGxcIjp0cnVlLFwiZGJUeXBlXCI6XCJiaXRcIixcImxlbmd0aFwiOlwiXCIsXCJkZWZhdWx0VmFsdWVcIjpcIjBcIixcIlRhYmxlSURcIjo4LFwiSXNQS0lEXCI6ZmFsc2UsXCJvcmRlcmlkXCI6Mzh9LHtcImlkXCI6NDgsXCJjYXB0aW9uXCI6XCLlnLDlnYDorr7nva7ml7bnmoRqc29u5a+56LGh6KGo6L6+XCIsXCJOYW1lXCI6XCJBZGRyU2V0dGluZ1wiLFwiSXNBdXRvSW5jcmVtZW50XCI6ZmFsc2UsXCJDYW5OdWxsXCI6dHJ1ZSxcImRiVHlwZVwiOlwidmFyY2hhclwiLFwibGVuZ3RoXCI6XCIyMDBcIixcIlRhYmxlSURcIjo4LFwiSXNQS0lEXCI6ZmFsc2UsXCJvcmRlcmlkXCI6OH0se1wiaWRcIjo0OSxcImNhcHRpb25cIjpcIuaPj+i/sFwiLFwiTmFtZVwiOlwiRGVzY1wiLFwiSXNBdXRvSW5jcmVtZW50XCI6ZmFsc2UsXCJDYW5OdWxsXCI6dHJ1ZSxcImRiVHlwZVwiOlwidmFyY2hhclwiLFwibGVuZ3RoXCI6XCIxMDBcIixcIlRhYmxlSURcIjo4LFwiSXNQS0lEXCI6ZmFsc2UsXCJvcmRlcmlkXCI6Mn0se1wiaWRcIjo1MCxcImNhcHRpb25cIjpcIuWIneWni+WAvFwiLFwiTmFtZVwiOlwiSW5pdFZhbHVlXCIsXCJJc0F1dG9JbmNyZW1lbnRcIjpmYWxzZSxcIkNhbk51bGxcIjp0cnVlLFwiZGJUeXBlXCI6XCJkb3VibGVcIixcImxlbmd0aFwiOlwiXCIsXCJUYWJsZUlEXCI6OCxcIklzUEtJRFwiOmZhbHNlLFwib3JkZXJpZFwiOjR9LHtcImlkXCI6NTEsXCJjYXB0aW9uXCI6XCLlvIDnirbmgIHkv6Hmga9cIixcIk5hbWVcIjpcIlN0YXR1c09wZW5JbmZvXCIsXCJJc0F1dG9JbmNyZW1lbnRcIjpmYWxzZSxcIkNhbk51bGxcIjp0cnVlLFwiZGJUeXBlXCI6XCJ2YXJjaGFyXCIsXCJsZW5ndGhcIjpcIjUwXCIsXCJUYWJsZUlEXCI6OCxcIklzUEtJRFwiOmZhbHNlLFwib3JkZXJpZFwiOjV9LHtcImlkXCI6NTIsXCJjYXB0aW9uXCI6XCLlhbPnirbmgIHkv6Hmga9cIixcIk5hbWVcIjpcIlN0YXRlQ2xvc2VJbmZvXCIsXCJJc0F1dG9JbmNyZW1lbnRcIjpmYWxzZSxcIkNhbk51bGxcIjp0cnVlLFwiZGJUeXBlXCI6XCJ2YXJjaGFyXCIsXCJsZW5ndGhcIjpcIjUwXCIsXCJUYWJsZUlEXCI6OCxcIklzUEtJRFwiOmZhbHNlLFwib3JkZXJpZFwiOjZ9LHtcImlkXCI6NTMsXCJjYXB0aW9uXCI6XCLmiqXorablvIDlhbNcIixcIk5hbWVcIjpcIklzQWxhcm1cIixcIklzQXV0b0luY3JlbWVudFwiOmZhbHNlLFwiQ2FuTnVsbFwiOnRydWUsXCJkYlR5cGVcIjpcImJpdFwiLFwibGVuZ3RoXCI6XCJcIixcImRlZmF1bHRWYWx1ZVwiOlwiMFwiLFwiVGFibGVJRFwiOjgsXCJJc1BLSURcIjpmYWxzZSxcIm9yZGVyaWRcIjo5fSx7XCJpZFwiOjU0LFwiY2FwdGlvblwiOlwi5oql6K2m5YC8XCIsXCJOYW1lXCI6XCJBbGFybVZhbHVlXCIsXCJJc0F1dG9JbmNyZW1lbnRcIjpmYWxzZSxcIkNhbk51bGxcIjp0cnVlLFwiZGJUeXBlXCI6XCJkb3VibGVcIixcImxlbmd0aFwiOlwiXCIsXCJUYWJsZUlEXCI6OCxcIklzUEtJRFwiOmZhbHNlLFwib3JkZXJpZFwiOjEwfSx7XCJpZFwiOjU1LFwiY2FwdGlvblwiOlwi5oql6K2m5LyY5YWI57qnXCIsXCJOYW1lXCI6XCJBbGFybVByaW9yaXR5XCIsXCJJc0F1dG9JbmNyZW1lbnRcIjpmYWxzZSxcIkNhbk51bGxcIjp0cnVlLFwiZGJUeXBlXCI6XCJpbnRcIixcImxlbmd0aFwiOlwiXCIsXCJUYWJsZUlEXCI6OCxcIklzUEtJRFwiOmZhbHNlLFwib3JkZXJpZFwiOjExfSx7XCJpZFwiOjU2LFwiY2FwdGlvblwiOlwi5oql6K2m57uEXCIsXCJOYW1lXCI6XCJBbGFybUdyb3VwXCIsXCJJc0F1dG9JbmNyZW1lbnRcIjpmYWxzZSxcIkNhbk51bGxcIjp0cnVlLFwiZGJUeXBlXCI6XCJ2YXJjaGFyXCIsXCJsZW5ndGhcIjpcIjUwXCIsXCJUYWJsZUlEXCI6OCxcIklzUEtJRFwiOmZhbHNlLFwib3JkZXJpZFwiOjEyfSx7XCJpZFwiOjU3LFwiY2FwdGlvblwiOlwi5oql6K2m6Ieq5Yqo56Gu6K6kXCIsXCJOYW1lXCI6XCJBbGFybUF1dG9Db25maXJtXCIsXCJJc0F1dG9JbmNyZW1lbnRcIjpmYWxzZSxcIkNhbk51bGxcIjp0cnVlLFwiZGJUeXBlXCI6XCJiaXRcIixcImxlbmd0aFwiOlwiXCIsXCJkZWZhdWx0VmFsdWVcIjpcIjBcIixcIlRhYmxlSURcIjo4LFwiSXNQS0lEXCI6ZmFsc2UsXCJvcmRlcmlkXCI6MTN9LHtcImlkXCI6NTgsXCJjYXB0aW9uXCI6XCLmiqXoraboh6rliqjlpI3kvY1cIixcIk5hbWVcIjpcIkFsYXJtQXV0b1Jlc2V0XCIsXCJJc0F1dG9JbmNyZW1lbnRcIjpmYWxzZSxcIkNhbk51bGxcIjp0cnVlLFwiZGJUeXBlXCI6XCJiaXRcIixcImxlbmd0aFwiOlwiXCIsXCJkZWZhdWx0VmFsdWVcIjpcIjBcIixcIlRhYmxlSURcIjo4LFwiSXNQS0lEXCI6ZmFsc2UsXCJvcmRlcmlkXCI6MTR9LHtcImlkXCI6NTksXCJjYXB0aW9uXCI6XCLmlbDmja7nu53lr7nlj5jljJbkv53lrZhcIixcIk5hbWVcIjpcIlZhbHVlQWJzb2x1dGVDaGFuZ2VcIixcIklzQXV0b0luY3JlbWVudFwiOmZhbHNlLFwiQ2FuTnVsbFwiOnRydWUsXCJkYlR5cGVcIjpcImJpdFwiLFwibGVuZ3RoXCI6XCJcIixcImRlZmF1bHRWYWx1ZVwiOlwiMFwiLFwiVGFibGVJRFwiOjgsXCJJc1BLSURcIjpmYWxzZSxcIm9yZGVyaWRcIjoxNX0se1wiaWRcIjo2MCxcImNhcHRpb25cIjpcIuaVsOaNrue7neWvueWPmOWMluWAvFwiLFwiTmFtZVwiOlwiVmFsdWVBYnNvbHV0ZUNoYW5nZVNldHRpbmdcIixcIklzQXV0b0luY3JlbWVudFwiOmZhbHNlLFwiQ2FuTnVsbFwiOnRydWUsXCJkYlR5cGVcIjpcImRvdWJsZVwiLFwibGVuZ3RoXCI6XCJcIixcIlRhYmxlSURcIjo4LFwiSXNQS0lEXCI6ZmFsc2UsXCJvcmRlcmlkXCI6MTZ9LHtcImlkXCI6NjEsXCJjYXB0aW9uXCI6XCLmlbDmja7nm7jlr7nlj5jljJbkv53lrZhcIixcIk5hbWVcIjpcIlZhbHVlUmVsYXRpdmVDaGFuZ2VcIixcIklzQXV0b0luY3JlbWVudFwiOmZhbHNlLFwiQ2FuTnVsbFwiOnRydWUsXCJkYlR5cGVcIjpcImJpdFwiLFwibGVuZ3RoXCI6XCJcIixcImRlZmF1bHRWYWx1ZVwiOlwiMFwiLFwiVGFibGVJRFwiOjgsXCJJc1BLSURcIjpmYWxzZSxcIm9yZGVyaWRcIjoxN30se1wiaWRcIjo2MixcImNhcHRpb25cIjpcIuaVsOaNruebuOWvueWPmOWMluWAvFwiLFwiTmFtZVwiOlwiVmFsdWVSZWxhdGl2ZUNoYW5nZVNldHRpbmdcIixcIklzQXV0b0luY3JlbWVudFwiOmZhbHNlLFwiQ2FuTnVsbFwiOnRydWUsXCJkYlR5cGVcIjpcImRvdWJsZVwiLFwibGVuZ3RoXCI6XCJcIixcIlRhYmxlSURcIjo4LFwiSXNQS0lEXCI6ZmFsc2UsXCJvcmRlcmlkXCI6MTh9LHtcImlkXCI6NjMsXCJjYXB0aW9uXCI6XCLmlbDmja7lrprml7bkv53lrZhcIixcIk5hbWVcIjpcIlZhbHVlT25UaW1lQ2hhbmdlXCIsXCJJc0F1dG9JbmNyZW1lbnRcIjpmYWxzZSxcIkNhbk51bGxcIjp0cnVlLFwiZGJUeXBlXCI6XCJiaXRcIixcImxlbmd0aFwiOlwiXCIsXCJkZWZhdWx0VmFsdWVcIjpcIjBcIixcIlRhYmxlSURcIjo4LFwiSXNQS0lEXCI6ZmFsc2UsXCJvcmRlcmlkXCI6MTl9LHtcImlkXCI6NjQsXCJjYXB0aW9uXCI6XCLmlbDmja7lrprml7blgLxcIixcIk5hbWVcIjpcIlZhbHVlT25UaW1lQ2hhbmdlU2V0dGluZ1wiLFwiSXNBdXRvSW5jcmVtZW50XCI6ZmFsc2UsXCJDYW5OdWxsXCI6dHJ1ZSxcImRiVHlwZVwiOlwiZG91YmxlXCIsXCJsZW5ndGhcIjpcIlwiLFwiVGFibGVJRFwiOjgsXCJJc1BLSURcIjpmYWxzZSxcIm9yZGVyaWRcIjoyMH0se1wiaWRcIjo2NSxcImNhcHRpb25cIjpcIueCueexu+Wei1wiLFwiTmFtZVwiOlwiVHlwZVwiLFwiSXNBdXRvSW5jcmVtZW50XCI6ZmFsc2UsXCJDYW5OdWxsXCI6dHJ1ZSxcImRiVHlwZVwiOlwiaW50XCIsXCJFbnVtRGVmaW5lXCI6XCJBbmFsb2cgPSAxLFxcbkRpZ2l0YWwgPSAyXCIsXCJsZW5ndGhcIjpcIlwiLFwiZGVmYXVsdFZhbHVlXCI6XCIyXCIsXCJUYWJsZUlEXCI6OCxcIklzUEtJRFwiOmZhbHNlLFwib3JkZXJpZFwiOjN9LHtcImlkXCI6NjYsXCJjYXB0aW9uXCI6XCLmqKHmi5/ph48t5bel56iL5Y2V5L2NXCIsXCJOYW1lXCI6XCJVbml0XCIsXCJJc0F1dG9JbmNyZW1lbnRcIjpmYWxzZSxcIkNhbk51bGxcIjp0cnVlLFwiZGJUeXBlXCI6XCJ2YXJjaGFyXCIsXCJsZW5ndGhcIjpcIjUwXCIsXCJUYWJsZUlEXCI6OCxcIklzUEtJRFwiOmZhbHNlLFwib3JkZXJpZFwiOjIxfSx7XCJpZFwiOjY3LFwiY2FwdGlvblwiOlwi5qih5ouf6YePLeWwj+aVsOeCueS9jeaVsFwiLFwiTmFtZVwiOlwiRFBDb3VudFwiLFwiSXNBdXRvSW5jcmVtZW50XCI6ZmFsc2UsXCJDYW5OdWxsXCI6dHJ1ZSxcImRiVHlwZVwiOlwidmFyY2hhclwiLFwibGVuZ3RoXCI6XCI1MFwiLFwiVGFibGVJRFwiOjgsXCJJc1BLSURcIjpmYWxzZSxcIm9yZGVyaWRcIjoyMn0se1wiaWRcIjo2OCxcImNhcHRpb25cIjpcIuaooeaLn+mHjy3ph4/nqIvovazljJblvIDlhbNcIixcIk5hbWVcIjpcIklzVHJhbnNmb3JtXCIsXCJJc0F1dG9JbmNyZW1lbnRcIjpmYWxzZSxcIkNhbk51bGxcIjp0cnVlLFwiZGJUeXBlXCI6XCJiaXRcIixcImxlbmd0aFwiOlwiXCIsXCJkZWZhdWx0VmFsdWVcIjpcIjBcIixcIlRhYmxlSURcIjo4LFwiSXNQS0lEXCI6ZmFsc2UsXCJvcmRlcmlkXCI6MjN9LHtcImlkXCI6NjksXCJjYXB0aW9uXCI6XCLmqKHmi5/ph48t6YeP56iL5LiK6ZmQXCIsXCJOYW1lXCI6XCJUcmFuc01heFwiLFwiSXNBdXRvSW5jcmVtZW50XCI6ZmFsc2UsXCJDYW5OdWxsXCI6dHJ1ZSxcImRiVHlwZVwiOlwiZG91YmxlXCIsXCJsZW5ndGhcIjpcIlwiLFwiVGFibGVJRFwiOjgsXCJJc1BLSURcIjpmYWxzZSxcIm9yZGVyaWRcIjoyNH0se1wiaWRcIjo3MCxcImNhcHRpb25cIjpcIuaooeaLn+mHjy3ph4/nqIvkuIvpmZBcIixcIk5hbWVcIjpcIlRyYW5zTWluXCIsXCJJc0F1dG9JbmNyZW1lbnRcIjpmYWxzZSxcIkNhbk51bGxcIjp0cnVlLFwiZGJUeXBlXCI6XCJkb3VibGVcIixcImxlbmd0aFwiOlwiXCIsXCJUYWJsZUlEXCI6OCxcIklzUEtJRFwiOmZhbHNlLFwib3JkZXJpZFwiOjI1fSx7XCJpZFwiOjcxLFwiY2FwdGlvblwiOlwi5qih5ouf6YePLeS8oOaEn+WZqOmHj+eoi+S4iumZkFwiLFwiTmFtZVwiOlwiU2Vuc29yTWF4XCIsXCJJc0F1dG9JbmNyZW1lbnRcIjpmYWxzZSxcIkNhbk51bGxcIjp0cnVlLFwiZGJUeXBlXCI6XCJkb3VibGVcIixcImxlbmd0aFwiOlwiXCIsXCJUYWJsZUlEXCI6OCxcIklzUEtJRFwiOmZhbHNlLFwib3JkZXJpZFwiOjI2fSx7XCJpZFwiOjcyLFwiY2FwdGlvblwiOlwi5qih5ouf6YePLeS8oOaEn+WZqOmHj+eoi+S4i+mZkFwiLFwiTmFtZVwiOlwiU2Vuc29yTWluXCIsXCJJc0F1dG9JbmNyZW1lbnRcIjpmYWxzZSxcIkNhbk51bGxcIjp0cnVlLFwiZGJUeXBlXCI6XCJ2YXJjaGFyXCIsXCJsZW5ndGhcIjpcIjUwXCIsXCJUYWJsZUlEXCI6OCxcIklzUEtJRFwiOmZhbHNlLFwib3JkZXJpZFwiOjI3fSx7XCJpZFwiOjczLFwiY2FwdGlvblwiOlwi5qih5ouf6YePLeW8gOW5s+aWuVwiLFwiTmFtZVwiOlwiSXNTcXVhcmVcIixcIklzQXV0b0luY3JlbWVudFwiOmZhbHNlLFwiQ2FuTnVsbFwiOnRydWUsXCJkYlR5cGVcIjpcImJpdFwiLFwibGVuZ3RoXCI6XCJcIixcImRlZmF1bHRWYWx1ZVwiOlwiMFwiLFwiVGFibGVJRFwiOjgsXCJJc1BLSURcIjpmYWxzZSxcIm9yZGVyaWRcIjoyOH0se1wiaWRcIjo3NCxcImNhcHRpb25cIjpcIuWIhuautee6v+aAp+WMllwiLFwiTmFtZVwiOlwiSXNMaW5lYXJcIixcIklzQXV0b0luY3JlbWVudFwiOmZhbHNlLFwiQ2FuTnVsbFwiOnRydWUsXCJkYlR5cGVcIjpcImJpdFwiLFwibGVuZ3RoXCI6XCJcIixcImRlZmF1bHRWYWx1ZVwiOlwiMFwiLFwiVGFibGVJRFwiOjgsXCJJc1BLSURcIjpmYWxzZSxcIm9yZGVyaWRcIjoyOX0se1wiaWRcIjo3NSxcImNhcHRpb25cIjpcIuWIhuautee6v+aAp+WMllgxXCIsXCJOYW1lXCI6XCJMaW5lYXJYMVwiLFwiSXNBdXRvSW5jcmVtZW50XCI6ZmFsc2UsXCJDYW5OdWxsXCI6dHJ1ZSxcImRiVHlwZVwiOlwiZG91YmxlXCIsXCJsZW5ndGhcIjpcIlwiLFwiVGFibGVJRFwiOjgsXCJJc1BLSURcIjpmYWxzZSxcIm9yZGVyaWRcIjozMH0se1wiaWRcIjo3NixcImNhcHRpb25cIjpcIuWIhuautee6v+aAp+WMllkxXCIsXCJOYW1lXCI6XCJMaW5lYXJZMVwiLFwiSXNBdXRvSW5jcmVtZW50XCI6ZmFsc2UsXCJDYW5OdWxsXCI6dHJ1ZSxcImRiVHlwZVwiOlwiZG91YmxlXCIsXCJsZW5ndGhcIjpcIlwiLFwiVGFibGVJRFwiOjgsXCJJc1BLSURcIjpmYWxzZSxcIm9yZGVyaWRcIjozMX0se1wiaWRcIjo3NyxcImNhcHRpb25cIjpcIuWIhuautee6v+aAp+WMllgyXCIsXCJOYW1lXCI6XCJMaW5lYXJYMlwiLFwiSXNBdXRvSW5jcmVtZW50XCI6ZmFsc2UsXCJDYW5OdWxsXCI6dHJ1ZSxcImRiVHlwZVwiOlwiZG91YmxlXCIsXCJsZW5ndGhcIjpcIlwiLFwiVGFibGVJRFwiOjgsXCJJc1BLSURcIjpmYWxzZSxcIm9yZGVyaWRcIjozMn0se1wiaWRcIjo3OCxcImNhcHRpb25cIjpcIuWIhuautee6v+aAp+WMllkyXCIsXCJOYW1lXCI6XCJMaW5lYXJZMlwiLFwiSXNBdXRvSW5jcmVtZW50XCI6ZmFsc2UsXCJDYW5OdWxsXCI6dHJ1ZSxcImRiVHlwZVwiOlwiZG91YmxlXCIsXCJsZW5ndGhcIjpcIlwiLFwiVGFibGVJRFwiOjgsXCJJc1BLSURcIjpmYWxzZSxcIm9yZGVyaWRcIjozM30se1wiaWRcIjo3OSxcIk5hbWVcIjpcIkxpbmVhclgzXCIsXCJJc0F1dG9JbmNyZW1lbnRcIjpmYWxzZSxcIkNhbk51bGxcIjp0cnVlLFwiZGJUeXBlXCI6XCJkb3VibGVcIixcImxlbmd0aFwiOlwiXCIsXCJUYWJsZUlEXCI6OCxcIklzUEtJRFwiOmZhbHNlLFwib3JkZXJpZFwiOjM0fSx7XCJpZFwiOjgwLFwiTmFtZVwiOlwiTGluZWFyWTNcIixcIklzQXV0b0luY3JlbWVudFwiOmZhbHNlLFwiQ2FuTnVsbFwiOnRydWUsXCJkYlR5cGVcIjpcImRvdWJsZVwiLFwibGVuZ3RoXCI6XCJcIixcIlRhYmxlSURcIjo4LFwiSXNQS0lEXCI6ZmFsc2UsXCJvcmRlcmlkXCI6MzV9LHtcImlkXCI6ODEsXCJOYW1lXCI6XCJMaW5lYXJYNFwiLFwiSXNBdXRvSW5jcmVtZW50XCI6ZmFsc2UsXCJDYW5OdWxsXCI6dHJ1ZSxcImRiVHlwZVwiOlwiZG91YmxlXCIsXCJsZW5ndGhcIjpcIlwiLFwiVGFibGVJRFwiOjgsXCJJc1BLSURcIjpmYWxzZSxcIm9yZGVyaWRcIjozNn1dLFwibmV3Q29sdW1uc1wiOlt7XCJpZFwiOjgzLFwiY2FwdGlvblwiOlwi5oql6K2m5LyY5YWI57qnMlwiLFwiTmFtZVwiOlwiQWxhcm1Qcmlvcml0eTJcIixcIklzQXV0b0luY3JlbWVudFwiOmZhbHNlLFwiQ2FuTnVsbFwiOnRydWUsXCJkYlR5cGVcIjpcImludFwiLFwibGVuZ3RoXCI6XCJcIixcIlRhYmxlSURcIjo4LFwiSXNQS0lEXCI6ZmFsc2UsXCJvcmRlcmlkXCI6Mzh9LHtcImlkXCI6ODQsXCJOYW1lXCI6XCJBbGFybVZhbHVlM1wiLFwiSXNBdXRvSW5jcmVtZW50XCI6ZmFsc2UsXCJDYW5OdWxsXCI6dHJ1ZSxcImRiVHlwZVwiOlwiZG91YmxlXCIsXCJsZW5ndGhcIjpcIlwiLFwiVGFibGVJRFwiOjgsXCJJc1BLSURcIjpmYWxzZSxcIm9yZGVyaWRcIjozOX0se1wiaWRcIjo4NSxcIk5hbWVcIjpcIkFsYXJtUHJpb3JpdHkzXCIsXCJJc0F1dG9JbmNyZW1lbnRcIjpmYWxzZSxcIkNhbk51bGxcIjp0cnVlLFwiZGJUeXBlXCI6XCJpbnRcIixcImxlbmd0aFwiOlwiXCIsXCJUYWJsZUlEXCI6OCxcIklzUEtJRFwiOmZhbHNlLFwib3JkZXJpZFwiOjQwfSx7XCJpZFwiOjg2LFwiTmFtZVwiOlwiQWxhcm1WYWx1ZTRcIixcIklzQXV0b0luY3JlbWVudFwiOmZhbHNlLFwiQ2FuTnVsbFwiOnRydWUsXCJkYlR5cGVcIjpcImRvdWJsZVwiLFwibGVuZ3RoXCI6XCJcIixcIlRhYmxlSURcIjo4LFwiSXNQS0lEXCI6ZmFsc2UsXCJvcmRlcmlkXCI6NDF9LHtcImlkXCI6ODcsXCJOYW1lXCI6XCJBbGFybVByaW9yaXR5NFwiLFwiSXNBdXRvSW5jcmVtZW50XCI6ZmFsc2UsXCJDYW5OdWxsXCI6dHJ1ZSxcImRiVHlwZVwiOlwiaW50XCIsXCJsZW5ndGhcIjpcIlwiLFwiVGFibGVJRFwiOjgsXCJJc1BLSURcIjpmYWxzZSxcIm9yZGVyaWRcIjo0Mn0se1wiaWRcIjo4OCxcIk5hbWVcIjpcIkFsYXJtVmFsdWU1XCIsXCJJc0F1dG9JbmNyZW1lbnRcIjpmYWxzZSxcIkNhbk51bGxcIjp0cnVlLFwiZGJUeXBlXCI6XCJkb3VibGVcIixcImxlbmd0aFwiOlwiXCIsXCJUYWJsZUlEXCI6OCxcIklzUEtJRFwiOmZhbHNlLFwib3JkZXJpZFwiOjQzfSx7XCJpZFwiOjg5LFwiTmFtZVwiOlwiQWxhcm1Qcmlvcml0eTVcIixcIklzQXV0b0luY3JlbWVudFwiOmZhbHNlLFwiQ2FuTnVsbFwiOnRydWUsXCJkYlR5cGVcIjpcImludFwiLFwibGVuZ3RoXCI6XCJcIixcIlRhYmxlSURcIjo4LFwiSXNQS0lEXCI6ZmFsc2UsXCJvcmRlcmlkXCI6NDR9XSxcImNoYW5nZWRDb2x1bW5zXCI6W3tcImlkXCI6ODIsXCJOYW1lXCI6XCJBbGFybVZhbHVlMlwiLFwiSXNBdXRvSW5jcmVtZW50XCI6ZmFsc2UsXCJDYW5OdWxsXCI6dHJ1ZSxcImRiVHlwZVwiOlwiZG91YmxlXCIsXCJsZW5ndGhcIjpcIlwiLFwiVGFibGVJRFwiOjgsXCJJc1BLSURcIjpmYWxzZSxcIm9yZGVyaWRcIjozNyxcIkJhY2t1cENoYW5nZWRQcm9wZXJ0aWVzXCI6e1wiTmFtZVwiOntcIk9yaWdpbmFsVmFsdWVcIjpcIkxpbmVhclk0XCJ9fX1dLFwiZGVsZXRlZENvbHVtbnNcIjpbXSxcIklEWENvbmZpZ3NcIjpbXSxcIklEXCI6MH0ifSx7Ik5hbWUiOiJkYXRhYmFzZWlkIiwiVmFsdWUiOjJ9XSwiUm93U3RhdGUiOjB9LHsiSXRlbXMiOlt7Ik5hbWUiOiJpZCIsIlZhbHVlIjozMn0seyJOYW1lIjoidHlwZSIsIlZhbHVlIjoiQ2hhbmdlVGFibGVBY3Rpb24ifSx7Ik5hbWUiOiJjb250ZW50IiwiVmFsdWUiOiJ7XCJPbGRUYWJsZU5hbWVcIjpcIkRldmljZVBvaW50XCIsXCJOZXdUYWJsZU5hbWVcIjpcIkRldmljZVBvaW50XCIsXCJvdGhlckNvbHVtbnNcIjpbe1wiaWRcIjo0MixcIk5hbWVcIjpcImlkXCIsXCJJc0F1dG9JbmNyZW1lbnRcIjp0cnVlLFwiQ2FuTnVsbFwiOmZhbHNlLFwiZGJUeXBlXCI6XCJpbnRcIixcIlRhYmxlSURcIjo4LFwiSXNQS0lEXCI6dHJ1ZSxcIm9yZGVyaWRcIjowfSx7XCJpZFwiOjQzLFwiY2FwdGlvblwiOlwi54K55ZCNXCIsXCJOYW1lXCI6XCJOYW1lXCIsXCJJc0F1dG9JbmNyZW1lbnRcIjpmYWxzZSxcIkNhbk51bGxcIjp0cnVlLFwiZGJUeXBlXCI6XCJpbnRcIixcImxlbmd0aFwiOlwiXCIsXCJUYWJsZUlEXCI6OCxcIklzUEtJRFwiOmZhbHNlLFwib3JkZXJpZFwiOjF9LHtcImlkXCI6NDQsXCJOYW1lXCI6XCJBZGRyZXNzXCIsXCJJc0F1dG9JbmNyZW1lbnRcIjpmYWxzZSxcIkNhbk51bGxcIjp0cnVlLFwiZGJUeXBlXCI6XCJ2YXJjaGFyXCIsXCJsZW5ndGhcIjpcIjIwMFwiLFwiVGFibGVJRFwiOjgsXCJJc1BLSURcIjpmYWxzZSxcIm9yZGVyaWRcIjo3fSx7XCJpZFwiOjQ1LFwiY2FwdGlvblwiOlwi5piv5ZCm55uR5rWL5Y+Y5YyWXCIsXCJOYW1lXCI6XCJJc1dhdGNoaW5nXCIsXCJJc0F1dG9JbmNyZW1lbnRcIjpmYWxzZSxcIkNhbk51bGxcIjp0cnVlLFwiZGJUeXBlXCI6XCJiaXRcIixcImxlbmd0aFwiOlwiXCIsXCJkZWZhdWx0VmFsdWVcIjpcIjBcIixcIlRhYmxlSURcIjo4LFwiSXNQS0lEXCI6ZmFsc2UsXCJvcmRlcmlkXCI6NDV9LHtcImlkXCI6NDgsXCJjYXB0aW9uXCI6XCLlnLDlnYDorr7nva7ml7bnmoRqc29u5a+56LGh6KGo6L6+XCIsXCJOYW1lXCI6XCJBZGRyU2V0dGluZ1wiLFwiSXNBdXRvSW5jcmVtZW50XCI6ZmFsc2UsXCJDYW5OdWxsXCI6dHJ1ZSxcImRiVHlwZVwiOlwidmFyY2hhclwiLFwibGVuZ3RoXCI6XCIyMDBcIixcIlRhYmxlSURcIjo4LFwiSXNQS0lEXCI6ZmFsc2UsXCJvcmRlcmlkXCI6OH0se1wiaWRcIjo0OSxcImNhcHRpb25cIjpcIuaPj+i/sFwiLFwiTmFtZVwiOlwiRGVzY1wiLFwiSXNBdXRvSW5jcmVtZW50XCI6ZmFsc2UsXCJDYW5OdWxsXCI6dHJ1ZSxcImRiVHlwZVwiOlwidmFyY2hhclwiLFwibGVuZ3RoXCI6XCIxMDBcIixcIlRhYmxlSURcIjo4LFwiSXNQS0lEXCI6ZmFsc2UsXCJvcmRlcmlkXCI6Mn0se1wiaWRcIjo1MCxcImNhcHRpb25cIjpcIuWIneWni+WAvFwiLFwiTmFtZVwiOlwiSW5pdFZhbHVlXCIsXCJJc0F1dG9JbmNyZW1lbnRcIjpmYWxzZSxcIkNhbk51bGxcIjp0cnVlLFwiZGJUeXBlXCI6XCJkb3VibGVcIixcImxlbmd0aFwiOlwiXCIsXCJUYWJsZUlEXCI6OCxcIklzUEtJRFwiOmZhbHNlLFwib3JkZXJpZFwiOjR9LHtcImlkXCI6NTEsXCJjYXB0aW9uXCI6XCLlvIDnirbmgIHkv6Hmga9cIixcIk5hbWVcIjpcIlN0YXR1c09wZW5JbmZvXCIsXCJJc0F1dG9JbmNyZW1lbnRcIjpmYWxzZSxcIkNhbk51bGxcIjp0cnVlLFwiZGJUeXBlXCI6XCJ2YXJjaGFyXCIsXCJsZW5ndGhcIjpcIjUwXCIsXCJUYWJsZUlEXCI6OCxcIklzUEtJRFwiOmZhbHNlLFwib3JkZXJpZFwiOjV9LHtcImlkXCI6NTIsXCJjYXB0aW9uXCI6XCLlhbPnirbmgIHkv6Hmga9cIixcIk5hbWVcIjpcIlN0YXRlQ2xvc2VJbmZvXCIsXCJJc0F1dG9JbmNyZW1lbnRcIjpmYWxzZSxcIkNhbk51bGxcIjp0cnVlLFwiZGJUeXBlXCI6XCJ2YXJjaGFyXCIsXCJsZW5ndGhcIjpcIjUwXCIsXCJUYWJsZUlEXCI6OCxcIklzUEtJRFwiOmZhbHNlLFwib3JkZXJpZFwiOjZ9LHtcImlkXCI6NTMsXCJjYXB0aW9uXCI6XCLmiqXorablvIDlhbNcIixcIk5hbWVcIjpcIklzQWxhcm1cIixcIklzQXV0b0luY3JlbWVudFwiOmZhbHNlLFwiQ2FuTnVsbFwiOnRydWUsXCJkYlR5cGVcIjpcImJpdFwiLFwibGVuZ3RoXCI6XCJcIixcImRlZmF1bHRWYWx1ZVwiOlwiMFwiLFwiVGFibGVJRFwiOjgsXCJJc1BLSURcIjpmYWxzZSxcIm9yZGVyaWRcIjo5fSx7XCJpZFwiOjU0LFwiY2FwdGlvblwiOlwi5oql6K2m5YC8XCIsXCJOYW1lXCI6XCJBbGFybVZhbHVlXCIsXCJJc0F1dG9JbmNyZW1lbnRcIjpmYWxzZSxcIkNhbk51bGxcIjp0cnVlLFwiZGJUeXBlXCI6XCJkb3VibGVcIixcImxlbmd0aFwiOlwiXCIsXCJUYWJsZUlEXCI6OCxcIklzUEtJRFwiOmZhbHNlLFwib3JkZXJpZFwiOjEwfSx7XCJpZFwiOjU1LFwiY2FwdGlvblwiOlwi5oql6K2m5LyY5YWI57qnXCIsXCJOYW1lXCI6XCJBbGFybVByaW9yaXR5XCIsXCJJc0F1dG9JbmNyZW1lbnRcIjpmYWxzZSxcIkNhbk51bGxcIjp0cnVlLFwiZGJUeXBlXCI6XCJpbnRcIixcImxlbmd0aFwiOlwiXCIsXCJUYWJsZUlEXCI6OCxcIklzUEtJRFwiOmZhbHNlLFwib3JkZXJpZFwiOjExfSx7XCJpZFwiOjU2LFwiY2FwdGlvblwiOlwi5oql6K2m57uEXCIsXCJOYW1lXCI6XCJBbGFybUdyb3VwXCIsXCJJc0F1dG9JbmNyZW1lbnRcIjpmYWxzZSxcIkNhbk51bGxcIjp0cnVlLFwiZGJUeXBlXCI6XCJ2YXJjaGFyXCIsXCJsZW5ndGhcIjpcIjUwXCIsXCJUYWJsZUlEXCI6OCxcIklzUEtJRFwiOmZhbHNlLFwib3JkZXJpZFwiOjEyfSx7XCJpZFwiOjU3LFwiY2FwdGlvblwiOlwi5oql6K2m6Ieq5Yqo56Gu6K6kXCIsXCJOYW1lXCI6XCJBbGFybUF1dG9Db25maXJtXCIsXCJJc0F1dG9JbmNyZW1lbnRcIjpmYWxzZSxcIkNhbk51bGxcIjp0cnVlLFwiZGJUeXBlXCI6XCJiaXRcIixcImxlbmd0aFwiOlwiXCIsXCJkZWZhdWx0VmFsdWVcIjpcIjBcIixcIlRhYmxlSURcIjo4LFwiSXNQS0lEXCI6ZmFsc2UsXCJvcmRlcmlkXCI6MTN9LHtcImlkXCI6NTgsXCJjYXB0aW9uXCI6XCLmiqXoraboh6rliqjlpI3kvY1cIixcIk5hbWVcIjpcIkFsYXJtQXV0b1Jlc2V0XCIsXCJJc0F1dG9JbmNyZW1lbnRcIjpmYWxzZSxcIkNhbk51bGxcIjp0cnVlLFwiZGJUeXBlXCI6XCJiaXRcIixcImxlbmd0aFwiOlwiXCIsXCJkZWZhdWx0VmFsdWVcIjpcIjBcIixcIlRhYmxlSURcIjo4LFwiSXNQS0lEXCI6ZmFsc2UsXCJvcmRlcmlkXCI6MTR9LHtcImlkXCI6NTksXCJjYXB0aW9uXCI6XCLmlbDmja7nu53lr7nlj5jljJbkv53lrZhcIixcIk5hbWVcIjpcIlZhbHVlQWJzb2x1dGVDaGFuZ2VcIixcIklzQXV0b0luY3JlbWVudFwiOmZhbHNlLFwiQ2FuTnVsbFwiOnRydWUsXCJkYlR5cGVcIjpcImJpdFwiLFwibGVuZ3RoXCI6XCJcIixcImRlZmF1bHRWYWx1ZVwiOlwiMFwiLFwiVGFibGVJRFwiOjgsXCJJc1BLSURcIjpmYWxzZSxcIm9yZGVyaWRcIjoxNX0se1wiaWRcIjo2MCxcImNhcHRpb25cIjpcIuaVsOaNrue7neWvueWPmOWMluWAvFwiLFwiTmFtZVwiOlwiVmFsdWVBYnNvbHV0ZUNoYW5nZVNldHRpbmdcIixcIklzQXV0b0luY3JlbWVudFwiOmZhbHNlLFwiQ2FuTnVsbFwiOnRydWUsXCJkYlR5cGVcIjpcImRvdWJsZVwiLFwibGVuZ3RoXCI6XCJcIixcIlRhYmxlSURcIjo4LFwiSXNQS0lEXCI6ZmFsc2UsXCJvcmRlcmlkXCI6MTZ9LHtcImlkXCI6NjEsXCJjYXB0aW9uXCI6XCLmlbDmja7nm7jlr7nlj5jljJbkv53lrZhcIixcIk5hbWVcIjpcIlZhbHVlUmVsYXRpdmVDaGFuZ2VcIixcIklzQXV0b0luY3JlbWVudFwiOmZhbHNlLFwiQ2FuTnVsbFwiOnRydWUsXCJkYlR5cGVcIjpcImJpdFwiLFwibGVuZ3RoXCI6XCJcIixcImRlZmF1bHRWYWx1ZVwiOlwiMFwiLFwiVGFibGVJRFwiOjgsXCJJc1BLSURcIjpmYWxzZSxcIm9yZGVyaWRcIjoxN30se1wiaWRcIjo2MixcImNhcHRpb25cIjpcIuaVsOaNruebuOWvueWPmOWMluWAvFwiLFwiTmFtZVwiOlwiVmFsdWVSZWxhdGl2ZUNoYW5nZVNldHRpbmdcIixcIklzQXV0b0luY3JlbWVudFwiOmZhbHNlLFwiQ2FuTnVsbFwiOnRydWUsXCJkYlR5cGVcIjpcImRvdWJsZVwiLFwibGVuZ3RoXCI6XCJcIixcIlRhYmxlSURcIjo4LFwiSXNQS0lEXCI6ZmFsc2UsXCJvcmRlcmlkXCI6MTh9LHtcImlkXCI6NjMsXCJjYXB0aW9uXCI6XCLmlbDmja7lrprml7bkv53lrZhcIixcIk5hbWVcIjpcIlZhbHVlT25UaW1lQ2hhbmdlXCIsXCJJc0F1dG9JbmNyZW1lbnRcIjpmYWxzZSxcIkNhbk51bGxcIjp0cnVlLFwiZGJUeXBlXCI6XCJiaXRcIixcImxlbmd0aFwiOlwiXCIsXCJkZWZhdWx0VmFsdWVcIjpcIjBcIixcIlRhYmxlSURcIjo4LFwiSXNQS0lEXCI6ZmFsc2UsXCJvcmRlcmlkXCI6MTl9LHtcImlkXCI6NjQsXCJjYXB0aW9uXCI6XCLmlbDmja7lrprml7blgLxcIixcIk5hbWVcIjpcIlZhbHVlT25UaW1lQ2hhbmdlU2V0dGluZ1wiLFwiSXNBdXRvSW5jcmVtZW50XCI6ZmFsc2UsXCJDYW5OdWxsXCI6dHJ1ZSxcImRiVHlwZVwiOlwiZG91YmxlXCIsXCJsZW5ndGhcIjpcIlwiLFwiVGFibGVJRFwiOjgsXCJJc1BLSURcIjpmYWxzZSxcIm9yZGVyaWRcIjoyMH0se1wiaWRcIjo2NSxcImNhcHRpb25cIjpcIueCueexu+Wei1wiLFwiTmFtZVwiOlwiVHlwZVwiLFwiSXNBdXRvSW5jcmVtZW50XCI6ZmFsc2UsXCJDYW5OdWxsXCI6dHJ1ZSxcImRiVHlwZVwiOlwiaW50XCIsXCJFbnVtRGVmaW5lXCI6XCJBbmFsb2cgPSAxLFxcbkRpZ2l0YWwgPSAyXCIsXCJsZW5ndGhcIjpcIlwiLFwiZGVmYXVsdFZhbHVlXCI6XCIyXCIsXCJUYWJsZUlEXCI6OCxcIklzUEtJRFwiOmZhbHNlLFwib3JkZXJpZFwiOjN9LHtcImlkXCI6NjYsXCJjYXB0aW9uXCI6XCLmqKHmi5/ph48t5bel56iL5Y2V5L2NXCIsXCJOYW1lXCI6XCJVbml0XCIsXCJJc0F1dG9JbmNyZW1lbnRcIjpmYWxzZSxcIkNhbk51bGxcIjp0cnVlLFwiZGJUeXBlXCI6XCJ2YXJjaGFyXCIsXCJsZW5ndGhcIjpcIjUwXCIsXCJUYWJsZUlEXCI6OCxcIklzUEtJRFwiOmZhbHNlLFwib3JkZXJpZFwiOjIxfSx7XCJpZFwiOjY3LFwiY2FwdGlvblwiOlwi5qih5ouf6YePLeWwj+aVsOeCueS9jeaVsFwiLFwiTmFtZVwiOlwiRFBDb3VudFwiLFwiSXNBdXRvSW5jcmVtZW50XCI6ZmFsc2UsXCJDYW5OdWxsXCI6dHJ1ZSxcImRiVHlwZVwiOlwidmFyY2hhclwiLFwibGVuZ3RoXCI6XCI1MFwiLFwiVGFibGVJRFwiOjgsXCJJc1BLSURcIjpmYWxzZSxcIm9yZGVyaWRcIjoyMn0se1wiaWRcIjo2OCxcImNhcHRpb25cIjpcIuaooeaLn+mHjy3ph4/nqIvovazljJblvIDlhbNcIixcIk5hbWVcIjpcIklzVHJhbnNmb3JtXCIsXCJJc0F1dG9JbmNyZW1lbnRcIjpmYWxzZSxcIkNhbk51bGxcIjp0cnVlLFwiZGJUeXBlXCI6XCJiaXRcIixcImxlbmd0aFwiOlwiXCIsXCJkZWZhdWx0VmFsdWVcIjpcIjBcIixcIlRhYmxlSURcIjo4LFwiSXNQS0lEXCI6ZmFsc2UsXCJvcmRlcmlkXCI6MjN9LHtcImlkXCI6NjksXCJjYXB0aW9uXCI6XCLmqKHmi5/ph48t6YeP56iL5LiK6ZmQXCIsXCJOYW1lXCI6XCJUcmFuc01heFwiLFwiSXNBdXRvSW5jcmVtZW50XCI6ZmFsc2UsXCJDYW5OdWxsXCI6dHJ1ZSxcImRiVHlwZVwiOlwiZG91YmxlXCIsXCJsZW5ndGhcIjpcIlwiLFwiVGFibGVJRFwiOjgsXCJJc1BLSURcIjpmYWxzZSxcIm9yZGVyaWRcIjoyNH0se1wiaWRcIjo3MCxcImNhcHRpb25cIjpcIuaooeaLn+mHjy3ph4/nqIvkuIvpmZBcIixcIk5hbWVcIjpcIlRyYW5zTWluXCIsXCJJc0F1dG9JbmNyZW1lbnRcIjpmYWxzZSxcIkNhbk51bGxcIjp0cnVlLFwiZGJUeXBlXCI6XCJkb3VibGVcIixcImxlbmd0aFwiOlwiXCIsXCJUYWJsZUlEXCI6OCxcIklzUEtJRFwiOmZhbHNlLFwib3JkZXJpZFwiOjI1fSx7XCJpZFwiOjcxLFwiY2FwdGlvblwiOlwi5qih5ouf6YePLeS8oOaEn+WZqOmHj+eoi+S4iumZkFwiLFwiTmFtZVwiOlwiU2Vuc29yTWF4XCIsXCJJc0F1dG9JbmNyZW1lbnRcIjpmYWxzZSxcIkNhbk51bGxcIjp0cnVlLFwiZGJUeXBlXCI6XCJkb3VibGVcIixcImxlbmd0aFwiOlwiXCIsXCJUYWJsZUlEXCI6OCxcIklzUEtJRFwiOmZhbHNlLFwib3JkZXJpZFwiOjI2fSx7XCJpZFwiOjcyLFwiY2FwdGlvblwiOlwi5qih5ouf6YePLeS8oOaEn+WZqOmHj+eoi+S4i+mZkFwiLFwiTmFtZVwiOlwiU2Vuc29yTWluXCIsXCJJc0F1dG9JbmNyZW1lbnRcIjpmYWxzZSxcIkNhbk51bGxcIjp0cnVlLFwiZGJUeXBlXCI6XCJ2YXJjaGFyXCIsXCJsZW5ndGhcIjpcIjUwXCIsXCJUYWJsZUlEXCI6OCxcIklzUEtJRFwiOmZhbHNlLFwib3JkZXJpZFwiOjI3fSx7XCJpZFwiOjczLFwiY2FwdGlvblwiOlwi5qih5ouf6YePLeW8gOW5s+aWuVwiLFwiTmFtZVwiOlwiSXNTcXVhcmVcIixcIklzQXV0b0luY3JlbWVudFwiOmZhbHNlLFwiQ2FuTnVsbFwiOnRydWUsXCJkYlR5cGVcIjpcImJpdFwiLFwibGVuZ3RoXCI6XCJcIixcImRlZmF1bHRWYWx1ZVwiOlwiMFwiLFwiVGFibGVJRFwiOjgsXCJJc1BLSURcIjpmYWxzZSxcIm9yZGVyaWRcIjoyOH0se1wiaWRcIjo3NCxcImNhcHRpb25cIjpcIuWIhuautee6v+aAp+WMllwiLFwiTmFtZVwiOlwiSXNMaW5lYXJcIixcIklzQXV0b0luY3JlbWVudFwiOmZhbHNlLFwiQ2FuTnVsbFwiOnRydWUsXCJkYlR5cGVcIjpcImJpdFwiLFwibGVuZ3RoXCI6XCJcIixcImRlZmF1bHRWYWx1ZVwiOlwiMFwiLFwiVGFibGVJRFwiOjgsXCJJc1BLSURcIjpmYWxzZSxcIm9yZGVyaWRcIjoyOX0se1wiaWRcIjo3NSxcImNhcHRpb25cIjpcIuWIhuautee6v+aAp+WMllgxXCIsXCJOYW1lXCI6XCJMaW5lYXJYMVwiLFwiSXNBdXRvSW5jcmVtZW50XCI6ZmFsc2UsXCJDYW5OdWxsXCI6dHJ1ZSxcImRiVHlwZVwiOlwiZG91YmxlXCIsXCJsZW5ndGhcIjpcIlwiLFwiVGFibGVJRFwiOjgsXCJJc1BLSURcIjpmYWxzZSxcIm9yZGVyaWRcIjozMH0se1wiaWRcIjo3NixcImNhcHRpb25cIjpcIuWIhuautee6v+aAp+WMllkxXCIsXCJOYW1lXCI6XCJMaW5lYXJZMVwiLFwiSXNBdXRvSW5jcmVtZW50XCI6ZmFsc2UsXCJDYW5OdWxsXCI6dHJ1ZSxcImRiVHlwZVwiOlwiZG91YmxlXCIsXCJsZW5ndGhcIjpcIlwiLFwiVGFibGVJRFwiOjgsXCJJc1BLSURcIjpmYWxzZSxcIm9yZGVyaWRcIjozMX0se1wiaWRcIjo3NyxcImNhcHRpb25cIjpcIuWIhuautee6v+aAp+WMllgyXCIsXCJOYW1lXCI6XCJMaW5lYXJYMlwiLFwiSXNBdXRvSW5jcmVtZW50XCI6ZmFsc2UsXCJDYW5OdWxsXCI6dHJ1ZSxcImRiVHlwZVwiOlwiZG91YmxlXCIsXCJsZW5ndGhcIjpcIlwiLFwiVGFibGVJRFwiOjgsXCJJc1BLSURcIjpmYWxzZSxcIm9yZGVyaWRcIjozMn0se1wiaWRcIjo3OCxcImNhcHRpb25cIjpcIuWIhuautee6v+aAp+WMllkyXCIsXCJOYW1lXCI6XCJMaW5lYXJZMlwiLFwiSXNBdXRvSW5jcmVtZW50XCI6ZmFsc2UsXCJDYW5OdWxsXCI6dHJ1ZSxcImRiVHlwZVwiOlwiZG91YmxlXCIsXCJsZW5ndGhcIjpcIlwiLFwiVGFibGVJRFwiOjgsXCJJc1BLSURcIjpmYWxzZSxcIm9yZGVyaWRcIjozM30se1wiaWRcIjo3OSxcIk5hbWVcIjpcIkxpbmVhclgzXCIsXCJJc0F1dG9JbmNyZW1lbnRcIjpmYWxzZSxcIkNhbk51bGxcIjp0cnVlLFwiZGJUeXBlXCI6XCJkb3VibGVcIixcImxlbmd0aFwiOlwiXCIsXCJUYWJsZUlEXCI6OCxcIklzUEtJRFwiOmZhbHNlLFwib3JkZXJpZFwiOjM0fSx7XCJpZFwiOjgwLFwiTmFtZVwiOlwiTGluZWFyWTNcIixcIklzQXV0b0luY3JlbWVudFwiOmZhbHNlLFwiQ2FuTnVsbFwiOnRydWUsXCJkYlR5cGVcIjpcImRvdWJsZVwiLFwibGVuZ3RoXCI6XCJcIixcIlRhYmxlSURcIjo4LFwiSXNQS0lEXCI6ZmFsc2UsXCJvcmRlcmlkXCI6MzV9LHtcImlkXCI6ODEsXCJOYW1lXCI6XCJMaW5lYXJYNFwiLFwiSXNBdXRvSW5jcmVtZW50XCI6ZmFsc2UsXCJDYW5OdWxsXCI6dHJ1ZSxcImRiVHlwZVwiOlwiZG91YmxlXCIsXCJsZW5ndGhcIjpcIlwiLFwiVGFibGVJRFwiOjgsXCJJc1BLSURcIjpmYWxzZSxcIm9yZGVyaWRcIjozNn0se1wiaWRcIjo4MixcImNhcHRpb25cIjpcIuaKpeitpuWAvDJcIixcIk5hbWVcIjpcIkFsYXJtVmFsdWUyXCIsXCJJc0F1dG9JbmNyZW1lbnRcIjpmYWxzZSxcIkNhbk51bGxcIjp0cnVlLFwiZGJUeXBlXCI6XCJkb3VibGVcIixcImxlbmd0aFwiOlwiXCIsXCJUYWJsZUlEXCI6OCxcIklzUEtJRFwiOmZhbHNlLFwib3JkZXJpZFwiOjM3fSx7XCJpZFwiOjgzLFwiY2FwdGlvblwiOlwi5oql6K2m5LyY5YWI57qnMlwiLFwiTmFtZVwiOlwiQWxhcm1Qcmlvcml0eTJcIixcIklzQXV0b0luY3JlbWVudFwiOmZhbHNlLFwiQ2FuTnVsbFwiOnRydWUsXCJkYlR5cGVcIjpcImludFwiLFwibGVuZ3RoXCI6XCJcIixcIlRhYmxlSURcIjo4LFwiSXNQS0lEXCI6ZmFsc2UsXCJvcmRlcmlkXCI6Mzh9LHtcImlkXCI6ODQsXCJOYW1lXCI6XCJBbGFybVZhbHVlM1wiLFwiSXNBdXRvSW5jcmVtZW50XCI6ZmFsc2UsXCJDYW5OdWxsXCI6dHJ1ZSxcImRiVHlwZVwiOlwiZG91YmxlXCIsXCJsZW5ndGhcIjpcIlwiLFwiVGFibGVJRFwiOjgsXCJJc1BLSURcIjpmYWxzZSxcIm9yZGVyaWRcIjozOX0se1wiaWRcIjo4NSxcIk5hbWVcIjpcIkFsYXJtUHJpb3JpdHkzXCIsXCJJc0F1dG9JbmNyZW1lbnRcIjpmYWxzZSxcIkNhbk51bGxcIjp0cnVlLFwiZGJUeXBlXCI6XCJpbnRcIixcImxlbmd0aFwiOlwiXCIsXCJUYWJsZUlEXCI6OCxcIklzUEtJRFwiOmZhbHNlLFwib3JkZXJpZFwiOjQwfSx7XCJpZFwiOjg2LFwiTmFtZVwiOlwiQWxhcm1WYWx1ZTRcIixcIklzQXV0b0luY3JlbWVudFwiOmZhbHNlLFwiQ2FuTnVsbFwiOnRydWUsXCJkYlR5cGVcIjpcImRvdWJsZVwiLFwibGVuZ3RoXCI6XCJcIixcIlRhYmxlSURcIjo4LFwiSXNQS0lEXCI6ZmFsc2UsXCJvcmRlcmlkXCI6NDF9LHtcImlkXCI6ODcsXCJOYW1lXCI6XCJBbGFybVByaW9yaXR5NFwiLFwiSXNBdXRvSW5jcmVtZW50XCI6ZmFsc2UsXCJDYW5OdWxsXCI6dHJ1ZSxcImRiVHlwZVwiOlwiaW50XCIsXCJsZW5ndGhcIjpcIlwiLFwiVGFibGVJRFwiOjgsXCJJc1BLSURcIjpmYWxzZSxcIm9yZGVyaWRcIjo0Mn0se1wiaWRcIjo4OCxcIk5hbWVcIjpcIkFsYXJtVmFsdWU1XCIsXCJJc0F1dG9JbmNyZW1lbnRcIjpmYWxzZSxcIkNhbk51bGxcIjp0cnVlLFwiZGJUeXBlXCI6XCJkb3VibGVcIixcImxlbmd0aFwiOlwiXCIsXCJUYWJsZUlEXCI6OCxcIklzUEtJRFwiOmZhbHNlLFwib3JkZXJpZFwiOjQzfSx7XCJpZFwiOjg5LFwiTmFtZVwiOlwiQWxhcm1Qcmlvcml0eTVcIixcIklzQXV0b0luY3JlbWVudFwiOmZhbHNlLFwiQ2FuTnVsbFwiOnRydWUsXCJkYlR5cGVcIjpcImludFwiLFwibGVuZ3RoXCI6XCJcIixcIlRhYmxlSURcIjo4LFwiSXNQS0lEXCI6ZmFsc2UsXCJvcmRlcmlkXCI6NDR9XSxcIm5ld0NvbHVtbnNcIjpbe1wiaWRcIjo5MCxcImNhcHRpb25cIjpcIuWBj+W3ruaKpeitplwiLFwiTmFtZVwiOlwiSXNBbGFybU9mZnNldFwiLFwiSXNBdXRvSW5jcmVtZW50XCI6ZmFsc2UsXCJDYW5OdWxsXCI6dHJ1ZSxcImRiVHlwZVwiOlwiYml0XCIsXCJsZW5ndGhcIjpcIlwiLFwiZGVmYXVsdFZhbHVlXCI6XCIwXCIsXCJUYWJsZUlEXCI6OCxcIklzUEtJRFwiOmZhbHNlLFwib3JkZXJpZFwiOjQ1fSx7XCJpZFwiOjkxLFwiY2FwdGlvblwiOlwi5YGP5beu5oql6K2m6K6+5a6a5YC8XCIsXCJOYW1lXCI6XCJBbGFybU9mZnNldE9yaWdpbmFsVmFsdWVcIixcIklzQXV0b0luY3JlbWVudFwiOmZhbHNlLFwiQ2FuTnVsbFwiOnRydWUsXCJkYlR5cGVcIjpcImRvdWJsZVwiLFwibGVuZ3RoXCI6XCJcIixcIlRhYmxlSURcIjo4LFwiSXNQS0lEXCI6ZmFsc2UsXCJvcmRlcmlkXCI6NDZ9LHtcImlkXCI6OTIsXCJjYXB0aW9uXCI6XCLlgY/lt67lgLxcIixcIk5hbWVcIjpcIkFsYXJtT2Zmc2V0VmFsdWVcIixcIklzQXV0b0luY3JlbWVudFwiOmZhbHNlLFwiQ2FuTnVsbFwiOnRydWUsXCJkYlR5cGVcIjpcImRvdWJsZVwiLFwibGVuZ3RoXCI6XCJcIixcIlRhYmxlSURcIjo4LFwiSXNQS0lEXCI6ZmFsc2UsXCJvcmRlcmlkXCI6NDd9LHtcImlkXCI6OTMsXCJjYXB0aW9uXCI6XCLlgY/lt67miqXorabkvJjlhYjnuqdcIixcIk5hbWVcIjpcIkFsYXJtT2Zmc2V0UHJpb3JpdHlcIixcIklzQXV0b0luY3JlbWVudFwiOmZhbHNlLFwiQ2FuTnVsbFwiOnRydWUsXCJkYlR5cGVcIjpcImludFwiLFwibGVuZ3RoXCI6XCJcIixcIlRhYmxlSURcIjo4LFwiSXNQS0lEXCI6ZmFsc2UsXCJvcmRlcmlkXCI6NDh9LHtcImlkXCI6OTQsXCJjYXB0aW9uXCI6XCLlj5jljJbnjofmiqXoraZcIixcIk5hbWVcIjpcIklzQWxhcm1QZXJjZW50XCIsXCJJc0F1dG9JbmNyZW1lbnRcIjpmYWxzZSxcIkNhbk51bGxcIjp0cnVlLFwiZGJUeXBlXCI6XCJiaXRcIixcImxlbmd0aFwiOlwiXCIsXCJkZWZhdWx0VmFsdWVcIjpcIjBcIixcIlRhYmxlSURcIjo4LFwiSXNQS0lEXCI6ZmFsc2UsXCJvcmRlcmlkXCI6NDl9LHtcImlkXCI6OTUsXCJjYXB0aW9uXCI6XCLlj5jljJbnjoflgLzvvIjnmb7liIbmr5TvvIlcIixcIk5hbWVcIjpcIlBlcmNlbnRcIixcIklzQXV0b0luY3JlbWVudFwiOmZhbHNlLFwiQ2FuTnVsbFwiOnRydWUsXCJkYlR5cGVcIjpcImRvdWJsZVwiLFwibGVuZ3RoXCI6XCJcIixcIlRhYmxlSURcIjo4LFwiSXNQS0lEXCI6ZmFsc2UsXCJvcmRlcmlkXCI6NTB9LHtcImlkXCI6OTYsXCJjYXB0aW9uXCI6XCLlj5jljJbnjoflkajmnJ/vvIjnp5LvvIlcIixcIk5hbWVcIjpcIkNoYW5nZUN5Y2xlXCIsXCJJc0F1dG9JbmNyZW1lbnRcIjpmYWxzZSxcIkNhbk51bGxcIjp0cnVlLFwiZGJUeXBlXCI6XCJpbnRcIixcImxlbmd0aFwiOlwiXCIsXCJUYWJsZUlEXCI6OCxcIklzUEtJRFwiOmZhbHNlLFwib3JkZXJpZFwiOjUxfSx7XCJpZFwiOjk3LFwiY2FwdGlvblwiOlwi5Y+Y5YyW546H5oql6K2m5LyY5YWI57qnXCIsXCJOYW1lXCI6XCJBbGFybVBlcmNlbnRQcmlvcml0eVwiLFwiSXNBdXRvSW5jcmVtZW50XCI6ZmFsc2UsXCJDYW5OdWxsXCI6dHJ1ZSxcImRiVHlwZVwiOlwiaW50XCIsXCJsZW5ndGhcIjpcIlwiLFwiVGFibGVJRFwiOjgsXCJJc1BLSURcIjpmYWxzZSxcIm9yZGVyaWRcIjo1Mn1dLFwiY2hhbmdlZENvbHVtbnNcIjpbXSxcImRlbGV0ZWRDb2x1bW5zXCI6W10sXCJJRFhDb25maWdzXCI6W10sXCJJRFwiOjB9In0seyJOYW1lIjoiZGF0YWJhc2VpZCIsIlZhbHVlIjoyfV0sIlJvd1N0YXRlIjowfSx7Ikl0ZW1zIjpbeyJOYW1lIjoiaWQiLCJWYWx1ZSI6MzN9LHsiTmFtZSI6InR5cGUiLCJWYWx1ZSI6IkNyZWF0ZVRhYmxlQWN0aW9uIn0seyJOYW1lIjoiY29udGVudCIsIlZhbHVlIjoie1wiVGFibGVcIjp7XCJpZFwiOjksXCJjYXB0aW9uXCI6XCLmjqfliLbljZXlhYNcIixcIk5hbWVcIjpcIkNvbnRyb2xVbml0XCIsXCJEYXRhYmFzZUlEXCI6MixcImlMb2NrXCI6MH0sXCJDb2x1bW5zXCI6W3tcImlkXCI6OTgsXCJOYW1lXCI6XCJpZFwiLFwiSXNBdXRvSW5jcmVtZW50XCI6dHJ1ZSxcIkNhbk51bGxcIjpmYWxzZSxcImRiVHlwZVwiOlwiaW50XCIsXCJUYWJsZUlEXCI6OSxcIklzUEtJRFwiOnRydWUsXCJvcmRlcmlkXCI6MH0se1wiaWRcIjo5OSxcImNhcHRpb25cIjpcIuWQjeensFwiLFwiTmFtZVwiOlwiTmFtZVwiLFwiSXNBdXRvSW5jcmVtZW50XCI6ZmFsc2UsXCJDYW5OdWxsXCI6dHJ1ZSxcImRiVHlwZVwiOlwidmFyY2hhclwiLFwibGVuZ3RoXCI6XCI1MFwiLFwiVGFibGVJRFwiOjksXCJJc1BLSURcIjpmYWxzZSxcIm9yZGVyaWRcIjoxfV0sXCJJRFhDb25maWdzXCI6W10sXCJJRFwiOjB9In0seyJOYW1lIjoiZGF0YWJhc2VpZCIsIlZhbHVlIjoyfV0sIlJvd1N0YXRlIjowfSx7Ikl0ZW1zIjpbeyJOYW1lIjoiaWQiLCJWYWx1ZSI6MzR9LHsiTmFtZSI6InR5cGUiLCJWYWx1ZSI6IkNoYW5nZVRhYmxlQWN0aW9uIn0seyJOYW1lIjoiY29udGVudCIsIlZhbHVlIjoie1wiT2xkVGFibGVOYW1lXCI6XCJDb21tdW5pY2F0aW9uRHJpdmVyXCIsXCJOZXdUYWJsZU5hbWVcIjpcIkNvbW11bmljYXRpb25Ecml2ZXJcIixcIm90aGVyQ29sdW1uc1wiOlt7XCJpZFwiOjM0LFwiTmFtZVwiOlwiaWRcIixcIklzQXV0b0luY3JlbWVudFwiOnRydWUsXCJDYW5OdWxsXCI6ZmFsc2UsXCJkYlR5cGVcIjpcImludFwiLFwiVGFibGVJRFwiOjYsXCJJc1BLSURcIjp0cnVlLFwib3JkZXJpZFwiOjB9LHtcImlkXCI6MzUsXCJjYXB0aW9uXCI6XCLlkI3np7BcIixcIk5hbWVcIjpcIk5hbWVcIixcIklzQXV0b0luY3JlbWVudFwiOmZhbHNlLFwiQ2FuTnVsbFwiOnRydWUsXCJkYlR5cGVcIjpcInZhcmNoYXJcIixcImxlbmd0aFwiOlwiNTBcIixcIlRhYmxlSURcIjo2LFwiSXNQS0lEXCI6ZmFsc2UsXCJvcmRlcmlkXCI6MX0se1wiaWRcIjozNixcImNhcHRpb25cIjpcIuWcsOWdgFwiLFwiTmFtZVwiOlwiQWRkcmVzc1wiLFwiSXNBdXRvSW5jcmVtZW50XCI6ZmFsc2UsXCJDYW5OdWxsXCI6dHJ1ZSxcImRiVHlwZVwiOlwidmFyY2hhclwiLFwibGVuZ3RoXCI6XCI1MFwiLFwiVGFibGVJRFwiOjYsXCJJc1BLSURcIjpmYWxzZSxcIm9yZGVyaWRcIjoyfSx7XCJpZFwiOjM3LFwiY2FwdGlvblwiOlwi56uv5Y+jXCIsXCJOYW1lXCI6XCJQb3J0XCIsXCJJc0F1dG9JbmNyZW1lbnRcIjpmYWxzZSxcIkNhbk51bGxcIjp0cnVlLFwiZGJUeXBlXCI6XCJpbnRcIixcImxlbmd0aFwiOlwiXCIsXCJUYWJsZUlEXCI6NixcIklzUEtJRFwiOmZhbHNlLFwib3JkZXJpZFwiOjN9LHtcImlkXCI6MzgsXCJjYXB0aW9uXCI6XCLnirbmgIFcIixcIk5hbWVcIjpcIlN0YXR1c1wiLFwiSXNBdXRvSW5jcmVtZW50XCI6ZmFsc2UsXCJDYW5OdWxsXCI6dHJ1ZSxcImRiVHlwZVwiOlwiaW50XCIsXCJFbnVtRGVmaW5lXCI6XCJOb25lID0gMCxcXG5PbmxpbmUgPSAxLFxcbk9mZmxpbmUgPSAyXCIsXCJsZW5ndGhcIjpcIlwiLFwiZGVmYXVsdFZhbHVlXCI6XCIwXCIsXCJUYWJsZUlEXCI6NixcIklzUEtJRFwiOmZhbHNlLFwib3JkZXJpZFwiOjR9LHtcImlkXCI6NDYsXCJOYW1lXCI6XCJTdXBwb3J0RW51bURldmljZVwiLFwiSXNBdXRvSW5jcmVtZW50XCI6ZmFsc2UsXCJDYW5OdWxsXCI6dHJ1ZSxcImRiVHlwZVwiOlwiYml0XCIsXCJsZW5ndGhcIjpcIlwiLFwiZGVmYXVsdFZhbHVlXCI6XCIwXCIsXCJUYWJsZUlEXCI6NixcIklzUEtJRFwiOmZhbHNlLFwib3JkZXJpZFwiOjV9LHtcImlkXCI6NDcsXCJOYW1lXCI6XCJTdXBwb3J0RW51bVBvaW50c1wiLFwiSXNBdXRvSW5jcmVtZW50XCI6ZmFsc2UsXCJDYW5OdWxsXCI6dHJ1ZSxcImRiVHlwZVwiOlwiYml0XCIsXCJsZW5ndGhcIjpcIlwiLFwiZGVmYXVsdFZhbHVlXCI6XCIwXCIsXCJUYWJsZUlEXCI6NixcIklzUEtJRFwiOmZhbHNlLFwib3JkZXJpZFwiOjZ9XSxcIm5ld0NvbHVtbnNcIjpbXSxcImNoYW5nZWRDb2x1bW5zXCI6W10sXCJkZWxldGVkQ29sdW1uc1wiOltdLFwiSURYQ29uZmlnc1wiOltdLFwiSURcIjowfSJ9LHsiTmFtZSI6ImRhdGFiYXNlaWQiLCJWYWx1ZSI6Mn1dLCJSb3dTdGF0ZSI6MH0seyJJdGVtcyI6W3siTmFtZSI6ImlkIiwiVmFsdWUiOjM1fSx7Ik5hbWUiOiJ0eXBlIiwiVmFsdWUiOiJDaGFuZ2VUYWJsZUFjdGlvbiJ9LHsiTmFtZSI6ImNvbnRlbnQiLCJWYWx1ZSI6IntcIk9sZFRhYmxlTmFtZVwiOlwiRGV2aWNlUG9pbnRcIixcIk5ld1RhYmxlTmFtZVwiOlwiRGV2aWNlUG9pbnRcIixcIm90aGVyQ29sdW1uc1wiOlt7XCJpZFwiOjQyLFwiTmFtZVwiOlwiaWRcIixcIklzQXV0b0luY3JlbWVudFwiOnRydWUsXCJDYW5OdWxsXCI6ZmFsc2UsXCJkYlR5cGVcIjpcImludFwiLFwiVGFibGVJRFwiOjgsXCJJc1BLSURcIjp0cnVlLFwib3JkZXJpZFwiOjB9LHtcImlkXCI6NDMsXCJjYXB0aW9uXCI6XCLngrnlkI1cIixcIk5hbWVcIjpcIk5hbWVcIixcIklzQXV0b0luY3JlbWVudFwiOmZhbHNlLFwiQ2FuTnVsbFwiOnRydWUsXCJkYlR5cGVcIjpcImludFwiLFwibGVuZ3RoXCI6XCJcIixcIlRhYmxlSURcIjo4LFwiSXNQS0lEXCI6ZmFsc2UsXCJvcmRlcmlkXCI6MX0se1wiaWRcIjo0NCxcIk5hbWVcIjpcIkFkZHJlc3NcIixcIklzQXV0b0luY3JlbWVudFwiOmZhbHNlLFwiQ2FuTnVsbFwiOnRydWUsXCJkYlR5cGVcIjpcInZhcmNoYXJcIixcImxlbmd0aFwiOlwiMjAwXCIsXCJUYWJsZUlEXCI6OCxcIklzUEtJRFwiOmZhbHNlLFwib3JkZXJpZFwiOjd9LHtcImlkXCI6NDUsXCJjYXB0aW9uXCI6XCLmmK/lkKbnm5HmtYvlj5jljJZcIixcIk5hbWVcIjpcIklzV2F0Y2hpbmdcIixcIklzQXV0b0luY3JlbWVudFwiOmZhbHNlLFwiQ2FuTnVsbFwiOnRydWUsXCJkYlR5cGVcIjpcImJpdFwiLFwibGVuZ3RoXCI6XCJcIixcImRlZmF1bHRWYWx1ZVwiOlwiMFwiLFwiVGFibGVJRFwiOjgsXCJJc1BLSURcIjpmYWxzZSxcIm9yZGVyaWRcIjo1M30se1wiaWRcIjo0OCxcImNhcHRpb25cIjpcIuWcsOWdgOiuvue9ruaXtueahGpzb27lr7nosaHooajovr5cIixcIk5hbWVcIjpcIkFkZHJTZXR0aW5nXCIsXCJJc0F1dG9JbmNyZW1lbnRcIjpmYWxzZSxcIkNhbk51bGxcIjp0cnVlLFwiZGJUeXBlXCI6XCJ2YXJjaGFyXCIsXCJsZW5ndGhcIjpcIjIwMFwiLFwiVGFibGVJRFwiOjgsXCJJc1BLSURcIjpmYWxzZSxcIm9yZGVyaWRcIjo4fSx7XCJpZFwiOjQ5LFwiY2FwdGlvblwiOlwi5o+P6L+wXCIsXCJOYW1lXCI6XCJEZXNjXCIsXCJJc0F1dG9JbmNyZW1lbnRcIjpmYWxzZSxcIkNhbk51bGxcIjp0cnVlLFwiZGJUeXBlXCI6XCJ2YXJjaGFyXCIsXCJsZW5ndGhcIjpcIjEwMFwiLFwiVGFibGVJRFwiOjgsXCJJc1BLSURcIjpmYWxzZSxcIm9yZGVyaWRcIjoyfSx7XCJpZFwiOjUwLFwiY2FwdGlvblwiOlwi5Yid5aeL5YC8XCIsXCJOYW1lXCI6XCJJbml0VmFsdWVcIixcIklzQXV0b0luY3JlbWVudFwiOmZhbHNlLFwiQ2FuTnVsbFwiOnRydWUsXCJkYlR5cGVcIjpcImRvdWJsZVwiLFwibGVuZ3RoXCI6XCJcIixcIlRhYmxlSURcIjo4LFwiSXNQS0lEXCI6ZmFsc2UsXCJvcmRlcmlkXCI6NH0se1wiaWRcIjo1MSxcImNhcHRpb25cIjpcIuW8gOeKtuaAgeS/oeaBr1wiLFwiTmFtZVwiOlwiU3RhdHVzT3BlbkluZm9cIixcIklzQXV0b0luY3JlbWVudFwiOmZhbHNlLFwiQ2FuTnVsbFwiOnRydWUsXCJkYlR5cGVcIjpcInZhcmNoYXJcIixcImxlbmd0aFwiOlwiNTBcIixcIlRhYmxlSURcIjo4LFwiSXNQS0lEXCI6ZmFsc2UsXCJvcmRlcmlkXCI6NX0se1wiaWRcIjo1MixcImNhcHRpb25cIjpcIuWFs+eKtuaAgeS/oeaBr1wiLFwiTmFtZVwiOlwiU3RhdGVDbG9zZUluZm9cIixcIklzQXV0b0luY3JlbWVudFwiOmZhbHNlLFwiQ2FuTnVsbFwiOnRydWUsXCJkYlR5cGVcIjpcInZhcmNoYXJcIixcImxlbmd0aFwiOlwiNTBcIixcIlRhYmxlSURcIjo4LFwiSXNQS0lEXCI6ZmFsc2UsXCJvcmRlcmlkXCI6Nn0se1wiaWRcIjo1MyxcImNhcHRpb25cIjpcIuaKpeitpuW8gOWFs1wiLFwiTmFtZVwiOlwiSXNBbGFybVwiLFwiSXNBdXRvSW5jcmVtZW50XCI6ZmFsc2UsXCJDYW5OdWxsXCI6dHJ1ZSxcImRiVHlwZVwiOlwiYml0XCIsXCJsZW5ndGhcIjpcIlwiLFwiZGVmYXVsdFZhbHVlXCI6XCIwXCIsXCJUYWJsZUlEXCI6OCxcIklzUEtJRFwiOmZhbHNlLFwib3JkZXJpZFwiOjl9LHtcImlkXCI6NTQsXCJjYXB0aW9uXCI6XCLmiqXorablgLxcIixcIk5hbWVcIjpcIkFsYXJtVmFsdWVcIixcIklzQXV0b0luY3JlbWVudFwiOmZhbHNlLFwiQ2FuTnVsbFwiOnRydWUsXCJkYlR5cGVcIjpcImRvdWJsZVwiLFwibGVuZ3RoXCI6XCJcIixcIlRhYmxlSURcIjo4LFwiSXNQS0lEXCI6ZmFsc2UsXCJvcmRlcmlkXCI6MTB9LHtcImlkXCI6NTUsXCJjYXB0aW9uXCI6XCLmiqXorabkvJjlhYjnuqdcIixcIk5hbWVcIjpcIkFsYXJtUHJpb3JpdHlcIixcIklzQXV0b0luY3JlbWVudFwiOmZhbHNlLFwiQ2FuTnVsbFwiOnRydWUsXCJkYlR5cGVcIjpcImludFwiLFwibGVuZ3RoXCI6XCJcIixcIlRhYmxlSURcIjo4LFwiSXNQS0lEXCI6ZmFsc2UsXCJvcmRlcmlkXCI6MTF9LHtcImlkXCI6NTYsXCJjYXB0aW9uXCI6XCLmiqXorabnu4RcIixcIk5hbWVcIjpcIkFsYXJtR3JvdXBcIixcIklzQXV0b0luY3JlbWVudFwiOmZhbHNlLFwiQ2FuTnVsbFwiOnRydWUsXCJkYlR5cGVcIjpcInZhcmNoYXJcIixcImxlbmd0aFwiOlwiNTBcIixcIlRhYmxlSURcIjo4LFwiSXNQS0lEXCI6ZmFsc2UsXCJvcmRlcmlkXCI6MTJ9LHtcImlkXCI6NTcsXCJjYXB0aW9uXCI6XCLmiqXoraboh6rliqjnoa7orqRcIixcIk5hbWVcIjpcIkFsYXJtQXV0b0NvbmZpcm1cIixcIklzQXV0b0luY3JlbWVudFwiOmZhbHNlLFwiQ2FuTnVsbFwiOnRydWUsXCJkYlR5cGVcIjpcImJpdFwiLFwibGVuZ3RoXCI6XCJcIixcImRlZmF1bHRWYWx1ZVwiOlwiMFwiLFwiVGFibGVJRFwiOjgsXCJJc1BLSURcIjpmYWxzZSxcIm9yZGVyaWRcIjoxM30se1wiaWRcIjo1OCxcImNhcHRpb25cIjpcIuaKpeitpuiHquWKqOWkjeS9jVwiLFwiTmFtZVwiOlwiQWxhcm1BdXRvUmVzZXRcIixcIklzQXV0b0luY3JlbWVudFwiOmZhbHNlLFwiQ2FuTnVsbFwiOnRydWUsXCJkYlR5cGVcIjpcImJpdFwiLFwibGVuZ3RoXCI6XCJcIixcImRlZmF1bHRWYWx1ZVwiOlwiMFwiLFwiVGFibGVJRFwiOjgsXCJJc1BLSURcIjpmYWxzZSxcIm9yZGVyaWRcIjoxNH0se1wiaWRcIjo1OSxcImNhcHRpb25cIjpcIuaVsOaNrue7neWvueWPmOWMluS/neWtmFwiLFwiTmFtZVwiOlwiVmFsdWVBYnNvbHV0ZUNoYW5nZVwiLFwiSXNBdXRvSW5jcmVtZW50XCI6ZmFsc2UsXCJDYW5OdWxsXCI6dHJ1ZSxcImRiVHlwZVwiOlwiYml0XCIsXCJsZW5ndGhcIjpcIlwiLFwiZGVmYXVsdFZhbHVlXCI6XCIwXCIsXCJUYWJsZUlEXCI6OCxcIklzUEtJRFwiOmZhbHNlLFwib3JkZXJpZFwiOjE1fSx7XCJpZFwiOjYwLFwiY2FwdGlvblwiOlwi5pWw5o2u57ud5a+55Y+Y5YyW5YC8XCIsXCJOYW1lXCI6XCJWYWx1ZUFic29sdXRlQ2hhbmdlU2V0dGluZ1wiLFwiSXNBdXRvSW5jcmVtZW50XCI6ZmFsc2UsXCJDYW5OdWxsXCI6dHJ1ZSxcImRiVHlwZVwiOlwiZG91YmxlXCIsXCJsZW5ndGhcIjpcIlwiLFwiVGFibGVJRFwiOjgsXCJJc1BLSURcIjpmYWxzZSxcIm9yZGVyaWRcIjoxNn0se1wiaWRcIjo2MSxcImNhcHRpb25cIjpcIuaVsOaNruebuOWvueWPmOWMluS/neWtmFwiLFwiTmFtZVwiOlwiVmFsdWVSZWxhdGl2ZUNoYW5nZVwiLFwiSXNBdXRvSW5jcmVtZW50XCI6ZmFsc2UsXCJDYW5OdWxsXCI6dHJ1ZSxcImRiVHlwZVwiOlwiYml0XCIsXCJsZW5ndGhcIjpcIlwiLFwiZGVmYXVsdFZhbHVlXCI6XCIwXCIsXCJUYWJsZUlEXCI6OCxcIklzUEtJRFwiOmZhbHNlLFwib3JkZXJpZFwiOjE3fSx7XCJpZFwiOjYyLFwiY2FwdGlvblwiOlwi5pWw5o2u55u45a+55Y+Y5YyW5YC8XCIsXCJOYW1lXCI6XCJWYWx1ZVJlbGF0aXZlQ2hhbmdlU2V0dGluZ1wiLFwiSXNBdXRvSW5jcmVtZW50XCI6ZmFsc2UsXCJDYW5OdWxsXCI6dHJ1ZSxcImRiVHlwZVwiOlwiZG91YmxlXCIsXCJsZW5ndGhcIjpcIlwiLFwiVGFibGVJRFwiOjgsXCJJc1BLSURcIjpmYWxzZSxcIm9yZGVyaWRcIjoxOH0se1wiaWRcIjo2MyxcImNhcHRpb25cIjpcIuaVsOaNruWumuaXtuS/neWtmFwiLFwiTmFtZVwiOlwiVmFsdWVPblRpbWVDaGFuZ2VcIixcIklzQXV0b0luY3JlbWVudFwiOmZhbHNlLFwiQ2FuTnVsbFwiOnRydWUsXCJkYlR5cGVcIjpcImJpdFwiLFwibGVuZ3RoXCI6XCJcIixcImRlZmF1bHRWYWx1ZVwiOlwiMFwiLFwiVGFibGVJRFwiOjgsXCJJc1BLSURcIjpmYWxzZSxcIm9yZGVyaWRcIjoxOX0se1wiaWRcIjo2NCxcImNhcHRpb25cIjpcIuaVsOaNruWumuaXtuWAvFwiLFwiTmFtZVwiOlwiVmFsdWVPblRpbWVDaGFuZ2VTZXR0aW5nXCIsXCJJc0F1dG9JbmNyZW1lbnRcIjpmYWxzZSxcIkNhbk51bGxcIjp0cnVlLFwiZGJUeXBlXCI6XCJkb3VibGVcIixcImxlbmd0aFwiOlwiXCIsXCJUYWJsZUlEXCI6OCxcIklzUEtJRFwiOmZhbHNlLFwib3JkZXJpZFwiOjIwfSx7XCJpZFwiOjY1LFwiY2FwdGlvblwiOlwi54K557G75Z6LXCIsXCJOYW1lXCI6XCJUeXBlXCIsXCJJc0F1dG9JbmNyZW1lbnRcIjpmYWxzZSxcIkNhbk51bGxcIjp0cnVlLFwiZGJUeXBlXCI6XCJpbnRcIixcIkVudW1EZWZpbmVcIjpcIkFuYWxvZyA9IDEsXFxuRGlnaXRhbCA9IDJcIixcImxlbmd0aFwiOlwiXCIsXCJkZWZhdWx0VmFsdWVcIjpcIjJcIixcIlRhYmxlSURcIjo4LFwiSXNQS0lEXCI6ZmFsc2UsXCJvcmRlcmlkXCI6M30se1wiaWRcIjo2NixcImNhcHRpb25cIjpcIuaooeaLn+mHjy3lt6XnqIvljZXkvY1cIixcIk5hbWVcIjpcIlVuaXRcIixcIklzQXV0b0luY3JlbWVudFwiOmZhbHNlLFwiQ2FuTnVsbFwiOnRydWUsXCJkYlR5cGVcIjpcInZhcmNoYXJcIixcImxlbmd0aFwiOlwiNTBcIixcIlRhYmxlSURcIjo4LFwiSXNQS0lEXCI6ZmFsc2UsXCJvcmRlcmlkXCI6MjF9LHtcImlkXCI6NjcsXCJjYXB0aW9uXCI6XCLmqKHmi5/ph48t5bCP5pWw54K55L2N5pWwXCIsXCJOYW1lXCI6XCJEUENvdW50XCIsXCJJc0F1dG9JbmNyZW1lbnRcIjpmYWxzZSxcIkNhbk51bGxcIjp0cnVlLFwiZGJUeXBlXCI6XCJ2YXJjaGFyXCIsXCJsZW5ndGhcIjpcIjUwXCIsXCJUYWJsZUlEXCI6OCxcIklzUEtJRFwiOmZhbHNlLFwib3JkZXJpZFwiOjIyfSx7XCJpZFwiOjY4LFwiY2FwdGlvblwiOlwi5qih5ouf6YePLemHj+eoi+i9rOWMluW8gOWFs1wiLFwiTmFtZVwiOlwiSXNUcmFuc2Zvcm1cIixcIklzQXV0b0luY3JlbWVudFwiOmZhbHNlLFwiQ2FuTnVsbFwiOnRydWUsXCJkYlR5cGVcIjpcImJpdFwiLFwibGVuZ3RoXCI6XCJcIixcImRlZmF1bHRWYWx1ZVwiOlwiMFwiLFwiVGFibGVJRFwiOjgsXCJJc1BLSURcIjpmYWxzZSxcIm9yZGVyaWRcIjoyM30se1wiaWRcIjo2OSxcImNhcHRpb25cIjpcIuaooeaLn+mHjy3ph4/nqIvkuIrpmZBcIixcIk5hbWVcIjpcIlRyYW5zTWF4XCIsXCJJc0F1dG9JbmNyZW1lbnRcIjpmYWxzZSxcIkNhbk51bGxcIjp0cnVlLFwiZGJUeXBlXCI6XCJkb3VibGVcIixcImxlbmd0aFwiOlwiXCIsXCJUYWJsZUlEXCI6OCxcIklzUEtJRFwiOmZhbHNlLFwib3JkZXJpZFwiOjI0fSx7XCJpZFwiOjcwLFwiY2FwdGlvblwiOlwi5qih5ouf6YePLemHj+eoi+S4i+mZkFwiLFwiTmFtZVwiOlwiVHJhbnNNaW5cIixcIklzQXV0b0luY3JlbWVudFwiOmZhbHNlLFwiQ2FuTnVsbFwiOnRydWUsXCJkYlR5cGVcIjpcImRvdWJsZVwiLFwibGVuZ3RoXCI6XCJcIixcIlRhYmxlSURcIjo4LFwiSXNQS0lEXCI6ZmFsc2UsXCJvcmRlcmlkXCI6MjV9LHtcImlkXCI6NzEsXCJjYXB0aW9uXCI6XCLmqKHmi5/ph48t5Lyg5oSf5Zmo6YeP56iL5LiK6ZmQXCIsXCJOYW1lXCI6XCJTZW5zb3JNYXhcIixcIklzQXV0b0luY3JlbWVudFwiOmZhbHNlLFwiQ2FuTnVsbFwiOnRydWUsXCJkYlR5cGVcIjpcImRvdWJsZVwiLFwibGVuZ3RoXCI6XCJcIixcIlRhYmxlSURcIjo4LFwiSXNQS0lEXCI6ZmFsc2UsXCJvcmRlcmlkXCI6MjZ9LHtcImlkXCI6NzIsXCJjYXB0aW9uXCI6XCLmqKHmi5/ph48t5Lyg5oSf5Zmo6YeP56iL5LiL6ZmQXCIsXCJOYW1lXCI6XCJTZW5zb3JNaW5cIixcIklzQXV0b0luY3JlbWVudFwiOmZhbHNlLFwiQ2FuTnVsbFwiOnRydWUsXCJkYlR5cGVcIjpcInZhcmNoYXJcIixcImxlbmd0aFwiOlwiNTBcIixcIlRhYmxlSURcIjo4LFwiSXNQS0lEXCI6ZmFsc2UsXCJvcmRlcmlkXCI6Mjd9LHtcImlkXCI6NzMsXCJjYXB0aW9uXCI6XCLmqKHmi5/ph48t5byA5bmz5pa5XCIsXCJOYW1lXCI6XCJJc1NxdWFyZVwiLFwiSXNBdXRvSW5jcmVtZW50XCI6ZmFsc2UsXCJDYW5OdWxsXCI6dHJ1ZSxcImRiVHlwZVwiOlwiYml0XCIsXCJsZW5ndGhcIjpcIlwiLFwiZGVmYXVsdFZhbHVlXCI6XCIwXCIsXCJUYWJsZUlEXCI6OCxcIklzUEtJRFwiOmZhbHNlLFwib3JkZXJpZFwiOjI4fSx7XCJpZFwiOjc0LFwiY2FwdGlvblwiOlwi5YiG5q6157q/5oCn5YyWXCIsXCJOYW1lXCI6XCJJc0xpbmVhclwiLFwiSXNBdXRvSW5jcmVtZW50XCI6ZmFsc2UsXCJDYW5OdWxsXCI6dHJ1ZSxcImRiVHlwZVwiOlwiYml0XCIsXCJsZW5ndGhcIjpcIlwiLFwiZGVmYXVsdFZhbHVlXCI6XCIwXCIsXCJUYWJsZUlEXCI6OCxcIklzUEtJRFwiOmZhbHNlLFwib3JkZXJpZFwiOjI5fSx7XCJpZFwiOjc1LFwiY2FwdGlvblwiOlwi5YiG5q6157q/5oCn5YyWWDFcIixcIk5hbWVcIjpcIkxpbmVhclgxXCIsXCJJc0F1dG9JbmNyZW1lbnRcIjpmYWxzZSxcIkNhbk51bGxcIjp0cnVlLFwiZGJUeXBlXCI6XCJkb3VibGVcIixcImxlbmd0aFwiOlwiXCIsXCJUYWJsZUlEXCI6OCxcIklzUEtJRFwiOmZhbHNlLFwib3JkZXJpZFwiOjMwfSx7XCJpZFwiOjc2LFwiY2FwdGlvblwiOlwi5YiG5q6157q/5oCn5YyWWTFcIixcIk5hbWVcIjpcIkxpbmVhclkxXCIsXCJJc0F1dG9JbmNyZW1lbnRcIjpmYWxzZSxcIkNhbk51bGxcIjp0cnVlLFwiZGJUeXBlXCI6XCJkb3VibGVcIixcImxlbmd0aFwiOlwiXCIsXCJUYWJsZUlEXCI6OCxcIklzUEtJRFwiOmZhbHNlLFwib3JkZXJpZFwiOjMxfSx7XCJpZFwiOjc3LFwiY2FwdGlvblwiOlwi5YiG5q6157q/5oCn5YyWWDJcIixcIk5hbWVcIjpcIkxpbmVhclgyXCIsXCJJc0F1dG9JbmNyZW1lbnRcIjpmYWxzZSxcIkNhbk51bGxcIjp0cnVlLFwiZGJUeXBlXCI6XCJkb3VibGVcIixcImxlbmd0aFwiOlwiXCIsXCJUYWJsZUlEXCI6OCxcIklzUEtJRFwiOmZhbHNlLFwib3JkZXJpZFwiOjMyfSx7XCJpZFwiOjc4LFwiY2FwdGlvblwiOlwi5YiG5q6157q/5oCn5YyWWTJcIixcIk5hbWVcIjpcIkxpbmVhclkyXCIsXCJJc0F1dG9JbmNyZW1lbnRcIjpmYWxzZSxcIkNhbk51bGxcIjp0cnVlLFwiZGJUeXBlXCI6XCJkb3VibGVcIixcImxlbmd0aFwiOlwiXCIsXCJUYWJsZUlEXCI6OCxcIklzUEtJRFwiOmZhbHNlLFwib3JkZXJpZFwiOjMzfSx7XCJpZFwiOjc5LFwiTmFtZVwiOlwiTGluZWFyWDNcIixcIklzQXV0b0luY3JlbWVudFwiOmZhbHNlLFwiQ2FuTnVsbFwiOnRydWUsXCJkYlR5cGVcIjpcImRvdWJsZVwiLFwibGVuZ3RoXCI6XCJcIixcIlRhYmxlSURcIjo4LFwiSXNQS0lEXCI6ZmFsc2UsXCJvcmRlcmlkXCI6MzR9LHtcImlkXCI6ODAsXCJOYW1lXCI6XCJMaW5lYXJZM1wiLFwiSXNBdXRvSW5jcmVtZW50XCI6ZmFsc2UsXCJDYW5OdWxsXCI6dHJ1ZSxcImRiVHlwZVwiOlwiZG91YmxlXCIsXCJsZW5ndGhcIjpcIlwiLFwiVGFibGVJRFwiOjgsXCJJc1BLSURcIjpmYWxzZSxcIm9yZGVyaWRcIjozNX0se1wiaWRcIjo4MSxcIk5hbWVcIjpcIkxpbmVhclg0XCIsXCJJc0F1dG9JbmNyZW1lbnRcIjpmYWxzZSxcIkNhbk51bGxcIjp0cnVlLFwiZGJUeXBlXCI6XCJkb3VibGVcIixcImxlbmd0aFwiOlwiXCIsXCJUYWJsZUlEXCI6OCxcIklzUEtJRFwiOmZhbHNlLFwib3JkZXJpZFwiOjM2fSx7XCJpZFwiOjgyLFwiY2FwdGlvblwiOlwi5oql6K2m5YC8MlwiLFwiTmFtZVwiOlwiQWxhcm1WYWx1ZTJcIixcIklzQXV0b0luY3JlbWVudFwiOmZhbHNlLFwiQ2FuTnVsbFwiOnRydWUsXCJkYlR5cGVcIjpcImRvdWJsZVwiLFwibGVuZ3RoXCI6XCJcIixcIlRhYmxlSURcIjo4LFwiSXNQS0lEXCI6ZmFsc2UsXCJvcmRlcmlkXCI6Mzd9LHtcImlkXCI6ODMsXCJjYXB0aW9uXCI6XCLmiqXorabkvJjlhYjnuqcyXCIsXCJOYW1lXCI6XCJBbGFybVByaW9yaXR5MlwiLFwiSXNBdXRvSW5jcmVtZW50XCI6ZmFsc2UsXCJDYW5OdWxsXCI6dHJ1ZSxcImRiVHlwZVwiOlwiaW50XCIsXCJsZW5ndGhcIjpcIlwiLFwiVGFibGVJRFwiOjgsXCJJc1BLSURcIjpmYWxzZSxcIm9yZGVyaWRcIjozOH0se1wiaWRcIjo4NCxcIk5hbWVcIjpcIkFsYXJtVmFsdWUzXCIsXCJJc0F1dG9JbmNyZW1lbnRcIjpmYWxzZSxcIkNhbk51bGxcIjp0cnVlLFwiZGJUeXBlXCI6XCJkb3VibGVcIixcImxlbmd0aFwiOlwiXCIsXCJUYWJsZUlEXCI6OCxcIklzUEtJRFwiOmZhbHNlLFwib3JkZXJpZFwiOjM5fSx7XCJpZFwiOjg1LFwiTmFtZVwiOlwiQWxhcm1Qcmlvcml0eTNcIixcIklzQXV0b0luY3JlbWVudFwiOmZhbHNlLFwiQ2FuTnVsbFwiOnRydWUsXCJkYlR5cGVcIjpcImludFwiLFwibGVuZ3RoXCI6XCJcIixcIlRhYmxlSURcIjo4LFwiSXNQS0lEXCI6ZmFsc2UsXCJvcmRlcmlkXCI6NDB9LHtcImlkXCI6ODYsXCJOYW1lXCI6XCJBbGFybVZhbHVlNFwiLFwiSXNBdXRvSW5jcmVtZW50XCI6ZmFsc2UsXCJDYW5OdWxsXCI6dHJ1ZSxcImRiVHlwZVwiOlwiZG91YmxlXCIsXCJsZW5ndGhcIjpcIlwiLFwiVGFibGVJRFwiOjgsXCJJc1BLSURcIjpmYWxzZSxcIm9yZGVyaWRcIjo0MX0se1wiaWRcIjo4NyxcIk5hbWVcIjpcIkFsYXJtUHJpb3JpdHk0XCIsXCJJc0F1dG9JbmNyZW1lbnRcIjpmYWxzZSxcIkNhbk51bGxcIjp0cnVlLFwiZGJUeXBlXCI6XCJpbnRcIixcImxlbmd0aFwiOlwiXCIsXCJUYWJsZUlEXCI6OCxcIklzUEtJRFwiOmZhbHNlLFwib3JkZXJpZFwiOjQyfSx7XCJpZFwiOjg4LFwiTmFtZVwiOlwiQWxhcm1WYWx1ZTVcIixcIklzQXV0b0luY3JlbWVudFwiOmZhbHNlLFwiQ2FuTnVsbFwiOnRydWUsXCJkYlR5cGVcIjpcImRvdWJsZVwiLFwibGVuZ3RoXCI6XCJcIixcIlRhYmxlSURcIjo4LFwiSXNQS0lEXCI6ZmFsc2UsXCJvcmRlcmlkXCI6NDN9LHtcImlkXCI6ODksXCJOYW1lXCI6XCJBbGFybVByaW9yaXR5NVwiLFwiSXNBdXRvSW5jcmVtZW50XCI6ZmFsc2UsXCJDYW5OdWxsXCI6dHJ1ZSxcImRiVHlwZVwiOlwiaW50XCIsXCJsZW5ndGhcIjpcIlwiLFwiVGFibGVJRFwiOjgsXCJJc1BLSURcIjpmYWxzZSxcIm9yZGVyaWRcIjo0NH0se1wiaWRcIjo5MCxcImNhcHRpb25cIjpcIuWBj+W3ruaKpeitplwiLFwiTmFtZVwiOlwiSXNBbGFybU9mZnNldFwiLFwiSXNBdXRvSW5jcmVtZW50XCI6ZmFsc2UsXCJDYW5OdWxsXCI6dHJ1ZSxcImRiVHlwZVwiOlwiYml0XCIsXCJsZW5ndGhcIjpcIlwiLFwiZGVmYXVsdFZhbHVlXCI6XCIwXCIsXCJUYWJsZUlEXCI6OCxcIklzUEtJRFwiOmZhbHNlLFwib3JkZXJpZFwiOjQ1fSx7XCJpZFwiOjkxLFwiY2FwdGlvblwiOlwi5YGP5beu5oql6K2m6K6+5a6a5YC8XCIsXCJOYW1lXCI6XCJBbGFybU9mZnNldE9yaWdpbmFsVmFsdWVcIixcIklzQXV0b0luY3JlbWVudFwiOmZhbHNlLFwiQ2FuTnVsbFwiOnRydWUsXCJkYlR5cGVcIjpcImRvdWJsZVwiLFwibGVuZ3RoXCI6XCJcIixcIlRhYmxlSURcIjo4LFwiSXNQS0lEXCI6ZmFsc2UsXCJvcmRlcmlkXCI6NDZ9LHtcImlkXCI6OTIsXCJjYXB0aW9uXCI6XCLlgY/lt67lgLxcIixcIk5hbWVcIjpcIkFsYXJtT2Zmc2V0VmFsdWVcIixcIklzQXV0b0luY3JlbWVudFwiOmZhbHNlLFwiQ2FuTnVsbFwiOnRydWUsXCJkYlR5cGVcIjpcImRvdWJsZVwiLFwibGVuZ3RoXCI6XCJcIixcIlRhYmxlSURcIjo4LFwiSXNQS0lEXCI6ZmFsc2UsXCJvcmRlcmlkXCI6NDd9LHtcImlkXCI6OTMsXCJjYXB0aW9uXCI6XCLlgY/lt67miqXorabkvJjlhYjnuqdcIixcIk5hbWVcIjpcIkFsYXJtT2Zmc2V0UHJpb3JpdHlcIixcIklzQXV0b0luY3JlbWVudFwiOmZhbHNlLFwiQ2FuTnVsbFwiOnRydWUsXCJkYlR5cGVcIjpcImludFwiLFwibGVuZ3RoXCI6XCJcIixcIlRhYmxlSURcIjo4LFwiSXNQS0lEXCI6ZmFsc2UsXCJvcmRlcmlkXCI6NDh9LHtcImlkXCI6OTQsXCJjYXB0aW9uXCI6XCLlj5jljJbnjofmiqXoraZcIixcIk5hbWVcIjpcIklzQWxhcm1QZXJjZW50XCIsXCJJc0F1dG9JbmNyZW1lbnRcIjpmYWxzZSxcIkNhbk51bGxcIjp0cnVlLFwiZGJUeXBlXCI6XCJiaXRcIixcImxlbmd0aFwiOlwiXCIsXCJkZWZhdWx0VmFsdWVcIjpcIjBcIixcIlRhYmxlSURcIjo4LFwiSXNQS0lEXCI6ZmFsc2UsXCJvcmRlcmlkXCI6NDl9LHtcImlkXCI6OTUsXCJjYXB0aW9uXCI6XCLlj5jljJbnjoflgLzvvIjnmb7liIbmr5TvvIlcIixcIk5hbWVcIjpcIlBlcmNlbnRcIixcIklzQXV0b0luY3JlbWVudFwiOmZhbHNlLFwiQ2FuTnVsbFwiOnRydWUsXCJkYlR5cGVcIjpcImRvdWJsZVwiLFwibGVuZ3RoXCI6XCJcIixcIlRhYmxlSURcIjo4LFwiSXNQS0lEXCI6ZmFsc2UsXCJvcmRlcmlkXCI6NTB9LHtcImlkXCI6OTYsXCJjYXB0aW9uXCI6XCLlj5jljJbnjoflkajmnJ/vvIjnp5LvvIlcIixcIk5hbWVcIjpcIkNoYW5nZUN5Y2xlXCIsXCJJc0F1dG9JbmNyZW1lbnRcIjpmYWxzZSxcIkNhbk51bGxcIjp0cnVlLFwiZGJUeXBlXCI6XCJpbnRcIixcImxlbmd0aFwiOlwiXCIsXCJUYWJsZUlEXCI6OCxcIklzUEtJRFwiOmZhbHNlLFwib3JkZXJpZFwiOjUxfSx7XCJpZFwiOjk3LFwiY2FwdGlvblwiOlwi5Y+Y5YyW546H5oql6K2m5LyY5YWI57qnXCIsXCJOYW1lXCI6XCJBbGFybVBlcmNlbnRQcmlvcml0eVwiLFwiSXNBdXRvSW5jcmVtZW50XCI6ZmFsc2UsXCJDYW5OdWxsXCI6dHJ1ZSxcImRiVHlwZVwiOlwiaW50XCIsXCJsZW5ndGhcIjpcIlwiLFwiVGFibGVJRFwiOjgsXCJJc1BLSURcIjpmYWxzZSxcIm9yZGVyaWRcIjo1Mn1dLFwibmV3Q29sdW1uc1wiOltdLFwiY2hhbmdlZENvbHVtbnNcIjpbXSxcImRlbGV0ZWRDb2x1bW5zXCI6W10sXCJJRFhDb25maWdzXCI6W10sXCJJRFwiOjB9In0seyJOYW1lIjoiZGF0YWJhc2VpZCIsIlZhbHVlIjoyfV0sIlJvd1N0YXRlIjowfSx7Ikl0ZW1zIjpbeyJOYW1lIjoiaWQiLCJWYWx1ZSI6MzZ9LHsiTmFtZSI6InR5cGUiLCJWYWx1ZSI6IkNoYW5nZVRhYmxlQWN0aW9uIn0seyJOYW1lIjoiY29udGVudCIsIlZhbHVlIjoie1wiT2xkVGFibGVOYW1lXCI6XCJEZXZpY2VcIixcIk5ld1RhYmxlTmFtZVwiOlwiRGV2aWNlXCIsXCJvdGhlckNvbHVtbnNcIjpbe1wiaWRcIjozOSxcIk5hbWVcIjpcImlkXCIsXCJJc0F1dG9JbmNyZW1lbnRcIjp0cnVlLFwiQ2FuTnVsbFwiOmZhbHNlLFwiZGJUeXBlXCI6XCJpbnRcIixcIlRhYmxlSURcIjo3LFwiSXNQS0lEXCI6dHJ1ZSxcIm9yZGVyaWRcIjowfSx7XCJpZFwiOjQwLFwiTmFtZVwiOlwiRHJpdmVySURcIixcIklzQXV0b0luY3JlbWVudFwiOmZhbHNlLFwiQ2FuTnVsbFwiOnRydWUsXCJkYlR5cGVcIjpcImludFwiLFwibGVuZ3RoXCI6XCJcIixcIlRhYmxlSURcIjo3LFwiSXNQS0lEXCI6ZmFsc2UsXCJvcmRlcmlkXCI6MX0se1wiaWRcIjo0MSxcImNhcHRpb25cIjpcIuWcsOWdgOS/oeaBr1wiLFwiTmFtZVwiOlwiQWRkcmVzc1wiLFwiSXNBdXRvSW5jcmVtZW50XCI6ZmFsc2UsXCJDYW5OdWxsXCI6dHJ1ZSxcImRiVHlwZVwiOlwidmFyY2hhclwiLFwibGVuZ3RoXCI6XCIxMDBcIixcIlRhYmxlSURcIjo3LFwiSXNQS0lEXCI6ZmFsc2UsXCJvcmRlcmlkXCI6Mn1dLFwibmV3Q29sdW1uc1wiOlt7XCJpZFwiOjEwMCxcImNhcHRpb25cIjpcIuWcsOWdgOiuvue9ruaXtueahGpzb27lr7nosaHooajovr5cIixcIk5hbWVcIjpcIkFkZHJTZXR0aW5nXCIsXCJJc0F1dG9JbmNyZW1lbnRcIjpmYWxzZSxcIkNhbk51bGxcIjp0cnVlLFwiZGJUeXBlXCI6XCJ2YXJjaGFyXCIsXCJsZW5ndGhcIjpcIjIwMFwiLFwiVGFibGVJRFwiOjcsXCJJc1BLSURcIjpmYWxzZSxcIm9yZGVyaWRcIjozfV0sXCJjaGFuZ2VkQ29sdW1uc1wiOltdLFwiZGVsZXRlZENvbHVtbnNcIjpbXSxcIklEWENvbmZpZ3NcIjpbXSxcIklEXCI6MH0ifSx7Ik5hbWUiOiJkYXRhYmFzZWlkIiwiVmFsdWUiOjJ9XSwiUm93U3RhdGUiOjB9LHsiSXRlbXMiOlt7Ik5hbWUiOiJpZCIsIlZhbHVlIjozN30seyJOYW1lIjoidHlwZSIsIlZhbHVlIjoiQ2hhbmdlVGFibGVBY3Rpb24ifSx7Ik5hbWUiOiJjb250ZW50IiwiVmFsdWUiOiJ7XCJPbGRUYWJsZU5hbWVcIjpcIkRldmljZVwiLFwiTmV3VGFibGVOYW1lXCI6XCJEZXZpY2VcIixcIm90aGVyQ29sdW1uc1wiOlt7XCJpZFwiOjM5LFwiTmFtZVwiOlwiaWRcIixcIklzQXV0b0luY3JlbWVudFwiOnRydWUsXCJDYW5OdWxsXCI6ZmFsc2UsXCJkYlR5cGVcIjpcImludFwiLFwiVGFibGVJRFwiOjcsXCJJc1BLSURcIjp0cnVlLFwib3JkZXJpZFwiOjB9LHtcImlkXCI6NDAsXCJOYW1lXCI6XCJEcml2ZXJJRFwiLFwiSXNBdXRvSW5jcmVtZW50XCI6ZmFsc2UsXCJDYW5OdWxsXCI6dHJ1ZSxcImRiVHlwZVwiOlwiaW50XCIsXCJsZW5ndGhcIjpcIlwiLFwiVGFibGVJRFwiOjcsXCJJc1BLSURcIjpmYWxzZSxcIm9yZGVyaWRcIjoxfSx7XCJpZFwiOjQxLFwiY2FwdGlvblwiOlwi5Zyw5Z2A5L+h5oGvXCIsXCJOYW1lXCI6XCJBZGRyZXNzXCIsXCJJc0F1dG9JbmNyZW1lbnRcIjpmYWxzZSxcIkNhbk51bGxcIjp0cnVlLFwiZGJUeXBlXCI6XCJ2YXJjaGFyXCIsXCJsZW5ndGhcIjpcIjEwMFwiLFwiVGFibGVJRFwiOjcsXCJJc1BLSURcIjpmYWxzZSxcIm9yZGVyaWRcIjoyfSx7XCJpZFwiOjEwMCxcImNhcHRpb25cIjpcIuWcsOWdgOiuvue9ruaXtueahGpzb27lr7nosaHooajovr5cIixcIk5hbWVcIjpcIkFkZHJTZXR0aW5nXCIsXCJJc0F1dG9JbmNyZW1lbnRcIjpmYWxzZSxcIkNhbk51bGxcIjp0cnVlLFwiZGJUeXBlXCI6XCJ2YXJjaGFyXCIsXCJsZW5ndGhcIjpcIjIwMFwiLFwiVGFibGVJRFwiOjcsXCJJc1BLSURcIjpmYWxzZSxcIm9yZGVyaWRcIjozfV0sXCJuZXdDb2x1bW5zXCI6W3tcImlkXCI6MTAxLFwiTmFtZVwiOlwiTmFtZVwiLFwiSXNBdXRvSW5jcmVtZW50XCI6ZmFsc2UsXCJDYW5OdWxsXCI6dHJ1ZSxcImRiVHlwZVwiOlwidmFyY2hhclwiLFwibGVuZ3RoXCI6XCI1MFwiLFwiVGFibGVJRFwiOjcsXCJJc1BLSURcIjpmYWxzZSxcIm9yZGVyaWRcIjoyfV0sXCJjaGFuZ2VkQ29sdW1uc1wiOltdLFwiZGVsZXRlZENvbHVtbnNcIjpbXSxcIklEWENvbmZpZ3NcIjpbXSxcIklEXCI6MH0ifSx7Ik5hbWUiOiJkYXRhYmFzZWlkIiwiVmFsdWUiOjJ9XSwiUm93U3RhdGUiOjB9LHsiSXRlbXMiOlt7Ik5hbWUiOiJpZCIsIlZhbHVlIjozOH0seyJOYW1lIjoidHlwZSIsIlZhbHVlIjoiQ2hhbmdlVGFibGVBY3Rpb24ifSx7Ik5hbWUiOiJjb250ZW50IiwiVmFsdWUiOiJ7XCJPbGRUYWJsZU5hbWVcIjpcIkRldmljZVwiLFwiTmV3VGFibGVOYW1lXCI6XCJEZXZpY2VcIixcIm90aGVyQ29sdW1uc1wiOlt7XCJpZFwiOjM5LFwiTmFtZVwiOlwiaWRcIixcIklzQXV0b0luY3JlbWVudFwiOnRydWUsXCJDYW5OdWxsXCI6ZmFsc2UsXCJkYlR5cGVcIjpcImludFwiLFwiVGFibGVJRFwiOjcsXCJJc1BLSURcIjp0cnVlLFwib3JkZXJpZFwiOjB9LHtcImlkXCI6NDAsXCJOYW1lXCI6XCJEcml2ZXJJRFwiLFwiSXNBdXRvSW5jcmVtZW50XCI6ZmFsc2UsXCJDYW5OdWxsXCI6dHJ1ZSxcImRiVHlwZVwiOlwiaW50XCIsXCJsZW5ndGhcIjpcIlwiLFwiVGFibGVJRFwiOjcsXCJJc1BLSURcIjpmYWxzZSxcIm9yZGVyaWRcIjoxfSx7XCJpZFwiOjQxLFwiY2FwdGlvblwiOlwi5Zyw5Z2A5L+h5oGvXCIsXCJOYW1lXCI6XCJBZGRyZXNzXCIsXCJJc0F1dG9JbmNyZW1lbnRcIjpmYWxzZSxcIkNhbk51bGxcIjp0cnVlLFwiZGJUeXBlXCI6XCJ2YXJjaGFyXCIsXCJsZW5ndGhcIjpcIjEwMFwiLFwiVGFibGVJRFwiOjcsXCJJc1BLSURcIjpmYWxzZSxcIm9yZGVyaWRcIjozfSx7XCJpZFwiOjEwMCxcImNhcHRpb25cIjpcIuWcsOWdgOiuvue9ruaXtueahGpzb27lr7nosaHooajovr5cIixcIk5hbWVcIjpcIkFkZHJTZXR0aW5nXCIsXCJJc0F1dG9JbmNyZW1lbnRcIjpmYWxzZSxcIkNhbk51bGxcIjp0cnVlLFwiZGJUeXBlXCI6XCJ2YXJjaGFyXCIsXCJsZW5ndGhcIjpcIjIwMFwiLFwiVGFibGVJRFwiOjcsXCJJc1BLSURcIjpmYWxzZSxcIm9yZGVyaWRcIjo0fSx7XCJpZFwiOjEwMSxcIk5hbWVcIjpcIk5hbWVcIixcIklzQXV0b0luY3JlbWVudFwiOmZhbHNlLFwiQ2FuTnVsbFwiOnRydWUsXCJkYlR5cGVcIjpcInZhcmNoYXJcIixcImxlbmd0aFwiOlwiNTBcIixcIlRhYmxlSURcIjo3LFwiSXNQS0lEXCI6ZmFsc2UsXCJvcmRlcmlkXCI6Mn1dLFwibmV3Q29sdW1uc1wiOlt7XCJpZFwiOjEwMixcImNhcHRpb25cIjpcIuaOp+WItuWNleWFg2lkXCIsXCJOYW1lXCI6XCJVbml0SWRcIixcIklzQXV0b0luY3JlbWVudFwiOmZhbHNlLFwiQ2FuTnVsbFwiOnRydWUsXCJkYlR5cGVcIjpcImludFwiLFwibGVuZ3RoXCI6XCJcIixcIlRhYmxlSURcIjo3LFwiSXNQS0lEXCI6ZmFsc2UsXCJvcmRlcmlkXCI6NX1dLFwiY2hhbmdlZENvbHVtbnNcIjpbXSxcImRlbGV0ZWRDb2x1bW5zXCI6W10sXCJJRFhDb25maWdzXCI6W10sXCJJRFwiOjB9In0seyJOYW1lIjoiZGF0YWJhc2VpZCIsIlZhbHVlIjoyfV0sIlJvd1N0YXRlIjowfSx7Ikl0ZW1zIjpbeyJOYW1lIjoiaWQiLCJWYWx1ZSI6Mzl9LHsiTmFtZSI6InR5cGUiLCJWYWx1ZSI6IkNoYW5nZVRhYmxlQWN0aW9uIn0seyJOYW1lIjoiY29udGVudCIsIlZhbHVlIjoie1wiT2xkVGFibGVOYW1lXCI6XCJEZXZpY2VcIixcIk5ld1RhYmxlTmFtZVwiOlwiRGV2aWNlXCIsXCJvdGhlckNvbHVtbnNcIjpbe1wiaWRcIjozOSxcIk5hbWVcIjpcImlkXCIsXCJJc0F1dG9JbmNyZW1lbnRcIjp0cnVlLFwiQ2FuTnVsbFwiOmZhbHNlLFwiZGJUeXBlXCI6XCJpbnRcIixcIlRhYmxlSURcIjo3LFwiSXNQS0lEXCI6dHJ1ZSxcIm9yZGVyaWRcIjowfSx7XCJpZFwiOjQwLFwiTmFtZVwiOlwiRHJpdmVySURcIixcIklzQXV0b0luY3JlbWVudFwiOmZhbHNlLFwiQ2FuTnVsbFwiOnRydWUsXCJkYlR5cGVcIjpcImludFwiLFwibGVuZ3RoXCI6XCJcIixcIlRhYmxlSURcIjo3LFwiSXNQS0lEXCI6ZmFsc2UsXCJvcmRlcmlkXCI6MX0se1wiaWRcIjo0MSxcImNhcHRpb25cIjpcIuWcsOWdgOS/oeaBr1wiLFwiTmFtZVwiOlwiQWRkcmVzc1wiLFwiSXNBdXRvSW5jcmVtZW50XCI6ZmFsc2UsXCJDYW5OdWxsXCI6dHJ1ZSxcImRiVHlwZVwiOlwidmFyY2hhclwiLFwibGVuZ3RoXCI6XCIxMDBcIixcIlRhYmxlSURcIjo3LFwiSXNQS0lEXCI6ZmFsc2UsXCJvcmRlcmlkXCI6M30se1wiaWRcIjoxMDAsXCJjYXB0aW9uXCI6XCLlnLDlnYDorr7nva7ml7bnmoRqc29u5a+56LGh6KGo6L6+XCIsXCJOYW1lXCI6XCJBZGRyU2V0dGluZ1wiLFwiSXNBdXRvSW5jcmVtZW50XCI6ZmFsc2UsXCJDYW5OdWxsXCI6dHJ1ZSxcImRiVHlwZVwiOlwidmFyY2hhclwiLFwibGVuZ3RoXCI6XCIyMDBcIixcIlRhYmxlSURcIjo3LFwiSXNQS0lEXCI6ZmFsc2UsXCJvcmRlcmlkXCI6NH0se1wiaWRcIjoxMDEsXCJOYW1lXCI6XCJOYW1lXCIsXCJJc0F1dG9JbmNyZW1lbnRcIjpmYWxzZSxcIkNhbk51bGxcIjp0cnVlLFwiZGJUeXBlXCI6XCJ2YXJjaGFyXCIsXCJsZW5ndGhcIjpcIjUwXCIsXCJUYWJsZUlEXCI6NyxcIklzUEtJRFwiOmZhbHNlLFwib3JkZXJpZFwiOjJ9LHtcImlkXCI6MTAyLFwiY2FwdGlvblwiOlwi5o6n5Yi25Y2V5YWDaWRcIixcIk5hbWVcIjpcIlVuaXRJZFwiLFwiSXNBdXRvSW5jcmVtZW50XCI6ZmFsc2UsXCJDYW5OdWxsXCI6dHJ1ZSxcImRiVHlwZVwiOlwiaW50XCIsXCJsZW5ndGhcIjpcIlwiLFwiVGFibGVJRFwiOjcsXCJJc1BLSURcIjpmYWxzZSxcIm9yZGVyaWRcIjo1fV0sXCJuZXdDb2x1bW5zXCI6W10sXCJjaGFuZ2VkQ29sdW1uc1wiOltdLFwiZGVsZXRlZENvbHVtbnNcIjpbXSxcIklEWENvbmZpZ3NcIjpbXSxcIklEXCI6MH0ifSx7Ik5hbWUiOiJkYXRhYmFzZWlkIiwiVmFsdWUiOjJ9XSwiUm93U3RhdGUiOjB9LHsiSXRlbXMiOlt7Ik5hbWUiOiJpZCIsIlZhbHVlIjo0MH0seyJOYW1lIjoidHlwZSIsIlZhbHVlIjoiQ2hhbmdlVGFibGVBY3Rpb24ifSx7Ik5hbWUiOiJjb250ZW50IiwiVmFsdWUiOiJ7XCJPbGRUYWJsZU5hbWVcIjpcIkRldmljZVBvaW50XCIsXCJOZXdUYWJsZU5hbWVcIjpcIkRldmljZVBvaW50XCIsXCJvdGhlckNvbHVtbnNcIjpbe1wiaWRcIjo0MixcIk5hbWVcIjpcImlkXCIsXCJJc0F1dG9JbmNyZW1lbnRcIjp0cnVlLFwiQ2FuTnVsbFwiOmZhbHNlLFwiZGJUeXBlXCI6XCJpbnRcIixcIlRhYmxlSURcIjo4LFwiSXNQS0lEXCI6dHJ1ZSxcIm9yZGVyaWRcIjowfSx7XCJpZFwiOjQzLFwiY2FwdGlvblwiOlwi54K55ZCNXCIsXCJOYW1lXCI6XCJOYW1lXCIsXCJJc0F1dG9JbmNyZW1lbnRcIjpmYWxzZSxcIkNhbk51bGxcIjp0cnVlLFwiZGJUeXBlXCI6XCJpbnRcIixcImxlbmd0aFwiOlwiXCIsXCJUYWJsZUlEXCI6OCxcIklzUEtJRFwiOmZhbHNlLFwib3JkZXJpZFwiOjF9LHtcImlkXCI6NDQsXCJOYW1lXCI6XCJBZGRyZXNzXCIsXCJJc0F1dG9JbmNyZW1lbnRcIjpmYWxzZSxcIkNhbk51bGxcIjp0cnVlLFwiZGJUeXBlXCI6XCJ2YXJjaGFyXCIsXCJsZW5ndGhcIjpcIjIwMFwiLFwiVGFibGVJRFwiOjgsXCJJc1BLSURcIjpmYWxzZSxcIm9yZGVyaWRcIjozfSx7XCJpZFwiOjQ1LFwiY2FwdGlvblwiOlwi5piv5ZCm55uR5rWL5Y+Y5YyWXCIsXCJOYW1lXCI6XCJJc1dhdGNoaW5nXCIsXCJJc0F1dG9JbmNyZW1lbnRcIjpmYWxzZSxcIkNhbk51bGxcIjp0cnVlLFwiZGJUeXBlXCI6XCJiaXRcIixcImxlbmd0aFwiOlwiXCIsXCJkZWZhdWx0VmFsdWVcIjpcIjBcIixcIlRhYmxlSURcIjo4LFwiSXNQS0lEXCI6ZmFsc2UsXCJvcmRlcmlkXCI6NTN9LHtcImlkXCI6NDgsXCJjYXB0aW9uXCI6XCLlnLDlnYDorr7nva7ml7bnmoRqc29u5a+56LGh6KGo6L6+XCIsXCJOYW1lXCI6XCJBZGRyU2V0dGluZ1wiLFwiSXNBdXRvSW5jcmVtZW50XCI6ZmFsc2UsXCJDYW5OdWxsXCI6dHJ1ZSxcImRiVHlwZVwiOlwidmFyY2hhclwiLFwibGVuZ3RoXCI6XCIyMDBcIixcIlRhYmxlSURcIjo4LFwiSXNQS0lEXCI6ZmFsc2UsXCJvcmRlcmlkXCI6NH0se1wiaWRcIjo0OSxcImNhcHRpb25cIjpcIuaPj+i/sFwiLFwiTmFtZVwiOlwiRGVzY1wiLFwiSXNBdXRvSW5jcmVtZW50XCI6ZmFsc2UsXCJDYW5OdWxsXCI6dHJ1ZSxcImRiVHlwZVwiOlwidmFyY2hhclwiLFwibGVuZ3RoXCI6XCIxMDBcIixcIlRhYmxlSURcIjo4LFwiSXNQS0lEXCI6ZmFsc2UsXCJvcmRlcmlkXCI6Mn0se1wiaWRcIjo1MCxcImNhcHRpb25cIjpcIuWIneWni+WAvFwiLFwiTmFtZVwiOlwiSW5pdFZhbHVlXCIsXCJJc0F1dG9JbmNyZW1lbnRcIjpmYWxzZSxcIkNhbk51bGxcIjp0cnVlLFwiZGJUeXBlXCI6XCJkb3VibGVcIixcImxlbmd0aFwiOlwiXCIsXCJUYWJsZUlEXCI6OCxcIklzUEtJRFwiOmZhbHNlLFwib3JkZXJpZFwiOjZ9LHtcImlkXCI6NTEsXCJjYXB0aW9uXCI6XCLlvIDnirbmgIHkv6Hmga9cIixcIk5hbWVcIjpcIlN0YXR1c09wZW5JbmZvXCIsXCJJc0F1dG9JbmNyZW1lbnRcIjpmYWxzZSxcIkNhbk51bGxcIjp0cnVlLFwiZGJUeXBlXCI6XCJ2YXJjaGFyXCIsXCJsZW5ndGhcIjpcIjUwXCIsXCJUYWJsZUlEXCI6OCxcIklzUEtJRFwiOmZhbHNlLFwib3JkZXJpZFwiOjd9LHtcImlkXCI6NTIsXCJjYXB0aW9uXCI6XCLlhbPnirbmgIHkv6Hmga9cIixcIk5hbWVcIjpcIlN0YXRlQ2xvc2VJbmZvXCIsXCJJc0F1dG9JbmNyZW1lbnRcIjpmYWxzZSxcIkNhbk51bGxcIjp0cnVlLFwiZGJUeXBlXCI6XCJ2YXJjaGFyXCIsXCJsZW5ndGhcIjpcIjUwXCIsXCJUYWJsZUlEXCI6OCxcIklzUEtJRFwiOmZhbHNlLFwib3JkZXJpZFwiOjh9LHtcImlkXCI6NTMsXCJjYXB0aW9uXCI6XCLmiqXorablvIDlhbNcIixcIk5hbWVcIjpcIklzQWxhcm1cIixcIklzQXV0b0luY3JlbWVudFwiOmZhbHNlLFwiQ2FuTnVsbFwiOnRydWUsXCJkYlR5cGVcIjpcImJpdFwiLFwibGVuZ3RoXCI6XCJcIixcImRlZmF1bHRWYWx1ZVwiOlwiMFwiLFwiVGFibGVJRFwiOjgsXCJJc1BLSURcIjpmYWxzZSxcIm9yZGVyaWRcIjo5fSx7XCJpZFwiOjU0LFwiY2FwdGlvblwiOlwi5oql6K2m5YC8XCIsXCJOYW1lXCI6XCJBbGFybVZhbHVlXCIsXCJJc0F1dG9JbmNyZW1lbnRcIjpmYWxzZSxcIkNhbk51bGxcIjp0cnVlLFwiZGJUeXBlXCI6XCJkb3VibGVcIixcImxlbmd0aFwiOlwiXCIsXCJUYWJsZUlEXCI6OCxcIklzUEtJRFwiOmZhbHNlLFwib3JkZXJpZFwiOjEwfSx7XCJpZFwiOjU1LFwiY2FwdGlvblwiOlwi5oql6K2m5LyY5YWI57qnXCIsXCJOYW1lXCI6XCJBbGFybVByaW9yaXR5XCIsXCJJc0F1dG9JbmNyZW1lbnRcIjpmYWxzZSxcIkNhbk51bGxcIjp0cnVlLFwiZGJUeXBlXCI6XCJpbnRcIixcImxlbmd0aFwiOlwiXCIsXCJUYWJsZUlEXCI6OCxcIklzUEtJRFwiOmZhbHNlLFwib3JkZXJpZFwiOjExfSx7XCJpZFwiOjU2LFwiY2FwdGlvblwiOlwi5oql6K2m57uEXCIsXCJOYW1lXCI6XCJBbGFybUdyb3VwXCIsXCJJc0F1dG9JbmNyZW1lbnRcIjpmYWxzZSxcIkNhbk51bGxcIjp0cnVlLFwiZGJUeXBlXCI6XCJ2YXJjaGFyXCIsXCJsZW5ndGhcIjpcIjUwXCIsXCJUYWJsZUlEXCI6OCxcIklzUEtJRFwiOmZhbHNlLFwib3JkZXJpZFwiOjEyfSx7XCJpZFwiOjU3LFwiY2FwdGlvblwiOlwi5oql6K2m6Ieq5Yqo56Gu6K6kXCIsXCJOYW1lXCI6XCJBbGFybUF1dG9Db25maXJtXCIsXCJJc0F1dG9JbmNyZW1lbnRcIjpmYWxzZSxcIkNhbk51bGxcIjp0cnVlLFwiZGJUeXBlXCI6XCJiaXRcIixcImxlbmd0aFwiOlwiXCIsXCJkZWZhdWx0VmFsdWVcIjpcIjBcIixcIlRhYmxlSURcIjo4LFwiSXNQS0lEXCI6ZmFsc2UsXCJvcmRlcmlkXCI6MTN9LHtcImlkXCI6NTgsXCJjYXB0aW9uXCI6XCLmiqXoraboh6rliqjlpI3kvY1cIixcIk5hbWVcIjpcIkFsYXJtQXV0b1Jlc2V0XCIsXCJJc0F1dG9JbmNyZW1lbnRcIjpmYWxzZSxcIkNhbk51bGxcIjp0cnVlLFwiZGJUeXBlXCI6XCJiaXRcIixcImxlbmd0aFwiOlwiXCIsXCJkZWZhdWx0VmFsdWVcIjpcIjBcIixcIlRhYmxlSURcIjo4LFwiSXNQS0lEXCI6ZmFsc2UsXCJvcmRlcmlkXCI6MTR9LHtcImlkXCI6NTksXCJjYXB0aW9uXCI6XCLmlbDmja7nu53lr7nlj5jljJbkv53lrZhcIixcIk5hbWVcIjpcIlZhbHVlQWJzb2x1dGVDaGFuZ2VcIixcIklzQXV0b0luY3JlbWVudFwiOmZhbHNlLFwiQ2FuTnVsbFwiOnRydWUsXCJkYlR5cGVcIjpcImJpdFwiLFwibGVuZ3RoXCI6XCJcIixcImRlZmF1bHRWYWx1ZVwiOlwiMFwiLFwiVGFibGVJRFwiOjgsXCJJc1BLSURcIjpmYWxzZSxcIm9yZGVyaWRcIjoxNX0se1wiaWRcIjo2MCxcImNhcHRpb25cIjpcIuaVsOaNrue7neWvueWPmOWMluWAvFwiLFwiTmFtZVwiOlwiVmFsdWVBYnNvbHV0ZUNoYW5nZVNldHRpbmdcIixcIklzQXV0b0luY3JlbWVudFwiOmZhbHNlLFwiQ2FuTnVsbFwiOnRydWUsXCJkYlR5cGVcIjpcImRvdWJsZVwiLFwibGVuZ3RoXCI6XCJcIixcIlRhYmxlSURcIjo4LFwiSXNQS0lEXCI6ZmFsc2UsXCJvcmRlcmlkXCI6MTZ9LHtcImlkXCI6NjEsXCJjYXB0aW9uXCI6XCLmlbDmja7nm7jlr7nlj5jljJbkv53lrZhcIixcIk5hbWVcIjpcIlZhbHVlUmVsYXRpdmVDaGFuZ2VcIixcIklzQXV0b0luY3JlbWVudFwiOmZhbHNlLFwiQ2FuTnVsbFwiOnRydWUsXCJkYlR5cGVcIjpcImJpdFwiLFwibGVuZ3RoXCI6XCJcIixcImRlZmF1bHRWYWx1ZVwiOlwiMFwiLFwiVGFibGVJRFwiOjgsXCJJc1BLSURcIjpmYWxzZSxcIm9yZGVyaWRcIjoxN30se1wiaWRcIjo2MixcImNhcHRpb25cIjpcIuaVsOaNruebuOWvueWPmOWMluWAvFwiLFwiTmFtZVwiOlwiVmFsdWVSZWxhdGl2ZUNoYW5nZVNldHRpbmdcIixcIklzQXV0b0luY3JlbWVudFwiOmZhbHNlLFwiQ2FuTnVsbFwiOnRydWUsXCJkYlR5cGVcIjpcImRvdWJsZVwiLFwibGVuZ3RoXCI6XCJcIixcIlRhYmxlSURcIjo4LFwiSXNQS0lEXCI6ZmFsc2UsXCJvcmRlcmlkXCI6MTh9LHtcImlkXCI6NjMsXCJjYXB0aW9uXCI6XCLmlbDmja7lrprml7bkv53lrZhcIixcIk5hbWVcIjpcIlZhbHVlT25UaW1lQ2hhbmdlXCIsXCJJc0F1dG9JbmNyZW1lbnRcIjpmYWxzZSxcIkNhbk51bGxcIjp0cnVlLFwiZGJUeXBlXCI6XCJiaXRcIixcImxlbmd0aFwiOlwiXCIsXCJkZWZhdWx0VmFsdWVcIjpcIjBcIixcIlRhYmxlSURcIjo4LFwiSXNQS0lEXCI6ZmFsc2UsXCJvcmRlcmlkXCI6MTl9LHtcImlkXCI6NjQsXCJjYXB0aW9uXCI6XCLmlbDmja7lrprml7blgLxcIixcIk5hbWVcIjpcIlZhbHVlT25UaW1lQ2hhbmdlU2V0dGluZ1wiLFwiSXNBdXRvSW5jcmVtZW50XCI6ZmFsc2UsXCJDYW5OdWxsXCI6dHJ1ZSxcImRiVHlwZVwiOlwiZG91YmxlXCIsXCJsZW5ndGhcIjpcIlwiLFwiVGFibGVJRFwiOjgsXCJJc1BLSURcIjpmYWxzZSxcIm9yZGVyaWRcIjoyMH0se1wiaWRcIjo2NSxcImNhcHRpb25cIjpcIueCueexu+Wei1wiLFwiTmFtZVwiOlwiVHlwZVwiLFwiSXNBdXRvSW5jcmVtZW50XCI6ZmFsc2UsXCJDYW5OdWxsXCI6dHJ1ZSxcImRiVHlwZVwiOlwiaW50XCIsXCJFbnVtRGVmaW5lXCI6XCJBbmFsb2cgPSAxLFxcbkRpZ2l0YWwgPSAyXCIsXCJsZW5ndGhcIjpcIlwiLFwiZGVmYXVsdFZhbHVlXCI6XCIyXCIsXCJUYWJsZUlEXCI6OCxcIklzUEtJRFwiOmZhbHNlLFwib3JkZXJpZFwiOjV9LHtcImlkXCI6NjYsXCJjYXB0aW9uXCI6XCLmqKHmi5/ph48t5bel56iL5Y2V5L2NXCIsXCJOYW1lXCI6XCJVbml0XCIsXCJJc0F1dG9JbmNyZW1lbnRcIjpmYWxzZSxcIkNhbk51bGxcIjp0cnVlLFwiZGJUeXBlXCI6XCJ2YXJjaGFyXCIsXCJsZW5ndGhcIjpcIjUwXCIsXCJUYWJsZUlEXCI6OCxcIklzUEtJRFwiOmZhbHNlLFwib3JkZXJpZFwiOjIxfSx7XCJpZFwiOjY3LFwiY2FwdGlvblwiOlwi5qih5ouf6YePLeWwj+aVsOeCueS9jeaVsFwiLFwiTmFtZVwiOlwiRFBDb3VudFwiLFwiSXNBdXRvSW5jcmVtZW50XCI6ZmFsc2UsXCJDYW5OdWxsXCI6dHJ1ZSxcImRiVHlwZVwiOlwidmFyY2hhclwiLFwibGVuZ3RoXCI6XCI1MFwiLFwiVGFibGVJRFwiOjgsXCJJc1BLSURcIjpmYWxzZSxcIm9yZGVyaWRcIjoyMn0se1wiaWRcIjo2OCxcImNhcHRpb25cIjpcIuaooeaLn+mHjy3ph4/nqIvovazljJblvIDlhbNcIixcIk5hbWVcIjpcIklzVHJhbnNmb3JtXCIsXCJJc0F1dG9JbmNyZW1lbnRcIjpmYWxzZSxcIkNhbk51bGxcIjp0cnVlLFwiZGJUeXBlXCI6XCJiaXRcIixcImxlbmd0aFwiOlwiXCIsXCJkZWZhdWx0VmFsdWVcIjpcIjBcIixcIlRhYmxlSURcIjo4LFwiSXNQS0lEXCI6ZmFsc2UsXCJvcmRlcmlkXCI6MjN9LHtcImlkXCI6NjksXCJjYXB0aW9uXCI6XCLmqKHmi5/ph48t6YeP56iL5LiK6ZmQXCIsXCJOYW1lXCI6XCJUcmFuc01heFwiLFwiSXNBdXRvSW5jcmVtZW50XCI6ZmFsc2UsXCJDYW5OdWxsXCI6dHJ1ZSxcImRiVHlwZVwiOlwiZG91YmxlXCIsXCJsZW5ndGhcIjpcIlwiLFwiVGFibGVJRFwiOjgsXCJJc1BLSURcIjpmYWxzZSxcIm9yZGVyaWRcIjoyNH0se1wiaWRcIjo3MCxcImNhcHRpb25cIjpcIuaooeaLn+mHjy3ph4/nqIvkuIvpmZBcIixcIk5hbWVcIjpcIlRyYW5zTWluXCIsXCJJc0F1dG9JbmNyZW1lbnRcIjpmYWxzZSxcIkNhbk51bGxcIjp0cnVlLFwiZGJUeXBlXCI6XCJkb3VibGVcIixcImxlbmd0aFwiOlwiXCIsXCJUYWJsZUlEXCI6OCxcIklzUEtJRFwiOmZhbHNlLFwib3JkZXJpZFwiOjI1fSx7XCJpZFwiOjcxLFwiY2FwdGlvblwiOlwi5qih5ouf6YePLeS8oOaEn+WZqOmHj+eoi+S4iumZkFwiLFwiTmFtZVwiOlwiU2Vuc29yTWF4XCIsXCJJc0F1dG9JbmNyZW1lbnRcIjpmYWxzZSxcIkNhbk51bGxcIjp0cnVlLFwiZGJUeXBlXCI6XCJkb3VibGVcIixcImxlbmd0aFwiOlwiXCIsXCJUYWJsZUlEXCI6OCxcIklzUEtJRFwiOmZhbHNlLFwib3JkZXJpZFwiOjI2fSx7XCJpZFwiOjcyLFwiY2FwdGlvblwiOlwi5qih5ouf6YePLeS8oOaEn+WZqOmHj+eoi+S4i+mZkFwiLFwiTmFtZVwiOlwiU2Vuc29yTWluXCIsXCJJc0F1dG9JbmNyZW1lbnRcIjpmYWxzZSxcIkNhbk51bGxcIjp0cnVlLFwiZGJUeXBlXCI6XCJ2YXJjaGFyXCIsXCJsZW5ndGhcIjpcIjUwXCIsXCJUYWJsZUlEXCI6OCxcIklzUEtJRFwiOmZhbHNlLFwib3JkZXJpZFwiOjI3fSx7XCJpZFwiOjczLFwiY2FwdGlvblwiOlwi5qih5ouf6YePLeW8gOW5s+aWuVwiLFwiTmFtZVwiOlwiSXNTcXVhcmVcIixcIklzQXV0b0luY3JlbWVudFwiOmZhbHNlLFwiQ2FuTnVsbFwiOnRydWUsXCJkYlR5cGVcIjpcImJpdFwiLFwibGVuZ3RoXCI6XCJcIixcImRlZmF1bHRWYWx1ZVwiOlwiMFwiLFwiVGFibGVJRFwiOjgsXCJJc1BLSURcIjpmYWxzZSxcIm9yZGVyaWRcIjoyOH0se1wiaWRcIjo3NCxcImNhcHRpb25cIjpcIuWIhuautee6v+aAp+WMllwiLFwiTmFtZVwiOlwiSXNMaW5lYXJcIixcIklzQXV0b0luY3JlbWVudFwiOmZhbHNlLFwiQ2FuTnVsbFwiOnRydWUsXCJkYlR5cGVcIjpcImJpdFwiLFwibGVuZ3RoXCI6XCJcIixcImRlZmF1bHRWYWx1ZVwiOlwiMFwiLFwiVGFibGVJRFwiOjgsXCJJc1BLSURcIjpmYWxzZSxcIm9yZGVyaWRcIjoyOX0se1wiaWRcIjo3NSxcImNhcHRpb25cIjpcIuWIhuautee6v+aAp+WMllgxXCIsXCJOYW1lXCI6XCJMaW5lYXJYMVwiLFwiSXNBdXRvSW5jcmVtZW50XCI6ZmFsc2UsXCJDYW5OdWxsXCI6dHJ1ZSxcImRiVHlwZVwiOlwiZG91YmxlXCIsXCJsZW5ndGhcIjpcIlwiLFwiVGFibGVJRFwiOjgsXCJJc1BLSURcIjpmYWxzZSxcIm9yZGVyaWRcIjozMH0se1wiaWRcIjo3NixcImNhcHRpb25cIjpcIuWIhuautee6v+aAp+WMllkxXCIsXCJOYW1lXCI6XCJMaW5lYXJZMVwiLFwiSXNBdXRvSW5jcmVtZW50XCI6ZmFsc2UsXCJDYW5OdWxsXCI6dHJ1ZSxcImRiVHlwZVwiOlwiZG91YmxlXCIsXCJsZW5ndGhcIjpcIlwiLFwiVGFibGVJRFwiOjgsXCJJc1BLSURcIjpmYWxzZSxcIm9yZGVyaWRcIjozMX0se1wiaWRcIjo3NyxcImNhcHRpb25cIjpcIuWIhuautee6v+aAp+WMllgyXCIsXCJOYW1lXCI6XCJMaW5lYXJYMlwiLFwiSXNBdXRvSW5jcmVtZW50XCI6ZmFsc2UsXCJDYW5OdWxsXCI6dHJ1ZSxcImRiVHlwZVwiOlwiZG91YmxlXCIsXCJsZW5ndGhcIjpcIlwiLFwiVGFibGVJRFwiOjgsXCJJc1BLSURcIjpmYWxzZSxcIm9yZGVyaWRcIjozMn0se1wiaWRcIjo3OCxcImNhcHRpb25cIjpcIuWIhuautee6v+aAp+WMllkyXCIsXCJOYW1lXCI6XCJMaW5lYXJZMlwiLFwiSXNBdXRvSW5jcmVtZW50XCI6ZmFsc2UsXCJDYW5OdWxsXCI6dHJ1ZSxcImRiVHlwZVwiOlwiZG91YmxlXCIsXCJsZW5ndGhcIjpcIlwiLFwiVGFibGVJRFwiOjgsXCJJc1BLSURcIjpmYWxzZSxcIm9yZGVyaWRcIjozM30se1wiaWRcIjo3OSxcIk5hbWVcIjpcIkxpbmVhclgzXCIsXCJJc0F1dG9JbmNyZW1lbnRcIjpmYWxzZSxcIkNhbk51bGxcIjp0cnVlLFwiZGJUeXBlXCI6XCJkb3VibGVcIixcImxlbmd0aFwiOlwiXCIsXCJUYWJsZUlEXCI6OCxcIklzUEtJRFwiOmZhbHNlLFwib3JkZXJpZFwiOjM0fSx7XCJpZFwiOjgwLFwiTmFtZVwiOlwiTGluZWFyWTNcIixcIklzQXV0b0luY3JlbWVudFwiOmZhbHNlLFwiQ2FuTnVsbFwiOnRydWUsXCJkYlR5cGVcIjpcImRvdWJsZVwiLFwibGVuZ3RoXCI6XCJcIixcIlRhYmxlSURcIjo4LFwiSXNQS0lEXCI6ZmFsc2UsXCJvcmRlcmlkXCI6MzV9LHtcImlkXCI6ODEsXCJOYW1lXCI6XCJMaW5lYXJYNFwiLFwiSXNBdXRvSW5jcmVtZW50XCI6ZmFsc2UsXCJDYW5OdWxsXCI6dHJ1ZSxcImRiVHlwZVwiOlwiZG91YmxlXCIsXCJsZW5ndGhcIjpcIlwiLFwiVGFibGVJRFwiOjgsXCJJc1BLSURcIjpmYWxzZSxcIm9yZGVyaWRcIjozNn0se1wiaWRcIjo4MixcImNhcHRpb25cIjpcIuaKpeitpuWAvDJcIixcIk5hbWVcIjpcIkFsYXJtVmFsdWUyXCIsXCJJc0F1dG9JbmNyZW1lbnRcIjpmYWxzZSxcIkNhbk51bGxcIjp0cnVlLFwiZGJUeXBlXCI6XCJkb3VibGVcIixcImxlbmd0aFwiOlwiXCIsXCJUYWJsZUlEXCI6OCxcIklzUEtJRFwiOmZhbHNlLFwib3JkZXJpZFwiOjM3fSx7XCJpZFwiOjgzLFwiY2FwdGlvblwiOlwi5oql6K2m5LyY5YWI57qnMlwiLFwiTmFtZVwiOlwiQWxhcm1Qcmlvcml0eTJcIixcIklzQXV0b0luY3JlbWVudFwiOmZhbHNlLFwiQ2FuTnVsbFwiOnRydWUsXCJkYlR5cGVcIjpcImludFwiLFwibGVuZ3RoXCI6XCJcIixcIlRhYmxlSURcIjo4LFwiSXNQS0lEXCI6ZmFsc2UsXCJvcmRlcmlkXCI6Mzh9LHtcImlkXCI6ODQsXCJOYW1lXCI6XCJBbGFybVZhbHVlM1wiLFwiSXNBdXRvSW5jcmVtZW50XCI6ZmFsc2UsXCJDYW5OdWxsXCI6dHJ1ZSxcImRiVHlwZVwiOlwiZG91YmxlXCIsXCJsZW5ndGhcIjpcIlwiLFwiVGFibGVJRFwiOjgsXCJJc1BLSURcIjpmYWxzZSxcIm9yZGVyaWRcIjozOX0se1wiaWRcIjo4NSxcIk5hbWVcIjpcIkFsYXJtUHJpb3JpdHkzXCIsXCJJc0F1dG9JbmNyZW1lbnRcIjpmYWxzZSxcIkNhbk51bGxcIjp0cnVlLFwiZGJUeXBlXCI6XCJpbnRcIixcImxlbmd0aFwiOlwiXCIsXCJUYWJsZUlEXCI6OCxcIklzUEtJRFwiOmZhbHNlLFwib3JkZXJpZFwiOjQwfSx7XCJpZFwiOjg2LFwiTmFtZVwiOlwiQWxhcm1WYWx1ZTRcIixcIklzQXV0b0luY3JlbWVudFwiOmZhbHNlLFwiQ2FuTnVsbFwiOnRydWUsXCJkYlR5cGVcIjpcImRvdWJsZVwiLFwibGVuZ3RoXCI6XCJcIixcIlRhYmxlSURcIjo4LFwiSXNQS0lEXCI6ZmFsc2UsXCJvcmRlcmlkXCI6NDF9LHtcImlkXCI6ODcsXCJOYW1lXCI6XCJBbGFybVByaW9yaXR5NFwiLFwiSXNBdXRvSW5jcmVtZW50XCI6ZmFsc2UsXCJDYW5OdWxsXCI6dHJ1ZSxcImRiVHlwZVwiOlwiaW50XCIsXCJsZW5ndGhcIjpcIlwiLFwiVGFibGVJRFwiOjgsXCJJc1BLSURcIjpmYWxzZSxcIm9yZGVyaWRcIjo0Mn0se1wiaWRcIjo4OCxcIk5hbWVcIjpcIkFsYXJtVmFsdWU1XCIsXCJJc0F1dG9JbmNyZW1lbnRcIjpmYWxzZSxcIkNhbk51bGxcIjp0cnVlLFwiZGJUeXBlXCI6XCJkb3VibGVcIixcImxlbmd0aFwiOlwiXCIsXCJUYWJsZUlEXCI6OCxcIklzUEtJRFwiOmZhbHNlLFwib3JkZXJpZFwiOjQzfSx7XCJpZFwiOjg5LFwiTmFtZVwiOlwiQWxhcm1Qcmlvcml0eTVcIixcIklzQXV0b0luY3JlbWVudFwiOmZhbHNlLFwiQ2FuTnVsbFwiOnRydWUsXCJkYlR5cGVcIjpcImludFwiLFwibGVuZ3RoXCI6XCJcIixcIlRhYmxlSURcIjo4LFwiSXNQS0lEXCI6ZmFsc2UsXCJvcmRlcmlkXCI6NDR9LHtcImlkXCI6OTAsXCJjYXB0aW9uXCI6XCLlgY/lt67miqXoraZcIixcIk5hbWVcIjpcIklzQWxhcm1PZmZzZXRcIixcIklzQXV0b0luY3JlbWVudFwiOmZhbHNlLFwiQ2FuTnVsbFwiOnRydWUsXCJkYlR5cGVcIjpcImJpdFwiLFwibGVuZ3RoXCI6XCJcIixcImRlZmF1bHRWYWx1ZVwiOlwiMFwiLFwiVGFibGVJRFwiOjgsXCJJc1BLSURcIjpmYWxzZSxcIm9yZGVyaWRcIjo0NX0se1wiaWRcIjo5MSxcImNhcHRpb25cIjpcIuWBj+W3ruaKpeitpuiuvuWumuWAvFwiLFwiTmFtZVwiOlwiQWxhcm1PZmZzZXRPcmlnaW5hbFZhbHVlXCIsXCJJc0F1dG9JbmNyZW1lbnRcIjpmYWxzZSxcIkNhbk51bGxcIjp0cnVlLFwiZGJUeXBlXCI6XCJkb3VibGVcIixcImxlbmd0aFwiOlwiXCIsXCJUYWJsZUlEXCI6OCxcIklzUEtJRFwiOmZhbHNlLFwib3JkZXJpZFwiOjQ2fSx7XCJpZFwiOjkyLFwiY2FwdGlvblwiOlwi5YGP5beu5YC8XCIsXCJOYW1lXCI6XCJBbGFybU9mZnNldFZhbHVlXCIsXCJJc0F1dG9JbmNyZW1lbnRcIjpmYWxzZSxcIkNhbk51bGxcIjp0cnVlLFwiZGJUeXBlXCI6XCJkb3VibGVcIixcImxlbmd0aFwiOlwiXCIsXCJUYWJsZUlEXCI6OCxcIklzUEtJRFwiOmZhbHNlLFwib3JkZXJpZFwiOjQ3fSx7XCJpZFwiOjkzLFwiY2FwdGlvblwiOlwi5YGP5beu5oql6K2m5LyY5YWI57qnXCIsXCJOYW1lXCI6XCJBbGFybU9mZnNldFByaW9yaXR5XCIsXCJJc0F1dG9JbmNyZW1lbnRcIjpmYWxzZSxcIkNhbk51bGxcIjp0cnVlLFwiZGJUeXBlXCI6XCJpbnRcIixcImxlbmd0aFwiOlwiXCIsXCJUYWJsZUlEXCI6OCxcIklzUEtJRFwiOmZhbHNlLFwib3JkZXJpZFwiOjQ4fSx7XCJpZFwiOjk0LFwiY2FwdGlvblwiOlwi5Y+Y5YyW546H5oql6K2mXCIsXCJOYW1lXCI6XCJJc0FsYXJtUGVyY2VudFwiLFwiSXNBdXRvSW5jcmVtZW50XCI6ZmFsc2UsXCJDYW5OdWxsXCI6dHJ1ZSxcImRiVHlwZVwiOlwiYml0XCIsXCJsZW5ndGhcIjpcIlwiLFwiZGVmYXVsdFZhbHVlXCI6XCIwXCIsXCJUYWJsZUlEXCI6OCxcIklzUEtJRFwiOmZhbHNlLFwib3JkZXJpZFwiOjQ5fSx7XCJpZFwiOjk1LFwiY2FwdGlvblwiOlwi5Y+Y5YyW546H5YC877yI55m+5YiG5q+U77yJXCIsXCJOYW1lXCI6XCJQZXJjZW50XCIsXCJJc0F1dG9JbmNyZW1lbnRcIjpmYWxzZSxcIkNhbk51bGxcIjp0cnVlLFwiZGJUeXBlXCI6XCJkb3VibGVcIixcImxlbmd0aFwiOlwiXCIsXCJUYWJsZUlEXCI6OCxcIklzUEtJRFwiOmZhbHNlLFwib3JkZXJpZFwiOjUwfSx7XCJpZFwiOjk2LFwiY2FwdGlvblwiOlwi5Y+Y5YyW546H5ZGo5pyf77yI56eS77yJXCIsXCJOYW1lXCI6XCJDaGFuZ2VDeWNsZVwiLFwiSXNBdXRvSW5jcmVtZW50XCI6ZmFsc2UsXCJDYW5OdWxsXCI6dHJ1ZSxcImRiVHlwZVwiOlwiaW50XCIsXCJsZW5ndGhcIjpcIlwiLFwiVGFibGVJRFwiOjgsXCJJc1BLSURcIjpmYWxzZSxcIm9yZGVyaWRcIjo1MX0se1wiaWRcIjo5NyxcImNhcHRpb25cIjpcIuWPmOWMlueOh+aKpeitpuS8mOWFiOe6p1wiLFwiTmFtZVwiOlwiQWxhcm1QZXJjZW50UHJpb3JpdHlcIixcIklzQXV0b0luY3JlbWVudFwiOmZhbHNlLFwiQ2FuTnVsbFwiOnRydWUsXCJkYlR5cGVcIjpcImludFwiLFwibGVuZ3RoXCI6XCJcIixcIlRhYmxlSURcIjo4LFwiSXNQS0lEXCI6ZmFsc2UsXCJvcmRlcmlkXCI6NTJ9XSxcIm5ld0NvbHVtbnNcIjpbe1wiaWRcIjoxMDMsXCJOYW1lXCI6XCJQYXJlbnRJZFwiLFwiSXNBdXRvSW5jcmVtZW50XCI6ZmFsc2UsXCJDYW5OdWxsXCI6dHJ1ZSxcImRiVHlwZVwiOlwiaW50XCIsXCJsZW5ndGhcIjpcIlwiLFwiVGFibGVJRFwiOjgsXCJJc1BLSURcIjpmYWxzZSxcIm9yZGVyaWRcIjo2fV0sXCJjaGFuZ2VkQ29sdW1uc1wiOltdLFwiZGVsZXRlZENvbHVtbnNcIjpbXSxcIklEWENvbmZpZ3NcIjpbXSxcIklEXCI6MH0ifSx7Ik5hbWUiOiJkYXRhYmFzZWlkIiwiVmFsdWUiOjJ9XSwiUm93U3RhdGUiOjB9LHsiSXRlbXMiOlt7Ik5hbWUiOiJpZCIsIlZhbHVlIjo0MX0seyJOYW1lIjoidHlwZSIsIlZhbHVlIjoiQ2hhbmdlVGFibGVBY3Rpb24ifSx7Ik5hbWUiOiJjb250ZW50IiwiVmFsdWUiOiJ7XCJPbGRUYWJsZU5hbWVcIjpcIkRldmljZVBvaW50XCIsXCJOZXdUYWJsZU5hbWVcIjpcIkRldmljZVBvaW50XCIsXCJvdGhlckNvbHVtbnNcIjpbe1wiaWRcIjo0MixcIk5hbWVcIjpcImlkXCIsXCJJc0F1dG9JbmNyZW1lbnRcIjp0cnVlLFwiQ2FuTnVsbFwiOmZhbHNlLFwiZGJUeXBlXCI6XCJpbnRcIixcIlRhYmxlSURcIjo4LFwiSXNQS0lEXCI6dHJ1ZSxcIm9yZGVyaWRcIjowfSx7XCJpZFwiOjQzLFwiY2FwdGlvblwiOlwi54K55ZCNXCIsXCJOYW1lXCI6XCJOYW1lXCIsXCJJc0F1dG9JbmNyZW1lbnRcIjpmYWxzZSxcIkNhbk51bGxcIjp0cnVlLFwiZGJUeXBlXCI6XCJpbnRcIixcImxlbmd0aFwiOlwiXCIsXCJUYWJsZUlEXCI6OCxcIklzUEtJRFwiOmZhbHNlLFwib3JkZXJpZFwiOjF9LHtcImlkXCI6NDQsXCJOYW1lXCI6XCJBZGRyZXNzXCIsXCJJc0F1dG9JbmNyZW1lbnRcIjpmYWxzZSxcIkNhbk51bGxcIjp0cnVlLFwiZGJUeXBlXCI6XCJ2YXJjaGFyXCIsXCJsZW5ndGhcIjpcIjIwMFwiLFwiVGFibGVJRFwiOjgsXCJJc1BLSURcIjpmYWxzZSxcIm9yZGVyaWRcIjozfSx7XCJpZFwiOjQ1LFwiY2FwdGlvblwiOlwi5piv5ZCm55uR5rWL5Y+Y5YyWXCIsXCJOYW1lXCI6XCJJc1dhdGNoaW5nXCIsXCJJc0F1dG9JbmNyZW1lbnRcIjpmYWxzZSxcIkNhbk51bGxcIjp0cnVlLFwiZGJUeXBlXCI6XCJiaXRcIixcImxlbmd0aFwiOlwiXCIsXCJkZWZhdWx0VmFsdWVcIjpcIjBcIixcIlRhYmxlSURcIjo4LFwiSXNQS0lEXCI6ZmFsc2UsXCJvcmRlcmlkXCI6NTR9LHtcImlkXCI6NDgsXCJjYXB0aW9uXCI6XCLlnLDlnYDorr7nva7ml7bnmoRqc29u5a+56LGh6KGo6L6+XCIsXCJOYW1lXCI6XCJBZGRyU2V0dGluZ1wiLFwiSXNBdXRvSW5jcmVtZW50XCI6ZmFsc2UsXCJDYW5OdWxsXCI6dHJ1ZSxcImRiVHlwZVwiOlwidmFyY2hhclwiLFwibGVuZ3RoXCI6XCIyMDBcIixcIlRhYmxlSURcIjo4LFwiSXNQS0lEXCI6ZmFsc2UsXCJvcmRlcmlkXCI6NH0se1wiaWRcIjo0OSxcImNhcHRpb25cIjpcIuaPj+i/sFwiLFwiTmFtZVwiOlwiRGVzY1wiLFwiSXNBdXRvSW5jcmVtZW50XCI6ZmFsc2UsXCJDYW5OdWxsXCI6dHJ1ZSxcImRiVHlwZVwiOlwidmFyY2hhclwiLFwibGVuZ3RoXCI6XCIxMDBcIixcIlRhYmxlSURcIjo4LFwiSXNQS0lEXCI6ZmFsc2UsXCJvcmRlcmlkXCI6Mn0se1wiaWRcIjo1MCxcImNhcHRpb25cIjpcIuWIneWni+WAvFwiLFwiTmFtZVwiOlwiSW5pdFZhbHVlXCIsXCJJc0F1dG9JbmNyZW1lbnRcIjpmYWxzZSxcIkNhbk51bGxcIjp0cnVlLFwiZGJUeXBlXCI6XCJkb3VibGVcIixcImxlbmd0aFwiOlwiXCIsXCJUYWJsZUlEXCI6OCxcIklzUEtJRFwiOmZhbHNlLFwib3JkZXJpZFwiOjd9LHtcImlkXCI6NTEsXCJjYXB0aW9uXCI6XCLlvIDnirbmgIHkv6Hmga9cIixcIk5hbWVcIjpcIlN0YXR1c09wZW5JbmZvXCIsXCJJc0F1dG9JbmNyZW1lbnRcIjpmYWxzZSxcIkNhbk51bGxcIjp0cnVlLFwiZGJUeXBlXCI6XCJ2YXJjaGFyXCIsXCJsZW5ndGhcIjpcIjUwXCIsXCJUYWJsZUlEXCI6OCxcIklzUEtJRFwiOmZhbHNlLFwib3JkZXJpZFwiOjh9LHtcImlkXCI6NTIsXCJjYXB0aW9uXCI6XCLlhbPnirbmgIHkv6Hmga9cIixcIk5hbWVcIjpcIlN0YXRlQ2xvc2VJbmZvXCIsXCJJc0F1dG9JbmNyZW1lbnRcIjpmYWxzZSxcIkNhbk51bGxcIjp0cnVlLFwiZGJUeXBlXCI6XCJ2YXJjaGFyXCIsXCJsZW5ndGhcIjpcIjUwXCIsXCJUYWJsZUlEXCI6OCxcIklzUEtJRFwiOmZhbHNlLFwib3JkZXJpZFwiOjl9LHtcImlkXCI6NTMsXCJjYXB0aW9uXCI6XCLmiqXorablvIDlhbNcIixcIk5hbWVcIjpcIklzQWxhcm1cIixcIklzQXV0b0luY3JlbWVudFwiOmZhbHNlLFwiQ2FuTnVsbFwiOnRydWUsXCJkYlR5cGVcIjpcImJpdFwiLFwibGVuZ3RoXCI6XCJcIixcImRlZmF1bHRWYWx1ZVwiOlwiMFwiLFwiVGFibGVJRFwiOjgsXCJJc1BLSURcIjpmYWxzZSxcIm9yZGVyaWRcIjoxMH0se1wiaWRcIjo1NCxcImNhcHRpb25cIjpcIuaKpeitpuWAvFwiLFwiTmFtZVwiOlwiQWxhcm1WYWx1ZVwiLFwiSXNBdXRvSW5jcmVtZW50XCI6ZmFsc2UsXCJDYW5OdWxsXCI6dHJ1ZSxcImRiVHlwZVwiOlwiZG91YmxlXCIsXCJsZW5ndGhcIjpcIlwiLFwiVGFibGVJRFwiOjgsXCJJc1BLSURcIjpmYWxzZSxcIm9yZGVyaWRcIjoxMX0se1wiaWRcIjo1NSxcImNhcHRpb25cIjpcIuaKpeitpuS8mOWFiOe6p1wiLFwiTmFtZVwiOlwiQWxhcm1Qcmlvcml0eVwiLFwiSXNBdXRvSW5jcmVtZW50XCI6ZmFsc2UsXCJDYW5OdWxsXCI6dHJ1ZSxcImRiVHlwZVwiOlwiaW50XCIsXCJsZW5ndGhcIjpcIlwiLFwiVGFibGVJRFwiOjgsXCJJc1BLSURcIjpmYWxzZSxcIm9yZGVyaWRcIjoxMn0se1wiaWRcIjo1NixcImNhcHRpb25cIjpcIuaKpeitpue7hFwiLFwiTmFtZVwiOlwiQWxhcm1Hcm91cFwiLFwiSXNBdXRvSW5jcmVtZW50XCI6ZmFsc2UsXCJDYW5OdWxsXCI6dHJ1ZSxcImRiVHlwZVwiOlwidmFyY2hhclwiLFwibGVuZ3RoXCI6XCI1MFwiLFwiVGFibGVJRFwiOjgsXCJJc1BLSURcIjpmYWxzZSxcIm9yZGVyaWRcIjoxM30se1wiaWRcIjo1NyxcImNhcHRpb25cIjpcIuaKpeitpuiHquWKqOehruiupFwiLFwiTmFtZVwiOlwiQWxhcm1BdXRvQ29uZmlybVwiLFwiSXNBdXRvSW5jcmVtZW50XCI6ZmFsc2UsXCJDYW5OdWxsXCI6dHJ1ZSxcImRiVHlwZVwiOlwiYml0XCIsXCJsZW5ndGhcIjpcIlwiLFwiZGVmYXVsdFZhbHVlXCI6XCIwXCIsXCJUYWJsZUlEXCI6OCxcIklzUEtJRFwiOmZhbHNlLFwib3JkZXJpZFwiOjE0fSx7XCJpZFwiOjU4LFwiY2FwdGlvblwiOlwi5oql6K2m6Ieq5Yqo5aSN5L2NXCIsXCJOYW1lXCI6XCJBbGFybUF1dG9SZXNldFwiLFwiSXNBdXRvSW5jcmVtZW50XCI6ZmFsc2UsXCJDYW5OdWxsXCI6dHJ1ZSxcImRiVHlwZVwiOlwiYml0XCIsXCJsZW5ndGhcIjpcIlwiLFwiZGVmYXVsdFZhbHVlXCI6XCIwXCIsXCJUYWJsZUlEXCI6OCxcIklzUEtJRFwiOmZhbHNlLFwib3JkZXJpZFwiOjE1fSx7XCJpZFwiOjU5LFwiY2FwdGlvblwiOlwi5pWw5o2u57ud5a+55Y+Y5YyW5L+d5a2YXCIsXCJOYW1lXCI6XCJWYWx1ZUFic29sdXRlQ2hhbmdlXCIsXCJJc0F1dG9JbmNyZW1lbnRcIjpmYWxzZSxcIkNhbk51bGxcIjp0cnVlLFwiZGJUeXBlXCI6XCJiaXRcIixcImxlbmd0aFwiOlwiXCIsXCJkZWZhdWx0VmFsdWVcIjpcIjBcIixcIlRhYmxlSURcIjo4LFwiSXNQS0lEXCI6ZmFsc2UsXCJvcmRlcmlkXCI6MTZ9LHtcImlkXCI6NjAsXCJjYXB0aW9uXCI6XCLmlbDmja7nu53lr7nlj5jljJblgLxcIixcIk5hbWVcIjpcIlZhbHVlQWJzb2x1dGVDaGFuZ2VTZXR0aW5nXCIsXCJJc0F1dG9JbmNyZW1lbnRcIjpmYWxzZSxcIkNhbk51bGxcIjp0cnVlLFwiZGJUeXBlXCI6XCJkb3VibGVcIixcImxlbmd0aFwiOlwiXCIsXCJUYWJsZUlEXCI6OCxcIklzUEtJRFwiOmZhbHNlLFwib3JkZXJpZFwiOjE3fSx7XCJpZFwiOjYxLFwiY2FwdGlvblwiOlwi5pWw5o2u55u45a+55Y+Y5YyW5L+d5a2YXCIsXCJOYW1lXCI6XCJWYWx1ZVJlbGF0aXZlQ2hhbmdlXCIsXCJJc0F1dG9JbmNyZW1lbnRcIjpmYWxzZSxcIkNhbk51bGxcIjp0cnVlLFwiZGJUeXBlXCI6XCJiaXRcIixcImxlbmd0aFwiOlwiXCIsXCJkZWZhdWx0VmFsdWVcIjpcIjBcIixcIlRhYmxlSURcIjo4LFwiSXNQS0lEXCI6ZmFsc2UsXCJvcmRlcmlkXCI6MTh9LHtcImlkXCI6NjIsXCJjYXB0aW9uXCI6XCLmlbDmja7nm7jlr7nlj5jljJblgLxcIixcIk5hbWVcIjpcIlZhbHVlUmVsYXRpdmVDaGFuZ2VTZXR0aW5nXCIsXCJJc0F1dG9JbmNyZW1lbnRcIjpmYWxzZSxcIkNhbk51bGxcIjp0cnVlLFwiZGJUeXBlXCI6XCJkb3VibGVcIixcImxlbmd0aFwiOlwiXCIsXCJUYWJsZUlEXCI6OCxcIklzUEtJRFwiOmZhbHNlLFwib3JkZXJpZFwiOjE5fSx7XCJpZFwiOjYzLFwiY2FwdGlvblwiOlwi5pWw5o2u5a6a5pe25L+d5a2YXCIsXCJOYW1lXCI6XCJWYWx1ZU9uVGltZUNoYW5nZVwiLFwiSXNBdXRvSW5jcmVtZW50XCI6ZmFsc2UsXCJDYW5OdWxsXCI6dHJ1ZSxcImRiVHlwZVwiOlwiYml0XCIsXCJsZW5ndGhcIjpcIlwiLFwiZGVmYXVsdFZhbHVlXCI6XCIwXCIsXCJUYWJsZUlEXCI6OCxcIklzUEtJRFwiOmZhbHNlLFwib3JkZXJpZFwiOjIwfSx7XCJpZFwiOjY0LFwiY2FwdGlvblwiOlwi5pWw5o2u5a6a5pe25YC8XCIsXCJOYW1lXCI6XCJWYWx1ZU9uVGltZUNoYW5nZVNldHRpbmdcIixcIklzQXV0b0luY3JlbWVudFwiOmZhbHNlLFwiQ2FuTnVsbFwiOnRydWUsXCJkYlR5cGVcIjpcImRvdWJsZVwiLFwibGVuZ3RoXCI6XCJcIixcIlRhYmxlSURcIjo4LFwiSXNQS0lEXCI6ZmFsc2UsXCJvcmRlcmlkXCI6MjF9LHtcImlkXCI6NjUsXCJjYXB0aW9uXCI6XCLngrnnsbvlnotcIixcIk5hbWVcIjpcIlR5cGVcIixcIklzQXV0b0luY3JlbWVudFwiOmZhbHNlLFwiQ2FuTnVsbFwiOnRydWUsXCJkYlR5cGVcIjpcImludFwiLFwiRW51bURlZmluZVwiOlwiQW5hbG9nID0gMSxcXG5EaWdpdGFsID0gMixcXG5Gb2xkZXI9M1wiLFwibGVuZ3RoXCI6XCJcIixcImRlZmF1bHRWYWx1ZVwiOlwiMlwiLFwiVGFibGVJRFwiOjgsXCJJc1BLSURcIjpmYWxzZSxcIm9yZGVyaWRcIjo1fSx7XCJpZFwiOjY2LFwiY2FwdGlvblwiOlwi5qih5ouf6YePLeW3peeoi+WNleS9jVwiLFwiTmFtZVwiOlwiVW5pdFwiLFwiSXNBdXRvSW5jcmVtZW50XCI6ZmFsc2UsXCJDYW5OdWxsXCI6dHJ1ZSxcImRiVHlwZVwiOlwidmFyY2hhclwiLFwibGVuZ3RoXCI6XCI1MFwiLFwiVGFibGVJRFwiOjgsXCJJc1BLSURcIjpmYWxzZSxcIm9yZGVyaWRcIjoyMn0se1wiaWRcIjo2NyxcImNhcHRpb25cIjpcIuaooeaLn+mHjy3lsI/mlbDngrnkvY3mlbBcIixcIk5hbWVcIjpcIkRQQ291bnRcIixcIklzQXV0b0luY3JlbWVudFwiOmZhbHNlLFwiQ2FuTnVsbFwiOnRydWUsXCJkYlR5cGVcIjpcInZhcmNoYXJcIixcImxlbmd0aFwiOlwiNTBcIixcIlRhYmxlSURcIjo4LFwiSXNQS0lEXCI6ZmFsc2UsXCJvcmRlcmlkXCI6MjN9LHtcImlkXCI6NjgsXCJjYXB0aW9uXCI6XCLmqKHmi5/ph48t6YeP56iL6L2s5YyW5byA5YWzXCIsXCJOYW1lXCI6XCJJc1RyYW5zZm9ybVwiLFwiSXNBdXRvSW5jcmVtZW50XCI6ZmFsc2UsXCJDYW5OdWxsXCI6dHJ1ZSxcImRiVHlwZVwiOlwiYml0XCIsXCJsZW5ndGhcIjpcIlwiLFwiZGVmYXVsdFZhbHVlXCI6XCIwXCIsXCJUYWJsZUlEXCI6OCxcIklzUEtJRFwiOmZhbHNlLFwib3JkZXJpZFwiOjI0fSx7XCJpZFwiOjY5LFwiY2FwdGlvblwiOlwi5qih5ouf6YePLemHj+eoi+S4iumZkFwiLFwiTmFtZVwiOlwiVHJhbnNNYXhcIixcIklzQXV0b0luY3JlbWVudFwiOmZhbHNlLFwiQ2FuTnVsbFwiOnRydWUsXCJkYlR5cGVcIjpcImRvdWJsZVwiLFwibGVuZ3RoXCI6XCJcIixcIlRhYmxlSURcIjo4LFwiSXNQS0lEXCI6ZmFsc2UsXCJvcmRlcmlkXCI6MjV9LHtcImlkXCI6NzAsXCJjYXB0aW9uXCI6XCLmqKHmi5/ph48t6YeP56iL5LiL6ZmQXCIsXCJOYW1lXCI6XCJUcmFuc01pblwiLFwiSXNBdXRvSW5jcmVtZW50XCI6ZmFsc2UsXCJDYW5OdWxsXCI6dHJ1ZSxcImRiVHlwZVwiOlwiZG91YmxlXCIsXCJsZW5ndGhcIjpcIlwiLFwiVGFibGVJRFwiOjgsXCJJc1BLSURcIjpmYWxzZSxcIm9yZGVyaWRcIjoyNn0se1wiaWRcIjo3MSxcImNhcHRpb25cIjpcIuaooeaLn+mHjy3kvKDmhJ/lmajph4/nqIvkuIrpmZBcIixcIk5hbWVcIjpcIlNlbnNvck1heFwiLFwiSXNBdXRvSW5jcmVtZW50XCI6ZmFsc2UsXCJDYW5OdWxsXCI6dHJ1ZSxcImRiVHlwZVwiOlwiZG91YmxlXCIsXCJsZW5ndGhcIjpcIlwiLFwiVGFibGVJRFwiOjgsXCJJc1BLSURcIjpmYWxzZSxcIm9yZGVyaWRcIjoyN30se1wiaWRcIjo3MixcImNhcHRpb25cIjpcIuaooeaLn+mHjy3kvKDmhJ/lmajph4/nqIvkuIvpmZBcIixcIk5hbWVcIjpcIlNlbnNvck1pblwiLFwiSXNBdXRvSW5jcmVtZW50XCI6ZmFsc2UsXCJDYW5OdWxsXCI6dHJ1ZSxcImRiVHlwZVwiOlwidmFyY2hhclwiLFwibGVuZ3RoXCI6XCI1MFwiLFwiVGFibGVJRFwiOjgsXCJJc1BLSURcIjpmYWxzZSxcIm9yZGVyaWRcIjoyOH0se1wiaWRcIjo3MyxcImNhcHRpb25cIjpcIuaooeaLn+mHjy3lvIDlubPmlrlcIixcIk5hbWVcIjpcIklzU3F1YXJlXCIsXCJJc0F1dG9JbmNyZW1lbnRcIjpmYWxzZSxcIkNhbk51bGxcIjp0cnVlLFwiZGJUeXBlXCI6XCJiaXRcIixcImxlbmd0aFwiOlwiXCIsXCJkZWZhdWx0VmFsdWVcIjpcIjBcIixcIlRhYmxlSURcIjo4LFwiSXNQS0lEXCI6ZmFsc2UsXCJvcmRlcmlkXCI6Mjl9LHtcImlkXCI6NzQsXCJjYXB0aW9uXCI6XCLliIbmrrXnur/mgKfljJZcIixcIk5hbWVcIjpcIklzTGluZWFyXCIsXCJJc0F1dG9JbmNyZW1lbnRcIjpmYWxzZSxcIkNhbk51bGxcIjp0cnVlLFwiZGJUeXBlXCI6XCJiaXRcIixcImxlbmd0aFwiOlwiXCIsXCJkZWZhdWx0VmFsdWVcIjpcIjBcIixcIlRhYmxlSURcIjo4LFwiSXNQS0lEXCI6ZmFsc2UsXCJvcmRlcmlkXCI6MzB9LHtcImlkXCI6NzUsXCJjYXB0aW9uXCI6XCLliIbmrrXnur/mgKfljJZYMVwiLFwiTmFtZVwiOlwiTGluZWFyWDFcIixcIklzQXV0b0luY3JlbWVudFwiOmZhbHNlLFwiQ2FuTnVsbFwiOnRydWUsXCJkYlR5cGVcIjpcImRvdWJsZVwiLFwibGVuZ3RoXCI6XCJcIixcIlRhYmxlSURcIjo4LFwiSXNQS0lEXCI6ZmFsc2UsXCJvcmRlcmlkXCI6MzF9LHtcImlkXCI6NzYsXCJjYXB0aW9uXCI6XCLliIbmrrXnur/mgKfljJZZMVwiLFwiTmFtZVwiOlwiTGluZWFyWTFcIixcIklzQXV0b0luY3JlbWVudFwiOmZhbHNlLFwiQ2FuTnVsbFwiOnRydWUsXCJkYlR5cGVcIjpcImRvdWJsZVwiLFwibGVuZ3RoXCI6XCJcIixcIlRhYmxlSURcIjo4LFwiSXNQS0lEXCI6ZmFsc2UsXCJvcmRlcmlkXCI6MzJ9LHtcImlkXCI6NzcsXCJjYXB0aW9uXCI6XCLliIbmrrXnur/mgKfljJZYMlwiLFwiTmFtZVwiOlwiTGluZWFyWDJcIixcIklzQXV0b0luY3JlbWVudFwiOmZhbHNlLFwiQ2FuTnVsbFwiOnRydWUsXCJkYlR5cGVcIjpcImRvdWJsZVwiLFwibGVuZ3RoXCI6XCJcIixcIlRhYmxlSURcIjo4LFwiSXNQS0lEXCI6ZmFsc2UsXCJvcmRlcmlkXCI6MzN9LHtcImlkXCI6NzgsXCJjYXB0aW9uXCI6XCLliIbmrrXnur/mgKfljJZZMlwiLFwiTmFtZVwiOlwiTGluZWFyWTJcIixcIklzQXV0b0luY3JlbWVudFwiOmZhbHNlLFwiQ2FuTnVsbFwiOnRydWUsXCJkYlR5cGVcIjpcImRvdWJsZVwiLFwibGVuZ3RoXCI6XCJcIixcIlRhYmxlSURcIjo4LFwiSXNQS0lEXCI6ZmFsc2UsXCJvcmRlcmlkXCI6MzR9LHtcImlkXCI6NzksXCJOYW1lXCI6XCJMaW5lYXJYM1wiLFwiSXNBdXRvSW5jcmVtZW50XCI6ZmFsc2UsXCJDYW5OdWxsXCI6dHJ1ZSxcImRiVHlwZVwiOlwiZG91YmxlXCIsXCJsZW5ndGhcIjpcIlwiLFwiVGFibGVJRFwiOjgsXCJJc1BLSURcIjpmYWxzZSxcIm9yZGVyaWRcIjozNX0se1wiaWRcIjo4MCxcIk5hbWVcIjpcIkxpbmVhclkzXCIsXCJJc0F1dG9JbmNyZW1lbnRcIjpmYWxzZSxcIkNhbk51bGxcIjp0cnVlLFwiZGJUeXBlXCI6XCJkb3VibGVcIixcImxlbmd0aFwiOlwiXCIsXCJUYWJsZUlEXCI6OCxcIklzUEtJRFwiOmZhbHNlLFwib3JkZXJpZFwiOjM2fSx7XCJpZFwiOjgxLFwiTmFtZVwiOlwiTGluZWFyWDRcIixcIklzQXV0b0luY3JlbWVudFwiOmZhbHNlLFwiQ2FuTnVsbFwiOnRydWUsXCJkYlR5cGVcIjpcImRvdWJsZVwiLFwibGVuZ3RoXCI6XCJcIixcIlRhYmxlSURcIjo4LFwiSXNQS0lEXCI6ZmFsc2UsXCJvcmRlcmlkXCI6Mzd9LHtcImlkXCI6ODIsXCJjYXB0aW9uXCI6XCLmiqXorablgLwyXCIsXCJOYW1lXCI6XCJBbGFybVZhbHVlMlwiLFwiSXNBdXRvSW5jcmVtZW50XCI6ZmFsc2UsXCJDYW5OdWxsXCI6dHJ1ZSxcImRiVHlwZVwiOlwiZG91YmxlXCIsXCJsZW5ndGhcIjpcIlwiLFwiVGFibGVJRFwiOjgsXCJJc1BLSURcIjpmYWxzZSxcIm9yZGVyaWRcIjozOH0se1wiaWRcIjo4MyxcImNhcHRpb25cIjpcIuaKpeitpuS8mOWFiOe6pzJcIixcIk5hbWVcIjpcIkFsYXJtUHJpb3JpdHkyXCIsXCJJc0F1dG9JbmNyZW1lbnRcIjpmYWxzZSxcIkNhbk51bGxcIjp0cnVlLFwiZGJUeXBlXCI6XCJpbnRcIixcImxlbmd0aFwiOlwiXCIsXCJUYWJsZUlEXCI6OCxcIklzUEtJRFwiOmZhbHNlLFwib3JkZXJpZFwiOjM5fSx7XCJpZFwiOjg0LFwiTmFtZVwiOlwiQWxhcm1WYWx1ZTNcIixcIklzQXV0b0luY3JlbWVudFwiOmZhbHNlLFwiQ2FuTnVsbFwiOnRydWUsXCJkYlR5cGVcIjpcImRvdWJsZVwiLFwibGVuZ3RoXCI6XCJcIixcIlRhYmxlSURcIjo4LFwiSXNQS0lEXCI6ZmFsc2UsXCJvcmRlcmlkXCI6NDB9LHtcImlkXCI6ODUsXCJOYW1lXCI6XCJBbGFybVByaW9yaXR5M1wiLFwiSXNBdXRvSW5jcmVtZW50XCI6ZmFsc2UsXCJDYW5OdWxsXCI6dHJ1ZSxcImRiVHlwZVwiOlwiaW50XCIsXCJsZW5ndGhcIjpcIlwiLFwiVGFibGVJRFwiOjgsXCJJc1BLSURcIjpmYWxzZSxcIm9yZGVyaWRcIjo0MX0se1wiaWRcIjo4NixcIk5hbWVcIjpcIkFsYXJtVmFsdWU0XCIsXCJJc0F1dG9JbmNyZW1lbnRcIjpmYWxzZSxcIkNhbk51bGxcIjp0cnVlLFwiZGJUeXBlXCI6XCJkb3VibGVcIixcImxlbmd0aFwiOlwiXCIsXCJUYWJsZUlEXCI6OCxcIklzUEtJRFwiOmZhbHNlLFwib3JkZXJpZFwiOjQyfSx7XCJpZFwiOjg3LFwiTmFtZVwiOlwiQWxhcm1Qcmlvcml0eTRcIixcIklzQXV0b0luY3JlbWVudFwiOmZhbHNlLFwiQ2FuTnVsbFwiOnRydWUsXCJkYlR5cGVcIjpcImludFwiLFwibGVuZ3RoXCI6XCJcIixcIlRhYmxlSURcIjo4LFwiSXNQS0lEXCI6ZmFsc2UsXCJvcmRlcmlkXCI6NDN9LHtcImlkXCI6ODgsXCJOYW1lXCI6XCJBbGFybVZhbHVlNVwiLFwiSXNBdXRvSW5jcmVtZW50XCI6ZmFsc2UsXCJDYW5OdWxsXCI6dHJ1ZSxcImRiVHlwZVwiOlwiZG91YmxlXCIsXCJsZW5ndGhcIjpcIlwiLFwiVGFibGVJRFwiOjgsXCJJc1BLSURcIjpmYWxzZSxcIm9yZGVyaWRcIjo0NH0se1wiaWRcIjo4OSxcIk5hbWVcIjpcIkFsYXJtUHJpb3JpdHk1XCIsXCJJc0F1dG9JbmNyZW1lbnRcIjpmYWxzZSxcIkNhbk51bGxcIjp0cnVlLFwiZGJUeXBlXCI6XCJpbnRcIixcImxlbmd0aFwiOlwiXCIsXCJUYWJsZUlEXCI6OCxcIklzUEtJRFwiOmZhbHNlLFwib3JkZXJpZFwiOjQ1fSx7XCJpZFwiOjkwLFwiY2FwdGlvblwiOlwi5YGP5beu5oql6K2mXCIsXCJOYW1lXCI6XCJJc0FsYXJtT2Zmc2V0XCIsXCJJc0F1dG9JbmNyZW1lbnRcIjpmYWxzZSxcIkNhbk51bGxcIjp0cnVlLFwiZGJUeXBlXCI6XCJiaXRcIixcImxlbmd0aFwiOlwiXCIsXCJkZWZhdWx0VmFsdWVcIjpcIjBcIixcIlRhYmxlSURcIjo4LFwiSXNQS0lEXCI6ZmFsc2UsXCJvcmRlcmlkXCI6NDZ9LHtcImlkXCI6OTEsXCJjYXB0aW9uXCI6XCLlgY/lt67miqXoraborr7lrprlgLxcIixcIk5hbWVcIjpcIkFsYXJtT2Zmc2V0T3JpZ2luYWxWYWx1ZVwiLFwiSXNBdXRvSW5jcmVtZW50XCI6ZmFsc2UsXCJDYW5OdWxsXCI6dHJ1ZSxcImRiVHlwZVwiOlwiZG91YmxlXCIsXCJsZW5ndGhcIjpcIlwiLFwiVGFibGVJRFwiOjgsXCJJc1BLSURcIjpmYWxzZSxcIm9yZGVyaWRcIjo0N30se1wiaWRcIjo5MixcImNhcHRpb25cIjpcIuWBj+W3ruWAvFwiLFwiTmFtZVwiOlwiQWxhcm1PZmZzZXRWYWx1ZVwiLFwiSXNBdXRvSW5jcmVtZW50XCI6ZmFsc2UsXCJDYW5OdWxsXCI6dHJ1ZSxcImRiVHlwZVwiOlwiZG91YmxlXCIsXCJsZW5ndGhcIjpcIlwiLFwiVGFibGVJRFwiOjgsXCJJc1BLSURcIjpmYWxzZSxcIm9yZGVyaWRcIjo0OH0se1wiaWRcIjo5MyxcImNhcHRpb25cIjpcIuWBj+W3ruaKpeitpuS8mOWFiOe6p1wiLFwiTmFtZVwiOlwiQWxhcm1PZmZzZXRQcmlvcml0eVwiLFwiSXNBdXRvSW5jcmVtZW50XCI6ZmFsc2UsXCJDYW5OdWxsXCI6dHJ1ZSxcImRiVHlwZVwiOlwiaW50XCIsXCJsZW5ndGhcIjpcIlwiLFwiVGFibGVJRFwiOjgsXCJJc1BLSURcIjpmYWxzZSxcIm9yZGVyaWRcIjo0OX0se1wiaWRcIjo5NCxcImNhcHRpb25cIjpcIuWPmOWMlueOh+aKpeitplwiLFwiTmFtZVwiOlwiSXNBbGFybVBlcmNlbnRcIixcIklzQXV0b0luY3JlbWVudFwiOmZhbHNlLFwiQ2FuTnVsbFwiOnRydWUsXCJkYlR5cGVcIjpcImJpdFwiLFwibGVuZ3RoXCI6XCJcIixcImRlZmF1bHRWYWx1ZVwiOlwiMFwiLFwiVGFibGVJRFwiOjgsXCJJc1BLSURcIjpmYWxzZSxcIm9yZGVyaWRcIjo1MH0se1wiaWRcIjo5NSxcImNhcHRpb25cIjpcIuWPmOWMlueOh+WAvO+8iOeZvuWIhuavlO+8iVwiLFwiTmFtZVwiOlwiUGVyY2VudFwiLFwiSXNBdXRvSW5jcmVtZW50XCI6ZmFsc2UsXCJDYW5OdWxsXCI6dHJ1ZSxcImRiVHlwZVwiOlwiZG91YmxlXCIsXCJsZW5ndGhcIjpcIlwiLFwiVGFibGVJRFwiOjgsXCJJc1BLSURcIjpmYWxzZSxcIm9yZGVyaWRcIjo1MX0se1wiaWRcIjo5NixcImNhcHRpb25cIjpcIuWPmOWMlueOh+WRqOacn++8iOenku+8iVwiLFwiTmFtZVwiOlwiQ2hhbmdlQ3ljbGVcIixcIklzQXV0b0luY3JlbWVudFwiOmZhbHNlLFwiQ2FuTnVsbFwiOnRydWUsXCJkYlR5cGVcIjpcImludFwiLFwibGVuZ3RoXCI6XCJcIixcIlRhYmxlSURcIjo4LFwiSXNQS0lEXCI6ZmFsc2UsXCJvcmRlcmlkXCI6NTJ9LHtcImlkXCI6OTcsXCJjYXB0aW9uXCI6XCLlj5jljJbnjofmiqXorabkvJjlhYjnuqdcIixcIk5hbWVcIjpcIkFsYXJtUGVyY2VudFByaW9yaXR5XCIsXCJJc0F1dG9JbmNyZW1lbnRcIjpmYWxzZSxcIkNhbk51bGxcIjp0cnVlLFwiZGJUeXBlXCI6XCJpbnRcIixcImxlbmd0aFwiOlwiXCIsXCJUYWJsZUlEXCI6OCxcIklzUEtJRFwiOmZhbHNlLFwib3JkZXJpZFwiOjUzfV0sXCJuZXdDb2x1bW5zXCI6W10sXCJjaGFuZ2VkQ29sdW1uc1wiOlt7XCJpZFwiOjEwMyxcIk5hbWVcIjpcIlBhcmVudElkXCIsXCJJc0F1dG9JbmNyZW1lbnRcIjpmYWxzZSxcIkNhbk51bGxcIjp0cnVlLFwiZGJUeXBlXCI6XCJpbnRcIixcImxlbmd0aFwiOlwiXCIsXCJkZWZhdWx0VmFsdWVcIjpcIjBcIixcIlRhYmxlSURcIjo4LFwiSXNQS0lEXCI6ZmFsc2UsXCJvcmRlcmlkXCI6NixcIkJhY2t1cENoYW5nZWRQcm9wZXJ0aWVzXCI6e1wiZGVmYXVsdFZhbHVlXCI6e1wiT3JpZ2luYWxWYWx1ZVwiOm51bGx9fX1dLFwiZGVsZXRlZENvbHVtbnNcIjpbXSxcIklEWENvbmZpZ3NcIjpbXSxcIklEXCI6MH0ifSx7Ik5hbWUiOiJkYXRhYmFzZWlkIiwiVmFsdWUiOjJ9XSwiUm93U3RhdGUiOjB9LHsiSXRlbXMiOlt7Ik5hbWUiOiJpZCIsIlZhbHVlIjo0Mn0seyJOYW1lIjoidHlwZSIsIlZhbHVlIjoiQ2hhbmdlVGFibGVBY3Rpb24ifSx7Ik5hbWUiOiJjb250ZW50IiwiVmFsdWUiOiJ7XCJPbGRUYWJsZU5hbWVcIjpcIkRldmljZVBvaW50XCIsXCJOZXdUYWJsZU5hbWVcIjpcIkRldmljZVBvaW50XCIsXCJvdGhlckNvbHVtbnNcIjpbe1wiaWRcIjo0MixcIk5hbWVcIjpcImlkXCIsXCJJc0F1dG9JbmNyZW1lbnRcIjp0cnVlLFwiQ2FuTnVsbFwiOmZhbHNlLFwiZGJUeXBlXCI6XCJpbnRcIixcIlRhYmxlSURcIjo4LFwiSXNQS0lEXCI6dHJ1ZSxcIm9yZGVyaWRcIjowfSx7XCJpZFwiOjQzLFwiY2FwdGlvblwiOlwi54K55ZCNXCIsXCJOYW1lXCI6XCJOYW1lXCIsXCJJc0F1dG9JbmNyZW1lbnRcIjpmYWxzZSxcIkNhbk51bGxcIjp0cnVlLFwiZGJUeXBlXCI6XCJpbnRcIixcImxlbmd0aFwiOlwiXCIsXCJUYWJsZUlEXCI6OCxcIklzUEtJRFwiOmZhbHNlLFwib3JkZXJpZFwiOjF9LHtcImlkXCI6NDQsXCJOYW1lXCI6XCJBZGRyZXNzXCIsXCJJc0F1dG9JbmNyZW1lbnRcIjpmYWxzZSxcIkNhbk51bGxcIjp0cnVlLFwiZGJUeXBlXCI6XCJ2YXJjaGFyXCIsXCJsZW5ndGhcIjpcIjIwMFwiLFwiVGFibGVJRFwiOjgsXCJJc1BLSURcIjpmYWxzZSxcIm9yZGVyaWRcIjozfSx7XCJpZFwiOjQ1LFwiY2FwdGlvblwiOlwi5piv5ZCm55uR5rWL5Y+Y5YyWXCIsXCJOYW1lXCI6XCJJc1dhdGNoaW5nXCIsXCJJc0F1dG9JbmNyZW1lbnRcIjpmYWxzZSxcIkNhbk51bGxcIjp0cnVlLFwiZGJUeXBlXCI6XCJiaXRcIixcImxlbmd0aFwiOlwiXCIsXCJkZWZhdWx0VmFsdWVcIjpcIjBcIixcIlRhYmxlSURcIjo4LFwiSXNQS0lEXCI6ZmFsc2UsXCJvcmRlcmlkXCI6NTR9LHtcImlkXCI6NDgsXCJjYXB0aW9uXCI6XCLlnLDlnYDorr7nva7ml7bnmoRqc29u5a+56LGh6KGo6L6+XCIsXCJOYW1lXCI6XCJBZGRyU2V0dGluZ1wiLFwiSXNBdXRvSW5jcmVtZW50XCI6ZmFsc2UsXCJDYW5OdWxsXCI6dHJ1ZSxcImRiVHlwZVwiOlwidmFyY2hhclwiLFwibGVuZ3RoXCI6XCIyMDBcIixcIlRhYmxlSURcIjo4LFwiSXNQS0lEXCI6ZmFsc2UsXCJvcmRlcmlkXCI6NH0se1wiaWRcIjo0OSxcImNhcHRpb25cIjpcIuaPj+i/sFwiLFwiTmFtZVwiOlwiRGVzY1wiLFwiSXNBdXRvSW5jcmVtZW50XCI6ZmFsc2UsXCJDYW5OdWxsXCI6dHJ1ZSxcImRiVHlwZVwiOlwidmFyY2hhclwiLFwibGVuZ3RoXCI6XCIxMDBcIixcIlRhYmxlSURcIjo4LFwiSXNQS0lEXCI6ZmFsc2UsXCJvcmRlcmlkXCI6Mn0se1wiaWRcIjo1MCxcImNhcHRpb25cIjpcIuWIneWni+WAvFwiLFwiTmFtZVwiOlwiSW5pdFZhbHVlXCIsXCJJc0F1dG9JbmNyZW1lbnRcIjpmYWxzZSxcIkNhbk51bGxcIjp0cnVlLFwiZGJUeXBlXCI6XCJkb3VibGVcIixcImxlbmd0aFwiOlwiXCIsXCJUYWJsZUlEXCI6OCxcIklzUEtJRFwiOmZhbHNlLFwib3JkZXJpZFwiOjd9LHtcImlkXCI6NTEsXCJjYXB0aW9uXCI6XCLlvIDnirbmgIHkv6Hmga9cIixcIk5hbWVcIjpcIlN0YXR1c09wZW5JbmZvXCIsXCJJc0F1dG9JbmNyZW1lbnRcIjpmYWxzZSxcIkNhbk51bGxcIjp0cnVlLFwiZGJUeXBlXCI6XCJ2YXJjaGFyXCIsXCJsZW5ndGhcIjpcIjUwXCIsXCJUYWJsZUlEXCI6OCxcIklzUEtJRFwiOmZhbHNlLFwib3JkZXJpZFwiOjh9LHtcImlkXCI6NTIsXCJjYXB0aW9uXCI6XCLlhbPnirbmgIHkv6Hmga9cIixcIk5hbWVcIjpcIlN0YXRlQ2xvc2VJbmZvXCIsXCJJc0F1dG9JbmNyZW1lbnRcIjpmYWxzZSxcIkNhbk51bGxcIjp0cnVlLFwiZGJUeXBlXCI6XCJ2YXJjaGFyXCIsXCJsZW5ndGhcIjpcIjUwXCIsXCJUYWJsZUlEXCI6OCxcIklzUEtJRFwiOmZhbHNlLFwib3JkZXJpZFwiOjl9LHtcImlkXCI6NTMsXCJjYXB0aW9uXCI6XCLmiqXorablvIDlhbNcIixcIk5hbWVcIjpcIklzQWxhcm1cIixcIklzQXV0b0luY3JlbWVudFwiOmZhbHNlLFwiQ2FuTnVsbFwiOnRydWUsXCJkYlR5cGVcIjpcImJpdFwiLFwibGVuZ3RoXCI6XCJcIixcImRlZmF1bHRWYWx1ZVwiOlwiMFwiLFwiVGFibGVJRFwiOjgsXCJJc1BLSURcIjpmYWxzZSxcIm9yZGVyaWRcIjoxMH0se1wiaWRcIjo1NCxcImNhcHRpb25cIjpcIuaKpeitpuWAvFwiLFwiTmFtZVwiOlwiQWxhcm1WYWx1ZVwiLFwiSXNBdXRvSW5jcmVtZW50XCI6ZmFsc2UsXCJDYW5OdWxsXCI6dHJ1ZSxcImRiVHlwZVwiOlwiZG91YmxlXCIsXCJsZW5ndGhcIjpcIlwiLFwiVGFibGVJRFwiOjgsXCJJc1BLSURcIjpmYWxzZSxcIm9yZGVyaWRcIjoxMX0se1wiaWRcIjo1NSxcImNhcHRpb25cIjpcIuaKpeitpuS8mOWFiOe6p1wiLFwiTmFtZVwiOlwiQWxhcm1Qcmlvcml0eVwiLFwiSXNBdXRvSW5jcmVtZW50XCI6ZmFsc2UsXCJDYW5OdWxsXCI6dHJ1ZSxcImRiVHlwZVwiOlwiaW50XCIsXCJsZW5ndGhcIjpcIlwiLFwiVGFibGVJRFwiOjgsXCJJc1BLSURcIjpmYWxzZSxcIm9yZGVyaWRcIjoxMn0se1wiaWRcIjo1NixcImNhcHRpb25cIjpcIuaKpeitpue7hFwiLFwiTmFtZVwiOlwiQWxhcm1Hcm91cFwiLFwiSXNBdXRvSW5jcmVtZW50XCI6ZmFsc2UsXCJDYW5OdWxsXCI6dHJ1ZSxcImRiVHlwZVwiOlwidmFyY2hhclwiLFwibGVuZ3RoXCI6XCI1MFwiLFwiVGFibGVJRFwiOjgsXCJJc1BLSURcIjpmYWxzZSxcIm9yZGVyaWRcIjoxM30se1wiaWRcIjo1NyxcImNhcHRpb25cIjpcIuaKpeitpuiHquWKqOehruiupFwiLFwiTmFtZVwiOlwiQWxhcm1BdXRvQ29uZmlybVwiLFwiSXNBdXRvSW5jcmVtZW50XCI6ZmFsc2UsXCJDYW5OdWxsXCI6dHJ1ZSxcImRiVHlwZVwiOlwiYml0XCIsXCJsZW5ndGhcIjpcIlwiLFwiZGVmYXVsdFZhbHVlXCI6XCIwXCIsXCJUYWJsZUlEXCI6OCxcIklzUEtJRFwiOmZhbHNlLFwib3JkZXJpZFwiOjE0fSx7XCJpZFwiOjU4LFwiY2FwdGlvblwiOlwi5oql6K2m6Ieq5Yqo5aSN5L2NXCIsXCJOYW1lXCI6XCJBbGFybUF1dG9SZXNldFwiLFwiSXNBdXRvSW5jcmVtZW50XCI6ZmFsc2UsXCJDYW5OdWxsXCI6dHJ1ZSxcImRiVHlwZVwiOlwiYml0XCIsXCJsZW5ndGhcIjpcIlwiLFwiZGVmYXVsdFZhbHVlXCI6XCIwXCIsXCJUYWJsZUlEXCI6OCxcIklzUEtJRFwiOmZhbHNlLFwib3JkZXJpZFwiOjE1fSx7XCJpZFwiOjU5LFwiY2FwdGlvblwiOlwi5pWw5o2u57ud5a+55Y+Y5YyW5L+d5a2YXCIsXCJOYW1lXCI6XCJWYWx1ZUFic29sdXRlQ2hhbmdlXCIsXCJJc0F1dG9JbmNyZW1lbnRcIjpmYWxzZSxcIkNhbk51bGxcIjp0cnVlLFwiZGJUeXBlXCI6XCJiaXRcIixcImxlbmd0aFwiOlwiXCIsXCJkZWZhdWx0VmFsdWVcIjpcIjBcIixcIlRhYmxlSURcIjo4LFwiSXNQS0lEXCI6ZmFsc2UsXCJvcmRlcmlkXCI6MTZ9LHtcImlkXCI6NjAsXCJjYXB0aW9uXCI6XCLmlbDmja7nu53lr7nlj5jljJblgLxcIixcIk5hbWVcIjpcIlZhbHVlQWJzb2x1dGVDaGFuZ2VTZXR0aW5nXCIsXCJJc0F1dG9JbmNyZW1lbnRcIjpmYWxzZSxcIkNhbk51bGxcIjp0cnVlLFwiZGJUeXBlXCI6XCJkb3VibGVcIixcImxlbmd0aFwiOlwiXCIsXCJUYWJsZUlEXCI6OCxcIklzUEtJRFwiOmZhbHNlLFwib3JkZXJpZFwiOjE3fSx7XCJpZFwiOjYxLFwiY2FwdGlvblwiOlwi5pWw5o2u55u45a+55Y+Y5YyW5L+d5a2YXCIsXCJOYW1lXCI6XCJWYWx1ZVJlbGF0aXZlQ2hhbmdlXCIsXCJJc0F1dG9JbmNyZW1lbnRcIjpmYWxzZSxcIkNhbk51bGxcIjp0cnVlLFwiZGJUeXBlXCI6XCJiaXRcIixcImxlbmd0aFwiOlwiXCIsXCJkZWZhdWx0VmFsdWVcIjpcIjBcIixcIlRhYmxlSURcIjo4LFwiSXNQS0lEXCI6ZmFsc2UsXCJvcmRlcmlkXCI6MTh9LHtcImlkXCI6NjIsXCJjYXB0aW9uXCI6XCLmlbDmja7nm7jlr7nlj5jljJblgLxcIixcIk5hbWVcIjpcIlZhbHVlUmVsYXRpdmVDaGFuZ2VTZXR0aW5nXCIsXCJJc0F1dG9JbmNyZW1lbnRcIjpmYWxzZSxcIkNhbk51bGxcIjp0cnVlLFwiZGJUeXBlXCI6XCJkb3VibGVcIixcImxlbmd0aFwiOlwiXCIsXCJUYWJsZUlEXCI6OCxcIklzUEtJRFwiOmZhbHNlLFwib3JkZXJpZFwiOjE5fSx7XCJpZFwiOjYzLFwiY2FwdGlvblwiOlwi5pWw5o2u5a6a5pe25L+d5a2YXCIsXCJOYW1lXCI6XCJWYWx1ZU9uVGltZUNoYW5nZVwiLFwiSXNBdXRvSW5jcmVtZW50XCI6ZmFsc2UsXCJDYW5OdWxsXCI6dHJ1ZSxcImRiVHlwZVwiOlwiYml0XCIsXCJsZW5ndGhcIjpcIlwiLFwiZGVmYXVsdFZhbHVlXCI6XCIwXCIsXCJUYWJsZUlEXCI6OCxcIklzUEtJRFwiOmZhbHNlLFwib3JkZXJpZFwiOjIwfSx7XCJpZFwiOjY0LFwiY2FwdGlvblwiOlwi5pWw5o2u5a6a5pe25YC8XCIsXCJOYW1lXCI6XCJWYWx1ZU9uVGltZUNoYW5nZVNldHRpbmdcIixcIklzQXV0b0luY3JlbWVudFwiOmZhbHNlLFwiQ2FuTnVsbFwiOnRydWUsXCJkYlR5cGVcIjpcImRvdWJsZVwiLFwibGVuZ3RoXCI6XCJcIixcIlRhYmxlSURcIjo4LFwiSXNQS0lEXCI6ZmFsc2UsXCJvcmRlcmlkXCI6MjF9LHtcImlkXCI6NjUsXCJjYXB0aW9uXCI6XCLngrnnsbvlnotcIixcIk5hbWVcIjpcIlR5cGVcIixcIklzQXV0b0luY3JlbWVudFwiOmZhbHNlLFwiQ2FuTnVsbFwiOnRydWUsXCJkYlR5cGVcIjpcImludFwiLFwiRW51bURlZmluZVwiOlwiQW5hbG9nID0gMSxcXG5EaWdpdGFsID0gMixcXG5Gb2xkZXI9M1wiLFwibGVuZ3RoXCI6XCJcIixcImRlZmF1bHRWYWx1ZVwiOlwiMlwiLFwiVGFibGVJRFwiOjgsXCJJc1BLSURcIjpmYWxzZSxcIm9yZGVyaWRcIjo1fSx7XCJpZFwiOjY2LFwiY2FwdGlvblwiOlwi5qih5ouf6YePLeW3peeoi+WNleS9jVwiLFwiTmFtZVwiOlwiVW5pdFwiLFwiSXNBdXRvSW5jcmVtZW50XCI6ZmFsc2UsXCJDYW5OdWxsXCI6dHJ1ZSxcImRiVHlwZVwiOlwidmFyY2hhclwiLFwibGVuZ3RoXCI6XCI1MFwiLFwiVGFibGVJRFwiOjgsXCJJc1BLSURcIjpmYWxzZSxcIm9yZGVyaWRcIjoyMn0se1wiaWRcIjo2NyxcImNhcHRpb25cIjpcIuaooeaLn+mHjy3lsI/mlbDngrnkvY3mlbBcIixcIk5hbWVcIjpcIkRQQ291bnRcIixcIklzQXV0b0luY3JlbWVudFwiOmZhbHNlLFwiQ2FuTnVsbFwiOnRydWUsXCJkYlR5cGVcIjpcInZhcmNoYXJcIixcImxlbmd0aFwiOlwiNTBcIixcIlRhYmxlSURcIjo4LFwiSXNQS0lEXCI6ZmFsc2UsXCJvcmRlcmlkXCI6MjN9LHtcImlkXCI6NjgsXCJjYXB0aW9uXCI6XCLmqKHmi5/ph48t6YeP56iL6L2s5YyW5byA5YWzXCIsXCJOYW1lXCI6XCJJc1RyYW5zZm9ybVwiLFwiSXNBdXRvSW5jcmVtZW50XCI6ZmFsc2UsXCJDYW5OdWxsXCI6dHJ1ZSxcImRiVHlwZVwiOlwiYml0XCIsXCJsZW5ndGhcIjpcIlwiLFwiZGVmYXVsdFZhbHVlXCI6XCIwXCIsXCJUYWJsZUlEXCI6OCxcIklzUEtJRFwiOmZhbHNlLFwib3JkZXJpZFwiOjI0fSx7XCJpZFwiOjY5LFwiY2FwdGlvblwiOlwi5qih5ouf6YePLemHj+eoi+S4iumZkFwiLFwiTmFtZVwiOlwiVHJhbnNNYXhcIixcIklzQXV0b0luY3JlbWVudFwiOmZhbHNlLFwiQ2FuTnVsbFwiOnRydWUsXCJkYlR5cGVcIjpcImRvdWJsZVwiLFwibGVuZ3RoXCI6XCJcIixcIlRhYmxlSURcIjo4LFwiSXNQS0lEXCI6ZmFsc2UsXCJvcmRlcmlkXCI6MjV9LHtcImlkXCI6NzAsXCJjYXB0aW9uXCI6XCLmqKHmi5/ph48t6YeP56iL5LiL6ZmQXCIsXCJOYW1lXCI6XCJUcmFuc01pblwiLFwiSXNBdXRvSW5jcmVtZW50XCI6ZmFsc2UsXCJDYW5OdWxsXCI6dHJ1ZSxcImRiVHlwZVwiOlwiZG91YmxlXCIsXCJsZW5ndGhcIjpcIlwiLFwiVGFibGVJRFwiOjgsXCJJc1BLSURcIjpmYWxzZSxcIm9yZGVyaWRcIjoyNn0se1wiaWRcIjo3MSxcImNhcHRpb25cIjpcIuaooeaLn+mHjy3kvKDmhJ/lmajph4/nqIvkuIrpmZBcIixcIk5hbWVcIjpcIlNlbnNvck1heFwiLFwiSXNBdXRvSW5jcmVtZW50XCI6ZmFsc2UsXCJDYW5OdWxsXCI6dHJ1ZSxcImRiVHlwZVwiOlwiZG91YmxlXCIsXCJsZW5ndGhcIjpcIlwiLFwiVGFibGVJRFwiOjgsXCJJc1BLSURcIjpmYWxzZSxcIm9yZGVyaWRcIjoyN30se1wiaWRcIjo3MixcImNhcHRpb25cIjpcIuaooeaLn+mHjy3kvKDmhJ/lmajph4/nqIvkuIvpmZBcIixcIk5hbWVcIjpcIlNlbnNvck1pblwiLFwiSXNBdXRvSW5jcmVtZW50XCI6ZmFsc2UsXCJDYW5OdWxsXCI6dHJ1ZSxcImRiVHlwZVwiOlwidmFyY2hhclwiLFwibGVuZ3RoXCI6XCI1MFwiLFwiVGFibGVJRFwiOjgsXCJJc1BLSURcIjpmYWxzZSxcIm9yZGVyaWRcIjoyOH0se1wiaWRcIjo3MyxcImNhcHRpb25cIjpcIuaooeaLn+mHjy3lvIDlubPmlrlcIixcIk5hbWVcIjpcIklzU3F1YXJlXCIsXCJJc0F1dG9JbmNyZW1lbnRcIjpmYWxzZSxcIkNhbk51bGxcIjp0cnVlLFwiZGJUeXBlXCI6XCJiaXRcIixcImxlbmd0aFwiOlwiXCIsXCJkZWZhdWx0VmFsdWVcIjpcIjBcIixcIlRhYmxlSURcIjo4LFwiSXNQS0lEXCI6ZmFsc2UsXCJvcmRlcmlkXCI6Mjl9LHtcImlkXCI6NzQsXCJjYXB0aW9uXCI6XCLliIbmrrXnur/mgKfljJZcIixcIk5hbWVcIjpcIklzTGluZWFyXCIsXCJJc0F1dG9JbmNyZW1lbnRcIjpmYWxzZSxcIkNhbk51bGxcIjp0cnVlLFwiZGJUeXBlXCI6XCJiaXRcIixcImxlbmd0aFwiOlwiXCIsXCJkZWZhdWx0VmFsdWVcIjpcIjBcIixcIlRhYmxlSURcIjo4LFwiSXNQS0lEXCI6ZmFsc2UsXCJvcmRlcmlkXCI6MzB9LHtcImlkXCI6NzUsXCJjYXB0aW9uXCI6XCLliIbmrrXnur/mgKfljJZYMVwiLFwiTmFtZVwiOlwiTGluZWFyWDFcIixcIklzQXV0b0luY3JlbWVudFwiOmZhbHNlLFwiQ2FuTnVsbFwiOnRydWUsXCJkYlR5cGVcIjpcImRvdWJsZVwiLFwibGVuZ3RoXCI6XCJcIixcIlRhYmxlSURcIjo4LFwiSXNQS0lEXCI6ZmFsc2UsXCJvcmRlcmlkXCI6MzF9LHtcImlkXCI6NzYsXCJjYXB0aW9uXCI6XCLliIbmrrXnur/mgKfljJZZMVwiLFwiTmFtZVwiOlwiTGluZWFyWTFcIixcIklzQXV0b0luY3JlbWVudFwiOmZhbHNlLFwiQ2FuTnVsbFwiOnRydWUsXCJkYlR5cGVcIjpcImRvdWJsZVwiLFwibGVuZ3RoXCI6XCJcIixcIlRhYmxlSURcIjo4LFwiSXNQS0lEXCI6ZmFsc2UsXCJvcmRlcmlkXCI6MzJ9LHtcImlkXCI6NzcsXCJjYXB0aW9uXCI6XCLliIbmrrXnur/mgKfljJZYMlwiLFwiTmFtZVwiOlwiTGluZWFyWDJcIixcIklzQXV0b0luY3JlbWVudFwiOmZhbHNlLFwiQ2FuTnVsbFwiOnRydWUsXCJkYlR5cGVcIjpcImRvdWJsZVwiLFwibGVuZ3RoXCI6XCJcIixcIlRhYmxlSURcIjo4LFwiSXNQS0lEXCI6ZmFsc2UsXCJvcmRlcmlkXCI6MzN9LHtcImlkXCI6NzgsXCJjYXB0aW9uXCI6XCLliIbmrrXnur/mgKfljJZZMlwiLFwiTmFtZVwiOlwiTGluZWFyWTJcIixcIklzQXV0b0luY3JlbWVudFwiOmZhbHNlLFwiQ2FuTnVsbFwiOnRydWUsXCJkYlR5cGVcIjpcImRvdWJsZVwiLFwibGVuZ3RoXCI6XCJcIixcIlRhYmxlSURcIjo4LFwiSXNQS0lEXCI6ZmFsc2UsXCJvcmRlcmlkXCI6MzR9LHtcImlkXCI6NzksXCJOYW1lXCI6XCJMaW5lYXJYM1wiLFwiSXNBdXRvSW5jcmVtZW50XCI6ZmFsc2UsXCJDYW5OdWxsXCI6dHJ1ZSxcImRiVHlwZVwiOlwiZG91YmxlXCIsXCJsZW5ndGhcIjpcIlwiLFwiVGFibGVJRFwiOjgsXCJJc1BLSURcIjpmYWxzZSxcIm9yZGVyaWRcIjozNX0se1wiaWRcIjo4MCxcIk5hbWVcIjpcIkxpbmVhclkzXCIsXCJJc0F1dG9JbmNyZW1lbnRcIjpmYWxzZSxcIkNhbk51bGxcIjp0cnVlLFwiZGJUeXBlXCI6XCJkb3VibGVcIixcImxlbmd0aFwiOlwiXCIsXCJUYWJsZUlEXCI6OCxcIklzUEtJRFwiOmZhbHNlLFwib3JkZXJpZFwiOjM2fSx7XCJpZFwiOjgxLFwiTmFtZVwiOlwiTGluZWFyWDRcIixcIklzQXV0b0luY3JlbWVudFwiOmZhbHNlLFwiQ2FuTnVsbFwiOnRydWUsXCJkYlR5cGVcIjpcImRvdWJsZVwiLFwibGVuZ3RoXCI6XCJcIixcIlRhYmxlSURcIjo4LFwiSXNQS0lEXCI6ZmFsc2UsXCJvcmRlcmlkXCI6Mzd9LHtcImlkXCI6ODIsXCJjYXB0aW9uXCI6XCLmiqXorablgLwyXCIsXCJOYW1lXCI6XCJBbGFybVZhbHVlMlwiLFwiSXNBdXRvSW5jcmVtZW50XCI6ZmFsc2UsXCJDYW5OdWxsXCI6dHJ1ZSxcImRiVHlwZVwiOlwiZG91YmxlXCIsXCJsZW5ndGhcIjpcIlwiLFwiVGFibGVJRFwiOjgsXCJJc1BLSURcIjpmYWxzZSxcIm9yZGVyaWRcIjozOH0se1wiaWRcIjo4MyxcImNhcHRpb25cIjpcIuaKpeitpuS8mOWFiOe6pzJcIixcIk5hbWVcIjpcIkFsYXJtUHJpb3JpdHkyXCIsXCJJc0F1dG9JbmNyZW1lbnRcIjpmYWxzZSxcIkNhbk51bGxcIjp0cnVlLFwiZGJUeXBlXCI6XCJpbnRcIixcImxlbmd0aFwiOlwiXCIsXCJUYWJsZUlEXCI6OCxcIklzUEtJRFwiOmZhbHNlLFwib3JkZXJpZFwiOjM5fSx7XCJpZFwiOjg0LFwiTmFtZVwiOlwiQWxhcm1WYWx1ZTNcIixcIklzQXV0b0luY3JlbWVudFwiOmZhbHNlLFwiQ2FuTnVsbFwiOnRydWUsXCJkYlR5cGVcIjpcImRvdWJsZVwiLFwibGVuZ3RoXCI6XCJcIixcIlRhYmxlSURcIjo4LFwiSXNQS0lEXCI6ZmFsc2UsXCJvcmRlcmlkXCI6NDB9LHtcImlkXCI6ODUsXCJOYW1lXCI6XCJBbGFybVByaW9yaXR5M1wiLFwiSXNBdXRvSW5jcmVtZW50XCI6ZmFsc2UsXCJDYW5OdWxsXCI6dHJ1ZSxcImRiVHlwZVwiOlwiaW50XCIsXCJsZW5ndGhcIjpcIlwiLFwiVGFibGVJRFwiOjgsXCJJc1BLSURcIjpmYWxzZSxcIm9yZGVyaWRcIjo0MX0se1wiaWRcIjo4NixcIk5hbWVcIjpcIkFsYXJtVmFsdWU0XCIsXCJJc0F1dG9JbmNyZW1lbnRcIjpmYWxzZSxcIkNhbk51bGxcIjp0cnVlLFwiZGJUeXBlXCI6XCJkb3VibGVcIixcImxlbmd0aFwiOlwiXCIsXCJUYWJsZUlEXCI6OCxcIklzUEtJRFwiOmZhbHNlLFwib3JkZXJpZFwiOjQyfSx7XCJpZFwiOjg3LFwiTmFtZVwiOlwiQWxhcm1Qcmlvcml0eTRcIixcIklzQXV0b0luY3JlbWVudFwiOmZhbHNlLFwiQ2FuTnVsbFwiOnRydWUsXCJkYlR5cGVcIjpcImludFwiLFwibGVuZ3RoXCI6XCJcIixcIlRhYmxlSURcIjo4LFwiSXNQS0lEXCI6ZmFsc2UsXCJvcmRlcmlkXCI6NDN9LHtcImlkXCI6ODgsXCJOYW1lXCI6XCJBbGFybVZhbHVlNVwiLFwiSXNBdXRvSW5jcmVtZW50XCI6ZmFsc2UsXCJDYW5OdWxsXCI6dHJ1ZSxcImRiVHlwZVwiOlwiZG91YmxlXCIsXCJsZW5ndGhcIjpcIlwiLFwiVGFibGVJRFwiOjgsXCJJc1BLSURcIjpmYWxzZSxcIm9yZGVyaWRcIjo0NH0se1wiaWRcIjo4OSxcIk5hbWVcIjpcIkFsYXJtUHJpb3JpdHk1XCIsXCJJc0F1dG9JbmNyZW1lbnRcIjpmYWxzZSxcIkNhbk51bGxcIjp0cnVlLFwiZGJUeXBlXCI6XCJpbnRcIixcImxlbmd0aFwiOlwiXCIsXCJUYWJsZUlEXCI6OCxcIklzUEtJRFwiOmZhbHNlLFwib3JkZXJpZFwiOjQ1fSx7XCJpZFwiOjkwLFwiY2FwdGlvblwiOlwi5YGP5beu5oql6K2mXCIsXCJOYW1lXCI6XCJJc0FsYXJtT2Zmc2V0XCIsXCJJc0F1dG9JbmNyZW1lbnRcIjpmYWxzZSxcIkNhbk51bGxcIjp0cnVlLFwiZGJUeXBlXCI6XCJiaXRcIixcImxlbmd0aFwiOlwiXCIsXCJkZWZhdWx0VmFsdWVcIjpcIjBcIixcIlRhYmxlSURcIjo4LFwiSXNQS0lEXCI6ZmFsc2UsXCJvcmRlcmlkXCI6NDZ9LHtcImlkXCI6OTEsXCJjYXB0aW9uXCI6XCLlgY/lt67miqXoraborr7lrprlgLxcIixcIk5hbWVcIjpcIkFsYXJtT2Zmc2V0T3JpZ2luYWxWYWx1ZVwiLFwiSXNBdXRvSW5jcmVtZW50XCI6ZmFsc2UsXCJDYW5OdWxsXCI6dHJ1ZSxcImRiVHlwZVwiOlwiZG91YmxlXCIsXCJsZW5ndGhcIjpcIlwiLFwiVGFibGVJRFwiOjgsXCJJc1BLSURcIjpmYWxzZSxcIm9yZGVyaWRcIjo0N30se1wiaWRcIjo5MixcImNhcHRpb25cIjpcIuWBj+W3ruWAvFwiLFwiTmFtZVwiOlwiQWxhcm1PZmZzZXRWYWx1ZVwiLFwiSXNBdXRvSW5jcmVtZW50XCI6ZmFsc2UsXCJDYW5OdWxsXCI6dHJ1ZSxcImRiVHlwZVwiOlwiZG91YmxlXCIsXCJsZW5ndGhcIjpcIlwiLFwiVGFibGVJRFwiOjgsXCJJc1BLSURcIjpmYWxzZSxcIm9yZGVyaWRcIjo0OH0se1wiaWRcIjo5MyxcImNhcHRpb25cIjpcIuWBj+W3ruaKpeitpuS8mOWFiOe6p1wiLFwiTmFtZVwiOlwiQWxhcm1PZmZzZXRQcmlvcml0eVwiLFwiSXNBdXRvSW5jcmVtZW50XCI6ZmFsc2UsXCJDYW5OdWxsXCI6dHJ1ZSxcImRiVHlwZVwiOlwiaW50XCIsXCJsZW5ndGhcIjpcIlwiLFwiVGFibGVJRFwiOjgsXCJJc1BLSURcIjpmYWxzZSxcIm9yZGVyaWRcIjo0OX0se1wiaWRcIjo5NCxcImNhcHRpb25cIjpcIuWPmOWMlueOh+aKpeitplwiLFwiTmFtZVwiOlwiSXNBbGFybVBlcmNlbnRcIixcIklzQXV0b0luY3JlbWVudFwiOmZhbHNlLFwiQ2FuTnVsbFwiOnRydWUsXCJkYlR5cGVcIjpcImJpdFwiLFwibGVuZ3RoXCI6XCJcIixcImRlZmF1bHRWYWx1ZVwiOlwiMFwiLFwiVGFibGVJRFwiOjgsXCJJc1BLSURcIjpmYWxzZSxcIm9yZGVyaWRcIjo1MH0se1wiaWRcIjo5NSxcImNhcHRpb25cIjpcIuWPmOWMlueOh+WAvO+8iOeZvuWIhuavlO+8iVwiLFwiTmFtZVwiOlwiUGVyY2VudFwiLFwiSXNBdXRvSW5jcmVtZW50XCI6ZmFsc2UsXCJDYW5OdWxsXCI6dHJ1ZSxcImRiVHlwZVwiOlwiZG91YmxlXCIsXCJsZW5ndGhcIjpcIlwiLFwiVGFibGVJRFwiOjgsXCJJc1BLSURcIjpmYWxzZSxcIm9yZGVyaWRcIjo1MX0se1wiaWRcIjo5NixcImNhcHRpb25cIjpcIuWPmOWMlueOh+WRqOacn++8iOenku+8iVwiLFwiTmFtZVwiOlwiQ2hhbmdlQ3ljbGVcIixcIklzQXV0b0luY3JlbWVudFwiOmZhbHNlLFwiQ2FuTnVsbFwiOnRydWUsXCJkYlR5cGVcIjpcImludFwiLFwibGVuZ3RoXCI6XCJcIixcIlRhYmxlSURcIjo4LFwiSXNQS0lEXCI6ZmFsc2UsXCJvcmRlcmlkXCI6NTJ9LHtcImlkXCI6OTcsXCJjYXB0aW9uXCI6XCLlj5jljJbnjofmiqXorabkvJjlhYjnuqdcIixcIk5hbWVcIjpcIkFsYXJtUGVyY2VudFByaW9yaXR5XCIsXCJJc0F1dG9JbmNyZW1lbnRcIjpmYWxzZSxcIkNhbk51bGxcIjp0cnVlLFwiZGJUeXBlXCI6XCJpbnRcIixcImxlbmd0aFwiOlwiXCIsXCJUYWJsZUlEXCI6OCxcIklzUEtJRFwiOmZhbHNlLFwib3JkZXJpZFwiOjUzfV0sXCJuZXdDb2x1bW5zXCI6W10sXCJjaGFuZ2VkQ29sdW1uc1wiOlt7XCJpZFwiOjEwMyxcIk5hbWVcIjpcIkZvbGRlcklkXCIsXCJJc0F1dG9JbmNyZW1lbnRcIjpmYWxzZSxcIkNhbk51bGxcIjp0cnVlLFwiZGJUeXBlXCI6XCJpbnRcIixcImxlbmd0aFwiOlwiXCIsXCJkZWZhdWx0VmFsdWVcIjpcIjBcIixcIlRhYmxlSURcIjo4LFwiSXNQS0lEXCI6ZmFsc2UsXCJvcmRlcmlkXCI6NixcIkJhY2t1cENoYW5nZWRQcm9wZXJ0aWVzXCI6e1wiTmFtZVwiOntcIk9yaWdpbmFsVmFsdWVcIjpcIlBhcmVudElkXCJ9fX1dLFwiZGVsZXRlZENvbHVtbnNcIjpbXSxcIklEWENvbmZpZ3NcIjpbXSxcIklEXCI6MH0ifSx7Ik5hbWUiOiJkYXRhYmFzZWlkIiwiVmFsdWUiOjJ9XSwiUm93U3RhdGUiOjB9LHsiSXRlbXMiOlt7Ik5hbWUiOiJpZCIsIlZhbHVlIjo0M30seyJOYW1lIjoidHlwZSIsIlZhbHVlIjoiQ3JlYXRlVGFibGVBY3Rpb24ifSx7Ik5hbWUiOiJjb250ZW50IiwiVmFsdWUiOiJ7XCJUYWJsZVwiOntcImlkXCI6MTAsXCJjYXB0aW9uXCI6XCLorr7lpIfngrnmlofku7blpLlcIixcIk5hbWVcIjpcIkRldmljZVBvaW50Rm9sZGVyXCIsXCJEYXRhYmFzZUlEXCI6MixcImlMb2NrXCI6MH0sXCJDb2x1bW5zXCI6W3tcImlkXCI6MTA0LFwiTmFtZVwiOlwiaWRcIixcIklzQXV0b0luY3JlbWVudFwiOnRydWUsXCJDYW5OdWxsXCI6ZmFsc2UsXCJkYlR5cGVcIjpcImludFwiLFwiVGFibGVJRFwiOjEwLFwiSXNQS0lEXCI6dHJ1ZSxcIm9yZGVyaWRcIjowfSx7XCJpZFwiOjEwNSxcIk5hbWVcIjpcIk5hbWVcIixcIklzQXV0b0luY3JlbWVudFwiOmZhbHNlLFwiQ2FuTnVsbFwiOnRydWUsXCJkYlR5cGVcIjpcInZhcmNoYXJcIixcImxlbmd0aFwiOlwiNTBcIixcIlRhYmxlSURcIjoxMCxcIklzUEtJRFwiOmZhbHNlLFwib3JkZXJpZFwiOjF9LHtcImlkXCI6MTA2LFwiTmFtZVwiOlwiUGFyZW50SWRcIixcIklzQXV0b0luY3JlbWVudFwiOmZhbHNlLFwiQ2FuTnVsbFwiOnRydWUsXCJkYlR5cGVcIjpcImludFwiLFwibGVuZ3RoXCI6XCJcIixcImRlZmF1bHRWYWx1ZVwiOlwiMFwiLFwiVGFibGVJRFwiOjEwLFwiSXNQS0lEXCI6ZmFsc2UsXCJvcmRlcmlkXCI6Mn1dLFwiSURYQ29uZmlnc1wiOltdLFwiSURcIjowfSJ9LHsiTmFtZSI6ImRhdGFiYXNlaWQiLCJWYWx1ZSI6Mn1dLCJSb3dTdGF0ZSI6MH0seyJJdGVtcyI6W3siTmFtZSI6ImlkIiwiVmFsdWUiOjQ0fSx7Ik5hbWUiOiJ0eXBlIiwiVmFsdWUiOiJDaGFuZ2VUYWJsZUFjdGlvbiJ9LHsiTmFtZSI6ImNvbnRlbnQiLCJWYWx1ZSI6IntcIk9sZFRhYmxlTmFtZVwiOlwiRGV2aWNlUG9pbnRGb2xkZXJcIixcIk5ld1RhYmxlTmFtZVwiOlwiRGV2aWNlUG9pbnRGb2xkZXJcIixcIm90aGVyQ29sdW1uc1wiOlt7XCJpZFwiOjEwNCxcIk5hbWVcIjpcImlkXCIsXCJJc0F1dG9JbmNyZW1lbnRcIjp0cnVlLFwiQ2FuTnVsbFwiOmZhbHNlLFwiZGJUeXBlXCI6XCJpbnRcIixcIlRhYmxlSURcIjoxMCxcIklzUEtJRFwiOnRydWUsXCJvcmRlcmlkXCI6MH0se1wiaWRcIjoxMDUsXCJOYW1lXCI6XCJOYW1lXCIsXCJJc0F1dG9JbmNyZW1lbnRcIjpmYWxzZSxcIkNhbk51bGxcIjp0cnVlLFwiZGJUeXBlXCI6XCJ2YXJjaGFyXCIsXCJsZW5ndGhcIjpcIjUwXCIsXCJUYWJsZUlEXCI6MTAsXCJJc1BLSURcIjpmYWxzZSxcIm9yZGVyaWRcIjoxfSx7XCJpZFwiOjEwNixcIk5hbWVcIjpcIlBhcmVudElkXCIsXCJJc0F1dG9JbmNyZW1lbnRcIjpmYWxzZSxcIkNhbk51bGxcIjp0cnVlLFwiZGJUeXBlXCI6XCJpbnRcIixcImxlbmd0aFwiOlwiXCIsXCJkZWZhdWx0VmFsdWVcIjpcIjBcIixcIlRhYmxlSURcIjoxMCxcIklzUEtJRFwiOmZhbHNlLFwib3JkZXJpZFwiOjJ9XSxcIm5ld0NvbHVtbnNcIjpbXSxcImNoYW5nZWRDb2x1bW5zXCI6W10sXCJkZWxldGVkQ29sdW1uc1wiOltdLFwiSURYQ29uZmlnc1wiOltdLFwiSURcIjowfSJ9LHsiTmFtZSI6ImRhdGFiYXNlaWQiLCJWYWx1ZSI6Mn1dLCJSb3dTdGF0ZSI6MH0seyJJdGVtcyI6W3siTmFtZSI6ImlkIiwiVmFsdWUiOjQ1fSx7Ik5hbWUiOiJ0eXBlIiwiVmFsdWUiOiJDaGFuZ2VUYWJsZUFjdGlvbiJ9LHsiTmFtZSI6ImNvbnRlbnQiLCJWYWx1ZSI6IntcIk9sZFRhYmxlTmFtZVwiOlwiRGV2aWNlUG9pbnRGb2xkZXJcIixcIk5ld1RhYmxlTmFtZVwiOlwiRGV2aWNlUG9pbnRGb2xkZXJcIixcIm90aGVyQ29sdW1uc1wiOlt7XCJpZFwiOjEwNCxcIk5hbWVcIjpcImlkXCIsXCJJc0F1dG9JbmNyZW1lbnRcIjp0cnVlLFwiQ2FuTnVsbFwiOmZhbHNlLFwiZGJUeXBlXCI6XCJpbnRcIixcIlRhYmxlSURcIjoxMCxcIklzUEtJRFwiOnRydWUsXCJvcmRlcmlkXCI6MH0se1wiaWRcIjoxMDUsXCJOYW1lXCI6XCJOYW1lXCIsXCJJc0F1dG9JbmNyZW1lbnRcIjpmYWxzZSxcIkNhbk51bGxcIjp0cnVlLFwiZGJUeXBlXCI6XCJ2YXJjaGFyXCIsXCJsZW5ndGhcIjpcIjUwXCIsXCJUYWJsZUlEXCI6MTAsXCJJc1BLSURcIjpmYWxzZSxcIm9yZGVyaWRcIjoxfSx7XCJpZFwiOjEwNixcIk5hbWVcIjpcIlBhcmVudElkXCIsXCJJc0F1dG9JbmNyZW1lbnRcIjpmYWxzZSxcIkNhbk51bGxcIjp0cnVlLFwiZGJUeXBlXCI6XCJpbnRcIixcImxlbmd0aFwiOlwiXCIsXCJkZWZhdWx0VmFsdWVcIjpcIjBcIixcIlRhYmxlSURcIjoxMCxcIklzUEtJRFwiOmZhbHNlLFwib3JkZXJpZFwiOjJ9XSxcIm5ld0NvbHVtbnNcIjpbe1wiaWRcIjoxMDcsXCJOYW1lXCI6XCJEcml2ZXJJZFwiLFwiSXNBdXRvSW5jcmVtZW50XCI6ZmFsc2UsXCJDYW5OdWxsXCI6dHJ1ZSxcImRiVHlwZVwiOlwiaW50XCIsXCJsZW5ndGhcIjpcIlwiLFwiVGFibGVJRFwiOjEwLFwiSXNQS0lEXCI6ZmFsc2UsXCJvcmRlcmlkXCI6M31dLFwiY2hhbmdlZENvbHVtbnNcIjpbXSxcImRlbGV0ZWRDb2x1bW5zXCI6W10sXCJJRFhDb25maWdzXCI6W10sXCJJRFwiOjB9In0seyJOYW1lIjoiZGF0YWJhc2VpZCIsIlZhbHVlIjoyfV0sIlJvd1N0YXRlIjowfSx7Ikl0ZW1zIjpbeyJOYW1lIjoiaWQiLCJWYWx1ZSI6NDZ9LHsiTmFtZSI6InR5cGUiLCJWYWx1ZSI6IkNoYW5nZVRhYmxlQWN0aW9uIn0seyJOYW1lIjoiY29udGVudCIsIlZhbHVlIjoie1wiT2xkVGFibGVOYW1lXCI6XCJEZXZpY2VQb2ludEZvbGRlclwiLFwiTmV3VGFibGVOYW1lXCI6XCJEZXZpY2VQb2ludEZvbGRlclwiLFwib3RoZXJDb2x1bW5zXCI6W3tcImlkXCI6MTA0LFwiTmFtZVwiOlwiaWRcIixcIklzQXV0b0luY3JlbWVudFwiOnRydWUsXCJDYW5OdWxsXCI6ZmFsc2UsXCJkYlR5cGVcIjpcImludFwiLFwiVGFibGVJRFwiOjEwLFwiSXNQS0lEXCI6dHJ1ZSxcIm9yZGVyaWRcIjowfSx7XCJpZFwiOjEwNSxcIk5hbWVcIjpcIk5hbWVcIixcIklzQXV0b0luY3JlbWVudFwiOmZhbHNlLFwiQ2FuTnVsbFwiOnRydWUsXCJkYlR5cGVcIjpcInZhcmNoYXJcIixcImxlbmd0aFwiOlwiNTBcIixcIlRhYmxlSURcIjoxMCxcIklzUEtJRFwiOmZhbHNlLFwib3JkZXJpZFwiOjF9LHtcImlkXCI6MTA2LFwiTmFtZVwiOlwiUGFyZW50SWRcIixcIklzQXV0b0luY3JlbWVudFwiOmZhbHNlLFwiQ2FuTnVsbFwiOnRydWUsXCJkYlR5cGVcIjpcImludFwiLFwibGVuZ3RoXCI6XCJcIixcImRlZmF1bHRWYWx1ZVwiOlwiMFwiLFwiVGFibGVJRFwiOjEwLFwiSXNQS0lEXCI6ZmFsc2UsXCJvcmRlcmlkXCI6Mn1dLFwibmV3Q29sdW1uc1wiOltdLFwiY2hhbmdlZENvbHVtbnNcIjpbe1wiaWRcIjoxMDcsXCJOYW1lXCI6XCJEZXZpY2VJZFwiLFwiSXNBdXRvSW5jcmVtZW50XCI6ZmFsc2UsXCJDYW5OdWxsXCI6dHJ1ZSxcImRiVHlwZVwiOlwiaW50XCIsXCJsZW5ndGhcIjpcIlwiLFwiVGFibGVJRFwiOjEwLFwiSXNQS0lEXCI6ZmFsc2UsXCJvcmRlcmlkXCI6MyxcIkJhY2t1cENoYW5nZWRQcm9wZXJ0aWVzXCI6e1wiTmFtZVwiOntcIk9yaWdpbmFsVmFsdWVcIjpcIkRyaXZlcklkXCJ9fX1dLFwiZGVsZXRlZENvbHVtbnNcIjpbXSxcIklEWENvbmZpZ3NcIjpbXSxcIklEXCI6MH0ifSx7Ik5hbWUiOiJkYXRhYmFzZWlkIiwiVmFsdWUiOjJ9XSwiUm93U3RhdGUiOjB9LHsiSXRlbXMiOlt7Ik5hbWUiOiJpZCIsIlZhbHVlIjo0N30seyJOYW1lIjoidHlwZSIsIlZhbHVlIjoiQ2hhbmdlVGFibGVBY3Rpb24ifSx7Ik5hbWUiOiJjb250ZW50IiwiVmFsdWUiOiJ7XCJPbGRUYWJsZU5hbWVcIjpcIkRldmljZVwiLFwiTmV3VGFibGVOYW1lXCI6XCJEZXZpY2VcIixcIm90aGVyQ29sdW1uc1wiOlt7XCJpZFwiOjM5LFwiTmFtZVwiOlwiaWRcIixcIklzQXV0b0luY3JlbWVudFwiOnRydWUsXCJDYW5OdWxsXCI6ZmFsc2UsXCJkYlR5cGVcIjpcImludFwiLFwiVGFibGVJRFwiOjcsXCJJc1BLSURcIjp0cnVlLFwib3JkZXJpZFwiOjB9LHtcImlkXCI6NDAsXCJjYXB0aW9uXCI6XCLpgJrorq/nvZHlhbNcIixcIk5hbWVcIjpcIkRyaXZlcklEXCIsXCJJc0F1dG9JbmNyZW1lbnRcIjpmYWxzZSxcIkNhbk51bGxcIjp0cnVlLFwiZGJUeXBlXCI6XCJpbnRcIixcImxlbmd0aFwiOlwiXCIsXCJUYWJsZUlEXCI6NyxcIklzUEtJRFwiOmZhbHNlLFwib3JkZXJpZFwiOjF9LHtcImlkXCI6NDEsXCJjYXB0aW9uXCI6XCLlnLDlnYDkv6Hmga9cIixcIk5hbWVcIjpcIkFkZHJlc3NcIixcIklzQXV0b0luY3JlbWVudFwiOmZhbHNlLFwiQ2FuTnVsbFwiOnRydWUsXCJkYlR5cGVcIjpcInZhcmNoYXJcIixcImxlbmd0aFwiOlwiMTAwXCIsXCJUYWJsZUlEXCI6NyxcIklzUEtJRFwiOmZhbHNlLFwib3JkZXJpZFwiOjN9LHtcImlkXCI6MTAwLFwiY2FwdGlvblwiOlwi5Zyw5Z2A6K6+572u5pe255qEanNvbuWvueixoeihqOi+vlwiLFwiTmFtZVwiOlwiQWRkclNldHRpbmdcIixcIklzQXV0b0luY3JlbWVudFwiOmZhbHNlLFwiQ2FuTnVsbFwiOnRydWUsXCJkYlR5cGVcIjpcInZhcmNoYXJcIixcImxlbmd0aFwiOlwiMjAwXCIsXCJUYWJsZUlEXCI6NyxcIklzUEtJRFwiOmZhbHNlLFwib3JkZXJpZFwiOjR9LHtcImlkXCI6MTAxLFwiY2FwdGlvblwiOlwi6K6+5aSH5ZCN56ewXCIsXCJOYW1lXCI6XCJOYW1lXCIsXCJJc0F1dG9JbmNyZW1lbnRcIjpmYWxzZSxcIkNhbk51bGxcIjp0cnVlLFwiZGJUeXBlXCI6XCJ2YXJjaGFyXCIsXCJsZW5ndGhcIjpcIjUwXCIsXCJUYWJsZUlEXCI6NyxcIklzUEtJRFwiOmZhbHNlLFwib3JkZXJpZFwiOjJ9LHtcImlkXCI6MTAyLFwiY2FwdGlvblwiOlwi5o6n5Yi25Y2V5YWDaWRcIixcIk5hbWVcIjpcIlVuaXRJZFwiLFwiSXNBdXRvSW5jcmVtZW50XCI6ZmFsc2UsXCJDYW5OdWxsXCI6dHJ1ZSxcImRiVHlwZVwiOlwiaW50XCIsXCJsZW5ndGhcIjpcIlwiLFwiVGFibGVJRFwiOjcsXCJJc1BLSURcIjpmYWxzZSxcIm9yZGVyaWRcIjo1fV0sXCJuZXdDb2x1bW5zXCI6W10sXCJjaGFuZ2VkQ29sdW1uc1wiOltdLFwiZGVsZXRlZENvbHVtbnNcIjpbXSxcIklEWENvbmZpZ3NcIjpbXSxcIklEXCI6MH0ifSx7Ik5hbWUiOiJkYXRhYmFzZWlkIiwiVmFsdWUiOjJ9XSwiUm93U3RhdGUiOjB9LHsiSXRlbXMiOlt7Ik5hbWUiOiJpZCIsIlZhbHVlIjo0OH0seyJOYW1lIjoidHlwZSIsIlZhbHVlIjoiQ2hhbmdlVGFibGVBY3Rpb24ifSx7Ik5hbWUiOiJjb250ZW50IiwiVmFsdWUiOiJ7XCJPbGRUYWJsZU5hbWVcIjpcIkRldmljZVBvaW50Rm9sZGVyXCIsXCJOZXdUYWJsZU5hbWVcIjpcIkRldmljZVBvaW50Rm9sZGVyXCIsXCJvdGhlckNvbHVtbnNcIjpbe1wiaWRcIjoxMDQsXCJOYW1lXCI6XCJpZFwiLFwiSXNBdXRvSW5jcmVtZW50XCI6dHJ1ZSxcIkNhbk51bGxcIjpmYWxzZSxcImRiVHlwZVwiOlwiaW50XCIsXCJUYWJsZUlEXCI6MTAsXCJJc1BLSURcIjp0cnVlLFwib3JkZXJpZFwiOjB9LHtcImlkXCI6MTA2LFwiTmFtZVwiOlwiUGFyZW50SWRcIixcIklzQXV0b0luY3JlbWVudFwiOmZhbHNlLFwiQ2FuTnVsbFwiOnRydWUsXCJkYlR5cGVcIjpcImludFwiLFwibGVuZ3RoXCI6XCJcIixcImRlZmF1bHRWYWx1ZVwiOlwiMFwiLFwiVGFibGVJRFwiOjEwLFwiSXNQS0lEXCI6ZmFsc2UsXCJvcmRlcmlkXCI6Mn0se1wiaWRcIjoxMDcsXCJOYW1lXCI6XCJEZXZpY2VJZFwiLFwiSXNBdXRvSW5jcmVtZW50XCI6ZmFsc2UsXCJDYW5OdWxsXCI6dHJ1ZSxcImRiVHlwZVwiOlwiaW50XCIsXCJsZW5ndGhcIjpcIlwiLFwiVGFibGVJRFwiOjEwLFwiSXNQS0lEXCI6ZmFsc2UsXCJvcmRlcmlkXCI6M31dLFwibmV3Q29sdW1uc1wiOltdLFwiY2hhbmdlZENvbHVtbnNcIjpbe1wiaWRcIjoxMDUsXCJOYW1lXCI6XCJOYW1lXCIsXCJJc0F1dG9JbmNyZW1lbnRcIjpmYWxzZSxcIkNhbk51bGxcIjp0cnVlLFwiZGJUeXBlXCI6XCJ2YXJjaGFyXCIsXCJsZW5ndGhcIjpcIjEwMFwiLFwiVGFibGVJRFwiOjEwLFwiSXNQS0lEXCI6ZmFsc2UsXCJvcmRlcmlkXCI6MSxcIkJhY2t1cENoYW5nZWRQcm9wZXJ0aWVzXCI6e1wibGVuZ3RoXCI6e1wiT3JpZ2luYWxWYWx1ZVwiOlwiNTBcIn19fV0sXCJkZWxldGVkQ29sdW1uc1wiOltdLFwiSURYQ29uZmlnc1wiOltdLFwiSURcIjowfSJ9LHsiTmFtZSI6ImRhdGFiYXNlaWQiLCJWYWx1ZSI6Mn1dLCJSb3dTdGF0ZSI6MH0seyJJdGVtcyI6W3siTmFtZSI6ImlkIiwiVmFsdWUiOjQ5fSx7Ik5hbWUiOiJ0eXBlIiwiVmFsdWUiOiJDaGFuZ2VUYWJsZUFjdGlvbiJ9LHsiTmFtZSI6ImNvbnRlbnQiLCJWYWx1ZSI6IntcIk9sZFRhYmxlTmFtZVwiOlwiRGV2aWNlUG9pbnRGb2xkZXJcIixcIk5ld1RhYmxlTmFtZVwiOlwiRGV2aWNlUG9pbnRGb2xkZXJcIixcIm90aGVyQ29sdW1uc1wiOlt7XCJpZFwiOjEwNCxcIk5hbWVcIjpcImlkXCIsXCJJc0F1dG9JbmNyZW1lbnRcIjp0cnVlLFwiQ2FuTnVsbFwiOmZhbHNlLFwiZGJUeXBlXCI6XCJpbnRcIixcIlRhYmxlSURcIjoxMCxcIklzUEtJRFwiOnRydWUsXCJvcmRlcmlkXCI6MH0se1wiaWRcIjoxMDUsXCJOYW1lXCI6XCJOYW1lXCIsXCJJc0F1dG9JbmNyZW1lbnRcIjpmYWxzZSxcIkNhbk51bGxcIjp0cnVlLFwiZGJUeXBlXCI6XCJ2YXJjaGFyXCIsXCJsZW5ndGhcIjpcIjEwMFwiLFwiVGFibGVJRFwiOjEwLFwiSXNQS0lEXCI6ZmFsc2UsXCJvcmRlcmlkXCI6MX0se1wiaWRcIjoxMDYsXCJOYW1lXCI6XCJQYXJlbnRJZFwiLFwiSXNBdXRvSW5jcmVtZW50XCI6ZmFsc2UsXCJDYW5OdWxsXCI6dHJ1ZSxcImRiVHlwZVwiOlwiaW50XCIsXCJsZW5ndGhcIjpcIlwiLFwiZGVmYXVsdFZhbHVlXCI6XCIwXCIsXCJUYWJsZUlEXCI6MTAsXCJJc1BLSURcIjpmYWxzZSxcIm9yZGVyaWRcIjoyfSx7XCJpZFwiOjEwNyxcIk5hbWVcIjpcIkRldmljZUlkXCIsXCJJc0F1dG9JbmNyZW1lbnRcIjpmYWxzZSxcIkNhbk51bGxcIjp0cnVlLFwiZGJUeXBlXCI6XCJpbnRcIixcImxlbmd0aFwiOlwiXCIsXCJUYWJsZUlEXCI6MTAsXCJJc1BLSURcIjpmYWxzZSxcIm9yZGVyaWRcIjozfV0sXCJuZXdDb2x1bW5zXCI6W3tcImlkXCI6MTA4LFwiTmFtZVwiOlwiVHlwZVwiLFwiSXNBdXRvSW5jcmVtZW50XCI6ZmFsc2UsXCJDYW5OdWxsXCI6dHJ1ZSxcImRiVHlwZVwiOlwiaW50XCIsXCJFbnVtRGVmaW5lXCI6XCJBbmFsb2cgPSAxLFxcbkRpZ2l0YWwgPSAyXCIsXCJsZW5ndGhcIjpcIlwiLFwiZGVmYXVsdFZhbHVlXCI6XCJcIixcIlRhYmxlSURcIjoxMCxcIklzUEtJRFwiOmZhbHNlLFwib3JkZXJpZFwiOjR9XSxcImNoYW5nZWRDb2x1bW5zXCI6W10sXCJkZWxldGVkQ29sdW1uc1wiOltdLFwiSURYQ29uZmlnc1wiOltdLFwiSURcIjowfSJ9LHsiTmFtZSI6ImRhdGFiYXNlaWQiLCJWYWx1ZSI6Mn1dLCJSb3dTdGF0ZSI6MH0seyJJdGVtcyI6W3siTmFtZSI6ImlkIiwiVmFsdWUiOjUwfSx7Ik5hbWUiOiJ0eXBlIiwiVmFsdWUiOiJDaGFuZ2VUYWJsZUFjdGlvbiJ9LHsiTmFtZSI6ImNvbnRlbnQiLCJWYWx1ZSI6IntcIk9sZFRhYmxlTmFtZVwiOlwiRGV2aWNlUG9pbnRcIixcIk5ld1RhYmxlTmFtZVwiOlwiRGV2aWNlUG9pbnRcIixcIm90aGVyQ29sdW1uc1wiOlt7XCJpZFwiOjQyLFwiTmFtZVwiOlwiaWRcIixcIklzQXV0b0luY3JlbWVudFwiOnRydWUsXCJDYW5OdWxsXCI6ZmFsc2UsXCJkYlR5cGVcIjpcImludFwiLFwiVGFibGVJRFwiOjgsXCJJc1BLSURcIjp0cnVlLFwib3JkZXJpZFwiOjB9LHtcImlkXCI6NDMsXCJjYXB0aW9uXCI6XCLngrnlkI1cIixcIk5hbWVcIjpcIk5hbWVcIixcIklzQXV0b0luY3JlbWVudFwiOmZhbHNlLFwiQ2FuTnVsbFwiOnRydWUsXCJkYlR5cGVcIjpcImludFwiLFwibGVuZ3RoXCI6XCJcIixcIlRhYmxlSURcIjo4LFwiSXNQS0lEXCI6ZmFsc2UsXCJvcmRlcmlkXCI6MX0se1wiaWRcIjo0NCxcIk5hbWVcIjpcIkFkZHJlc3NcIixcIklzQXV0b0luY3JlbWVudFwiOmZhbHNlLFwiQ2FuTnVsbFwiOnRydWUsXCJkYlR5cGVcIjpcInZhcmNoYXJcIixcImxlbmd0aFwiOlwiMjAwXCIsXCJUYWJsZUlEXCI6OCxcIklzUEtJRFwiOmZhbHNlLFwib3JkZXJpZFwiOjN9LHtcImlkXCI6NDUsXCJjYXB0aW9uXCI6XCLmmK/lkKbnm5HmtYvlj5jljJZcIixcIk5hbWVcIjpcIklzV2F0Y2hpbmdcIixcIklzQXV0b0luY3JlbWVudFwiOmZhbHNlLFwiQ2FuTnVsbFwiOnRydWUsXCJkYlR5cGVcIjpcImJpdFwiLFwibGVuZ3RoXCI6XCJcIixcImRlZmF1bHRWYWx1ZVwiOlwiMFwiLFwiVGFibGVJRFwiOjgsXCJJc1BLSURcIjpmYWxzZSxcIm9yZGVyaWRcIjo1NH0se1wiaWRcIjo0OCxcImNhcHRpb25cIjpcIuWcsOWdgOiuvue9ruaXtueahGpzb27lr7nosaHooajovr5cIixcIk5hbWVcIjpcIkFkZHJTZXR0aW5nXCIsXCJJc0F1dG9JbmNyZW1lbnRcIjpmYWxzZSxcIkNhbk51bGxcIjp0cnVlLFwiZGJUeXBlXCI6XCJ2YXJjaGFyXCIsXCJsZW5ndGhcIjpcIjIwMFwiLFwiVGFibGVJRFwiOjgsXCJJc1BLSURcIjpmYWxzZSxcIm9yZGVyaWRcIjo0fSx7XCJpZFwiOjQ5LFwiY2FwdGlvblwiOlwi5o+P6L+wXCIsXCJOYW1lXCI6XCJEZXNjXCIsXCJJc0F1dG9JbmNyZW1lbnRcIjpmYWxzZSxcIkNhbk51bGxcIjp0cnVlLFwiZGJUeXBlXCI6XCJ2YXJjaGFyXCIsXCJsZW5ndGhcIjpcIjEwMFwiLFwiVGFibGVJRFwiOjgsXCJJc1BLSURcIjpmYWxzZSxcIm9yZGVyaWRcIjoyfSx7XCJpZFwiOjUwLFwiY2FwdGlvblwiOlwi5Yid5aeL5YC8XCIsXCJOYW1lXCI6XCJJbml0VmFsdWVcIixcIklzQXV0b0luY3JlbWVudFwiOmZhbHNlLFwiQ2FuTnVsbFwiOnRydWUsXCJkYlR5cGVcIjpcImRvdWJsZVwiLFwibGVuZ3RoXCI6XCJcIixcIlRhYmxlSURcIjo4LFwiSXNQS0lEXCI6ZmFsc2UsXCJvcmRlcmlkXCI6N30se1wiaWRcIjo1MSxcImNhcHRpb25cIjpcIuW8gOeKtuaAgeS/oeaBr1wiLFwiTmFtZVwiOlwiU3RhdHVzT3BlbkluZm9cIixcIklzQXV0b0luY3JlbWVudFwiOmZhbHNlLFwiQ2FuTnVsbFwiOnRydWUsXCJkYlR5cGVcIjpcInZhcmNoYXJcIixcImxlbmd0aFwiOlwiNTBcIixcIlRhYmxlSURcIjo4LFwiSXNQS0lEXCI6ZmFsc2UsXCJvcmRlcmlkXCI6OH0se1wiaWRcIjo1MixcImNhcHRpb25cIjpcIuWFs+eKtuaAgeS/oeaBr1wiLFwiTmFtZVwiOlwiU3RhdGVDbG9zZUluZm9cIixcIklzQXV0b0luY3JlbWVudFwiOmZhbHNlLFwiQ2FuTnVsbFwiOnRydWUsXCJkYlR5cGVcIjpcInZhcmNoYXJcIixcImxlbmd0aFwiOlwiNTBcIixcIlRhYmxlSURcIjo4LFwiSXNQS0lEXCI6ZmFsc2UsXCJvcmRlcmlkXCI6OX0se1wiaWRcIjo1MyxcImNhcHRpb25cIjpcIuaKpeitpuW8gOWFs1wiLFwiTmFtZVwiOlwiSXNBbGFybVwiLFwiSXNBdXRvSW5jcmVtZW50XCI6ZmFsc2UsXCJDYW5OdWxsXCI6dHJ1ZSxcImRiVHlwZVwiOlwiYml0XCIsXCJsZW5ndGhcIjpcIlwiLFwiZGVmYXVsdFZhbHVlXCI6XCIwXCIsXCJUYWJsZUlEXCI6OCxcIklzUEtJRFwiOmZhbHNlLFwib3JkZXJpZFwiOjEwfSx7XCJpZFwiOjU0LFwiY2FwdGlvblwiOlwi5oql6K2m5YC8XCIsXCJOYW1lXCI6XCJBbGFybVZhbHVlXCIsXCJJc0F1dG9JbmNyZW1lbnRcIjpmYWxzZSxcIkNhbk51bGxcIjp0cnVlLFwiZGJUeXBlXCI6XCJkb3VibGVcIixcImxlbmd0aFwiOlwiXCIsXCJUYWJsZUlEXCI6OCxcIklzUEtJRFwiOmZhbHNlLFwib3JkZXJpZFwiOjExfSx7XCJpZFwiOjU1LFwiY2FwdGlvblwiOlwi5oql6K2m5LyY5YWI57qnXCIsXCJOYW1lXCI6XCJBbGFybVByaW9yaXR5XCIsXCJJc0F1dG9JbmNyZW1lbnRcIjpmYWxzZSxcIkNhbk51bGxcIjp0cnVlLFwiZGJUeXBlXCI6XCJpbnRcIixcImxlbmd0aFwiOlwiXCIsXCJUYWJsZUlEXCI6OCxcIklzUEtJRFwiOmZhbHNlLFwib3JkZXJpZFwiOjEyfSx7XCJpZFwiOjU2LFwiY2FwdGlvblwiOlwi5oql6K2m57uEXCIsXCJOYW1lXCI6XCJBbGFybUdyb3VwXCIsXCJJc0F1dG9JbmNyZW1lbnRcIjpmYWxzZSxcIkNhbk51bGxcIjp0cnVlLFwiZGJUeXBlXCI6XCJ2YXJjaGFyXCIsXCJsZW5ndGhcIjpcIjUwXCIsXCJUYWJsZUlEXCI6OCxcIklzUEtJRFwiOmZhbHNlLFwib3JkZXJpZFwiOjEzfSx7XCJpZFwiOjU3LFwiY2FwdGlvblwiOlwi5oql6K2m6Ieq5Yqo56Gu6K6kXCIsXCJOYW1lXCI6XCJBbGFybUF1dG9Db25maXJtXCIsXCJJc0F1dG9JbmNyZW1lbnRcIjpmYWxzZSxcIkNhbk51bGxcIjp0cnVlLFwiZGJUeXBlXCI6XCJiaXRcIixcImxlbmd0aFwiOlwiXCIsXCJkZWZhdWx0VmFsdWVcIjpcIjBcIixcIlRhYmxlSURcIjo4LFwiSXNQS0lEXCI6ZmFsc2UsXCJvcmRlcmlkXCI6MTR9LHtcImlkXCI6NTgsXCJjYXB0aW9uXCI6XCLmiqXoraboh6rliqjlpI3kvY1cIixcIk5hbWVcIjpcIkFsYXJtQXV0b1Jlc2V0XCIsXCJJc0F1dG9JbmNyZW1lbnRcIjpmYWxzZSxcIkNhbk51bGxcIjp0cnVlLFwiZGJUeXBlXCI6XCJiaXRcIixcImxlbmd0aFwiOlwiXCIsXCJkZWZhdWx0VmFsdWVcIjpcIjBcIixcIlRhYmxlSURcIjo4LFwiSXNQS0lEXCI6ZmFsc2UsXCJvcmRlcmlkXCI6MTV9LHtcImlkXCI6NTksXCJjYXB0aW9uXCI6XCLmlbDmja7nu53lr7nlj5jljJbkv53lrZhcIixcIk5hbWVcIjpcIlZhbHVlQWJzb2x1dGVDaGFuZ2VcIixcIklzQXV0b0luY3JlbWVudFwiOmZhbHNlLFwiQ2FuTnVsbFwiOnRydWUsXCJkYlR5cGVcIjpcImJpdFwiLFwibGVuZ3RoXCI6XCJcIixcImRlZmF1bHRWYWx1ZVwiOlwiMFwiLFwiVGFibGVJRFwiOjgsXCJJc1BLSURcIjpmYWxzZSxcIm9yZGVyaWRcIjoxNn0se1wiaWRcIjo2MCxcImNhcHRpb25cIjpcIuaVsOaNrue7neWvueWPmOWMluWAvFwiLFwiTmFtZVwiOlwiVmFsdWVBYnNvbHV0ZUNoYW5nZVNldHRpbmdcIixcIklzQXV0b0luY3JlbWVudFwiOmZhbHNlLFwiQ2FuTnVsbFwiOnRydWUsXCJkYlR5cGVcIjpcImRvdWJsZVwiLFwibGVuZ3RoXCI6XCJcIixcIlRhYmxlSURcIjo4LFwiSXNQS0lEXCI6ZmFsc2UsXCJvcmRlcmlkXCI6MTd9LHtcImlkXCI6NjEsXCJjYXB0aW9uXCI6XCLmlbDmja7nm7jlr7nlj5jljJbkv53lrZhcIixcIk5hbWVcIjpcIlZhbHVlUmVsYXRpdmVDaGFuZ2VcIixcIklzQXV0b0luY3JlbWVudFwiOmZhbHNlLFwiQ2FuTnVsbFwiOnRydWUsXCJkYlR5cGVcIjpcImJpdFwiLFwibGVuZ3RoXCI6XCJcIixcImRlZmF1bHRWYWx1ZVwiOlwiMFwiLFwiVGFibGVJRFwiOjgsXCJJc1BLSURcIjpmYWxzZSxcIm9yZGVyaWRcIjoxOH0se1wiaWRcIjo2MixcImNhcHRpb25cIjpcIuaVsOaNruebuOWvueWPmOWMluWAvFwiLFwiTmFtZVwiOlwiVmFsdWVSZWxhdGl2ZUNoYW5nZVNldHRpbmdcIixcIklzQXV0b0luY3JlbWVudFwiOmZhbHNlLFwiQ2FuTnVsbFwiOnRydWUsXCJkYlR5cGVcIjpcImRvdWJsZVwiLFwibGVuZ3RoXCI6XCJcIixcIlRhYmxlSURcIjo4LFwiSXNQS0lEXCI6ZmFsc2UsXCJvcmRlcmlkXCI6MTl9LHtcImlkXCI6NjMsXCJjYXB0aW9uXCI6XCLmlbDmja7lrprml7bkv53lrZhcIixcIk5hbWVcIjpcIlZhbHVlT25UaW1lQ2hhbmdlXCIsXCJJc0F1dG9JbmNyZW1lbnRcIjpmYWxzZSxcIkNhbk51bGxcIjp0cnVlLFwiZGJUeXBlXCI6XCJiaXRcIixcImxlbmd0aFwiOlwiXCIsXCJkZWZhdWx0VmFsdWVcIjpcIjBcIixcIlRhYmxlSURcIjo4LFwiSXNQS0lEXCI6ZmFsc2UsXCJvcmRlcmlkXCI6MjB9LHtcImlkXCI6NjQsXCJjYXB0aW9uXCI6XCLmlbDmja7lrprml7blgLxcIixcIk5hbWVcIjpcIlZhbHVlT25UaW1lQ2hhbmdlU2V0dGluZ1wiLFwiSXNBdXRvSW5jcmVtZW50XCI6ZmFsc2UsXCJDYW5OdWxsXCI6dHJ1ZSxcImRiVHlwZVwiOlwiZG91YmxlXCIsXCJsZW5ndGhcIjpcIlwiLFwiVGFibGVJRFwiOjgsXCJJc1BLSURcIjpmYWxzZSxcIm9yZGVyaWRcIjoyMX0se1wiaWRcIjo2NSxcImNhcHRpb25cIjpcIueCueexu+Wei1wiLFwiTmFtZVwiOlwiVHlwZVwiLFwiSXNBdXRvSW5jcmVtZW50XCI6ZmFsc2UsXCJDYW5OdWxsXCI6dHJ1ZSxcImRiVHlwZVwiOlwiaW50XCIsXCJFbnVtRGVmaW5lXCI6XCJBbmFsb2cgPSAxLFxcbkRpZ2l0YWwgPSAyXCIsXCJsZW5ndGhcIjpcIlwiLFwiZGVmYXVsdFZhbHVlXCI6XCIyXCIsXCJUYWJsZUlEXCI6OCxcIklzUEtJRFwiOmZhbHNlLFwib3JkZXJpZFwiOjV9LHtcImlkXCI6NjYsXCJjYXB0aW9uXCI6XCLmqKHmi5/ph48t5bel56iL5Y2V5L2NXCIsXCJOYW1lXCI6XCJVbml0XCIsXCJJc0F1dG9JbmNyZW1lbnRcIjpmYWxzZSxcIkNhbk51bGxcIjp0cnVlLFwiZGJUeXBlXCI6XCJ2YXJjaGFyXCIsXCJsZW5ndGhcIjpcIjUwXCIsXCJUYWJsZUlEXCI6OCxcIklzUEtJRFwiOmZhbHNlLFwib3JkZXJpZFwiOjIyfSx7XCJpZFwiOjY3LFwiY2FwdGlvblwiOlwi5qih5ouf6YePLeWwj+aVsOeCueS9jeaVsFwiLFwiTmFtZVwiOlwiRFBDb3VudFwiLFwiSXNBdXRvSW5jcmVtZW50XCI6ZmFsc2UsXCJDYW5OdWxsXCI6dHJ1ZSxcImRiVHlwZVwiOlwidmFyY2hhclwiLFwibGVuZ3RoXCI6XCI1MFwiLFwiVGFibGVJRFwiOjgsXCJJc1BLSURcIjpmYWxzZSxcIm9yZGVyaWRcIjoyM30se1wiaWRcIjo2OCxcImNhcHRpb25cIjpcIuaooeaLn+mHjy3ph4/nqIvovazljJblvIDlhbNcIixcIk5hbWVcIjpcIklzVHJhbnNmb3JtXCIsXCJJc0F1dG9JbmNyZW1lbnRcIjpmYWxzZSxcIkNhbk51bGxcIjp0cnVlLFwiZGJUeXBlXCI6XCJiaXRcIixcImxlbmd0aFwiOlwiXCIsXCJkZWZhdWx0VmFsdWVcIjpcIjBcIixcIlRhYmxlSURcIjo4LFwiSXNQS0lEXCI6ZmFsc2UsXCJvcmRlcmlkXCI6MjR9LHtcImlkXCI6NjksXCJjYXB0aW9uXCI6XCLmqKHmi5/ph48t6YeP56iL5LiK6ZmQXCIsXCJOYW1lXCI6XCJUcmFuc01heFwiLFwiSXNBdXRvSW5jcmVtZW50XCI6ZmFsc2UsXCJDYW5OdWxsXCI6dHJ1ZSxcImRiVHlwZVwiOlwiZG91YmxlXCIsXCJsZW5ndGhcIjpcIlwiLFwiVGFibGVJRFwiOjgsXCJJc1BLSURcIjpmYWxzZSxcIm9yZGVyaWRcIjoyNX0se1wiaWRcIjo3MCxcImNhcHRpb25cIjpcIuaooeaLn+mHjy3ph4/nqIvkuIvpmZBcIixcIk5hbWVcIjpcIlRyYW5zTWluXCIsXCJJc0F1dG9JbmNyZW1lbnRcIjpmYWxzZSxcIkNhbk51bGxcIjp0cnVlLFwiZGJUeXBlXCI6XCJkb3VibGVcIixcImxlbmd0aFwiOlwiXCIsXCJUYWJsZUlEXCI6OCxcIklzUEtJRFwiOmZhbHNlLFwib3JkZXJpZFwiOjI2fSx7XCJpZFwiOjcxLFwiY2FwdGlvblwiOlwi5qih5ouf6YePLeS8oOaEn+WZqOmHj+eoi+S4iumZkFwiLFwiTmFtZVwiOlwiU2Vuc29yTWF4XCIsXCJJc0F1dG9JbmNyZW1lbnRcIjpmYWxzZSxcIkNhbk51bGxcIjp0cnVlLFwiZGJUeXBlXCI6XCJkb3VibGVcIixcImxlbmd0aFwiOlwiXCIsXCJUYWJsZUlEXCI6OCxcIklzUEtJRFwiOmZhbHNlLFwib3JkZXJpZFwiOjI3fSx7XCJpZFwiOjcyLFwiY2FwdGlvblwiOlwi5qih5ouf6YePLeS8oOaEn+WZqOmHj+eoi+S4i+mZkFwiLFwiTmFtZVwiOlwiU2Vuc29yTWluXCIsXCJJc0F1dG9JbmNyZW1lbnRcIjpmYWxzZSxcIkNhbk51bGxcIjp0cnVlLFwiZGJUeXBlXCI6XCJ2YXJjaGFyXCIsXCJsZW5ndGhcIjpcIjUwXCIsXCJUYWJsZUlEXCI6OCxcIklzUEtJRFwiOmZhbHNlLFwib3JkZXJpZFwiOjI4fSx7XCJpZFwiOjczLFwiY2FwdGlvblwiOlwi5qih5ouf6YePLeW8gOW5s+aWuVwiLFwiTmFtZVwiOlwiSXNTcXVhcmVcIixcIklzQXV0b0luY3JlbWVudFwiOmZhbHNlLFwiQ2FuTnVsbFwiOnRydWUsXCJkYlR5cGVcIjpcImJpdFwiLFwibGVuZ3RoXCI6XCJcIixcImRlZmF1bHRWYWx1ZVwiOlwiMFwiLFwiVGFibGVJRFwiOjgsXCJJc1BLSURcIjpmYWxzZSxcIm9yZGVyaWRcIjoyOX0se1wiaWRcIjo3NCxcImNhcHRpb25cIjpcIuWIhuautee6v+aAp+WMllwiLFwiTmFtZVwiOlwiSXNMaW5lYXJcIixcIklzQXV0b0luY3JlbWVudFwiOmZhbHNlLFwiQ2FuTnVsbFwiOnRydWUsXCJkYlR5cGVcIjpcImJpdFwiLFwibGVuZ3RoXCI6XCJcIixcImRlZmF1bHRWYWx1ZVwiOlwiMFwiLFwiVGFibGVJRFwiOjgsXCJJc1BLSURcIjpmYWxzZSxcIm9yZGVyaWRcIjozMH0se1wiaWRcIjo3NSxcImNhcHRpb25cIjpcIuWIhuautee6v+aAp+WMllgxXCIsXCJOYW1lXCI6XCJMaW5lYXJYMVwiLFwiSXNBdXRvSW5jcmVtZW50XCI6ZmFsc2UsXCJDYW5OdWxsXCI6dHJ1ZSxcImRiVHlwZVwiOlwiZG91YmxlXCIsXCJsZW5ndGhcIjpcIlwiLFwiVGFibGVJRFwiOjgsXCJJc1BLSURcIjpmYWxzZSxcIm9yZGVyaWRcIjozMX0se1wiaWRcIjo3NixcImNhcHRpb25cIjpcIuWIhuautee6v+aAp+WMllkxXCIsXCJOYW1lXCI6XCJMaW5lYXJZMVwiLFwiSXNBdXRvSW5jcmVtZW50XCI6ZmFsc2UsXCJDYW5OdWxsXCI6dHJ1ZSxcImRiVHlwZVwiOlwiZG91YmxlXCIsXCJsZW5ndGhcIjpcIlwiLFwiVGFibGVJRFwiOjgsXCJJc1BLSURcIjpmYWxzZSxcIm9yZGVyaWRcIjozMn0se1wiaWRcIjo3NyxcImNhcHRpb25cIjpcIuWIhuautee6v+aAp+WMllgyXCIsXCJOYW1lXCI6XCJMaW5lYXJYMlwiLFwiSXNBdXRvSW5jcmVtZW50XCI6ZmFsc2UsXCJDYW5OdWxsXCI6dHJ1ZSxcImRiVHlwZVwiOlwiZG91YmxlXCIsXCJsZW5ndGhcIjpcIlwiLFwiVGFibGVJRFwiOjgsXCJJc1BLSURcIjpmYWxzZSxcIm9yZGVyaWRcIjozM30se1wiaWRcIjo3OCxcImNhcHRpb25cIjpcIuWIhuautee6v+aAp+WMllkyXCIsXCJOYW1lXCI6XCJMaW5lYXJZMlwiLFwiSXNBdXRvSW5jcmVtZW50XCI6ZmFsc2UsXCJDYW5OdWxsXCI6dHJ1ZSxcImRiVHlwZVwiOlwiZG91YmxlXCIsXCJsZW5ndGhcIjpcIlwiLFwiVGFibGVJRFwiOjgsXCJJc1BLSURcIjpmYWxzZSxcIm9yZGVyaWRcIjozNH0se1wiaWRcIjo3OSxcIk5hbWVcIjpcIkxpbmVhclgzXCIsXCJJc0F1dG9JbmNyZW1lbnRcIjpmYWxzZSxcIkNhbk51bGxcIjp0cnVlLFwiZGJUeXBlXCI6XCJkb3VibGVcIixcImxlbmd0aFwiOlwiXCIsXCJUYWJsZUlEXCI6OCxcIklzUEtJRFwiOmZhbHNlLFwib3JkZXJpZFwiOjM1fSx7XCJpZFwiOjgwLFwiTmFtZVwiOlwiTGluZWFyWTNcIixcIklzQXV0b0luY3JlbWVudFwiOmZhbHNlLFwiQ2FuTnVsbFwiOnRydWUsXCJkYlR5cGVcIjpcImRvdWJsZVwiLFwibGVuZ3RoXCI6XCJcIixcIlRhYmxlSURcIjo4LFwiSXNQS0lEXCI6ZmFsc2UsXCJvcmRlcmlkXCI6MzZ9LHtcImlkXCI6ODEsXCJOYW1lXCI6XCJMaW5lYXJYNFwiLFwiSXNBdXRvSW5jcmVtZW50XCI6ZmFsc2UsXCJDYW5OdWxsXCI6dHJ1ZSxcImRiVHlwZVwiOlwiZG91YmxlXCIsXCJsZW5ndGhcIjpcIlwiLFwiVGFibGVJRFwiOjgsXCJJc1BLSURcIjpmYWxzZSxcIm9yZGVyaWRcIjozN30se1wiaWRcIjo4MixcImNhcHRpb25cIjpcIuaKpeitpuWAvDJcIixcIk5hbWVcIjpcIkFsYXJtVmFsdWUyXCIsXCJJc0F1dG9JbmNyZW1lbnRcIjpmYWxzZSxcIkNhbk51bGxcIjp0cnVlLFwiZGJUeXBlXCI6XCJkb3VibGVcIixcImxlbmd0aFwiOlwiXCIsXCJUYWJsZUlEXCI6OCxcIklzUEtJRFwiOmZhbHNlLFwib3JkZXJpZFwiOjM4fSx7XCJpZFwiOjgzLFwiY2FwdGlvblwiOlwi5oql6K2m5LyY5YWI57qnMlwiLFwiTmFtZVwiOlwiQWxhcm1Qcmlvcml0eTJcIixcIklzQXV0b0luY3JlbWVudFwiOmZhbHNlLFwiQ2FuTnVsbFwiOnRydWUsXCJkYlR5cGVcIjpcImludFwiLFwibGVuZ3RoXCI6XCJcIixcIlRhYmxlSURcIjo4LFwiSXNQS0lEXCI6ZmFsc2UsXCJvcmRlcmlkXCI6Mzl9LHtcImlkXCI6ODQsXCJOYW1lXCI6XCJBbGFybVZhbHVlM1wiLFwiSXNBdXRvSW5jcmVtZW50XCI6ZmFsc2UsXCJDYW5OdWxsXCI6dHJ1ZSxcImRiVHlwZVwiOlwiZG91YmxlXCIsXCJsZW5ndGhcIjpcIlwiLFwiVGFibGVJRFwiOjgsXCJJc1BLSURcIjpmYWxzZSxcIm9yZGVyaWRcIjo0MH0se1wiaWRcIjo4NSxcIk5hbWVcIjpcIkFsYXJtUHJpb3JpdHkzXCIsXCJJc0F1dG9JbmNyZW1lbnRcIjpmYWxzZSxcIkNhbk51bGxcIjp0cnVlLFwiZGJUeXBlXCI6XCJpbnRcIixcImxlbmd0aFwiOlwiXCIsXCJUYWJsZUlEXCI6OCxcIklzUEtJRFwiOmZhbHNlLFwib3JkZXJpZFwiOjQxfSx7XCJpZFwiOjg2LFwiTmFtZVwiOlwiQWxhcm1WYWx1ZTRcIixcIklzQXV0b0luY3JlbWVudFwiOmZhbHNlLFwiQ2FuTnVsbFwiOnRydWUsXCJkYlR5cGVcIjpcImRvdWJsZVwiLFwibGVuZ3RoXCI6XCJcIixcIlRhYmxlSURcIjo4LFwiSXNQS0lEXCI6ZmFsc2UsXCJvcmRlcmlkXCI6NDJ9LHtcImlkXCI6ODcsXCJOYW1lXCI6XCJBbGFybVByaW9yaXR5NFwiLFwiSXNBdXRvSW5jcmVtZW50XCI6ZmFsc2UsXCJDYW5OdWxsXCI6dHJ1ZSxcImRiVHlwZVwiOlwiaW50XCIsXCJsZW5ndGhcIjpcIlwiLFwiVGFibGVJRFwiOjgsXCJJc1BLSURcIjpmYWxzZSxcIm9yZGVyaWRcIjo0M30se1wiaWRcIjo4OCxcIk5hbWVcIjpcIkFsYXJtVmFsdWU1XCIsXCJJc0F1dG9JbmNyZW1lbnRcIjpmYWxzZSxcIkNhbk51bGxcIjp0cnVlLFwiZGJUeXBlXCI6XCJkb3VibGVcIixcImxlbmd0aFwiOlwiXCIsXCJUYWJsZUlEXCI6OCxcIklzUEtJRFwiOmZhbHNlLFwib3JkZXJpZFwiOjQ0fSx7XCJpZFwiOjg5LFwiTmFtZVwiOlwiQWxhcm1Qcmlvcml0eTVcIixcIklzQXV0b0luY3JlbWVudFwiOmZhbHNlLFwiQ2FuTnVsbFwiOnRydWUsXCJkYlR5cGVcIjpcImludFwiLFwibGVuZ3RoXCI6XCJcIixcIlRhYmxlSURcIjo4LFwiSXNQS0lEXCI6ZmFsc2UsXCJvcmRlcmlkXCI6NDV9LHtcImlkXCI6OTAsXCJjYXB0aW9uXCI6XCLlgY/lt67miqXoraZcIixcIk5hbWVcIjpcIklzQWxhcm1PZmZzZXRcIixcIklzQXV0b0luY3JlbWVudFwiOmZhbHNlLFwiQ2FuTnVsbFwiOnRydWUsXCJkYlR5cGVcIjpcImJpdFwiLFwibGVuZ3RoXCI6XCJcIixcImRlZmF1bHRWYWx1ZVwiOlwiMFwiLFwiVGFibGVJRFwiOjgsXCJJc1BLSURcIjpmYWxzZSxcIm9yZGVyaWRcIjo0Nn0se1wiaWRcIjo5MSxcImNhcHRpb25cIjpcIuWBj+W3ruaKpeitpuiuvuWumuWAvFwiLFwiTmFtZVwiOlwiQWxhcm1PZmZzZXRPcmlnaW5hbFZhbHVlXCIsXCJJc0F1dG9JbmNyZW1lbnRcIjpmYWxzZSxcIkNhbk51bGxcIjp0cnVlLFwiZGJUeXBlXCI6XCJkb3VibGVcIixcImxlbmd0aFwiOlwiXCIsXCJUYWJsZUlEXCI6OCxcIklzUEtJRFwiOmZhbHNlLFwib3JkZXJpZFwiOjQ3fSx7XCJpZFwiOjkyLFwiY2FwdGlvblwiOlwi5YGP5beu5YC8XCIsXCJOYW1lXCI6XCJBbGFybU9mZnNldFZhbHVlXCIsXCJJc0F1dG9JbmNyZW1lbnRcIjpmYWxzZSxcIkNhbk51bGxcIjp0cnVlLFwiZGJUeXBlXCI6XCJkb3VibGVcIixcImxlbmd0aFwiOlwiXCIsXCJUYWJsZUlEXCI6OCxcIklzUEtJRFwiOmZhbHNlLFwib3JkZXJpZFwiOjQ4fSx7XCJpZFwiOjkzLFwiY2FwdGlvblwiOlwi5YGP5beu5oql6K2m5LyY5YWI57qnXCIsXCJOYW1lXCI6XCJBbGFybU9mZnNldFByaW9yaXR5XCIsXCJJc0F1dG9JbmNyZW1lbnRcIjpmYWxzZSxcIkNhbk51bGxcIjp0cnVlLFwiZGJUeXBlXCI6XCJpbnRcIixcImxlbmd0aFwiOlwiXCIsXCJUYWJsZUlEXCI6OCxcIklzUEtJRFwiOmZhbHNlLFwib3JkZXJpZFwiOjQ5fSx7XCJpZFwiOjk0LFwiY2FwdGlvblwiOlwi5Y+Y5YyW546H5oql6K2mXCIsXCJOYW1lXCI6XCJJc0FsYXJtUGVyY2VudFwiLFwiSXNBdXRvSW5jcmVtZW50XCI6ZmFsc2UsXCJDYW5OdWxsXCI6dHJ1ZSxcImRiVHlwZVwiOlwiYml0XCIsXCJsZW5ndGhcIjpcIlwiLFwiZGVmYXVsdFZhbHVlXCI6XCIwXCIsXCJUYWJsZUlEXCI6OCxcIklzUEtJRFwiOmZhbHNlLFwib3JkZXJpZFwiOjUwfSx7XCJpZFwiOjk1LFwiY2FwdGlvblwiOlwi5Y+Y5YyW546H5YC877yI55m+5YiG5q+U77yJXCIsXCJOYW1lXCI6XCJQZXJjZW50XCIsXCJJc0F1dG9JbmNyZW1lbnRcIjpmYWxzZSxcIkNhbk51bGxcIjp0cnVlLFwiZGJUeXBlXCI6XCJkb3VibGVcIixcImxlbmd0aFwiOlwiXCIsXCJUYWJsZUlEXCI6OCxcIklzUEtJRFwiOmZhbHNlLFwib3JkZXJpZFwiOjUxfSx7XCJpZFwiOjk2LFwiY2FwdGlvblwiOlwi5Y+Y5YyW546H5ZGo5pyf77yI56eS77yJXCIsXCJOYW1lXCI6XCJDaGFuZ2VDeWNsZVwiLFwiSXNBdXRvSW5jcmVtZW50XCI6ZmFsc2UsXCJDYW5OdWxsXCI6dHJ1ZSxcImRiVHlwZVwiOlwiaW50XCIsXCJsZW5ndGhcIjpcIlwiLFwiVGFibGVJRFwiOjgsXCJJc1BLSURcIjpmYWxzZSxcIm9yZGVyaWRcIjo1Mn0se1wiaWRcIjo5NyxcImNhcHRpb25cIjpcIuWPmOWMlueOh+aKpeitpuS8mOWFiOe6p1wiLFwiTmFtZVwiOlwiQWxhcm1QZXJjZW50UHJpb3JpdHlcIixcIklzQXV0b0luY3JlbWVudFwiOmZhbHNlLFwiQ2FuTnVsbFwiOnRydWUsXCJkYlR5cGVcIjpcImludFwiLFwibGVuZ3RoXCI6XCJcIixcIlRhYmxlSURcIjo4LFwiSXNQS0lEXCI6ZmFsc2UsXCJvcmRlcmlkXCI6NTN9LHtcImlkXCI6MTAzLFwiTmFtZVwiOlwiRm9sZGVySWRcIixcIklzQXV0b0luY3JlbWVudFwiOmZhbHNlLFwiQ2FuTnVsbFwiOnRydWUsXCJkYlR5cGVcIjpcImludFwiLFwibGVuZ3RoXCI6XCJcIixcImRlZmF1bHRWYWx1ZVwiOlwiMFwiLFwiVGFibGVJRFwiOjgsXCJJc1BLSURcIjpmYWxzZSxcIm9yZGVyaWRcIjo2fV0sXCJuZXdDb2x1bW5zXCI6W3tcImlkXCI6MTA5LFwiTmFtZVwiOlwiRGV2aWNlSWRcIixcIklzQXV0b0luY3JlbWVudFwiOmZhbHNlLFwiQ2FuTnVsbFwiOnRydWUsXCJkYlR5cGVcIjpcImludFwiLFwibGVuZ3RoXCI6XCJcIixcIlRhYmxlSURcIjo4LFwiSXNQS0lEXCI6ZmFsc2UsXCJvcmRlcmlkXCI6N31dLFwiY2hhbmdlZENvbHVtbnNcIjpbXSxcImRlbGV0ZWRDb2x1bW5zXCI6W10sXCJJRFhDb25maWdzXCI6W10sXCJJRFwiOjB9In0seyJOYW1lIjoiZGF0YWJhc2VpZCIsIlZhbHVlIjoyfV0sIlJvd1N0YXRlIjowfSx7Ikl0ZW1zIjpbeyJOYW1lIjoiaWQiLCJWYWx1ZSI6NTF9LHsiTmFtZSI6InR5cGUiLCJWYWx1ZSI6IkNoYW5nZVRhYmxlQWN0aW9uIn0seyJOYW1lIjoiY29udGVudCIsIlZhbHVlIjoie1wiT2xkVGFibGVOYW1lXCI6XCJEZXZpY2VQb2ludFwiLFwiTmV3VGFibGVOYW1lXCI6XCJEZXZpY2VQb2ludFwiLFwib3RoZXJDb2x1bW5zXCI6W3tcImlkXCI6NDIsXCJOYW1lXCI6XCJpZFwiLFwiSXNBdXRvSW5jcmVtZW50XCI6dHJ1ZSxcIkNhbk51bGxcIjpmYWxzZSxcImRiVHlwZVwiOlwiaW50XCIsXCJUYWJsZUlEXCI6OCxcIklzUEtJRFwiOnRydWUsXCJvcmRlcmlkXCI6MH0se1wiaWRcIjo0NCxcIk5hbWVcIjpcIkFkZHJlc3NcIixcIklzQXV0b0luY3JlbWVudFwiOmZhbHNlLFwiQ2FuTnVsbFwiOnRydWUsXCJkYlR5cGVcIjpcInZhcmNoYXJcIixcImxlbmd0aFwiOlwiMjAwXCIsXCJUYWJsZUlEXCI6OCxcIklzUEtJRFwiOmZhbHNlLFwib3JkZXJpZFwiOjN9LHtcImlkXCI6NDUsXCJjYXB0aW9uXCI6XCLmmK/lkKbnm5HmtYvlj5jljJZcIixcIk5hbWVcIjpcIklzV2F0Y2hpbmdcIixcIklzQXV0b0luY3JlbWVudFwiOmZhbHNlLFwiQ2FuTnVsbFwiOnRydWUsXCJkYlR5cGVcIjpcImJpdFwiLFwibGVuZ3RoXCI6XCJcIixcImRlZmF1bHRWYWx1ZVwiOlwiMFwiLFwiVGFibGVJRFwiOjgsXCJJc1BLSURcIjpmYWxzZSxcIm9yZGVyaWRcIjo1NX0se1wiaWRcIjo0OCxcImNhcHRpb25cIjpcIuWcsOWdgOiuvue9ruaXtueahGpzb27lr7nosaHooajovr5cIixcIk5hbWVcIjpcIkFkZHJTZXR0aW5nXCIsXCJJc0F1dG9JbmNyZW1lbnRcIjpmYWxzZSxcIkNhbk51bGxcIjp0cnVlLFwiZGJUeXBlXCI6XCJ2YXJjaGFyXCIsXCJsZW5ndGhcIjpcIjIwMFwiLFwiVGFibGVJRFwiOjgsXCJJc1BLSURcIjpmYWxzZSxcIm9yZGVyaWRcIjo0fSx7XCJpZFwiOjQ5LFwiY2FwdGlvblwiOlwi5o+P6L+wXCIsXCJOYW1lXCI6XCJEZXNjXCIsXCJJc0F1dG9JbmNyZW1lbnRcIjpmYWxzZSxcIkNhbk51bGxcIjp0cnVlLFwiZGJUeXBlXCI6XCJ2YXJjaGFyXCIsXCJsZW5ndGhcIjpcIjEwMFwiLFwiVGFibGVJRFwiOjgsXCJJc1BLSURcIjpmYWxzZSxcIm9yZGVyaWRcIjoyfSx7XCJpZFwiOjUwLFwiY2FwdGlvblwiOlwi5Yid5aeL5YC8XCIsXCJOYW1lXCI6XCJJbml0VmFsdWVcIixcIklzQXV0b0luY3JlbWVudFwiOmZhbHNlLFwiQ2FuTnVsbFwiOnRydWUsXCJkYlR5cGVcIjpcImRvdWJsZVwiLFwibGVuZ3RoXCI6XCJcIixcIlRhYmxlSURcIjo4LFwiSXNQS0lEXCI6ZmFsc2UsXCJvcmRlcmlkXCI6OH0se1wiaWRcIjo1MSxcImNhcHRpb25cIjpcIuW8gOeKtuaAgeS/oeaBr1wiLFwiTmFtZVwiOlwiU3RhdHVzT3BlbkluZm9cIixcIklzQXV0b0luY3JlbWVudFwiOmZhbHNlLFwiQ2FuTnVsbFwiOnRydWUsXCJkYlR5cGVcIjpcInZhcmNoYXJcIixcImxlbmd0aFwiOlwiNTBcIixcIlRhYmxlSURcIjo4LFwiSXNQS0lEXCI6ZmFsc2UsXCJvcmRlcmlkXCI6OX0se1wiaWRcIjo1MixcImNhcHRpb25cIjpcIuWFs+eKtuaAgeS/oeaBr1wiLFwiTmFtZVwiOlwiU3RhdGVDbG9zZUluZm9cIixcIklzQXV0b0luY3JlbWVudFwiOmZhbHNlLFwiQ2FuTnVsbFwiOnRydWUsXCJkYlR5cGVcIjpcInZhcmNoYXJcIixcImxlbmd0aFwiOlwiNTBcIixcIlRhYmxlSURcIjo4LFwiSXNQS0lEXCI6ZmFsc2UsXCJvcmRlcmlkXCI6MTB9LHtcImlkXCI6NTMsXCJjYXB0aW9uXCI6XCLmiqXorablvIDlhbNcIixcIk5hbWVcIjpcIklzQWxhcm1cIixcIklzQXV0b0luY3JlbWVudFwiOmZhbHNlLFwiQ2FuTnVsbFwiOnRydWUsXCJkYlR5cGVcIjpcImJpdFwiLFwibGVuZ3RoXCI6XCJcIixcImRlZmF1bHRWYWx1ZVwiOlwiMFwiLFwiVGFibGVJRFwiOjgsXCJJc1BLSURcIjpmYWxzZSxcIm9yZGVyaWRcIjoxMX0se1wiaWRcIjo1NCxcImNhcHRpb25cIjpcIuaKpeitpuWAvFwiLFwiTmFtZVwiOlwiQWxhcm1WYWx1ZVwiLFwiSXNBdXRvSW5jcmVtZW50XCI6ZmFsc2UsXCJDYW5OdWxsXCI6dHJ1ZSxcImRiVHlwZVwiOlwiZG91YmxlXCIsXCJsZW5ndGhcIjpcIlwiLFwiVGFibGVJRFwiOjgsXCJJc1BLSURcIjpmYWxzZSxcIm9yZGVyaWRcIjoxMn0se1wiaWRcIjo1NSxcImNhcHRpb25cIjpcIuaKpeitpuS8mOWFiOe6p1wiLFwiTmFtZVwiOlwiQWxhcm1Qcmlvcml0eVwiLFwiSXNBdXRvSW5jcmVtZW50XCI6ZmFsc2UsXCJDYW5OdWxsXCI6dHJ1ZSxcImRiVHlwZVwiOlwiaW50XCIsXCJsZW5ndGhcIjpcIlwiLFwiVGFibGVJRFwiOjgsXCJJc1BLSURcIjpmYWxzZSxcIm9yZGVyaWRcIjoxM30se1wiaWRcIjo1NixcImNhcHRpb25cIjpcIuaKpeitpue7hFwiLFwiTmFtZVwiOlwiQWxhcm1Hcm91cFwiLFwiSXNBdXRvSW5jcmVtZW50XCI6ZmFsc2UsXCJDYW5OdWxsXCI6dHJ1ZSxcImRiVHlwZVwiOlwidmFyY2hhclwiLFwibGVuZ3RoXCI6XCI1MFwiLFwiVGFibGVJRFwiOjgsXCJJc1BLSURcIjpmYWxzZSxcIm9yZGVyaWRcIjoxNH0se1wiaWRcIjo1NyxcImNhcHRpb25cIjpcIuaKpeitpuiHquWKqOehruiupFwiLFwiTmFtZVwiOlwiQWxhcm1BdXRvQ29uZmlybVwiLFwiSXNBdXRvSW5jcmVtZW50XCI6ZmFsc2UsXCJDYW5OdWxsXCI6dHJ1ZSxcImRiVHlwZVwiOlwiYml0XCIsXCJsZW5ndGhcIjpcIlwiLFwiZGVmYXVsdFZhbHVlXCI6XCIwXCIsXCJUYWJsZUlEXCI6OCxcIklzUEtJRFwiOmZhbHNlLFwib3JkZXJpZFwiOjE1fSx7XCJpZFwiOjU4LFwiY2FwdGlvblwiOlwi5oql6K2m6Ieq5Yqo5aSN5L2NXCIsXCJOYW1lXCI6XCJBbGFybUF1dG9SZXNldFwiLFwiSXNBdXRvSW5jcmVtZW50XCI6ZmFsc2UsXCJDYW5OdWxsXCI6dHJ1ZSxcImRiVHlwZVwiOlwiYml0XCIsXCJsZW5ndGhcIjpcIlwiLFwiZGVmYXVsdFZhbHVlXCI6XCIwXCIsXCJUYWJsZUlEXCI6OCxcIklzUEtJRFwiOmZhbHNlLFwib3JkZXJpZFwiOjE2fSx7XCJpZFwiOjU5LFwiY2FwdGlvblwiOlwi5pWw5o2u57ud5a+55Y+Y5YyW5L+d5a2YXCIsXCJOYW1lXCI6XCJWYWx1ZUFic29sdXRlQ2hhbmdlXCIsXCJJc0F1dG9JbmNyZW1lbnRcIjpmYWxzZSxcIkNhbk51bGxcIjp0cnVlLFwiZGJUeXBlXCI6XCJiaXRcIixcImxlbmd0aFwiOlwiXCIsXCJkZWZhdWx0VmFsdWVcIjpcIjBcIixcIlRhYmxlSURcIjo4LFwiSXNQS0lEXCI6ZmFsc2UsXCJvcmRlcmlkXCI6MTd9LHtcImlkXCI6NjAsXCJjYXB0aW9uXCI6XCLmlbDmja7nu53lr7nlj5jljJblgLxcIixcIk5hbWVcIjpcIlZhbHVlQWJzb2x1dGVDaGFuZ2VTZXR0aW5nXCIsXCJJc0F1dG9JbmNyZW1lbnRcIjpmYWxzZSxcIkNhbk51bGxcIjp0cnVlLFwiZGJUeXBlXCI6XCJkb3VibGVcIixcImxlbmd0aFwiOlwiXCIsXCJUYWJsZUlEXCI6OCxcIklzUEtJRFwiOmZhbHNlLFwib3JkZXJpZFwiOjE4fSx7XCJpZFwiOjYxLFwiY2FwdGlvblwiOlwi5pWw5o2u55u45a+55Y+Y5YyW5L+d5a2YXCIsXCJOYW1lXCI6XCJWYWx1ZVJlbGF0aXZlQ2hhbmdlXCIsXCJJc0F1dG9JbmNyZW1lbnRcIjpmYWxzZSxcIkNhbk51bGxcIjp0cnVlLFwiZGJUeXBlXCI6XCJiaXRcIixcImxlbmd0aFwiOlwiXCIsXCJkZWZhdWx0VmFsdWVcIjpcIjBcIixcIlRhYmxlSURcIjo4LFwiSXNQS0lEXCI6ZmFsc2UsXCJvcmRlcmlkXCI6MTl9LHtcImlkXCI6NjIsXCJjYXB0aW9uXCI6XCLmlbDmja7nm7jlr7nlj5jljJblgLxcIixcIk5hbWVcIjpcIlZhbHVlUmVsYXRpdmVDaGFuZ2VTZXR0aW5nXCIsXCJJc0F1dG9JbmNyZW1lbnRcIjpmYWxzZSxcIkNhbk51bGxcIjp0cnVlLFwiZGJUeXBlXCI6XCJkb3VibGVcIixcImxlbmd0aFwiOlwiXCIsXCJUYWJsZUlEXCI6OCxcIklzUEtJRFwiOmZhbHNlLFwib3JkZXJpZFwiOjIwfSx7XCJpZFwiOjYzLFwiY2FwdGlvblwiOlwi5pWw5o2u5a6a5pe25L+d5a2YXCIsXCJOYW1lXCI6XCJWYWx1ZU9uVGltZUNoYW5nZVwiLFwiSXNBdXRvSW5jcmVtZW50XCI6ZmFsc2UsXCJDYW5OdWxsXCI6dHJ1ZSxcImRiVHlwZVwiOlwiYml0XCIsXCJsZW5ndGhcIjpcIlwiLFwiZGVmYXVsdFZhbHVlXCI6XCIwXCIsXCJUYWJsZUlEXCI6OCxcIklzUEtJRFwiOmZhbHNlLFwib3JkZXJpZFwiOjIxfSx7XCJpZFwiOjY0LFwiY2FwdGlvblwiOlwi5pWw5o2u5a6a5pe25YC8XCIsXCJOYW1lXCI6XCJWYWx1ZU9uVGltZUNoYW5nZVNldHRpbmdcIixcIklzQXV0b0luY3JlbWVudFwiOmZhbHNlLFwiQ2FuTnVsbFwiOnRydWUsXCJkYlR5cGVcIjpcImRvdWJsZVwiLFwibGVuZ3RoXCI6XCJcIixcIlRhYmxlSURcIjo4LFwiSXNQS0lEXCI6ZmFsc2UsXCJvcmRlcmlkXCI6MjJ9LHtcImlkXCI6NjUsXCJjYXB0aW9uXCI6XCLngrnnsbvlnotcIixcIk5hbWVcIjpcIlR5cGVcIixcIklzQXV0b0luY3JlbWVudFwiOmZhbHNlLFwiQ2FuTnVsbFwiOnRydWUsXCJkYlR5cGVcIjpcImludFwiLFwiRW51bURlZmluZVwiOlwiQW5hbG9nID0gMSxcXG5EaWdpdGFsID0gMlwiLFwibGVuZ3RoXCI6XCJcIixcImRlZmF1bHRWYWx1ZVwiOlwiMlwiLFwiVGFibGVJRFwiOjgsXCJJc1BLSURcIjpmYWxzZSxcIm9yZGVyaWRcIjo1fSx7XCJpZFwiOjY2LFwiY2FwdGlvblwiOlwi5qih5ouf6YePLeW3peeoi+WNleS9jVwiLFwiTmFtZVwiOlwiVW5pdFwiLFwiSXNBdXRvSW5jcmVtZW50XCI6ZmFsc2UsXCJDYW5OdWxsXCI6dHJ1ZSxcImRiVHlwZVwiOlwidmFyY2hhclwiLFwibGVuZ3RoXCI6XCI1MFwiLFwiVGFibGVJRFwiOjgsXCJJc1BLSURcIjpmYWxzZSxcIm9yZGVyaWRcIjoyM30se1wiaWRcIjo2NyxcImNhcHRpb25cIjpcIuaooeaLn+mHjy3lsI/mlbDngrnkvY3mlbBcIixcIk5hbWVcIjpcIkRQQ291bnRcIixcIklzQXV0b0luY3JlbWVudFwiOmZhbHNlLFwiQ2FuTnVsbFwiOnRydWUsXCJkYlR5cGVcIjpcInZhcmNoYXJcIixcImxlbmd0aFwiOlwiNTBcIixcIlRhYmxlSURcIjo4LFwiSXNQS0lEXCI6ZmFsc2UsXCJvcmRlcmlkXCI6MjR9LHtcImlkXCI6NjgsXCJjYXB0aW9uXCI6XCLmqKHmi5/ph48t6YeP56iL6L2s5YyW5byA5YWzXCIsXCJOYW1lXCI6XCJJc1RyYW5zZm9ybVwiLFwiSXNBdXRvSW5jcmVtZW50XCI6ZmFsc2UsXCJDYW5OdWxsXCI6dHJ1ZSxcImRiVHlwZVwiOlwiYml0XCIsXCJsZW5ndGhcIjpcIlwiLFwiZGVmYXVsdFZhbHVlXCI6XCIwXCIsXCJUYWJsZUlEXCI6OCxcIklzUEtJRFwiOmZhbHNlLFwib3JkZXJpZFwiOjI1fSx7XCJpZFwiOjY5LFwiY2FwdGlvblwiOlwi5qih5ouf6YePLemHj+eoi+S4iumZkFwiLFwiTmFtZVwiOlwiVHJhbnNNYXhcIixcIklzQXV0b0luY3JlbWVudFwiOmZhbHNlLFwiQ2FuTnVsbFwiOnRydWUsXCJkYlR5cGVcIjpcImRvdWJsZVwiLFwibGVuZ3RoXCI6XCJcIixcIlRhYmxlSURcIjo4LFwiSXNQS0lEXCI6ZmFsc2UsXCJvcmRlcmlkXCI6MjZ9LHtcImlkXCI6NzAsXCJjYXB0aW9uXCI6XCLmqKHmi5/ph48t6YeP56iL5LiL6ZmQXCIsXCJOYW1lXCI6XCJUcmFuc01pblwiLFwiSXNBdXRvSW5jcmVtZW50XCI6ZmFsc2UsXCJDYW5OdWxsXCI6dHJ1ZSxcImRiVHlwZVwiOlwiZG91YmxlXCIsXCJsZW5ndGhcIjpcIlwiLFwiVGFibGVJRFwiOjgsXCJJc1BLSURcIjpmYWxzZSxcIm9yZGVyaWRcIjoyN30se1wiaWRcIjo3MSxcImNhcHRpb25cIjpcIuaooeaLn+mHjy3kvKDmhJ/lmajph4/nqIvkuIrpmZBcIixcIk5hbWVcIjpcIlNlbnNvck1heFwiLFwiSXNBdXRvSW5jcmVtZW50XCI6ZmFsc2UsXCJDYW5OdWxsXCI6dHJ1ZSxcImRiVHlwZVwiOlwiZG91YmxlXCIsXCJsZW5ndGhcIjpcIlwiLFwiVGFibGVJRFwiOjgsXCJJc1BLSURcIjpmYWxzZSxcIm9yZGVyaWRcIjoyOH0se1wiaWRcIjo3MixcImNhcHRpb25cIjpcIuaooeaLn+mHjy3kvKDmhJ/lmajph4/nqIvkuIvpmZBcIixcIk5hbWVcIjpcIlNlbnNvck1pblwiLFwiSXNBdXRvSW5jcmVtZW50XCI6ZmFsc2UsXCJDYW5OdWxsXCI6dHJ1ZSxcImRiVHlwZVwiOlwidmFyY2hhclwiLFwibGVuZ3RoXCI6XCI1MFwiLFwiVGFibGVJRFwiOjgsXCJJc1BLSURcIjpmYWxzZSxcIm9yZGVyaWRcIjoyOX0se1wiaWRcIjo3MyxcImNhcHRpb25cIjpcIuaooeaLn+mHjy3lvIDlubPmlrlcIixcIk5hbWVcIjpcIklzU3F1YXJlXCIsXCJJc0F1dG9JbmNyZW1lbnRcIjpmYWxzZSxcIkNhbk51bGxcIjp0cnVlLFwiZGJUeXBlXCI6XCJiaXRcIixcImxlbmd0aFwiOlwiXCIsXCJkZWZhdWx0VmFsdWVcIjpcIjBcIixcIlRhYmxlSURcIjo4LFwiSXNQS0lEXCI6ZmFsc2UsXCJvcmRlcmlkXCI6MzB9LHtcImlkXCI6NzQsXCJjYXB0aW9uXCI6XCLliIbmrrXnur/mgKfljJZcIixcIk5hbWVcIjpcIklzTGluZWFyXCIsXCJJc0F1dG9JbmNyZW1lbnRcIjpmYWxzZSxcIkNhbk51bGxcIjp0cnVlLFwiZGJUeXBlXCI6XCJiaXRcIixcImxlbmd0aFwiOlwiXCIsXCJkZWZhdWx0VmFsdWVcIjpcIjBcIixcIlRhYmxlSURcIjo4LFwiSXNQS0lEXCI6ZmFsc2UsXCJvcmRlcmlkXCI6MzF9LHtcImlkXCI6NzUsXCJjYXB0aW9uXCI6XCLliIbmrrXnur/mgKfljJZYMVwiLFwiTmFtZVwiOlwiTGluZWFyWDFcIixcIklzQXV0b0luY3JlbWVudFwiOmZhbHNlLFwiQ2FuTnVsbFwiOnRydWUsXCJkYlR5cGVcIjpcImRvdWJsZVwiLFwibGVuZ3RoXCI6XCJcIixcIlRhYmxlSURcIjo4LFwiSXNQS0lEXCI6ZmFsc2UsXCJvcmRlcmlkXCI6MzJ9LHtcImlkXCI6NzYsXCJjYXB0aW9uXCI6XCLliIbmrrXnur/mgKfljJZZMVwiLFwiTmFtZVwiOlwiTGluZWFyWTFcIixcIklzQXV0b0luY3JlbWVudFwiOmZhbHNlLFwiQ2FuTnVsbFwiOnRydWUsXCJkYlR5cGVcIjpcImRvdWJsZVwiLFwibGVuZ3RoXCI6XCJcIixcIlRhYmxlSURcIjo4LFwiSXNQS0lEXCI6ZmFsc2UsXCJvcmRlcmlkXCI6MzN9LHtcImlkXCI6NzcsXCJjYXB0aW9uXCI6XCLliIbmrrXnur/mgKfljJZYMlwiLFwiTmFtZVwiOlwiTGluZWFyWDJcIixcIklzQXV0b0luY3JlbWVudFwiOmZhbHNlLFwiQ2FuTnVsbFwiOnRydWUsXCJkYlR5cGVcIjpcImRvdWJsZVwiLFwibGVuZ3RoXCI6XCJcIixcIlRhYmxlSURcIjo4LFwiSXNQS0lEXCI6ZmFsc2UsXCJvcmRlcmlkXCI6MzR9LHtcImlkXCI6NzgsXCJjYXB0aW9uXCI6XCLliIbmrrXnur/mgKfljJZZMlwiLFwiTmFtZVwiOlwiTGluZWFyWTJcIixcIklzQXV0b0luY3JlbWVudFwiOmZhbHNlLFwiQ2FuTnVsbFwiOnRydWUsXCJkYlR5cGVcIjpcImRvdWJsZVwiLFwibGVuZ3RoXCI6XCJcIixcIlRhYmxlSURcIjo4LFwiSXNQS0lEXCI6ZmFsc2UsXCJvcmRlcmlkXCI6MzV9LHtcImlkXCI6NzksXCJOYW1lXCI6XCJMaW5lYXJYM1wiLFwiSXNBdXRvSW5jcmVtZW50XCI6ZmFsc2UsXCJDYW5OdWxsXCI6dHJ1ZSxcImRiVHlwZVwiOlwiZG91YmxlXCIsXCJsZW5ndGhcIjpcIlwiLFwiVGFibGVJRFwiOjgsXCJJc1BLSURcIjpmYWxzZSxcIm9yZGVyaWRcIjozNn0se1wiaWRcIjo4MCxcIk5hbWVcIjpcIkxpbmVhclkzXCIsXCJJc0F1dG9JbmNyZW1lbnRcIjpmYWxzZSxcIkNhbk51bGxcIjp0cnVlLFwiZGJUeXBlXCI6XCJkb3VibGVcIixcImxlbmd0aFwiOlwiXCIsXCJUYWJsZUlEXCI6OCxcIklzUEtJRFwiOmZhbHNlLFwib3JkZXJpZFwiOjM3fSx7XCJpZFwiOjgxLFwiTmFtZVwiOlwiTGluZWFyWDRcIixcIklzQXV0b0luY3JlbWVudFwiOmZhbHNlLFwiQ2FuTnVsbFwiOnRydWUsXCJkYlR5cGVcIjpcImRvdWJsZVwiLFwibGVuZ3RoXCI6XCJcIixcIlRhYmxlSURcIjo4LFwiSXNQS0lEXCI6ZmFsc2UsXCJvcmRlcmlkXCI6Mzh9LHtcImlkXCI6ODIsXCJjYXB0aW9uXCI6XCLmiqXorablgLwyXCIsXCJOYW1lXCI6XCJBbGFybVZhbHVlMlwiLFwiSXNBdXRvSW5jcmVtZW50XCI6ZmFsc2UsXCJDYW5OdWxsXCI6dHJ1ZSxcImRiVHlwZVwiOlwiZG91YmxlXCIsXCJsZW5ndGhcIjpcIlwiLFwiVGFibGVJRFwiOjgsXCJJc1BLSURcIjpmYWxzZSxcIm9yZGVyaWRcIjozOX0se1wiaWRcIjo4MyxcImNhcHRpb25cIjpcIuaKpeitpuS8mOWFiOe6pzJcIixcIk5hbWVcIjpcIkFsYXJtUHJpb3JpdHkyXCIsXCJJc0F1dG9JbmNyZW1lbnRcIjpmYWxzZSxcIkNhbk51bGxcIjp0cnVlLFwiZGJUeXBlXCI6XCJpbnRcIixcImxlbmd0aFwiOlwiXCIsXCJUYWJsZUlEXCI6OCxcIklzUEtJRFwiOmZhbHNlLFwib3JkZXJpZFwiOjQwfSx7XCJpZFwiOjg0LFwiTmFtZVwiOlwiQWxhcm1WYWx1ZTNcIixcIklzQXV0b0luY3JlbWVudFwiOmZhbHNlLFwiQ2FuTnVsbFwiOnRydWUsXCJkYlR5cGVcIjpcImRvdWJsZVwiLFwibGVuZ3RoXCI6XCJcIixcIlRhYmxlSURcIjo4LFwiSXNQS0lEXCI6ZmFsc2UsXCJvcmRlcmlkXCI6NDF9LHtcImlkXCI6ODUsXCJOYW1lXCI6XCJBbGFybVByaW9yaXR5M1wiLFwiSXNBdXRvSW5jcmVtZW50XCI6ZmFsc2UsXCJDYW5OdWxsXCI6dHJ1ZSxcImRiVHlwZVwiOlwiaW50XCIsXCJsZW5ndGhcIjpcIlwiLFwiVGFibGVJRFwiOjgsXCJJc1BLSURcIjpmYWxzZSxcIm9yZGVyaWRcIjo0Mn0se1wiaWRcIjo4NixcIk5hbWVcIjpcIkFsYXJtVmFsdWU0XCIsXCJJc0F1dG9JbmNyZW1lbnRcIjpmYWxzZSxcIkNhbk51bGxcIjp0cnVlLFwiZGJUeXBlXCI6XCJkb3VibGVcIixcImxlbmd0aFwiOlwiXCIsXCJUYWJsZUlEXCI6OCxcIklzUEtJRFwiOmZhbHNlLFwib3JkZXJpZFwiOjQzfSx7XCJpZFwiOjg3LFwiTmFtZVwiOlwiQWxhcm1Qcmlvcml0eTRcIixcIklzQXV0b0luY3JlbWVudFwiOmZhbHNlLFwiQ2FuTnVsbFwiOnRydWUsXCJkYlR5cGVcIjpcImludFwiLFwibGVuZ3RoXCI6XCJcIixcIlRhYmxlSURcIjo4LFwiSXNQS0lEXCI6ZmFsc2UsXCJvcmRlcmlkXCI6NDR9LHtcImlkXCI6ODgsXCJOYW1lXCI6XCJBbGFybVZhbHVlNVwiLFwiSXNBdXRvSW5jcmVtZW50XCI6ZmFsc2UsXCJDYW5OdWxsXCI6dHJ1ZSxcImRiVHlwZVwiOlwiZG91YmxlXCIsXCJsZW5ndGhcIjpcIlwiLFwiVGFibGVJRFwiOjgsXCJJc1BLSURcIjpmYWxzZSxcIm9yZGVyaWRcIjo0NX0se1wiaWRcIjo4OSxcIk5hbWVcIjpcIkFsYXJtUHJpb3JpdHk1XCIsXCJJc0F1dG9JbmNyZW1lbnRcIjpmYWxzZSxcIkNhbk51bGxcIjp0cnVlLFwiZGJUeXBlXCI6XCJpbnRcIixcImxlbmd0aFwiOlwiXCIsXCJUYWJsZUlEXCI6OCxcIklzUEtJRFwiOmZhbHNlLFwib3JkZXJpZFwiOjQ2fSx7XCJpZFwiOjkwLFwiY2FwdGlvblwiOlwi5YGP5beu5oql6K2mXCIsXCJOYW1lXCI6XCJJc0FsYXJtT2Zmc2V0XCIsXCJJc0F1dG9JbmNyZW1lbnRcIjpmYWxzZSxcIkNhbk51bGxcIjp0cnVlLFwiZGJUeXBlXCI6XCJiaXRcIixcImxlbmd0aFwiOlwiXCIsXCJkZWZhdWx0VmFsdWVcIjpcIjBcIixcIlRhYmxlSURcIjo4LFwiSXNQS0lEXCI6ZmFsc2UsXCJvcmRlcmlkXCI6NDd9LHtcImlkXCI6OTEsXCJjYXB0aW9uXCI6XCLlgY/lt67miqXoraborr7lrprlgLxcIixcIk5hbWVcIjpcIkFsYXJtT2Zmc2V0T3JpZ2luYWxWYWx1ZVwiLFwiSXNBdXRvSW5jcmVtZW50XCI6ZmFsc2UsXCJDYW5OdWxsXCI6dHJ1ZSxcImRiVHlwZVwiOlwiZG91YmxlXCIsXCJsZW5ndGhcIjpcIlwiLFwiVGFibGVJRFwiOjgsXCJJc1BLSURcIjpmYWxzZSxcIm9yZGVyaWRcIjo0OH0se1wiaWRcIjo5MixcImNhcHRpb25cIjpcIuWBj+W3ruWAvFwiLFwiTmFtZVwiOlwiQWxhcm1PZmZzZXRWYWx1ZVwiLFwiSXNBdXRvSW5jcmVtZW50XCI6ZmFsc2UsXCJDYW5OdWxsXCI6dHJ1ZSxcImRiVHlwZVwiOlwiZG91YmxlXCIsXCJsZW5ndGhcIjpcIlwiLFwiVGFibGVJRFwiOjgsXCJJc1BLSURcIjpmYWxzZSxcIm9yZGVyaWRcIjo0OX0se1wiaWRcIjo5MyxcImNhcHRpb25cIjpcIuWBj+W3ruaKpeitpuS8mOWFiOe6p1wiLFwiTmFtZVwiOlwiQWxhcm1PZmZzZXRQcmlvcml0eVwiLFwiSXNBdXRvSW5jcmVtZW50XCI6ZmFsc2UsXCJDYW5OdWxsXCI6dHJ1ZSxcImRiVHlwZVwiOlwiaW50XCIsXCJsZW5ndGhcIjpcIlwiLFwiVGFibGVJRFwiOjgsXCJJc1BLSURcIjpmYWxzZSxcIm9yZGVyaWRcIjo1MH0se1wiaWRcIjo5NCxcImNhcHRpb25cIjpcIuWPmOWMlueOh+aKpeitplwiLFwiTmFtZVwiOlwiSXNBbGFybVBlcmNlbnRcIixcIklzQXV0b0luY3JlbWVudFwiOmZhbHNlLFwiQ2FuTnVsbFwiOnRydWUsXCJkYlR5cGVcIjpcImJpdFwiLFwibGVuZ3RoXCI6XCJcIixcImRlZmF1bHRWYWx1ZVwiOlwiMFwiLFwiVGFibGVJRFwiOjgsXCJJc1BLSURcIjpmYWxzZSxcIm9yZGVyaWRcIjo1MX0se1wiaWRcIjo5NSxcImNhcHRpb25cIjpcIuWPmOWMlueOh+WAvO+8iOeZvuWIhuavlO+8iVwiLFwiTmFtZVwiOlwiUGVyY2VudFwiLFwiSXNBdXRvSW5jcmVtZW50XCI6ZmFsc2UsXCJDYW5OdWxsXCI6dHJ1ZSxcImRiVHlwZVwiOlwiZG91YmxlXCIsXCJsZW5ndGhcIjpcIlwiLFwiVGFibGVJRFwiOjgsXCJJc1BLSURcIjpmYWxzZSxcIm9yZGVyaWRcIjo1Mn0se1wiaWRcIjo5NixcImNhcHRpb25cIjpcIuWPmOWMlueOh+WRqOacn++8iOenku+8iVwiLFwiTmFtZVwiOlwiQ2hhbmdlQ3ljbGVcIixcIklzQXV0b0luY3JlbWVudFwiOmZhbHNlLFwiQ2FuTnVsbFwiOnRydWUsXCJkYlR5cGVcIjpcImludFwiLFwibGVuZ3RoXCI6XCJcIixcIlRhYmxlSURcIjo4LFwiSXNQS0lEXCI6ZmFsc2UsXCJvcmRlcmlkXCI6NTN9LHtcImlkXCI6OTcsXCJjYXB0aW9uXCI6XCLlj5jljJbnjofmiqXorabkvJjlhYjnuqdcIixcIk5hbWVcIjpcIkFsYXJtUGVyY2VudFByaW9yaXR5XCIsXCJJc0F1dG9JbmNyZW1lbnRcIjpmYWxzZSxcIkNhbk51bGxcIjp0cnVlLFwiZGJUeXBlXCI6XCJpbnRcIixcImxlbmd0aFwiOlwiXCIsXCJUYWJsZUlEXCI6OCxcIklzUEtJRFwiOmZhbHNlLFwib3JkZXJpZFwiOjU0fSx7XCJpZFwiOjEwMyxcIk5hbWVcIjpcIkZvbGRlcklkXCIsXCJJc0F1dG9JbmNyZW1lbnRcIjpmYWxzZSxcIkNhbk51bGxcIjp0cnVlLFwiZGJUeXBlXCI6XCJpbnRcIixcImxlbmd0aFwiOlwiXCIsXCJkZWZhdWx0VmFsdWVcIjpcIjBcIixcIlRhYmxlSURcIjo4LFwiSXNQS0lEXCI6ZmFsc2UsXCJvcmRlcmlkXCI6Nn0se1wiaWRcIjoxMDksXCJOYW1lXCI6XCJEZXZpY2VJZFwiLFwiSXNBdXRvSW5jcmVtZW50XCI6ZmFsc2UsXCJDYW5OdWxsXCI6dHJ1ZSxcImRiVHlwZVwiOlwiaW50XCIsXCJsZW5ndGhcIjpcIlwiLFwiVGFibGVJRFwiOjgsXCJJc1BLSURcIjpmYWxzZSxcIm9yZGVyaWRcIjo3fV0sXCJuZXdDb2x1bW5zXCI6W10sXCJjaGFuZ2VkQ29sdW1uc1wiOlt7XCJpZFwiOjQzLFwiY2FwdGlvblwiOlwi54K55ZCNXCIsXCJOYW1lXCI6XCJOYW1lXCIsXCJJc0F1dG9JbmNyZW1lbnRcIjpmYWxzZSxcIkNhbk51bGxcIjp0cnVlLFwiZGJUeXBlXCI6XCJ2YXJjaGFyXCIsXCJsZW5ndGhcIjpcIjUwXCIsXCJUYWJsZUlEXCI6OCxcIklzUEtJRFwiOmZhbHNlLFwib3JkZXJpZFwiOjEsXCJCYWNrdXBDaGFuZ2VkUHJvcGVydGllc1wiOntcImxlbmd0aFwiOntcIk9yaWdpbmFsVmFsdWVcIjpcIlwifSxcImRiVHlwZVwiOntcIk9yaWdpbmFsVmFsdWVcIjpcImludFwifX19XSxcImRlbGV0ZWRDb2x1bW5zXCI6W10sXCJJRFhDb25maWdzXCI6W10sXCJJRFwiOjB9In0seyJOYW1lIjoiZGF0YWJhc2VpZCIsIlZhbHVlIjoyfV0sIlJvd1N0YXRlIjowfSx7Ikl0ZW1zIjpbeyJOYW1lIjoiaWQiLCJWYWx1ZSI6NTJ9LHsiTmFtZSI6InR5cGUiLCJWYWx1ZSI6IkNoYW5nZVRhYmxlQWN0aW9uIn0seyJOYW1lIjoiY29udGVudCIsIlZhbHVlIjoie1wiT2xkVGFibGVOYW1lXCI6XCJEZXZpY2VcIixcIk5ld1RhYmxlTmFtZVwiOlwiRGV2aWNlXCIsXCJvdGhlckNvbHVtbnNcIjpbe1wiaWRcIjozOSxcIk5hbWVcIjpcImlkXCIsXCJJc0F1dG9JbmNyZW1lbnRcIjp0cnVlLFwiQ2FuTnVsbFwiOmZhbHNlLFwiZGJUeXBlXCI6XCJpbnRcIixcIlRhYmxlSURcIjo3LFwiSXNQS0lEXCI6dHJ1ZSxcIm9yZGVyaWRcIjowfSx7XCJpZFwiOjQwLFwiY2FwdGlvblwiOlwi6YCa6K6v572R5YWzXCIsXCJOYW1lXCI6XCJEcml2ZXJJRFwiLFwiSXNBdXRvSW5jcmVtZW50XCI6ZmFsc2UsXCJDYW5OdWxsXCI6dHJ1ZSxcImRiVHlwZVwiOlwiaW50XCIsXCJsZW5ndGhcIjpcIlwiLFwiVGFibGVJRFwiOjcsXCJJc1BLSURcIjpmYWxzZSxcIm9yZGVyaWRcIjoxfSx7XCJpZFwiOjQxLFwiY2FwdGlvblwiOlwi5Zyw5Z2A5L+h5oGvXCIsXCJOYW1lXCI6XCJBZGRyZXNzXCIsXCJJc0F1dG9JbmNyZW1lbnRcIjpmYWxzZSxcIkNhbk51bGxcIjp0cnVlLFwiZGJUeXBlXCI6XCJ2YXJjaGFyXCIsXCJsZW5ndGhcIjpcIjEwMFwiLFwiVGFibGVJRFwiOjcsXCJJc1BLSURcIjpmYWxzZSxcIm9yZGVyaWRcIjozfSx7XCJpZFwiOjEwMCxcImNhcHRpb25cIjpcIuWcsOWdgOiuvue9ruaXtueahGpzb27lr7nosaHooajovr5cIixcIk5hbWVcIjpcIkFkZHJTZXR0aW5nXCIsXCJJc0F1dG9JbmNyZW1lbnRcIjpmYWxzZSxcIkNhbk51bGxcIjp0cnVlLFwiZGJUeXBlXCI6XCJ2YXJjaGFyXCIsXCJsZW5ndGhcIjpcIjIwMFwiLFwiVGFibGVJRFwiOjcsXCJJc1BLSURcIjpmYWxzZSxcIm9yZGVyaWRcIjo0fSx7XCJpZFwiOjEwMSxcImNhcHRpb25cIjpcIuiuvuWkh+WQjeensFwiLFwiTmFtZVwiOlwiTmFtZVwiLFwiSXNBdXRvSW5jcmVtZW50XCI6ZmFsc2UsXCJDYW5OdWxsXCI6dHJ1ZSxcImRiVHlwZVwiOlwidmFyY2hhclwiLFwibGVuZ3RoXCI6XCI1MFwiLFwiVGFibGVJRFwiOjcsXCJJc1BLSURcIjpmYWxzZSxcIm9yZGVyaWRcIjoyfSx7XCJpZFwiOjEwMixcImNhcHRpb25cIjpcIuaOp+WItuWNleWFg2lkXCIsXCJOYW1lXCI6XCJVbml0SWRcIixcIklzQXV0b0luY3JlbWVudFwiOmZhbHNlLFwiQ2FuTnVsbFwiOnRydWUsXCJkYlR5cGVcIjpcImludFwiLFwibGVuZ3RoXCI6XCJcIixcIlRhYmxlSURcIjo3LFwiSXNQS0lEXCI6ZmFsc2UsXCJvcmRlcmlkXCI6NX1dLFwibmV3Q29sdW1uc1wiOltdLFwiY2hhbmdlZENvbHVtbnNcIjpbXSxcImRlbGV0ZWRDb2x1bW5zXCI6W10sXCJJRFhDb25maWdzXCI6W10sXCJJRFwiOjB9In0seyJOYW1lIjoiZGF0YWJhc2VpZCIsIlZhbHVlIjoyfV0sIlJvd1N0YXRlIjowfV0sIkNvbHVtbnMiOlt7IkNvbHVtbk5hbWUiOiJpZCIsIkRhdGFUeXBlIjoiU3lzdGVtLkludDY0In0seyJDb2x1bW5OYW1lIjoidHlwZSIsIkRhdGFUeXBlIjoiU3lzdGVtLlN0cmluZyJ9LHsiQ29sdW1uTmFtZSI6ImNvbnRlbnQiLCJEYXRhVHlwZSI6IlN5c3RlbS5TdHJpbmcifSx7IkNvbHVtbk5hbWUiOiJkYXRhYmFzZWlkIiwiRGF0YVR5cGUiOiJTeXN0ZW0uSW50NjQifV19XSwiRGF0YVNldE5hbWUiOiI0Mzc3YjBkNi1lZmY5LTQzMzAtYWJlYi1mMGFhM2FmMjZlYWUifQ==";}}
