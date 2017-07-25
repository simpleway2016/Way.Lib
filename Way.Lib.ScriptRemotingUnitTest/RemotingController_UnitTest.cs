using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

namespace Way.Lib.ScriptRemotingUnitTest
{
    [TestClass]
    public class RemotingController_UnitTest
    {
        class TestController: ScriptRemoting.RemotingController
        {
            public IQueryable<EJ.DBColumn> Columns
            {
                get
                {
                    MyDB db = new MyDB();
                    return from m in db.DBColumn
                           select new EJ.DBColumn
                           {
                               Name = m.Name,
                               id = m.id,
                               caption = m.caption,
                           };
                }
            }
        }

        [TestMethod]
        public void LoadData_Test()
        {
            var result = new TestController().LoadData("Columns",2,3, null);
        }
        [TestMethod]
        public void GetDataLength_Test()
        {
            var result = new TestController().GetDataLength("Columns", null);
        }
    }
}
