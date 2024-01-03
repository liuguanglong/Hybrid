using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hybrid.Web.Shared
{
    public static class TreeDataExtention
    {
        public static void PrepareTreeData(this List<Dictionary<String,Object>> treeitems, List<Dictionary<String, Object>> treeitems1)
        {
            var listID = treeitems.Where(x => x.ContainsKey("items")).Select(x => x["id"]);

            for(int k=0; k<listID.Count();k++)
            {
                String id = (String)listID.ElementAt(k);
                var item  = treeitems.Where(x => (String)x["id"] == id).FirstOrDefault();
                if (item != null)
                {
                    List<Dictionary<String, Object>> items = (List<Dictionary<String, Object>>)item["items"];
                    for (int i = 0; i < items.Count; i++)
                    {
                        String itemid = (String)items[i]["item_id"];
                        var data = treeitems1.Where(x => (String)x["id"] == itemid).FirstOrDefault();
                        if (data != null)
                        {
                            items[i] = data;
                            treeitems1.Remove(data);
                        }
                    }

                    PrepareTreeData(items,treeitems1);
                }
            }
        }
    }
}
