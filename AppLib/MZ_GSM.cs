using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AppLib
{
    public class GSM_Message
    {
        public string Phone;
        public string Content;
        public DateTime Time;
        public int Index;
        public int TotalPags;
        public int CurrentPag;
        public string MessageID;
        public string Codes;
    }

    public class MZ_GSM : IDisposable
    {
        class PDUDecoder
        {
            private string serviceCenterAddress;
            private string originatorAddress;
            private string serviceCenterTimeStamp;
            private string userData;
            private string userDataLenghth;
            char 参数显示用户数据编码方案;
            public PDUDecoder(string strPDU)
            {
                int lenSCA = Convert.ToInt32(strPDU.Substring(0, 2), 16) * 2 + 2;       //短消息中心占长度
                serviceCenterAddress = strPDU.Substring(0, lenSCA);

                strPDU = strPDU.Substring(lenSCA);
                //char PDU_Type = strPDU[0];
                //char 所有成功的短信发送参考数目 = strPDU[1];
                int pdu_type = Convert.ToInt32(strPDU.Substring(0, 2), 16);
                #region pdu_type 位说明
                //0,1  发送:01 接收00
                //2    普通：0，长短信：1
                //4,3  设置vp域格式
                //5    不会反馈状态报告：0，会反馈：1
                //6    带报头：1 ，只是短信内容：0
                //7    pdu串不带回复地址：0   带回复地址：1
                #endregion

                int lenOA = Convert.ToInt32(strPDU.Substring(2, 2), 16);           //OA占用长度
                if (lenOA % 2 == 1)                                                     //奇数则加1 F位
                {
                    lenOA++;
                }
                lenOA += 4;                 //加号码编码的头部长度
                originatorAddress = strPDU.Substring(2, lenOA);

                strPDU = strPDU.Substring(lenOA + 2);

                int 接收方手机长度 = Convert.ToInt32(strPDU.Substring(0, 2), 16);
                string 接收方手机 = strPDU.Substring(2, 接收方手机长度);
                strPDU = strPDU.Substring(2 + 接收方手机长度);

                char 参数显示消息中心以何种方式处理消息内容 = strPDU[0];
                参数显示用户数据编码方案 = strPDU[1];

                serviceCenterTimeStamp = strPDU.Substring(2, 14);
                strPDU = strPDU.Substring(2 + 14);

                userDataLenghth = strPDU.Substring(0, 2);
                lenUD = Convert.ToInt32(userDataLenghth, 16) * 2;
                userData = strPDU.Substring(2);
                if ((pdu_type & (1 << 6)) == (1 << 6))
                {
                    //有报头数据
                    try
                    {
                        int headerlen = Convert.ToInt32(userData.Substring(0, 2), 16) * 2;
                        string headerstr = userData.Substring(2, headerlen);
                        headerstr = headerstr.Substring(2);
                        int 后面还有多少数据 = Convert.ToInt32(headerstr.Substring(0, 2), 16) * 2;
                        headerstr = headerstr.Substring(2);
                        MessageID = headerstr.Substring(0, headerstr.Length - 4);
                        headerstr = headerstr.Substring(headerstr.Length - 4);
                        TotalPags = Convert.ToInt32(headerstr.Substring(0, 2), 16);
                        CurrentPag = Convert.ToInt32(headerstr.Substring(2, 2), 16);
                        lenUD -= 2 + headerstr.Length;
                        userData = userData.Substring(2 + headerlen);
                    }
                    catch
                    {
                    }
                }
            }
            public int TotalPags;
            public int CurrentPag;
            public string MessageID;
            int lenUD;
            public string ServiceCenterAddress
            {
                get
                {
                    int len = Convert.ToInt32(serviceCenterAddress.Substring(0, 2));   //获取SCA长度
                    string result = serviceCenterAddress.Substring(4, len - 4);        //去掉起头部分
                    //ParityChange函数见上一篇的编码
                    result = ParityChange(result);          //奇偶互换
                    result = result.TrimEnd('F', 'f');      //去掉结尾F
                    return result;
                }


            }


            public string OriginatorAddress
            {
                get
                {
                    int len = Convert.ToInt32(originatorAddress.Substring(0, 2), 16);    //十六进制字符串转为整形数据
                    string result = string.Empty;
                    if (len % 2 == 1)       //号码长度是奇数，长度加1 编码时加了F
                    {
                        len++;
                    }
                    result = originatorAddress.Substring(4, len);    //去掉头部
                    result = ParityChange(result).TrimEnd('F', 'f');  //奇偶互换，并去掉结尾F
                    if (result.StartsWith("86"))
                        result = result.Substring(2);

                    return result;
                }


            }


            public string ServiceCenterTimeStamp
            {
                get
                {
                    string result = ParityChange(serviceCenterTimeStamp);   //奇偶互换
                    result = "20" + result.Substring(0, 12);            //年加开始的“20”


                    return result;
                }
            }


            int gsmDecode7bit(byte[] pSrc, byte[] pDst, int nSrcLength)
            {
                int nSrc;        // 源字符串的计数值
                int nDst;        // 目标解码串的计数值
                int nByte;       // 当前正在处理的组内字节的序号，范围是0-6
                byte nLeft;    // 上一字节残余的数据

                // 计数值初始化
                nSrc = 0;
                nDst = 0;

                // 组内字节序号和残余数据初始化
                nByte = 0;
                nLeft = 0;

                // 将源数据每7个字节分为一组，解压缩成8个字节
                // 循环该处理过程，直至源数据被处理完
                // 如果分组不到7字节，也能正确处理
                while (nSrc < nSrcLength)
                {
                    // 将源字节右边部分与残余数据相加，去掉最高位，得到一个目标解码字节
                    pDst[nDst] = (byte)(((pSrc[nSrc] << nByte) | nLeft) & 0x7f);
                    // 将该字节剩下的左边部分，作为残余数据保存起来
                    nLeft = (byte)(pSrc[nSrc] >> (7 - nByte));

                    // 修改目标串的指针和计数值
                    nDst++;

                    // 修改字节计数值
                    nByte++;

                    // 到了一组的最后一个字节
                    if (nByte == 7)
                    {
                        // 额外得到一个目标解码字节
                        pDst[nDst] = nLeft;

                        // 修改目标串的指针和计数值

                        nDst++;

                        // 组内字节序号和残余数据初始化
                        nByte = 0;
                        nLeft = 0;
                    }

                    // 修改源串的指针和计数值

                    nSrc++;
                }

                //pDst[nDst] = 0;

                // 返回目标串长度
                return nDst;
            }


            public string UserData
            {
                get
                {
                    string result = string.Empty;
                    //四个一组，每组译为一个USC2字符
                    if (参数显示用户数据编码方案 == '0')
                    {

                        //7bit 解码
                        List<byte> bs = new List<byte>();
                        int bitlen = 1;
                        int 补下一个 = 0;
                        for (int i = 0; i + 1 < userData.Length; i += 2)
                        {
                            int byte1 = Convert.ToInt32(userData.Substring(i, 2), 16);
                            int src = byte1 & Convert.ToInt32("1".PadLeft(8 - bitlen, '1'), 2);
                            src = src << (bitlen - 1);
                            src |= 补下一个;
                            bs.Add((byte)src);
                            补下一个 = byte1 >> (8 - bitlen);

                            bitlen++;
                            if (bitlen == 8)
                            {
                                if (补下一个 > 0)
                                {
                                    bs.Add((byte)补下一个);
                                }
                                bitlen = 1;
                                补下一个 = 0;
                            }
                        }
                        return System.Text.Encoding.ASCII.GetString(bs.ToArray());
                    }
                    else
                    {
                        //int 余 = lenUD % 4;
                        //for (int i = 余; i + 4 < userData.Length; i += 4)
                        //{
                        //    string temp = userData.Substring(i, 4);


                        //    int byte1 = Convert.ToInt16(temp, 16);


                        //    result += ((char)byte1).ToString();
                        //}
                        for (int i = 0; i + 4 <= userData.Length; i += 4)
                        {
                            string temp = userData.Substring(i, 4);


                            int byte1 = Convert.ToInt16(temp, 16);


                            result += ((char)byte1).ToString();
                        }
                    }
                    if (MessageID != null)
                    {
                        return string.Format("({0}/{1}){2}", CurrentPag, TotalPags, result);
                    }
                    else
                        return result;
                }


            }


            /// <summary>
            /// 奇偶互换 (+F)
            /// </summary>
            /// <param name="str">要被转换的字符串</param>
            /// <returns>转换后的结果字符串</returns>
            private string ParityChange(string str)
            {
                string result = string.Empty;
                if (str.Length % 2 != 0)         //奇字符串 补F
                {
                    str += "F";
                }
                for (int i = 0; i < str.Length; i += 2)
                {
                    result += str[i + 1];
                    result += str[i];
                }
                return result;
            }
        }

        object lockobj = new object();
        List<byte> m_receiveds = new List<byte>();
        SerialPort m_SerialPort;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="port">串口号</param>
        public MZ_GSM(int port)
        {
            // PDUDecoder dd = new PDUDecoder("0891683108707515F16405A10180F6000851100211554523370608040681020200670064002E00310030003000380036002E0063006E002067E58BE230024E2D56FD79FB52A85E7F4E1C516C53F83002");

            m_SerialPort = new SerialPort("COM" + port, 9600, Parity.None, 8, StopBits.One);
            //m_SerialPort.DataReceived += m_SerialPort_DataReceived;
            //m_SerialPort.ErrorReceived += m_SerialPort_ErrorReceived;
            m_SerialPort.ReadTimeout = 20000;
            m_SerialPort.WriteTimeout = 20000;
            m_SerialPort.Open();

        }

        public void init()
        {
            lock (lockobj)
            {
                SendCmd("AT\r");
            }
        }

        void m_SerialPort_ErrorReceived(object sender, SerialErrorReceivedEventArgs e)
        {
            throw (new Exception("m_SerialPort_ErrorReceived"));
        }

        void m_SerialPort_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            byte[] bs = new byte[m_SerialPort.BytesToRead];
            m_SerialPort.Read(bs, 0, bs.Length);
            lock (lockobj)
            {
                m_receiveds.AddRange(bs);
            }
        }

        /// <summary>
        /// 将中心号码编码
        /// </summary>
        /// <param name="strValue"></param>
        /// <returns></returns>
        private string EncodCenterNumber(string centerNumber)
        {
            string resultString = "91";
            //判断是否为空
            if (string.IsNullOrEmpty(centerNumber))
            {
                return "00";
            }
            //取出+
            if (centerNumber.Substring(0, 1) == "+")
            {
                centerNumber = centerNumber.Substring(1, centerNumber.Length - 1);
            }
            //补位
            if (System.Math.IEEERemainder(double.Parse(centerNumber.Length.ToString()), 2) != 0)
            {
                centerNumber = centerNumber + "F";
            }
            //奇偶换位
            for (int i = 0; i < centerNumber.Length; i = i + 2)
            {
                resultString = resultString + centerNumber.Substring(i + 1, 1) + centerNumber.Substring(i, 1);
            }
            resultString = (resultString.Length / 2).ToString("X2") + resultString;
            return resultString;
        }



        /// <summary>
        /// 将电话号码编码
        /// </summary>
        /// <param name="phoneNumber">电话号码</param>
        /// <returns></returns>
        private string EncodPhoneNumber(string phoneNumber)
        {
            string resultString = string.Empty;
            //判断是否为空
            if (phoneNumber == "")
            {
                return "";
            }
            //取出+
            if (phoneNumber.Substring(0, 1) == "+")
            {
                phoneNumber = phoneNumber.Substring(1, phoneNumber.Length - 1);
            }

            if (!phoneNumber.StartsWith("86"))
                phoneNumber = "86" + phoneNumber;
            //补位
            if (System.Math.IEEERemainder(double.Parse(phoneNumber.Length.ToString()), 2) != 0)
            {
                phoneNumber = phoneNumber + "F";
            }
            //奇偶换位
            for (int i = 0; i < phoneNumber.Length; i = i + 2)
            {
                resultString = resultString + phoneNumber.Substring(i + 1, 1) + phoneNumber.Substring(i, 1);
            }
            return resultString;
        }



        public void DeleteMessage(int index)
        {
            lock (lockobj)
            {
                SendCmd("AT+CMGD=" + index + "\r", new string[] { "OK" });
            }
        }

        bool IsChina(string CString)
        {
            for (int i = 0; i < CString.Length; i++)
            {
                if (Convert.ToInt32(Convert.ToChar(CString.Substring(i, 1))) > Convert.ToInt32(Convert.ToChar(128)))
                {
                    return true;
                }
            }
            return false;
        }

        public static void test()
        {
            PDUDecoder decoder = new PDUDecoder("0891683108200075F12405A15965F60008511062110453236260A86B63901A8FC74E2D884C7F5194F64E3A6B64624B673A53F778015F00901A77ED4FE1670D52A1FF0C8BF78F935165624B673A9A8C8BC17801003100360032003800370034FF0C611F8C2260A87684652F6301FF0130104E2D56FD94F6884C3011");
            string dd = decoder.UserData;
        }

        public List<GSM_Message> GetAllMessages()
        {
            lock (lockobj)
            {
                List<GSM_Message> msgs = new List<GSM_Message>();
                SendCmd("AT\r", new string[] { "OK" });
                SendCmd("AT+CMGF=0\r", new string[] { "OK" });
                m_SerialPort.Write("AT+CMGL=4\r");
                while (true)
                {
                    string line = m_SerialPort.ReadLine();
                    if (line.Contains("OK"))
                        break;
                    else if (line.Contains("ERROR"))
                        throw (new Exception("AT+CMGL=4 ERROR!"));
                    else if (line.StartsWith("+CMGL:"))
                    {
                        line = line.Substring("+CMGL:".Length);
                        int index = Convert.ToInt32(line.Substring(0, line.IndexOf(",")));
                        GSM_Message msgObj = new GSM_Message();
                        msgObj.Index = index;
                        msgs.Add(msgObj);
                        try
                        {
                            msgObj.Content = m_SerialPort.ReadLine();
                            msgObj.Codes = msgObj.Content;
                            PDUDecoder decoder = new PDUDecoder(msgObj.Content);
                            string timestr = decoder.ServiceCenterTimeStamp.Substring(0, 4) + "-" + decoder.ServiceCenterTimeStamp.Substring(4, 2) + "-" + decoder.ServiceCenterTimeStamp.Substring(6, 2) + " " + decoder.ServiceCenterTimeStamp.Substring(8, 2) + ":" + decoder.ServiceCenterTimeStamp.Substring(10, 2) + ":" + decoder.ServiceCenterTimeStamp.Substring(12, 2);
                            msgObj.Phone = decoder.OriginatorAddress;
                            msgObj.MessageID = decoder.MessageID;
                            msgObj.TotalPags = decoder.TotalPags;
                            msgObj.CurrentPag = decoder.CurrentPag;
                            msgObj.Content = decoder.UserData;
                            msgObj.Time = Convert.ToDateTime(timestr);

                        }
                        catch (Exception ex)
                        {
                        }
                    }
                }
                return msgs;
            }
        }

        /// <summary>
        /// 将短信内容编码
        /// </summary>
        /// <param name="messageString">信息内容</param>
        /// <returns></returns>
        private string EncodMessage(string messageString)
        {
            int messageLength = messageString.Length * 2;
            string resultString = messageLength.ToString("X2");
            //编码信息内容
            Byte[] bytes = new Byte[messageString.Length * 2 + 2];
            string strValue1 = messageString;
            System.Text.Encoding.Unicode.GetBytes(strValue1, 0, strValue1.Length, bytes, 0);
            for (int i = 0; i < bytes.Length; i = i + 2)
            {
                if (bytes[i] == 0 && bytes[i + 1] == 0)
                {
                    i = bytes.Length;
                }
                else
                {
                    resultString = resultString + bytes[i + 1].ToString("X2") + bytes[i].ToString("X2");
                }
            }
            return resultString;
        }



        /// <summary>
        /// 将要发送的信息组合
        /// </summary>
        /// <param name="centerNumber">消息中心号码</param>
        /// <param name="phoneNumber">接收号码</param>
        /// <param name="messageString">信息内容</param>
        /// <returns></returns>
        public string GetSendMsg(string centerNumber, string phoneNumber, string messageString, out int length)
        {
            string resultString = string.Empty;
            //resultString = messageLength.ToString() + "\x00d";
            //手机号码
            resultString = resultString + "11000D91" + EncodPhoneNumber(phoneNumber) + "000800";
            //短信内容
            resultString = resultString + EncodMessage(messageString);
            length = (resultString.Length / 2);

            resultString = EncodCenterNumber(centerNumber) + resultString;

            return resultString;
        }
        void SendCmd(string cmd)
        {
            SendCmd(cmd, new string[] { "OK" });
        }
        void SendCmd(string cmd, string[] mayReturns)
        {
            m_SerialPort.Write(cmd);
            readUntilFind(cmd, mayReturns);
        }
        void readUntilFind(string cmd, string[] mayReturns)
        {
            readUntilFind(cmd, mayReturns, null);
        }
        void readUntilFind(string cmd, string[] mayReturns, StringBuilder buffers)
        {
            while (true)
            {
                string line = m_SerialPort.ReadLine();
                if (buffers != null)
                {
                    buffers.AppendLine(line);
                }
                if (mayReturns.FirstOrDefault(m => line.Contains(m)) != null)
                    break;
                else if (line.Contains("ERROR"))
                    throw (new Exception(cmd + " Error!"));

            }
        }
        public void SendMessage(string phoneNumber, string content)
        {
            if (IsChina(content) == false && content.Length < 30)
            {
                SendEngMessage(phoneNumber, content);
                return;
            }

            if (content.Length > 70)
            {
                for (int i = 0; i < content.Length; i += 70)
                {
                    int len = content.Length - i;
                    string text = content.Substring(i, Math.Min(len, 70));
                    SendMessage(phoneNumber, text);
                }
                return;
            }
            lock (lockobj)
            {
                SendCmd("AT\r");
                SendCmd("AT+CMGF=0\r");

                int msglen;
                string msg = GetSendMsg(null, phoneNumber, content, out msglen);//"+8613800668500"

                m_SerialPort.Write("AT+CMGS=" + msglen + "\r");
                string buffertext = "";
                while (true)
                {
                    buffertext += m_SerialPort.ReadExisting();

                    if (buffertext.Contains('>'))
                        break;
                    else if (buffertext.Contains("ERROR"))
                        throw (new Exception(buffertext));
                }

                m_SerialPort.Write(msg);
                m_SerialPort.Write(new byte[] { 0x1a }, 0, 1);
                readUntilFind("AT+CMGS=", new string[] { "OK" });
            }
        }


        /// <summary>
        /// 发送短信
        /// </summary>
        /// <param name="phoneNumber">手机号</param>
        /// <param name="content">短信内容</param>
        public void SendEngMessage(string phoneNumber, string content)
        {
            SendCmd("AT\r", new string[] { "OK" });
            SendCmd("AT+CMGF=1\r", new string[] { "OK" });
            m_SerialPort.WriteLine("AT+CMGS=" + phoneNumber + "\r");
            string buffertext = "";
            while (true)
            {
                buffertext += m_SerialPort.ReadExisting();

                if (buffertext.Contains('>'))
                    break;
                else if (buffertext.Contains("ERROR"))
                    throw (new Exception(buffertext));
            }
            m_SerialPort.Write(content + "\r");
            m_SerialPort.Write(new byte[] { 0x1a }, 0, 1);
            readUntilFind("AT+CMGS=" + phoneNumber, new string[] { "OK" });
        }


        public void Dispose()
        {
            m_SerialPort.Close();
            m_SerialPort.Dispose();
        }
    }
}
