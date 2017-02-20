using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Way.Lib.Pay
{
    /// <summary>
    /// 监听付款结果
    /// </summary>
    public interface IPayResultListener
    {
        /// <summary>
        /// 交易成功触发
        /// </summary>
        /// <param name="tradeID">交易编号</param>
        /// <param name="message">官方返回的信息</param>
        void OnPaySuccessed(string tradeID, string message);

        /// <summary>
        /// 交易失败触发
        /// </summary>
        /// <param name="tradeID">交易编号</param>
        /// <param name="reason">失败原因</param>
        /// <param name="message">官方返回的信息</param>
        void OnPayFailed(string tradeID,string reason , string message);

        

        /// <summary>
        /// 捕获支付接口返回的信息
        /// </summary>
        /// <param name="tradeID"></param>
        /// <param name="message"></param>
        void OnLog(string tradeID, string message);
    }
}
