using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Way.Lib;
using Way.Lib.ScriptRemoting;

namespace PandaAudioServer
{
    class RemotingClient
    {
        public static byte[] Exponent;
        public static byte[] Modulus;
        public static RemotingClient Instance;

        internal enum WayScriptRemotingMessageType
        {
            Result = 1,
            Notify = 2,
            SendSessionID = 3,
            InvokeError = 4,
            UploadFileBegined = 5,
            RSADecrptError = 6,
        }
        class InitInfo
        {
            public class rsainfo
            {
                public string Exponent;
                public string Modulus;
            }
            public rsainfo rsa;
            public string SessionID;
        }
        internal class ResultInfo<T>
        {
            public string sessionid;
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
        static string _sessionid = "";
        public static string SessionID
        {
            get
            {
                return _sessionid;
            }
            set
            {
                _sessionid = value;
            }
        }
       
        string _ServerUrl;
        string _Referer;
        public RemotingClient(string serverUrl)
        {
            _Referer = $"{serverUrl}/main.html";
               _ServerUrl = serverUrl + "/wayscriptremoting_invoke?a=1";
        }
        public delegate void CallbackHandler<T>(T result, string error);


        public void Init(out byte[] exponent, out byte[] modulus)
        {
            Dictionary<string, string> values = new Dictionary<string, string>();
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Add("Referer", _Referer);
            values["m"] = (new  {
                Action= "init",
                ClassFullName = "Way.EJServer.MainController",
            }).ToJsonString() ;
            var resultTask = client.PostAsync(_ServerUrl, new FormUrlEncodedContent(values));
            resultTask.Wait();
            var result = resultTask.Result;
            if (result.StatusCode != System.Net.HttpStatusCode.OK)
            {
                throw new Exception(result.ReasonPhrase);
            }
            var responseStringTask = result.Content.ReadAsStringAsync();
            responseStringTask.Wait();
            var responseString = responseStringTask.Result;
            var initInfo = responseString.FromJson<InitInfo>();
            SessionID = initInfo.SessionID;
            modulus = Way.Lib.RSA.HexStringToBytes( initInfo.rsa.Modulus);
            exponent = Way.Lib.RSA.HexStringToBytes(initInfo.rsa.Exponent);
        }
        public async void Invoke<T>(string name,  CallbackHandler<T> callback, params object[] methodParams)
        {
            Invoke<T>(name, RSAApplyScene.None, callback, methodParams);
        }
        public async void Invoke<T>(string name, RSAApplyScene rsaScene, CallbackHandler<T> callback , params object[] methodParams )
        {
            
            try
            {
                if (name == "Login")
                {
                    Exception err = null;
                    await Task.Run(() => {
                        try
                        {
                            Instance.Init(out Exponent, out Modulus);
                        }
                        catch (Exception ex)
                        {
                            err = ex.InnerException != null ? ex.InnerException : ex;
                        }
                    });
                    if (err != null)
                    {
                        callback(default(T), err.Message);
                        return;
                    }
                }
                Dictionary<string, string> values = new Dictionary<string, string>();
                string[] ps;
                if (methodParams != null)
                {
                    ps = new string[methodParams.Length];
                    for (int i = 0; i < methodParams.Length; i++)
                    {
                        ps[i] = methodParams[i].ToJsonString();
                        if (rsaScene.HasFlag(RSAApplyScene.EncryptParameters))
                        {
                            ps[i] = Way.Lib.RSA.EncryptByKey(System.Net.WebUtility.UrlEncode(ps[i]), Exponent, Modulus);
                        }
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
                    SessionID = SessionID
                }).ToJsonString();

                var result = await client.PostAsync(_ServerUrl, new FormUrlEncodedContent(values));
                if (result.StatusCode != System.Net.HttpStatusCode.OK)
                {
                    callback(default(T), result.ReasonPhrase);
                    return;
                }
                var responseString = await result.Content.ReadAsStringAsync();
                if(responseString.StartsWith("{") == false)
                {
                    responseString = Way.Lib.RSA.DecryptContentFromDEncrypt(responseString, Exponent, Modulus);
                    responseString = System.Net.WebUtility.UrlDecode(responseString);
                }
                try
                {
                    var response = responseString.FromJson<ResultInfo<T>>();
                    if (response.type == WayScriptRemotingMessageType.InvokeError)
                    {
                        Debug.WriteLine($"{name} error:{response.result}");
                        callback(default(T), response.result.ToSafeString());
                    }
                    else if (response.type == WayScriptRemotingMessageType.Result)
                    {
                        if (response.sessionid != null && response.sessionid.Length > 0)
                            SessionID = response.sessionid;
                        callback(response.result, null);
                    }
                    else
                    {
                        Debug.WriteLine($"{name} error:{response.result}");
                        callback(default(T), response.result.ToSafeString());
                    }
                }
                catch (Exception ex)
                {
                    try
                    {
                        var response = responseString.FromJson<ResultInfo<string>>();

                        if (response.type == WayScriptRemotingMessageType.Result)
                        {
                            if (response.sessionid != null && response.sessionid.Length > 0)
                                SessionID = response.sessionid;
                            callback(default(T), null);
                        }
                        else
                        {               
                            callback(default(T), response.result.ToSafeString());
                        }
                    }
                    catch (Exception ex2)
                    {
                        Debug.WriteLine($"{name} error:{ex.ToString()}");
                        callback(default(T), ex.ToString());
                    }
                }
            }
            catch(Exception ex)
            {
                callback(default(T), ex.ToString());
            }
           
        }

        public T InvokeSync<T>(string name, params object[] methodParams)
        {
            return InvokeSync<T>(name, RSAApplyScene.None, methodParams);
        }
         public T InvokeSync<T>(string name,RSAApplyScene rsaScene, params object[] methodParams)
        {
            Dictionary<string, string> values = new Dictionary<string, string>();
            string[] ps;
            if (methodParams != null)
            {
                ps = new string[methodParams.Length];
                for (int i = 0; i < methodParams.Length; i++)
                {
                    ps[i] = methodParams[i].ToJsonString();
                    if (rsaScene.HasFlag(RSAApplyScene.EncryptParameters))
                    {
                        ps[i] = Way.Lib.RSA.EncryptByKey(System.Net.WebUtility.UrlEncode(ps[i]), Exponent, Modulus);
                    }
                }
            }
            else
            {
                ps = new string[0];
            }
            
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Add("Referer", _Referer);
            values["m"] = (new InvokeArg
            {
                ClassFullName = "Way.EJServer.MainController",
                MethodName = name,
                Parameters = ps,
                SessionID = SessionID
            }).ToJsonString();
            var resultTask = client.PostAsync(_ServerUrl, new FormUrlEncodedContent(values));
            resultTask.Wait();
            var result = resultTask.Result;
            if (result.StatusCode != System.Net.HttpStatusCode.OK)
            {
                throw new Exception(result.ReasonPhrase);
            }
            var responseStringTask = result.Content.ReadAsStringAsync();
            responseStringTask.Wait();
            var responseString = responseStringTask.Result;
            if (responseString.StartsWith("{") == false)
            {
                responseString = Way.Lib.RSA.DecryptContentFromDEncrypt(responseString, Exponent, Modulus);
                responseString = System.Net.WebUtility.UrlDecode(responseString);
            }

            try
            {
                var response = responseString.FromJson<ResultInfo<T>>();
                if (response.type == WayScriptRemotingMessageType.InvokeError)
                {
                    throw new Exception(response.result.ToSafeString());
                }
                else if (response.type == WayScriptRemotingMessageType.Result)
                {
                    if (response.sessionid != null && response.sessionid.Length > 0)
                        SessionID = response.sessionid;
                    return response.result;
                }
                else
                {
                    throw new Exception(response.result.ToSafeString());
                }
            }
            catch(Exception ex)
            {
                string err = null;
                try
                {
                    var response = responseString.FromJson<ResultInfo<string>>();
                    err = response.result.ToSafeString();
                   
                }
                catch(Exception ex2)
                {
                    throw ex;
                }
                if(err != null)
                    throw new Exception(err);
            }

            return default(T);

        }
    }
}
