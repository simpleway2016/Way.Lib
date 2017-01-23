using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Controls;
using Way.Lib.VSIX.Extend.AppCodeBuilder.Editors;

namespace Way.Lib.VSIX.Extend.AppCodeBuilder
{
    public class WayGridViewBuilder : IAppCodeBuilder,IDataControl
    {
        Impls.WayGridView.Control _control;
        public WayGridViewBuilder()
        {
            this.ControlId = "grid1";
            this.PageSize = 20;
            _control = new Impls.WayGridView.Control(this);
        }

        public string ControlId
        {
            get;
            set;
        }
        [System.ComponentModel.Description("每次加载数据量，0表示一次性全部加载")]
        public int PageSize
        {
            get;
            set;
        }
        [System.ComponentModel.Description("是否允许编辑数据")]
        public bool AllowEdit
        {
            get;
            set;
        }
        [System.ComponentModel.Description("显示搜索区")]
        public bool ShowSearchArea
        {
            get;
            set;
        }
       
     
        [System.ComponentModel.Category("数据源"), System.ComponentModel.Editor(typeof(DatabaseSelector) , typeof(System.Drawing.Design.UITypeEditor))]
        public ValueDescription DBContext
        {
            get;
            set;
        }

        ValueDescription _Table;
        [System.ComponentModel.Category("数据源"), System.ComponentModel.Editor(typeof(TableSelector), typeof(System.Drawing.Design.UITypeEditor))]
        public ValueDescription Table
        {
            get
            {
                return _Table;
            }
            set
            {
                _Table = value;
                _control.OnDatasourceChanged();
            }
        }

        [System.ComponentModel.Browsable(false)]
        public UserControl ViewControl
        {
            get
            {
                return _control;
            }
        }

       
    }
}
