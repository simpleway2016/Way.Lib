using System;
using System.Collections.Generic;
using System.Text;

namespace PandaAudioServer
{
    interface ISms
    {
        string Format(string content, params object[] parameters);
        void Send(string phoneNumber, string message);
    }
}
