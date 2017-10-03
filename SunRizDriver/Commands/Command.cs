using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SunRizDriver
{
    public class Command
    {
        public string Type;
        public int Interval = 1000;
        public string DeviceAddress;
        public string[] Points;
        public object[] Values;
        public List<string> ParentPath;
    }
}
