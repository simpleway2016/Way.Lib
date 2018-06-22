using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Linq;

namespace Way.Lib.Serialization
{
    /// <summary>
    /// 序列化对象，保持类型和原对象一直，支持在字段定义NonSerializedAttribute，避免被序列化
    /// </summary>
    public class Serializer
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string SerializeObject(object obj)
        {
            var converter = new MyJsonConvert();
            var result = Newtonsoft.Json.JsonConvert.SerializeObject(obj, converter);
            return $"/*{Newtonsoft.Json.JsonConvert.SerializeObject(converter.TypeDescs)}*/{result}";
        }
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="content"></param>
        /// <returns></returns>
        public static T DeserializeObject<T>(string content)
        {
            var converter = new MyJsonConvert();
            return Newtonsoft.Json.JsonConvert.DeserializeObject<T>(content, converter);
        }
    }

    class MyJsonConvert : Newtonsoft.Json.JsonConverter
    {
        public class TypeDesc
        {
            public string Assembly;
            public string Type;
        }
        public List<TypeDesc> TypeDescs = new List<TypeDesc>();

        public override bool CanRead => true;
        public override bool CanWrite => true;

        int getTypedescIndex(Type type)
        {
            var assembly = type.Assembly.GetName().Name;
            var typename = type.FullName;
            var item = TypeDescs.FirstOrDefault(m => m.Assembly == assembly && m.Type == typename);
            if (item != null)
                return TypeDescs.IndexOf(item);

            TypeDescs.Add(new TypeDesc
            {
                Assembly = assembly,
                Type = typename
            });
            return TypeDescs.Count - 1;
        }
        Type getTypeByDescIndex(int index)
        {
            if (index < 0)
                return typeof(object);

            var assembly = Assembly.Load(new AssemblyName(this.TypeDescs[index].Assembly));
            return assembly.GetType(this.TypeDescs[index].Type);
        }

        public override bool CanConvert(Type objectType)
        {
            return true;
        }

        object ReadJsonForDictionary(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var dict = (System.Collections.IDictionary)Activator.CreateInstance(objectType);
            while (true)
            {
                reader.Read();
                if (reader.TokenType == JsonToken.StartObject)
                {
                    var key = ReadJson(reader, null, null, serializer);

                    reader.Read();
                    dict[key] = ReadJson(reader, null, null, serializer);

                }
                else if (reader.TokenType == JsonToken.EndArray)
                {
                    break;
                }
            }

            return dict;
        }

        object ReadJsonForArray(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            Type itemType = null;
            System.Collections.IList list = null;
            if (objectType.IsArray)
            {
                itemType = objectType.GetElementType();
                list = (System.Collections.IList)Activator.CreateInstance(typeof(List<>).MakeGenericType(itemType));
            }
            else if (objectType.IsGenericType)
            {
                itemType = objectType.GetGenericArguments()[0];
                list = (System.Collections.IList)Activator.CreateInstance(objectType);
            }

            while (true)
            {
                reader.Read();
                if (reader.TokenType == JsonToken.StartObject || reader.TokenType == JsonToken.StartArray)
                {
                    var item = ReadJson(reader, null, null, serializer);
                    list.Add(item);
                }
                else if (reader.TokenType == JsonToken.EndArray)
                {
                    break;
                }
            }

            if (objectType.IsArray)
            {
                var method = typeof(System.Linq.Enumerable).GetMethods().Where(m => m.Name == "ToArray" && m.IsGenericMethod).FirstOrDefault();
                var arr = method.MakeGenericMethod(itemType).Invoke(null, new object[] { list });
                return arr;
            }
            else
            {
                return list;
            }
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null)
                return null;

            if (string.IsNullOrEmpty(reader.Path))
            {
                this.TypeDescs = Newtonsoft.Json.JsonConvert.DeserializeObject<List<TypeDesc>>((string)reader.Value);

                reader.Read();
            }

            reader.Read();
            reader.Read();
            objectType = getTypeByDescIndex(Convert.ToInt32(reader.Value));

            reader.Read();
            var pname = (string)reader.Value;


            if (pname == "b")
            {
                byte[] bs = reader.ReadAsBytes();
                reader.Read();//记得移到下一个元素
                System.Runtime.Serialization.Formatters.Binary.BinaryFormatter deserializer = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
                using (var ms = new System.IO.MemoryStream(bs))
                {
                    object newobj = deserializer.Deserialize(ms);
                    return newobj;
                }
            }

            //读取到v值处
            reader.Read();

            if (objectType != null && objectType.GetTypeInfo().ImplementedInterfaces.Contains(typeof(System.Collections.IDictionary)))
            {
                var value = ReadJsonForDictionary(reader, objectType, existingValue, serializer);
                reader.Read();
                return value;
            }


            if (reader.TokenType == JsonToken.StartArray)
            {
                var value = ReadJsonForArray(reader, objectType, existingValue, serializer);
                reader.Read();
                return value;
            }

            if (objectType.IsValueType || objectType == typeof(string))
            {
                var value = Newtonsoft.Json.JsonConvert.DeserializeObject(reader.Value.ToString(), objectType);
                reader.Read();
                return value;
            }



            var result = Activator.CreateInstance(objectType);

            string fieldName;
            object fieldValue;
            while (true)
            {

                reader.Read();
                if (reader.TokenType == JsonToken.PropertyName)
                {
                    fieldName = (string)reader.Value;
                    fieldValue = null;


                    reader.Read();
                    fieldValue = ReadJson(reader, null, null, serializer);
                    
                    var fieldInfo = getFieldInfo(objectType, fieldName);
                    if (fieldInfo != null)
                    {
                        fieldInfo.SetValue(result, fieldValue);
                    }
                }
                else if (reader.TokenType == JsonToken.EndObject)
                {
                    reader.Read();
                    break;
                }
            }

            return result;
        }

        FieldInfo getFieldInfo(Type type,string fieldName)
        {
            if(fieldName.Contains("-"))
            {
                string[] info = fieldName.Split('-');
                fieldName = info[1];
                Type thisType = type.BaseType;
                while(thisType != typeof(object))
                {
                    if(thisType.FullName == info[0])
                    {
                        return thisType.GetField(fieldName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly);
                    }
                }
                return null;
            }
            else
            {
                return type.GetField(fieldName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly);
            }
        }

        public override void WriteJson(JsonWriter writer, object obj, JsonSerializer serializer)
        {
            Type type = obj.GetType();

            writer.WriteStartObject();
            writer.WritePropertyName("t");
            writer.WriteValue(getTypedescIndex(type));

            if (type == typeof(System.Data.DataTable) || type == typeof(System.Data.DataSet))
            {
                //可以序列化，那就直接序列化
                System.Runtime.Serialization.Formatters.Binary.BinaryFormatter se = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
                using (System.IO.MemoryStream ms = new System.IO.MemoryStream())
                {
                    se.Serialize(ms, obj);
                    ms.Position = 0;
                    byte[] bs = new byte[ms.Length];
                    ms.Read(bs, 0, bs.Length);

                    writer.WritePropertyName("b");
                    writer.WriteValue(bs);
                }

                writer.WriteEndObject();
                return;
            }

            writer.WritePropertyName("v");


            if (type.IsValueType || obj is string)
            {
                writer.WriteValue(Newtonsoft.Json.JsonConvert.SerializeObject(obj));
            }
            else if (obj is System.Collections.IDictionary)
            {
                writer.WriteStartArray();
                var dict = (System.Collections.IDictionary)obj;
                foreach (var key in dict.Keys)
                {
                    var value = dict[key];
                    if (value == null)
                        continue;

                    serializer.Serialize(writer, key);
                    serializer.Serialize(writer, value);
                }
                writer.WriteEndArray();
            }
            else if (obj is Array)
            {
                writer.WriteStartArray();
                var enumerator = ((System.Collections.IEnumerable)obj).GetEnumerator();
                while (enumerator.MoveNext())
                {
                    serializer.Serialize(writer, enumerator.Current);
                }
                writer.WriteEndArray();
            }
            else
            {
                writer.WriteStartObject();
                Type thisType = type;
                while (thisType != typeof(object))
                {
                    var fields = thisType.GetFields(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance | BindingFlags.DeclaredOnly);
                    foreach (var field in fields)
                    {
                        if (field.GetCustomAttribute(typeof(NonSerializedAttribute)) != null)
                            continue;

                        var value = field.GetValue(obj);
                        if(thisType == type)
                         writer.WritePropertyName(field.Name);
                        else
                        {
                            writer.WritePropertyName($"{thisType.FullName}-{field.Name}");
                        }
                        serializer.Serialize(writer, value);
                    }
                    thisType = thisType.BaseType;
                }
                writer.WriteEndObject();
            }
            writer.WriteEndObject();
        }
    }
}
