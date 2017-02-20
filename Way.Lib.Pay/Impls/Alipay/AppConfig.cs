using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Way.Lib.Pay.Alipay
{
    public class AppConfig
    {

        //public static string alipay_public_key = @"MIGfMA0GCSqGSIb3DQEBAQUAA4GNADCBiQKBgQDDI6d306Q8fIfCOaTXyiUeJHkrIvYISRcc73s3vF1ZT7XN8RNPwJxo8pWaJMmvyTn9N4HQ632qJBVHf8sxHi/fEsraprwCtzvzQETrNRwVxLO5jVmRGi60j8Ue1efIlzPXV9je9mkjzOmdssymZkh2QhUrCmZYI/FCEa3/cNMW0QIDAQAB";
        ////这里要配置没有经过的原始私钥
        //public static string merchant_private_key = @"此处请填写开发者私钥去头去尾去回车，一行字符串";
        //public static string merchant_public_key = @"此处填写开发者公钥";
        //public static string appId = "此处填写开发者应用appid";
        //public static string serverUrl = "https://openapi.alipay.com/gateway.do";
        //public static string mapiUrl = "https://mapi.alipay.com/gateway.do";


        public string alipay_public_key;
        //这里要配置没有经过PKCS8转换的原始私钥
        public string merchant_private_key;
        public string merchant_public_key;
        public  string appId;
        public  string serverUrl= "https://openapi.alipay.com/gateway.do";
        public  string mapiUrl= "https://mapi.alipay.com/gateway.do";
        public  string monitorUrl= "http://mcloudmonitor.com/gateway.do";
        public  string pid;


        public  string charset= "utf-8";//"utf-8";
        public  string sign_type= "RSA";
        public  string version = "1.0";

        public Com.Alipay.IAlipayTradeService CreateClientInstance()
        {
            return Com.Alipay.F2FBiz.CreateClientInstance(this.serverUrl, this.appId, this.merchant_private_key, this.version,
                                 this.sign_type, this.alipay_public_key, this.charset);
        }
        public Aop.Api.DefaultAopClient CreateAopClient()
        {
            return new Aop.Api.DefaultAopClient(this.serverUrl, this.appId, this.merchant_private_key, "json", this.version,
                                 this.sign_type, this.alipay_public_key, this.charset);
        }
    }
}
