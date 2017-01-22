using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Way.Lib.VSIX.Extend.Services
{
    public interface ITypeDiscoverer
    {
        Type[] GetTypes(Type basetype);
    }
}
