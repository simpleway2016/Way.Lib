using System;
using System.Collections.Generic;
using System.Text;
using OPC.Data;

namespace OPC
{
    [Serializable]
    public class OPCNode
    {
        public OPCNode(string name)
        {
            this._Name = name;
        }

        [NonSerialized]
        public OpcServer OpcServer;
        internal List<string> _FullPathObj;

        public System.Collections.Generic.List<string> FullPathObj
        {
            get
            {
                return _FullPathObj;
            }
            set
            {
                _FullPathObj = value;
            }
        }

        [NonSerialized]
        public OPCNode Parent;

        private string _Name = "";
        public string Name
        {
            get
            {
                return _Name;
            }
            set
            {
                _Name = value;
            }
        }

        private string _ID = "";
        public string ID
        {
            get
            {
                return _ID;
            }
            set
            {
                _ID = value;
            }
        }

        private bool _isItemProperty = false;
        public bool IsItemProperty
        {
            get
            {
                return _isItemProperty;
            }
            set
            {
                _isItemProperty = value;
            }
        }

        private object _Tag = null;
        public object Tag
        {
            get
            {
                return _Tag;
            }
            set
            {
                _Tag = value;
            }
        }

        internal List<OPCNode> _Nodes;
        public List<OPCNode> Nodes
        {
            get
            {
                if (_Nodes == null)
                {

                    _Nodes = this.OpcServer.GetNodes(this.FullPathObj);
                }
                return _Nodes;
            }
        }

        internal List<OPCNodeItem> _NodeItems;
        public List<OPCNodeItem> NodeItems
        {
            get
            {
                if (_NodeItems == null)
                {

                    _NodeItems = this.OpcServer.GetNodeItems(this.FullPathObj);
                }
                return _NodeItems;
            }
        }
    }
}
