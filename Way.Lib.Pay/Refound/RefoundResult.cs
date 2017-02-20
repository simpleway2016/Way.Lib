using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Way.Lib.Pay
{
    /// <summary>
    /// 退款处理结果
    /// </summary>
    public class RefoundResult
    {
        /// <summary>
        /// 
        /// </summary>
        public enum ResultEnum
        {
            /// <summary>
            /// 退款成功
            /// </summary>
            SUCCESS = 0,
            /// <summary>
            /// 退款失败
            /// </summary>
            FAIL= 1
        }
        /// <summary>
        /// 
        /// </summary>
        public ResultEnum Result { get; set; }

        /// <summary>
        /// 服务器返回的信息
        /// </summary>
        public string ServerMessage { get; set; }
        /// <summary>
        /// 错误描述
        /// </summary>
        public string Error { get; set; }
    }
}
