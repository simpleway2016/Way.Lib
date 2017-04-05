using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Way.Lib.ScriptRemoting.Net
{
    public class Response
    {
        bool _sendedHeader = false;
        bool _makeHeaders = false;
        internal NetStream mClient;
        private ValueCollection _Headers = new ValueCollection();
        public ValueCollection Headers
        {
            get
            {
                return _Headers;
            }
        }
        System.IO.MemoryStream _buffer;
        System.IO.MemoryStream buffer
        {
            get
            {
                if (_buffer == null)
                    _buffer = new System.IO.MemoryStream();
                return _buffer;
            }
        }

        internal Response(NetStream client)
        {
            mClient = client;
        }
        static string[][] s_HTTPStatusDescriptions;
        static Response()
        {
            s_HTTPStatusDescriptions = new string[][]
    {
        null,
        new string[]
        {
            "Continue",
            "Switching Protocols",
            "Processing"
        },
        new string[]
        {
            "OK",
            "Created",
            "Accepted",
            "Non-Authoritative Information",
            "No Content",
            "Reset Content",
            "Partial Content",
            "Multi-Status"
        },
        new string[]
        {
            "Multiple Choices",
            "Moved Permanently",
            "Found",
            "See Other",
            "Not Modified",
            "Use Proxy",
            string.Empty,
            "Temporary Redirect"
        },
        new string[]
        {
            "Bad Request",
            "Unauthorized",
            "Payment Required",
            "Forbidden",
            "Not Found",
            "Method Not Allowed",
            "Not Acceptable",
            "Proxy Authentication Required",
            "Request Timeout",
            "Conflict",
            "Gone",
            "Length Required",
            "Precondition Failed",
            "Request Entity Too Large",
            "Request-Uri Too Long",
            "Unsupported Media Type",
            "Requested Range Not Satisfiable",
            "Expectation Failed",
            string.Empty,
            string.Empty,
            string.Empty,
            string.Empty,
            "Unprocessable Entity",
            "Locked",
            "Failed Dependency"
        },
        new string[]
        {
            "Internal Server Error",
            "Not Implemented",
            "Bad Gateway",
            "Service Unavailable",
            "Gateway Timeout",
            "Http Version Not Supported",
            string.Empty,
            "Insufficient Storage"
        }
    };
        }

        byte[] getBytes(string content)
        {
            return System.Text.Encoding.UTF8.GetBytes(content);
        }
        internal void WriteStringBody(string content)
        {
            byte[] data = getBytes(content);
            _Headers["Content-Length"] = data.Length.ToString();
            this.Write(data);
        }
        public void Write(string content)
        {
            byte[] data = getBytes(content);
            this.Write(data);
        }
        public void Write(byte[] content)
        {
            Write(content, 0, content.Length);
        }
        public void Write(byte[] content , int offset ,int count)
        {
            if (_Headers["Content-Length"] == null)
            {
                this.buffer.Write(content , offset , count);
                return;
            }

            if (!_sendedHeader)
            {
                _sendedHeader = true;
                if (!_makeHeaders)
                {
                    MakeResponseHeaders(-1, false, -1, 0, DateTime.Now.ToUniversalTime().ToString("R", System.Globalization.DateTimeFormatInfo.InvariantInfo), null, true);
                }
                StringBuilder buffer = new StringBuilder();
                foreach (var headeritem in this.Headers)
                {
                    buffer.Append($"{headeritem.Key}: {headeritem.Value}\r\n");
                }
                buffer.Append("\r\n");
                mClient.Socket.Send(getBytes("HTTP/1.1 200 OK\r\n"));
                mClient.Socket.Send(getBytes(buffer.ToString()));
            }
            if(_buffer != null && _buffer.Length > 0)
            {
                byte[] bs = new byte[_buffer.Length];
                _buffer.Position = 0;
                _buffer.Read(bs, 0, bs.Length);
                _buffer.Dispose();
                _buffer = null;
                mClient.Socket.Send(bs, 0, bs.Length, System.Net.Sockets.SocketFlags.None);
            }
            mClient.Socket.Send(content , offset , count , System.Net.Sockets.SocketFlags.None);
        }
        internal void SendFileNoChanged()
        {
            mClient.Socket.Send(getBytes("HTTP/1.1 304 " + GetStatusDescription(304) + "\r\nConnection: Close\r\n\r\n"));
            this.End();
        }
        internal void SendFileNotFound()
        {
            mClient.Socket.Send(getBytes("HTTP/1.1 404 " + GetStatusDescription(404) + "\r\nConnection: Close\r\n\r\n"));
            this.End();
        }
        internal void CloseSocket()
        {
            mClient.Close();
            mClient = null;
        }
        public void End()
        {
            if (_buffer != null && _buffer.Length > 0)
            {
                _Headers["Content-Length"] = _buffer.Length.ToString();
                byte[] bs = new byte[_buffer.Length];
                _buffer.Position = 0;
                _buffer.Read(bs, 0, bs.Length);
                _buffer.Dispose();
                _buffer = null;
                this.Write(bs , 0 , bs.Length);
            }
            if (mClient == null)
                return;
           
            //wait for close
            while (true)
            {
                try
                {
                    mClient.ReceiveDatas(1);
                }
                catch
                {
                    break;
                }
            }
            mClient.Close();
            mClient = null;
        }

        internal static string GetStatusDescription(int code)
        {
            if (code >= 100 && code < 600)
            {
                int num = code / 100;
                int num2 = code % 100;
                if (num2 < s_HTTPStatusDescriptions[num].Length)
                {
                    return s_HTTPStatusDescriptions[num][num2];
                }
            }
            return string.Empty;
        }
        void setHeaderIfNot(string key , string value)
        {
            if (string.IsNullOrEmpty(value))
                return;

            if (this.Headers[key] == null)
            {
                this.Headers[key] = value;
            }
        }
        internal void MakeResponseHeaders( 
           long contentLength, bool keepAlive,
           int range, int rangeend, string lastModifyDate, string cookie, bool AcceptRanges)
        {
            _makeHeaders = true;
            setHeaderIfNot("Server" , "WayServer");
            setHeaderIfNot("Set-Cookie", cookie);
            setHeaderIfNot("Date", DateTime.Now.ToUniversalTime().ToString("R", System.Globalization.DateTimeFormatInfo.InvariantInfo));
            setHeaderIfNot("Last-Modified", lastModifyDate);

            if (contentLength >= 0)
            {
                if (range >= 0)
                {
                    setHeaderIfNot("Content-Length", (rangeend - range + 1).ToString());
                }
                else
                {
                    setHeaderIfNot("Content-Length", contentLength.ToString());
                }
            }
            setHeaderIfNot("Content-Type", "text/html");
            if (range >= 0)
            {
                setHeaderIfNot("Content-Range", "bytes " + range + "-" + rangeend + "/" + contentLength);
            }

            if (AcceptRanges == false)
            {
                setHeaderIfNot("Accept-Ranges", "none");
            }
            else if (range == 0)
            {
                setHeaderIfNot("Accept-Ranges", "bytes");
            }

            //sb.Append("Content-Encoding: gzip\r\n");//压缩
            if (!keepAlive)
            {
                setHeaderIfNot("Connection", "Close");
            }
            
        }
    }
}
