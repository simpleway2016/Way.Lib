using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Way.Lib.ScriptRemoting
{
    public interface IUploadFileHandler
    {
        void OnGettingFileData(byte[] data);
        void OnUploadFileCompleted();
        void OnUploadFileError();
    }
    /// <summary>
    /// 分页信息定义
    /// </summary>
    public class PagerInfo
    {
        public int PageIndex;
        public int PageSize;
    }

    /// <summary>
    /// 
    /// </summary>
    public class RemotingController
    {
        internal string SocketID;
        public SessionState Session
        {
            get;
            internal set;
        }

        /// <summary>
        /// 获取当前上下文对应的Session
        /// </summary>
        /// <returns></returns>
        public static SessionState GetCurrentSession()
        {
            var thread = System.Threading.Thread.CurrentThread;
            if (SessionState.ThreadSessions.ContainsKey(thread))
                return (SessionState)SessionState.ThreadSessions[thread];
            else
                return null;
        }

        internal void onLoad()
        {
            this.OnLoad();
        }
        protected virtual void OnLoad()
        {
        }


        Type getType(string fullname)
        {
            Assembly currentAssembly = this.GetType().GetTypeInfo().Assembly;
            Type type = null;
            if (DataSourceTypes.ContainsKey(fullname))
            {
                type = DataSourceTypes[fullname];
            }
            else
            {
                type = currentAssembly.GetType(fullname);
                if (type == null)
                {
                    Assembly[] assemblies = PlatformHelper.GetAppAssemblies();
                    foreach (var assembly in assemblies)
                    {
                        if (assembly != currentAssembly)
                        {
                            type = assembly.GetType(fullname);
                            if (type != null)
                                break;
                        }

                    }
                }
                if (type == null)
                {
                    throw new Exception("无法找到类" + fullname);
                }
                else if (type.GetTypeInfo().GetCustomAttribute(typeof(RemotingMethodAttribute)) == null)
                {
                    throw new Exception(fullname + "无权访问");
                }
                try
                {
                    DataSourceTypes.Add(fullname, type);
                }
                catch
                {
                }
            }
            return type;
        }

        static void getSearchingDictionary(Dictionary<string, object> dic,string root , Newtonsoft.Json.Linq.JObject searchModel)
        {
            foreach (var jobj in searchModel)
            {
                string name = jobj.Key;
                var valueObj = jobj.Value;
                if (valueObj is Newtonsoft.Json.Linq.JValue)
                {
                    var value = ((Newtonsoft.Json.Linq.JValue)valueObj).Value;
                    if (value != null)
                    {
                        if (value is string)
                        {
                            string strvalue = value.ToString().Trim();
                            if (strvalue.Length > 0)
                            {
                                dic.Add(root + name, strvalue);
                            }
                        }
                        else
                        {
                            dic.Add(root + name, value);
                        }
                    }
                }
                else if (valueObj is Newtonsoft.Json.Linq.JObject)
                {
                    getSearchingDictionary(dic, name + ".", valueObj as Newtonsoft.Json.Linq.JObject);
                }
            }
        }

        static object search(object result, Type dataItemType, Newtonsoft.Json.Linq.JObject searchModel)
        {
            Dictionary<string, object> dic = new Dictionary<string, object>();
            getSearchingDictionary(dic,"", searchModel);
            if (dic.Count == 0)
                return result;

            ParameterExpression param = Expression.Parameter(dataItemType, "n");
            Expression totalExpression = null;
            foreach (var searchKeyPair in dic)
            {

                PropertyInfo pinfo;
                Expression left = ResultHelper.GetPropertyExpression(param, dataItemType, searchKeyPair.Key, out pinfo);
                Type ptype = pinfo.PropertyType;

                if (ptype.GetTypeInfo().IsGenericType)
                {
                    ptype = ptype.GetGenericArguments()[0];
                    left = Expression.Convert(left, ptype);
                }

                if (ptype == typeof(string))
                {
                    left = Expression.Call(left,
                        typeof(string).GetMethod("Contains", new Type[] { typeof(string) }),
                        Expression.Constant(searchKeyPair.Value));
                }
                else if (ptype == typeof(int) || ptype == typeof(double) || ptype == typeof(decimal) || ptype == typeof(float) || ptype == typeof(short) || ptype == typeof(long))
                {
                    string value = searchKeyPair.Value.ToString();
                    if (value.StartsWith(">="))
                    {
                        value = value.Replace(">=", "");
                        //等式右边的值
                        Expression right = Expression.Constant(Convert.ChangeType(value, ptype));
                        left = Expression.GreaterThanOrEqual(left, right);
                    }
                    else if (value.StartsWith(">"))
                    {
                        value = value.Replace(">", "");
                        //等式右边的值
                        Expression right = Expression.Constant(Convert.ChangeType(value, ptype));
                        left = Expression.GreaterThan(left, right);
                    }
                    else if (value.StartsWith("<="))
                    {
                        value = value.Replace("<=", "");
                        //等式右边的值
                        Expression right = Expression.Constant(Convert.ChangeType(value, ptype));
                        left = Expression.LessThanOrEqual(left, right);
                    }
                    else if (value.StartsWith("<"))
                    {
                        value = value.Replace("<", "");
                        //等式右边的值
                        Expression right = Expression.Constant(Convert.ChangeType(value, ptype));
                        left = Expression.LessThan(left, right);
                    }
                    else
                    {
                        //等式右边的值
                        try
                        {
                            value = value.Replace("=", "");
                            Expression right = Expression.Constant(Convert.ChangeType(value, ptype));
                            left = Expression.Equal(left, right);
                        }
                        catch
                        {
                            continue;
                        }
                    }
                }
                //else if (control is System.Web.UI.WebControls.ListControl || control is TextBoxList || control is ParentToChildSelector)
                //{
                //    if (ptype.IsEnum)
                //    {
                //        //等式右边的值
                //        Expression right = Expression.Constant(Enum.Parse(ptype, value));
                //        left = Expression.Equal(left, right);
                //    }
                //    else
                //    {
                //        //等式右边的值
                //        Expression right = Expression.Constant(Convert.ChangeType(value, ptype));
                //        left = Expression.Equal(left, right);
                //    }


                //}
                //else if (control.Attributes["_IsDate"] == "1")
                //{
                //    if (control.Attributes["_IsFrom"] == "1")
                //    {
                //        if (control.Attributes["_IsTo"] == "1")
                //        {
                //            //应该是指定月份，或者年份
                //            var ms = System.Text.RegularExpressions.Regex.Matches(value, @"[0-9]+");
                //            if (ms.Count == 1)
                //            {
                //                //只有年份
                //                Expression expression1;

                //                //等式右边的值
                //                value = string.Format("{0}-1-1", ms[0]);
                //                Expression right = Expression.Constant(Convert.ChangeType(value, ptype));
                //                expression1 = Expression.GreaterThanOrEqual(left, right);

                //                value = string.Format("{0}-1-1", ms[0].Value.ToInt() + 1);
                //                right = Expression.Constant(Convert.ChangeType(value, ptype));
                //                right = Expression.LessThan(left, right);

                //                left = Expression.And(expression1, right);
                //            }
                //            else if (ms.Count == 2)
                //            {
                //                //有月份
                //                DateTime monthdate = Convert.ToDateTime(value);

                //                Expression expression1;
                //                //等式右边的值
                //                value = monthdate.ToString("yyyy-MM-01");
                //                Expression right = Expression.Constant(Convert.ChangeType(value, ptype));
                //                expression1 = Expression.GreaterThanOrEqual(left, right);

                //                value = monthdate.AddMonths(1).ToString("yyyy-MM-01");
                //                right = Expression.Constant(Convert.ChangeType(value, ptype));
                //                right = Expression.LessThan(left, right);

                //                left = Expression.And(expression1, right);
                //            }
                //            else if (ms.Count == 3)
                //            {
                //                //有日
                //                DateTime day = Convert.ToDateTime(value);
                //                Expression expression1;
                //                //等式右边的值
                //                value = day.ToString("yyyy-MM-dd");
                //                Expression right = Expression.Constant(Convert.ChangeType(value, ptype));
                //                expression1 = Expression.GreaterThanOrEqual(left, right);

                //                value = day.AddDays(1).ToString("yyyy-MM-dd");
                //                right = Expression.Constant(Convert.ChangeType(value, ptype));
                //                right = Expression.LessThan(left, right);

                //                left = Expression.AndAlso(expression1, right);
                //            }
                //        }
                //        else
                //        {
                //            //等式右边的值
                //            Expression right = Expression.Constant(Convert.ChangeType(value, ptype));
                //            left = Expression.GreaterThanOrEqual(left, right);
                //        }
                //    }
                //    else
                //    {
                //        if (value.Contains(":"))
                //        {
                //            value = value.ToDateTime().AddSeconds(1).ToString("yyyy-MM-dd HH:mm:ss");
                //        }
                //        else
                //        {
                //            value = value.ToDateTime().ToString("yyyy-MM-dd").ToDateTime().AddDays(1).ToString("yyyy-MM-dd");
                //        }
                //        //等式右边的值
                //        Expression right = Expression.Constant(Convert.ChangeType(value, ptype));
                //        left = Expression.LessThan(left, right);
                //    }
                //}

                if (left != null)
                {
                    //left = Expression.Lambda(left, param);
                    if (totalExpression == null)
                        totalExpression = left;
                    else
                        totalExpression = Expression.AndAlso(totalExpression, left);
                }
            }

            Type queryableType = (result is System.Linq.IQueryable) ? typeof(System.Linq.Queryable) : typeof(System.Linq.Enumerable);
            if (totalExpression != null)
            {
                totalExpression = Expression.Lambda(totalExpression, param);
                var methods = queryableType.GetMethods(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static).Where(m => m.Name == "Where");
                foreach (System.Reflection.MethodInfo method in methods)
                {
                    try
                    {
                        System.Reflection.MethodInfo mmm = method.MakeGenericMethod(dataItemType);
                        result = mmm.Invoke(null, new object[] { result, totalExpression });
                        break;
                    }
                    catch (Exception ex)
                    {
                    }
                }
            }
            return result;
        }

        static Dictionary<string, Type> DataSourceTypes = new Dictionary<string, Type>();
        protected virtual void OnGettingDataSource(string fullname,string target)
        {
        }
        /// <summary>
        /// 通用获取数据的函数
        /// </summary>
        /// <param name="pagerInfo">分页信息</param>
        /// <param name="target">数据源，如：Project1.DBContext1.Datas，表示把Project1.DBContext1的Datas属性作为数据源</param>
        /// <param name="fields">需要绑定的字段</param>
        /// <param name="searchJsonStr">搜索条件的model</param>
        /// <returns></returns>
        [RemotingMethod]
        public object GetDataSource( PagerInfo pagerInfo ,  string target, string[] fields,string searchJsonStr)
        {
           
            int index = target.LastIndexOf(".");
            string propertyName = target.Substring(index + 1);
            string fullname = target.Substring(0, index);

            this.OnGettingDataSource(fullname, propertyName);

            string pkid = null;//主键值
            Type type = getType(fullname);
            Dictionary<string, object> activeObjs = new Dictionary<string, object>();
            try
            {
                object obj = Activator.CreateInstance(type);
                activeObjs[fullname] = obj;

                object[] arrResult = null;
                object result = null;

                if (propertyName.EndsWith("()"))
                {
                    var method = type.GetMethod(propertyName.Substring(0, propertyName.Length - 2));
                    if (method.GetCustomAttribute(typeof(RemotingMethodAttribute)) == null)
                    {
                        throw new Exception(propertyName + "函数不是RemotingMethod");
                    }
                    result = method.Invoke(obj, null);
                }
                else
                {
                    result = type.GetProperty(propertyName).GetValue(obj);
                }

                Type dataItemType = result.GetType();
                if (dataItemType.IsArray)
                {
                    dataItemType = dataItemType.GetElementType();
                }
                else if (dataItemType.GetTypeInfo().IsGenericType)
                {
                    dataItemType = dataItemType.GetGenericArguments()[0];
                }

                if (pagerInfo.PageIndex == 0)
                {
                    //获取主键名
                    if (dataItemType.GetTypeInfo().IsSubclassOf(typeof(EntityDB.DataItem)))
                    {
                        EntityDB.Attributes.Table tableatt = dataItemType.GetTypeInfo().GetCustomAttribute(typeof(EntityDB.Attributes.Table)) as EntityDB.Attributes.Table;
                        if (tableatt != null)
                        {
                            pkid = tableatt.IDField;
                            if (fields.Contains(pkid) == false)
                            {
                                var list = new List<string>(fields);
                                list.Add(pkid);
                                fields = list.ToArray();
                            }
                        }
                    }
                }
                if (searchJsonStr.IsNullOrEmpty() == false)
                {
                    var searchModel = (Newtonsoft.Json.Linq.JObject)Newtonsoft.Json.JsonConvert.DeserializeObject(searchJsonStr);
                    result = search(result, dataItemType, searchModel);
                }
                if (result is IQueryable)
                {
                    result = ResultHelper.InvokeSelect(result, fields);
                }
                result = ResultHelper.InvokeSkip(result, pagerInfo.PageIndex * pagerInfo.PageSize);
                result = ResultHelper.InvokeTake(result, pagerInfo.PageSize);
                arrResult = (object[])ResultHelper.InvokeToArray(result);

                List<Dictionary<string, object>> finalResult = new List<Dictionary<string, object>>();
                foreach (var dataitem in arrResult)
                {
                    Dictionary<string, object> jData = new Dictionary<string, object>();
                    foreach (string field in fields)
                    {
                        if (field.Contains(":"))
                        {
                            string[] fieldInfo = field.Split(':');
                            string[] memeberInfo = fieldInfo[1].Split('.');

                            object fieldValue = ResultHelper.GetPropertyValue(dataitem, fieldInfo[0]);
                            if (fieldValue == null)
                            {
                                ResultHelper.SetJsonDataValue(jData, fieldInfo[0] + ".value", "");
                                ResultHelper.SetJsonDataValue(jData, fieldInfo[0] + ".text", "");
                                continue;
                            }

                            ResultHelper.SetJsonDataValue(jData, fieldInfo[0] + ".value", fieldValue);

                            StringBuilder myfullName = new StringBuilder();
                            for (int i = 0; i < memeberInfo.Length - 2; i++)
                            {
                                if (myfullName.Length > 0)
                                    myfullName.Append('.');
                                myfullName.Append(memeberInfo[i]);
                            }
                            object myDataSourceObj = null;
                            Type myType = type;
                            if (myfullName.Length == 0)
                            {
                                myDataSourceObj = obj;
                            }
                            else
                            {
                                myDataSourceObj = activeObjs[myfullName.ToString()];
                                if (myDataSourceObj == null)
                                {
                                    myType = getType(myfullName.ToString());
                                    myDataSourceObj = Activator.CreateInstance(myType);
                                    activeObjs[myfullName.ToString()] = myDataSourceObj;
                                }
                            }
                            object query = myType.GetProperty(memeberInfo[memeberInfo.Length - 2]).GetValue(myDataSourceObj);
                            object findedDataItem = ResultHelper.InvokeWhereEquals(query, memeberInfo[memeberInfo.Length - 1], fieldValue);
                            findedDataItem = ResultHelper.InvokeSelect(findedDataItem , fieldInfo[2]);
                           
                            if (findedDataItem != null)
                            {
                                fieldValue = ResultHelper.InvokeFirstOrDefault(findedDataItem);
                                if (fieldValue == null)
                                    fieldValue = "";
                                fieldValue = fieldValue is Enum ? fieldValue.ToString() : fieldValue;
                                ResultHelper.SetJsonDataValue(jData, fieldInfo[0] + ".text", fieldValue);
                            }
                            else
                            {
                                ResultHelper.SetJsonDataValue(jData, fieldInfo[0] + ".text", "");
                            }
                        }
                        else
                        {
                            object fieldValue = ResultHelper.GetPropertyValue(dataitem, field);
                            if (fieldValue == null)
                                fieldValue = "";

                            if(fieldValue is Enum)
                            {
                                ResultHelper.SetJsonDataValue(jData, field + ".value", (int)fieldValue);
                                ResultHelper.SetJsonDataValue(jData, field + ".caption", fieldValue.ToString());
                            }
                            else
                            {
                                ResultHelper.SetJsonDataValue(jData, field, fieldValue);
                            }
                            
                        }
                    }
                    finalResult.Add(jData);
                }

                var pkvalueDic = new Dictionary<string, object>();
                pkvalueDic["pkid"] = pkid == null ? "" : pkid;
                finalResult.Add(pkvalueDic);
                return   finalResult.ToArray();
            }
            catch(Exception ex)
            {
                throw ex;
            }
            finally
            {
                foreach( KeyValuePair<string,object> item in activeObjs )
                {
                    if(item.Value is IDisposable)
                    {
                        ((IDisposable)item.Value).Dispose();
                    }
                }
            }
        }

        /// <summary>
        /// 计算总记录数
        /// </summary>
        /// <param name="target"></param>
        /// <param name="searchJsonStr">查询条件model</param>
        /// <returns></returns>
        [RemotingMethod]
        public int Count(string target,string searchJsonStr)
        {
            int index = target.LastIndexOf(".");
            string propertyName = target.Substring(index + 1);
            string fullname = target.Substring(0, index);

            Type type = getType(fullname);
            object obj = Activator.CreateInstance(type);
            object result = null;

            if (propertyName.EndsWith("()"))
            {
                var method = type.GetMethod(propertyName.Substring(0, propertyName.Length - 2));
                if (method.GetCustomAttribute(typeof(RemotingMethodAttribute)) == null)
                {
                    throw new Exception(propertyName + "函数不是RemotingMethod");
                }
                result = method.Invoke(obj, null);
            }
            else
            {
                result = type.GetProperty(propertyName).GetValue(obj);
            }

            Type dataItemType = result.GetType();
            if (dataItemType.IsArray)
            {
                dataItemType = dataItemType.GetElementType();
            }
            else if (dataItemType.GetTypeInfo().IsGenericType)
            {
                dataItemType = dataItemType.GetGenericArguments()[0];
            }

            if (searchJsonStr.IsNullOrEmpty() == false)
            {
                var searchModel = (Newtonsoft.Json.Linq.JObject)Newtonsoft.Json.JsonConvert.DeserializeObject(searchJsonStr);
                result = search(result, dataItemType, searchModel);
            }
            return Convert.ToInt32( ResultHelper.InvokeCount(result));
        }

        /// <summary>
        /// 合计指定字段的
        /// </summary>
        /// <param name="target"></param>
        /// <param name="fields"></param>
        /// <param name="searchJsonStr"></param>
        /// <returns></returns>
        [RemotingMethod]
        public object Sum(string target, string[] fields, string searchJsonStr)
        {
            int index = target.LastIndexOf(".");
            string propertyName = target.Substring(index + 1);
            string fullname = target.Substring(0, index);

            Type type = getType(fullname);
            object obj = Activator.CreateInstance(type);
            object result = null;

            if (propertyName.EndsWith("()"))
            {
                var method = type.GetMethod(propertyName.Substring(0, propertyName.Length - 2));
                if (method.GetCustomAttribute(typeof(RemotingMethodAttribute)) == null)
                {
                    throw new Exception(propertyName + "函数不是RemotingMethod");
                }
                result = method.Invoke(obj, null);
            }
            else
            {
                result = type.GetProperty(propertyName).GetValue(obj);
            }

            Type dataItemType = result.GetType();
            if (dataItemType.IsArray)
            {
                dataItemType = dataItemType.GetElementType();
            }
            else if (dataItemType.GetTypeInfo().IsGenericType)
            {
                dataItemType = dataItemType.GetGenericArguments()[0];
            }

            if (searchJsonStr.IsNullOrEmpty() == false)
            {
                var searchModel = (Newtonsoft.Json.Linq.JObject)Newtonsoft.Json.JsonConvert.DeserializeObject(searchJsonStr);
                result = search(result, dataItemType, searchModel);
            }

            Dictionary<string, object> finallResult = new Dictionary<string, object>();
            foreach (string field in fields)
            {
                if (field.Length == 0)
                    continue;

                var query = ResultHelper.InvokeSelect(result, field);
                var value = ResultHelper.InvokeSum(query);
                finallResult[field] = value;
            }

            return finallResult;
        }

        protected virtual void OnSavingData(object dataitem)
        {
        }

        /// <summary>
        /// 把对象保存到数据库
        /// </summary>
        /// <param name="target"></param>
        /// <param name="dataJson"></param>
        /// <returns>返回主键值</returns>
        [RemotingMethod]
        public object SaveData(string target,string dataJson)
        {
            int index = target.LastIndexOf(".");
            string propertyName = target.Substring(index + 1);
            string fullname = target.Substring(0, index);


            Type type = getType(fullname);
            var dbContext = Activator.CreateInstance(type) as EntityDB.DBContext;
            if (dbContext == null)
            {
                throw new Exception(fullname + " is not " + typeof(EntityDB.DBContext).FullName);
            }
            var tableType = type.GetProperty(propertyName).PropertyType.GetGenericArguments()[0];
            EntityDB.Attributes.Table myTableAttr = tableType.GetTypeInfo().GetCustomAttribute(typeof(EntityDB.Attributes.Table)) as EntityDB.Attributes.Table;
            var dataitem = Newtonsoft.Json.JsonConvert.DeserializeObject(dataJson, tableType) as EntityDB.DataItem;
            if (dataitem == null)
            {
                throw new Exception("dataitem is not EntityDB.DataItem");
            }
            this.OnSavingData(dataitem);
            dbContext.Update(dataitem);
            if (myTableAttr.IDField.IsNullOrEmpty())
                return null;
            return dataitem.GetValue(myTableAttr.IDField);
        }
        /// <summary>
        /// 文件开始传输
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="fileSize"></param>
        /// <param name="offset">文件偏移</param>
        /// <returns>返回一个IUploadFileHandler对象，处理上传的文件数据</returns>
        public virtual IUploadFileHandler OnBeginUploadFile(string fileName, int fileSize,int offset)
        {
            return null;
        }

        /// <summary>
        /// 把消息发送到客户端，客户端对象的.onmessage会接收到，这个方法调用方必须catch exception
        /// </summary>
        /// <param name="msg"></param>
        public void SendMessage(string msg)
        {
            if(RemotingClientHandler.KeepAliveHandlers.ContainsKey(this.SocketID) == false)
            {
                throw new NotConnectKeepAliveException("客户端目前还没有成功与服务器建立长连接");
            }
            RemotingClientHandler.KeepAliveHandlers[SocketID].SendClientMessage(msg);
        }

        /// <summary>
        /// 设置当长连接connect时触发的action
        /// </summary>
        /// <param name="_OnKeepAliveConnect"></param>
        public void SetOnKeepAliveConnect(Action _OnKeepAliveConnect)
        {
            if (this.SocketID.IsNullOrEmpty() == false)
            {
                this.Session.OnKeepAliveConnectEvents[this.SocketID] = _OnKeepAliveConnect;
            }
        }

        /// <summary>
        /// 设置当长连接close时触发的action
        /// </summary>
        /// <param name="_OnKeepAliveClose"></param>
        public void SetOnKeepAliveClose(Action _OnKeepAliveClose)
        {
            if (this.SocketID.IsNullOrEmpty() == false)
            {
                this.Session.OnKeepAliveCloseEvents[this.SocketID] = _OnKeepAliveClose;
            }
        }
    }
}
