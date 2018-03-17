
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SunRizServer{


    /// <summary>
	/// 历史记录
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
