using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SunRizModbusTcpDriver;
using System.Threading;
using System.Diagnostics;

namespace SunRizModbusTcpDriverUnitTest
{
    [TestClass]
    public class ModbusDriverServerTest
    {
        [TestMethod]
        public void GetServerInfoTest()
        {
            SunRizModbusTcpDriver.ModbusDriverServer server = new ModbusDriverServer(588);
            server.Start();

            SunRizDriver.SunRizDriverClient client = new SunRizDriver.SunRizDriverClient("127.0.0.1" , 588);
            var info = client.GetServerInfo();

            server.Stop();

        }

        [TestMethod]
        public void AddPointToWatchTest()
        {
            SunRizModbusTcpDriver.ModbusDriverServer server = new ModbusDriverServer(588);
            server.Start();
            __doagain:
            bool hasError = false;
            SunRizDriver.SunRizDriverClient client = new SunRizDriver.SunRizDriverClient("127.0.0.1", 588);
            var points = new string[] {
                "1/3","3/1","1/0","1/2","3/2","1/4","1/1",
                "3/0","3/3"
            };
            client.AddPointToWatch("127.0.0.1/502", points, (point, value) => {
                Debug.WriteLine($"point:{point} value:{value}");
            },(err)=> {
                hasError = true;
                Debug.WriteLine($"AddPointToWatchTest Error :{err}");
               
            });

            Thread.Sleep(5000);
            if (hasError)
                goto __doagain;
            server.Stop();
            Thread.Sleep(3000);
        }

        [TestMethod]
        public void ReadValueTest()
        {
            SunRizModbusTcpDriver.ModbusDriverServer server = new ModbusDriverServer(588);
            server.Start();

            SunRizDriver.SunRizDriverClient client = new SunRizDriver.SunRizDriverClient("127.0.0.1", 588);
            var points = new string[] {
                "1/3","3/1","1/0","1/2","3/2","1/4","1/1",
                "3/0","3/3"
            };
            var values = client.ReadValue("127.0.0.1/502", points);
            server.Stop();
        }

        [TestMethod]
        public void WriteValueTest()
        {
            SunRizModbusTcpDriver.ModbusDriverServer server = new ModbusDriverServer(1588);
            server.Start();

            SunRizDriver.SunRizDriverClient client = new SunRizDriver.SunRizDriverClient("127.0.0.1", 1588);
            var points = new string[] {
                "3/5"
            };
            var values = new object[] {  4.896  };
            var result = client.WriteValue("127.0.0.1/502", points, values);
            server.Stop();
        }
        [TestMethod]
        public void CheckDeviceExistTest()
        {
            SunRizModbusTcpDriver.ModbusDriverServer server = new ModbusDriverServer(588);
            server.Start();

            SunRizDriver.SunRizDriverClient client = new SunRizDriver.SunRizDriverClient("127.0.0.1", 588);
           
            var result = client.CheckDeviceExist("127.0.0.1/502");
            server.Stop();
        }

        [TestMethod]
        public void GetPointPropertiesTest()
        {
            SunRizModbusTcpDriver.ModbusDriverServer server = new ModbusDriverServer(588);
            server.Start();

            SunRizDriver.SunRizDriverClient client = new SunRizDriver.SunRizDriverClient("127.0.0.1", 588);

            var result = client.GetPointProperties();
            server.Stop();
        }
        [TestMethod]
        public void GetPointAddressTest()
        {
            SunRizModbusTcpDriver.ModbusDriverServer server = new ModbusDriverServer(588);
            server.Start();

            SunRizDriver.SunRizDriverClient client = new SunRizDriver.SunRizDriverClient("127.0.0.1", 588);

            var result = client.GetPointAddress(new System.Collections.Generic.Dictionary<string, string>() {
                { "Modbus功能码" , "1"},
                 { "Modbus点地址" , "12"}
            });
            server.Stop();
        }
    }


}
