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
        /// <summary>
        /// 初始化子节点
        /// </summary>
        public override void InitChildren()
        {
            base.InitChildren();

            var authorizeNode = new SolutionNode() {
                Text = "授权管理"
            };
            this.Nodes.Add(authorizeNode);

            var userNode = new SolutionNode()
            {
                Text = "用户权限管理"
            };
            this.Nodes.Add(userNode);

            var setttingNode = new SystemSettingNode()
            {
                Text = "系统配置"
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
