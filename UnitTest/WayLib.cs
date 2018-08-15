using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;
using Way.Lib;

namespace UnitTest
{
    [TestClass]
    public class WayLib
    {
        class DYModel : DynamicModel
        {
            int getValue()
            {
                dynamic self = this;
                return self.age;
            }
        }
        [TestMethod]
        public void DynamicModel_test()
        {
            dynamic model = new DYModel();
            model.age = 2;
           var testv =  model.getValue;
            model.age = 3;
         
        }

        [TestMethod]
        public void DataModel_Rollback()
        {
            Model m = new Model();
           var originalJson =  Newtonsoft.Json.JsonConvert.SerializeObject(m);
            m.Name = "abc";
            m.Rollback();
            if (originalJson != Newtonsoft.Json.JsonConvert.SerializeObject(m))
                throw new Exception("error");
        }

        public static AsyncLocal<string> ThreadLocalResources = new AsyncLocal<string>();
        public static int ThreadLocal_Test_count;
        [TestMethod]
        public void ThreadLocal_Test()
        {
            ThreadLocal_Test_count = 0;
            for (int i = 0; i < 100; i ++)
            Task.Run(() => {
                if(ThreadLocalResources.Value == null)
                {
                    string guid = Guid.NewGuid().ToString();
                    ThreadLocalResources.Value = guid;
                    Interlocked.Increment(ref ThreadLocal_Test_count);

                    new TestClass().test(guid);
                }
                else
                {
                    
                }
            });
            Thread.Sleep(2000);
            if (ThreadLocal_Test_count != 100)
                throw new Exception("结果错误");
        }

        [TestMethod]
        public void TaskTest()
        {
           var dict = new ConcurrentDictionary<int, TestClass>();
            for (int i = 0; i < 100; i++)
            {
                Task.Run(() => {
                    int threadid = Thread.CurrentThread.ManagedThreadId; 
                    if(dict.ContainsKey(threadid) == false)
                    {
                        dict.TryAdd(threadid, new TestClass());
                    }
                    var obj = dict[threadid];
                    if(obj.Value % 10 != 0)
                    {
                        throw new Exception("error");
                    }
                    for(int j = 0; j < 1000; j ++)
                    {
                        //这里不用Interlock也没有问题，因为多个Task，如果属于同一个线程，那么，它们是一个执行完，再到另一个，不会有同时执行的情况
                        obj.Value++;
                    }
                });
            }

            Thread.Sleep(3000);
            int total = 0;
            foreach( var key in dict )
            {
                total += key.Value.Value;
            }
            if(total != 100*1000)
                throw new Exception("error");
        }

    }

 

    class TestClass
    {
        public int Value = 0;
        public void test(string guid)
        {
            new Thread(() => {
                if (WayLib.ThreadLocalResources.Value != guid)
                {
                    Interlocked.Increment(ref WayLib.ThreadLocal_Test_count);
                }
            }).Start();
            
        }
    }

    class Model : Way.Lib.DataModel
    {

        string _Name;
        public string Name
        {
            get
            {
                return _Name;
            }
            set
            {
                if (_Name != value)
                {
                    _Name = value;
                    this.OnPropertyChanged("Name", null, value);
                }
            }
        }
    }
}
