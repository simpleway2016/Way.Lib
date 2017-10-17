using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SunRizModbusTcpDriver
{
    public enum FunctionCode
    {
        ReadCoilStatus = 1,
        ReadInputStatus = 2,
        ReadHoldingRegister = 3,
        ReadInputRegister = 4,
        WriteCoilStatus = 5,
        WriteHoldingRegister = 6,
    }
}
