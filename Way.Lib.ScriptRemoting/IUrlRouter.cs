using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Way.Lib.ScriptRemoting
{
    public class HttpConnectInformation
    {
        string _ip;
        Net.Request _request;
        public Net.Request Request
        {
            get
            {
                return _request;
            }
        }
        Net.Response _response;
        public Net.Response Response
        {
            get
            {
                return _response;
            }
        }
        internal HttpConnectInformation(Net.Request request,Net.Response response)
        {
            _ip = request.RemoteEndPoint.ToString().Split(':')[0];
            _request = request;
            _response = response;
        }

        SessionState _Session;
        public SessionState Session
        {
            get
            {
                if (_Session == null)
                {
                    var cookie = (string)_request.Headers["Cookie"];
                    if (cookie != null)
                    {
                        var match = System.Text.RegularExpressions.Regex.Match(cookie, @"WayScriptRemoting\=(?<g>(\w|\-)+)");
                        if (match != null)
                        {
                            _Session = SessionState.GetSession(match.Groups["g"].Value, _ip);
                        }
                    }
                }
                return _Session;
            }
        }
        Dictionary<string, string> _Cookie;
        public Dictionary<string, string> Cookie
        {
            get
            {
                if (_Cookie == null)
                {
                    _Cookie = new Dictionary<string, string>();
                       var cookie = (string)_request.Headers["Cookie"];
                    if (cookie != null)
                    {
                        var matches = System.Text.RegularExpressions.Regex.Matches(cookie, @"(?<n>(\w)+)\=(?<v>([^\=\;])+)");
                        if (matches != null)
                        {
                            foreach (Match m in matches)
                            {
                                //"WayScriptRemoting=6dd63747-4325-4bac-8a65-9ac70519870f; test=%u6211%u4EEC%3B2"

                                _Cookie.Add(m.Groups["n"].Value , WebUtility.UrlDecode( m.Groups["v"].Value));

                            }
                        }
                    }
                }
                return _Cookie;
            }
        }

    }
    /// <summary>
    /// 自定义路由接口
    /// </summary>
    public interface IUrlRouter
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="originalUrl">原始url</param>
        /// <param name="fromUrl">从哪个url访问的</param>
        /// <param name="connectInfo"></param>
        /// <returns>如果返回null，表示不改变路由</returns>
        string GetUrl(string originalUrl,string fromUrl, HttpConnectInformation connectInfo);

       
    }
}
