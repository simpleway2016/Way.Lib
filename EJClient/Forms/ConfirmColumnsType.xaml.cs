using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Way.Lib;
namespace EJClient.Forms
{
    /// <summary>
    /// ConfirmColumnsType.xaml 的交互逻辑
    /// </summary>
    public partial class ConfirmColumnsType : Window
    {
        IEnumerable<ColumnInfo> _columns;
        public ConfirmColumnsType()
        {
            InitializeComponent();
        }
        public ConfirmColumnsType(TableInfo tableinfo):this()
        {
            this.Title = $"表{tableinfo.TableName}以下字段的类型需要修正";
            _columns = from n in tableinfo.Columns
                                 where n.dbType.StartsWith("[未识别]")
                                 select new ColumnInfo(n);
            list.ItemsSource = _columns;
        }

        private void btnOK_Click(object sender, RoutedEventArgs e)
        {
            if(_columns.Any(m=>m.Type.IsNullOrEmpty()))
            {
                Helper.ShowError(this, "请修正所有字段类型");
                return;
            }
            this.DialogResult = true;
        }

        class ColumnInfo
        {
            EJ.DBColumn _column;
            string _originalType;
            string _type;
            public ColumnInfo(EJ.DBColumn column)
            {
                _column = column;
                _originalType = column.dbType;
            }
            public string Name
            {
                get
                {
                    return _column.Name;
                }
            }
            public string OriginalType
            {
                get
                {
                    return _originalType;
                }
            }
            public string Type
            {
                get
                {
                    return _type;
                }
                set
                {
                    _type = value;
                    _column.dbType = value;
                }
            }
            public List<string> AllTypes
            {
                get
                {
                    return ColumnType.SupportTypes;
                }
            }
        }

      
    }
}
