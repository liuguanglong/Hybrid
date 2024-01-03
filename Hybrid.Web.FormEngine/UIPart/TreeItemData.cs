using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static MudBlazor.CategoryTypes;

namespace Hybrid.Web.FormEngine
{
    public class TreeItemData
    {
        public Dictionary<String, Object> data = new Dictionary<String, Object>();
        public HashSet<TreeItemData> TreeItems { get; set; }
    }

    public static class TreeItemDataExtention
    {
        public static HashSet<TreeItemData> ToTreeItem(this List<Dictionary<String,Object>> items)
        {
            HashSet<TreeItemData> data = new HashSet<TreeItemData>();
            foreach (var item in items)
            {
                if (item.ContainsKey("root") && (int)item["root"] == 1)
                {
                    TreeItemData d = new TreeItemData();
                    d.data = item;
                    String id = (String)item["id"];
                    d.TreeItems = PrepareTreeData(id, items);
                    data.Add(d);
                }
            }
            return data;
        }

        private static HashSet<TreeItemData> PrepareTreeData(String parentId, List<Dictionary<String, Object>> list)
        {
            HashSet<TreeItemData> data = new HashSet<TreeItemData>();
            foreach (var item in list)
            {
                if (item.ContainsKey("parent_id") && (String)item["parent_id"] == parentId)
                {
                    TreeItemData d = new TreeItemData();
                    d.data = item;
                    String id = (String)item["id"];
                    d.TreeItems = PrepareTreeData(id, list);
                    data.Add(d);
                }
            }
            return data;
        }
    }
}
