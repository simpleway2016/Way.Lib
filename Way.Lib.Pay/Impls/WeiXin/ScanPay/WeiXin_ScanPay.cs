using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using WxPayAPI;

namespace Way.Lib.Pay.WeiXin
{
    /// <summary>
    /// 微信扫码支付（客户扫商家的二维码）
    /// </summary>
    [PayInterface(PayInterfaceType.WeiXinScanQRCode)]
    class WeiXin_ScanPay : WeiXin_BarcodePay
    {
        /// <summary>
        /// 创建支付
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns>返回二维码内容</returns>
        public override string StartPay(PayParameter parameter)
        {
            if (parameter.TradeID.IsNullOrEmpty())
            {
                throw new Exception("交易编号为空");
            }

            WxPayConfig config = new WxPayAPI.WxPayConfig(PayFactory.GetInterfaceXmlConfig(PayInterfaceType.WeiXinScanQRCode, parameter.TradeID));
            //回掉通知页面
            string notifyUrl = string.Format("http://{0}/{1}", HttpContext.Current.Request.Url.Authority, WeiXin_ScanPay_HttpModule.NotifyPageName);

            WxPayData data = new WxPayData();
            data.SetValue("body", parameter.TradeName == null ? parameter.Description : parameter.TradeName);//商品描述
            data.SetValue("attach", "");//附加数据
            data.SetValue("out_trade_no", parameter.TradeID);//随机字符串
            data.SetValue("total_fee", Convert.ToInt32(parameter.Amount * 100).ToString());//总金额
            data.SetValue("time_start", DateTime.Now.ToString("yyyyMMddHHmmss"));//交易起始时间
            data.SetValue("notify_url", notifyUrl);//不用回调的话可以注释这句
            if (parameter.ExpireTime != DateTime.MinValue)
            {
                data.SetValue("time_expire", parameter.ExpireTime.ToString("yyyyMMddHHmmss"));//交易结束时间
            }
            else
            {
                //默认十分钟
                data.SetValue("time_expire", DateTime.Now.AddMinutes(10).ToString("yyyyMMddHHmmss"));//交易结束时间
            }
            data.SetValue("goods_tag", "");//商品标记
            data.SetValue("trade_type", "NATIVE");//交易类型
            data.SetValue("product_id", "123456");//商品ID

            WxPayData result = WxPayApi.UnifiedOrder(data ,config, 30);//调用统一下单接口
            string qrcode = result.GetValue("code_url").ToString();//获得统一下单接口返回的二维码内容
            return qrcode;
        }

        /// <summary>
        /// 检查支付状态
        /// </summary>
        public override void CheckPayState(PayParameter parameter)
        {
            try
            {
                WxPayConfig config = new WxPayAPI.WxPayConfig(PayFactory.GetInterfaceXmlConfig(  PayInterfaceType.WeiXinScanQRCode , parameter.TradeID));
                WxPayData queryOrderInput = new WxPayData();
                queryOrderInput.SetValue("out_trade_no", parameter.TradeID);
                WxPayData result = WxPayApi.OrderQuery(queryOrderInput , config);
                string xml = result.ToXml();
                PayFactory.OnLog(parameter.TradeID, xml);

                if (result.GetValue("return_code").ToString() == "SUCCESS"
                    && result.GetValue("result_code").ToString() == "SUCCESS")
                {
                    //支付成功
                    if (result.GetValue("trade_state").ToString() == "SUCCESS")
                    {
                        //触发回调函数
                        PayFactory.OnPaySuccessed(parameter.TradeID,xml);
                        return;
                    }
                    else if (result.GetValue("trade_state").ToString() == "NOTPAY")
                    {
                        //这是一开始生成二维码后，会是这个状态
                        return;
                    }
                }
                
            }
            catch
            {

            }
        }

    }
}
