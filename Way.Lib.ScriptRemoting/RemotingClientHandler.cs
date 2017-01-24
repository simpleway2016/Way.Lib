using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Way.Lib.ScriptRemoting
{
    class RemotingClientHandler
    {
        public enum RemotingStreamType
        {
            Text,
            Bytes
        }

        public RemotingStreamType StreamType
        {
            get;
            private set;
        }
        static Type RemotingPageType = typeof(RemotingController);
        internal static ArrayList KeepAliveHandlers = ArrayList.Synchronized(new ArrayList());
        static ConcurrentDictionary<string, TypeDefine> ExistTypes = new ConcurrentDictionary<string, TypeDefine>();
        public delegate void SendDataHandler(string data);
        internal SendDataHandler mSendDataFunc;
        internal Action mCloseStreamHandler;
        SessionState Session;
        internal string GroupName;
        IUploadFileHandler mUploadFileHandler;
        int mFileGettedSize = 0;
        MessageBag mCurrentBag;
        string mClientIP;
        internal object Tag1;
        internal object Tag2;
        internal DateTime _heartTime;
        string _Referer;
        public RemotingClientHandler(SendDataHandler sendFunc , Action closeStreamHandler,string clientIP,string referer)
        {
            _Referer = referer;
               mCloseStreamHandler = closeStreamHandler;
            mSendDataFunc = sendFunc;
            this.StreamType = RemotingStreamType.Text;
            mClientIP = clientIP;
            _heartTime = DateTime.Now;
        }
        public virtual void OnReceived(string data)
        {
            try
            {
                MessageBag msgBag = Newtonsoft.Json.JsonConvert.DeserializeObject<MessageBag>(data);
                mCurrentBag = msgBag;
                if (this.Session == null)
                {
                    if (msgBag.SessionID.IsNullOrEmpty())
                    {
                        this.Session = SessionState.GetSession(Guid.NewGuid().ToString(), this.mClientIP);
                    }
                    else
                    {
                        this.Session = SessionState.GetSession(msgBag.SessionID, this.mClientIP);
                        if (this.Session == null)
                        {
                            this.Session = SessionState.GetSession(Guid.NewGuid().ToString(), this.mClientIP);
                        }
                    }
                }

                if (msgBag.Action == "init")
                {
                    handleInit(msgBag);
                }
                else if (msgBag.Action == "exit")
                {
                    mCloseStreamHandler();
                }
                else if (msgBag.Action == "UploadFile")
                {
                    this.StreamType = RemotingStreamType.Bytes;
                    handleUploadFile(msgBag);
                }
                else if (msgBag.MethodName.IsNullOrEmpty() == false)
                {
                    RemotingController.CheckHtmlFile(this._Referer);
                    handleMethodInvoke(msgBag);
                }
                else if (msgBag.GroupName.IsNullOrEmpty() == false)
                {
                    this.GroupName = msgBag.GroupName;
                    if(RemotingController.MessageReceiverConnect(this.Session , this.GroupName) == false)
                    {
                        throw new Exception("服务器拒绝你接收信息");
                    }
                    KeepAliveHandlers.Add(this);
                    return;
                }
            }
            catch(Exception ex)
            {
                throw ex;
            }
            finally
            {
                var thread = System.Threading.Thread.CurrentThread;
                if (SessionState.ThreadSessions.ContainsKey(thread))
                {
                    SessionState.ThreadSessions.Remove(thread);
                }
            }
            
        }

        public virtual void OnReceived(byte[] data)
        {
            try
            {
                mFileGettedSize += data.Length;
                if (mUploadFileHandler != null)
                {
                    mUploadFileHandler.OnGettingFileData(data);
                }
                SendData(MessageType.Result, mFileGettedSize);

                if (mFileGettedSize >= mCurrentBag.FileSize)
                {
                    if (mUploadFileHandler != null)
                    {
                        mUploadFileHandler.OnUploadFileCompleted();
                    }
                    mUploadFileHandler = null;
                    //重新接收text
                    this.StreamType = RemotingStreamType.Text;
                    SendData(MessageType.Result, "ok");
                }
            }
            catch (Exception ex)
            {
                this.StreamType = RemotingStreamType.Text;
                var baseException = ex.GetBaseException();
                SendData(MessageType.InvokeError, baseException != null ? baseException.Message : ex.Message);
            }
        }
        public virtual void OnDisconnected()
        {
            if (mUploadFileHandler != null)
            {
                try
                {
                    mUploadFileHandler.OnUploadFileError();
                }
                catch
                {
                }
                mUploadFileHandler = null;
            }
            try
            {
                if (this.GroupName.IsNullOrEmpty() == false)
                {
                    KeepAliveHandlers.Remove(this);
                }
            }
            catch
            {
            }
        }
        
        static TypeDefine checkRemotingName(string remoteName)
        {
            
            TypeDefine pageDefine = null;
            if (ExistTypes.ContainsKey(remoteName))
            {
                pageDefine = ExistTypes[remoteName];
            }
            else
            {
                Assembly[] assemblies = PlatformHelper.GetAppAssemblies();

                for (int i = 0; i < assemblies.Length; i++)
                {
                    var type = assemblies[i].GetType(remoteName);
                    if (type != null)
                    {
                        pageDefine = new ScriptRemoting.TypeDefine();
                        pageDefine.ControllerType = type;
                        break;
                    }
                }
                if (pageDefine == null)
                {
                    throw new Exception("无法找到" + remoteName + "类定义");
                }
                if (pageDefine.ControllerType != RemotingPageType && pageDefine.ControllerType.GetTypeInfo().IsSubclassOf(RemotingPageType) == false)
                {
                    throw new Exception(remoteName + "不是RemotingPage的子类");
                }

                MethodInfo[] methods = pageDefine.ControllerType.GetMethods( BindingFlags.Public | BindingFlags.Instance | BindingFlags.NonPublic);
                foreach (MethodInfo m in methods)
                {
                    if (m.GetCustomAttribute<RemotingMethodAttribute>() != null)
                    {
                        pageDefine.Methods.Add(m);
                    }
                }
                try
                {
                    ExistTypes[remoteName] = pageDefine;
                }
                catch(Exception ex)
                {
                    throw ex;
                }
            }
            return pageDefine;
        }

        void handleInit(MessageBag msgBag)
        {
            try
            {
                bool outputRSAKey = false;
                string remoteName = msgBag.ClassFullName;
                TypeDefine pageDefine = checkRemotingName(remoteName);


                StringBuilder methodOutput = new StringBuilder();
                methodOutput.Append(@"(function (_super) {
    __extends(func, _super);
    function func() {
        _super.apply(this, arguments);
    }
");
                foreach (MethodInfo method in pageDefine.Methods)
                {
                   
                    methodOutput.Append($"func.prototype." + method.Name + " = function (");
                    var parameters = method.GetParameters();

                    for (int i = 0; i < parameters.Length; i++)
                    {
                        methodOutput.Append(parameters[i].Name);
                        methodOutput.Append(',');
                    }
                    methodOutput.AppendLine("callback){");
                    methodOutput.Append("_super.prototype.pageInvoke.call(this,'" + method.Name + "',[");
                    for (int i = 0; i < parameters.Length; i++)
                    {
                        methodOutput.Append(parameters[i].Name);
                        if (i < parameters.Length - 1)
                        {
                            methodOutput.Append(',');
                        }
                    }
                    RemotingMethodAttribute methodAttr = (RemotingMethodAttribute)method.GetCustomAttribute(typeof(RemotingMethodAttribute));
                    if (methodAttr.UseRSA != RSAApplyScene.None)
                    {
                        outputRSAKey = true;
                        methodOutput.AppendLine($"] , callback,true,{(methodAttr.UseRSA.HasFlag(RSAApplyScene.WithSubmit) ?"true":"false")},{(methodAttr.UseRSA.HasFlag(RSAApplyScene.WithReturn) ? "true" : "false")} );");
                    }
                    else
                    {
                        methodOutput.AppendLine("] , callback );");
                    }
                    methodOutput.AppendLine("};");
                }

                methodOutput.Append(@"
    return func;
}(WayScriptRemoting));
");
                RemotingController currentPage = (RemotingController)Activator.CreateInstance(pageDefine.ControllerType);
                currentPage.Session = this.Session;
                currentPage.onLoad();
                if (outputRSAKey)
                {
                    CreateRSAKey(this.Session);
                }
                mSendDataFunc(Newtonsoft.Json.JsonConvert.SerializeObject(new
                {
                    text = methodOutput.ToString(),
                    SessionID = this.Session.SessionID,
                    rsa = outputRSAKey ? new { Exponent=this.Session["$$_rsa_PublicKeyExponent"], Modulus= this.Session["$$_rsa_PublicKeyModulus"] } : null,
                }));
            }
            catch (Exception ex)
            {
                mSendDataFunc(Newtonsoft.Json.JsonConvert.SerializeObject(new
                {
                    err = ex.Message,
                }));
            }
        }
        static byte[] HexStringToBytes(string hex,int index,int len)
        {
            if (len == 0)
            {
                return new byte[0];
            }



            byte[] result = new byte[len / 2];
            int myindex = 0;
            int endindex = index + len;
            for (int i = index; i < endindex; i+=2)
            {
                result[myindex] = byte.Parse(hex.Substring(i, 2), System.Globalization.NumberStyles.AllowHexSpecifier);
                myindex++;
            }

            return result;
        }
        private static string BytesToHexString(byte[] input)
        {
            StringBuilder hexString = new StringBuilder(64);

            for (int i = 0; i < input.Length; i++)
            {
                hexString.Append(String.Format("{0:X2}", input[i]));
            }
            return hexString.ToString();
        }
        internal static void CreateRSAKey(SessionState session)
        {
            if (session["$$_RSACryptoServiceProvider"] == null)
            {

                Way.Lib.RSA rsa = new RSA();

                session["$$_RSACryptoServiceProvider"] = rsa;
                session["$$_rsa_PublicKeyExponent"] = rsa.PublicKeyExponent;
                session["$$_rsa_PublicKeyModulus"] = rsa.PublicKeyModulus;
            }
        }

        internal static string DecrptRSA(SessionState session,string content)
        {
            RSA rsa = session["$$_RSACryptoServiceProvider"] as RSA;
            if (rsa == null )
            {
                throw new RSADecrptException();
            }
            return System.Net.WebUtility.UrlDecode(rsa.Decrypt(content)) ;
        }

        void handleUploadFile(MessageBag msgBag)
        {
            mFileGettedSize = 0;
            try
            {
                string remoteName = msgBag.ClassFullName;
                var pageDefine = checkRemotingName(remoteName);

                RemotingController currentPage = (RemotingController)Activator.CreateInstance(pageDefine.ControllerType);
                currentPage.Session = this.Session;
                currentPage.onLoad();
                mFileGettedSize = msgBag.Offset;
                mUploadFileHandler = currentPage.OnBeginUploadFile(msgBag.FileName,msgBag.State, msgBag.FileSize , msgBag.Offset);
                SendData(MessageType.UploadFileBegined,"ok");
            }
            catch (Exception ex)
            {
                var baseException = ex.GetBaseException();
                SendData(MessageType.InvokeError, baseException != null ? baseException.Message : ex.Message);
            }
        }

       void handleMethodInvoke(MessageBag msgBag)
        {
            try
            {
                string remoteName = (from m in RemotingController.ParsedHtmls where string.Equals(this._Referer, m.Url, StringComparison.CurrentCultureIgnoreCase) select m.Controller).FirstOrDefault();
                var pageDefine = checkRemotingName(remoteName);

                RemotingController currentPage = (RemotingController)Activator.CreateInstance(pageDefine.ControllerType);
                currentPage.Session = this.Session;
                currentPage.onLoad();


                MethodInfo methodinfo = pageDefine.Methods.Single(m => m.Name == msgBag.MethodName);
                RemotingMethodAttribute methodAttr = (RemotingMethodAttribute)methodinfo.GetCustomAttribute(typeof(RemotingMethodAttribute));
                var pInfos = methodinfo.GetParameters();

                if(methodAttr.UseRSA.HasFlag(RSAApplyScene.WithSubmit))
                {
                    var parameterStr = DecrptRSA(this.Session, msgBag.Parameters[0]);
                    msgBag.Parameters = (string[])Newtonsoft.Json.JsonConvert.DeserializeObject( $"[{parameterStr}]" , typeof(string[]) );
                }

                if (pInfos.Length != msgBag.Parameters.Length)
                    throw new Exception($"{msgBag.MethodName}参数个数不相符");
                object[] parameters = new object[pInfos.Length];
                for (int i = 0; i < parameters.Length; i++)
                {
                    Type pType = pInfos[i].ParameterType;

                    if (pType.GetTypeInfo().IsValueType)
                    {
                        parameters[i] = Convert.ChangeType(msgBag.Parameters[i], pType);
                    }
                    else
                    {
                        parameters[i] = Newtonsoft.Json.JsonConvert.DeserializeObject(msgBag.Parameters[i], pType);
                    }
                }
                var result = methodinfo.Invoke(currentPage, parameters);
                if (methodAttr.UseRSA.HasFlag(RSAApplyScene.WithReturn) && result != null)
                {
                    SendData(MessageType.Result, result, encryptToReturn);
                }
                else
                {
                    SendData(MessageType.Result, result);
                }
            }
            catch (RSADecrptException)
            {
                CreateRSAKey(this.Session);
                SendData(MessageType.RSADecrptError, new { Exponent = this.Session["$$_rsa_PublicKeyExponent"], Modulus = this.Session["$$_rsa_PublicKeyModulus"] });
            }
            catch (Exception ex)
            {
                var baseException = ex.GetBaseException();
                SendData(MessageType.InvokeError, baseException != null ? baseException.Message : ex.Message);
            }
        }

        string encryptToReturn(string ret)
        {
            if (ret == null)
                return null;
            RSA rsa = this.Session["$$_RSACryptoServiceProvider"] as RSA;
            return rsa.Encrypt2(System.Net.WebUtility.UrlEncode(ret));
        }

        public void SendData(MessageType msgType, object resultObj)
        {
            SendData(msgType , resultObj,null);
        }
        public void SendData(MessageType msgType, object resultObj,Func<string,string> rsaFunc)
        {
            try
            {
                lock (this)
                {
                    string objstr;
                    if (resultObj is Dictionary<string, object>[])
                    {

                        objstr = ResultHelper.ToJson((Dictionary<string, object>[])resultObj);
                    }
                    else
                    {
                        objstr = Newtonsoft.Json.JsonConvert.SerializeObject(resultObj);
                    }
                   
                    var dataStr = "{\"result\":" + objstr + ",\"type\":" + ((int)msgType) + "}";
                    if (rsaFunc != null)
                        dataStr = rsaFunc(dataStr);
                    mSendDataFunc(dataStr);
                }
            }
            catch (Exception ex)
            {
                mCloseStreamHandler();
                throw ex;
            }
        }
        public void SendClientMessage(string msg)
        {
            try
            {
                lock (this)
                {
                    var data = Newtonsoft.Json.JsonConvert.SerializeObject(new
                    {
                        msg = msg
                    });
                    mSendDataFunc(data);
                }
            }
            catch (Exception ex)
            {
                mCloseStreamHandler();
                throw ex;
            }
        }

    }
    class TypeDefine
    {
        public Type ControllerType;
        public List<MethodInfo> Methods = new List<MethodInfo>();
    }
    class MessageBag
    {
        public string SessionID;
        public string GroupName;
        public string ClassFullName;
        public string Action;
        public string MethodName;
        public string FileName;
        public string State;
        public int FileSize;
        public int Offset;
        public string[] Parameters;
    }

    enum MessageType
    {
        Result = 1,
        Notify = 2,
        SendSessionID = 3,
        InvokeError = 4,
        UploadFileBegined = 5,
        RSADecrptError = 6
    }

}
