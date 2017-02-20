using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using WxPayAPI;

namespace Way.Lib.Pay.WeiXin
{
    /// <summary>
    /// 微信条码支付接口，（商家扫客户条码）
    /// </summary>
    [PayInterface(PayInterfaceType.WeiXinBarcode)]
    class WeiXin_BarcodePay : IPay
    {
       
      
        /// <summary>
        /// 检查订单状态
        /// </summary>
        public virtual void CheckPayState(PayParameter parameter)
        {
            try
            {
                WxPayConfig config = new WxPayAPI.WxPayConfig(PayFactory.GetInterfaceXmlConfig(PayInterfaceType.WeiXinBarcode, parameter.TradeID));
                checkPayStateByConfig(parameter, config);
            }
            catch
            {
            }
        }

        /// <summary>
        /// 检查订单状态
        /// </summary>
        /// <param name="parameter"></param>
        /// <param name="config"></param>
        /// <returns>只要有结果，无论成功或者失败，返回true，不确定支付结果返回false</returns>
        bool checkPayStateByConfig(PayParameter parameter, WxPayConfig config)
        {
            try
            {
               
                WxPayData queryOrderInput = new WxPayData();
                queryOrderInput.SetValue("out_trade_no", parameter.TradeID);
                WxPayData result = WxPayApi.OrderQuery(queryOrderInput, config);
                string xml = result.ToXml();

                PayFactory.OnLog(parameter.TradeID, xml);
                if (result.GetValue("return_code").ToString() == "SUCCESS"
                    && result.GetValue("result_code").ToString() == "SUCCESS")
                {
                    //支付成功
                    if (result.GetValue("trade_state").ToString() == "SUCCESS")
                    {
                        //触发回调函数
                        PayFactory.OnPaySuccessed(parameter.TradeID, result.ToXml());
                        return true;
                    }
                    //用户支付中，需要继续查询
                    else if (result.GetValue("trade_state").ToString() == "USERPAYING")
                    {
                        return false;
                    }
                    else if (result.GetValue("trade_state").ToString() == "NOTPAY")
                    {
                        //触发回调函数
                        PayFactory.OnPayFailed(parameter.TradeID, "放弃支付", xml);
                        return true;
                    }
                }

                string returnMsg = result.GetValue("err_code_des").ToSafeString();
                if (string.IsNullOrEmpty(returnMsg))
                    returnMsg = result.GetValue("return_msg").ToSafeString();

                //如果返回错误码为“此交易订单号不存在”，直接失败
                if (result.GetValue("err_code").ToString() == "ORDERNOTEXIST")
                {
                    //触发回调函数
                    PayFactory.OnPayFailed(parameter.TradeID, returnMsg, xml);
                    return true;
                }
                else if (result.GetValue("err_code").ToString() == "SYSTEMERROR")
                {
                    //如果是系统错误，则后续继续
                    return false;
                }
                else if (result.GetValue("return_code").ToString() == "FAIL"
                    || result.GetValue("result_code").ToString() == "FAIL")
                {
                    //FAIL
                    //触发回调函数
                    PayFactory.OnPayFailed(parameter.TradeID, returnMsg, xml);
                    return true;
                }
            }
            catch
            {

            }
            return false;
        }


        public virtual string StartPay(PayParameter parameter)
        {
            try
            {
                WxPayConfig config = new WxPayAPI.WxPayConfig(PayFactory.GetInterfaceXmlConfig(PayInterfaceType.WeiXinBarcode, parameter.TradeID));
                WxPayData data = new WxPayData();
                data.SetValue("auth_code", parameter.AuthCode);//授权码
                data.SetValue("body", parameter.TradeName == null ? parameter.Description : parameter.TradeName);//商品描述
                data.SetValue("total_fee", Convert.ToInt32(parameter.Amount * 100));//总金额,以分为单位
                data.SetValue("out_trade_no", parameter.TradeID);//产生随机的商户订单号

                WxPayData result = WxPayApi.Micropay(data, config, 20); //提交被扫支付，接收返回结果
                string xml = result.ToXml();
                PayFactory.OnLog(parameter.TradeID, xml);
                string returnMsg = result.IsSet("return_msg") ? result.GetValue("return_msg").ToSafeString() : result.GetValue("err_code_des").ToSafeString();
                //如果提交被扫支付接口调用失败，则抛异常
                if (!result.IsSet("return_code") || result.GetValue("return_code").ToString() == "FAIL")
                {
                    //触发回调函数
                    PayFactory.OnPayFailed(parameter.TradeID, returnMsg, xml);
                    return null;
                }

                //签名验证
                result.CheckSign(config);


                //刷卡支付直接成功
                if (result.GetValue("return_code").ToString() == "SUCCESS" &&
                    result.GetValue("result_code").ToString() == "SUCCESS")
                {
                    //触发回调函数
                    PayFactory.OnPaySuccessed(parameter.TradeID, result.ToXml());
                    return null;
                }

                //1）业务结果明确失败
                if (result.GetValue("err_code").ToString() != "USERPAYING" &&
           result.GetValue("err_code").ToString() != "SYSTEMERROR")
                {
                    //触发回调函数
                    PayFactory.OnPayFailed(parameter.TradeID, result.GetValue("err_code_des").ToSafeString(), xml);
                    return null;
                }

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
            catch(Exception ex)
            {
                throw ex;
            }
            return null;
        }


        public virtual RefoundResult Refound(RefoundParameter parameter)
        {
            WxPayConfig config = new WxPayAPI.WxPayConfig(PayFactory.GetInterfaceXmlConfig(PayInterfaceType.WeiXinBarcode, parameter.TradeID));

            WxPayData data = new WxPayData();
            data.SetValue("out_trade_no", parameter.TradeID);

            data.SetValue("total_fee",(int)(parameter.TotalAmount*100));//订单总金额
            data.SetValue("refund_fee", (int)(parameter.Amount * 100));//退款金额
            data.SetValue("out_refund_no", Guid.NewGuid().ToString());//随机生成商户退款单号
            data.SetValue("op_user_id", config.MCHID);//操作员，默认为商户号

            WxPayData result = WxPayApi.Refund(data , config);//提交退款申请给API，接收返回数据
            string err = result.GetValue("err_code_des") as string;

            RefoundResult finallyResult = new RefoundResult();

            if (result.GetValue("return_code").ToString() == "SUCCESS"
                   && result.GetValue("result_code").ToString() == "SUCCESS")
            {
                //退款成功
                finallyResult.Result = RefoundResult.ResultEnum.SUCCESS;
            }
            else
            {
                finallyResult.Result = RefoundResult.ResultEnum.FAIL;
                
            }
            finallyResult.Error = err;
            finallyResult.ServerMessage = result.ToXml();
            return finallyResult;
        }
    }
}
