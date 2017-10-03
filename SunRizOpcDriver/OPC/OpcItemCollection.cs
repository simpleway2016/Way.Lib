using System;
using System.Collections.Generic;
using System.Text;
using OPC.Data;

namespace OPC
{
    public class OpcItemCollection : BaseCollection<OPCItem>
    {
        internal OPC.Data.OpcGroup Group;
        public OpcItemCollection(OPC.Data.OpcGroup Group)
        {
            this.Group = Group;
        }

        public void AddRange(OPCItem[] items)
        {
            Group.AddItems(items);
            for (int i = 0; i < items.Length; i++)
            {
                base.InsertItem( this.Count , items[i] );
            }
        }

        public OPCItem GetItemByServerHandler(int serverhandler)
        {
            foreach (OPCItem item in this)
            {
                if (item.HandleServer == serverhandler)
                    return item;
            }
            return null;
        }

        public OPCItem GetItemByClientHandler(int clienthandler)
        {
            foreach (OPCItem item in this)
            {
                if (item.HandleClient == clienthandler)
                    return item;
            }
            return null;
        }

        public OPCItemState[] GetItemValues()
        {
            int[] handler = new int[this.Count];
            for (int i = 0; i < this.Count; i++)
            {
                handler[i] = this[i].HandleServer;
            }
            OPC.Data.OPCItemState[] itemValues = this.Group.Read(handler);

            return itemValues;
        }

        public void WriteItemValues(object[] values)
        {

            int[] handler = new int[values.Length];
            for (int i = 0; i < values.Length; i++)
            {
                handler[i] = this[i].HandleServer;
            }

            int[] arrErr;
            //group.Write(serverhandles, arrVal, 9988, out cancelID, out arrErr);
            this.Group.Write(handler, values, out arrErr);
        }

        protected override void ClearItems()
        {
            int[] serverhandles = new int[this.Count] ;
            for (int i = 0; i < serverhandles.Length; i++)
            {
                serverhandles[i] = this[i].HandleServer;
            }
            int[] remerrors;
            Group.RemoveItems(serverhandles,out remerrors);
            base.ClearItems();
        }

        protected override void RemoveItem(int index)
        {
            OPCItem item = this[index];
            int[] serverhandles = new int[1] { item.HandleServer };
            int[] remerrors;
            Group.RemoveItems(serverhandles ,out remerrors);
            base.RemoveItem(index);
        }

        protected override void InsertItem(int index, OPCItem item)
        {
            Group.AddItems(new OPCItem[1] { item });
            base.InsertItem(index, item);
        }
    }
}
