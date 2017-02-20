using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Way.Lib.Pay
{
    /// <summary>
    /// 支付接口参数配置接口
    /// </summary>
    public interface IPayConfig
    {
        /// <summary>
        /// 获取支付接口的xml配置信息
        /// </summary>
        /// <param name="interfacetype">接口类型</param>
        /// <param name="tradeID">交易编号</param>
        /// <returns>返回xml配置信息</returns>
        string GetInterfaceXmlConfig(PayInterfaceType interfacetype, string tradeID);
    }
}
