using System;
using System.Collections.Generic;
using System.Text;
using Way.Lib.ScriptRemoting;
using System.Linq;

namespace PandaAudioServer
{
    public class AdminController : BaseController
    {
        public class RegInfo:SysDB.SystemRegCode
        {
            public string MakerName { get; set; }
        }

        public override int UserId
        {
            get
            {
                if (this.Session["AdminUserId"] == null)
                    throw new Exception("请先登录");
                return (int)this.Session["AdminUserId"];
            }
        }

        public IQueryable<RegInfo> RegCodes
        {
            get
            {
                if (this.UserId == 0)
                    return null;
                return from m in this.db.SystemRegCode
                       select new RegInfo {
                           id = m.id,
                           RegGuid = m.RegGuid,
                           MakerName = db.AdminUser.Where(u=>u.id == m.MakerUserId).Select(u=>u.UserName).FirstOrDefault(),
                           MakeTime = m.MakeTime
                       };
            }
        }

        /// <summary>
        /// 生成注册码
        /// </summary>
        /// <param name="count"></param>
        /// <returns></returns>
        [RemotingMethod(UseRSA = RSAApplyScene.EncryptParameters)]
        public bool MakeRegCode(int count)
        {
            for(int i = 0; i < count; i ++)
            {
                var regitem = new SysDB.SystemRegCode()
                {
                    MakerUserId = this.UserId,
                    RegGuid = Guid.NewGuid().ToString("N"),
                };
                this.db.Insert(regitem);
            }
            return true;
        }


        [RemotingMethod(UseRSA = RSAApplyScene.EncryptParameters)]
        public bool Login(string username, string password)
        {
            string ip = this.Request.RemoteEndPoint.ToString().Split(':')[0];
            if (IPError.IsIPLocked(ip))
                throw new Exception("禁止访问");

            var user = this.db.AdminUser.FirstOrDefault(m => m.UserName == username && m.Password == password);
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

            this.Session["AdminUserId"] = user.id;
            return true;
        }
    }
}
