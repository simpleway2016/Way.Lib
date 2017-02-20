
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Way.Lib.Pay.Alipay
{
    class Alipay_WebPay_HttpModule : IHttpModule
    {
        public const string NotifyPageName = "Alipay_WebPay_Notify";
        public const string ReturnPageName = "Alipay_WebPay_Return";

        public static Dictionary<string, string> ReturnUrlConfigs = new Dictionary<string, string>();
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
                    using (CLog log = new CLog("alipay handleNotify error "))
                    {
                        log.Log(ex.ToString());
                    }
                }
                Response.End();
            }
            else if (Request.Url.AbsolutePath.EndsWith(ReturnPageName))
            {
                try
                {
                    handleReturn();

                }
                catch (Exception ex)
                {
                    using (CLog log = new CLog("alipay handleReturn error "))
                    {
                        log.Log(ex.ToString());
                    }
                }
                Response.End();
            }

        }

        private void handleNotify()
        {
            using (CLog log = new CLog("alipay handleNotify "))
            {
                log.Log(Request.Form.ToJson());
                SortedDictionary<string, string> sPara = GetRequestPost();

                if (sPara.Count > 0)//判断是否有带返回参数
                {
                    string out_trade_no = Request.Form["out_trade_no"];
                    var config = new Config(PayFactory.GetInterfaceXmlConfig(PayInterfaceType.AlipayWebPay, out_trade_no));
                    Com.Alipay.Notify aliNotify = new Com.Alipay.Notify(config);
                    bool verifyResult = aliNotify.Verify(sPara, Request.Form["notify_id"], Request.Form["sign"]);

                    log.Log("verifyResult:{0}", verifyResult);
                    if (verifyResult)//验证成功
                    {
                        /////////////////////////////////////////////////////////////////////////////////////////////////////////////
                        //请在这里加上商户的业务逻辑程序代码


                        //——请根据您的业务逻辑来编写程序（以下代码仅作参考）——
                        //获取支付宝的通知返回参数，可参考技术文档中服务器异步通知参数列表

                        //商户订单号

                       

                        //支付宝交易号

                        string trade_no = Request.Form["trade_no"];

                        //交易状态
                        string trade_status = Request.Form["trade_status"];

                        log.Log(Request.Form["trade_status"]);
                        PayFactory.OnLog(out_trade_no, Request.Form.ToJson());

                        if (Request.Form["trade_status"] == "TRADE_FINISHED")
                        {
                            //判断该笔订单是否在商户网站中已经做过处理
                            //如果没有做过处理，根据订单号（out_trade_no）在商户网站的订单系统中查到该笔订单的详细，并执行商户的业务程序
                            //请务必判断请求时的total_fee、seller_id与通知时获取的total_fee、seller_id为一致的
                            //如果有做过处理，不执行商户的业务程序

                            //注意：
                            //退款日期超过可退款期限后（如三个月可退款），支付宝系统发送该交易状态通知
                        }
                        else if (Request.Form["trade_status"] == "TRADE_SUCCESS")
                        {
                            //判断该笔订单是否在商户网站中已经做过处理
                            //如果没有做过处理，根据订单号（out_trade_no）在商户网站的订单系统中查到该笔订单的详细，并执行商户的业务程序
                            //请务必判断请求时的total_fee、seller_id与通知时获取的total_fee、seller_id为一致的
                            //如果有做过处理，不执行商户的业务程序

                            //注意：
                            //付款完成后，支付宝系统发送该交易状态通知

                            log.Log("OnPaySuccessed");
                            PayFactory.OnPaySuccessed(out_trade_no, Request.Form.ToJson());
                        }
                        else
                        {
                        }

                        //——请根据您的业务逻辑来编写程序（以上代码仅作参考）——

                        Response.Write("success");  //请不要修改或删除

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

        private void handleReturn()
        {
            using(CLog log = new CLog("alipay handleReturn "))
            {
                log.Log(Request.QueryString.ToString());
                SortedDictionary<string, string> sPara = GetRequestGet();

                if (sPara.Count > 0)//判断是否有带返回参数
                {
                    //Com.Alipay.Notify aliNotify = new Com.Alipay.Notify();
                    //bool verifyResult = aliNotify.Verify(sPara, Request.QueryString["notify_id"], Request.QueryString["sign"]);
                    bool verifyResult = true;
                    if (verifyResult)//验证成功
                    {
                        /////////////////////////////////////////////////////////////////////////////////////////////////////////////
                        //请在这里加上商户的业务逻辑程序代码


                        //——请根据您的业务逻辑来编写程序（以下代码仅作参考）——
                        //获取支付宝的通知返回参数，可参考技术文档中页面跳转同步通知参数列表

                        //商户订单号

                        string out_trade_no = Request.QueryString["out_trade_no"];

                        //支付宝交易号

                        string trade_no = Request.QueryString["trade_no"];

                        //交易状态
                        string trade_status = Request.QueryString["trade_status"];

                        string myStatus = "";

                        log.Log("trade_status:{0}", Request.QueryString["trade_status"]);

                        if (Request.QueryString["trade_status"] == "TRADE_FINISHED" || Request.QueryString["trade_status"] == "TRADE_SUCCESS")
                        {
                            //判断该笔订单是否在商户网站中已经做过处理
                            //如果没有做过处理，根据订单号（out_trade_no）在商户网站的订单系统中查到该笔订单的详细，并执行商户的业务程序
                            //如果有做过处理，不执行商户的业务程序

                            PayFactory.OnPaySuccessed(out_trade_no, Request.Form.ToJson());
                            myStatus = "SUCCESS";
                        }
                        else
                        {
                            myStatus = HttpUtility.UrlEncode(Request.QueryString["trade_status"]);
                        }

                        string returnUrl = ReturnUrlConfigs[out_trade_no];
                        if (returnUrl.IsNullOrEmpty() == false)
                        {
                            log.Log("returnUrl:{0}" , returnUrl);

                            if (returnUrl.Contains("?") == false)
                            {
                                returnUrl += "?";
                            }
                            else
                            {
                                returnUrl += "&";
                            }
                            returnUrl += "status=" + myStatus + "&tradeID="+ HttpUtility.UrlEncode(out_trade_no) + "&interface=" + this.GetType().Name;
                            //移除跳转地址，防止堆积过多
                            ReturnUrlConfigs.Remove(out_trade_no);
                            Response.Write("<script>location.href=\"" + returnUrl + "\"</script>");
                        }
                    }

                }
            }
        }

        /// <summary>
        /// 获取支付宝GET过来通知消息，并以“参数名=参数值”的形式组成数组
        /// </summary>
        /// <returns>request回来的信息组成的数组</returns>
        public SortedDictionary<string, string> GetRequestGet()
        {
            int i = 0;
            SortedDictionary<string, string> sArray = new SortedDictionary<string, string>();
            NameValueCollection coll;
            //Load Form variables into NameValueCollection variable.
            coll = Request.QueryString;

            // Get names of all forms into a string array.
            String[] requestItem = coll.AllKeys;

            for (i = 0; i < requestItem.Length; i++)
            {
                sArray.Add(requestItem[i], Request.QueryString[requestItem[i]]);
            }

            return sArray;
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
