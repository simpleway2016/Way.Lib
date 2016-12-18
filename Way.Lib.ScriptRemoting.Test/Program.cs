using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Way.Lib.ScriptRemoting.Test
{
    public class Program
    {
        public static void Main(string[] args)
        {
            if (System.IO.Directory.Exists(@"D:\注释\2016\EasyJobCore\Way.Lib.ScriptRemoting.Test.Web"))
                ScriptRemotingServer.Start(9090, @"D:\注释\2016\EasyJobCore\Way.Lib.ScriptRemoting.Test.Web", 1);
            else
                ScriptRemotingServer.Start(9090, @"D:\projects\c#\Way\Way.Lib.ScriptRemoting.Test", 1);
            Console.ReadKey();
        }
    }
}
