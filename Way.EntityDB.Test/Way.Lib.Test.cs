using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
namespace Way.EntityDB.Test
{
    [TestClass]
    public class WayLibTest
    {
        [TestMethod]
        public void SerializeObject()
        {
            var p = new List1() {
                Name = "n"
            };
            p.Add("a1");
            p.Add("a2");
            p.Add("a3");
            p.Add("a4");
            p.Add("a5");
            var j = Way.Lib.Serialization.Serializer.SerializeObject(p);
            var dd = Way.Lib.Serialization.Serializer.DeserializeObject<List1>(j);
            if(dd.Count != p.Count || dd.LastOrDefault() != p.LastOrDefault() || dd.Name != p.Name)
                throw new Exception("List1结果错误");

            Dictionary<int, object> dic1 = new Dictionary<int, object>();
            dic1[1] = "abc";
            dic1[2] = 20;
            j = Way.Lib.Serialization.Serializer.SerializeObject(dic1);
            var dict2 = Way.Lib.Serialization.Serializer.DeserializeObject<Dictionary<int, object>>(j);
            if ((int)dict2[2] != (int)dic1[2])
                throw new Exception(" Dictionary<int, object>结果错误");

            var json = Way.Lib.Serialization.Serializer.SerializeObject(new c1 {
                Name = "name",
                Column = "col",
                f2 = "a",
            });
            var obj = Way.Lib.Serialization.Serializer.DeserializeObject<c1>(json);
            if (obj.Name != "name" || obj.Column != "col" || obj.f2 != "a")
                throw new Exception("结果错误");
        }
    }
    class List1 : List<string>
    {
        public string Name;
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
        public string f2;
        public string Column
        {
            get;
            set;
        }
    }
}
