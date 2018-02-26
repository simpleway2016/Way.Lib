using SunRizServer;
using SunRizStudio.Models;
using SunRizStudio.Models.Nodes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;

namespace SunRizStudio.Documents
{
    class PointDocumentController
    {
        public Device Device;
        Grid _gridProperty;
        DevicePoint _pointModel;
        internal DevicePoint OriginalModel;
        SolutionNode _parentNode;
        BaseDocument _container;
        SunRizDriver.SunRizDriverClient gatewayClient;
        Dictionary<string, string> _PointJsonDict = null;
        TextBox _textBoxForSelect = null;
        Popup _popup;
        public PointDocumentController(BaseDocument container, Grid gridProperty,Device device, DevicePoint_TypeEnum type, SolutionNode parent, DevicePoint dataModel, int folderId)
        {
            _container = container;
            _gridProperty = gridProperty;
            Device = device;
            _parentNode = parent;
            OriginalModel = dataModel;
            if (OriginalModel == null)
            {
                OriginalModel = new DevicePoint()
                {
                    Type = type,
                    DeviceId = this.Device.id,
                    FolderId = folderId
                };
            }

            //复制一份，给刷新按钮使用
            _pointModel = OriginalModel.Clone<DevicePoint>();
            _container.DataContext = _pointModel;
            _pointModel.PropertyChanged += _pointModel_PropertyChanged;
            if (_pointModel.AddrSetting.IsBlank() == false)
            {
                _PointJsonDict = _pointModel.AddrSetting.JsonToObject<Dictionary<string, string>>();
            }
            setPointPropertyInput();
        }

        private void _pointModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "ValueAbsoluteChange" && _pointModel.ValueAbsoluteChange == true)
            {
                _pointModel.ValueOnTimeChange = false;
                _pointModel.ValueRelativeChange = false;
            }
            else if (e.PropertyName == "ValueOnTimeChange" && _pointModel.ValueOnTimeChange == true)
            {
                _pointModel.ValueAbsoluteChange = false;
                _pointModel.ValueRelativeChange = false;
            }
            else if (e.PropertyName == "ValueRelativeChange" && _pointModel.ValueRelativeChange == true)
            {
                _pointModel.ValueAbsoluteChange = false;
                _pointModel.ValueOnTimeChange = false;
            }
        }

        /// <summary>
        /// 设置数据连接相关的属性输入框
        /// </summary>
        void setPointPropertyInput()
        {
            //获取通讯网关
            _container.Cursor = Cursors.Hand;
            Helper.Remote.Invoke<CommunicationDriver>("GetDriver", (gateway, err) => {
                _container.Cursor = null;
                if (err != null)
                {
                    MessageBox.Show(MainWindow.Instance, err);
                }
                else
                {
                    gatewayClient = new SunRizDriver.SunRizDriverClient(gateway.Address, gateway.Port.GetValueOrDefault());
                    var serverInfo = gatewayClient.GetServerInfo();
                    string[] properties = gatewayClient.GetPointProperties();
                    bool isNew = false;
                    if (_PointJsonDict == null)
                    {
                        _PointJsonDict = new Dictionary<string, string>();
                        isNew = true;
                    }
                    for(int i = 0; i < properties.Length; i ++)
                    {
                        var item = properties[i];
                        string strProperty = item;
                        if (strProperty.Contains("{"))
                            strProperty = strProperty.Substring(0, strProperty.IndexOf("{"));

                        if(isNew)
                        {
                            _PointJsonDict[strProperty] = "";
                        }
                        addPropertyItem(strProperty,i ==0 && serverInfo.SupportEnumPoints);
                    }
                }
            }, Device.DriverID);

        }

        /// <summary>
        /// 添加一个点属性输入框
        /// </summary>
        /// <param name="name"></param>
        void addPropertyItem(string name,bool supportEnumPoints)
        {
            int rowNumber = _gridProperty.RowDefinitions.Count;
            //给grid加入一行
            _gridProperty.RowDefinitions.Add(new RowDefinition()
            {
                Height = new GridLength(26)
            });

            TextBlock label = new TextBlock();
            label.Text = name;
            label.HorizontalAlignment = HorizontalAlignment.Left;
            label.VerticalAlignment = VerticalAlignment.Center;
            label.Margin = new Thickness(5, 0, 0, 0);
            label.SetValue(Grid.RowProperty, rowNumber);
            label.SetValue(Grid.ColumnProperty, 0);
            _gridProperty.Children.Add(label);

            //值输入框
            TextBox textbox = new TextBox();
            textbox.Padding = new Thickness(2, 4, 0, 0);
            textbox.BorderThickness = new Thickness(0);
            textbox.SetValue(Grid.RowProperty, rowNumber);
            textbox.SetValue(Grid.ColumnProperty, 1);
            textbox.Text = _PointJsonDict[name];
            textbox.TextChanged += (s, e) =>
            {
                _PointJsonDict[name] = textbox.Text;
            };
            _gridProperty.Children.Add(textbox);

            if(supportEnumPoints )
            {
                //如果可以枚举点，那么加入一个按钮，点击选择设备点
                Button btn = new Button();
                btn.Content = "...";
                btn.Width = 26;
                btn.Height = 20;
                btn.HorizontalAlignment = HorizontalAlignment.Right;
                btn.VerticalAlignment = VerticalAlignment.Center;
                btn.Margin = new Thickness(0, 0, 5, 0);
                btn.SetValue(Grid.RowProperty, rowNumber);
                btn.SetValue(Grid.ColumnProperty, 1);
                _gridProperty.Children.Add(btn);
                btn.Click += (s, e) =>
                {
                    _textBoxForSelect = textbox;
                    showPointSelector(btn);
                };
            }
        }

        /// <summary>
        /// 显示设备点选择控件
        /// </summary>
        /// <param name="btn"></param>
        void showPointSelector(Button btn)
        {
            if (_popup == null)
            {
                TreeView treeview = new TreeView();
                treeview.Width = 300;
                treeview.Height = 500;
                treeview.Background = System.Windows.Media.Brushes.AliceBlue;
                LoadingTreeItem loadingNode = new LoadingTreeItem();
                loadingNode.Header = "正在加载...";
                treeview.Items.Add(loadingNode);
                loadPoints(new List<string>(), treeview.Items);

                _popup = new Popup();
                _popup.PlacementTarget = btn;
                _popup.Placement = PlacementMode.Right;
                _popup.Child = treeview;
                _popup.StaysOpen = false;
            }
            _popup.IsOpen = true;
        }

        void loadPoints(List<string> parentPath,ItemCollection nodes)
        {
            
            Task.Run(() => {
                try
                {
                    gatewayClient.EnumDevicePoint(this.Device.Address, parentPath, (info) =>
                    {
                        this._container.Dispatcher.Invoke(() => {
                            if(nodes[0] is LoadingTreeItem)
                            {
                                nodes.Clear();
                            }
                            TreeViewItem node = new TreeViewItem();
                            node.Header = info.Name;
                            if(info.IsFolder)
                            {
                                LoadingTreeItem loadingNode = new LoadingTreeItem();
                                loadingNode.Header = "正在加载...";
                                node.Items.Add(loadingNode);

                                var path = new List<string>(parentPath);
                                path.Add(info.Name);
                                node.Tag = path;
                                node.Expanded += Node_Expanded;
                            }
                            else
                            {
                                node.Tag = info.Path;
                                node.PreviewMouseDown += Point_MouseDown;
                            }
                            nodes.Add(node);
                        });
                    });

                    if (nodes[0] is LoadingTreeItem)
                    {
                        this._container.Dispatcher.Invoke(() => {
                            nodes.Clear();
                        });                        
                    }
                }
                catch (Exception ex)
                {
                    this._container.Dispatcher.Invoke(() => {
                        MessageBox.Show(MainWindow.Instance, ex.Message);
                    });                    
                }
            });
        }

        private void Point_MouseDown(object sender, MouseButtonEventArgs e)
        {
            TreeViewItem node = sender as TreeViewItem;
            _textBoxForSelect.Text = node.Tag.ToString();
            _popup.IsOpen = false;
        }

        private void Node_Expanded(object sender, RoutedEventArgs e)
        {
            TreeViewItem node = sender as TreeViewItem;
            if (node.Items[0] is LoadingTreeItem)
            {
                loadPoints(node.Tag as List<string>, node.Items);
            }
        }

        /// <summary>
        /// 保存到数据库
        /// </summary>
        /// <param name="closeOnSuccess">成功后是否关闭窗口</param>
        internal void saveToServer(bool closeOnSuccess)
        {
           
            if (_pointModel.Name.IsBlank() || _pointModel.Desc.IsBlank())
            {
                MessageBox.Show(MainWindow.Instance, "请输入点名和描述");
                return;
            }
            if (_pointModel.Name.StartsWith("/") == false)
            {
                MessageBox.Show(MainWindow.Instance, "点名必须以“/”反斜杠为起始字符");
                return;
            }
            _container.Cursor = Cursors.Hand;
            try
            {
                _pointModel.AddrSetting = _PointJsonDict.ToJsonString();
                _pointModel.Address = gatewayClient.GetPointAddress(_PointJsonDict);
            }
            catch(Exception ex)
            {
                _container.Cursor = null;
                MessageBox.Show(MainWindow.Instance, ex.Message);
                return;
            }
            Helper.Remote.Invoke<int>("UpdatePoint", (ret, err) => {
                _container.Cursor = null;
                if (err != null)
                {
                    MessageBox.Show(MainWindow.Instance, err);
                }
                else
                {
                    //更新TabItem的文字
                    this._container.Title = _pointModel.Desc;
                    if (_pointModel.id == null)
                    {
                        //是添加的新点
                        _pointModel.id = ret;
                        _parentNode.Nodes.Add(new DevicePointNode(_pointModel));
                    }
                    _pointModel.ChangedProperties.Clear();
                    OriginalModel.CopyValue(_pointModel);
                    OriginalModel.ChangedProperties.Clear();

                    if (closeOnSuccess)
                    {
                        //关闭当前document
                        MainWindow.Instance.CloseDocument(_container);
                    }
                }
            }, _pointModel);
        }

        internal void refresh()
        {
            _pointModel = OriginalModel.Clone<DevicePoint>();
            _container.DataContext = _pointModel;
        }
    }

    class LoadingTreeItem: TreeViewItem
    {

    }
}
