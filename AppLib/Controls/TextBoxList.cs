using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Caching;
using System.Web.Script.Serialization;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace AppLib.Controls
{
    public class TextBoxList : TextBox, IKeyObject, IAutoDataBindControl, IPostBackEventHandler 
    {

        #region 属性
        public event EventHandler SelectChanged;
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
        public int PageSize
        {
            get
            {
                if (ViewState["PageSize"] == null)
                    ViewState["PageSize"] = 10;
                return Convert.ToInt32(ViewState["PageSize"]);
            }
            set
            {
                ViewState["PageSize"] = value;
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

        [System.ComponentModel.Category("系统设定"), System.ComponentModel.Description("隐藏下拉箭头")]
        public bool HideDropDownImage
        {
            get
            {
                if (ViewState["HideDropDownImage"] == null)
                    ViewState["HideDropDownImage"] = false;
                return (bool)ViewState["HideDropDownImage"];
            }
            set
            {
                ViewState["HideDropDownImage"] = value;
            }
        }


        public int TextLengthToSelect
        {
            get
            {
                return ViewState["TextLengthToSelect"] == null ? 0 : (int)ViewState["TextLengthToSelect"];
            }
            set
            {
                ViewState["TextLengthToSelect"] = value;
            }
        }

        string _ListWidth = "200px";
        [System.ComponentModel.Category("系统设定"), System.ComponentModel.Description("列表的宽度")]
        public string ListWidth
        {
            get
            {
                return _ListWidth;
            }
            set
            {
                if (_ListWidth != value)
                {
                    _ListWidth = value;
                }
            }
        }
        string _ValueID;
        public string ValueID
        {
            get {
                return _ValueID;
            }
            set
            {
                _ValueID = value;
                if (string.IsNullOrEmpty(JSDataSource))
                {
                    if (string.IsNullOrEmpty(value))
                    {
                        this.Text = "";
                    }
                    else
                    {
                        if (KeyTextField != KeyIDField)
                        {
                            using (Way.EntityDB.DBContext db = AppHelper.CreateLinqDataBase(this.DatabaseConfig))
                            {

                                Type dtType = db.GetType();
                                var query = dtType.GetProperty(KeyTableName).GetValue(db);
                                query = Way.EntityDB.DBContext.InvokeWhereEquals(query, KeyIDField, value);
                                query = Way.EntityDB.DBContext.InvokeSelect(query, KeyTextField);
                                this.Text = Convert.ToString(Way.EntityDB.DBContext.InvokeFirstOrDefault(query));
                            }
                        }
                        else
                        {
                            this.Text = value;
                        }
                    }
                }
                else
                {
                    JSDataSource jsdatasource = AppHelper.GetControlsByTypes(this.Page, new Type[] { typeof(JSDataSource) }).FirstOrDefault(m => m.ID == this.JSDataSource) as JSDataSource;
                    if (jsdatasource != null)
                    {
                        this.Text = jsdatasource.GetTextByValue(value);
                    }
                   
                }
            }
        }
        #endregion

        public TextBoxList()
        {
            
        }

        class JsonObject
        {
            public string DatabaseConfig;
            public string KeyOrderString;
            public string KeyIDField;
            public string KeyTermString;
            public string KeyTextField;
            public string KeyTableName;
        }

        #region OutputData
        protected void LinqOutputData( )
        {
            using (CLog log = new CLog(""))
            {
                try
                {
                    string stateFileName = this.Page.Request.QueryString["stateFileName"];
                    JsonObject jsonObject = new FileStatePersister(this.Page).ReadState<JsonObject>(stateFileName);
                    HtmlTextWriter writer = new HtmlTextWriter(Page.Response.Output);
                    if (Page.Request.QueryString["p"] != null)
                    {
                        writer.Write(@"
<!DOCTYPE html>

<html xmlns=""http://www.w3.org/1999/xhtml"">
<head runat=""server"">
<meta http-equiv=""Content-Type"" content=""text/html; charset=utf-8""/>
    <title></title>

</head>
<body style=""margin:0px;overflow:hidden;background-color:white;"">
<div>
");

                        using (Way.EntityDB.DBContext database = AppHelper.CreateLinqDataBase(jsonObject.DatabaseConfig))
                        {
                            int pageindex = 0;
                            try
                            {
                                pageindex = Convert.ToInt32(Page.Request.QueryString["p"]);
                            }
                            catch { }
                            int rowcount = 0;
                            string orderby = jsonObject.KeyOrderString;
                            if (string.IsNullOrEmpty(orderby))
                            {
                                orderby = string.Format("[{0}]", jsonObject.KeyIDField);
                            }
                            string termstring = jsonObject.KeyTermString;
                           

                            int pagesize = Convert.ToInt32(Page.Request.QueryString["pagesize"]);

                            Type dbType = database.GetType();
                            var queryPinfos = dbType.GetProperties().Where(m => m.Name == jsonObject.KeyTableName);
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
                            if (!string.IsNullOrEmpty(termstring))
                            {
                                try
                                {
                                    log.Log("条件过滤:{0}", termstring);
                                    query = Way.EntityDB.DBContext.GetQueryByString(query, termstring);
                                    log.Log("过滤后:{0}", query);
                                }
                                catch (Exception ex)
                                {
                                    throw new Exception(ex.Message + "\r\n出错的表达式：" + termstring);
                                }
                            }

                            if (Page.Request.QueryString["key"].Length > 0)
                            {
                                query = Way.EntityDB.DBContext.InvokeWhereWithMethod(query, typeof(string).GetMethod("Contains"), jsonObject.KeyTextField, Page.Request.QueryString["key"]);
                                
                            }

                            if (!string.IsNullOrEmpty(orderby))
                            {
                                try
                                {
                                    query = Way.EntityDB.DBContext.GetQueryForOrderBy(query, orderby);
                                }
                                catch (Exception ex)
                                {
                                    throw new Exception(ex.Message + "\r\n出错的表达式：" + orderby);
                                }
                            }

                            if (true)
                            {
                                Type queryType = typeof(System.Linq.Queryable);
                                var methods = queryType.GetMethods(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static).Where(m => m.Name == "Count");
                                foreach (System.Reflection.MethodInfo method in methods)
                                {
                                    if(method.GetParameters().Length == 1)
                                    {
                                        System.Reflection.MethodInfo mmm = method.MakeGenericMethod(dataType);
                                        if (mmm != null)
                                        {
                                            rowcount = Convert.ToInt32(mmm.Invoke(null, new object[] { query }));
                                        }
                                        break;
                                    }
                                    
                                }

                                methods = queryType.GetMethods(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static).Where(m => m.Name == "Skip");
                                foreach (System.Reflection.MethodInfo method in methods)
                                {
                                    if (method.GetParameters().Length == 2)
                                    {
                                        System.Reflection.MethodInfo mmm = method.MakeGenericMethod(dataType);
                                        if (mmm != null)
                                        {
                                            query = mmm.Invoke(null, new object[] { query, pageindex * pagesize });
                                        }
                                    }
                                }
                                methods = queryType.GetMethods(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static).Where(m => m.Name == "Take");
                                foreach (System.Reflection.MethodInfo method in methods)
                                {
                                    if (method.GetParameters().Length == 2)
                                    {
                                        System.Reflection.MethodInfo mmm = method.MakeGenericMethod(dataType);
                                        if (mmm != null)
                                        {
                                            query = mmm.Invoke(null, new object[] { query, pagesize });

                                        }
                                    }
                                }
                            }
                            try
                            {
                                query = Way.EntityDB.DBContext.InvokeSelect(query , jsonObject.KeyTextField);
                                System.Collections.IEnumerable datasource = (System.Collections.IEnumerable)query;
                                foreach (var textitem in datasource)
                                {
                                    string text = Convert.ToString(textitem);
                                    if (text == null)
                                        text = "";
                                    string html = System.Web.HttpUtility.HtmlEncode(text);
                                    writer.Write("<div onclick=\"js_" + Page.Request.QueryString["cid"] + ".setValue(this.innerHTML);\" title=\"" + html + "\" class=\"_textBoxList_div1\">" + html + "</div>");
                                }
                            }
                            catch
                            {
                            }
                            int pagecount = rowcount / pagesize;
                            if (rowcount % pagesize > 0)
                                pagecount++;
                            if (pagecount > 1)
                            {
                                writer.Write("<div class=\"_textBoxList_div1\">");
                                if (pageindex > 0)
                                {
                                    writer.Write("<div onclick=\"js_" + Page.Request.QueryString["cid"] + ".go(" + (pageindex - 1) + ");\" class=\"_textBoxList_btn\">");
                                    writer.WriteEncodedText("上一页");
                                    writer.Write("</div>");
                                }
                                else
                                {
                                    writer.Write("<div class=\"_textBoxList_btn2\">");
                                    writer.WriteEncodedText("上一页");
                                    writer.Write("</div>");
                                }
                                if (pageindex < pagecount - 1)
                                {
                                    writer.Write("<div onclick=\"js_" + Page.Request.QueryString["cid"] + ".go(" + (pageindex + 1) + ");\" class=\"_textBoxList_btn\">");
                                    writer.WriteEncodedText("下一页");
                                    writer.Write("</div>");
                                }
                                else
                                {
                                    writer.Write("<div class=\"_textBoxList_btn2\">");
                                    writer.WriteEncodedText("下一页");
                                    writer.Write("</div>");
                                }


                                writer.Write("</div>");
                            }
                        }

                        //parent.window.js_" + Page.Request.QueryString["cid"] + ".focus();
                        writer.Write("</div><script lang=\"javascript\">parent.window.js_" + Page.Request.QueryString["cid"] + ".frameLoaded();</script>");
                        //writer.Write("<script lang=\"javascript\">parent.window.document.getElementsByName(\"" + Page.Request.QueryString["cid"] + "_iframe\")[0].style.height = (document.body.clientHeight - 1) + 'px';</script>");
                        writer.Write("</body></html>");
                    }
                    Page.Response.End();
                }
                catch (Exception ex)
                {
                    log.Log("err:{0}", ex);
                    Page.Response.End();
                }
            }
        }
      
        #endregion


        protected override bool LoadPostData(string postDataKey, System.Collections.Specialized.NameValueCollection postCollection)
        {
            //onload 必须加入这句
            //Page.RegisterRequiresPostBack(this);
            bool result = base.LoadPostData(postDataKey, postCollection);

            try
            {
                if (string.IsNullOrEmpty(this.JSDataSource))
                {
                    if (!string.IsNullOrEmpty(this.Text))
                    {
                        if (KeyTextField != KeyIDField)
                        {
                            using (Way.EntityDB.DBContext db = AppHelper.CreateLinqDataBase(this.DatabaseConfig))
                            {
                                Type dtType = db.GetType();
                                var query = dtType.GetProperty(KeyTableName).GetValue(db);
                                query = Way.EntityDB.DBContext.InvokeWhereEquals(query, KeyTextField, this.Text);
                                query = Way.EntityDB.DBContext.InvokeSelect(query , KeyIDField);
                                _ValueID = Convert.ToString( Way.EntityDB.DBContext.InvokeFirstOrDefault(query));
                            }
                        }
                        else
                        {
                            _ValueID = this.Text;
                        }
                    }
                    else
                    {
                        _ValueID = "";
                    }
                }
                else
                {
                    JSDataSource jsdatasource = AppHelper.GetControlsByTypes(this.Page, new Type[] { typeof(JSDataSource) }).FirstOrDefault(m => m.ID == this.JSDataSource) as JSDataSource;
                    if (jsdatasource != null)
                    {
                        _ValueID = jsdatasource.GetValueByText(this.Text);
                    }
                }
            }
            catch
            {
            }
            return result;
        }

        protected override void OnInit(EventArgs e)
        {
            

            if (this.DesignMode)
            {
                base.OnInit(e);
                return;
            }
            if (Page.Request.QueryString["rendercontrol"] == "TextBoxList")
            {
                string stateFileName = this.Page.Request.QueryString["stateFileName"];
                JsonObject jsonObject = new FileStatePersister(this.Page).ReadState<JsonObject>(stateFileName);

                try
                {
                    LinqOutputData();
                }
                catch
                {
                }
            }
            else
            {
               
                base.OnInit(e);
            }
        }

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
            if (!this.Page.ClientScript.IsClientScriptIncludeRegistered("__TextBoxList_JSFile"))
            {
                Page.ClientScript.RegisterClientScriptInclude("__TextBoxList_JSFile",
             Page.ClientScript.GetWebResourceUrl(this.GetType(),
                                         "AppLib.js.TextBoxList.js"));
            }

            AppHelper.RegisterJquery(this.Page);

            #region style
            if (!this.Page.ClientScript.IsClientScriptBlockRegistered("__TextBoxList_style"))
            {
                
                string style = @"
<style>
._textBoxList_div1
{
overflow:hidden;
padding:3px;
font-size:14px;
border-bottom:1px solid #cccccc;
cursor:pointer;
}
._textBoxList_btn
{
font-size:12px;
padding-top:2px;
padding-right:10px;
font-weight:bold;
cursor:pointer;
float:left;
}
._textBoxList_btn2
{
color:#eeeeee;
font-size:12px;
padding-top:2px;
padding-right:10px;
font-weight:bold;
float:left;
}
</style>
";
                this.Page.ClientScript.RegisterClientScriptBlock(this.Page.GetType(), "__TextBoxList_style",style);

            }
            #endregion
        }

        protected override void Render(System.Web.UI.HtmlTextWriter writer)
        {
            if (!HideDropDownImage && string.IsNullOrEmpty(CssClass))
            {
                this.Style["background-image"] = "url(/inc/imgs/___dropdown.gif)";
                this.Style["background-position"] = "right";
                this.Style["background-repeat"] = "no-repeat";
            }

            base.Render(writer);

            if (this.DesignMode)
                return;
            
            if (string.IsNullOrEmpty(JSDataSource))
            {
                new FileStatePersister(this.Page).WriteState((string)ViewState["stateFileName"], new JsonObject()
                {
                    DatabaseConfig = this.DatabaseConfig,
                    KeyIDField = this.KeyIDField,
                    KeyOrderString = this.KeyOrderString,
                    KeyTableName = this.KeyTableName,
                    KeyTermString = this.KeyTermString,
                    KeyTextField = this.KeyTextField
                });

                writer.Write(string.Format("<iframe name=\"{0}_iframe\" src='?rendercontrol=TextBoxList&pagesize={1}&cid={0}&stateFileName=" + ViewState["stateFileName"] + "' style=\"display:none;\"></iframe>",
                ClientID, PageSize));
                writer.Write(string.Format("<div id=\"{0}_div\" _src='?rendercontrol=TextBoxList&pagesize={1}&cid={0}&stateFileName=" + ViewState["stateFileName"] + "' style=\"z-index:999;border:1px solid black;display:none;position:absolute;background-color:white;\"></div>",
                   ClientID, PageSize));
                writer.Write("<script lang=\"javascript\">var js_" + ClientID + " = new JTextBoxList(" + TextLengthToSelect + ",\"" + ClientID + "\")</script>");
            }

            else
            {
                Control jsdatasource = AppHelper.GetControlsByTypes(this.Page , new Type[] {typeof(JSDataSource)}).FirstOrDefault(m=>m.ID == this.JSDataSource);
                if (jsdatasource == null)
                {
                    writer.WriteLine("在当前页面无法找到" + JSDataSource);
                }
                else
                {
                    writer.Write(string.Format("<div id=\"{0}_div\" _src='?rendercontrol=TextBoxList&pagesize={1}&cid={0}&stateFileName=" + ViewState["stateFileName"] + "' style=\"z-index:999;border:1px solid black;display:none;position:absolute;background-color:white;\"></div>",
                  ClientID, PageSize));
                    writer.Write("<script lang=\"javascript\">var js_" + ClientID + " = new JTextBoxList(" + TextLengthToSelect + ",\"" + ClientID + "\",\"" + jsdatasource.ClientID + "\"," + this.PageSize + ")</script>");
                }
            }
            if (SelectChanged != null)
            {

                writer.Write("<script lang=\"javascript\">js_" + ClientID + ".setPostBack(\"" + this.Page.ClientScript.GetPostBackEventReference(this, "SelectChanged") + "\");</script>");
            
            }
            

        }





        public IAutoDataBindControl AutoDataBindControl
        {
            get { return this; }
        }

        public void RaisePostBackEvent(string eventArgument)
        {
            if ( eventArgument =="SelectChanged" && this.SelectChanged != null)
                this.SelectChanged(this, null);
        }
    }

}
