using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Way.Lib.Pay
{
    public enum PayInterfaceType
    {
        
        //Alipay = 1,
        
        //WeChatPay = 2,
        /// <summary>
        /// 支付宝 - 条码（刷卡）支付
        /// </summary>
        AlipayBarcode = 1 | (1<<8),
        /// <summary>
        /// 支付宝 - 扫码支付（客户扫商家）
        /// </summary>
        AlipayScanQRCode = 1 | (1 << 9),
        /// <summary>
        /// 支付宝 - 网页支付
        /// </summary>
        AlipayWebPay = 1 | (1 << 10),
        /// <summary>
        /// 微信 - 条码（刷卡）支付
        /// </summary>
        WeiXinBarcode = 2 | (1 << 11),
        /// <summary>
        /// 微信 - 扫码支付（客户扫商家）
        /// </summary>
        WeiXinScanQRCode = 2 | (1 << 12),

    }
}
