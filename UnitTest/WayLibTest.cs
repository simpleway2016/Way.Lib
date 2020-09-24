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
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Diagnostics;
using System.Security.Cryptography;
using Way.Lib.Collections;
using System.Linq;

namespace UnitTest
{
    [TestClass]
    public class WayLibTest
    {
        class DYModel : DynamicModel
        {
            int getValue()
            {
                dynamic self = this;
                return self.age;
            }
        }
      

         string PUBLICKEY = @"-----BEGIN PUBLIC KEY-----
MIIBIjANBgkqhkiG9w0BAQEFAAOCAQ8AMIIBCgKCAQEAt54sj/DZNdxpRetHIGxp
jwVeBLzTXiiPpPxGSo7IxAXQItPLjO0drJUksw6Y9WaVzz8S1NYTrZvuALbD+bBG
3sWwejJUHen12C/VDcSrLjl99x5iNGp6dxtnolILEcTE1ejvTCCdvwQFNDaODuE+
W5GZMMMA6wogkAlqONhlbF91fwhbVcoqi6AiEV/znVbT+nKkH6f9I9bs0vhsGyhf
G2sWLOTdEmU8Yu7YegslL01yi0Jfz8QxsMvQHtg5aQ/UOFFdILnwuZQvaMx+a0us
mYIC+5Wwv47VPrzm6u2XHg2VOmYfp/bThdRUouCIe4SYLwrNgpSCAvdwqDeWiJai
jQIDAQAB
-----END PUBLIC KEY-----
";
         string PRIVATEKEY = @"-----BEGIN RSA PRIVATE KEY-----
MIIEowIBAAKCAQEAt54sj/DZNdxpRetHIGxpjwVeBLzTXiiPpPxGSo7IxAXQItPL
jO0drJUksw6Y9WaVzz8S1NYTrZvuALbD+bBG3sWwejJUHen12C/VDcSrLjl99x5i
NGp6dxtnolILEcTE1ejvTCCdvwQFNDaODuE+W5GZMMMA6wogkAlqONhlbF91fwhb
Vcoqi6AiEV/znVbT+nKkH6f9I9bs0vhsGyhfG2sWLOTdEmU8Yu7YegslL01yi0Jf
z8QxsMvQHtg5aQ/UOFFdILnwuZQvaMx+a0usmYIC+5Wwv47VPrzm6u2XHg2VOmYf
p/bThdRUouCIe4SYLwrNgpSCAvdwqDeWiJaijQIDAQABAoIBABL6oHSIWos8qwz1
ErcBPa/wyBUJR3e2DZLqGIHgXFQWnbrb1XBNmgGasN5pJdpHrjhrtpCVSBdrxKAC
RCNs2wZvvJwE07j709NyrjzsFR2EB8chNtlgICRriry2ajp98jKCDyn5PRTfX4JU
GT7kfUONux3VKiUhONhqgLY7d9BJqMtAWiu8ATKnjbFZJ/g72qdtsROGEngWzyjl
ZoWb+NisXaJpfx49EgJubRW7tEWG8BlQiiVopY5W48Hnm5ojP2Be9TaRV7sDJ5cx
zqZ9DaT2Gaw6Irx9HvX7JJFDd9dltszhRwFpifST4RHeXKWh6YvbbUPGbnSOirgb
7omUFwECgYEA4Spwjfc9pE1uHyXUKquPTgKZdj+D/oHk53jFMpmI5Nvja8oPcp1H
9+oxoTyfK3Y6sA1RyBIVWmbPVd82YDeYNPfSXjRQtAA3TNXkqmnAqXlXCCPL2DLa
t5zXsP40gXktuFA8cJ3l459R1dhuzC0CBLFH6FdfPMNyg9Wkk6yRJC0CgYEA0MM0
Den+PeKPa0QsRg68AEnrJh7Bbz7OwIHcLMUs86+S6nN+qghrLy0QYB/WxjRNuovS
rqUDTnJBVICoXgCbxOggdQCSHcwS8SXv/zp34nkjjHqtY+a6fEvguwLeFBmEGPsX
K5MAjzmoTNcDNq0LHDrVXuNDsrsPWFpMA6JAk+ECgYA24qhzAoxlC3bHYuo3yH/z
DREpUMw08qbAgaDX97L1zl++O4/OaWZMmSql2egWqQgfN+/ya+4Gjj8F+JYBmCui
5bHCws/VQKQ6N304yPRsmyZK1qbuuV61GSc+foh/8vDhF7XT4blS5dgF328KmAIA
8NHNYdbWMfItDpvCX3M1tQKBgGg+IP8VqbuTJxRj/UuaTexC/OWTE/oNvcXI0n2U
k8a3FEB4HXagL41mDjhBjch5E5sj+Lb2dPk1+kNM30XZPe8MDjD+cPfqEEdUL0we
EQEIhmS+WVh6PQKWDQi0/NnCiADFWKOMiwn5u31rHBKwQ8z739G63/IekCIJLM3f
tilhAoGBAJwC3uXrvr6ALyU+WzMtKrK1KxL6pwcn5ze8cb0kdJc5W0LLtpho2+Nd
LdzX05BZHDh5GUxxaipRHpGbZEDRnYvbrL0VcYLzRy4QiY7aAHhJo7u5BnobXyGD
AkwHI7pU+rJUgRv4oU708GtL8nlQ09g4j+dQGvqsapSYgQWSR3sS
-----END RSA PRIVATE KEY-----
";



        [TestMethod]
        public void KeyPair()
        {
            var pair = Way.Lib.RSA.CreateKeyPair();
        }

        class ValueObject
        {
            public int Value;
            public int? ThreadId;
        }
        [TestMethod]
        public void ConcurrentDictionaryActionQueueTest()
        {
            ConcurrentDictionaryActionQueue<int> userActions = new ConcurrentDictionaryActionQueue<int>();
            for (int k = 0; k < 1000; k++)
            {
                ConcurrentDictionary<int, ValueObject> userValues = new ConcurrentDictionary<int, ValueObject>();
                int total = 10000000;
                int[] userids = new[] { 1, 2, 3, 4, 5, 6, 7 };
                int[] values = new int[userids.Length];
              
                int done = 0;
                Random random = new Random();
                
                Parallel.For(0, total, (index) =>
                {
                    var userid = random.Next(1, 8);

                    var userValue = userValues.GetOrAdd(userid, (u) => new ValueObject());
                    Interlocked.Increment(ref userValue.Value);

                    userActions.Add(userid, () =>
                    {                       
                        values[userid - 1]++;
                        Interlocked.Increment(ref done);
                    });
                });

                userActions.WaitAll();
                var sum1 = values.Sum();
                var sum2 = userValues.Sum(m => m.Value.Value);
                if (sum1 != total)
                    throw new Exception("total error");
                for (int i = 0; i < values.Length; i++)
                {
                    var value1 = values[i];
                    var value2 = userValues[userids[i]].Value;
                    if (value1 != value2)
                    {
                        throw new Exception("value error");
                    }
                }
                System.Diagnostics.Debug.WriteLine("第{0}次" , k + 1);
            }
        }


        [TestMethod]
        public void Sign()
        {
            var pair = Way.Lib.RSA.CreateKeyPair();
            var publicKey = Way.Lib.RSA.GetPublicKey(pair[0]);
            var privateKey = Way.Lib.RSA.GetPrivateKey(pair[1] , RSAKeyType.PKCS1);

            var content = "abc";

            var signed = Way.Lib.RSA.SignRSA2(content, privateKey);
            var ret = Way.Lib.RSA.VerifyRSA2Signed(content, publicKey, signed);
            if (!ret)
                throw new Exception("sign error");
        }


        [TestMethod]
        public void 私钥解密()
        {
            var rsa =  System.Security.Cryptography.RSA.Create();
            var pubkey = rsa.ExportParameters(false);
            var prikey = rsa.ExportParameters(true);

            pubkey = Way.Lib.RSA.GetPublicKey(PUBLICKEY);
            var bs = Way.Lib.RSA.EncryptByPublicKey(Encoding.UTF8.GetBytes("abc我的"), pubkey);
            var fefe = Convert.ToBase64String(bs);
           prikey = Way.Lib.RSA.GetPrivateKey(PRIVATEKEY, RSAKeyType.PKCS1);
            bs = Convert.FromBase64String("sEz2iMKQbIdczNmDcxwd6bCotgj2auVaWfHZVMt10z1xF4+RIVUQ1eVHwKcUaxt1Vv+SAbdSXGCaIEtZg0k7E78X39AWavbDJVeGJSEhm4IGkD/tqnzEuotzcwiFQLhxj9FKejVM7dOWyS/VvXz7Src1h9R5HSTk6b9fyakTeeiGIu3iVZ/hNeatqSp204oUxlcmotQL4sUwfTAzZ+ITZZlD9NhUptCv+iWqmuDq1GnmP1NlrvbKljDT37zmX6vAiuppyZ1VeqZecq63qyAvO6/Idg+f+Tesa6MFCXSDHk8O7tzRts27j+VoTVAPTXdtF6gHdid8trnDanU/uycL7Q==");
           bs= Way.Lib.RSA.DecryptByPrivateKey(bs, prikey);
           var content = Encoding.UTF8.GetString(bs);
        }

        [TestMethod]
        public void aes()
        {
           
           var bs = Convert.FromBase64String("8BFzLPk4qDE=");

           bs = Way.Lib.AES.TripleDESDecrypt(bs, "STTuU02NmZi9Ce1E76iWXdgE");
           var content = Encoding.UTF8.GetString(bs);

            bs = Way.Lib.AES.TripleDESEncrypt(Encoding.UTF8.GetBytes("abc我的"), "STTuU02NmZi9Ce1E76iWXdgE");
            content = Convert.ToBase64String(bs);
        }

        [TestMethod]
        public void 私钥加密()
        {

            RSACryptoServiceProvider rsa = new RSACryptoServiceProvider(2048);
            var privatekey = rsa.ExportParameters(true);
            var publickey = rsa.ExportParameters(false);

           
            var 需要加密的内容 = "b=8XL3BEL8EWYX6XVP2222&a=49.88&u=周巍lang&p=+8618610032557b=8XL3BEL8EWYX6XVP2222&a=49.88&u=周巍lang&p=+8618610032557b=8XL3BEL8EWYX6XVP2222&a=49.88&u=周巍lang&p=+8618610032557b=8XL3BEL8EWYX6XVP2222&a=49.88&u=周巍lang&p=+8618610032557b=8XL3BEL8EWYX6XVP2222&a=49.88&u=周巍lang&p=+8618610032557b=8XL3BEL8EWYX6XVP2222&a=49.88&u=周巍lang&p=+8618610032557b=8XL3BEL8EWYX6XVP2222&a=49.88&u=周巍lang&p=+8618610032557b=8XL3BEL8EWYX6XVP2222&a=49.88&u=周巍lang&p=+8618610032557b=8XL3BEL8EWYX6XVP2222&a=49.88&u=周巍lang&p=+8618610032557b=8XL3BEL8EWYX6XVP2222&a=49.88&u=周巍lang&p=+8618610032557b=8XL3BEL8EWYX6XVP2222&a=49.88&u=周巍lang&p=+8618610032557b=8XL3BEL8EWYX6XVP2222&a=49.88&u=周巍lang&p=+8618610032557";
            byte[] outBytes = Way.Lib.RSA.EncryptByPrivateKey(Encoding.UTF8.GetBytes(需要加密的内容), privatekey);
           

          outBytes = Way.Lib.RSA.DecryptByPublicKey(outBytes, publickey);
            var result = Encoding.UTF8.GetString(outBytes);

            outBytes = Way.Lib.RSA.EncryptByPublicKey(Encoding.UTF8.GetBytes(需要加密的内容), publickey);
            outBytes = Way.Lib.RSA.DecryptByPrivateKey(outBytes, privatekey);
            result = Encoding.UTF8.GetString(outBytes);
        }

        [TestMethod]
        public void Huawei()
        {
           var content = HttpClient.PostQueryString("https://login.cloud.huawei.com/oauth2/v2/token", new Dictionary<string, string> {
                { "grant_type", "client_credentials" },
                { "client_secret", "775705190657e09fc12bfb1a3d1759b7bff78a2b6b734b903125aecfdb27362c" },
                 { "client_id", "100946793" }
            }, 8000);

            var ret = JsonConvert.DeserializeObject<JObject>(content);
           var accessToken = ret["access_token"].ToString();
            var expires_in = DateTime.Now.AddSeconds(Convert.ToInt32(ret["expires_in"].ToString()));

            string apiUrl = "https://api.push.hicloud.com/pushsend.do";
            String postUrl = apiUrl + "?nsp_ctx=" + System.Web.HttpUtility.UrlEncode("{\"ver\":\"1\", \"appId\":\"100946793\"}",System.Text.Encoding.UTF8 );


            JArray deviceTokens = new JArray();
            deviceTokens.Add("0867926032613273300004132500CN01");

            JObject body = new JObject();//仅通知栏消息需要设置标题和内容，透传消息key和value为用户自定义
            body.Add("title", "Push message title" + DateTime.Now.Millisecond);//消息标题
            body.Add("content", "Push message content" + DateTime.Now.Millisecond);//消息内容体

            JObject param = new JObject();
            param.Add("appPkgName", "com.lr.bitcoinwin");//定义需要打开的appPkgName

            JObject action = new JObject();
            action.Add("type", 3);//类型3为打开APP，其他行为请参考接口文档设置
            action.Add("param", param);//消息点击动作参数

            JObject msg = new JObject();
            msg.Add("type", 3);//3: 通知栏消息，异步透传消息请根据接口文档设置
            msg.Add("action", action);//消息点击动作
            msg.Add("body", body);//通知栏消息body内容

            JObject customObj = new JObject();
            customObj.Add("custom", "abc");
            customObj.Add("title", "title");
            customObj.Add("body", "body");
            JArray customArr = new JArray();
            customArr.Add(customObj);

            JObject ext = new JObject();//扩展信息，含BI消息统计，特定展示风格，消息折叠。
            //ext.Add("biTag", "Trump");//设置消息标签，如果带了这个标签，会在回执中推送给CP用于检测某种类型消息的到达率和状态
            //ext.Add("icon", "http://pic.qiantucdn.com/58pic/12/38/18/13758PIC4GV.jpg");//自定义推送消息在通知栏的图标,value为一个公网可以访问的URL
            ext.Add("customize", customArr);
            ext.Add("badgeAddNum", "1");
            ext.Add("badgeClass", "com.lr.abc.activity");


            JObject hps = new JObject();//华为PUSH消息总结构体
            hps.Add("msg", msg);
            hps.Add("ext", ext);

            JObject payload = new JObject();
            payload.Add("hps", hps);

            String postBody = String.Format(
                "access_token={0}&nsp_svc={1}&nsp_ts={2}&device_token_list={3}&payload={4}",
                System.Web.HttpUtility.UrlEncode(accessToken, System.Text.Encoding.UTF8),
                System.Web.HttpUtility.UrlEncode("openpush.message.api.send", System.Text.Encoding.UTF8),

                (int)(DateTime.Now - Convert.ToDateTime("1970-01-01")).TotalSeconds,
                System.Web.HttpUtility.UrlEncode(JsonConvert.SerializeObject(deviceTokens), System.Text.Encoding.UTF8),
                System.Web.HttpUtility.UrlEncode(JsonConvert.SerializeObject(payload), System.Text.Encoding.UTF8));

            content = HttpClient.PostQueryString(postUrl, postBody, 8000);
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

        [TestMethod]
        public void testClient()
        {
            var dict = new Dictionary<string, object>() {

                        { "jsonrpc" , "2.0"},
                        { "method" , "abc"}
                    };
            var headers = new Dictionary<string, string>();


            for (int i = 0; i < 40000; i ++)
            {
                try
                {
                    var content = HttpClient.PostJson("http://localhost:8988/main.html", headers , dict , 5000);
                    //Thread.Sleep(10);
                    Debug.WriteLine(i.ToString());
                }
                catch (Exception ex) 
                {

                }
               
            }
        }

        [TestMethod]
        public void httpClient()
        {
            NetStream stream = new NetStream("localhost", 5000);
            var bs = System.Text.Encoding.UTF8.GetBytes("GET /api/values/GetName HTTP/1.1\r\nHost: localhost\r\nContent-Length: 9990\r\n\r\n");
            stream.Write(bs);
            int readed = stream.Read(bs, 0, bs.Length);
            var content = System.Text.Encoding.UTF8.GetString(bs);
        }

        [TestMethod]
        public void ConcurrentDictionaryTest()
        {

            System.Diagnostics.Stopwatch sw = new Stopwatch();

            var list = new ConcurrentDictionary<object,bool>();
            sw.Start();
            List<object> buffer = new List<object>();
            var t1 = Task.Run(() => {
                for (int i = 0; i < 10000; i++)
                {
                    var c = new object();
                    list.TryAdd(c,true);
                    buffer.Add(c);
                }
            });
            var t2 = Task.Run(() => {
                for (int i = 0; i < 10000; i++)
                {
                    list.TryAdd(new object() , true);
                }
            });

            var t3 = Task.Run(() => {
                Random random = new Random();
                for (int i = 0; i < 10000; i++)
                {
                    while (buffer.Count < 10)
                        Thread.Sleep(0);
                     list.FirstOrDefault(m => m.Key == buffer[8]);
                }
            });
            Task.WaitAll(t1, t2, t3);
            sw.Stop();
            var ms = sw.ElapsedMilliseconds;
            if (list.Count != 20000)
                throw new Exception("数量不对");
            sw.Reset();
            sw.Start();

            t1 = Task.Run(() => {
                for (int i = 0; i < 5000; i++)
                {
                    var c = buffer[i];
                    list.TryRemove(c,out bool b);
                }
            });

            t2 = Task.Run(() => {
                for (int i = 8000 - 1; i >= 0; i--)
                {
                    var c = buffer[i];
                    list.TryRemove(c, out bool b);
                }
            });

            Task.WaitAll(t1, t2);

            sw.Stop();
            ms = sw.ElapsedMilliseconds;
            if (list.Count != 12000)
                throw new Exception("数量不对");

            var item = list.FirstOrDefault(m => m.Key == buffer[9999]).Key;
            if (item == null)
                throw new Exception("item is null");
        }

        [TestMethod]
        public void ConcurrentListTest()
        {

            System.Diagnostics.Stopwatch sw = new Stopwatch();

            var list = new ConcurrentList<object>();
            sw.Start();
            List<object> buffer = new List<object>();
            var t1 = Task.Run(() => {             
                for(int i = 0; i < 10000; i ++)
                {
                    var c = new object();
                    list.Add(c);
                    buffer.Add(c);
                }
            });
            var t2 = Task.Run(() => {
                for (int i = 0; i < 10000; i++)
                {
                    list.Add(new object());
                }
            });

            var t3 = Task.Run(() => {
                Random random = new Random();
                for (int i = 0; i < 10000; i++)
                {
                    while (buffer.Count < 10)
                        Thread.Sleep(0);
                    list.FirstOrDefault(m => m == buffer[8]);
                }
            });
            Task.WaitAll(t1, t2,t3);
            var ms = sw.ElapsedMilliseconds;
            if (list.Count != 20000)
                throw new Exception("数量不对");

            sw.Reset();
            sw.Start();

            t1 = Task.Run(() => {
                for (int i = 0; i < 5000; i++)
                {
                    var c = buffer[i];
                    list.Remove(c);
                }
            });

            t2 = Task.Run(() => {
                for (int i = 8000 - 1; i >=0; i--)
                {
                    var c = buffer[i];
                    list.Remove(c);
                }
            });

            Task.WaitAll(t1, t2);

            sw.Stop();
            ms = sw.ElapsedMilliseconds;
            if (list.Count != 12000)
                throw new Exception("数量不对");

            if( list.ToArray().Length != 12000)
                throw new Exception("数量不对");

            var item = list.FirstOrDefault(m => m == buffer[9999]);
            if (item == null)
                throw new Exception("item is null");
        }

        [TestMethod]
        public void ConcurrentListTest2()
        {
            DateTime startTime = DateTime.Now;
            int addCount = 0;
            int deleteCount = 0;
            ConcurrentQueue<object> fifo = new ConcurrentQueue<object>();
            var list = new ConcurrentList<object>();

            var t1 = Task.Run(() => {
                while((DateTime.Now - startTime).TotalMinutes < 10)
                {
                    var c = new object();
                    list.Add(c);
                    Interlocked.Increment(ref addCount);
                    fifo.Enqueue(c);

                    if (list.Count > 10000)
                        Thread.Sleep(100);                    
                }
            });
            var t2 = Task.Run(() => {
                while ((DateTime.Now - startTime).TotalMinutes < 10)
                {
                    if(fifo.TryDequeue(out object c))
                    {
                        list.Remove(c);
                        Interlocked.Increment(ref deleteCount);
                    }
                }
            });

            Task.WaitAll(t1, t2);
            if (addCount - deleteCount != list.Count)
                throw new Exception("数量不对");
        }
    }

 

    class TestClass
    {
        public int Value = 0;
        public void test(string guid)
        {
            new Thread(() => {
                if (WayLibTest.ThreadLocalResources.Value != guid)
                {
                    Interlocked.Increment(ref WayLibTest.ThreadLocal_Test_count);
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
