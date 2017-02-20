
using Way.Lib.Pay.Alipay.ScanPay;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Way.Lib.Pay.Alipay
{
    class Alipay_ScanPay_HttpModule : IHttpModule
    {
        public const string NotifyPageName = "Alipay_ScanPay_Notify";

      
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
                    using (CLog log = new CLog("alipay scan handleNotify error "))
                    {
                        log.Log(ex.ToString());
                    }
                }
                Response.End();
            }
           

        }

        private void handleNotify()
        {
            using (CLog log = new CLog("alipay scan handleNotify "))
            {
                log.Log(Request.Form.ToJson());

                SortedDictionary<string, string> sPara = GetRequestPost();

                if (sPara.Count > 0)//判断是否有带返回参数
                {
                    //商户订单号
                    string out_trade_no = Request.Form["out_trade_no"];
                    var config = new Config(PayFactory.GetInterfaceXmlConfig( PayInterfaceType.AlipayScanQRCode , out_trade_no));
                    Notify aliNotify = new Notify(config.AppConfig.charset, config.AppConfig.sign_type, config.AppConfig.pid,
                        config.AppConfig.mapiUrl, config.AppConfig.alipay_public_key);

                    ////对异步通知进行延签
                    bool verifyResult = aliNotify.Verify(sPara, Request.Form["notify_id"], Request.Form["sign"]);


                    log.Log("verifyResult:{0}" , verifyResult);
                    if (verifyResult && CheckParams()) //验签成功 && 关键业务参数校验成功
                    {
                        /////////////////////////////////////////////////////////////////////////////////////////////////////////////
                        //请在这里加上商户的业务逻辑程序代码


                        //——请根据您的业务逻辑来编写程序（以下代码仅作参考）——
                        //获取支付宝的通知返回参数，可参考技术文档中服务器异步通知参数列表

                       


                        //支付宝交易号
                        string trade_no = Request.Form["trade_no"];

                        //交易状态
                        //在支付宝的业务通知中，只有交易通知状态为TRADE_SUCCESS或TRADE_FINISHED时，才是买家付款成功。
                        string trade_status = Request.Form["trade_status"];

                        log.Log(trade_status);
                        PayFactory.OnLog(out_trade_no, Request.Form.ToJson());

                        if ( trade_status == "TRADE_SUCCESS")
                        {
                            PayFactory.OnPaySuccessed(out_trade_no, Request.Form.ToJson());
                        }

                        //判断是否在商户网站中已经做过了这次通知返回的处理
                        //如果没有做过处理，那么执行商户的业务程序
                        //如果有做过处理，那么不执行商户的业务程序

                        Response.Write("success");  //请不要修改或删除

                        //——请根据您的业务逻辑来编写程序（以上代码仅作参考）——

                        /////////////////////////////////////////////////////////////////////////////////////////////////////////////
                    }
                    else//验证失败
                    {
                        log.Log("fail");
                        Response.Write("fail");
                    }
                }
                else
                {
                    log.Log("无通知参数");
                    Response.Write("无通知参数");
                }
            }
        }


        /// <summary>
        /// 对支付宝异步通知的关键参数进行校验
        /// </summary>
        /// <returns></returns>
        private bool CheckParams()
        {
            bool ret = true;

            //获得商户订单号out_trade_no
            string out_trade_no = Request.Form["out_trade_no"];
            //TODO 商户需要验证该通知数据中的out_trade_no是否为商户系统中创建的订单号，

            //获得支付总金额total_amount
            string total_amount = Request.Form["total_amount"];
            //TODO 判断total_amount是否确实为该订单的实际金额（即商户订单创建时的金额），

            //获得卖家账号seller_email
            string seller_email = Request.Form["seller_email"];
            //TODO 校验通知中的seller_email（或者seller_id) 是否为out_trade_no这笔单据的对应的操作方（有的时候，一个商户可能有多个seller_id / seller_email）

            //获得调用方的appid；
            //如果是非授权模式，appid是商户的appid；如果是授权模式（token调用），appid是系统商的appid
            string app_id = Request.Form["app_id"];
            //TODO 验证app_id是否是调用方的appid；。

            //验证上述四个参数，完全吻合则返回参数校验成功
            return ret;

        }

        /// <summary>
        /// 获取支付宝POST过来通知消息，并以“参数名=参数值”的形式组成数组
        /// </summary>
        /// <returns>request回来的信息组成的数组</returns>
        public SortedDictionary<string, string> GetRequestPost()
        {
            int i = 0;
            SortedDictionary<string, string> sArray = new SortedDictionary<string, string>();
            NameValueCollection coll;
            //Load Form variables into NameValueCollection variable.
            coll = Request.Form;

            // Get names of all forms into a string array.
            String[] requestItem = coll.AllKeys;

            for (i = 0; i < requestItem.Length; i++)
            {
                sArray.Add(requestItem[i], Request.Form[requestItem[i]]);
            }

            return sArray;
        }

    }
}
