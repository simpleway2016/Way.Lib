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
        /// 传输的参数是否使用RSA加密
        /// </summary>
        public bool SubmitByRSA
        {
            get;
            set;
        }
    
        public RemotingMethodAttribute()
        {
        }
     
    }
}
