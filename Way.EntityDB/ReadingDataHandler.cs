using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Way.EntityDB
{
    //class MyReadingDataHandler : System.Data.Entity.IReadingDataHandler
    //{
    //    internal static System.Collections.ArrayList ReadingThreads = System.Collections.ArrayList.Synchronized(new System.Collections.ArrayList());
    //    public void EndReading()
    //    {
    //        var threadid = System.Threading.Thread.CurrentThread.ManagedThreadId;
    //        while (true)
    //        {
    //            try
    //            {
    //                int count = ReadingThreads.Count;
    //                for (int i = 0; i < count; i++)
    //                {

    //                    MyReadingDataHandlerState athread = ReadingThreads[i] as MyReadingDataHandlerState;
    //                    if (athread != null && athread.ThreadID == threadid)
    //                    {
    //                        foreach (var dataitem in athread.DataItems)
    //                            dataitem.m_notSendPropertyChanged = false;
    //                        athread.DataItems.Clear();
    //                        ReadingThreads.Remove(athread);
    //                        break;
    //                    }

    //                }
    //                break;
    //            }
    //            catch (System.ArgumentOutOfRangeException)
    //            {
    //                //如果索引超出范围，从头检查
    //                continue;
    //            }
    //        }
    //    }

    //    public void StartReading()
    //    {

    //        ReadingThreads.Add(new MyReadingDataHandlerState()
    //        {
    //            ThreadID = System.Threading.Thread.CurrentThread.ManagedThreadId,
    //        });

    //    }
    //}
    //class MyReadingDataHandlerState
    //{
    //    public int ThreadID;
    //    public List<DataItem> DataItems = new List<DataItem>();
    //}
    
}
