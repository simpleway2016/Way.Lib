using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Way.Lib;


namespace SunRizStudio
{
    class RemotingClient
    {
        public enum RSAApplyScene
        {
            /// <summary>
            /// 不使用RSA加密
            /// </summary>
            None = 0,
            /// <summary>
            /// js提交的参数，使用RSA加密
            /// </summary>
            EncryptParameters = 1,
            /// <summary>
            /// 服务器返回的数据，使用RSA加密
            /// </summary>
            EncryptResult = EncryptParameters << 1,
            /// <summary>
            ///  js提交的参数，服务器返回的数据，都使用RSA加密
            /// </summary>
            EncryptResultAndParameters = EncryptParameters | EncryptResult,
        }
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
            public string ParameterJson;
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

        Config _config;
        public RemotingClient(Config config)
        {
            _config = config;
             
        }
        public delegate void CallbackHandler<T>(T result, string error);


        public void Init(out byte[] exponent, out byte[] modulus)
        {
            var serverUrl = _config.ServerUrl + "/wayscriptremoting_invoke?a=1";
            Dictionary<string, string> values = new Dictionary<string, string>();
            HttpClient client = new HttpClient();
            values["m"] = (new  {
                Action= "init",
                ClassFullName = "SunRizServer.Controllers.StudioController",
            }).ToJsonString() ;
            var resultTask = client.PostAsync(serverUrl, new FormUrlEncodedContent(values));
            resultTask.Wait();
            var result = resultTask.Result;
            if (result.StatusCode != System.Net.HttpStatusCode.OK)
            {
                throw new Exception(result.ReasonPhrase);
            }
            var responseStringTask = result.Content.ReadAsStringAsync();
            responseStringTask.Wait();
            var responseString = responseStringTask.Result;
            var initInfo = responseString.ToJsonObject<InitInfo>();
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
                            //Helper.Client.Init(out Helper.Exponent, out Helper.Modulus);
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
                string parameterJson;
                if (methodParams != null)
                {
                    if (rsaScene.HasFlag(RSAApplyScene.EncryptParameters))
                    {
                        parameterJson = Way.Lib.RSA.EncryptByKey(System.Text.Encoding.UTF8.GetBytes(methodParams.ToJsonString()).ToJsonString(), Helper.Exponent, Helper.Modulus);
                    }
                    else
                    {
                        parameterJson = methodParams.ToJsonString();
                    }
                }
                else
                {
                    parameterJson = "[]";
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
                //            ClassFullName = "SunRizServer.Controllers.StudioController",
                //            MethodName = name,
                //            Parameters = ps,
                //            SessionID = RemotingCookie
                //        }
                //    }).ToJsonString()));

                //});

                //string body = System.Text.Encoding.UTF8.GetString(bs);
                HttpClient client = new HttpClient();
                values["m"] = (new InvokeArg
                {
                    ClassFullName = "SunRizServer.Controllers.StudioController",
                    MethodName = name,
                    ParameterJson = parameterJson,
                    SessionID = SessionID
                }).ToJsonString();

                var serverUrl = _config.ServerUrl + "/wayscriptremoting_invoke?a=1";
                var result = await client.PostAsync(serverUrl, new FormUrlEncodedContent(values));
                if (result.StatusCode != System.Net.HttpStatusCode.OK)
                {
                    callback(default(T), result.ReasonPhrase);
                    return;
                }
                var responseString = await result.Content.ReadAsStringAsync();
                if(responseString.StartsWith("{") == false)
                {
                    responseString = Way.Lib.RSA.DecryptContentFromDEncrypt(responseString, Helper.Exponent, Helper.Modulus);
                    responseString = responseString.Replace("\\u", "");
                    byte[] bs = new byte[responseString.Length / 2];
                    for (int i = 0; i < bs.Length; i += 2)
                    {
                        bs[i + 1] = (byte)Convert.ToInt32(responseString.Substring(i * 2, 2), 16);
                        bs[i] = (byte)Convert.ToInt32(responseString.Substring(i * 2 + 2, 2), 16);
                    }
                    responseString = System.Text.Encoding.Unicode.GetString(bs);
                }
                try
                {
                    var response = responseString.ToJsonObject<ResultInfo<T>>();
                    if (response.type == WayScriptRemotingMessageType.InvokeError)
                    {
                        callback(default(T), responseString.ToJsonObject<Newtonsoft.Json.Linq.JToken>().Value<string>("result"));
                    }
                    else if (response.type == WayScriptRemotingMessageType.Result)
                    {
                        if (response.sessionid != null && response.sessionid.Length > 0)
                            SessionID = response.sessionid;
                        callback(response.result, null);
                    }
                    else
                    {
                        callback(default(T), responseString.ToJsonObject<Newtonsoft.Json.Linq.JToken>().Value<string>("result"));
                    }
                }
                catch (Exception ex)
                {
                    try
                    {
                        var response = responseString.ToJsonObject<ResultInfo<string>>();

                        if (response.type == WayScriptRemotingMessageType.Result)
                        {
                            if (response.sessionid != null && response.sessionid.Length > 0)
                                SessionID = response.sessionid;
                            callback(default(T), null);
                        }
                        else
                        {
                            if (response.result == "请先登录")
                            {
                                //if (Login.instance == null)
                                //{
                                //    Login login = new Login();
                                //    login.Topmost = true;
                                //    if (login.ShowDialog() == true)
                                //    {
                                //        Invoke<T>(name, callback, methodParams);
                                //        return;
                                //    }
                                //}
                                //else
                                //{
                                //    await Task.Run(() =>
                                //    {
                                //        while (Login.instance != null)
                                //            System.Threading.Thread.Sleep(200);
                                //    });
                                //    Invoke<T>(name, callback, methodParams);
                                //    return;
                                //}
                            }

                            Debug.WriteLine($"{name} error:{response.result}");
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
            string parameterJson;
            if (methodParams != null)
            {
                if (rsaScene.HasFlag(RSAApplyScene.EncryptParameters))
                {
                    parameterJson = Way.Lib.RSA.EncryptByKey(System.Text.Encoding.UTF8.GetBytes(methodParams.ToJsonString()).ToJsonString(), Helper.Exponent, Helper.Modulus);
                }
                else
                {
                    parameterJson = methodParams.ToJsonString();
                }
            }
            else
            {
                parameterJson = "[]";
            }

            HttpClient client = new HttpClient();
            values["m"] = (new InvokeArg
            {
                ClassFullName = "SunRizServer.Controllers.StudioController",
                MethodName = name,
                ParameterJson = parameterJson,
                SessionID = SessionID
            }).ToJsonString();

            var serverUrl = _config.ServerUrl + "/wayscriptremoting_invoke?a=1";
            var resultTask = client.PostAsync(serverUrl, new FormUrlEncodedContent(values));
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
                
                responseString = Way.Lib.RSA.DecryptContentFromDEncrypt(responseString, Helper.Exponent, Helper.Modulus);
                responseString = responseString.Replace("\\u", "");
                byte[] bs = new byte[responseString.Length / 2];
                for (int i = 0; i < bs.Length; i += 2)
                {
                    bs[i + 1] = (byte)Convert.ToInt32(responseString.Substring(i * 2, 2), 16);
                    bs[i] = (byte)Convert.ToInt32(responseString.Substring(i * 2 + 2, 2), 16);
                }
                responseString = System.Text.Encoding.Unicode.GetString(bs);
            }

            try
            {
                var response = responseString.ToJsonObject<ResultInfo<T>>();
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
                    var response = responseString.ToJsonObject<ResultInfo<string>>();
                    if (response.result == "请先登录")
                    {
                        //if (Login.instance == null)
                        //{
                        //    Login login = new Login();
                        //    login.Topmost = true;
                        //    if (login.ShowDialog() == true)
                        //    {
                        //        return InvokeSync<T>(name , methodParams);
                        //    }
                        //}
                        //else
                        //{
                        //    while(Login.instance != null)
                        //    {
                        //        System.Windows.Forms.Application.DoEvents();
                        //        System.Threading.Thread.Sleep(10);
                        //    }
                        //    return InvokeSync<T>(name, methodParams);
                        //}
                    }
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
