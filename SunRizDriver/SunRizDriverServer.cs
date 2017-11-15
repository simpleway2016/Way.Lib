using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SunRizDriver
{
    public class SunRizDriverServer
    {
        public enum ServerStatus
        {
            Stopped = 0,
            Running = 1,
        }

        System.Net.Sockets.TcpListener _tcpListener;
        public ServerStatus Started = ServerStatus.Stopped;
        public virtual string Name
        {
            get
            {
                return "SunRizDriverServer";
            }
        }
        /// <summary>
        /// 是否支持罗列设备
        /// </summary>
        public virtual bool SupportEnumDevice => false;
        /// <summary>
        /// 是否支持罗列设备点
        /// </summary>
        public virtual bool SupportEnumPoints => false;

        public SunRizDriverServer(int port)
        {
            _tcpListener = new System.Net.Sockets.TcpListener( System.Net.IPAddress.Any , port);
        }

        public void Start()
        {
            Started =  ServerStatus.Running;
            _tcpListener.Start();
            new Thread(()=> {
                while(Started == ServerStatus.Running)
                {
                    try
                    {
                        var socket = _tcpListener.AcceptSocket();
                        this.handleSocket(socket);
                    }
                    catch
                    {

                    }
                }
            }).Start();
        }

        protected virtual CommandHandler GetHandler(string type)
        {
            var assembly = typeof(Command).Assembly;
            var handlerType = assembly.GetType($"SunRizDriver.CommandHandlers.{type}Handler");
            return (CommandHandler)Activator.CreateInstance(handlerType, new object[] { this });
        }

        void handleSocket(Socket socket)
        {
           
            var client = new Way.Lib.NetStream(socket);
            new Thread(() => {
                try
                {
                    var len = client.ReadInt();
                    byte[] bs = client.ReceiveDatas(len);
                    Command cmd = Newtonsoft.Json.JsonConvert.DeserializeObject<Command>( System.Text.Encoding.UTF8.GetString(bs) );
                    var handler = GetHandler(cmd.Type);
                    handler.Handle(client,cmd);

                    client.ReceiveDatas(1);
                }
                catch(Exception ex)
                {

                }
                finally
                {
                    client.Dispose();
                }
            }).Start();
        }

        public void Stop()
        {
            Started =  ServerStatus.Stopped;
            _tcpListener.Stop();
        }

        public virtual void AddPointToWatch(Command command,Action<int,PointValueType,object> onValueReceive)
        {
            throw new NotImplementedException();
        }
        public virtual bool[] WriteValue(Command command)
        {
            throw new NotImplementedException();
        }
        public virtual void ReadValue(Command command, Action<int, PointValueType, object> onValueReceive)
        {
            throw new NotImplementedException();
        }
        public virtual bool CheckDeviceExist(Command command)
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// 获取需要多少个属性去描述一个点的地址，返回json格式的字符串数组
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        public virtual string[] GetPointProperties(Command command)
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// 根据点的属性描述，返回点地址
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        public virtual string GetPointAddress(Command command)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 获取需要多少个属性去描述一个设备的地址，返回json格式的字符串数组
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        public virtual string[] GetDeviceProperties(Command command)
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// 根据设备的属性描述，返回设备地址
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        public virtual string GetDeviceAddress(Command command)
        {
            throw new NotImplementedException();
        }
        public virtual string[] EnumDevice(Command command)
        {
            throw new NotImplementedException();
        }
        public virtual void EnumPoints(Command command, Action<string> onFindPoint)
        {
            throw new NotImplementedException();
        }
    }
}
