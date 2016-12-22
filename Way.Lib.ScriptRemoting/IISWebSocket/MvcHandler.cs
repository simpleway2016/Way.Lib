#if NET46

#else
using Microsoft.AspNetCore.Routing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Way.Lib.ScriptRemoting.IISWebSocket
{
    class MvcHandler : IRouteHandler
    {
        public RequestDelegate GetRequestHandler(HttpContext httpContext, RouteData routeData)
        {
            return null;
        }
    }
}
#endif