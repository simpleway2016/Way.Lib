using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EJClient.AppCodeBuilder
{
    public interface IAppCodeBuilder
    {
        string Name { get; }
        string DefaultControlId { get; }
        List<SampleCode> Build(SampleColumn[] columns,string controlId);
    }
}
