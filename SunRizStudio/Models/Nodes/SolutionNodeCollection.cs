using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SunRizStudio.Models
{
    class SolutionNodeCollection : ObservableCollection<SolutionNode>
    {
        SolutionNode _owner;
        public SolutionNodeCollection(SolutionNode owner)
        {
            _owner = owner;
        }
        protected override void InsertItem(int index, SolutionNode item)
        {
            item.Parent = _owner;
            base.InsertItem(index, item);
            item.InitChildren();
        }
        protected override void RemoveItem(int index)
        {
            var item = this[index];
            item.Parent = null;
            base.RemoveItem(index);
        }
    }
}
