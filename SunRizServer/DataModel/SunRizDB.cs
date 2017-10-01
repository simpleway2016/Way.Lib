using System;
using System.Collections.Generic;
using System.Text;

namespace SunRizServer
{
    public class SunRizDB : DB.SunRiz
    {
        public SunRizDB():base("data source=\"D:/注释/2016/EasyJobCore/SunRizServer/SunRiz.db\"", Way.EntityDB.DatabaseType.Sqlite)
        {

        }
    }
}
