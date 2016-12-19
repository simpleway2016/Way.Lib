using Microsoft.CSharp;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Web;

namespace ECWeb.Database
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
                    _" + t.Name + @" = new Way.EntityDB.WayQueryable<" + nameSpace + @"." + t.Name + @">(this.Set<" + nameSpace + @"." + t.Name + @">());
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
            result.Append("static string _designData = \""+ System.Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(json)) +"\";");
            result.Append("}}\r\n");
            return result.ToString();
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
                default:
                    return type;

            }
            return "";
        }

        class TempAssembly
        {
            public string ClassFullName;
            public Type TableType;
        }
        static List<TempAssembly> TempAssemblies = new List<TempAssembly>();
        internal static Type BuildAssemblyForSingleObject(EJ.DBTable table, EJ.DBColumn[] columns)
        {
            string nameSpace = "db" + table.DatabaseID;
            string classFullName = nameSpace + ".table" + table.id;

            var tempAssemlby = TempAssemblies.FirstOrDefault(m=>m.ClassFullName == classFullName);
            if (tempAssemlby != null)
                return tempAssemlby.TableType;

            //创建编译器实例。   
            var provider = new CSharpCodeProvider();
            //设置编译参数。   
            var paras = new CompilerParameters();
            paras.GenerateExecutable = false;
            paras.GenerateInMemory = true;

            //创建动态代码。   
            string classSource = BuildTableForSelectData(  nameSpace, table, columns);

            //编译代码。   
            CompilerResults result = provider.CompileAssemblyFromSource(paras, classSource.ToString());

            //获取编译后的程序集。   
            Assembly assembly = result.CompiledAssembly;
            Type tableType = assembly.GetTypes()[0];
            TempAssemblies.Add(new TempAssembly()
                {
                    ClassFullName = classFullName,
                    TableType = tableType
                });
            return tableType;
        }


         static string BuildTableForSelectData(  string nameSpace, EJ.DBTable table, EJ.DBColumn[] columns)
        {
            var pkcolumn = columns.FirstOrDefault(m => m.IsPKID == true);
            StringBuilder result = new StringBuilder();
            StringBuilder enumDefines = new StringBuilder();

            result.Append(@"
    public class " + table.Name + @"
    {

public  " + table.Name + @"()
        {
        }

");

            foreach (var column in columns)
            {
             
                string dataType = GetLinqTypeString(column.dbType);

                result.Append(@"
        public " + dataType + @" " + column.Name + @"
        {
            get;
            set;
        }
");
            }

            result.Append("}}");

            result.Insert(0, @"
using System;
namespace " + nameSpace + @"{
" + enumDefines);

            return result.ToString();
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
    [Way.EntityDB.Attributes.Table("""+ table.Name + @""",""" + (pkcolumn == null ? "" : pkcolumn.Name.Trim()) + @""")]
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
                string caption = column.caption.ToSafeString();
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
                   
                    string[] enumitems = column.EnumDefine.Split(',');

                    enumDefines.Append(@"
/// <summary>
/// 
	/// </summary>
public enum " + table.Name + "_" + column.Name + @"Enum:int
{
    
");
                    foreach (string enumitem in enumitems)
                    {
                        if (enumitem.Trim().Length == 0)
                            continue;
                        enumDefines.Append(@"
/// <summary>
/// 
	/// </summary>
");
                        enumDefines.Append(enumitem);
                        enumDefines.Append(",\r\n");

                    }
                    enumDefines.Append("}\r\n");

                    dataType = table.Name + "_" + column.Name + "Enum";
                    eqString = "=(" + dataType + ")int.MinValue";
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

                result.Append(@"
" + dataType + @" _" + column.Name + eqString  + @";
/// <summary>
/// " + column.caption + @"
	/// </summary>
[Way.EntityDB.WayLinqColumnAttribute(Comment="""",Caption=""" + caption + @""",Storage = ""_" + column.Name.Trim() + @"""" + att + @")]
        public " + dataType + @" " + column.Name + @"
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
        public string BuildOldClassCode(EJDB db, string nameSpace, EJ.DBTable table, List<EJ.DBColumn> columns)
        {
            return null;
        }
    
        static string BuildLinq(EJDB db, string nameSpace, EJ.DBTable table, List<EJ.DBColumn> columns)
        {
            var pkcolumn = columns.FirstOrDefault(m => m.IsPKID == true);


            StringBuilder result = new StringBuilder();
            result.Append(@"
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration;
");

            result.Append(@"
namespace " + nameSpace + @".Linq
{

        internal class " + table.Name.Trim() + @"_Configuration : EntityTypeConfiguration<" + nameSpace + @".Linq." + table.Name.Trim() + @">
    {
        internal " + table.Name.Trim() + @"_Configuration()
        {
           " + (pkcolumn == null ? "HasKey(c => c." + columns[0].Name.Trim() + ");" : " HasKey(c => c." + pkcolumn.Name.Trim() + ");") + @"
            
        }
    }

    /// <summary>
	/// " + table.caption + @"
	/// </summary>
    [Serializable]
    [System.ComponentModel.DataAnnotations.Schema.Table(""" + table.Name.Trim() + @""")]
    [EntityDB.Attributes.Table(""" + (pkcolumn == null ? "" : pkcolumn.Name.Trim()) + @""")]
    [System.Data.Linq.Mapping.TableAttribute(Name = @""" + table.Name.Trim() + @""")]
    public class " + table.Name + @" : " + nameSpace + @"." + table.Name + @"
    {
       }
}");
            return result.ToString();
        }
        static string BuildInterface(EJDB db, string nameSpace, EJ.DBTable table, List<EJ.DBColumn> columns)
        {
            return null;
        }
    }
}