using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;
using System.Net.Sockets;
using System.Net;
using System.Collections.Concurrent;
using System.Security.Authentication;

namespace Way.Lib.ScriptRemoting
{
    public class HttpServer
    {
        internal static string ScriptFilePath;

        List<TcpListener> m_listeners = new List<TcpListener>();
        IEnumerable<int> m_Ports;
        bool m_stopped = true;

        internal static string HtmlTempPath;
        internal bool SupportHTTP;
        internal X509Certificate2 SSLKey;
        SslProtocols SslProtocols;
        internal List<IUrlRouter> Routers = new List<IUrlRouter>();
        internal List<ICustomHttpHandler> Handlers = new List<ICustomHttpHandler>();
        internal ConcurrentDictionary<string, SessionState> AllSessions = new ConcurrentDictionary<string, SessionState>();

        private string _Root;
        /// <summary>
        /// web文件夹路径
        /// </summary>
        public string Root
        {
            get => _Root;
            set
            {
                if (_Root != value)
                {
                    _Root = System.IO.Path.GetFullPath(value);
                }
            }
        }


        int _SessionTimeout = 20;
        /// <summary>
        /// session过期时间，单位(分)
        /// </summary>
        public int SessionTimeout
        {
            get
            {
                return _SessionTimeout;
            }
            set
            {
                if (_SessionTimeout != value)
                {
                    _SessionTimeout = value;
                }
            }
        }
        internal WebPathManger WebPathManger;
        static HttpServer()
        {
            HtmlTempPath = AppDomain.CurrentDomain.BaseDirectory + "$_HtmlTemps";
            try
            {
                if (Directory.Exists(HtmlTempPath))
                {
                    Directory.Delete(HtmlTempPath, true);
                    Directory.CreateDirectory(HtmlTempPath);
                }
                else
                {
                    Directory.CreateDirectory(HtmlTempPath);
                }
            }
            catch { }

            makeTscFiles();
        }
        public HttpServer(IEnumerable<int> ports, string webRootPath)
        {
            m_Ports = ports;
            var root = webRootPath.Replace("\\", "/");
            Root = root.EndsWith("/") ? root : root + "/";
            WebPathManger = new WebPathManger(this);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ssl"></param>
        /// <param name="sslProtocols"></param>
        /// <param name="supportHttp">是否同时支持http协议，默认false</param>
        public void UseHttps(X509Certificate2 ssl, SslProtocols sslProtocols, bool supportHttp = false)
        {
            SslProtocols = sslProtocols;
               SupportHTTP = supportHttp;
            SSLKey = ssl;
        }


        /// <summary>
        /// 注册路由器
        /// </summary>
        /// <param name="router"></param>
        public void RegisterRouter(IUrlRouter router)
        {
            Routers.Add(router);
        }

        /// <summary>
        /// 注册HttpHandler
        /// </summary>
        /// <param name="router"></param>
        public void RegisterHandler(ICustomHttpHandler handler)
        {
            Handlers.Add(handler);
        }

        static void makeTscFiles()
        {

            try
            {
                ScriptFilePath = AppDomain.CurrentDomain.BaseDirectory + "WayScriptRemoting.js";
                FileStream fs = File.Create(ScriptFilePath);
                var assembly = typeof(HttpServer).GetTypeInfo().Assembly;
                var allNames = assembly.GetManifestResourceNames();
                foreach (var filename in allNames)
                {
                    if (filename.EndsWith(".js") && filename.Contains(".Scripts."))
                    {
                        var stream = assembly.GetManifestResourceStream(filename);
                        byte[] bs = new byte[stream.Length];
                        stream.Read(bs, 0, bs.Length);
                        stream.Dispose();
                        fs.Write(bs, 0, bs.Length);
                        fs.Write(new byte[] { (byte)10, (byte)13 }, 0, 2);
                    }
                }
                fs.Dispose();
            }
            catch
            {
                throw;
            }
        }


        public void Start()
        {
            if (m_stopped)
            {
                m_stopped = false;
                try
                {
                    foreach (var port in m_Ports)
                    {
                        IPEndPoint ipe = new IPEndPoint(IPAddress.Any, port);
                        var listener = new TcpListener(ipe);
                        listener.Start();
                        m_listeners.Add(listener);
                    }
                    foreach (var listener in m_listeners)
                    {
                        new Thread(_start) { IsBackground = true }.Start(listener);
                    }
                    new Thread(checkSessionTimeout).Start();
                }
                catch
                {
                    foreach (var listener in m_listeners)
                    {
                        listener.Stop();
                    }
                    throw;
                }
            }
           
        }

        public void Stop()
        {
            m_stopped = true;
            foreach (var listener in m_listeners)
            {
                listener.Stop();
            }
            m_listeners.Clear();
        }

        void checkSessionTimeout()
        {

            while (!m_stopped)
            {
                if (SessionTimeout <= 0)
                    SessionTimeout = 1;
                try
                {
                    if (SessionTimeout != int.MaxValue)
                    {
                        foreach (var kv in AllSessions)
                        {
                            if ((DateTime.Now - kv.Value.LastUseTime).TotalMinutes > SessionTimeout)
                            {
                                //在lock中重新判断
                                if ((DateTime.Now - kv.Value.LastUseTime).TotalMinutes > SessionTimeout)
                                {
                                    SessionState obj;
                                    AllSessions.TryRemove(kv.Key, out obj);
                                    try
                                    {
                                        foreach (var keypair in kv.Value)
                                        {
                                            try
                                            {
                                                if (keypair.Value is IDisposable)
                                                {
                                                    ((IDisposable)keypair.Value).Dispose();
                                                }
                                            }
                                            catch { }
                                        }
                                    }
                                    catch
                                    {

                                    }

                                    try
                                    {
                                        SessionState.OnSessionRemoved(kv.Value);
                                    }
                                    catch
                                    {

                                    }

                                    kv.Value.Clear();
                                }

                                break;
                            }
                        }
                    }
                }
                catch
                {

                }
                Thread.Sleep(60000);
            }
        }

        internal void HandleSocket(NetStream client)
        {
            RemotingContext.Current = null;
            try
            {
                var context = RemotingContext.Current;
                context.Server = this;

                var Request = new Net.Request(client, context);
                client.ReadTimeout = 0;                

                ISocketHandler handler = null;
                if (Request.Headers.ContainsKey("Connection") == false || Request.Headers["Connection"].ToLower().Contains("upgrade") == false)
                {
                    handler = new HttpSocketHandler(Request);
                }
                else
                {
                    handler = new WebSocketHandler(Request);
                }

                handler.Handle();
            }
            catch (Exception)
            {
                client.Close();
            }
            finally
            {
                RemotingContext.Current = null;
            }
        }

        void _start(object obj)
        {
            TcpListener listener = (TcpListener)obj;
            while (!m_stopped)
            {
                try
                {
                    var socket = listener.AcceptSocket();

                    new Thread(() =>
                    {
                        try
                        {
                            if (this.SSLKey == null)
                                HandleSocket(new NetStream(socket));
                            else
                            {
                                if (this.SupportHTTP)
                                {
                                    //同时支持http
                                    if (socket.Available < 4)
                                    {
                                        var startTime = DateTime.Now;
                                        Thread.Sleep(100);
                                        while (socket.Available < 4)
                                        {
                                            if ((DateTime.Now - startTime).TotalSeconds > 5)
                                            {
                                                //超时
                                                socket.Dispose();
                                                return;
                                            }
                                            Thread.Sleep(1000);

                                        }
                                    }
                                    byte[] bs = new byte[4];
                                    int readed = socket.Receive(bs, SocketFlags.Peek);
                                    var text = System.Text.Encoding.UTF8.GetString(bs).ToLower();
                                    if (text.StartsWith("get") || text.StartsWith("post"))
                                    {
                                        //http
                                        HandleSocket(new NetStream(socket));
                                    }
                                    else
                                    {
                                        HandleSocket(new NetStream(socket, this.SSLKey,this.SslProtocols));
                                    }

                                }
                                else
                                {
                                    HandleSocket(new NetStream(socket,  this.SSLKey,SslProtocols));

                                }
                            }
                        }
                        catch
                        {
                            try
                            {
                                socket.Dispose();
                            }
                            catch { }
                        }
                        finally
                        {
                            RemotingContext.Current = null;
                        }
                    }).Start();

                }
                catch
                {
                    Thread.Sleep(1000);
                }
            }
        }

    }
}
