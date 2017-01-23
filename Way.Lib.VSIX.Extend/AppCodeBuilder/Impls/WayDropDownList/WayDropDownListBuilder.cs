using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using Way.Lib.VSIX.Extend.AppCodeBuilder.Editors;

namespace Way.Lib.VSIX.Extend.AppCodeBuilder.Impls.WayDropDownList
{
    public class WayDropDownListBuilder : IAppCodeBuilder, IDataControl
    {
        Impls.WayDropDownList.Control _Control;
        [System.ComponentModel.Browsable(false)]
        public UserControl ViewControl
        {
            get
            {
                return _Control;
            }
        }

        public WayDropDownListBuilder()
        {
            _Control = new WayDropDownList.Control(this);
            this.ControlId = "drpList1";
        }

        public string ControlId
        { get; set; }
        [System.ComponentModel.Category("数据源"), System.ComponentModel.Editor(typeof(DatabaseSelector), typeof(System.Drawing.Design.UITypeEditor))]
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
            }
        }
        [System.ComponentModel.Category("数据源"),System.ComponentModel.Description("text所属字段"), System.ComponentModel.Editor(typeof(ColumnSelector), typeof(System.Drawing.Design.UITypeEditor))]
        public string TextMember
        {
            get;
            set;
        }
        [System.ComponentModel.Category("数据源"), System.ComponentModel.Description("value所属字段"), System.ComponentModel.Editor(typeof(ColumnSelector), typeof(System.Drawing.Design.UITypeEditor))]
        public string ValueMember
        {
            get;
            set;
        }
        [System.ComponentModel.Description("如果为True，用户不能输入文字")]
        public bool SelectOnly
        {
            get;
            set;
        }
      
    }
}
