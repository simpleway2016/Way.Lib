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
        static HttpSocketHandler()
        {
            ContentTypeDefines = new Dictionary<string, string>();
            StreamReader sr = null;
            if (File.Exists(AppDomain.CurrentDomain.BaseDirectory + "ContentTypes.txt"))
            {
                sr = new StreamReader(File.OpenRead(AppDomain.CurrentDomain.BaseDirectory + "ContentTypes.txt") , System.Text.Encoding.UTF8);
            }
            else
            {
                var stream = typeof(RemotingController).GetTypeInfo().Assembly.GetManifestResourceStream("Way.Lib.ScriptRemoting.ContentTypes.txt");
                byte[] bs = new byte[stream.Length];
                stream.Read(bs, 0, bs.Length);
                stream.Dispose();
                File.WriteAllBytes(AppDomain.CurrentDomain.BaseDirectory + "ContentTypes.txt", bs);
                sr = new StreamReader(File.OpenRead(AppDomain.CurrentDomain.BaseDirectory + "ContentTypes.txt"), System.Text.Encoding.UTF8);
            }
            
            while(true)
            {
                string line = sr.ReadLine();
                if (line == null)
                    break;
                line = Regex.Replace(line, @"(\t| )+", " ");
                string[] infos = line.Split(' ');
                string key = infos[0].Trim();
                var value = infos[1].Trim();
                if (key.Length == 0 || value.Length == 0)
                    continue;
                ContentTypeDefines[key] = value;
            }
            sr.Dispose();

            ControllerUrlConfig = new Dictionary<string, string>();
               Type basetype = typeof(RemotingController);
            Assembly[] assemblies = PlatformHelper.GetAppAssemblies();
            foreach( Assembly assembly in assemblies )
            {
                var types = assembly.GetTypes();
                foreach( var type in types )
                {
                    if(type.GetTypeInfo().IsSubclassOf(basetype))
                    {
                        var attr = type.GetTypeInfo().GetCustomAttribute(typeof(RemotingUrlAttribute), true) as RemotingUrlAttribute;
                        if(attr != null)
                        {
                            if(ControllerUrlConfig.ContainsKey(attr.Url) == false)
                            {
                                ControllerUrlConfig[attr.Url] = type.FullName;
                            }
                        }
                    }
                }
            }
        }
        HttpConnectInformation _currentHttpConnectInformation;
        static string[] NotAllowDownloadFiles = new string[] { ".dll", ".exe", ".config",".db" };
        static Dictionary<string, string> ContentTypeDefines;

        internal static Dictionary<string, string> ControllerUrlConfig;
        RemotingContext _context;
        public HttpSocketHandler(Net.Request request)
        {
            _context = RemotingContext.Current;
            _context.Request = request;
            _context.Response = new Net.Response(request.mClient , _context);
           
        }
        public void Handle()
        {
           
            try
            {
                if (_context.Request.Headers.ContainsKey("Connection") == false)
                {
                    _context.Response.Headers["Connection"] = "Keep-Alive";
                }
                else
                {
                    _context.Response.Headers["Connection"] = _context.Request.Headers["Connection"];
                }
                //访问ts脚本
                if (_context.Request.Headers["GET"].ToSafeString().EndsWith("/WayScriptRemoting", StringComparison.CurrentCultureIgnoreCase))
                {
                    outputFile(null, HttpServer.ScriptFilePath);
                }
                else if (_context.Request.Headers["POST"].ToSafeString().StartsWith("/wayscriptremoting_invoke?a=", StringComparison.CurrentCultureIgnoreCase))
                {
                    _context.Request.urlRequestHandler();
                    string json = _context.Request.Form["m"];
                    RemotingClientHandler rs = new ScriptRemoting.RemotingClientHandler((string data) =>
                    {
                        _context.Response.WriteStringBody(data);
                    }, null, _context.Request.RemoteEndPoint.ToString().Split(':')[0], (string)_context.Request.Headers["Referer"],
                    (key) =>
                    {
                        return _context.Request.Headers[key];
                    });
                    rs.OnReceived(json);

                }
                else if (_context.Request.Headers["POST"].ToSafeString().StartsWith("/wayscriptremoting_recreatersa?a=" , StringComparison.CurrentCultureIgnoreCase))
                {
                    _context.Request.urlRequestHandler();
                    string json = _context.Request.Form["m"];
                    RemotingClientHandler rs = new ScriptRemoting.RemotingClientHandler((string data) =>
                    {
                        _context.Response.WriteStringBody(data);
                    }, null, _context.Request.RemoteEndPoint.ToString().Split(':')[0], (string)_context.Request.Headers["Referer"],
                    (key) =>
                    {
                        return _context.Request.Headers[key];
                    });
                    rs.handleReCreateRSA(json.FromJson<MessageBag>());

                }
                else if (_context.Request.Headers["POST"].ToSafeString().EndsWith("?WayVirtualWebSocket=1", StringComparison.CurrentCultureIgnoreCase)
                    || _context.Request.Headers["POST"].ToSafeString().EndsWith("&WayVirtualWebSocket=1", StringComparison.CurrentCultureIgnoreCase))
                {
                    _context.Request.urlRequestHandler();
                    new VirtualWebSocketHandler(_context.Request.Form, (data) =>
                   {
                       _context.Response.WriteStringBody(data);
                   }, () =>
                     {
                         _context.Response.End();

                         return 0;
                     }, () =>
                      {
                          _context.Response.CloseSocket();
                          return 0;
                      }, _context.Request.RemoteEndPoint.ToString().Split(':')[0],(string)_context.Request.Headers["Referer"],
                      (key) =>
                      {
                          return _context.Request.Headers[key];
                      }

                    ).Handle();

                    return;
                }
                else
                {
                   
                    string url = (_context.Request.Headers["GET"] == null ? _context.Request.Headers["POST"] : _context.Request.Headers["GET"]).ToSafeString();
                   
                     if (url.Contains("?"))
                    {
                        MatchCollection matches = Regex.Matches(url, @"(?<n>(\w)+)\=(?<v>([^\=\&])+)");
                        foreach( Match m in matches )
                        {
                            _context.Request.Query[m.Groups["n"].Value] = WebUtility.UrlDecode(m.Groups["v"].Value);
                        }
                        url = url.Substring(0, url.IndexOf("?"));
                    }
                   

                    string ext = Path.GetExtension(url).ToLower();
                    if (NotAllowDownloadFiles.Contains(ext))
                    {
                        //不能访问dll exe等文件
                        throw new Exception("not allow");
                    }
                    

                    try
                    {
                        _context.Request.urlRequestHandler();

                    }
                    catch (Exception ex)
                    {

                    }

                    if ((_context.Server.Routers.Count > 0 || _context.Server.Handlers.Count > 0) && _currentHttpConnectInformation == null)
                    {
                        _currentHttpConnectInformation = new HttpConnectInformation(_context.Request, _context.Response);
                        //先设一个默认controller，后面具体controller可以替换
                        _context.Controller = new RemotingController() { Session = _currentHttpConnectInformation.Session };
                    }

                    url = getUrl(url);

                    checkHandlers(url);
                    if (_context.Response.mClient == null)
                        throw new Exception("ended");

                    var controllerConfig = ControllerUrlConfig.FirstOrDefault(m => url.StartsWith(m.Key, StringComparison.CurrentCultureIgnoreCase) && url.Substring(m.Key.Length).Contains("/") == false);
                    if (controllerConfig.Value != null)
                    {
                        RemotingClientHandler rs = new ScriptRemoting.RemotingClientHandler(null, null, _context.Request.RemoteEndPoint.ToString().Split(':')[0], null, null);
                        rs.handleUrlMethod(controllerConfig.Value , url.Substring(controllerConfig.Key.Length));
                    }
                    else
                    {
                        if (url.Length == 0 || url == "/")
                        {
                            url = _context.Server.WebPathManger.GetFileUrl("/index.html");
                        }
                        if (Path.GetExtension(url).IsNullOrEmpty())//访问的路径如果没有扩展名，默认指向.html文件
                        {
                            url = _context.Server.WebPathManger.GetFileUrl($"{url}.html");
                        }
                        else
                        {
                            url = _context.Server.WebPathManger.GetFileUrl(url);
                        }                       

                        if (_context.Response.mClient != null)
                        {
                            string filepath = url;
                            if (filepath.StartsWith("/"))
                                filepath = filepath.Substring(1);
                            filepath = Path.GetFullPath(_context.Server.Root + filepath);
                            if (filepath.ToLower().Contains(_context.Server.Root.ToLower()) == false)
                            {
                                //访问了Root范围外文件
                                _context.Response.SendFileNotFound();
                            }
                            else
                            {
                                outputFile(url, filepath);
                            }
                        }
                    }
                }
            }
            catch(Exception ex)
            {
            }

            _context.Request.Dispose();
            _context.Response.End();
        }
        void checkHandlers(string visitingUrl)
        {

            foreach (var handler in _context.Server.Handlers)
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
            
            foreach (var router in _context.Server.Routers)
            {
                var url = router.GetUrl(visitingUrl, (string)_context.Request.Headers["Referer"], _currentHttpConnectInformation);
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
                _context.Response.SendFileNotFound();
                return;
            }
            var since = _context.Request.Headers["If-Modified-Since"].ToSafeString();
            var lastWriteTime = new System.IO.FileInfo(filePath).LastWriteTime.ToString("R");
            if (lastWriteTime == since)
            {
                _context.Response.SendFileNoChanged();
            }
            else
            {
                outputFile(url , filePath, lastWriteTime);
            }
        }


         void outputFile(string url , string filePath , string lastModifyTime)
        {
            byte[] bs;
            var fs = new System.IO.FileStream(filePath, System.IO.FileMode.Open, System.IO.FileAccess.Read, FileShare.ReadWrite);
            
            string ext = System.IO.Path.GetExtension(filePath).ToLower();
            if (string.IsNullOrEmpty(_context.Response.Headers["Content-Type"]))
            {
                if (ContentTypeDefines.ContainsKey(ext))
                {
                    _context.Response.Headers["Content-Type"] = ContentTypeDefines[ext];
                }
                else
                {
                    _context.Response.Headers["Content-Type"] = "application/octet-stream";
                }
            }
            int range = -1, rangeEnd = 0;
            string RangeStr = _context.Request.Headers["Range"];
            if (RangeStr != null)
            {
                _context.Response.StatusCode = 206;
                var rangeInfo = RangeStr.Replace("bytes=", "").Split('-');
                range = Convert.ToInt32(rangeInfo[0]);
                if (rangeInfo[1].IsNullOrEmpty())
                {
                    rangeEnd = (int)fs.Length - 1;
                }
                else
                {
                    rangeEnd = Convert.ToInt32(rangeInfo[1]);
                }
            }

            _context.Response.MakeResponseHeaders(fs.Length, false, range, rangeEnd, lastModifyTime, null, true);
            
            bs = new byte[4096];
            if (range >= 0)
            {
                fs.Position = range;
                int totalRead = rangeEnd - range + 1;
                while (totalRead > 0)
                {
                    int toread = Math.Min(bs.Length, totalRead);
                    int count = fs.Read(bs, 0, toread);
                    if (count == 0)
                        break;
                    totalRead -= count;
                    _context.Response.Write(bs, 0, count);
                }
            }
            else
            {
                while (true)
                {
                    int count = fs.Read(bs, 0, bs.Length);
                    if (count == 0)
                        break;
                    _context.Response.Write(bs, 0, count);
                }
            }
            fs.Dispose();
                

        }
        

      

    }
}
