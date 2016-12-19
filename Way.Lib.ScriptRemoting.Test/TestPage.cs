using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;


namespace Way.Lib.ScriptRemoting.Test
{
    [RemotingMethod]
    public class TestPage : Way.Lib.ScriptRemoting.RemotingController,IUploadFileHandler
    {

        protected override string OnGetDataSourcePath(string datasourceName)
        {
            if (datasourceName == "grid")
            {
                return "Way.Lib.ScriptRemoting.WinTest.MyDB.DBColumn";
            }
            else if (datasourceName == "DBTable")
            {
                return "Way.Lib.ScriptRemoting.WinTest.MyDB.DBTable";
            }
            return null;
        }
        protected override void OnBeforeSavingData(object dataitem)
        {
            EJ.DBColumn column = dataitem as EJ.DBColumn;
            column.TableID = 3;
            this.SendMessage( Newtonsoft.Json.JsonConvert.SerializeObject(dataitem));
        }


        public object[] TestData
        {
            get
            {
                return new object[] {
                new {Index = 1,name="a1", child = new { name="child1"} },
                new {Index = 2 ,name="b2", child = new { name="child2"} },
                };
            }
        }


        public object[] TestData2
        {
            get
            {
                return new object[] {
                new {Index = 1,text="text1"  },
                new {Index = 2 ,text="text2" },
                };
            }
        }

        [RemotingMethod]
        public object getTestData()
        {
            List<object> arrs = new List<object>();
            for (int i = 0; i < 100; i++)
            {
                arrs.Add(new { Index = i,color= (i%2 == 0 ?"yellow":"#cccccc"), name = "a" + i, child = new { name = "child" + i } });
            }
            return arrs.ToArray();
        }

        [RemotingMethod]
        public int Test(int count)
        {
            return count;
        }

       public class Pager
        {
            public int index;
            public int size;
        }
        public class DataItem
        {
            public int Index;
            public string name;
        }
        [RemotingMethod]
        public DataItem[] getDatas(int p , Pager pager)
        {
            return new DataItem[] {
                new ScriptRemoting.Test.TestPage.DataItem() {Index = 1,name="a1" },
                new ScriptRemoting.Test.TestPage.DataItem() {Index = 2 ,name="b2"},
            };
        }

        [RemotingMethod]
        public string Test2(int count)
        {
            return DateTime.Now.ToString();
        }

        FileStream fs;
        public override IUploadFileHandler OnBeginUploadFile(string fileName, int fileSize,int offset)
        {
            if (File.Exists("d:\\aa\\" + fileName))
            {
                fs = File.OpenWrite("d:\\aa\\" + fileName);
                fs.Position = offset;
            }
            else
            {
                fs = File.Create("d:\\aa\\" + fileName);
            }
            return this;
        }
        protected override void OnLoad()
        {
            this.SetOnKeepAliveClose(()=>
            {
                Debug.WriteLine("KeepAlive Closed");
            });
            base.OnLoad();
        }

        int l = 0;
        void IUploadFileHandler.OnGettingFileData(byte[] data)
        {
            l += 10;
            fs.Write(data, 0, data.Length);
            fs.Flush();
        }

        void IUploadFileHandler.OnUploadFileCompleted()
        {
            fs.Dispose();
        }

        void IUploadFileHandler.OnUploadFileError()
        {
            fs.Dispose();
        }
    }
}