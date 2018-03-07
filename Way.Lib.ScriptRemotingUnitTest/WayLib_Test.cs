using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Threading.Tasks;

namespace Way.Lib.ScriptRemotingUnitTest
{
    [TestClass]
    public class WayLib_Test
    {
        //

        [TestMethod]
        public void GetRSAParameters_Test()
        {
            string key = "MIICWwIBAAKBgQC7jqx7A21ZocVc14hH+YSpTs8rN16wo4kYvW7RZYgPb29uTAddUwjRVTVwWTsWRPfuYPsGrBAlcI8PcifJb6MdJ0w/K+FZJMXmWZEpYxg/QD/Qx2gcGtCza/xElcHR3s2zhxgDKRYPItYKqARXFd/VuwiE667Y1Ksu3azwKqO/kwIDAQABAoGAfBfE1MsKsZAQBgJwn7ZeaKrE9UH4O4Sn8596T78Oi6/eGSrigIOsxNvMtJ3FM1HEfIrb66kyMaNMdBrCakubrk5dZ/a2nu1ywM/J5Clr8fvU81hY4Ye+krTzc85L+ge/gWWVoGXepYq2gKhquxWQxsaJ2zz8K6UgL70CNCwag9ECQQDt8HxN845MKnOBondbaG7vSG5C+01SaH/uAYpK3Sj7WehK+2m5jrl+MA6SDi5mNbwuLd+7ANroxBU0hOgQAfwJAkEAycswnT32v04Nc8So6BggtJVofwcK5JR3UbX6W4UaUPeIfYz4BPx3SBYfI0pSggEcHFPWH3AS323B+XxJFK69uwJAbmGpGPSLJ/Rtn08CdgpNpH4CgNpaNYe7CWv3fuF4eJpt9BMMKgP3M34R1Fn11n7JLNclOnicFW2ZtMKPcZWqGQJAezJ09Jre6P6zEcmvwTrxxK4uxNa83L6Tdixes784SNRG3TfSN+EWxcjTq8z1QG+DBPxeDoVy0DuHIFSznU/tfwJACBlluBsVJ92lGOT149J2YiRBEDE0E4Mdj30pTvHZx7Rz1+5l6c8GuHkaqjZoSZbcmEalqomQgHU3Co8B1/iUEA==";
            var result = Way.Lib.RSA.GetRSAParameters(key);
        }

        [TestMethod]
        public void CLog_Test()
        {
            int count = 100;
            int doneCount = 0;
            for (int i = 0; i < count; i++)
            {
                int index = i + 1;
                Task.Run(() =>
                {
                    using (Way.Lib.CLog log = new CLog("Test", false))
                    {
                        log.Log($"a{index}" );
                        System.Threading.Interlocked.Increment(ref doneCount);
                    }
                });
            }
            while (doneCount < count)
                System.Threading.Thread.Sleep(10);
        }
    }
}
