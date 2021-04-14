using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace Way.Lib
{
    public class SHA
    {
        /// <summary>
        /// 对数据进行HmacSHA1计算
        /// </summary>
        /// <param name="data">需要加密的数据</param>
        /// <param name="key">密钥</param>
        /// <returns></returns>
        public static byte[] HMACSHA1(byte[] data, string key)
        {
            //HMACSHA1加密
            HMACSHA1 hmacsha1 = new HMACSHA1();
            hmacsha1.Key = System.Text.Encoding.UTF8.GetBytes(key);

           return hmacsha1.ComputeHash(data);
        }
    }
}
