﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace SunRizStudio.Models
{
    class GatewayNode : SolutionNode
    {
        public Dialogs.GatewayDialog.Gateway Gateway
        {
            get;
            private set;
        }
        public void UpdateText()
        {
            this.Text = $"{Gateway.Name}";
        }
        public GatewayNode(Dialogs.GatewayDialog.Gateway data)
        {
            Gateway = data;
            this.Icon = "/Images/solution/gateway.png";
           
            UpdateText();
        }

        protected override void OnDoublicClick(object window, MouseButtonEventArgs e)
        {
            var data = this.Gateway.Clone<Dialogs.GatewayDialog.Gateway>();
            Dialogs.GatewayDialog dialog = new Dialogs.GatewayDialog(data);
            dialog.Owner = (Window)window;
            dialog.Title = this.Text;
            if (dialog.ShowDialog() == true)
            {
                this.Gateway = dialog.Data;
                this.Gateway.ChangedProperties.Clear();
                this.UpdateText();
            }
        }
    }
}
