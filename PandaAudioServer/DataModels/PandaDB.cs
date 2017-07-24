using System;
using System.Collections.Generic;
using System.Text;

namespace PandaAudioServer
{
    class PandaDB : db.DB.PandaAudio
    {
        public PandaDB():base("Server=192.168.50.128;Port=5432;UserId=postgres;Password=123456;database=pandaaudio;", Way.EntityDB.DatabaseType.PostgreSql)
        {

        }
    }
}
