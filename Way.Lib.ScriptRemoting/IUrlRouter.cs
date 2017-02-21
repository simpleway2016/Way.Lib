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
        System.Collections.Hashtable _headers;
        string _ip;
        internal HttpConnectInformation(System.Collections.Hashtable headers, string ip)
        {
            _ip = ip;
            _headers = headers;
        }

        SessionState _Session;
        public SessionState Session
        {
            get
            {
                if (_Session == null)
                {
                    var cookie = (string)_headers["Cookie"];
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
                       var cookie = (string)_headers["Cookie"];
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

        public Dictionary<string, string> RequestQuery
        {
            get;
            internal set;
        }
        public Dictionary<string, string> RequestForm
        {
            get;
            internal set;
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
        /// <param name="requestQuery"></param>
        /// <returns>如果返回null，表示不改变路由</returns>
        string GetUrl(string originalUrl,string fromUrl, HttpConnectInformation connectInfo,Dictionary<string,string> requestQuery);

       
    }
}
