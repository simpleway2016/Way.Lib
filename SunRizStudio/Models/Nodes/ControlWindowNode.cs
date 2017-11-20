using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SunRizStudio.Models.Nodes
{
    class ControlWindowNode : SolutionNode
    {
        SunRizServer.ControlWindow _dataModel;
        public ControlWindowNode(SunRizServer.ControlWindow dataModel)
        {
            _dataModel = dataModel;
            this.Text = _dataModel.Name;
        }
    }
}
