using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI;
using System.Web.UI.Design;
using System.Web.UI.WebControls;

namespace AppLib.Controls
{
    #region 设计时
    internal class WayControlBuilder : ControlBuilder
    {


        /// <summary>
        /// 
        /// </summary>
        /// <param name="tagName"></param>
        /// <param name="attribs"></param>
        /// <returns></returns>
        public override Type GetChildControlType(string tagName, IDictionary attribs)
        {
            // Allows TableRow without "runat=server" attribute to be added to the collection.
            if (tagName.Contains("ValueChange"))
                return typeof(ValueChange);
            else if (tagName == "asp:TextBox")
            {
                return typeof(TextBox);
            }

            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="s"></param>
        public override void AppendLiteralString(string s)
        {
            // Ignores literals between rows.
        }


       
    }

    public class MyDesigner : System.Web.UI.Design.WebControls.PanelContainerDesigner
    {
        public MyDesigner()
        {

        }

        public override string GetEditableDesignerRegionContent(EditableDesignerRegion region)
        {
            ((EntityEditor)this.Component).WebFormsRootDesigner = this.RootDesigner;
            return base.GetEditableDesignerRegionContent(region);
        }
    }
    #endregion

    public enum ActionType
    {
        Insert = 0,
        Update = 1,
        None = 2
    }

    /// <summary>
    /// 
    /// </summary>
    [Designer(typeof(MyDesigner))]
    public class EntityEditor : Panel, IAutoDataBindControl, IPostBackEventHandler
    {
        internal WebFormsRootDesigner WebFormsRootDesigner;

        #region 事件

        public delegate void InsertDataHandler(object database, object sender, object dataitem, ControlEventArg e);
        /// <summary>
        /// 添加数据时触发的事件
        /// </summary>
        [System.ComponentModel.Category("系统设定"), System.ComponentModel.Description("添加数据时触发的事件")]
        public event InsertDataHandler BeforeInsertData;
        [System.ComponentModel.Category("系统设定"), System.ComponentModel.Description("更新数据时触发的事件")]
        public event InsertDataHandler BeforeUpdateData;
        public delegate void AfterSaveHandler(object database, object sender, ActionType actionType, object dataitem);


        [System.ComponentModel.Category("系统设定"), System.ComponentModel.Description("保存数据后触发的事件")]
        public event AfterSaveHandler AfterSave;


        [System.ComponentModel.Category("系统设定"), System.ComponentModel.Description("保存数据前触发的事件")]
        public event AfterSaveHandler BeforeSave;
        public delegate void BeforeChangeModeHandler(object sender, ActionType actionType, object dataitem);
        [System.ComponentModel.Category("系统设定"), System.ComponentModel.Description("ChangeMode执行前触发")]
        public event BeforeChangeModeHandler BeforeChangeMode;

     
        #endregion

        #region 属性
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

        [System.ComponentModel.Category("系统设定"), System.ComponentModel.Editor(typeof(Editor.TableNameSelector), typeof(System.Drawing.Design.UITypeEditor))]
        public string TableName
        {
            get
            {
                return Convert.ToString(ViewState["TableName"]);
            }
            set
            {
                ViewState["TableName"] = value;
            }
        }
        [System.ComponentModel.Category("系统设定")]
        public string IDFieldName
        {
            get
            {
                return Convert.ToString(ViewState["IDFieldName"]);
            }
            set
            {
                ViewState["IDFieldName"] = value;
            }
        }

        [System.ComponentModel.Category("系统设定"), System.ComponentModel.Description("添加成功的提示语句")]
        public string InsertSuccessedMessage
        {
            get
            {
                return Convert.ToString(ViewState["InsertSuccessedMessage"]);
            }
            set
            {
                ViewState["InsertSuccessedMessage"] = value;
            }
        }

        [System.ComponentModel.Category("系统设定"), System.ComponentModel.Description("修改数据成功的提示语句")]
        public string UpdateSuccessedMessage
        {
            get
            {
                return Convert.ToString(ViewState["UpdateSuccessedMessage"]);
            }
            set
            {
                ViewState["UpdateSuccessedMessage"] = value;
            }
        }

        List<System.Web.UI.Control> _ChildControls = null;
        List<System.Web.UI.Control> ChildControls
        {
            get
            {
                if (_ChildControls == null)
                {
                    _ChildControls = AppHelper.GetControlsByTypes(Controls, new Type[] { typeof(System.Web.UI.WebControls.WebControl) });
                }
                return _ChildControls;
            }
        }

        object _CurrentDataItem = null;
        public object CurrentDataItem
        {
            get{
                if (_CurrentDataItem != null)
                    return _CurrentDataItem;
                if (ViewState["CurrentDataItem"] != null)
                {
                    Type type = ViewState["CurrentDataItemType"] as Type;
                    _CurrentDataItem = Newtonsoft.Json.JsonConvert.DeserializeObject(ViewState["CurrentDataItem"].ToString() , type);
                }
                return _CurrentDataItem;
            }
            set
            {
                _CurrentDataItem = value;
                if (value == null)
                    ViewState["CurrentDataItem"] = null;
                else
                {
                    ViewState["CurrentDataItemType"] = value.GetType();
                    ViewState["CurrentDataItem"] = Newtonsoft.Json.JsonConvert.SerializeObject(value);
                }
            }
        }

        /// <summary>
        /// 获取_EntityEditor_Insert的js表示
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public string InsertJS
        {
            get
            {
                return string.Format("_EntityEditor_Insert('{0}')", this.ClientID);
            }
        }

        /// <summary>
        /// 获取当前正执行怎样的操作的中文表述，值可能是"新增"或者"编辑"
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public string CurrentActionDescription
        {
            get
            {
                if (ViewState["CurrentActionDescription"] == null)
                    ViewState["CurrentActionDescription"] = "新增";
                return Convert.ToString(ViewState["CurrentActionDescription"]);


            }
            private set
            {
                ViewState["CurrentActionDescription"] = value;
            }
        }
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public ActionType CurrentMode
        {
            get
            {
                if (ViewState["CurrentMode"] == null)
                    return ActionType.None;
                return (ActionType)ViewState["CurrentMode"];
            }
            private set
            {
                ViewState["CurrentMode"] = value;
            }
        }

        /// <summary>
        /// 获取_EntityEditor_Save的js表示
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public string SaveJS
        {
            get
            {
                return string.Format("_EntityEditor_Save('{0}')", this.ClientID);
            }
        }

        /// <summary>
        /// 获取_EntityEditor_Cancel的js表示
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public string CancelJS
        {
            get
            {
                return string.Format("_EntityEditor_Cancel('{0}')", this.ClientID);
            }
        }

        #endregion

       

        public EntityEditor()
        {

        }

        public string GetModifyDataJS(object dataid)
        {
            return string.Format("_EntityEditor_ModifyData('{0}' , {1});",this.ClientID ,dataid);
        }

        protected override void LoadViewState(object savedState)
        {
            base.LoadViewState(savedState);
            m_defaultValues = ViewState["m_defaultValues"] as List<object[]>;
        }

        void createDefaultValues()
        {
            if (ViewState["m_defaultValues"] == null)
            {
                m_defaultValues = new List<object[]>();
                ViewState["m_defaultValues"] = m_defaultValues;
                foreach (System.Web.UI.WebControls.WebControl ctrl in this.ChildControls)
                {
                    string datafield = ctrl.Attributes["_DataField"];
                    if (string.IsNullOrEmpty(datafield))
                        continue;
 
                    if (ctrl is TextBoxList)
                    {
                        m_defaultValues.Add(new object[] { ctrl.ID, ((TextBoxList)ctrl).Text });
                    }
                    else
                    {
                        m_defaultValues.Add(new object[] { ctrl.ID, GetControlValue(ctrl) });
                    }
                }
            }
        }

        protected override object SaveViewState()
        {
            createDefaultValues();
            return base.SaveViewState();
        }


        protected override void OnLoad(EventArgs e)
        {
            if (!this.DesignMode)
            {
                if (!Page.IsPostBack)
                {
                    this.DataBind();
                    this.Bind();
                }


            }

            base.OnLoad(e);
            WriteJS();

            string commandKey = string.Format("{0}_$$_command", ClientID);
            this.Page.ClientScript.RegisterHiddenField(commandKey, "");
            
            if (!string.IsNullOrEmpty(this.Page.Request.Form[commandKey]))
            {
                Command(this.Page.Request.Form[commandKey]);
            }
        }

        private void Command(string command)
        {
            if (command == "save")
            {
                this.Save();
            }
            else if (command == "cancel")
            {
                this.ChangeMode(ActionType.None, null);
            }
            else if (command == "insert")
            {
                this.ChangeMode(ActionType.Insert, null);
            }
            else if (command.StartsWith("m:"))
            {
                this.ChangeMode(ActionType.Update, command.Substring(2));
            }
        }


        List<object[]> m_defaultValues;
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            
        }

      

        public void Save()
        {
            try
            {
                #region command


                if (CurrentMode == ActionType.None)
                {
                    CurrentMode = ActionType.Insert;
                    CurrentDataItem = CreateNewData();
                    
                    if (BeforeChangeMode != null)
                    {
                        BeforeChangeMode(this, ActionType.Insert, null);
                    }
                }

                if (true)
                {
                    #region linq
                    var dataitem = CurrentDataItem as Way.EntityDB.DataItem;
                    foreach (System.Web.UI.WebControls.WebControl ctrl in this.ChildControls)
                    {
                        if (ctrl is Label)
                            continue;

                        string datafield = ctrl.Attributes["_DataField"];
                        if (string.IsNullOrEmpty(datafield))
                            continue;

                        if (!string.IsNullOrEmpty(ctrl.Attributes["_Custom"]))
                            continue;

                        object value = GetControlValue(ctrl);
                        if (value == null && CurrentMode == ActionType.Update)
                        {
                            dataitem.SetValue(datafield, null);
                            continue;
                        }
                        if (CurrentMode == ActionType.Insert)
                        {
                            if (value == null || (value is string && ((string)value).Length == 0))
                                continue;
                        }

                        PropertyInfo pinfo = dataitem.GetType().GetProperty(datafield);
                        if (pinfo == null)
                            throw new Exception(string.Format("{0}没有包含属性{1}", dataitem.GetType().FullName, datafield));
                        if (pinfo.PropertyType.IsGenericType && "".Equals(value))
                            value = null;
                        try
                        {
                            dataitem.SetValue(datafield, value);
                        }
                        catch
                        {

                            if (pinfo == null)
                                throw new Exception("EntityEditor无法找到" + datafield + "字段");
                            Type datatype = pinfo.PropertyType;
                            if (datatype == typeof(int?))
                            {
                                throw (new VerifyException("“" + ctrl.Attributes["_Caption"] + "”只能输入整数", ctrl));
                            }
                            else if (datatype == typeof(float?) || datatype == typeof(double?) || datatype == typeof(decimal?))
                            {
                                throw (new VerifyException("“" + ctrl.Attributes["_Caption"] + "”只能输入数字", ctrl));
                            }
                            else if (datatype == typeof(DateTime?))
                            {
                                throw (new VerifyException("“" + ctrl.Attributes["_Caption"] + "”只能输入时间", ctrl));
                            }
                            else
                            {
                                throw (new VerifyException("“" + ctrl.Attributes["_Caption"] + "”输入的数据格式不正确", ctrl));
                            }
                        }
                    }
                    #endregion
                }
                #endregion

                if (CurrentMode == ActionType.Insert)
                {

                    Way.EntityDB.DataItem dataitem = CurrentDataItem as Way.EntityDB.DataItem;
                        #region insert


                        using (Way.EntityDB.DBContext database = AppHelper.CreateLinqDataBase(this.DatabaseConfig))
                        {
                            //database.BeginTransaction();
                            try
                            {

                                ControlEventArg arg = new ControlEventArg();
                                if (BeforeInsertData != null)
                                {
                                    BeforeInsertData(database, this, dataitem, arg);
                                }
                                if (BeforeSave != null)
                                {
                                    BeforeSave(database, this, ActionType.Insert, dataitem);
                                }
                                if (!arg.Handle)
                                {
                                    database.Update(dataitem);
                                }
                                ClearValues();
                                if (AfterSave != null)
                                {
                                    AfterSave(database, this, ActionType.Insert, dataitem);
                                }
         
                                this.CurrentDataItem = CreateNewData();

                                //database.CommitTransaction();

                                AppHelper.Alert(this.Page, string.IsNullOrEmpty(InsertSuccessedMessage) ? "添加成功！" : InsertSuccessedMessage);
                            }
                            catch (Exception ex)
                            {
                                //database.RollbackTransaction();
                                throw ex;
                            }
                        }
                        #endregion
                    
                }
                else
                {
                 
                        var dataitem = CurrentDataItem as Way.EntityDB.DataItem;
                        #region update


                        using (Way.EntityDB.DBContext database = AppHelper.CreateLinqDataBase(this.DatabaseConfig))
                        {
                            //database.BeginTransaction();
                            try
                            {
                               
                                ControlEventArg arg = new ControlEventArg();
                                if (BeforeUpdateData != null)
                                {
                                    BeforeUpdateData(database, this, dataitem, arg);
                                }
                                if (BeforeSave != null)
                                {
                                    BeforeSave(database, this, ActionType.Update, dataitem);
                                }
                                if (!arg.Handle)
                                {
                                    database.Update(dataitem);
                                }
                                ClearValues();
                                if (AfterSave != null)
                                {
                                    AfterSave(database, this, ActionType.Update, dataitem);
                                }
                                this.ChangeMode(ActionType.None, null);

                                AppHelper.Alert(this.Page, string.IsNullOrEmpty(UpdateSuccessedMessage) ? "保存成功！" : UpdateSuccessedMessage);

                            }
                            catch (Exception ex)
                            {
                                //database.RollbackTransaction();
                                throw ex;
                            }
                        }
                        #endregion
               
                }


            }
            catch (VerifyException ex)
            {
                if (ex.Control != null)
                {
                    this.Page.ClientScript.RegisterStartupScript(this.Page.GetType(), Guid.NewGuid().ToString(), "<script language=\"javascript\">document.getElementById('" + ex.Control.ClientID + "').focus();</script>\r\n");
                }
                AppHelper.Alert(this.Page, ex.Message);
            }
            catch (Exception ex)
            {
                AppHelper.Alert(this.Page, ex.Message);
            }
        }

        void ClearValues()
        {
            foreach (System.Web.UI.WebControls.WebControl ctrl in this.ChildControls)
            {
                string datafield = ctrl.Attributes["_DataField"];
                if (string.IsNullOrEmpty(datafield))
                    continue;

                object[] obj = m_defaultValues.FirstOrDefault(m => m[0].Equals(ctrl.ID));
                if (obj != null)
                {
                    if (ctrl is TextBoxList)
                    {
                        ((TextBoxList)ctrl).Text = Convert.ToString(obj[1]);
                    }
                    else
                    {
                        SetControlValue(ctrl, obj[1]);
                    }
                }
            }
        }

        internal static object GetControlValue(object ctrl)
        {

            if (ctrl is TextBoxList)
            {
                return ((TextBoxList)ctrl).ValueID;
            }
            else if (ctrl is Label)
            {
                return ((Label)ctrl).Text;
            }
            else if (ctrl is ParentToChildSelector)
            {
                ParentToChildSelector pSelector = ctrl as ParentToChildSelector;
                try
                {
                    string[] values = pSelector.SelectedValue.Split(',');
                    return values[values.Length - 1];
                }
                catch
                {
                    return null;
                }
            }
            else if (ctrl is CheckBox)
            {
                return ((CheckBox)ctrl).Checked;
            }
            else if (ctrl is TextBox)
            {
                if (((TextBox)ctrl).TextMode == TextBoxMode.Password)
                    return ((TextBox)ctrl).Text;
                return ((TextBox)ctrl).Text.Trim();
            }
            else if (ctrl is DropDownList)
            {
                return ((DropDownList)ctrl).SelectedValue;
            }
            else if (ctrl is RadioButtonList)
            {
                return ((RadioButtonList)ctrl).SelectedValue;
            }
            else if (ctrl is ListBox)
            {
                ListBox list = ctrl as ListBox;
                StringBuilder value = new StringBuilder();
                foreach (ListItem item in list.Items)
                {
                    if (item.Selected)
                        value.Append(item.Value);
                    value.Append(',');
                }
                return value.ToString();
            }
            else if (ctrl is CheckBoxList)
            {
                CheckBoxList list = ctrl as CheckBoxList;
                StringBuilder value = new StringBuilder();
                foreach (ListItem item in list.Items)
                {
                    if (item.Selected)
                    {
                        if(value.Length > 0)
                            value.Append(',');
                        value.Append(item.Value);
                    }
                }
                return value.ToString();
            }
            else
                throw (new Exception("GetControlValue目前不支持" + ctrl));
        }

        private void SetControlValue(object ctrl, object value)
        {
           
            try
            {
                if (value is decimal || value is decimal?)
                {
                    value = value.ToDouble();
                }
                if (ctrl is TextBoxList)
                {
                    ((TextBoxList)ctrl).ValueID = Convert.ToString(value);
                }
                else if (ctrl is Label)
                {
                    ((Label)ctrl).Text = Convert.ToString(value);
                }
                else if (ctrl is ParentToChildSelector)
                {
                    ((ParentToChildSelector)ctrl).SetLastSelectionValue(Convert.ToString(value));
                }
                else if (ctrl is CheckBox)
                {
                    if (value == null)
                    {
                        ((CheckBox)ctrl).Checked = false;
                    }
                    else
                    {
                        ((CheckBox)ctrl).Checked = Convert.ToBoolean(value);
                    }
                }
                else if (ctrl is TextBox)
                {
                    if (value is DateTime)
                    {
                        DateTime time = Convert.ToDateTime(value);
                        if (time.Hour == 0 && time.Minute == 0 && time.Second == 0)
                        {
                            ((TextBox)ctrl).Text = time.ToString("yyyy-MM-dd");
                        }
                        else
                        {
                            ((TextBox)ctrl).Text = value.ToString();
                        }
                    }
                    else
                    {
                        ((TextBox)ctrl).Text = value.ToString();
                    }
                }
                else if (ctrl is DropDownList)
                {
                    ((DropDownList)ctrl).SelectedValue = value.ToString();
                }
                else if (ctrl is RadioButtonList)
                {
                    ((RadioButtonList)ctrl).SelectedValue = value.ToString();
                }
                else if (ctrl is ListBox)
                {
                    ListBox list = ctrl as ListBox;
                    string myvalue = "," + value + ",";
                    foreach (ListItem item in list.Items)
                    {
                        item.Selected = myvalue.Contains("," + item.Value + ",");
                    }
                }
                else if (ctrl is CheckBoxList)
                {
                    string[] nowvalue = value.ToString().Split(',');
                    CheckBoxList list = ctrl as CheckBoxList;
                    foreach (ListItem item in list.Items)
                    {
                        item.Selected = nowvalue.Contains( item.Value);
                    }
                }
                else
                    throw (new VerifyException("EntityEditor目前不支持" + ctrl, null));
            }
            catch (VerifyException ex)
            {
                throw (ex);
            }
            catch (Exception ex)
            {
            }
        }


        /// <summary>
        /// 生成js文件
        /// </summary>
        private void WriteJS()
        {

            if (!this.Page.ClientScript.IsClientScriptIncludeRegistered("__EntityEditor_JSFile"))
            {
                Page.ClientScript.RegisterClientScriptInclude("__EntityEditor_JSFile",
              Page.ClientScript.GetWebResourceUrl(this.GetType(),
                                          "AppLib.js.EntityEditor.js"));
            }

            if (!this.Page.ClientScript.IsClientScriptIncludeRegistered("_WdatePicker_JS"))
            {
                Page.ClientScript.RegisterClientScriptInclude("_WdatePicker_JS", "/inc/My97DatePicker/WdatePicker.js");
            }


        }


        object CreateNewData( )
        {
           
                if (string.IsNullOrEmpty(DatabaseConfig))
                {
                    throw new Exception("database.config未设置对应的Linq数据库类");
                }
                using (Way.EntityDB.DBContext db = AppHelper.CreateLinqDataBase(this.DatabaseConfig))
                {
                    Type dbType = db.GetType();

                    var queryPinfos = dbType.GetProperties().Where(m => m.Name == this.TableName);
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
                        throw (new Exception("无法在DBContext找到" + TableName + "对应的IQueryable<>属性"));
                    object query = queryPinfo.GetValue(db);
                    Type dataType = query.GetType().GetGenericArguments()[0];
                    return Activator.CreateInstance(dataType);
                }

        }

        object getDataItem(object dataID)
        {
    
                if (string.IsNullOrEmpty(DatabaseConfig))
                {
                    throw new Exception("database.config未设置对应的Linq数据库类");
                }

                #region linq
                using (Way.EntityDB.DBContext db = AppHelper.CreateLinqDataBase(this.DatabaseConfig))
                {
                    Type dbType = db.GetType();

                    var queryPinfos = dbType.GetProperties().Where(m => m.Name == this.TableName);
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
                        throw (new Exception("无法在DBContext找到" + TableName + "对应的IQueryable<>属性"));
                    object query = queryPinfo.GetValue(db);
                    Type dataType = query.GetType().GetGenericArguments()[0];

                    string pkid = null;
                    if (dataType != null)
                    {
                        try
                        {
                            object[] atts = dataType.GetCustomAttributes(typeof(Way.EntityDB.Attributes.Table), true);
                            pkid = ((Way.EntityDB.Attributes.Table)atts[0]).KeyName;
                        }
                        catch
                        {
                        }
                        Expression left, right;
                        LinqHelper.GetExpression(dataType, pkid, dataID, out left, out right);
                        left = Expression.Equal(left, right);
                        left = Expression.Lambda(left, Expression.Parameter(dataType, "n"));
                        Type queryableType = typeof(System.Linq.Queryable);
                        var methods = queryableType.GetMethods(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static).Where(m => m.Name == "Where");
                        foreach (System.Reflection.MethodInfo method in methods)
                        {
                            try
                            {
                                System.Reflection.MethodInfo mmm = method.MakeGenericMethod(dataType);
                                query = mmm.Invoke(null, new object[] { query, left });
                                break;
                            }
                            catch (Exception ex)
                            {
                            }
                        }
                        methods = queryableType.GetMethods(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static).Where(m => m.Name == "FirstOrDefault");
                        foreach (System.Reflection.MethodInfo method in methods)
                        {
                            try
                            {
                                System.Reflection.MethodInfo mmm = method.MakeGenericMethod(dataType);
                                return mmm.Invoke(null, new object[] { query });
                            }
                            catch (Exception ex)
                            {
                            }
                        }
                    }
                   
                }
                return null;
                #endregion
           
        }

        public void ChangeMode(ActionType actionType, object dataID)
        {
            createDefaultValues();
            if (actionType == ActionType.Update)
            {
                this.CurrentMode = ActionType.Update;
                CurrentActionDescription = "编辑";

                using (Way.EntityDB.DBContext db = AppHelper.CreateLinqDataBase(this.DatabaseConfig))
                {
                    Type dataType = AppHelper.GetDataItemType(this, this.TableName);
                    if (dataType == null)
                        throw (new Exception("无法找到" + TableName + "对应的数据库类"));
                   
                       
                        PropertyInfo pinfo = db.GetType().GetProperty(this.TableName , BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
                        object tablequery = pinfo.GetValue(db);
                        tablequery = Way.EntityDB.DBContext.GetQueryByString(tablequery, IDFieldName + "=" + dataID);
                        Type tdataType = pinfo.PropertyType.GetGenericArguments()[0];

                        //FirstOrDefault
                        Type queryableType = typeof(System.Linq.Queryable);
                        var methods = queryableType.GetMethods(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static).Where(m => m.Name == "FirstOrDefault");
                        foreach (System.Reflection.MethodInfo method in methods)
                        {
                            try
                            {
                                System.Reflection.MethodInfo mmm = method.MakeGenericMethod(tdataType);
                                this.CurrentDataItem = mmm.Invoke(null, new object[] { tablequery });
                                break;
                            }
                            catch (Exception ex)
                            {
                            }
                        }
                    
                    

                    if (BeforeChangeMode != null)
                    {
                        BeforeChangeMode(this, actionType, this.CurrentDataItem);
                    }
                    bindModifyData();
                }
            }
            else if (actionType == ActionType.Insert)
            {
                this.CurrentMode = ActionType.Insert;
                CurrentActionDescription = "新增";

                this.CurrentDataItem = CreateNewData();
                ClearValues();

                if (BeforeChangeMode != null)
                {
                    BeforeChangeMode(this, actionType, null);
                }
            }
            else if (actionType == ActionType.None)
            {
                this.CurrentDataItem = null;
                ClearValues();
                CurrentActionDescription = "新增";
                if (BeforeChangeMode != null)
                {
                    BeforeChangeMode(this, actionType, null);
                }
            }


        }

        void Bind()
        {
            using (Way.EntityDB.DBContext db = AppHelper.CreateLinqDataBase(this.DatabaseConfig))
            {

                foreach (System.Web.UI.WebControls.WebControl ctrl in this.ChildControls)
                {
                    string datafield = ctrl.Attributes["_DataField"];
                    if (string.IsNullOrEmpty(datafield))
                        continue;

                    if (ctrl is ListControl)
                    {
                        ListControl list = ctrl as ListControl;
                        string sql = ctrl.Attributes["_sql"];
                        if (!string.IsNullOrEmpty(sql))
                        {
                            using (var dtable = db.Database.SelectTable(sql))
                            {
                                foreach (var drow in dtable.Rows)
                                {
                                    list.Items.Add(new ListItem()
                                    {
                                        Text = drow[dtable.Columns[1].ColumnName].ToString(),
                                        Value = drow[dtable.Columns[0].ColumnName].ToString()
                                    });
                                }
                            }
                            ctrl.Attributes.Remove("_sql");
                        }
                    }
                    ctrl.DataBind();
                }


            }
        }

        protected override void Render(HtmlTextWriter writer)
        {
            base.Render(writer);
            List<Control> vdControls = AppHelper.GetControlsByTypes(this.Controls, new Type[] { typeof(BaseValidator) });
            writer.WriteLine("<script lang=\"ja\">function " + ClientID + "_Save(){" + Page.ClientScript.GetPostBackEventReference(GetPostBackOptions("save"), false) + "}");
            writer.WriteLine("</script>");
        }
        protected virtual PostBackOptions GetPostBackOptions(string argument)
        {
            PostBackOptions postBackOptions = new PostBackOptions(this, string.Empty);
            postBackOptions.ClientSubmit = false;
            postBackOptions.Argument = argument;
            if (this.Page != null)
            {
                postBackOptions.PerformValidation = true;
                postBackOptions.ValidationGroup = this.ID;
                postBackOptions.ClientSubmit = true;
            }
            return postBackOptions;
        }

        void bindModifyData()
        {

            if (this.CurrentDataItem != null)
            {
                #region 绑定修改的数据
                foreach (System.Web.UI.WebControls.WebControl ctrl in this.ChildControls)
                {
                    string datafield = ctrl.Attributes["_DataField"];
                    if (string.IsNullOrEmpty(datafield))
                        continue;
                    if (ctrl.Attributes["_Custom"] == "1")
                        continue;
                    if (ctrl is TextBox && ((TextBox)ctrl).TextMode == TextBoxMode.Password)
                        continue;

                    SetControlValue(ctrl, ((Way.EntityDB.DataItem)this.CurrentDataItem).GetValue(datafield));
                }
                #endregion
            }
        }

        internal void MakeColumns(Type tableType
            , System.ComponentModel.ITypeDescriptorContext context)
        {
            if (this.Controls.Count > 0)
                return;

            Way.EntityDB.Attributes.Table tableAtt = tableType.GetCustomAttribute(typeof(Way.EntityDB.Attributes.Table)) as Way.EntityDB.Attributes.Table;
            if (tableAtt == null)
                tableAtt = new Way.EntityDB.Attributes.Table("","");

            var properies = tableType.GetProperties();
            properies = (from m in properies
                         where m.GetCustomAttributes(typeof(System.ComponentModel.DataAnnotations.Schema.NotMappedAttribute), true).Length == 0
                         select m).ToArray();

            int columnCount = 1;
            生成几列 frm = new 生成几列();
            if (frm.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                columnCount = frm.ColumnCount;
            }



            IWebApplication webapp = (IWebApplication)context.GetService(typeof(IWebApplication));
            string currentAspPath = webapp.RootProjectItem.PhysicalPath + "\\" + WebFormsRootDesigner.DocumentUrl.Substring(2).Replace("/", "\\");

            string folder = System.IO.Path.GetDirectoryName(currentAspPath);
            string styleFolder = null;
            while (true)
            {
                if (System.IO.Directory.Exists(folder + "\\EntityEditorStyle"))
                {
                    styleFolder = folder + "\\EntityEditorStyle";
                    break;
                }
                else
                {
                    folder = System.IO.Path.GetDirectoryName(folder);
                }
            }

            List<System.Reflection.PropertyInfo> bkcolumns = new List<System.Reflection.PropertyInfo>();
            using (NeedToSearch frm2 = new NeedToSearch())
            {
                frm2.Text = "哪些列字段需要放入表单";
                List<System.Reflection.PropertyInfo> datasource = new List<System.Reflection.PropertyInfo>();

                foreach (var pinfo in properies)
                {
                    if (pinfo.Name == tableAtt.KeyName)
                    {
                        bkcolumns.Add(pinfo);
                        continue;
                    }
                    datasource.Add(pinfo);
                }
                foreach (var pinfo in datasource)
                {
                    Way.EntityDB.WayDBColumnAttribute columnAtt = pinfo.GetCustomAttribute(typeof(Way.EntityDB.WayDBColumnAttribute)) as Way.EntityDB.WayDBColumnAttribute;
                    if (columnAtt == null)
                        columnAtt = new Way.EntityDB.WayDBColumnAttribute();
                    frm2.listBox1.Items.Add(columnAtt.Caption + " - " + pinfo.Name);
                }
                
                frm2.checkBox1.Checked = true;
                frm2.ShowDialog();

                foreach (int c in frm2.listBox1.SelectedIndices)
                {
                    bkcolumns.Add(datasource[c]);
                }
            }

            System.Web.UI.HtmlControls.HtmlElement ele = new System.Web.UI.HtmlControls.HtmlElement();
            StringBuilder tableString = null;
            if (columnCount > 1)
            {
                tableString = new StringBuilder();
                tableString.AppendLine("<table border=\"0\">");
            }

            int cellindex = 0;

            StringBuilder controlContents = new StringBuilder();
            foreach (var pinfo in bkcolumns)
            {
                if (pinfo.Name == tableAtt.KeyName)
                {
                    continue;
                }

                List<ValueChange> valueselects = new List<ValueChange>();
                if (pinfo.PropertyType.IsEnum)
                {
                    string[] values = Enum.GetNames(pinfo.PropertyType);
                    foreach (string v in values)
                    {
                        valueselects.Add(new ValueChange()
                        {
                            对应输出 = v,
                            原值 = v,
                        });
                    }
                }
                string content = null;
                Way.EntityDB.WayDBColumnAttribute columnAtt = pinfo.GetCustomAttribute(typeof(Way.EntityDB.WayDBColumnAttribute)) as Way.EntityDB.WayDBColumnAttribute;
                if (columnAtt == null)
                    columnAtt = new Way.EntityDB.WayDBColumnAttribute();

                if (pinfo.PropertyType == typeof(DateTime) || pinfo.PropertyType == typeof(DateTime?))
                {
                    content = System.IO.File.ReadAllText(styleFolder + "\\date.html");
                }
                else if (valueselects.Count > 0)
                {
                    content = System.IO.File.ReadAllText(styleFolder + "\\dropdownlist.html");
                    StringBuilder itemcontent = new StringBuilder();
                    itemcontent.AppendLine("<asp:ListItem Value=\"\">请选择...</asp:ListItem>");
                    foreach (ValueChange vc in valueselects)
                    {
                        itemcontent.AppendLine(string.Format("<asp:ListItem Value=\"{0}\">{1}</asp:ListItem>", vc.原值, vc.对应输出));
                    }
                    content = content.Replace("{$itemcontent$}", itemcontent.ToString());
                }
                else
                {
                    content = System.IO.File.ReadAllText(styleFolder + "\\text.html");
                }
                content = content.Replace("{$id$}", this.ID);
                content = content.Replace("{$title$}", columnAtt.Caption);
                content = content.Replace("{$name$}", pinfo.Name);
                content = content.Replace("{$caption$}", columnAtt.Caption);
                content = content.Replace("{$divid$}", this.ClientID + "_Item_" + pinfo.Name);
                if (tableString != null)
                {
                    if (cellindex == 0)
                        tableString.AppendLine("<tr>");

                    tableString.AppendLine("<td>");
                    tableString.AppendLine(content);
                    tableString.AppendLine("</td>");
                    cellindex++;
                    if (cellindex == columnCount)
                    {
                        cellindex = 0;
                        tableString.AppendLine("</tr>");
                    }
                }
                else
                {
                    controlContents.AppendLine(content);
                }

            }
            if (cellindex > 0)
            {
                for (int i = cellindex; i < columnCount; i++)
                {
                    tableString.AppendLine("<td>&nbsp;</td>");
                }
                tableString.AppendLine("</tr>");
            }

            //System.Diagnostics.Debugger.Launch();
            string editorStyle = System.IO.File.ReadAllText(styleFolder + "\\EntityEditor.html");
            if (tableString != null)
            {
                tableString.AppendLine("</table>");
                ele.InnerHtml = editorStyle.Replace("{$clientid$}", this.ClientID).Replace("{$id$}", this.ID).Replace("{$content$}", tableString.ToString());
            }
            else
            {
                ele.InnerHtml = editorStyle.Replace("{$clientid$}", this.ClientID).Replace("{$id$}", this.ID).Replace("{$content$}", controlContents.ToString());

            }
            try
            {
                WebFormsRootDesigner.AddControlToDocument(ele, this, ControlLocation.LastChild);
            }
            catch (Exception ex)
            {
                throw (new Exception("模板的格式无法通过，如果“”里有单引号，必须用&#39;表示，\r\n具体错误：" + ex.Message));
            }
        }





        void IPostBackEventHandler.RaisePostBackEvent(string eventArgument)
        {
            Command(eventArgument);
        }
    }
}
