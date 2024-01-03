using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Hybrid.Web.Shared
{
    public class DictionarySerialiazerExtention
    {
        public static Dictionary<string, object> SerializeToDictionary(object obj)
        {
            PropertyInfo[] properties = obj.GetType().GetProperties();
            Dictionary<string, object> result = new Dictionary<string, object>();

            foreach (var property in properties)
            {
                var v = property.GetValue(obj);
                Type type = v.GetType();
                if (type.IsPrimitive)
                {
                    result[property.Name] = v;
                }
                else if(v is String)
                {
                    result[property.Name] = v;
                }
                else if(type.IsGenericType && type.GetGenericTypeDefinition() == typeof(List<>))
                {
                    Console.WriteLine($"{type.Name} is not a primitive data type.");
                }

                result[property.Name] = property.GetValue(obj);
            }

            return result;
        }

        // 将 Dictionary 反序列化为对象
        public static T DeserializeFromDictionary<T>(Dictionary<string, object> data)
        {
            T obj = Activator.CreateInstance<T>();
            PropertyInfo[] properties = obj.GetType().GetProperties();

            foreach (var property in properties)
            {
                if (data.TryGetValue(property.Name, out object value))
                {
                    property.SetValue(obj, value);
                }
            }

            return obj;
        }
    }
}
