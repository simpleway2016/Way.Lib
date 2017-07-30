using System;
using System.Collections.Generic;
using System.Text;
using Way.Lib.ScriptRemoting;
using System.Linq;
using SysDB;
using System.IO;

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

        public int UserId
        {
            get
            {
                if (this.Session["UserId"] == null)
                    throw new Exception("请先登录");
                return (int)this.Session["UserId"];
            }
        }

        public MainController()
        {
            string path = $"{WebRoot}effects";
            if (System.IO.Directory.Exists(path) == false)
            {
                Directory.CreateDirectory(path);
            }
        }

        protected override void OnUnLoad()
        {
            if (_db != null)
            {
                _db.Dispose();
                _db = null;
            }
            base.OnUnLoad();

        }

        [RemotingMethod]
        public bool Login(string username, string password)
        {
            string ip = this.Request.RemoteEndPoint.ToString();
            if (IPError.IsIPLocked(ip))
                throw new Exception("禁止访问");

            var user = this.db.UserInfo.FirstOrDefault(m => m.UserName == username && m.Password == password);
            if (user == null)
            {
                var iperror = IPError.GetInstance(ip);
                iperror.MarkError();
                this.Session["iperror"] = iperror;
                if (iperror.ErrorCount < 7)
                    throw new Exception("用户名或密码错误");
                else
                    throw new Exception($"用户名或密码错误，剩余{iperror.ErrorCount}次机会！");
            }

            if (this.Session["iperror"] != null)
            {
                ((IPError)this.Session["iperror"]).Clear();
                this.Session.Remove("iperror");
            }
            this.Session["UserId"] = user.id;
            return true;
        }

        [RemotingMethod]
        public bool Register(UserInfo user)
        {
            string ip = this.Request.RemoteEndPoint.ToString();
            if (IPError.IsIPLocked(ip))
                throw new Exception("禁止访问");

            if (string.IsNullOrEmpty(user.RegGuid) || user.RegGuid != (string)this.Session["phoneCode"])
            {
                var iperror = IPError.GetInstance(ip);
                this.Session["iperror"] = iperror;
                iperror.MarkError();
                if (iperror.ErrorCount < 7)
                    throw new Exception("手机验证码错误");
                else
                    throw new Exception($"手机验证码，剩余{iperror.ErrorCount}次机会！");
            }

            if (this.Session["iperror"] != null)
            {
                ((IPError)this.Session["iperror"]).Clear();
                this.Session.Remove("iperror");
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
                return true;
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
        public bool SendPhoneVerifyCode(string regcode, string phoneNumber)
        {
            string ip = this.Request.RemoteEndPoint.ToString();
            if (IPError.IsIPLocked(ip))
                throw new Exception("禁止访问");

            if (this.db.SystemRegCode.Any(m => m.RegGuid == regcode && m.UserId == null) == false)
            {
                var iperror = IPError.GetInstance(ip);
                this.Session["iperror"] = iperror;
                iperror.MarkError();
                if (iperror.ErrorCount < 7)
                    throw new Exception("认证码错误");
                else
                    throw new Exception($"认证码错误，剩余{iperror.ErrorCount}次机会！");
            }
            else
            {
                if (this.Session["iperror"] != null)
                {
                    ((IPError)this.Session["iperror"]).Clear();
                    this.Session.Remove("iperror");
                }
                var sms = Factory.GetService<ISms>();
                this.Session["phoneCode"] = new Random().Next(1000, 9999).ToString();
                this.Session["regCode"] = regcode;
                var msg = sms.Format("{0}", this.Session["regcode"].ToString());
                sms.Send(phoneNumber, msg);
            }
            return true;
        }

        [RemotingMethod]
        public string GetProjectEffect(string name)
        {
            var effect = this.db.UserEffect.FirstOrDefault(m => m.UserId == this.UserId &&
                            m.Type == UserEffect_TypeEnum.Project &&
                            m.Name == name);
            if (effect == null)
                return null;

            string filepath = $"{WebRoot}effects/{effect.FileName}";
            return System.IO.File.ReadAllText(filepath, System.Text.Encoding.UTF8);
        }

        [RemotingMethod]
        public bool SaveProjectEffect(string name,string content)
        {
            string filepath;
            var effect = this.db.UserEffect.FirstOrDefault(m => m.UserId == this.UserId &&
                           m.Type == UserEffect_TypeEnum.Project &&
                           m.Name == name);
            if (effect != null)
            {
                filepath = $"{WebRoot}effects/{effect.FileName}";
                File.Delete(filepath);
            }
            else
            {
                effect = new UserEffect();
                effect.UserId = this.UserId;
                effect.Name = name;
                effect.Type = UserEffect_TypeEnum.Project;
            }
            
            effect.FileName = Guid.NewGuid().ToString();

            filepath = $"{WebRoot}effects/{effect.FileName}";
            File.WriteAllText(filepath,content, System.Text.Encoding.UTF8);

            this.db.Update(effect);
            return true;
        }
    }
}
