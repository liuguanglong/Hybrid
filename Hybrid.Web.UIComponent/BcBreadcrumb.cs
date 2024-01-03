using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using Hybrid.Web.Shared;

namespace Hybrid.Web.UIComponent
{
    public class BcBreadcrumb {
        public BcParamString Text { get; }
        public BcParamString Href { get; }

        public bool Disabled { get; }
        public int Index { get; }
        public string Icon { get; }

        public BcBreadcrumb(string text, string href, bool disabled = false, string icon = null)
        {
            Text = new BcParamString(text, null);
            Disabled = disabled;
            Href = new BcParamString(href, null);
            Icon = icon;
        }

        public BcBreadcrumb(Dictionary<String, Object> data)
        {
            String text = data.getValue<String>("text");
            String href = data.getValue<String>("href");
            Disabled = data.getValue<bool>("disabled");
            Index= data.getValue<Int32>("index");

            List<Dictionary<String, Object>> listHrefParams = data.getList("params").Where(x => (String)x["param_type"] == "href").ToList();
            List<Dictionary<String, Object>> listTextParams = data.getList("params").Where(x => (String)x["param_type"] == "text").ToList();
            BcParam[] arrayTextParams = new BcParam[listHrefParams.Count];
            BcParam[] arrayHrefParams = new BcParam[listTextParams.Count];
            foreach (var param in listHrefParams)
            {
                int index = param.getValue<int>("index");
                String path = param.getValue<String>("data_path");
                String type = param.getValue<String>("data_type");
                arrayHrefParams[index] = new BcParam(path, type);
            }
            Href = new BcParamString(href, arrayHrefParams);

            foreach (var param in listTextParams)
            {
                int index = param.getValue<int>("index");
                String path = param.getValue<String>("data_path");
                String type = param.getValue<String>("data_type");
                arrayTextParams[index] = new BcParam(path, type);
            }
            Text = new BcParamString(text, arrayTextParams);
        }
    }
}
