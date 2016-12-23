using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Way.EntityDB
{
    public class WayQueryable<T> : IQueryable<T>, IEnumerator<T>, System.Collections.IEnumerator,
        IOrderedQueryable<T>
    {
        class MyQueryable : IQueryable
        {
            public IQueryable m_source;
            public MyQueryable(IQueryable source)
            {
                m_source = source;
            }

            Type IQueryable.ElementType
            {
                get { return m_source.ElementType; }
            }

            System.Linq.Expressions.Expression IQueryable.Expression
            {
                get { return m_source.Expression; }
            }

            IQueryProvider _Provider;
            IQueryProvider IQueryable.Provider
            {
                get
                {
                    if (_Provider == null)
                        _Provider = new MyQueryProvider(m_source.Provider);
                    return _Provider;
                }
            }

            System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
            {
                return m_source.GetEnumerator();
            }
        }
        class MyQueryProvider : IQueryProvider
        {
            static Type basetype = typeof(EntityDB.DataItem);
            IQueryProvider m_provider;
            internal MyQueryProvider(IQueryProvider provider)
            {
                m_provider = provider;
            }
            IQueryable<T> IQueryProvider.CreateQuery<T>(System.Linq.Expressions.Expression expression)
            {
                IQueryable<T> query = m_provider.CreateQuery<T>(expression);
                TypeInfo t = typeof(T).GetTypeInfo();
                if (t.IsInterface || t.IsSubclassOf(basetype))
                    return new WayQueryable<T>(query);
                else
                    return query;
            }

            IQueryable IQueryProvider.CreateQuery(System.Linq.Expressions.Expression expression)
            {
                IQueryable query = m_provider.CreateQuery(expression);
                Type type = query.GetType();
                if (type.GetTypeInfo().IsGenericType)
                {
                    Type[] genericTypes = type.GetGenericArguments();
                    if (genericTypes.Length == 1)
                    {
                        Type wayqueryType = typeof(WayQueryable<>).MakeGenericType(type.GetGenericArguments());
                        return (IQueryable)Activator.CreateInstance(wayqueryType, new object[] { query });
                    }
                    else
                    {
                        return query;
                    }
                }
                else
                {
                    return new MyQueryable(m_provider.CreateQuery(expression));
                }
            }

            TResult IQueryProvider.Execute<TResult>(System.Linq.Expressions.Expression expression)
            {
                var item = m_provider.Execute<TResult>(expression);
                EntityDB.DataItem dataitem = item as EntityDB.DataItem;
                if (dataitem != null)
                    dataitem.ChangedProperties.Clear();
                return item;
            }

            object IQueryProvider.Execute(System.Linq.Expressions.Expression expression)
            {
                var item = m_provider.Execute(expression);
                EntityDB.DataItem dataitem = item as EntityDB.DataItem;
                if (dataitem != null)
                    dataitem.ChangedProperties.Clear();
                return item;
            }
        }

        IQueryable<T> m_source;

        IEnumerator<T> _MyEnumerator = null;
        IEnumerator<T> MyEnumerator
        {
            get
            {
                if (_MyEnumerator == null)
                    _MyEnumerator = m_source.GetEnumerator();
                return _MyEnumerator;
            }
        }
        public WayQueryable(IQueryable<T> source)
        {
            m_source = source;
        }

        //public T FirstOrDefault()
        //{
        //    var data = ((IQueryable<T>)this).FirstOrDefault();

        //    DataItem ditem = data as DataItem;
        //    if (ditem != null)
        //        ditem.ChangedProperties.Clear();


        //    return data;
        //}

        public IEnumerator<T> GetEnumerator()
        {
            return this;
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return this;
        }

        public Type ElementType
        {
            get { return m_source.ElementType; }
        }

        public System.Linq.Expressions.Expression Expression
        {
            get { return m_source.Expression; }
        }

        IQueryProvider _Provider;
        public IQueryProvider Provider
        {
            get
            {
                if (_Provider == null)
                    _Provider = new MyQueryProvider(m_source.Provider);
                return _Provider;
            }
        }

        object System.Collections.IEnumerator.Current
        {
            get
            {
                EntityDB.DataItem ditem = this.MyEnumerator.Current as EntityDB.DataItem;
                if (ditem != null)
                    ditem.ChangedProperties.Clear();
                return ditem;
            }
        }

        bool System.Collections.IEnumerator.MoveNext()
        {
            return this.MyEnumerator.MoveNext();
        }

        void System.Collections.IEnumerator.Reset()
        {
            this.MyEnumerator.Reset();
        }

        T IEnumerator<T>.Current
        {
            get
            {
                T obj = this.MyEnumerator.Current;
                EntityDB.DataItem ditem = obj as EntityDB.DataItem;
                if (ditem != null)
                    ditem.ChangedProperties.Clear();
                return obj;
            }
        }

        public override string ToString()
        {
            return m_source.ToString();
        }

        void IDisposable.Dispose()
        {
            if(_MyEnumerator != null)
                _MyEnumerator.Dispose();
            _MyEnumerator = null;
        }
    }
}
