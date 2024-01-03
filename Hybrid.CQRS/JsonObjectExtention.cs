using GraphQL.Client.Serializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Threading.Tasks;

namespace Hybrid.CQRS
{
    public static class JsonObjectExtention
    {
        public static JsonElement? Get(this JsonElement element, string name) =>
        element.ValueKind != JsonValueKind.Null && element.ValueKind != JsonValueKind.Undefined && element.TryGetProperty(name, out var value)
            ? value : (JsonElement?)null;

        public static JsonElement? Get(this JsonElement element, int index)
        {
            if (element.ValueKind == JsonValueKind.Null || element.ValueKind == JsonValueKind.Undefined)
                return null;
            // Throw if index < 0
            return index < element.GetArrayLength() ? element[index] : null;
        }

        public static Dictionary<String,Object> ToDictionary(this JsonObject obj)
        {
            var options = new JsonSerializerOptions();
            options.Converters.Add(new BussinessCoreJsonConverter());
            var dic = JsonSerializer.Deserialize<Dictionary<string, object>>(obj, options);

            return dic;
        }

        public static Dictionary<String,Object> ToScreenData(this JsonObject obj)
        {
            var options = new JsonSerializerOptions();
            options.Converters.Add(new BussinessCoreJsonConverter());
            var dic = JsonSerializer.Deserialize<Dictionary<string, object>>(obj, options);

            flatten(dic);

            return dic;
        }

        public static Dictionary<String, Object> ToMutilPageScreenData(this JsonObject obj)
        {
            var options = new JsonSerializerOptions();
            options.Converters.Add(new BussinessCoreJsonConverter());
            var dic = JsonSerializer.Deserialize<Dictionary<string, object>>(obj, options);

            flattenWithPageInfo(dic);

            return dic;
        }


        public static PageInfo GetPageInfo(this JsonObject obj,String datasource)
        {
            var options = new JsonSerializerOptions();
            options.Converters.Add(new BussinessCoreJsonConverter());

            if (obj.ContainsKey(datasource))
            {
                var paginfo = obj[datasource]["pageInfo"];
                var dic = JsonSerializer.Deserialize<Dictionary<string, object>>(paginfo, options);

                PageInfo info = new PageInfo();
                info.startCursor = (String)dic["startCursor"];
                info.endCursor = (String)dic["endCursor"];
                info.hasPreviousPage = (bool)dic["hasPreviousPage"];
                info.hasNextPage = (bool)dic["hasNextPage"];

                return info;
            }
            return null;
        }

        public static Object flatterSubItem(Object item)
        {
            switch(item)
            {
                case Dictionary<String,Object> dic:
                    if(dic.ContainsKey("edges"))
                    {
                        List<Dictionary<String, Object>> newValue = new List<Dictionary<string, object>>();
                        List<Dictionary<String, Object>> v = (List<Dictionary<String, Object>>)dic["edges"];
                        foreach (var it in v)
                        {
                            if (it.ContainsKey("node") && it["node"] is Dictionary<String, Object>)
                            {
                                var v1 = (Dictionary<String, Object>)it["node"];
                                foreach (var key2 in v1.Keys)
                                {
                                    v1[key2] = flatterSubItem(v1[key2]);
                                }    
                                newValue.Add(v1);
                            }
                        }

                        return newValue;
                    }
                    else
                    {
                        foreach(var k in dic.Keys)
                            dic[k] = flatterSubItem(dic[k]);
                        return dic;
                    }
                default:
                     return item;
            }
        }
        public static void flatten(Dictionary<String,Object> dic)
        {
            foreach(var k in dic.Keys)
            {
                dic[k] = flatterSubItem(dic[k]);
            }
        }

        public static void flattenWithPageInfo(Dictionary<String, Object> dic)
        {
            foreach (var k in dic.Keys)
            {
                dic[k] = flatterSubItemWithPageInfo(dic[k]);
            }
        }

        public static Object flatterSubItemWithPageInfo(Object item)
        {
            switch (item)
            {
                case Dictionary<String, Object> dic:
                    if (dic.ContainsKey("edges"))
                    {
                        List<Dictionary<String, Object>> newValue = new List<Dictionary<string, object>>();
                        List<Dictionary<String, Object>> v = (List<Dictionary<String, Object>>)dic["edges"];
                        foreach (var it in v)
                        {
                            if (it.ContainsKey("node") && it["node"] is Dictionary<String, Object>)
                            {
                                var v1 = (Dictionary<String, Object>)it["node"];
                                foreach (var key2 in v1.Keys)
                                {
                                    v1[key2] = flatterSubItem(v1[key2]);
                                }
                                newValue.Add(v1);
                            }
                        }

                        if(dic.ContainsKey("pageInfo") == false)
                        {
                            return newValue;
                        }
                        else
                        {
                            Dictionary<String,Object> ret = new Dictionary<String,Object>();
                            ret.Add("nodes", newValue);

                            Dictionary<String,Object> paginfo = (Dictionary<String, Object>)dic["pageInfo"];

                            PageInfo info = new PageInfo();
                            info.startCursor = (String)paginfo["startCursor"];
                            info.endCursor = (String)paginfo["endCursor"];
                            info.hasPreviousPage = (bool)paginfo["hasPreviousPage"];
                            info.hasNextPage = (bool)paginfo["hasNextPage"];

                            ret.Add("pageInfo", info);

                            return ret;
                        }
                    }
                    else
                    {
                        foreach (var k in dic.Keys)
                            dic[k] = flatterSubItem(dic[k]);
                        return dic;
                    }
                default:
                    return item;
            }
        }
    }
}
