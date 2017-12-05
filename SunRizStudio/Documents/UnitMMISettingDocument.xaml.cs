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
    public partial class UnitMMISettingDocument : BaseDocument
    {
        ControlUnit _originalModel;
        public UnitMMISettingDocument()
        {
            InitializeComponent();
        }
        internal UnitMMISettingDocument( ControlUnit dataModel)
        {
            _originalModel = dataModel;
            InitializeComponent();
            this.Title = $"“{dataModel.Name}”配置-MMI";
            this.DataContext = _originalModel.Clone<ControlUnit>();
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

            Helper.Remote.Invoke<int>("UpdateControlUnit", (ret, err) => {
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
