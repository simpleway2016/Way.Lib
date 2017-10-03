using System;
using System.Collections.Generic;
using System.Text;
using OPC.Data;

namespace OPC
{
    [Serializable]
    public class OPCNodeItem
    {
        [NonSerialized]
        internal OpcServer server;
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
    }
}
