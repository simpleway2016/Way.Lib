﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Way.Lib.ScriptRemoting.Net
{
    public class Response : System.IO.Stream
    {
        /// <summary>
        /// 全局默认输出的Header
        /// </summary>
        public static Dictionary<string, string> DefaultHeaders = new Dictionary<string, string>();
        bool _sendedHeader = false;
        bool _makeHeaders = false;
        internal NetStream mClient;
        RemotingContext _context;
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
        RemotingContext _Context;
        public RemotingContext Context
        {
            get
            {
                if (_Context == null)
                    _Context = RemotingContext.Current;
                return _context;
            }
        }

        public override bool CanRead
        {
            get
            {
                return false;
            }
        }

        public int StatusCode = 200;

        public override bool CanSeek
        {
            get
            {
                return false;
            }
        }

        public override bool CanWrite
        {
            get
            {
                return true;
            }
        }

        public override long Length
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public int? ContentLength
        {
            get
            {
                if (_Headers.ContainsKey("Content-Length") == false)
                    return null;
                return Convert.ToInt32(_Headers["Content-Length"]);
            }
            set
            {
                if (value == null)
                {
                    if (_Headers.ContainsKey("Content-Length"))
                    {
                        _Headers.Remove("Content-Length");
                    }
                }
                else
                    _Headers["Content-Length"] = value.ToString();
            }
        }

        public override long Position
        {
            get
            {
                throw new NotImplementedException();
            }

            set
            {
                throw new NotImplementedException();
            }
        }

        internal Response(NetStream client,RemotingContext context)
        {
            _context = context;
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

        /// <summary>
        /// 把数据进行gzip压缩
        /// </summary>
        /// <param name="byteArray"></param>
        /// <returns></returns>
        public byte[] GZip(byte[] byteArray)
        {
            using (System.IO.MemoryStream ms = new System.IO.MemoryStream())
            {
                System.IO.Compression.GZipStream sw = new System.IO.Compression.GZipStream(ms, System.IO.Compression.CompressionMode.Compress);
                //Compress
                sw.Write(byteArray, 0, byteArray.Length);
                //Close, DO NOT FLUSH cause bytes will go missing...
                sw.Close();
                //Transform byte[] zip data to string
                return ms.ToArray();
            }
        }

        byte[] getBytes(string content)
        {
            string contentType = null;
            string charset = null;
            if (this.Headers.ContainsKey("Content-Type"))
            {
                contentType = this.Headers["Content-Type"];
                if(contentType.Contains(";charset="))
                {
                    try
                    {
                        var charsetGroup = contentType.Split(';').FirstOrDefault(m => m.Trim().StartsWith("charset="));
                        charset = charsetGroup.Substring(charsetGroup.IndexOf("=") + 1);
                        if (charset.Length == 0 || charset == "utf-8")
                            charset = null;
                    }
                    catch
                    {

                    }
                }
            }
            var encoding = charset == null ? System.Text.Encoding.UTF8 : System.Text.Encoding.GetEncoding(charset);
            return encoding.GetBytes(content);
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
        public void WriteFile(Stream filestream)
        {
            MakeResponseHeaders((int)filestream.Length, false, -1, 0, DateTime.Now.ToUniversalTime().ToString("R", System.Globalization.DateTimeFormatInfo.InvariantInfo), null, true);
            StringBuilder buffer = new StringBuilder();
            buffer.Append($"HTTP/1.1 {this.StatusCode} {GetStatusDescription(this.StatusCode)}\r\n");
            foreach (var headeritem in this.Headers)
            {
                try
                {
                    buffer.Append($"{headeritem.Key}: {headeritem.Value}\r\n");
                }
                catch
                {

                }
            }
            buffer.Append("\r\n");
            mClient.Write(System.Text.Encoding.UTF8.GetBytes(buffer.ToString()));


            byte[] bs = new byte[4096];
            var total = filestream.Length;
            while (total > 0)
            {
                var readed = filestream.Read(bs, 0, bs.Length);
                total -= readed;
                mClient.Write(bs, 0, readed);
            }
            this.End();
        }
        public override void Write(byte[] content , int offset ,int count)
        {
            if (mClient == null)
                return;

            if (_Headers["Content-Length"] == null && this.buffer != null)
            {
                this.buffer.Write(content, offset, count);
                if (this.buffer.Length > 1024*8)
                {
                    content = new byte[_buffer.Length];
                    _buffer.Position = 0;
                    _buffer.Read(content, 0, content.Length);
                    _buffer.Dispose();
                    _buffer = null;

                    offset = 0;
                    count = content.Length;
                }
                else
                {
                    return;
                }
            }

            if (!_sendedHeader)
            {
                _sendedHeader = true;
                if (!_makeHeaders)
                {
                    MakeResponseHeaders(-1, false, -1, 0, DateTime.Now.ToUniversalTime().ToString("R", System.Globalization.DateTimeFormatInfo.InvariantInfo), null, true);
                }
                StringBuilder buffer = new StringBuilder();
                buffer.Append($"HTTP/1.1 {this.StatusCode} {GetStatusDescription(this.StatusCode)}\r\n");
                foreach (var headeritem in this.Headers)
                {
                    try
                    {
                        if (headeritem.Key == "Content-Length" && Convert.ToInt32(headeritem.Value) < 0)
                            continue;
                        buffer.Append($"{headeritem.Key}: {headeritem.Value}\r\n");
                    }
                    catch
                    {

                    }
                }
                buffer.Append("\r\n");
                mClient.Write(System.Text.Encoding.UTF8.GetBytes(buffer.ToString()));
            }
            if(_buffer != null && _buffer.Length > 0)
            {
                byte[] bs = new byte[_buffer.Length];
                _buffer.Position = 0;
                _buffer.Read(bs, 0, bs.Length);
                _buffer.Dispose();
                _buffer = null;
                mClient.Write(bs, 0, bs.Length);
            }
            mClient.Write(content , offset , count);
        }
       
        internal void SendFileNoChanged()
        {
            if (mClient == null)
                return;

            if (_buffer != null )
            {
                _buffer = null;
            }
            _sendedHeader = true;
            mClient.Write(System.Text.Encoding.UTF8.GetBytes("HTTP/1.1 304 " + GetStatusDescription(304) + $"\r\nContent-Length: 0\r\nConnection: Close\r\n\r\n"));

            CloseSocket();
        }
        string getConnectString()
        {
            if (Headers.ContainsKey("Connection"))
            {
                return $"Connection: {Headers["Connection"]}";
            }
            return "Connection: Close";
        }

        /// <summary>
        /// 重定向
        /// </summary>
        /// <param name="url"></param>
        public void Redirect(string url)
        {
            Redirect(302, url);
        }

        /// <summary>
        /// 重定向
        /// </summary>
        /// <param name="statusCode">应为301 302 303</param>
        /// <param name="url"></param>
        public void Redirect(int statusCode, string url)
        {
            if (mClient == null)
                return;

            if (_buffer != null)
            {
                _buffer = null;
            }
            _sendedHeader = true;

            mClient.Write(System.Text.Encoding.UTF8.GetBytes($"HTTP/1.1 {statusCode} " + GetStatusDescription(statusCode) + $"\r\nLocation: " + url + $"\r\nConnection: Close\r\n\r\n"));

            CloseSocket();
        }

        internal void SendFileNotFound()
        {
            if (mClient == null)
                return;

            if (_buffer != null)
            {
                _buffer = null;
            }
            _sendedHeader = true;
            mClient.Write(System.Text.Encoding.UTF8.GetBytes("HTTP/1.1 404 " + GetStatusDescription(404) + $"\r\nContent-Length: 0\r\nConnection: Close\r\n\r\n"));

            CloseSocket();
        }
        internal void SendServerError()
        {
            if (mClient == null)
                return;

            if (_buffer != null)
            {
                _buffer = null;
            }
            _sendedHeader = true;
            mClient.Write(System.Text.Encoding.UTF8.GetBytes("HTTP/1.1 504 " + GetStatusDescription(504) + $"\r\nContent-Length: 0\r\nConnection: Close\r\n\r\n"));

            CloseSocket();
        }
        internal void CloseSocket()
        {
            mClient.Socket.Shutdown(System.Net.Sockets.SocketShutdown.Send);//表示发送数据完全结束
            Task.Run(() =>
            {
                mClient.Socket.Receive(new byte[1], 1, System.Net.Sockets.SocketFlags.None);
            }).Wait(3000);

            mClient.Close();
            mClient = null;
        }
        public void End()
        {
            if (mClient == null)
                return;

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

            if(!_sendedHeader)
            {
                _sendedHeader = true;
                mClient.Write(System.Text.Encoding.UTF8.GetBytes($"HTTP/1.1 {this.StatusCode} {GetStatusDescription(this.StatusCode)}\r\nContent-Length: 0\r\n{getConnectString()}\r\n\r\n"));
            }

            if(Headers.ContainsKey("Connection") && string.Equals( Headers["Connection"] , "keep-alive" , StringComparison.CurrentCultureIgnoreCase))
            {
                mClient.ReadTimeout = 6 * 1000;//5秒超时
                var oc = mClient;

                new Thread(()=> {
                    _context.Server.HandleSocket(oc);
                }).Start();
                
                mClient = null;
                return;
            }

            CloseSocket();
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
            if (!string.IsNullOrEmpty(lastModifyDate))
            {
                setHeaderIfNot("Last-Modified", lastModifyDate);
            }

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
            setHeaderIfNot("Content-Type", "text/html;charset=utf-8");
            if (range >= 0)
            {
                setHeaderIfNot("Content-Range", "bytes " + range + "-" + rangeend + "/" + contentLength);
            }

            if (AcceptRanges == false)
            {
                setHeaderIfNot("Accept-Ranges", "none");
            }
            else 
            {
                setHeaderIfNot("Accept-Ranges", "bytes");
            }

            foreach( var h in DefaultHeaders)
            {
                setHeaderIfNot(h.Key, h.Value);
            }

            //sb.Append("Content-Encoding: gzip\r\n");//压缩
            if (!keepAlive)
            {
                setHeaderIfNot("Connection", "Close");
            }
            
        }

        public override void Flush()
        {
            throw new NotImplementedException();
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            throw new NotImplementedException();
        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            throw new NotImplementedException();
        }

        public override void SetLength(long value)
        {
            throw new NotImplementedException();
        }

      
    }
}
