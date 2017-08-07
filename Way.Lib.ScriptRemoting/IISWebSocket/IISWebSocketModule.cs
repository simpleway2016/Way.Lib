#if NET46
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Routing;
using System.Web.WebSockets;

namespace Way.Lib.ScriptRemoting.IISWebSocket
{
    class IISWebSocketModule : IHttpHandler ,IHttpModule
    {
        public void ProcessRequest(HttpContext context)
        {
            if (context.IsWebSocketRequest)
            {
                context.AcceptWebSocketRequest(ProcessChat);
            }
        }

        private async Task ProcessChat(AspNetWebSocketContext context)
        {
            WebSocket socket = context.WebSocket;
            RemotingClientHandler rs = new ScriptRemoting.RemotingClientHandler((string data)=>
            {
                var buffer = new ArraySegment<byte>(Encoding.UTF8.GetBytes(data));
                socket.SendAsync(buffer, WebSocketMessageType.Text, true, CancellationToken.None).Wait();
            } , ()=>
            {
                socket.CloseAsync(WebSocketCloseStatus.NormalClosure, null, CancellationToken.None).Wait();
            },context.UserHostAddress.Split(':')[0],context.UrlReferrer.ToString(),null);
            var bs = new byte[204800];
            while (true)
            {
                if (socket.State == WebSocketState.Open)
                {
                    try
                    {
                        ArraySegment<byte> buffer = new ArraySegment<byte>(bs);
                        WebSocketReceiveResult result = await socket.ReceiveAsync(buffer, CancellationToken.None);
                        if (result.Count == 0)
                        {
                            //客户端要求关闭
                            socket.CloseAsync(WebSocketCloseStatus.NormalClosure, null, CancellationToken.None).Wait();
                            continue;
                        }

                        if (rs.StreamType == RemotingClientHandler.RemotingStreamType.Text)
                        {
                            string userMsg = Encoding.UTF8.GetString(buffer.Array, 0, result.Count);
                            rs.OnReceived(userMsg);
                        }
                        else
                        {
                            
                            byte[] data = new byte[result.Count];
                            Array.Copy(bs, data, result.Count);
                            rs.OnReceived(data);
                        }
                    }
                    catch
                    {
                    }
                }
                else
                {
                    break;
                }
            }
            rs.OnDisconnected();
        }

        public void Init(HttpApplication context)
        {
            context.PostResolveRequestCache += Context_PostResolveRequestCache;
        }

        private void Context_PostResolveRequestCache(object sender, EventArgs e)
        {
            HttpContext context = ((HttpApplication)sender).Context;
            if (context.IsWebSocketRequest)
            {
                //HttpContextBase contextWrapper = new HttpContextWrapper(context);
                //     RouteData routeData = this.RouteCollection.GetRouteData(contextWrapper);
                //RequestContext requestContext = new RequestContext(contextWrapper, routeData);
                //     IHttpHandler handler = routeData.RouteHandler.GetHttpHandler(requestContext);
                context.RemapHandler(this);
            }
            else if (context.Request.RawUrl.ToLower().StartsWith("/wayscriptremoting_invoke?a="))
            {
                string json = context.Request.Form["m"];
                RemotingClientHandler rs = new ScriptRemoting.RemotingClientHandler((string data) =>
                {
                    context.Response.Write(data);
                    
                }, null, context.Request.UserHostAddress.Split(':')[0],context.Request.UrlReferrer.ToString(),
                (key) =>
                      {
                          return context.Request.Headers[key];
                      });
                rs.OnReceived(json);
                context.Response.End();
            }
            else if( context.Request.RawUrl.ToLower().EndsWith("wayscriptremoting") )
            {
                //m_client.Socket.Send(System.Text.Encoding.UTF8.GetBytes("HTTP/1.1 304 " + System.Web.HttpWorkerRequest.GetStatusDescription(304) + "\r\nConnection: Close\r\n\r\n"));
                var since = context.Request.Headers["If-Modified-Since"];
                var lastWriteTime = new System.IO.FileInfo(ScriptRemotingServer.ScriptFilePath).LastWriteTime.ToString("R");
                if (lastWriteTime == since)
                {
                    context.Response.StatusDescription = System.Web.HttpWorkerRequest.GetStatusDescription(304);
                    context.Response.StatusCode = 304;
                }
                else
                {
                    context.Response.AddHeader("Last-Modified", lastWriteTime);
                    context.Response.WriteFile(ScriptRemotingServer.ScriptFilePath);
                }
                context.Response.End();
            }
        }

        public void Dispose()
        {
             
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }

    }
}
#endif