
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


        }

        /// <summary>
	    /// 
	    /// </summary>
        /// <param name="modelBuilder"></param>
         protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
   modelBuilder.Entity<SunRizServer.ImageFiles>().HasKey(m => m.id);
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

static string _designData = "eyJUYWJsZXMiOlt7IlRhYmxlTmFtZSI6IlNxbGl0ZSIsIlJvd3MiOlt7Ikl0ZW1zIjpbeyJOYW1lIjoiaWQiLCJWYWx1ZSI6MTd9LHsiTmFtZSI6InR5cGUiLCJWYWx1ZSI6IkNyZWF0ZVRhYmxlQWN0aW9uIn0seyJOYW1lIjoiY29udGVudCIsIlZhbHVlIjoie1wiVGFibGVcIjp7XCJpZFwiOjUsXCJjYXB0aW9uXCI6XCLlm77niYfmlofku7ZcIixcIk5hbWVcIjpcIkltYWdlRmlsZXNcIixcIkRhdGFiYXNlSURcIjoyLFwiaUxvY2tcIjowfSxcIkNvbHVtbnNcIjpbe1wiaWRcIjoyOSxcIk5hbWVcIjpcImlkXCIsXCJJc0F1dG9JbmNyZW1lbnRcIjp0cnVlLFwiQ2FuTnVsbFwiOmZhbHNlLFwiZGJUeXBlXCI6XCJpbnRcIixcIlRhYmxlSURcIjo1LFwiSXNQS0lEXCI6dHJ1ZSxcIm9yZGVyaWRcIjowfSx7XCJpZFwiOjMwLFwiTmFtZVwiOlwiTmFtZVwiLFwiSXNBdXRvSW5jcmVtZW50XCI6ZmFsc2UsXCJDYW5OdWxsXCI6dHJ1ZSxcImRiVHlwZVwiOlwidmFyY2hhclwiLFwibGVuZ3RoXCI6XCI1MFwiLFwiVGFibGVJRFwiOjUsXCJJc1BLSURcIjpmYWxzZSxcIm9yZGVyaWRcIjoxfSx7XCJpZFwiOjMxLFwiTmFtZVwiOlwiUGFyZW50SWRcIixcIklzQXV0b0luY3JlbWVudFwiOmZhbHNlLFwiQ2FuTnVsbFwiOnRydWUsXCJkYlR5cGVcIjpcImludFwiLFwibGVuZ3RoXCI6XCJcIixcIlRhYmxlSURcIjo1LFwiSXNQS0lEXCI6ZmFsc2UsXCJvcmRlcmlkXCI6Mn0se1wiaWRcIjozMixcIk5hbWVcIjpcIklzRm9sZGVyXCIsXCJJc0F1dG9JbmNyZW1lbnRcIjpmYWxzZSxcIkNhbk51bGxcIjp0cnVlLFwiZGJUeXBlXCI6XCJiaXRcIixcImxlbmd0aFwiOlwiXCIsXCJkZWZhdWx0VmFsdWVcIjpcIjBcIixcIlRhYmxlSURcIjo1LFwiSXNQS0lEXCI6ZmFsc2UsXCJvcmRlcmlkXCI6M30se1wiaWRcIjozMyxcImNhcHRpb25cIjpcIuaWh+S7tuWunumZheaWh+S7tuWQjVwiLFwiTmFtZVwiOlwiRmlsZU5hbWVcIixcIklzQXV0b0luY3JlbWVudFwiOmZhbHNlLFwiQ2FuTnVsbFwiOnRydWUsXCJkYlR5cGVcIjpcInZhcmNoYXJcIixcImxlbmd0aFwiOlwiNTBcIixcIlRhYmxlSURcIjo1LFwiSXNQS0lEXCI6ZmFsc2UsXCJvcmRlcmlkXCI6NH1dLFwiSURYQ29uZmlnc1wiOltdLFwiSURcIjowfSJ9LHsiTmFtZSI6ImRhdGFiYXNlaWQiLCJWYWx1ZSI6Mn1dLCJSb3dTdGF0ZSI6MH0seyJJdGVtcyI6W3siTmFtZSI6ImlkIiwiVmFsdWUiOjE4fSx7Ik5hbWUiOiJ0eXBlIiwiVmFsdWUiOiJDaGFuZ2VUYWJsZUFjdGlvbiJ9LHsiTmFtZSI6ImNvbnRlbnQiLCJWYWx1ZSI6IntcIk9sZFRhYmxlTmFtZVwiOlwiSW1hZ2VGaWxlc1wiLFwiTmV3VGFibGVOYW1lXCI6XCJJbWFnZUZpbGVzXCIsXCJvdGhlckNvbHVtbnNcIjpbe1wiaWRcIjoyOSxcIk5hbWVcIjpcImlkXCIsXCJJc0F1dG9JbmNyZW1lbnRcIjp0cnVlLFwiQ2FuTnVsbFwiOmZhbHNlLFwiZGJUeXBlXCI6XCJpbnRcIixcIlRhYmxlSURcIjo1LFwiSXNQS0lEXCI6dHJ1ZSxcIm9yZGVyaWRcIjowfSx7XCJpZFwiOjMwLFwiTmFtZVwiOlwiTmFtZVwiLFwiSXNBdXRvSW5jcmVtZW50XCI6ZmFsc2UsXCJDYW5OdWxsXCI6dHJ1ZSxcImRiVHlwZVwiOlwidmFyY2hhclwiLFwibGVuZ3RoXCI6XCI1MFwiLFwiVGFibGVJRFwiOjUsXCJJc1BLSURcIjpmYWxzZSxcIm9yZGVyaWRcIjoxfSx7XCJpZFwiOjMxLFwiTmFtZVwiOlwiUGFyZW50SWRcIixcIklzQXV0b0luY3JlbWVudFwiOmZhbHNlLFwiQ2FuTnVsbFwiOnRydWUsXCJkYlR5cGVcIjpcImludFwiLFwibGVuZ3RoXCI6XCJcIixcIlRhYmxlSURcIjo1LFwiSXNQS0lEXCI6ZmFsc2UsXCJvcmRlcmlkXCI6Mn0se1wiaWRcIjozMixcIk5hbWVcIjpcIklzRm9sZGVyXCIsXCJJc0F1dG9JbmNyZW1lbnRcIjpmYWxzZSxcIkNhbk51bGxcIjp0cnVlLFwiZGJUeXBlXCI6XCJiaXRcIixcImxlbmd0aFwiOlwiXCIsXCJkZWZhdWx0VmFsdWVcIjpcIjBcIixcIlRhYmxlSURcIjo1LFwiSXNQS0lEXCI6ZmFsc2UsXCJvcmRlcmlkXCI6M30se1wiaWRcIjozMyxcImNhcHRpb25cIjpcIuaWh+S7tuWunumZheaWh+S7tuWQjVwiLFwiTmFtZVwiOlwiRmlsZU5hbWVcIixcIklzQXV0b0luY3JlbWVudFwiOmZhbHNlLFwiQ2FuTnVsbFwiOnRydWUsXCJkYlR5cGVcIjpcInZhcmNoYXJcIixcImxlbmd0aFwiOlwiNTBcIixcIlRhYmxlSURcIjo1LFwiSXNQS0lEXCI6ZmFsc2UsXCJvcmRlcmlkXCI6NH1dLFwibmV3Q29sdW1uc1wiOltdLFwiY2hhbmdlZENvbHVtbnNcIjpbXSxcImRlbGV0ZWRDb2x1bW5zXCI6W10sXCJJRFhDb25maWdzXCI6W10sXCJJRFwiOjB9In0seyJOYW1lIjoiZGF0YWJhc2VpZCIsIlZhbHVlIjoyfV0sIlJvd1N0YXRlIjowfSx7Ikl0ZW1zIjpbeyJOYW1lIjoiaWQiLCJWYWx1ZSI6MTl9LHsiTmFtZSI6InR5cGUiLCJWYWx1ZSI6IkNoYW5nZVRhYmxlQWN0aW9uIn0seyJOYW1lIjoiY29udGVudCIsIlZhbHVlIjoie1wiT2xkVGFibGVOYW1lXCI6XCJJbWFnZUZpbGVzXCIsXCJOZXdUYWJsZU5hbWVcIjpcIkltYWdlRmlsZXNcIixcIm90aGVyQ29sdW1uc1wiOlt7XCJpZFwiOjI5LFwiTmFtZVwiOlwiaWRcIixcIklzQXV0b0luY3JlbWVudFwiOnRydWUsXCJDYW5OdWxsXCI6ZmFsc2UsXCJkYlR5cGVcIjpcImludFwiLFwiVGFibGVJRFwiOjUsXCJJc1BLSURcIjp0cnVlLFwib3JkZXJpZFwiOjB9LHtcImlkXCI6MzAsXCJjYXB0aW9uXCI6XCLmmL7npLrnmoTlkI3np7BcIixcIk5hbWVcIjpcIk5hbWVcIixcIklzQXV0b0luY3JlbWVudFwiOmZhbHNlLFwiQ2FuTnVsbFwiOnRydWUsXCJkYlR5cGVcIjpcInZhcmNoYXJcIixcImxlbmd0aFwiOlwiNTBcIixcIlRhYmxlSURcIjo1LFwiSXNQS0lEXCI6ZmFsc2UsXCJvcmRlcmlkXCI6MX0se1wiaWRcIjozMixcIk5hbWVcIjpcIklzRm9sZGVyXCIsXCJJc0F1dG9JbmNyZW1lbnRcIjpmYWxzZSxcIkNhbk51bGxcIjp0cnVlLFwiZGJUeXBlXCI6XCJiaXRcIixcImxlbmd0aFwiOlwiXCIsXCJkZWZhdWx0VmFsdWVcIjpcIjBcIixcIlRhYmxlSURcIjo1LFwiSXNQS0lEXCI6ZmFsc2UsXCJvcmRlcmlkXCI6M30se1wiaWRcIjozMyxcImNhcHRpb25cIjpcIuaWh+S7tuWunumZheaWh+S7tuWQjVwiLFwiTmFtZVwiOlwiRmlsZU5hbWVcIixcIklzQXV0b0luY3JlbWVudFwiOmZhbHNlLFwiQ2FuTnVsbFwiOnRydWUsXCJkYlR5cGVcIjpcInZhcmNoYXJcIixcImxlbmd0aFwiOlwiNTBcIixcIlRhYmxlSURcIjo1LFwiSXNQS0lEXCI6ZmFsc2UsXCJvcmRlcmlkXCI6NH1dLFwibmV3Q29sdW1uc1wiOltdLFwiY2hhbmdlZENvbHVtbnNcIjpbe1wiaWRcIjozMSxcIk5hbWVcIjpcIlBhcmVudElkXCIsXCJJc0F1dG9JbmNyZW1lbnRcIjpmYWxzZSxcIkNhbk51bGxcIjp0cnVlLFwiZGJUeXBlXCI6XCJpbnRcIixcImxlbmd0aFwiOlwiXCIsXCJkZWZhdWx0VmFsdWVcIjpcIjBcIixcIlRhYmxlSURcIjo1LFwiSXNQS0lEXCI6ZmFsc2UsXCJvcmRlcmlkXCI6MixcIkJhY2t1cENoYW5nZWRQcm9wZXJ0aWVzXCI6e1wiZGVmYXVsdFZhbHVlXCI6e1wiT3JpZ2luYWxWYWx1ZVwiOm51bGx9fX1dLFwiZGVsZXRlZENvbHVtbnNcIjpbXSxcIklEWENvbmZpZ3NcIjpbXSxcIklEXCI6MH0ifSx7Ik5hbWUiOiJkYXRhYmFzZWlkIiwiVmFsdWUiOjJ9XSwiUm93U3RhdGUiOjB9XSwiQ29sdW1ucyI6W3siQ29sdW1uTmFtZSI6ImlkIiwiRGF0YVR5cGUiOiJTeXN0ZW0uSW50NjQifSx7IkNvbHVtbk5hbWUiOiJ0eXBlIiwiRGF0YVR5cGUiOiJTeXN0ZW0uU3RyaW5nIn0seyJDb2x1bW5OYW1lIjoiY29udGVudCIsIkRhdGFUeXBlIjoiU3lzdGVtLlN0cmluZyJ9LHsiQ29sdW1uTmFtZSI6ImRhdGFiYXNlaWQiLCJEYXRhVHlwZSI6IlN5c3RlbS5JbnQ2NCJ9XX1dLCJEYXRhU2V0TmFtZSI6IjQzNzdiMGQ2LWVmZjktNDMzMC1hYmViLWYwYWEzYWYyNmVhZSJ9";}}
