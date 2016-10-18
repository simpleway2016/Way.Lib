using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace EJClient.Forms.BugCenter.ToolbarItems
{
    public class TDraw : TButton
    { 
        public override bool IsActived
        {
            get
            {
                return base.IsActived;
            }
            set
            {
                base.IsActived = value;
                BugEditor.instance.canvas.IsHitTestVisible = value;
            }
        }


        public void Do(Point location)
        {
            TextBox t = new TextBox();
            t.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
            t.VerticalAlignment = System.Windows.VerticalAlignment.Top;
            t.Margin = new Thickness(location.X,location.Y , 0,0);
            BugEditor.instance.mainGrid.Children.Add(t);
            t.Focus();
        }
    }
}
