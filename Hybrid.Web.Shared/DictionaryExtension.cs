using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Hybrid.Web.Shared
{
    public static class DictionaryExtension
    {
        public static T getValue<T>(this Dictionary<String,Object> Values,String name)
        {
            String[] pathList = name.Split(new char[] { '.' });
            String memberName = pathList.Last();

            Dictionary<String, Object> dic = Values;
            if (pathList.Length > 1)
            {
                pathList = pathList.Take(pathList.Length - 1).ToArray();
                dic = getDictionary(Values, pathList, 0);
            }

            if (dic.ContainsKey(memberName))
                return (T)dic.GetValueOrDefault(memberName);
            else
                return default(T);
        }

        public static Object getValue(this Dictionary<String, Object> Values, String name)
        {
            String[] pathList = name.Split(new char[] { '.' });
            String memberName = pathList.Last();

            Dictionary<String, Object> dic = Values;
            if (pathList.Length > 1)
            {
                pathList = pathList.Take(pathList.Length - 1).ToArray();
                dic = getDictionary(Values, pathList, 0);
            }

            if (dic.ContainsKey(memberName))
                return dic.GetValueOrDefault(memberName);
            else
                return null;
        }

        public static void changeValue(this Dictionary<String,Object> Values,String name, Object value)
        {
            String[] pathList = name.Split(new char[] { '.' });
            String memberName = pathList.Last();

            Dictionary<String, Object> dic = Values;
            if (pathList.Length > 1)
            {
                pathList = pathList.Take(pathList.Length - 1).ToArray();
                dic = getDictionary(Values, pathList, 0);
            }

            if (dic.ContainsKey(memberName))
                dic[memberName] = value;
            else
                dic.Add(memberName,value);
        }

        public static Dictionary<String, Object> getDictionary(this Dictionary<String, Object> Values, String path)
        {
            String[] pathList = path.Split(new char[] { '.' });
            return getDictionary(Values, pathList, 0);
        }

        public static List<Dictionary<String, Object>> getList(this Dictionary<String, Object> Values, String name)
        {
            String[] pathList = name.Split(new char[] { '.' });
            String memberName = pathList.Last();

            Dictionary<String, Object> dic = Values;
            if (pathList.Length > 1)
            {
                pathList = pathList.Take(pathList.Length - 1).ToArray();
                dic = getDictionary(Values, pathList, 0);
            }

            if (dic.ContainsKey(memberName))
                return (List<Dictionary<String,Object>>)dic.GetValueOrDefault(memberName);
            else
                return new List<Dictionary<string, object>>();
        }

        private static Dictionary<String, Object> getDictionary(Dictionary<String, Object> dic, String[] listhPath, int index)
        {
            String path = listhPath[index];
            string pattern = @"^(.*?)\[(\d+)\]$";
            Match match = Regex.Match(path, pattern);

            if (match.Success)
            {
                String p1 = match.Groups[1].Value;
                int indexItems = int.Parse(match.Groups[2].Value);
                List<Dictionary<String, Object>> ret = (List<Dictionary<String, Object>>)dic[p1];

                if (ret.Count == 0)
                    return new Dictionary<string, object>();

                Dictionary<String, Object> d = ret[indexItems];
                index++;
                if (index == listhPath.Length)
                    return d;
                else
                    return getDictionary(d, listhPath, index);
            }

            if (dic.ContainsKey(path))
            {
                var d = (Dictionary<String, Object>)dic[path];
                index++;
                if (index == listhPath.Length)
                    return d;
                else
                    return getDictionary(d, listhPath, index);
            }

            return new Dictionary<string, object>();
        }

        
    }
}
