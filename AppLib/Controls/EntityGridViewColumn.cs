using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI.WebControls;

namespace AppLib.Controls
{
    public class EntityGridViewColumn:BoundField,IKeyObject
    {
        string _KeyTableName;
        [System.ComponentModel.Category("系统设定"), System.ComponentModel.Description("关联的外键表名"),
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
        [System.ComponentModel.Category("系统设定"), System.ComponentModel.Description("关联的外键表ID字段"),
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
        [System.ComponentModel.Category("系统设定"), System.ComponentModel.Description("关联的外键表显示字段"),
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

        bool _Statistical;
        [System.ComponentModel.Category("系统设定"), System.ComponentModel.Description("是否进行统计"),
        System.ComponentModel.Editor(typeof(Editor.TableFieldSelector), typeof(System.Drawing.Design.UITypeEditor))]
        public bool Statistical
        {
            get
            {
                return _Statistical;
            }
            set
            {
                if (_Statistical != value)
                {
                    _Statistical = value;
                }
            }
        }

        List<ValueChange> _ValueConfigs = new List<ValueChange>();
         [System.ComponentModel.Category("系统设定"), System.ComponentModel.Description("值的转换输出"),
        System.Web.UI.PersistenceMode(System.Web.UI.PersistenceMode.InnerProperty), System.ComponentModel.MergableProperty(false),
        System.ComponentModel.Editor(typeof(Editor.CollectionBaseEditor), typeof(System.Drawing.Design.UITypeEditor))]
        public List<ValueChange> ValueChanges
        {
            get
            {
                return _ValueConfigs;

            }
        }

        protected override DataControlField CreateField()
        {
            return this;
        }

        [System.ComponentModel.Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public System.ComponentModel.ISite Site
        {
            get { return null; }
        }

        public EntityGridViewColumn()
        {
            
        }

        internal IAutoDataBindControl _AutoDataBindControl;
        public IAutoDataBindControl AutoDataBindControl
        {
            get { return _AutoDataBindControl; }
        }
    }

    public class EntityCheckboxColumn : EntityGridViewColumn
    {
    }

    [Editor.TitleField("原值")]
    public class ValueChange:ICloneable
    {
        public string 原值
        {
            get;
            set;
        }

        public string 对应输出
        {
            get;
            set;
        }



        public object Clone()
        {
            return new ValueChange() {
                原值 = this.原值,
                对应输出 = this.对应输出
            };
        }
    }
}
