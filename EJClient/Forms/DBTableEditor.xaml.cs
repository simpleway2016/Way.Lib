using EJClient.Designer;
using EJClient.TreeNode;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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
    /// DBTableEditor.xaml 的交互逻辑
    /// </summary>
    public partial class DBTableEditor : Window
    {
       

        static string[] Tables;
        #region design class

      class MyClassProperty:EJ.classproperty
        {
            DBTableEditor _editor;
            public MyClassProperty(DBTableEditor editor)
            {
                _editor = editor;
                this.ForeignKeys = new System.Collections.ObjectModel.ObservableCollection<数据列基本信息>();
            }
            public override int? foreignkey_tableid {
                get => base.foreignkey_tableid;
                set {
                    if(base.foreignkey_tableid != value)
                    {
                        base.foreignkey_tableid = value;

                        this.ForeignKeys.Clear();
 
                        var columns = Helper.Client.InvokeSync<EJ.DBColumn[]>("GetColumnList", (this.iscollection == false) ? _editor.m_modifyingTable.id.GetValueOrDefault() : value.GetValueOrDefault());
                        foreach (var c in columns)
                        {
                            this.ForeignKeys.Add(new 数据列基本信息(c, _editor));
                        }
                    }
                }
            }

            public override bool? iscollection
            {
                get => base.iscollection;
                set
                {
                    if (base.iscollection != value)
                    {
                        base.iscollection = value;

                        this.ForeignKeys.Clear();

                        var columns = Helper.Client.InvokeSync<EJ.DBColumn[]>("GetColumnList", (this.iscollection == false) ? _editor.m_modifyingTable.id.GetValueOrDefault() : foreignkey_tableid.GetValueOrDefault());
                        foreach (var c in columns)
                        {
                            this.ForeignKeys.Add(new 数据列基本信息(c, _editor));
                        }
                    }
                }
            }
           

            public System.Collections.ObjectModel.ObservableCollection<数据列基本信息> ForeignKeys { get; set; }
        }

        internal class 索引 : INotifyPropertyChanged
        {
            public System.Collections.ObjectModel.ObservableCollection<数据列基本信息> Columns;
            public bool IsUnique;
            public bool IsClustered;

            public 索引(System.Collections.ObjectModel.ObservableCollection<数据列基本信息> columns, bool isUnique, bool isClustered)
            {
                IsUnique = isUnique;
                IsClustered = isClustered;
                this.Columns = columns;
            }
            public string[] ColumnNames
            {
                get;
                set;
            }

            public void OnPropertyChanged()
            {
                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("ColumnNames"));
            }
            public event PropertyChangedEventHandler PropertyChanged;

            public event PropertyChangingEventHandler PropertyChanging;
        }
        class 级联删除信息
        {
            int m_databaseid;
            public 级联删除信息(int databaseid,string tableName)
            {
                m_databaseid = databaseid;
                this.TableName = tableName;
                Columns = new System.Collections.ObjectModel.ObservableCollection<string>();
                reBindColumns();
            }
            public void reBindColumns()
            {
                Columns.Clear();
                if (!string.IsNullOrEmpty(this.TableName))
                {
                    string[] columns = Helper.Client.InvokeSync<string[]>("GetColumnNames", m_databaseid, this.TableName);

                    foreach (string c in columns)
                    {
                        Columns.Add(c);
                    }
                }
            }
            public string TableName
            {
                get;
                set;
            }
            public string ColumnName
            {
                get;
                set;
            }
            public string[] Tables
            {
                get
                {
                    return DBTableEditor.Tables;
                }
            }
            public System.Collections.ObjectModel.ObservableCollection<string> Columns
            {
                get;
                set;
            }
        
           
        }
        class 数据表基本信息
        {
            public string 注释
            {
                get
                {
                    return table.caption;
                }
                set
                {
                    table.caption = value.Trim();
                }
            }
            public string 表名
            {
                get
                {
                    return table.Name;
                }
                set
                {
                    table.Name = value.Trim();
                }
            }
            EJ.DBTable table;
            public 数据表基本信息(EJ.DBTable table)
            {
                this.table = table;
            }
        }

        internal class 数据列基本信息 : INotifyPropertyChanged
        {
            internal EJ.DBColumn m_column;
            System.Windows.Forms.PropertyGrid m_pgGrid;
            DBTableEditor m_parentEditor;
            public 数据列基本信息(EJ.DBColumn column, DBTableEditor parentEditor)
            {
                m_parentEditor = parentEditor;
                m_pgGrid = parentEditor.pgridForColumn;
                m_column = column;
            }

            [Browsable(false)]
            public int? id
            {
                get
                {
                    return m_column.id;
                }
            }

            [Category("字段属性"),DisplayName("1:Name")]
            public virtual string Name
            {
                get
                {
                    return m_column.Name;
                }
                set
                {
                    value = value.Trim();
                    if (m_column.Name != value)
                    {
                        if (m_parentEditor.m_columns.Where(m => m != this && string.Equals(m.Name , value , StringComparison.CurrentCultureIgnoreCase) ).Count() > 0)
                        {
                            MessageBox.Show( m_parentEditor , "列名与其它列冲突！");
                            return;
                        }

                        foreach (var idxConfigItem in m_parentEditor.m_IDXConfigs)
                        {
                            bool changed = false;
                            for (int i = 0; i < idxConfigItem.ColumnNames.Length; i++)
                            {
                                if (idxConfigItem.ColumnNames[i] == m_column.Name)
                                {
                                    changed = true;
                                    idxConfigItem.ColumnNames[i] = value;
                                }
                            }
                            if (changed)
                            {
                                //要重新创建一个string[]，否则就算触发OnPropertyChanged，也不会引起Selector的OnDataSourceChanged
                                idxConfigItem.ColumnNames = idxConfigItem.ColumnNames.ToJsonString().ToJsonObject<string[]>();
                                idxConfigItem.OnPropertyChanged();
                            }
                        }

                        m_column.Name = value;
                        OnPropertyChanged("Name");

                      
                    }
                }

            }

            bool _IsChecked = false;
            [Browsable(false)]
            public virtual bool IsChecked
            {
                get
                {
                    return _IsChecked;
                }
                set
                {
                    if (_IsChecked != value)
                    {
                        _IsChecked = value;
                        if (PropertyChanged != null)
                            PropertyChanged(this, new PropertyChangedEventArgs("IsChecked"));
                    }
                }
            } 

            bool _IsSelected = false;
            [Browsable(false)]
            public virtual bool IsSelected
            {
                get
                {
                    return _IsSelected;
                }
                set
                {
                    if (_IsSelected != value)
                    {
                        _IsSelected = value;
                        if (PropertyChanged != null)
                            PropertyChanged(this, new PropertyChangedEventArgs("IsSelected"));
                    }
                }
            }

           [Category("字段属性"), DisplayName("3:CanNull")]
            public bool? CanNull
            {
                get
                {
                    return m_column.CanNull;
                }
                set
                {
                    if (m_column.CanNull != value)
                    {
                        if (value == false && IsPKID == true)
                        {
                            MessageBox.Show(m_parentEditor, "主键不允许可以为空！");
                            return;
                        }

                        m_column.CanNull = value;
                        OnPropertyChanged("CanNull");
                    }
                }
            }

            [Browsable(false)]
           public string showCaption
           {
               get
               {
                   if (m_column.caption != null)
                   {
                       if (m_column.caption.Contains(","))
                           return m_column.caption.Substring(0, m_column.caption.IndexOf(","));
                       else if (m_column.caption.Contains("，"))
                           return m_column.caption.Substring(0, m_column.caption.IndexOf("，"));
                   }
                   return m_column.caption;
               }
           }

            //
            // 摘要:
            //     caption
           [Category("字段属性"), DisplayName("2:中文注释")]
            public string caption
            {
                get
                {
                    return m_column.caption;
                }
                set
                {
                    value = value.Trim();
                    if (m_column.caption != value)
                    {
                        m_column.caption = value;
                        if (PropertyChanged != null)
                        {
                            PropertyChanged(this, new PropertyChangedEventArgs("showCaption"));
                            PropertyChanged(this, new PropertyChangedEventArgs("caption"));
                        }
                    }
                }
            }
            //
            // 摘要:
            //     数据库字段类型
           [Category("字段属性"), DisplayName("4:数据库中的类型"),
            System.ComponentModel.TypeConverter(typeof(DbTypeConvert))]
            public string dbType
            {
                get
                {
                    return m_column.dbType;
                }
                set
                {
                    if (m_column.dbType != value)
                    {
                        if (value.Contains("char"))
                        {
                            length = "50";
                        }
                        else if (value.Contains("decimal"))
                        {
                            length = "18,4"; 
                        }
                        else
                        {
                            length = ""; 
                        }
                        m_column.dbType = value;
                        OnPropertyChanged("dbType");
                    }
                }
            }
            //
            // 摘要:
            //     默认值
           [Category("字段属性"), DisplayName("5:默认值")]
            public string defaultValue
            {
                get
                {
                    return m_column.defaultValue;
                }
                set
                {
                    if (m_column.defaultValue != value)
                    {
                        m_column.defaultValue = value;
                        OnPropertyChanged("defaultValue");
                    }
                }
            }
            //
            // 摘要:
            //     Enum定义
           [Category("字段属性"), DisplayName("6:Enum"),
             System.ComponentModel.Editor(typeof(Editor.EnumEditor), typeof(System.Drawing.Design.UITypeEditor))]
            public string EnumDefine
            {
                get
                {
                    return m_column.EnumDefine;
                }
                set
                {
                    if (m_column.EnumDefine != value)
                    {
                        m_column.EnumDefine = value;
                        this.dbType = "int";
                        if (PropertyChanged != null)
                            PropertyChanged(this, new PropertyChangedEventArgs("EnumDefine"));
                    }
                }
            }

            //
            // 摘要:
            //     自增长
            [Category("字段属性"), DisplayName("3:自增长"), System.ComponentModel.TypeConverter(typeof(BooleanTypeConvert))]
            public bool? IsAutoIncrement
            {
                get
                {
                    return m_column.IsAutoIncrement;
                }
                set
                {
                    if (m_column.IsAutoIncrement != value)
                    {
                        m_column.IsAutoIncrement = value;
                        OnPropertyChanged("IsAutoIncrement");
                    }
                }
            }
            //
            // 摘要:
            //     是否是主键
             [Category("字段属性"), DisplayName("3:主键"), System.ComponentModel.TypeConverter(typeof(BooleanTypeConvert))]
            public bool? IsPKID
            {
                get
                {
                    return m_column.IsPKID;
                }
                set
                {
                    if (m_column.IsPKID != value)
                    {
                        if (value == true && m_parentEditor.m_columns.Count(m => m != this && m.IsPKID == true) > 0)
                        {
                            MessageBox.Show("一个表只能有一个主键，您可以另外设置唯一值索引");
                            return;
                        }
                        m_column.IsPKID = value;
                        if (value == true)
                        {
                            CanNull = false;
                        }
                        OnPropertyChanged("IsPKID");
                    }
                }
            }
            //
            // 摘要:
            //     长度
             [Category("字段属性"), DisplayName("7:Length")]
            public string length
            {
                get
                {
                    return m_column.length;
                }
                set
                {
                    if (m_column.length != value)
                    {
                        m_column.length = value;
                        OnPropertyChanged("length");
                    }
                }
            }


            public event PropertyChangedEventHandler PropertyChanged;
            public void OnPropertyChanged(string name)
            {
                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs(name));

             
            }
        }

        #endregion

        System.Windows.Forms.PropertyGrid pgridForTable;
        System.Windows.Forms.PropertyGrid pgridForColumn;
        DatabaseItemNode m_DBNode;
        public EJ.DBTable m_table;
        EJ.DBTable m_modifyingTable;
        
        bool m_relationChanged = false;
        bool m_IsModify = false;
        System.Collections.ObjectModel.ObservableCollection<级联删除信息> m_deleteConfigs = new System.Collections.ObjectModel.ObservableCollection<级联删除信息>();

        System.Collections.ObjectModel.ObservableCollection<索引> m_IDXConfigs = new System.Collections.ObjectModel.ObservableCollection<索引>();
        System.Collections.ObjectModel.ObservableCollection<数据列基本信息> m_columns = new System.Collections.ObjectModel.ObservableCollection<数据列基本信息>();
        System.Collections.ObjectModel.ObservableCollection<MyClassProperty> m_properties = new System.Collections.ObjectModel.ObservableCollection<MyClassProperty>();
        internal DBTableEditor(DatabaseItemNode dbnode , EJ.DBTable currentTable)
        {
            m_DBNode = dbnode;

            InitializeComponent();

            //把目前所有表信息，放入Resources["AllDBTables"]，便于页面绑定
            var 数据表Node = dbnode.Children.FirstOrDefault(m => m is 数据表Node);
            var alldbtables = new System.Collections.ObjectModel.ObservableCollection<EJ.DBTable>();
            foreach (DBTableNode tablenode in 数据表Node.Children)
            {
                alldbtables.Add(tablenode.Table);
            }
            this.Resources["AllDBTables"] = alldbtables;
            this.Resources["DBColumns"] = m_columns;

            pgridForTable = new System.Windows.Forms.PropertyGrid();
            pgridForTable.LineColor = System.Drawing.ColorTranslator.FromHtml("#cccccc");
            hostTablePG.Child = pgridForTable;

            pgridForColumn = new System.Windows.Forms.PropertyGrid();
            pgridForColumn.LineColor = System.Drawing.ColorTranslator.FromHtml("#cccccc");
            hostColumnPG.Child = pgridForColumn;

            listUniqueIndex.ItemsSource = m_IDXConfigs;
            if (currentTable == null)
            {
                m_table = new EJ.DBTable();
                m_table.DatabaseID = dbnode.Database.id;

                数据列基本信息 item = new 数据列基本信息(new EJ.DBColumn()
                {
                    Name = "id",
                    CanNull = false,
                    IsPKID = true,
                    IsAutoIncrement = true,
                    dbType = "int",

                }, this);
                m_columns.Add(item);
            }
            else
            {
                tabControl.SelectedItem = columnTab;
                m_modifyingTable = currentTable;
                m_IsModify = true;
                m_table = (EJ.DBTable)Helper.Clone(currentTable);
                var columns = Helper.Client.InvokeSync<EJ.DBColumn[]>("GetColumnList", currentTable.id.Value);
                foreach (var c in columns)
                {
                    m_columns.Add(new 数据列基本信息(c, this));
                }

                var existProperties = Helper.Client.InvokeSync<EJ.classproperty[]>("GetClassPropertyList", currentTable.id.Value);
                foreach (var c in existProperties)
                {
                    var p = new MyClassProperty(this)
                    {
                        foreignkey_columnid = c.foreignkey_columnid,
                        foreignkey_tableid = c.foreignkey_tableid,
                        id = c.id,
                        iscollection = c.iscollection,
                        name = c.name,
                        tableid = c.tableid,
                    };
                    p.ChangedProperties.Clear();
                    m_properties.Add(p);
                }

                var delconfigs = Helper.Client.InvokeSync<EJ.DBDeleteConfig[]>("GetTableDeleteConfigList", currentTable.id.Value);
                foreach (var delitem in delconfigs)
                {
                    m_deleteConfigs.Add(new 级联删除信息(currentTable.DatabaseID.Value, delitem.RelaTable_Desc)
                    {
                        ColumnName = delitem.RelaColumn_Desc
                    });
                }

                var idxConfig = Helper.Client.InvokeSync<EJ.IDXIndex[]>("GetTableIDXIndexList", currentTable.id.Value);
                foreach (var config in idxConfig)
                {
                    m_IDXConfigs.Add(new 索引(m_columns, config.IsUnique.Value, config.IsClustered.Value)
                    {
                        ColumnNames = config.Keys.Split(',')
                    });
                }
            }

            Tables = Helper.Client.InvokeSync<string[]>("GetTableNames", dbnode.Database.id.Value).OrderBy(m => m).ToArray();

            pgridForTable.SelectedObject = new 数据表基本信息(m_table);


            m_deleteConfigs.CollectionChanged += m_deleteConfigs_CollectionChanged;
            listDelConfig.ItemsSource = m_deleteConfigs;
            treeColumns.ItemsSource = m_columns;
            listProperties.ItemsSource = m_properties;


        }


        void m_deleteConfigs_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            m_relationChanged = true;
        }

        private void btnAddColumn_Click_1(object sender, RoutedEventArgs e)
        {
            int index = 1;
            string columnName = "column" + index;
            while (m_columns.Where(m => m.Name == columnName).Count() > 0)
            {
                index++;
                columnName = "column" + index;
            }
            数据列基本信息 item = new 数据列基本信息(new EJ.DBColumn()
                {
                    Name = columnName,
                    CanNull = true,
                    IsPKID = false,
                    IsAutoIncrement = false,
                    dbType = "varchar",
                    length = "50"
                }, this);

            var currentSelectedItem = m_columns.FirstOrDefault(m => m.IsSelected);
            if (currentSelectedItem != null)
            {
                m_columns.Insert( m_columns.IndexOf(currentSelectedItem) + 1 , item);
            }
            else
            {
                m_columns.Add(item);
            }
            item.IsSelected = true;
        }

        private void treeColumns_SelectedItemChanged_1(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            pgridForColumn.SelectedObject = treeColumns.SelectedItem;
        }

        private void btnOK_Click_1(object sender, RoutedEventArgs e)
        {
            this.Cursor = Cursors.Wait;
            e.Handled = true;
            if (string.IsNullOrEmpty(m_table.Name))
            {
                this.Cursor = null;
                MessageBox.Show(this, "表名不能为空");
                return;
            }
            for (int i = 0; i < m_columns.Count; i++)
            {
                
                if (m_columns[i].EnumDefine.IsNullOrEmpty() == false && m_columns[i].dbType != "int")
                {
                    this.Cursor = null;
                    MessageBox.Show(this, string.Format("{0}({1})定义为枚举类型，所以必须是int类型", m_columns[i].Name, m_columns[i].caption));
                    return;
                }
            }
           
            List<object> idsConfigs = new List<object>();
            foreach (索引 c in m_IDXConfigs)
            {
                //如果索引包含已经删除的字段，则忽略
                if (c.ColumnNames.Any(m => m_columns.Any(o=>o.Name == m) == false))
                {
                    this.Cursor = null;
                    MessageBox.Show(this, $"索引包含不存在的字段{c.ColumnNames.FirstOrDefault(m => m_columns.Any(o => o.Name == m) == false)}");
                    return;
                }

                if (c.ColumnNames.Length > 0)
                {
                    idsConfigs.Add(new
                        {
                            c.IsUnique,
                            c.IsClustered,
                            c.ColumnNames,
                        });
                }
            }

            List<EJ.DBDeleteConfig> delconfigs = new List<EJ.DBDeleteConfig>();
            foreach (var delitem in m_deleteConfigs)
            {
                if (string.IsNullOrEmpty(delitem.TableName) || string.IsNullOrEmpty(delitem.ColumnName))
                    continue;
                if (delconfigs.Count(m=>m.RelaTable_Desc == delitem.TableName && m.RelaColumn_Desc == delitem.ColumnName) > 0)
                    continue;
                delconfigs.Add(new EJ.DBDeleteConfig()
                    {
                        RelaTable_Desc = delitem.TableName,
                        RelaColumn_Desc = delitem.ColumnName,
                    });
            }

            try
            {
                EJ.DBColumn[] columns = new EJ.DBColumn[m_columns.Count];
                for (int i = 0; i < m_columns.Count; i++)
                {
                    columns[i] = m_columns[i].m_column;
                    m_columns[i].m_column.orderid = i;
                }

                if (m_IsModify)
                {
                    var propertyItems = m_properties.Where(m => m.name.IsNullOrEmpty() == false).ToArray().ToJsonString().ToJsonObject<EJ.classproperty[]>();
                    Helper.Client.InvokeSync<string>("ModifyTable", m_table, columns, delconfigs, idsConfigs , propertyItems);
                    m_modifyingTable.Name = m_table.Name;
                    m_modifyingTable.caption = m_table.caption;

                    foreach (UI.Document doc in MainWindow.instance.documentContainer.Items)
                    {
                        if (doc is UI.ModuleDocument)
                        {
                            var ui = ((UI.ModuleDocument)doc).getTableById(this.m_table.id.GetValueOrDefault());
                            if (ui != null)
                            {
                                ui.DataSource = new UI.Table._DataSource()
                                {
                                    Table = m_table,
                                    Columns = Helper.Client.InvokeSync<EJ.DBColumn[]>("GetColumns", m_table.id.GetValueOrDefault()),
                                };
                                ui.DataBind();
                            }
                        }
                    }
                }
                else
                {
                    m_table = Helper.Client.InvokeSync<EJ.DBTable>("CreateTable", m_table , columns , delconfigs , idsConfigs, m_properties.Where(m => m.name.IsNullOrEmpty() == false).ToArray());
                    //ChangedProperties本来是0，但ToJsonObject转换时，每个属性都进行set操作，又产生了ChangedProperties
                    m_table.ChangedProperties.Clear();

                    数据表Node parent = (数据表Node)m_DBNode.Children.FirstOrDefault(m => m is 数据表Node);
                    parent.Children.Insert(0, new DBTableNode(m_table, parent));
                    this.DialogResult = true;
                }

                this.Close();
                if (m_relationChanged)
                {
                    foreach (UI.Document doc in MainWindow.instance.documentContainer.Items)
                    {
                        if (doc is UI.ModuleDocument)
                        {
                            ((UI.ModuleDocument)doc).TableChangeRelation(this.m_table.id.GetValueOrDefault());
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.GetBaseException().Message);
                return;
            }
            finally
            {
                this.Cursor = null;
            }
        }

        private void btnDeleteColumn_Click_1(object sender, RoutedEventArgs e)
        {
            数据列基本信息 column = treeColumns.SelectedItem as 数据列基本信息;
            if (column == null)
                return;
            int index = m_columns.IndexOf(column);
            m_columns.Remove(column);
            var idx = m_IDXConfigs.FirstOrDefault(m => m.ColumnNames.Contains(column.Name));
            if(idx != null)
            {
                m_IDXConfigs.Remove(idx);
            }
            try
            {
                m_columns[index].IsSelected = true;
            }
            catch
            {
            }
        }
        private void RelaColumnNameChanged(object sender, SelectionChangedEventArgs e)
        {
            m_relationChanged = true;
        }
        private void RelaTableNameChanged(object sender, SelectionChangedEventArgs e)
        {
            m_relationChanged = true;
            ComboBox selTableName = (ComboBox)sender;
            ((级联删除信息)selTableName.Tag).reBindColumns();
        }

        private void addDeleteRela_Click_1(object sender, RoutedEventArgs e)
        {
            m_deleteConfigs.Add(new 级联删除信息(m_table.DatabaseID.Value, null));
        }

        private void delDeleteRela_Click_1(object sender, RoutedEventArgs e)
        {
            if (listDelConfig.SelectedItem != null)
            {
                m_deleteConfigs.Remove( (级联删除信息)listDelConfig.SelectedItem );
            }
        }
        private void addProperty_Click(object sender, RoutedEventArgs e)
        {
            m_properties.Add(new MyClassProperty(this));
        }

        private void delProperty_Click(object sender, RoutedEventArgs e)
        {
            if (listProperties.SelectedItem != null)
            {
                m_properties.Remove((MyClassProperty)listProperties.SelectedItem);
            }
        }

        private void btnMoveUp_Click_1(object sender, RoutedEventArgs e)
        {
            数据列基本信息 column = treeColumns.SelectedItem as 数据列基本信息;
            if (column == null || column == m_columns[0])
                return;

            m_relationChanged = true;//位置改变，也需要重画线
            int index = m_columns.IndexOf(column);
            m_columns.RemoveAt(index);
            m_columns.Insert(index - 1, column);
            column.IsSelected = true;
        }

        private void btnMoveDown_Click_1(object sender, RoutedEventArgs e)
        {
            数据列基本信息 column = treeColumns.SelectedItem as 数据列基本信息;
            if (column == null || column == m_columns.Last())
                return;

            m_relationChanged = true;//位置改变，也需要重画线
            int index = m_columns.IndexOf(column);
            m_columns.RemoveAt(index);
            m_columns.Insert(index + 1, column);
            column.IsSelected = true;
        }

        private void addUniqueIndex_Click_1(object sender, RoutedEventArgs e)
        {
            m_IDXConfigs.Add(new 索引(this.m_columns,false , false )
                {
                    ColumnNames = new string[0],
                });
        }

        private void delUniqueIndex_Click_1(object sender, RoutedEventArgs e)
        {
            索引 item = listUniqueIndex.SelectedItem as 索引;
            if (item != null)
            {
                m_IDXConfigs.Remove(item);
            }
        }

        private void btnCopy_Click_1(object sender, RoutedEventArgs e)
        {
            List<EJ.DBColumn> copyColumns = new List<EJ.DBColumn>();
            foreach (var column in m_columns)
            {
                if (column.IsChecked)
                {
                    copyColumns.Add(column.m_column);
                }
            }
            Clipboard.SetText("DBColumns:" + copyColumns.ToJsonString());
        }

        private void btnPaste_Click_1(object sender, RoutedEventArgs e)
        {
            string content = Clipboard.GetText();
            if (content.StartsWith("DBColumns:"))
            {
                EJ.DBColumn[] sourceColumns = content.Substring("DBColumns:".Length).ToJsonObject<EJ.DBColumn[]>();
                for (int i = 0; i < sourceColumns.Length; i++)
                {
                    if (m_columns.Where(m => m.Name == sourceColumns[i].Name).Count() > 0)
                        continue;
                    sourceColumns[i].id = null;

                    数据列基本信息 item = new 数据列基本信息(sourceColumns[i], this);
                    m_columns.Add(item);
                }
            }
        }

    }

    internal class 唯一值索引Selector : StackPanel
    {
        public static readonly DependencyProperty DataSourceProperty = DependencyProperty.Register("DataSource", typeof(object), typeof(唯一值索引Selector),
            new FrameworkPropertyMetadata(null, new PropertyChangedCallback(OnDataSourceChanged)));

        object _DataSource;
        public object DataSource
        {
            get
            {
                return _DataSource;
            }
            set
            {
                _DataSource = value;
                this.Children.Clear();
                if (value != null)
                {
                    EJClient.Forms.DBTableEditor.索引 dataContext = (EJClient.Forms.DBTableEditor.索引)this.DataContext;
                    if (true)
                    {
                        ComboBox comboBox = new ComboBox();
                        comboBox.Items.Add("唯一值");
                        comboBox.Items.Add("非唯一值");
                        comboBox.SelectedIndex = dataContext.IsUnique ? 0 : 1;
                        comboBox.Margin = new Thickness(3);
                        comboBox.SelectionChanged += 唯一值_SelectionChanged;
                        this.Children.Add(comboBox);
                    }
                    if (true)
                    {
                        ComboBox comboBox = new ComboBox();
                        comboBox.Items.Add("非聚焦");
                        comboBox.Items.Add("聚焦");
                        comboBox.SelectedIndex = dataContext.IsClustered ? 1 : 0;
                        comboBox.SelectionChanged += 聚焦_SelectionChanged;
                        comboBox.Margin = new Thickness(3);
                        this.Children.Add(comboBox);
                    }
                    foreach (var column in (string[])value)
                    {
                        ComboBox comboBox = new ComboBox();
                        comboBox.Items.Add(column.ToString());
                        comboBox.SelectedItem = column;
                        this.Children.Add(comboBox);
                        comboBox.DropDownOpened += comboBox_DropDownOpened;
                        comboBox.SelectionChanged += comboBox_SelectionChanged;
                        
                      
                        comboBox.Margin = new Thickness(3);
                        
                    }
                    if (true)
                    {
                        ComboBox comboBox = new ComboBox();
                        comboBox.DropDownOpened += comboBox_DropDownOpened;
                        comboBox.SelectionChanged += comboBox_SelectionChanged;
                        comboBox.Margin = new Thickness(3);
                        this.Children.Add(comboBox);
                    }
                }
            }
        }
        void 唯一值_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            EJClient.Forms.DBTableEditor.索引 dataContext = (EJClient.Forms.DBTableEditor.索引)this.DataContext;
            ComboBox comboBox = sender as ComboBox;
            dataContext.IsUnique = comboBox.SelectedIndex == 0;
        }
        void 聚焦_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            EJClient.Forms.DBTableEditor.索引 dataContext = (EJClient.Forms.DBTableEditor.索引)this.DataContext;
            ComboBox comboBox = sender as ComboBox;
            dataContext.IsClustered = comboBox.SelectedIndex == 1;
        }
        void comboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBox comboBox = sender as ComboBox;
            if (comboBox.SelectedItem == null)
                return;
            if (comboBox.SelectedItem.Equals(""))
            {
                if (comboBox != this.Children[this.Children.Count - 1])
                this.Children.Remove(comboBox);
            }
            else
            {
                int index = this.Children.IndexOf(comboBox);
                if (this.Children.Count - 1 == index)
                {
                    //添加一个combobox
                    comboBox = new ComboBox();
                    comboBox.DropDownOpened += comboBox_DropDownOpened;
                    comboBox.SelectionChanged += comboBox_SelectionChanged;
                    comboBox.Margin = new Thickness(3);
                    this.Children.Add(comboBox);
                }
            }

            EJClient.Forms.DBTableEditor.索引 dataContext = (EJClient.Forms.DBTableEditor.索引)this.DataContext;
            List<string> columns = new List<string>();
            for (int i = 2; i < this.Children.Count; i ++ )
            {
                ComboBox selector = (ComboBox)this.Children[i] ;
                string name = Convert.ToString(selector.SelectedItem);
                if (!string.IsNullOrEmpty(name) && columns.Contains(name) == false)
                    columns.Add(name);
            }
            dataContext.ColumnNames = columns.ToArray();
        }

        void comboBox_DropDownOpened(object sender, EventArgs e)
        {
            ComboBox comboBox = sender as ComboBox;
            EJClient.Forms.DBTableEditor.索引 dataContext = (EJClient.Forms.DBTableEditor.索引)this.DataContext;
            var oldselected = comboBox.SelectedItem ;
            comboBox.Items.Clear();
            comboBox.Items.Add("");
            foreach (EJClient.Forms.DBTableEditor.数据列基本信息 column in dataContext.Columns)
            {
                comboBox.Items.Add(column.m_column.Name);
            }

            comboBox.SelectedItem = oldselected;
        }

        private static void OnDataSourceChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            ((唯一值索引Selector)sender).DataSource = e.NewValue;
        }


    }
}
