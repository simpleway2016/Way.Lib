using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace SunRizServer
{
    public class SysDB : DB.SunRiz
    {
        static SysDB()
        {
            using (var db = new SysDB())
            {
                if(db.UserInfo.Count() == 0)
                {
                    db.Insert(new UserInfo() {
                        Name = "sa",
                        Password = "1",
                        Role = UserInfo_RoleEnum.Admin
                    });
                }
            }
        }
        public SysDB():base(Config.ConnectionString,Config.DbType)
        {

        }
    }
}
