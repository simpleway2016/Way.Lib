using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SunRizStudio
{
    class Helper
    {
        public static byte[] Exponent;
        public static byte[] Modulus;
        static RemotingClient _Remote;
        public static RemotingClient Remote
        {
            get
            {
                if(_Remote == null)
                {
                    _Remote = new RemotingClient("http://localhost:8988");
                }
                return _Remote;
            }
        }
    }
}
