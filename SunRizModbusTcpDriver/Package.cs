using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SunRizModbusTcpDriver
{
    public class Package
    {
        /// <summary>
        /// 事务id
        /// </summary>
        public int TranId
        {
            get;
            set;
        }
        public int DataLength
        {
            get;
            set;
        }

        public Package()
        {
            this.TranId = 1;
        }

        public virtual byte[] BuildCommand()
        {
            var len = this.DataLength + 1;
            byte[] cmd = new byte[7];
            cmd[0] = (byte)(this.TranId >> 8);
            cmd[1] = (byte)(this.TranId & 0xFF);
            cmd[2] = 0x0;
            cmd[3] = 0x0;
            cmd[4] = (byte)(len >> 8);
            cmd[5] = (byte)(len & 0xFF);
            cmd[6] = 0x1;
            return cmd;
        }
    }
}
