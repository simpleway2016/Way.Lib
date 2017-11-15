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
                        node.DoublicClickHandler = editUnitClick;
                        this.Nodes.Add(node);
                    }
                }
            });
        }

        void editUnitClick(object sender, MouseButtonEventArgs e)
        {
            TreeViewItem treeviewitem = sender as TreeViewItem;
            var curNode = treeviewitem.DataContext as ControlUnitNode;
            Dialogs.InputBox frm = new Dialogs.InputBox("请输入新单元名称", "编辑控制单元");
            frm.Owner = MainWindow.Instance;
            frm.Value = curNode.Data.Name;
            if (frm.ShowDialog() == true && !string.IsNullOrEmpty(frm.Value))
            {
                SunRizServer.ControlUnit unit = curNode.Data.Clone<SunRizServer.ControlUnit>();
                unit.Name = frm.Value;
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
            Dialogs.InputBox frm = new Dialogs.InputBox("请输入单元名称" , "新建控制单元");
            frm.Owner = MainWindow.Instance;
            if(frm.ShowDialog() == true && !string.IsNullOrEmpty(frm.Value))
            {
                var unit = new SunRizServer.ControlUnit();
                unit.Name = frm.Value;
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
                        node.DoublicClickHandler = editUnitClick;
                        this.Nodes.Add(node);
                    }
                }, unit);
            }
        }
    }
}
