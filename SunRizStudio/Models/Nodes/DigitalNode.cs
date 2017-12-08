using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SunRizServer;

namespace SunRizStudio.Models.Nodes
{
    class DigitalNode : AnalogNode
    {
        protected override DevicePointFolder_TypeEnum FolderType =>  DevicePointFolder_TypeEnum.Digital;

        public DigitalNode(SunRizServer.DevicePointFolder folderModel):base(folderModel)
        {
        }
    }
}
