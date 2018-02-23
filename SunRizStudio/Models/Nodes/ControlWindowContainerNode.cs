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
    class ControlWindowContainerNode : SolutionNode
    {
        internal int _controlUnitId;
        SunRizServer.ControlWindowFolder _folderModel;
        internal int _folderId = 0;
        public ControlWindowContainerNode(int controlUnitId, SunRizServer.ControlWindowFolder folder)
        {

            _controlUnitId = controlUnitId;
            _folderModel = folder;
            if (_folderModel != null)
            {
                _folderId = _folderModel.id.Value;
                this.Text = _folderModel.Name;
                Icon = "/Images/solution/folder.png";
            }
            else
            {
                Icon = "/Images/solution/graphic.png";
            }
            //this.ContextMenuItems.Add(new ContextMenuItem()
            //{
            //    Icon = "/Images/solution/folder.png",
            //    Text = "添加文件夹...",
            //    ClickHandler = addFolderClick,
            //});
            this.ContextMenuItems.Add(new ContextMenuItem()
            {
                Icon = "/Images/solution/window.png",
                Text = "添加监视画面...",
                ClickHandler = addWindowClick,

            });

            if (_folderId != 0)
            {
                this.ContextMenuItems.Add(new ContextMenuItem()
                {
                    Text = "删除",
                    ClickHandler = delClick,

                });
                this.DoublicClickHandler = doubleClick;
            }
        }

        void delClick(object sender, RoutedEventArgs e)
        {
            if( MessageBox.Show("确定删除吗？" , "" , MessageBoxButton.OKCancel , MessageBoxImage.Question) == MessageBoxResult.OK )
            {
                MainWindow.Instance.Cursor = Cursors.Hand;
                Helper.Remote.Invoke<int>("DeleteControlWindowFolder", (ret, err) =>
                {
                    MainWindow.Instance.Cursor = null;
                    this.Parent.Nodes.Remove(this);
                }, _folderId);
            }
        }
        void doubleClick(object sender, RoutedEventArgs e)
        {
            if (this._folderId != 0)
            {
                var inputbox = new Dialogs.InputBox("请输入新文件夹名称", "修改文件夹");
                inputbox.Owner = MainWindow.Instance;
                inputbox.Value = this._folderModel.Name;
                if (inputbox.ShowDialog() == true && !inputbox.Value.IsBlank())
                {
                    _folderModel.Name = inputbox.Value.Trim();
                    if (_folderModel.ChangedProperties.Count > 0)
                    {
                        MainWindow.Instance.Cursor = Cursors.Hand;
                        Helper.Remote.Invoke<int>("UpdateControlWindowFolder", (ret, err) =>
                        {
                            MainWindow.Instance.Cursor = null;
                            if (err != null)
                            {
                                MessageBox.Show(MainWindow.Instance, err);
                            }
                            else
                            {
                                this.Text = _folderModel.Name;
                            }
                        }, _folderModel);
                    }
                }
            }
        }

        void addWindowClick(object sender, RoutedEventArgs e)
        {
            var doc = new Documents.ControlWindowDocument(this, null, false);
            MainWindow.Instance.SetActiveDocument(doc);
        }

        void addFolderClick(object sender, RoutedEventArgs e)
        {
            var inputbox = new Dialogs.InputBox("请输入文件夹名称", "添加文件夹");
            inputbox.Owner = MainWindow.Instance;
            if (inputbox.ShowDialog() == true && !inputbox.Value.IsBlank())
            {
                SunRizServer.ControlWindowFolder folder = new SunRizServer.ControlWindowFolder();
                folder.ParentId = _folderId;
                folder.Name = inputbox.Value.Trim();
                folder.ControlUnitId = _controlUnitId;

                MainWindow.Instance.Cursor = Cursors.Hand;
                Helper.Remote.Invoke<int>("UpdateControlWindowFolder", (ret, err) =>
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
                        var newNode = new ControlWindowContainerNode(_controlUnitId, folder);
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

        /// <summary>
        /// 异步加载子节点
        /// </summary>
        protected override void LoadChildrenAsync()
        {
            base.LoadChildrenAsync();

            Helper.Remote.Invoke<SunRizServer.ControlWindowFolder[]>("GetControlWindowFolders", (ret, err) =>
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
                        var newNode = new ControlWindowContainerNode(_controlUnitId, folder);
                        this.Nodes.Add(newNode);
                    }

                    //加载监控画面
                    loadWindows();
                }
            }, _controlUnitId, _folderId);

        }

        void loadWindows()
        {
            //异步加载监控画面
            Helper.Remote.Invoke<SunRizServer.ControlWindow[]>("GetWindows", (ret, err) =>
            {
                if (err != null)
                {
                    MessageBox.Show(MainWindow.Instance, err);
                }
                else
                {

                    //加载子节点
                    foreach (var window in ret)
                    {
                        SunRizServer.ControlWindow curWin = window;
                        foreach( Documents.DocTabItem item in MainWindow.Instance.documentContainer.Items )
                        {
                            if(item.Content is Documents.ControlWindowDocument)
                            {
                                var doc = item.Content as Documents.ControlWindowDocument;
                                if(doc._dataModel.id == curWin.id)
                                {
                                    //采用现有打开窗口的_dataModel
                                    curWin = doc._dataModel;
                                    break;
                                }
                            }
                        }
                        var newNode = new ControlWindowNode(curWin);
                        this.Nodes.Add(newNode);
                    }
                }
            }, _controlUnitId, _folderId);
        }
    }
}
