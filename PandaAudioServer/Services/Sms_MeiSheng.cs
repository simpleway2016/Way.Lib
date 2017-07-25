using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;
using System.Linq;
using System.Xml.Linq;
using System.Xml.XPath;
namespace PandaAudioServer.Services
{
    public class Sms_MeiSheng : ISms
    {
        string _username = "JSM41800";
        string _password = "xingyue@888";
        string _key = "951hugbttbig";
        string _url = "http://112.74.76.186:8030/service/httpService/httpInterface.do?method=sendUtf8Msg";

        public string Format(string content, params object[] parameters)
        {
            StringBuilder result = new StringBuilder();
            var matchs = Regex.Matches(content, @"\{(?<number>[0-9]+)\}");
            foreach (Match m in matchs)
            {
                if (result.Length > 0)
                    result.Append(',');
                int index = Convert.ToInt32(m.Groups["number"].Value);
                result.Append($"@{index+1}@={parameters[index]}");
            }
            return result.ToString();
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
            url.Append($"&content={message}");
            url.Append("&code=utf-8");

            var result = GetUrltoHtml(url.ToString());
          
            XDocument xmldoc = XDocument.Parse(result);
            XElement xroot = xmldoc.Root;
            var statusElement = xroot.XPathSelectElement("//status");
            if (statusElement == null)
                throw new Exception("返回结果没有status节点");

            var status = statusElement.Value;
            if (status != "0")
            {
                switch (status)
                {
                    case "100":
                        throw new Exception("发送失败");
                    case "101":
                        throw new Exception("用户账号不存在或密码错误");
                    case "102":
                        throw new Exception("账号已禁用");
                    case "103":
                        throw new Exception("参数不正确");
                    case "105":
                        throw new Exception("短信内容超过300字或为空、或内容编码格式不正确");
                    case "106":
                        throw new Exception("手机号码超过100个或有错误号码");
                    case "108":
                        throw new Exception("余额不足");
                    case "109":
                        throw new Exception("ip错误");
                }
            }
        }

        static string GetUrltoHtml(string Url)
        {
            System.Net.WebRequest wReq = System.Net.WebRequest.Create(Url);
            var responseTask = wReq.GetResponseAsync();
            responseTask.Wait();
            System.Net.WebResponse wResp = responseTask.Result;
            System.IO.Stream respStream = wResp.GetResponseStream();
            // Dim reader As StreamReader = New StreamReader(respStream)
            using (System.IO.StreamReader reader = new System.IO.StreamReader(respStream, Encoding.UTF8))
            {
                return reader.ReadToEnd();
            }
        }

    }
}
