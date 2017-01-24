using System;
using System.Collections;
using System.Collections.Generic;
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
        RSACryptoServiceProvider _rsa;
        RSAParameters _parameter;
        string _PublicKeyExponent;
        public string PublicKeyExponent {
            get { return _PublicKeyExponent; }
        }

        string _publicKeyModulus;
        public string PublicKeyModulus {
            get { return _publicKeyModulus; }
        }
        public RSA()
        {
            _rsa = new RSACryptoServiceProvider();

            //把公钥适当转换，准备发往客户端
            _parameter = _rsa.ExportParameters(true);
            _PublicKeyExponent = BytesToHexString(_parameter.Exponent);
            _publicKeyModulus = BytesToHexString(_parameter.Modulus);
        }
        /// <summary>
        /// 公钥加密
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        public string Decrypt(  string content)
        {
            StringBuilder result = new StringBuilder();
            for (int i = 0; i < content.Length; i += 256)
            {
                byte[] bs = null;
                try
                {
                    bs = _rsa.Decrypt(HexStringToBytes(content, i, 256), false);
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
        /// 利用私钥加密
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public string Encrypt2(string data)
        {
            StringBuilder result = new StringBuilder();
            for (int j = 0; j < data.Length; j += 110)
            {
                string content = data.Substring(j , Math.Min(110, data.Length - j) );
                byte[] source = System.Text.Encoding.ASCII.GetBytes(content);
                BigInteger d = new BigInteger(_parameter.D);
                BigInteger n = new BigInteger(_parameter.Modulus);
                int sug = 127;
                int len = source.Length;
                int cycle = 0;
                if ((len % sug) == 0) cycle = len / sug; else cycle = len / sug + 1;

                List<byte> temp = new List<byte>();
                int blockLen = 0;
                for (int i = 0; i < cycle; i++)
                {
                    if (len >= sug) blockLen = sug; else blockLen = len;

                    byte[] context = new byte[blockLen];
                    int po = i * sug;
                    Array.Copy(source, po, context, 0, blockLen);

                    BigInteger biText = new BigInteger(context);
                    BigInteger biEnText = biText.modPow(d, n);

                    byte[] b = biEnText.getBytes();
                    temp.AddRange(b);
                    len -= blockLen;
                }
                result.Append( BytesToHexString(temp.ToArray()));
            }
            return result.ToString();
        }
        /// <summary>
        /// 解开私钥加密的内容
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public string Decrypt2(string data)
        {
            StringBuilder result = new StringBuilder();
            for (int j = 0; j < data.Length; j += 256)
            {
                byte[] source = HexStringToBytes(data, j, 256);
                BigInteger e = new BigInteger(_parameter.Exponent);
                BigInteger n = new BigInteger(_parameter.Modulus);

                int bk = 128;
                int len = source.Length;
                int cycle = 0;
                if ((len % bk) == 0) cycle = len / bk; else cycle = len / bk + 1;

                List<byte> temp = new List<byte>();
                int blockLen = 0;
                for (int i = 0; i < cycle; i++)
                {
                    if (len >= bk) blockLen = bk; else blockLen = len;

                    byte[] context = new byte[blockLen];
                    int po = i * bk;
                    Array.Copy(source, po, context, 0, blockLen);

                    BigInteger biText = new BigInteger(context);
                    BigInteger biEnText = biText.modPow(e, n);

                    byte[] b = biEnText.getBytes();
                    temp.AddRange(b);
                    len -= blockLen;
                }
                result.Append( System.Text.Encoding.ASCII.GetString(temp.ToArray()));
            }

            return result.ToString();

            
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
        private static string BytesToHexString(byte[] input)
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
