
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

        String _SafeArea;
        /// <summary>
        /// 安全区
        /// </summary>
        public virtual String SafeArea
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
