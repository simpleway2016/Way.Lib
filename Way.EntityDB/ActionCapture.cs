using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Way.EntityDB
{
    public interface IActionCapture
    {
        Type DataItemType
        {
            get;
        }
        void BeforeDelete(object database, DatabaseModifyEventArg e);
        void BeforeInsert(object database, DatabaseModifyEventArg e);
        void BeforeUpdate(object database, DatabaseModifyEventArg e);

        void AfterDelete(object database, DatabaseModifyEventArg e);
        void AfterInsert(object database, DatabaseModifyEventArg e);
        void AfterUpdate(object database, DatabaseModifyEventArg e);
    }
    public abstract class ActionCapture<T> : IActionCapture
    {
        public ActionCapture()
        {
            
        }
        Type  _DataItemType;
        public Type DataItemType
        {
            get
            {
                if(_DataItemType == null)
                    _DataItemType =  typeof(T);
                return _DataItemType;
            }
        }
        public virtual void BeforeDelete(object database, DatabaseModifyEventArg e) { }
        public virtual void BeforeInsert(object database, DatabaseModifyEventArg e) { }
        public virtual void BeforeUpdate(object database, DatabaseModifyEventArg e) { }

        public virtual void AfterDelete(object database, DatabaseModifyEventArg e) { }
        public virtual void AfterInsert(object database, DatabaseModifyEventArg e) { }
        public virtual void AfterUpdate(object database, DatabaseModifyEventArg e) { }
    }

}
