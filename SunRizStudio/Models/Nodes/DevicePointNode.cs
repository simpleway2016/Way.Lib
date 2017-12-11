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
    class DevicePointNode : SolutionNode
    {
        DevicePoint _point;
        public DevicePointNode(DevicePoint point)
        {
            _point = point;
            this.Text = point.Desc;
            point.PropertyChanged += Point_PropertyChanged;

            this.Icon = "/Images/solution/point.png";
            this.ShowInTree = false;
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
            //先查看是否已经打开document，如果打开，则激活就可以
            foreach ( TabItem item in MainWindow.Instance.documentContainer.Items )
            {
                var doc = item.Content as Documents.BaseDocument;
                if(doc.DataContext is DevicePoint && ((DevicePoint)doc.DataContext).id == _point.id)
                {
                    //active
                    MainWindow.Instance.documentContainer.SelectedItem = item;
                    return;
                }
            }

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

        public override void MouseDown(object sender, MouseButtonEventArgs e)
        {
            FrameworkElement treeviewitem = sender as FrameworkElement;
            DataObject data = new DataObject("Text", new { Type = "Point", Name = this._point.Name }.ToJsonString());
            DragDrop.DoDragDrop(treeviewitem, data, DragDropEffects.Move);
        }
    }
}
