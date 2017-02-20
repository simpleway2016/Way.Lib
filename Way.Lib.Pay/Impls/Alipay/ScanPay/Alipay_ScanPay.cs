using Aop.Api.Request;
using Aop.Api.Response;
using Com.Alipay;
using Com.Alipay.Business;
using Com.Alipay.Domain;
using Com.Alipay.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Way.Lib.Pay.Alipay
{
    /// <summary>
    /// 扫码支付（客户扫商家的二维码）
    /// </summary>
    [PayInterface(PayInterfaceType.AlipayScanQRCode)]
    class Alipay_ScanPay : Alipay_BarcodePay
    {
      
        /// <summary>
        /// 创建支付
        /// </summary>
        /// <returns>返回二维码内容</returns>
        public override string StartPay(PayParameter parameter)
        {
            if (parameter.TradeID.IsNullOrEmpty())
            {
                throw new Exception("交易编号为空");
            }

            Config config = new Alipay.Config(PayFactory.GetInterfaceXmlConfig(PayInterfaceType.AlipayScanQRCode, parameter.TradeID));
            IAlipayTradeService serviceClient = config.AppConfig.CreateClientInstance();

            AlipayTradePrecreateContentBuilder builder = BuildPrecreateContent(config, parameter);
            string out_trade_no = builder.out_trade_no;

            //回掉通知页面
            string notifyUrl = string.Format("http://{0}/{1}", HttpContext.Current.Request.Url.Authority, Alipay_ScanPay_HttpModule.NotifyPageName);

            AlipayF2FPrecreateResult precreateResult = serviceClient.tradePrecreate(builder , notifyUrl);

            PayFactory.OnLog(parameter.TradeID, precreateResult.response.Body);
            if (precreateResult.response.QrCode.IsNullOrEmpty())
            { 
                //如果没有生成二维码内容，认为失败
                throw new Exception(precreateResult.response.SubMsg);
            }

           if(precreateResult.Status == ResultEnum.FAILED)
            {
                throw new Exception(precreateResult.response.SubMsg);
            }

            return precreateResult.response.QrCode;
        }

        private AlipayTradePrecreateContentBuilder BuildPrecreateContent(Config config,PayParameter parameter)
        {

            AlipayTradePrecreateContentBuilder builder = new AlipayTradePrecreateContentBuilder();
            builder.out_trade_no = parameter.TradeID;
            builder.total_amount = parameter.Amount.ToString();
            builder.undiscountable_amount = "0";
            builder.operator_id = "test";
            builder.subject = "扫码支付";
            if (parameter.ExpireTime != DateTime.MinValue)
            {
                builder.time_expire = parameter.ExpireTime.ToString("yyyy-MM-dd HH:mm:ss"); 
            }
            else
            {
                //默认10分钟
                builder.time_expire = System.DateTime.Now.AddMinutes(10).ToString("yyyy-MM-dd HH:mm:ss"); ;
            }
            builder.body = parameter.TradeName == null ? parameter.Description : parameter.TradeName;
            builder.store_id = "";    //很重要的参数，可以用作之后的营销     
            builder.seller_id = config.AppConfig.pid;       //可以是具体的收款账号。

            builder.goods_detail = new List<GoodsInfo>();
            //扩展参数
            //系统商接入可以填此参数用作返佣
            //ExtendParams exParam = new ExtendParams();
            //exParam.sysServiceProviderId = "20880000000000";
            //builder.extendParams = exParam;

            return builder;

        }

        /// <summary>
        /// 检查订单状态
        /// </summary>
        public override void CheckPayState(PayParameter parameter)
        {
            try
            {
                Config config = new Alipay.Config(PayFactory.GetInterfaceXmlConfig(  PayInterfaceType.AlipayScanQRCode , parameter.TradeID));
                IAlipayTradeService serviceClient = config.AppConfig.CreateClientInstance();
                var result = serviceClient.tradeQuery(parameter.TradeID);
                PayFactory.OnLog(parameter.TradeID, result.response.Body);
                //客户没有扫码之前，会返回交易不存在
                if (result.Status == ResultEnum.SUCCESS)
                {
                    PayFactory.OnPaySuccessed(parameter.TradeID, result.response.Body);
                }
               
            }
            catch
            {

            }
        }


      
    }
}
