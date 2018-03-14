using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace SunRizStudio.Models
{
    class SolutionNode : INotifyPropertyChanged
    {
        public bool ShowInTree = true;
        string _Icon;
        public string Icon
        {
            get => _Icon;
            set
            {
                if (value != _Icon)
                {
                    _Icon = value;
                    this.OnPropertyChanged("Icon");
                }
            }
        }

        string _Text;
        public virtual string Text
        {
            get => _Text;
            set
            {
                if (value != _Text)
                {
                    _Text = value;
                    this.OnPropertyChanged("Text");
                }
            }
        }
        FontWeight _FontWeight = FontWeights.Normal;
        /// <summary>
        /// 是否是粗体
        /// </summary>
        public virtual FontWeight FontWeight
        {
            get => _FontWeight;
            set
            {
                if (value != _FontWeight)
                {
                    _FontWeight = value;
                    this.OnPropertyChanged("FontWeight");
                }
            }
        }
        string _ToolTip;
        public virtual string ToolTip
        {
            get => _ToolTip;
            set
            {
                if (value != _ToolTip)
                {
                    _ToolTip = value;
                    this.OnPropertyChanged("ToolTip");
                }
            }
        }

        bool _IsExpanded;
        public bool IsExpanded
        {
            get => _IsExpanded;
            set
            {
                if (value != _IsExpanded)
                {
                    _IsExpanded = value;
                    OnExpandChanged();
                    this.OnPropertyChanged("IsExpanded");
                }
            }
        }

        bool _IsSelected;
        public bool IsSelected
        {
            get => _IsSelected;
            set
            {
                if (value != _IsSelected)
                {
                    _IsSelected = value;
                    OnSelectChanged();
                    this.OnPropertyChanged("IsSelected");
                }
            }
        }

        public SolutionNode Parent
        {
            get;
            set;
        }

        public RoutedEventHandler ClickHandler
        {
            get;
            set;
        }


        public MouseButtonEventHandler DoublicClickHandler
        {
            get;
            set;
        }


        ObservableCollection<ContextMenuItem> _ContextMenuItems = new ObservableCollection<ContextMenuItem>();
        public ObservableCollection<ContextMenuItem> ContextMenuItems
        {
            get => _ContextMenuItems;
        }

        public bool HasContextMenu
        {
            get => _ContextMenuItems.Count > 0;
        }

        SolutionNodeCollection _Nodes;
        public SolutionNodeCollection Nodes
        {
            get => _Nodes ?? (_Nodes = new SolutionNodeCollection(this));
        }

        //  TreeNodes不需要InitChildren，所以不用SolutionNodeCollection
        ObservableCollection<SolutionNode> _TreeNodes;
        public ObservableCollection<SolutionNode> TreeNodes
        {
            get => _TreeNodes ?? (_TreeNodes = new ObservableCollection<SolutionNode>());
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string proName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(proName));
            }
        }

        public SolutionNode()
        {
            this.Nodes.CollectionChanged += Nodes_CollectionChanged;
            this.DoublicClickHandler = (s, e) =>
            {
                OnDoublicClick(s, e);
            };
        }

        protected virtual void LoadChildrenAsync()
        {

        }

        protected virtual void OnExpandChanged()
        {
            if (this.Nodes.FirstOrDefault() is Models.Nodes.LoadingNode)
            {
                LoadChildrenAsync();
            }
        }
        protected virtual void OnSelectChanged()
        {
            if (this.Nodes.FirstOrDefault() is Models.Nodes.LoadingNode)
            {
                LoadChildrenAsync();
            }
            MainWindow.Instance.lstBottomList.ItemsSource = this.Nodes;
        }
        private void Nodes_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Add)
            {
                int index = e.NewStartingIndex;
                foreach (SolutionNode node in e.NewItems)
                {
                    if (node.ShowInTree)
                    {
                        this.TreeNodes.Insert(index, node);
                        index++;
                    }
                }
            }
            else if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Move)
            {

            }
            else if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Remove)
            {
                foreach (SolutionNode node in e.OldItems)
                {
                    if (this.TreeNodes.Contains(node))
                        this.TreeNodes.Remove(node);
                }

            }
            else if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Reset)
            {
                this.TreeNodes.Clear();
            }
        }

        /// <summary>
        /// 初始化子节点
        /// </summary>
        public virtual void InitChildren()
        {

        }

        protected virtual void OnDoublicClick(object sender, MouseButtonEventArgs e)
        {
            if (this.Nodes.Count > 0)
            {
                this.IsExpanded = true;
                MainWindow.Instance.lstBottomList.ItemsSource = this.Nodes;
            }
        }

        public virtual void MouseDown(object sender, MouseButtonEventArgs e)
        {

        }
        public virtual void MouseUp(object sender, MouseButtonEventArgs e)
        {

        }
        public virtual void MouseMove(object sender, MouseEventArgs e)
        {

        }
    }

    class ContextMenuItem
    {
        public string Text
        {
            get;
            set;
        }
        public string Icon
        {
            get;
            set;
        }
        public object Tag
        {
            get;
            set;
        }
        public RoutedEventHandler ClickHandler
        {
            get;
            set;
        }
    }
}
