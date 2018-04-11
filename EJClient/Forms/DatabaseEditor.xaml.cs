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
    public partial class DatabaseEditor : Window
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
        EJ.Databases _currentData;
        public DatabaseEditor()
        {
            InitializeComponent();
        }
        public DatabaseEditor(int projectid):this(projectid , null)
        {
        }
        public DatabaseEditor(int projectid,EJ.Databases database)
        {
            InitializeComponent();
            _projectID = projectid;
            _currentData = database;
            if (_currentData == null)
            {
                _currentData = new EJ.Databases();
                _currentData.Guid = Guid.NewGuid().ToString();
                _currentData.ProjectID = projectid;
                _currentData.conStr = "server=;uid=;pwd=;Database=";
            }

            var dbtypes = typeof(EJ.Databases_dbTypeEnum).GetFields();
            List<DBType> source = new List<Forms.DatabaseEditor.DBType>();
            for(int i = 1; i < dbtypes.Length; i ++)
            {
                source.Add(new DBType() { Name = dbtypes[i].Name , Value = (EJ.Databases_dbTypeEnum)(int)dbtypes[i].GetValue(null)});
            }
            cmbDBType.ItemsSource = source;
            root.DataContext = _currentData;
          
        }

        private void btnOK_Click(object sender, RoutedEventArgs e)
        {
            this.Cursor = Cursors.Wait;
            Helper.Client.Invoke<int>("UpdateDatabase", (id,err)=> {
                this.Cursor = null;
                if (err != null)
                    Helper.ShowError(this , err);
                else
                {
                    this.DialogResult = true;
                }
            } , _currentData);
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
