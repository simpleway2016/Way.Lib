﻿using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Collections;
using System.Collections.Generic;
using System.Threading;

namespace Way.Lib.ScriptRemoting
{
    public class NetStreamEventArgs 
    {
        public bool HasError
        {
            get;
            set;
        }
        public byte[] Data
        {
            get;
            set;
        }
    }
    /// <summary>
    /// NetStream 的摘要说明。
    /// </summary>
    public class NetStream : Stream
    {
        class ReceiveState
        {
            public int Length;
            public int Offset;
            public byte[] Data;
            public ReceivedHandler Callback;
        }
       

       
        private Socket m_ClientSocket;
        public Socket Socket
        {
            get
            {
                return m_ClientSocket;
            }
            private set
            {
                m_ClientSocket = value;
            }
        }
        private bool m_Active;
        private System.Text.Encoding code = System.Text.Encoding.UTF8;
       

        private const int dataBuffer = 1024;

        public System.Text.Encoding Encoding
        {
            get
            {
                return code;
            }
            set
            {
                code = value;
            }
        }

        private System.Text.Encoding _ErrorEncoding = System.Text.Encoding.UTF8;
        public System.Text.Encoding ErrorEncoding
        {
            get
            {
                return _ErrorEncoding;
            }
            set
            {
                _ErrorEncoding = value;
            }
        }

        protected override void Dispose(bool disposing)
        {
            this.Close();
            base.Dispose(disposing);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Address">主机名或者ip地址</param>
        /// <param name="port">端口</param>
        public NetStream(string Address, int port)
        {

            Socket socket2 = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            socket2.SendTimeout = 6000;
            socket2.ReceiveTimeout = 6000;

            socket2.Connect(IPAddress.Parse(Address), port);
            this.m_ClientSocket = socket2;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="SocketClient"></param>
        public NetStream(Socket SocketClient)
        {
            this.m_ClientSocket = SocketClient;
            this.Socket.SendTimeout = 6000;
            this.Socket.ReceiveTimeout = 6000;
            this.Socket.ReceiveBufferSize = 1024 * 100;

            try
            {
                //经典代码,再也不用写什么心跳包了，接收数据必须采用BeginReceive
                byte[] inValue = new byte[] { 1, 0, 0, 0, 0x20, 0x4e, 0, 0, 0xd0, 0x07, 0, 0 }; //True, 20 秒, 2 秒
                this.Socket.IOControl(IOControlCode.KeepAliveValues, inValue, null);
            }
            catch { return; }

        }

        /// <summary>
        /// 
        /// </summary>
        public void Connect()
        {

        }

        /// <summary>
        /// 
        /// </summary>
        public void Close()
        {
          
            try
            {
                Socket.Dispose();
            }
            catch
            {
            }
            Socket = null;
        }



        public bool Connected
        {
            get
            {
                return Socket.Connected;
            }
        }

        public void WriteHtmlFile(string filePath)
        {
            StreamReader reader = new System.IO.StreamReader(new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite), this.Encoding);
            while (true)
            {
                string line = reader.ReadLine();
                if (line == null)
                    break;
                WriteLine(line);
            }
            reader.Dispose();
        }

        public delegate void ReceivedHandler(object sender ,NetStreamEventArgs e );
        public void BeginReceiveDatas(byte[] buffer, int offset, int size , ReceivedHandler callback)
        {
           
            ReceiveState state = new ReceiveState();
            state.Data = buffer;
            state.Offset = offset;
            state.Length = size;
            state.Callback = callback;
            try
            {
                var task = Socket.ReceiveAsync(new ArraySegment<byte>(state.Data,state.Offset,state.Length), SocketFlags.None);
                task.Wait();
                receiveDataCallBack(task.Result, state);
            }
            catch
            {
                try
                {
                    Socket.Dispose();
                }
                catch
                {
                }
                Socket = null;
                NetStreamEventArgs arg = new NetStreamEventArgs();
                arg.HasError = true;
                state.Callback.Invoke(this, arg);
            }
        }
        private void receiveDataCallBack(int length, ReceiveState state)
        {
            if (length == 0 )
            {
                try
                {
                    Socket.Dispose();
                }
                catch
                {
                }
                Socket = null;
                NetStreamEventArgs arg = new NetStreamEventArgs();
                arg.HasError = true;
                state.Callback.Invoke(this, arg);
                return;
            }

           
            state.Offset += length;
            if (state.Offset < state.Length)
            {
                var task = Socket.ReceiveAsync(new ArraySegment<byte>(state.Data, state.Offset, state.Length - state.Offset), SocketFlags.None);
                task.Wait();
                receiveDataCallBack(task.Result, state);
               
            }
            else
            {
                NetStreamEventArgs arg = new NetStreamEventArgs();
                arg.Data = state.Data;
                state.Callback.Invoke(this, arg);
            }
        }

        public byte[] ReceiveDatas(int length)
        {
            int offset = 0;
            byte[] buffer = new byte[length];
            int readed;
            while (true)
            {
                if (Socket.Available >= length)
                {
                    readed = Socket.Receive(buffer, offset, buffer.Length - offset, SocketFlags.None);
                }
                else
                {
                    var task = Socket.ReceiveAsync( new ArraySegment<byte>(buffer, offset, buffer.Length - offset) , SocketFlags.None );
                    task.Wait();
                    readed = task.Result;

                    if (readed == 0)
                    {
                        throw new Exception("Socket已经断开");
                    }
                }
                offset += readed;
                if (offset >= buffer.Length)
                    break;
            }

            return buffer;
        }
        public void ReceiveDatas(byte[] buffer , int offset , int length)
        {
            int readed;
            while (true)
            {
                if (Socket.Available >= length)
                {
                    readed = Socket.Receive(buffer, offset, buffer.Length - offset, SocketFlags.None);
                }
                else
                {
                    var task = Socket.ReceiveAsync(new ArraySegment<byte>(buffer, offset, buffer.Length - offset), SocketFlags.None);
                    task.Wait();
                    readed = task.Result;
                    
                    if (readed == 0)
                    {
                        throw new Exception("Socket已经断开");
                    }
                }
                

                offset += readed;
                if (offset >= buffer.Length)
                    break;
            }
        }

        /// <summary>
        /// 读取一行，或者读取到某个字符则返回
        /// </summary>
        /// <param name="encoding"></param>
        /// <param name="finallyReaded">读取了几个字节</param>
        /// <param name="_char">指定字符</param>
        /// <returns></returns>
        public string ReadLineOREndWithOtherChar(System.Text.Encoding encoding, ref int finallyReaded, char _char)
        {
            List<byte> datas = new List<byte>(1024);
            byte[] bs = new byte[1];
            int readed;
            while (true)
            {
               
                if(Socket.Available > 0)
                {
                    readed = Socket.Receive(bs);
                }
                else
                {
                    var task = Socket.ReceiveAsync(new ArraySegment<byte>(bs, 0, bs.Length), SocketFlags.None);
                    task.Wait();
                    readed = task.Result;

                    if (readed == 0)
                    {
                        throw new Exception("Socket已经断开");
                    }
                }

                finallyReaded += readed;
                if (readed > 0)
                {
                    byte b = bs[0];
                    if (b == _char)
                    {
                        return encoding.GetString(datas.ToArray());
                    }
                    if (b == 0xa || b == 0xd)
                    {
                        if (b == 0xa)
                            return encoding.GetString(datas.ToArray());
                    }
                    else
                    {
                        datas.Add(bs[0]);
                    }
                }
            }

        }



      
        public string ReadLine()
        {
            byte[] bs = new byte[1];
            List<byte> lineBuffer = new List<byte>(1024);
            
            while(true)
            {
                if (Socket.Available > 0)
                {
                    Socket.Receive(bs);
                }
                else
                {
                    var task = Socket.ReceiveAsync(new ArraySegment<byte>(bs, 0, bs.Length), SocketFlags.None);
                    task.Wait();
                    
                    if (task.Result == 0)
                    {
                        throw new Exception("Socket已经断开");
                    }
                }

                if (bs[0] == 0xa)
                {
                    lineBuffer.RemoveAt(lineBuffer.Count - 1);
                    break;
                }
                else
                {
                    lineBuffer.Add(bs[0]);
                }
            }
           

            return this.code.GetString(lineBuffer.ToArray());
        }
       

        public void WriteLine(string text)
        {
            byte[] buffer = this.Encoding.GetBytes(text + "\r\n");
            _write(buffer, 0, buffer.Length);
        }


        public void Write(byte[] buffer)
        {
            Write(buffer, 0, buffer.Length);
        }

        public void Write(byte[] buffer, int size)
        {
            Write(buffer, 0, size);
        }

        public void WriteByEachSize(byte[] buffer, int offset, int count, int eachSize)
        {
            if (buffer.Length == 0)
                return;
            try
            {
                int writed = offset;

                while (count > 0)
                {
                    int f = Socket.Send(buffer, writed, Math.Min(count, eachSize), SocketFlags.None);
                    writed += f;
                    count -= f;
                    if (f == 0)
                    {
                        Thread.Sleep(10);
                    }
                }
            }
            catch
            {
            }

        }

        /// <summary>
        /// 按块大小发送数据
        /// </summary>
        /// <param name="buffer"></param>
        /// <param name="eachSize">每次发送多少字节</param>
        public void WriteByEachSize(byte[] buffer, int eachSize)
        {
            WriteByEachSize(buffer, 0, buffer.Length, eachSize);
        }
        public int _write(byte[] buffer)
        {

            return _write(buffer, 0, buffer.Length);

        }
        public int _write(byte[] buffer, int offset, int count)
        {
            if (buffer.Length == 0)
                return 0;

            if (count > 10240)
            {
                WriteByEachSize(buffer, offset, count, 10240);
                return count;
            }
            try
            {
                int writed = offset;


                while (count > 0)
                {
                    int f = Socket.Send(buffer, writed, count, SocketFlags.None);
                    writed += f;
                    count -= f;
                    if (f == 0)
                    {
                        Thread.Sleep(10);
                    }
                }

                return count;
            }
            catch
            {
            }
            return 0;
        }



        public override bool CanRead
        {
            get { return true; }
        }

        public override bool CanSeek
        {
            get { return false; }
        }

        public override bool CanWrite
        {
            get { return true; }
        }

        public override void Flush()
        {

        }

        public override long Length
        {
            get { throw new Exception("The method or operation is not implemented."); }
        }

        public override long Position
        {
            get
            {
                throw new Exception("The method or operation is not implemented.");
            }
            set
            {
                throw new Exception("The method or operation is not implemented.");
            }
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            if (count <= 0)
                return 0;

            var task = Socket.ReceiveAsync(new ArraySegment<byte>(buffer, offset, count), SocketFlags.None);
            task.Wait();
            var readed = task.Result;

            if (readed == 0)
            {
                throw new Exception("Socket 已经断开");
            }
            return readed;
        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public override void SetLength(long value)
        {
            throw new Exception("The method or operation is not implemented.");
        }
        public delegate void BeforeWriteStringHandler(ref string content);
        public event BeforeWriteStringHandler BeforeWriteString;
        public override void Write(byte[] buffer, int offset, int count)
        {

            _write(buffer, offset, count);
        }

        public override void WriteByte(byte value)
        {
            this.Write(new byte[1] { value });
        }

        public override int ReadByte()
        {
            byte[] bs = this.ReceiveDatas(1);
            return (int)bs[0];
        }

        public void Write(char _char)
        {
            this.Write(new byte[1] { (byte)_char });
        }

        /// <summary>
        /// 此方法会写入\0字符，不需要写入\0字符请调用WriteString
        /// </summary>
        /// <param name="text"></param>
        public void Write(string text)
        {
            byte[] bs = this.code.GetBytes(text);
            byte[] newbs = new byte[bs.Length + 1];
            Array.Copy(bs, newbs, bs.Length);
            newbs[bs.Length] = 0;
            this.Write(newbs);

        }

        public void WriteString(string text)
        {
            if (text != null)
            {
                if (BeforeWriteString != null)
                {
                    BeforeWriteString(ref text);
                }

                byte[] bs = this.code.GetBytes(text);
                this.Write(bs);
            }
        }

        public void Write(int _int)
        {
            byte[] bs = BitConverter.GetBytes(_int);
            this.Write(bs);
        }

        public int ReadInt()
        {
            byte[] bs = this.ReceiveDatas(4);
            return BitConverter.ToInt32(bs, 0);
        }

        public double ReadDouble()
        {
            byte[] bs = this.ReceiveDatas(8);
            return BitConverter.ToDouble(bs, 0);
        }
        public float ReadFloat()
        {
            byte[] bs = this.ReceiveDatas(4);
            return BitConverter.ToSingle(bs, 0);
        }
        public long ReadLong()
        {
            byte[] bs = this.ReceiveDatas(8);
            return BitConverter.ToInt64(bs, 0);
        }

        public short ReadShort()
        {
            byte[] bs = this.ReceiveDatas(2);
            return BitConverter.ToInt16(bs, 0);
        }

        public bool ReadBoolean()
        {
            byte[] bs = this.ReceiveDatas(1);
            return BitConverter.ToBoolean(bs, 0);
        }

        public void Write(bool _int)
        {
            byte[] bs = BitConverter.GetBytes(_int);
            this.Write(bs);
        }

        public void Write(short _int)
        {
            byte[] bs = BitConverter.GetBytes(_int);
            this.Write(bs);
        }

        public void Write(long _long)
        {
            byte[] bs = BitConverter.GetBytes(_long);
            this.Write(bs);
        }

        public void Write(float _float)
        {
            byte[] bs = BitConverter.GetBytes(_float);
            this.Write(bs);
        }

        public void Write(double _double)
        {
            byte[] bs = BitConverter.GetBytes(_double);
            this.Write(bs);
        }
    }


}