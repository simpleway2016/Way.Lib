using Microsoft.CSharp;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Way.EJServer
{
    /*
    这里修改了EntityFramework src\EntityFramework\ModelConfiguration\Mappers\TypeMapper.cs
     * var baseType = _mappingContext.Model.GetEntityType(type.BaseType().Name);
                baseType = null;
     * 这里把baseType设为了null
    */

    public class CodeBuilder : ICodeBuilder
    {
        
        public  string BuilderDB(EJDB db, EJ.Databases databaseObj, string nameSpace,List<EJ.DBTable> tables)
        {

            StringBuilder _Database_deleteCodes = new StringBuilder();
            foreach (var t in tables)
            {

                var delConfigs = db.DBDeleteConfig.Where(m => m.TableID == t.id).ToList();

                if (delConfigs.Count > 0)
                {
                    StringBuilder codestrs = new StringBuilder();
                    for (int i = 0; i < delConfigs.Count; i ++ )
                    {
                        var configitem = delConfigs[i];
                        var delDBTable = db.DBTable.FirstOrDefault(m => m.id == configitem.RelaTableID);
                        var relaColumn = db.DBColumn.FirstOrDefault(m => m.id == configitem.RelaColumID);
                        var rela_pkcolumn = db.DBColumn.FirstOrDefault(m => m.TableID == configitem.RelaTableID && m.IsPKID == true);
                        if (rela_pkcolumn == null)
                            throw new Exception("关联表" + delDBTable.Name + "没有定义主键");
                        codestrs.Append(@"
                    var items" + i + @" = (from m in db."+delDBTable.Name+@"
                    where m." + relaColumn.Name + @" == deletingItem.id
                    select new " + nameSpace + @"." + delDBTable.Name + @"
                    {
                        " + rela_pkcolumn.Name + @" = m." + rela_pkcolumn.Name + @"
                    });
                    while(true)
                    {
                        var data2del = items" + i + @".Take(100).ToList();
                        if(data2del.Count() ==0)
                            break;
                        foreach (var t in data2del)
                        {
                            db.Delete(t);
                        }
                    }
");
                    }

                    _Database_deleteCodes.Append(@"
                    if (e.DataItem is " + nameSpace + @"." + t.Name + @")
                    {
                        var deletingItem = (" + nameSpace + @"." + t.Name + @")e.DataItem;
                        " + codestrs + @"
                    }
");
                }
            }


            StringBuilder result = new StringBuilder();
            result.Append(@"
namespace "+nameSpace+@".DB{
    /// <summary>
	/// 
	/// </summary>
    public class " + databaseObj.Name + @" : Way.EntityDB.DBContext
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name=""connection""></param>
        /// <param name=""dbType""></param>
        public " + databaseObj.Name + @"(string connection, Way.EntityDB.DatabaseType dbType): base(connection, dbType)
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
            var db =  sender as " + nameSpace + ".DB." + databaseObj.Name + @";
            if (db == null)
                return;

" + _Database_deleteCodes + @"
        }

        /// <summary>
	    /// 
	    /// </summary>
        /// <param name=""modelBuilder""></param>
         protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
   ");

            foreach (var t in tables)
            {
                var pkcolumn = db.DBColumn.FirstOrDefault(m => m.TableID == t.id && m.IsPKID == true);
                if (pkcolumn == null)
                    throw new Exception(string.Format("表{0}缺少主键" , t.Name));
                result.AppendLine("modelBuilder.Entity<" + nameSpace + @"." + t.Name + @">().HasKey(m => m." + pkcolumn.Name+");");
            }
            result.AppendLine("}");

            foreach (var t in tables)
            {
                result.Append(@"
        System.Linq.IQueryable<" + nameSpace + @"." + t.Name + @"> _" + t.Name + @";
        /// <summary>
        /// " + t.caption + @"
        /// </summary>
        public virtual System.Linq.IQueryable<" + nameSpace + @"." + t.Name + @"> " + t.Name + @"
        {
             get
            {
                if (_" + t.Name + @" == null)
                {
                    _" + t.Name + @" = this.Set<" + nameSpace + @"." + t.Name + @">();
                }
                return _" + t.Name + @";
            }
        }
");
            }
            result.Append("\r\n");

            
            var dt = db.Database.SelectDataSet("select * from __action where databaseid=" + databaseObj.id + " order by [id]");
            dt.Tables[0].TableName = databaseObj.dbType.ToString();
            dt.DataSetName = databaseObj.Guid;
            string json = Newtonsoft.Json.JsonConvert.SerializeObject(dt);
            string content = Convert.ToBase64String(GZip(System.Text.Encoding.UTF8.GetBytes(json)));
           
            result.AppendLine("protected override string GetDesignString(){System.Text.StringBuilder result = new System.Text.StringBuilder(); ");
            result.AppendLine("result.Append(\"\\r\\n\");");
            for (int i = 0; i < content.Length; i += 200)
            {
                int len = Math.Min(content.Length - i, 200);
                result.AppendLine("result.Append(\"" + content.Substring(i , len) + "\");");
            }
            result.AppendLine("return result.ToString();}");
            result.Append("}}\r\n");
            return result.ToString();
        }
        static byte[] GZip(byte[] byteArray)
        {
            using (System.IO.MemoryStream ms = new System.IO.MemoryStream())
            {
                System.IO.Compression.GZipStream sw = new System.IO.Compression.GZipStream(ms, System.IO.Compression.CompressionMode.Compress);
                //Compress
                sw.Write(byteArray, 0, byteArray.Length);
                //Close, DO NOT FLUSH cause bytes will go missing...
                sw.Close();
                //Transform byte[] zip data to string
                return ms.ToArray();
            }
        }

        public string[] BuildTable(EJDB db,string nameSpace, EJ.DBTable table)
        {
            var columns = db.DBColumn.Where(m => m.TableID == table.id).ToList();
            string[] codes = new string[1];
            codes[0] = BuildTable(db, nameSpace, table, columns);
            return codes;
        }
        /// <summary>
        /// </summary>
        /// <param name="type"></param>
        public static string GetLinqTypeString(string type)
        {
            switch (type)
            {

                case "bigint":
                    return "System.Nullable<Int64>";
                case "binary":
                    return "Byte[]";
                case "bit":
                    return "System.Nullable<Boolean>";
                case "char":
                    return "String";
                case "datetime":
                    return "System.Nullable<DateTime>";
                case "decimal":
                    return "System.Nullable<Decimal>";
                case "float":
                    return "System.Nullable<float>";
                case "double":
                    return "System.Nullable<double>";
                case "image":
                    return "Byte[]";
                case "int":
                    return "System.Nullable<Int32>";
                case "money":
                    return "System.Nullable<Decimal>";
                case "nchar":
                    return "String";
                case "ntext":
                    return "String";
                case "numeric":
                    return "System.Nullable<Decimal>";
                case "nvarchar":
                    return "String";
                case "real":
                    return "System.Nullable<float>";
                case "smalldatetime":
                    return "System.Nullable<DateTime>";
                case "smallint":
                    return "System.Nullable<Int16>";
                case "smallmoney":
                    return "System.Nullable<Decimal>";
                case "text":
                    return "String";
                case "timestamp":
                    return "Byte[]";
                case "varbinary":
                    return "Byte[]";
                case "varchar":
                    return "String";
                default:
                    return type;

            }
        }

        class TempAssembly
        {
            public string ClassFullName;
            public Type TableType;
        }
          

        static string BuildTable(EJDB db, string nameSpace, EJ.DBTable table, List<EJ.DBColumn> columns)
        {
            var pkcolumn = columns.FirstOrDefault(m => m.IsPKID == true);
            StringBuilder result = new StringBuilder();
            StringBuilder enumDefines = new StringBuilder();
            

            result.Append(@"

    /// <summary>
	/// " + table.caption + @"
	/// </summary>
    [System.ComponentModel.DataAnnotations.Schema.Table("""+ table.Name.ToLower() +@""")]
    [Way.EntityDB.Attributes.Table(""" + (pkcolumn == null ? "" : pkcolumn.Name.Trim()) + @""")]
    public class " + table.Name + @" :Way.EntityDB.DataItem
    {

        /// <summary>
	    /// 
	    /// </summary>
        public  " + table.Name + @"()
        {
        }

");

            foreach (var column in columns)
            {
                string caption = column.caption == null ? "" : column.caption;
                if (caption.Contains(","))
                {
                    caption = caption.Substring(0, caption.IndexOf(","));
                }
                else if (caption.Contains("，"))
                {
                    caption = caption.Substring(0, caption.IndexOf("，"));
                }

                string dataType = GetLinqTypeString(column.dbType);
                string att = ",DbType=\"" + column.dbType;
                if (string.IsNullOrEmpty(column.length))
                {
                    if (column.dbType.Contains("char"))
                    {
                        att += "(50)";
                    }
                }
                else
                {
                    if (column.dbType.Contains("char"))
                    {
                        att += "(" + column.length + ")";
                    }
                }
                att += "\"";
                if (column.IsPKID == true)
                {
                    //,AutoSync= AutoSync.OnInsert ,IsPrimaryKey=true,IsDbGenerated=true
                    att += " ,IsPrimaryKey=true";
                }
                if (column.IsAutoIncrement == true)
                {
                    att += ",IsDbGenerated=true";
                }
                if (column.CanNull == false)
                {
                    att += ",CanBeNull=false";
                }

                string eqString = "";
                if (!string.IsNullOrEmpty(column.EnumDefine) && column.dbType == "int")
                {
                    if (column.EnumDefine.Trim().StartsWith("$"))
                    {
                        var target = column.EnumDefine.Trim().Substring(1).Split('.');
                        dataType = "System.Nullable<" + target[0] + "_" + target[1] + "Enum>";
                    }
                    else
                    {
                        string[] enumitems = column.EnumDefine.Replace("\r", "").Split('\n');

                        enumDefines.Append(@"
/// <summary>
/// 
/// </summary>
public enum " + table.Name + "_" + column.Name + @"Enum:int
{
    
");
                        StringBuilder enumComments = new StringBuilder();
                        for (int i = 0; i < enumitems.Length; i++)
                        {
                            var code = enumitems[i].Trim();
                            if (code.Length == 0)
                                continue;
                            if (code.StartsWith("//"))
                            {
                                if (code.Length > 2)
                                {
                                    enumComments.AppendLine("/// " + code.Substring(2));
                                }

                            }
                            else
                            {
                                enumDefines.Append(@"
/// <summary>
" + enumComments + @"/// </summary>
");
                                enumComments.Clear();
                                enumDefines.AppendLine(code);
                            }

                        }
                        enumDefines.Append("}\r\n");

                        dataType = "System.Nullable<" + table.Name + "_" + column.Name + "Enum>";
                    }
                }

            
                if (!string.IsNullOrEmpty(column.defaultValue))
                {
                    if (column.defaultValue.Trim().Length > 0)
                    {
                        eqString = column.defaultValue.Trim();
                        if (dataType == "String")
                        {
                            if (eqString.StartsWith("'") && eqString.EndsWith("'") && eqString.Length > 1)
                                eqString = eqString.Substring(1, eqString.Length - 2);
                            eqString = "\"" + eqString + "\"";
                        }
                        else if(dataType == "System.Nullable<Decimal>")
                        {
                            eqString = eqString + "m";
                        }
                        else if (dataType == "System.Nullable<float>")
                        {
                            eqString = eqString + "f";
                        }
                        else if (dataType == "System.Nullable<Boolean>")
                        {
                            if (eqString == "1")
                                eqString = "true";
                            else if (eqString == "0")
                                eqString = "false";
                        }
                        else if (!string.IsNullOrEmpty(column.EnumDefine) && column.dbType == "int")
                        {
                            eqString = "(" + dataType + ")(" + eqString + ")";
                        }
                        eqString = "=" + eqString;
                    }
                }

                string otherAttrs = "";
                if(column.IsPKID == true)
                {
                    otherAttrs = "\r\n[System.ComponentModel.DataAnnotations.Key]";
                }

                result.Append(@"
        " + dataType + @" _" + column.Name + eqString  + @";
        /// <summary>
        /// " + column.caption + @"
        /// </summary>"+ otherAttrs + @"
        [System.ComponentModel.DataAnnotations.Schema.Column("""+ column.Name.ToLower() + @""")]
        [Way.EntityDB.WayDBColumnAttribute(Name="""+column.Name.ToLower()+@""",Comment="""",Caption=""" + caption + @""",Storage = ""_" + column.Name.Trim() + @"""" + att + @")]
        public virtual " + dataType + @" " + column.Name + @"
        {
            get
            {
                return this._" + column.Name + @";
            }
            set
            {
                if ((this._" + column.Name + @" != value))
                {
                    this.SendPropertyChanging(""" + column.Name.Trim() + @""",this._" + column.Name.Trim() + @",value);
                    this._" + column.Name + @" = value;
                    this.SendPropertyChanged(""" + column.Name.Trim() + @""");

                }
            }
        }
");
            }

            var classProperties = db.classproperty.Where(m => m.tableid == table.id).ToArray();
            foreach( var pro in classProperties )
            {
                try {
                    var foreign_table = db.DBTable.FirstOrDefault(m => m.id == pro.foreignkey_tableid);
                    var column = db.DBColumn.FirstOrDefault(m => m.id == pro.foreignkey_columnid);


                    if (pro.iscollection == false)
                    {                      
                      
                        if ( column != null && column.TableID == table.id)
                        {
                            result.Append(@"
        [System.ComponentModel.DataAnnotations.Schema.ForeignKey(""" + column.Name + @""")]
        public virtual " + foreign_table.Name + @" " + pro.name + @" { get; set; }
");
                        }
                        else
                        {
                            //与其他表一对一
                            result.Append(@"
        public virtual " + foreign_table.Name + @" " + pro.name + @" { get; set; }
");
                        }
                    }
                    else
                    {
                        if (column != null)
                        {

                            //与其他表多对一
                            result.Append(@"
        [System.ComponentModel.DataAnnotations.Schema.ForeignKey(""" + column.Name + @""")]
        public virtual ICollection<" + foreign_table.Name + @"> " + pro.name + @" { get; set; }
");
                        }
                    }
                }
                catch
                {
                }
               
            }

            result.Append("}}\r\n");

            result.Insert(0, @"namespace " + nameSpace + @"{
" + enumDefines);

            return result.ToString();
        }
        public static string GetTypeString(string type)
        {
            switch (type)
            {
                case "bigint":
                    return "Int64";
                case "binary":
                    return "Byte[]";
                case "bit":
                    return "Boolean";
                case "char":
                    return "string";
                case "datetime":
                    return "DateTime";
                case "decimal":
                    return "Decimal";
                case "float":
                    return "Double";
                case "image":
                    return "Byte[]";
                case "int":
                    return "Int32";
                case "money":
                    return "Decimal";
                case "nchar":
                    return "String";
                case "ntext":
                    return "String";
                case "numeric":
                    return "Decimal";
                case "nvarchar":
                    return "String";
                case "real":
                    return "Single";
                case "smalldatetime":
                    return "DateTime";
                case "smallint":
                    return "Int16";
                case "smallmoney":
                    return "Decimal";
                case "sql_variant":
                    return "Object";
                case "text":
                    return "String";
                case "timestamp":
                    return "Byte[]";
                case "tinyint":
                    return "Byte";
                case "uniqueidentifier":
                    return "Guid";
                case "varbinary":
                    return "Byte[]";
                case "varchar":
                    return "String";

            }
            return "";
        }

        /// <summary>
        /// 生成一般的model类
        /// </summary>
        /// <param name="db"></param>
        /// <param name="nameSpace"></param>
        /// <param name="table"></param>
        /// <returns></returns>
        public string[] BuildSimpleTable(EJDB db, string nameSpace, EJ.DBTable table)
        {
            var columns = db.DBColumn.Where(m => m.TableID == table.id).ToList();
            string[] codes = new string[1];
            codes[0] = BuildSimpleTable(db, nameSpace, table, columns);
            return codes;
        }

        static string BuildSimpleTable(EJDB db, string nameSpace, EJ.DBTable table, List<EJ.DBColumn> columns)
        {
            var pkcolumn = columns.FirstOrDefault(m => m.IsPKID == true);
            StringBuilder result = new StringBuilder();
            StringBuilder enumDefines = new StringBuilder();


            result.Append(@"

    /// <summary>
	/// " + table.caption + @"
	/// </summary>
    public class " + table.Name + @" :Way.Lib.DataModel
    {

        /// <summary>
	    /// 
	    /// </summary>
        public  " + table.Name + @"()
        {
        }

");

            foreach (var column in columns)
            {
                string caption = column.caption == null ? "" : column.caption;
                if (caption.Contains(","))
                {
                    caption = caption.Substring(0, caption.IndexOf(","));
                }
                else if (caption.Contains("，"))
                {
                    caption = caption.Substring(0, caption.IndexOf("，"));
                }

                string dataType = GetLinqTypeString(column.dbType);
                string att = ",DbType=\"" + column.dbType;
                if (string.IsNullOrEmpty(column.length))
                {
                    if (column.dbType.Contains("char"))
                    {
                        att += "(50)";
                    }
                }
                else
                {
                    if (column.dbType.Contains("char"))
                    {
                        att += "(" + column.length + ")";
                    }
                }
                att += "\"";
                if (column.IsPKID == true)
                {
                    //,AutoSync= AutoSync.OnInsert ,IsPrimaryKey=true,IsDbGenerated=true
                    att += " ,IsPrimaryKey=true";
                }
                if (column.IsAutoIncrement == true)
                {
                    att += ",IsDbGenerated=true";
                }
                if (column.CanNull == false)
                {
                    att += ",CanBeNull=false";
                }

                string eqString = "";
                if (!string.IsNullOrEmpty(column.EnumDefine) && column.dbType == "int")
                {
                    if (column.EnumDefine.Trim().StartsWith("$"))
                    {
                        var target = column.EnumDefine.Trim().Substring(1).Split('.');
                        dataType = "System.Nullable<" + target[0] + "_" + target[1] + "Enum>";
                    }
                    else
                    {
                        string[] enumitems = column.EnumDefine.Replace("\r", "").Split('\n');

                        enumDefines.Append(@"
/// <summary>
/// 
/// </summary>
public enum " + table.Name + "_" + column.Name + @"Enum:int
{
    
");
                        StringBuilder enumComments = new StringBuilder();
                        for (int i = 0; i < enumitems.Length; i++)
                        {
                            var code = enumitems[i].Trim();
                            if (code.Length == 0)
                                continue;
                            if (code.StartsWith("//"))
                            {
                                if (code.Length > 2)
                                {
                                    enumComments.AppendLine("/// " + code.Substring(2));
                                }

                            }
                            else
                            {
                                enumDefines.Append(@"
/// <summary>
" + enumComments + @"/// </summary>
");
                                enumComments.Clear();
                                enumDefines.AppendLine(code);
                            }

                        }
                        enumDefines.Append("}\r\n");

                        dataType = "System.Nullable<" + table.Name + "_" + column.Name + "Enum>";
                    }
                }


                if (!string.IsNullOrEmpty(column.defaultValue))
                {
                    if (column.defaultValue.Trim().Length > 0)
                    {
                        eqString = column.defaultValue.Trim();
                        if (dataType == "String")
                        {
                            if (eqString.StartsWith("'") && eqString.EndsWith("'") && eqString.Length > 1)
                                eqString = eqString.Substring(1, eqString.Length - 2);
                            eqString = "\"" + eqString + "\"";
                        }
                        else if (dataType == "System.Nullable<Decimal>")
                        {
                            eqString = eqString + "m";
                        }
                        else if (dataType == "System.Nullable<float>")
                        {
                            eqString = eqString + "f";
                        }
                        else if (dataType == "System.Nullable<Boolean>")
                        {
                            if (eqString == "1")
                                eqString = "true";
                            else if (eqString == "0")
                                eqString = "false";
                        }
                        else if (!string.IsNullOrEmpty(column.EnumDefine) && column.dbType == "int")
                        {
                            eqString = "(" + dataType + ")(" + eqString + ")";
                        }
                        eqString = "=" + eqString;
                    }
                }

               
                result.Append(@"
        " + dataType + @" _" + column.Name + eqString + @";
        /// <summary>
        /// " + column.caption + @"
        /// </summary>
        public virtual " + dataType + @" " + column.Name + @"
        {
            get
            {
                return this._" + column.Name + @";
            }
            set
            {
                if ((this._" + column.Name + @" != value))
                {
                    var original = this._" + column.Name + @";
                    this._" + column.Name + @" = value;
                    this.OnPropertyChanged(""" + column.Name.Trim() + @""",original,value);

                }
            }
        }
");
            }


            var classProperties = db.classproperty.Where(m => m.tableid == table.id).ToArray();
            foreach (var pro in classProperties)
            {
                try
                {
                    var foreign_table = db.DBTable.FirstOrDefault(m => m.id == pro.foreignkey_tableid);
                    if (pro.iscollection == false)
                    {

                        var column = db.DBColumn.FirstOrDefault(m => m.id == pro.foreignkey_columnid);
                        if (column.TableID == table.id)
                        {
                            result.Append(@"
        public virtual " + foreign_table.Name + @" " + pro.name + @" { get; set; }
");
                        }
                        else
                        {
                            //与其他表一对一
                            result.Append(@"
        public virtual " + foreign_table.Name + @" " + pro.name + @" { get; set; }
");
                        }
                    }
                    else
                    {
                        //与其他表多对一
                        result.Append(@"
        public virtual ICollection<" + foreign_table.Name + @"> " + pro.name + @" { get; set; }
");
                    }
                }
                catch
                {
                }

            }

            result.Append("}}\r\n");

            result.Insert(0, @"namespace " + nameSpace + @"{
" + enumDefines);

            return result.ToString();
        }
    }
}