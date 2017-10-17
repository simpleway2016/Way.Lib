
using System;
using System.Text;
using System.Collections;
using System.Runtime.InteropServices;
using System.Reflection;

using OPC.Common;
using OPC.Data.Interface;
using System.Runtime.InteropServices.ComTypes;
using System.Collections.Generic;
using System.Net;
using OPCdotNETLib;
using System.Threading;

namespace OPC.Data
{

    #region OPCProperty OPCPropertyData OPCPropertyItem

    // ------------- managed side only structs ----------------------
    public class OPCProperty					// QueryAvailableProperties
    {
        public int PropertyID;
        public string Description;
        public VarEnum DataType;

        public override string ToString()
        {
            return "ID:" + PropertyID + " '" + Description + "' T:" + DUMMY_VARIANT.VarEnumToString(DataType);
        }
    }

    public class OPCPropertyData				// GetItemProperties
    {
        public int PropertyID;
        public int Error;
        public object Data;

        public override string ToString()
        {
            if (Error == HRESULTS.S_OK)
                return "ID:" + PropertyID + " Data:" + Data.ToString();
            else
                return "ID:" + PropertyID + " Error:" + Error.ToString();
        }
    }

    public class OPCPropertyItem				// LookupItemIDs
    {
        public int PropertyID;
        public int Error;
        public string newItemID;

        public override string ToString()
        {
            if (Error == HRESULTS.S_OK)
                return "ID:" + PropertyID + " newID:" + newItemID;
            else
                return "ID:" + PropertyID + " Error:" + Error.ToString();
        }
    }



    #endregion



    #region 委托


    // ----------------- event argument+handler ------------------------

    // IOPCShutdown
    public class ShutdownRequestEventArgs : EventArgs
    {
        public ShutdownRequestEventArgs(string shutdownReasonp)
        {
            shutdownReason = shutdownReasonp;
        }

        public string shutdownReason;
    }
    public delegate void ShutdownRequestEventHandler(object sender, ShutdownRequestEventArgs e);

    public delegate void EnumNodesHandler(List<string> FullPathObj , OPC.OPCNode node);


    #endregion


    public delegate void EnumAllItemHandler(string ItemId,string name,bool IsItem,ref bool Continue);
    public delegate void EnumMoveHandler(string ItemId);


    // --------------------------- OpcServer ------------------------

    [ComVisible(true)]
    public class OpcServer : IOPCShutdown
    {
        public event EnumAllItemHandler OnEnumAllItems;
        public event EnumMoveHandler OnEnumMoveDown;
        public event EnumMoveHandler OnEnumMoveUp;

        public event EnumNodesHandler EnumNodes;
        private bool Disconnected = false;

        #region 属性
        private List<OPCNode> _Nodes;
        public List<OPCNode> Nodes
        {
            get
            {
                if (_Nodes == null)
                {

                    _Nodes = this.GetNodes(null);
                }
                return _Nodes;
            }
        }

        private List<OPCNodeItem> _NodeItems;
        public List<OPCNodeItem> NodeItems
        {
            get
            {
                if (_NodeItems == null)
                {

                    _NodeItems = this.GetNodeItems(null);
                }
                return _NodeItems;
            }
        }

        private OpcGroupCollection _OpcGroups;
        public OpcGroupCollection OpcGroups
        {
            get
            {
                if (_OpcGroups == null)
                {
                    _OpcGroups = new OpcGroupCollection(this);
                }
                return _OpcGroups;
            }
        }
        #endregion

        
        public string ServerName;
        public string ProgID;
        private bool IsFirstVersion = false;
        public OpcServer(bool IsFirstVersion)
        {
            this.IsFirstVersion = IsFirstVersion;
        }

        public OpcServer()
        {

        }
        ~OpcServer()
        {
            
            try
            {
                if (!Disconnected)
                Disconnect();
            }
            catch
            {
            }
        }

        #region EnumAllNodes
        /// <summary>
        /// 枚举所有节点
        /// </summary>
        public void EnumAllNodes()
        {
            DoBrowse();
        }

        
        private void OnEnumNodes(List<string> FullPathObj , OPC.OPCNode node)
        {
            if (this.EnumNodes != null)
            {
                this.EnumNodes(FullPathObj, node);
            }
        }

        private void DoBrowse()
        {

                OPCNAMESPACETYPE opcorgi = this.QueryOrganization();

                
                if (opcorgi == OPCNAMESPACETYPE.OPC_NS_HIERARCHIAL)
                {
                    this.MoveToRoot();
                    this._Nodes = new List<OPCNode>();
                    RecurBrowse(_Nodes , null, null);
                }
        }

        private void MoveDown(string name)
        {
            this.ChangeBrowsePosition(OPCBROWSEDIRECTION.OPC_BROWSE_DOWN, name);
            if (this.OnEnumMoveDown != null)
            {
                this.OnEnumMoveDown(name);
            }
        }

        #region EnumAllItems 枚举所有Item
        /// <summary>
        /// 枚举所有Item
        /// </summary>
        public void EnumAllItems()
        {
            if (this.OnEnumAllItems == null)
            {
                throw (new Exception("请设置OnEnumAllItems事件"));
            }
            try
            {
                QueryOrganization();
            }
            catch (OnlyItemsException)
            {
                MoveToRoot();
                this._enumItems();
                return;
            }
            catch (Exception ex)
            {
                throw(ex);
            }
            MoveToRoot();
            Thread t = new Thread(new ParameterizedThreadStart(this._enumAllItems));
            t.Start("");
        }

        private void _enumItems()
        {
            UCOMIEnumString enumerator;
            BrowseOPCItemIDs(OPCBROWSETYPE.OPC_LEAF, "", VarEnum.VT_EMPTY, 0, out enumerator);
            if (enumerator == null)
            {
                return;
            }

            int cft;
            string[] strF = new string[1];
            int hresult;
            do
            {
                cft = 0;
                hresult = enumerator.Next(1, strF, out cft);
                if (cft > 0)
                {
                    for (int i = 0; i < cft; i++)
                    {
                        string name = strF[i];
                        string itemname = this.GetItemID(name);
                        bool Continue = true;
                        this.OnEnumAllItems(itemname, name, true , ref Continue);
                    }
                }
            }
            while (hresult == HRESULTS.S_OK);

            int rc = Marshal.ReleaseComObject(enumerator);
            enumerator = null;
        }

        private void _enumAllItems(object parentID)
        {

            string parent = parentID.ToString();
            UCOMIEnumString enumerator;
            BrowseOPCItemIDs(OPCBROWSETYPE.OPC_BRANCH, "", VarEnum.VT_EMPTY, 0, out enumerator);
            if (enumerator == null)
            {
                goto endsub;
            }

            int cft;
            string[] strF = new string[1];
            int hresult;
            do
            {
                cft = 0;
                hresult = enumerator.Next(1, strF, out cft);
                if (cft > 0)
                {
                    for (int i = 0; i < cft; i++)
                    {

                        string itemname = strF[i];
                        bool Continue = true;
                        this.OnEnumAllItems(parent + itemname, itemname, false , ref Continue);
                        if (Continue)
                        {
                            MoveDown(itemname);
                            _enumAllItems(parent + itemname + "/");
                        }
                        
                    }
                }
            }
            while (hresult == HRESULTS.S_OK);

            int rc = Marshal.ReleaseComObject(enumerator);
            enumerator = null;


            _enumItems();
        endsub:
            MoveUp();
        }
        #endregion


        private void MoveUp()
        {
            try
            {
                ifBrowse.ChangeBrowsePosition(OPCBROWSEDIRECTION.OPC_BROWSE_UP, "");
            }
            catch
            {
            }

            if (this.OnEnumMoveUp != null)
            {
                this.OnEnumMoveUp("");
            }
        }

        private void MoveToRoot()
        {
            while (true)
            {
                try
                {
                    ifBrowse.ChangeBrowsePosition(OPCBROWSEDIRECTION.OPC_BROWSE_UP, "");
                }
                catch
                {
                    return;
                }
            }
        }

        private void RecurBrowse(List<OPCNode> parentNodes , OPCNode parent , List<string> FullPathObj)
        {

            ArrayList lst = this.Browse(OPCBROWSETYPE.OPC_BRANCH);
                if (lst == null)
                    return ;
                if (lst.Count < 1)
                    return ;

                foreach (string s in lst)
                {
                    this.ChangeBrowsePosition(OPCBROWSEDIRECTION.OPC_BROWSE_DOWN, s);

                    OPCNode node = new OPCNode(s);
                    node.OpcServer = this;
                    if( FullPathObj == null )
                    {
                        FullPathObj = new List<string>();
                    }
                    FullPathObj.Add(s);
                    node.FullPathObj = FullPathObj;
                    node.Parent = parent;
 
                    this.OnEnumNodes(FullPathObj, node);
                    parentNodes.Add(node);

                    ArrayList lstItem = this.Browse(OPCBROWSETYPE.OPC_LEAF);
                    node._NodeItems = new List<OPCNodeItem>();
                    if (lstItem != null)
                    {
                        foreach (string sItem in lstItem)
                        {
                            OPCNodeItem item = new OPCNodeItem();
                            item.Name = sItem;
                            item.ID = this.GetItemID(sItem);
                            node._NodeItems.Add(item);
                        }
                    }

                    
                    node._Nodes = new List<OPCNode>();
                    RecurBrowse(node._Nodes ,node , FullPathObj);
                    this.ChangeBrowsePosition(OPCBROWSEDIRECTION.OPC_BROWSE_UP, "");
                }

        }

        #endregion

        #region GetNodeItems
        public List<OPCNodeItem> GetNodeItems(List<string> FullPathObj)
        {
            this.MoveToRoot();
            if (FullPathObj == null)
            {
                FullPathObj = new List<string>();
            }

            foreach (string s in FullPathObj)
            {
                this.ChangeBrowsePosition(OPCBROWSEDIRECTION.OPC_BROWSE_DOWN, s);
            }

            List<OPCNodeItem> newNodes = new List<OPCNodeItem>();
            ArrayList lst = this.Browse(OPCBROWSETYPE.OPC_LEAF);
            foreach (string s in lst)
            {
                OPCNodeItem node = new OPCNodeItem();
                newNodes.Add(node);
                node.Name = s;
                node.ID = this.GetItemID(s);
                node.FullPathObj = new List<string>();
                foreach (string path in FullPathObj)
                {
                    node.FullPathObj.Add(path);
                }
                node.FullPathObj.Add(s);
                node.server = this;
            }
            return newNodes;
        }
        #endregion

        #region GetNodes
        public delegate void AsyncGetNodeHandler(object tag,OPCNode node);
        public event AsyncGetNodeHandler AsyncGetNode;
        public delegate void AsyncGetNodeCompletedHandler(object sender , object tag);
        public event AsyncGetNodeCompletedHandler AsyncGetNodeCompleted; 
        /// <summary>
        /// 枚举点
        /// </summary>
        /// <param name="FullPathObj"></param>
        /// <param name="tag"></param>
        public void AsyncGetNodes(List<string> FullPathObj, Object tag)
        {
            AsyncGetNodes(FullPathObj, tag, false);
        }

        /// <summary>
        /// 枚举点
        /// </summary>
        /// <param name="FullPathObj"></param>
        /// <param name="tag"></param>
        /// <param name="useCurrentThread">使用当前线程</param>
        public void AsyncGetNodes(List<string> FullPathObj, Object tag,bool useCurrentThread)
        {
            #region first
            bool onlyitem = false;
            try
            {
                QueryOrganization();
            }
            catch (OnlyItemsException)
            {
                onlyitem = true;
            }
            catch (Exception)
            {

            }
            this.MoveToRoot();
            //this.ChangeBrowsePosition(OPCBROWSEDIRECTION.OPC_BROWSE_TO, "/");
            if (FullPathObj == null)
            {
                FullPathObj = new List<string>();
            }

            foreach (string s in FullPathObj)
            {
                this.ChangeBrowsePosition(OPCBROWSEDIRECTION.OPC_BROWSE_DOWN, s);
            }
            #endregion

            if (useCurrentThread == false)
            {
                Thread t = new Thread(new System.Threading.ParameterizedThreadStart(Async_enumNodes));

                t.Start(new object[] { FullPathObj, tag, onlyitem });
            }
            else
            {
                Async_enumNodes(new object[] { FullPathObj, tag, onlyitem });
            }
        }

        private void Async_enumNodes(object param)
        {
            object[] params_ = param as object[];
            List<string> FullPathObj = params_[0] as List<string>;
            bool onlyitem = (bool)params_[2];
            object tag = params_[1];

            ArrayList lst = null;
            UCOMIEnumString enumerator;
            int cft;
            string[] strF = new string[100];
            int hresult;

            if (onlyitem == false)
            {

                BrowseOPCItemIDs(OPCBROWSETYPE.OPC_BRANCH, "", VarEnum.VT_EMPTY, 0, out enumerator);
                if (enumerator != null)
                {



                    do
                    {
                        cft = 0;
                        hresult = enumerator.Next(1, strF, out cft);
                        if (cft > 0)
                        {
                            OPCNode newNode = new OPCNode(strF[0]);
                            newNode.FullPathObj = new List<string>();
                            foreach (string path in FullPathObj)
                            {
                                newNode.FullPathObj.Add(path);
                            }
                            newNode.FullPathObj.Add(newNode.Name);

                            if (AsyncGetNode != null)
                            {
                                AsyncGetNode(tag, newNode);
                            }
                        }
                        else
                        {
                            break;
                        }
                    }
                    while (hresult == HRESULTS.S_OK);
                }
            }

            BrowseOPCItemIDs(OPCBROWSETYPE.OPC_LEAF, "", VarEnum.VT_EMPTY, 0, out enumerator);
            if (enumerator == null)
            {
                if (AsyncGetNodeCompleted != null)
                {
                    this.AsyncGetNodeCompleted(this, tag);
                }
                return;
            }

            do
            {
                cft = 0;
                hresult = enumerator.Next(1, strF, out cft);
                if (cft > 0)
                {
                    OPCNode newNode = new OPCNode(strF[0]);
                    newNode.FullPathObj = new List<string>();
                    foreach (string path in FullPathObj)
                    {
                        newNode.FullPathObj.Add(path);
                    }
                    newNode.FullPathObj.Add(newNode.Name);

                    if (AsyncGetNode != null)
                    {
                        newNode.ID = this.GetItemID(newNode.Name);
                        newNode.IsItemProperty = true;
                        AsyncGetNode(tag, newNode);
                    }
                }
                else
                {
                    break;
                }
            }
            while (hresult == HRESULTS.S_OK);

            if (AsyncGetNodeCompleted != null)
            {
                this.AsyncGetNodeCompleted(this, tag);
            }
        }

        public List<OPCNode> GetNodes(List<string> FullPathObj)
        {
            try
            {
                QueryOrganization();
            }
            catch (OnlyItemsException)
            {
                return new List<OPCNode>();
            }
            catch (Exception)
            {
               
            }
            this.MoveToRoot();
            //this.ChangeBrowsePosition(OPCBROWSEDIRECTION.OPC_BROWSE_TO, "/");
            if (FullPathObj == null)
            {
                FullPathObj = new List<string>();
            }

            foreach (string s in FullPathObj)
            {
                this.ChangeBrowsePosition(OPCBROWSEDIRECTION.OPC_BROWSE_DOWN, s);
            }

            List<OPCNode> newNodes = new List<OPCNode>();
            ArrayList lst = this.Browse(OPCBROWSETYPE.OPC_BRANCH);
            foreach (string s in lst)
            {
                OPCNode node = new OPCNode(s);
                newNodes.Add(node);
                node.FullPathObj = new List<string>();
                foreach (string path in FullPathObj)
                {
                    node.FullPathObj.Add(path);
                }
                node.FullPathObj.Add(s);
                node.OpcServer = this;
            }
            return newNodes;
        }
        #endregion

        public void Connect(string clsidOPCserver)
        {
            Connect(clsidOPCserver , null);
        }
        public void Connect(string clsidOPCserver , string server)
        {
            
            Disconnect();

            Type typeofOPCserver = null;
            bool localhost = false;
            if (server != null &&  server.ToLower() !="localhost" && server.Length > 0)
            {
                localhost = false;
            }
            else
            {
                localhost = true;
            }

            try
            {
                if (localhost)
                {
                    typeofOPCserver = Type.GetTypeFromProgID(clsidOPCserver, true);
                }
                else
                {

                    typeofOPCserver = Type.GetTypeFromProgID(clsidOPCserver, server, true);
                }
            }
            catch (Exception ex)
            {
                throw (new Exception("Type.GetTypeFromProgID出错，" + ex.Message));
            }

            if (typeofOPCserver == null)
                Marshal.ThrowExceptionForHR(HRESULTS.OPC_E_NOTFOUND);

            try
            {
                OPCserverObj = Activator.CreateInstance(typeofOPCserver);
            }
            catch (Exception ex)
            {
                throw (new Exception("Activator.CreateInstance创建对象出错，" + ex.Message));
            }
            ifServer = (IOPCServer)OPCserverObj;
            if (ifServer == null)
                Marshal.ThrowExceptionForHR(HRESULTS.CONNECT_E_NOCONNECTION);

            // connect all interfaces
            try
            {
                ifCommon = (IOPCCommon)OPCserverObj;
            }
            catch
            {
            }

            try
            {
                if (!IsFirstVersion)
                {
                    cpointcontainer = (IConnectionPointContainer)OPCserverObj;
                }
            }
            catch
            {
                throw (new Exception("Can't find interface IConnectionPointContainer"));
            }

            if(cpointcontainer != null)
            AdviseIOPCShutdown();

            this.ServerName = server;
            this.ProgID = clsidOPCserver;
        }

        public void Disconnect()
        {
            try
            {
                foreach (OpcGroup group in this.OpcGroups)
                {
                    try
                    {
                        group.Items.Clear();
                    }
                    catch
                    {
                    }
                    
                }

                this.OpcGroups.Clear();
            }
            catch
            {
            }
            Disconnected = true;
            try
            {
                if (!(shutdowncpoint == null))
                {
                    if (shutdowncookie != 0)
                    {
                        shutdowncpoint.Unadvise(shutdowncookie);
                        shutdowncookie = 0;
                    }
                    int rc = Marshal.ReleaseComObject(shutdowncpoint);
                    shutdowncpoint = null;
                }

                
            }
            catch
            {
            }


            cpointcontainer = null;
            ifCommon = null;
            ifServer = null;
            try
            {

                if (!(OPCserverObj == null))
                {
                    int rc = Marshal.ReleaseComObject(OPCserverObj);
                    OPCserverObj = null;
                }
            }
            catch
            {
            }
            GC.Collect();
        }

        public SERVERSTATUS GetStatus()
        {
            SERVERSTATUS serverStatus;
            ifServer.GetStatus(out serverStatus);
            return serverStatus;
        }

        public string GetErrorString(int errorCode, int localeID)
        {
            string errorres;
            ifServer.GetErrorString(errorCode, localeID, out errorres);
            return errorres;
        }


        internal void AddGroup(OpcGroup group)
        {
            AddGroup(group, new int[] { group.TimeBias}, new float[] { group.PercentDeadband}, 0);
        }
        private void AddGroup(OpcGroup group,
                                    int[] biasTime, float[] percentDeadband, int localeID)
        {
            if (ifServer == null)
                Marshal.ThrowExceptionForHR(HRESULTS.E_ABORT);

            group.ifServer = ifServer;
            group.state.Public = false;
            group.Server = this;

            group.internalAdd(biasTime, percentDeadband, localeID);
            return;
        }


        // --------------------------------- IOPCServerPublicGroups (indirect) -----------------
        public OpcGroup GetPublicGroup(string groupName)
        {
            if (ifServer == null)
                Marshal.ThrowExceptionForHR(HRESULTS.E_ABORT);

            OpcGroup grp = new OpcGroup(ref ifServer, true, groupName, false, 1000);
            grp.internalAdd(null, null, 0);
            return grp;
        }





        // --------------------------------- IOPCCommon -------------------------
        public void SetLocaleID(int lcid)
        {
            if (ifCommon == null)
            {
                throw (new Exception("服务器没有实现IOPCCommon接口，无法调用SetLocaleID"));
            }
            ifCommon.SetLocaleID(lcid);
        }
        public void GetLocaleID(out int lcid)
        {
            if (ifCommon == null)
            {
                throw (new Exception("服务器没有实现IOPCCommon接口，无法调用GetLocaleID"));
            }
            ifCommon.GetLocaleID(out lcid);
        }
        public void QueryAvailableLocaleIDs(out int[] lcids)
        {
            if (ifCommon == null)
            {
                throw (new Exception("服务器没有实现IOPCCommon接口，无法调用QueryAvailableLocaleIDs"));
            }

            lcids = null;
            int count;
            IntPtr ptrIds;
            int hresult = ifCommon.QueryAvailableLocaleIDs(out count, out ptrIds);
            if (HRESULTS.Failed(hresult))
                Marshal.ThrowExceptionForHR(hresult);
            if (((int)ptrIds) == 0)
                return;
            if (count < 1)
            { Marshal.FreeCoTaskMem(ptrIds); return; }

            lcids = new int[count];
            Marshal.Copy(ptrIds, lcids, 0, count);
            Marshal.FreeCoTaskMem(ptrIds);
        }

        public void SetClientName(string name)
        {
            if (ifCommon == null)
            {
                throw (new Exception("服务器没有实现IOPCCommon接口，无法调用SetClientName"));
            }
            ifCommon.SetClientName(name);
        }








        // ------------------------ IOPCBrowseServerAddressSpace ---------------

        private OPCNAMESPACETYPE QueryOrganizationType = OPCNAMESPACETYPE.none;
        public OPCNAMESPACETYPE QueryOrganization()
        {
            if (QueryOrganizationType != OPCNAMESPACETYPE.none)
            {
                return QueryOrganizationType;
            }

            OPCNAMESPACETYPE ns;
            ifBrowse.QueryOrganization(out ns);
            if( ns != OPCNAMESPACETYPE.OPC_NS_HIERARCHIAL )
                throw (new OnlyItemsException("此服务器只有Items，没有任何group节点"));
            QueryOrganizationType = ns;
            return ns;
        }

        public void ChangeBrowsePosition(OPCBROWSEDIRECTION direction, string name)
        {
            try
            {
                ifBrowse.ChangeBrowsePosition(direction, name);
            }
            catch (Exception ex)
            {
                throw (new Exception("ChangeBrowsePosition执行错误，不存在节点“" + name + "”，具体错误信息：" + ex.ToString()));
            }
        }

        public void BrowseOPCItemIDs(OPCBROWSETYPE filterType, string filterCriteria,
                                        VarEnum dataTypeFilter, OPCACCESSRIGHTS accessRightsFilter,
                                        out UCOMIEnumString stringEnumerator)
        {
            stringEnumerator = null;
            object enumtemp;
            ifBrowse.BrowseOPCItemIDs(filterType, filterCriteria, (short)dataTypeFilter, accessRightsFilter, out enumtemp);
            stringEnumerator = (UCOMIEnumString)enumtemp;
            enumtemp = null;
        }

        public string GetItemID(string itemDataID)
        {
            string itemid;
            ifBrowse.GetItemID(itemDataID, out itemid);
            return itemid;
        }

        public void BrowseAccessPaths(string itemID, out UCOMIEnumString stringEnumerator)
        {
            stringEnumerator = null;
            object enumtemp;
            ifBrowse.BrowseAccessPaths(itemID, out enumtemp);
            stringEnumerator = (UCOMIEnumString)enumtemp;
            enumtemp = null;
        }

        // extra helper
        public ArrayList Browse(OPCBROWSETYPE typ)
        {
            ArrayList lst = null;
            UCOMIEnumString enumerator;
            BrowseOPCItemIDs(typ, "", VarEnum.VT_EMPTY, 0, out enumerator);
            if (enumerator == null)
                return null;

            lst = new ArrayList(500);
            int cft;
            string[] strF = new string[100];
            int hresult;
            do
            {
                cft = 0;
                hresult = enumerator.Next(100, strF, out cft);
                if (cft > 0)
                {
                    for (int i = 0; i < cft; i++)
                        lst.Add(strF[i]);
                }
            }
            while (hresult == HRESULTS.S_OK);

            int rc = Marshal.ReleaseComObject(enumerator);
            enumerator = null;
            lst.TrimToSize();
            return lst;
        }








        // ------------------------ IOPCItemProperties ---------------

        public void QueryAvailableProperties(string itemID, out OPCProperty[] opcProperties)
        {
            opcProperties = null;

            int count = 0;
            IntPtr ptrID;
            IntPtr ptrDesc;
            IntPtr ptrTyp;
            ifItmProps.QueryAvailableProperties(itemID, out count, out ptrID, out ptrDesc, out ptrTyp);
            if ((count == 0) || (count > 10000))
                return;

            int runID = (int)ptrID;
            int runDesc = (int)ptrDesc;
            int runTyp = (int)ptrTyp;
            if ((runID == 0) || (runDesc == 0) || (runTyp == 0))
                Marshal.ThrowExceptionForHR(HRESULTS.E_ABORT);

            opcProperties = new OPCProperty[count];

            IntPtr ptrString;
            for (int i = 0; i < count; i++)
            {
                opcProperties[i] = new OPCProperty();

                opcProperties[i].PropertyID = Marshal.ReadInt32((IntPtr)runID);
                runID += 4;

                ptrString = (IntPtr)Marshal.ReadInt32((IntPtr)runDesc);
                runDesc += 4;
                opcProperties[i].Description = Marshal.PtrToStringUni(ptrString);
                Marshal.FreeCoTaskMem(ptrString);

                opcProperties[i].DataType = (VarEnum)Marshal.ReadInt16((IntPtr)runTyp);
                runTyp += 2;
            }

            Marshal.FreeCoTaskMem(ptrID);
            Marshal.FreeCoTaskMem(ptrDesc);
            Marshal.FreeCoTaskMem(ptrTyp);
        }



        public bool GetItemProperties(string itemID, int[] propertyIDs, out OPCPropertyData[] propertiesData)
        {
            propertiesData = null;
            int count = propertyIDs.Length;
            if (count < 1)
                return false;

            IntPtr ptrDat;
            IntPtr ptrErr;
            int hresult = ifItmProps.GetItemProperties(itemID, count, propertyIDs, out ptrDat, out ptrErr);
            if (HRESULTS.Failed(hresult))
                Marshal.ThrowExceptionForHR(hresult);

            int runDat = (int)ptrDat;
            int runErr = (int)ptrErr;
            if ((runDat == 0) || (runErr == 0))
                Marshal.ThrowExceptionForHR(HRESULTS.E_ABORT);

            propertiesData = new OPCPropertyData[count];

            for (int i = 0; i < count; i++)
            {
                propertiesData[i] = new OPCPropertyData();
                propertiesData[i].PropertyID = propertyIDs[i];

                propertiesData[i].Error = Marshal.ReadInt32((IntPtr)runErr);
                runErr += 4;

                if (propertiesData[i].Error == HRESULTS.S_OK)
                {
                    propertiesData[i].Data = Marshal.GetObjectForNativeVariant((IntPtr)runDat);
                    DUMMY_VARIANT.VariantClear((IntPtr)runDat);
                }
                else
                    propertiesData[i].Data = null;

                runDat += DUMMY_VARIANT.ConstSize;
            }

            Marshal.FreeCoTaskMem(ptrDat);
            Marshal.FreeCoTaskMem(ptrErr);
            return hresult == HRESULTS.S_OK;
        }


        public bool LookupItemIDs(string itemID, int[] propertyIDs, out OPCPropertyItem[] propertyItems)
        {
            propertyItems = null;
            int count = propertyIDs.Length;
            if (count < 1)
                return false;

            IntPtr ptrErr;
            IntPtr ptrIds;
            int hresult = ifItmProps.LookupItemIDs(itemID, count, propertyIDs, out ptrIds, out ptrErr);
            if (HRESULTS.Failed(hresult))
                Marshal.ThrowExceptionForHR(hresult);

            int runIds = (int)ptrIds;
            int runErr = (int)ptrErr;
            if ((runIds == 0) || (runErr == 0))
                Marshal.ThrowExceptionForHR(HRESULTS.E_ABORT);

            propertyItems = new OPCPropertyItem[count];

            IntPtr ptrString;
            for (int i = 0; i < count; i++)
            {
                propertyItems[i] = new OPCPropertyItem();
                propertyItems[i].PropertyID = propertyIDs[i];

                propertyItems[i].Error = Marshal.ReadInt32((IntPtr)runErr);
                runErr += 4;

                if (propertyItems[i].Error == HRESULTS.S_OK)
                {
                    ptrString = (IntPtr)Marshal.ReadInt32((IntPtr)runIds);
                    propertyItems[i].newItemID = Marshal.PtrToStringUni(ptrString);
                    Marshal.FreeCoTaskMem(ptrString);
                }
                else
                    propertyItems[i].newItemID = null;

                runIds += 4;
            }

            Marshal.FreeCoTaskMem(ptrIds);
            Marshal.FreeCoTaskMem(ptrErr);
            return hresult == HRESULTS.S_OK;
        }




        // ------------------------ IOPCShutdown --------------- COMMON CALLBACK
        void IOPCShutdown.ShutdownRequest(string shutdownReason)
        {
            ShutdownRequestEventArgs e = new ShutdownRequestEventArgs(shutdownReason);
            if (ShutdownRequested != null)
                ShutdownRequested(this, e);
        }

        // -------------------------- event ---------------------
        public event ShutdownRequestEventHandler ShutdownRequested;





        // -------------------------- private ---------------------

        private void AdviseIOPCShutdown()
        {
            
            Type sinktype = typeof(IOPCShutdown);
            Guid sinkguid = sinktype.GUID;

            cpointcontainer.FindConnectionPoint(ref sinkguid, out shutdowncpoint);
            if (shutdowncpoint == null)
                return;
            shutdowncpoint.Advise(this, out shutdowncookie);
        }

        private object OPCserverObj = null;
        private IOPCServer ifServer = null;
        private IOPCCommon ifCommon = null;

        private IOPCBrowseServerAddressSpace _ifBrowse = null;
        private IOPCBrowseServerAddressSpace ifBrowse
        {
            get
            {
                if (_ifBrowse == null)
                    _ifBrowse = (IOPCBrowseServerAddressSpace)ifServer;
                return _ifBrowse;
            }
        }

        private IOPCItemProperties _ifItmProps = null;
        private IOPCItemProperties ifItmProps
        {
            get
            {
                if (_ifItmProps == null)
                    _ifItmProps = (IOPCItemProperties)ifServer;
                return _ifItmProps;
            }
        }

        private IConnectionPointContainer cpointcontainer = null;
        private IConnectionPoint shutdowncpoint = null;
        private int shutdowncookie = 0;

}	// class OpcServer











}	// namespace OPC.Data
