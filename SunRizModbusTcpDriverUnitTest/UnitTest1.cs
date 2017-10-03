using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SunRizModbusTcpDriver;
namespace SunRizModbusTcpDriverUnitTest
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void ReadPackageTest()
        {
            Way.Lib.NetStream client = new Way.Lib.NetStream("127.0.0.1", 502);
            WriteValuePackage package = new WriteValuePackage(FunctionCode.WriteHoldingRegister);
            package.Address = 1;
            package.Value = -388;
            var cmd = package.BuildCommand();
            client.Write(cmd);

            var result = package.ParseAnswer((len)=> {
                return client.ReceiveDatas(len);
            });
            
        }
    }
}
