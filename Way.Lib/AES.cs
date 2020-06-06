using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace Way.Lib
{
    public class AES
    {
        /// <summary>
        ///  加密
        /// </summary>
        /// <param name="str">明文（待加密）</param>
        /// <param name="key">密钥 16个字母</param>
        /// <returns></returns>
        public static string Encrypt(string str, string key)
        {
            if (string.IsNullOrEmpty(str)) return null;
            Byte[] toEncryptArray = Encoding.UTF8.GetBytes(str);

            using (RijndaelManaged rm = new RijndaelManaged
            {
                Key = Encoding.UTF8.GetBytes(key),
                Mode = CipherMode.ECB,
                Padding = PaddingMode.PKCS7
            })
            {
                using (ICryptoTransform cTransform = rm.CreateEncryptor())
                {
                    Byte[] resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);
                    return Convert.ToBase64String(resultArray);
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="content"></param>
        /// <param name="key">密钥 16个字母</param>
        /// <returns></returns>
        public static byte[] Encrypt(byte[] content, string key)
        {
            if (content == null) return null;

            using (RijndaelManaged rm = new RijndaelManaged
            {
                Key = Encoding.UTF8.GetBytes(key),
                Mode = CipherMode.ECB,
                Padding = PaddingMode.PKCS7
            })
            {
                using (ICryptoTransform cTransform = rm.CreateEncryptor())
                {
                    Byte[] resultArray = cTransform.TransformFinalBlock(content, 0, content.Length);
                    return resultArray;
                }
            }             
        }


        /// <summary>
        ///  解密
        /// </summary>
        /// <param name="str">明文（待解密）</param>
        /// <param name="key">密钥 16个字母</param>
        /// <returns></returns>
        public static string Decrypt(string str, string key)
        {
            if (string.IsNullOrEmpty(str)) return null;
            Byte[] toEncryptArray = Convert.FromBase64String(str);

            using (RijndaelManaged rm = new RijndaelManaged
            {
                Key = Encoding.UTF8.GetBytes(key),
                Mode = CipherMode.ECB,
                Padding = PaddingMode.PKCS7
            })
            {

                using (ICryptoTransform cTransform = rm.CreateDecryptor())
                {
                    Byte[] resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);

                    return Encoding.UTF8.GetString(resultArray);
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="content"></param>
        /// <param name="key">密钥 16个字母</param>
        /// <returns></returns>
        public static byte[] Decrypt(byte[] content, string key)
        {
            if (content == null) return null;

            using (RijndaelManaged rm = new RijndaelManaged
            {
                Key = Encoding.UTF8.GetBytes(key),
                Mode = CipherMode.ECB,
                Padding = PaddingMode.PKCS7
            })
            {
                using (ICryptoTransform cTransform = rm.CreateDecryptor())
                {
                    return cTransform.TransformFinalBlock(content, 0, content.Length);
                }
            }

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="toDecryptArray"></param>
        /// <param name="key">24个字符的密钥</param>
        /// <returns></returns>
        public static byte[] TripleDESDecrypt(byte[] toDecryptArray, string key)
        {
            using (var tripleDES = TripleDES.Create())
            {
                var byteKey = Encoding.UTF8.GetBytes(key);
                tripleDES.Key = byteKey;
                tripleDES.Mode = CipherMode.ECB;
                tripleDES.Padding = PaddingMode.PKCS7;

                using (ICryptoTransform cTransform = tripleDES.CreateDecryptor())
                {
                    return cTransform.TransformFinalBlock(toDecryptArray, 0, toDecryptArray.Length);
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="toEncryptArray"></param>
        /// <param name="key">24个字符的密钥</param>
        /// <returns></returns>
        public static byte[] TripleDESEncrypt(byte[] toEncryptArray, string key)
        {
            using (var tripleDES = TripleDES.Create())
            {
                var byteKey = Encoding.UTF8.GetBytes(key);
                tripleDES.Key = byteKey;
                tripleDES.Mode = CipherMode.ECB;
                tripleDES.Padding = PaddingMode.PKCS7;

                using (ICryptoTransform cTransform = tripleDES.CreateEncryptor())
                {
                    return cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);
                }
            }
        }

    }
}
