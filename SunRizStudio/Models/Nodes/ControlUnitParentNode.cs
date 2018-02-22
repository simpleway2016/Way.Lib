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
    /// 【控制单元】
    /// </summary>
    class ControlUnitParentNode : SolutionNode
    {

        public ControlUnitParentNode()
        {
            //设置右键菜单
            this.ContextMenuItems.Add(new ContextMenuItem()
            {
                Text = "添加控制单元",
                ClickHandler = addControlUnitClick,
            });

            bindChilds();
        }

        /// <summary>
        /// 绑定单元列表
        /// </summary>
        void bindChilds()
        {
            this.Text = "控制单元 Loading...";
            Helper.Remote.Invoke<SunRizServer.ControlUnit[]>("GetUnitList", (ret, err) => {
                this.Text = "控制单元";
                if (err != null)
                {
                    MessageBox.Show(MainWindow.Instance, err);
                }
                else
                {
                    foreach (var unit in ret)
                    {
                        ControlUnitNode node = new ControlUnitNode(unit);
                        node.ContextMenuItems.Add(new ContextMenuItem()
                        {
                            Text = "重命名",
                            ClickHandler = editUnitClick,
                            Tag = node,
                        });
                        this.Nodes.Add(node);
                    }
                }
            });
        }

        void editUnitClick(object sender, RoutedEventArgs e)
        {
            FrameworkElement treeviewitem = sender as FrameworkElement;
            var curNode = (treeviewitem.DataContext as ContextMenuItem).Tag as ControlUnitNode;
            Dialogs.InputBox frm = new Dialogs.InputBox("请输入单元编号，如01、02等数字编号形式", "编辑控制单元");
            frm.Owner = MainWindow.Instance;
            var match = System.Text.RegularExpressions.Regex.Match(curNode.Data.Name, @"[0-9]+");
            frm.Value = match.Value;
            if (frm.ShowDialog() == true && !string.IsNullOrEmpty(frm.Value))
            {
                match = System.Text.RegularExpressions.Regex.Match(frm.Value, @"[0-9]+");
                if (match.Value != frm.Value)
                {
                    MessageBox.Show(MainWindow.Instance, "必须是01、02等数字编号形式");
                    editUnitClick(sender, e);
                    return;
                }

                SunRizServer.ControlUnit unit = curNode.Data.Clone<SunRizServer.ControlUnit>();
                unit.Name ="UNIT" + frm.Value;
                Helper.Remote.Invoke<int>("UpdateControlUnit", (ret, err) => {
                    if (err != null)
                    {
                        MessageBox.Show(MainWindow.Instance, err);
                    }
                    else
                    {
                        unit.ChangedProperties.Clear();
                        curNode.Data = unit;
                    }
                }, unit);
            }
        }

        void addControlUnitClick(object sender, RoutedEventArgs e)
        {
            Dialogs.InputBox frm = new Dialogs.InputBox("请输入单元编号，如01、02等数字编号形式", "新建控制单元");
            frm.Owner = MainWindow.Instance;
            if(frm.ShowDialog() == true && !string.IsNullOrEmpty(frm.Value))
            {
                var match = System.Text.RegularExpressions.Regex.Match(frm.Value, @"[0-9]+");
                if ( match.Value != frm.Value )
                {
                    MessageBox.Show(MainWindow.Instance, "必须是01、02等数字编号形式");
                    addControlUnitClick(sender, e);
                    return;
                }
                var unit = new SunRizServer.ControlUnit();
                unit.Name = "UNIT" + frm.Value;
                MainWindow.Instance.Cursor = Cursors.Hand;

                //异步调用服务器方法
                Helper.Remote.Invoke<int>("UpdateControlUnit", (ret, err) => {
                    MainWindow.Instance.Cursor = null;
                    if (err != null)
                    {
                        MessageBox.Show(MainWindow.Instance, err);
                    }
                    else
                    {
                        unit.id = ret;
                        unit.ChangedProperties.Clear();
                        this.IsExpanded = true;
                        ControlUnitNode node = new ControlUnitNode(unit);
                        node.ContextMenuItems.Add(new ContextMenuItem()
                        {
                            Text = "重命名",
                            ClickHandler = editUnitClick,
                            Tag = node,
                        });
                        this.Nodes.Add(node);
                    }
                }, unit);
            }
        }
    }
}
