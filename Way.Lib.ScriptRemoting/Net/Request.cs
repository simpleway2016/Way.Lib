using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Way.Lib.ScriptRemoting.Net
{
    public class Request : System.IO.Stream
    {
        internal NetStream mClient;

        private ValueCollection _Headers = new ValueCollection();
        public ValueCollection Headers
        {
            get
            {
                return _Headers;
            }
        }
        ValueCollection _Form = new ValueCollection();
        public ValueCollection Form
        {
            get
            {
                return _Form;
            }
        }
        ValueCollection _Query = new ValueCollection();
        public ValueCollection Query
        {
            get
            {
                return _Query;
            }
        }
        public System.Net.EndPoint RemoteEndPoint
        {
            get
            {
                return mClient.Socket.RemoteEndPoint;
            }
        }

        public override bool CanRead
        {
            get
            {
                return true;
            }
        }

        public override bool CanSeek
        {
            get
            {
                return false;
            }
        }

        public override bool CanWrite
        {
            get
            {
                return false;
            }
        }

        public override long Length
        {
            get
            {
                return Convert.ToInt64(_Headers["Content-Length"]);
            }
        }

        public override long Position
        {
            get
            {
                throw new NotImplementedException();
            }

            set
            {
                throw new NotImplementedException();
            }
        }

        internal Request(NetStream client)
        {
            mClient = client;
            init();
        }

        void init()
        {
            while (true)
            {
                string line = mClient.ReadLine();
#if DEBUG
                //System.Diagnostics.Debug.WriteLine(line);
#endif      
                if (line.Length == 0)
                    break;
                else
                {
                    if (line.StartsWith("GET"))
                    {
                        try
                        {
                            _Headers["GET"] = line.Split(' ')[1];
                        }
                        catch
                        {
                        }
                    }
                    else if (line.StartsWith("POST"))
                    {
                        try
                        {
                            _Headers["POST"] = line.Split(' ')[1];
                        }
                        catch
                        {
                        }
                    }
                    else
                    {
                        int flag = line.IndexOf(":");
                        if (flag > 0)
                        {
                            try
                            {
                                string name = line.Substring(0, flag);
                                string value = line.Substring(flag + 1);
                                _Headers[name] = value.Trim();
                            }
                            catch
                            {
                            }
                        }
                    }
                }
            }
        }

        internal void urlRequestHandler()
        {
            if (string.Compare(_Headers["Content-Type"], "application/json", true) == 0)
            {
                //post 的是json数据
                System.Text.Encoding codec = null;
                if( _Headers["Content-Type"].Contains(";") )
                {
                    try
                    {
                        var charset = _Headers["Content-Type"].Split(';').SingleOrDefault(m => m.StartsWith("charset="));
                        if (charset != null)
                        {
                            charset = charset.Trim().Substring(8);
                            codec = System.Text.Encoding.GetEncoding(charset);
                        }
                    }
                    catch
                    {
                    }
                   
                }
                if(codec == null && !string.IsNullOrEmpty( _Headers["Charset"]) )
                {
                    try
                    {
                        codec = System.Text.Encoding.GetEncoding(_Headers["Charset"]);
                    }
                    catch
                    {
                    }
                }
                if (codec == null)
                    codec = System.Text.Encoding.UTF8;
                int contentLength = Convert.ToInt32(_Headers["Content-Length"]);
                byte[] jsonBS = mClient.ReceiveDatas(contentLength);
                string jsonStr = codec.GetString(jsonBS);
                var jsonObj = Newtonsoft.Json.JsonConvert.DeserializeObject(jsonStr);
                if (jsonObj is Newtonsoft.Json.Linq.JArray)
                {

                }
                else
                {
                    Newtonsoft.Json.Linq.JToken jsonToken = (Newtonsoft.Json.Linq.JToken)jsonObj;
                    foreach (Newtonsoft.Json.Linq.JProperty item in jsonToken)
                    {
                        _Form.Add(item.Name, item.Value.ToString());
                    }
                }
            }
            else if (this.Headers["Content-Type"].ToSafeString().Contains("x-www-form-urlencoded"))
            {
                int contentLength = Convert.ToInt32(_Headers["Content-Length"]);
                List<byte> buffer = new List<byte>();

                string keyName = null;
                for (int i = 0; i < contentLength; i++)
                {
                    int b = this.mClient.ReadByte();
                    if (b == (int)'=')
                    {
                        keyName = System.Text.Encoding.UTF8.GetString(buffer.ToArray());
                        buffer.Clear();

                        continue;
                    }
                    else if (b == (int)'&')
                    {

                        string str = System.Text.Encoding.UTF8.GetString(buffer.ToArray());
                        string value = WebUtility.UrlDecode(str);
                        buffer.Clear();
                        _Form.Add(keyName, value);
                        keyName = null;
                        continue;
                    }
                    buffer.Add((byte)b);
                }
                if (buffer.Count > 0 && keyName != null)
                {
                    string str = System.Text.Encoding.UTF8.GetString(buffer.ToArray());
                    string value = WebUtility.UrlDecode(str);
                    _Form.Add(keyName, value);
                }
            }
        }

        public override void Flush()
        {
            throw new NotImplementedException();
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            return mClient.Read(buffer, offset, count);
        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            throw new NotImplementedException();
        }

        public override void SetLength(long value)
        {
            throw new NotImplementedException();
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            throw new NotImplementedException();
        }
    }
}
