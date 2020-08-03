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
    /// 按照key，将action分类成不同队列，每个队列用各自的线程执行action
    /// </summary>
    public class ConcurrentDictionaryActionQueue<TKey>:IDisposable
    {
        Dictionary<TKey, ActionQueue<TKey>> _dict = new Dictionary<TKey, ActionQueue<TKey>>();

        /// <summary>
        /// 移除指定key队列
        /// </summary>
        /// <param name="key"></param>
        internal void Remove(TKey key)
        {
            lock (_dict)
            {
                var item = _dict[key];
                if (item.Using)
                    return;

                _dict.Remove(key);
                item.Dispose();
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


            ActionQueue<TKey> queue;
            lock (_dict)
            {
                if (_disposed)
                    return;

                if ( _dict.TryGetValue(key, out queue) == false)
                {
                    _dict[key] = queue = new ActionQueue<TKey>( key , this);
                }
                queue.Using = true;
            }
            queue.AddAction(action);
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
            while (_dict.Count > 0)
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

            lock (_dict)
            {
                foreach (var item in _dict)
                {
                    item.Value.Dispose();
                }
                _dict.Clear();
            }
        }
    }

    class ActionQueue<TKey> : IDisposable
    {
        ConcurrentQueue<Action> Actions { get; }
        internal Task _task;

        public bool HasMission => this.Actions.Count > 0 || _task != null;
        bool _disposed;
        internal bool Using;
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
        public void AddAction(Action action)
        {
            if (_disposed)
                return;

            this.Actions.Enqueue(action);
            lock (this)
            {
                if (_task == null)
                {
                    _task = Task.Run(run);
                }
            }
        }

        void run()
        {
            //执行队列里面的任务
            while (!_disposed && this.Actions.TryDequeue(out Action o))
            {
                o();
            }
           
            lock (this)
            {
                if (!_disposed && this.Actions.Count > 0)
                {
                    _task = Task.Run(run);
                }
                else
                {
                    Using = false;
                    _task = null;
                    _container.Remove(this.Key);
                }
            }
        }

        public void Dispose()
        {
            try
            {
                _disposed = true;
                while (_task != null) //只有_exited = true时，才表示任务执行完毕，不会有任务执行了一半
                {
                    Thread.Sleep(100);
                }
            }
            catch
            {

            }
        }
    }
}
