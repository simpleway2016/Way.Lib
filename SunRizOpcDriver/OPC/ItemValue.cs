using System;
using System.Collections.Generic;
using System.Text;

namespace OPCdotNETLib
{
    class ItemValue
    {
        public string id = "";
        public OPC.Data.OPCItemState value;
        public ItemValue(string id, OPC.Data.OPCItemState value)
        {
            this.id = id;
            this.value = value;
        }
    }
}
