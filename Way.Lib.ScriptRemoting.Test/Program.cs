using System;
using System.Collections.Generic;
using System.Text;
using System.Security.Cryptography;
using System.Collections;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Way.Lib.ScriptRemoting.WinTest;
using System.IO;
using System.Security.Cryptography.X509Certificates;

namespace Way.Lib.ScriptRemoting.Test
{
    public class Program
    {
        public static void Main(string[] args)
        {
            // Console.OutputEncoding = System.Text.Encoding.Unicode;
            // Console.WriteLine("你好福娃额放假哦");
            // Console.SetCursorPosition(2, 0);
            // Console.Write("ab");//相当于backspace  

            Console.OutputEncoding = System.Text.Encoding.UTF8;
            Console.WriteLine("server starting...");
            HttpServer server = null;
            if (System.IO.Directory.Exists($"{Way.Lib.PlatformHelper.GetAppDirectory()}Web"))
            {
                
                Console.WriteLine($"path:{Way.Lib.PlatformHelper.GetAppDirectory()}Web");
                server = new HttpServer(new int[] { 10001 }, $"{Way.Lib.PlatformHelper.GetAppDirectory()}Web");
            }
            else
            {
              
                Console.WriteLine($"path:{Way.Lib.PlatformHelper.GetAppDirectory()}../../../../Way.Lib.ScriptRemoting.Test.Web");
                server = new HttpServer(new int[] { 10001 }, $"{Way.Lib.PlatformHelper.GetAppDirectory()}../../../../Way.Lib.ScriptRemoting.Test.Web");
            }
            //server.UseHttps(new X509Certificate2(Way.Lib.PlatformHelper.GetAppDirectory() + "server.pfx", "linezero") );
            Console.WriteLine("server started");
            try
            {
                //创建一个数据库对象实例，数据库第一次实例化，会检查数据库结构，会自动更新数据库结构
                using (var db = new MyDB())
                {

                }
                //到这里可以确定数据库结构已经更新了
                Console.WriteLine("database ready");
            }

            catch(Exception ex)
            {
                Console.WriteLine("发生错误");
                Console.WriteLine("错误信息：" + ex.Message);
            }

            server.Start();
            while (true)
            {
                Console.Write("Web>");
                var line = Console.ReadLine();
                if (line == null)
                {
                    //是在后台运行的
                    while (true)
                    {
                        System.Threading.Thread.Sleep(10000000);
                    }
                }
                else if (line == "exit")
                    break;
            }
            server.Stop();
            System.Diagnostics.Process.GetCurrentProcess().Kill();
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



        public class RSAPrivateKeyCrypto : IDisposable
        {
            RSAParameters paramsters;
            public RSAPrivateKeyCrypto(RSAParameters paramsters)
            {
                this.paramsters = paramsters;
            }
            public byte[] Encrypt(byte[] source)
            {
                BigInteger d = new BigInteger(paramsters.D);
                BigInteger n = new BigInteger(paramsters.Modulus);
                int sug = 127;
                int len = source.Length;
                int cycle = 0;
                if ((len % sug) == 0) cycle = len / sug; else cycle = len / sug + 1;

                ArrayList temp = new ArrayList();
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
                return (byte[])temp.ToArray(typeof(byte));
            }
            public byte[] Decrypt(byte[] source)
            {
                BigInteger e = new BigInteger(paramsters.Exponent);
                BigInteger n = new BigInteger(paramsters.Modulus);

                int bk = 128;
                int len = source.Length;
                int cycle = 0;
                if ((len % bk) == 0) cycle = len / bk; else cycle = len / bk + 1;

                ArrayList temp = new ArrayList();
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
                return (byte[])temp.ToArray(typeof(byte));
            }

            #region IDisposable 成员

            public void Dispose()
            {

            }

            #endregion
        }
    
}
