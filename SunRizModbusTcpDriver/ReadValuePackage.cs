using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SunRizModbusTcpDriver
{
    public class ReadValuePackage : Package
    {
        public FunctionCode Function
        {
            get;
            set;
        }
        /// <summary>
        /// 起始地址，从1开始
        /// </summary>
        public int StartAddress
        {
            get;
            set;
        }
        /// <summary>
        /// 数量
        /// </summary>
        public int Quantity
        {
            get;
            set;
        }
        public ReadValuePackage(FunctionCode function)
        {
            this.Function = function;
        }
        public override byte[] BuildCommand()
        {
            this.DataLength = 5;
            var cmd1 = base.BuildCommand();
            byte[] myCommand = new byte[5];
            myCommand[0] = (byte)this.Function;
            myCommand[1] = (byte)(this.StartAddress >> 8);
            myCommand[2] = (byte)(this.StartAddress & 0xFF);
            myCommand[3] = (byte)(this.Quantity >> 8);
            myCommand[4] = (byte)(this.Quantity & 0xFF);

            //合并cmd1 + myCommand
            byte[] result = new byte[cmd1.Length + myCommand.Length];
            Array.Copy(cmd1, result, cmd1.Length);
            Array.Copy(myCommand, 0, result, cmd1.Length, myCommand.Length);
            return result;
        }

        public short[] ParseAnswer(Func<int , byte[]> readDataFunc)
        {
            var cmd = readDataFunc(7);
            int len = ((cmd[4] << 8) | cmd[5]);
            this.TranId = ((cmd[0] << 8) | cmd[1]);
            this.DataLength = len - 1;
             var data = readDataFunc(this.DataLength);
            int funcCode = data[0];
            this.Function = (FunctionCode)funcCode;
            int byteCount = data[1];
            byte[] result = new byte[data.Length - 2];
            Array.Copy(data, 2, result, 0, result.Length);

            if (this.Function ==  FunctionCode.ReadCoilStatus || this.Function == FunctionCode.ReadInputStatus)
            {
                //开关型
                short[] answer = new short[this.Quantity];
                int index = 0;
                for (int i = 0; i < result.Length; i++)
                {
                    //每个字节，表示8个开关量
                    byte value = result[i];
                    for (int j = 0; j < 8 && index < answer.Length; j++)
                    {
                        answer[index++] = ((value & (1 << j)) != 0) ? (short)1 : (short)0;
                    }
                }
                return answer;
            }
            else if(this.Function == FunctionCode.ReadHoldingRegister || this.Function == FunctionCode.ReadInputRegister)
            {
                //short型
                short[] answer = new short[this.Quantity];
                int index = 0;
                for (int i = 0; i < result.Length; i+=2)
                {
                    byte hi = result[i];
                    byte lo = result[i + 1];
                    answer[index++] = BitConverter.ToInt16( new byte[] { lo , hi  } , 0 );
                }
                return answer;
            }
            throw new Exception($"not support function code{funcCode}");
        }
    }
}
