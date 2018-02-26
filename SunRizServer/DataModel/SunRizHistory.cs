
using System;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;

namespace SunRizServer{


    /// <summary>
	/// 历史记录
	/// </summary>
    [System.ComponentModel.DataAnnotations.Schema.Table("history")]
    [Way.EntityDB.Attributes.Table("id")]
    public class History :Way.EntityDB.DataItem
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

        System.Nullable<Int32> _PointId;
        /// <summary>
        /// 
        /// </summary>
        [System.ComponentModel.DataAnnotations.Schema.Column("pointid")]
        [Way.EntityDB.WayDBColumnAttribute(Name="pointid",Comment="",Caption="",Storage = "_PointId",DbType="int")]
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
                    this.SendPropertyChanging("PointId",this._PointId,value);
                    this._PointId = value;
                    this.SendPropertyChanged("PointId");

                }
            }
        }

        String _Address;
        /// <summary>
        /// 点地址
        /// </summary>
        [System.ComponentModel.DataAnnotations.Schema.Column("address")]
        [Way.EntityDB.WayDBColumnAttribute(Name="address",Comment="",Caption="点地址",Storage = "_Address",DbType="varchar(100)")]
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

        System.Nullable<DateTime> _Time;
        /// <summary>
        /// 
        /// </summary>
        [System.ComponentModel.DataAnnotations.Schema.Column("time")]
        [Way.EntityDB.WayDBColumnAttribute(Name="time",Comment="",Caption="",Storage = "_Time",DbType="datetime")]
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
                    this.SendPropertyChanging("Time",this._Time,value);
                    this._Time = value;
                    this.SendPropertyChanged("Time");

                }
            }
        }

        System.Nullable<double> _Value;
        /// <summary>
        /// 
        /// </summary>
        [System.ComponentModel.DataAnnotations.Schema.Column("value")]
        [Way.EntityDB.WayDBColumnAttribute(Name="value",Comment="",Caption="",Storage = "_Value",DbType="double")]
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
                    this.SendPropertyChanging("Value",this._Value,value);
                    this._Value = value;
                    this.SendPropertyChanged("Value");

                }
            }
        }
}}

namespace SunRizServer.DB{
    /// <summary>
	/// 
	/// </summary>
    public class SunRizHistory : Way.EntityDB.DBContext
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="dbType"></param>
        public SunRizHistory(string connection, Way.EntityDB.DatabaseType dbType): base(connection, dbType)
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
            var db =  sender as SunRizServer.DB.SunRizHistory;
            if (db == null)
                return;


        }

        /// <summary>
	    /// 
	    /// </summary>
        /// <param name="modelBuilder"></param>
         protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
   modelBuilder.Entity<SunRizServer.History>().HasKey(m => m.id);
}

        System.Linq.IQueryable<SunRizServer.History> _History;
        /// <summary>
        /// 历史记录
        /// </summary>
        public virtual System.Linq.IQueryable<SunRizServer.History> History
        {
             get
            {
                if (_History == null)
                {
                    _History = this.Set<SunRizServer.History>();
                }
                return _History;
            }
        }

protected override string GetDesignString(){System.Text.StringBuilder result = new System.Text.StringBuilder(); 
result.Append("eyJUYWJsZXMiOlt7IlRhYmxlTmFtZSI6IlNxbGl0ZSIsIlJvd3MiOlt7Ikl0ZW1zIjpbeyJOYW1lIjoiaWQiLCJWYWx1ZSI6NzR9LHsiTmFtZSI6InR5cGUiLCJWYWx1ZSI6IkNyZWF0ZVRhYmxlQWN0aW9uIn0seyJOYW1lIjoiY29udGVudCIsIlZhbHVlIjoie1wi");
result.Append("VGFibGVcIjp7XCJpZFwiOjE1LFwiY2FwdGlvblwiOlwi5Y6G5Y+y6K6w5b2VXCIsXCJOYW1lXCI6XCJIaXN0b3J5XCIsXCJEYXRhYmFzZUlEXCI6MyxcImlMb2NrXCI6MH0sXCJDb2x1bW5zXCI6W3tcImlkXCI6MjU5LFwiTmFtZVwiOlwiaWRcIixcIklzQXV0b0lu");
result.Append("Y3JlbWVudFwiOnRydWUsXCJDYW5OdWxsXCI6ZmFsc2UsXCJkYlR5cGVcIjpcImludFwiLFwiVGFibGVJRFwiOjE1LFwiSXNQS0lEXCI6dHJ1ZSxcIm9yZGVyaWRcIjowfSx7XCJpZFwiOjI2MCxcIk5hbWVcIjpcIlBvaW50SWRcIixcIklzQXV0b0luY3JlbWVudFwi");
result.Append("OmZhbHNlLFwiQ2FuTnVsbFwiOnRydWUsXCJkYlR5cGVcIjpcImludFwiLFwibGVuZ3RoXCI6XCJcIixcIlRhYmxlSURcIjoxNSxcIklzUEtJRFwiOmZhbHNlLFwib3JkZXJpZFwiOjF9LHtcImlkXCI6MjYxLFwiY2FwdGlvblwiOlwi54K55Zyw5Z2AXCIsXCJOYW1l");
result.Append("XCI6XCJBZGRyZXNzXCIsXCJJc0F1dG9JbmNyZW1lbnRcIjpmYWxzZSxcIkNhbk51bGxcIjp0cnVlLFwiZGJUeXBlXCI6XCJ2YXJjaGFyXCIsXCJsZW5ndGhcIjpcIjEwMFwiLFwiVGFibGVJRFwiOjE1LFwiSXNQS0lEXCI6ZmFsc2UsXCJvcmRlcmlkXCI6Mn0se1wi");
result.Append("aWRcIjoyNjIsXCJOYW1lXCI6XCJUaW1lXCIsXCJJc0F1dG9JbmNyZW1lbnRcIjpmYWxzZSxcIkNhbk51bGxcIjp0cnVlLFwiZGJUeXBlXCI6XCJkYXRldGltZVwiLFwibGVuZ3RoXCI6XCJcIixcIlRhYmxlSURcIjoxNSxcIklzUEtJRFwiOmZhbHNlLFwib3JkZXJp");
result.Append("ZFwiOjN9LHtcImlkXCI6MjYzLFwiTmFtZVwiOlwiVmFsdWVcIixcIklzQXV0b0luY3JlbWVudFwiOmZhbHNlLFwiQ2FuTnVsbFwiOnRydWUsXCJkYlR5cGVcIjpcImRvdWJsZVwiLFwibGVuZ3RoXCI6XCJcIixcIlRhYmxlSURcIjoxNSxcIklzUEtJRFwiOmZhbHNl");
result.Append("LFwib3JkZXJpZFwiOjR9XSxcIklEWENvbmZpZ3NcIjpbXSxcIklEXCI6MH0ifSx7Ik5hbWUiOiJkYXRhYmFzZWlkIiwiVmFsdWUiOjN9XSwiUm93U3RhdGUiOjB9XSwiQ29sdW1ucyI6W3siQ29sdW1uTmFtZSI6ImlkIiwiRGF0YVR5cGUiOiJTeXN0ZW0uSW50NjQi");
result.Append("fSx7IkNvbHVtbk5hbWUiOiJ0eXBlIiwiRGF0YVR5cGUiOiJTeXN0ZW0uU3RyaW5nIn0seyJDb2x1bW5OYW1lIjoiY29udGVudCIsIkRhdGFUeXBlIjoiU3lzdGVtLlN0cmluZyJ9LHsiQ29sdW1uTmFtZSI6ImRhdGFiYXNlaWQiLCJEYXRhVHlwZSI6IlN5c3RlbS5J");
result.Append("bnQ2NCJ9XX1dLCJEYXRhU2V0TmFtZSI6IjcwNGNkY2NjLWZmZWYtNDdlZi1hMDAyLThlZDVkMjg3NGJmNyJ9");
return result.ToString();}
}}
