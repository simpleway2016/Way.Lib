using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;

namespace Way.EntityDB.Test
{
    [TestClass]
    public class WayLibTest
    {
        [TestMethod]
        public void SerializeObject()
        {
            var json = Way.Lib.Serialization.Serializer.SerializeObject(new c1 {
                Name = "name",
                Column = "col"
            });
            var obj = Way.Lib.Serialization.Serializer.DeserializeObject<c1>(json);
            if (obj.Name != "name" || obj.Column != "col")
                throw new Exception("结果错误");
        }
    }

    class c1:c0
    {
        public string Name
        {
            get;
            set;
        }
    }
    class c0
    {
        public string Column
        {
            get;
            set;
        }
    }
}
