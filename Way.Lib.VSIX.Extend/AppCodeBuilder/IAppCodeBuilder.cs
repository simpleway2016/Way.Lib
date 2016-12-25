using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Way.Lib.VSIX.Extend.AppCodeBuilder
{
    public interface IAppCodeBuilder
    {
        string Name { get; }

        List<SampleCode> Build(SampleColumn[] columns);
    }
}
