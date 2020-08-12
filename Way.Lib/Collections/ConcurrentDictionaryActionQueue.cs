using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;

namespace Way.Lib.Collections
{
    /// <summary>
    /// 按照key，将action分类成不同队列，每个队列用各自的Task执行action
    /// </summary>
    public class ConcurrentDictionaryActionQueue<TKey>:IDisposable
    {
        internal ConcurrentDictionary<TKey, ActionQueue<TKey>> _dict = new ConcurrentDictionary<TKey, ActionQueue<TKey>>();
        internal int ActionCount = 0;

        /// <summary>
        /// 移除指定key队列
        /// </summary>
        /// <param name="key"></param>
        internal void Remove(TKey key)
        {
            if (_dict.TryGetValue(key, out ActionQueue<TKey> item))
            {
                item._task?.Wait();
                _dict.TryRemove(key, out ActionQueue<TKey> o);
            }
        }

        /// <summary>
        /// 添加action到队列当中
        /// </summary>
        /// <param name="key"></param>
        /// <param name="action"></param>
        public void Add(TKey key , Action action)
        {
            if (_disposed)
                return;

            Interlocked.Increment(ref ActionCount);
            ActionQueue<TKey> queue = null;

            while (true)
            {
                if (_disposed)
                    return;

                queue = _dict.GetOrAdd(key, (k) => new ActionQueue<TKey>(k, this));

                if (queue.TryAddAction(action))
                {
                    break;
                }
            }
        }

        /// <summary>
        /// 等待队列执行完毕
        /// </summary>
        /// <param name="key"></param>
        /// <param name="millisecondsTimeout">The number of milliseconds to wait, or System.Threading.Timeout.Infinite (-1) to wait indefinitely.</param>
        public void Wait(TKey key,int millisecondsTimeout = System.Threading.Timeout.Infinite)
        {
            if( _dict.TryGetValue(key , out ActionQueue<TKey> o))
            {
                try
                {
                    o._task?.Wait(millisecondsTimeout);
                }
                catch
                {
                }
            }
        }
        /// <summary>
        /// 等待所有队列执行完毕
        /// </summary>
        /// <param name="millisecondsTimeout">The number of milliseconds to wait, or System.Threading.Timeout.Infinite (-1) to wait indefinitely.</param>
        public void WaitAll(int millisecondsTimeout = System.Threading.Timeout.Infinite)
        {
            DateTime start = DateTime.Now;
            while (/*_dict.Count > 0 || */this.ActionCount > 0)
            {
                Thread.Sleep(10);
                if (millisecondsTimeout > 0 && (DateTime.Now - start).TotalMilliseconds >= millisecondsTimeout)
                    return;
            }
        }

        bool _disposed;
        public void Dispose()
        {
            if (_disposed)
                return;
            _disposed = true;

            foreach (var item in _dict)
            {
                item.Value._task?.Wait();
            }
            _dict.Clear();
        }
    }

    class ActionQueue<TKey> 
    {
        ConcurrentQueue<Action> Actions { get; }
        internal Task _task;
        ManualResetEvent _waitObj = new ManualResetEvent(false);
        bool _disposed;

        public TKey Key { get; }
        ConcurrentDictionaryActionQueue<TKey> _container;
        public ActionQueue(TKey key, ConcurrentDictionaryActionQueue<TKey> container)
        {
            this.Key = key;
            this.Actions = new ConcurrentQueue<Action>();
            _container = container;
        }

        

        /// <summary>
        /// 添加一个任务到队列当中
        /// </summary>
        /// <param name="action"></param>
        public bool TryAddAction(Action action)
        {
            lock (this)
            {
                if (_disposed)
                {
                    return false;
                }

                this.Actions.Enqueue(action);
                if (_task == null)
                {
                    _task = Task.Run(run);
                }
            }
            return true;
        }

        void run()
        {
            //执行队列里面的任务
            while (true)
            {
                if (this.Actions.TryDequeue(out Action o))
                {
                    if (_disposed == true)
                    {

                    }
                    try
                    {
                        o();
                    }
                    catch
                    {
                    }
                    Interlocked.Decrement(ref _container.ActionCount);
                }
                else
                {
                    lock(this)
                    {
                        if( this.Actions.Count == 0 )
                        {
                            _disposed = true;

                            if (_container._dict.ContainsKey(Key))
                                _container._dict.TryRemove(Key, out ActionQueue<TKey> o2);
                            return;
                        }                       
                    }
                    
                }
            }
           
            
        }

       
    }
}
