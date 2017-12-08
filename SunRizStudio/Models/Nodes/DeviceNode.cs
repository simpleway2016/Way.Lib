using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SunRizStudio.Models.Nodes
{
    class DeviceNode : SolutionNode
    {
        SunRizServer.Device _Data;
        public SunRizServer.Device Data
        {
            get
            {
                return _Data;
            }
            set
            {
                if (_Data != value)
                {
                    _Data = value;
                    //更新节点text
                    this.Text = value.Name;
                }
            }
        }
        public DeviceNode(SunRizServer.Device data)
        {
            this.Data = data;

            //绑定子节点
            bindChilds();
        }

        void bindChilds()
        {
            this.Nodes.Add(new SolutionNode() {
                Text = "配置",
                DoublicClickHandler = (s, e) => {
                    var dialog = new Dialogs.DeviceDialog();
                    dialog.Device = this.Data.Clone<SunRizServer.Device>();
                    dialog.Owner = MainWindow.Instance;
                    if(dialog.ShowDialog() == true)
                    {
                        this.Data = dialog.Device;
                    }
                }
            });


            var tagNode = new SolutionNode();
            tagNode.Text = "标签";
            this.Nodes.Add(tagNode);

            tagNode.Nodes.Add(new AnalogNode(null) { Text = "模拟量"});
            tagNode.Nodes.Add(new DigitalNode(null) { Text = "开关量" });

            this.Nodes.Add(new SolutionNode()
            {
                Text = "控制任务"
            });
        }
    }
}
