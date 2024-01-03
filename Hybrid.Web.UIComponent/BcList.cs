using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Hybrid.Web.Shared;

namespace Hybrid.Web.UIComponent
{
    public class BcList : UIElement
    {
        public List<UIElement> Elements { get; set; } = new List<UIElement>();
        public BcPart part { get; set; }
        public String partId { get; set; }
        public String Width { get; set; } = "80%";
        public String Height { get; set; } = "60%";
        public int PageCount { get; set; } = 3;

        public BcList(String name, String label, String datapath) : base(name, label, datapath)
        {
        }

        public BcList(Dictionary<String, Object> data) : base(data)
        {
            this.partId = data.getValue<String>("part[0].bc_screen_part.id");
            this.Width  = data.getValue<String>("width");
            this.Height = data.getValue<String>("height");
            this.PageCount = data.getValue<Int32>("pagecount");
        }
    }
}
