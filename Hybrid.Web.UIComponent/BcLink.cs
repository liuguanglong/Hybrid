using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Hybrid.Web.Shared;

namespace Hybrid.Web.UIComponent
{
    public class BcLink : UIElement
    {
        public String Content { get; set; }
        public BcParamString? Href { get; set; }
        public BcLink(Dictionary<String, Object> data) : base(data)
        {
            Content = data.getValue<String>("content");
            String href = data.getValue<String>("href");

            List<Dictionary<String, Object>> listParams = data.getList("paramlist").ToList();
            BcParam[] arrayParams = new BcParam[listParams.Count];
            foreach (var param in listParams)
            {
                int index = param.getValue<int>("index");
                String path = param.getValue<String>("data_path");
                String type = param.getValue<String>("data_type");
                arrayParams[index] = new BcParam(path,type);
            }
            Href = new BcParamString(href, arrayParams);
        }
    }
}
