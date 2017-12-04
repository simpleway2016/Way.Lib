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
            bindChilds();
        }

        void addDeviceClick(object sender, RoutedEventArgs e)
        {
            Dialogs.InputBox frm = new Dialogs.InputBox("请输入控制器名称", "添加控制器");
            frm.Owner = MainWindow.Instance;
            if (frm.ShowDialog() == true && !string.IsNullOrEmpty(frm.Value))
            {
                var device = new SunRizServer.Device();
                device.Name = frm.Value;
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
                        node.DoublicClickHandler = editUnitClick;
                        this.Nodes.Add(node);
                    }
                }, device);
            }
        }

        /// <summary>
        /// 绑定子节点
        /// </summary>
        void bindChilds()
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
                });
                settingNode.Nodes.Add(new SolutionNode()
                {
                    Text = "MMI",
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
                        node.DoublicClickHandler = editUnitClick;
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

        void editUnitClick(object sender, MouseButtonEventArgs e)
        {
            TreeViewItem treeviewitem = sender as TreeViewItem;
            var curNode = treeviewitem.DataContext as DeviceNode;
            Dialogs.InputBox frm = new Dialogs.InputBox("请输入新控制器名称", "编辑控制器");
            frm.Owner = MainWindow.Instance;
            frm.Value = curNode.Data.Name;
            if (frm.ShowDialog() == true && !string.IsNullOrEmpty(frm.Value))
            {
                var device = curNode.Data.Clone<SunRizServer.Device>();
                device.Name = frm.Value;
              
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
