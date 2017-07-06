﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Way.Lib.ScriptRemoting.WinTest
{
    [Way.Lib.ScriptRemoting.RemotingMethod]
    public class MyDB:EJ.DB.easyjob
    {
        public MyDB() : base($"Data Source=\"{AppContext.BaseDirectory}/EasyJob.db\"" ,  EntityDB.DatabaseType.Sqlite)
        {
        }
    }
}
