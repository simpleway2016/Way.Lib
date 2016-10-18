
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Linq.Mapping;
using System.Drawing;
using System.Linq;
using System.Text;
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
    [Serializable]
    [EntityDB.Attributes.Table("id")]
    [System.Data.Linq.Mapping.TableAttribute(Name = @"Bug")]
    public class Bug :EntityDB.DataItem
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
[EntityDB.WayLinqColumnAttribute(Comment="",Caption="id",Storage = "_id",DbType="int" ,IsPrimaryKey=true,AutoSync=AutoSync.OnInsert,IsDbGenerated=true)]
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
[EntityDB.WayLinqColumnAttribute(Comment="",Caption="标题",Storage = "_Title",DbType="varchar(50)")]
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
[EntityDB.WayLinqColumnAttribute(Comment="",Caption="提交者ID",Storage = "_SubmitUserID",DbType="int")]
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
[EntityDB.WayLinqColumnAttribute(Comment="",Caption="提交时间",Storage = "_SubmitTime",DbType="datetime")]
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
[EntityDB.WayLinqColumnAttribute(Comment="",Caption="处理者ID",Storage = "_HandlerID",DbType="int")]
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
[EntityDB.WayLinqColumnAttribute(Comment="",Caption="最后反馈时间",Storage = "_LastDate",DbType="datetime")]
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
[EntityDB.WayLinqColumnAttribute(Comment="",Caption="处理完毕时间",Storage = "_FinishTime",DbType="datetime")]
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
[EntityDB.WayLinqColumnAttribute(Comment="",Caption="当前状态",Storage = "_Status",DbType="int")]
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