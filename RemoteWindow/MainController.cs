using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Way.Lib.ScriptRemoting;

namespace RemoteWindow
{
    public class MainController :  RemotingController
    {
        [RemotingMethod]
        public string Test()
        {
            SendGroupMessage("group1", DateTime.Now.ToString());
            return "Hello World!";
        }
    }
}
