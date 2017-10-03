using System;
using System.Collections.Generic;
using System.Text;

namespace SunRizServer
{
    public class SysDB : DB.SunRiz
    {
        public SysDB():base(Config.ConnectionString,Config.DbType)
        {

        }
    }
}
