using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace AppLib
{
    public class RSA
    {
        private static void EncodeIntegerBigEndian(BinaryWriter stream, byte[] value, bool forceUnsigned = true)
        {
            stream.Write((byte)0x02); // INTEGER
            var prefixZeros = 0;
            for (var i = 0; i < value.Length; i++)
            {
                if (value[i] != 0) break;
                prefixZeros++;
            }
            if (value.Length - prefixZeros == 0)
            {
                EncodeLength(stream, 1);
                stream.Write((byte)0);
            }
            else
            {
                if (forceUnsigned && value[prefixZeros] > 0x7f)
                {
                    // Add a prefix zero to force unsigned if the MSB is 1
                    EncodeLength(stream, value.Length - prefixZeros + 1);
                    stream.Write((byte)0);
                }
                else
                {
                    EncodeLength(stream, value.Length - prefixZeros);
                }
                for (var i = prefixZeros; i < value.Length; i++)
                {
                    stream.Write(value[i]);
                }
            }
        }

        private static void EncodeLength(BinaryWriter stream, int length)
        {
            if (length < 0) throw new ArgumentOutOfRangeException("length", "Length must be non-negative");
            if (length < 0x80)
            {
                // Short form
                stream.Write((byte)length);
            }
            else
            {
                // Long form
                var temp = length;
                var bytesRequired = 0;
                while (temp > 0)
                {
                    temp >>= 8;
                    bytesRequired++;
                }
                stream.Write((byte)(bytesRequired | 0x80));
                for (var i = bytesRequired - 1; i >= 0; i--)
                {
                    stream.Write((byte)(length >> (8 * i) & 0xff));
                }
            }
        }
        public static String ExportPublicKeyToPEMFormat(RSACryptoServiceProvider csp)
        {
            TextWriter outputStream = new StringWriter();

            var parameters = csp.ExportParameters(false);
            using (var stream = new MemoryStream())
            {
                var writer = new BinaryWriter(stream);
                writer.Write((byte)0x30); // SEQUENCE
                using (var innerStream = new MemoryStream())
                {
                    var innerWriter = new BinaryWriter(innerStream);
                    EncodeIntegerBigEndian(innerWriter, new byte[] { 0x00 }); // Version
                    EncodeIntegerBigEndian(innerWriter, parameters.Modulus);
                    EncodeIntegerBigEndian(innerWriter, parameters.Exponent);

                    //All Parameter Must Have Value so Set Other Parameter Value Whit Invalid Data  (for keeping Key Structure  use "parameters.Exponent" value for invalid data)
                    EncodeIntegerBigEndian(innerWriter, parameters.Exponent); // instead of parameters.D
                    EncodeIntegerBigEndian(innerWriter, parameters.Exponent); // instead of parameters.P
                    EncodeIntegerBigEndian(innerWriter, parameters.Exponent); // instead of parameters.Q
                    EncodeIntegerBigEndian(innerWriter, parameters.Exponent); // instead of parameters.DP
                    EncodeIntegerBigEndian(innerWriter, parameters.Exponent); // instead of parameters.DQ
                    EncodeIntegerBigEndian(innerWriter, parameters.Exponent); // instead of parameters.InverseQ

                    var length = (int)innerStream.Length;
                    EncodeLength(writer, length);
                    writer.Write(innerStream.GetBuffer(), 0, length);
                }

                var base64 = Convert.ToBase64String(stream.GetBuffer(), 0, (int)stream.Length).ToCharArray();
                outputStream.WriteLine("-----BEGIN PUBLIC KEY-----");
                // Output as Base64 with lines chopped at 64 characters
                for (var i = 0; i < base64.Length; i += 64)
                {
                    outputStream.WriteLine(base64, i, Math.Min(64, base64.Length - i));
                }
                outputStream.WriteLine("-----END PUBLIC KEY-----");

                return outputStream.ToString();

            }
        }

        /// <summary>
        /// 分段加密
        /// </summary>
        /// <param name="pfxFilePath"></param>
        /// <param name="password"></param>
        /// <param name="content"></param>
        /// <returns></returns>
        public static string BlockEncrypt(string pfxFilePath,string password,string content)
        {
            string base64 = Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(content));
            StringBuilder result = new StringBuilder();
            int totolLen = base64.Length;
            int index = 0;
            while (totolLen > 0)
            {
                int len = Math.Min(32, totolLen);
                string tocode = base64.Substring(index, len);
                index += len;
                totolLen -= len;
                byte[] codeData = AppLib.RSA.EncryptByPfx(pfxFilePath, password, tocode);
                result.Append(bin2hex(codeData));
            }
            return result.ToString();
        }

        /// <summary>
        /// 分段解密
        /// </summary>
        /// <param name="pfxFilePath"></param>
        /// <param name="password"></param>
        /// <param name="content"></param>
        /// <returns></returns>
        public static string BlockDecrypt(string pfxFilePath, string password, string content)
        {
            StringBuilder result = new StringBuilder();
            int totolLen = content.Length;
            int index = 0;
            while (totolLen > 0)
            {
                string tocode = content.Substring(index, 256);
                index += 256;
                totolLen -= 256;
                byte[] data = hex2bin(tocode);

                string decrypt = AppLib.RSA.DecryptByPfx(pfxFilePath, password, data);
                result.Append(decrypt);
            }

            return System.Text.Encoding.UTF8.GetString(Convert.FromBase64String(result.ToString()));
        }

        static byte[] hex2bin(string content)
        {
            byte[] result = new byte[content.Length / 2];
            for (int i = 0; i < content.Length; i+=2)
            {
                string hex = content.Substring(i, 2);
                result[i / 2] =  (byte)Convert.ToInt32( hex , 16 );
            }
            return result;
        }
        static string bin2hex(byte[] data)
        {
            StringBuilder str = new StringBuilder();
            for (int i = 0; i < data.Length; i++)
            {
                str.Append(data[i].ToString("X2"));
            }
            return str.ToString();
        }
        public static byte[] EncryptByPfx(string pfxFilePath,string password,string data)
        {
            byte[] dataToEncrypt = System.Text.Encoding.UTF8.GetBytes(data);
            byte[] encryptedData;


            X509Certificate2 pubcrt = new X509Certificate2(pfxFilePath, password, X509KeyStorageFlags.Exportable);
            RSACryptoServiceProvider pubkey = (RSACryptoServiceProvider)pubcrt.PrivateKey;
           
            encryptedData = RSAEncrypt(dataToEncrypt, pubkey.ExportParameters(true), false);

            pubkey.Clear();
            return encryptedData;
        }

        public static string DecryptByPfx(string pfxFilePath, string password, byte[] data)
        {

            byte[] decryptedData;

            X509Certificate2 pubcrt = new X509Certificate2(pfxFilePath);
            RSACryptoServiceProvider prvkey = (RSACryptoServiceProvider)pubcrt.PublicKey.Key;

            decryptedData = RSADecrypt(data, prvkey.ExportParameters(false), false);

            prvkey.Clear();
            return System.Text.Encoding.UTF8.GetString(decryptedData);
        }

        static byte[] RSADecrypt(byte[] DataToDecrypt, RSAParameters RSAKeyInfo, bool DoOAEPPadding)
        {
            byte[] decryptedData;
            //Create a new instance of RSACryptoServiceProvider.
            using (RSACryptoServiceProvider RSA = new RSACryptoServiceProvider())
            {
                //Import the RSA Key information. This needs
                //to include the private key information.
                RSA.ImportParameters(RSAKeyInfo);

                //Decrypt the passed byte array and specify OAEP padding.  
                //OAEP padding is only available on Microsoft Windows XP or
                //later.  
                decryptedData = RSA.Decrypt(DataToDecrypt, DoOAEPPadding);
            }
            return decryptedData;

        }
        static byte[] RSAEncrypt(byte[] DataToEncrypt, RSAParameters RSAKeyInfo, bool DoOAEPPadding)
        {
            byte[] encryptedData;
            //Create a new instance of RSACryptoServiceProvider.
            using (RSACryptoServiceProvider RSA = new RSACryptoServiceProvider())
            {
                //Import the RSA Key information. This only needs
                //toinclude the public key information.
                RSA.ImportParameters(RSAKeyInfo);

                //Encrypt the passed byte array and specify OAEP padding.  
                //OAEP padding is only available on Microsoft Windows XP or
                //later.  
                encryptedData = RSA.Encrypt(DataToEncrypt, DoOAEPPadding);
            }
            return encryptedData;

        }
    }
}
