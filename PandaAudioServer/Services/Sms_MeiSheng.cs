using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace PandaAudioServer.Services
{
    class Sms_MeiSheng : ISms
    {
        string _username = "JSM41800";
        string _password = "v39tgwll";
        string _key = "951hugbttbig";
        string _url = "http://112.74.76.186:8040/service/httpService/httpInterface.do?method=sendUtf8Msg";

        public string Format(string content, params object[] parameters)
        {
            while(true)
            {
                var match = Regex.Match(content, @"\@(?<number>[0-9]+)\@");
                if(match == null || match.Length == 0)
                {
                    break;
                }
                else
                {
                    content = content.Replace(match.Value, "{" + (Convert.ToInt32(match.Groups["number"]) - 1) + "}");
                }
            }
            return string.Format(content, parameters);
        }

        public void Send(string phoneNumber, string message)
        {
            StringBuilder url = new StringBuilder(_url);
            url.Append($"&username={_username}");
            url.Append($"&password={_password}");
            url.Append($"&veryCode={_key}");
            url.Append($"&mobile={phoneNumber}");
            url.Append($"&tempid=JSM41800-0001");
            url.Append("&msgtype=2");
            url.Append($"&content={System.Net.WebUtility.UrlEncode(message)}");
            url.Append("&code=utf-8");

            var request = System.Net.WebRequest.CreateHttp(url.ToString());
           
        }
    }
}
