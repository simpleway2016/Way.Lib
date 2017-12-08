using SunRizServer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace SunRizStudio.Models.Nodes
{
    class AnalogNode : SolutionNode
    {
        protected virtual DevicePointFolder_TypeEnum FolderType
        {
            get
            {
                return DevicePointFolder_TypeEnum.Analog;
            }
        }
        public SunRizServer.DevicePointFolder FolderModel;
        public AnalogNode(SunRizServer.DevicePointFolder folderModel)
        {
            FolderModel = folderModel??new DevicePointFolder() { id = 0};
               Icon = "/Images/solution/folder.png";

            this.ContextMenuItems.Add(new ContextMenuItem()
            {
                Icon = "/Images/solution/folder.png",
                Text = "添加文件夹...",
                ClickHandler = addFolderClick,
            });
            this.ContextMenuItems.Add(new ContextMenuItem()
            {
                Icon = "/Images/solution/point.png",
                Text = "添加设备点...",
                ClickHandler = addPointClick,

            });
            if(FolderModel.id != 0)
            {
                this.ContextMenuItems.Add(new ContextMenuItem()
                {
                    Text = "重命名...",
                    ClickHandler = renameClick,

                });

                this.ContextMenuItems.Add(new ContextMenuItem()
                {
                    Icon = "/Images/solution/delete.png",
                    Text = "删除",
                    ClickHandler = (s, e) =>
                    {
                        if (MessageBox.Show("确定删除此文件夹，以及它的所有设备点吗？", "系统提示", MessageBoxButton.OKCancel) == MessageBoxResult.OK)
                        {
                            MainWindow.Instance.Cursor = Cursors.Hand;
                            Helper.Remote.Invoke<int>("DeleteDevicePointFolder", (ret, err) =>
                            {
                                MainWindow.Instance.Cursor = null;
                                if (err != null)
                                {
                                    MessageBox.Show(MainWindow.Instance, err);
                                }
                                else
                                {
                                    this.Parent.Nodes.Remove(this);
                                }
                            }, FolderModel.id);
                        }
                    },
                });
            }
        }

        void addPointClick(object sender, RoutedEventArgs e)
        {
            //获取device对象
            Device device = this.GetDevice();
            if(device.DriverID == null)
            {
                MessageBox.Show(MainWindow.Instance, "请先配置控制器");
                return;
            }
            if (this.FolderType == DevicePointFolder_TypeEnum.Analog)
            {
                var doc = new Documents.AnalogPointDocument(device,this,null, FolderModel.id.Value);
                MainWindow.Instance.SetActiveDocument(doc);
            }
            else if (this.FolderType == DevicePointFolder_TypeEnum.Digital)
            {              
                var doc = new Documents.DigitalPointDocument(device, this, null, FolderModel.id.Value);
                MainWindow.Instance.SetActiveDocument(doc);
            }
        }

        /// <summary>
        /// 双击编辑
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void renameClick(object sender, RoutedEventArgs e)
        {
            if (this.FolderModel.id != 0)
            {
                var inputbox = new Dialogs.InputBox("请输入新文件夹名称", "修改文件夹");
                inputbox.Owner = MainWindow.Instance;
                inputbox.Value = this.FolderModel.Name;
                if (inputbox.ShowDialog() == true && !inputbox.Value.IsBlank())
                {
                    FolderModel.Name = inputbox.Value.Trim();
                    if (FolderModel.ChangedProperties.Count > 0)
                    {
                        MainWindow.Instance.Cursor = Cursors.Hand;
                        Helper.Remote.Invoke<int>("UpdateDevicePointFolder", (ret, err) =>
                        {
                            MainWindow.Instance.Cursor = null;
                            if (err != null)
                            {
                                MessageBox.Show(MainWindow.Instance, err);
                            }
                            else
                            {
                                this.Text = FolderModel.Name;
                            }
                        }, FolderModel);
                    }
                }
            }
        }
        void addFolderClick(object sender, RoutedEventArgs e)
        {
            var inputbox = new Dialogs.InputBox("请输入文件夹名称", "添加文件夹");
            inputbox.Owner = MainWindow.Instance;
            if (inputbox.ShowDialog() == true && !inputbox.Value.IsBlank())
            {
                SunRizServer.DevicePointFolder folder = new DevicePointFolder();
                folder.ParentId = this.FolderModel.id;
                folder.Type = this.FolderType;
                folder.Name = inputbox.Value.Trim();
                folder.DeviceId = GetDevice().id;

                MainWindow.Instance.Cursor = Cursors.Hand;
                Helper.Remote.Invoke<int>("UpdateDevicePointFolder", (ret, err) =>
                {
                    MainWindow.Instance.Cursor = null;
                    if (err != null)
                    {
                        MessageBox.Show(MainWindow.Instance, err);
                    }
                    else
                    {
                        folder.id = ret;
                        folder.ChangedProperties.Clear();

                        //添加node对象
                        AnalogNode newNode = null;
                        if (this.FolderType == DevicePointFolder_TypeEnum.Analog)
                        {
                            newNode = new AnalogNode(folder);
                        }
                        else
                        {
                            newNode = new DigitalNode(folder);
                        }
                        newNode.Text = folder.Name;
                        this.Nodes.Insert(0, newNode);
                        this.IsExpanded = true;
                    }
                }, folder);
            }
        }

        public override void InitChildren()
        {
            this.Nodes.Add(new LoadingNode());

           
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

        /// <summary>
        /// 异步加载子节点
        /// </summary>
        protected override void LoadChildrenAsync()
        {
            base.LoadChildrenAsync();

            //获取Device对象
            SunRizServer.Device device = this.GetDevice();

            Helper.Remote.Invoke<DevicePointFolder[]>("GetDevicePointFolders", (ret, err) =>
            {
                if (err != null)
                {
                    MessageBox.Show(MainWindow.Instance, err);
                }
                else
                {
                    this.Nodes.Clear();
                    //加载子节点
                    foreach (var folder in ret)
                    {
                        AnalogNode newNode = null;
                        if (FolderType == DevicePointFolder_TypeEnum.Analog)
                        {
                            newNode = new AnalogNode(folder);
                        }
                        else
                        {
                            newNode = new DigitalNode(folder);
                        }
                        newNode.Text = folder.Name;
                        this.Nodes.Add(newNode);
                    }

                    //加载点
                    loadPoints(device);
                }
            }, device.id, FolderType, this.FolderModel.id);
        }

        void loadPoints(SunRizServer.Device device)
        {
            //异步加载设备点
            Helper.Remote.Invoke<DevicePoint[]>("GetDevicePoints", (ret, err) =>
            {
                if (err != null)
                {
                    MessageBox.Show(MainWindow.Instance, err);
                }
                else
                {

                    //加载子节点
                    foreach (var point in ret)
                    {
                        var newNode = new DevicePointNode(point);
                        this.Nodes.Add(newNode);
                    }
                }
            }, device.id, FolderType, this.FolderModel.id);
        }
    }
}
