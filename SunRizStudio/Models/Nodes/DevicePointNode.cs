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

            this.ContextMenuItems.Add(new ContextMenuItem() {
                ClickHandler = (s,e)=>doubleClick(s,e),
                Text = "属性"
            });

            this.ContextMenuItems.Add(new ContextMenuItem()
            {
                ClickHandler = (s, e) => transformTest(),
                Text = "值转换测试(正向)..."
            });

            this.ContextMenuItems.Add(new ContextMenuItem()
            {
                ClickHandler = (s, e) => transformBackTest(),
                Text = "值转换测试(反向)..."
            });
        }

        void transformTest()
        {
            Dialogs.InputBox frm = new Dialogs.InputBox("请输入初始值", "值转换测试");
            frm.Owner = MainWindow.Instance;
            if (frm.ShowDialog() == true)
            {
                if (!string.IsNullOrEmpty(frm.Value))
                {
                    var detail = new Newtonsoft.Json.Linq.JObject();
                    detail["IsSquare"] = this._point.IsSquare.GetValueOrDefault();
                    detail["IsTransform"] = this._point.IsTransform.GetValueOrDefault();
                    detail["IsLinear"] = this._point.IsLinear.GetValueOrDefault();
                    detail["DPCount"] = this._point.DPCount;
                    if (this._point.IsTransform == true)
                    {
                        detail["SensorMax"] = this._point.SensorMax;
                        detail["SensorMin"] = this._point.SensorMin;
                    }
                    if (this._point.IsLinear == true)
                    {
                        detail["LinearX1"] = this._point.LinearX1;
                        detail["LinearX2"] = this._point.LinearX2;
                        detail["LinearX3"] = this._point.LinearX3;
                        detail["LinearX4"] = this._point.LinearX4;
                        detail["LinearX5"] = this._point.LinearX5;
                        detail["LinearX6"] = this._point.LinearX6;
                        detail["LinearY1"] = this._point.LinearY1;
                        detail["LinearY2"] = this._point.LinearY2;
                        detail["LinearY3"] = this._point.LinearY3;
                        detail["LinearY4"] = this._point.LinearY4;
                        detail["LinearY5"] = this._point.LinearY5;
                        detail["LinearY6"] = this._point.LinearY6;
                    }
                    var point = new Newtonsoft.Json.Linq.JObject();
                    point["detail"] = detail;
                    point["max"] = this._point.TransMax;
                    point["min"] = this._point.TransMin;
                    var value = SunRizDriver.Helper.Transform(point, frm.Value);
                    MessageBox.Show(MainWindow.Instance, $"转换后的值为：{value}");
                }
            }
        }
        void transformBackTest()
        {
            Dialogs.InputBox frm = new Dialogs.InputBox("请输入值", "值转换测试（反向）");
            frm.Owner = MainWindow.Instance;
            if (frm.ShowDialog() == true)
            {
                if (!string.IsNullOrEmpty(frm.Value))
                {
                    var detail = new Newtonsoft.Json.Linq.JObject();
                    detail["IsSquare"] = this._point.IsSquare.GetValueOrDefault();
                    detail["IsTransform"] = this._point.IsTransform.GetValueOrDefault();
                    detail["IsLinear"] = this._point.IsLinear.GetValueOrDefault();
                    detail["DPCount"] = this._point.DPCount;
                    if (this._point.IsTransform == true)
                    {
                        detail["SensorMax"] = this._point.SensorMax;
                        detail["SensorMin"] = this._point.SensorMin;
                    }
                    if (this._point.IsLinear == true)
                    {
                        detail["LinearX1"] = this._point.LinearX1;
                        detail["LinearX2"] = this._point.LinearX2;
                        detail["LinearX3"] = this._point.LinearX3;
                        detail["LinearX4"] = this._point.LinearX4;
                        detail["LinearX5"] = this._point.LinearX5;
                        detail["LinearX6"] = this._point.LinearX6;
                        detail["LinearY1"] = this._point.LinearY1;
                        detail["LinearY2"] = this._point.LinearY2;
                        detail["LinearY3"] = this._point.LinearY3;
                        detail["LinearY4"] = this._point.LinearY4;
                        detail["LinearY5"] = this._point.LinearY5;
                        detail["LinearY6"] = this._point.LinearY6;
                    }
                    var point = new Newtonsoft.Json.Linq.JObject();
                    point["detail"] = detail;
                    point["max"] = this._point.TransMax;
                    point["min"] = this._point.TransMin;
                    var value = SunRizDriver.Helper.GetRealValue(point, frm.Value);
                    MessageBox.Show(MainWindow.Instance, $"转换后的值为：{value}");
                }
            }
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
            if (e.ChangedButton == MouseButton.Left)
            {
                //记录鼠标按下的位置
                FrameworkElement treeviewitem = sender as FrameworkElement;
                var window = treeviewitem.GetParentByName<Window>(null);
                _downPoint = e.GetPosition(window);
                _mouseDowned = true;
            }
        }

        bool _mouseDowned = false;
        Point _downPoint;
        public override void MouseMove(object sender, MouseEventArgs e)
        {
            if (_mouseDowned)
            {
                FrameworkElement treeviewitem = sender as FrameworkElement;
                var window = treeviewitem.GetParentByName<Window>(null);
                var point = e.GetPosition(window);

                //如果移动过大，则是拖拽
                if (Math.Abs(point.X - _downPoint.X) > 3 || Math.Abs(point.Y - _downPoint.Y) > 3)
                {
                    DataObject data = new DataObject("Text", new { Type = "Point", Name = this._point.Name }.ToJsonString());
                    DragDrop.DoDragDrop(treeviewitem, data, DragDropEffects.Move);

                    _mouseDowned = false;
                }
            }
        }

        public override void MouseUp(object sender, MouseButtonEventArgs e)
        {
            _mouseDowned = false;
        }
    }
}
