using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Dynamic;
using System.Text;
using System.Linq;
using System.Reflection;
using System.Collections;

namespace Way.Lib
{
    public class DynamicModel : System.Dynamic.DynamicObject, INotifyPropertyChanged
    {
        Dictionary<string, object> _dict = new Dictionary<string, object>();
        Dictionary<string, List<string>> _depends = new Dictionary<string, List<string>>();
        object _source;
        Type _sourceType;
        MethodInfo[] _computedMethods;
        protected dynamic Self;
        public DynamicModel(object source):this()
        {
            _source = source;
            _sourceType = source?.GetType();
        }
        public DynamicModel()
        {
            Self = this;
            var methods = this.GetType().GetMethods(System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance);
            _computedMethods = (from m in methods
                                where m.DeclaringType != typeof(System.Dynamic.DynamicObject)
                                 && m.DeclaringType != typeof(DynamicModel)
                                  && m.DeclaringType != typeof(object)
                                select m).ToArray();
            foreach( var m in _computedMethods )
            {
                _depends[m.Name] = new List<string>();
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;
        /// <summary>
        /// 把集合转换为ObservableCollection类型
        /// </summary>
        /// <typeparam name="T">DynamicModel以及其派生类</typeparam>
        /// <param name="source"></param>
        /// <returns></returns>
        public static System.Collections.ObjectModel.ObservableCollection<T> GetObserve<T>(IList source)  where T : DynamicModel
        {
            Type t = typeof(T);
            var result = new System.Collections.ObjectModel.ObservableCollection<T>();
            foreach( var item in source )
            {
                if (item is T)
                    result.Add((T)item);
                else
                    result.Add((T)Activator.CreateInstance(t , item));
            }
            return result;
        }

        protected virtual void OnPropertyChanged(string name,object oldValue ,object newValue)
        {
            if(this.PropertyChanged != null)
            {
                this.PropertyChanged(this, new PropertyChangedEventArgs(name));
            }

            foreach( var depend in _depends )
            {
                if(depend.Value.Contains(name))
                {
                    this.OnPropertyChanged(depend.Key, null, null);
                }
            }
        }
        List<List<string>> _recordingDepends = new List<List<string>>();
        public override bool TryGetMember(GetMemberBinder binder, out object result)
        {
            if(_recordingDepends.Count > 0)
            {
                var recorder = _recordingDepends.Last();
                //记录依赖关系
                if(recorder.Contains(binder.Name) == false)
                {
                    recorder.Add(binder.Name);
                }
            }
            var computedMethod = _computedMethods.FirstOrDefault(m => m.Name == binder.Name);
            if(computedMethod != null)
            {
                _recordingDepends.Add(_depends[binder.Name]);
                try
                {
                    result = computedMethod.Invoke(this, null);
                }
                catch
                {
                    result = null;
                }
                _recordingDepends.RemoveAt(_recordingDepends.Count - 1);
                return true;
            }
            if(_dict.ContainsKey(binder.Name))
            {
                result = _dict[binder.Name];
                return true;
            }
            var fieldInfo = _sourceType?.GetField(binder.Name, System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public);
            if(fieldInfo == null)
            {
                result = null;
                return true;
            }
            result = fieldInfo.GetValue(_source);
            return true;
        }

        public override bool TrySetMember(SetMemberBinder binder, object value)
        {
            if (_dict.ContainsKey(binder.Name))
            {
                var original = _dict[binder.Name];
                if ((original == null && value != null) || original?.Equals(value) == false)
                {
                    _dict[binder.Name] = value;
                    this.OnPropertyChanged(binder.Name , original , value);
                }
                return true;
            }
            var fieldInfo = _sourceType?.GetField(binder.Name, System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public);
            if (fieldInfo == null)
            {
                _dict[binder.Name] = value;
                this.OnPropertyChanged(binder.Name , null , value);
                return true;
            }

            var originalValue = fieldInfo?.GetValue(_source);
            if ((originalValue == null && value != null) || originalValue?.Equals( value) == false)
            {
                fieldInfo.SetValue(_source, value);
                this.OnPropertyChanged(binder.Name, originalValue, value);
               
            }
            return true;
        }
    }

    
}
