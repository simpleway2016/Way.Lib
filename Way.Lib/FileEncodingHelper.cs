using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Way.Lib
{
    public class FileEncodingHelper
    {
        /// <summary>
        /// 获取文件的编码类型
        /// </summary>
        /// <param name="filename">文件路径</param>
        /// <returns>文件的编码类型</returns>
        public static System.Text.Encoding GetFileEncoding(string filename)
        {
            try
            {
                using (FileStream fs = File.OpenRead(filename))
                {
                    Encoding r = GetType(fs);
                    return r;
                }
            }
            catch
            {
                return Encoding.Default;
            }
        }

        static System.Text.Encoding GetType(FileStream fs)
        {
#if NETSTANDARD2_0
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
#endif
            try
            {
                fs.Position = 0;
                byte[] Unicode = new byte[] { 0xFF, 0xFE, 0x41 };
                byte[] UnicodeBIG = new byte[] { 0xFE, 0xFF, 0x00 };
                byte[] UTF8 = new byte[] { 0xEF, 0xBB, 0xBF }; //带BOM
                Encoding reVal = Encoding.Default;

                byte[] ss = new byte[3];
                fs.Read(ss, 0, 3);

                fs.Position = 0;
                if (IsUTF8Bytes(fs) || (ss[0] == 0xEF && ss[1] == 0xBB && ss[2] == 0xBF))
                {
                    reVal = Encoding.UTF8;
                }
                else if (ss[0] == 0xFE && ss[1] == 0xFF && ss[2] == 0x00)
                {
                    reVal = Encoding.BigEndianUnicode;
                }
                else if (ss[0] == 0xFF && ss[1] == 0xFE && ss[2] == 0x41)
                {
                    reVal = Encoding.Unicode;
                }
                //else if (ss[0] == 0x31 && ss[1] == 0xA1 && ss[2] == 0xA2)
                else
                {
                    reVal = Encoding.GetEncoding("GB2312");
                }
                
                return reVal;
            }
            catch
            {
                return Encoding.Default;
            }

        }

        /// <summary>
        /// 判断是否是不带 BOM 的 UTF8 格式
        /// </summary>
        private static bool IsUTF8Bytes(Stream stream)
        {
            try
            {

                int charByteCounter = 1; //计算当前正分析的字符应还有的字节数
                byte curByte; //当前分析的字节.
                for (int i = 0; i < stream.Length; i++)
                {
                    curByte = (byte)stream.ReadByte();
                    if (charByteCounter == 1)
                    {
                        if (curByte >= 0x80)
                        {
                            //判断当前
                            while (((curByte <<= 1) & 0x80) != 0)
                            {
                                charByteCounter++;
                            }
                            //标记位首位若为非0 则至少以2个1开始 如:110XXXXX...........1111110X
                            if (charByteCounter == 1 || charByteCounter > 6)
                            {
                                return false;
                            }
                        }
                    }
                    else
                    {
                        //若是UTF-8 此时第一位必须为1
                        if ((curByte & 0xC0) != 0x80)
                        {
                            return false;
                        }
                        charByteCounter--;
                    }
                }
                if (charByteCounter > 1)
                {
                    throw new Exception("非预期的byte格式");
                }
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
