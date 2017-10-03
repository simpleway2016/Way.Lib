using System;
using System.Collections.Generic;
using System.Text;

namespace OPCdotNETLib
{
    public class OnlyItemsException : Exception
    {
        public OnlyItemsException(string message)
            : base(message)
        {
        }
    }
}
