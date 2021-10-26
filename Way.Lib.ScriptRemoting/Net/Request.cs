﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Way.Lib.ScriptRemoting.Net
{
  
    public class Request :IDisposable
    {
        class RequestBodyStream : System.IO.Stream
        {
            Request _request;
            public RequestBodyStream(Request request)
            {
                _request = request;
            }
            public override bool CanRead => true;

            public override bool CanSeek => false;

            public override bool CanWrite => false;

            public override long Length => _request.ContentLength;

            public override long Position { get => throw new NotSupportedException(); set => throw new NotSupportedException(); }

            public override void Flush()
            {
                
            }

            public override int Read(byte[] buffer, int offset, int count)
            {
               return _request.mClient.Read(buffer, 0, count);
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

        internal NetStream mClient;
        RemotingContext _context;

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
                return mClient.RemoteEndPoint;
            }
        }

        RemotingContext _Context;
        public RemotingContext Context
        {
            get
            {
                if (_Context == null)
                    _Context = RemotingContext.Current;
                return _context;
            }
        }

        public string ContentType
        {
            get;
            set;
        }
        public int ContentLength
        {
            get;
            private set;
        }

        byte[] _contentData;
        System.IO.Stream _Body;
        public System.IO.Stream Body
        {
            get
            {
                if (_Body == null)
                {
                    if (_contentData != null)
                    {
                        _Body = new System.IO.MemoryStream();
                        _Body.Write(_contentData, 0, _contentData.Length);
                        _Body.Position = 0;
                        _contentData = null;
                    }
                    else
                    {
                        _Body = new RequestBodyStream(this);
                    }
                }
                return _Body;
            }
        }

        internal Request(NetStream client,RemotingContext context)
        {
            _context = context;
            mClient = client;
            init();
        }

        /// <summary>
        /// 把相对url转换为实际文件路径
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public string MapPath(string url)
        {
            var root = RemotingContext.Current.WebRoot;
            root = root.Substring(0, root.Length - 1);
            return $"{root}{url}";
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
            try
            {
                this.ContentLength = Convert.ToInt32( _Headers["Content-Length"]);
            }
            catch { }
            this.ContentType = this.Headers["Content-Type"];
        }

        internal void urlRequestHandler()
        {
            if (string.Compare(this.ContentType, "application/json", true) == 0)
            {
                //post 的是json数据
                System.Text.Encoding codec = null;
                if(this.ContentType.Contains(";") )
                {
                    try
                    {
                        var charset = this.ContentType.Split(';').SingleOrDefault(m => m.StartsWith("charset="));
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

                _contentData = mClient.ReceiveDatas(this.ContentLength);

                string jsonStr = codec.GetString(_contentData);
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
            else if (this.ContentType.ToSafeString().Contains("x-www-form-urlencoded"))
            {
               
                _contentData = mClient.ReceiveDatas(this.ContentLength);

                List<byte> buffer = new List<byte>();

                string keyName = null;
                for (int i = 0; i < this.ContentLength; i++)
                {
                    int b = _contentData[i];
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
            else if (this.ContentType.StartsWith("multipart/form-data;"))
            {
                //multipart/form-data; boundary="9b58a389-566b-4e7f-91ea-6ea881703bbf"

                var m = Regex.Match(this.ContentType, @"boundary=\""(?<b>.+)\""");
                string boundary = m.Groups["b"].Value;
                System.Text.Encoding codec = null;
                if (codec == null && !string.IsNullOrEmpty(_Headers["Charset"]))
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

                _contentData = mClient.ReceiveDatas(this.ContentLength);

                bool itemstart = false;
                bool contentstart = false;
                string itemvalue = null;
                string itemname = null;
                using (var ms = new MemoryStream(_contentData))
                using (var sr = new StreamReader(ms , codec))
                {
                    while (true)
                    {
                        var line = sr.ReadLine();
                        if (line == null)
                            break;
                        if(line == $"--{boundary}")
                        {
                            if(itemstart && contentstart)
                            {
                                itemstart = false;
                                contentstart = false;
                                if (itemvalue.Length > 0)
                                {
                                    itemvalue = itemvalue.Substring(0, itemvalue.Length - 2);
                                }
                                if (!string.IsNullOrEmpty(itemname))
                                {
                                    _Form.Add(itemname, itemvalue);
                                }
                            }
                            itemstart = true;
                            itemvalue = "";
                            itemname = null;
                        }
                        else if(itemstart)
                        {
                            if(contentstart)
                            {
                                if(line == $"--{boundary}--")
                                {
                                    itemstart = false;
                                    contentstart = false;
                                    if (itemvalue.Length > 0)
                                    {
                                        itemvalue = itemvalue.Substring(0, itemvalue.Length - 2);
                                    }
                                    if(!string.IsNullOrEmpty(itemname))
                                    {
                                        _Form.Add(itemname, itemvalue);
                                    }
                                    continue;
                                }
                                itemvalue += line + "\r\n";

                            }
                            else if(line.Length == 0)
                            {
                                contentstart = true;
                            }
                            else
                            {
                                if(line.StartsWith("Content-Disposition:"))
                                {
                                    try
                                    {
                                        line = line.Substring("Content-Disposition:".Length);
                                        var index = line.IndexOf("name=");
                                        if (index >= 0)
                                        {
                                            itemname = line.Substring(index + 5);
                                        }
                                    }
                                    catch
                                    {

                                    }
                                    
                                }
                            }
                        }
                    }
                }
                   
                /*
                 --9b58a389-566b-4e7f-91ea-6ea881703bbf
Content-Type: text/plain; charset=utf-8
Content-Disposition: form-data; name=m

{"ClassFullName":"PandaAudioServer.MainController","MethodName":"test","ParameterJson":"[\"1\",\"2\"]","SessionID":null}
--9b58a389-566b-4e7f-91ea-6ea881703bbf--
                 */
            }
        }

        public void Dispose()
        {
            _contentData = null;
            if (_Body != null && _Body is System.IO.MemoryStream)
            {
                _Body.Dispose();
                _Body = null;
            }
        }
    }
}
