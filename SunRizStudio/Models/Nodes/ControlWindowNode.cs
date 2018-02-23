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
    class ControlWindowNode : SolutionNode
    {
        internal SunRizServer.ControlWindow DataModel;

        public ControlWindowNode(SunRizServer.ControlWindow dataModel)
        {
            DataModel = dataModel;
            DataModel.PropertyChanged += _dataModel_PropertyChanged;
            this.Text = DataModel.Name + " " + DataModel.Code;
            this.Icon = "/images/solution/window.png";
            this.DoublicClickHandler = (s,e) =>{
                open();
            };

            this.ContextMenuItems.Add(new ContextMenuItem()
            {
                Text = "打开",
                ClickHandler = (s,e)=> open(),

            });
            this.ContextMenuItems.Add(new ContextMenuItem()
            {
                Text = "脚本编辑",
                ClickHandler = (s, e) => openCode(),

            });
            this.ContextMenuItems.Add(new ContextMenuItem()
            {
                Text = "删除",
                ClickHandler = delClick,

            });
        }
        void delClick(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("确定删除吗？", "", MessageBoxButton.OKCancel, MessageBoxImage.Question) == MessageBoxResult.OK)
            {
                MainWindow.Instance.Cursor = Cursors.Hand;
                Helper.Remote.Invoke<int>("DeleteControlWindow", (ret, err) =>
                {
                    MainWindow.Instance.Cursor = null;
                    this.Parent.Nodes.Remove(this);
                }, DataModel.id);
            }
        }
        void open()
        {
            //先查看是否已经打开document，如果打开，则激活就可以
            foreach (TabItem item in MainWindow.Instance.documentContainer.Items)
            {
                if (item.Content is Documents.ControlWindowDocument)
                {
                    var document = item.Content as Documents.ControlWindowDocument;
                    if (document.IsRunMode == false && document._dataModel.id == DataModel.id)
                    {
                        //激活document
                        MainWindow.Instance.documentContainer.SelectedItem = item;
                        return;
                    }
                }
            }
            var doc = new Documents.ControlWindowDocument(this.Parent as ControlWindowContainerNode, DataModel, false);
            MainWindow.Instance.SetActiveDocument(doc);
        }
        void openCode()
        {            
            Helper.Remote.Invoke<string>("GetWindowScript", (script, err) => {
                if (err != null)
                {
                    MessageBox.Show(MainWindow.Instance, err);
                }
                else
                {
                    Dialogs.TextEditor frm = new Dialogs.TextEditor(script, DataModel.id.Value);
                    frm.Title = DataModel.Name;
                    frm.ShowDialog();
                }
            }, DataModel.id);
        }
        private void _dataModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
           if(e.PropertyName == "Name" || e.PropertyName == "Code")
            {
                try
                {
                    this.Text = DataModel.Name + " " + DataModel.Code;
                }
                catch
                {
                    DataModel.PropertyChanged -= _dataModel_PropertyChanged;
                }
            }
        }

        public override void MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                FrameworkElement treeviewitem = sender as FrameworkElement;
                DataObject data = new DataObject("Text", new { Type = "GroupControl", windowCode = DataModel.Code }.ToJsonString());
                DragDrop.DoDragDrop(treeviewitem, data, DragDropEffects.Move);
            }
        }
    
    }
}
