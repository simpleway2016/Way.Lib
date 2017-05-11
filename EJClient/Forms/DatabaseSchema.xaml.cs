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

namespace EJClient.Forms
{
    /// <summary>
    /// DatabaseEditor.xaml 的交互逻辑
    /// </summary>
    public partial class DatabaseSchema : Window
    {
        class DBType
        {
            public string Name
            {
                get;
                set;
            }
            public EJ.Databases_dbTypeEnum Value
            {
                get;
                set;
            }
        }
        int _projectID;
        EJ.Databases _target;
        EJ.Databases _currentData;
        public DatabaseSchema()
        {
            InitializeComponent();
        }
        public DatabaseSchema(EJ.Databases database)
        {
            InitializeComponent();
            _target = database;
            _currentData = new EJ.Databases();
            _currentData.conStr = "server=;uid=;pwd=;Database=";

            var dbtypes = typeof(Way.EntityDB.DatabaseType).GetFields();
            List<DBType> source = new List<DBType>();
            for(int i = 1; i < dbtypes.Length; i ++)
            {
                source.Add(new DBType() { Name = dbtypes[i].Name , Value = (EJ.Databases_dbTypeEnum)(int)dbtypes[i].GetValue(null)});
            }
            cmbDBType.ItemsSource = source;
            root.DataContext = _currentData;
          
        }

        private async void btnOK_Click(object sender, RoutedEventArgs e)
        {
            this.Cursor = Cursors.Wait;
            _currentData.Name = _currentData.Name.ToLower();//统一用小写
            List<string> tableNames = null;
            try
            {
                await Task.Run(() =>
                {
                    var dbservice = Way.EntityDB.Design.DBHelper.CreateDatabaseDesignService((Way.EntityDB.DatabaseType)(int)_currentData.dbType);
                    var db = Way.EntityDB.DBContext.CreateDatabaseService(_currentData.conStr, (Way.EntityDB.DatabaseType)(int)_currentData.dbType);
                    tableNames = dbservice.GetCurrentTableNames(db);

                });
                this.Cursor = null;
                ImportSchema frm = new Forms.ImportSchema(tableNames, _target.id.Value, _currentData);
                frm.Owner = this;
                if (frm.ShowDialog() == true)
                {
                    this.DialogResult = true;
                }
            }
            catch (Exception ex)
            {
                this.Cursor = null;
                Helper.ShowError(this, ex);
            }
        }

        private void cmbDBType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (_currentData.dbType == EJ.Databases_dbTypeEnum.MySql)
                txt_conStr.Text = "server=;User Id=;password=;Database=";
            else if (_currentData.dbType == EJ.Databases_dbTypeEnum.SqlServer)
                txt_conStr.Text = "server=;uid=;pwd=;Database=";
            else if (_currentData.dbType == EJ.Databases_dbTypeEnum.Sqlite)
                txt_conStr.Text = "data source=\"\"";
            else if (_currentData.dbType == EJ.Databases_dbTypeEnum.PostgreSql)
                txt_conStr.Text = "Server=;Port=5432;UserId=;Password=;Database=;";
        }
    }
}
