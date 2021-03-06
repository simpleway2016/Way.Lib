using System;
using System.Xml.Serialization;

namespace Aop.Api.Domain
{
    /// <summary>
    /// AlipayInsScenePolicyQueryModel Data Structure.
    /// </summary>
    [Serializable]
    public class AlipayInsScenePolicyQueryModel : AopObject
    {
        /// <summary>
        /// 商户生成的外部投保业务号
        /// </summary>
        [XmlElement("out_biz_no")]
        public string OutBizNo { get; set; }

        /// <summary>
        /// 产品编码；由蚂蚁保险平台分配，商户通过该产品编码投保特定的保险产品（如饿了么外卖延误险）
        /// </summary>
        [XmlElement("prod_code")]
        public string ProdCode { get; set; }
    }
}
