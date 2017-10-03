using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SunRizOpcDriver;
using System.Threading;
using System.Diagnostics;
using System.Collections.Generic;

namespace SunRizModbusTcpDriverUnitTest
{
    [TestClass]
    public class OpcDriverServerTest
    {
        [TestMethod]
        public void EnumDeviceTest()
        {
            var server = new SunRizOpcDriver.OpcDriverServer(588);
            server.Start();

            SunRizDriver.SunRizDriverClient client = new SunRizDriver.SunRizDriverClient("127.0.0.1", 588);
            var info = client.EnumDevice(null);

            server.Stop();

        }
        [TestMethod]
        public void EnumDevicePointTest()
        {
            var server = new SunRizOpcDriver.OpcDriverServer(588);
            server.Start();

            SunRizDriver.SunRizDriverClient client = new SunRizDriver.SunRizDriverClient("127.0.0.1", 588);
            List<string> parentPath = new List<string>();
            parentPath.Add("Simulation Items");
            parentPath.Add("Random");

            client.EnumDevicePoint("127.0.0.1|Matrikon.OPC.Simulation.1" , parentPath, (point)=> {
                 Debug.WriteLine( $"path:{point.Path} name:{point.Name} isfolder:{point.IsFolder}" );
             });

            server.Stop();

        }

        [TestMethod]
        public void GetServerInfoTest()
        {
            SunRizOpcDriver.OpcDriverServer server = new OpcDriverServer(588);
            server.Start();

            SunRizDriver.SunRizDriverClient client = new SunRizDriver.SunRizDriverClient("127.0.0.1" , 588);
            var info = client.GetServerInfo();

            server.Stop();

        }

        [TestMethod]
        public void AddPointToWatchTest()
        {
            SunRizOpcDriver.OpcDriverServer server = new OpcDriverServer(588);
            server.Start();
            __doagain:
            bool hasError = false;
            SunRizDriver.SunRizDriverClient client = new SunRizDriver.SunRizDriverClient("127.0.0.1", 588);
            var points = new string[] {
                "Random.ArrayOfReal8","Random.Real4","Random.String"
            };
            client.AddPointToWatch("127.0.0.1|Matrikon.OPC.Simulation.1", points, (point, value) => {
                Debug.WriteLine($"point:{point} value:{value}");
            },(err)=> {
                hasError = true;
                Debug.WriteLine($"AddPointToWatchTest Error :{err}");
               
            });

            Thread.Sleep(50000);
            if (hasError)
                goto __doagain;
            server.Stop();
            Thread.Sleep(3000);
        }

        [TestMethod]
        public void ReadValueTest()
        {
            SunRizOpcDriver.OpcDriverServer server = new OpcDriverServer(588);
            server.Start();

            SunRizDriver.SunRizDriverClient client = new SunRizDriver.SunRizDriverClient("127.0.0.1", 588);
            var points = new string[] {
               "Bucket Brigade.ArrayOfReal8","Bucket Brigade.Real8"
            };
            var values = client.ReadValue("127.0.0.1|Matrikon.OPC.Simulation.1", points);
            server.Stop();
        }

        [TestMethod]
        public void WriteValueTest()
        {
            SunRizOpcDriver.OpcDriverServer server = new OpcDriverServer(588);
            server.Start();

            SunRizDriver.SunRizDriverClient client = new SunRizDriver.SunRizDriverClient("127.0.0.1", 588);
            var points = new string[] {
                "Bucket Brigade.Real8","Bucket Brigade.ArrayOfReal8","Bucket Brigade.ArrayOfString"
            };
            var values = new object[] {388.273 , new float[] {1.1f , 3f } , new string[] { "a1", "b2" } };
            var result = client.WriteValue("127.0.0.1|Matrikon.OPC.Simulation.1", points, values);
            server.Stop();
        }
        [TestMethod]
        public void CheckDeviceExistTest()
        {
            SunRizOpcDriver.OpcDriverServer server = new OpcDriverServer(588);
            server.Start();

            SunRizDriver.SunRizDriverClient client = new SunRizDriver.SunRizDriverClient("127.0.0.1", 588);
           
            var result = client.CheckDeviceExist("127.0.0.1|Matrikon.OPC.Simulation.1");
            server.Stop();
        }
    }


}
