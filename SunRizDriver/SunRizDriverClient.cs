using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SunRizDriver
{
    public class SunRizDriverClient : IDisposable
    {
        public string Address
        {
            get;
            private set;
        }
        public int Port
        {
            get;
            private set;
        }
        public SunRizDriverClient(string address , int port)
        {
            this.Address = address;
            this.Port = port;
           
        }

        /// <summary>
        /// 检查是否可以和driver正常连接
        /// </summary>
        /// <returns></returns>
        public bool CheckDriver()
        {
            try
            {
                new Way.Lib.NetStream(this.Address, this.Port).Dispose();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public void Dispose()
        {
           
        }

        public ServerInfo GetServerInfo()
        {
            var client = new Way.Lib.NetStream(this.Address, this.Port);
            var json = Newtonsoft.Json.JsonConvert.SerializeObject(new Command() {
                Type = "GetServerInfo"
            });
            byte[] bs = System.Text.Encoding.UTF8.GetBytes(json);
            client.Write(bs.Length);
            client.Write(bs);

            int len = client.ReadInt();
            bs = client.ReceiveDatas(len);
            client.Dispose();
            return Newtonsoft.Json.JsonConvert.DeserializeObject<ServerInfo>( System.Text.Encoding.UTF8.GetString(bs));
        }
        public string[] EnumDevice(string opcServerAddress)
        {
            var client = new Way.Lib.NetStream(this.Address, this.Port);
            var json = Newtonsoft.Json.JsonConvert.SerializeObject(new Command()
            {
                Type = "EnumDevice",
                DeviceAddress = opcServerAddress,
            });
            byte[] bs = System.Text.Encoding.UTF8.GetBytes(json);
            client.Write(bs.Length);
            client.Write(bs);

            int len = client.ReadInt();
            bs = client.ReceiveDatas(len);
            client.Dispose();
            return Newtonsoft.Json.JsonConvert.DeserializeObject<string[]>(System.Text.Encoding.UTF8.GetString(bs));
        }
        public void EnumDevicePoint(string opcServerAddress ,List<string> parentPath, Action<PointInfomation> onFindPoint)
        {
            var client = new Way.Lib.NetStream(this.Address, this.Port);
            var json = Newtonsoft.Json.JsonConvert.SerializeObject(new Command()
            {
                Type = "EnumDevicePoint",
                DeviceAddress = opcServerAddress,
                ParentPath = parentPath,
            });
            byte[] bs = System.Text.Encoding.UTF8.GetBytes(json);
            client.Write(bs.Length);
            client.Write(bs);

            while(true)
            {
                var len = client.ReadInt();
                if (len < 0)
                    break;
                bs = client.ReceiveDatas(len);
                string point = System.Text.Encoding.UTF8.GetString(bs);
                onFindPoint(Newtonsoft.Json.JsonConvert.DeserializeObject<PointInfomation>(point));
            }
            client.Dispose();
        }
        public string[] GetPointProperties()
        {
            var client = new Way.Lib.NetStream(this.Address, this.Port);
            var json = Newtonsoft.Json.JsonConvert.SerializeObject(new Command()
            {
                Type = "GetPointProperties"
            });
            byte[] bs = System.Text.Encoding.UTF8.GetBytes(json);
            client.Write(bs.Length);
            client.Write(bs);

            int len = client.ReadInt();
            bs = client.ReceiveDatas(len);
            client.Dispose();
            return Newtonsoft.Json.JsonConvert.DeserializeObject<string[]>( System.Text.Encoding.UTF8.GetString(bs));
        }
        /// <summary>
        /// 获取点路径
        /// </summary>
        /// <param name="proObj">属性值</param>
        /// <returns></returns>
        public string GetPointAddress(Dictionary<string,string> proValues)
        {
            var client = new Way.Lib.NetStream(this.Address, this.Port);
            var json = Newtonsoft.Json.JsonConvert.SerializeObject(new Command()
            {
                Type = "GetPointAddress",
                Values = new object[] {proValues }
            });
            byte[] bs = System.Text.Encoding.UTF8.GetBytes(json);
            client.Write(bs.Length);
            client.Write(bs);

            int len = client.ReadInt();
            bs = client.ReceiveDatas(len);
            client.Dispose();
            return System.Text.Encoding.UTF8.GetString(bs);
        }


        public string[] GetDeviceProperties()
        {
            var client = new Way.Lib.NetStream(this.Address, this.Port);
            var json = Newtonsoft.Json.JsonConvert.SerializeObject(new Command()
            {
                Type = "GetDeviceProperties"
            });
            byte[] bs = System.Text.Encoding.UTF8.GetBytes(json);
            client.Write(bs.Length);
            client.Write(bs);

            int len = client.ReadInt();
            bs = client.ReceiveDatas(len);
            client.Dispose();
            return Newtonsoft.Json.JsonConvert.DeserializeObject<string[]>(System.Text.Encoding.UTF8.GetString(bs));
        }
        /// <summary>
        /// 获取设备路径
        /// </summary>
        /// <param name="proObj">属性值</param>
        /// <returns></returns>
        public string GetDeviceAddress(Dictionary<string, string> proValues)
        {
            var client = new Way.Lib.NetStream(this.Address, this.Port);
            var json = Newtonsoft.Json.JsonConvert.SerializeObject(new Command()
            {
                Type = "GetDeviceAddress",
                Values = new object[] { proValues }
            });
            byte[] bs = System.Text.Encoding.UTF8.GetBytes(json);
            client.Write(bs.Length);
            client.Write(bs);

            int len = client.ReadInt();
            bs = client.ReceiveDatas(len);
            client.Dispose();
            return System.Text.Encoding.UTF8.GetString(bs);
        }


        public bool CheckDeviceExist(string deviceAddress)
        {
            var client = new Way.Lib.NetStream(this.Address, this.Port);
            var json = Newtonsoft.Json.JsonConvert.SerializeObject(new Command()
            {
                Type = "CheckDeviceExist",
                DeviceAddress = deviceAddress
            });
            byte[] bs = System.Text.Encoding.UTF8.GetBytes(json);
            client.Write(bs.Length);
            client.Write(bs);

            bool result = client.ReadBoolean();
           
            client.Dispose();
            return result;
        }
        public bool WriteValue(string deviceAddress, string point, object value)
        {
            return WriteValue(deviceAddress, new string[] { point }, new object[] { value })[0];
        }
        public bool[] WriteValue(string deviceAddress, string[] points,object[] values )
        {
            var result = new bool[points.Length];

            var client = new Way.Lib.NetStream(this.Address, this.Port);
            var json = Newtonsoft.Json.JsonConvert.SerializeObject(new Command()
            {
                Type = "WriteValue",
                DeviceAddress = deviceAddress,
                Points = points,
                Values=values
            });
            byte[] bs = System.Text.Encoding.UTF8.GetBytes(json);
            client.Write(bs.Length);
            client.Write(bs);

            for(int i = 0; i < result.Length; i ++)
            {
                result[i] = client.ReadBoolean();
            }
            client.Dispose();
            return result;
        }
        public object[] ReadValue(string deviceAddress, string[] points)
        {
            var client = new Way.Lib.NetStream(this.Address, this.Port);
            var json = Newtonsoft.Json.JsonConvert.SerializeObject(new Command()
            {
                Type = "ReadValue",
                DeviceAddress = deviceAddress,
                Points = points
            });
            byte[] bs = System.Text.Encoding.UTF8.GetBytes(json);
            client.Write(bs.Length);
            client.Write(bs);

            object[] values = new object[points.Length];
            for(int i = 0; i < points.Length; i ++)
            {
                int index = client.ReadInt();
                PointValueType valueType = (PointValueType)client.ReadInt();
                if (valueType == PointValueType.Short)
                {
                    values[index] = client.ReadShort();
                }
                else if (valueType == PointValueType.Int)
                {
                    values[index] = client.ReadInt();
                }
                else if (valueType == PointValueType.Float)
                {
                    values[index] = client.ReadFloat();
                }
                else if (valueType == PointValueType.String)
                {
                    values[index] = System.Text.Encoding.UTF8.GetString(client.ReceiveDatas(client.ReadInt()));
                }
                else
                    throw new Exception($"not support value type {valueType}");
            }
            client.Dispose();
            return values;
        }
        public Way.Lib.NetStream AddPointToWatch(string deviceAddress, string[] points,Action<string, object> onReceiveValue, Action<string> onError)
        {
            return AddPointToWatch(deviceAddress, points, 1000, onReceiveValue, onError);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="deviceAddress">设备地址</param>
        /// <param name="points">点路径</param>
        /// <param name="interval">更新时间间隔，单位：毫秒</param>
        /// <param name="onReceiveValue"></param>
        /// <param name="onError"></param>
        public Way.Lib.NetStream AddPointToWatch(string deviceAddress , string[] points, int interval, Action<string,object> onReceiveValue,Action<string> onError)
        {
            if (interval < 1000)
                interval = 1000;
            try
            {
                var client = new Way.Lib.NetStream(this.Address, this.Port);
                var json = Newtonsoft.Json.JsonConvert.SerializeObject(new Command()
                {
                    Type = "AddPointToWatch",
                    DeviceAddress = deviceAddress,
                    Points = points,
                    Interval = interval
                });
                byte[] bs = System.Text.Encoding.UTF8.GetBytes(json);
                client.Write(bs.Length);
                client.Write(bs);

                Task.Run(() =>
                {
                    while (true)
                    {
                        try
                        {
                            int index = client.ReadInt();
                            PointValueType valueType = (PointValueType)client.ReadInt();
                            if (valueType == PointValueType.Short)
                            {
                                onReceiveValue(points[index], client.ReadShort());
                            }
                            else if (valueType == PointValueType.Int)
                            {
                                onReceiveValue(points[index], client.ReadInt());
                            }
                            else if (valueType == PointValueType.Float)
                            {
                                onReceiveValue(points[index], client.ReadFloat());
                            }
                            else if (valueType == PointValueType.String)
                            {
                                var str = System.Text.Encoding.UTF8.GetString( client.ReceiveDatas(client.ReadInt()));
                                onReceiveValue(points[index], str);
                            }
                            else
                                throw new Exception($"not support value type {valueType}");
                        }
                        catch (Exception ex)
                        {
                            client.Close();
                            onError(ex.Message);
                            return;
                        }
                    }
                });
                return client;
            }
            catch(Exception ex)
            {
                onError(ex.Message);
            }
            return null;
        }
    }
}
