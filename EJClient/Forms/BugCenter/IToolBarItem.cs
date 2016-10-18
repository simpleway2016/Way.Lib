using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace EJClient.Forms.BugCenter
{
    interface IToolBarItem
    {
        bool IsActived
        { get; set; }

        void Do(Point location);
    }
}
