using System;
using System.Collections.Generic;
using System.Text;

namespace PandaAudioServer
{
    class Factory
    {
        static Dictionary<Type, object> ExistServices = new Dictionary<Type, object>();

        public static void RegisterService<T>( object obj )
        {
            ExistServices.Add( typeof(T) , obj );
        }

        public static T GetService<T>()
        {
            var type = typeof(T);
            foreach( var key in ExistServices )
            {
                if (key.Key == type)
                    return (T)key.Value;
            }
            return default(T);
        }
    }
}
