using System;
using System.Collections.Generic;
using System.Text;
using Way.Lib.ScriptRemoting;
using System.Linq;

namespace PandaAudioServer
{
    [RemotingMethod]
    public class MainController : RemotingController
    {
        PandaDB _db;
        public PandaDB db
        {
            get
            {
                return _db ?? (_db = new PandaDB());
            }
        }

        protected override void OnUnLoad()
        {
            if(_db != null)
            {
                _db.Dispose();
                _db = null;
            }
            base.OnUnLoad();

        }

        [RemotingMethod]
        public void Login( db.UserInfo user )
        {

        }

        [RemotingMethod]
        public void Register(db.UserInfo user)
        {
            string ip = this.Request.RemoteEndPoint.ToString();
            if (IPError.IsIPLocked(ip))
                throw new Exception("禁止访问");

            if ( string.IsNullOrEmpty(user.RegGuid) || user.RegGuid != (string)this.Session["phoneCode"])
            {
                var iperror = IPError.GetInstance(ip);
                iperror.MarkError();
                if (iperror.ErrorCount < 7)
                    throw new Exception("手机验证码错误");
                else
                    throw new Exception($"手机验证码，剩余{iperror.ErrorCount}次机会！");
            }

            user.RegGuid = this.Session["regCode"].ToString();
            this.db.BeginTransaction();
            try
            {
                this.db.Insert(user);
                var sysRegCode = this.db.SystemRegCode.FirstOrDefault(m => m.RegGuid == user.RegGuid && m.UserId == null);
                sysRegCode.UserId = user.id;

                this.db.Update(sysRegCode);

                this.db.CommitTransaction();
            }
            catch
            {
                this.db.RollbackTransaction();
                throw;
            }
        }

        /// <summary>
        /// 发送手机验证码
        /// </summary>
        /// <param name="phoneNumber"></param>
        [RemotingMethod]
        public void SendPhoneVerifyCode(string regcode, string phoneNumber)
        {
            string ip = this.Request.RemoteEndPoint.ToString();
            if (IPError.IsIPLocked(ip))
                throw new Exception("禁止访问");

            if(this.db.SystemRegCode.Any(m => m.RegGuid == regcode && m.UserId == null) == false)
            {
                var iperror = IPError.GetInstance(ip);
                iperror.MarkError();
                if (iperror.ErrorCount < 7)
                    throw new Exception("认证码错误");
                else
                    throw new Exception($"认证码错误，剩余{iperror.ErrorCount}次机会！");
            }
            else
            {
                var sms = Factory.GetService<ISms>();
                this.Session["phoneCode"] = new Random().Next(1000, 9999).ToString();
                this.Session["regCode"] = regcode;
                var msg = sms.Format("{0}", this.Session["regcode"].ToString());
                sms.Send(phoneNumber, msg);
            }
        }
    }
}
