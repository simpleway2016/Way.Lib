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
        string _Icon;
        public string Icon
        {
            get => _Icon;
            set
            {
                if(value != _Icon)
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
                    OnExpandedChanged();
                    this.OnPropertyChanged("IsExpanded");
                }
            }
        }

        protected virtual void OnExpandedChanged()
        {

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
            get => _Nodes?? (_Nodes = new SolutionNodeCollection(this));
        }
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string proName)
        {
            if(PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(proName));
            }
        }

        public SolutionNode()
        {
            this.DoublicClickHandler = (s, e) => {
                OnDoublicClick(s,e);
            };
        }

        /// <summary>
        /// 初始化子节点
        /// </summary>
        public virtual void InitChildren()
        {

        }

        protected virtual void OnDoublicClick(object window, MouseButtonEventArgs e)
        {

        }

        public virtual void MouseDown(object sender, MouseButtonEventArgs e)
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
        public RoutedEventHandler ClickHandler
        {
            get;
            set;
        }
    }
}
