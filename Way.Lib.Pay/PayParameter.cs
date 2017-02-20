using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Way.Lib.Pay
{
    /// <summary>
    /// 发起支付设置的参数
    /// </summary>
    public class PayParameter
    {
        /// <summary>
        /// 交易编号
        /// </summary>
        public string TradeID { get; set; }

        /// <summary>
        /// 订单名称
        /// </summary>
        public string TradeName { get; set; }

        /// <summary>
        /// 网页支付接口，支付提交后，返回的页面路径
        /// </summary>
        public string ReturnUrl { get; set; }

        /// <summary>
        /// 交易描述
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// 交易金额
        /// </summary>
        public double Amount { get; set; }

        /// <summary>
        /// 失效时间
        /// </summary>
        public DateTime ExpireTime { get; set; }

        /// <summary>
        /// 扫码支付授权码，设备读取用户微信中的条码或者二维码信息
        /// </summary>
        public string AuthCode { get; set; }
        /// <summary>
        /// 支付方法执行时，最长的等待时间，单位：秒
        /// </summary>
        public int Timeout { get; set; }

        public PayParameter()
        {
            this.Timeout = 60;
        }
    }
}
