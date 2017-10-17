
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
        /// 
        /// </summary>
        [System.ComponentModel.DataAnnotations.Schema.Column("driverid")]
        [Way.EntityDB.WayDBColumnAttribute(Name="driverid",Comment="",Caption="",Storage = "_DriverID",DbType="int")]
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
}}
namespace SunRizServer{


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

        System.Nullable<Int32> _DeviceID;
        /// <summary>
        /// 
        /// </summary>
        [System.ComponentModel.DataAnnotations.Schema.Column("deviceid")]
        [Way.EntityDB.WayDBColumnAttribute(Name="deviceid",Comment="",Caption="",Storage = "_DeviceID",DbType="int")]
        public virtual System.Nullable<Int32> DeviceID
        {
            get
            {
                return this._DeviceID;
            }
            set
            {
                if ((this._DeviceID != value))
                {
                    this.SendPropertyChanging("DeviceID",this._DeviceID,value);
                    this._DeviceID = value;
                    this.SendPropertyChanged("DeviceID");

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


                    if (e.DataItem is SunRizServer.CommunicationDriver)
                    {
                        var deletingItem = (SunRizServer.CommunicationDriver)e.DataItem;
                        
                    var items0 = (from m in db.Device
                    where m.DriverID == deletingItem.id
                    select new SunRizServer.Device
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

                    if (e.DataItem is SunRizServer.Device)
                    {
                        var deletingItem = (SunRizServer.Device)e.DataItem;
                        
                    var items0 = (from m in db.DevicePoint
                    where m.DeviceID == deletingItem.id
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

static string _designData = "eyJUYWJsZXMiOlt7IlRhYmxlTmFtZSI6IlNxbGl0ZSIsIlJvd3MiOlt7Ikl0ZW1zIjpbeyJOYW1lIjoiaWQiLCJWYWx1ZSI6MTd9LHsiTmFtZSI6InR5cGUiLCJWYWx1ZSI6IkNyZWF0ZVRhYmxlQWN0aW9uIn0seyJOYW1lIjoiY29udGVudCIsIlZhbHVlIjoie1wiVGFibGVcIjp7XCJpZFwiOjUsXCJjYXB0aW9uXCI6XCLlm77niYfmlofku7ZcIixcIk5hbWVcIjpcIkltYWdlRmlsZXNcIixcIkRhdGFiYXNlSURcIjoyLFwiaUxvY2tcIjowfSxcIkNvbHVtbnNcIjpbe1wiaWRcIjoyOSxcIk5hbWVcIjpcImlkXCIsXCJJc0F1dG9JbmNyZW1lbnRcIjp0cnVlLFwiQ2FuTnVsbFwiOmZhbHNlLFwiZGJUeXBlXCI6XCJpbnRcIixcIlRhYmxlSURcIjo1LFwiSXNQS0lEXCI6dHJ1ZSxcIm9yZGVyaWRcIjowfSx7XCJpZFwiOjMwLFwiTmFtZVwiOlwiTmFtZVwiLFwiSXNBdXRvSW5jcmVtZW50XCI6ZmFsc2UsXCJDYW5OdWxsXCI6dHJ1ZSxcImRiVHlwZVwiOlwidmFyY2hhclwiLFwibGVuZ3RoXCI6XCI1MFwiLFwiVGFibGVJRFwiOjUsXCJJc1BLSURcIjpmYWxzZSxcIm9yZGVyaWRcIjoxfSx7XCJpZFwiOjMxLFwiTmFtZVwiOlwiUGFyZW50SWRcIixcIklzQXV0b0luY3JlbWVudFwiOmZhbHNlLFwiQ2FuTnVsbFwiOnRydWUsXCJkYlR5cGVcIjpcImludFwiLFwibGVuZ3RoXCI6XCJcIixcIlRhYmxlSURcIjo1LFwiSXNQS0lEXCI6ZmFsc2UsXCJvcmRlcmlkXCI6Mn0se1wiaWRcIjozMixcIk5hbWVcIjpcIklzRm9sZGVyXCIsXCJJc0F1dG9JbmNyZW1lbnRcIjpmYWxzZSxcIkNhbk51bGxcIjp0cnVlLFwiZGJUeXBlXCI6XCJiaXRcIixcImxlbmd0aFwiOlwiXCIsXCJkZWZhdWx0VmFsdWVcIjpcIjBcIixcIlRhYmxlSURcIjo1LFwiSXNQS0lEXCI6ZmFsc2UsXCJvcmRlcmlkXCI6M30se1wiaWRcIjozMyxcImNhcHRpb25cIjpcIuaWh+S7tuWunumZheaWh+S7tuWQjVwiLFwiTmFtZVwiOlwiRmlsZU5hbWVcIixcIklzQXV0b0luY3JlbWVudFwiOmZhbHNlLFwiQ2FuTnVsbFwiOnRydWUsXCJkYlR5cGVcIjpcInZhcmNoYXJcIixcImxlbmd0aFwiOlwiNTBcIixcIlRhYmxlSURcIjo1LFwiSXNQS0lEXCI6ZmFsc2UsXCJvcmRlcmlkXCI6NH1dLFwiSURYQ29uZmlnc1wiOltdLFwiSURcIjowfSJ9LHsiTmFtZSI6ImRhdGFiYXNlaWQiLCJWYWx1ZSI6Mn1dLCJSb3dTdGF0ZSI6MH0seyJJdGVtcyI6W3siTmFtZSI6ImlkIiwiVmFsdWUiOjE4fSx7Ik5hbWUiOiJ0eXBlIiwiVmFsdWUiOiJDaGFuZ2VUYWJsZUFjdGlvbiJ9LHsiTmFtZSI6ImNvbnRlbnQiLCJWYWx1ZSI6IntcIk9sZFRhYmxlTmFtZVwiOlwiSW1hZ2VGaWxlc1wiLFwiTmV3VGFibGVOYW1lXCI6XCJJbWFnZUZpbGVzXCIsXCJvdGhlckNvbHVtbnNcIjpbe1wiaWRcIjoyOSxcIk5hbWVcIjpcImlkXCIsXCJJc0F1dG9JbmNyZW1lbnRcIjp0cnVlLFwiQ2FuTnVsbFwiOmZhbHNlLFwiZGJUeXBlXCI6XCJpbnRcIixcIlRhYmxlSURcIjo1LFwiSXNQS0lEXCI6dHJ1ZSxcIm9yZGVyaWRcIjowfSx7XCJpZFwiOjMwLFwiTmFtZVwiOlwiTmFtZVwiLFwiSXNBdXRvSW5jcmVtZW50XCI6ZmFsc2UsXCJDYW5OdWxsXCI6dHJ1ZSxcImRiVHlwZVwiOlwidmFyY2hhclwiLFwibGVuZ3RoXCI6XCI1MFwiLFwiVGFibGVJRFwiOjUsXCJJc1BLSURcIjpmYWxzZSxcIm9yZGVyaWRcIjoxfSx7XCJpZFwiOjMxLFwiTmFtZVwiOlwiUGFyZW50SWRcIixcIklzQXV0b0luY3JlbWVudFwiOmZhbHNlLFwiQ2FuTnVsbFwiOnRydWUsXCJkYlR5cGVcIjpcImludFwiLFwibGVuZ3RoXCI6XCJcIixcIlRhYmxlSURcIjo1LFwiSXNQS0lEXCI6ZmFsc2UsXCJvcmRlcmlkXCI6Mn0se1wiaWRcIjozMixcIk5hbWVcIjpcIklzRm9sZGVyXCIsXCJJc0F1dG9JbmNyZW1lbnRcIjpmYWxzZSxcIkNhbk51bGxcIjp0cnVlLFwiZGJUeXBlXCI6XCJiaXRcIixcImxlbmd0aFwiOlwiXCIsXCJkZWZhdWx0VmFsdWVcIjpcIjBcIixcIlRhYmxlSURcIjo1LFwiSXNQS0lEXCI6ZmFsc2UsXCJvcmRlcmlkXCI6M30se1wiaWRcIjozMyxcImNhcHRpb25cIjpcIuaWh+S7tuWunumZheaWh+S7tuWQjVwiLFwiTmFtZVwiOlwiRmlsZU5hbWVcIixcIklzQXV0b0luY3JlbWVudFwiOmZhbHNlLFwiQ2FuTnVsbFwiOnRydWUsXCJkYlR5cGVcIjpcInZhcmNoYXJcIixcImxlbmd0aFwiOlwiNTBcIixcIlRhYmxlSURcIjo1LFwiSXNQS0lEXCI6ZmFsc2UsXCJvcmRlcmlkXCI6NH1dLFwibmV3Q29sdW1uc1wiOltdLFwiY2hhbmdlZENvbHVtbnNcIjpbXSxcImRlbGV0ZWRDb2x1bW5zXCI6W10sXCJJRFhDb25maWdzXCI6W10sXCJJRFwiOjB9In0seyJOYW1lIjoiZGF0YWJhc2VpZCIsIlZhbHVlIjoyfV0sIlJvd1N0YXRlIjowfSx7Ikl0ZW1zIjpbeyJOYW1lIjoiaWQiLCJWYWx1ZSI6MTl9LHsiTmFtZSI6InR5cGUiLCJWYWx1ZSI6IkNoYW5nZVRhYmxlQWN0aW9uIn0seyJOYW1lIjoiY29udGVudCIsIlZhbHVlIjoie1wiT2xkVGFibGVOYW1lXCI6XCJJbWFnZUZpbGVzXCIsXCJOZXdUYWJsZU5hbWVcIjpcIkltYWdlRmlsZXNcIixcIm90aGVyQ29sdW1uc1wiOlt7XCJpZFwiOjI5LFwiTmFtZVwiOlwiaWRcIixcIklzQXV0b0luY3JlbWVudFwiOnRydWUsXCJDYW5OdWxsXCI6ZmFsc2UsXCJkYlR5cGVcIjpcImludFwiLFwiVGFibGVJRFwiOjUsXCJJc1BLSURcIjp0cnVlLFwib3JkZXJpZFwiOjB9LHtcImlkXCI6MzAsXCJjYXB0aW9uXCI6XCLmmL7npLrnmoTlkI3np7BcIixcIk5hbWVcIjpcIk5hbWVcIixcIklzQXV0b0luY3JlbWVudFwiOmZhbHNlLFwiQ2FuTnVsbFwiOnRydWUsXCJkYlR5cGVcIjpcInZhcmNoYXJcIixcImxlbmd0aFwiOlwiNTBcIixcIlRhYmxlSURcIjo1LFwiSXNQS0lEXCI6ZmFsc2UsXCJvcmRlcmlkXCI6MX0se1wiaWRcIjozMixcIk5hbWVcIjpcIklzRm9sZGVyXCIsXCJJc0F1dG9JbmNyZW1lbnRcIjpmYWxzZSxcIkNhbk51bGxcIjp0cnVlLFwiZGJUeXBlXCI6XCJiaXRcIixcImxlbmd0aFwiOlwiXCIsXCJkZWZhdWx0VmFsdWVcIjpcIjBcIixcIlRhYmxlSURcIjo1LFwiSXNQS0lEXCI6ZmFsc2UsXCJvcmRlcmlkXCI6M30se1wiaWRcIjozMyxcImNhcHRpb25cIjpcIuaWh+S7tuWunumZheaWh+S7tuWQjVwiLFwiTmFtZVwiOlwiRmlsZU5hbWVcIixcIklzQXV0b0luY3JlbWVudFwiOmZhbHNlLFwiQ2FuTnVsbFwiOnRydWUsXCJkYlR5cGVcIjpcInZhcmNoYXJcIixcImxlbmd0aFwiOlwiNTBcIixcIlRhYmxlSURcIjo1LFwiSXNQS0lEXCI6ZmFsc2UsXCJvcmRlcmlkXCI6NH1dLFwibmV3Q29sdW1uc1wiOltdLFwiY2hhbmdlZENvbHVtbnNcIjpbe1wiaWRcIjozMSxcIk5hbWVcIjpcIlBhcmVudElkXCIsXCJJc0F1dG9JbmNyZW1lbnRcIjpmYWxzZSxcIkNhbk51bGxcIjp0cnVlLFwiZGJUeXBlXCI6XCJpbnRcIixcImxlbmd0aFwiOlwiXCIsXCJkZWZhdWx0VmFsdWVcIjpcIjBcIixcIlRhYmxlSURcIjo1LFwiSXNQS0lEXCI6ZmFsc2UsXCJvcmRlcmlkXCI6MixcIkJhY2t1cENoYW5nZWRQcm9wZXJ0aWVzXCI6e1wiZGVmYXVsdFZhbHVlXCI6e1wiT3JpZ2luYWxWYWx1ZVwiOm51bGx9fX1dLFwiZGVsZXRlZENvbHVtbnNcIjpbXSxcIklEWENvbmZpZ3NcIjpbXSxcIklEXCI6MH0ifSx7Ik5hbWUiOiJkYXRhYmFzZWlkIiwiVmFsdWUiOjJ9XSwiUm93U3RhdGUiOjB9LHsiSXRlbXMiOlt7Ik5hbWUiOiJpZCIsIlZhbHVlIjoyMH0seyJOYW1lIjoidHlwZSIsIlZhbHVlIjoiQ3JlYXRlVGFibGVBY3Rpb24ifSx7Ik5hbWUiOiJjb250ZW50IiwiVmFsdWUiOiJ7XCJUYWJsZVwiOntcImlkXCI6NixcImNhcHRpb25cIjpcIumAmuiur+mpseWKqOWIl+ihqFwiLFwiTmFtZVwiOlwiQ29tbXVuaWNhdGlvbkRyaXZlclwiLFwiRGF0YWJhc2VJRFwiOjIsXCJpTG9ja1wiOjB9LFwiQ29sdW1uc1wiOlt7XCJpZFwiOjM0LFwiTmFtZVwiOlwiaWRcIixcIklzQXV0b0luY3JlbWVudFwiOnRydWUsXCJDYW5OdWxsXCI6ZmFsc2UsXCJkYlR5cGVcIjpcImludFwiLFwiVGFibGVJRFwiOjYsXCJJc1BLSURcIjp0cnVlLFwib3JkZXJpZFwiOjB9LHtcImlkXCI6MzUsXCJjYXB0aW9uXCI6XCLlkI3np7BcIixcIk5hbWVcIjpcIk5hbWVcIixcIklzQXV0b0luY3JlbWVudFwiOmZhbHNlLFwiQ2FuTnVsbFwiOnRydWUsXCJkYlR5cGVcIjpcInZhcmNoYXJcIixcImxlbmd0aFwiOlwiNTBcIixcIlRhYmxlSURcIjo2LFwiSXNQS0lEXCI6ZmFsc2UsXCJvcmRlcmlkXCI6MX0se1wiaWRcIjozNixcImNhcHRpb25cIjpcIuWcsOWdgFwiLFwiTmFtZVwiOlwiQWRkcmVzc1wiLFwiSXNBdXRvSW5jcmVtZW50XCI6ZmFsc2UsXCJDYW5OdWxsXCI6dHJ1ZSxcImRiVHlwZVwiOlwidmFyY2hhclwiLFwibGVuZ3RoXCI6XCI1MFwiLFwiVGFibGVJRFwiOjYsXCJJc1BLSURcIjpmYWxzZSxcIm9yZGVyaWRcIjoyfSx7XCJpZFwiOjM3LFwiY2FwdGlvblwiOlwi56uv5Y+jXCIsXCJOYW1lXCI6XCJQb3J0XCIsXCJJc0F1dG9JbmNyZW1lbnRcIjpmYWxzZSxcIkNhbk51bGxcIjp0cnVlLFwiZGJUeXBlXCI6XCJpbnRcIixcImxlbmd0aFwiOlwiXCIsXCJUYWJsZUlEXCI6NixcIklzUEtJRFwiOmZhbHNlLFwib3JkZXJpZFwiOjN9LHtcImlkXCI6MzgsXCJjYXB0aW9uXCI6XCLnirbmgIFcIixcIk5hbWVcIjpcIlN0YXR1c1wiLFwiSXNBdXRvSW5jcmVtZW50XCI6ZmFsc2UsXCJDYW5OdWxsXCI6dHJ1ZSxcImRiVHlwZVwiOlwiaW50XCIsXCJFbnVtRGVmaW5lXCI6XCJOb25lID0gMCxcXG5PbmxpbmUgPSAxLFxcbk9mZmxpbmUgPSAyXCIsXCJsZW5ndGhcIjpcIlwiLFwiZGVmYXVsdFZhbHVlXCI6XCIwXCIsXCJUYWJsZUlEXCI6NixcIklzUEtJRFwiOmZhbHNlLFwib3JkZXJpZFwiOjR9XSxcIklEWENvbmZpZ3NcIjpbXSxcIklEXCI6MH0ifSx7Ik5hbWUiOiJkYXRhYmFzZWlkIiwiVmFsdWUiOjJ9XSwiUm93U3RhdGUiOjB9LHsiSXRlbXMiOlt7Ik5hbWUiOiJpZCIsIlZhbHVlIjoyMX0seyJOYW1lIjoidHlwZSIsIlZhbHVlIjoiQ3JlYXRlVGFibGVBY3Rpb24ifSx7Ik5hbWUiOiJjb250ZW50IiwiVmFsdWUiOiJ7XCJUYWJsZVwiOntcImlkXCI6NyxcImNhcHRpb25cIjpcIuiuvuWkh+S/oeaBr1wiLFwiTmFtZVwiOlwiRGV2aWNlXCIsXCJEYXRhYmFzZUlEXCI6MixcImlMb2NrXCI6MH0sXCJDb2x1bW5zXCI6W3tcImlkXCI6MzksXCJOYW1lXCI6XCJpZFwiLFwiSXNBdXRvSW5jcmVtZW50XCI6dHJ1ZSxcIkNhbk51bGxcIjpmYWxzZSxcImRiVHlwZVwiOlwiaW50XCIsXCJUYWJsZUlEXCI6NyxcIklzUEtJRFwiOnRydWUsXCJvcmRlcmlkXCI6MH0se1wiaWRcIjo0MCxcIk5hbWVcIjpcIkRyaXZlcklEXCIsXCJJc0F1dG9JbmNyZW1lbnRcIjpmYWxzZSxcIkNhbk51bGxcIjp0cnVlLFwiZGJUeXBlXCI6XCJpbnRcIixcImxlbmd0aFwiOlwiXCIsXCJUYWJsZUlEXCI6NyxcIklzUEtJRFwiOmZhbHNlLFwib3JkZXJpZFwiOjF9LHtcImlkXCI6NDEsXCJjYXB0aW9uXCI6XCLlnLDlnYDkv6Hmga9cIixcIk5hbWVcIjpcIkFkZHJlc3NcIixcIklzQXV0b0luY3JlbWVudFwiOmZhbHNlLFwiQ2FuTnVsbFwiOnRydWUsXCJkYlR5cGVcIjpcInZhcmNoYXJcIixcImxlbmd0aFwiOlwiMTAwXCIsXCJUYWJsZUlEXCI6NyxcIklzUEtJRFwiOmZhbHNlLFwib3JkZXJpZFwiOjJ9XSxcIklEWENvbmZpZ3NcIjpbXSxcIklEXCI6MH0ifSx7Ik5hbWUiOiJkYXRhYmFzZWlkIiwiVmFsdWUiOjJ9XSwiUm93U3RhdGUiOjB9LHsiSXRlbXMiOlt7Ik5hbWUiOiJpZCIsIlZhbHVlIjoyMn0seyJOYW1lIjoidHlwZSIsIlZhbHVlIjoiQ2hhbmdlVGFibGVBY3Rpb24ifSx7Ik5hbWUiOiJjb250ZW50IiwiVmFsdWUiOiJ7XCJPbGRUYWJsZU5hbWVcIjpcIkNvbW11bmljYXRpb25Ecml2ZXJcIixcIk5ld1RhYmxlTmFtZVwiOlwiQ29tbXVuaWNhdGlvbkRyaXZlclwiLFwib3RoZXJDb2x1bW5zXCI6W3tcImlkXCI6MzQsXCJOYW1lXCI6XCJpZFwiLFwiSXNBdXRvSW5jcmVtZW50XCI6dHJ1ZSxcIkNhbk51bGxcIjpmYWxzZSxcImRiVHlwZVwiOlwiaW50XCIsXCJUYWJsZUlEXCI6NixcIklzUEtJRFwiOnRydWUsXCJvcmRlcmlkXCI6MH0se1wiaWRcIjozNSxcImNhcHRpb25cIjpcIuWQjeensFwiLFwiTmFtZVwiOlwiTmFtZVwiLFwiSXNBdXRvSW5jcmVtZW50XCI6ZmFsc2UsXCJDYW5OdWxsXCI6dHJ1ZSxcImRiVHlwZVwiOlwidmFyY2hhclwiLFwibGVuZ3RoXCI6XCI1MFwiLFwiVGFibGVJRFwiOjYsXCJJc1BLSURcIjpmYWxzZSxcIm9yZGVyaWRcIjoxfSx7XCJpZFwiOjM2LFwiY2FwdGlvblwiOlwi5Zyw5Z2AXCIsXCJOYW1lXCI6XCJBZGRyZXNzXCIsXCJJc0F1dG9JbmNyZW1lbnRcIjpmYWxzZSxcIkNhbk51bGxcIjp0cnVlLFwiZGJUeXBlXCI6XCJ2YXJjaGFyXCIsXCJsZW5ndGhcIjpcIjUwXCIsXCJUYWJsZUlEXCI6NixcIklzUEtJRFwiOmZhbHNlLFwib3JkZXJpZFwiOjJ9LHtcImlkXCI6MzcsXCJjYXB0aW9uXCI6XCLnq6/lj6NcIixcIk5hbWVcIjpcIlBvcnRcIixcIklzQXV0b0luY3JlbWVudFwiOmZhbHNlLFwiQ2FuTnVsbFwiOnRydWUsXCJkYlR5cGVcIjpcImludFwiLFwibGVuZ3RoXCI6XCJcIixcIlRhYmxlSURcIjo2LFwiSXNQS0lEXCI6ZmFsc2UsXCJvcmRlcmlkXCI6M30se1wiaWRcIjozOCxcImNhcHRpb25cIjpcIueKtuaAgVwiLFwiTmFtZVwiOlwiU3RhdHVzXCIsXCJJc0F1dG9JbmNyZW1lbnRcIjpmYWxzZSxcIkNhbk51bGxcIjp0cnVlLFwiZGJUeXBlXCI6XCJpbnRcIixcIkVudW1EZWZpbmVcIjpcIk5vbmUgPSAwLFxcbk9ubGluZSA9IDEsXFxuT2ZmbGluZSA9IDJcIixcImxlbmd0aFwiOlwiXCIsXCJkZWZhdWx0VmFsdWVcIjpcIjBcIixcIlRhYmxlSURcIjo2LFwiSXNQS0lEXCI6ZmFsc2UsXCJvcmRlcmlkXCI6NH1dLFwibmV3Q29sdW1uc1wiOltdLFwiY2hhbmdlZENvbHVtbnNcIjpbXSxcImRlbGV0ZWRDb2x1bW5zXCI6W10sXCJJRFhDb25maWdzXCI6W10sXCJJRFwiOjB9In0seyJOYW1lIjoiZGF0YWJhc2VpZCIsIlZhbHVlIjoyfV0sIlJvd1N0YXRlIjowfSx7Ikl0ZW1zIjpbeyJOYW1lIjoiaWQiLCJWYWx1ZSI6MjN9LHsiTmFtZSI6InR5cGUiLCJWYWx1ZSI6IkNyZWF0ZVRhYmxlQWN0aW9uIn0seyJOYW1lIjoiY29udGVudCIsIlZhbHVlIjoie1wiVGFibGVcIjp7XCJpZFwiOjgsXCJOYW1lXCI6XCJEZXZpY2VQb2ludFwiLFwiRGF0YWJhc2VJRFwiOjIsXCJpTG9ja1wiOjB9LFwiQ29sdW1uc1wiOlt7XCJpZFwiOjQyLFwiTmFtZVwiOlwiaWRcIixcIklzQXV0b0luY3JlbWVudFwiOnRydWUsXCJDYW5OdWxsXCI6ZmFsc2UsXCJkYlR5cGVcIjpcImludFwiLFwiVGFibGVJRFwiOjgsXCJJc1BLSURcIjp0cnVlLFwib3JkZXJpZFwiOjB9LHtcImlkXCI6NDMsXCJOYW1lXCI6XCJEZXZpY2VJRFwiLFwiSXNBdXRvSW5jcmVtZW50XCI6ZmFsc2UsXCJDYW5OdWxsXCI6dHJ1ZSxcImRiVHlwZVwiOlwiaW50XCIsXCJsZW5ndGhcIjpcIlwiLFwiVGFibGVJRFwiOjgsXCJJc1BLSURcIjpmYWxzZSxcIm9yZGVyaWRcIjoxfSx7XCJpZFwiOjQ0LFwiTmFtZVwiOlwiQWRkcmVzc1wiLFwiSXNBdXRvSW5jcmVtZW50XCI6ZmFsc2UsXCJDYW5OdWxsXCI6dHJ1ZSxcImRiVHlwZVwiOlwidmFyY2hhclwiLFwibGVuZ3RoXCI6XCIyMDBcIixcIlRhYmxlSURcIjo4LFwiSXNQS0lEXCI6ZmFsc2UsXCJvcmRlcmlkXCI6Mn0se1wiaWRcIjo0NSxcImNhcHRpb25cIjpcIuaYr+WQpuebkea1i+WPmOWMllwiLFwiTmFtZVwiOlwiSXNXYXRjaGluZ1wiLFwiSXNBdXRvSW5jcmVtZW50XCI6ZmFsc2UsXCJDYW5OdWxsXCI6dHJ1ZSxcImRiVHlwZVwiOlwiYml0XCIsXCJsZW5ndGhcIjpcIlwiLFwiZGVmYXVsdFZhbHVlXCI6XCIwXCIsXCJUYWJsZUlEXCI6OCxcIklzUEtJRFwiOmZhbHNlLFwib3JkZXJpZFwiOjN9XSxcIklEWENvbmZpZ3NcIjpbXSxcIklEXCI6MH0ifSx7Ik5hbWUiOiJkYXRhYmFzZWlkIiwiVmFsdWUiOjJ9XSwiUm93U3RhdGUiOjB9LHsiSXRlbXMiOlt7Ik5hbWUiOiJpZCIsIlZhbHVlIjoyNH0seyJOYW1lIjoidHlwZSIsIlZhbHVlIjoiQ2hhbmdlVGFibGVBY3Rpb24ifSx7Ik5hbWUiOiJjb250ZW50IiwiVmFsdWUiOiJ7XCJPbGRUYWJsZU5hbWVcIjpcIkRldmljZVwiLFwiTmV3VGFibGVOYW1lXCI6XCJEZXZpY2VcIixcIm90aGVyQ29sdW1uc1wiOlt7XCJpZFwiOjM5LFwiTmFtZVwiOlwiaWRcIixcIklzQXV0b0luY3JlbWVudFwiOnRydWUsXCJDYW5OdWxsXCI6ZmFsc2UsXCJkYlR5cGVcIjpcImludFwiLFwiVGFibGVJRFwiOjcsXCJJc1BLSURcIjp0cnVlLFwib3JkZXJpZFwiOjB9LHtcImlkXCI6NDAsXCJOYW1lXCI6XCJEcml2ZXJJRFwiLFwiSXNBdXRvSW5jcmVtZW50XCI6ZmFsc2UsXCJDYW5OdWxsXCI6dHJ1ZSxcImRiVHlwZVwiOlwiaW50XCIsXCJsZW5ndGhcIjpcIlwiLFwiVGFibGVJRFwiOjcsXCJJc1BLSURcIjpmYWxzZSxcIm9yZGVyaWRcIjoxfSx7XCJpZFwiOjQxLFwiY2FwdGlvblwiOlwi5Zyw5Z2A5L+h5oGvXCIsXCJOYW1lXCI6XCJBZGRyZXNzXCIsXCJJc0F1dG9JbmNyZW1lbnRcIjpmYWxzZSxcIkNhbk51bGxcIjp0cnVlLFwiZGJUeXBlXCI6XCJ2YXJjaGFyXCIsXCJsZW5ndGhcIjpcIjEwMFwiLFwiVGFibGVJRFwiOjcsXCJJc1BLSURcIjpmYWxzZSxcIm9yZGVyaWRcIjoyfV0sXCJuZXdDb2x1bW5zXCI6W10sXCJjaGFuZ2VkQ29sdW1uc1wiOltdLFwiZGVsZXRlZENvbHVtbnNcIjpbXSxcIklEWENvbmZpZ3NcIjpbXSxcIklEXCI6MH0ifSx7Ik5hbWUiOiJkYXRhYmFzZWlkIiwiVmFsdWUiOjJ9XSwiUm93U3RhdGUiOjB9LHsiSXRlbXMiOlt7Ik5hbWUiOiJpZCIsIlZhbHVlIjoyNX0seyJOYW1lIjoidHlwZSIsIlZhbHVlIjoiQ2hhbmdlVGFibGVBY3Rpb24ifSx7Ik5hbWUiOiJjb250ZW50IiwiVmFsdWUiOiJ7XCJPbGRUYWJsZU5hbWVcIjpcIkNvbW11bmljYXRpb25Ecml2ZXJcIixcIk5ld1RhYmxlTmFtZVwiOlwiQ29tbXVuaWNhdGlvbkRyaXZlclwiLFwib3RoZXJDb2x1bW5zXCI6W3tcImlkXCI6MzQsXCJOYW1lXCI6XCJpZFwiLFwiSXNBdXRvSW5jcmVtZW50XCI6dHJ1ZSxcIkNhbk51bGxcIjpmYWxzZSxcImRiVHlwZVwiOlwiaW50XCIsXCJUYWJsZUlEXCI6NixcIklzUEtJRFwiOnRydWUsXCJvcmRlcmlkXCI6MH0se1wiaWRcIjozNSxcImNhcHRpb25cIjpcIuWQjeensFwiLFwiTmFtZVwiOlwiTmFtZVwiLFwiSXNBdXRvSW5jcmVtZW50XCI6ZmFsc2UsXCJDYW5OdWxsXCI6dHJ1ZSxcImRiVHlwZVwiOlwidmFyY2hhclwiLFwibGVuZ3RoXCI6XCI1MFwiLFwiVGFibGVJRFwiOjYsXCJJc1BLSURcIjpmYWxzZSxcIm9yZGVyaWRcIjoxfSx7XCJpZFwiOjM2LFwiY2FwdGlvblwiOlwi5Zyw5Z2AXCIsXCJOYW1lXCI6XCJBZGRyZXNzXCIsXCJJc0F1dG9JbmNyZW1lbnRcIjpmYWxzZSxcIkNhbk51bGxcIjp0cnVlLFwiZGJUeXBlXCI6XCJ2YXJjaGFyXCIsXCJsZW5ndGhcIjpcIjUwXCIsXCJUYWJsZUlEXCI6NixcIklzUEtJRFwiOmZhbHNlLFwib3JkZXJpZFwiOjJ9LHtcImlkXCI6MzcsXCJjYXB0aW9uXCI6XCLnq6/lj6NcIixcIk5hbWVcIjpcIlBvcnRcIixcIklzQXV0b0luY3JlbWVudFwiOmZhbHNlLFwiQ2FuTnVsbFwiOnRydWUsXCJkYlR5cGVcIjpcImludFwiLFwibGVuZ3RoXCI6XCJcIixcIlRhYmxlSURcIjo2LFwiSXNQS0lEXCI6ZmFsc2UsXCJvcmRlcmlkXCI6M30se1wiaWRcIjozOCxcImNhcHRpb25cIjpcIueKtuaAgVwiLFwiTmFtZVwiOlwiU3RhdHVzXCIsXCJJc0F1dG9JbmNyZW1lbnRcIjpmYWxzZSxcIkNhbk51bGxcIjp0cnVlLFwiZGJUeXBlXCI6XCJpbnRcIixcIkVudW1EZWZpbmVcIjpcIk5vbmUgPSAwLFxcbk9ubGluZSA9IDEsXFxuT2ZmbGluZSA9IDJcIixcImxlbmd0aFwiOlwiXCIsXCJkZWZhdWx0VmFsdWVcIjpcIjBcIixcIlRhYmxlSURcIjo2LFwiSXNQS0lEXCI6ZmFsc2UsXCJvcmRlcmlkXCI6NH1dLFwibmV3Q29sdW1uc1wiOlt7XCJpZFwiOjQ2LFwiTmFtZVwiOlwiU3VwcG9ydEVudW1EZXZpY2VcIixcIklzQXV0b0luY3JlbWVudFwiOmZhbHNlLFwiQ2FuTnVsbFwiOnRydWUsXCJkYlR5cGVcIjpcImJpdFwiLFwibGVuZ3RoXCI6XCJcIixcImRlZmF1bHRWYWx1ZVwiOlwiMFwiLFwiVGFibGVJRFwiOjYsXCJJc1BLSURcIjpmYWxzZSxcIm9yZGVyaWRcIjo1fV0sXCJjaGFuZ2VkQ29sdW1uc1wiOltdLFwiZGVsZXRlZENvbHVtbnNcIjpbXSxcIklEWENvbmZpZ3NcIjpbXSxcIklEXCI6MH0ifSx7Ik5hbWUiOiJkYXRhYmFzZWlkIiwiVmFsdWUiOjJ9XSwiUm93U3RhdGUiOjB9LHsiSXRlbXMiOlt7Ik5hbWUiOiJpZCIsIlZhbHVlIjoyNn0seyJOYW1lIjoidHlwZSIsIlZhbHVlIjoiQ2hhbmdlVGFibGVBY3Rpb24ifSx7Ik5hbWUiOiJjb250ZW50IiwiVmFsdWUiOiJ7XCJPbGRUYWJsZU5hbWVcIjpcIkNvbW11bmljYXRpb25Ecml2ZXJcIixcIk5ld1RhYmxlTmFtZVwiOlwiQ29tbXVuaWNhdGlvbkRyaXZlclwiLFwib3RoZXJDb2x1bW5zXCI6W3tcImlkXCI6MzQsXCJOYW1lXCI6XCJpZFwiLFwiSXNBdXRvSW5jcmVtZW50XCI6dHJ1ZSxcIkNhbk51bGxcIjpmYWxzZSxcImRiVHlwZVwiOlwiaW50XCIsXCJUYWJsZUlEXCI6NixcIklzUEtJRFwiOnRydWUsXCJvcmRlcmlkXCI6MH0se1wiaWRcIjozNSxcImNhcHRpb25cIjpcIuWQjeensFwiLFwiTmFtZVwiOlwiTmFtZVwiLFwiSXNBdXRvSW5jcmVtZW50XCI6ZmFsc2UsXCJDYW5OdWxsXCI6dHJ1ZSxcImRiVHlwZVwiOlwidmFyY2hhclwiLFwibGVuZ3RoXCI6XCI1MFwiLFwiVGFibGVJRFwiOjYsXCJJc1BLSURcIjpmYWxzZSxcIm9yZGVyaWRcIjoxfSx7XCJpZFwiOjM2LFwiY2FwdGlvblwiOlwi5Zyw5Z2AXCIsXCJOYW1lXCI6XCJBZGRyZXNzXCIsXCJJc0F1dG9JbmNyZW1lbnRcIjpmYWxzZSxcIkNhbk51bGxcIjp0cnVlLFwiZGJUeXBlXCI6XCJ2YXJjaGFyXCIsXCJsZW5ndGhcIjpcIjUwXCIsXCJUYWJsZUlEXCI6NixcIklzUEtJRFwiOmZhbHNlLFwib3JkZXJpZFwiOjJ9LHtcImlkXCI6MzcsXCJjYXB0aW9uXCI6XCLnq6/lj6NcIixcIk5hbWVcIjpcIlBvcnRcIixcIklzQXV0b0luY3JlbWVudFwiOmZhbHNlLFwiQ2FuTnVsbFwiOnRydWUsXCJkYlR5cGVcIjpcImludFwiLFwibGVuZ3RoXCI6XCJcIixcIlRhYmxlSURcIjo2LFwiSXNQS0lEXCI6ZmFsc2UsXCJvcmRlcmlkXCI6M30se1wiaWRcIjozOCxcImNhcHRpb25cIjpcIueKtuaAgVwiLFwiTmFtZVwiOlwiU3RhdHVzXCIsXCJJc0F1dG9JbmNyZW1lbnRcIjpmYWxzZSxcIkNhbk51bGxcIjp0cnVlLFwiZGJUeXBlXCI6XCJpbnRcIixcIkVudW1EZWZpbmVcIjpcIk5vbmUgPSAwLFxcbk9ubGluZSA9IDEsXFxuT2ZmbGluZSA9IDJcIixcImxlbmd0aFwiOlwiXCIsXCJkZWZhdWx0VmFsdWVcIjpcIjBcIixcIlRhYmxlSURcIjo2LFwiSXNQS0lEXCI6ZmFsc2UsXCJvcmRlcmlkXCI6NH0se1wiaWRcIjo0NixcIk5hbWVcIjpcIlN1cHBvcnRFbnVtRGV2aWNlXCIsXCJJc0F1dG9JbmNyZW1lbnRcIjpmYWxzZSxcIkNhbk51bGxcIjp0cnVlLFwiZGJUeXBlXCI6XCJiaXRcIixcImxlbmd0aFwiOlwiXCIsXCJkZWZhdWx0VmFsdWVcIjpcIjBcIixcIlRhYmxlSURcIjo2LFwiSXNQS0lEXCI6ZmFsc2UsXCJvcmRlcmlkXCI6NX1dLFwibmV3Q29sdW1uc1wiOlt7XCJpZFwiOjQ3LFwiTmFtZVwiOlwiU3VwcG9ydEVudW1Qb2ludHNcIixcIklzQXV0b0luY3JlbWVudFwiOmZhbHNlLFwiQ2FuTnVsbFwiOnRydWUsXCJkYlR5cGVcIjpcImJpdFwiLFwibGVuZ3RoXCI6XCJcIixcImRlZmF1bHRWYWx1ZVwiOlwiMFwiLFwiVGFibGVJRFwiOjYsXCJJc1BLSURcIjpmYWxzZSxcIm9yZGVyaWRcIjo2fV0sXCJjaGFuZ2VkQ29sdW1uc1wiOltdLFwiZGVsZXRlZENvbHVtbnNcIjpbXSxcIklEWENvbmZpZ3NcIjpbXSxcIklEXCI6MH0ifSx7Ik5hbWUiOiJkYXRhYmFzZWlkIiwiVmFsdWUiOjJ9XSwiUm93U3RhdGUiOjB9XSwiQ29sdW1ucyI6W3siQ29sdW1uTmFtZSI6ImlkIiwiRGF0YVR5cGUiOiJTeXN0ZW0uSW50NjQifSx7IkNvbHVtbk5hbWUiOiJ0eXBlIiwiRGF0YVR5cGUiOiJTeXN0ZW0uU3RyaW5nIn0seyJDb2x1bW5OYW1lIjoiY29udGVudCIsIkRhdGFUeXBlIjoiU3lzdGVtLlN0cmluZyJ9LHsiQ29sdW1uTmFtZSI6ImRhdGFiYXNlaWQiLCJEYXRhVHlwZSI6IlN5c3RlbS5JbnQ2NCJ9XX1dLCJEYXRhU2V0TmFtZSI6IjQzNzdiMGQ2LWVmZjktNDMzMC1hYmViLWYwYWEzYWYyNmVhZSJ9";}}
