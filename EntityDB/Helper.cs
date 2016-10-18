//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Reflection;
//using System.Text;
//using System.Threading.Tasks;

//namespace EntityDB
//{
//    class Helper
//    {
//        static bool inited = false;
//        static List<Type> _AppTypes = new List<Type>();
//        internal static List<Type> AppTypes
//        {
//            get
//            {
//                if (!inited)
//                {
//                    Assembly assembly = Assembly.GetEntryAssembly();
//                    if (assembly != null)
//                    {
//                        Init(assembly);
//                    }
//                    else
//                    {
//                        throw new Exception("必须在程序入口处，调用DBContext.Init方法，如 DBContext.Init(Assembly.GetExecutingAssembly());");
//                    }
//                }
//                return _AppTypes;
//            }

//        }
//        internal static void Init(System.Reflection.Assembly mainAssembly)
//        {
//            if (inited)
//                return;
//            inited = true;

//            var referencedAssemblies = mainAssembly.GetReferencedAssemblies();
//            foreach (var aName in referencedAssemblies)
//            {
//                try
//                {
//                    Assembly.Load(aName);
//                }
//                catch (Exception ex)
//                {
//                    throw new Exception("加载" + aName + "错误," + ex.Message);
//                }
//            }


//            //把所有dll的类型放入局部变量
//            System.Reflection.Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
//            foreach (System.Reflection.Assembly assembly in assemblies)
//            {
//                _AppTypes.AddRange(assembly.GetTypes());
//            }
//            AppDomain.CurrentDomain.AssemblyLoad += CurrentDomain_AssemblyLoad;
//        }

//        static void CurrentDomain_AssemblyLoad(object sender, AssemblyLoadEventArgs args)
//        {
//            _AppTypes.AddRange(args.LoadedAssembly.GetTypes());
//        }


//        /// <summary>
//        /// 查找实现了指定基类的类
//        /// </summary>
//        /// <typeparam name="T">基类</typeparam>
//        /// <returns></returns>
//        public static Type[] ViewImplBaseType<T>()
//        {
//            Type basetype = typeof(T);
//            List<Type> result = new List<Type>();
//            for (int i = 0; i < Helper.AppTypes.Count; i++)
//            {
//                Type type = Helper.AppTypes[i];
//                if (type.IsSubclassOf(basetype))
//                {
//                    result.Add(type);
//                }
//            }
//            return result.ToArray();
//        }

//        /// <summary>
//        /// 查看指定接口分别被哪些类实现了
//        /// </summary>
//        /// <typeparam name="T"></typeparam>
//        /// <returns></returns>
//        public static Type[] ViewInterfaceTypes<T>()
//        {
//            Type interfacetype = typeof(T);
//            List<Type> result = new List<Type>();
//            for (int i = 0; i < Helper.AppTypes.Count; i++)
//            {
//                Type type = Helper.AppTypes[i];
//                if (type.IsAbstract == false && type.GetInterface(interfacetype.FullName) != null)
//                {
//                    result.Add(type);
//                }
//            }
//            return result.ToArray();
//        }
//    }
//}
