using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PandaAudioServer.Services;

namespace PandaAudioServerUnitTest
{
    [TestClass]
    public class SmsTest
    {
        [TestMethod]
        public void Sms_MeiShengTest()
        {
            Sms_MeiSheng obj = new Sms_MeiSheng();
            var msg = obj.Format("{0}", "1234");
            obj.Send("13261952754", msg);
        }
    }
}
