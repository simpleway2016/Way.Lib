using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace EJClient.TreeNode
{
    class TreeNodeBase : INotifyPropertyChanged
    {
        string _Name;
        public virtual string Name
        {
            get
            {
                return _Name;
            }
            set
            {
                if (_Name != value)
                {
                    _Name = value;
                    if (PropertyChanged != null)
                        PropertyChanged(this, new PropertyChangedEventArgs("Name"));
                }
            }

        }

        public virtual Visibility IconVisibility
        {
            get
            {
                if (string.IsNullOrEmpty(this.Icon))
                    return Visibility.Collapsed;
                return Visibility.Visible;
            }
        }

        bool _IsSelected = false;
        public virtual bool IsSelected
        {
            get
            {
                return _IsSelected;
            }
            set
            {
                if (_IsSelected != value)
                {
                    _IsSelected = value;
                    if (PropertyChanged != null)
                        PropertyChanged(this, new PropertyChangedEventArgs("IsSelected"));
                }
            }
        }
        bool _IsExpanded = false;
        public virtual bool IsExpanded
        {
            get
            {
                return _IsExpanded;
            }
            set
            {
                if (_IsExpanded != value)
                {
                    _IsExpanded = value;
                    if (PropertyChanged != null)
                        PropertyChanged(this, new PropertyChangedEventArgs("IsExpanded"));
                }
            }
        }
        public virtual ContextMenu ContextMenu
        {
            get;
            set;
        }

        public virtual string Icon
        {
            get;
            set;
        }

        public TreeNodeBase Self
        {
            get{
                return this;
            }
        }

        public TreeNodeBase Parent
        {
            get;
            set;
        }
        public TreeNodeBase(TreeNodeBase parent)
        {
            Parent = parent;
        }

        //必须用ObservableCollection，因为只有它可以令tree刷新ui
        System.Collections.ObjectModel.ObservableCollection<TreeNodeBase> _Children = new System.Collections.ObjectModel.ObservableCollection<TreeNodeBase>();
        public System.Collections.ObjectModel.ObservableCollection<TreeNodeBase> Children
        {
            get
            {
                return _Children;
            }
        }

        public virtual void ReBindItems()
        {
        }

        public virtual void OnDoubleClick(object sender, MouseButtonEventArgs e)
        {

        }
        public virtual void LeftMouseDown(object sender, MouseButtonEventArgs e)
        {

        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
