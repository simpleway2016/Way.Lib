using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace SunRizStudio.Extention
{
    public class SortableListView : ListView
    {
        public delegate void SortHandler(string order);
        public event SortHandler Sort;

        public void OnSort(string order)
        {
            if(Sort != null)
            {
                Sort(order);
            }
        }
    }
}
