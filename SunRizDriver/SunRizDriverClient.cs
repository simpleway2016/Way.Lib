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

        public void Dispose()
        {
           
        }

        public string GetName()
        {
            var client = new Way.Lib.NetStream(this.Address, this.Port);
            var json = Newtonsoft.Json.JsonConvert.SerializeObject(new Command() {
                Type = "GetName"
            });
            byte[] bs = System.Text.Encoding.UTF8.GetBytes(json);
            client.Write(bs.Length);
            client.Write(bs);

            int len = client.ReadInt();
            bs = client.ReceiveDatas(len);
            client.Dispose();
            return System.Text.Encoding.UTF8.GetString(bs);
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

        public void AddPointToWatch(string deviceAddress , string[] points,Action<string,object> onReceiveValue,Action<string> onError)
        {
            try
            {
                var client = new Way.Lib.NetStream(this.Address, this.Port);
                var json = Newtonsoft.Json.JsonConvert.SerializeObject(new Command()
                {
                    Type = "AddPointToWatch",
                    DeviceAddress = deviceAddress,
                    Points = points
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
            }
            catch(Exception ex)
            {
                onError(ex.Message);
            }
        }
    }
}
