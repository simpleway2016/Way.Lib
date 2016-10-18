using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppLib
{
    public class RightAttribute : Attribute
    {
        public string Aspx
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="aspx">与哪个页面url具备相同权限判断,如 /test/index.aspx </param>
        public RightAttribute(string aspx)
        {
            Aspx = aspx;
        }
    }
}
