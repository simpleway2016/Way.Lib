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
    public class MainController : BaseController
    {
        public MainController()
        {
            string path = $"{WebRoot}effects";
            if (System.IO.Directory.Exists(path) == false)
            {
                Directory.CreateDirectory(path);
            }
        }

        [RemotingMethod(UseRSA = RSAApplyScene.EncryptResultAndParameters)]
        public bool CheckUser(string loginCode)
        {
            try
            {
                var user = this.db.UserInfo.FirstOrDefault(m => m.id == this.UserId);
                user.LastCheckTime = DateTime.Now;
                this.db.Update(user);

                if (user.LoginCode != loginCode)
                    return false;
                return true;
            }
            catch
            {
                return false;
            }
        }

        [RemotingMethod( UseRSA = RSAApplyScene.EncryptResultAndParameters)]
        public string Login(string username, string password)
        {
            string ip = this.Request.RemoteEndPoint.ToString().Split(':')[0];
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
                    throw new Exception($"用户名或密码错误，剩余{iperror.GetChance()}次机会！");
            }

            if (this.Session["iperror"] != null)
            {
                ((IPError)this.Session["iperror"]).Clear();
                this.Session.Remove("iperror");
            }
            user.LoginCode = Guid.NewGuid().ToString("N");
            this.db.Update(user);
            this.Session["UserId"] = user.id;
            return user.LoginCode;
        }

        [RemotingMethod(UseRSA = RSAApplyScene.EncryptParameters)]
        public bool Register(UserInfo user,string phonecode)
        {
            string ip = this.Request.RemoteEndPoint.ToString().Split(':')[0];
            if (IPError.IsIPLocked(ip))
                throw new Exception("禁止访问");

            //借助RegGuid字段存一下验证码
            if (string.IsNullOrEmpty(phonecode) || phonecode != (string)this.Session["phoneCode"])
            {
                var iperror = IPError.GetInstance(ip);
                this.Session["iperror"] = iperror;
                iperror.MarkError();
                if (iperror.ErrorCount < 7)
                    throw new Exception("手机验证码错误");
                else
                    throw new Exception($"手机验证码，剩余{iperror.GetChance()}次机会！");
            }

            if (this.Session["iperror"] != null)
            {
                ((IPError)this.Session["iperror"]).Clear();
                this.Session.Remove("iperror");
            }

            if(this.db.UserInfo.Any(m=>m.UserName == user.UserName))
                throw new Exception("用户名已存在，请重新填写用户名");

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
        [RemotingMethod(UseRSA = RSAApplyScene.EncryptParameters)]
        public bool SendPhoneVerifyCode(string regcode, string phoneNumber)
        {
            string ip = this.Request.RemoteEndPoint.ToString().Split(':')[0];
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
                    throw new Exception($"认证码错误，剩余{iperror.GetChance()}次机会！");
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
                var msg = sms.Format("{0}", this.Session["phoneCode"].ToString());
                sms.Send(phoneNumber, msg);
            }
            return true;
        }

        [RemotingMethod]
        public string DownloadEffect(string name, UserEffect_TypeEnum type)
        {
            var effect = this.db.UserEffect.FirstOrDefault(m => m.UserId == this.UserId &&
                            m.Type == type &&
                            m.Name == name);
            if (effect == null)
                return null;

            string filepath = $"{WebRoot}effects/{effect.FileName}";
            return System.IO.File.ReadAllText(filepath, System.Text.Encoding.UTF8);
        }

        [RemotingMethod]
        public bool UploadEffect(string name, UserEffect_TypeEnum type, string content,int version)
        {
            string filepath;
            var effect = this.db.UserEffect.FirstOrDefault(m => m.UserId == this.UserId &&
                           m.Type == type &&
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
                effect.Type = type;
            }
                        
            effect.Version = version;
            effect.FileName = Guid.NewGuid().ToString();

            filepath = $"{WebRoot}effects/{effect.FileName}";
            File.WriteAllText(filepath,content, System.Text.Encoding.UTF8);

            this.db.Update(effect);
            return true;
        }

        
        [RemotingMethod]
        public object CompareEffects(UserEffect[] clientEffect)
        {
            var serverEffects = this.db.UserEffect.Where(m => m.UserId == this.UserId).ToArray();
            //先计算需要下载的effect
            var downloads = (from m in serverEffects
                             where clientEffect.Any(client => string.Equals(client.Name, m.Name) && client.Version >= m.Version) == false
                             select new { m.Name,m.Type,m.Version}).ToArray();

            var uploads = (from m in clientEffect
                           where serverEffects.Any(server=>string.Equals(server.Name , m.Name) && server.Version >= m.Version) == false
                           select new { m.Name, m.Type }).ToArray();

            return new {
                downloads,
                uploads
            };
        }
    }
}
