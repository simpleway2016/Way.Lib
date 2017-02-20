using System;
using System.Xml.Serialization;
using System.Collections.Generic;

namespace Aop.Api.Domain
{
    /// <summary>
    /// AlipayInsSceneInsureModel Data Structure.
    /// </summary>
    [Serializable]
    public class AlipayInsSceneInsureModel : AopObject
    {
        /// <summary>
        /// 投保人
        /// </summary>
        [XmlElement("applicant")]
        public InsPerson Applicant { get; set; }

        /// <summary>
        /// 投保参数 ,每个产品特有的投保参数,如航空险的航班信息;标准json格式
        /// </summary>
        [XmlElement("biz_data")]
        public string BizData { get; set; }

        /// <summary>
        /// 生效时间
        /// </summary>
        [XmlElement("effect_start_time")]
        public string EffectStartTime { get; set; }

        /// <summary>
        /// 被保险人
        /// </summary>
        [XmlArray("insureds")]
        [XmlArrayItem("ins_person")]
        public List<InsPerson> Insureds { get; set; }

        /// <summary>
        /// 商户生成的外部投保业务号,必须保证唯一
        /// </summary>
        [XmlElement("out_biz_no")]
        public string OutBizNo { get; set; }

        /// <summary>
        /// 险种保障期限,数字+"Y/M/D"结尾,非固定期限险种或多固定期限险种必填
        /// </summary>
        [XmlElement("period")]
        public string Period { get; set; }

        /// <summary>
        /// 产品编码;由蚂蚁保险平台分配,商户通过该产品编码投保特定的保险产品（如饿了么外卖延误险）
        /// </summary>
        [XmlElement("prod_code")]
        public string ProdCode { get; set; }

        /// <summary>
        /// 渠道来源
        /// </summary>
        [XmlElement("source")]
        public string Source { get; set; }
    }
}
