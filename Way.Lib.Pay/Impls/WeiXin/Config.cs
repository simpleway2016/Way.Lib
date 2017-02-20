using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Web;
using System.Xml;

namespace WxPayAPI
{
    /**
    * 	配置账号信息
    */
    class WxPayConfig
    {
       
        public WxPayConfig(string xml)
        {
            XmlDocument xmldoc = new XmlDocument();
            xmldoc.LoadXml(xml);
            
            Type configType = this.GetType();
            foreach (XmlElement element in xmldoc.DocumentElement.ChildNodes)
            {
                FieldInfo field = configType.GetField(element.Name, System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.IgnoreCase);
                if (field == null)
                {
                    PropertyInfo property = configType.GetProperty(element.Name, System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.IgnoreCase);
                    if (property == null)
                        continue;
                    object value = Convert.ChangeType(element.InnerText, property.PropertyType);
                    property.SetValue(this, value);
                }
                else
                {
                    object value = Convert.ChangeType(element.InnerText, field.FieldType);
                    field.SetValue(this, value);
                }
            }
        }

        //=======【基本信息设置】=====================================
        /* 微信公众号信息配置
        * APPID：绑定支付的APPID（必须配置）
        * MCHID：商户号（必须配置）
        * KEY：商户支付密钥，参考开户邮件设置（必须配置）
        * APPSECRET：公众帐号secert（仅JSAPI支付的时候需要配置）
        */
        public string APPID ;
        public string MCHID ;
        public string KEY ;
        public string APPSECRET;

        //=======【证书路径设置】===================================== 
        /* 证书路径,注意应该填写绝对路径（仅退款、撤销订单时需要）
        */
        public string SSLCERT_PATH;

        string _SSLCERT_PASSWORD;
        public string SSLCERT_PASSWORD //默认密码是商户号
        {
            get {
                return _SSLCERT_PASSWORD == null ? MCHID : _SSLCERT_PASSWORD;
            }
            set
            {
                _SSLCERT_PASSWORD = value;
            }
        }



        //=======【支付结果通知url】===================================== 
        /* 支付结果通知回调url，用于商户接收支付结果
        */
        public string NOTIFY_URL= "http://paysdk.weixin.qq.com/example/ResultNotifyPage.aspx";

        //=======【商户系统后台机器IP】===================================== 
        /* 此参数可手动配置也可在程序中自动获取
        */
        public string IP = "8.8.8.8";


        //=======【代理服务器设置】===================================
        /* 默认IP和端口号分别为0.0.0.0和0，此时不开启代理（如有需要才设置）
        */
        public string PROXY_URL= "http://10.152.18.220:8080";

        //=======【上报信息配置】===================================
        /* 测速上报等级，0.关闭上报; 1.仅错误时上报; 2.全量上报
        */
        public int REPORT_LEVENL = 1;

        //=======【日志级别】===================================
        /* 日志等级，0.不输出日志；1.只输出错误信息; 2.输出错误和正常信息; 3.输出错误信息、正常信息和调试信息
        */
        public int LOG_LEVENL = 0;
    }
}