using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SunRizModbusTcpDriver
{
    public class WriteValuePackage : Package
    {
        public FunctionCode Function
        {
            get;
            set;
        }
        /// <summary>
        /// 起始地址，从1开始
        /// </summary>
        public int Address
        {
            get;
            set;
        }
        /// <summary>
        /// 数量
        /// </summary>
        public short Value
        {
            get;
            set;
        }
        public WriteValuePackage(FunctionCode function)
        {
            this.Function = function;
        }
        public override byte[] BuildCommand()
        {
            this.DataLength = 5;
            var cmd1 = base.BuildCommand();
            byte[] myCommand = new byte[5];
            myCommand[0] = (byte)this.Function;
            myCommand[1] = (byte)(this.Address >> 8);
            myCommand[2] = (byte)(this.Address & 0xFF);
            if (this.Function == FunctionCode.WriteCoilStatus)
            {
                if (Value != 0)
                {
                    myCommand[3] = 0xFF;
                    myCommand[4] = 0;
                }
                else
                {
                    myCommand[3] = 0;
                    myCommand[4] = 0;
                }
            }
            else
            {
                var valueBytes = BitConverter.GetBytes(this.Value);
                myCommand[3] = valueBytes[1];
                myCommand[4] = valueBytes[0];
            }

            //合并cmd1 + myCommand
            byte[] result = new byte[cmd1.Length + myCommand.Length];
            Array.Copy(cmd1, result, cmd1.Length);
            Array.Copy(myCommand, 0, result, cmd1.Length, myCommand.Length);
            return result;
        }

        public bool ParseAnswer(Func<int , byte[]> readDataFunc)
        {
            var cmd = readDataFunc(7);
            int len = ((cmd[4] << 8) | cmd[5]);
            this.TranId = ((cmd[0] << 8) | cmd[1]);
            this.DataLength = len - 1;
             var data = readDataFunc(this.DataLength);
            int funcCode = data[0];
            this.Function = (FunctionCode)funcCode;

            var address = (data[1] << 8) | data[2];
            

            if (this.Function ==  FunctionCode.WriteCoilStatus)
            {
                var value = (data[3] << 8) | data[4];
                return (this.Value >0 && value > 0) || (this.Value == 0 && value == 0);
            }
            else if(this.Function == FunctionCode.WriteHoldingRegister )
            {
                var value = BitConverter.ToInt16( new byte[] {data[4],data[3] } , 0);
                return value == this.Value;
            }
            throw new Exception($"not support function code{funcCode}");
        }
    }
}
