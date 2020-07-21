using System;
using System.Collections;
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
    /// 线程安全的集合
    /// </summary>
    public class ConcurrentList<T> : IEnumerator<T>,IEnumerable<T>
    {
        List<ConcurrentListItem<T>> _source;
        int _position = -1;
        public ConcurrentList()
        {
            _source = new List<ConcurrentListItem<T>>();
        }

        public T Current
        {
            get
            {
                if(_position == -1)
                    return default(T);

                if (_position < _source.Count)
                {
                    return _source[_position].Data;
                }

                return default(T);
            }
        }

        object IEnumerator.Current => this.Current;

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

        public void Dispose()
        {
        }

        public void Clear()
        {
            for (int i = 0; i < _source.Count; i++)
            {
                if(_source[i].TryDelete())
                    Interlocked.Decrement(ref _Count);
            }
        }

        public void Remove(T item)
        {
            if (item == null)
                return;

            for (int i = 0; i < _source.Count; i++)
            {
                var s = _source[i];
                if (s != null && s.Used == 1 && (object)s.Data == (object)item)
                {
                    if (_source[i].TryDelete())
                    {
                        Interlocked.Decrement(ref _Count);                       
                    }
                    return;
                }
            }
        }

        public void Add(T item)
        {
            for (int i = 0; i < _source.Count; i++)
            {
                var s = _source[i];
                if (s != null && s.Used == 0)
                {
                    if( s.TrySetData(item))
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
        public bool MoveNext()
        {
            _position++;
            while (_position < _source.Count && _source[_position] != null && _source[_position].Used == 0)
            {
                _position++;
            }


            return _position < _source.Count;
        }

        public void Reset()
        {
            _position = -1;
        }

        public IEnumerator<T> GetEnumerator()
        {
            this.Reset();
            return this;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            this.Reset();
            return this;
        }
    }
}
