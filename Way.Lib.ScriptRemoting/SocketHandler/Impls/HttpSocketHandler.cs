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
        static string[] NotAllowDownloadFiles = new string[] { ".dll", ".exe", ".config",".db" };

        //static List<string> compiledTSFiles = new List<string>();

        public Net.Request Request
        {
            get;
            private set;
        }
        public Net.Response Response
        {
            get;
            private set;
        }
        public HttpSocketHandler(Net.Request request)
        {
            this.Request = request;
            this.Response = new Net.Response(request.mClient);
           
        }
        public void Handle()
        {
           
            try
            {
                //访问ts脚本
                if (this.Request.Headers["GET"].ToSafeString().EndsWith("/SERVERID"))
                {
                    this.Response.WriteStringBody(ScriptRemotingServer.SERVERID);
                }
                else if (this.Request.Headers["GET"].ToSafeString().ToLower().EndsWith("wayscriptremoting"))
                {
                    var since = this.Request.Headers["If-Modified-Since"].ToSafeString();
                    var lastWriteTime = new System.IO.FileInfo(ScriptRemotingServer.ScriptFilePath).LastWriteTime.ToString("R");
                    if (lastWriteTime == since)
                    {
                        this.Response.SendFileNoChanged();
                    }
                    else
                    {
                        outputFile(ScriptRemotingServer.ScriptFilePath, lastWriteTime);
                    }
                }
                else if (this.Request.Headers["POST"].ToSafeString().ToLower().StartsWith("/wayscriptremoting_invoke?a="))
                {
                    Request.urlRequestHandler();
                    string json = Request.Form["m"];
                    RemotingClientHandler rs = new ScriptRemoting.RemotingClientHandler((string data) =>
                    {
                        this.Response.WriteStringBody(data);
                    }, null, this.Request.RemoteEndPoint.ToString().Split(':')[0], (string)this.Request.Headers["Referer"],
                    (key) =>
                    {
                        return this.Request.Headers[key];
                    });
                    rs.OnReceived(json);

                }
                else if (this.Request.Headers["POST"].ToSafeString().EndsWith("?WayVirtualWebSocket=1")
                    || this.Request.Headers["POST"].ToSafeString().EndsWith("&WayVirtualWebSocket=1"))
                {
                    Request.urlRequestHandler();
                    new VirtualWebSocketHandler(Request.Form, (data) =>
                   {
                       Response.WriteStringBody(data);
                   }, () =>
                     {
                         Response.End();

                         return 0;
                     }, () =>
                      {
                          this.Response.CloseSocket();
                          return 0;
                      }, this.Request.RemoteEndPoint.ToString().Split(':')[0],(string)this.Request.Headers["Referer"],
                      (key) =>
                      {
                          return this.Request.Headers[key];
                      }

                    ).Handle();

                    return;
                }
                else
                {
                   
                    string url = (this.Request.Headers["GET"] == null ? this.Request.Headers["POST"] : this.Request.Headers["GET"]).ToSafeString();
                   
                     if (url.Contains("?"))
                    {
                        MatchCollection matches = Regex.Matches(url, @"(?<n>(\w)+)\=(?<v>([^\=\&])+)");
                        foreach( Match m in matches )
                        {
                            Request.Query[m.Groups["n"].Value] = WebUtility.UrlDecode(m.Groups["v"].Value);
                        }
                        url = url.Substring(0, url.IndexOf("?"));
                    }
                    string ext = Path.GetExtension(url).ToLower();
                    if (NotAllowDownloadFiles.Contains(ext))
                    {
                        //不能访问dll exe等文件
                        throw new Exception("not allow");
                    }
                    if (Path.GetExtension(url).IsNullOrEmpty())//访问的路径如果没有扩展名，默认指向.html文件
                    {
                        url = WebPathManger.getFileUrl($"{url}.html");
                    }
                    else
                    {
                        url = WebPathManger.getFileUrl(url);
                    }

                    try
                    {
                        Request.urlRequestHandler();

                    }
                    catch (Exception ex)
                    {

                    }

                    url = getUrl(url);
                    checkHandlers(url);
                    
                    if (Response.mClient != null)
                    {
                        string filepath = url;
                        if (filepath.StartsWith("/"))
                            filepath = filepath.Substring(1);
                        filepath = ScriptRemotingServer.Root + filepath;
                        outputFile(url, filepath);
                    }
                }
            }
            catch(Exception ex)
            {
            }


            this.Response.End();
        }
        void checkHandlers(string visitingUrl)
        {

            if (ScriptRemotingServer.Handlers.Count > 0 && _currentHttpConnectInformation == null)
            {
                _currentHttpConnectInformation = new HttpConnectInformation(Request , Response);
            }

            foreach (var handler in ScriptRemotingServer.Handlers)
            {
                try
                {
                    bool handled = false;
                    handler.Handle(visitingUrl, _currentHttpConnectInformation, ref handled);
                    if (handled)
                    {
                        _currentHttpConnectInformation.Response.End();
                        return;
                    }
                }
                catch
                {
                    _currentHttpConnectInformation.Response.End();
                }
            }
           
        }
        string getUrl(string visitingUrl)
        {
            
            if (ScriptRemotingServer.Routers.Count > 0 && _currentHttpConnectInformation == null)
            {
                _currentHttpConnectInformation = new HttpConnectInformation(Request , Response);
            }

            foreach (var router in ScriptRemotingServer.Routers)
            {
                var url = router.GetUrl(visitingUrl, (string)this.Request.Headers["Referer"], _currentHttpConnectInformation);
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
                this.Response.SendFileNotFound();
                return;
            }
            var since = this.Request.Headers["If-Modified-Since"].ToSafeString();
            var lastWriteTime = new System.IO.FileInfo(filePath).LastWriteTime.ToString("R");
            if (lastWriteTime == since)
            {
                this.Response.SendFileNoChanged();
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
                        this.Response.MakeResponseHeaders(bs.Length, false, -1, 0, lastModifyTime, null, true);
                        this.Response.Write(bs);
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

            Response.MakeResponseHeaders(fs.Length, false, -1, 0, lastModifyTime, null, true);
            
            bs = new byte[4096];
            while (true)
            {
                int count = fs.Read(bs, 0, bs.Length);
                if (count == 0)
                    break;
                Response.Write(bs ,0 , count  );
            }
            fs.Dispose();
                

        }
        

      

    }
}
