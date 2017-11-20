using SunRizServer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace SunRizStudio.Models.Nodes
{
    class DevicePointNode : SolutionNode
    {
        DevicePoint _point;
        public DevicePointNode(DevicePoint point)
        {
            _point = point;
            this.Text = point.Desc;
            point.PropertyChanged += Point_PropertyChanged;

            this.Icon = "/Images/solution/point.png";
            this.DoublicClickHandler = doubleClick;
        }

        private void Point_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
             if(e.PropertyName == "Desc")
            {
                this.Text = _point.Desc;
            }
        }

        void doubleClick(object sender, RoutedEventArgs e)
        {
            if (_point.Type == DevicePoint_TypeEnum.Analog)
            {
                var doc = new Documents.AnalogPointDocument(this.GetDevice(), this.Parent, _point, _point.FolderId.Value);
                MainWindow.Instance.SetActiveDocument(doc);
            }
            else
            {
                var doc = new Documents.DigitalPointDocument(this.GetDevice(), this.Parent, _point, _point.FolderId.Value);
                MainWindow.Instance.SetActiveDocument(doc);
            }
        }
        Device GetDevice()
        {
            //加载子节点
            //找到DeviceNode
            SolutionNode parentNode = this.Parent;
            while (!(parentNode is DeviceNode))
                parentNode = parentNode.Parent;

            //获取Device对象
            return ((DeviceNode)parentNode).Data;

        }
    }
}
