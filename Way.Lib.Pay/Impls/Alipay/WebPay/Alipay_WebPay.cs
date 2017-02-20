using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Way.Lib.Pay.Alipay.WebPay
{
    /// <summary>
    /// 支付宝网页支付接口
    /// </summary>
    [PayInterface( PayInterfaceType.AlipayWebPay)]
    class Alipay_WebPay : IPay
    {
       

        public string StartPay(PayParameter parameter)
        {
            if (parameter.TradeID.IsNullOrEmpty())
            {
                throw new Exception("交易编号为空");
            }
            Config config = new Alipay.Config(PayFactory.GetInterfaceXmlConfig(PayInterfaceType.AlipayWebPay, parameter.TradeID));
            //回掉通知页面
            string notifyUrl = string.Format("http://{0}/{1}", HttpContext.Current.Request.Url.Authority , Alipay_WebPay_HttpModule.NotifyPageName);

            string returnUrl = string.Format("http://{0}/{1}", HttpContext.Current.Request.Url.Authority, Alipay_WebPay_HttpModule.ReturnPageName);

            //把请求参数打包成数组
            SortedDictionary<string, string> sParaTemp = new SortedDictionary<string, string>();
            sParaTemp.Add("service", config.WebConfig.service);
            sParaTemp.Add("partner", config.WebConfig.pid);
            sParaTemp.Add("seller_id", config.WebConfig.seller_id);
            sParaTemp.Add("_input_charset", config.WebConfig.input_charset.ToLower());
            sParaTemp.Add("payment_type", config.WebConfig.payment_type);
            sParaTemp.Add("notify_url", notifyUrl);
            sParaTemp.Add("return_url", returnUrl);
            sParaTemp.Add("anti_phishing_key", config.WebConfig.anti_phishing_key);
            sParaTemp.Add("exter_invoke_ip", config.WebConfig.exter_invoke_ip);
            sParaTemp.Add("out_trade_no", parameter.TradeID);
            sParaTemp.Add("subject", parameter.TradeName);//订单名称
            sParaTemp.Add("total_fee", parameter.Amount.ToString());
            sParaTemp.Add("body", parameter.Description);//商品描述


            //其他业务参数根据在线开发文档，添加参数.文档地址:https://doc.open.alipay.com/doc2/detail.htm?spm=a219a.7629140.0.0.O9yorI&treeId=62&articleId=103740&docType=1
            //如sParaTemp.Add("参数名","参数值");

            //建立请求
            var submitBuilder = new Com.Alipay.Submit(config);
            string sHtmlText = submitBuilder.BuildRequest(sParaTemp, "get", "确认");

            Alipay_WebPay_HttpModule.ReturnUrlConfigs.Add(parameter.TradeID , parameter.ReturnUrl);

            return sHtmlText;
        }

        public void CheckPayState(PayParameter parameter)
        {
        }

     
        public virtual RefoundResult Refound(RefoundParameter parameter)
        {
            throw new NotImplementedException();
        }
    }
}
