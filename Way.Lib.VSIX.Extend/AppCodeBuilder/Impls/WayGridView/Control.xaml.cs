using System;
using System.Collections.Generic;
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

namespace Way.Lib.VSIX.Extend.AppCodeBuilder.Impls.WayGridView
{
    /// <summary>
    /// Control.xaml 的交互逻辑
    /// </summary>
    public partial class Control : UserControl
    {
        Type _dbtype;
        WayGridViewBuilder _builder;
        SampleColumn[] _columns;
        public Control(WayGridViewBuilder builder)
        {
            InitializeComponent();
            _builder = builder;
        }

        public void OnDatasourceChanged()
        {
            if (_builder.Table != null)
            {
                var type = ((PropertyInfo)_builder.Table.Value).PropertyType;
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
                _dbtype = ((PropertyInfo)_builder.Table.Value).DeclaringType;
                SampleColumn.s_Tables = _dbtype.GetProperties(BindingFlags.Public | BindingFlags.Instance).OrderBy(m => m.Name).
                Where(m => m.PropertyType.IsGenericType || m.PropertyType.IsArray || m.PropertyType.HasElementType).
                Select(m => m.Name).ToArray();


                var properties = dataType.GetProperties();
                _columns = (from m in properties
                              where m.GetCustomAttribute<Way.EntityDB.WayLinqColumnAttribute>() != null
                              select getColumn(m)).ToArray();
            
                lstColumns.ItemsSource = _columns;
            }
        }

    

        SampleColumn getColumn( PropertyInfo pro)
        {
            var column = new SampleColumn
            {
                Name = pro.Name,
                caption = pro.GetCustomAttribute<Way.EntityDB.WayLinqColumnAttribute>().Comment,
                PropertyInfo = pro,
            };
            
            return column;
        }
        private void RelaTableNameChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBox list = (ComboBox)sender;
            SampleColumn column = (SampleColumn)list.Tag;
            column.RelaColumns.Clear();
            var pro = _dbtype.GetProperty(column.RelaTableName);

            Type dataType;
            if (pro.PropertyType.IsGenericType)
            {
                dataType = pro.PropertyType.GenericTypeArguments[0];
            }
            else if (pro.PropertyType.IsArray)
            {
                dataType = pro.PropertyType.GetElementType();
            }
            else if (pro.PropertyType.HasElementType)
            {
                dataType = pro.PropertyType.GetElementType();
            }
            else
            {
                return;
            }
            var properties = dataType.GetProperties().Where(m=> m.GetCustomAttribute<Way.EntityDB.WayLinqColumnAttribute>() != null);
            foreach( var p in properties )
                column.RelaColumns.Add(p.Name);
           

        }

        private void btnMakeCode_Click(object sender, RoutedEventArgs e)
        {
            build();
        }

        private void build()
        {
            var columns = (from m in _columns
                           where m.IsChecked == true
                           select m).ToArray();
            if (columns.Length == 0)
                return;

            try
            {
                string templateFolderPath = BuilderForm.GetService<Services.IApplication>().GetTemplatePath();
                if (System.IO.Directory.Exists(templateFolderPath) == false)
                    throw new Exception("缺乏代码模板文件夹，不能生成代码");

                var encode = System.Text.Encoding.GetEncoding("gb2312");
                templateFolderPath += "//WayGridView//";

                if (true)
                {




                    #region html代码
                    if (true)
                    {
                        string htmlTemplate;
                        string itemTemplate;
                        string headerItemTemplate;
                        string searchTemplate = null;

                        htmlTemplate = File.ReadAllText($"{templateFolderPath}html.txt", encode);
                        itemTemplate = File.ReadAllText($"{templateFolderPath}html.Item.txt", encode);
                        headerItemTemplate = File.ReadAllText($"{templateFolderPath}html.HeaderItem.txt", encode);
                       

                        htmlTemplate = htmlTemplate.Replace("{%ControlId}", _builder.ControlId)
                            .Replace("{%PageSize}", _builder.PageSize.ToString())
                            .Replace("{%AllowEdit}", _builder.AllowEdit ? "true" : "")
                             .Replace("{%SearchID}", _builder.ShowSearchArea ? _builder.ControlId + "_search" : "")
                            .Replace("{%ControlDatasource}", $"{((Type)_builder.DBContext.Value).FullName}.{((PropertyInfo)_builder.Table.Value).Name}");


                        StringBuilder itemBuffer = new StringBuilder();
                        foreach (var column in columns)
                        {
                            if (column.RelaTableName.IsNullOrEmpty() == false && column.RelaColumnName.IsNullOrEmpty() == false && column.DisplayColumnName.IsNullOrEmpty() == false)
                            {
                                itemBuffer.AppendLine(itemTemplate.Replace("{%Text}", $"{{@{column.Name}:{((Type)_builder.DBContext.Value).FullName}.{column.RelaTableName}.{column.RelaColumnName}:{column.DisplayColumnName}}}"));
                            }
                            else
                            {
                                itemBuffer.AppendLine(itemTemplate.Replace("{%Text}", "{@" + column.Name + "}"));
                            }
                        }
                        StringBuilder headerBuffer = new StringBuilder();
                        foreach (var column in columns)
                        {

                             headerBuffer.AppendLine(headerItemTemplate.Replace("{%Text}", column.caption.IsNullOrEmpty() ? column.Name : column.caption));
                           
                        }
                       
                        htmlTemplate = htmlTemplate.Replace("{%Items}", itemBuffer.ToString());
                        htmlTemplate = htmlTemplate.Replace("{%HeaderItems}", headerBuffer.ToString());

                        if (_builder.ShowSearchArea)
                        {
                            searchTemplate = File.ReadAllText($"{templateFolderPath}html.Search.txt", encode).
                                Replace("{%ElementId}", _builder.ControlId + "_search").
                                Replace("{%ControlId}", _builder.ControlId  );

                            string textTemplate = File.ReadAllText($"{templateFolderPath}html.Search.Item.Text.txt", encode);
                            string selectorTemplate = File.ReadAllText($"{templateFolderPath}html.Search.Item.Selector.txt", encode);
                            StringBuilder searchBuffer = new StringBuilder();
                            foreach (var column in columns)
                            {
                                string template = textTemplate;
                                bool relateOtherTable = column.RelaTableName.IsNullOrEmpty() == false && column.RelaColumnName.IsNullOrEmpty() == false && column.DisplayColumnName.IsNullOrEmpty() == false;
                                if (column.PropertyInfo.PropertyType.IsEnum || relateOtherTable)
                                {
                                    template = selectorTemplate;
                                }
                                template = template.Replace("{%Caption}" , column.caption.IsNullOrEmpty() ? column.Name : column.caption)
                                    .Replace("{%ColumnName}" , column.Name);
                                if (column.PropertyInfo.PropertyType.IsEnum)
                                {
                                    StringBuilder arrStr = new StringBuilder();
                                    arrStr.Append("[");
                                    FieldInfo[] fields = column.PropertyInfo.PropertyType.GetFields();
                                    for(int i = 1; i < fields.Length; i ++)
                                    {
                                        if (i > 1)
                                            arrStr.Append(',');
                                        arrStr.Append($"{{text:\"{fields[i].Name}\",value:\"{fields[i].Name}\"}}");
                                    }
                         
                                    arrStr.Append("]");
                                    template = template.Replace("{%DataSource}", arrStr.ToString())
                                        .Replace("{%AttributeArea}", "selectonly=\"true\"");
                                }
                                else if(relateOtherTable)
                                {
                                    template = template.Replace("{%DataSource}", $"{((Type)_builder.DBContext.Value).FullName}.{column.RelaTableName}")
                                        .Replace("{%AttributeArea}", $"textMember='{column.DisplayColumnName}' valueMember='{column.RelaColumnName}'");
                                }
                                searchBuffer.AppendLine(template);
                            }
                            searchTemplate = searchTemplate.Replace("{%SearchItems}", searchBuffer.ToString());
                        }

                        if (!searchTemplate.IsNullOrEmpty())
                            searchTemplate += "\r\n";
                        txtHtml.Text = searchTemplate + htmlTemplate;
                    }
                    #endregion

                }
            }
            catch (Exception ex)
            {
                Helper.ShowError(ex);
            }
        }
    }
}
