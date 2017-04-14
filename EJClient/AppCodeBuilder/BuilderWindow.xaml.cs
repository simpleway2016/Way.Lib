﻿using System;
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

namespace EJClient.AppCodeBuilder
{
    /// <summary>
    /// BuilderWindow.xaml 的交互逻辑
    /// </summary>
    public partial class BuilderWindow : Window
    {
        int _TableId;
        int _databaseid;
        SampleColumn[] _columns;
        IAppCodeBuilder _builder;
        System.Windows.Forms.PropertyGrid _propertyGrid;
        public BuilderWindow(int tableid,int databaseid, IAppCodeBuilder builder)
        {
            _builder = builder;
            _databaseid = databaseid;
               _TableId = tableid;
            InitializeComponent();

            _propertyGrid = new System.Windows.Forms.PropertyGrid();
            _propertyGrid.LineColor = System.Drawing.ColorTranslator.FromHtml("#cccccc");
            _propertyGrid.SelectedObject = builder;
            hostBuilderPG.Child = _propertyGrid;

            loadColumns();
        }

        void loadColumns()
        {
            try {
                SampleColumn.s_Tables = Helper.Client.InvokeSync<string[]>("GetTableNames", _databaseid).OrderBy(m => m).ToArray();
                _columns = Helper.Client.InvokeSync<SampleColumn[]>("GetColumns", _TableId);
                lstColumns.ItemsSource = _columns;
            }
            catch(Exception ex) {
                Helper.ShowError(ex);
            }
           
        }

        private void RelaTableNameChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBox list = (ComboBox)sender;
            SampleColumn column = (SampleColumn)list.Tag;
            column.RelaColumns.Clear();
            try
            {
                var columnNames = Helper.Client.InvokeSync<string[]>("GetColumnNamesByTableName", column.RelaTableName, _databaseid);
                foreach (string c in columnNames)
                {
                    column.RelaColumns.Add(c);
                }
            }
            catch (Exception ex)
            {
                Helper.ShowError(ex);
            }

        }

        private void btnMake_Click(object sender, RoutedEventArgs e)
        {
            var codes = _builder.Build(_columns);
            lstCodes.ItemsSource = codes;
        }

        private void btnCopy_Click(object sender, RoutedEventArgs e)
        {
             var sampleCode = ((Button)sender).Tag as SampleCode;
            Clipboard.SetText(sampleCode.Code);
        }
    }
}
