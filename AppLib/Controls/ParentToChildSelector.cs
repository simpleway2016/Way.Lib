using EntityDB;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Caching;
using System.Web.UI;
using System.Web.UI.Design;
using System.Web.UI.WebControls;

namespace AppLib.Controls
{
    public class ParentToChildSelectorDesigner : ControlDesigner
    {
        public override string GetDesignTimeHtml()
        {
            ParentToChildSelector ctrl = ((ParentToChildSelector)this.Component);
            return "<input type='text' class='" + ctrl.CssClass + "' style=\"background-image:url(/inc/imgs/___dropdown.gif);background-position:right;background-repeat:no-repeat;\">";
        }
    }

    [Designer(typeof(ParentToChildSelectorDesigner))]
    public class ParentToChildSelector : BaseControl , IAutoDataBindControl , IPostBackDataHandler
    {
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

        [System.ComponentModel.Category("系统设定")]
        public bool 层次不确定
        {
            get
            {
                if (ViewState["层次不确定"] == null)
                    ViewState["层次不确定"] = false;
                return Convert.ToBoolean(ViewState["层次不确定"]);
            }
            set
            {
                ViewState["层次不确定"] = value;
            }
        }
        List<PCItem> _Items = null;
        [System.ComponentModel.Category("系统设定"), System.ComponentModel.Description("数据项设定"),
       System.Web.UI.PersistenceMode(System.Web.UI.PersistenceMode.InnerProperty), System.ComponentModel.MergableProperty(false),
       System.ComponentModel.Editor(typeof(Editor.CollectionBaseEditor), typeof(System.Drawing.Design.UITypeEditor))]
        public List<PCItem> Items
        {
            get
            {
                if (_Items == null)
                {
                    _Items = new List<PCItem>();
                }

                return _Items;
            }
        }

        [System.ComponentModel.Editor(typeof(Editor.JSDataSourceSelector), typeof(System.Drawing.Design.UITypeEditor)),
    System.ComponentModel.Category("系统设定")
    ]
        public string JSDataSource
        {
            get;
            set;
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden),Browsable(false)]
        public string SelectedValue
        {
            get
            {
                return Convert.ToString(ViewState["SelectedValue"]);
            }
            set
            {
                ViewState["SelectedValue"] = value;
            }
        }

        protected override void LoadViewState(object savedState)
        {
            base.LoadViewState(savedState);
            _Items = ViewState["Items"] as List<PCItem>;
        }

        protected override object SaveViewState()
        {
            ViewState["Items"] = _Items;
            return base.SaveViewState();
        }
        public void SetSelectedText(string[] texts)
        {
            StringBuilder selectedvalue = new StringBuilder();
            using (EntityDB.DBContext db = AppHelper.CreateLinqDataBase(this.DatabaseConfig))
            {
                Type dbtype = db.GetType();
                for (int i = 0; i < texts.Length; i++)
                {
                    string text = texts[i];
                    if (text.Length == 0)
                        break;
                    PCItem item = null;
                    if (i < this.Items.Count)
                        item = this.Items[i];
                    else
                        item = this.Items[this.Items.Count - 1];

                    var query = dbtype.GetProperty(item.KeyTableName).GetValue(db);

                    if (!string.IsNullOrEmpty(item.TermString))
                    {
                        query = DBContext.GetQueryByString(query, item.TermString);
                    }

                    query = DBContext.InvokeWhereEquals(query, item.KeyTextField, text);
                    query = DBContext.InvokeSelect(query, item.KeyIDField);
                    string id = Convert.ToString(DBContext.InvokeFirstOrDefault(query));
                    if (selectedvalue.Length > 0)
                        selectedvalue.Append(',');
                    selectedvalue.Append(id);
                }
            }

            this.SelectedValue = selectedvalue.ToString();
        }
        public string[] GetSelectedText()
        {
            List<string> texts = new List<string>();
           
            using (EntityDB.DBContext db = AppHelper.CreateLinqDataBase(this.DatabaseConfig))
            {
                Type dbtype = db.GetType();
                string[] ids = this.SelectedValue.Split(',');
                for (int i = 0; i < ids.Length; i++)
                {
                    string id = ids[i];
                    if (id.Length == 0)
                        break;
                    PCItem item = null;
                    if (i < this.Items.Count)
                        item = this.Items[i];
                    else
                        item = this.Items[this.Items.Count - 1];

                    var query = dbtype.GetProperty(item.KeyTableName).GetValue(db);

                    if (!string.IsNullOrEmpty(item.TermString))
                    {
                        query = DBContext.GetQueryByString(query, item.TermString);
                    }
                   
                    query = DBContext.InvokeWhereEquals(query, item.KeyIDField, id);
                    query = DBContext.InvokeSelect(query, item.KeyTextField);
                    string text = Convert.ToString( DBContext.InvokeFirstOrDefault(query) );
                    texts.Add(text);
                }
            }

            return texts.ToArray();
        }

        /// <summary>
        /// 获取最终select项的值
        /// </summary>
        /// <returns></returns>
        public string GetLastSelectionValue()
        {
            if (SelectedValue == null)
                return null;
            string[] values = SelectedValue.Split(',');

            try
            {
                if (this.层次不确定 || !string.IsNullOrEmpty(JSDataSource))
                    return values[values.Length - 1];
                return values[Items.Count - 1];
            }
            catch
            {
                return null;
            }
        }

        public void SetLastSelectionValue(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                SelectedValue = null;
                return;
            }
            StringBuilder arrValue = new StringBuilder();
            arrValue.Append(value);

            using (EntityDB.DBContext db = AppHelper.CreateLinqDataBase(this.DatabaseConfig))
            {    
                Type objType = db.GetType();

                if (string.IsNullOrEmpty(JSDataSource))
                {
                    if (this.层次不确定)
                    {
                        int index = Items.Count - 1;
                        PCItem item = Items[index];
                        while (true)
                        {
                            var query = objType.GetProperty(item.KeyTableName).GetValue(db);
                            if (!string.IsNullOrEmpty(item.TermString))
                            {
                                query = DBContext.GetQueryByString(query, item.TermString);
                            }
                            query = DBContext.InvokeWhereEquals(query, item.KeyIDField, value);
                            string lastvalue = Convert.ToString(DBContext.InvokeFirstOrDefault(DBContext.InvokeSelect(query, item.KeyParentIDField)));
                            if (string.IsNullOrEmpty(lastvalue))
                            {
                                break;
                            }
                        __checkagain:
                            query = objType.GetProperty(item.KeyTableName).GetValue(db);
                            if (!string.IsNullOrEmpty(item.TermString))
                            {
                                query = DBContext.GetQueryByString(query, item.TermString);
                            }
                            query = DBContext.InvokeWhereEquals(query, item.KeyIDField, lastvalue);
                            object myid = DBContext.InvokeFirstOrDefault(DBContext.InvokeSelect(query, item.KeyIDField));
                            if (myid == null)
                            {
                                index--;
                                if (index < 0)
                                    break;
                                item = Items[index];
                                goto __checkagain;
                            }
                            else
                            {
                                arrValue.Insert(0, lastvalue + ",");
                                value = lastvalue;
                            }
                        }

                    }
                    else
                    {
                        for (int i = Items.Count - 1; i > 0; i--)
                        {
                            PCItem item = Items[i];
                            var query = objType.GetProperty(item.KeyTableName).GetValue(db);

                            if (!string.IsNullOrEmpty(item.TermString))
                            {
                                query = EntityDB.DBContext.GetQueryByString(query, item.TermString);
                            }
                            query = DBContext.InvokeWhereEquals(query, item.KeyIDField, value);

                            string lastvalue = Convert.ToString(DBContext.InvokeFirstOrDefault(DBContext.InvokeSelect(query, item.KeyParentIDField)));
                            if (string.IsNullOrEmpty(lastvalue))
                            {
                                arrValue.Clear();
                                break;
                            }

                            arrValue.Insert(0, lastvalue + ",");
                            value = lastvalue;
                        }
                    }
                }
                else
                {
                    List<System.Web.UI.Control> controls = AppHelper.GetControlsByTypes(this.Page, new Type[] { typeof(JSDataSource) });
                    JSDataSource jsdataSource = (JSDataSource)controls.FirstOrDefault(m => m.ID == this.JSDataSource);
                    throw new Exception("JSDataSource暂时不支持");
                }
            }

            SelectedValue = arrValue.ToString();
        }

      

        protected override void OnInit(EventArgs e)
        {
            if (this.DesignMode)
            {
                base.OnInit(e);
                return;
            }

            if (Page.Request.QueryString["rendercontrol"] == "ParentToChildSelector")
            {
                string stateFileName = this.Page.Request.QueryString["stateFileName"];
                JsonObject jsonObject = new FileStatePersister(this.Page).ReadState<JsonObject>(stateFileName);

                LinqOutputData();
            }
            else
            {
                base.OnInit(e);
            }
        }

        #region 输出数据
        class JsonObject
        {
            public string DatabaseConfig;
            public bool 层次不确定;
            public PCItem[] Items;
            public string ClientID;
        }
        protected void LinqOutputData()
        {
            string stateFileName = this.Page.Request.QueryString["stateFileName"];
            JsonObject jsonObject = new FileStatePersister(this.Page).ReadState<JsonObject>(stateFileName);

            HtmlTextWriter writer = new HtmlTextWriter(Page.Response.Output);
            writer.Write(@"
<!DOCTYPE html>

<html xmlns=""http://www.w3.org/1999/xhtml"">
<head runat=""server"">
<meta http-equiv=""Content-Type"" content=""text/html; charset=utf-8""/>
    <title></title>
<style>
.div1
{
height:20px;
width:100%;
overflow:hidden;
padding:3px;
font-size:14px;
border-bottom:1px solid #cccccc;
cursor:pointer;
}
.btn
{
font-size:12px;
padding-top:2px;
float:right;
padding-right:10px;
font-weight:bold;
cursor:pointer;
}
</style>
</head>
<body style=""margin:0px;overflow:hidden;background-color:white;"">
<div id='div1'>
");


            int layer = Convert.ToInt32(Page.Request.QueryString["layer"]);

            string[] selectedValues = Page.Request.QueryString["allid"].Split(',');


            using (EntityDB.DBContext db = AppHelper.CreateLinqDataBase(jsonObject.DatabaseConfig))
            {

                object lastid = null;
                if (layer > 0)
                    lastid = selectedValues[layer - 1];

                List<PCItem> myitems = new List<PCItem>(jsonObject.Items);
                while (jsonObject.层次不确定 && layer >= myitems.Count && myitems.Count > 0)
                {
                    PCItem newitem = (PCItem)myitems[myitems.Count - 1].Clone();
                    myitems.Add(newitem);
                }

                while (jsonObject.层次不确定 && selectedValues.Length > myitems.Count && myitems.Count > 0)
                {
                    PCItem newitem = (PCItem)myitems[myitems.Count - 1].Clone();
                    myitems.Add(newitem);
                }

                for (int i = layer; i < myitems.Count; i++)
                {
                    PCItem pcitem = myitems[i];
                    int rowcount = 0;

                    string action = null;
                    action = "_layer=\"" + (layer + 1) + "\" ";

                    string termstring = pcitem.TermString;
                    if (lastid != null)
                    {
                        if (string.IsNullOrEmpty(termstring))
                        {
                            termstring = string.Format("[{0}]={1}", pcitem.KeyParentIDField, lastid);
                        }
                        else
                        {
                            termstring = string.Format("({2}) and [{0}]={1}", pcitem.KeyParentIDField, lastid, termstring);
                        }
                    }

                    Type dbType = db.GetType();
                    var queryPinfos = dbType.GetProperties().Where(m => m.Name == pcitem.KeyTableName);
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
                        throw (new Exception("无法在DBContext找到" + pcitem.KeyTableName + "对应的IQueryable<>属性"));

                    object query = queryPinfo.GetValue(db);
                    Type dataType = query.GetType().GetGenericArguments()[0];
                    if (!string.IsNullOrEmpty(termstring))
                    {
                        
                            try
                            {
                                query = EntityDB.DBContext.GetQueryByString(query, termstring);
                            }
                            catch (Exception ex)
                            {
                                throw new Exception(ex.Message + "\r\n出错的表达式：" + termstring);
                            }
                        
                    }
                    if (!string.IsNullOrEmpty(pcitem.OrderString))
                    {
                        try
                        {
                            query = EntityDB.DBContext.GetQueryForOrderBy(query, pcitem.OrderString);

                        }
                        catch (Exception ex)
                        {
                            throw new Exception(ex.Message + "\r\n出错的表达式：" + pcitem.OrderString);
                        }
                    }
                    if (true)
                    {
                        bool hasData = false;
                        lastid = null;
                       System.Collections.IEnumerable datasource = (System.Collections.IEnumerable)query;
                       try
                       {
                           foreach (object dataitem in datasource)
                           {
                               if (!hasData)
                               {
                                   hasData = true;
                                   writer.Write("<select " + action + " size=\"12\" name=\"" + Page.Request.QueryString["cid"] + "\">");

                               }
                               string idvalue = Convert.ToString(DataItem.GetValue( dataitem ,  pcitem.KeyIDField));
                               if (idvalue == null)
                                   idvalue = "";
                               string text = Convert.ToString(DataItem.GetValue(dataitem, pcitem.KeyTextField));
                               if (text == null)
                                   text = "";
                               writer.AddAttribute("value", System.Web.HttpUtility.HtmlEncode(idvalue));
                               try
                               {
                                   if (idvalue == selectedValues[i])
                                   {
                                       lastid = idvalue;
                                       writer.AddAttribute("Selected", "true");
                                   }
                               }
                               catch
                               {
                               }
                               writer.RenderBeginTag("option");
                               writer.Write(System.Web.HttpUtility.HtmlEncode(text));
                               writer.RenderEndTag();
                           }
                       }
                       catch
                       {
                       }
                       if (hasData)
                       {
                           writer.Write("</select>");
                           if (lastid == null)
                           {
                               break;
                           }
                       }
                       else
                       {
                           break;
                       }

                    }

                }

            }
            writer.Write("</div><script lang=\"ja\">parent.js_" + Page.Request.QueryString["cid"] + ".loadData();</script>\r\n");
            writer.Write("</body></html>");

            Page.Response.End();
        }
      
        #endregion

        protected override void OnLoad(EventArgs e)
        {
            if (this.DesignMode)
            {
                base.OnLoad(e);
                return;
            }
            Page.RegisterRequiresPostBack(this);
            if (ViewState["stateFileName"] == null)
            {
                ViewState["stateFileName"] = "WS_" + CacheStatePersister.CreateKey(Page);
            }
            WriteJS();
            base.OnLoad(e);
        }

        static bool writedjs = false;
        /// <summary>
        /// 生成js文件
        /// </summary>
        private void WriteJS()
        {
            if (writedjs == false)
            {
                writedjs = true;
                
                string folder = this.Page.Server.MapPath("/inc/imgs");
                if (System.IO.Directory.Exists(folder) == false)
                {
                    System.IO.Directory.CreateDirectory(folder);
                }
                if (System.IO.File.Exists(folder + "\\loading.gif") == false)
                {
                    System.IO.File.WriteAllBytes(folder + "\\loading.gif", ResourceJS.loading_gif);
                }
                if (System.IO.File.Exists(folder + "\\___dropdown.gif") == false)
                {
                    System.IO.File.WriteAllBytes(folder + "\\___dropdown.gif", ResourceJS.dropdown_gif);
                }
            }
            if (!this.Page.ClientScript.IsClientScriptIncludeRegistered("__ParentToChildSelector_JSFile"))
            {
                Page.ClientScript.RegisterClientScriptInclude("__ParentToChildSelector_JSFile",
             Page.ClientScript.GetWebResourceUrl(this.GetType(),
                                         "AppLib.js.ParentToChildSelector.js"));
            }
            AppHelper.RegisterJquery(this.Page);
        }

        protected override void Render(System.Web.UI.HtmlTextWriter writer)
        {
            if (this.DesignMode)
                return;

            string styleText="";
            if (string.IsNullOrEmpty(CssClass))
            {
                styleText = " style=\"background-image:url(/inc/imgs/___dropdown.gif);background-position:right;background-repeat:no-repeat;width:" + this.Width + ";\" ";
            }
            if (string.IsNullOrEmpty(JSDataSource))
            {
                new FileStatePersister(this.Page).WriteState((string)ViewState["stateFileName"], new JsonObject()
                {
                    DatabaseConfig = this.DatabaseConfig,
                    Items = this.Items.ToArray(),
                    层次不确定 = this.层次不确定
                });

                ///_ParentToChildSelectorPage.aspx
                writer.Write(string.Format("<iframe name=\"{0}_iframe\" src='?rendercontrol=ParentToChildSelector&stateFileName=" + ViewState["stateFileName"] + "&cid={0}&allid={1}&layer=0' style=\"display:none;\"></iframe>",
                    ClientID, this.SelectedValue));
                writer.Write(string.Format("<input type=\"text\" readonly name=\"{0}_text\" id=\"{0}_text\" class=\"{1}\" "+styleText+">", ClientID, this.CssClass));
                writer.Write(string.Format("<div style=\"position:absolute;display:none;\" _src=\"?rendercontrol=ParentToChildSelector&stateFileName=" + ViewState["stateFileName"] + "&cid={0}\" id=\"{0}_div\"></div>", ClientID));
                writer.Write("<script lang=\"ja\">var js_" + ClientID + " = new JS_ParentToChildSelector('" + ClientID + "');</script>");
            }
            else
            {
                Control jsdatasource = AppHelper.GetControlsByTypes(this.Page, new Type[] { typeof(JSDataSource) }).FirstOrDefault(m => m.ID == this.JSDataSource);
                if (jsdatasource == null)
                {
                    writer.WriteLine("在当前页面无法找到" + JSDataSource);
                }
                else
                {
                    writer.Write(string.Format("<input type=\"text\" readonly name=\"{0}_text\" class=\"{1}\" " + styleText + ">", ClientID, this.CssClass));
                    writer.Write(string.Format("<div style=\"position:absolute;display:none;\" id=\"{0}_div\"></div>", ClientID));

                    string json = "false";
                    if (!string.IsNullOrEmpty(this.SelectedValue))
                    {
                        json = new System.Web.Script.Serialization.JavaScriptSerializer().Serialize(this.SelectedValue.Split(','));
                    }
                    writer.Write("<script lang=\"ja\">var js_" + ClientID + " = new JS_ParentToChildSelectorByDataSource('" + ClientID + "','" + jsdatasource.ClientID + "',"+json+");</script>");
                }
               
            }
        }

        string _text = "";
        public string Text
        {
            get
            {
                return _text;
            }
        }

        public bool LoadPostData(string postDataKey, System.Collections.Specialized.NameValueCollection postCollection)
        {
            //onload必须加入这句
            //Page.RegisterRequiresPostBack(this);
            try
            {
                _text = this.Page.Request.Form[ClientID + "_text"];
                if (this.Page.Request.Form[ClientID] != null)
                    SelectedValue = this.Page.Request.Form[ClientID];

            }
            catch
            {
            }
            return false;
        }

        public void RaisePostDataChangedEvent()
        {
            
        }
    }
    [Editor.TitleField("KeyTableName"),Serializable]
    public class PCItem : ICloneable, IKeyObject, IServiceProvider
    {
        [NonSerialized]
        internal ParentToChildSelector Container;
        string _KeyTableName;
        [System.ComponentModel.Category("系统设定"), System.ComponentModel.Description("表名"),
        System.ComponentModel.Browsable(true),
        System.ComponentModel.Editor(typeof(Editor.TableNameSelector), typeof(System.Drawing.Design.UITypeEditor))]
        public string KeyTableName
        {
            get
            {
                return _KeyTableName;
            }
            set
            {
                if (_KeyTableName != value)
                {
                    _KeyTableName = value;
                }
            }
        }

        string _KeyIDField;
        [System.ComponentModel.Category("系统设定"), System.ComponentModel.Description("ID字段"),
        System.ComponentModel.Editor(typeof(Editor.TableFieldSelector), typeof(System.Drawing.Design.UITypeEditor))]
        public string KeyIDField
        {
            get
            {
                return _KeyIDField;
            }
            set
            {
                if (_KeyIDField != value)
                {
                    _KeyIDField = value;
                }
            }
        }

        string _KeyTextField;
        [System.ComponentModel.Category("系统设定"), System.ComponentModel.Description("显示字段"),
        System.ComponentModel.Editor(typeof(Editor.TableFieldSelector), typeof(System.Drawing.Design.UITypeEditor))]
        public string KeyTextField
        {
            get
            {
                return _KeyTextField;
            }
            set
            {
                if (_KeyTextField != value)
                {
                    _KeyTextField = value;
                }
            }
        }

        string _OrderString;
        [System.ComponentModel.Category("系统设定"), System.ComponentModel.Description("排序语句")]
        public string OrderString
        {
            get
            {
                return _OrderString;
            }
            set
            {
                if (_OrderString != value)
                {
                    _OrderString = value;
                }
            }
        }

        string _TermString;
        [System.ComponentModel.Category("系统设定"), System.ComponentModel.Description("条件语句")]
        public string TermString
        {
            get
            {
                return _TermString;
            }
            set
            {
                if (_TermString != value)
                {
                    _TermString = value;
                }
            }
        }

        string _KeyParentIDField;
        [System.ComponentModel.Category("系统设定"), System.ComponentModel.Description("父表的id字段"),
        System.ComponentModel.Editor(typeof(Editor.TableFieldSelector), typeof(System.Drawing.Design.UITypeEditor))]
        public string KeyParentIDField
        {
            get
            {
                return _KeyParentIDField;
            }
            set
            {
                if (_KeyParentIDField != value)
                {
                    _KeyParentIDField = value;
                }
            }
        }

        [System.ComponentModel.Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public System.ComponentModel.ISite Site
        {
            get
            {
                if (Container == null)
                    return null;
                return Container.Site;
            }
        }

        public object Clone()
        {
            return new PCItem()
            {
                KeyTableName = this.KeyTableName,
                KeyIDField = this.KeyIDField,
                Container = this.Container,
                KeyTextField = this.KeyTextField,
                KeyParentIDField = this.KeyParentIDField,
                TermString = this.TermString,
                OrderString = this.OrderString
            };
        }

        public object GetService(Type type)
        {
            return this.Site.GetService(type);
        }


        public IAutoDataBindControl AutoDataBindControl
        {
            get { return Container; }
        }
    }

   
}
