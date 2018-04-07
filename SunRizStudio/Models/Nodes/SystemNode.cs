using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SunRizStudio.Models.Nodes
{
    /// <summary>
    /// 【系统】节点
    /// </summary>
    class SystemNode : SolutionNode
    {
        public SystemNode()
        {
            Icon = "/Images/solution/system.png";
        }
        /// <summary>
        /// 初始化子节点
        /// </summary>
        public override void InitChildren()
        {
            base.InitChildren();

            var authorizeNode = new SolutionNode() {
                Text = "授权管理",
                Icon = "/Images/solution/key.png"
            };
            this.Nodes.Add(authorizeNode);

            var userNode = new SolutionNode()
            {
                Text = "用户权限管理",
                Icon = "/Images/solution/users.png",
                DoublicClickHandler = (s, e) => {
                    MainWindow.Instance.ShowUserMgrWindow();
                },
            };
            this.Nodes.Add(userNode);

            var alarmNode = new SolutionNode()
            {
                Text = "报警查询",
                Icon = "/Images/solution/alarm.png",
                DoublicClickHandler = (s, e) => {
                    MainWindow.Instance.ShowAlarmWindow();
                },

            };
            this.Nodes.Add(alarmNode);

            var historyNode = new SolutionNode()
            {
                Text = "历史查询",
                Icon = "/Images/solution/history.png",
                DoublicClickHandler = (s, e) => {
                    MainWindow.Instance.ShowHistoryWindow();
                },

            };
            this.Nodes.Add(historyNode);


            var logNode = new SolutionNode()
            {
                Text = "系统日志查询",
                Icon = "/Images/solution/log.png",
                DoublicClickHandler = (s, e) => {
                    MainWindow.Instance.ShowSysLogWindow();
                },

            };
            this.Nodes.Add(logNode);

            var setttingNode = new SystemSettingNode()
            {
                Text = "系统配置",
                Icon = "/Images/solution/setting.png",                
            };
            this.Nodes.Add(setttingNode);

            var drviersNode = new DriversNode()
            {
                Text = "通讯网关"
            };
            this.Nodes.Add(drviersNode);

            var videoWindowNode = new ControlWindowContainerNode(0 , null)
            {
                Text = "监视画面"                
            };
            this.Nodes.Add(videoWindowNode);

            var controlUnitNode = new ControlUnitParentNode();
            this.Nodes.Add(controlUnitNode);
        }
    }
}
