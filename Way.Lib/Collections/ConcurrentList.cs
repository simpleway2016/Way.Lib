using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace Way.Lib.Collections
{
    class ConcurrentListItem<T>
    {
        public int Used = 0;
        public T Data;

        public bool TryDelete()
        {
            if(Used == 1 )
            {
                if (Interlocked.CompareExchange(ref Used, 0, 1) == 1)
                {
                    this.Data = default(T);
                    return true;
                }
            }
            return false;
        }

        public bool TrySetData(T data)
        {
            if (Interlocked.CompareExchange(ref Used, 1, 0) == 0)
            {
                this.Data = data;
                return true;
            }
            return false;
        }
    }

    /// <summary>
    /// 线程安全的集合，并且集合更改，不会引起其他线程foreach等枚举操作异常
    /// </summary>
    public class ConcurrentList<T> : IEnumerable<T>
    {
        List<ConcurrentListItem<T>> _source;
        ConcurrentQueue<int> _freeQueue = new ConcurrentQueue<int>();
        
        public ConcurrentList()
        {
            _source = new List<ConcurrentListItem<T>>();
        }

        private int _Count;
        public int Count
        {
            get => _Count;
            set
            {
                if (_Count != value)
                {
                    _Count = value;
                }
            }
        }


        public void Clear()
        {
            lock (_source)
            {
                _source.Clear();
                _freeQueue = new ConcurrentQueue<int>();
            }
        }

        public void Remove(T item)
        {
            for (int i = 0; i < _source.Count; i++)
            {
                var s = _source[i];
                if (s != null && (object)s.Data == (object)item)
                {
                    if (_source[i].TryDelete())
                    {
                        _freeQueue.Enqueue(i);
                        Interlocked.Decrement(ref _Count);                       
                    }
                    return;
                }
            }
        }

        public void Add(T item)
        {
            while(_freeQueue.Count > 0)
            {
                if(_freeQueue.TryDequeue(out int index))
                {
                    var s = _source[index];
                    if (s.TrySetData(item))
                    {
                        Interlocked.Increment(ref _Count);
                        return;
                    }
                }
            }

            lock (_source)
            {
                _source.Add(new ConcurrentListItem<T>() { 
                    Data = item,
                    Used = 1
                });
            }
            Interlocked.Increment(ref _Count);
        }
        public void AddRange(IEnumerable<T> items)
        {
            foreach( var item in items)
            {
                this.Add(item);
            }
        }
              

        public IEnumerator<T> GetEnumerator()
        {
            return new ConcurrentListEnumerator<T>(this);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return new ConcurrentListEnumerator<T>(this);
        }


        class ConcurrentListEnumerator<T> : IEnumerator<T>
        {
            int _position = -1;
            ConcurrentList<T> _list;
            public ConcurrentListEnumerator(ConcurrentList<T> source)
            {
                _list = source;
            }

            public T Current
            {
                get
                {
                    if (_position == -1)
                        return default(T);

                    if (_position < _list._source.Count)
                    {
                        return _list._source[_position].Data;
                    }

                    return default(T);
                }
            }

            object IEnumerator.Current => this.Current;

            public void Dispose()
            {
                
            }

            public bool MoveNext()
            {
                _position++;
                while (_position < _list._source.Count && _list._source[_position] != null && _list._source[_position].Used == 0)
                {
                    _position++;
                }


                return _position < _list._source.Count;
            }

            public void Reset()
            {
                _position = -1;
            }
        }
    }

   
}
