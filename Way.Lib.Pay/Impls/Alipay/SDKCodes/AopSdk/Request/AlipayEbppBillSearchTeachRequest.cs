using System;
using System.Collections.Generic;
using Aop.Api.Response;

namespace Aop.Api.Request
{
    /// <summary>
    /// AOP API: alipay.ebpp.bill.search.teach
    /// </summary>
    public class AlipayEbppBillSearchTeachRequest : IAopRequest<AlipayEbppBillSearchTeachResponse>
    {
        /// <summary>
        /// 11111
        /// </summary>
        public string BillKey { get; set; }

        /// <summary>
        /// 1
        /// </summary>
        public string ChargeInst { get; set; }

        /// <summary>
        /// 1
        /// </summary>
        public string ChargeoffInst { get; set; }

        /// <summary>
        /// 1
        /// </summary>
        public string CompanyId { get; set; }

        /// <summary>
        /// 1
        /// </summary>
        public string Extend { get; set; }

        /// <summary>
        /// 1
        /// </summary>
        public string OrderType { get; set; }

        /// <summary>
        /// 1
        /// </summary>
        public string SubOrderType { get; set; }

        #region IAopRequest Members
		private bool  needEncrypt=false;
        private string apiVersion = "1.0";
		private string terminalType;
		private string terminalInfo;
        private string prodCode;
		private string notifyUrl;


		public void SetNeedEncrypt(bool needEncrypt){
             this.needEncrypt=needEncrypt;
        }

        public bool GetNeedEncrypt(){

            return this.needEncrypt;
        }

		public void SetNotifyUrl(string notifyUrl){
            this.notifyUrl = notifyUrl;
        }

        public string GetNotifyUrl(){
            return this.notifyUrl;
        }

        public void SetTerminalType(String terminalType){
			this.terminalType=terminalType;
		}

    	public string GetTerminalType(){
    		return this.terminalType;
    	}

    	public void SetTerminalInfo(String terminalInfo){
    		this.terminalInfo=terminalInfo;
    	}

    	public string GetTerminalInfo(){
    		return this.terminalInfo;
    	}

        public void SetProdCode(String prodCode){
            this.prodCode=prodCode;
        }

        public string GetProdCode(){
            return this.prodCode;
        }

        public string GetApiName()
        {
            return "alipay.ebpp.bill.search.teach";
        }

        public void SetApiVersion(string apiVersion){
            this.apiVersion=apiVersion;
        }

        public string GetApiVersion(){
            return this.apiVersion;
        }

        public IDictionary<string, string> GetParameters()
        {
            AopDictionary parameters = new AopDictionary();
            parameters.Add("bill_key", this.BillKey);
            parameters.Add("charge_inst", this.ChargeInst);
            parameters.Add("chargeoff_inst", this.ChargeoffInst);
            parameters.Add("company_id", this.CompanyId);
            parameters.Add("extend", this.Extend);
            parameters.Add("order_type", this.OrderType);
            parameters.Add("sub_order_type", this.SubOrderType);
            return parameters;
        }

        #endregion
    }
}
