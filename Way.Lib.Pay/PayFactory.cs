using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Way.Lib.Pay
{
   /// <summary>
   ///  
   /// </summary>
    public class PayFactory
    {
        static IPayConfig _Config;
        static Dictionary<PayInterfaceType, Type> _PayInterfaceDefineds;
        static List<IPayResultListener> _PayResultListeners = new List<IPayResultListener>();
        /// <summary>
        /// 是否设置了HttpApplication.BeginRequest事件
        /// </summary>
        static bool SettedWebBeginRequest = false;

        /// <summary>
        /// 记录每个支付接口对应的枚举类型
        /// </summary>
        static Dictionary<PayInterfaceType, Type> PayInterfaceDefineds
        {
            get {
                if(_PayInterfaceDefineds == null)
                {
                    _PayInterfaceDefineds = new Dictionary<PayInterfaceType, Type>();
                    //循环当前程序集的所有Type，取出所有接口类型
                    Type ipayType = typeof(IPay);
                    Type[] types = ipayType.Assembly.GetTypes();
                    foreach( Type type in types )
                    {
                        //判断这个类是否实现了IPay接口
                        if ( type.GetInterface(ipayType.FullName) != null )
                        {
                            var atts = type.GetCustomAttributes(typeof(PayInterfaceAttribute), false);
                            if (atts.Length == 0)
                                continue;
                            PayInterfaceAttribute myAtt = atts[0] as PayInterfaceAttribute;
                            _PayInterfaceDefineds.Add(myAtt.InterfaceType , type);
                        }
                    }
                }
                return _PayInterfaceDefineds;
            }
        }

        
        /// <summary>
        /// 注册支付结果监听器
        /// </summary>
        /// <param name="listener"></param>
        public static void RegisterResultListener(IPayResultListener listener)
        {
            _PayResultListeners.Add(listener);
        }

     
        /// <summary>
        /// 开启支付功能，此方法必须在static Global()这个静态构造函数里面执行
        /// </summary>
        public static void Enable(IPayConfig config, IPayResultListener listener)
        {
            if (config == null)
            {
                throw new Exception("config is null");
            }
            _Config = config;
            if(listener != null)
            {
                RegisterResultListener(listener);
            }
            if (!SettedWebBeginRequest)
            {
                SettedWebBeginRequest = true;
                using (CLog log = new CLog("PayFactory.Enable "))
                {
                    try
                    {
                        //取出所有IHttpModule，注册到HttpApplication
                        Type[] types = typeof(IPay).Assembly.GetTypes();
                        string moduleName = typeof(IHttpModule).FullName;
                        foreach (Type type in types)
                        {
                            //判断这个类是否实现了取出所有IHttpModule接口
                            if (type.GetInterface(moduleName) != null)
                            {
                                log.Log(type.FullName);
                                HttpApplication.RegisterModule(type);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        using (CLog logErr = new CLog("PayFactory.Enable error "))
                        {
                            logErr.Log(ex.ToString());
                        }
                    }
                }
                
            }
        }

        /// <summary>
        /// 根据支付类型，创建支付接口
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static IPay CreatePay(PayInterfaceType type )
        {
            if(SettedWebBeginRequest == false)
            {
                throw new Exception("请在static Global()这个静态构造函数里面执行PayFactory.Enable()");
            }
            if (_PayResultListeners.Count == 0)
            {
                throw new Exception("必须先添加一个PayResultListener");
            }

            var objType = PayInterfaceDefineds[type];
            if(objType != null)
            {
                return (IPay)Activator.CreateInstance(objType);
            }
            return null;
        }

        /// <summary>
        /// 触发所有监听器的OnPayFailed
        /// </summary>
        /// <param name="tradeID">交易编号</param>
        /// <param name="reason">失败原因</param>
        /// <param name="message">官方返回的信息</param>
        internal static void OnPayFailed(string tradeID, string reason, string message)
        {
            foreach (IPayResultListener listener in _PayResultListeners)
            {
                listener.OnPayFailed(tradeID, reason, message);
            }
        }

        /// <summary>
        /// 触发所有监听器的OnPaySuccessed
        /// </summary>
        /// <param name="tradeID">交易编号</param>
        /// <param name="message">官方返回的信息</param>
        internal static void OnPaySuccessed(string tradeID, string message)
        {
            foreach (IPayResultListener listener in _PayResultListeners)
            {
                listener.OnPaySuccessed(tradeID, message);
            }
        }
        internal static void OnLog(string tradeID, string message)
        {
            foreach (IPayResultListener listener in _PayResultListeners)
            {
                listener.OnLog(tradeID, message);
            }
        }
        /// <summary>
        /// 获取支付接口的xml配置信息
        /// </summary>
        /// <param name="interfacetype">接口类型</param>
        /// <param name="tradeID">交易编号</param>
        /// <returns>返回xml配置信息</returns>
        internal static string GetInterfaceXmlConfig(PayInterfaceType interfacetype, string tradeID)
        {
            if (_Config != null)
                return _Config.GetInterfaceXmlConfig(interfacetype, tradeID);
            return null;
        }
    }

}
