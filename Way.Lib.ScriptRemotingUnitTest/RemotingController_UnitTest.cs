using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using System.Linq.Expressions;
using System.Collections.Generic;

namespace Way.Lib.ScriptRemotingUnitTest
{
    [TestClass]
    public class RemotingController_UnitTest
    {
        class TestController: ScriptRemoting.RemotingController
        {
            
        }

        enum TestEnum
        {
            e1 = 0,
            e2 = 1,
        }

        [TestMethod]
        public void LoadData_Test()
        {
            var result = new TestController().LoadData("Columns",2,3, "{id:'&3>=1'}");
        }
        [TestMethod]
        public void GetDataLength_Test()
        {
            var result = new TestController().GetDataLength("Columns", null);
        }

        [TestMethod]
        public void FindData_Test()
        {
            var searchModel = Newtonsoft.Json.JsonConvert.DeserializeObject<Newtonsoft.Json.Linq.JObject>( Newtonsoft.Json.JsonConvert.SerializeObject(new {
                time = new string[] { ">2010-1-1" , "<2012-3-3"},
                age = ">10 <200"
            }));
            var data = new List<Object1>();
            data.Add(new Object1
            {
                time = new DateTime(2010,2,3),
                age = 33
            });

                     var result = (IQueryable<Object1>)ScriptRemoting.RemotingController.FindData(data , searchModel);
            if (result.Count() != 1)
                throw new Exception("结果错误");
        }
    }

    class Object1
    {
        public DateTime time { get; set; }
        public int age { get; set; }
    }
}
