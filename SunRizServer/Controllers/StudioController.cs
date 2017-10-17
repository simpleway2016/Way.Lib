using System;
using System.Collections.Generic;
using System.Text;
using Way.Lib.ScriptRemoting;
using System.Linq;
namespace SunRizServer.Controllers
{
    public class StudioController : BaseController
    {
        [RemotingMethod]
        public int AddGateway(CommunicationDriver driver)
        {            
            if (driver.id == null &&  this.db.CommunicationDriver.Any(m => m.Address == driver.Address && m.Port == driver.Port))
                throw new Exception("此网关已存在");
            else if (this.db.CommunicationDriver.Any(m => m.id != driver.id && m.Address == driver.Address && m.Port == driver.Port))
                throw new Exception("此网关已存在");

            this.db.Update(driver);
            return driver.id.Value;
        }

        [RemotingMethod]
        public CommunicationDriver[] GetGatewayList()
        {
            return this.db.CommunicationDriver.ToArray();
        }
    }
}
