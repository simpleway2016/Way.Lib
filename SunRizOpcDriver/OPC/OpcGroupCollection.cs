using System;
using System.Collections.Generic;
using System.Text;
using OPC.Data;

namespace OPC
{
    public class OpcGroupCollection : BaseCollection<OpcGroup>
    {
        internal OPC.Data.OpcServer Server;
        public OpcGroupCollection(OPC.Data.OpcServer Server)
        {
            this.Server = Server;
        }

        protected override void ClearItems()
        {
            foreach (OpcGroup g in this)
            {
                g.Remove();
            }
            base.ClearItems();
        }

        protected override void RemoveItem(int index)
        {
            OpcGroup group = this[index];
            group.Remove();
            base.RemoveItem(index);
        }

        protected override void InsertItem(int index, OpcGroup item)
        {
            this.Server.AddGroup(item);
            base.InsertItem(index, item);
        }
    }
}
