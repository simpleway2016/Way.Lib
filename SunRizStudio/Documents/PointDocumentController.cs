﻿using SunRizServer;
using SunRizStudio.Models;
using SunRizStudio.Models.Nodes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
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
            if(_pointModel.AddrSetting.IsBlank() == false)
            {
                _PointJsonDict = _pointModel.AddrSetting.JsonToObject<Dictionary<string, string>>();
            }
            setPointPropertyInput();
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
                    string[] properties = gatewayClient.GetPointProperties();
                    bool isNew = false;
                    if (_PointJsonDict == null)
                    {
                        _PointJsonDict = new Dictionary<string, string>();
                        isNew = true;
                    }
                    foreach (var item in properties)
                    {
                        string strProperty = item;
                        if (strProperty.Contains("{"))
                            strProperty = strProperty.Substring(0, strProperty.IndexOf("{"));

                        if(isNew)
                        {
                            _PointJsonDict[strProperty] = "";
                        }
                        addPropertyItem(strProperty);
                    }
                }
            }, Device.DriverID);

        }

        /// <summary>
        /// 添加一个点属性输入框
        /// </summary>
        /// <param name="name"></param>
        void addPropertyItem(string name)
        {
            int rowNumber = _gridProperty.RowDefinitions.Count;
            //给grid加入一行
            _gridProperty.RowDefinitions.Add(new RowDefinition()
            {
                Height = new GridLength(26)
            });

            TextBlock label = new TextBlock();
            label.Text = name;
            label.HorizontalAlignment = HorizontalAlignment.Center;
            label.VerticalAlignment = VerticalAlignment.Center;
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
}
