﻿#if NET46

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

namespace Way.Lib.ScriptRemoting.IISWebSocket
{
    class CoreMvcRoute : Microsoft.AspNetCore.Routing.IRouter
    {
       
        public VirtualPathData GetVirtualPath(VirtualPathContext context)
        {
            return null;
        }

        private async Task ProcessChat(System.Net.WebSockets.WebSocket socket,string clientip)
        {
            RemotingClientHandler rs = new ScriptRemoting.RemotingClientHandler((string data) =>
            {
                var buffer = new ArraySegment<byte>(Encoding.UTF8.GetBytes(data));
                socket.SendAsync(buffer, WebSocketMessageType.Text, true, CancellationToken.None).Wait();
            }, () =>
            {
                socket.CloseAsync(WebSocketCloseStatus.NormalClosure, null, CancellationToken.None).Wait();
            }, clientip);
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

        public Task RouteAsync(RouteContext context)
        {
            string clientip = context.HttpContext.Connection.RemoteIpAddress.ToString().Split(':')[0];
            if (context.HttpContext.WebSockets.IsWebSocketRequest)
            {
                var task = context.HttpContext.WebSockets.AcceptWebSocketAsync();
                task.Wait();
                var websocket = task.Result;
                return ProcessChat(websocket, clientip);
            }
            else
            {
                if (context.HttpContext.Request.Path.Value.EndsWith("wayscriptremoting"))
                {
                    var since = context.HttpContext.Request.Headers["If-Modified-Since"];
                    var lastWriteTime = new System.IO.FileInfo(ScriptRemotingServer.ScriptFilePath).LastWriteTime.ToString("R");
                    if (lastWriteTime == since)
                    {
                        //context.HttpContext.Response.StatusDescription = System.Web.HttpWorkerRequest.GetStatusDescription(304);
                        context.HttpContext.Response.StatusCode = 304;
                        return Microsoft.AspNetCore.Routing.Internal.TaskCache.CompletedTask;

                    }
                    else
                    {
                        context.HttpContext.Response.Headers.Add("Last-Modified", lastWriteTime);
                        string content = System.Text.Encoding.UTF8.GetString(System.IO.File.ReadAllBytes(ScriptRemotingServer.ScriptFilePath));
                        return context.HttpContext.Response.WriteAsync(content);
                    }
                }
                else if (context.HttpContext.Request.Path.Value.ToLower().StartsWith("/wayscriptremoting_invoke"))
                {
                    string json = context.HttpContext.Request.Form["m"];
                    string content = null;

                    RemotingClientHandler rs = new ScriptRemoting.RemotingClientHandler((string data) =>
                    {
                        content = data;

                    }, null, clientip);
                    rs.OnReceived(json);

                    return context.HttpContext.Response.WriteAsync(content);
                }
                else
                {
                    return Microsoft.AspNetCore.Routing.Internal.TaskCache.CompletedTask;
                }
            }
        }
    }
}
#endif