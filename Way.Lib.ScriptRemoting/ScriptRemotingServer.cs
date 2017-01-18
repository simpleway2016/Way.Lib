using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Reflection;
using Microsoft.AspNetCore.Hosting;

#if NET46
#else
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
#endif
namespace Way.Lib.ScriptRemoting
{
    /// <summary>
    /// 
    /// </summary>
    public static class ScriptRemotingServer
    {
        internal static string SERVERID = Guid.NewGuid().ToString();
        internal static List<IUrlRouter> Routers = new List<IUrlRouter>();
        internal static string ScriptFilePath;
        static SocketServer socketServer;
        internal static string Root;
    
        static void makeTscFiles(string root)
        {
            return;

            try
            {
               
                Stream stream;
                byte[] fileData;
                

                ScriptFilePath = root + "_WayScriptRemoting/tsc/WayScriptRemoting.js";

                //合并入jquery.js
                string[] jsFiles = new string[] { "BigInt.js", "RSA.js", "Barrett.js","jquery.js", "sonic.js" };
                FileStream fs = File.Create(ScriptFilePath + ".tmp");
                foreach (string jsfile in jsFiles)
                {
                    stream = typeof(ScriptRemotingServer).GetTypeInfo().Assembly.GetManifestResourceStream($"Way.Lib.ScriptRemoting.{jsfile}");
                    fileData = new byte[stream.Length];
                    stream.Read(fileData, 0, fileData.Length);
                    stream.Dispose();
                    fs.Write(fileData, 0, fileData.Length);
                }
               
                fileData = File.ReadAllBytes(ScriptFilePath);
                fs.Write(fileData, 0, fileData.Length);
                fs.Dispose();

                File.Delete(ScriptFilePath);
                File.Move(ScriptFilePath + ".tmp", ScriptFilePath);
            }
            catch(Exception ex)
            {
                throw new Exception(string.Format("解压typescript编译工具时发生错误，" + ex.Message));
            }
        }

        ///// <summary>
        ///// 编译多个ts为一个js文件
        ///// </summary>
        ///// <param name="tsFolder">ts所在文件夹，如：/scripts</param>
        ///// <param name="jsFilePath">生成的js文件路径，如：/scripts/test.js</param>
        //public static void CompileTsToJs(string tsFolder , string jsFilePath)
        //{
        //    if (Root == null)
        //        throw new Exception("请先执行Start或者StartForAsp");
        //    if (tsFolder.StartsWith("/"))
        //        tsFolder = tsFolder.Substring(1);
        //    if (jsFilePath.StartsWith("/"))
        //        jsFilePath = jsFilePath.Substring(1);
        //    tsFolder = Root + tsFolder.Replace("/", "\\");
        //    jsFilePath = Root + jsFilePath.Replace("/", "\\");
        //    var fs = File.Create(jsFilePath);
        //    CompileTsToStream(tsFolder, fs);
        //    fs.Dispose();
        //}
        //static void CompileTsToStream(string tsFolder, Stream stream)
        //{
        //    string[] files = Directory.GetFiles(tsFolder, "*.ts");
        //    foreach (string filepath in files)
        //    {
        //        var typescriptCompiler = new ProcessStartInfo
        //        {
        //            WindowStyle = ProcessWindowStyle.Hidden,
        //            FileName = Root + "_WayScriptRemoting\\tsc\\tsc.exe",
        //            Arguments = string.Format("\"{0}\"", filepath)
        //        };
        //        string jspath = filepath.Substring(0, filepath.LastIndexOf(".") + 1) + "js";
        //        var process = Process.Start(typescriptCompiler);
        //        process.WaitForExit();

        //        byte[] bs = File.ReadAllBytes(jspath);
        //        stream.Write(bs,0,bs.Length);
        //    }
        //    string[] dirs = Directory.GetDirectories(tsFolder);
        //    foreach (string folder in dirs)
        //    {
        //        CompileTsToStream(folder,stream);
        //    }
        //}
        /// <summary>
        /// 启动ScriptRemotingServer，建议在Application_Start执行
        /// </summary>
        /// <param name="port">端口号</param>
        /// <param name="webRootPath">网站文件所在文件夹</param>
        /// <param name="gcCollectInterval">内存回收间隔（单位：小时），0表示不回收</param>
        public static void Start(int port,string webRootPath,int gcCollectInterval)
        {
            webRootPath = webRootPath.Replace("\\", "/");
            if (socketServer == null)
            {
                Root = webRootPath.EndsWith("/") ? webRootPath : webRootPath + "/";
                makeTscFiles(Root);

                socketServer = new ScriptRemoting.SocketServer(port);
                socketServer.Start();

                if(gcCollectInterval > 0)
                {
                    new Thread(gcCollect) {IsBackground = true }.Start(gcCollectInterval);
                }
            }
        }

        static void gcCollect(object interval)
        {
            int millsec = 60*60*1000*(int)interval;
            while(true)
            {
                Thread.Sleep(millsec);
                try
                {
                    GC.Collect(GC.MaxGeneration, GCCollectionMode.Forced, true);
                    Debug.WriteLine("内存回收GC.Collect");
                }
                catch { }
            }
        }



#if NET46
        /// <summary>
        /// 在Asp.Net中启用ScriptRemoting
        /// </summary>
        /// <param name="gcCollectInterval">内存回收间隔（单位：小时），0表示不回收</param>
        public static void StartForAsp(int gcCollectInterval)
        {
            if (gcCollectInterval > 0)
            {
                new Thread(gcCollect) { IsBackground = true }.Start(gcCollectInterval);
            }
            //using (ServerManager manager = new ServerManager())
            //{

            //    Configuration webConfig = manager.GetApplicationHostConfiguration();

            //    ConfigurationSection handlersSection =
            //        webConfig.GetSection("system.webServer/handlers", "Default Web Site/MyApp");

            //    ConfigurationElementCollection handlersCollection = handlersSection.GetCollection();

            //    ConfigurationElement handlerElement = handlersCollection.CreateElement();

            //    handlerElement.SetAttributeValue("name", "MyHandler");
            //    handlerElement.SetAttributeValue("path", "*.ext");
            //    handlerElement.SetAttributeValue("verb", "*");
            //    handlerElement.SetAttributeValue("type", "A.B");

            //    handlersCollection.Add(handlerElement);

            //    manager.CommitChanges();
            //}
            Root = System.Web.HttpRuntime.AppDomainAppPath;
            makeTscFiles(Root);
            System.Web.HttpApplication.RegisterModule(typeof(IISWebSocket.IISWebSocketModule));
        }
#else
        /// <summary>
        ///  在MVC Core中启用ScriptRemoting
        /// </summary>
        /// <param name="app"></param>
        /// <param name="gcCollectInterval">内存回收间隔（单位：小时），0表示不回收</param>
        public static void StartForMvc(IApplicationBuilder app, int gcCollectInterval)
        {
            if (gcCollectInterval > 0)
            {
                new Thread(gcCollect) { IsBackground = true }.Start(gcCollectInterval);
            }
            var route = new IISWebSocket.CoreMvcRoute();

            var environment = (IHostingEnvironment)app.ApplicationServices.GetService(typeof(IHostingEnvironment));
            makeTscFiles(environment.WebRootPath + "/");
            //    string filepath = he.WebRootPath + feature.Path.Replace("/", "\\");

            //var resolver = routes.ApplicationBuilder.ApplicationServices.GetRequiredService<IInlineConstraintResolver>();
            //routes.Routes.Add(new Route(route, "wayscriptremoting_invoke", resolver));
            //routes.Routes.Add(new Route(route, "wayscriptremoting", resolver));
            //routes.Routes.Add(new Route(route, "wayscriptremoting_socket", resolver));
            app.UseWebSockets();

            app.Use(async (http, next) =>
            {
                await route.HandleRequest(http, next);
            });
        }
#endif
        /// <summary>
        /// 注册路由器
        /// </summary>
        /// <param name="router"></param>
        public static void RegisterRouter(IUrlRouter router)
        {
            Routers.Add(router);
        }
    }
    class SocketServer
    {
        TcpListener m_listener;
        int m_Port;
        bool m_stopped = false;

        public SocketServer(int port)
        {
            m_Port = port;
            
        }

        public void Start()
        {
            m_stopped = false;
            IPEndPoint ipe = new IPEndPoint(IPAddress.Any, m_Port);
            m_listener = new TcpListener(ipe);
            m_listener.Start();

            new Thread(_start) { IsBackground=true}.Start();
        }
    
        public void Stop()
        {
            m_stopped = true;
            m_listener.Stop();
            m_listener = null;
        }

        void _start()
        {
            while (!m_stopped)
            {
                try
                {
                    var task = m_listener.AcceptSocketAsync();
                    task.Wait();
                    //Debug.WriteLine("new socket in");
                    var socket = task.Result;

                    Task.Run(() =>
                    {
                        try
                        {
                            Connection session = new Connection(socket);
                            session.OnConnect();
                        }
                        catch
                        {
                        }
                    });
                }
                catch
                {
                    break;
                }
            }
        }

    }
}
