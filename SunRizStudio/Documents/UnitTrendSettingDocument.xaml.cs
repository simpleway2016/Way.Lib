using SunRizServer;
using SunRizStudio.Models;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SunRizStudio.Documents
{
    public partial class UnitTrendSettingDocument : BaseDocument
    {
        public string UpdateMethodName = "UpdateControlUnit";
        ControlUnit _originalModel;
        public UnitTrendSettingDocument()
        {
            InitializeComponent();
        }
        internal UnitTrendSettingDocument( ControlUnit dataModel)
        {
            _originalModel = dataModel;
            InitializeComponent();
            this.Title = $"“{dataModel.Name}”配置-趋势";
            this.DataContext = _originalModel.Clone<ControlUnit>();
        }

        public override void OnClose(ref bool canceled)
        {
            if(((ControlUnit)this.DataContext).ChangedProperties.Count > 0)
            {
                var dialogResult = MessageBox.Show(MainWindow.Instance, "窗口已修改，是否保存？", "", MessageBoxButton.YesNoCancel);
                if (dialogResult == MessageBoxResult.Yes)
                {
                    save(false);
                }
                else if (dialogResult == MessageBoxResult.Cancel)
                {
                    canceled = true;
                    return;
                }
            }
            base.OnClose(ref canceled);
        }

        private void btnOK_Click(object sender, RoutedEventArgs e)
        {
            save(true);
        }

        private void btnApply_Click(object sender, RoutedEventArgs e)
        {
            save(false);
        }

        void save(bool closeOnSuccess)
        {
            this.Cursor = Cursors.Hand;

            Helper.Remote.Invoke<int>(this.UpdateMethodName, (ret, err) => {
                this.Cursor = null;
                if (err != null)
                {
                    MessageBox.Show(MainWindow.Instance, err);
                }
                else
                {
                    ControlUnit unit = this.DataContext as ControlUnit;
                    unit.ChangedProperties.Clear();
                    _originalModel.CopyValue(unit);
                    _originalModel.ChangedProperties.Clear();

                    if (closeOnSuccess)
                    {
                        //关闭当前document
                        MainWindow.Instance.CloseDocument(this);
                    }
                }
            }, this.DataContext);
        }


        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            //关闭当前document
            MainWindow.Instance.CloseDocument(this);
        }

        private void btnRefresh_Click(object sender, RoutedEventArgs e)
        {
            this.DataContext = this._originalModel.Clone<ControlUnit>();
        }
    }
}
