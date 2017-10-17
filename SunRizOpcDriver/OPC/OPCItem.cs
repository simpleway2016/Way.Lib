
using System;
using System.Text;
using System.Runtime.InteropServices;
using System.Reflection;
using System.Diagnostics;

using OPC.Common;
using OPC.Data.Interface;
using System.Runtime.InteropServices.ComTypes;


namespace OPC.Data
{
    public class OPCItem
    {
        public int Error;			// content below only valid if Error=S_OK
        public int HandleServer;
        public int HandleClient;
        public string ID;
        public Type DataType;
        public OPCACCESSRIGHTS AccessRights;
        public byte[] Blob;
        public OPC.Data.OpcGroup Group;

        public OPCItem()
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ID">点ID</param>
        /// <param name="handlerClient">Item客户端句柄，可任意取值</param>
        public OPCItem(string ID , int handlerClient)
        {
            this.ID = ID;
            this.HandleClient = handlerClient;
        }

        public bool CanRead
        {
            get
            {
                return (AccessRights & OPCACCESSRIGHTS.OPC_READABLE) != 0;
            }
        }

        public bool CanWrite
        {
            get
            {
                return (AccessRights & OPCACCESSRIGHTS.OPC_WRITEABLE) != 0;
            }
        }

        public OPCItemState GetValue()
        {
            int[] handler = new int[1] { this.HandleServer };
            OPC.Data.OPCItemState[] itemValues = this.Group.Read(handler);

            if ( itemValues == null || itemValues.Length == 0)
                return null;

            OPC.Data.OPCItemState s = itemValues[0];
            if (HRESULTS.Succeeded(s.Error))
            {
                return s;
            }
            else
            {
                throw (new Exception("ERROR 0x" + s.Error.ToString("X")));
            }
        }


        public void WriteValue(object value)
        {
            //if (this.DataType != null)
            //{
            //    string vt = value.GetType().FullName;
            //    string ov = value.ToString();
            //    value = Convert.ChangeType(value, this.DataType);
            //    if (value == null)
            //    {
            //        throw (new Exception("不能把类型为“" + vt + "”的值为“" + ov + "”转换为" + this.DataType.FullName));
            //    }
            //}
            int[] handler = new int[1] { this.HandleServer };
            object[] objValues = new object[1] { value};
            int[] arrErr;
            //group.Write(serverhandles, arrVal, 9988, out cancelID, out arrErr);
            if (this.Group == null)
            {
                throw(new Exception("请先把item添加到Group"));
            }
            this.Group.Write(handler, objValues, out arrErr);
        }
    }
}
