using System;
using System.Collections.Generic;
using System.Text;

namespace OPC
{
    public class BaseCollection<T> : IList<T>, System.Collections.IList
    {
        private List<T> list = new List<T>();
        public BaseCollection()
        {
        }



        #region IList<T> Members

        public int IndexOf(T item)
        {
            return this.list.IndexOf(item);
        }

        public void Insert(int index, T item)
        {
            this.InsertItem(index, item);
        }

        protected virtual void InsertItem(int index, T item)
        {
            this.list.Insert(index, item);
        }

        protected virtual void RemoveItem(int index)
        {
            this.list.RemoveAt(index);
        }

        public void RemoveAt(int index)
        {
            this.RemoveItem(index);
        }

        public T this[int index]
        {
            get
            {
                return this.list[index];
            }
            set
            {
                this.list[index] = value;
            }
        }

        #endregion

        #region ICollection<T> Members

        public void Add(T item)
        {
            this.Insert(this.list.Count, item);
        }

        public void Clear()
        {
            this.ClearItems();
        }

        protected virtual void ClearItems()
        {
            this.list.Clear();
        }

        public bool Contains(T item)
        {
            return this.list.Contains(item);
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            this.list.CopyTo(array, arrayIndex);
        }

        public int Count
        {
            get { return this.list.Count; }
        }

        public bool IsReadOnly
        {
            get { return false; }
        }

        public bool Remove(T item)
        {
            for (int i = 0; i < this.Count; i++)
            {

                if (this[i].Equals(item))
                {
                    this.RemoveItem(i);
                    return true;
                }
            }
            return false;

        }

        #endregion

        #region IEnumerable<T> Members

        public IEnumerator<T> GetEnumerator()
        {
            return this.list.GetEnumerator();
        }

        #endregion

        #region IEnumerable Members

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return this.list.GetEnumerator();
        }

        #endregion



        #region IList Members

        public int Add(object value)
        {
            this.Add((T)value);
            return this.Count - 1;
        }

        public bool Contains(object value)
        {
            return this.Contains((T)value);
        }

        public int IndexOf(object value)
        {
            return this.IndexOf((T)value);
        }

        public void Insert(int index, object value)
        {
            this.Insert(index, (T)value);
        }

        public bool IsFixedSize
        {
            get { return true; }
        }

        public void Remove(object value)
        {
            this.Remove((T)value);
        }

        object System.Collections.IList.this[int index]
        {
            get
            {
                return this.list[index];
            }
            set
            {
                this.list[index] = (T)value;
            }
        }

        #endregion

        #region ICollection Members

        public void CopyTo(Array array, int index)
        {
            this.CopyTo((T[])array, index);
        }

        public bool IsSynchronized
        {
            get { return true; }
        }

        public object SyncRoot
        {
            get { return null; }
        }

        #endregion


    }
}
