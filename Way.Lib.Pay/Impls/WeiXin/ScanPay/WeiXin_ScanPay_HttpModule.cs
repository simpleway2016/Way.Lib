
using Way.Lib.Pay.Alipay.ScanPay;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using WxPayAPI;

namespace Way.Lib.Pay.WeiXin
{
    class WeiXin_ScanPay_HttpModule : IHttpModule
    {
        public const string NotifyPageName = "WeiXin_ScanPay_Notify";

      
        HttpResponse Response { get; set; }
        HttpRequest Request { get; set; }

        public void Dispose()
        {
           
        }

        public void Init(HttpApplication context)
        {
            context.BeginRequest += Context_BeginRequest;
        }

        private void Context_BeginRequest(object sender, EventArgs e)
        {
            Response = HttpContext.Current.Response;
            Request = HttpContext.Current.Request;

            if (Request.Url.AbsolutePath.EndsWith(NotifyPageName))
            {
                //notify url
                try
                {
                    handleNotify();

                }
                catch (Exception ex)
                {
                    using (CLog log = new CLog("weixin scan handleNotify error "))
                    {
                        log.Log(ex.ToString());
                    }
                }
                Response.End();
            }
           

        }

        private void handleNotify()
        {
            using (CLog log = new CLog("weixin scan handleNotify "))
            {
                WxPayData notifyData = GetNotifyData();
                string json = notifyData.ToJson();
                log.Log( "xml:{0}" , json);

                string out_trade_no = notifyData.GetValue("out_trade_no").ToString();

                PayFactory.OnLog(out_trade_no, json);

                WxPayConfig config = new WxPayAPI.WxPayConfig( PayFactory.GetInterfaceXmlConfig(PayInterfaceType.WeiXinScanQRCode,out_trade_no));

                try
                {
                    notifyData.CheckSign(config);
                }
                catch (WxPayException ex)
                {
                    log.Log("签名错误");
                    //若签名错误，则立即返回结果给微信支付后台
                    WxPayData res = new WxPayData();
                    res.SetValue("return_code", "FAIL");
                    res.SetValue("return_msg", ex.Message);

                    Response.Write(res.ToXml());
                    Response.End();
                }

                string result_code = notifyData.GetValue("result_code").ToString();
                string return_code = notifyData.GetValue("return_code").ToString();
                //out_trade_no  result_code  return_code
                if(result_code == "SUCCESS" && return_code == "SUCCESS")
                {
                    PayFactory.OnPaySuccessed(out_trade_no, json);

                    WxPayData data = new WxPayData();
                    data.SetValue("return_code", "SUCCESS");
                    data.SetValue("return_msg", "OK");
                    data.SetValue("appid", config.APPID);
                    data.SetValue("mch_id", config.MCHID);
                    data.SetValue("result_code", "SUCCESS");
                    data.SetValue("err_code_des", "OK");
                    data.SetValue("sign", data.MakeSign(config));

                    //log.Log("write to weixin:{0}", data.ToJson());

                    Response.Write(data.ToXml());
                }
            }
        }

        public WxPayData GetNotifyData()
        {
            //接收从微信后台POST过来的数据
            System.IO.Stream s = Request.InputStream;
            int count = 0;
            byte[] buffer = new byte[1024];
            StringBuilder builder = new StringBuilder();
            while ((count = s.Read(buffer, 0, 1024)) > 0)
            {
                builder.Append(Encoding.UTF8.GetString(buffer, 0, count));
            }
            s.Flush();
            s.Close();
            s.Dispose();

          
            //转换数据格式并验证签名
            WxPayData data = new WxPayData();
            data.FromXml(builder.ToString());
           
            
            return data;
        }
    }
}
