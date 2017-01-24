using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Way.Lib.ScriptRemoting
{
    public enum RSAApplyScene
    {
        /// <summary>
        /// 不使用RSA加密
        /// </summary>
        None = 0,
        /// <summary>
        /// js提交的参数，使用RSA加密
        /// </summary>
        WithSubmit = 1,
        /// <summary>
        /// 服务器返回的数据，使用RSA加密
        /// </summary>
        WithReturn = WithSubmit<<1,
        /// <summary>
        ///  js提交的参数，服务器返回的数据，都使用RSA加密
        /// </summary>
        WithSubmitAndReturn = WithSubmit | WithReturn,
    }
    /// <summary>
    /// 定义了此属性的函数，才允许被JS执行
    /// </summary>
    public class RemotingMethodAttribute : Attribute
    {
       
        /// <summary>
        /// RSA加密场景设置
        /// </summary>
        public RSAApplyScene UseRSA
        {
            get;
            set;
        }

    
        public RemotingMethodAttribute()
        {
            UseRSA = RSAApplyScene.None;
        }
     
    }
}
