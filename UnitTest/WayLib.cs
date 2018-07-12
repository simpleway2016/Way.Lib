using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace UnitTest
{
    [TestClass]
    public class WayLib
    {
        [TestMethod]
        public void Rollback()
        {
            Model m = new Model();
           var originalJson =  Newtonsoft.Json.JsonConvert.SerializeObject(m);
            m.Name = "abc";
            m.Rollback();
            if (originalJson != Newtonsoft.Json.JsonConvert.SerializeObject(m))
                throw new Exception("error");
        }
    }

    class Model : Way.Lib.DataModel
    {

        string _Name;
        public string Name
        {
            get
            {
                return _Name;
            }
            set
            {
                if (_Name != value)
                {
                    _Name = value;
                    this.OnPropertyChanged("Name", null, value);
                }
            }
        }
    }
}
