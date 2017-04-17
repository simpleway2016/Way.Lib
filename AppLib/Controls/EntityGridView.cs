using Way.EntityDB;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI.Design;
using System.Web.UI.WebControls;
using System.Windows.Forms;
using System.Xml;

namespace AppLib.Controls
{
    [Serializable]
    class StatisticalRec
    {
        public string DataField
        {
            get;
            set;
        }
        public double Result
        {
            get;
            set;
        }
    }
    public interface IGridViewStyle
    {
        void SetStyle(object gridview);
    }
    public class EntityGridViewDesigner : System.Web.UI.Design.WebControls.GridViewDesigner
    {
        public EntityGridViewDesigner()
        {

        }

        public override string GetDesignTimeHtml()
        {
            EntityGridView grid = ((EntityGridView)this.Component);
            grid.WebFormsRootDesigner = this.RootDesigner;

            foreach (DataControlField column in grid.Columns)
            {
                if (column is EntityGridViewColumn)
                {
                    ((EntityGridViewColumn)column)._AutoDataBindControl = grid;
                }
            }
            return base.GetDesignTimeHtml();
        }

    }
    public class AfterGetSourceArg
    {
        public object DataBase
        {
            get;
            internal set;
        }
        public object DataSource
        {
            get;
            internal set;
        }
    }

    public class GridViewGettingSourceArg
    {
        public object DataBase
        {
            get;
            internal set;
        }
        /// <summary>
        /// 是否根据你的数据源，自动生成列
        /// </summary>
        public bool AutoMakeColumns
        {
            get;
            set;
        }
    }

    [Designer(typeof(EntityGridViewDesigner))]
    public class EntityGridView : GridView, IAutoDataBindControl
    {

        Type m_dataType;
        #region 事件
        /// <summary>
        /// 
        /// </summary>
        /// <param name="grid"></param>
        /// <param name="database"></param>
        /// <param name="ids"></param>
        /// <param name="e"> </param>
        public delegate void DeleteDatasHandler(object sender, object database, string[] ids, ControlEventArg e);
        public delegate void HandleCheckHandler(object sender, string[] ids, ControlEventArg e);
        public delegate void CellDataBoundHandler(object sender, object database, TableCell cell, DataControlField column, object dataItem, ControlEventArg e);
        public delegate void RowClickHandler(object sender, int rowindex);
        public delegate IQueryable OnGettingDataSourceHandler(object sender, GridViewGettingSourceArg e);
        public delegate System.Collections.IEnumerable AfterGetDataSourceHandler(object sender, AfterGetSourceArg e);
        public delegate object GetLinqQueryHandler(object sender,object database, object query);

        [System.ComponentModel.Category("系统设定"), System.ComponentModel.Description("绑定数据时使用的query对象，用用于自定义查询")]
        public event GetLinqQueryHandler BindLinqQuery;
        [System.ComponentModel.Category("系统设定"), System.ComponentModel.Description("在搜索、SqlOrder生效后使用的query对象，用用于自定义查询")]
        public event GetLinqQueryHandler AfterBindSearchLinqQuery;
        [System.ComponentModel.Category("系统设定"), System.ComponentModel.Description("GridView获取数据源时触发，您可以return一个新的数据源，而不用GridView指定的TableName")]
        public event OnGettingDataSourceHandler GettingDataSource;
        [System.ComponentModel.Category("系统设定"), System.ComponentModel.Description("GridView获取数据源真实数据后触发，您可以return一个新的数据源")]
        public event AfterGetDataSourceHandler AfterGetDataSource;

        [System.ComponentModel.Category("系统设定"), System.ComponentModel.Description("删除选中的数据时触发")]
        public event DeleteDatasHandler DeleteDatas;
        [System.ComponentModel.Category("系统设定"), System.ComponentModel.Description("处理打勾的项")]
        public event HandleCheckHandler HandleChecked;
        [System.ComponentModel.Category("系统设定"), System.ComponentModel.Description("绑定输出每行每列的内容时触发")]
        public event CellDataBoundHandler CellDataBound;
        [System.ComponentModel.Category("系统设定"), System.ComponentModel.Description("点击行的事件")]
        public event RowClickHandler RowClick;

        #endregion

        #region 私有变量
        Way.EntityDB.DBContext Database;
        internal System.Web.UI.Design.WebFormsRootDesigner WebFormsRootDesigner;
        #endregion

        #region 属性
        [Category("外观")]
        public string RowMouseOverColor
        {
            get
            {
                if (ViewState["RowMouseOverColor"] == null)
                    return "#cfe5c9";
                else
                    return (string)ViewState["RowMouseOverColor"];
            }
            set
            {
                ViewState["RowMouseOverColor"] = value;
            }
        }
        [Category("外观")]
        public string RowMouseClickedColor
        {
            get
            {
                if (ViewState["RowMouseClickedColor"] == null)
                    return "#cfe5c9";
                else
                    return (string)ViewState["RowMouseClickedColor"];
            }
            set
            {
                ViewState["RowMouseClickedColor"] = value;
            }
        }
        public object DBContextOnBinding
        {
            get;
            private set;
        }

        List<StatisticalRec> StatisticalRecs
        {
            get
            {
                if (ViewState["StatisticalRecs"] == null)
                    ViewState["StatisticalRecs"] = new List<StatisticalRec>();
                return (List<StatisticalRec>)ViewState["StatisticalRecs"];
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

        string _TableName;
        [System.ComponentModel.Category("系统设定"), System.ComponentModel.Editor(typeof(Editor.TableNameSelector), typeof(System.Drawing.Design.UITypeEditor))]
        public string TableName
        {
            get
            {
                return _TableName;
            }
            set
            {
                if (_TableName != value)
                {
                    _TableName = value;
                }
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
                if (ViewState["IDFieldName"] != value)
                {
                    ViewState["IDFieldName"] = value;
                    if (SqlOrderby == null)
                    {
                        SqlOrderby = "[" + value + "] desc";
                    }
                }
            }
        }
        [System.ComponentModel.Category("系统设定"), System.ComponentModel.Description("页面第一次加载时，是否执行DataBind")]
        public bool BindOnFirstLoad
        {
            get
            {
                if (ViewState["BindOnFirstLoad"] == null)
                    ViewState["BindOnFirstLoad"] = true;
                return (bool)ViewState["BindOnFirstLoad"];
            }
            set
            {
                ViewState["BindOnFirstLoad"] = value;
            }
        }
        /// <summary>
        /// 获取_EntityGridView_Delete的js表示
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public string DeleteJS
        {
            get
            {
                return string.Format("_EntityGridView_Delete('{0}','{1}',false);", this.ClientID, IDFieldName);
            }
        }
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public string HandleCheckJS
        {
            get
            {
                return string.Format("_EntityGridView_HandleChecked('{0}','{1}');", this.ClientID, IDFieldName);
            }
        }
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public string DeleteJSNoAsk
        {
            get
            {
                return string.Format("_EntityGridView_Delete('{0}','{1}',true);", this.ClientID, IDFieldName);
            }
        }
        /// <summary>
        /// 获取_EntityGridView_Search的js表示
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public string SearchJS
        {
            get
            {
                return string.Format("_EntityGridView_Search(&#39;{0}&#39;);", this.ClientID);
            }
        }
        /// <summary>
        /// 获取重新绑定数据的js
        /// </summary>
        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public string ReBindDataJS
        {
            get
            {
                return string.Format("_EntityGridView_DataBind('{0}');", this.ClientID);
            }
        }
        [System.ComponentModel.Category("系统设定"), System.ComponentModel.Description("sql条件语句")]
        public string SqlTermString
        {
            get
            {
                return Convert.ToString( ViewState["SqlTermString"]);
            }
            set
            {
                if (ViewState["SqlTermString"] != value)
                {
                    ViewState["SqlTermString"] = value;
                }
            }
        }

        [System.ComponentModel.Category("系统设定"), System.ComponentModel.Description("sql order by语句")]
        public string SqlOrderby
        {
            get
            {
                return Convert.ToString(ViewState["SqlOrderby"]);
            }
            set
            {
                if (ViewState["SqlOrderby"] != value)
                {
                    ViewState["SqlOrderby"] = value;
                }
            }
        }

        [System.ComponentModel.Editor(typeof(Editor.AspNetPagerSelector), typeof(System.Drawing.Design.UITypeEditor)),
        Category("系统设定")
        ]
        public string AspNetPager
        {
            get;
            set;
        }

        [System.ComponentModel.Editor(typeof(Editor.EntityGridViewSearchSelector), typeof(System.Drawing.Design.UITypeEditor)),
       Category("系统设定")
       ]
        public string GridSearch
        {
            get;
            set;
        }


        public override bool Visible
        {
            get
            {
                return base.Visible;
            }
            set
            {
                base.Visible = value;
                try
                {
                    List<System.Web.UI.Control> pagers = AppHelper.GetControlsByTypes(this.Page, new Type[] { typeof(Wuqi.Webdiyer.AspNetPager) });
                    Wuqi.Webdiyer.AspNetPager pager = pagers.FirstOrDefault(m => m.ID == AspNetPager) as Wuqi.Webdiyer.AspNetPager;
                    if (pager != null)
                        pager.Visible = value;

                    List<System.Web.UI.Control> searchs = AppHelper.GetControlsByTypes(this.Page, new Type[] { typeof(EntityGridViewSearch) });
                    EntityGridViewSearch search = searchs.FirstOrDefault(m => m.ID == GridSearch) as EntityGridViewSearch;
                    if (search != null)
                        search.Visible = value;
                }
                catch
                {
                }
            }
        }
        #endregion

        public EntityGridView()
        {
            this.ShowHeaderWhenEmpty = true;
            this.AutoGenerateColumns = false;
            this.AllowPaging = false;

        }

        IGridViewStyle m_style;


        void pager_PageChanged(object sender, EventArgs e)
        {
            this.DataBind();
        }

        Wuqi.Webdiyer.AspNetPager _Pager;
        Wuqi.Webdiyer.AspNetPager Pager
        {
            get
            {
                if (_Pager == null && !string.IsNullOrEmpty(AspNetPager))
                {
                    List<System.Web.UI.Control> pagers = AppHelper.GetControlsByTypes(this.Page, new Type[] { typeof(Wuqi.Webdiyer.AspNetPager) });
                    _Pager = pagers.FirstOrDefault(m => m.ID == AspNetPager) as Wuqi.Webdiyer.AspNetPager;
                    if (_Pager != null)
                    {
                        _Pager.PageChanged += pager_PageChanged;
                    }
                }
                return _Pager;
            }
        }

        object _FinallyQuery;
        /// <summary>
        /// 获取最后绑定时使用的query
        /// </summary>
        /// <returns></returns>
        public object FinallyQuery
        {
            get
            {
                return _FinallyQuery;
            }
        }

        protected override void OnLoad(EventArgs e)
        {
            if (!this.DesignMode)
            {
                m_style = AppHelper.CreateInstance<IGridViewStyle>();
                if (m_style != null)
                {
                    m_style.SetStyle(this);
                }

                var mypager = this.Pager;

                if (!Page.IsPostBack)
                {
                    BindSearchItem();
                    this.DataBind();
                }

                this.WriteGridJS();

                string commandKey = string.Format("{0}_$$_command", ClientID);
                this.Page.ClientScript.RegisterHiddenField(commandKey, "");

                if (!string.IsNullOrEmpty(this.Page.Request.Form[commandKey]))
                {
                    Command(this.Page.Request.Form[commandKey]);
                }
            }
            base.OnLoad(e);
        }
        string m_orderByClick
        {
            get
            {
                return Convert.ToString(ViewState["m_orderByClick"]);
            }
            set
            {
                ViewState["m_orderByClick"] = value;
            }
        }

        void HandleCheckeds()
        {
            string fieldname = null;
            string ids = "";
            for (int i = 0; i < Columns.Count; i++)
            {
                if (this.Columns[i] is EntityCheckboxColumn)
                {
                    fieldname = "[" + (this.Columns[i] as EntityCheckboxColumn).DataField + "]";
                    ids = Page.Request.Form[this.ClientID + "_chk_" + (this.Columns[i] as EntityCheckboxColumn).DataField];
                    if (ids.Length > 0)
                        break;
                }
            }
            if (ids.EndsWith(","))
                ids = ids.Substring(0, ids.Length - 1);

            if (fieldname == null  )
                return;
            string[] arr = ids.Split(',');
            List<string> intarr = new List<string>();
            foreach (string id in arr)
            {
                if (id.Length > 0)
                    intarr.Add(id);
            }
            string[] deletingIds = intarr.ToArray();
            if (HandleChecked != null )
            {
                HandleChecked(this, deletingIds, new ControlEventArg());
            }
        }

        private void Command(string command)
        {
            if (command == "delete")
            {
                ViewState["StatisticalRecs"] = null;
                this.DeleteCheckedItems();
                this.DataBind();
            }
            if (command == "HandleChecked")
            {
                HandleCheckeds();
            }
            else if (command == "search")
            {
                ViewState["StatisticalRecs"] = null;
                if (this.Pager != null)
                    this.Pager.CurrentPageIndex = 1;
                else
                    this.DataBind();
            }
            else if (command == "rebind")
            {
                ViewState["StatisticalRecs"] = null;
                this.DataBind();
            }
            else if (command.StartsWith("click:"))
            {
                int index = Convert.ToInt32(command.Substring(6));
                if (RowClick != null)
                    RowClick(this, index);
            }
            else if (command.StartsWith("order:"))
            {
                m_orderByClick = command.Substring(6);
                if (this.Pager != null)
                    this.Pager.CurrentPageIndex = 1;
                else
                    this.DataBind();
            }
        }

        string getOrderString()
        {
            if ( !string.IsNullOrEmpty( m_orderByClick))
                return m_orderByClick;
            return SqlOrderby;
        }

        void BindSearchItem()
        {
            if (string.IsNullOrEmpty(GridSearch))
                return;
            List<System.Web.UI.Control> controls = AppHelper.GetControlsByTypes(this.Page, new Type[] { typeof(EntityGridViewSearch) });
            System.Web.UI.Control searchConatiner = controls.FirstOrDefault(m => m.ID == GridSearch);
            if (searchConatiner == null)
                return;
            List<System.Web.UI.Control> childControls = AppHelper.GetControlsByTypes(searchConatiner.Controls, new Type[] { typeof(System.Web.UI.WebControls.WebControl) });
            foreach (WebControl control in childControls)
            {
                if (control.Attributes["_Search"] == "1")
                {
                    if (control is System.Web.UI.WebControls.ListControl)
                    {
                        System.Web.UI.WebControls.ListControl list = control as System.Web.UI.WebControls.ListControl;
                        string sql = control.Attributes["_sql"];
                        if (!string.IsNullOrEmpty(sql))
                        {
                            list.Items.Add(new ListItem()
                            {
                                Text = "",
                                Value = ""
                            });
                            using (Way.EntityDB.DBContext db = AppHelper.CreateLinqDataBase(this.DatabaseConfig))
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
                            }
                        }
                    }
                }
            }
        }
        public TableCell GetCellByHeaderText(TableCell cell, string header)
        {
            var row = (GridViewRow)cell.Parent;
            return GetCellByHeaderText(row, header);
        }
        public TableCell GetCellByHeaderText(GridViewRow row, string header)
        {
            try
            {
                for (int i = 0; i < this.Columns.Count; i++)
                {
                    if (this.Columns[i].HeaderText == header)
                    {
                        return row.Cells[i];
                    }
                }
            }
            catch { }
            return null;
        }

        #region linq 查询组合
        object getLinqSearch(object query, Type tableType)
        {
            if (string.IsNullOrEmpty(GridSearch))
                return query;
            List<System.Web.UI.Control> controls = AppHelper.GetControlsByTypes(this.Page, new Type[] { typeof(EntityGridViewSearch) });
            System.Web.UI.Control searchConatiner = controls.FirstOrDefault(m => m.ID == GridSearch);
            if (searchConatiner == null)
                throw new Exception("can't find EntityGridViewSearch with id:" + GridSearch);

            ParameterExpression param = Expression.Parameter(tableType, "n");
            Type queryableType = typeof(System.Linq.Queryable);

            List<System.Web.UI.Control> childControls = AppHelper.GetControlsByTypes(searchConatiner.Controls, new Type[] { typeof(System.Web.UI.WebControls.WebControl) });
            Expression totalExpression = null;
            foreach (WebControl control in childControls)
            {
                if (control.Attributes["_Search"] == "1")
                {
                    string value;
                    value = Convert.ToString(EntityEditor.GetControlValue(control));
                    if (value != null)
                        value = value.Trim();
                    if (string.IsNullOrEmpty(value))
                        continue;

                    PropertyInfo pinfo;
                    Expression left = DBContext.GetPropertyExpression(param, tableType, control.Attributes["_DataField"], out pinfo);
                    Type ptype = pinfo.PropertyType;
                   
                    if (ptype.IsGenericType)
                    {
                        ptype = ptype.GetGenericArguments()[0];
                        left = Expression.Convert(left, ptype);
                    }
                    if (ptype == typeof(int) || ptype == typeof(double) || ptype == typeof(decimal) || ptype == typeof(float) || ptype == typeof(short) || ptype == typeof(long))
                    {
                        if (value.StartsWith(">="))
                        {
                            value = value.Replace(">=", "");
                            //等式右边的值
                            Expression right = Expression.Constant(Convert.ChangeType(value, ptype));
                            left = Expression.GreaterThanOrEqual(left, right);
                        }
                        else if (value.StartsWith(">"))
                        {
                            value = value.Replace(">", "");
                            //等式右边的值
                            Expression right = Expression.Constant(Convert.ChangeType(value, ptype));
                            left = Expression.GreaterThan(left, right);
                        }
                        else if (value.StartsWith("<="))
                        {
                            value = value.Replace("<=", "");
                            //等式右边的值
                            Expression right = Expression.Constant(Convert.ChangeType(value, ptype));
                            left = Expression.LessThanOrEqual(left, right);
                        }
                        else if (value.StartsWith("<"))
                        {
                            value = value.Replace("<", "");
                            //等式右边的值
                            Expression right = Expression.Constant(Convert.ChangeType(value, ptype));
                            left = Expression.LessThan(left, right);
                        }
                        else
                        {
                            //等式右边的值
                            Expression right = Expression.Constant(Convert.ChangeType(value, ptype));
                            left = Expression.Equal(left, right);
                        }
                    }
                    else if (control is System.Web.UI.WebControls.ListControl || control is TextBoxList || control is ParentToChildSelector)
                    {
                        if (ptype.IsEnum)
                        {
                            //等式右边的值
                            Expression right = Expression.Constant(Enum.Parse(ptype , value));
                            left = Expression.Equal(left, right);
                        }
                        else
                        {
                            //等式右边的值
                            Expression right = Expression.Constant(Convert.ChangeType(value, ptype));
                            left = Expression.Equal(left, right);
                        }
                       

                    }
                    else if (control.Attributes["_IsDate"] == "1")
                    {
                        if (control.Attributes["_IsFrom"] == "1")
                        {
                            if (control.Attributes["_IsTo"] == "1")
                            {
                                //应该是指定月份，或者年份
                                var ms = System.Text.RegularExpressions.Regex.Matches(value, @"[0-9]+");
                                if (ms.Count == 1)
                                {
                                    //只有年份
                                    Expression expression1;
                                    
                                    //等式右边的值
                                    value = string.Format("{0}-1-1", ms[0]);
                                    Expression right = Expression.Constant(Convert.ChangeType(value, ptype));
                                    expression1 = Expression.GreaterThanOrEqual(left, right);

                                    value = string.Format("{0}-1-1", ms[0].Value.ToInt() + 1);
                                    right = Expression.Constant(Convert.ChangeType(value, ptype));
                                    right = Expression.LessThan(left, right);

                                    left = Expression.And(expression1, right);
                                }
                                else if (ms.Count == 2)
                                {
                                    //有月份
                                    DateTime monthdate = Convert.ToDateTime(value);

                                    Expression expression1;
                                    //等式右边的值
                                    value = monthdate.ToString("yyyy-MM-01");
                                    Expression right = Expression.Constant(Convert.ChangeType(value, ptype));
                                    expression1 = Expression.GreaterThanOrEqual(left, right);

                                    value = monthdate.AddMonths(1).ToString("yyyy-MM-01");
                                    right = Expression.Constant(Convert.ChangeType(value, ptype));
                                    right = Expression.LessThan(left, right);

                                    left = Expression.And(expression1, right);
                                }
                                else if (ms.Count == 3)
                                {
                                    //有日
                                    DateTime day = Convert.ToDateTime(value);
                                    Expression expression1;
                                    //等式右边的值
                                    value = day.ToString("yyyy-MM-dd");
                                    Expression right = Expression.Constant(Convert.ChangeType(value, ptype));
                                    expression1 = Expression.GreaterThanOrEqual(left, right);

                                    value = day.AddDays(1).ToString("yyyy-MM-dd");
                                    right = Expression.Constant(Convert.ChangeType(value, ptype));
                                    right = Expression.LessThan(left, right);

                                    left = Expression.AndAlso(expression1, right);
                                }
                            }
                            else
                            {
                                //等式右边的值
                                Expression right = Expression.Constant(Convert.ChangeType(value, ptype));
                                left = Expression.GreaterThanOrEqual(left, right);
                            }
                        }
                        else
                        {
                            if (value.Contains(":"))
                            {
                                value = value.ToDateTime().AddSeconds(1).ToString("yyyy-MM-dd HH:mm:ss");
                            }
                            else
                            {
                                value = value.ToDateTime().ToString("yyyy-MM-dd").ToDateTime().AddDays(1).ToString("yyyy-MM-dd");
                            }
                            //等式右边的值
                            Expression right = Expression.Constant(Convert.ChangeType(value, ptype));
                            left = Expression.LessThan(left, right);
                        }
                    }
                    else
                    {
                        left = Expression.Call(left,
                            typeof(string).GetMethod("Contains", new Type[] { typeof(string) }),
                            Expression.Constant(value));

                    }
                    if (left != null)
                    {
                        //left = Expression.Lambda(left, param);
                        if (totalExpression == null)
                            totalExpression = left;
                        else
                            totalExpression = Expression.AndAlso(totalExpression, left);
                    }
                }
            }
            if (totalExpression != null)
            {
                totalExpression = Expression.Lambda(totalExpression, param);
                var methods = queryableType.GetMethods(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static).Where(m => m.Name == "Where");
                foreach (System.Reflection.MethodInfo method in methods)
                {
                    try
                    {
                        System.Reflection.MethodInfo mmm = method.MakeGenericMethod(tableType);
                        query = mmm.Invoke(null, new object[] { query, totalExpression });
                        break;
                    }
                    catch (Exception ex)
                    {
                    }
                }
            }
            return query;
        }
        #endregion

        /// <summary>
        /// 删除checked的项
        /// </summary>
        public void DeleteCheckedItems()
        {
            string fieldname = null;
            string ids = "";
            for (int i = 0; i < Columns.Count; i++)
            {
                if (this.Columns[i] is EntityCheckboxColumn)
                {
                    fieldname = "[" + (this.Columns[i] as EntityCheckboxColumn).DataField + "]";
                    ids = Page.Request.Form[this.ClientID + "_chk_" + (this.Columns[i] as EntityCheckboxColumn).DataField];
                    if (ids == null)
                    {
                        if (!Page.ClientScript.IsStartupScriptRegistered(Page.GetType(), "__rygridview$$alert"))
                            Page.ClientScript.RegisterStartupScript(Page.GetType(), "__rygridview$$alert", "<script lang=\"javascript\">alert('请选择需要删除的数据！');</script>\r\n");
                        return;
                    }
                    if (ids.Length > 0)
                        break;
                }
            }
            if (ids.EndsWith(","))
                ids = ids.Substring(0, ids.Length - 1);

            if (fieldname == null || ids.Length == 0)
                return;
            string[] arr = ids.Split(',');
            List<string> intarr = new List<string>();
            foreach (string id in arr)
            {
                if (id.Length > 0)
                    intarr.Add(id);
            }
            string[] deletingIds = intarr.ToArray();
            ControlEventArg arg = new ControlEventArg();
            using (Way.EntityDB.DBContext db = AppHelper.CreateLinqDataBase(this.DatabaseConfig))
            {
                if (DeleteDatas != null)
                {
                    DeleteDatas(this, db, deletingIds, arg);
                }
                if (!arg.Handle)
                {
                    object query = OnGettingDataSource(db);
                    m_dataType = query.GetType().GetGenericArguments()[0];
                    string pkid = null;
                    if (m_dataType != null)
                    {
                        try
                        {
                            object[] atts = m_dataType.GetCustomAttributes(typeof(Way.EntityDB.Attributes.Table), true);
                            pkid = ((Way.EntityDB.Attributes.Table)atts[0]).IDField;
                        }
                        catch
                        {
                        }
                    }
                    if (pkid != null)
                    {
                        foreach (string id in deletingIds)
                        {
                            if (id.Length > 0)
                            {
                                var dateitemquery = DBContext.InvokeWhereEquals(query, pkid, id);
                                var dataitem = DBContext.InvokeFirstOrDefault(dateitemquery);
                                if (dataitem != null)
                                {
                                    db.Delete(dataitem as Way.EntityDB.DataItem);
                                }
                            }
                        }
                    }
                }
            }
        }

        

        protected virtual object OnGettingDataSource(object db)
        {
            object result = null;
            if (GettingDataSource != null)
            {
                GridViewGettingSourceArg arg = new GridViewGettingSourceArg()
                {
                    DataBase = db,
                    AutoMakeColumns = false,
                };
                result = GettingDataSource(this, arg);
                if (result != null)
                {
                    if (arg.AutoMakeColumns)
                    {
                        this.Columns.Clear();
                        try
                        {
                            Type dataType = result.GetType().GetGenericArguments()[0];
                            System.Reflection.PropertyInfo[] pinfos = dataType.GetProperties();
                            foreach (System.Reflection.PropertyInfo pinfo in pinfos)
                            {
                                var att = pinfo.GetCustomAttribute(typeof(System.ComponentModel.BrowsableAttribute)) as System.ComponentModel.BrowsableAttribute;
                                if (att != null && att.Browsable == false)
                                    continue;
                                EntityGridViewColumn column = new EntityGridViewColumn();
                                column.HeaderText = pinfo.Name;
                                column.DataField = pinfo.Name;
                                if (pinfo.PropertyType == typeof(decimal) || pinfo.PropertyType == typeof(decimal?))
                                    column.DataFormatString = "{0:0.00}";
                                this.Columns.Add(column);
                            }
                        }
                        catch
                        {
                        }
                        if (m_style != null)
                        {
                            m_style.SetStyle(this);
                        }
                    }
                    return result;
                }
            }

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

            return queryPinfo.GetValue(db);
        }

        

        protected virtual void LinqBind()
        {
            using (Way.EntityDB.DBContext db = AppHelper.CreateLinqDataBase(this.DatabaseConfig))
            {
                this.DBContextOnBinding = db;
                object query = OnGettingDataSource(db);
                if (query == null)
                    throw new Exception("query is null");

                m_dataType = query.GetType().GetGenericArguments()[0];

                string pkid = null;
                if (m_dataType != null && this.IDFieldName.IsNullOrEmpty())
                {
                    try
                    {
                        object[] atts = m_dataType.GetCustomAttributes(typeof(Way.EntityDB.Attributes.Table), true);
                        pkid = ((Way.EntityDB.Attributes.Table)atts[0]).IDField;
                    }
                    catch
                    {
                    }
                }
                else if (this.IDFieldName.IsNullOrEmpty() == false)
                    pkid = this.IDFieldName;


                if (true)
                {
                    string termstring = this.SqlTermString;
                    if (!string.IsNullOrEmpty(termstring))
                    {
                        query = Way.EntityDB.DBContext.GetQueryByString(query, termstring);
                        if (query == null)
                            throw new Exception("GetQueryByString return null");
                    }
                }


                
               
                //string termstring = GetCurrentTermString();
                if (BindLinqQuery != null)
                {
                    object resultquery = BindLinqQuery(this, db , query);
                    if (resultquery != null)
                    {
                        query = resultquery;
                        //这是dataType可能已经发生变化，所以重新获取
                        m_dataType = query.GetType().GetGenericArguments()[0];
                    }
                }

                //getLinqSearch GetQueryForOrderBy必须放在
                //BindLinqQuery后面，因为BindLinqQuery有可能返回的是另外一种Type的query，
                //如果getLinqSearch放前面，只会用gridview的m_dataType去做属性匹配
                query = getLinqSearch(query, m_dataType);
                if (query == null)
                    throw new Exception("getLinqSearch return null");

                string order = getOrderString();
               
                if (!string.IsNullOrEmpty(order))
                {
                    query = Way.EntityDB.DBContext.GetQueryForOrderBy(query, order);
                }

                if (AfterBindSearchLinqQuery != null)
                {
                    object resultquery = AfterBindSearchLinqQuery(this, db, query);
                    if (resultquery != null)
                    {
                        query = resultquery;
                        //这是dataType可能已经发生变化，所以重新获取
                        m_dataType = query.GetType().GetGenericArguments()[0];
                    }
                }

                if (ViewState["StatisticalRecs"] == null)
                {
                    for (int i = 0; i < this.Columns.Count; i++)
                    {
                        EntityGridViewColumn column = this.Columns[i] as EntityGridViewColumn;
                        if (column != null && column.Statistical && !string.IsNullOrEmpty(column.DataField))
                        {
                            var result = DBContext.InvokeSelect(query, column.DataField);
                            result = DBContext.InvokeSum(result);
                            if (result != null)
                            {
                                this.StatisticalRecs.Add(new StatisticalRec()
                                    {
                                        DataField = column.DataField,
                                        Result = Convert.ToDouble(result),
                                    });
                            }
                        }
                    }
                }
            __checkagain:

                _FinallyQuery = query;
                if (this.Pager == null)
                {

                }
                else
                {
                    Type queryType = typeof(System.Linq.Queryable);
                    var methods = queryType.GetMethods(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static).Where(m => m.Name == "Count");
                    foreach (System.Reflection.MethodInfo method in methods)
                    {
                        if (method.GetParameters().Length == 1)
                        {
                            System.Reflection.MethodInfo mmm = method.MakeGenericMethod(m_dataType);
                            if (mmm != null)
                            {
                                this.Pager.RecordCount = Convert.ToInt32(mmm.Invoke(null, new object[] { query }));
                                if(Pager.ShowCustomInfoSection == Wuqi.Webdiyer.ShowCustomInfoSection.Never)
                                Pager.ShowCustomInfoSection = Wuqi.Webdiyer.ShowCustomInfoSection.Left;
                                this.Pager.CustomInfoHTML = " 总记录数:<b>" + this.Pager.RecordCount + "</b>";
                                break;
                            }
                        }
                    }

                   
                    methods = queryType.GetMethods(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static).Where(m => m.Name == "Skip");
                    foreach (System.Reflection.MethodInfo method in methods)
                    {
                        System.Reflection.MethodInfo mmm = method.MakeGenericMethod(m_dataType);
                        if (mmm != null)
                        {
                            try
                            {
                                var skipquery = mmm.Invoke(null, new object[] { query, (this.Pager.CurrentPageIndex - 1) * this.Pager.PageSize });
                                skipquery.ToString();
                                query = skipquery;
                            }
                            catch (Exception ex)
                            {
                                if (string.IsNullOrEmpty(order) && pkid.IsNullOrEmpty() == false)
                                {
                                    query = DBContext.GetQueryForOrderBy(query, pkid);
                                    query = mmm.Invoke(null, new object[] { query, (this.Pager.CurrentPageIndex - 1) * this.Pager.PageSize });
                                }
                                else
                                    throw ex;
                            }
                            break;
                        }
                    }
                    methods = queryType.GetMethods(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static).Where(m => m.Name == "Take");
                    foreach (System.Reflection.MethodInfo method in methods)
                    {
                        System.Reflection.MethodInfo mmm = method.MakeGenericMethod(m_dataType);
                        if (mmm != null)
                        {
                            if (BindOnFirstLoad == false && Page.IsPostBack == false)
                            {
                                query = mmm.Invoke(null, new object[] { query, 0 });
                            }
                            else
                            {
                                query = mmm.Invoke(null, new object[] { query, this.Pager.PageSize });
                            }
                            break;
                        }
                    }


                    if (this.Pager.CurrentPageIndex > 1 &&
                        (this.Pager.CurrentPageIndex - 1) * this.Pager.PageSize >= this.Pager.RecordCount)
                    {
                        this.Pager.CurrentPageIndex--;
                        goto __checkagain;
                    }
                }


                Database = db;
                if (query != null)
                {
                    var methods = typeof(System.Linq.Enumerable).GetMethods(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static).Where(m => m.Name == "ToList");
                    foreach (System.Reflection.MethodInfo method in methods)
                    {
                        System.Reflection.MethodInfo mmm = method.MakeGenericMethod(m_dataType);
                        if (mmm != null)
                        {
                            DataSource = mmm.Invoke(null, new object[] { query });
                            if (AfterGetDataSource != null)
                            {
                                var newdatasource = AfterGetDataSource(this, new AfterGetSourceArg()
                                {
                                    DataBase = db,
                                    DataSource = DataSource,
                                });
                                if (newdatasource != null)
                                {
                                    DataSource = newdatasource;
                                }
                            }
                            break;
                        }
                    }
                }
                base.DataBind();

            }
            this.DBContextOnBinding = null;
        }

        public new void DataBind()
        {
            if (string.IsNullOrEmpty(DatabaseConfig))
            {
                throw new Exception(this.DatabaseConfig + "未设置对应的Linq数据库类，请设置DatabaseConfig属性！");
            }
            LinqBind();
        }

        void doStatistical(Way.EntityDB.DBContext db, TableCell cell, DataControlField column, object dataItem)
        {
            if (column is EntityGridViewColumn)
            {
                EntityGridViewColumn entityColumn = (EntityGridViewColumn)column;
                if (entityColumn.Statistical == false)
                    return;
                try
                {
                    var query = this.StatisticalRecs.FirstOrDefault(m => m.DataField == entityColumn.DataField).Result;
                    if (!string.IsNullOrEmpty(entityColumn.DataFormatString))
                    {
                        cell.Text = string.Format(entityColumn.DataFormatString, query);
                    }
                    else
                    {
                        cell.Text = Convert.ToString(query);
                    }
                }
                catch
                {
                }
            }
        }

        protected virtual void OnCellDataBound(Way.EntityDB.DBContext db, TableCell cell, DataControlField column, object dataItem)
        {
            object cellValue = null;
            if (column is BoundField)
            {

                try
                {
                    BoundField bf = column as BoundField;
                    cellValue = Way.EntityDB.DataItem.GetValue(dataItem, bf.DataField);
                    if (cellValue == null)
                    {
                        cell.Text = "&nbsp;";
                    }
                }
                catch(Exception ex)
                {
                }
            }

            if (CellDataBound != null)
            {
                ControlEventArg arg = new ControlEventArg();
                CellDataBound(this, db, cell, column, dataItem, arg);
                if (arg.Handle)
                    return;
            }
            EntityGridViewColumn rycolumn = column as EntityGridViewColumn;
            if (rycolumn == null)
                return;

            if (rycolumn.ValueChanges.Count > 0)
            {
                if (cellValue == null)
                    cellValue = "";

                //值转换
                string value = Convert.ToString(cellValue);
                ValueChange curVC = rycolumn.ValueChanges.FirstOrDefault(m => m.原值 == value);
                if (curVC == null)
                    curVC = rycolumn.ValueChanges.FirstOrDefault(m => string.IsNullOrEmpty(m.原值));
                if (curVC != null)
                {
                    if (curVC.对应输出 != null && curVC.对应输出.Contains("<%#DataBinder.Eval(Container.DataItem"))//<%#DataBinder.Eval(Container.DataItem, "ProjectId") %>
                    {
                        try
                        {
                            string outputtext = curVC.对应输出;
                            while (true)
                            {
                                int index = outputtext.IndexOf("<%#DataBinder.Eval(Container.DataItem");
                                string text = outputtext.Substring(0, index);

                                outputtext = outputtext.Substring(index);
                                string bindingStr = outputtext.Substring(0, outputtext.IndexOf(">"));
                                string bindingDataField = bindingStr.Substring(bindingStr.IndexOf("\"") + 1);
                                bindingDataField = bindingDataField.Substring(0, bindingDataField.IndexOf("\""));

                                string bindingvalue = "";
                                if (dataItem is Way.EntityDB.DataItem)
                                {
                                    bindingvalue = Convert.ToString(((Way.EntityDB.DataItem)dataItem).GetValue(bindingDataField));

                                }

                                outputtext = outputtext.Substring(outputtext.IndexOf("\"") + 1);
                                string datafield = outputtext.Substring(0, outputtext.IndexOf("\""));
                                outputtext = outputtext.Substring(outputtext.IndexOf("%>") + 2);
                                text = text + bindingvalue + outputtext;
                                if (text.Contains("<%#DataBinder.Eval(Container.DataItem"))
                                {
                                    outputtext = text;
                                }
                                else
                                {
                                    cell.Text = text;
                                    break;
                                }
                            }
                        }
                        catch
                        {
                        }
                    }
                    else
                    {
                        cell.Text = curVC.对应输出;
                    }
                }
                return;
            }

            if (string.IsNullOrEmpty(rycolumn.KeyTableName) || string.IsNullOrEmpty(rycolumn.KeyIDField) || string.IsNullOrEmpty(rycolumn.KeyTextField))
                return;
            if (cellValue == null)
                cellValue = "";
            string idstr = cellValue.ToString();
            if (idstr.Length > 0)
            {
                Type dbtype = db.GetType();
                var query = dbtype.GetProperty(rycolumn.KeyTableName).GetValue(db);
                query = DBContext.InvokeWhereEquals(query, rycolumn.KeyIDField, cellValue);
                query = DBContext.InvokeSelect(query, rycolumn.KeyTextField);
                object text = DBContext.InvokeFirstOrDefault(query );
                if (text != null)
                {
                    cell.Text = text.ToString();
                }
                else
                {
                    cell.Text = "";
                }
            }
            else
            {
                cell.Text = "";
            }
        }

        /// <summary>
        /// 生成js文件
        /// </summary>
        private void WriteGridJS()
        {
            AppHelper.RegisterJquery(this.Page);
            if (!this.Page.ClientScript.IsClientScriptIncludeRegistered("__EntityGridView_JSFile"))
            {
                Page.ClientScript.RegisterClientScriptInclude("__EntityGridView_JSFile",
             Page.ClientScript.GetWebResourceUrl(this.GetType(),
                                         "AppLib.js.EntityGridView.js"));
            }
            if (!this.Page.ClientScript.IsClientScriptIncludeRegistered("_WdatePicker_JS"))
            {
                Page.ClientScript.RegisterClientScriptInclude("_WdatePicker_JS", "/inc/My97DatePicker/WdatePicker.js");
            }

        }
        protected override void Render(System.Web.UI.HtmlTextWriter writer)
        {
            base.Render(writer);
            writer.WriteLine();
            writer.WriteLine(" <script language=\"JavaScript\">");
            writer.WriteLine(string.Format("_EntityGridView_SetStyle('{0}','{1}','{2}');", this.ClientID, this.RowMouseClickedColor, this.RowMouseOverColor));
            writer.WriteLine("</script>");
        }
        protected override void OnRowDataBound(GridViewRowEventArgs e)
        {
            if (Database != null)
            {
                if (e.Row.RowType == DataControlRowType.Pager)
                {

                }
                else if (e.Row.RowType == DataControlRowType.Header)
                {
                    e.Row.Attributes["_RowType"] = "Header";
                    for (int i = 0; i < Columns.Count && i < e.Row.Cells.Count; i++)
                    {

                        if (this.Columns[i] is EntityCheckboxColumn)
                        {
                            e.Row.Cells[i].Text = "<input onclick=\"_EntityGridView_HeaderCheck(this , '" + this.ClientID + "_chk_" + (this.Columns[i] as EntityCheckboxColumn).DataField + "')\" type=\"checkbox\"/>";
                        }
                        else if (this.Columns[i] is BoundField)
                        {
                            e.Row.Cells[i].Style["cursor"] = "pointer";
                            BoundField column = this.Columns[i] as BoundField;

                            if (!string.IsNullOrEmpty(column.DataField))
                            {
                                if (m_orderByClick == column.DataField)
                                {
                                    e.Row.Cells[i].Text += "&nbsp;↑";
                                    e.Row.Cells[i].Attributes["onclick"] = "_EntityGridView_Order('" + this.ClientID + "','" + column.DataField + " desc')";
                                }
                                else if (m_orderByClick == column.DataField + " desc")
                                {
                                    e.Row.Cells[i].Attributes["onclick"] = "_EntityGridView_Order('" + this.ClientID + "','" + column.DataField + "')";
                                    e.Row.Cells[i].Text += "&nbsp;↓";
                                }
                                else
                                {
                                    e.Row.Cells[i].Attributes["onclick"] = "_EntityGridView_Order('" + this.ClientID + "','" + column.DataField + "')";
                                }
                            }
                        }
                    }
                }
                else if (e.Row.RowType == DataControlRowType.Footer)
                {
                    e.Row.Attributes["_RowType"] = "Footer";
                    for (int i = 0; i < Columns.Count; i++)
                    {
                        doStatistical(Database, e.Row.Cells[i], Columns[i], e.Row.DataItem);
                    }
                }
                else if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    e.Row.Attributes["_RowType"] = "DataRow";
                    if (e.Row.DataItem != null)
                    {
                        for (int i = 0; i < Columns.Count; i++)
                        {
                            if (this.Columns[i] is EntityCheckboxColumn)
                            {
                                
                                e.Row.Cells[i].Text = "<input name=\"" + this.ClientID + "_chk_" + (this.Columns[i] as EntityCheckboxColumn).DataField + "\" value=\"" + DBContext.GetPropertyValue(e.Row.DataItem  , (this.Columns[i] as EntityCheckboxColumn).DataField)  + "\" type=\"checkbox\"/>";
                               
                            }
                            OnCellDataBound(Database, e.Row.Cells[i], Columns[i], e.Row.DataItem);
                        }
                    }
                    if (RowClick != null)
                    {
                        e.Row.Attributes.Add("onclick", "_EntityGridView_RowClick('" + this.ClientID + "' , " + e.Row.RowIndex + ")");
                    }
                }
            }
            else
            {
                for (int i = 0; i < Columns.Count; i++)
                {
                    if (this.Columns[i] is EntityCheckboxColumn)
                    {
                        e.Row.Cells[i].Text = "<input type=\"checkbox\"/>";
                    }
                }
            }
            base.OnRowDataBound(e);
        }

        internal void MakeSearch(Type tableType , Way.EntityDB.Attributes.Table tableAtt , System.Reflection.PropertyInfo[] columns
            , System.ComponentModel.ITypeDescriptorContext context)
        {
            //System.Diagnostics.Debugger.Launch();
            if (string.IsNullOrEmpty(GridSearch))
            {
                return;
            }

            Type t = typeof(EntityGridViewSearch);
            List<System.Web.UI.Control> controls = new List<System.Web.UI.Control>();
            foreach (IComponent component in ((System.Web.UI.Control)context.Instance).Site.Container.Components)
            {
                Type ctrltype = component.GetType();
                if (ctrltype == t || ctrltype.IsSubclassOf(t))
                {
                    controls.Add(component as System.Web.UI.Control);
                }
            }

            System.Web.UI.Control searchControl = controls.FirstOrDefault(m => m.ID == GridSearch) as System.Web.UI.Control;
            if (searchControl == null)
            {
                return;
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
            using (NeedToSearch frm = new NeedToSearch())
            {
                frm.Text = "哪些列需要搜索";
                List<System.Reflection.PropertyInfo> datasource = new List<System.Reflection.PropertyInfo>();

                foreach (var pinfo in columns)
                {
                    if (pinfo.Name == tableAtt.IDField)
                    {
                        continue;
                    }
                    datasource.Add(pinfo);
                }
                foreach (var pinfo in datasource)
                {
                    Way.EntityDB.WayDBColumnAttribute columnAtt = pinfo.GetCustomAttribute(typeof(Way.EntityDB.WayDBColumnAttribute)) as Way.EntityDB.WayDBColumnAttribute;
                    if (columnAtt == null)
                        columnAtt = new WayDBColumnAttribute();

                    frm.listBox1.Items.Add(columnAtt.Caption + " - " + pinfo.Name);
                }

                frm.checkBox1.Checked = true;
                if (frm.ShowDialog() != DialogResult.OK)
                    return;
                foreach (int c in frm.listBox1.SelectedIndices)
                {
                    bkcolumns.Add(datasource[c]);
                }
            }
            if (bkcolumns.Count == 0)
                return;

            StringBuilder control_contents = new StringBuilder();
            foreach (var pinfo in bkcolumns)
            {
                if (pinfo.Name == tableAtt.IDField)
                    continue;
                Way.EntityDB.WayDBColumnAttribute columnAtt = pinfo.GetCustomAttribute(typeof(Way.EntityDB.WayDBColumnAttribute)) as Way.EntityDB.WayDBColumnAttribute;
                if (columnAtt == null)
                    columnAtt = new WayDBColumnAttribute();

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

                if (pinfo.PropertyType == typeof(DateTime) || pinfo.PropertyType == typeof(DateTime?))
                {
                    content = System.IO.File.ReadAllText(styleFolder + "\\search\\search_date.html");
                }
                else if (valueselects.Count > 0)
                {
                    content = System.IO.File.ReadAllText(styleFolder + "\\search\\search_dropdownlist.html");
                    StringBuilder itemcontent = new StringBuilder();
                    itemcontent.AppendLine("<asp:ListItem Value=\"\"></asp:ListItem>");
                    foreach (ValueChange vc in valueselects)
                    {
                        itemcontent.AppendLine(string.Format("<asp:ListItem Value=\"{0}\">{1}</asp:ListItem>", vc.原值, vc.对应输出));
                    }
                    content = content.Replace("{$itemcontent$}", itemcontent.ToString());
                }
                else
                {
                    content = System.IO.File.ReadAllText(styleFolder + "\\search\\search_text.html");
                }
                content = content.Replace("{$title$}", columnAtt.Caption);
                content = content.Replace("{$name$}", pinfo.Name);

                control_contents.AppendLine(content);
            }
            string bodystring = System.IO.File.ReadAllText(styleFolder + "\\search\\search.html");
            System.Web.UI.HtmlControls.HtmlElement searchBody = new System.Web.UI.HtmlControls.HtmlElement();
            searchBody.InnerHtml = bodystring.Replace("{$clientid$}", this.ClientID).Replace("{$content$}", control_contents.ToString());
            WebFormsRootDesigner.AddControlToDocument(searchBody, searchControl, ControlLocation.LastChild);
        }
        internal void MakeColumns(Type tableType,  System.ComponentModel.ITypeDescriptorContext context)
        {

            Way.EntityDB.Attributes.Table tableAtt = tableType.GetCustomAttribute(typeof(Way.EntityDB.Attributes.Table)) as Way.EntityDB.Attributes.Table;
            if (tableAtt == null)
                tableAtt = new Way.EntityDB.Attributes.Table("","");

            var properies = tableType.GetProperties();
            properies = (from m in properies
                             where m.GetCustomAttributes(typeof(System.ComponentModel.DataAnnotations.Schema.NotMappedAttribute), true).Length == 0
                             select m).ToArray();

            context.OnComponentChanging();
            //IServiceProvider service1 = this.Site;
            //IComponentChangeService sv = (IComponentChangeService)service1.GetService(typeof(IComponentChangeService));
            //sv.OnComponentChanging(this, null);

            List<System.Reflection.PropertyInfo> bkcolumns = new List<System.Reflection.PropertyInfo>();
            using (NeedToSearch frm = new NeedToSearch())
            {
                frm.Text = "需要在Grid显示哪些列";
                List<System.Reflection.PropertyInfo> datasource = new List<System.Reflection.PropertyInfo>();

                foreach (var pinfo in properies)
                {
                    if (pinfo.Name == tableAtt.IDField)
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
                        columnAtt = new WayDBColumnAttribute();

                    frm.listBox1.Items.Add(columnAtt.Caption + " - " + pinfo.Name);
                }
                frm.checkBox1.Checked = true;
                frm.ShowDialog();

                foreach (int c in frm.listBox1.SelectedIndices)
                {
                    bkcolumns.Add(datasource[c]);
                }
            }

            foreach (var pinfo in bkcolumns)
            {
                Way.EntityDB.WayDBColumnAttribute columnAtt = pinfo.GetCustomAttribute(typeof(Way.EntityDB.WayDBColumnAttribute)) as Way.EntityDB.WayDBColumnAttribute;
                if (columnAtt == null)
                    columnAtt = new WayDBColumnAttribute();

                if (pinfo.Name == tableAtt.IDField )
                {
                    if (this.Columns.Count == 0)
                    {
                        EntityCheckboxColumn chkColumn = new EntityCheckboxColumn();
                        chkColumn.DataField = pinfo.Name;
                        this.Columns.Add(chkColumn);
                    }
                    continue;
                }

                EntityGridViewColumn column = new EntityGridViewColumn();
                column._AutoDataBindControl = this;
                column.DataField = pinfo.Name;
                column.HeaderText = columnAtt.Caption;

                if (pinfo.PropertyType == typeof(decimal) || pinfo.PropertyType == typeof(decimal?))
                {
                    column.DataFormatString = "{0:N2}";
                }
                this.Columns.Add(column);
            }


            context.OnComponentChanged();

            MakeSearch(tableType, tableAtt, properies, context);
        }
    }
}
