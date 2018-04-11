

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
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
    /// DatabaseUpdate.xaml 的交互逻辑
    /// </summary>
    public partial class DatabaseUpdate : Window
    {
        class history
        {
            public string ConStr;
        }
        public DatabaseUpdate()
        {
            InitializeComponent();
            txtInfo.Document.LineHeight = 5;
            try
            {
                if (File.Exists(AppDomain.CurrentDomain.BaseDirectory + "history.txt"))
                {
                    var history = File.ReadAllText(AppDomain.CurrentDomain.BaseDirectory + "history.txt").ToJsonObject<history>();
                    txtConStr.Text = history.ConStr;
                }
            }
            catch
            {
            }

            string[] typenames = Enum.GetNames(typeof(EJ.Databases_dbTypeEnum));
            foreach (string tn in typenames)
            {
                cmbDBType.Items.Add(tn);
            }
        }

        private void txtFileName_TextChanged(object sender, TextChangedEventArgs e)
        {
            btnStart.IsEnabled = txtFileName.Text.Trim().Length > 0 && txtConStr.Text.Trim().Length > 0;
        }

        private void btnSelectFile_Click(object sender, RoutedEventArgs e)
        {
            using (System.Windows.Forms.OpenFileDialog fd = new System.Windows.Forms.OpenFileDialog())
            {
                fd.Filter = "*.action|*.action";
                if (fd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    txtFileName.Text = fd.FileName; 
                    using (var dset = new System.Data.DataSet())
                    {
                        dset.ReadXml(fd.FileName);
                        cmbDBType.SelectedValue = dset.Tables[0].TableName;
                    }
                }
            }
        }

        protected override void OnClosing(System.ComponentModel.CancelEventArgs e)
        {
            if (btnOK.IsEnabled == false)
                e.Cancel = true;

            base.OnClosing(e);
        }

        string m_filePath;
        string m_conStr;
        string m_currentDBType;
        private void btnStart_Click(object sender, RoutedEventArgs e)
        {
            m_currentDBType = cmbDBType.SelectedValue.ToString();
            m_filePath = txtFileName.Text.Trim();
            m_conStr = txtConStr.Text.Trim();
            txtInfo.Document.Blocks.Clear();
            txtInfo.AppendText(DateTime.Now.ToString() + "\r\n");
            lblStatus.Content = "正在更新数据库结构...";
            txtConStr.IsEnabled = false;
            txtFileName.IsEnabled = false;
            btnSelectFile.IsEnabled = false;
            btnStart.IsEnabled = false;
            btnOK.IsEnabled = false;

            File.WriteAllText(AppDomain.CurrentDomain.BaseDirectory + "history.txt", new history()
            {
                ConStr = m_conStr
            }.ToJsonString());

            new Thread(run).Start();
        }
        void finish()
        {
            lblStatus.Content = "执行完毕！";
            txtConStr.IsEnabled = true;
            txtFileName.IsEnabled = true;
            btnSelectFile.IsEnabled = true;
            btnStart.IsEnabled = true;
            btnOK.IsEnabled = true;
        }
        void setOutputText(string text)
        {
            this.Dispatcher.Invoke(new Action(() =>
            {
                txtInfo.AppendText(text + "\r\n");
            }));
        }
        void run()
        {
            //try
            //{
            //    using (var dset = new System.Data.DataSet())
            //    {
            //        dset.ReadXml(m_filePath);

            //        System.Data.DataTable dtable = dset.Tables[0];
            //        System.Web.Script.Serialization.JavaScriptSerializer jsonObj = new System.Web.Script.Serialization.JavaScriptSerializer();
            //        var db = Way.EntityDB.DBContext.CreateDatabaseService(m_conStr, (Way.EntityDB.DatabaseType)Enum.Parse(typeof(Way.EntityDB.DatabaseType), m_currentDBType));

            //        IDatabaseDesignService dbservice = Way.EntityDB.Design.DBHelper.CreateDatabaseDesignService(db.DBContext.DatabaseType);
                   
            //        if (dbservice == null)
            //            throw new Exception("无法初始化IDatabaseService for " + m_currentDBType);
            //        dbservice.CreateEasyJobTable(db);

            //        var dbconfig = db.ExecSqlString("select contentConfig from __WayEasyJob").ToString().ToJsonObject<DataBaseConfig>();
            //        if (dbconfig.DatabaseGuid.IsNullOrEmpty() == false && dbconfig.DatabaseGuid != dset.DataSetName)
            //            throw new Exception("此结构脚本并不是对应此数据库");


            //        db.DBContext.BeginTransaction();
            //        try
            //        {
            //            dtable.DefaultView.RowFilter = "id>" + dbconfig.LastUpdatedID;
            //            dtable.DefaultView.Sort = "id";
            //            int count = dtable.DefaultView.Count;
            //            int done = 0;
            //            int? lastid = null;
            //            foreach (System.Data.DataRowView datarow in dtable.DefaultView)
            //            {
            //                string actionType = datarow["type"].ToString();
            //                int id = Convert.ToInt32( datarow["id"]);

            //                string json = datarow["content"].ToString();
                           

            //                Type type = typeof(Way.EntityDB.Design.Actions.Action).Assembly.GetType(actionType);
            //                var actionItem = (Way.EntityDB.Design.Actions.Action)jsonObj.Deserialize(json, type);

            //                setOutputText(string.Format("{0}、{1}", id, actionItem));
            //                actionItem.Invoke(db);

            //                done++;
            //                lastid = id;
            //                this.Dispatcher.Invoke(() =>
            //                {

            //                    progressBar.Maximum = count;
            //                    progressBar.Value = Math.Min(done, count);
            //                });
            //            }
            //            if (lastid != null)
            //            {
            //                Way.EntityDB.Design.DBUpgrade.SetLastUpdateID(lastid.Value,  dset.DataSetName , db);
            //            }
            //            db.DBContext.CommitTransaction();
            //        }
            //        catch (Exception ex)
            //        {
            //            db.DBContext.RollbackTransaction();
            //            throw ex;
            //        }
            //    }
            //}
            //catch (Exception ex)
            //{
            //    if (ex.InnerException != null)
            //    {
            //        setOutputText("Error:" + ex.Message + "\r\n" + ex.InnerException.Message);
            //    }
            //    else
            //    {
            //        setOutputText("Error:" + ex.Message);
            //    }
            //}
            //finally
            //{
            //    this.Dispatcher.Invoke(new Action(() =>
            //    {
            //        finish();
            //    }));
            //}
        }

        private void btnOK_Click_1(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
