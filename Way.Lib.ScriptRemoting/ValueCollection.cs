using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Way.Lib.ScriptRemoting
{
    public class ValueCollection:Dictionary<string,string>
    {

        public string this[string key]
        {
            get
            {
                if (base.ContainsKey(key) == false)
                    return null;
                return base[key];
            }
            set
            {
                base[key] = value;
            }
        }

        public ValueCollection():base(StringComparer.OrdinalIgnoreCase)
        { 
        }
    }
}
