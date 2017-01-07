using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Way.Lib.ScriptRemoting
{
    class VirtualWebSocketHandler : ISocketHandler
    {
        internal static System.Collections.Hashtable ActivedVirtualWebSocketHandler = Hashtable.Synchronized(new Hashtable());
        protected Dictionary<string, string> mRequestForms;
        protected Connection mConnection;
        RemotingClientHandler.SendDataHandler mSendFunc;
        

        public VirtualWebSocketHandler(Connection con, Dictionary<string, string> forms, RemotingClientHandler.SendDataHandler sendFunc)
        {
            mRequestForms = forms;
            mConnection = con;
            mSendFunc = sendFunc;
        }

        public void Handle()
        {
            string mode = mRequestForms["mode"];
            if (mode == "init")
            {
                this.mSendFunc(Guid.NewGuid().ToString());
            }
            else if (mode == "receive")
            {
                string id = mRequestForms["id"];
                string binaryType = mRequestForms["binaryType"];

                RemotingClientHandler remotingHandler;

                if (ActivedVirtualWebSocketHandler.ContainsKey(id))
                {
                    remotingHandler = (RemotingClientHandler)ActivedVirtualWebSocketHandler[id];
                   
                }
                else
                {
                    remotingHandler = new ScriptRemoting.RemotingClientHandler(null,null, mConnection.mClient.Socket.RemoteEndPoint.ToString().Split(':')[0]);

                    ActivedVirtualWebSocketHandler.Add(id , remotingHandler);
                }
                remotingHandler.Tag2 = true;
                if (remotingHandler.mSendDataFunc == null)
                {
                    remotingHandler.mSendDataFunc = (data) =>
                    {
                        try
                        {
                            int count = 0;
                            while (remotingHandler.Tag1 == null && count < 500)
                            {
                                System.Threading.Thread.Sleep(10);
                                count++;
                            }

                            RemotingClientHandler.SendDataHandler func = remotingHandler.Tag1 as RemotingClientHandler.SendDataHandler;
                            if (func == null)
                            {
                                remotingHandler.mCloseStreamHandler.Invoke();
                                return;
                            }
                            remotingHandler.Tag2 = false;
                            remotingHandler.Tag1 = null;
                            func.Invoke(data);
                        }
                        catch
                        {
                        }
                    };
                }
                remotingHandler.mCloseStreamHandler = () =>
                {

                    try
                    {
                        this.mConnection.mClient.Close();
                        ActivedVirtualWebSocketHandler.Remove(id);
                    }
                    catch
                    {
                    }
                };
                remotingHandler.Tag1 = mSendFunc;

                while (true)
                {
                    try
                    {
                        this.mConnection.mClient.ReceiveDatas(1);
                    }
                    catch
                    {
                        break;
                    }
                }
               
                this.mConnection.mClient.Close();
                if ((bool)remotingHandler.Tag2)
                {
                    if (ActivedVirtualWebSocketHandler.ContainsKey(id))
                    {
                        ActivedVirtualWebSocketHandler.Remove(id);
                    }
                    remotingHandler.OnDisconnected();
                }
                return;
            }
            else if (mode == "send")
            {
                string data = mRequestForms["data"];
                string id = mRequestForms["id"];
                string binaryType = mRequestForms["binaryType"];
                int count = 0;
                while (!ActivedVirtualWebSocketHandler.ContainsKey(id) && count<300)
                {
                    System.Threading.Thread.Sleep(10);
                    count++;
                }
                if (ActivedVirtualWebSocketHandler.ContainsKey(id))
                {
                    RemotingClientHandler handler = (RemotingClientHandler)ActivedVirtualWebSocketHandler[id];
                    try
                    {
                        if (binaryType == "arraybuffer")
                        {
                            string[] strArr = data.Split('%');
                            byte[] bs = new byte[strArr.Length - 1];
                            for (int i = 1; i < strArr.Length; i++)
                            {
                                bs[i - 1] = (byte)Convert.ToInt32( strArr[i] , 16);
                            }
                            handler.OnReceived(bs);
                        }
                        else
                        {
                            handler.OnReceived(data);
                        }
                    }
                    catch
                    {
                        
                    }
                }
                mSendFunc("");

            }

            //wait for close
            while (true)
            {
                try
                {
                    this.mConnection.mClient.ReceiveDatas(1);
                }
                catch
                {
                    break;
                }
            }
            this.mConnection.mClient.Close();
        }
    }
}
