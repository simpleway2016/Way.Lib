using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Way.Lib
{
    public static class ResultHelper
    {
        public static object InvokeTake(object resultObj, int takeSize)
        {
            Type objectType = resultObj.GetType();
            Type dataType = null;

            if (objectType.IsArray)
            {
                dataType = objectType.GetElementType();
            }
            else
            {
                dataType = objectType.GetTypeInfo().GetGenericArguments()[0];
            }

            MethodInfo takeMethod = null;
            Type queryType = (resultObj is System.Linq.IQueryable) ? typeof(System.Linq.Queryable) : typeof(System.Linq.Enumerable);

            var methods = queryType.GetTypeInfo().GetMethods(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static).Where(m => m.Name == "Take");
            foreach (System.Reflection.MethodInfo method in methods)
            {
                if (method.IsGenericMethod)
                {
                    takeMethod = method;
                    break;
                }

            }

            if (takeMethod == null)
                throw new Exception("找不到泛型Take方法");

            System.Reflection.MethodInfo mmm = takeMethod.MakeGenericMethod(dataType);
            if (mmm != null)
            {
                return mmm.Invoke(null, new object[] { resultObj, takeSize });
            }
            else
                throw new Exception(takeMethod.Name + ".MakeGenericMethod失败，参数类型：" + dataType.FullName);
        }

        public static object InvokeSkip(object resultObj, int skip)
        {
            Type objectType = resultObj.GetType();
            Type dataType = null;

            if (objectType.IsArray)
            {
                dataType = objectType.GetElementType();
            }
            else
            {
                dataType = objectType.GetTypeInfo().GetGenericArguments()[0];
            }

            MethodInfo skipMethod = null;
            Type queryType = (resultObj is System.Linq.IQueryable) ? typeof(System.Linq.Queryable) : typeof(System.Linq.Enumerable);

            var methods = queryType.GetTypeInfo().GetMethods(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static).Where(m => m.Name == "Skip");
            foreach (System.Reflection.MethodInfo method in methods)
            {
                if (method.IsGenericMethod)
                {
                    skipMethod = method;
                    break;
                }

            }
            if (skipMethod == null)
                throw new Exception("找不到泛型Skip方法");

            System.Reflection.MethodInfo mmm = skipMethod.MakeGenericMethod(dataType);
            if (mmm != null)
            {
                return mmm.Invoke(null, new object[] { resultObj, skip });
            }
            else
                throw new Exception(skipMethod.Name + ".MakeGenericMethod失败，参数类型：" + dataType.FullName);
        }

        public static object InvokeSum(object linqQuery)
        {
            Type objectType = linqQuery.GetType();
            Type dataType = null;

            if (objectType.IsArray)
            {
                dataType = objectType.GetElementType();
            }
            else
            {
                dataType = objectType.GetTypeInfo().GetGenericArguments()[0];
            }

            Type myType = (linqQuery is System.Linq.IQueryable) ? typeof(System.Linq.Queryable) : typeof(System.Linq.Enumerable);
            var methods = myType.GetTypeInfo().GetMethods(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static);
            foreach (System.Reflection.MethodInfo method in methods)
            {
                if (method.Name != "Sum" || method.IsGenericMethod || method.ReturnType != dataType)
                    continue;
                if (method.GetParameters().Length != 1)
                    continue;

                return method.Invoke(null, new object[] { linqQuery });

            }
            return null;
        }

        public static object InvokeCount(object linqQuery)
        {
            Type objectType = linqQuery.GetType();
            Type dataType = null;

            if (objectType.IsArray)
            {
                dataType = objectType.GetElementType();
            }
            else
            {
                dataType = objectType.GetTypeInfo().GetGenericArguments()[0];
            }

            Type myType = (linqQuery is System.Linq.IQueryable) ? typeof(System.Linq.Queryable) : typeof(System.Linq.Enumerable);
            var methods = myType.GetMethods(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static).Where(m => m.Name == "Count");
            foreach (System.Reflection.MethodInfo method in methods)
            {
                if (method.GetParameters().Length == 1)
                {
                    System.Reflection.MethodInfo mmm = method.MakeGenericMethod(dataType);
                    if (mmm != null)
                    {
                        return mmm.Invoke(null, new object[] { linqQuery });
                    }
                }
            }
            return null;
        }


        public static object InvokeSelect(object linqQuery, string[] propertyNames)
        {
            Type objectType = linqQuery.GetType();
            Type dataType = objectType.GetTypeInfo().GetGenericArguments()[0];
            ParameterExpression param = System.Linq.Expressions.Expression.Parameter(dataType, "n");

            System.Linq.Expressions.Expression left = CreateSelectExpression(param,propertyNames);


            System.Linq.Expressions.Expression expression = System.Linq.Expressions.Expression.Lambda(left, param);

            Type myType = (linqQuery is System.Linq.IQueryable) ? typeof(System.Linq.Queryable) : typeof(System.Linq.Enumerable);

            var methods = myType.GetTypeInfo().GetMethods(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static);
            foreach (System.Reflection.MethodInfo method in methods)
            {
                if (method.Name != "Select" || method.IsGenericMethod == false)
                    continue;
                System.Reflection.MethodInfo mmm = method.MakeGenericMethod(dataType, dataType);
                return mmm.Invoke(null, new object[] { linqQuery, expression });

            }
            return null;
        }

        public static object InvokeSelect(object linqQuery, string propertyName)
        {
            Type objectType = linqQuery.GetType();
            Type dataType = null;

            if (objectType.IsArray)
            {
                object[] arr = (object[])linqQuery;
                if (arr.Length == 0)
                {
                    return null;
                }
                dataType = arr[0].GetType();
                object[] result = new object[arr.Length];
                for (int i = 0; i < arr.Length; i++)
                {
                    result[i] = arr[i].GetType().GetTypeInfo().GetProperty(propertyName).GetValue(arr[i]);
                }
                return result;
            }
            else
            {
                dataType = objectType.GetTypeInfo().GetGenericArguments()[0];
            }
            ParameterExpression param = System.Linq.Expressions.Expression.Parameter(dataType, "n");
            PropertyInfo pinfo;
            System.Linq.Expressions.Expression left = GetPropertyExpression(param, dataType, propertyName, out pinfo);


            System.Linq.Expressions.Expression expression = System.Linq.Expressions.Expression.Lambda(left, param);

            Type myType = (linqQuery is System.Linq.IQueryable) ? typeof(System.Linq.Queryable) : typeof(System.Linq.Enumerable);

            var methods = myType.GetTypeInfo().GetMethods(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static);
            foreach (System.Reflection.MethodInfo method in methods)
            {
                if (method.Name != "Select" || method.IsGenericMethod == false)
                    continue;
                System.Reflection.MethodInfo mmm = method.MakeGenericMethod(dataType, pinfo.PropertyType);
                return mmm.Invoke(null, new object[] { linqQuery, expression });

            }
            return null;
        }
        public static object InvokeToArray(object resultObj)
        {
            Type objectType = resultObj.GetType();
            Type dataType = null;

            if (objectType.IsArray)
            {
                dataType = objectType.GetElementType();
            }
            else
            {
                dataType = objectType.GetTypeInfo().GetGenericArguments()[0];
            }

            MethodInfo toArrayMethod = null;
            Type queryType = typeof(System.Linq.Enumerable);

            var methods = queryType.GetTypeInfo().GetMethods(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static).Where(m => m.Name == "ToArray");
            foreach (System.Reflection.MethodInfo method in methods)
            {
                if (method.IsGenericMethod)
                {
                    toArrayMethod = method;
                    break;
                }
            }

            if (toArrayMethod == null)
                throw new Exception("找不到泛型ToArray方法");

            System.Reflection.MethodInfo mmm = toArrayMethod.MakeGenericMethod(dataType);
            if (mmm != null)
            {
                return mmm.Invoke(null, new object[] { resultObj });
            }
            else
                throw new Exception(toArrayMethod.Name + ".MakeGenericMethod失败，参数类型：" + dataType.FullName);
        }

        public static Expression CreateSelectExpression(ParameterExpression n, string[] fields)
        {
            string[] newFields = new string[fields.Length];
            for (int i = 0; i < fields.Length; i++)
            {
                if (fields[i].Contains(":"))
                {
                    newFields[i] = fields[i].Substring(0, fields[i].IndexOf(":"));
                }
                else
                {
                    newFields[i] = fields[i];
                }
            }
            fields = newFields;
            Type targetType = n.Type;
            List<MemberAssignment> binds = new List<MemberAssignment>();
            List<string> existsProNames = new List<string>(fields.Length);
            foreach (string field in fields)
            {
                PropertyInfo property;
                if (field.Contains("."))
                {
                    property = targetType.GetTypeInfo().GetProperty(field.Substring(0 , field.IndexOf(".")));
                }
                else
                {
                    property = targetType.GetTypeInfo().GetProperty(field);
                }
                if (property == null)
                {
                    throw new Exception($"{targetType.FullName}不包含属性{field}");
                }
                if (existsProNames.Contains(property.Name))
                    continue;

                existsProNames.Add(property.Name);
                binds.Add(Expression.Bind(property, Expression.Property(n, property)));
            }

            Expression body = Expression.MemberInit(Expression.New(targetType), binds.ToArray());
            return body;

        }
        public static System.Linq.Expressions.Expression GetPropertyExpression(ParameterExpression param, Type dataType, string propertyName, out PropertyInfo propertyInfo)
        {
            System.Linq.Expressions.Expression left = null;
            string[] dataFieldArr = propertyName.Split('.');
            System.Linq.Expressions.Expression lastObjectExpress = param;
            Type currentObjType = dataType;
            propertyInfo = null;
            for (int i = 0; i < dataFieldArr.Length; i++)
            {
                propertyInfo = currentObjType.GetTypeInfo().GetProperty(dataFieldArr[i], BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
                if (propertyInfo == null)
                    throw new Exception("属性" + dataFieldArr[i] + "无效");
                left = System.Linq.Expressions.Expression.Property(lastObjectExpress, propertyInfo);
                if (i < dataFieldArr.Length - 1)
                {
                    currentObjType = propertyInfo.PropertyType;
                    lastObjectExpress = left;
                }
            }
            return left;
        }

        public static object InvokeFirstOrDefault(object linqQuery)
        {
            Type objectType = linqQuery.GetType();
            Type dataType = null;

            if (objectType.IsArray)
            {
                dataType = objectType.GetElementType();
            }
            else
            {
                dataType = objectType.GetTypeInfo().GetGenericArguments()[0];
            }
            Type myType = (linqQuery is System.Linq.IQueryable) ? typeof(System.Linq.Queryable) : typeof(System.Linq.Enumerable);
            var methods = myType.GetTypeInfo().GetMethods(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static);
            foreach (System.Reflection.MethodInfo method in methods)
            {
                if (method.Name != "FirstOrDefault" || method.IsGenericMethod == false)
                    continue;
                System.Reflection.MethodInfo mmm = method.MakeGenericMethod(dataType);
                return mmm.Invoke(null, new object[] { linqQuery });

            }
            return null;
        }

        public static object GetPropertyValue(object obj , string propertyName)
        {
            string[] dataFieldArr = propertyName.Split('.');
            Type currentObjType = obj.GetType();
            PropertyInfo propertyInfo = null;
            object currentValue = obj;
            for (int i = 0; i < dataFieldArr.Length; i++)
            {
                propertyInfo = currentObjType.GetTypeInfo().GetProperty(dataFieldArr[i], BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
                if (propertyInfo == null)
                    return null;
                currentValue = propertyInfo.GetValue(currentValue);
                if (currentValue == null)
                    return null;
                if (i < dataFieldArr.Length - 1)
                {
                    currentObjType = propertyInfo.PropertyType;
                }
            }
            return currentValue;
        }
        public static string ToJson(Dictionary<string,object>[] obj)
        {
            StringBuilder str = new StringBuilder();
            str.Append('[');
            for(int i = 0; i < obj.Length; i ++)
            {
                if(i > 0)
                {
                    str.Append(',');
                }
                ToJson(str, obj[i]);
            }
            str.Append(']');
            return str.ToString();
        }

        static void ToJson(StringBuilder str  ,Dictionary<string, object> obj)
        {
            str.Append('{');
            bool first = true;
             foreach ( KeyValuePair<string,object> item in obj )
            {
                if(!first)
                {
                    str.Append(',');
                }
                first = false;
                str.Append('\"');
                str.Append(item.Key);
                str.Append('\"');
                str.Append(':');
                if(item.Value is Dictionary<string, object>)
                {
                    ToJson(str, (Dictionary<string, object>)item.Value);
                }
                else
                {
                    str.Append(Newtonsoft.Json.JsonConvert.SerializeObject(item.Value));
                }
                
            }
            str.Append('}');
        }
        static void setJsonValue(Dictionary<string, object> jsonData, string propertyName, object value)
        {
            if (jsonData.ContainsKey(propertyName) && jsonData[propertyName] is Dictionary<string, object>)
            {
                SetJsonDataValue(jsonData, propertyName + ".value", value);
            }
            else
            {
                jsonData[propertyName] = value;
            }
        }
        public static void SetJsonDataValue(Dictionary<string,object> jsonData , string propertyName,object value)
        {
            if(propertyName.Contains(".") == false)
            {
                setJsonValue(jsonData, propertyName, value);
                return;
            }
            string[] dataFieldArr = propertyName.Split('.');
            for (int i = 0; i < dataFieldArr.Length; i++)
            {
                var name = dataFieldArr[i];
                if (i < dataFieldArr.Length - 1)
                {
                    Dictionary<string, object> newJsonData = null;
                    if (jsonData.ContainsKey(name) && jsonData[name] is Dictionary<string, object>)
                        newJsonData = (Dictionary<string, object>)jsonData[name];
                    else
                    {
                        newJsonData = new Dictionary<string, object>();
                        jsonData[name] = newJsonData;
                    }
                   
                    jsonData = newJsonData;
                }
                else
                {
                    setJsonValue(jsonData, name, value);
                }
            }
        }

        public static object InvokeWhereEquals(object linqQuery, string propertyName, object value)
        {
            Type objectType = linqQuery.GetType();
            Type dataType = null;

            if (objectType.IsArray)
            {
                object[] arr = (object[])linqQuery;
                List<object> result = new List<object>();
                foreach (var item in arr)
                {
                    if( item.GetType().GetTypeInfo().GetProperty(propertyName).GetValue(item).Equals( value) )
                    {
                        result.Add(item);
                    }
                }
                return result.ToArray();
            }
            else
            {
                dataType = objectType.GetTypeInfo().GetGenericArguments()[0];
            }

            ParameterExpression param = System.Linq.Expressions.Expression.Parameter(dataType, "n");
            System.Reflection.PropertyInfo pinfo;
            System.Linq.Expressions.Expression left, right;

            left = GetPropertyExpression(param, dataType, propertyName, out pinfo);
            if (pinfo.PropertyType.GetTypeInfo().IsGenericType)
            {
                Type ptype = pinfo.PropertyType.GetTypeInfo().GetGenericArguments()[0];
                left = System.Linq.Expressions.Expression.Convert(left, ptype);
                //等式右边的值
                right = System.Linq.Expressions.Expression.Constant(Convert.ChangeType(value, ptype));
            }
            else
            {
                //等式右边的值
                right = System.Linq.Expressions.Expression.Constant(Convert.ChangeType(value, pinfo.PropertyType));
            }

            System.Linq.Expressions.Expression expression = System.Linq.Expressions.Expression.Equal(left, right);
            expression = System.Linq.Expressions.Expression.Lambda(expression, param);

            Type queryableType = (linqQuery is System.Linq.IQueryable) ? typeof(System.Linq.Queryable) : typeof(System.Linq.Enumerable);
            var methods = queryableType.GetTypeInfo().GetMethods(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static);
            foreach (System.Reflection.MethodInfo method in methods)
            {
                if (method.Name == "Where" && method.IsGenericMethod)
                {
                    System.Reflection.MethodInfo mmm = method.MakeGenericMethod(dataType);
                    return mmm.Invoke(null, new object[] { linqQuery, expression });
                }
            }
            return null;
        }
    }
}
