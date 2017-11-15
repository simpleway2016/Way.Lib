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
        public SunRizServer.DevicePointFolder FolderModel = new DevicePointFolder() { id = 0 };
        public AnalogNode()
        {
            Icon = "/Images/solution/folder.png";

            this.ContextMenuItems.Add(new ContextMenuItem() {
                Icon = "/Images/solution/folder.png",
                Text = "添加文件夹...",
                ClickHandler = addFolderClick,
            });

            this.DoublicClickHandler = doubleClick;
        }

        /// <summary>
        /// 双击编辑
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void doubleClick(object sender, RoutedEventArgs e)
        {
            if(this.FolderModel.id != 0)
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
        void addFolderClick(object sender , RoutedEventArgs e)
        {
            var inputbox = new Dialogs.InputBox("请输入文件夹名称", "添加文件夹");
            inputbox.Owner = MainWindow.Instance;
            if(inputbox.ShowDialog() == true && !inputbox.Value.IsBlank())
            {
                SunRizServer.DevicePointFolder folder = new DevicePointFolder();
                folder.ParentId = this.FolderModel.id;
                folder.Type = this.FolderType;
                folder.Name = inputbox.Value.Trim();
                folder.DeviceId = GetDevice().id;

                MainWindow.Instance.Cursor = Cursors.Hand;
                Helper.Remote.Invoke<int>("UpdateDevicePointFolder", (ret, err) => {
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
                        if(this.FolderType == DevicePointFolder_TypeEnum.Analog)
                        {
                            newNode = new AnalogNode();
                        }
                        else
                        {
                            newNode = new DigitalNode();
                        }
                        newNode.FolderModel = folder;
                        newNode.Text = folder.Name;
                        this.Nodes.Insert(0 , newNode);
                        this.IsExpanded = true;
                    }
                }, folder);
            }
        }

        public override void InitChildren()
        {           
            this.Nodes.Add(new LoadingNode());

            if(this.FolderModel.id != 0)
            {
                this.ContextMenuItems.Add(new ContextMenuItem()
                {
                    Icon = "/Images/solution/delete.png",
                    Text = "删除",
                    ClickHandler = (s,e)=> {
                        if( MessageBox.Show("确定删除此文件夹，以及它的所有设备点吗？" , "系统提示" , MessageBoxButton.OKCancel) == MessageBoxResult.OK )
                        {
                            MainWindow.Instance.Cursor = Cursors.Hand;
                            Helper.Remote.Invoke<int>("DeleteDevicePointFolder", (ret, err) => {
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

        protected override void OnExpandedChanged()
        {
            base.OnExpandedChanged();
           
            if(this.IsExpanded && this.Nodes.Any(m=>m is LoadingNode))
            {
                //获取Device对象
                SunRizServer.Device device = this.GetDevice();

                Helper.Remote.Invoke<DevicePointFolder[]>("GetDevicePointFolders", (ret, err) => {
                    if(err != null)
                    {
                        MessageBox.Show(MainWindow.Instance, err);
                    }
                    else
                    {
                        this.Nodes.Clear();
                        //加载子节点
                        foreach ( var folder in ret )
                        {
                            AnalogNode newNode = null;
                            if(FolderType == DevicePointFolder_TypeEnum.Analog)
                            {
                                newNode = new AnalogNode();
                            }
                            else
                            {
                                newNode = new DigitalNode();
                            }
                            newNode.FolderModel = folder;
                            newNode.Text = folder.Name;
                            this.Nodes.Add(newNode);
                        }

                        //加载点
                        loadPoints(device);
                    }
                },device.id , FolderType ,this.FolderModel.id );

               
            }
        }

        void loadPoints(SunRizServer.Device device)
        {
            //异步加载设备点
            Helper.Remote.Invoke<DevicePoint[]>("GetDevicePoints", (ret, err) => {
                if (err != null)
                {
                    MessageBox.Show(MainWindow.Instance, err);
                }
                else
                {
                    
                    //加载子节点
                    foreach (var point in ret)
                    {
                        var newNode = new SolutionNode();
                        newNode.Text = point.Name;
                        this.Nodes.Add(newNode);
                    }
                }
            }, device.id, FolderType, this.FolderModel.id);
        }
    }
}
