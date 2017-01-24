using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Way.Lib.ScriptRemoting
{
    /// <summary>
    /// 定义了此属性的函数，才允许被JS执行
    /// </summary>
    public class RemotingMethodAttribute : Attribute
    {
        /// <summary>
        /// Javascript传输的参数,是否使用RSA加密
        /// </summary>
        public bool SubmitUseRSA
        {
            get;
            set;
        }
        /// <summary>
        /// 函数返回的值，是否采用rsa加密
        /// </summary>
        public bool ReturnUseRSA
        {
            get;
            set;
        }

    
        public RemotingMethodAttribute()
        {
        }
     
    }
}
