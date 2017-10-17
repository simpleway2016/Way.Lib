using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SunRizDriver
{
    public abstract class CommandHandler
    {
        public abstract void Handle(Way.Lib.NetStream stream,Command command);
    }
}
