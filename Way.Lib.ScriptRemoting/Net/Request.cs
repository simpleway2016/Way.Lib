using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Way.Lib.ScriptRemoting.Net
{
    public class Request
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
                //Debug.WriteLine(line);
                if (line.Length == 0)
                    break;
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
                    else if (line.StartsWith("GET"))
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
                }
            }
        }

        internal void urlRequestHandler()
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
}
