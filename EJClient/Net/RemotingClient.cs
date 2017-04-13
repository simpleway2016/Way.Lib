using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace EJClient.Net
{
    class RemotingClient
    {
        enum WayScriptRemotingMessageType
        {
            Result = 1,
            Notify = 2,
            SendSessionID = 3,
            InvokeError = 4,
            UploadFileBegined = 5,
            RSADecrptError = 6,
        }
        class ResultInfo<T>
        {
            public T result;
            public WayScriptRemotingMessageType type;
        }
        class InvokeArg
        {
            public string ClassFullName;
            public string MethodName;
            public string[] Parameters;
            public string SessionID;
        }
        static string RemotingCookie = "";
        string _ServerUrl;
        string _Referer;
        public RemotingClient(string serverUrl)
        {
            _Referer = $"{serverUrl}/main.html";
               _ServerUrl = serverUrl + "/wayscriptremoting_invoke?a=1";
        }
        public delegate void CallbackHandler<T>(T result, string error);
        public async void Invoke<T>(string name, CallbackHandler<T> callback , params object[] methodParams)
        {
            Dictionary<string, string> values = new Dictionary<string, string>();
            string[] ps;
            if (methodParams != null)
            {
                ps = new string[methodParams.Length];
                for (int i = 0; i < methodParams.Length; i++)
                {
                    ps[i] = methodParams[i].ToJsonString();
                }
            }
            else
            {
                ps = new string[0];
            }

            //System.Net.WebClient web = new System.Net.WebClient();
            //web.Headers["Content-Type"] = "application/json";
            //byte[] bs = null;
            //await Task.Run(()=>
            //{
            //    bs  = web.UploadData(_ServerUrl, System.Text.Encoding.UTF8.GetBytes((new InvokeM
            //    {
            //        m = new InvokeArg
            //        {
            //            ClassFullName = "Way.EJServer.MainController",
            //            MethodName = name,
            //            Parameters = ps,
            //            SessionID = RemotingCookie
            //        }
            //    }).ToJsonString()));

            //});

            //string body = System.Text.Encoding.UTF8.GetString(bs);
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Add("Referer", _Referer);
            values["m"] = (new InvokeArg
            {
                ClassFullName = "Way.EJServer.MainController",
                MethodName = name,
                Parameters = ps,
                SessionID = RemotingCookie
            }).ToJsonString();
            var result = await client.PostAsync(_ServerUrl, new FormUrlEncodedContent(values));
            if (result.StatusCode != System.Net.HttpStatusCode.OK)
            {
                callback(default(T), result.ReasonPhrase);
                return;
            }
            var responseString = await result.Content.ReadAsStringAsync();
            try
            {
                var response = responseString.ToJsonObject<ResultInfo<T>>();
                if (response.type == WayScriptRemotingMessageType.InvokeError)
                {
                    callback(default(T), response.result.ToSafeString());
                }
                else if (response.type == WayScriptRemotingMessageType.Result)
                {
                    callback(response.result, null);
                }
                else
                {
                    callback(default(T), response.result.ToSafeString());
                }
            }
            catch
            {
                var response = responseString.ToJsonObject<ResultInfo<string>>();
                callback(default(T), response.result.ToSafeString());
            }
           
        }
    }
}
