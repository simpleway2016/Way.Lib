﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Way.Lib.ScriptRemoting
{ /// <summary>
    /// 2字节数据头
    /// </summary>
    public class DataFrameHeader
    {
        private bool _fin;
        private bool _rsv1;
        private bool _rsv2;
        private bool _rsv3;
        private sbyte _opcode;
        private bool _maskcode;
        private sbyte _payloadlength;

        /// <summary>
        /// FIN
        /// </summary>
        public bool FIN { get { return _fin; } }

        /// <summary>
        /// RSV1
        /// </summary>
        public bool RSV1 { get { return _rsv1; } }

        /// <summary>
        /// RSV2
        /// </summary>
        public bool RSV2 { get { return _rsv2; } }

        /// <summary>
        /// RSV3
        /// </summary>
        public bool RSV3 { get { return _rsv3; } }

        /// <summary>
        /// OpCode
        /// </summary>
        public sbyte OpCode { get { return _opcode; } }

        /// <summary>
        /// 是否有掩码
        /// </summary>
        public bool HasMask { get { return _maskcode; } }

        /// <summary>
        /// Payload Length
        /// </summary>
        public sbyte Length { get { return _payloadlength; } }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <remarks>主要用于解析接收数据</remarks>
        public DataFrameHeader(byte[] buffer)
        {
            if (buffer.Length < 2)
                throw new Exception("无效的数据头.");
            //第一个字节
            _fin = (buffer[0] & 0x80) == 0x80;
            _rsv1 = (buffer[0] & 0x40) == 0x40;
            _rsv2 = (buffer[0] & 0x20) == 0x20;
            _rsv3 = (buffer[0] & 0x10) == 0x10;
            _opcode = (sbyte)(buffer[0] & 0x0f);
            //第二个字节
            _maskcode = (buffer[1] & 0x80) == 0x80;
            _payloadlength = (sbyte)(buffer[1] & 0x7f);

        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <remarks>主要用于发送封装数据</remarks>
        public DataFrameHeader(bool fin, bool rsv1, bool rsv2, bool rsv3, sbyte opcode, bool hasmask, int length)
        {
            _fin = fin;
            _rsv1 = rsv1;
            _rsv2 = rsv2;
            _rsv3 = rsv3;
            _opcode = opcode;
            //第二个字节
            _maskcode = hasmask;
            _payloadlength = (sbyte)length;
        }

        /// <summary>
        /// 返回帧头字节
        /// </summary>
        /// <returns></returns>
        public byte[] GetBytes()
        {
            byte[] buffer = new byte[2] { 0, 0 };

            if (_fin) buffer[0] ^= 0x80;
            if (_rsv1) buffer[0] ^= 0x40;
            if (_rsv2) buffer[0] ^= 0x20;
            if (_rsv3) buffer[0] ^= 0x10;
            buffer[0] ^= (byte)_opcode;

            if (_maskcode) buffer[1] ^= 0x80;
            buffer[1] ^= (byte)_payloadlength;

            return buffer;
        }
    }
    /// <summary>
    /// 数据帧
    /// </summary>
    public class DataFrame
    {
        DataFrameHeader _header;
        private byte[] _extend = new byte[0];
        private byte[] _mask = new byte[0];
        private byte[] _content = new byte[0];

        public byte[] Content
        {
            get
            {
                return _content;
            }
        }
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <remarks>主要用于解析接收数据</remarks>
        public DataFrame(byte[] buffer)
        {
            //格式化帧头
            _header = new DataFrameHeader(buffer);
            //填充扩展长度字节
            if (_header.Length == 126)
            {
                _extend = new byte[2];
                Buffer.BlockCopy(buffer, 2, _extend, 0, 2);
            }
            else if (_header.Length == 127)
            {
                _extend = new byte[8];
                Buffer.BlockCopy(buffer, 2, _extend, 0, 8);
            }
            //是否有掩码
            if (_header.HasMask)
            {
                _mask = new byte[4];
                Buffer.BlockCopy(buffer, _extend.Length + 2, _mask, 0, 4);
            }
            //消息体
            if (_extend.Length == 0)
            {
                _content = new byte[_header.Length];
                Buffer.BlockCopy(buffer, _extend.Length + _mask.Length + 2, _content, 0, _content.Length);
            }
            else if (_extend.Length == 2)
            {
                _content = new byte[BitConverter.ToUInt16(_extend , 0)];
                Buffer.BlockCopy(buffer, _extend.Length + _mask.Length + 2, _content, 0, _content.Length);
            }
            else
            {
                _content = new byte[BitConverter.ToUInt64(_extend, 0)];
                Buffer.BlockCopy(buffer, _extend.Length + _mask.Length + 2, _content, 0, _content.Length);
            }
            //如果有掩码，则需要还原原始数据
            if (_header.HasMask) _content = Mask(_content, _mask);

        }

        public DataFrame()
        {
        }
        public void InitWithByteContent(byte[] content)
        {
            _content = content;
            int length = _content.Length;

            if (length < 126)
            {
                _extend = new byte[0];
                _header = new DataFrameHeader(true, false, false, false, OpCode.Binary, false, length);
            }
            else if (length < 65536)
            {
                _extend = new byte[2];
                _header = new DataFrameHeader(true, false, false, false, OpCode.Binary, false, 126);
                _extend[0] = (byte)(length / 256);
                _extend[1] = (byte)(length % 256);
            }
            else
            {
                _extend = new byte[8];
                _header = new DataFrameHeader(true, false, false, false, OpCode.Binary, false, 127);

                int left = length;
                int unit = 256;

                for (int i = 7; i > 1; i--)
                {
                    _extend[i] = (byte)(left % unit);
                    left = left / unit;

                    if (left == 0)
                        break;
                }
            }
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <remarks>主要用于发送封装数据</remarks>
        public DataFrame(string content)
        {
            _content = Encoding.UTF8.GetBytes(content);
            int length = _content.Length;

            if (length < 126)
            {
                _extend = new byte[0];
                _header = new DataFrameHeader(true, false, false, false, OpCode.Text, false, length);
            }
            else if (length < 65536)
            {
                _extend = new byte[2];
                _header = new DataFrameHeader(true, false, false, false, OpCode.Text, false, 126);
                _extend[0] = (byte)(length / 256);
                _extend[1] = (byte)(length % 256);
            }
            else
            {
                _extend = new byte[8];
                _header = new DataFrameHeader(true, false, false, false, OpCode.Text, false, 127);

                int left = length;
                int unit = 256;

                for (int i = 7; i > 1; i--)
                {
                    _extend[i] = (byte)(left % unit);
                    left = left / unit;

                    if (left == 0)
                        break;
                }
            }
        }

        /// <summary>
        /// 获取适合传送的字节数据
        /// </summary>
        public byte[] GetBytes()
        {
            byte[] buffer = new byte[2 + _extend.Length + _mask.Length + _content.Length];
            Buffer.BlockCopy(_header.GetBytes(), 0, buffer, 0, 2);
            Buffer.BlockCopy(_extend, 0, buffer, 2, _extend.Length);
            Buffer.BlockCopy(_mask, 0, buffer, 2 + _extend.Length, _mask.Length);
            Buffer.BlockCopy(_content, 0, buffer, 2 + _extend.Length + _mask.Length, _content.Length);
            return buffer;
        }

        /// <summary>
        /// 获取文本
        /// </summary>
        public string Text
        {
            get
            {
                if (_header.OpCode != OpCode.Text)
                    return string.Empty;

                return Encoding.UTF8.GetString(_content);
            }
        }

        /// <summary>
        /// 加掩码运算
        /// </summary>
        private byte[] Mask(byte[] data, byte[] mask)
        {
            for (var i = 0; i < data.Length; i++)
            {
                data[i] = (byte)(data[i] ^ mask[i % 4]);
            }

            return data;
        }

    }
}
