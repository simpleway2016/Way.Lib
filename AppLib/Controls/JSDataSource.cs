using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Caching;
using System.Web.Script.Serialization;
using System.Web.UI;
using System.Web.UI.Design;
using System.Web.UI.WebControls;

namespace AppLib.Controls
{
    [Serializable]
    [AppLib.Controls.Editor.TitleField("Text")]
    public class DataSourceItem : ICloneable
    {
        /// <summary>
        /// 如果DisplayText没有值，则显示Text属性
        /// </summary>
        [System.ComponentModel.Description("如果DisplayText没有值，则显示Text属性")]
        public string DisplayText
        {
            get;
            set;
        }
        public string Description
        {
            get;
            set;
        }
        public string Text
        {
            get;
            set;
        }
        public string Value
        {
            get;
            set;
        }

        List<DataSourceItem> _Items = new List<DataSourceItem>();
        [System.ComponentModel.Category("系统设定"), System.ComponentModel.Description("验证项设定"),
System.Web.UI.PersistenceMode(System.Web.UI.PersistenceMode.InnerProperty), System.ComponentModel.MergableProperty(false),
System.ComponentModel.Editor(typeof(Editor.CollectionBaseEditor), typeof(System.Drawing.Design.UITypeEditor))]
        public List<DataSourceItem> Items
        {
            get
            {
                return _Items;

            }
        }

        public object Clone()
        {
            DataSourceItem newitem = new DataSourceItem();
            newitem.Text = this.Text;
            newitem.DisplayText = this.DisplayText;
            newitem.Value = this.Value;
            newitem.Description = this.Description;
            foreach (DataSourceItem c in this.Items)
            {
                newitem.Items.Add(c.Clone() as DataSourceItem);
            }
            return newitem;
        }
    }

    [Designer(typeof(MyControlDesigner))]
    public class JSDataSource : System.Web.UI.WebControls.WebControl, IKeyObject, IAutoDataBindControl
    {

        #region 属性
        [System.ComponentModel.Category("系统设定"), System.ComponentModel.Description("验证项设定"),
       System.Web.UI.PersistenceMode(System.Web.UI.PersistenceMode.InnerProperty), System.ComponentModel.MergableProperty(false),
       System.ComponentModel.Editor(typeof(Editor.CollectionBaseEditor), typeof(System.Drawing.Design.UITypeEditor))]
        public List<DataSourceItem> DataSourceItems
        {
            get
            {
                if (ViewState["DataSourceItems"] == null)
                    ViewState["DataSourceItems"] = new List<DataSourceItem>();
                return ViewState["DataSourceItems"] as List<DataSourceItem>;

            }
        }

        [System.ComponentModel.Category("系统设定"), System.ComponentModel.Editor(typeof(Editor.DatabaseSelector), typeof(System.Drawing.Design.UITypeEditor))]
        public string DatabaseConfig
        {
            get
            {
  
                return Convert.ToString(ViewState["DatabaseConfig"]);
            }
            set
            {
                ViewState["DatabaseConfig"] = value;
            }
        }

      

        [System.ComponentModel.Category("系统设定"), System.ComponentModel.Description("条件语句"),
        System.ComponentModel.Browsable(true)]
        public string KeyTermString
        {
            get
            {
                return Convert.ToString( ViewState["KeyTermString"]);
            }
            set
            {
                ViewState["KeyTermString"] = value;
            }
        }

        [System.ComponentModel.Category("系统设定"), System.ComponentModel.Description("排序语句"),
        System.ComponentModel.Browsable(true)]
        public string KeyOrderString
        {
            get
            {
                return Convert.ToString(ViewState["KeyOrderString"]);
            }
            set
            {
                ViewState["KeyOrderString"] = value;
            }
        }

        [System.ComponentModel.Category("系统设定"), System.ComponentModel.Description("关联的表名"),
        System.ComponentModel.Browsable(true),
        System.ComponentModel.Editor(typeof(Editor.TableNameSelector), typeof(System.Drawing.Design.UITypeEditor))]
        public string KeyTableName
        {
            get
            {
                return Convert.ToString(ViewState["KeyTableName"]);
            }
            set
            {
                ViewState["KeyTableName"] = value;
            }
        }

        [System.ComponentModel.Category("系统设定"), System.ComponentModel.Description("Description字段名"),
       System.ComponentModel.Browsable(true),
       System.ComponentModel.Editor(typeof(Editor.TableFieldSelector), typeof(System.Drawing.Design.UITypeEditor))]
        public string KeyDescriptionField
        {
            get
            {
                return Convert.ToString(ViewState["KeyDescriptionField"]);
            }
            set
            {
                ViewState["KeyDescriptionField"] = value;
            }
        }

        [System.ComponentModel.Category("系统设定"), System.ComponentModel.Description("关联的表ID字段"),
        System.ComponentModel.Editor(typeof(Editor.TableFieldSelector), typeof(System.Drawing.Design.UITypeEditor))]
        public string KeyIDField
        {
            get
            {
                return Convert.ToString(ViewState["KeyIDField"]);
            }
            set
            {
                ViewState["KeyIDField"] = value;
            }
        }

        [System.ComponentModel.Category("系统设定"), System.ComponentModel.Description("关联的表显示字段"),
        System.ComponentModel.Editor(typeof(Editor.TableFieldSelector), typeof(System.Drawing.Design.UITypeEditor))]
        public string KeyTextField
        {
            get
            {
                return Convert.ToString(ViewState["KeyTextField"]);
            }
            set
            {
                ViewState["KeyTextField"] = value;
            }
        }

    
        #endregion

        public JSDataSource()
        {
            
        }

        public string GetValueByText(string text)
        {
            foreach (DataSourceItem item in this.DataSourceItems)
            {
                if (item.Text == text)
                    return item.Value;
            }
            if (KeyTextField != KeyIDField)
            {
                using (Way.EntityDB.DBContext db = AppHelper.CreateLinqDataBase(this.DatabaseConfig))
                {
                    Type dbType = db.GetType();
                    var query = dbType.GetProperty(KeyTableName).GetValue(db);
                    query = Way.EntityDB.DBContext.InvokeWhereEquals(query, KeyTextField, text);
                    query = Way.EntityDB.DBContext.InvokeSelect(query, KeyIDField);

                    return Convert.ToString(Way.EntityDB.DBContext.InvokeFirstOrDefault(query));
                }
            }
            else
            {
                return text;
            }
        }
        public string GetTextByValue(string value)
        {
            foreach (DataSourceItem item in this.DataSourceItems)
            {
                if (item.Value == value)
                    return item.Text;
            }
            if (KeyTextField != KeyIDField)
            {
                using (Way.EntityDB.DBContext db = AppHelper.CreateLinqDataBase(this.DatabaseConfig))
                {
                    Type dbType = db.GetType();
                    var query = dbType.GetProperty(KeyTableName).GetValue(db);
                    query = Way.EntityDB.DBContext.InvokeWhereEquals(query, KeyIDField, value);
                    query = Way.EntityDB.DBContext.InvokeSelect(query, KeyTextField);

                    return Convert.ToString(Way.EntityDB.DBContext.InvokeFirstOrDefault(query));

                }
            }
            else
            {
                return value;
            }
        }

        protected override void Render(HtmlTextWriter writer)
        {
           
         

            List<DataSourceItem> arr = new List<DataSourceItem>(this.DataSourceItems.ToArray());
            try
            {
                if (!string.IsNullOrEmpty(DatabaseConfig) && !string.IsNullOrEmpty(KeyTableName))
                {
                    using (Way.EntityDB.DBContext database = AppHelper.CreateLinqDataBase(this.DatabaseConfig))
                    {
                        Type dbType = database.GetType();
                        var queryPinfos = dbType.GetProperties().Where(m => m.Name == KeyTableName);
                        System.Reflection.PropertyInfo queryPinfo = null;
                        foreach (System.Reflection.PropertyInfo p in queryPinfos)
                        {
                            if (queryPinfo == null)
                                queryPinfo = p;

                            if (p.DeclaringType == dbType)
                            {
                                queryPinfo = p;
                                break;
                            }
                        }

                        if (queryPinfo == null)
                            throw (new Exception("无法在DBContext找到" + KeyTableName + "对应的IQueryable<>属性"));

                        object query = queryPinfo.GetValue(database);
                        Type dataType = query.GetType().GetGenericArguments()[0];
                        if (!string.IsNullOrEmpty(KeyTermString))
                        {
                            try
                            {
                                query = Way.EntityDB.DBContext.GetQueryByString(query, KeyTermString);
                            }
                            catch (Exception ex)
                            {
                            }
                        }
                        if (!string.IsNullOrEmpty(KeyOrderString))
                        {
                            try
                            {
                                query = Way.EntityDB.DBContext.GetQueryForOrderBy(query, KeyOrderString);
                            }
                            catch (Exception ex)
                            {
                            }
                        }

                        System.Collections.IEnumerable datasource = query as System.Collections.IEnumerable;
                        foreach (Way.EntityDB.DataItem dataitem in datasource)
                        {
                            DataSourceItem item = (new DataSourceItem()
                            {
                                Text = Convert.ToString(dataitem.GetValue(KeyTextField)),
                                Value = Convert.ToString(dataitem.GetValue(KeyIDField)),
                            });
                            if (!string.IsNullOrEmpty(KeyDescriptionField))
                                item.Description = Convert.ToString(dataitem.GetValue(KeyDescriptionField));
                            arr.Add(item);
                        }
                    }
                }
            }
            catch(Exception ex)
            {
                HttpContext.Current.Response.Write("<Script lang='ja'>alert(" + new System.Web.Script.Serialization.JavaScriptSerializer().Serialize(ex.ToString()) + ");</script>");
            }
            string json = new System.Web.Script.Serialization.JavaScriptSerializer().Serialize(arr);
            arr.Clear();

            writer.Write("<script lang='ja'>var " + this.ClientID + "=");
            writer.Write(json);
            writer.WriteLine(";</script>");
        }


        public IAutoDataBindControl AutoDataBindControl
        {
            get { return this; }
        }
    }

}
