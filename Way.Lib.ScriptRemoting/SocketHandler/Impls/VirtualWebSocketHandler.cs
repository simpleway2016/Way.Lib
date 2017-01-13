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
        RemotingClientHandler.SendDataHandler mSendFunc;
        Func<int> mWaitForCloseFunc;
        Func<int> mCloseConnectionFunc;
        string mClientIP;
        string _Referer;
        static VirtualWebSocketHandler()
        {
            new System.Threading.Thread(() =>
            {
                while(true)
                {
                    try
                    {
                        foreach(string id in ActivedVirtualWebSocketHandler.Keys)
                        {
                            var handler = (RemotingClientHandler)ActivedVirtualWebSocketHandler[id];
                            if(handler != null&&(DateTime.Now - handler._heartTime).TotalMinutes > 2)
                            {
                                handler.OnDisconnected();
                                //删除
                                ActivedVirtualWebSocketHandler.Remove(id);
                                
                            }
                        }
                    }
                    catch
                    {
                        System.Threading.Thread.Sleep(100);
                        continue;
                    }
                    System.Threading.Thread.Sleep(30000);
                }
            }).Start();
        }

        public VirtualWebSocketHandler(Dictionary<string, string> forms, 
            RemotingClientHandler.SendDataHandler sendFunc, 
            Func<int> waitForCloseFunc,
            Func<int> closeConFunc,
            string clientip,string referer)
        {
            _Referer = referer;
            mCloseConnectionFunc = closeConFunc;
            mClientIP = clientip;
            mRequestForms = forms;
            mSendFunc = sendFunc;
            mWaitForCloseFunc = waitForCloseFunc;
        }

        public void Handle()
        {
            try
            {
                string mode = mRequestForms["mode"];
                if (mode == "init")
                {
                    this.mSendFunc(Guid.NewGuid().ToString());
                }
                else if (mode == "heart")
                {
                    string id = mRequestForms["id"];
                    if (ActivedVirtualWebSocketHandler.ContainsKey(id))
                    {
                        var remotingHandler = (RemotingClientHandler)ActivedVirtualWebSocketHandler[id];
                        if (remotingHandler != null)
                        {
                            remotingHandler._heartTime = DateTime.Now;
                        }
                    }
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
                        remotingHandler = new ScriptRemoting.RemotingClientHandler(null, null, mClientIP,_Referer);

                        ActivedVirtualWebSocketHandler.Add(id, remotingHandler);
                    }
                    remotingHandler.Tag2 = true;
                    if (remotingHandler.mSendDataFunc == null)
                    {
                        remotingHandler.mSendDataFunc = (data) =>
                        {
                            try
                            {
                                int count = 0;
                                while (remotingHandler.Tag1 == null && count < 2000)
                                {
                                    System.Threading.Thread.Sleep(10);
                                    count++;
                                }

                                RemotingClientHandler.SendDataHandler func = remotingHandler.Tag1 as RemotingClientHandler.SendDataHandler;
                                if (func == null)
                                {
                                    if (ActivedVirtualWebSocketHandler.ContainsKey(id))
                                    {
                                        remotingHandler.OnDisconnected();
                                        ActivedVirtualWebSocketHandler.Remove(id);
                                      
                                    }
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
                            remotingHandler.OnDisconnected();
                            mCloseConnectionFunc();
                            ActivedVirtualWebSocketHandler.Remove(id);

                        }
                        catch
                        {
                        }
                    };
                    remotingHandler.Tag1 = mSendFunc;

                    if (mWaitForCloseFunc() == 0)//如果不是0，表示是mvc结构，无法检测socket的断开状态
                    {

                        if ((bool)remotingHandler.Tag2)
                        {
                            remotingHandler.OnDisconnected();
                            if (ActivedVirtualWebSocketHandler.ContainsKey(id))
                            {
                                ActivedVirtualWebSocketHandler.Remove(id);
                            }
                        }
                    }
                    return;
                }
                else if (mode == "send")
                {
                    string data = mRequestForms["data"];
                    string id = mRequestForms["id"];
                    string binaryType = mRequestForms["binaryType"];
                    int count = 0;
                    while (!ActivedVirtualWebSocketHandler.ContainsKey(id) && count < 300)
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
                                    bs[i - 1] = (byte)Convert.ToInt32(strArr[i], 16);
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
            }
            catch
            {

            }
            //wait for close
            mWaitForCloseFunc();
        }
    }
}
