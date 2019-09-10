using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;
using Way.Lib;
using System.IO;
using System.Security.Cryptography.X509Certificates;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Math;
using System.Text;
using Org.BouncyCastle.Crypto.Engines;
using Org.BouncyCastle.Security;
using Org.BouncyCastle.Utilities.IO.Pem;
using Org.BouncyCastle.Asn1.X509;
using Org.BouncyCastle.Asn1.Pkcs;
using Org.BouncyCastle.Asn1;
using System.Net;

namespace UnitTest
{
    [TestClass]
    public class WayLib
    {
        class DYModel : DynamicModel
        {
            int getValue()
            {
                dynamic self = this;
                return self.age;
            }
        }
      

        const string PUBLICKEY =
                @"-----BEGIN PUBLIC KEY-----
                MIGfMA0GCSqGSIb3DQEBAQUAA4GNADCBiQKBgQCLlEKOroTGplfSSmP9emm5da62NSR/QRoqPn0e/t+GVdevyRs3/J7oOpkGdgkW6s/65LIB+y+HwzeNnVvrt2y831Kdbi4nxlpvmxf1BFHJSdXHNtJ+2nfKvRRBj8+DtEu9jelQbSYp23qC+NwnpXdOyQqtKUdSsEdmL6iKc0ofsQIDAQAB
                -----END PUBLIC KEY-----";
        const string PRIVATEKEY =
                @"-----BEGIN RSA PRIVATE KEY-----
                MIICXQIBAAKBgQDpsDr+W45aFHIkvotZaGK/THlFFpuZfUtghhWkHAm3H7yvL42J
                4xHrTr6IeUDCl4eKe6qiIgvYSNoL3u4SERGOeYmV1F+cocu9IMGnNoicbh1zVW6e
                8/iGT3xaYQizJoVuWA/TC/zdds2ihCJfHDBDsouOCXecPapyWCGQNsH5sQIDAQAB
                AoGBAM/JbFs4y5WbMncrmjpQj+UrOXVOCeLrvrc/4kQ+zgCvTpWywbaGWiuRo+cz
                cXrVQ6bGGU362e9hr8f4XFViKemDL4SmJbgSDa1K71i+/LnnzF6sjiDBFQ/jA9SK
                4PYrY7a3IkeBQnJmknanykugyQ1xmCjbuh556fOeRPaHnhx1AkEA/flrxJSy1Z+n
                Y1RPgDOeDqyG6MhwU1Jl0yJ1sw3Or4qGRXhjTeGsCrKqV0/ajqdkDEM7FNkqnmsB
                +vPd116J6wJBAOuNY3oOWvy2fQ32mj6XV+S2vcG1osEUaEuWvEgkGqJ9co6100Qp
                j15036AQEEDqbjdqS0ShfeRSwevTJZIap9MCQCeMGDDjKrnDA5CfB0YiQ4FrchJ7
                a6o90WdAHW3FP6LsAh59MZFmC6Ea0xWHdLPz8stKCMAlVNKYPRWztZ6ctQMCQQC8
                iWbeAy+ApvBhhMjg4HJRdpNbwO6MbLEuD3CUrZFEDfTrlU2MeVdv20xC6ZiY3Qtq
                /4FPZZNGdZcSEuc3km5RAkApGkZmWetNwDJMcUJbSBrQMFfrQObqMPBPe+gEniQq
                Ttwu1OULHlmUg9eW31wRI2uiXcFCJMHuro6iOQ1VJ4Qs
                -----END RSA PRIVATE KEY-----";

        [TestMethod]
        public void 私钥加密()
        {

            var priKey = Way.Lib.RSA.GetPrivateKey("MIICXAIBAAKBgQCv19cfVLpis7dRFQUP4i26W7k7siN4Rk+YY34UZ7OwZ2wHG7GkxZosLXdDdHl4Tww0sdPEXf0PeIsIFN/wEEUzxdaNydo9H/FvwjYEwyTYt6Ob0PXiDqk/FnkyNx9CJRtnDYHFbavTjILAzavKAVimASjabTKBwclUNNf5nhsyBQIDAQABAoGAcY1PZPMg/XYSjjCluTEU2IA86NjLYPL+mWi+VUz2U5clwp1WpRHZ0md12cCQZGmfdzPSjb8oGOJ93bUlO3A2TvuPfXrrJuqSLzCeEyfyrsPEZ13dZCL7LGZ9kU23odAD65AoE6uWR8PFS/MhaM66rUY2N8SIkv6XKfdf0V93ueECQQDUvnCMTBZnow0HA7bYPoO9HCpQ7WVR+bsgCH3ZUIBlbtKf5cfYOK7RXQAQWeUn6u1HHuucJ1bKcsWGq78ff6RdAkEA05isav9lTHCQYmCZSb46kOHVNdfzNGAbnP/1V9f9jrDe//YPslDhOUyTvQcB80RbxClvrIxDNadt35OI8s1pyQJBAMUamA30JMHqOBSqpUoeSVH5eV83Qys7E9ru4yJnSj4v+ia47nnuslE5N+juULi2GRZOmH45mFjDEyzdjJqzWOUCQEkm0gzXqKypibEJFlWBN3wZJv3LX6Auzb0UXDx3RoiLKz0wUzLhdUu65qSGBK2WZ2dEr//mKeIltP2DYugWDckCQFYnYBNnqX6K31fsnbcbGIRaNr5DXUhu2wE3q8uIY1acXul+QJoYsdhSjjTATa0cYzOWf3Blv5P7LVQYFfyCF+U=", RSAKeyType.PKCS1);

            var 需要加密的内容 = "b=8XL3BEL8EWYX6XVP2222&a=49.88&u=周巍lang&p=+8618610032557";
            byte[] outBytes = Way.Lib.RSA.EncryptByPrivateKey(Encoding.UTF8.GetBytes(需要加密的内容), priKey);
            var base64fff = Convert.ToBase64String(outBytes);

            //公钥解密
            var 需要解密的内容 = "VmgQKL9DDu26HS594F6uLxOh8zr2LM0HxcFSVoWHdbeDYK/ixnCa1XFCM+/gtgOPJh+wMAdIMeSVg2cYJJ3u9ubr0yFrddwkrELkEQoPlX70ncwI4Fj7oaEOFMEO5AIq3o++IcI5vz238W54AHiZ3ONpW/N88fma4nNYesA2hrg=";
            var pubkey = RSA.GetPublicKey("MIGfMA0GCSqGSIb3DQEBAQUAA4GNADCBiQKBgQCLlEKOroTGplfSSmP9emm5da62NSR/QRoqPn0e/t+GVdevyRs3/J7oOpkGdgkW6s/65LIB+y+HwzeNnVvrt2y831Kdbi4nxlpvmxf1BFHJSdXHNtJ+2nfKvRRBj8+DtEu9jelQbSYp23qC+NwnpXdOyQqtKUdSsEdmL6iKc0ofsQIDAQAB");
            outBytes = RSA.DecryptByPublicKey(Convert.FromBase64String(需要解密的内容), pubkey);
            var result = Encoding.UTF8.GetString(outBytes);
        }

        [TestMethod]
        public void DynamicModel_test()
        {
            HttpClient client = new HttpClient();
           var str =  HttpClient.PostQueryString("http://incapp.incmarkets.com/app_INC_Trader/download/ipa.en_US.html?t=66666", "", 8000);
            DateTime dateStart = new DateTime(1970, 1, 1);
            var timeStamp = (new DateTime(2019,8,14) - dateStart).TotalMilliseconds ;
            /*
             :1566604800000
对比数据：    1566691200000
             */

            var dd22 = 1566604800000.0 / (1000 * 60 * 60 * 24.0);

            DateTime dateTimeStart =  new DateTime(1970, 1, 1);
            var t = 1566664853019 + 8 * 60 * 60 * 1000;
            long lTime = long.Parse(t + "0000");
            TimeSpan toNow = new TimeSpan(lTime);
            var dd = dateTimeStart.Add(toNow);

            dynamic model = new DYModel();
            model.age = 2;
           var testv =  model.getValue;
            model.age = 3;
         
        }



        [TestMethod]
        public void DataModel_Rollback()
        {
            Model m = new Model();
           var originalJson =  Newtonsoft.Json.JsonConvert.SerializeObject(m);
            m.Name = "abc";
            m.Rollback();
            if (originalJson != Newtonsoft.Json.JsonConvert.SerializeObject(m))
                throw new Exception("error");
        }

        public static AsyncLocal<string> ThreadLocalResources = new AsyncLocal<string>();
        public static int ThreadLocal_Test_count;
        [TestMethod]
        public void ThreadLocal_Test()
        {
            ThreadLocal_Test_count = 0;
            for (int i = 0; i < 100; i ++)
            Task.Run(() => {
                if(ThreadLocalResources.Value == null)
                {
                    string guid = Guid.NewGuid().ToString();
                    ThreadLocalResources.Value = guid;
                    Interlocked.Increment(ref ThreadLocal_Test_count);

                    new TestClass().test(guid);
                }
                else
                {
                    
                }
            });
            Thread.Sleep(2000);
            if (ThreadLocal_Test_count != 100)
                throw new Exception("结果错误");
        }

        [TestMethod]
        public void TaskTest()
        {
           var dict = new ConcurrentDictionary<int, TestClass>();
            for (int i = 0; i < 100; i++)
            {
                Task.Run(() => {
                    int threadid = Thread.CurrentThread.ManagedThreadId; 
                    if(dict.ContainsKey(threadid) == false)
                    {
                        dict.TryAdd(threadid, new TestClass());
                    }
                    var obj = dict[threadid];
                    if(obj.Value % 10 != 0)
                    {
                        throw new Exception("error");
                    }
                    for(int j = 0; j < 1000; j ++)
                    {
                        //这里不用Interlock也没有问题，因为多个Task，如果属于同一个线程，那么，它们是一个执行完，再到另一个，不会有同时执行的情况
                        obj.Value++;
                    }
                });
            }

            Thread.Sleep(3000);
            int total = 0;
            foreach( var key in dict )
            {
                total += key.Value.Value;
            }
            if(total != 100*1000)
                throw new Exception("error");
        }

    }

 

    class TestClass
    {
        public int Value = 0;
        public void test(string guid)
        {
            new Thread(() => {
                if (WayLib.ThreadLocalResources.Value != guid)
                {
                    Interlocked.Increment(ref WayLib.ThreadLocal_Test_count);
                }
            }).Start();
            
        }
    }

    class Model : Way.Lib.DataModel
    {

        string _Name;
        public string Name
        {
            get
            {
                return _Name;
            }
            set
            {
                if (_Name != value)
                {
                    _Name = value;
                    this.OnPropertyChanged("Name", null, value);
                }
            }
        }
    }
}
