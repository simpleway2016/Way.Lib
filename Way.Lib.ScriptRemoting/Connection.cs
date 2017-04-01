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
        NetStream mClient;
        internal Net.Request Request;

        public string SessionID = Guid.NewGuid().ToString();
        public Connection(Socket socket)
        {
            mClient = new NetStream(socket);
        }

 
        public void OnConnect()
        {
            try
            {
                Request = new Net.Request(mClient);

                ISocketHandler handler = null;
                if ("Upgrade".Equals(Request.Headers["Connection"]) == false)
                {
                    handler = new HttpSocketHandler(Request);
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
