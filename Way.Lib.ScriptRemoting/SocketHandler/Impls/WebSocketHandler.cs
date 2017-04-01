using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Way.Lib.ScriptRemoting
{
    class WebSocketHandler : ISocketHandler
    {
        RemotingClientHandler mRemotingHandler;
        public static Hashtable ReadyWebSocketHandlers = Hashtable.Synchronized(new Hashtable());
        public string SessionID
        {
            get;
            private set;
        }

        NetStream mClient;
        protected Connection Connection
        {
            get;
            private set;
        }

        /// <summary>
        /// 响应串
        /// </summary>
        public string Response
        {
            get
            {
                string secWebSocketKey = this.Connection.Request.Headers["Sec-WebSocket-Key"].ToString();
                string m_Magic = "258EAFA5-E914-47DA-95CA-C5AB0DC85B11";
                string responseKey = Convert.ToBase64String(SHA1.Create().ComputeHash(Encoding.ASCII.GetBytes(secWebSocketKey + m_Magic)));

                StringBuilder response = new StringBuilder(); //响应串
                response.Append("HTTP/1.1 101 Web Socket Protocol Way\r\n");

                //将请求串的键值转换为对应的响应串的键值并添加到响应串
                response.AppendFormat("Upgrade: {0}\r\n", this.Connection.Request.Headers["Upgrade"]);
                response.AppendFormat("Connection: {0}\r\n", this.Connection.Request.Headers["Connection"]);
                response.AppendFormat("Sec-WebSocket-Accept: {0}\r\n", responseKey);
                response.AppendFormat("WebSocket-Origin: {0}\r\n", this.Connection.Request.Headers["Origin"]);
                response.AppendFormat("WebSocket-Location: {0}\r\n", this.Connection.Request.Headers["Host"]);

                response.Append("\r\n");

                return response.ToString();
            }
        }

        public WebSocketHandler(Connection session)
        {
            this.Connection = session;
            mClient = session.Request.mClient;
            mRemotingHandler = new ScriptRemoting.RemotingClientHandler((string data) =>
            {
                byte[] bs = new DataFrame(data).GetBytes();
                mClient.Socket.Send(bs);
            } , ()=>
            {
                mClient.Close();
            },session.Request.mClient.Socket.RemoteEndPoint.ToString().Split(':')[0],null);
        }
        public void Handle()
        {
            mClient.WriteString(this.Response);

            try
            {
                while (true)
                {
                    if (mRemotingHandler.StreamType == RemotingClientHandler.RemotingStreamType.Text)
                    {
                        string text = readSocketText();
                        if (text.Length == 0)
                        {
                            //客户端断开
                            mClient.Close();
                            continue;
                        }
                        try
                        {
                            mRemotingHandler.OnReceived(text);
                        }
                        catch
                        {
                        }
                    }
                    else
                    {
                        byte[] content = readSocketBytes();
                        if (content.Length == 0)
                        {
                            //客户端断开
                            mClient.Close();
                            continue;
                        }
                        try
                        {
                            mRemotingHandler.OnReceived(content);
                        }
                        catch
                        {
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                mRemotingHandler.OnDisconnected();
            }
        }

        string readSocketText()
        {
            DataFrame frame = new DataFrame(readBytes());
            return frame.Text;
        }
        byte[] readSocketBytes()
        {
            DataFrame frame = new DataFrame(readBytes());
            return frame.Content;
        }
        byte[] readBytes()
        {
            byte[] data = mClient.ReceiveDatas(2);

            DataFrameHeader _header = new DataFrameHeader(data);
            byte[] _extend = new byte[0];
            int maskLength = _header.HasMask ? 4 : 0;
            if (_header.Length == 126)
            {
                _extend = mClient.ReceiveDatas(2);
            }
            else if (_header.Length == 127)
            {
                _extend = mClient.ReceiveDatas(8);
            }
            int contentLength;
            //消息体
            if (_extend.Length == 0)
            {
                contentLength = _header.Length + maskLength;
            }
            else if (_extend.Length == 2)
            {
                _extend = new byte[] { _extend[1], _extend[0] };//低位 高位对调一下
                contentLength = BitConverter.ToUInt16(_extend, 0) + maskLength;
            }
            else
            {
                _extend = new byte[] { _extend[7], _extend[6], _extend[5], _extend[4], _extend[3], _extend[2], _extend[1], _extend[0] };
                contentLength = (int)BitConverter.ToUInt64(_extend, 0) + maskLength;
            }
            int contentOffset = data.Length + _extend.Length;
            byte[] bufferData = new byte[data.Length + _extend.Length + contentLength];
            Array.Copy(data, bufferData, data.Length);
            Array.Copy(_extend, 0, bufferData, data.Length, _extend.Length);

            mClient.ReceiveDatas(bufferData, contentOffset, contentLength);
            return bufferData;
        }

    }

}
