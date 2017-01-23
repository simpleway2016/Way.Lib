using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Way.Lib.VSIX.Extend.AppCodeBuilder.Impls.WayRelateList
{
    /// <summary>
    /// Control.xaml 的交互逻辑
    /// </summary>
    public partial class Control : UserControl
    {
        RelateConfig[] _source;
        WayRelateListBuilder _builder;
        internal static List<ValueDescription> DBContexts = null;
        public Control(WayRelateListBuilder builder)
        {
            if(DBContexts == null)
            {
                DBContexts = new List<Extend.ValueDescription>();
                   var typeDiscoverer = BuilderForm.GetService<Services.ITypeDiscoverer>();
                var dbtypes = typeDiscoverer.GetTypes(typeof(Way.EntityDB.DBContext));
                if (dbtypes != null)
                {
                    dbtypes = dbtypes.Where(m => m.GetConstructor(new Type[0]) != null).ToArray();
                    foreach (var dbtype in dbtypes)
                    {
                        DBContexts.Add(new ValueDescription(dbtype, "Name"));

                    }
                }
            }
            _builder = builder;
            InitializeComponent();
            _source = new RelateConfig[10];
            for(int i = 0; i < _source.Length; i ++)
            {
                _source[i] = new WayRelateList.RelateConfig(_source);
            }
            lstColumns.ItemsSource = _source;
        }

        private void btnMakeCode_Click(object sender, RoutedEventArgs e)
        {
            var columns = (from m in _source
                           where m.DBContext != null && m.Table != null
                           select m).ToArray();
            if (columns.Length == 0)
                return;

            try
            {
                string templateFolderPath = BuilderForm.GetService<Services.IApplication>().GetTemplatePath();
                if (System.IO.Directory.Exists(templateFolderPath) == false)
                    throw new Exception("缺乏代码模板文件夹，不能生成代码");

                var encode = System.Text.Encoding.GetEncoding("gb2312");
                templateFolderPath += "//WayRelateList//";

                if (true)
                {




                    #region html代码
                    if (true)
                    {
                        string htmlTemplate;

                        htmlTemplate = File.ReadAllText($"{templateFolderPath}html.txt", encode);
                      


                        htmlTemplate = htmlTemplate.Replace("{%ControlId}", _builder.ControlId);


                        StringBuilder itemBuffer = new StringBuilder();
                        foreach (var column in columns)
                        {
                            itemBuffer.AppendLine($"<config datasource=\"{column.DBContext.Value.ToString() + "." + ((PropertyInfo)column.Table.Value).Name}\" {(column.RelaColumnName.IsNullOrEmpty() ?"": "relateMember='"+column.RelaColumnName+"'")} textMember=\"{column.TextColumnName}\" {(column.Loop?"loop='true'":"")} valueMember=\"{column.ValueColumnName}\"></config>");
                        }
                        
                        txtHtml.Text =  htmlTemplate.Replace("{%DataSource}", itemBuffer.ToString());
                    }
                    #endregion

                }
            }
            catch (Exception ex)
            {
                Helper.ShowError(ex);
            }
        }

        private void btnCopy_Click(object sender, RoutedEventArgs e)
        {
            Clipboard.SetText(txtHtml.Text);
        }
    }

    class RelateConfig: System.ComponentModel.INotifyPropertyChanged
    {
        RelateConfig[] _group;
        public RelateConfig(RelateConfig[] group)
        {
            _group = group;
            this.DBContexts = Control.DBContexts;
            this.Tables = new ObservableCollection<ValueDescription>();
            this.Columns = new ObservableCollection<string>();
        }
       
        public List<ValueDescription> DBContexts
        {
            get;
            set;
        }

        ValueDescription _DBContext;
        public ValueDescription DBContext
        {
            get
            {
                return _DBContext;
            }
            set
            {
                _DBContext = value;
                this.Tables.Clear();
                var properties = ((Type)_DBContext.Value).GetProperties(BindingFlags.Public | BindingFlags.Instance).OrderBy(m => m.Name).Where(m => m.PropertyType.IsGenericType || m.PropertyType.IsArray || m.PropertyType.HasElementType).ToArray();
                foreach(var pro in properties )
                {
                    this.Tables.Add(new ValueDescription(pro ,"Name"));
                }
                for(int i = 0; i < _group.Length;i++)
                {
                    if (_group[i].DBContext == null)
                    {
                        _group[i].DBContext = value;
                        break;
                    }
                }
                this.PropertyChanged(this, new PropertyChangedEventArgs("DBContext"));
            }
        }
       

        public ObservableCollection<ValueDescription> Tables
        {
            get;
            set;
        }

        ValueDescription _Table;

        public event PropertyChangedEventHandler PropertyChanged;

        public ValueDescription Table
        {
            get { return _Table; }
            set
            {
                _Table = value;
                this.Columns.Clear();

                var type = ((PropertyInfo)_Table.Value).PropertyType;
                Type dataType;
                if (type.IsGenericType)
                {
                    dataType = type.GenericTypeArguments[0];
                }
                else if (type.IsArray)
                {
                    dataType = type.GetElementType();
                }
                else if (type.HasElementType)
                {
                    dataType = type.GetElementType();
                }
                else
                {
                    return;
                }

                var properties = dataType.GetProperties();
                foreach( var p in properties )
                {
                    if(p.GetCustomAttribute<Way.EntityDB.WayLinqColumnAttribute>() != null)
                    {
                        this.Columns.Add(p.Name);
                    }
                }
               
            }
        }

        public ObservableCollection<string> Columns
        {
            get;
            set;
        }
        public string RelaColumnName
        {
            get;
            set;
        }
        public string TextColumnName
        {
            get;
            set;
        }
        public string ValueColumnName
        {
            get;
            set;
        }
        public bool Loop { get; set; }
    }

}
