using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppLib
{
    public class ControlEventArg
    {
        /// <summary>
        /// 是否已处理，如果为true，控件不再自己处理
        /// </summary>
        public bool Handle
        {
            get;
            set;
        }
    }
}
