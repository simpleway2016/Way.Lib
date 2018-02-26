using SunRizServer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace SunRizStudio.Models.Nodes
{
    /// <summary>
    /// 【单元】
    /// </summary>
    class ControlUnitNode : SolutionNode
    {
        SunRizServer.ControlUnit _Data;
        public SunRizServer.ControlUnit Data
        {
            get
            {
                return _Data;
            }
            set
            {
                if(_Data != value)
                {
                    _Data = value;
                    //更新节点text
                    this.Text = value.Name;
                }
            }
        }
        public ControlUnitNode(SunRizServer.ControlUnit data)
        {
            this.Data = data;

            //设置右键菜单
            this.ContextMenuItems.Add(new ContextMenuItem()
            {
                Text = "添加控制器",
                ClickHandler = addDeviceClick,
            });

            //绑定子节点
            bindChildren();
        }

        void addDeviceClick(object sender, RoutedEventArgs e)
        {
            Dialogs.InputBox frm = new Dialogs.InputBox("请输入控制器编号，如01、02等数字编号形式", "添加控制器");
            frm.Owner = MainWindow.Instance;
            if (frm.ShowDialog() == true && !string.IsNullOrEmpty(frm.Value))
            {
                var match = System.Text.RegularExpressions.Regex.Match(frm.Value, @"[0-9]+");
                if (match.Value != frm.Value)
                {
                    MessageBox.Show(MainWindow.Instance, "必须是01、02等数字编号形式");
                    addDeviceClick(sender, e);
                    return;
                }
                var device = new SunRizServer.Device();
                device.Name = "DROP" + frm.Value;
                device.UnitId = this.Data.id;
                Helper.Remote.Invoke<int>("UpdateDevice", (ret, err) => {
                    if (err != null)
                    {
                        MessageBox.Show(MainWindow.Instance, err);
                    }
                    else
                    {
                        device.id = ret;
                        device.ChangedProperties.Clear();
                        this.IsExpanded = true;
                        var node = new DeviceNode(device);
                        node.ContextMenuItems.Add(new ContextMenuItem()
                        {
                            Text = "重命名",
                            ClickHandler = editDeviceClick,
                            Tag = node,
                        });
                        this.Nodes.Add(node);
                    }
                }, device);
            }
        }

        /// <summary>
        /// 绑定子节点
        /// </summary>
        void bindChildren()
        {
            var settingNode = new SolutionNode()
            {
                Text = "单元配置",
            };
            if(true)
            {
                settingNode.Nodes.Add(new SolutionNode()
                {
                    Text = "报警",
                    DoublicClickHandler = alarmSetting_doubleClick,
                });
                settingNode.Nodes.Add(new SolutionNode()
                {
                    Text = "趋势",
                    DoublicClickHandler = trendSetting_doubleClick,
                });
                settingNode.Nodes.Add(new SolutionNode()
                {
                    Text = "MMI",
                    DoublicClickHandler = mmiSetting_doubleClick,
                });
            }
            this.Nodes.Add(settingNode);

            this.Nodes.Add(new ControlWindowContainerNode(this.Data.id.Value , null)
            {
                Text = "监视画面",
            });

            this.Text = this.Data.Name + " Loading...";
            Helper.Remote.Invoke<SunRizServer.Device[]>("GetDeviceList", (ret, err) => {
                this.Text = this.Data.Name;
                if (err != null)
                {
                    MessageBox.Show(MainWindow.Instance, err);
                }
                else
                {
                    foreach (var data in ret)
                    {
                        var node = new DeviceNode(data);
                        node.ContextMenuItems.Add(new ContextMenuItem() {
                            Text = "重命名",
                            ClickHandler = editDeviceClick,
                            Tag = node,
                        });
                        this.Nodes.Add(node);
                    }
                }
            },this.Data.id);
        }

        void alarmSetting_doubleClick(object sender, RoutedEventArgs e)
        {
            var unit = this.Data;

            //先查看是否已经打开document，如果打开，则激活就可以
            foreach (TabItem item in MainWindow.Instance.documentContainer.Items)
            {
                var doc = item.Content as Documents.BaseDocument;
                if (item.Content is Documents.UnitAlarmSettingDocument && ((ControlUnit)doc.DataContext).id == unit.id)
                {
                    //active
                    MainWindow.Instance.documentContainer.SelectedItem = item;
                    return;
                }
            }
            if (true)
            {
                var doc = new Documents.UnitAlarmSettingDocument(unit);
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
                if (item.Content is Documents.UnitTrendSettingDocument && ((ControlUnit)doc.DataContext).id == unit.id)
                {
                    //active
                    MainWindow.Instance.documentContainer.SelectedItem = item;
                    return;
                }
            }
            if (true)
            {
                var doc = new Documents.UnitTrendSettingDocument(unit);
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
                if (item.Content is Documents.UnitMMISettingDocument && ((ControlUnit)doc.DataContext).id == unit.id)
                {
                    //active
                    MainWindow.Instance.documentContainer.SelectedItem = item;
                    return;
                }
            }
            if (true)
            {
                var doc = new Documents.UnitMMISettingDocument(unit);
                MainWindow.Instance.SetActiveDocument(doc);
            }
        }
        void editDeviceClick(object sender, RoutedEventArgs e)
        {
            FrameworkElement treeviewitem = sender as FrameworkElement;
            var curNode = ((ContextMenuItem)treeviewitem.DataContext).Tag as DeviceNode;
            Dialogs.InputBox frm = new Dialogs.InputBox("请输入新控制器编号，如01、02等数字编号形式", "编辑控制器");
            frm.Owner = MainWindow.Instance;
            var match = System.Text.RegularExpressions.Regex.Match(curNode.Data.Name, @"[0-9]+");
            frm.Value = match.Value;
            if (frm.ShowDialog() == true && !string.IsNullOrEmpty(frm.Value))
            {
                match = System.Text.RegularExpressions.Regex.Match(frm.Value, @"[0-9]+");
                if (match.Value != frm.Value)
                {
                    MessageBox.Show(MainWindow.Instance, "必须是01、02等数字编号形式");
                    editDeviceClick(sender, e);
                    return;
                }

                var device = curNode.Data.Clone<SunRizServer.Device>();
                device.Name = "DROP" + frm.Value;
              
                MainWindow.Instance.Cursor = Cursors.Hand;

                //异步调用服务器方法
                Helper.Remote.Invoke<int>("UpdateDevice", (ret, err) => {
                    MainWindow.Instance.Cursor = null;
                    if (err != null)
                    {
                        MessageBox.Show(MainWindow.Instance, err);
                    }
                    else
                    {
                        device.ChangedProperties.Clear();
                        curNode.Data = device;
                    }
                }, device);
            }
        }
    }
}
