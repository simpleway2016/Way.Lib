
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SunRizServer{


    /// <summary>
	/// 图片文件
	/// </summary>
    public class ImageFiles :Way.Lib.DataModel
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
        /// 显示的名称
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

        System.Nullable<Int32> _ParentId=0;
        /// <summary>
        /// 
        /// </summary>
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
                    var original = this._ParentId;
                    this._ParentId = value;
                    this.OnPropertyChanged("ParentId",original,value);

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

        String _FileName;
        /// <summary>
        /// 文件实际文件名
        /// </summary>
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
                    var original = this._FileName;
                    this._FileName = value;
                    this.OnPropertyChanged("FileName",original,value);

                }
            }
        }

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
    public class CommunicationDriver :Way.Lib.DataModel
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
        /// 名称
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

        String _Address;
        /// <summary>
        /// 地址
        /// </summary>
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
                    var original = this._Address;
                    this._Address = value;
                    this.OnPropertyChanged("Address",original,value);

                }
            }
        }

        System.Nullable<Int32> _Port;
        /// <summary>
        /// 端口
        /// </summary>
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
                    var original = this._Port;
                    this._Port = value;
                    this.OnPropertyChanged("Port",original,value);

                }
            }
        }

        System.Nullable<CommunicationDriver_StatusEnum> _Status=(System.Nullable<CommunicationDriver_StatusEnum>)(0);
        /// <summary>
        /// 状态
        /// </summary>
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
                    var original = this._Status;
                    this._Status = value;
                    this.OnPropertyChanged("Status",original,value);

                }
            }
        }

        System.Nullable<Boolean> _SupportEnumDevice=false;
        /// <summary>
        /// 
        /// </summary>
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
                    var original = this._SupportEnumDevice;
                    this._SupportEnumDevice = value;
                    this.OnPropertyChanged("SupportEnumDevice",original,value);

                }
            }
        }

        System.Nullable<Boolean> _SupportEnumPoints=false;
        /// <summary>
        /// 
        /// </summary>
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
                    var original = this._SupportEnumPoints;
                    this._SupportEnumPoints = value;
                    this.OnPropertyChanged("SupportEnumPoints",original,value);

                }
            }
        }
}}
namespace SunRizServer{


    /// <summary>
	/// 设备信息
	/// </summary>
    public class Device :Way.Lib.DataModel
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

        System.Nullable<Int32> _DriverID;
        /// <summary>
        /// 通讯网关
        /// </summary>
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
                    var original = this._DriverID;
                    this._DriverID = value;
                    this.OnPropertyChanged("DriverID",original,value);

                }
            }
        }

        String _Address;
        /// <summary>
        /// 地址信息
        /// </summary>
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
                    var original = this._Address;
                    this._Address = value;
                    this.OnPropertyChanged("Address",original,value);

                }
            }
        }

        String _AddrSetting;
        /// <summary>
        /// 地址设置时的json对象表达
        /// </summary>
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
                    var original = this._AddrSetting;
                    this._AddrSetting = value;
                    this.OnPropertyChanged("AddrSetting",original,value);

                }
            }
        }

        String _Name;
        /// <summary>
        /// 设备名称
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

        System.Nullable<Int32> _UnitId;
        /// <summary>
        /// 控制单元id
        /// </summary>
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
                    var original = this._UnitId;
                    this._UnitId = value;
                    this.OnPropertyChanged("UnitId",original,value);

                }
            }
        }

        public virtual CommunicationDriver Driver { get; set; }
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
    public class DevicePoint :Way.Lib.DataModel
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
        /// 点名
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

        String _Address;
        /// <summary>
        /// 
        /// </summary>
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
                    var original = this._Address;
                    this._Address = value;
                    this.OnPropertyChanged("Address",original,value);

                }
            }
        }

        System.Nullable<Boolean> _IsWatching=false;
        /// <summary>
        /// 是否监测变化
        /// </summary>
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
                    var original = this._IsWatching;
                    this._IsWatching = value;
                    this.OnPropertyChanged("IsWatching",original,value);

                }
            }
        }

        String _AddrSetting;
        /// <summary>
        /// 地址设置时的json对象表达
        /// </summary>
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
                    var original = this._AddrSetting;
                    this._AddrSetting = value;
                    this.OnPropertyChanged("AddrSetting",original,value);

                }
            }
        }

        String _Desc;
        /// <summary>
        /// 描述
        /// </summary>
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
                    var original = this._Desc;
                    this._Desc = value;
                    this.OnPropertyChanged("Desc",original,value);

                }
            }
        }

        System.Nullable<double> _InitValue;
        /// <summary>
        /// 初始值
        /// </summary>
        public virtual System.Nullable<double> InitValue
        {
            get
            {
                return this._InitValue;
            }
            set
            {
                if ((this._InitValue != value))
                {
                    var original = this._InitValue;
                    this._InitValue = value;
                    this.OnPropertyChanged("InitValue",original,value);

                }
            }
        }

        String _StatusOpenInfo;
        /// <summary>
        /// 开状态信息
        /// </summary>
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
                    var original = this._StatusOpenInfo;
                    this._StatusOpenInfo = value;
                    this.OnPropertyChanged("StatusOpenInfo",original,value);

                }
            }
        }

        String _StateCloseInfo;
        /// <summary>
        /// 关状态信息
        /// </summary>
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
                    var original = this._StateCloseInfo;
                    this._StateCloseInfo = value;
                    this.OnPropertyChanged("StateCloseInfo",original,value);

                }
            }
        }

        System.Nullable<Boolean> _IsAlarm=false;
        /// <summary>
        /// 报警开关
        /// </summary>
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
                    var original = this._IsAlarm;
                    this._IsAlarm = value;
                    this.OnPropertyChanged("IsAlarm",original,value);

                }
            }
        }

        System.Nullable<double> _AlarmValue;
        /// <summary>
        /// 报警值
        /// </summary>
        public virtual System.Nullable<double> AlarmValue
        {
            get
            {
                return this._AlarmValue;
            }
            set
            {
                if ((this._AlarmValue != value))
                {
                    var original = this._AlarmValue;
                    this._AlarmValue = value;
                    this.OnPropertyChanged("AlarmValue",original,value);

                }
            }
        }

        System.Nullable<Int32> _AlarmPriority;
        /// <summary>
        /// 报警优先级
        /// </summary>
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
                    var original = this._AlarmPriority;
                    this._AlarmPriority = value;
                    this.OnPropertyChanged("AlarmPriority",original,value);

                }
            }
        }

        String _AlarmGroup;
        /// <summary>
        /// 报警组
        /// </summary>
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
                    var original = this._AlarmGroup;
                    this._AlarmGroup = value;
                    this.OnPropertyChanged("AlarmGroup",original,value);

                }
            }
        }

        System.Nullable<Boolean> _AlarmAutoConfirm=false;
        /// <summary>
        /// 报警自动确认
        /// </summary>
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
                    var original = this._AlarmAutoConfirm;
                    this._AlarmAutoConfirm = value;
                    this.OnPropertyChanged("AlarmAutoConfirm",original,value);

                }
            }
        }

        System.Nullable<Boolean> _AlarmAutoReset=false;
        /// <summary>
        /// 报警自动复位
        /// </summary>
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
                    var original = this._AlarmAutoReset;
                    this._AlarmAutoReset = value;
                    this.OnPropertyChanged("AlarmAutoReset",original,value);

                }
            }
        }

        System.Nullable<Boolean> _ValueAbsoluteChange=false;
        /// <summary>
        /// 数据绝对变化保存
        /// </summary>
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
                    var original = this._ValueAbsoluteChange;
                    this._ValueAbsoluteChange = value;
                    this.OnPropertyChanged("ValueAbsoluteChange",original,value);

                }
            }
        }

        System.Nullable<double> _ValueAbsoluteChangeSetting;
        /// <summary>
        /// 数据绝对变化值
        /// </summary>
        public virtual System.Nullable<double> ValueAbsoluteChangeSetting
        {
            get
            {
                return this._ValueAbsoluteChangeSetting;
            }
            set
            {
                if ((this._ValueAbsoluteChangeSetting != value))
                {
                    var original = this._ValueAbsoluteChangeSetting;
                    this._ValueAbsoluteChangeSetting = value;
                    this.OnPropertyChanged("ValueAbsoluteChangeSetting",original,value);

                }
            }
        }

        System.Nullable<Boolean> _ValueRelativeChange=false;
        /// <summary>
        /// 数据相对变化保存
        /// </summary>
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
                    var original = this._ValueRelativeChange;
                    this._ValueRelativeChange = value;
                    this.OnPropertyChanged("ValueRelativeChange",original,value);

                }
            }
        }

        System.Nullable<double> _ValueRelativeChangeSetting;
        /// <summary>
        /// 数据相对变化值
        /// </summary>
        public virtual System.Nullable<double> ValueRelativeChangeSetting
        {
            get
            {
                return this._ValueRelativeChangeSetting;
            }
            set
            {
                if ((this._ValueRelativeChangeSetting != value))
                {
                    var original = this._ValueRelativeChangeSetting;
                    this._ValueRelativeChangeSetting = value;
                    this.OnPropertyChanged("ValueRelativeChangeSetting",original,value);

                }
            }
        }

        System.Nullable<Boolean> _ValueOnTimeChange=false;
        /// <summary>
        /// 数据定时保存
        /// </summary>
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
                    var original = this._ValueOnTimeChange;
                    this._ValueOnTimeChange = value;
                    this.OnPropertyChanged("ValueOnTimeChange",original,value);

                }
            }
        }

        System.Nullable<double> _ValueOnTimeChangeSetting;
        /// <summary>
        /// 数据定时值
        /// </summary>
        public virtual System.Nullable<double> ValueOnTimeChangeSetting
        {
            get
            {
                return this._ValueOnTimeChangeSetting;
            }
            set
            {
                if ((this._ValueOnTimeChangeSetting != value))
                {
                    var original = this._ValueOnTimeChangeSetting;
                    this._ValueOnTimeChangeSetting = value;
                    this.OnPropertyChanged("ValueOnTimeChangeSetting",original,value);

                }
            }
        }

        System.Nullable<DevicePoint_TypeEnum> _Type=(System.Nullable<DevicePoint_TypeEnum>)(2);
        /// <summary>
        /// 点类型
        /// </summary>
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
                    var original = this._Type;
                    this._Type = value;
                    this.OnPropertyChanged("Type",original,value);

                }
            }
        }

        String _Unit;
        /// <summary>
        /// 模拟量-工程单位
        /// </summary>
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
                    var original = this._Unit;
                    this._Unit = value;
                    this.OnPropertyChanged("Unit",original,value);

                }
            }
        }

        String _DPCount;
        /// <summary>
        /// 模拟量-小数点位数
        /// </summary>
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
                    var original = this._DPCount;
                    this._DPCount = value;
                    this.OnPropertyChanged("DPCount",original,value);

                }
            }
        }

        System.Nullable<Boolean> _IsTransform=false;
        /// <summary>
        /// 模拟量-量程转化开关
        /// </summary>
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
                    var original = this._IsTransform;
                    this._IsTransform = value;
                    this.OnPropertyChanged("IsTransform",original,value);

                }
            }
        }

        System.Nullable<double> _TransMax;
        /// <summary>
        /// 模拟量-量程上限
        /// </summary>
        public virtual System.Nullable<double> TransMax
        {
            get
            {
                return this._TransMax;
            }
            set
            {
                if ((this._TransMax != value))
                {
                    var original = this._TransMax;
                    this._TransMax = value;
                    this.OnPropertyChanged("TransMax",original,value);

                }
            }
        }

        System.Nullable<double> _TransMin;
        /// <summary>
        /// 模拟量-量程下限
        /// </summary>
        public virtual System.Nullable<double> TransMin
        {
            get
            {
                return this._TransMin;
            }
            set
            {
                if ((this._TransMin != value))
                {
                    var original = this._TransMin;
                    this._TransMin = value;
                    this.OnPropertyChanged("TransMin",original,value);

                }
            }
        }

        System.Nullable<double> _SensorMax;
        /// <summary>
        /// 模拟量-传感器量程上限
        /// </summary>
        public virtual System.Nullable<double> SensorMax
        {
            get
            {
                return this._SensorMax;
            }
            set
            {
                if ((this._SensorMax != value))
                {
                    var original = this._SensorMax;
                    this._SensorMax = value;
                    this.OnPropertyChanged("SensorMax",original,value);

                }
            }
        }

        String _SensorMin;
        /// <summary>
        /// 模拟量-传感器量程下限
        /// </summary>
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
                    var original = this._SensorMin;
                    this._SensorMin = value;
                    this.OnPropertyChanged("SensorMin",original,value);

                }
            }
        }

        System.Nullable<Boolean> _IsSquare=false;
        /// <summary>
        /// 模拟量-开平方
        /// </summary>
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
                    var original = this._IsSquare;
                    this._IsSquare = value;
                    this.OnPropertyChanged("IsSquare",original,value);

                }
            }
        }

        System.Nullable<Boolean> _IsLinear=false;
        /// <summary>
        /// 分段线性化
        /// </summary>
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
                    var original = this._IsLinear;
                    this._IsLinear = value;
                    this.OnPropertyChanged("IsLinear",original,value);

                }
            }
        }

        System.Nullable<double> _LinearX1;
        /// <summary>
        /// 分段线性化X1
        /// </summary>
        public virtual System.Nullable<double> LinearX1
        {
            get
            {
                return this._LinearX1;
            }
            set
            {
                if ((this._LinearX1 != value))
                {
                    var original = this._LinearX1;
                    this._LinearX1 = value;
                    this.OnPropertyChanged("LinearX1",original,value);

                }
            }
        }

        System.Nullable<double> _LinearY1;
        /// <summary>
        /// 分段线性化Y1
        /// </summary>
        public virtual System.Nullable<double> LinearY1
        {
            get
            {
                return this._LinearY1;
            }
            set
            {
                if ((this._LinearY1 != value))
                {
                    var original = this._LinearY1;
                    this._LinearY1 = value;
                    this.OnPropertyChanged("LinearY1",original,value);

                }
            }
        }

        System.Nullable<double> _LinearX2;
        /// <summary>
        /// 分段线性化X2
        /// </summary>
        public virtual System.Nullable<double> LinearX2
        {
            get
            {
                return this._LinearX2;
            }
            set
            {
                if ((this._LinearX2 != value))
                {
                    var original = this._LinearX2;
                    this._LinearX2 = value;
                    this.OnPropertyChanged("LinearX2",original,value);

                }
            }
        }

        System.Nullable<double> _LinearY2;
        /// <summary>
        /// 分段线性化Y2
        /// </summary>
        public virtual System.Nullable<double> LinearY2
        {
            get
            {
                return this._LinearY2;
            }
            set
            {
                if ((this._LinearY2 != value))
                {
                    var original = this._LinearY2;
                    this._LinearY2 = value;
                    this.OnPropertyChanged("LinearY2",original,value);

                }
            }
        }

        System.Nullable<double> _LinearX3;
        /// <summary>
        /// 
        /// </summary>
        public virtual System.Nullable<double> LinearX3
        {
            get
            {
                return this._LinearX3;
            }
            set
            {
                if ((this._LinearX3 != value))
                {
                    var original = this._LinearX3;
                    this._LinearX3 = value;
                    this.OnPropertyChanged("LinearX3",original,value);

                }
            }
        }

        System.Nullable<double> _LinearY3;
        /// <summary>
        /// 
        /// </summary>
        public virtual System.Nullable<double> LinearY3
        {
            get
            {
                return this._LinearY3;
            }
            set
            {
                if ((this._LinearY3 != value))
                {
                    var original = this._LinearY3;
                    this._LinearY3 = value;
                    this.OnPropertyChanged("LinearY3",original,value);

                }
            }
        }

        System.Nullable<double> _LinearX4;
        /// <summary>
        /// 
        /// </summary>
        public virtual System.Nullable<double> LinearX4
        {
            get
            {
                return this._LinearX4;
            }
            set
            {
                if ((this._LinearX4 != value))
                {
                    var original = this._LinearX4;
                    this._LinearX4 = value;
                    this.OnPropertyChanged("LinearX4",original,value);

                }
            }
        }

        System.Nullable<double> _AlarmValue2;
        /// <summary>
        /// 报警值2
        /// </summary>
        public virtual System.Nullable<double> AlarmValue2
        {
            get
            {
                return this._AlarmValue2;
            }
            set
            {
                if ((this._AlarmValue2 != value))
                {
                    var original = this._AlarmValue2;
                    this._AlarmValue2 = value;
                    this.OnPropertyChanged("AlarmValue2",original,value);

                }
            }
        }

        System.Nullable<Int32> _AlarmPriority2;
        /// <summary>
        /// 报警优先级2
        /// </summary>
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
                    var original = this._AlarmPriority2;
                    this._AlarmPriority2 = value;
                    this.OnPropertyChanged("AlarmPriority2",original,value);

                }
            }
        }

        System.Nullable<double> _AlarmValue3;
        /// <summary>
        /// 
        /// </summary>
        public virtual System.Nullable<double> AlarmValue3
        {
            get
            {
                return this._AlarmValue3;
            }
            set
            {
                if ((this._AlarmValue3 != value))
                {
                    var original = this._AlarmValue3;
                    this._AlarmValue3 = value;
                    this.OnPropertyChanged("AlarmValue3",original,value);

                }
            }
        }

        System.Nullable<Int32> _AlarmPriority3;
        /// <summary>
        /// 
        /// </summary>
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
                    var original = this._AlarmPriority3;
                    this._AlarmPriority3 = value;
                    this.OnPropertyChanged("AlarmPriority3",original,value);

                }
            }
        }

        System.Nullable<double> _AlarmValue4;
        /// <summary>
        /// 
        /// </summary>
        public virtual System.Nullable<double> AlarmValue4
        {
            get
            {
                return this._AlarmValue4;
            }
            set
            {
                if ((this._AlarmValue4 != value))
                {
                    var original = this._AlarmValue4;
                    this._AlarmValue4 = value;
                    this.OnPropertyChanged("AlarmValue4",original,value);

                }
            }
        }

        System.Nullable<Int32> _AlarmPriority4;
        /// <summary>
        /// 
        /// </summary>
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
                    var original = this._AlarmPriority4;
                    this._AlarmPriority4 = value;
                    this.OnPropertyChanged("AlarmPriority4",original,value);

                }
            }
        }

        System.Nullable<double> _AlarmValue5;
        /// <summary>
        /// 
        /// </summary>
        public virtual System.Nullable<double> AlarmValue5
        {
            get
            {
                return this._AlarmValue5;
            }
            set
            {
                if ((this._AlarmValue5 != value))
                {
                    var original = this._AlarmValue5;
                    this._AlarmValue5 = value;
                    this.OnPropertyChanged("AlarmValue5",original,value);

                }
            }
        }

        System.Nullable<Int32> _AlarmPriority5;
        /// <summary>
        /// 
        /// </summary>
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
                    var original = this._AlarmPriority5;
                    this._AlarmPriority5 = value;
                    this.OnPropertyChanged("AlarmPriority5",original,value);

                }
            }
        }

        System.Nullable<Boolean> _IsAlarmOffset=false;
        /// <summary>
        /// 偏差报警
        /// </summary>
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
                    var original = this._IsAlarmOffset;
                    this._IsAlarmOffset = value;
                    this.OnPropertyChanged("IsAlarmOffset",original,value);

                }
            }
        }

        System.Nullable<double> _AlarmOffsetOriginalValue;
        /// <summary>
        /// 偏差报警设定值
        /// </summary>
        public virtual System.Nullable<double> AlarmOffsetOriginalValue
        {
            get
            {
                return this._AlarmOffsetOriginalValue;
            }
            set
            {
                if ((this._AlarmOffsetOriginalValue != value))
                {
                    var original = this._AlarmOffsetOriginalValue;
                    this._AlarmOffsetOriginalValue = value;
                    this.OnPropertyChanged("AlarmOffsetOriginalValue",original,value);

                }
            }
        }

        System.Nullable<double> _AlarmOffsetValue;
        /// <summary>
        /// 偏差值
        /// </summary>
        public virtual System.Nullable<double> AlarmOffsetValue
        {
            get
            {
                return this._AlarmOffsetValue;
            }
            set
            {
                if ((this._AlarmOffsetValue != value))
                {
                    var original = this._AlarmOffsetValue;
                    this._AlarmOffsetValue = value;
                    this.OnPropertyChanged("AlarmOffsetValue",original,value);

                }
            }
        }

        System.Nullable<Int32> _AlarmOffsetPriority;
        /// <summary>
        /// 偏差报警优先级
        /// </summary>
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
                    var original = this._AlarmOffsetPriority;
                    this._AlarmOffsetPriority = value;
                    this.OnPropertyChanged("AlarmOffsetPriority",original,value);

                }
            }
        }

        System.Nullable<Boolean> _IsAlarmPercent=false;
        /// <summary>
        /// 变化率报警
        /// </summary>
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
                    var original = this._IsAlarmPercent;
                    this._IsAlarmPercent = value;
                    this.OnPropertyChanged("IsAlarmPercent",original,value);

                }
            }
        }

        System.Nullable<double> _Percent;
        /// <summary>
        /// 变化率值（百分比）
        /// </summary>
        public virtual System.Nullable<double> Percent
        {
            get
            {
                return this._Percent;
            }
            set
            {
                if ((this._Percent != value))
                {
                    var original = this._Percent;
                    this._Percent = value;
                    this.OnPropertyChanged("Percent",original,value);

                }
            }
        }

        System.Nullable<Int32> _ChangeCycle;
        /// <summary>
        /// 变化率周期（秒）
        /// </summary>
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
                    var original = this._ChangeCycle;
                    this._ChangeCycle = value;
                    this.OnPropertyChanged("ChangeCycle",original,value);

                }
            }
        }

        System.Nullable<Int32> _AlarmPercentPriority;
        /// <summary>
        /// 变化率报警优先级
        /// </summary>
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
                    var original = this._AlarmPercentPriority;
                    this._AlarmPercentPriority = value;
                    this.OnPropertyChanged("AlarmPercentPriority",original,value);

                }
            }
        }

        System.Nullable<Int32> _FolderId=0;
        /// <summary>
        /// 
        /// </summary>
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
                    var original = this._FolderId;
                    this._FolderId = value;
                    this.OnPropertyChanged("FolderId",original,value);

                }
            }
        }

        System.Nullable<Int32> _DeviceId;
        /// <summary>
        /// 
        /// </summary>
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
                    var original = this._DeviceId;
                    this._DeviceId = value;
                    this.OnPropertyChanged("DeviceId",original,value);

                }
            }
        }

        System.Nullable<double> _LinearY4;
        /// <summary>
        /// 
        /// </summary>
        public virtual System.Nullable<double> LinearY4
        {
            get
            {
                return this._LinearY4;
            }
            set
            {
                if ((this._LinearY4 != value))
                {
                    var original = this._LinearY4;
                    this._LinearY4 = value;
                    this.OnPropertyChanged("LinearY4",original,value);

                }
            }
        }

        System.Nullable<double> _LinearX5;
        /// <summary>
        /// 
        /// </summary>
        public virtual System.Nullable<double> LinearX5
        {
            get
            {
                return this._LinearX5;
            }
            set
            {
                if ((this._LinearX5 != value))
                {
                    var original = this._LinearX5;
                    this._LinearX5 = value;
                    this.OnPropertyChanged("LinearX5",original,value);

                }
            }
        }

        System.Nullable<double> _LinearY5;
        /// <summary>
        /// 
        /// </summary>
        public virtual System.Nullable<double> LinearY5
        {
            get
            {
                return this._LinearY5;
            }
            set
            {
                if ((this._LinearY5 != value))
                {
                    var original = this._LinearY5;
                    this._LinearY5 = value;
                    this.OnPropertyChanged("LinearY5",original,value);

                }
            }
        }

        System.Nullable<double> _LinearX6;
        /// <summary>
        /// 
        /// </summary>
        public virtual System.Nullable<double> LinearX6
        {
            get
            {
                return this._LinearX6;
            }
            set
            {
                if ((this._LinearX6 != value))
                {
                    var original = this._LinearX6;
                    this._LinearX6 = value;
                    this.OnPropertyChanged("LinearX6",original,value);

                }
            }
        }

        System.Nullable<double> _LinearY6;
        /// <summary>
        /// 
        /// </summary>
        public virtual System.Nullable<double> LinearY6
        {
            get
            {
                return this._LinearY6;
            }
            set
            {
                if ((this._LinearY6 != value))
                {
                    var original = this._LinearY6;
                    this._LinearY6 = value;
                    this.OnPropertyChanged("LinearY6",original,value);

                }
            }
        }

        System.Nullable<double> _HiAlarmValue;
        /// <summary>
        /// 高报警值
        /// </summary>
        public virtual System.Nullable<double> HiAlarmValue
        {
            get
            {
                return this._HiAlarmValue;
            }
            set
            {
                if ((this._HiAlarmValue != value))
                {
                    var original = this._HiAlarmValue;
                    this._HiAlarmValue = value;
                    this.OnPropertyChanged("HiAlarmValue",original,value);

                }
            }
        }

        System.Nullable<Int32> _HiAlarmPriority;
        /// <summary>
        /// 
        /// </summary>
        public virtual System.Nullable<Int32> HiAlarmPriority
        {
            get
            {
                return this._HiAlarmPriority;
            }
            set
            {
                if ((this._HiAlarmPriority != value))
                {
                    var original = this._HiAlarmPriority;
                    this._HiAlarmPriority = value;
                    this.OnPropertyChanged("HiAlarmPriority",original,value);

                }
            }
        }

        System.Nullable<double> _HiAlarmValue2;
        /// <summary>
        /// 
        /// </summary>
        public virtual System.Nullable<double> HiAlarmValue2
        {
            get
            {
                return this._HiAlarmValue2;
            }
            set
            {
                if ((this._HiAlarmValue2 != value))
                {
                    var original = this._HiAlarmValue2;
                    this._HiAlarmValue2 = value;
                    this.OnPropertyChanged("HiAlarmValue2",original,value);

                }
            }
        }

        System.Nullable<Int32> _HiAlarmPriority2;
        /// <summary>
        /// 
        /// </summary>
        public virtual System.Nullable<Int32> HiAlarmPriority2
        {
            get
            {
                return this._HiAlarmPriority2;
            }
            set
            {
                if ((this._HiAlarmPriority2 != value))
                {
                    var original = this._HiAlarmPriority2;
                    this._HiAlarmPriority2 = value;
                    this.OnPropertyChanged("HiAlarmPriority2",original,value);

                }
            }
        }

        System.Nullable<double> _HiAlarmValue3;
        /// <summary>
        /// 
        /// </summary>
        public virtual System.Nullable<double> HiAlarmValue3
        {
            get
            {
                return this._HiAlarmValue3;
            }
            set
            {
                if ((this._HiAlarmValue3 != value))
                {
                    var original = this._HiAlarmValue3;
                    this._HiAlarmValue3 = value;
                    this.OnPropertyChanged("HiAlarmValue3",original,value);

                }
            }
        }

        System.Nullable<Int32> _HiAlarmPriority3;
        /// <summary>
        /// 
        /// </summary>
        public virtual System.Nullable<Int32> HiAlarmPriority3
        {
            get
            {
                return this._HiAlarmPriority3;
            }
            set
            {
                if ((this._HiAlarmPriority3 != value))
                {
                    var original = this._HiAlarmPriority3;
                    this._HiAlarmPriority3 = value;
                    this.OnPropertyChanged("HiAlarmPriority3",original,value);

                }
            }
        }

        System.Nullable<double> _HiAlarmValue4;
        /// <summary>
        /// 
        /// </summary>
        public virtual System.Nullable<double> HiAlarmValue4
        {
            get
            {
                return this._HiAlarmValue4;
            }
            set
            {
                if ((this._HiAlarmValue4 != value))
                {
                    var original = this._HiAlarmValue4;
                    this._HiAlarmValue4 = value;
                    this.OnPropertyChanged("HiAlarmValue4",original,value);

                }
            }
        }

        System.Nullable<Int32> _HiAlarmPriority4;
        /// <summary>
        /// 
        /// </summary>
        public virtual System.Nullable<Int32> HiAlarmPriority4
        {
            get
            {
                return this._HiAlarmPriority4;
            }
            set
            {
                if ((this._HiAlarmPriority4 != value))
                {
                    var original = this._HiAlarmPriority4;
                    this._HiAlarmPriority4 = value;
                    this.OnPropertyChanged("HiAlarmPriority4",original,value);

                }
            }
        }

        System.Nullable<double> _HiAlarmValue5;
        /// <summary>
        /// 
        /// </summary>
        public virtual System.Nullable<double> HiAlarmValue5
        {
            get
            {
                return this._HiAlarmValue5;
            }
            set
            {
                if ((this._HiAlarmValue5 != value))
                {
                    var original = this._HiAlarmValue5;
                    this._HiAlarmValue5 = value;
                    this.OnPropertyChanged("HiAlarmValue5",original,value);

                }
            }
        }

        System.Nullable<Int32> _HiAlarmPriority5;
        /// <summary>
        /// 
        /// </summary>
        public virtual System.Nullable<Int32> HiAlarmPriority5
        {
            get
            {
                return this._HiAlarmPriority5;
            }
            set
            {
                if ((this._HiAlarmPriority5 != value))
                {
                    var original = this._HiAlarmPriority5;
                    this._HiAlarmPriority5 = value;
                    this.OnPropertyChanged("HiAlarmPriority5",original,value);

                }
            }
        }

        System.Nullable<Int32> _SafeArea;
        /// <summary>
        /// 安全区
        /// </summary>
        public virtual System.Nullable<Int32> SafeArea
        {
            get
            {
                return this._SafeArea;
            }
            set
            {
                if ((this._SafeArea != value))
                {
                    var original = this._SafeArea;
                    this._SafeArea = value;
                    this.OnPropertyChanged("SafeArea",original,value);

                }
            }
        }
}}
namespace SunRizServer{


    /// <summary>
	/// 控制单元
	/// </summary>
    public class ControlUnit :Way.Lib.DataModel
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
        /// 名称
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

        System.Nullable<double> _Max;
        /// <summary>
        /// 量程上限
        /// </summary>
        public virtual System.Nullable<double> Max
        {
            get
            {
                return this._Max;
            }
            set
            {
                if ((this._Max != value))
                {
                    var original = this._Max;
                    this._Max = value;
                    this.OnPropertyChanged("Max",original,value);

                }
            }
        }

        System.Nullable<double> _Min;
        /// <summary>
        /// 量程下限
        /// </summary>
        public virtual System.Nullable<double> Min
        {
            get
            {
                return this._Min;
            }
            set
            {
                if ((this._Min != value))
                {
                    var original = this._Min;
                    this._Min = value;
                    this.OnPropertyChanged("Min",original,value);

                }
            }
        }

        String _WinBgColor;
        /// <summary>
        /// 窗口背景色
        /// </summary>
        public virtual String WinBgColor
        {
            get
            {
                return this._WinBgColor;
            }
            set
            {
                if ((this._WinBgColor != value))
                {
                    var original = this._WinBgColor;
                    this._WinBgColor = value;
                    this.OnPropertyChanged("WinBgColor",original,value);

                }
            }
        }

        String _WinFrontColor;
        /// <summary>
        /// 窗口前景色
        /// </summary>
        public virtual String WinFrontColor
        {
            get
            {
                return this._WinFrontColor;
            }
            set
            {
                if ((this._WinFrontColor != value))
                {
                    var original = this._WinFrontColor;
                    this._WinFrontColor = value;
                    this.OnPropertyChanged("WinFrontColor",original,value);

                }
            }
        }

        String _GridColor;
        /// <summary>
        /// 网格颜色
        /// </summary>
        public virtual String GridColor
        {
            get
            {
                return this._GridColor;
            }
            set
            {
                if ((this._GridColor != value))
                {
                    var original = this._GridColor;
                    this._GridColor = value;
                    this.OnPropertyChanged("GridColor",original,value);

                }
            }
        }

        String _LineColor1;
        /// <summary>
        /// 点颜色
        /// </summary>
        public virtual String LineColor1
        {
            get
            {
                return this._LineColor1;
            }
            set
            {
                if ((this._LineColor1 != value))
                {
                    var original = this._LineColor1;
                    this._LineColor1 = value;
                    this.OnPropertyChanged("LineColor1",original,value);

                }
            }
        }

        String _LineColor2;
        /// <summary>
        /// 
        /// </summary>
        public virtual String LineColor2
        {
            get
            {
                return this._LineColor2;
            }
            set
            {
                if ((this._LineColor2 != value))
                {
                    var original = this._LineColor2;
                    this._LineColor2 = value;
                    this.OnPropertyChanged("LineColor2",original,value);

                }
            }
        }

        String _LineColor3;
        /// <summary>
        /// 
        /// </summary>
        public virtual String LineColor3
        {
            get
            {
                return this._LineColor3;
            }
            set
            {
                if ((this._LineColor3 != value))
                {
                    var original = this._LineColor3;
                    this._LineColor3 = value;
                    this.OnPropertyChanged("LineColor3",original,value);

                }
            }
        }

        String _LineColor4;
        /// <summary>
        /// 
        /// </summary>
        public virtual String LineColor4
        {
            get
            {
                return this._LineColor4;
            }
            set
            {
                if ((this._LineColor4 != value))
                {
                    var original = this._LineColor4;
                    this._LineColor4 = value;
                    this.OnPropertyChanged("LineColor4",original,value);

                }
            }
        }

        String _LineColor5;
        /// <summary>
        /// 
        /// </summary>
        public virtual String LineColor5
        {
            get
            {
                return this._LineColor5;
            }
            set
            {
                if ((this._LineColor5 != value))
                {
                    var original = this._LineColor5;
                    this._LineColor5 = value;
                    this.OnPropertyChanged("LineColor5",original,value);

                }
            }
        }

        String _LineColor6;
        /// <summary>
        /// 
        /// </summary>
        public virtual String LineColor6
        {
            get
            {
                return this._LineColor6;
            }
            set
            {
                if ((this._LineColor6 != value))
                {
                    var original = this._LineColor6;
                    this._LineColor6 = value;
                    this.OnPropertyChanged("LineColor6",original,value);

                }
            }
        }

        String _LineColor7;
        /// <summary>
        /// 
        /// </summary>
        public virtual String LineColor7
        {
            get
            {
                return this._LineColor7;
            }
            set
            {
                if ((this._LineColor7 != value))
                {
                    var original = this._LineColor7;
                    this._LineColor7 = value;
                    this.OnPropertyChanged("LineColor7",original,value);

                }
            }
        }

        String _LineColor8;
        /// <summary>
        /// 
        /// </summary>
        public virtual String LineColor8
        {
            get
            {
                return this._LineColor8;
            }
            set
            {
                if ((this._LineColor8 != value))
                {
                    var original = this._LineColor8;
                    this._LineColor8 = value;
                    this.OnPropertyChanged("LineColor8",original,value);

                }
            }
        }

        String _LineColor9;
        /// <summary>
        /// 
        /// </summary>
        public virtual String LineColor9
        {
            get
            {
                return this._LineColor9;
            }
            set
            {
                if ((this._LineColor9 != value))
                {
                    var original = this._LineColor9;
                    this._LineColor9 = value;
                    this.OnPropertyChanged("LineColor9",original,value);

                }
            }
        }

        String _LineColor10;
        /// <summary>
        /// 
        /// </summary>
        public virtual String LineColor10
        {
            get
            {
                return this._LineColor10;
            }
            set
            {
                if ((this._LineColor10 != value))
                {
                    var original = this._LineColor10;
                    this._LineColor10 = value;
                    this.OnPropertyChanged("LineColor10",original,value);

                }
            }
        }

        String _LineColor11;
        /// <summary>
        /// 
        /// </summary>
        public virtual String LineColor11
        {
            get
            {
                return this._LineColor11;
            }
            set
            {
                if ((this._LineColor11 != value))
                {
                    var original = this._LineColor11;
                    this._LineColor11 = value;
                    this.OnPropertyChanged("LineColor11",original,value);

                }
            }
        }

        String _LineColor12;
        /// <summary>
        /// 
        /// </summary>
        public virtual String LineColor12
        {
            get
            {
                return this._LineColor12;
            }
            set
            {
                if ((this._LineColor12 != value))
                {
                    var original = this._LineColor12;
                    this._LineColor12 = value;
                    this.OnPropertyChanged("LineColor12",original,value);

                }
            }
        }

        System.Nullable<Int32> _ForwardCount=5;
        /// <summary>
        /// 向前浏览画面数量
        /// </summary>
        public virtual System.Nullable<Int32> ForwardCount
        {
            get
            {
                return this._ForwardCount;
            }
            set
            {
                if ((this._ForwardCount != value))
                {
                    var original = this._ForwardCount;
                    this._ForwardCount = value;
                    this.OnPropertyChanged("ForwardCount",original,value);

                }
            }
        }

        System.Nullable<Int32> _MaxOpen=8;
        /// <summary>
        /// 最多打开画面数量
        /// </summary>
        public virtual System.Nullable<Int32> MaxOpen
        {
            get
            {
                return this._MaxOpen;
            }
            set
            {
                if ((this._MaxOpen != value))
                {
                    var original = this._MaxOpen;
                    this._MaxOpen = value;
                    this.OnPropertyChanged("MaxOpen",original,value);

                }
            }
        }

        System.Nullable<Int32> _DialogCount=5;
        /// <summary>
        /// 弹出窗口数量
        /// </summary>
        public virtual System.Nullable<Int32> DialogCount
        {
            get
            {
                return this._DialogCount;
            }
            set
            {
                if ((this._DialogCount != value))
                {
                    var original = this._DialogCount;
                    this._DialogCount = value;
                    this.OnPropertyChanged("DialogCount",original,value);

                }
            }
        }

        System.Nullable<Boolean> _IsLockDialog=true;
        /// <summary>
        /// 锁定弹出窗口
        /// </summary>
        public virtual System.Nullable<Boolean> IsLockDialog
        {
            get
            {
                return this._IsLockDialog;
            }
            set
            {
                if ((this._IsLockDialog != value))
                {
                    var original = this._IsLockDialog;
                    this._IsLockDialog = value;
                    this.OnPropertyChanged("IsLockDialog",original,value);

                }
            }
        }

        System.Nullable<double> _AlarmShinePercent;
        /// <summary>
        /// 窗口闪烁行的百分比
        /// </summary>
        public virtual System.Nullable<double> AlarmShinePercent
        {
            get
            {
                return this._AlarmShinePercent;
            }
            set
            {
                if ((this._AlarmShinePercent != value))
                {
                    var original = this._AlarmShinePercent;
                    this._AlarmShinePercent = value;
                    this.OnPropertyChanged("AlarmShinePercent",original,value);

                }
            }
        }

        String _AlarmStatusChangeColor;
        /// <summary>
        /// 状态变化报警颜色
        /// </summary>
        public virtual String AlarmStatusChangeColor
        {
            get
            {
                return this._AlarmStatusChangeColor;
            }
            set
            {
                if ((this._AlarmStatusChangeColor != value))
                {
                    var original = this._AlarmStatusChangeColor;
                    this._AlarmStatusChangeColor = value;
                    this.OnPropertyChanged("AlarmStatusChangeColor",original,value);

                }
            }
        }

        String _AlarmTimeoutColor;
        /// <summary>
        /// 控制站超时颜色
        /// </summary>
        public virtual String AlarmTimeoutColor
        {
            get
            {
                return this._AlarmTimeoutColor;
            }
            set
            {
                if ((this._AlarmTimeoutColor != value))
                {
                    var original = this._AlarmTimeoutColor;
                    this._AlarmTimeoutColor = value;
                    this.OnPropertyChanged("AlarmTimeoutColor",original,value);

                }
            }
        }

        String _AlarmPTimeoutColor;
        /// <summary>
        /// 点超时颜色
        /// </summary>
        public virtual String AlarmPTimeoutColor
        {
            get
            {
                return this._AlarmPTimeoutColor;
            }
            set
            {
                if ((this._AlarmPTimeoutColor != value))
                {
                    var original = this._AlarmPTimeoutColor;
                    this._AlarmPTimeoutColor = value;
                    this.OnPropertyChanged("AlarmPTimeoutColor",original,value);

                }
            }
        }

        String _UnConfigAlarmColor1;
        /// <summary>
        /// 未确认报警颜色
        /// </summary>
        public virtual String UnConfigAlarmColor1
        {
            get
            {
                return this._UnConfigAlarmColor1;
            }
            set
            {
                if ((this._UnConfigAlarmColor1 != value))
                {
                    var original = this._UnConfigAlarmColor1;
                    this._UnConfigAlarmColor1 = value;
                    this.OnPropertyChanged("UnConfigAlarmColor1",original,value);

                }
            }
        }

        String _UnConfigAlarmColor2;
        /// <summary>
        /// 
        /// </summary>
        public virtual String UnConfigAlarmColor2
        {
            get
            {
                return this._UnConfigAlarmColor2;
            }
            set
            {
                if ((this._UnConfigAlarmColor2 != value))
                {
                    var original = this._UnConfigAlarmColor2;
                    this._UnConfigAlarmColor2 = value;
                    this.OnPropertyChanged("UnConfigAlarmColor2",original,value);

                }
            }
        }

        String _UnConfigAlarmColor3;
        /// <summary>
        /// 
        /// </summary>
        public virtual String UnConfigAlarmColor3
        {
            get
            {
                return this._UnConfigAlarmColor3;
            }
            set
            {
                if ((this._UnConfigAlarmColor3 != value))
                {
                    var original = this._UnConfigAlarmColor3;
                    this._UnConfigAlarmColor3 = value;
                    this.OnPropertyChanged("UnConfigAlarmColor3",original,value);

                }
            }
        }

        String _UnConfigAlarmColor4;
        /// <summary>
        /// 
        /// </summary>
        public virtual String UnConfigAlarmColor4
        {
            get
            {
                return this._UnConfigAlarmColor4;
            }
            set
            {
                if ((this._UnConfigAlarmColor4 != value))
                {
                    var original = this._UnConfigAlarmColor4;
                    this._UnConfigAlarmColor4 = value;
                    this.OnPropertyChanged("UnConfigAlarmColor4",original,value);

                }
            }
        }

        String _UnConfigAlarmColor5;
        /// <summary>
        /// 
        /// </summary>
        public virtual String UnConfigAlarmColor5
        {
            get
            {
                return this._UnConfigAlarmColor5;
            }
            set
            {
                if ((this._UnConfigAlarmColor5 != value))
                {
                    var original = this._UnConfigAlarmColor5;
                    this._UnConfigAlarmColor5 = value;
                    this.OnPropertyChanged("UnConfigAlarmColor5",original,value);

                }
            }
        }

        String _UnConfigAlarmColor6;
        /// <summary>
        /// 
        /// </summary>
        public virtual String UnConfigAlarmColor6
        {
            get
            {
                return this._UnConfigAlarmColor6;
            }
            set
            {
                if ((this._UnConfigAlarmColor6 != value))
                {
                    var original = this._UnConfigAlarmColor6;
                    this._UnConfigAlarmColor6 = value;
                    this.OnPropertyChanged("UnConfigAlarmColor6",original,value);

                }
            }
        }

        String _UnConfigAlarmColor7;
        /// <summary>
        /// 
        /// </summary>
        public virtual String UnConfigAlarmColor7
        {
            get
            {
                return this._UnConfigAlarmColor7;
            }
            set
            {
                if ((this._UnConfigAlarmColor7 != value))
                {
                    var original = this._UnConfigAlarmColor7;
                    this._UnConfigAlarmColor7 = value;
                    this.OnPropertyChanged("UnConfigAlarmColor7",original,value);

                }
            }
        }

        String _UnConfigAlarmColor8;
        /// <summary>
        /// 
        /// </summary>
        public virtual String UnConfigAlarmColor8
        {
            get
            {
                return this._UnConfigAlarmColor8;
            }
            set
            {
                if ((this._UnConfigAlarmColor8 != value))
                {
                    var original = this._UnConfigAlarmColor8;
                    this._UnConfigAlarmColor8 = value;
                    this.OnPropertyChanged("UnConfigAlarmColor8",original,value);

                }
            }
        }

        String _ConfigAlarmColor1;
        /// <summary>
        /// 确认报警颜色
        /// </summary>
        public virtual String ConfigAlarmColor1
        {
            get
            {
                return this._ConfigAlarmColor1;
            }
            set
            {
                if ((this._ConfigAlarmColor1 != value))
                {
                    var original = this._ConfigAlarmColor1;
                    this._ConfigAlarmColor1 = value;
                    this.OnPropertyChanged("ConfigAlarmColor1",original,value);

                }
            }
        }

        String _ConfigAlarmColor2;
        /// <summary>
        /// 
        /// </summary>
        public virtual String ConfigAlarmColor2
        {
            get
            {
                return this._ConfigAlarmColor2;
            }
            set
            {
                if ((this._ConfigAlarmColor2 != value))
                {
                    var original = this._ConfigAlarmColor2;
                    this._ConfigAlarmColor2 = value;
                    this.OnPropertyChanged("ConfigAlarmColor2",original,value);

                }
            }
        }

        String _ConfigAlarmColor3;
        /// <summary>
        /// 
        /// </summary>
        public virtual String ConfigAlarmColor3
        {
            get
            {
                return this._ConfigAlarmColor3;
            }
            set
            {
                if ((this._ConfigAlarmColor3 != value))
                {
                    var original = this._ConfigAlarmColor3;
                    this._ConfigAlarmColor3 = value;
                    this.OnPropertyChanged("ConfigAlarmColor3",original,value);

                }
            }
        }

        String _ConfigAlarmColor4;
        /// <summary>
        /// 
        /// </summary>
        public virtual String ConfigAlarmColor4
        {
            get
            {
                return this._ConfigAlarmColor4;
            }
            set
            {
                if ((this._ConfigAlarmColor4 != value))
                {
                    var original = this._ConfigAlarmColor4;
                    this._ConfigAlarmColor4 = value;
                    this.OnPropertyChanged("ConfigAlarmColor4",original,value);

                }
            }
        }

        String _ConfigAlarmColor5;
        /// <summary>
        /// 
        /// </summary>
        public virtual String ConfigAlarmColor5
        {
            get
            {
                return this._ConfigAlarmColor5;
            }
            set
            {
                if ((this._ConfigAlarmColor5 != value))
                {
                    var original = this._ConfigAlarmColor5;
                    this._ConfigAlarmColor5 = value;
                    this.OnPropertyChanged("ConfigAlarmColor5",original,value);

                }
            }
        }

        String _ConfigAlarmColor6;
        /// <summary>
        /// 
        /// </summary>
        public virtual String ConfigAlarmColor6
        {
            get
            {
                return this._ConfigAlarmColor6;
            }
            set
            {
                if ((this._ConfigAlarmColor6 != value))
                {
                    var original = this._ConfigAlarmColor6;
                    this._ConfigAlarmColor6 = value;
                    this.OnPropertyChanged("ConfigAlarmColor6",original,value);

                }
            }
        }

        String _ConfigAlarmColor7;
        /// <summary>
        /// 
        /// </summary>
        public virtual String ConfigAlarmColor7
        {
            get
            {
                return this._ConfigAlarmColor7;
            }
            set
            {
                if ((this._ConfigAlarmColor7 != value))
                {
                    var original = this._ConfigAlarmColor7;
                    this._ConfigAlarmColor7 = value;
                    this.OnPropertyChanged("ConfigAlarmColor7",original,value);

                }
            }
        }

        String _ConfigAlarmColor8;
        /// <summary>
        /// 
        /// </summary>
        public virtual String ConfigAlarmColor8
        {
            get
            {
                return this._ConfigAlarmColor8;
            }
            set
            {
                if ((this._ConfigAlarmColor8 != value))
                {
                    var original = this._ConfigAlarmColor8;
                    this._ConfigAlarmColor8 = value;
                    this.OnPropertyChanged("ConfigAlarmColor8",original,value);

                }
            }
        }

        String _UnBackAlarmColor1;
        /// <summary>
        /// 未确认返回颜色
        /// </summary>
        public virtual String UnBackAlarmColor1
        {
            get
            {
                return this._UnBackAlarmColor1;
            }
            set
            {
                if ((this._UnBackAlarmColor1 != value))
                {
                    var original = this._UnBackAlarmColor1;
                    this._UnBackAlarmColor1 = value;
                    this.OnPropertyChanged("UnBackAlarmColor1",original,value);

                }
            }
        }

        String _UnBackAlarmColor2;
        /// <summary>
        /// 
        /// </summary>
        public virtual String UnBackAlarmColor2
        {
            get
            {
                return this._UnBackAlarmColor2;
            }
            set
            {
                if ((this._UnBackAlarmColor2 != value))
                {
                    var original = this._UnBackAlarmColor2;
                    this._UnBackAlarmColor2 = value;
                    this.OnPropertyChanged("UnBackAlarmColor2",original,value);

                }
            }
        }

        String _UnBackAlarmColor3;
        /// <summary>
        /// 
        /// </summary>
        public virtual String UnBackAlarmColor3
        {
            get
            {
                return this._UnBackAlarmColor3;
            }
            set
            {
                if ((this._UnBackAlarmColor3 != value))
                {
                    var original = this._UnBackAlarmColor3;
                    this._UnBackAlarmColor3 = value;
                    this.OnPropertyChanged("UnBackAlarmColor3",original,value);

                }
            }
        }

        String _UnBackAlarmColor4;
        /// <summary>
        /// 
        /// </summary>
        public virtual String UnBackAlarmColor4
        {
            get
            {
                return this._UnBackAlarmColor4;
            }
            set
            {
                if ((this._UnBackAlarmColor4 != value))
                {
                    var original = this._UnBackAlarmColor4;
                    this._UnBackAlarmColor4 = value;
                    this.OnPropertyChanged("UnBackAlarmColor4",original,value);

                }
            }
        }

        String _UnBackAlarmColor5;
        /// <summary>
        /// 
        /// </summary>
        public virtual String UnBackAlarmColor5
        {
            get
            {
                return this._UnBackAlarmColor5;
            }
            set
            {
                if ((this._UnBackAlarmColor5 != value))
                {
                    var original = this._UnBackAlarmColor5;
                    this._UnBackAlarmColor5 = value;
                    this.OnPropertyChanged("UnBackAlarmColor5",original,value);

                }
            }
        }

        String _UnBackAlarmColor6;
        /// <summary>
        /// 
        /// </summary>
        public virtual String UnBackAlarmColor6
        {
            get
            {
                return this._UnBackAlarmColor6;
            }
            set
            {
                if ((this._UnBackAlarmColor6 != value))
                {
                    var original = this._UnBackAlarmColor6;
                    this._UnBackAlarmColor6 = value;
                    this.OnPropertyChanged("UnBackAlarmColor6",original,value);

                }
            }
        }

        String _UnBackAlarmColor7;
        /// <summary>
        /// 
        /// </summary>
        public virtual String UnBackAlarmColor7
        {
            get
            {
                return this._UnBackAlarmColor7;
            }
            set
            {
                if ((this._UnBackAlarmColor7 != value))
                {
                    var original = this._UnBackAlarmColor7;
                    this._UnBackAlarmColor7 = value;
                    this.OnPropertyChanged("UnBackAlarmColor7",original,value);

                }
            }
        }

        String _UnBackAlarmColor8;
        /// <summary>
        /// 
        /// </summary>
        public virtual String UnBackAlarmColor8
        {
            get
            {
                return this._UnBackAlarmColor8;
            }
            set
            {
                if ((this._UnBackAlarmColor8 != value))
                {
                    var original = this._UnBackAlarmColor8;
                    this._UnBackAlarmColor8 = value;
                    this.OnPropertyChanged("UnBackAlarmColor8",original,value);

                }
            }
        }

        String _BackAlarmColor1;
        /// <summary>
        /// 确认返回颜色
        /// </summary>
        public virtual String BackAlarmColor1
        {
            get
            {
                return this._BackAlarmColor1;
            }
            set
            {
                if ((this._BackAlarmColor1 != value))
                {
                    var original = this._BackAlarmColor1;
                    this._BackAlarmColor1 = value;
                    this.OnPropertyChanged("BackAlarmColor1",original,value);

                }
            }
        }

        String _BackAlarmColor2;
        /// <summary>
        /// 
        /// </summary>
        public virtual String BackAlarmColor2
        {
            get
            {
                return this._BackAlarmColor2;
            }
            set
            {
                if ((this._BackAlarmColor2 != value))
                {
                    var original = this._BackAlarmColor2;
                    this._BackAlarmColor2 = value;
                    this.OnPropertyChanged("BackAlarmColor2",original,value);

                }
            }
        }

        String _BackAlarmColor3;
        /// <summary>
        /// 
        /// </summary>
        public virtual String BackAlarmColor3
        {
            get
            {
                return this._BackAlarmColor3;
            }
            set
            {
                if ((this._BackAlarmColor3 != value))
                {
                    var original = this._BackAlarmColor3;
                    this._BackAlarmColor3 = value;
                    this.OnPropertyChanged("BackAlarmColor3",original,value);

                }
            }
        }

        String _BackAlarmColor4;
        /// <summary>
        /// 
        /// </summary>
        public virtual String BackAlarmColor4
        {
            get
            {
                return this._BackAlarmColor4;
            }
            set
            {
                if ((this._BackAlarmColor4 != value))
                {
                    var original = this._BackAlarmColor4;
                    this._BackAlarmColor4 = value;
                    this.OnPropertyChanged("BackAlarmColor4",original,value);

                }
            }
        }

        String _BackAlarmColor5;
        /// <summary>
        /// 
        /// </summary>
        public virtual String BackAlarmColor5
        {
            get
            {
                return this._BackAlarmColor5;
            }
            set
            {
                if ((this._BackAlarmColor5 != value))
                {
                    var original = this._BackAlarmColor5;
                    this._BackAlarmColor5 = value;
                    this.OnPropertyChanged("BackAlarmColor5",original,value);

                }
            }
        }

        String _BackAlarmColor6;
        /// <summary>
        /// 
        /// </summary>
        public virtual String BackAlarmColor6
        {
            get
            {
                return this._BackAlarmColor6;
            }
            set
            {
                if ((this._BackAlarmColor6 != value))
                {
                    var original = this._BackAlarmColor6;
                    this._BackAlarmColor6 = value;
                    this.OnPropertyChanged("BackAlarmColor6",original,value);

                }
            }
        }

        String _BackAlarmColor7;
        /// <summary>
        /// 
        /// </summary>
        public virtual String BackAlarmColor7
        {
            get
            {
                return this._BackAlarmColor7;
            }
            set
            {
                if ((this._BackAlarmColor7 != value))
                {
                    var original = this._BackAlarmColor7;
                    this._BackAlarmColor7 = value;
                    this.OnPropertyChanged("BackAlarmColor7",original,value);

                }
            }
        }

        String _BackAlarmColor8;
        /// <summary>
        /// 
        /// </summary>
        public virtual String BackAlarmColor8
        {
            get
            {
                return this._BackAlarmColor8;
            }
            set
            {
                if ((this._BackAlarmColor8 != value))
                {
                    var original = this._BackAlarmColor8;
                    this._BackAlarmColor8 = value;
                    this.OnPropertyChanged("BackAlarmColor8",original,value);

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
    public class DevicePointFolder :Way.Lib.DataModel
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

        System.Nullable<Int32> _ParentId=0;
        /// <summary>
        /// 
        /// </summary>
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
                    var original = this._ParentId;
                    this._ParentId = value;
                    this.OnPropertyChanged("ParentId",original,value);

                }
            }
        }

        System.Nullable<Int32> _DeviceId;
        /// <summary>
        /// 
        /// </summary>
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
                    var original = this._DeviceId;
                    this._DeviceId = value;
                    this.OnPropertyChanged("DeviceId",original,value);

                }
            }
        }

        System.Nullable<DevicePointFolder_TypeEnum> _Type;
        /// <summary>
        /// 
        /// </summary>
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
                    var original = this._Type;
                    this._Type = value;
                    this.OnPropertyChanged("Type",original,value);

                }
            }
        }
}}
namespace SunRizServer{


    /// <summary>
	/// 监控画面
	/// </summary>
    public class ControlWindow :Way.Lib.DataModel
    {

        /// <summary>
	    /// 
	    /// </summary>
        public  ControlWindow()
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

        System.Nullable<Int32> _ControlUnitId;
        /// <summary>
        /// 控制单元id
        /// </summary>
        public virtual System.Nullable<Int32> ControlUnitId
        {
            get
            {
                return this._ControlUnitId;
            }
            set
            {
                if ((this._ControlUnitId != value))
                {
                    var original = this._ControlUnitId;
                    this._ControlUnitId = value;
                    this.OnPropertyChanged("ControlUnitId",original,value);

                }
            }
        }

        String _Name;
        /// <summary>
        /// 名称
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

        System.Nullable<Int32> _FolderId;
        /// <summary>
        /// 所属文件夹
        /// </summary>
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
                    var original = this._FolderId;
                    this._FolderId = value;
                    this.OnPropertyChanged("FolderId",original,value);

                }
            }
        }

        String _FilePath;
        /// <summary>
        /// 文件名
        /// </summary>
        public virtual String FilePath
        {
            get
            {
                return this._FilePath;
            }
            set
            {
                if ((this._FilePath != value))
                {
                    var original = this._FilePath;
                    this._FilePath = value;
                    this.OnPropertyChanged("FilePath",original,value);

                }
            }
        }

        String _Code;
        /// <summary>
        /// 编号
        /// </summary>
        public virtual String Code
        {
            get
            {
                return this._Code;
            }
            set
            {
                if ((this._Code != value))
                {
                    var original = this._Code;
                    this._Code = value;
                    this.OnPropertyChanged("Code",original,value);

                }
            }
        }

        System.Nullable<Int32> _windowWidth;
        /// <summary>
        /// 
        /// </summary>
        public virtual System.Nullable<Int32> windowWidth
        {
            get
            {
                return this._windowWidth;
            }
            set
            {
                if ((this._windowWidth != value))
                {
                    var original = this._windowWidth;
                    this._windowWidth = value;
                    this.OnPropertyChanged("windowWidth",original,value);

                }
            }
        }

        System.Nullable<Int32> _windowHeight;
        /// <summary>
        /// 
        /// </summary>
        public virtual System.Nullable<Int32> windowHeight
        {
            get
            {
                return this._windowHeight;
            }
            set
            {
                if ((this._windowHeight != value))
                {
                    var original = this._windowHeight;
                    this._windowHeight = value;
                    this.OnPropertyChanged("windowHeight",original,value);

                }
            }
        }

        System.Nullable<Boolean> _IsStartup=false;
        /// <summary>
        /// 是否是启动画面
        /// </summary>
        public virtual System.Nullable<Boolean> IsStartup
        {
            get
            {
                return this._IsStartup;
            }
            set
            {
                if ((this._IsStartup != value))
                {
                    var original = this._IsStartup;
                    this._IsStartup = value;
                    this.OnPropertyChanged("IsStartup",original,value);

                }
            }
        }
}}
namespace SunRizServer{


    /// <summary>
	/// 监视画面文件夹
	/// </summary>
    public class ControlWindowFolder :Way.Lib.DataModel
    {

        /// <summary>
	    /// 
	    /// </summary>
        public  ControlWindowFolder()
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

        System.Nullable<Int32> _ControlUnitId;
        /// <summary>
        /// 控制单元id
        /// </summary>
        public virtual System.Nullable<Int32> ControlUnitId
        {
            get
            {
                return this._ControlUnitId;
            }
            set
            {
                if ((this._ControlUnitId != value))
                {
                    var original = this._ControlUnitId;
                    this._ControlUnitId = value;
                    this.OnPropertyChanged("ControlUnitId",original,value);

                }
            }
        }

        String _Name;
        /// <summary>
        /// 名称
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

        System.Nullable<Int32> _ParentId=0;
        /// <summary>
        /// 
        /// </summary>
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
                    var original = this._ParentId;
                    this._ParentId = value;
                    this.OnPropertyChanged("ParentId",original,value);

                }
            }
        }
}}
namespace SunRizServer{


    /// <summary>
	/// 记录子窗体
	/// </summary>
    public class ChildWindow :Way.Lib.DataModel
    {

        /// <summary>
	    /// 
	    /// </summary>
        public  ChildWindow()
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

        System.Nullable<Int32> _WindowId;
        /// <summary>
        /// 
        /// </summary>
        public virtual System.Nullable<Int32> WindowId
        {
            get
            {
                return this._WindowId;
            }
            set
            {
                if ((this._WindowId != value))
                {
                    var original = this._WindowId;
                    this._WindowId = value;
                    this.OnPropertyChanged("WindowId",original,value);

                }
            }
        }

        System.Nullable<Int32> _ChildWindowId;
        /// <summary>
        /// 
        /// </summary>
        public virtual System.Nullable<Int32> ChildWindowId
        {
            get
            {
                return this._ChildWindowId;
            }
            set
            {
                if ((this._ChildWindowId != value))
                {
                    var original = this._ChildWindowId;
                    this._ChildWindowId = value;
                    this.OnPropertyChanged("ChildWindowId",original,value);

                }
            }
        }
}}
namespace SunRizServer{


    /// <summary>
	/// 系统配置
	/// </summary>
    public class SystemSetting :Way.Lib.DataModel
    {

        /// <summary>
	    /// 
	    /// </summary>
        public  SystemSetting()
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

        System.Nullable<double> _Max;
        /// <summary>
        /// 量程上限
        /// </summary>
        public virtual System.Nullable<double> Max
        {
            get
            {
                return this._Max;
            }
            set
            {
                if ((this._Max != value))
                {
                    var original = this._Max;
                    this._Max = value;
                    this.OnPropertyChanged("Max",original,value);

                }
            }
        }

        System.Nullable<double> _Min;
        /// <summary>
        /// 量程下限
        /// </summary>
        public virtual System.Nullable<double> Min
        {
            get
            {
                return this._Min;
            }
            set
            {
                if ((this._Min != value))
                {
                    var original = this._Min;
                    this._Min = value;
                    this.OnPropertyChanged("Min",original,value);

                }
            }
        }

        String _WinBgColor;
        /// <summary>
        /// 窗口背景色
        /// </summary>
        public virtual String WinBgColor
        {
            get
            {
                return this._WinBgColor;
            }
            set
            {
                if ((this._WinBgColor != value))
                {
                    var original = this._WinBgColor;
                    this._WinBgColor = value;
                    this.OnPropertyChanged("WinBgColor",original,value);

                }
            }
        }

        String _WinFrontColor;
        /// <summary>
        /// 窗口前景色
        /// </summary>
        public virtual String WinFrontColor
        {
            get
            {
                return this._WinFrontColor;
            }
            set
            {
                if ((this._WinFrontColor != value))
                {
                    var original = this._WinFrontColor;
                    this._WinFrontColor = value;
                    this.OnPropertyChanged("WinFrontColor",original,value);

                }
            }
        }

        String _GridColor;
        /// <summary>
        /// 网格颜色
        /// </summary>
        public virtual String GridColor
        {
            get
            {
                return this._GridColor;
            }
            set
            {
                if ((this._GridColor != value))
                {
                    var original = this._GridColor;
                    this._GridColor = value;
                    this.OnPropertyChanged("GridColor",original,value);

                }
            }
        }

        String _LineColor1;
        /// <summary>
        /// 点颜色
        /// </summary>
        public virtual String LineColor1
        {
            get
            {
                return this._LineColor1;
            }
            set
            {
                if ((this._LineColor1 != value))
                {
                    var original = this._LineColor1;
                    this._LineColor1 = value;
                    this.OnPropertyChanged("LineColor1",original,value);

                }
            }
        }

        String _LineColor2;
        /// <summary>
        /// 
        /// </summary>
        public virtual String LineColor2
        {
            get
            {
                return this._LineColor2;
            }
            set
            {
                if ((this._LineColor2 != value))
                {
                    var original = this._LineColor2;
                    this._LineColor2 = value;
                    this.OnPropertyChanged("LineColor2",original,value);

                }
            }
        }

        String _LineColor3;
        /// <summary>
        /// 
        /// </summary>
        public virtual String LineColor3
        {
            get
            {
                return this._LineColor3;
            }
            set
            {
                if ((this._LineColor3 != value))
                {
                    var original = this._LineColor3;
                    this._LineColor3 = value;
                    this.OnPropertyChanged("LineColor3",original,value);

                }
            }
        }

        String _LineColor4;
        /// <summary>
        /// 
        /// </summary>
        public virtual String LineColor4
        {
            get
            {
                return this._LineColor4;
            }
            set
            {
                if ((this._LineColor4 != value))
                {
                    var original = this._LineColor4;
                    this._LineColor4 = value;
                    this.OnPropertyChanged("LineColor4",original,value);

                }
            }
        }

        String _LineColor5;
        /// <summary>
        /// 
        /// </summary>
        public virtual String LineColor5
        {
            get
            {
                return this._LineColor5;
            }
            set
            {
                if ((this._LineColor5 != value))
                {
                    var original = this._LineColor5;
                    this._LineColor5 = value;
                    this.OnPropertyChanged("LineColor5",original,value);

                }
            }
        }

        String _LineColor6;
        /// <summary>
        /// 
        /// </summary>
        public virtual String LineColor6
        {
            get
            {
                return this._LineColor6;
            }
            set
            {
                if ((this._LineColor6 != value))
                {
                    var original = this._LineColor6;
                    this._LineColor6 = value;
                    this.OnPropertyChanged("LineColor6",original,value);

                }
            }
        }

        String _LineColor7;
        /// <summary>
        /// 
        /// </summary>
        public virtual String LineColor7
        {
            get
            {
                return this._LineColor7;
            }
            set
            {
                if ((this._LineColor7 != value))
                {
                    var original = this._LineColor7;
                    this._LineColor7 = value;
                    this.OnPropertyChanged("LineColor7",original,value);

                }
            }
        }

        String _LineColor8;
        /// <summary>
        /// 
        /// </summary>
        public virtual String LineColor8
        {
            get
            {
                return this._LineColor8;
            }
            set
            {
                if ((this._LineColor8 != value))
                {
                    var original = this._LineColor8;
                    this._LineColor8 = value;
                    this.OnPropertyChanged("LineColor8",original,value);

                }
            }
        }

        String _LineColor9;
        /// <summary>
        /// 
        /// </summary>
        public virtual String LineColor9
        {
            get
            {
                return this._LineColor9;
            }
            set
            {
                if ((this._LineColor9 != value))
                {
                    var original = this._LineColor9;
                    this._LineColor9 = value;
                    this.OnPropertyChanged("LineColor9",original,value);

                }
            }
        }

        String _LineColor10;
        /// <summary>
        /// 
        /// </summary>
        public virtual String LineColor10
        {
            get
            {
                return this._LineColor10;
            }
            set
            {
                if ((this._LineColor10 != value))
                {
                    var original = this._LineColor10;
                    this._LineColor10 = value;
                    this.OnPropertyChanged("LineColor10",original,value);

                }
            }
        }

        String _LineColor11;
        /// <summary>
        /// 
        /// </summary>
        public virtual String LineColor11
        {
            get
            {
                return this._LineColor11;
            }
            set
            {
                if ((this._LineColor11 != value))
                {
                    var original = this._LineColor11;
                    this._LineColor11 = value;
                    this.OnPropertyChanged("LineColor11",original,value);

                }
            }
        }

        String _LineColor12;
        /// <summary>
        /// 
        /// </summary>
        public virtual String LineColor12
        {
            get
            {
                return this._LineColor12;
            }
            set
            {
                if ((this._LineColor12 != value))
                {
                    var original = this._LineColor12;
                    this._LineColor12 = value;
                    this.OnPropertyChanged("LineColor12",original,value);

                }
            }
        }

        System.Nullable<Int32> _ForwardCount=5;
        /// <summary>
        /// 向前浏览画面数量
        /// </summary>
        public virtual System.Nullable<Int32> ForwardCount
        {
            get
            {
                return this._ForwardCount;
            }
            set
            {
                if ((this._ForwardCount != value))
                {
                    var original = this._ForwardCount;
                    this._ForwardCount = value;
                    this.OnPropertyChanged("ForwardCount",original,value);

                }
            }
        }

        System.Nullable<Int32> _MaxOpen=8;
        /// <summary>
        /// 最多打开画面数量
        /// </summary>
        public virtual System.Nullable<Int32> MaxOpen
        {
            get
            {
                return this._MaxOpen;
            }
            set
            {
                if ((this._MaxOpen != value))
                {
                    var original = this._MaxOpen;
                    this._MaxOpen = value;
                    this.OnPropertyChanged("MaxOpen",original,value);

                }
            }
        }

        System.Nullable<Int32> _DialogCount=5;
        /// <summary>
        /// 弹出窗口数量
        /// </summary>
        public virtual System.Nullable<Int32> DialogCount
        {
            get
            {
                return this._DialogCount;
            }
            set
            {
                if ((this._DialogCount != value))
                {
                    var original = this._DialogCount;
                    this._DialogCount = value;
                    this.OnPropertyChanged("DialogCount",original,value);

                }
            }
        }

        System.Nullable<Boolean> _IsLockDialog=true;
        /// <summary>
        /// 锁定弹出窗口
        /// </summary>
        public virtual System.Nullable<Boolean> IsLockDialog
        {
            get
            {
                return this._IsLockDialog;
            }
            set
            {
                if ((this._IsLockDialog != value))
                {
                    var original = this._IsLockDialog;
                    this._IsLockDialog = value;
                    this.OnPropertyChanged("IsLockDialog",original,value);

                }
            }
        }

        System.Nullable<double> _AlarmShinePercent;
        /// <summary>
        /// 窗口闪烁行的百分比
        /// </summary>
        public virtual System.Nullable<double> AlarmShinePercent
        {
            get
            {
                return this._AlarmShinePercent;
            }
            set
            {
                if ((this._AlarmShinePercent != value))
                {
                    var original = this._AlarmShinePercent;
                    this._AlarmShinePercent = value;
                    this.OnPropertyChanged("AlarmShinePercent",original,value);

                }
            }
        }

        String _AlarmStatusChangeColor;
        /// <summary>
        /// 状态变化报警颜色
        /// </summary>
        public virtual String AlarmStatusChangeColor
        {
            get
            {
                return this._AlarmStatusChangeColor;
            }
            set
            {
                if ((this._AlarmStatusChangeColor != value))
                {
                    var original = this._AlarmStatusChangeColor;
                    this._AlarmStatusChangeColor = value;
                    this.OnPropertyChanged("AlarmStatusChangeColor",original,value);

                }
            }
        }

        String _AlarmTimeoutColor;
        /// <summary>
        /// 控制站超时颜色
        /// </summary>
        public virtual String AlarmTimeoutColor
        {
            get
            {
                return this._AlarmTimeoutColor;
            }
            set
            {
                if ((this._AlarmTimeoutColor != value))
                {
                    var original = this._AlarmTimeoutColor;
                    this._AlarmTimeoutColor = value;
                    this.OnPropertyChanged("AlarmTimeoutColor",original,value);

                }
            }
        }

        String _AlarmPTimeoutColor;
        /// <summary>
        /// 点超时颜色
        /// </summary>
        public virtual String AlarmPTimeoutColor
        {
            get
            {
                return this._AlarmPTimeoutColor;
            }
            set
            {
                if ((this._AlarmPTimeoutColor != value))
                {
                    var original = this._AlarmPTimeoutColor;
                    this._AlarmPTimeoutColor = value;
                    this.OnPropertyChanged("AlarmPTimeoutColor",original,value);

                }
            }
        }

        String _UnConfigAlarmColor1;
        /// <summary>
        /// 未确认报警颜色
        /// </summary>
        public virtual String UnConfigAlarmColor1
        {
            get
            {
                return this._UnConfigAlarmColor1;
            }
            set
            {
                if ((this._UnConfigAlarmColor1 != value))
                {
                    var original = this._UnConfigAlarmColor1;
                    this._UnConfigAlarmColor1 = value;
                    this.OnPropertyChanged("UnConfigAlarmColor1",original,value);

                }
            }
        }

        String _UnConfigAlarmColor2;
        /// <summary>
        /// 
        /// </summary>
        public virtual String UnConfigAlarmColor2
        {
            get
            {
                return this._UnConfigAlarmColor2;
            }
            set
            {
                if ((this._UnConfigAlarmColor2 != value))
                {
                    var original = this._UnConfigAlarmColor2;
                    this._UnConfigAlarmColor2 = value;
                    this.OnPropertyChanged("UnConfigAlarmColor2",original,value);

                }
            }
        }

        String _UnConfigAlarmColor3;
        /// <summary>
        /// 
        /// </summary>
        public virtual String UnConfigAlarmColor3
        {
            get
            {
                return this._UnConfigAlarmColor3;
            }
            set
            {
                if ((this._UnConfigAlarmColor3 != value))
                {
                    var original = this._UnConfigAlarmColor3;
                    this._UnConfigAlarmColor3 = value;
                    this.OnPropertyChanged("UnConfigAlarmColor3",original,value);

                }
            }
        }

        String _UnConfigAlarmColor4;
        /// <summary>
        /// 
        /// </summary>
        public virtual String UnConfigAlarmColor4
        {
            get
            {
                return this._UnConfigAlarmColor4;
            }
            set
            {
                if ((this._UnConfigAlarmColor4 != value))
                {
                    var original = this._UnConfigAlarmColor4;
                    this._UnConfigAlarmColor4 = value;
                    this.OnPropertyChanged("UnConfigAlarmColor4",original,value);

                }
            }
        }

        String _UnConfigAlarmColor5;
        /// <summary>
        /// 
        /// </summary>
        public virtual String UnConfigAlarmColor5
        {
            get
            {
                return this._UnConfigAlarmColor5;
            }
            set
            {
                if ((this._UnConfigAlarmColor5 != value))
                {
                    var original = this._UnConfigAlarmColor5;
                    this._UnConfigAlarmColor5 = value;
                    this.OnPropertyChanged("UnConfigAlarmColor5",original,value);

                }
            }
        }

        String _UnConfigAlarmColor6;
        /// <summary>
        /// 
        /// </summary>
        public virtual String UnConfigAlarmColor6
        {
            get
            {
                return this._UnConfigAlarmColor6;
            }
            set
            {
                if ((this._UnConfigAlarmColor6 != value))
                {
                    var original = this._UnConfigAlarmColor6;
                    this._UnConfigAlarmColor6 = value;
                    this.OnPropertyChanged("UnConfigAlarmColor6",original,value);

                }
            }
        }

        String _UnConfigAlarmColor7;
        /// <summary>
        /// 
        /// </summary>
        public virtual String UnConfigAlarmColor7
        {
            get
            {
                return this._UnConfigAlarmColor7;
            }
            set
            {
                if ((this._UnConfigAlarmColor7 != value))
                {
                    var original = this._UnConfigAlarmColor7;
                    this._UnConfigAlarmColor7 = value;
                    this.OnPropertyChanged("UnConfigAlarmColor7",original,value);

                }
            }
        }

        String _UnConfigAlarmColor8;
        /// <summary>
        /// 
        /// </summary>
        public virtual String UnConfigAlarmColor8
        {
            get
            {
                return this._UnConfigAlarmColor8;
            }
            set
            {
                if ((this._UnConfigAlarmColor8 != value))
                {
                    var original = this._UnConfigAlarmColor8;
                    this._UnConfigAlarmColor8 = value;
                    this.OnPropertyChanged("UnConfigAlarmColor8",original,value);

                }
            }
        }

        String _ConfigAlarmColor1;
        /// <summary>
        /// 确认报警颜色
        /// </summary>
        public virtual String ConfigAlarmColor1
        {
            get
            {
                return this._ConfigAlarmColor1;
            }
            set
            {
                if ((this._ConfigAlarmColor1 != value))
                {
                    var original = this._ConfigAlarmColor1;
                    this._ConfigAlarmColor1 = value;
                    this.OnPropertyChanged("ConfigAlarmColor1",original,value);

                }
            }
        }

        String _ConfigAlarmColor2;
        /// <summary>
        /// 
        /// </summary>
        public virtual String ConfigAlarmColor2
        {
            get
            {
                return this._ConfigAlarmColor2;
            }
            set
            {
                if ((this._ConfigAlarmColor2 != value))
                {
                    var original = this._ConfigAlarmColor2;
                    this._ConfigAlarmColor2 = value;
                    this.OnPropertyChanged("ConfigAlarmColor2",original,value);

                }
            }
        }

        String _ConfigAlarmColor3;
        /// <summary>
        /// 
        /// </summary>
        public virtual String ConfigAlarmColor3
        {
            get
            {
                return this._ConfigAlarmColor3;
            }
            set
            {
                if ((this._ConfigAlarmColor3 != value))
                {
                    var original = this._ConfigAlarmColor3;
                    this._ConfigAlarmColor3 = value;
                    this.OnPropertyChanged("ConfigAlarmColor3",original,value);

                }
            }
        }

        String _ConfigAlarmColor4;
        /// <summary>
        /// 
        /// </summary>
        public virtual String ConfigAlarmColor4
        {
            get
            {
                return this._ConfigAlarmColor4;
            }
            set
            {
                if ((this._ConfigAlarmColor4 != value))
                {
                    var original = this._ConfigAlarmColor4;
                    this._ConfigAlarmColor4 = value;
                    this.OnPropertyChanged("ConfigAlarmColor4",original,value);

                }
            }
        }

        String _ConfigAlarmColor5;
        /// <summary>
        /// 
        /// </summary>
        public virtual String ConfigAlarmColor5
        {
            get
            {
                return this._ConfigAlarmColor5;
            }
            set
            {
                if ((this._ConfigAlarmColor5 != value))
                {
                    var original = this._ConfigAlarmColor5;
                    this._ConfigAlarmColor5 = value;
                    this.OnPropertyChanged("ConfigAlarmColor5",original,value);

                }
            }
        }

        String _ConfigAlarmColor6;
        /// <summary>
        /// 
        /// </summary>
        public virtual String ConfigAlarmColor6
        {
            get
            {
                return this._ConfigAlarmColor6;
            }
            set
            {
                if ((this._ConfigAlarmColor6 != value))
                {
                    var original = this._ConfigAlarmColor6;
                    this._ConfigAlarmColor6 = value;
                    this.OnPropertyChanged("ConfigAlarmColor6",original,value);

                }
            }
        }

        String _ConfigAlarmColor7;
        /// <summary>
        /// 
        /// </summary>
        public virtual String ConfigAlarmColor7
        {
            get
            {
                return this._ConfigAlarmColor7;
            }
            set
            {
                if ((this._ConfigAlarmColor7 != value))
                {
                    var original = this._ConfigAlarmColor7;
                    this._ConfigAlarmColor7 = value;
                    this.OnPropertyChanged("ConfigAlarmColor7",original,value);

                }
            }
        }

        String _ConfigAlarmColor8;
        /// <summary>
        /// 
        /// </summary>
        public virtual String ConfigAlarmColor8
        {
            get
            {
                return this._ConfigAlarmColor8;
            }
            set
            {
                if ((this._ConfigAlarmColor8 != value))
                {
                    var original = this._ConfigAlarmColor8;
                    this._ConfigAlarmColor8 = value;
                    this.OnPropertyChanged("ConfigAlarmColor8",original,value);

                }
            }
        }

        String _UnBackAlarmColor1;
        /// <summary>
        /// 未确认返回颜色
        /// </summary>
        public virtual String UnBackAlarmColor1
        {
            get
            {
                return this._UnBackAlarmColor1;
            }
            set
            {
                if ((this._UnBackAlarmColor1 != value))
                {
                    var original = this._UnBackAlarmColor1;
                    this._UnBackAlarmColor1 = value;
                    this.OnPropertyChanged("UnBackAlarmColor1",original,value);

                }
            }
        }

        String _UnBackAlarmColor2;
        /// <summary>
        /// 
        /// </summary>
        public virtual String UnBackAlarmColor2
        {
            get
            {
                return this._UnBackAlarmColor2;
            }
            set
            {
                if ((this._UnBackAlarmColor2 != value))
                {
                    var original = this._UnBackAlarmColor2;
                    this._UnBackAlarmColor2 = value;
                    this.OnPropertyChanged("UnBackAlarmColor2",original,value);

                }
            }
        }

        String _UnBackAlarmColor3;
        /// <summary>
        /// 
        /// </summary>
        public virtual String UnBackAlarmColor3
        {
            get
            {
                return this._UnBackAlarmColor3;
            }
            set
            {
                if ((this._UnBackAlarmColor3 != value))
                {
                    var original = this._UnBackAlarmColor3;
                    this._UnBackAlarmColor3 = value;
                    this.OnPropertyChanged("UnBackAlarmColor3",original,value);

                }
            }
        }

        String _UnBackAlarmColor4;
        /// <summary>
        /// 
        /// </summary>
        public virtual String UnBackAlarmColor4
        {
            get
            {
                return this._UnBackAlarmColor4;
            }
            set
            {
                if ((this._UnBackAlarmColor4 != value))
                {
                    var original = this._UnBackAlarmColor4;
                    this._UnBackAlarmColor4 = value;
                    this.OnPropertyChanged("UnBackAlarmColor4",original,value);

                }
            }
        }

        String _UnBackAlarmColor5;
        /// <summary>
        /// 
        /// </summary>
        public virtual String UnBackAlarmColor5
        {
            get
            {
                return this._UnBackAlarmColor5;
            }
            set
            {
                if ((this._UnBackAlarmColor5 != value))
                {
                    var original = this._UnBackAlarmColor5;
                    this._UnBackAlarmColor5 = value;
                    this.OnPropertyChanged("UnBackAlarmColor5",original,value);

                }
            }
        }

        String _UnBackAlarmColor6;
        /// <summary>
        /// 
        /// </summary>
        public virtual String UnBackAlarmColor6
        {
            get
            {
                return this._UnBackAlarmColor6;
            }
            set
            {
                if ((this._UnBackAlarmColor6 != value))
                {
                    var original = this._UnBackAlarmColor6;
                    this._UnBackAlarmColor6 = value;
                    this.OnPropertyChanged("UnBackAlarmColor6",original,value);

                }
            }
        }

        String _UnBackAlarmColor7;
        /// <summary>
        /// 
        /// </summary>
        public virtual String UnBackAlarmColor7
        {
            get
            {
                return this._UnBackAlarmColor7;
            }
            set
            {
                if ((this._UnBackAlarmColor7 != value))
                {
                    var original = this._UnBackAlarmColor7;
                    this._UnBackAlarmColor7 = value;
                    this.OnPropertyChanged("UnBackAlarmColor7",original,value);

                }
            }
        }

        String _UnBackAlarmColor8;
        /// <summary>
        /// 
        /// </summary>
        public virtual String UnBackAlarmColor8
        {
            get
            {
                return this._UnBackAlarmColor8;
            }
            set
            {
                if ((this._UnBackAlarmColor8 != value))
                {
                    var original = this._UnBackAlarmColor8;
                    this._UnBackAlarmColor8 = value;
                    this.OnPropertyChanged("UnBackAlarmColor8",original,value);

                }
            }
        }

        String _BackAlarmColor1;
        /// <summary>
        /// 确认返回颜色
        /// </summary>
        public virtual String BackAlarmColor1
        {
            get
            {
                return this._BackAlarmColor1;
            }
            set
            {
                if ((this._BackAlarmColor1 != value))
                {
                    var original = this._BackAlarmColor1;
                    this._BackAlarmColor1 = value;
                    this.OnPropertyChanged("BackAlarmColor1",original,value);

                }
            }
        }

        String _BackAlarmColor2;
        /// <summary>
        /// 
        /// </summary>
        public virtual String BackAlarmColor2
        {
            get
            {
                return this._BackAlarmColor2;
            }
            set
            {
                if ((this._BackAlarmColor2 != value))
                {
                    var original = this._BackAlarmColor2;
                    this._BackAlarmColor2 = value;
                    this.OnPropertyChanged("BackAlarmColor2",original,value);

                }
            }
        }

        String _BackAlarmColor3;
        /// <summary>
        /// 
        /// </summary>
        public virtual String BackAlarmColor3
        {
            get
            {
                return this._BackAlarmColor3;
            }
            set
            {
                if ((this._BackAlarmColor3 != value))
                {
                    var original = this._BackAlarmColor3;
                    this._BackAlarmColor3 = value;
                    this.OnPropertyChanged("BackAlarmColor3",original,value);

                }
            }
        }

        String _BackAlarmColor4;
        /// <summary>
        /// 
        /// </summary>
        public virtual String BackAlarmColor4
        {
            get
            {
                return this._BackAlarmColor4;
            }
            set
            {
                if ((this._BackAlarmColor4 != value))
                {
                    var original = this._BackAlarmColor4;
                    this._BackAlarmColor4 = value;
                    this.OnPropertyChanged("BackAlarmColor4",original,value);

                }
            }
        }

        String _BackAlarmColor5;
        /// <summary>
        /// 
        /// </summary>
        public virtual String BackAlarmColor5
        {
            get
            {
                return this._BackAlarmColor5;
            }
            set
            {
                if ((this._BackAlarmColor5 != value))
                {
                    var original = this._BackAlarmColor5;
                    this._BackAlarmColor5 = value;
                    this.OnPropertyChanged("BackAlarmColor5",original,value);

                }
            }
        }

        String _BackAlarmColor6;
        /// <summary>
        /// 
        /// </summary>
        public virtual String BackAlarmColor6
        {
            get
            {
                return this._BackAlarmColor6;
            }
            set
            {
                if ((this._BackAlarmColor6 != value))
                {
                    var original = this._BackAlarmColor6;
                    this._BackAlarmColor6 = value;
                    this.OnPropertyChanged("BackAlarmColor6",original,value);

                }
            }
        }

        String _BackAlarmColor7;
        /// <summary>
        /// 
        /// </summary>
        public virtual String BackAlarmColor7
        {
            get
            {
                return this._BackAlarmColor7;
            }
            set
            {
                if ((this._BackAlarmColor7 != value))
                {
                    var original = this._BackAlarmColor7;
                    this._BackAlarmColor7 = value;
                    this.OnPropertyChanged("BackAlarmColor7",original,value);

                }
            }
        }

        String _BackAlarmColor8;
        /// <summary>
        /// 
        /// </summary>
        public virtual String BackAlarmColor8
        {
            get
            {
                return this._BackAlarmColor8;
            }
            set
            {
                if ((this._BackAlarmColor8 != value))
                {
                    var original = this._BackAlarmColor8;
                    this._BackAlarmColor8 = value;
                    this.OnPropertyChanged("BackAlarmColor8",original,value);

                }
            }
        }

        String _HistoryPath;
        /// <summary>
        /// 历史保存路径
        /// </summary>
        public virtual String HistoryPath
        {
            get
            {
                return this._HistoryPath;
            }
            set
            {
                if ((this._HistoryPath != value))
                {
                    var original = this._HistoryPath;
                    this._HistoryPath = value;
                    this.OnPropertyChanged("HistoryPath",original,value);

                }
            }
        }

        System.Nullable<Int32> _HistoryStoreAlarm;
        /// <summary>
        /// 历史路径剩余空间报警阀门(单位：G)
        /// </summary>
        public virtual System.Nullable<Int32> HistoryStoreAlarm
        {
            get
            {
                return this._HistoryStoreAlarm;
            }
            set
            {
                if ((this._HistoryStoreAlarm != value))
                {
                    var original = this._HistoryStoreAlarm;
                    this._HistoryStoreAlarm = value;
                    this.OnPropertyChanged("HistoryStoreAlarm",original,value);

                }
            }
        }

        String _LogPath;
        /// <summary>
        /// 系统日志存储路径
        /// </summary>
        public virtual String LogPath
        {
            get
            {
                return this._LogPath;
            }
            set
            {
                if ((this._LogPath != value))
                {
                    var original = this._LogPath;
                    this._LogPath = value;
                    this.OnPropertyChanged("LogPath",original,value);

                }
            }
        }

        System.Nullable<Int32> _LogDays=90;
        /// <summary>
        /// 系统日志存储天数
        /// </summary>
        public virtual System.Nullable<Int32> LogDays
        {
            get
            {
                return this._LogDays;
            }
            set
            {
                if ((this._LogDays != value))
                {
                    var original = this._LogDays;
                    this._LogDays = value;
                    this.OnPropertyChanged("LogDays",original,value);

                }
            }
        }

        System.Nullable<Int32> _LogMaxStore;
        /// <summary>
        /// 日志最大存档空间(单位：G)
        /// </summary>
        public virtual System.Nullable<Int32> LogMaxStore
        {
            get
            {
                return this._LogMaxStore;
            }
            set
            {
                if ((this._LogMaxStore != value))
                {
                    var original = this._LogMaxStore;
                    this._LogMaxStore = value;
                    this.OnPropertyChanged("LogMaxStore",original,value);

                }
            }
        }
}}
namespace SunRizServer{


    /// <summary>
	/// 报警记录
	/// </summary>
    public class Alarm :Way.Lib.DataModel
    {

        /// <summary>
	    /// 
	    /// </summary>
        public  Alarm()
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

        String _Address;
        /// <summary>
        /// 报警地址
        /// </summary>
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
                    var original = this._Address;
                    this._Address = value;
                    this.OnPropertyChanged("Address",original,value);

                }
            }
        }

        String _AddressDesc;
        /// <summary>
        /// 地址描述
        /// </summary>
        public virtual String AddressDesc
        {
            get
            {
                return this._AddressDesc;
            }
            set
            {
                if ((this._AddressDesc != value))
                {
                    var original = this._AddressDesc;
                    this._AddressDesc = value;
                    this.OnPropertyChanged("AddressDesc",original,value);

                }
            }
        }

        System.Nullable<Int32> _PointId;
        /// <summary>
        /// 点id
        /// </summary>
        public virtual System.Nullable<Int32> PointId
        {
            get
            {
                return this._PointId;
            }
            set
            {
                if ((this._PointId != value))
                {
                    var original = this._PointId;
                    this._PointId = value;
                    this.OnPropertyChanged("PointId",original,value);

                }
            }
        }

        String _Content;
        /// <summary>
        /// 报警内容
        /// </summary>
        public virtual String Content
        {
            get
            {
                return this._Content;
            }
            set
            {
                if ((this._Content != value))
                {
                    var original = this._Content;
                    this._Content = value;
                    this.OnPropertyChanged("Content",original,value);

                }
            }
        }

        System.Nullable<Boolean> _IsConfirm=false;
        /// <summary>
        /// 是否已经确认
        /// </summary>
        public virtual System.Nullable<Boolean> IsConfirm
        {
            get
            {
                return this._IsConfirm;
            }
            set
            {
                if ((this._IsConfirm != value))
                {
                    var original = this._IsConfirm;
                    this._IsConfirm = value;
                    this.OnPropertyChanged("IsConfirm",original,value);

                }
            }
        }

        System.Nullable<Boolean> _IsReset=false;
        /// <summary>
        /// 是否已经复位
        /// </summary>
        public virtual System.Nullable<Boolean> IsReset
        {
            get
            {
                return this._IsReset;
            }
            set
            {
                if ((this._IsReset != value))
                {
                    var original = this._IsReset;
                    this._IsReset = value;
                    this.OnPropertyChanged("IsReset",original,value);

                }
            }
        }

        System.Nullable<Int32> _ConfirmUserId;
        /// <summary>
        /// 确认人员id
        /// </summary>
        public virtual System.Nullable<Int32> ConfirmUserId
        {
            get
            {
                return this._ConfirmUserId;
            }
            set
            {
                if ((this._ConfirmUserId != value))
                {
                    var original = this._ConfirmUserId;
                    this._ConfirmUserId = value;
                    this.OnPropertyChanged("ConfirmUserId",original,value);

                }
            }
        }

        System.Nullable<DateTime> _ConfirmTime;
        /// <summary>
        /// 确认时间
        /// </summary>
        public virtual System.Nullable<DateTime> ConfirmTime
        {
            get
            {
                return this._ConfirmTime;
            }
            set
            {
                if ((this._ConfirmTime != value))
                {
                    var original = this._ConfirmTime;
                    this._ConfirmTime = value;
                    this.OnPropertyChanged("ConfirmTime",original,value);

                }
            }
        }

        System.Nullable<DateTime> _ResetTime;
        /// <summary>
        /// 复位时间
        /// </summary>
        public virtual System.Nullable<DateTime> ResetTime
        {
            get
            {
                return this._ResetTime;
            }
            set
            {
                if ((this._ResetTime != value))
                {
                    var original = this._ResetTime;
                    this._ResetTime = value;
                    this.OnPropertyChanged("ResetTime",original,value);

                }
            }
        }

        System.Nullable<DateTime> _AlarmTime;
        /// <summary>
        /// 报警时间
        /// </summary>
        public virtual System.Nullable<DateTime> AlarmTime
        {
            get
            {
                return this._AlarmTime;
            }
            set
            {
                if ((this._AlarmTime != value))
                {
                    var original = this._AlarmTime;
                    this._AlarmTime = value;
                    this.OnPropertyChanged("AlarmTime",original,value);

                }
            }
        }

        System.Nullable<double> _PointValue;
        /// <summary>
        /// 报警时点值
        /// </summary>
        public virtual System.Nullable<double> PointValue
        {
            get
            {
                return this._PointValue;
            }
            set
            {
                if ((this._PointValue != value))
                {
                    var original = this._PointValue;
                    this._PointValue = value;
                    this.OnPropertyChanged("PointValue",original,value);

                }
            }
        }

        System.Nullable<Int32> _AlarmGroup;
        /// <summary>
        /// 所属报警组
        /// </summary>
        public virtual System.Nullable<Int32> AlarmGroup
        {
            get
            {
                return this._AlarmGroup;
            }
            set
            {
                if ((this._AlarmGroup != value))
                {
                    var original = this._AlarmGroup;
                    this._AlarmGroup = value;
                    this.OnPropertyChanged("AlarmGroup",original,value);

                }
            }
        }

        System.Nullable<Int32> _Priority;
        /// <summary>
        /// 优先级
        /// </summary>
        public virtual System.Nullable<Int32> Priority
        {
            get
            {
                return this._Priority;
            }
            set
            {
                if ((this._Priority != value))
                {
                    var original = this._Priority;
                    this._Priority = value;
                    this.OnPropertyChanged("Priority",original,value);

                }
            }
        }

        System.Nullable<Boolean> _IsBack=false;
        /// <summary>
        /// 是否已经自动返回（已经不再报警）
        /// </summary>
        public virtual System.Nullable<Boolean> IsBack
        {
            get
            {
                return this._IsBack;
            }
            set
            {
                if ((this._IsBack != value))
                {
                    var original = this._IsBack;
                    this._IsBack = value;
                    this.OnPropertyChanged("IsBack",original,value);

                }
            }
        }

        String _Expression;
        /// <summary>
        /// 判断报警的公式
        /// </summary>
        public virtual String Expression
        {
            get
            {
                return this._Expression;
            }
            set
            {
                if ((this._Expression != value))
                {
                    var original = this._Expression;
                    this._Expression = value;
                    this.OnPropertyChanged("Expression",original,value);

                }
            }
        }

        public virtual DevicePoint PointObject { get; set; }
}}
namespace SunRizServer{

/// <summary>
/// 
/// </summary>
public enum UserInfo_RoleEnum:int
{
    

/// <summary>
/// 
/// </summary>
None = 0,

/// <summary>
/// 
/// </summary>

Designer = 1,

/// <summary>
/// 
/// </summary>

User = 1<<1,

/// <summary>
/// 
/// </summary>

Admin = 1<<30 | Designer,
}


    /// <summary>
	/// 
	/// </summary>
    public class UserInfo :Way.Lib.DataModel
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

        String _Password;
        /// <summary>
        /// 
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

        System.Nullable<UserInfo_RoleEnum> _Role=(System.Nullable<UserInfo_RoleEnum>)(0);
        /// <summary>
        /// 角色
        /// </summary>
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
                    var original = this._Role;
                    this._Role = value;
                    this.OnPropertyChanged("Role",original,value);

                }
            }
        }

        System.Nullable<Int32> _AlarmGroups;
        /// <summary>
        /// 关注哪些报警组
        /// </summary>
        public virtual System.Nullable<Int32> AlarmGroups
        {
            get
            {
                return this._AlarmGroups;
            }
            set
            {
                if ((this._AlarmGroups != value))
                {
                    var original = this._AlarmGroups;
                    this._AlarmGroups = value;
                    this.OnPropertyChanged("AlarmGroups",original,value);

                }
            }
        }

        System.Nullable<Int32> _SafeArea;
        /// <summary>
        /// 哪些安全区
        /// </summary>
        public virtual System.Nullable<Int32> SafeArea
        {
            get
            {
                return this._SafeArea;
            }
            set
            {
                if ((this._SafeArea != value))
                {
                    var original = this._SafeArea;
                    this._SafeArea = value;
                    this.OnPropertyChanged("SafeArea",original,value);

                }
            }
        }
}}
namespace SunRizServer{


    /// <summary>
	/// 
	/// </summary>
    public class History :Way.Lib.DataModel
    {

        /// <summary>
	    /// 
	    /// </summary>
        public  History()
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

        System.Nullable<Int32> _PointId;
        /// <summary>
        /// 
        /// </summary>
        public virtual System.Nullable<Int32> PointId
        {
            get
            {
                return this._PointId;
            }
            set
            {
                if ((this._PointId != value))
                {
                    var original = this._PointId;
                    this._PointId = value;
                    this.OnPropertyChanged("PointId",original,value);

                }
            }
        }

        String _Address;
        /// <summary>
        /// 点地址
        /// </summary>
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
                    var original = this._Address;
                    this._Address = value;
                    this.OnPropertyChanged("Address",original,value);

                }
            }
        }

        System.Nullable<DateTime> _Time;
        /// <summary>
        /// 
        /// </summary>
        public virtual System.Nullable<DateTime> Time
        {
            get
            {
                return this._Time;
            }
            set
            {
                if ((this._Time != value))
                {
                    var original = this._Time;
                    this._Time = value;
                    this.OnPropertyChanged("Time",original,value);

                }
            }
        }

        System.Nullable<double> _Value;
        /// <summary>
        /// 
        /// </summary>
        public virtual System.Nullable<double> Value
        {
            get
            {
                return this._Value;
            }
            set
            {
                if ((this._Value != value))
                {
                    var original = this._Value;
                    this._Value = value;
                    this.OnPropertyChanged("Value",original,value);

                }
            }
        }
}}
namespace SunRizServer{


    /// <summary>
	/// 系统日志表
	/// </summary>
    public class SysLog :Way.Lib.DataModel
    {

        /// <summary>
	    /// 
	    /// </summary>
        public  SysLog()
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

        System.Nullable<DateTime> _Time;
        /// <summary>
        /// 
        /// </summary>
        public virtual System.Nullable<DateTime> Time
        {
            get
            {
                return this._Time;
            }
            set
            {
                if ((this._Time != value))
                {
                    var original = this._Time;
                    this._Time = value;
                    this.OnPropertyChanged("Time",original,value);

                }
            }
        }

        String _Content;
        /// <summary>
        /// 描述
        /// </summary>
        public virtual String Content
        {
            get
            {
                return this._Content;
            }
            set
            {
                if ((this._Content != value))
                {
                    var original = this._Content;
                    this._Content = value;
                    this.OnPropertyChanged("Content",original,value);

                }
            }
        }

        System.Nullable<Int32> _UserId;
        /// <summary>
        /// 操作人
        /// </summary>
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
                    var original = this._UserId;
                    this._UserId = value;
                    this.OnPropertyChanged("UserId",original,value);

                }
            }
        }

        String _UserName;
        /// <summary>
        /// 用户名称，实际不会有值
        /// </summary>
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
                    var original = this._UserName;
                    this._UserName = value;
                    this.OnPropertyChanged("UserName",original,value);

                }
            }
        }
}}
