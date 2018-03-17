
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
result.Append("LFwib3JkZXJpZFwiOjR9XSxcIklEWENvbmZpZ3NcIjpbXSxcIklEXCI6MH0ifSx7Ik5hbWUiOiJkYXRhYmFzZWlkIiwiVmFsdWUiOjN9XSwiUm93U3RhdGUiOjB9LHsiSXRlbXMiOlt7Ik5hbWUiOiJpZCIsIlZhbHVlIjo5MX0seyJOYW1lIjoidHlwZSIsIlZhbHVl");
result.Append("IjoiQ2hhbmdlVGFibGVBY3Rpb24ifSx7Ik5hbWUiOiJjb250ZW50IiwiVmFsdWUiOiJ7XCJPbGRUYWJsZU5hbWVcIjpcIkhpc3RvcnlcIixcIk5ld1RhYmxlTmFtZVwiOlwiSGlzdG9yeVwiLFwib3RoZXJDb2x1bW5zXCI6W3tcImlkXCI6MjU5LFwiTmFtZVwiOlwi");
result.Append("aWRcIixcIklzQXV0b0luY3JlbWVudFwiOnRydWUsXCJDYW5OdWxsXCI6ZmFsc2UsXCJkYlR5cGVcIjpcImludFwiLFwiVGFibGVJRFwiOjE1LFwiSXNQS0lEXCI6dHJ1ZSxcIm9yZGVyaWRcIjowfSx7XCJpZFwiOjI2MCxcIk5hbWVcIjpcIlBvaW50SWRcIixcIklz");
result.Append("QXV0b0luY3JlbWVudFwiOmZhbHNlLFwiQ2FuTnVsbFwiOnRydWUsXCJkYlR5cGVcIjpcImludFwiLFwibGVuZ3RoXCI6XCJcIixcIlRhYmxlSURcIjoxNSxcIklzUEtJRFwiOmZhbHNlLFwib3JkZXJpZFwiOjF9LHtcImlkXCI6MjYxLFwiY2FwdGlvblwiOlwi54K5");
result.Append("5Zyw5Z2AXCIsXCJOYW1lXCI6XCJBZGRyZXNzXCIsXCJJc0F1dG9JbmNyZW1lbnRcIjpmYWxzZSxcIkNhbk51bGxcIjp0cnVlLFwiZGJUeXBlXCI6XCJ2YXJjaGFyXCIsXCJsZW5ndGhcIjpcIjEwMFwiLFwiVGFibGVJRFwiOjE1LFwiSXNQS0lEXCI6ZmFsc2UsXCJv");
result.Append("cmRlcmlkXCI6Mn0se1wiaWRcIjoyNjIsXCJOYW1lXCI6XCJUaW1lXCIsXCJJc0F1dG9JbmNyZW1lbnRcIjpmYWxzZSxcIkNhbk51bGxcIjp0cnVlLFwiZGJUeXBlXCI6XCJkYXRldGltZVwiLFwibGVuZ3RoXCI6XCJcIixcIlRhYmxlSURcIjoxNSxcIklzUEtJRFwi");
result.Append("OmZhbHNlLFwib3JkZXJpZFwiOjN9LHtcImlkXCI6MjYzLFwiTmFtZVwiOlwiVmFsdWVcIixcIklzQXV0b0luY3JlbWVudFwiOmZhbHNlLFwiQ2FuTnVsbFwiOnRydWUsXCJkYlR5cGVcIjpcImRvdWJsZVwiLFwibGVuZ3RoXCI6XCJcIixcIlRhYmxlSURcIjoxNSxc");
result.Append("IklzUEtJRFwiOmZhbHNlLFwib3JkZXJpZFwiOjR9XSxcIm5ld0NvbHVtbnNcIjpbXSxcImNoYW5nZWRDb2x1bW5zXCI6W10sXCJkZWxldGVkQ29sdW1uc1wiOltdLFwiSURYQ29uZmlnc1wiOlt7XCJJc1VuaXF1ZVwiOmZhbHNlLFwiSXNDbHVzdGVyZWRcIjpmYWxz");
result.Append("ZSxcIkNvbHVtbk5hbWVzXCI6W1wiVGltZVwiXSxcIk5hbWVcIjpudWxsfV0sXCJJRFwiOjB9In0seyJOYW1lIjoiZGF0YWJhc2VpZCIsIlZhbHVlIjozfV0sIlJvd1N0YXRlIjowfSx7Ikl0ZW1zIjpbeyJOYW1lIjoiaWQiLCJWYWx1ZSI6OTJ9LHsiTmFtZSI6InR5");
result.Append("cGUiLCJWYWx1ZSI6IkNoYW5nZVRhYmxlQWN0aW9uIn0seyJOYW1lIjoiY29udGVudCIsIlZhbHVlIjoie1wiT2xkVGFibGVOYW1lXCI6XCJIaXN0b3J5XCIsXCJOZXdUYWJsZU5hbWVcIjpcIkhpc3RvcnlcIixcIm90aGVyQ29sdW1uc1wiOlt7XCJpZFwiOjI1OSxc");
result.Append("Ik5hbWVcIjpcImlkXCIsXCJJc0F1dG9JbmNyZW1lbnRcIjp0cnVlLFwiQ2FuTnVsbFwiOmZhbHNlLFwiZGJUeXBlXCI6XCJpbnRcIixcIlRhYmxlSURcIjoxNSxcIklzUEtJRFwiOnRydWUsXCJvcmRlcmlkXCI6MH0se1wiaWRcIjoyNjAsXCJOYW1lXCI6XCJQb2lu");
result.Append("dElkXCIsXCJJc0F1dG9JbmNyZW1lbnRcIjpmYWxzZSxcIkNhbk51bGxcIjp0cnVlLFwiZGJUeXBlXCI6XCJpbnRcIixcImxlbmd0aFwiOlwiXCIsXCJUYWJsZUlEXCI6MTUsXCJJc1BLSURcIjpmYWxzZSxcIm9yZGVyaWRcIjoxfSx7XCJpZFwiOjI2MSxcImNhcHRp");
result.Append("b25cIjpcIueCueWcsOWdgFwiLFwiTmFtZVwiOlwiQWRkcmVzc1wiLFwiSXNBdXRvSW5jcmVtZW50XCI6ZmFsc2UsXCJDYW5OdWxsXCI6dHJ1ZSxcImRiVHlwZVwiOlwidmFyY2hhclwiLFwibGVuZ3RoXCI6XCIxMDBcIixcIlRhYmxlSURcIjoxNSxcIklzUEtJRFwi");
result.Append("OmZhbHNlLFwib3JkZXJpZFwiOjJ9LHtcImlkXCI6MjYyLFwiTmFtZVwiOlwiVGltZVwiLFwiSXNBdXRvSW5jcmVtZW50XCI6ZmFsc2UsXCJDYW5OdWxsXCI6dHJ1ZSxcImRiVHlwZVwiOlwiZGF0ZXRpbWVcIixcImxlbmd0aFwiOlwiXCIsXCJUYWJsZUlEXCI6MTUs");
result.Append("XCJJc1BLSURcIjpmYWxzZSxcIm9yZGVyaWRcIjozfSx7XCJpZFwiOjI2MyxcIk5hbWVcIjpcIlZhbHVlXCIsXCJJc0F1dG9JbmNyZW1lbnRcIjpmYWxzZSxcIkNhbk51bGxcIjp0cnVlLFwiZGJUeXBlXCI6XCJkb3VibGVcIixcImxlbmd0aFwiOlwiXCIsXCJUYWJs");
result.Append("ZUlEXCI6MTUsXCJJc1BLSURcIjpmYWxzZSxcIm9yZGVyaWRcIjo0fV0sXCJuZXdDb2x1bW5zXCI6W10sXCJjaGFuZ2VkQ29sdW1uc1wiOltdLFwiZGVsZXRlZENvbHVtbnNcIjpbXSxcIklEWENvbmZpZ3NcIjpbe1wiSXNVbmlxdWVcIjpmYWxzZSxcIklzQ2x1c3Rl");
result.Append("cmVkXCI6ZmFsc2UsXCJDb2x1bW5OYW1lc1wiOltcIlRpbWVcIl0sXCJOYW1lXCI6bnVsbH0se1wiSXNVbmlxdWVcIjpmYWxzZSxcIklzQ2x1c3RlcmVkXCI6ZmFsc2UsXCJDb2x1bW5OYW1lc1wiOltcIkFkZHJlc3NcIl0sXCJOYW1lXCI6bnVsbH1dLFwiSURcIjow");
result.Append("fSJ9LHsiTmFtZSI6ImRhdGFiYXNlaWQiLCJWYWx1ZSI6M31dLCJSb3dTdGF0ZSI6MH1dLCJDb2x1bW5zIjpbeyJDb2x1bW5OYW1lIjoiaWQiLCJEYXRhVHlwZSI6IlN5c3RlbS5JbnQ2NCJ9LHsiQ29sdW1uTmFtZSI6InR5cGUiLCJEYXRhVHlwZSI6IlN5c3RlbS5T");
result.Append("dHJpbmcifSx7IkNvbHVtbk5hbWUiOiJjb250ZW50IiwiRGF0YVR5cGUiOiJTeXN0ZW0uU3RyaW5nIn0seyJDb2x1bW5OYW1lIjoiZGF0YWJhc2VpZCIsIkRhdGFUeXBlIjoiU3lzdGVtLkludDY0In1dfV0sIkRhdGFTZXROYW1lIjoiNzA0Y2RjY2MtZmZlZi00N2Vm");
result.Append("LWEwMDItOGVkNWQyODc0YmY3In0=");
return result.ToString();}
}}
