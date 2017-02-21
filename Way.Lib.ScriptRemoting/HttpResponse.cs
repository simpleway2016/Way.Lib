using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Way.Lib.ScriptRemoting
{
    public class HttpResponse
    {
        internal NetStream _client;
        StringBuilder _content = new StringBuilder();
        internal HttpResponse(NetStream netclient)
        {
            _client = netclient;
        }
        public void Write(string text)
        {
            _content.Append(text);
        }

        static string[][] s_HTTPStatusDescriptions;
        static HttpResponse()
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
        internal static String MakeResponseHeaders(int statusCode, String moreHeaders,
           long contentLength, bool keepAlive,
           int range, int rangeend, string lastModifyDate, string cookie, bool AcceptRanges)
        {
            StringBuilder sb = new StringBuilder();
            if (range >= 0)
                statusCode = 206;


            sb.Append("HTTP/1.1 " + statusCode + " " + GetStatusDescription(statusCode) + "\r\n");
            sb.Append("Server: Microsoft-IIS6.0\r\n");
            if (cookie != null)
                sb.Append("Set-Cookie: " + cookie + "\r\n");
            sb.Append("Date: " + DateTime.Now.ToUniversalTime().ToString("R", System.Globalization.DateTimeFormatInfo.InvariantInfo) + "\r\n");
            if (lastModifyDate != null)
            {
                sb.Append("Last-Modified: " + lastModifyDate + "\r\n");
            }
            if (contentLength >= 0)
            {
                if (range >= 0)
                    sb.Append("Content-Length: " + (rangeend - range + 1) + "\r\n");
                else
                {
                    sb.Append("Content-Length: " + contentLength + "\r\n");
                }
            }
            if (range >= 0)
            {
                sb.Append("Content-Range: bytes " + range + "-" + rangeend + "/" + contentLength + "\r\n");
            }
            if (moreHeaders != null)
                sb.Append(moreHeaders);

            if (AcceptRanges == false)
            {
                sb.Append("Accept-Ranges: none\r\n");
            }
            else if (range == 0)
            {
                sb.Append("Accept-Ranges: bytes\r\n");
            }

            //sb.Append("Content-Encoding: gzip\r\n");//压缩
            if (!keepAlive)
                sb.Append("Connection: Close\r\n");

            sb.Append("\r\n");
            return sb.ToString();
        }
        public void End()
        {
            if (_client == null)
                return;
            byte[] bs = System.Text.Encoding.UTF8.GetBytes(_content.ToString());
            _content.Clear();
            _content = null;
            string headers = MakeResponseHeaders(200, "Content-Type: text/html\r\n", bs.Length, false, -1, 0, DateTime.Now.ToUniversalTime().ToString("R", System.Globalization.DateTimeFormatInfo.InvariantInfo), null, true);
            _client.Socket.Send(System.Text.Encoding.UTF8.GetBytes(headers));
            _client.Socket.Send(bs);

            //wait for close
            while (true)
            {
                try
                {
                    _client.ReceiveDatas(1);
                }
                catch
                {
                    break;
                }
            }
            _client.Close();
            _client = null;
        }
    }
}
