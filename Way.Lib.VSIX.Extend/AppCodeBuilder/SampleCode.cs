using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Way.Lib.VSIX.Extend.AppCodeBuilder
{
    public class SampleCode
    {
        public SampleCode(int height)
        {
            this.Height = height;
        }
        public string Name
        { get; set; }

        public string Code { get; set; }

        public int Height { get; set; }
    }
}
