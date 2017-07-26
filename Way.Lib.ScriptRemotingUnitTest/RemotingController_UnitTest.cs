using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using System.Linq.Expressions;

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
                    var query = from m in db.DBColumn
                                join c in db.DBTable on m.TableID equals c.id
                                select new EJ.DBColumn
                                {
                                    Name = c.Name + "-" + m.Name,
                                    id = m.id
                                };
                    return query;
                }
            }
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
    }
}
