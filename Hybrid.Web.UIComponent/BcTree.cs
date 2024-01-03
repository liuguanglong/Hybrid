using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

using Hybrid.Web.Shared;

namespace Hybrid.Web.UIComponent
{
    public class BcTree : UIElement
    {
        public BcParamString? NodeContent { get; set; }
        public BcParamString? NodeLink { get; set; }

        public BcTree(String name, String label, String datapath, String rootID) : base(name, label, datapath)
        {
        }

        public BcTree(Dictionary<String, Object> data) : base(data)
        {
            String text = data.getValue<String>("text");
            String link = data.getValue<String>("node_link");

            List<Dictionary<String, Object>> listParams = data.getList("paramlist").Where(x => (String)x["param_type"] == "NodeText").ToList();
            BcParam[] arrayParams = new BcParam[listParams.Count];
            foreach (var param in listParams)
            {
                int index = param.getValue<int>("index");
                String path = param.getValue<String>("data_path");
                String type = param.getValue<String>("data_type");
                arrayParams[index] = new BcParam(path, type);
            }
            NodeContent = new BcParamString(text, arrayParams);


            List<Dictionary<String, Object>> listParams1 = data.getList("paramlist").Where(x => (String)x["param_type"] == "NodeLink").ToList();
            BcParam[] arrayParams1 = new BcParam[listParams1.Count];
            foreach (var param in listParams1)
            {
                int index = param.getValue<int>("index");
                String path = param.getValue<String>("data_path");
                String type = param.getValue<String>("data_type");
                arrayParams1[index] = new BcParam(path, type);
            }
            NodeLink = new BcParamString(link, arrayParams1);

        }
    }
}
