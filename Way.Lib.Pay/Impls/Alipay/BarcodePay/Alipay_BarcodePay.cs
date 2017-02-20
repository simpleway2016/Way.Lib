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
using System.Threading;
using System.Threading.Tasks;
using System.Web;

namespace Way.Lib.Pay.Alipay
{
    /// <summary>
    /// 支付宝条码付款，（商家扫客户条码）
    /// </summary>
    [PayInterface(PayInterfaceType.AlipayBarcode)]
    class Alipay_BarcodePay : IPay
    {
        /// <summary>
        /// 直接付款，适合条码支付
        /// </summary>
        /// <param name="parameter"></param>
        public virtual string StartPay(PayParameter parameter)
        {
            if (parameter.AuthCode.IsNullOrEmpty())
            {
                throw new Exception("条码为空");
            }
            if (parameter.TradeID.IsNullOrEmpty())
            {
                throw new Exception("交易编号为空");
            }


            try
            {
                Config config = new Config(PayFactory.GetInterfaceXmlConfig(PayInterfaceType.AlipayBarcode, parameter.TradeID));


                //创建提交的内容
                AlipayTradePayContentBuilder builder = BuildPayContent(config, parameter);
                var client = config.AppConfig.CreateAopClient();
                AlipayTradePayRequest payRequest = new AlipayTradePayRequest();
                payRequest.BizContent = builder.BuildJson();

                AlipayTradePayResponse payResponse = client.Execute(payRequest);

                PayFactory.OnLog(parameter.TradeID, payResponse.Body);

                string[] errorCodes = new string[] { "20000", "20001", "40001", "40002", "40003", "40004", "40006" };//明确一定是错误的代码

                if (errorCodes.Contains(payResponse.Code))
                {
                    PayFactory.OnPayFailed(parameter.TradeID, payResponse.SubMsg, payResponse.Body);
                }
                else if (payResponse.Code == ResultCode.SUCCESS)
                {
                    PayFactory.OnPaySuccessed(parameter.TradeID, payResponse.Body);
                }
                else
                {
                    //到这里，不能确定支付结果，循环30秒确定
                    int checkTimes = parameter.Timeout / 2;
                    Thread.Sleep(1000);
                    for (int i = 0; i < checkTimes; i++)
                    {
                        if (checkPayStateByConfig(parameter, config))
                        {
                            break;
                        }
                        if (i + 1 == checkTimes)
                            break;
                        Thread.Sleep(2000);
                    }
                }

            }
            catch (Exception ex)
            {

            }

            return null;
        }

      

        private AlipayTradePayContentBuilder BuildPayContent(Config config,PayParameter parameter)
        {
            AlipayTradePayContentBuilder builder = new AlipayTradePayContentBuilder();

            builder.out_trade_no = parameter.TradeID;
            builder.scene = "bar_code";
            builder.auth_code = parameter.AuthCode;
            builder.total_amount = parameter.Amount.ToString();
            builder.discountable_amount = parameter.Amount.ToString();
            builder.undiscountable_amount = "0";
            builder.operator_id = "test";
            builder.subject = "条码支付";
            if (parameter.ExpireTime != DateTime.MinValue)
            {
                var minutes = Convert.ToInt32((parameter.ExpireTime - DateTime.Now).TotalMinutes);
                builder.timeout_express = Math.Max(1, minutes) + "m";
            }
            else
            {
                //默认2分钟
                builder.timeout_express = "2m";
            }
            builder.body = parameter.TradeName == null ? parameter.Description : parameter.TradeName;
            builder.store_id = "";    //很重要的参数，可以用作之后的营销     
            builder.seller_id = config.AppConfig.pid;       //可以是具体的收款账号。

            builder.goods_detail = new List<Com.Alipay.Model.GoodsInfo>();
            //扩展参数
            //系统商接入可以填此参数用作返佣
            //ExtendParams exParam = new ExtendParams();
            //exParam.sysServiceProviderId = "20880000000000";
            //builder.extendParams = exParam;

            return builder;

        }

        /// <summary>
        /// 根据TradeID检查支付状态
        /// </summary>
        public virtual void CheckPayState(PayParameter parameter)
        {
          
            try
            {
                Config config = new Alipay.Config(PayFactory.GetInterfaceXmlConfig(PayInterfaceType.AlipayBarcode, parameter.TradeID));
                checkPayStateByConfig(parameter, config);
            }
            catch
            {
               
            }
        }

        bool checkPayStateByConfig(PayParameter parameter , Config config)
        {

            IAlipayTradeService serviceClient = config.AppConfig.CreateClientInstance();
            var result = serviceClient.tradeQuery(parameter.TradeID);
            PayFactory.OnLog(parameter.TradeID, result.response.Body);
            if (result.Status == ResultEnum.SUCCESS)
            {
                PayFactory.OnPaySuccessed(parameter.TradeID, result.response.Body);
                return true;
            }
            return false;
        }

        public virtual RefoundResult Refound(RefoundParameter parameter)
        {
            if (parameter.TradeID.IsNullOrEmpty())
            {
                throw new Exception("TradeID is null");
            }
            Config config = new Alipay.Config(PayFactory.GetInterfaceXmlConfig(PayInterfaceType.AlipayBarcode, parameter.TradeID));
            var serviceClient = config.AppConfig.CreateClientInstance();

            AlipayTradeRefundContentBuilder builder = new AlipayTradeRefundContentBuilder();

            //支付宝交易号与商户网站订单号不能同时为空
            builder.out_trade_no = parameter.TradeID;

            //退款金额
            builder.refund_amount = parameter.Amount.ToString();

            builder.refund_reason = parameter.Reason;

            AlipayF2FRefundResult refundResult = serviceClient.tradeRefund(builder);

            RefoundResult finallyResult = new RefoundResult();

            //请在这里加上商户的业务逻辑程序代码
            //——请根据您的业务逻辑来编写程序（以下代码仅作参考）——
            switch (refundResult.Status)
            {
                case ResultEnum.SUCCESS:
                    finallyResult.Result = RefoundResult.ResultEnum.SUCCESS;
                    finallyResult.ServerMessage = refundResult.response.Body;
                    break;
                case ResultEnum.FAILED:
                    finallyResult.Result = RefoundResult.ResultEnum.FAIL;
                    finallyResult.ServerMessage = refundResult.response.Body;
                    finallyResult.Error = refundResult.response.SubMsg;
                    break;
                case ResultEnum.UNKNOWN:
                    finallyResult.Result = RefoundResult.ResultEnum.FAIL;
                    if (refundResult.response == null)
                    {
                        finallyResult.Error = "配置或网络异常，请检查";
                    }
                    else
                    {
                        finallyResult.Error = "系统异常，请走人工退款流程";
                    }
                    break;
            }

            return finallyResult;
        }

    }
}
