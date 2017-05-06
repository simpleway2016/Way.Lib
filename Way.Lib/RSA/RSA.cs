using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Way.Lib
{
    public class RSADecrptException : Exception
    {
    }
    public class RSA
    {
        System.Security.Cryptography.RSA _rsa;
        System.Security.Cryptography.RSAParameters _parameter;
        string _KeyExponent;
        public string KeyExponent
        {
            get { return _KeyExponent; }
        }

        string _KeyModulus;
        public string KeyModulus
        {
            get { return _KeyModulus; }
        }
        public byte[] D
        {
            get
            {
                return _parameter.D;
            }
        }
        public RSA()
        {
            _rsa = System.Security.Cryptography.RSA.Create();
            _rsa.KeySize = 1024;//默认是2048，也就是_parameter.Modulus是256字节，但是js那边的算法会卡死
            //把公钥适当转换，准备发往客户端
            _parameter = _rsa.ExportParameters(true);
            _KeyExponent = BytesToHexString(_parameter.Exponent);
            _KeyModulus = BytesToHexString(_parameter.Modulus);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="content">内容建议使用System.Net.WebUtility.UrlEncode编码一次，避免中文乱码</param>
        /// <param name="exponent"></param>
        /// <param name="modulus"></param>
        /// <returns></returns>
        public static string EncryptByKey(string content , byte[] exponent,byte[] modulus)
        {
            RSAParameters rp = new RSAParameters();
            rp.Exponent = exponent;
            rp.Modulus = modulus;

            var rsa = System.Security.Cryptography.RSA.Create();
            rsa.ImportParameters(rp);

            if (content.Length <= 110)
            {
                var data = rsa.Encrypt(System.Text.Encoding.ASCII.GetBytes(content), System.Security.Cryptography.RSAEncryptionPadding.Pkcs1);
                return BytesToHexString(data);
            }
            else
            {
                var result = new StringBuilder();
                var total = content.Length;
                for (var i = 0; i < content.Length; i += 110)
                {
                    var text = content.Substring(i, Math.Min(110, total));
                    total -= text.Length;
                    var data = rsa.Encrypt(System.Text.Encoding.ASCII.GetBytes(content), System.Security.Cryptography.RSAEncryptionPadding.Pkcs1);
                    result.Append( BytesToHexString(data));
                }
                return result.ToString();
            }

          
        }

      
        private static int GetIntegerSize(BinaryReader binr)
        {
            byte bt = 0;
            byte lowbyte = 0x00;
            byte highbyte = 0x00;
            int count = 0;
            bt = binr.ReadByte();
            if (bt != 0x02)		//expect integer
                return 0;
            bt = binr.ReadByte();

            if (bt == 0x81)
                count = binr.ReadByte();	// data size in next byte
            else
                if (bt == 0x82)
            {
                highbyte = binr.ReadByte(); // data size in next 2 bytes
                lowbyte = binr.ReadByte();
                byte[] modint = { lowbyte, highbyte, 0x00, 0x00 };
                count = BitConverter.ToInt32(modint, 0);
            }
            else
            {
                count = bt;     // we already have the data size
            }

            while (binr.ReadByte() == 0x00)
            {	//remove high order zeros in data
                count -= 1;
            }
            binr.BaseStream.Seek(-1, SeekOrigin.Current);		//last ReadByte wasn't a removed zero, so back up a byte
            return count;
        }
        static RSAParameters getRSAPrivateKey(byte[] privkey)
        {
            byte[] MODULUS, E, D, P, Q, DP, DQ, IQ;

            // --------- Set up stream to decode the asn.1 encoded RSA private key ------
            MemoryStream mem = new MemoryStream(privkey);
            BinaryReader binr = new BinaryReader(mem);  //wrap Memory Stream with BinaryReader for easy reading
            byte bt = 0;
            ushort twobytes = 0;
            int elems = 0;
            try
            {
                twobytes = binr.ReadUInt16();
                if (twobytes == 0x8130) //data read as little endian order (actual data order for Sequence is 30 81)
                    binr.ReadByte();    //advance 1 byte
                else if (twobytes == 0x8230)
                    binr.ReadInt16();    //advance 2 bytes
                else
                    throw new Exception("转换私钥失败");

                twobytes = binr.ReadUInt16();
                if (twobytes != 0x0102) //version number
                    throw new Exception("转换私钥失败");
                bt = binr.ReadByte();
                if (bt != 0x00)
                    throw new Exception("转换私钥失败");


                //------ all private key components are Integer sequences ----
                elems = GetIntegerSize(binr);
                MODULUS = binr.ReadBytes(elems);

                elems = GetIntegerSize(binr);
                E = binr.ReadBytes(elems);

                elems = GetIntegerSize(binr);
                D = binr.ReadBytes(elems);

                elems = GetIntegerSize(binr);
                P = binr.ReadBytes(elems);

                elems = GetIntegerSize(binr);
                Q = binr.ReadBytes(elems);

                elems = GetIntegerSize(binr);
                DP = binr.ReadBytes(elems);

                elems = GetIntegerSize(binr);
                DQ = binr.ReadBytes(elems);

                elems = GetIntegerSize(binr);
                IQ = binr.ReadBytes(elems);
                
                RSAParameters RSAparams = new RSAParameters();
                RSAparams.Modulus = MODULUS;
                RSAparams.Exponent = E;
                RSAparams.D = D;
                RSAparams.P = P;
                RSAparams.Q = Q;
                RSAparams.DP = DP;
                RSAparams.DQ = DQ;
                RSAparams.InverseQ = IQ;
                return RSAparams;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                binr.Dispose();
            }
        }

        /// <summary>
        /// 解开Exponent加密的内容
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        public string Decrypt(string content)
        {
            StringBuilder result = new StringBuilder();
            for (int i = 0; i < content.Length; i += 256)
            {
                byte[] bs = null;
                try
                {
                    bs = _rsa.Decrypt(HexStringToBytes(content, i, 256), System.Security.Cryptography.RSAEncryptionPadding.Pkcs1);
                }
                catch
                {
                    throw new RSADecrptException();
                }
                string str = System.Text.Encoding.ASCII.GetString(bs);
                result.Append(str);
            }

            return result.ToString();
        }

        /// <summary>
        /// 利用D进行加密
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public string EncryptByD(string data)
        {
            BigInteger d = new BigInteger(_parameter.D);
            BigInteger n = new BigInteger(_parameter.Modulus);

            StringBuilder result = new StringBuilder();
            for (int j = 0; j < data.Length; j += 110)
            {
                string content = data.Substring(j, Math.Min(110, data.Length - j));
                byte[] source = System.Text.Encoding.ASCII.GetBytes(content);

                BigInteger biText = new BigInteger(source);
                BigInteger biEnText = biText.modPow(d, n);

                byte[] b = biEnText.getBytes();
                result.Append(BytesToHexString(b));
             }
            return result.ToString();
        }
        /// <summary>
        /// 解开D加密的内容
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public string DecryptContentFromDEncrypt(string data)
        {
            BigInteger e = new BigInteger(_parameter.Exponent);
            BigInteger n = new BigInteger(_parameter.Modulus);

            StringBuilder result = new StringBuilder();
            for (int j = 0; j < data.Length; j += 256)
            {
                byte[] source = HexStringToBytes(data, j, 256);
                BigInteger biText = new BigInteger(source);
                BigInteger biEnText = biText.modPow(e, n);

                byte[] b = biEnText.getBytes();
                result.Append(System.Text.Encoding.ASCII.GetString(b));
            }

            return result.ToString();


        }
        public static byte[] HexStringToBytes(string hex)
        {
            return HexStringToBytes(hex ,0 , hex.Length);
        }
        static byte[] HexStringToBytes(string hex, int index, int len)
        {
            if (len == 0)
            {
                return new byte[0];
            }



            byte[] result = new byte[len / 2];
            int myindex = 0;
            int endindex = index + len;
            for (int i = index; i < endindex; i += 2)
            {
                result[myindex] = byte.Parse(hex.Substring(i, 2), System.Globalization.NumberStyles.AllowHexSpecifier);
                myindex++;
            }

            return result;
        }
        public static string BytesToHexString(byte[] input)
        {
            StringBuilder hexString = new StringBuilder(64);

            for (int i = 0; i < input.Length; i++)
            {
                hexString.Append(String.Format("{0:X2}", input[i]));
            }
            return hexString.ToString();
        }
    }
}
