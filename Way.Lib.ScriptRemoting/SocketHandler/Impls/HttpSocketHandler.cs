using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Text.RegularExpressions;

namespace Way.Lib.ScriptRemoting
{
    class HttpSocketHandler : ISocketHandler
    {
        HttpConnectInformation _currentHttpConnectInformation;
        static string[][] s_HTTPStatusDescriptions;
        static HttpSocketHandler()
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

        public static string GetStatusDescription(int code)
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

        //static List<string> compiledTSFiles = new List<string>();
        Dictionary<string, string> RequestForms;
        Dictionary<string, string> RequestQueryString = new Dictionary<string, string>();
        public Connection Connection
        {
            get;
            private set;
        }
        public HttpSocketHandler(Connection session)
        {
            this.Connection = session;
        }
        public void Handle()
        {
            try
            {
                //访问ts脚本
                if (this.Connection.mKeyValues["GET"].ToSafeString().EndsWith("/SERVERID"))
                {
                    outputHttpResponse(ScriptRemotingServer.SERVERID);
                }
                else if (this.Connection.mKeyValues["GET"].ToSafeString().ToLower().EndsWith("wayscriptremoting"))
                {
                    //m_client.Socket.Send(System.Text.Encoding.UTF8.GetBytes("HTTP/1.1 304 " + System.Web.HttpWorkerRequest.GetStatusDescription(304) + "\r\nConnection: Close\r\n\r\n"));
                    var since = this.Connection.mKeyValues["If-Modified-Since"].ToSafeString();
                    var lastWriteTime = new System.IO.FileInfo(ScriptRemotingServer.ScriptFilePath).LastWriteTime.ToString("R");
                    if (lastWriteTime == since)
                    {
                        this.Connection.mClient.Socket.Send(System.Text.Encoding.UTF8.GetBytes("HTTP/1.1 304 " + GetStatusDescription(304) + "\r\nConnection: Close\r\n\r\n"));
                    }
                    else
                    {
                        outputFile(ScriptRemotingServer.ScriptFilePath, lastWriteTime);
                    }
                }
                else if (this.Connection.mKeyValues["POST"].ToSafeString().ToLower().StartsWith("/wayscriptremoting_invoke?a="))
                {
                    urlRequestHandler();
                    string json = RequestForms["m"];
                    RemotingClientHandler rs = new ScriptRemoting.RemotingClientHandler((string data) =>
                    {
                        outputHttpResponse(data);
                    }, null, this.Connection.mClient.Socket.RemoteEndPoint.ToString().Split(':')[0], (string)this.Connection.mKeyValues["Referer"]);
                    rs.OnReceived(json);

                }
                else if (this.Connection.mKeyValues["POST"].ToSafeString().EndsWith("?WayVirtualWebSocket=1")
                    || this.Connection.mKeyValues["POST"].ToSafeString().EndsWith("&WayVirtualWebSocket=1"))
                {
                    urlRequestHandler();
                    new VirtualWebSocketHandler(RequestForms, (data) =>
                   {
                       outputHttpResponse(data);
                   }, () =>
                     {
                         while (true)
                         {
                             try
                             {
                                 this.Connection.mClient.ReceiveDatas(1);
                             }
                             catch
                             {
                                 break;
                             }
                         }
                         this.Connection.mClient.Close();

                         return 0;
                     }, () =>
                      {
                          this.Connection.mClient.Close();
                          return 0;
                      }, this.Connection.mClient.Socket.RemoteEndPoint.ToString().Split(':')[0],(string)this.Connection.mKeyValues["Referer"]

                    ).Handle();

                    return;
                }
                else if (this.Connection.mKeyValues["Content-Type"].ToSafeString().Contains("x-www-form-urlencoded"))
                {
                    try
                    {
                        urlRequestHandler();

                    }
                    catch (Exception ex)
                    {
                        
                    }
                }
                else
                {
                  
                    string url = this.Connection.mKeyValues["GET"].ToSafeString();
                    if (url.Contains("?"))
                    {
                        MatchCollection matches = Regex.Matches(url, @"(?<n>(\w)+)\=(?<v>([^\=\&])+)");
                        foreach( Match m in matches )
                        {
                            RequestQueryString[m.Groups["n"].Value] = WebUtility.UrlDecode(m.Groups["v"].Value);
                        }
                        url = url.Substring(0, url.IndexOf("?"));
                    }
                    url = getUrl(url);

                    string filepath = url;
                    if (filepath.StartsWith("/"))
                        filepath = filepath.Substring(1);
                    filepath = ScriptRemotingServer.Root + filepath;
                    string ext = Path.GetExtension(filepath).ToLower();
                    if (ext == ".aspx")
                    {
                        outputAspx(filepath);
                    }
                    else if (ext == ".html")
                    {
                        outputFile(url , filepath);
                    }
                    else
                    {
                        outputFile(url, filepath);
                    }
                }
            }
            catch
            {
            }


            //wait for close
            while (true)
            {
                try
                {
                    this.Connection.mClient.ReceiveDatas(1);
                }
                catch
                {
                    break;
                }
            }
            this.Connection.mClient.Close();
        }

        string getUrl(string visitingUrl)
        {
            
            if (ScriptRemotingServer.Routers.Count > 0 && _currentHttpConnectInformation == null)
            {
                _currentHttpConnectInformation = new HttpConnectInformation(this.Connection.mKeyValues, this.Connection.mClient.Socket.RemoteEndPoint.ToString().Split(':')[0]);
            }
            foreach (var router in ScriptRemotingServer.Routers)
            {

                var url = router.GetUrl(visitingUrl, (string)this.Connection.mKeyValues["Referer"], _currentHttpConnectInformation,RequestQueryString);
                if (url != null)
                {
                    visitingUrl = url;
                    break;
                }
            }
            if (visitingUrl == "/")
            {
                visitingUrl = "/index.html";
            }
            return visitingUrl;
        }

        void outputAspx(string filepath)
        {

        }
        void outputHttpResponse(string text)
        {
            byte[] bs = System.Text.Encoding.UTF8.GetBytes(text);
            string headers = MakeResponseHeaders(200, "Content-Type: text/html\r\n", bs.Length, false, -1, 0, DateTime.Now.ToUniversalTime().ToString("R", System.Globalization.DateTimeFormatInfo.InvariantInfo), null, true);
            this.Connection.mClient.Socket.Send(System.Text.Encoding.UTF8.GetBytes(headers));
            this.Connection.mClient.Socket.Send(bs);

        }

        //void outputTS(string tspath,string jspath)
        //{
        //    if (compiledTSFiles.Contains(tspath) == false)
        //    {
        //        var typescriptCompiler = new ProcessStartInfo
        //        {
        //            WindowStyle = ProcessWindowStyle.Hidden,
        //            FileName = ScriptRemotingServer.Root + "_WayScriptRemoting/tsc/tsc.exe",
        //            Arguments = string.Format("\"{0}\"", tspath)
        //        };

        //        var process = Process.Start(typescriptCompiler);
        //        process.WaitForExit();
        //        compiledTSFiles.Add(tspath);
        //    }
        //    outputFile(jspath);
        //}

        void outputFile(string url, string filePath)
        {
            if (File.Exists(filePath) == false)
            {
                this.Connection.mClient.Socket.Send(System.Text.Encoding.UTF8.GetBytes("HTTP/1.1 404 " + GetStatusDescription(404) + "\r\nConnection: Close\r\n\r\n"));
                return;
            }
            var since = this.Connection.mKeyValues["If-Modified-Since"].ToSafeString();
            var lastWriteTime = new System.IO.FileInfo(filePath).LastWriteTime.ToString("R");
            if (lastWriteTime == since)
            {
                this.Connection.mClient.Socket.Send(System.Text.Encoding.UTF8.GetBytes("HTTP/1.1 304 " + GetStatusDescription(304) + "\r\nConnection: Close\r\n\r\n"));
            }
            else
            {
                outputFile(url , filePath, lastWriteTime);
            }
        }

        string outputWithMaster(string url, string filePath)
        {
            using (var fs = new System.IO.FileStream(filePath, System.IO.FileMode.Open, System.IO.FileAccess.Read, System.IO.FileShare.Read))
            {
                byte[] bs = new byte[20];
                fs.Read(bs, 0, bs.Length);
                fs.Position = 0;

                if (System.Text.Encoding.UTF8.GetString(bs).StartsWith("<Master "))
                {
                    Way.Lib.HtmlUtil.HtmlParser parser = new HtmlUtil.HtmlParser();
                    parser.Parse(new StreamReader(fs));

                    var masterUrl = parser.Nodes[0].Attributes.Where(m => m.Name == "src").Select(m => m.Value).FirstOrDefault();
                    if (masterUrl.StartsWith("/") == false)
                    {
                        if (url.EndsWith("/") == false)
                        {
                            if (url.Contains("/"))
                                url = url.Substring(0, url.LastIndexOf("/"));
                            url += "/";
                        }
                        masterUrl = url + masterUrl;
                    }
                    masterUrl = getUrl(masterUrl);
                    string filepath = masterUrl;
                    if (filepath.StartsWith("/"))
                        filepath = filepath.Substring(1);
                    filepath = ScriptRemotingServer.Root + filepath;
                    string masterContent = outputWithMaster(masterUrl, filepath);
                    foreach( HtmlUtil.HtmlNode node in parser.Nodes )
                    {
                        if(node.Name == "Variable")
                        {
                            var name = node.Attributes.Where(m => m.Name == "name").Select(m => m.Value).FirstOrDefault();
                            masterContent = masterContent.Replace($"{{%{name}%}}" , node.getInnerHtml());
                        }
                    }
                    return masterContent;
                }
                else
                {
                    bs = new byte[fs.Length];
                    fs.Read(bs, 0, bs.Length);
                    return System.Text.Encoding.UTF8.GetString(bs);
                }
            }
        }
        static Dictionary<string, string> MasterFileTemps = new Dictionary<string, string>();
         void outputFile(string url , string filePath , string lastModifyTime)
        {
            string headers;
            byte[] bs;
            var fs = new System.IO.FileStream(filePath, System.IO.FileMode.Open, System.IO.FileAccess.Read, System.IO.FileShare.Read);
            if (filePath.EndsWith(".html" , StringComparison.CurrentCultureIgnoreCase))
            {
                bs = new byte[20];
                fs.Read(bs , 0 , bs.Length);
                fs.Position = 0;
                if (System.Text.Encoding.UTF8.GetString(bs).StartsWith("<Master "))
                {
                    //母版模式
                    fs.Dispose();
                    if (MasterFileTemps.ContainsKey(filePath) && new FileInfo(MasterFileTemps[filePath]).LastWriteTime == new FileInfo(filePath).LastWriteTime)
                    {
                        fs = new System.IO.FileStream(MasterFileTemps[filePath], System.IO.FileMode.Open, System.IO.FileAccess.Read, System.IO.FileShare.Read);
                    }
                    else
                    {
                        //文件更新，解析过需要清除，重新解析
                        for(int i = 0; i < RemotingController.ParsedHtmls.Count; i ++)
                        {
                            var info = RemotingController.ParsedHtmls[i];
                            if (info != null)
                            {
                                string infourl = info.Url;
                                Match m = Regex.Match(infourl, @"http(s)?://(\w|\:|\.)+(?<u>/(.)+)");
                                infourl = m.Groups["u"].Value;
                                if (infourl.StartsWith(url , StringComparison.CurrentCultureIgnoreCase))
                                {
                                    RemotingController.ParsedHtmls.RemoveAt(i);
                                    i--;
                                }
                            }
                        }
                        var content = outputWithMaster(url, filePath);
                        bs = System.Text.Encoding.UTF8.GetBytes(content);
                        string temppath = ScriptRemotingServer.HtmlTempPath + "/" + Guid.NewGuid();
                        File.WriteAllBytes(temppath, bs);
                        new FileInfo(temppath).LastWriteTime = new FileInfo(filePath).LastWriteTime;
                        try
                        {
                            MasterFileTemps[filePath] = temppath;
                        }
                        catch
                        { }
                        headers = MakeResponseHeaders(200, MakeContentTypeHeader(filePath), bs.Length, false, -1, 0, lastModifyTime, null, true);
                        this.Connection.mClient.Socket.Send(System.Text.Encoding.UTF8.GetBytes(headers));
                        this.Connection.mClient.Socket.Send(bs, bs.Length, System.Net.Sockets.SocketFlags.None);
                        return;
                    }
                }
                else
                {
                    DateTime lastModified = DateTime.Parse(lastModifyTime);
                    //文件更新，解析过需要清除，重新解析
                    for (int i = 0; i < RemotingController.ParsedHtmls.Count; i++)
                    {
                        var info = RemotingController.ParsedHtmls[i];
                        if (info != null)
                        {
                            string infourl = info.Url;
                            Match m = Regex.Match(infourl, @"http(s)?://(\w|\:|\.)+(?<u>/(.)+)");
                            infourl = m.Groups["u"].Value;
                            if (infourl.StartsWith(url, StringComparison.CurrentCultureIgnoreCase))
                            {
                                if (lastModified != info.LastModified)
                                {
                                    RemotingController.ParsedHtmls.RemoveAt(i);
                                    i--;
                                }
                            }
                        }
                    }
                }
            }
             
            headers = MakeResponseHeaders(200, MakeContentTypeHeader(filePath), fs.Length, false, -1, 0, lastModifyTime, null, true);
            this.Connection.mClient.Socket.Send(System.Text.Encoding.UTF8.GetBytes(headers));

            bs = new byte[4096];
            while (true)
            {
                int count = fs.Read(bs, 0, bs.Length);
                if (count == 0)
                    break;
                this.Connection.mClient.Socket.Send(bs ,count , System.Net.Sockets.SocketFlags.None );
            }
            fs.Dispose();
                

        }
        void urlRequestHandler()
        {
            int contentLength = Convert.ToInt32(this.Connection.mKeyValues["Content-Length"]);
            List<byte> buffer = new List<byte>();
            RequestForms = new Dictionary<string, string>();
            string keyName = null;
            for (int i = 0; i < contentLength; i++)
            {
                int b = this.Connection.mClient.ReadByte();
                if (b == (int)'=')
                {
                    keyName = System.Text.Encoding.UTF8.GetString(buffer.ToArray());
                    buffer.Clear();

                    continue;
                }
                else if(b == (int)'&')
                {
                    
                    string str = System.Text.Encoding.UTF8.GetString(buffer.ToArray());
                    string value = WebUtility.UrlDecode(str);
                    buffer.Clear();
                    RequestForms.Add(keyName, value);
                    keyName = null;
                    continue;
                }
                buffer.Add((byte)b);
            }
            if(buffer.Count > 0 && keyName != null)
            {
                string str = System.Text.Encoding.UTF8.GetString(buffer.ToArray());
                string value = WebUtility.UrlDecode(str);
                RequestForms.Add(keyName, value);
            }
        }

        private static String MakeContentTypeHeader(string fileName)
        {
            string contentType = null;

            int lastDot = fileName.LastIndexOf('.');

            if (lastDot >= 0)
            {
                switch (fileName.Substring(lastDot).ToLower())
                {
                    case ".js": contentType = "application/javascript"; break;
                    case ".gif": contentType = "image/gif"; break;
                    case ".jpg": contentType = "image/jpeg"; break;
                    case ".jpeg": contentType = "image/jpeg"; break;
                    case ".png": contentType = "image/png"; break;
                    case ".htm":
                    case ".html":
                    case ".aspx":
                        return "Content-Type: text/html\r\n";
                    default:
                        string filename = WebUtility.UrlEncode(fileName);
                        return "Content-Type: application/octet-stream\r\nContent-Disposition: attachment;filename=" + filename + "\r\n";
                }
            }

            if (contentType == null)
                return null;

            return "Content-Type: " + contentType + "\r\n";
        }

        private String MakeResponseHeaders(int statusCode, String moreHeaders,
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
    }
}
