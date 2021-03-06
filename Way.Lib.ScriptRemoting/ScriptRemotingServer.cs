﻿//using System;
//using System.Collections.Generic;
//using System.Diagnostics;
//using System.IO;
//using System.IO.Compression;
//using System.Linq;
//using System.Net;
//using System.Net.Sockets;
//using System.Text;
//using System.Threading;
//using System.Threading.Tasks;
//using System.Reflection;
//using System.Security.Cryptography.X509Certificates;


//namespace Way.Lib.ScriptRemoting
//{
//    /// <summary>
//    /// 
//    /// </summary>
//    public static class ScriptRemotingServer
//    {
//        internal static bool SupportHTTP;
//        internal static X509Certificate2 SSLKey;
//        internal static List<IUrlRouter> Routers = new List<IUrlRouter>();
//        internal static List<ICustomHttpHandler> Handlers = new List<ICustomHttpHandler>();
//        internal static string ScriptFilePath;
//        internal static string HtmlTempPath;
//        static SocketServer socketServer;
//        internal static string Root;

//        static void makeTscFiles(string root)
//        {



//            try
//            {


//                ScriptFilePath = Way.Lib.PlatformHelper.GetAppDirectory() + "WayScriptRemoting.js";
//                FileStream fs = File.Create(ScriptFilePath);
//                var assembly = typeof(ScriptRemotingServer).GetTypeInfo().Assembly;
//                var allNames = assembly.GetManifestResourceNames();
//                foreach (var filename in allNames)
//                {
//                    if (filename.EndsWith(".js") && filename.Contains(".Scripts."))
//                    {
//                        var stream = assembly.GetManifestResourceStream(filename);
//                        byte[] bs = new byte[stream.Length];
//                        stream.Read(bs, 0, bs.Length);
//                        stream.Dispose();
//                        fs.Write(bs, 0, bs.Length);
//                        fs.Write(new byte[] { (byte)10, (byte)13 }, 0, 2);
//                    }
//                }
//                fs.Dispose();
//                //return;

//                ////合并入jquery.js
//                //string[] jsFiles = new string[] { "BigInt.js", "RSA.js", "Barrett.js","jquery.js", "sonic.js" };
//                //FileStream fs = File.Create(ScriptFilePath + ".tmp");
//                //foreach (string jsfile in jsFiles)
//                //{
//                //    stream = typeof(ScriptRemotingServer).GetTypeInfo().Assembly.GetManifestResourceStream($"Way.Lib.ScriptRemoting.{jsfile}");
//                //    fileData = new byte[stream.Length];
//                //    stream.Read(fileData, 0, fileData.Length);
//                //    stream.Dispose();
//                //    fs.Write(fileData, 0, fileData.Length);
//                //}

//                //fileData = File.ReadAllBytes(ScriptFilePath);
//                //fs.Write(fileData, 0, fileData.Length);
//                //fs.Dispose();

//                //File.Delete(ScriptFilePath);
//                //File.Move(ScriptFilePath + ".tmp", ScriptFilePath);
//            }
//            catch
//            {
//                throw;
//                //throw new Exception(string.Format("解压typescript编译工具时发生错误，" + ex.Message));
//            }
//        }

//        ///// <summary>
//        ///// 编译多个ts为一个js文件
//        ///// </summary>
//        ///// <param name="tsFolder">ts所在文件夹，如：/scripts</param>
//        ///// <param name="jsFilePath">生成的js文件路径，如：/scripts/test.js</param>
//        //public static void CompileTsToJs(string tsFolder , string jsFilePath)
//        //{
//        //    if (Root == null)
//        //        throw new Exception("请先执行Start或者StartForAsp");
//        //    if (tsFolder.StartsWith("/"))
//        //        tsFolder = tsFolder.Substring(1);
//        //    if (jsFilePath.StartsWith("/"))
//        //        jsFilePath = jsFilePath.Substring(1);
//        //    tsFolder = Root + tsFolder.Replace("/", "\\");
//        //    jsFilePath = Root + jsFilePath.Replace("/", "\\");
//        //    var fs = File.Create(jsFilePath);
//        //    CompileTsToStream(tsFolder, fs);
//        //    fs.Dispose();
//        //}
//        //static void CompileTsToStream(string tsFolder, Stream stream)
//        //{
//        //    string[] files = Directory.GetFiles(tsFolder, "*.ts");
//        //    foreach (string filepath in files)
//        //    {
//        //        var typescriptCompiler = new ProcessStartInfo
//        //        {
//        //            WindowStyle = ProcessWindowStyle.Hidden,
//        //            FileName = Root + "_WayScriptRemoting\\tsc\\tsc.exe",
//        //            Arguments = string.Format("\"{0}\"", filepath)
//        //        };
//        //        string jspath = filepath.Substring(0, filepath.LastIndexOf(".") + 1) + "js";
//        //        var process = Process.Start(typescriptCompiler);
//        //        process.WaitForExit();

//        //        byte[] bs = File.ReadAllBytes(jspath);
//        //        stream.Write(bs,0,bs.Length);
//        //    }
//        //    string[] dirs = Directory.GetDirectories(tsFolder);
//        //    foreach (string folder in dirs)
//        //    {
//        //        CompileTsToStream(folder,stream);
//        //    }
//        //}

//        public static void Stop()
//        {
//            if (socketServer != null)
//            {
//                socketServer.Stop();
//                socketServer = null;
//            }
//        }

//        /// <summary>
//        /// 
//        /// </summary>
//        /// <param name="ssl"></param>
//        /// <param name="supportHttp">是否同时支持http协议，默认false</param>
//        public static void UseHttps(X509Certificate2 ssl, bool supportHttp = false)
//        {
//            SupportHTTP = supportHttp;
//            SSLKey = ssl;
//        }

//        /// <summary>
//        /// 启动ScriptRemotingServer，建议在Application_Start执行
//        /// </summary>
//        /// <param name="port">端口号</param>
//        /// <param name="webRootPath">网站文件所在文件夹</param>
//        /// <param name="gcCollectInterval">内存回收间隔（单位：小时），0表示不回收</param>
//        public static void Start(int port, string webRootPath, int gcCollectInterval)
//        {
//            Start(new int[] { port }, webRootPath, gcCollectInterval);
//        }

//        /// <summary>
//        /// 启动ScriptRemotingServer，建议在Application_Start执行
//        /// </summary>
//        /// <param name="ports">需要监听的端口号</param>
//        /// <param name="webRootPath">网站文件所在文件夹</param>
//        /// <param name="gcCollectInterval">内存回收间隔（单位：小时），0表示不回收</param>
//        public static void Start(IEnumerable<int> ports, string webRootPath, int gcCollectInterval)
//        {
//            webRootPath = webRootPath.Replace("\\", "/");
//            if (socketServer == null)
//            {
//                Root = webRootPath.EndsWith("/") ? webRootPath : webRootPath + "/";
//                makeTscFiles(Root);

//                HtmlTempPath = PlatformHelper.GetAppDirectory() + "$_HtmlTemps";
//                try
//                {
//                    if (Directory.Exists(HtmlTempPath))
//                    {
//                        Directory.Delete(HtmlTempPath, true);
//                        Directory.CreateDirectory(HtmlTempPath);
//                    }
//                    else
//                    {
//                        Directory.CreateDirectory(HtmlTempPath);
//                    }
//                }
//                catch { }


//                socketServer = new ScriptRemoting.SocketServer(ports);
//                socketServer.Start();

//                if (gcCollectInterval > 0)
//                {
//                    new Thread(gcCollect) { IsBackground = true }.Start(gcCollectInterval);
//                }
//            }
//        }

//        static void gcCollect(object interval)
//        {
//            int millsec = 60 * 60 * 1000 * (int)interval;
//            while (true)
//            {
//                Thread.Sleep(millsec);
//                try
//                {
//                    GC.Collect(GC.MaxGeneration, GCCollectionMode.Forced, true);
//                    Debug.WriteLine("内存回收GC.Collect");
//                }
//                catch { }
//            }
//        }



//#if NET46
//        /// <summary>
//        /// 在Asp.Net中启用ScriptRemoting
//        /// </summary>
//        /// <param name="gcCollectInterval">内存回收间隔（单位：小时），0表示不回收</param>
//        //public static void StartForAsp(int gcCollectInterval)
//        //{
//        //    if (gcCollectInterval > 0)
//        //    {
//        //        new Thread(gcCollect) { IsBackground = true }.Start(gcCollectInterval);
//        //    }
//        //    //using (ServerManager manager = new ServerManager())
//        //    //{

//        //    //    Configuration webConfig = manager.GetApplicationHostConfiguration();

//        //    //    ConfigurationSection handlersSection =
//        //    //        webConfig.GetSection("system.webServer/handlers", "Default Web Site/MyApp");

//        //    //    ConfigurationElementCollection handlersCollection = handlersSection.GetCollection();

//        //    //    ConfigurationElement handlerElement = handlersCollection.CreateElement();

//        //    //    handlerElement.SetAttributeValue("name", "MyHandler");
//        //    //    handlerElement.SetAttributeValue("path", "*.ext");
//        //    //    handlerElement.SetAttributeValue("verb", "*");
//        //    //    handlerElement.SetAttributeValue("type", "A.B");

//        //    //    handlersCollection.Add(handlerElement);

//        //    //    manager.CommitChanges();
//        //    //}
//        //    Root = System.Web.HttpRuntime.AppDomainAppPath;
//        //    makeTscFiles(Root);
//        //    System.Web.HttpApplication.RegisterModule(typeof(IISWebSocket.IISWebSocketModule));
//        //}
//#else
//        ///// <summary>
//        /////  在MVC Core中启用ScriptRemoting
//        ///// </summary>
//        ///// <param name="app"></param>
//        ///// <param name="gcCollectInterval">内存回收间隔（单位：小时），0表示不回收</param>
//        //public static void StartForMvc(IApplicationBuilder app, int gcCollectInterval)
//        //{
//        //    if (gcCollectInterval > 0)
//        //    {
//        //        new Thread(gcCollect) { IsBackground = true }.Start(gcCollectInterval);
//        //    }
//        //    var route = new IISWebSocket.CoreMvcRoute();

//        //    var environment = (IHostingEnvironment)app.ApplicationServices.GetService(typeof(IHostingEnvironment));
//        //    makeTscFiles(environment.WebRootPath + "/");
//        //    //    string filepath = he.WebRootPath + feature.Path.Replace("/", "\\");

//        //    //var resolver = routes.ApplicationBuilder.ApplicationServices.GetRequiredService<IInlineConstraintResolver>();
//        //    //routes.Routes.Add(new Route(route, "wayscriptremoting_invoke", resolver));
//        //    //routes.Routes.Add(new Route(route, "wayscriptremoting", resolver));
//        //    //routes.Routes.Add(new Route(route, "wayscriptremoting_socket", resolver));
//        //    app.UseWebSockets();

//        //    app.Use(async (http, next) =>
//        //    {
//        //        await route.HandleRequest(http, next);
//        //    });
//        //}
//#endif
//        /// <summary>
//        /// 注册路由器
//        /// </summary>
//        /// <param name="router"></param>
//        public static void RegisterRouter(IUrlRouter router)
//        {
//            Routers.Add(router);
//        }

//        /// <summary>
//        /// 注册HttpHandler
//        /// </summary>
//        /// <param name="router"></param>
//        public static void RegisterHandler(ICustomHttpHandler handler)
//        {
//            Handlers.Add(handler);
//        }
//    }
//    class SocketServer
//    {
//        List<TcpListener> m_listeners = new List<TcpListener>();
//        IEnumerable<int> m_Ports;
//        bool m_stopped = false;

//        public SocketServer(IEnumerable<int> ports)
//        {
//            m_Ports = ports;

//        }
//        public void Start()
//        {
//            m_stopped = false;
//            try
//            {
//                foreach (var port in m_Ports)
//                {
//                    IPEndPoint ipe = new IPEndPoint(IPAddress.Any, port);
//                    var listener = new TcpListener(ipe);                    
//                    listener.Start();
//                    m_listeners.Add(listener);
//                }
//                foreach (var listener in m_listeners)
//                {
//                    new Thread(_start) { IsBackground = true }.Start(listener);
//                }
//            }
//            catch
//            {
//                foreach (var listener in m_listeners)
//                {
//                    listener.Stop();
//                }
//                throw;
//            }
//        }

//        public void Stop()
//        {
//            m_stopped = true;
//            foreach (var listener in m_listeners)
//            {
//                listener.Stop();
//            }
//            m_listeners.Clear();
//        }

//        internal static void HandleSocket(NetStream client)
//        {
//            RemotingContext.Current = null;
//            try
//            {
//                var Request = new Net.Request(client);
//                client.ReadTimeout = 0;

//                ISocketHandler handler = null;
//                if (Request.Headers.ContainsKey("Connection") == false || Request.Headers["Connection"].Contains("Upgrade") == false)
//                {
//                    handler = new HttpSocketHandler(Request);
//                }
//                else
//                {
//                    handler = new WebSocketHandler(Request);
//                }

//                handler.Handle();
//            }
//            catch(Exception)
//            {
//                client.Close();
//            }
//            finally
//            {
//                RemotingContext.Current = null;
//            }
//        }

//        void _start(object obj)
//        {
//            TcpListener listener = (TcpListener)obj;
//            while (!m_stopped)
//            {
//                try
//                {
//                    var socket = listener.AcceptSocket();

//                    new Thread(() =>
//                    {
//                        try
//                        {
//                            if (ScriptRemotingServer.SSLKey == null)
//                                HandleSocket(new NetStream(socket));
//                            else
//                            {
//                                if (ScriptRemotingServer.SupportHTTP)
//                                {
//                                    //同时支持http
//                                    if (socket.Available < 4)
//                                    {
//                                        var startTime = DateTime.Now;
//                                        Thread.Sleep(100);
//                                        while (socket.Available < 4)
//                                        {
//                                            if ((DateTime.Now - startTime).TotalSeconds > 5)
//                                            {
//                                                //超时
//                                                socket.Dispose();
//                                                return;
//                                            }
//                                            Thread.Sleep(1000);

//                                        }
//                                    }
//                                    byte[] bs = new byte[4];
//                                    int readed = socket.Receive(bs, SocketFlags.Peek);
//                                    var text = System.Text.Encoding.UTF8.GetString(bs).ToLower();
//                                    if (text.StartsWith("get") || text.StartsWith("post"))
//                                    {
//                                        //http
//                                        HandleSocket(new NetStream(socket));
//                                    }
//                                    else
//                                    {
//                                        HandleSocket(new NetStream(socket, ScriptRemotingServer.SSLKey));
//                                    }

//                                }
//                                else
//                                {
//                                    HandleSocket(new NetStream(socket, ScriptRemotingServer.SSLKey));

//                                }
//                            }
//                        }
//                        catch {
//                            try
//                            {
//                                socket.Dispose();
//                            }
//                            catch { }
//                        }
//                        finally
//                        {
//                            RemotingContext.Current = null;
//                        }
//                    }).Start();

//                }
//                catch
//                {
//                    Thread.Sleep(1000);
//                }
//            }
//        }

//    }
//}
