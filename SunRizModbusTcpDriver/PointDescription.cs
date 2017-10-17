using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SunRizModbusTcpDriver
{
    class PointGroup
    {
        public FunctionCode Function;
        public int StartAddress;
        public List<PointDescription> Points = new List<PointDescription>();
    }
    class PointDescription
    {
        public FunctionCode Function;
        public int Address;
        public short Value;
        public short? OriginalValue;
        public int Index;
        public PointDescription(string path,int index)
        {
            this.Index = index;
            string[] info = path.Split('/');
            this.Function = (FunctionCode)int.Parse(info[0]);
            this.Address = int.Parse(info[1]);
        }
    }
}
