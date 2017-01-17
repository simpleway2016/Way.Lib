#if NET46

#else
using Microsoft.AspNetCore.Routing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using System.Text;
using System.Net.WebSockets;
using System.Threading;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Hosting;
using System.Text.RegularExpressions;

namespace Way.Lib.ScriptRemoting.IISWebSocket
{
    class CoreMvcRoute : Microsoft.AspNetCore.Routing.IRouter
    {

        public VirtualPathData GetVirtualPath(VirtualPathContext context)
        {
            return null;
        }

        private async Task ProcessChat(System.Net.WebSockets.WebSocket socket, string clientip,string referer)
        {
            RemotingClientHandler rs = new ScriptRemoting.RemotingClientHandler((string data) =>
            {
                var buffer = new ArraySegment<byte>(Encoding.UTF8.GetBytes(data));
                socket.SendAsync(buffer, WebSocketMessageType.Text, true, CancellationToken.None).Wait();
            }, () =>
            {
                socket.CloseAsync(WebSocketCloseStatus.NormalClosure, null, CancellationToken.None).Wait();
            }, clientip,referer);
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

        public Task HandleRequest(HttpContext context , Func<Task> next)
        {
            string clientip = context.Connection.RemoteIpAddress.ToString().Split(':')[0];
            if (context.WebSockets.IsWebSocketRequest)
            {
                var task = context.WebSockets.AcceptWebSocketAsync();
                task.Wait();
                var websocket = task.Result;
                return ProcessChat(websocket, clientip,context.Request.Headers["Referer"]);
            }
            else
            {
                if (context.Request.QueryString.ToSafeString().Contains("WayVirtualWebSocket=1"))
                {
                    Dictionary<string, string> forms = new Dictionary<string, string>();
                    foreach (string key in context.Request.Form.Keys)
                    {
                        forms.Add(key, context.Request.Form[key]);
                    }
                    return Task.Run(() =>
                    {
                        ManualResetEvent waitObject = new ManualResetEvent(false);
                        new VirtualWebSocketHandler(forms, (data) =>
                        {
                            int statusCode = context.Response.StatusCode;
                            context.Response.WriteAsync(data).Wait();
                            waitObject.Set();
                        }, () =>
                        {
                            return 1;
                        }, () =>
                        {
                            waitObject.Set();
                            return 0;
                        }, clientip, context.Request.Headers["Referer"]

                      ).Handle();
                        waitObject.WaitOne();
                    });
                }
                else if (context.Request.Path.Value.EndsWith("wayscriptremoting"))
                {
                    var since = context.Request.Headers["If-Modified-Since"];
                    var lastWriteTime = new System.IO.FileInfo(ScriptRemotingServer.ScriptFilePath).LastWriteTime.ToString("R");
                    if (lastWriteTime == since)
                    {
                        //context.Response.StatusDescription = System.Web.HttpWorkerRequest.GetStatusDescription(304);
                        context.Response.StatusCode = 304;
                        return Microsoft.AspNetCore.Routing.Internal.TaskCache.CompletedTask;
                    }
                    else
                    {
                        context.Response.Headers.Add("Last-Modified", lastWriteTime);
                        string content = System.Text.Encoding.UTF8.GetString(System.IO.File.ReadAllBytes(ScriptRemotingServer.ScriptFilePath));
                        return context.Response.WriteAsync(content);
                    }
                }
                else if (context.Request.Path.Value.EndsWith("/SERVERID"))
                {
                    return context.Response.WriteAsync(ScriptRemotingServer.SERVERID);
                }
                else if (context.Request.Path.Value.ToLower().StartsWith("/wayscriptremoting_invoke"))
                {
                    string json = context.Request.Form["m"];
                    string content = null;
                    
                    RemotingClientHandler rs = new ScriptRemoting.RemotingClientHandler((string data) =>
                    {
                        content = data;

                    }, null, clientip, context.Request.Headers["Referer"]);
                    rs.OnReceived(json);

                    return context.Response.WriteAsync(content);
                }
                else
                {

                    //var feature = (Microsoft.AspNetCore.Http.Features.IHttpRequestFeature)context.Features[typeof(Microsoft.AspNetCore.Http.Features.IHttpRequestFeature)];

                    //if (feature != null && feature.Path.ToLower().EndsWith(".html"))
                    //{
                    //    var he = (IHostingEnvironment)context.RequestServices.GetService(typeof(IHostingEnvironment));
                    //    string filepath = he.WebRootPath + feature.Path.Replace("/", "\\");

                    //    RemotingController.CheckHtmlFile(filepath, feature.Path);
                    //}

                    //var ae = context.RequestServices.GetService(typeof(IApplicationEnvironment)) as IApplicationEnvironment;

                    if (next == null)
                        return Microsoft.AspNetCore.Routing.Internal.TaskCache.CompletedTask;
                    return next();
                }
            }
        }

        public Task RouteAsync(RouteContext context)
        {
            return HandleRequest(context.HttpContext, null);
        }
    }

}
#endif