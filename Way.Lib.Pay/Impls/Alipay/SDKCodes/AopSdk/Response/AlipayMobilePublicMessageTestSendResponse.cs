using System;
using System.Xml.Serialization;

namespace Aop.Api.Response
{
    /// <summary>
    /// AlipayMobilePublicMessageTestSendResponse.
    /// </summary>
    public class AlipayMobilePublicMessageTestSendResponse : AopResponse
    {
        /// <summary>
        /// 结果码
        /// </summary>
        [XmlElement("code")]
        public string Code { get; set; }

        /// <summary>
        /// 结果描述
        /// </summary>
        [XmlElement("msg")]
        public string Msg { get; set; }
    }
}
