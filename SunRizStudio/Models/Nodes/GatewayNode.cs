using SunRizServer;
using System;
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
        public SunRizServer.CommunicationDriver Gateway
        {
            get;
            private set;
        }
        public void UpdateText()
        {
            this.Text = $"{Gateway.Name}";
            this.ToolTip = $"{Gateway.Address}:{Gateway.Port}";

        }
        public GatewayNode(CommunicationDriver data)
        {
            Gateway = data;
            this.Icon = "/Images/solution/gateway.png";
           
            UpdateText();
        }

        protected override void OnDoublicClick(object sender, MouseButtonEventArgs e)
        {
            var data = this.Gateway.Clone<CommunicationDriver>();
            Dialogs.GatewayDialog dialog = new Dialogs.GatewayDialog(data);
            dialog.Owner = ((FrameworkElement)sender).GetParentByName<Window>(null);
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
