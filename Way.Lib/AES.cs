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
    }
}
