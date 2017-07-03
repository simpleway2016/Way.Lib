using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;


namespace Way.EntityDB.Design.Database.SqlServer
{
    [EntityDB.Attributes.DatabaseTypeAttribute(DatabaseType.SqlServer)]
    class SqlServerTableService : Services.ITableDesignService
    {
        public void CreateTable(EntityDB.IDatabaseService db, EJ.DBTable table, EJ.DBColumn[] columns
            , IndexInfo[] IDXConfigs)
        {

                string sqlstr;
                sqlstr = @"
CREATE TABLE [" + table.Name.ToLower() + @"] (
";

                foreach (EJ.DBColumn column in columns)
                {
                    sqlstr += "[" + column.Name.ToLower() + "] [" + column.dbType + "]";
                    if (column.dbType.IndexOf("char") >= 0)
                    {
                        if (!string.IsNullOrEmpty(column.length))
                            sqlstr += " (" + column.length + ")";
                        else
                        {
                            sqlstr += " (50)";
                        }
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(column.length))
                        {
                            sqlstr += " (" + column.length + ")";
                        }
                    }

                    if (column.IsAutoIncrement == true)
                    {
                        sqlstr += " IDENTITY (1, 1)";
                    }
                    if (column.CanNull == false || column.IsPKID == true || column.IsAutoIncrement == true)
                        sqlstr += " NOT";
                    sqlstr += " NULL ";

                    
                    if (!string.IsNullOrEmpty( column.defaultValue) )
                    {
                        string defaultValue = column.defaultValue.Trim();
                        sqlstr += " CONSTRAINT [DF_" + table.Name.ToLower() + "_" + column.Name.ToLower() + "] DEFAULT ('" + defaultValue.Replace("'","''") + "')";
                        
                    }

                    sqlstr += ",";
                }
            if (sqlstr.EndsWith(","))
            {
                sqlstr = sqlstr.Remove(sqlstr.Length - 1);
            }
                sqlstr += ")";


                db.ExecSqlString(sqlstr);

            foreach (var column in columns)
            {
                if (column.IsPKID == true)
                {
                    //设为主键
                    db.ExecSqlString("alter table [" + table.Name.ToLower() + "] add constraint PK_" + table.Name.ToLower() + "_" + column.Name.ToLower() + " primary key ([" + column.Name.ToLower() + "])");
                    
                }
            }

            if (IDXConfigs != null && IDXConfigs.Length > 0)
                {
                    foreach (var c in IDXConfigs)
                    {
                        createIndex(db, table.Name.ToLower(), c);
                    }
                }
            
        }
        /// <summary>
        /// 根据列创建索引
        /// </summary>
        /// <param name="database"></param>
        /// <param name="table"></param>
        /// <param name="indexInfo"></param>
        void createIndex(EntityDB.IDatabaseService database, string table, IndexInfo indexInfo)
        {
            table = table.ToLower();
            var columns = indexInfo.ColumnNames.OrderBy(m => m).Select(m=>m.ToLower()).ToArray();
            string columnsStr = "";
            string name = table + "_";
            for (int i = 0; i < columns.Length; i++)
            {
                name += columns[i] + "_";
                columnsStr += "[" + columns[i] + "]";
                if (i < columns.Length - 1)
                    columnsStr += ",";
            }

            try
            {
                string type = "";
                if (indexInfo.IsUnique)
                    type = "unique ";

                if (indexInfo.IsClustered)
                {
                    type += "CLUSTERED ";
                }
                else
                {
                    type += "NONCLUSTERED ";
                }
                database.ExecSqlString(string.Format("CREATE {3} index IDX_{2} on [{0}] ({1})", table, columnsStr, name,type));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// 删除table的所有包含字段的索引
        /// </summary>
        /// <param name="database"></param>
        /// <param name="table"></param>
        /// <param name="columnNames">包含的字段</param>
        void dropTableAllUniqueIndexWithColumns(EntityDB.IDatabaseService database, string table, List<string> columnNames)
        {
            table = table.ToLower();
            List<string> toDelIndexes = new List<string>();
            using (var sp_helpResult = database.SelectDataSet("sp_help [" + table + "]"))
            {
                foreach (var dtable in sp_helpResult.Tables)
                {
                    if (dtable.Columns.Any(m=>m.ColumnName == "index_keys"))
                    {
                        foreach (var drow in dtable.Rows)
                        {
                            string existColumnString = drow["index_keys"].ToString();
                             string indexName = drow["index_name"].ToString();
                             if (indexName.StartsWith("IDX_"))
                             {
                                 foreach (string column in columnNames)
                                 {
                                     if (Regex.IsMatch(existColumnString, @"\b(" + column + @")\b" , RegexOptions.IgnoreCase))
                                     {
                                         toDelIndexes.Add(indexName);
                                         break;
                                     }
                                 }
                             }
                        }
                        break;
                    }
                }
            }

            foreach (string indexName in toDelIndexes)
            {
                database.ExecSqlString("drop   index "+ indexName +" on ["+table+"]");
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="database"></param>
        /// <param name="table"></param>
        void dropTableIndex(EntityDB.IDatabaseService database, string table,IndexInfo[] dontDelIndexes)
        {
            table = table.ToLower();
            List<string> toDelIndexes = new List<string>();
            using (var sp_helpResult = database.SelectDataSet("sp_help [" + table + "]"))
            {
                foreach (var dtable in sp_helpResult.Tables)
                {
                    if (dtable.Columns.Any(m=>m.ColumnName == "index_keys"))
                    {
                        foreach (WayDataRow drow in dtable.Rows)
                        {
                            string existColumnString = drow["index_keys"].ToString();
                            string indexName = drow["index_name"].ToString();
                            string index_description = drow["index_description"].ToString();

                            if (index_description.Contains("primary key") == false  )
                            {
                                if (dontDelIndexes.Count(m => string.Equals( m.Name , indexName , StringComparison.CurrentCultureIgnoreCase)) == 0)
                                {
                                    toDelIndexes.Add(indexName);
                                }
                            }
                        }
                        break;
                    }
                }
            }

            foreach (string indexName in toDelIndexes)
            {
                database.ExecSqlString("drop   index " + indexName + " on [" + table + "]");
            }
        }
        void deletecolumn(EntityDB.IDatabaseService database, string table, string column)
        {
            table = table.ToLower();
            column = column.ToLower();

            #region delete
            using (var sp_helpResult = database.SelectDataSet("sp_help [" + table + "]"))
            {
                foreach (var dtable in sp_helpResult.Tables)
                {
                    if (dtable.Columns.Any(m=>m.ColumnName == "constraint_name"))
                    {
                        var query = dtable.Rows.Where(m=>m["constraint_keys"].ToSafeString().ToLower() == column.ToLower());
                        if (query.Count() > 0)
                        {
                            database.ExecSqlString("alter  table  [" + table + "]  drop  constraint " + query.First()["constraint_name"]);
                        }
                        break;
                    }
                }

                //删除默认值
                foreach (var dtable in sp_helpResult.Tables)
                {
                    if (dtable.Columns.Any(m=>m.ColumnName=="constraint_name"))
                    {
                        var query = from m in dtable.Rows
                                    where ((string)m["constraint_type"]).ToLower().EndsWith(" " + column.ToLower()) && ((string)m["constraint_type"]).ToLower().StartsWith("default on ")
                                    select m;

                       if (query.Count() > 0)
                        {
                            database.ExecSqlString("alter  table  [" + table + "]  drop  constraint " + query.First()["constraint_name"]);
                        }
                        break;
                    }
                }
            }

            #endregion

            database.ExecSqlString("ALTER TABLE [" + table + "] DROP COLUMN [" + column + "]");
        }
        /// <summary>
        /// 返回没有变化的索引
        /// </summary>
        /// <param name="database"></param>
        /// <param name="tablename"></param>
        /// <param name="idxConfigs"></param>
        /// <returns></returns>
        List<IndexInfo> checkIfIdxChanged(EntityDB.IDatabaseService database, string tablename, IndexInfo[] idxConfigs)
        {
            tablename = tablename.ToLower();
            List<IndexInfo> result = new List<IndexInfo>();
            List<IndexInfo> existKeys = new List<IndexInfo>();
            using (var sp_helpResult = database.SelectDataSet("sp_help [" + tablename + "]"))
            {
                foreach (var dtable in sp_helpResult.Tables)
                {
                    if (dtable.Columns.Any(m=>m.ColumnName=="index_keys"))
                    {
                        foreach (WayDataRow drow in dtable.Rows)
                        {
                            string existColumnString = drow["index_keys"].ToString();
                            string indexName = drow["index_name"].ToString();
                            string index_description = drow["index_description"].ToString();
                            if (index_description.Contains("primary key") == false)
                            {
                                //去除空格
                                string flag = existColumnString.Split(',').ToSplitString();
                                string dbname = flag.Split(',').OrderBy(m => m).ToArray().ToSplitString().ToLower();
                                //再排序，不要在去除空格之前排序
                                existKeys.Add(new IndexInfo
                                {
                                    Name = indexName,
                                    IsUnique = index_description.Contains("unique"),
                                    IsClustered = index_description.Contains("clustered") && !index_description.Contains("nonclustered"),
                                    ColumnNames = new string[] { dbname },
                                });
                            }
                            else
                            {
                                if (idxConfigs.Count(m => m.IsClustered) > 0)
                                {
                                    //去除主键
                                    database.ExecSqlString("alter  table  [" + tablename + "]  drop  constraint " + indexName);
                                    //设为主键
                                    string flag = existColumnString.Split(',').ToSplitString();
                                    string ppname = flag.Split(',').OrderBy(m => m).ToArray().ToSplitString("," , "[{0}]").ToLower();
                                    database.ExecSqlString("alter table [" + tablename + "] add constraint " + indexName + " primary key NONCLUSTERED (" + ppname + ")");
                                }
                            }
                            
                        }
                        break;
                    }
                }
            }
           

            foreach (var nowConfigItem in idxConfigs)
            {
                string myname = nowConfigItem.ColumnNames.OrderBy(m => m).ToArray().ToSplitString().ToLower();
                var fined = existKeys.FirstOrDefault(m => m.IsUnique == nowConfigItem.IsUnique && m.IsClustered == nowConfigItem.IsClustered && m.ColumnNames[0].ToLower() == myname);
                if (fined != null)
                {
                    nowConfigItem.Name = fined.Name;
                    result.Add(nowConfigItem);
                }
                
            }
            return result;
        }

        public void ChangeTable(EntityDB.IDatabaseService database, string oldTableName, string newTableName, 
            EJ.DBColumn[] addColumns, EJ.DBColumn[] changedColumns, EJ.DBColumn[] deletedColumns,EJ.DBColumn[] otherColumns
            , IndexInfo[] indexInfos)
        {
            oldTableName = oldTableName.ToLower();
            newTableName = newTableName.ToLower();

            //先判断表明是否更改
            if (oldTableName != newTableName)
            {
                //更改表名
                database.ExecSqlString("EXEC sp_rename '" + oldTableName + "', '" + newTableName + "'");
            }


            //获取那个索引存在了
            var existIndexed = checkIfIdxChanged( database , newTableName , indexInfos);
            
            dropTableIndex(database, newTableName, existIndexed.ToArray());

            foreach (var column in deletedColumns)
            {
                deletecolumn(database, newTableName.ToLower(), column.Name.ToLower());
            }

            foreach (var column in changedColumns)
            {
                int changeColumnCount = 0;
                var changeitem = column.BackupChangedProperties["Name"];
                if (changeitem != null)
                {
                    changeColumnCount++;
                    #region 改名
                    database.ExecSqlString($"EXEC sp_rename '[{newTableName.ToLower()}].[{changeitem.OriginalValue}]', '{column.Name.ToLower()}', 'COLUMN'"); 
                    #endregion
                }


                changeitem = column.BackupChangedProperties["IsAutoIncrement"];
                if (changeitem != null)
                {
                    changeColumnCount++;

                    var flagColumnName = "_tempcolumn";
                    while ( Convert.ToInt32( database.ExecSqlString($"Select count(*) from syscolumns Where  ID=OBJECT_ID('{newTableName}') and name='{flagColumnName}'")) > 0)
                    {
                        flagColumnName += "1";
                    }
                    #region 变更自增长
                    if (column.IsAutoIncrement == false)
                    {
                        //去掉自增长
                        database.ExecSqlString($"alter table [{newTableName.ToLower()}] add {flagColumnName.ToLower()} {column.dbType}");
                        database.ExecSqlString($"update [{newTableName.ToLower()}] set {flagColumnName.ToLower()}=[{column.Name.ToLower()}]");
                        deletecolumn(database, newTableName.ToLower(), column.Name.ToLower());
                        database.ExecSqlString($"EXEC sp_rename '[{newTableName.ToLower()}].{flagColumnName.ToLower()}', '{column.Name.ToLower()}', 'COLUMN'");
                       
                    }
                    else
                    {
                        //设为自增长
                        database.ExecSqlString($"alter table [{newTableName.ToLower()}] add {flagColumnName.ToLower()} {column.dbType} IDENTITY (1, 1)");
                        deletecolumn(database, newTableName.ToLower(), column.Name.ToLower());
                        database.ExecSqlString($"EXEC sp_rename '[{newTableName.ToLower()}].{flagColumnName.ToLower()}', '{column.Name.ToLower()}', 'COLUMN'");
                    }

                    //去掉自增长后，由于原来的列删除了，所以如果原来是主键，必须重新设置为主键
                    if (column.IsPKID == true)
                    {
                        //主键不允许为空
                        database.ExecSqlString($"alter table [{newTableName.ToLower()}] alter column [{column.Name.ToLower()}] [{column.dbType}] not null");
                        changeitem = column.BackupChangedProperties["IsPKID"];
                        if (changeitem == null)
                        {
                            //标识IsPKID发生变化
                            column.BackupChangedProperties["IsPKID"] = new EntityDB.DataValueChangedItem()
                            {
                                OriginalValue = false,
                            };
                        }
                    }
                    #endregion
                }

                 changeitem = column.BackupChangedProperties["IsPKID"];
                 if (changeitem != null)
                 {
                     changeColumnCount++;

                     #region 变更主键
                     if (column.IsPKID == false)
                     {
                        //去除主键
                        string sql = @"
DECLARE @NAME SYSNAME
DECLARE @TB_NAME SYSNAME
SET @TB_NAME = '" + newTableName + @"'
SELECT TOP 1 @NAME = NAME FROM SYS.OBJECTS WITH(NOLOCK)
WHERE TYPE_DESC ='PRIMARY_KEY_CONSTRAINT'

AND PARENT_OBJECT_ID = (SELECT OBJECT_ID

FROM SYS.OBJECTS WITH(NOLOCK)

WHERE NAME = @TB_NAME )
SELECT @NAME
";
                        var constraintName = database.ExecSqlString(sql).ToSafeString();
                        if(constraintName.IsNullOrEmpty() == false)
                        {
                            //如果constraintName没有值，那么就是变更自增长字段的时候，原来字段被删除了
                            database.ExecSqlString($"ALTER TABLE {newTableName.ToLower()} DROP CONSTRAINT {constraintName}");
                        }
                     }
                     else
                     {
                         //设为主键
                         database.ExecSqlString("alter table [" + newTableName.ToLower() + "] add constraint PK_" + newTableName.ToLower() + "_" + column.Name.ToLower() + " primary key ([" + column.Name.ToLower() + "])");
                     } 
                     #endregion
                 }


                 bool defaultvalueChanged = false;
                 changeitem = column.BackupChangedProperties["defaultValue"];
                 if (changeitem != null)
                 {
                     defaultvalueChanged = true;
                     changeColumnCount++;

                    #region 默认值
                    //获取默认值的id
                    var defaultSettingID = database.ExecSqlString($"Select cdefault from syscolumns Where  ID=OBJECT_ID('{newTableName.ToLower()}') and name='{column.Name.ToLower()}'");
                    if (defaultSettingID != null && Convert.ToInt32(defaultSettingID) != 0)
                    {
                        var defaultKeyName = database.ExecSqlString($"select name from sysObjects where type='D' and id={defaultSettingID}");
                        if (defaultKeyName != null)
                        {
                            //如果进到这里，那么表示原来有默认值
                            database.ExecSqlString($"alter  table  [{newTableName}]  drop  constraint {defaultKeyName}");
                        }
                    }
                   
                     #endregion
                 }

                 if (column.BackupChangedProperties.Count > changeColumnCount)
                 {
                     #region 如果其他地方还有更改
                     string sql = "alter table [" + newTableName.ToLower() + "] alter column [" + column.Name.ToLower() + "] [" + column.dbType + "]";

                     if (column.dbType.IndexOf("char") >= 0)
                     {
                         if (!string.IsNullOrEmpty(column.length))
                             sql += " (" + column.length + ")";
                         else
                         {
                             sql += " (50)";
                         }
                     }
                     else
                     {
                         if (!string.IsNullOrEmpty(column.length))
                         {
                             sql += " (" + column.length + ")";
                         }
                     }

                    //先改变字段类型，下面再设置默认值，和非null
                    if (column.IsPKID == true || column.IsAutoIncrement == true)
                    {
                        database.ExecSqlString(sql);
                    }
                    else
                    {
                        database.ExecSqlString(sql + " NULL");
                    }
                    if (column.CanNull == false && !string.IsNullOrEmpty(column.defaultValue))
                     {
                         string defaultValue = column.defaultValue.Trim();
                        
                         database.ExecSqlString("update [" + newTableName.ToLower() + "] set [" + column.Name.ToLower() + "]='" + defaultValue.Replace("'","''") + "' where [" + column.Name.ToLower() + "] is null");
                     }


                    if (column.CanNull == false || column.IsPKID == true || column.IsAutoIncrement == true)
                    {
                        database.ExecSqlString(sql + " NOT NULL");
                    }
                     #endregion
                 }

                 #region 设置默认值
                 if ( defaultvalueChanged && !string.IsNullOrEmpty(column.defaultValue))
                 {
                     string sql = "";
                     string defaultValue = column.defaultValue.Trim();
                     sql += "alter  table  [" + newTableName + "]  add  constraint DF_" + newTableName.ToLower() + "_" + column.Name.ToLower() + " default '" + defaultValue.Replace("'","''") + "' for [" + column.Name.ToLower() + "]";
                         
                     
                     if (sql.Length > 0)
                         database.ExecSqlString(sql);
                     

                     database.ExecSqlString("update ["+newTableName.ToLower() + "] set ["+column.Name.ToLower() + "]='" + defaultValue.Replace("'","''") + "' where ["+column.Name.ToLower() + "] is null");
                 } 
                 #endregion
            }

            foreach (var column in addColumns)
            {
                #region 新增字段
                string sql = "alter table [" + newTableName.ToLower() + "] add [" + column.Name.ToLower() + "] [" + column.dbType + "]";

                if (column.dbType.IndexOf("char") >= 0)
                {
                    if (!string.IsNullOrEmpty(column.length))
                        sql += " (" + column.length + ")";
                    else
                    {
                        sql += " (50)";
                    }
                }
                else
                {
                    if (!string.IsNullOrEmpty(column.length))
                    {
                        sql += " (" + column.length + ")";
                    }
                }

                if (column.IsAutoIncrement == true)
                    sql += " IDENTITY (1, 1)";

                if (column.CanNull == false || column.IsPKID == true || column.IsAutoIncrement == true)
                {
                    sql += " NOT";
                }
                sql += " NULL  ";
                if (!string.IsNullOrEmpty(column.defaultValue))
                {
                    string defaultValue = column.defaultValue.Trim();
                    sql += " default '" + defaultValue.Replace("'","''") + "' with values";
                    
                }
                database.ExecSqlString(sql);

                if (column.IsPKID == true)
                    database.ExecSqlString("nalter table [" + newTableName.ToLower() + "] add constraint pk_" + newTableName.ToLower() + "_" + column.Name.ToLower() + " primary key ([" + column.Name.ToLower() + "])");
                
                #endregion
            }

            if (  indexInfos != null && indexInfos.Length > 0)
            {
                foreach (var c in indexInfos)
                {
                    if (existIndexed.Contains(c))
                        continue;

                    createIndex(database, newTableName, c);
                }
            }
        }


        public void DeleteTable(EntityDB.IDatabaseService database, string tableName)
        {
            tableName = tableName.ToLower();
            string sql = "if exists(select id from sysobjects where name='" + tableName + "' and xtype='U') drop table [" + tableName + "]";
            database.ExecSqlString(sql);
        }
    }
}