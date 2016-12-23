using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Sockets;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;


namespace Way.Lib.ScriptRemoting
{
    class Connection
    {
        internal NetStream mClient;
        internal Hashtable mKeyValues;

        public string SessionID = Guid.NewGuid().ToString();
        public Connection(Socket socket)
        {
            mKeyValues = new Hashtable();
            mClient = new NetStream(socket);
        }

 
        public void OnConnect()
        {
            try
            {
                while (true)
                {
                    string line = mClient.ReadLine();
                    //Debug.WriteLine(line);
                    if (line.Length == 0)
                        break;
                    else
                    {
                        int flag = line.IndexOf(":");
                        if (flag > 0)
                        {
                            try
                            {
                                string name = line.Substring(0, flag);
                                string value = line.Substring(flag + 1);
                                mKeyValues[name] = value.Trim();
                            }
                            catch
                            {
                            }
                        }
                        else if (line.StartsWith("GET"))
                        {
                            try
                            {
                                mKeyValues["GET"] = line.Split(' ')[1];
                            }
                            catch
                            {
                            }
                        }
                        else if (line.StartsWith("POST"))
                        {
                            try
                            {
                                mKeyValues["POST"] = line.Split(' ')[1];
                            }
                            catch
                            {
                            }
                        }
                    }
                }

                ISocketHandler handler = null;
                if ("Upgrade".Equals(mKeyValues["Connection"]) == false)
                {
                    handler = new HttpSocketHandler(this);
                }
                else
                {
                    handler = new WebSocketHandler(this);
                }
                handler.Handle();
            }
            catch
            {

            }
        }


    }
}
