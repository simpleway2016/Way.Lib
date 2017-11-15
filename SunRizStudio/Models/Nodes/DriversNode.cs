using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace SunRizStudio.Models.Nodes
{
    /// <summary>
    /// 【通讯驱动】
    /// </summary>
    class DriversNode : SolutionNode
    {
        public DriversNode()
        {
            this.Icon = "/Images/solution/device.png";

            //设置右键菜单
            this.ContextMenuItems.Add(new ContextMenuItem() {
                Text = "添加通讯网关",
                Icon = "/Images/solution/gateway.png",
                ClickHandler = addGatewayClick,
            });
        }

        /// <summary>
        /// 添加通讯网关 click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void addGatewayClick(object sender, RoutedEventArgs e)
        {
            Dialogs.GatewayDialog dialog = new Dialogs.GatewayDialog();
            dialog.Owner = MainWindow.Instance;
            dialog.Title = "添加通讯网关";
            if (dialog.ShowDialog() == true)
            {
                var data = dialog.Data;
                this.Nodes.Add(new Models.GatewayNode(data));
                this.IsExpanded = true;
            }
        }

        /// <summary>
        /// 初始化子菜单
        /// </summary>
        public override void InitChildren()
        {
            base.InitChildren();

            Helper.Remote.Invoke<SunRizServer.CommunicationDriver[]>("GetGatewayList", (datas, err) =>
            {
                if (err != null)
                    MessageBox.Show(MainWindow.Instance, err);
                else
                {
                    foreach (var gateway in datas)
                    {
                        this.Nodes.Add(new Models.GatewayNode(gateway));
                    }
                }
            });
        }
    }
}
