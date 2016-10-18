using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppLib
{
    public class VerifyException : Exception
    {
        public System.Web.UI.Control Control;
        public VerifyException(string msg, System.Web.UI.Control ctrl)
            : base(msg)
        {
            this.Control = ctrl;
        }
    }
}
