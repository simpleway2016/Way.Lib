using SunRizServer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace SunRizStudio.Models.Nodes
{
    class SystemSettingNode : SolutionNode
    {
        /// <summary>
        /// 实际类型是SystemSetting，因为字段和ControlUnit相似，所以用ControlUnit
        /// </summary>
        ControlUnit Data;
        public SystemSettingNode()
        {
            this.Nodes.Add(new LoadingNode());
        }
        
        /// <summary>
        /// 异步加载子节点
        /// </summary>
        protected override void LoadChildrenAsync()
        {
            
            base.LoadChildrenAsync();
            //实际Model类型是SystemSetting，因为字段和ControlUnit相似，所以用ControlUnit
            Helper.Remote.Invoke<ControlUnit>("GetSystemSetting", (data, err) => {
                if (err != null)
                {
                    MessageBox.Show(MainWindow.Instance, err);
                }
                else
                {
                    this.Nodes.Clear();
                    this.Data = data;
                    this.Nodes.Add(new SolutionNode()
                    {
                        Text = "报警",
                        DoublicClickHandler = alarmSetting_doubleClick,
                    });
                    this.Nodes.Add(new SolutionNode()
                    {
                        Text = "趋势",
                        DoublicClickHandler = trendSetting_doubleClick,
                    });
                    this.Nodes.Add(new SolutionNode()
                    {
                        Text = "MMI",
                        DoublicClickHandler = mmiSetting_doubleClick,
                    });
                    this.Nodes.Add(new SolutionNode()
                    {
                        Text = "历史收集",
                        DoublicClickHandler = history_doubleClick,
                    });
                    this.Nodes.Add(new SolutionNode()
                    {
                        Text = "系统日志",
                        DoublicClickHandler = log_doubleClick,
                    });
                }
            });
        }

        void alarmSetting_doubleClick(object sender, RoutedEventArgs e)
        {
            var unit = this.Data;

            //先查看是否已经打开document，如果打开，则激活就可以
            foreach (TabItem item in MainWindow.Instance.documentContainer.Items)
            {
                var doc = item.Content as Documents.BaseDocument;
                if (item.Content is Documents.UnitAlarmSettingDocument && (item.Content as Documents.UnitAlarmSettingDocument).UpdateMethodName == "UpdateSystemSetting")
                {
                    //active
                    MainWindow.Instance.documentContainer.SelectedItem = item;
                    return;
                }
            }
            if (true)
            {
                var doc = new Documents.UnitAlarmSettingDocument(unit);
                doc.UpdateMethodName = "UpdateSystemSetting";
                MainWindow.Instance.SetActiveDocument(doc);
            }
        }
        void trendSetting_doubleClick(object sender, RoutedEventArgs e)
        {
            var unit = this.Data;

            //先查看是否已经打开document，如果打开，则激活就可以
            foreach (TabItem item in MainWindow.Instance.documentContainer.Items)
            {
                var doc = item.Content as Documents.BaseDocument;
                if (item.Content is Documents.UnitTrendSettingDocument && (item.Content as Documents.UnitTrendSettingDocument).UpdateMethodName == "UpdateSystemSetting")
                {
                    //active
                    MainWindow.Instance.documentContainer.SelectedItem = item;
                    return;
                }
            }
            if (true)
            {
                var doc = new Documents.UnitTrendSettingDocument(unit);
                doc.UpdateMethodName = "UpdateSystemSetting";
                MainWindow.Instance.SetActiveDocument(doc);
            }
        }
        void mmiSetting_doubleClick(object sender, RoutedEventArgs e)
        {
            var unit = this.Data;

            //先查看是否已经打开document，如果打开，则激活就可以
            foreach (TabItem item in MainWindow.Instance.documentContainer.Items)
            {
                var doc = item.Content as Documents.BaseDocument;
                if (item.Content is Documents.UnitMMISettingDocument && (item.Content as Documents.UnitMMISettingDocument).UpdateMethodName == "UpdateSystemSetting")
                {
                    //active
                    MainWindow.Instance.documentContainer.SelectedItem = item;
                    return;
                }
            }
            if (true)
            {
                var doc = new Documents.UnitMMISettingDocument(unit);
                doc.UpdateMethodName = "UpdateSystemSetting";
                MainWindow.Instance.SetActiveDocument(doc);
            }
        }

        void history_doubleClick(object sender, RoutedEventArgs e)
        {
            //先查看是否已经打开document，如果打开，则激活就可以
            foreach (TabItem item in MainWindow.Instance.documentContainer.Items)
            {
                var doc = item.Content as Documents.BaseDocument;
                if (item.Content is Documents.HistoryStorageSettingDocument)
                {
                    //active
                    MainWindow.Instance.documentContainer.SelectedItem = item;
                    return;
                }
            }
            if (true)
            {
                var doc = new Documents.HistoryStorageSettingDocument();
                MainWindow.Instance.SetActiveDocument(doc);
            }
        }

        void log_doubleClick(object sender, RoutedEventArgs e)
        {
            //先查看是否已经打开document，如果打开，则激活就可以
            foreach (TabItem item in MainWindow.Instance.documentContainer.Items)
            {
                var doc = item.Content as Documents.BaseDocument;
                if (item.Content is Documents.LogStorageSettingDocument)
                {
                    //active
                    MainWindow.Instance.documentContainer.SelectedItem = item;
                    return;
                }
            }
            if (true)
            {
                var doc = new Documents.LogStorageSettingDocument();
                MainWindow.Instance.SetActiveDocument(doc);
            }
        }
    }
}
