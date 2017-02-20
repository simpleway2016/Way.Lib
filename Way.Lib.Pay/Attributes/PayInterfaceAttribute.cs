using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Way.Lib.Pay
{
    /// <summary>
    /// 定义支付接口类对应的枚举类型
    /// </summary>
    class PayInterfaceAttribute : Attribute
    {
        /// <summary>
        /// 接口类型
        /// </summary>
        public PayInterfaceType InterfaceType
        {
            get;
            set;
        }
        public PayInterfaceAttribute(PayInterfaceType type)
        {
            InterfaceType = type;
        }
    }
}
